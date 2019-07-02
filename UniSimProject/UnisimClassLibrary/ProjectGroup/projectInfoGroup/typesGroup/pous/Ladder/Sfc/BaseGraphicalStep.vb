Imports System.Math
Imports System.IO
Imports System.Xml
Imports System.Threading.Timer
Public MustInherit Class BaseGraphicalStep
    Implements IDocumentable, IHasLocalId, IMovableObject

    Protected m_Name As String
    Protected m_documentation As String
    Protected m_Number As Integer
    Protected m_Initial As Boolean
    Protected m_Negated As Boolean

    Protected m_Active As Boolean
    Protected m_PreActive As Boolean
    Protected m_TimeActivation As DateTime

    Protected m_Final As Boolean
    Protected m_Sfc As Sfc
    Protected m_Position As Drawing.Point
    Protected Area As Drawing.Rectangle
    Protected m_Selected As Boolean
    Protected TopRectSelected As Boolean
    Protected BottomRectSelected As Boolean
    Protected BackColor As Drawing.Color
    Protected SelectionColor As Drawing.Color
    Protected NotSelectionColor As Drawing.Color
    Protected TextColor As Drawing.Color
    Protected m_Dimension As Integer
    Protected Carattere As Drawing.Font
    Protected ColorActive As Drawing.Color
    Protected ColorPreActive As Drawing.Color
    Protected ColorDeactive As Drawing.Color
    Protected GraphToDraw As Drawing.Graphics
    Protected DrawState As Boolean
    'La seguenta lista serve solo ai fini dell'esportazione xml
    Protected XmlPreviousConnectionsList As ArrayList
    Protected XmlNextConnectionsList As ArrayList
    Public Property Name() As String Implements IDocumentable.Name
        Get
            Name = m_Name
        End Get
        Set(ByVal Value As String)
            m_Name = Value
        End Set
    End Property
    Property Documentation() As String Implements IDocumentable.Documentation
        Get
            Return m_documentation
        End Get
        Set(ByVal Value As String)
            m_documentation = Value
        End Set
    End Property
    Property Number() As Integer Implements IHasLocalId.Number
        Get
            Number = m_Number
        End Get
        Set(ByVal Value As Integer)
            m_Number = Value
        End Set
    End Property
    Property Initial() As String
        Get
            Initial = m_Initial
        End Get
        Set(ByVal Value As String)
            m_Initial = Value
        End Set
    End Property
    Property Negated() As String
        Get
            Negated = m_Negated
        End Get
        Set(ByVal Value As String)
            m_Negated = Value
        End Set
    End Property

    Public MustOverride Sub DisposeMe()
    Public MustOverride Sub Active()
    Public MustOverride Sub Disactive()
    Public MustOverride Sub PreActive()
    Public MustOverride Sub Cancel(ByVal CancellSmallRectangels As Boolean) Implements IMovableObject.Cancel
    Public MustOverride Sub Draw(ByVal DrawSmallRectangels As Boolean) Implements IMovableObject.Draw
    Public MustOverride Sub CalculateArea() Implements IMovableObject.CalculateArea
    Public MustOverride Sub DrawStepState(ByVal Cancel As Boolean)
    Public MustOverride Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter)
    Public MustOverride Sub xmlImport(ByRef RefXMLProjectReader As XmlTextReader)
    Public MustOverride Sub ResolveVariablesLinks()
    Public MustOverride Function GetDescription() As String Implements IDocumentable.GetDescription
    Public MustOverride Function GetSystemDescription() As String Implements IDocumentable.GetSystemDescription
    Public MustOverride Function GetIdentifier() As String Implements IDocumentable.GetIdentifier
    Public MustOverride Sub Move(ByVal dx As Integer, ByVal dy As Integer) Implements IMovableObject.Move
    Public MustOverride Sub SetGraphToDraw(ByRef Graph As Drawing.Graphics) Implements IMovableObject.SetGraphToDraw
    Public MustOverride Function CreateInstance(ByRef NewSfc As Sfc) As Object

    Public Sub New(ByVal StepNumber As Integer, ByRef StepName As String, ByVal StepDocumentation As String, ByRef RefSfc As Sfc, ByVal Ini As Boolean, ByVal Fin As Boolean, ByVal P As Drawing.Point, ByVal BackCol As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, ByRef Graph As Drawing.Graphics, ByVal DrState As Boolean, ByVal Dimen As Integer)
        m_Number = StepNumber
        m_Name = StepName
        m_documentation = StepDocumentation
        m_Sfc = RefSfc
        m_Initial = Ini
        m_Final = Fin
        m_Position = P
        BackColor = BackCol
        SelectionColor = SelectionCol
        NotSelectionColor = NotSelectionCol
        TextColor = TextCol
        Carattere = Car
        ColorActive = ColActive
        ColorDeactive = ColDeactive
        ColorPreActive = ColPreActive
        GraphToDraw = Graph
        DrawState = DrState
        m_Dimension = Dimen
        Area = New Drawing.Rectangle
        XmlPreviousConnectionsList = New ArrayList
        XmlNextConnectionsList = New ArrayList

    End Sub
    Public Sub New(ByVal RefSfc As Sfc, ByVal BackCol As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, ByVal Graph As Drawing.Graphics, ByVal DrState As Boolean, ByVal Dimen As Integer)
        m_Sfc = RefSfc
        BackColor = BackCol
        SelectionColor = SelectionCol
        NotSelectionColor = NotSelectionCol
        TextColor = TextCol
        Carattere = Car
        ColorActive = ColActive
        ColorDeactive = ColDeactive
        ColorPreActive = ColPreActive
        GraphToDraw = Graph
        DrawState = DrState
        m_Dimension = Dimen
        Area = New Drawing.Rectangle
        XmlPreviousConnectionsList = New ArrayList
        XmlNextConnectionsList = New ArrayList
    End Sub
    Public Sub New()
    End Sub
    Public Sub CreateXmlConvergencesAndDivergences(ByRef NextNumberAvaiable As Integer)
        'Crea una convergenza selettiva se la fase ha più elementi precedenti....
        '....e memorizza nella fase i e nelle transizioni i collegamenti
        If XmlPreviousConnectionsList.Count > 1 Then
            'Occorre una convergenza, la crea e aggiunge riferimento nelle XmlPreviousConnectionsList delle fase
            Dim NumberTemp As Integer = NextNumberAvaiable    'Numero della convergenza
            NextNumberAvaiable = NextNumberAvaiable + 1
            Dim ConnectionsListTemp As New ArrayList
            'Aggiunge i riferimenti agli elementi precedenti nella convergenza
            For Each Elem As Object In XmlPreviousConnectionsList
                'Aggiunge il riferimento all'elemento precedente
                ConnectionsListTemp.Add(Elem)
            Next Elem
            'Crea la convergenza
            Dim ConvDivStructTemp As ConvDivStruct = m_Sfc.XmlSelectionConvergences.Add(NumberTemp, Nothing, Nothing, ConnectionsListTemp, Nothing, Nothing)
            'Aggiunge il riferimento alla convergenza nella fase
            XmlPreviousConnectionsList.Clear()  'Nella fase alla fine della creazione delle converg e div ci può essere solo un collegamento 
            XmlPreviousConnectionsList.Add(ConvDivStructTemp)
        End If

        'Crea una divergenza selettiva se la fase ha più elementi successivi....
        '....e memorizza nella fase i e nelle transizioni i collegamenti
        If XmlNextConnectionsList.Count > 1 Then
            'Occorre una divergenza, la crea e aggiunge il riferimento alla fase precedente nella divergenza
            Dim NumberTemp As Integer = NextNumberAvaiable    'Numero della divergenza
            NextNumberAvaiable = NextNumberAvaiable + 1
            Dim ConnectionsListTemp As New ArrayList
            'Aggiunge il riferimento alla fase nella divergenza
            ConnectionsListTemp.Add(Me)
            Dim ConvDivStructTemp As ConvDivStruct = m_Sfc.XmlSelectionDivergences.Add(NumberTemp, Nothing, Nothing, ConnectionsListTemp, Nothing, Nothing)
            'Aggiunge il riferimento alla divergenza nella XmlConnectionsList degli elementi successivi
            For Each Elem As Object In XmlNextConnectionsList
                'Aggiunge il riferimento alla divergenza
                Elem.ReadXmlPreviousConnectionsList.Add(ConvDivStructTemp)
                'Rimuove il riferimento a se stesso nella XmlPreviousConnectionsList degli elementi successivi alla fase
                Elem.ReadXmlPreviousConnectionsList.Remove(Me)
            Next Elem
        End If
    End Sub
    Public Sub ResolveXmlConvergencesAndDivergences()
        Dim ST As ConvDivStruct
        For Each localId As Integer In XmlPreviousConnectionsList
            'Cerca l'id (il numero) tra le transizioni
            Dim T As GraphicalTransition = m_Sfc.GraphicalTransitionsList.FindTransitionByNumber(localId)
            'Se la trova collega la transizione alla fase
            If Not IsNothing(T) Then
                T.ReadNextsGraphicalStepsList.Add(Me)
            Else
                'Se non la trova è una divergenza simultanea o convergenza selettiva
                'cerca tra le divergenza simultanee
                ST = m_Sfc.XmlSimultaneousDivergences.FindStructByNumber(localId)
                'Se la trova collega le transizioni precedenti alla fase
                If Not IsNothing(ST) Then
                    'Cerca le transizioni precedenti alla divergenza
                    For Each ConvPrecLocalId As Integer In ST.XmlPreviousConnectionsList
                        T = m_Sfc.GraphicalTransitionsList.FindTransitionByNumber(ConvPrecLocalId)
                        'Se la trova collega la transizione
                        If Not IsNothing(T) Then
                            T.ReadNextsGraphicalStepsList.Add(Me)
                        End If
                    Next ConvPrecLocalId
                Else
                    'E' una convergenza selettiva
                    ST = m_Sfc.XmlSelectionConvergences.FindStructByNumber(localId)
                    'Se la trova collega le transizioni precedenti alla fase
                    If Not IsNothing(ST) Then
                        For Each ConvPrecLocalId As Integer In ST.XmlPreviousConnectionsList
                            'Cerca gli elementi precedenti alla convergenza
                            T = m_Sfc.GraphicalTransitionsList.FindTransitionByNumber(ConvPrecLocalId)
                            'Se la trova collega la transizione
                            If Not IsNothing(T) Then
                                T.ReadNextsGraphicalStepsList.Add(Me)
                            Else
                                'Altrimenti cerca tra le divergenze simultanee
                                Dim ST2 As ConvDivStruct = m_Sfc.XmlSimultaneousDivergences.FindStructByNumber(ConvPrecLocalId)
                                'Se la trova collega le transizioni precedenti alla fase
                                For Each Conv2PrecLocalId As Integer In ST2.XmlPreviousConnectionsList
                                    'Cerca le transizioni precedenti alla convergenza
                                    T = m_Sfc.GraphicalTransitionsList.FindTransitionByNumber(Conv2PrecLocalId)
                                    'Se la trova collega la transizione alla fase
                                    If Not IsNothing(T) Then
                                        T.ReadNextsGraphicalStepsList.Add(Me)
                                    End If
                                Next Conv2PrecLocalId
                            End If
                        Next ConvPrecLocalId
                    End If
                End If
            End If
        Next localId
    End Sub
    Public Sub SetDrawState(ByRef Value As Boolean)
        DrawState = Value
    End Sub
    Function ReadTimeActivation() As DateTime
        ReadTimeActivation = m_TimeActivation
    End Function
    Function ReadActive() As Boolean
        ReadActive = m_Active
    End Function
    Function ReadPreActive() As Boolean
        ReadPreActive = m_PreActive
    End Function
    Public Sub SetFinal(ByVal Value As Boolean)
        m_Final = Value
    End Sub
    Public Function ReadFinal() As Boolean
        ReadFinal = m_Final
    End Function
    Public Sub SetInitial(ByVal Value As Boolean)
        m_Initial = Value
    End Sub
    Public Function ReadInitial() As Boolean
        ReadInitial = m_Initial
    End Function
    Public Sub SetPosition(ByVal p As Drawing.Point)
        m_Position = p
        CalculateArea()
    End Sub
    Public Function ReadPosition() As Drawing.Point
        ReadPosition = m_Position
    End Function
    Public Sub SetSelected(ByVal v As Boolean)
        m_Selected = v
    End Sub
    Public Sub SelectObject() Implements IMovableObject.SelectObject
        SetSelected(True)
    End Sub
    Public Sub DeselectObject() Implements IMovableObject.DeselectObject
        SetSelected(False)
    End Sub
    Public Function ReadSelected() As Boolean
        ReadSelected = m_Selected
    End Function
    Public Sub SetDimension(ByVal Dimen As Integer)
        m_Dimension = Dimen
    End Sub
    Public Function ReadDimension() As Integer
        ReadDimension = m_Dimension
    End Function
    Public Property Size() As Drawing.Size Implements IMovableObject.Size
        Get
            Return New Drawing.Size(ReadDimension(), ReadDimension())
        End Get
        Set(ByVal value As Drawing.Size)
            m_Dimension = Math.Max(value.Width, value.Height)
        End Set
    End Property
    Public Sub SetTopRectSelected(ByVal v As Boolean)
        TopRectSelected = v
    End Sub
    Public Function ReadTopRectSelected() As Boolean
        ReadTopRectSelected = TopRectSelected
    End Function
    Public Function ReadXmlPreviousConnectionsList() As ArrayList
        ReadXmlPreviousConnectionsList = XmlPreviousConnectionsList
    End Function
    Public Function ReadXmlNextConnectionsList() As ArrayList
        ReadXmlNextConnectionsList = XmlNextConnectionsList
    End Function
    Public Sub SetBottomRectSelected(ByVal v As Boolean)
        BottomRectSelected = v
    End Sub
    Public Function ReadBottomRectSelected() As Boolean
        ReadBottomRectSelected = BottomRectSelected
    End Function
    Public Function ReadArea() As Drawing.Rectangle
        ReadArea = Area
    End Function
    Public Function CeIlMioRetSup(ByVal x As Integer, ByVal y As Integer) As Boolean
        If Abs(x - m_Position.X) <= CInt(m_Dimension / 10) And Abs(y - (m_Position.Y - CInt(m_Dimension / 2) - CInt(m_Dimension / 10))) <= (m_Dimension / 10) Then
            CeIlMioRetSup = True
        End If
    End Function
    Public Function CeIlMioRetInf(ByVal x As Integer, ByVal y As Integer) As Boolean
        If Abs(x - m_Position.X) <= CInt(m_Dimension / 10) And Abs(y - (m_Position.Y + CInt(m_Dimension / 2) + CInt(m_Dimension / 10))) <= (m_Dimension / 10) Then
            CeIlMioRetInf = True
        End If
    End Function
    Public Function MyAreaIsHere(ByVal x As Integer, ByVal y As Integer) As Boolean
        If Abs(x - m_Position.X) <= (m_Dimension / 2) And Abs(y - m_Position.Y) <= (m_Dimension / 2) Then
            MyAreaIsHere = True
        End If
    End Function

    Public Property Selected() As Boolean Implements ISelectable.Selected
        Get
            Return ReadSelected()
        End Get
        Set(ByVal value As Boolean)
            SetSelected(value)
        End Set
    End Property

    Public Property Position() As System.Drawing.Point Implements IGraphicalObject.Position
        Get
            Return m_Position
        End Get
        Set(ByVal value As System.Drawing.Point)
            m_Position = value
        End Set
    End Property
End Class
