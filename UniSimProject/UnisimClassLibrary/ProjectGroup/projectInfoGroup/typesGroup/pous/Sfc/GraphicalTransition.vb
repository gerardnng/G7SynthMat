Imports System.Math
Imports System.IO
Imports System.Xml
Imports System.Threading
Public Class GraphicalTransition
    Implements IDocumentable, IHasLocalId, IGraphicalObject

    Protected m_Name As String
    Protected m_documentation As String
    Protected m_Number As Integer
    Protected m_Sfc As Sfc 'Sfc cui appartiene
    Protected m_Condition As BooleanExpression
    Protected m_ConditionString As String     'Memorizza dopo l'importazione xml temporaneamente la condizione
    Protected m_ActualCondition As Boolean
    Protected m_Superable As Boolean
    Protected m_PreviousGraphicalStepsList As GraphicalStepsList
    Protected m_NextGraphicalStepsList As GraphicalStepsList
    Protected m_Dimension As Integer
    Protected m_Position As Drawing.Point
    Protected Area As Drawing.Rectangle
    Protected m_Selected As Boolean
    Protected NotSelectionColor As Drawing.Color
    Protected SelectionColor As Drawing.Color
    Protected CoeffSnap As Integer
    Protected Carattere As Drawing.Font
    Protected BackColor As Drawing.Color
    Protected TextColor As Drawing.Color
    Protected ColorConditionTrue As Drawing.Color
    Protected ColorConditionFalse As Drawing.Color
    Protected ColorActive As Drawing.Color
    Protected ColorPreActive As Drawing.Color
    Protected ColorDeactive As Drawing.Color
    Protected GraphToDraw As Drawing.Graphics
    Protected DrawState As Boolean
    'La seguenta lista serve solo ai fini dell'esportazione xml
    Protected XmlPreviousConnectionsList As ArrayList
    Property Documentation() As String Implements IDocumentable.Documentation
        Get
            Return m_documentation
        End Get
        Set(ByVal Value As String)
            m_documentation = Value
        End Set
    End Property
    Property Name() As String Implements IDocumentable.Name
        Get
            Name = m_Name
        End Get
        Set(ByVal Value As String)
            m_Name = Value
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
    Public Sub New(ByVal N As Integer, ByVal Name As String, ByVal Documentation As String, ByRef LPreviousSteps As GraphicalStepsList, ByRef LNextSteps As GraphicalStepsList, ByRef Condition As BooleanExpression, ByRef RefSFC As Sfc, ByVal PoxX As Integer, ByVal CSnap As Integer, ByVal ColSfondo As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColConditionTrue As Drawing.Color, ByVal ColConditionFalse As Drawing.Color, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, ByVal Graph As Drawing.Graphics, ByVal DrState As Boolean, ByVal Dimen As Integer)
        m_Number = N
        m_Name = Name
        m_documentation = Documentation
        m_PreviousGraphicalStepsList = LPreviousSteps
        m_NextGraphicalStepsList = LNextSteps
        m_Condition = Condition
        m_Sfc = RefSFC
        m_Condition = Condition
        m_Position.X = PoxX
        NotSelectionColor = NotSelectionCol
        SelectionColor = SelectionCol
        CoeffSnap = CSnap
        Carattere = Car
        BackColor = ColSfondo
        TextColor = TextCol
        ColorActive = ColActive
        ColorPreActive = ColPreActive
        ColorDeactive = Drawing.Color.Brown
        ColorConditionTrue = ColConditionTrue
        ColorConditionFalse = ColConditionFalse
        GraphToDraw = Graph
        DrawState = DrState
        m_Dimension = Dimen
        Area = New Drawing.Rectangle
        XmlPreviousConnectionsList = New ArrayList
        CalculusArea()
    End Sub
    Public Sub New(ByRef RefSFC As Sfc, ByVal CSnap As Integer, ByVal ColSfondo As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColConditionTrue As Drawing.Color, ByVal ColConditionFalse As Drawing.Color, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, ByVal Graph As Drawing.Graphics, ByVal DrState As Boolean, ByVal Dimen As Integer)
        m_PreviousGraphicalStepsList = New GraphicalStepsList
        m_NextGraphicalStepsList = New GraphicalStepsList
        m_Sfc = RefSFC
        'Crea il body e imposta il tipo di body (EnumBodyType.ST)
        m_Position.X = -1
        NotSelectionColor = NotSelectionCol
        SelectionColor = SelectionCol
        CoeffSnap = CSnap
        Carattere = Car
        BackColor = ColSfondo
        TextColor = TextCol
        ColorActive = ColActive
        ColorPreActive = ColPreActive
        ColorDeactive = Drawing.Color.Brown
        ColorConditionTrue = ColConditionTrue
        ColorConditionFalse = ColConditionFalse
        GraphToDraw = Graph
        DrawState = DrState
        m_Dimension = Dimen
        Area = New Drawing.Rectangle
        XmlPreviousConnectionsList = New ArrayList
    End Sub

    Public Function CreateInstance(ByRef NewSfc As Sfc, ByRef StepsList As GraphicalStepsList) As GraphicalTransition
        Dim NewPreviousGraphicalStepsList As New GraphicalStepsList
        Dim NewNextGraphicalStepsList As New GraphicalStepsList
        For Each S As BaseGraphicalStep In m_PreviousGraphicalStepsList
            NewPreviousGraphicalStepsList.Add(StepsList.FindStepByNumber(S.Number))
        Next S
        For Each S As BaseGraphicalStep In m_NextGraphicalStepsList
            NewNextGraphicalStepsList.Add(StepsList.FindStepByNumber(S.Number))
        Next S
        CreateInstance = New GraphicalTransition(m_Number, m_Name, m_documentation, NewPreviousGraphicalStepsList, NewNextGraphicalStepsList, m_Condition, NewSfc, m_Position.X, CoeffSnap, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ColorConditionTrue, ColorConditionFalse, ColorActive, ColorDeactive, ColorPreActive, Nothing, False, m_Dimension)

    End Function
    Public Sub xmlExport(ByRef RefXmlProjectWriter As XmlTextWriter)
        'Esporta la transizione
        RefXmlProjectWriter.WriteStartElement("transition")
        'Attributi della transizione
        RefXmlProjectWriter.WriteAttributeString("height", m_Dimension.ToString)
        RefXmlProjectWriter.WriteAttributeString("width", m_Dimension.ToString)
        RefXmlProjectWriter.WriteAttributeString("localId", m_Number.ToString)

        'm_Position
        RefXmlProjectWriter.WriteStartElement("position")
        'Attributi di m_Position
        'Nella transizione m_Position si riferisce al centro del trattino quindi sottrae CInt(m_Dimension / 10) a m_Position.X
        RefXmlProjectWriter.WriteAttributeString("x", (m_Position.X - CInt(m_Dimension / 10)).ToString)
        RefXmlProjectWriter.WriteAttributeString("y", (m_Position.Y).ToString)
        RefXmlProjectWriter.WriteEndElement() 'm_Position

        'ConnectionPointIn
        Dim ConnectionsList As New ConnectionPointIn(XmlPreviousConnectionsList)
        ConnectionsList.xmlExport(RefXmlProjectWriter)

        'condition  (Riferimento al nome della variabile booleana che controlla
        RefXmlProjectWriter.WriteStartElement("condition")
        'reference
        RefXmlProjectWriter.WriteStartElement("reference")

        'Attributi di reference
        RefXmlProjectWriter.WriteAttributeString("name", m_Condition.GetExpressionString)

        RefXmlProjectWriter.WriteEndElement() 'reference
        RefXmlProjectWriter.WriteEndElement() 'condition

        If Documentation <> "" Then
            RefXmlProjectWriter.WriteElementString("documentation", Me.Documentation)  'documentation
        End If

        RefXmlProjectWriter.WriteEndElement() 'transition
    End Sub
    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader)
        'Memorizza la profondità del nodo
        Dim NodeDepth As Integer = RefXmlProjectReader.Depth
        'Legge gli attributi
        If RefXmlProjectReader.MoveToAttribute("localId") Then
            m_Number = RefXmlProjectReader.Value
        End If
        'Se l'elemento non è vuoto si sposta sul nodo successivo
        If Not RefXmlProjectReader.IsEmptyElement Then
            RefXmlProjectReader.Read()

            'Scorre fino alla fine della transizione
            While RefXmlProjectReader.Depth > NodeDepth
                Select Case RefXmlProjectReader.Name
                    Case "position"
                        'Controlla se è l'inizio dell'elemento
                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                            'Legge solo il valore di x della posizione, il valore y viene calcolato automaticamente
                            'Nella transizione m_Position si riferisce al centro del trattino quindi aggiunge CInt(m_Dimension / 10) a m_Position.X
                            If RefXmlProjectReader.MoveToAttribute("x") Then
                                m_Position.X = RefXmlProjectReader.Value + CInt(m_Dimension / 10)
                            End If
                        End If
                    Case "connectionPointIn"
                        Dim ConnectionsList As New ConnectionPointIn(XmlPreviousConnectionsList)
                        ConnectionsList.xmlImport(RefXmlProjectReader)

                    Case "condition"
                        'Memorizza la profondità del nodo
                        Dim ConditionNodeDepth As Integer = RefXmlProjectReader.Depth
                        'Se l'elemento non è vuoto si sposta sul nodo successivo
                        If Not RefXmlProjectReader.IsEmptyElement Then
                            RefXmlProjectReader.Read()
                        End If
                        'Scorre fino alla fine della condition
                        While RefXmlProjectReader.Depth > ConditionNodeDepth
                            Select Case RefXmlProjectReader.Name
                                Case "reference"
                                    'Controlla se è l'inizio dell'elemento
                                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                                        'Legge gli attributi
                                        If RefXmlProjectReader.MoveToAttribute("name") Then
                                            m_ConditionString = RefXmlProjectReader.Value
                                        End If
                                    End If
                                Case "inline"   'E' un elemento body
                                    'Controlla se è l'inizio dell'elemento
                                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                                    End If
                            End Select
                            'Si sposta sul nodo successivo se non è la fine dell'elemento
                            If RefXmlProjectReader.Depth > NodeDepth Then
                                RefXmlProjectReader.Read()
                            End If

                        End While
                        ' Supporta il tag <comment> per retrocompatibilità con la 0.4.5L
                        ' (versione non ufficiale usata nell'anno accademico 2006/2007)
                        ' (salva sempre usando il tag standard <documentation>)
                    Case "documentation", "comment"
                        'Controlla se è l'inizio dell'elemento
                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                            Documentation = RefXmlProjectReader.ReadString()
                        End If
                End Select
                'Si sposta sul nodo successivo se non è la fine dell'elemento
                If RefXmlProjectReader.Depth > NodeDepth Then
                    RefXmlProjectReader.Read()
                End If

            End While
        End If
        'Aggiorna l'area della transizione
        CalculusArea()
    End Sub
    Public Sub ResolveVariablesLinks()
        'Risolve i riferimenti dei nomi di variabili nella transizione
        m_Condition = New BooleanExpression(m_Sfc)
        m_Condition.SetExpression(m_ConditionString)
    End Sub
    Public Sub CreateXmlConvergencesAndDivergences(ByRef NextNumberAvaiable As Integer)
        'Crea le convergenze e divergenze simultanee e memorizza nelle fasi i collegamenti
        'Convergenze simultanee
        If m_PreviousGraphicalStepsList.Count = 1 Then
            'Non occorre una convergenza e aggiunge riferimento alla fase precedente in XmlPreviousConnectionsList della transizione 
            XmlPreviousConnectionsList.Add(m_PreviousGraphicalStepsList(0))
            'Alla fase precedente aggiunge il riferimento a se stesso
            m_PreviousGraphicalStepsList(0).ReadXmlNextConnectionsList.Add(Me)

        Else
            'Occorre una convergenza, la crea e aggiunge il riferimento alle fasi precedenti in XmlPreviousConnectionsList della convergenza
            Dim NumberTemp As Integer = NextNumberAvaiable    'Numero della convergenza
            NextNumberAvaiable = NextNumberAvaiable + 1
            Dim ConnectionsListTemp As New ArrayList
            'Aggiunge la convergenza e aggiunge il riferimento alle fasi precedenti nella convergenza
            For Each F As BaseGraphicalStep In m_PreviousGraphicalStepsList
                ConnectionsListTemp.Add(F)
            Next F
            Dim ConvDivStrut As ConvDivStruct = m_Sfc.XmlSimultaneousConvergences.Add(NumberTemp, Nothing, Nothing, ConnectionsListTemp, Nothing, Nothing)
            'Aggiunge alla transizione il riferimento alla convergenza
            XmlPreviousConnectionsList.Add(ConvDivStrut)
            'Ad ogni fase precedente aggiunge il riferimento alla convergenza
            For Each F As BaseGraphicalStep In m_PreviousGraphicalStepsList
                F.ReadXmlNextConnectionsList.Add(ConvDivStrut)
            Next F
        End If

        'Divergenze simultanee
        If m_NextGraphicalStepsList.Count = 1 Then
            'Non occorre una di divergenza e aggiunge il riferimento alla transizione in XmlPreviousConnectionsLists della fase successiva 
            m_NextGraphicalStepsList(0).ReadXmlPreviousConnectionsList.add(Me)
        Else
            'Occorre una divergenza, la crea e aggiunge il numero nella XmlPreviosConnectionsLists della divergenza....
            '....il riferimento alla transizione
            Dim ConnectionsListTemp As New ArrayList    'Lista connessioni della divergenza
            ConnectionsListTemp.Add(Me)
            Dim NumberTemp As Integer = NextNumberAvaiable    'Numero della convergenza
            NextNumberAvaiable = NextNumberAvaiable + 1
            'Aggiunge la divergenza e aggiunge alla transizione il riferimento alla convergenza
            Dim ConvDivStrut As ConvDivStruct = m_Sfc.XmlSimultaneousDivergences.Add(NumberTemp, Nothing, Nothing, ConnectionsListTemp, Nothing, Nothing)
            'Aggiunge il riferimento alla convergenza creata in XMLPreviousConnectionsLists delle fasi successivi
            For Each F As BaseGraphicalStep In m_NextGraphicalStepsList
                F.ReadXmlPreviousConnectionsList.Add(ConvDivStrut)
            Next F
        End If
    End Sub
    Public Sub ResolveXmlConvergencesAndDivergences()
        Dim ST As ConvDivStruct
        For Each localId As Integer In XmlPreviousConnectionsList
            'Cerca l'id (il numero) tra le fasi
            Dim S As BaseGraphicalStep = m_Sfc.GraphicalStepsList.FindStepByNumber(localId)
            'Se la trova collega la transizione
            If Not IsNothing(S) Then
                Me.ReadPreviousGraphicalStepsList.Add(S)
            Else
                'Se non la trova è una convergenza simultanea o divergenza selettiva
                'cerca tra le convergenze simultanee
                ST = m_Sfc.XmlSimultaneousConvergences.FindStructByNumber(localId)
                'Se la trova collega gli elementi precedenti alla transizione
                If Not IsNothing(ST) Then
                    For Each ConvPrecLocalId As Integer In ST.XmlPreviousConnectionsList
                        'Cerca le fasi precedenti alla convergenza
                        S = m_Sfc.GraphicalStepsList.FindStepByNumber(ConvPrecLocalId)
                        'Se la trova collega la transizione
                        If Not IsNothing(S) Then
                            m_PreviousGraphicalStepsList.Add(S)
                        Else
                            'Se non è una fase è una divergenza selettiva
                            Dim ST2 As ConvDivStruct = m_Sfc.XmlSelectionDivergences.FindStructByNumber(ConvPrecLocalId)
                            'Se la trova collega le fasi precedenti alla transizione
                            For Each Conv2PrecLocalId As Integer In ST2.XmlPreviousConnectionsList
                                'Cerca le fasi precedenti alla convergenza
                                S = m_Sfc.GraphicalStepsList.FindStepByNumber(Conv2PrecLocalId)
                                'Se la trova collega la transizione
                                If Not IsNothing(S) Then
                                    m_PreviousGraphicalStepsList.Add(S)
                                End If
                            Next Conv2PrecLocalId
                        End If
                    Next ConvPrecLocalId
                Else
                    'E' una divergenza selettiva
                    ST = m_Sfc.XmlSelectionDivergences.FindStructByNumber(localId)
                    'Se la trova collega le fasi precedenti alla transizione
                    If Not IsNothing(ST) Then
                        'Cerca le fasi precedenti alla convergenza
                        For Each ConvPrecLocalId As Integer In ST.XmlPreviousConnectionsList
                            S = m_Sfc.GraphicalStepsList.FindStepByNumber(ConvPrecLocalId)
                            'Se la trova collega la transizione
                            If Not IsNothing(S) Then
                                m_PreviousGraphicalStepsList.Add(S)
                            End If
                        Next ConvPrecLocalId
                    End If
                End If
            End If
        Next localId
    End Sub
    Public Sub DisposeMe()
        Me.Finalize()
    End Sub
    Public Function ReadPreviousGraphicalStepsList() As GraphicalStepsList
        ReadPreviousGraphicalStepsList = m_PreviousGraphicalStepsList
    End Function
    Public Function ReadNextsGraphicalStepsList() As GraphicalStepsList
        ReadNextsGraphicalStepsList = m_NextGraphicalStepsList
    End Function
    Public Function ReadXmlPreviousConnectionsList() As ArrayList
        ReadXmlPreviousConnectionsList = XmlPreviousConnectionsList
    End Function
    Public Sub SetSuperable(ByVal Value As Boolean)
        m_Superable = Value
    End Sub
    Public Function ReadSuperable() As Boolean
        ReadSuperable = m_Superable
    End Function
    Public Sub SetCondition(ByVal Value As BooleanExpression)
        m_Condition = Value
    End Sub
    Public Function ReadCondition() As BooleanExpression
        ReadCondition = m_Condition
    End Function
    Public Sub SetActualCondition(ByVal Value As Boolean)
        'Se è cambiata la condizione ridisegna lo stato se richiesto
        If Value <> m_ActualCondition And DrawState Then
            DrawTransitionState()
        End If
        m_ActualCondition = Value
    End Sub
    Public Function ReadActualCondition() As Boolean
        ReadActualCondition = m_ActualCondition
    End Function
    Public Sub SetPosition(ByVal x As Integer)
        m_Position.X = x
    End Sub
    Public Function ReadPosition() As Integer
        ReadPosition = m_Position.X
    End Function
    Public Property Position() As Drawing.Point Implements IGraphicalObject.Position
        Get
            Return m_Position
        End Get
        Set(ByVal value As Drawing.Point)
            SetPosition(value.X)
        End Set
    End Property
    Public Sub SetDimension(ByVal Dimen As Integer)
        m_Dimension = Dimen
    End Sub
    Public Function ReadDimension() As Integer
        ReadDimension = m_Dimension
    End Function
    Public Property Size() As Drawing.Size Implements IGraphicalObject.Size
        Get
            Return New Drawing.Size(ReadDimension(), ReadDimension())
        End Get
        Set(ByVal value As Drawing.Size)
            SetDimension(Math.Max(value.Width, value.Height))
        End Set
    End Property
    Public Function ReadArea() As Drawing.Rectangle
        ReadArea = Area
    End Function
    Public Sub SetSelected(ByVal v As Boolean)
        m_Selected = v
    End Sub
    Public Sub SelectObject() Implements IGraphicalObject.SelectObject
        SetSelected(True)
    End Sub
    Public Sub DeselectObject() Implements IGraphicalObject.DeselectObject
        SetSelected(False)
    End Sub
    Public Function ReadSelected() As Boolean
        ReadSelected = m_Selected
    End Function
    Public Property Selected() As Boolean Implements IGraphicalObject.Selected
        Get
            Return ReadSelected()
        End Get
        Set(ByVal value As Boolean)
            SetSelected(value)
        End Set
    End Property
    Public Sub SetDrawState(ByRef Value As Boolean)
        DrawState = Value
    End Sub
    'Public Function TryCondition() As Boolean
    '    Dim Response As Boolean = m_Condition.Evaluate()
    '   'Se è cambiata la condizione ridisegna lo stato se richiesto
    '   If Response <> m_ActualCondition And DrawState Then
    '       m_ActualCondition = Response
    '       DrawTransitionState()
    '   End If
    '   TryCondition = m_ActualCondition
    'End Function
    Public Function TrySuperable() As Boolean
        'Calcola la condizione
        Dim Response As Boolean = m_Condition.Evaluate()
        'Se la condizione è vera valuta se è superabile
        If Response Then
            If m_PreviousGraphicalStepsList.ReadIfAllStepsActive Then
                'Tutte la fasi precedenti sono attive e la condizione è vara
                m_Superable = True
            Else
                m_Superable = False
            End If
        Else
            'La condizione è falsa
            m_Superable = False
        End If
        'Se la condizione è cambiata ridisegna lo stato se richiesto
        If Response <> m_ActualCondition And DrawState Then
            m_ActualCondition = Response
            DrawTransitionState()
        End If
        TrySuperable = m_Superable
    End Function
    Public Sub DeactivePreviousSteps()
        m_PreviousGraphicalStepsList.DeactiveAllSteps()
    End Sub
    Public Sub PreActiveNextSteps()
        m_NextGraphicalStepsList.PreActiveSteps()
    End Sub
    Public Sub SetGraphToDraw(ByRef Graph As Drawing.Graphics) Implements IGraphicalObject.SetGraphToDraw
        GraphToDraw = Graph
    End Sub
    Public Sub Draw(ByVal FlagShowDetails As Boolean) Implements IGraphicalObject.Draw
        Draw(SelectionColor, NotSelectionColor, TextColor, FlagShowDetails)
        'Disegna lo stato se richiesto
        If DrawState Then
            DrawTransitionState()
        End If
    End Sub
    Public Sub Cancel(ByVal FlagShowDetails As Boolean) Implements IGraphicalObject.Cancel
        Draw(BackColor, BackColor, BackColor, FlagShowDetails)
    End Sub
    Public Sub Draw(ByVal Col1 As Drawing.Color, ByVal Col2 As Drawing.Color, ByVal Col3 As Drawing.Color, ByVal FlagShowDetails As Boolean)
        Try
            If Monitor.TryEnter(GraphToDraw, 2000) Then
                Dim Right1, Left1, Right2, Left2, Max, Min, Med, Med1, Med2, MedMedi, TopArea, BottomArea, LeftArea, RightArea As Integer
                TopArea = 2147483647
                BottomArea = 0

                CalculusTransitionPoints(Right1, Left1, Right2, Left2, Max, Min, Med, Med1, Med2, MedMedi, m_Dimension)

                'Store LeftArea
                If Left1 < Left2 Then
                    LeftArea = Left1
                Else
                    LeftArea = Left2
                End If

                'Store RightArea
                If Right2 > Right1 Then
                    RightArea = Right2
                Else
                    RightArea = Right1
                End If

                'Snappa i coefficienti
                MedMedi = Snap(MedMedi, CoeffSnap)
                Med = Snap(Med, CoeffSnap)
                Med1 = Snap(Med1, CoeffSnap)
                Med2 = Snap(Med2, CoeffSnap)

                Dim p1, p2 As Drawing.Point
                Dim Penna As New Drawing.Pen(Col1)
                If m_Selected Then
                    'Penna.Color = SelectionColor
                Else
                    Penna.Color = Col2
                End If
                Penna.Width = 1

                'Controlla se sono una o più fasi precedenti
                If m_PreviousGraphicalStepsList.CountStepsList > 1 Then 'sono più fasi quindi disegna la linea doppia
                    'Disegna la linee orizzontali superiori
                    p1.X = Left1
                    p1.Y = Max + m_Dimension
                    p2.X = Right1
                    p2.Y = Max + m_Dimension
                    GraphToDraw.DrawLine(Penna, p1, p2)
                    p1.Y = p1.Y + 2
                    p2.Y = p2.Y + 2
                    GraphToDraw.DrawLine(Penna, p1, p2)
                    'Collega le fasi con le linee orizzontali
                    For Each F As BaseGraphicalStep In m_PreviousGraphicalStepsList
                        p1 = F.ReadPosition
                        p1.X = p1.X
                        p1.Y = p1.Y + CInt(m_Dimension / 2)
                        p2.X = p1.X
                        p2.Y = Max + m_Dimension
                        GraphToDraw.DrawLine(Penna, p1, p2)
                        'Memorizza TopArea
                        If p1.Y < TopArea Then
                            TopArea = p1.Y
                        End If
                    Next F
                End If

                'Controlla se sono una o più Steps successive
                If m_NextGraphicalStepsList.CountStepsList > 1 Then  'sono più Steps quindi Draw la linea doppia
                    'Disegna la linea orizzontale inferiore
                    p1.X = Left2
                    p1.Y = Min - m_Dimension
                    p2.X = Right2
                    p2.Y = Min - m_Dimension
                    GraphToDraw.DrawLine(Penna, p1, p2)
                    p1.Y = p1.Y - 2
                    p2.Y = p2.Y - 2
                    GraphToDraw.DrawLine(Penna, p1, p2)
                    'Collega le Steps con le linee orizzontali            
                    For Each F As BaseGraphicalStep In m_NextGraphicalStepsList
                        p1 = F.ReadPosition
                        p1.X = p1.X
                        p1.Y = p1.Y + -CInt(m_Dimension / 2)
                        p2.X = p1.X
                        p2.Y = Min - m_Dimension
                        GraphToDraw.DrawLine(Penna, p1, p2)
                        'Memorizza BottomArea
                        If p1.Y > BottomArea Then
                            BottomArea = p1.Y
                        End If
                    Next F
                End If

                'Linea superiore verticale
                p1.X = Med1
                p1.Y = Max + m_Dimension + 2
                p2.X = Med1
                p2.Y = Max + m_Dimension + CInt(m_Dimension / 4) + 2
                GraphToDraw.DrawLine(Penna, p1, p2)
                'Store TopArea
                If p1.Y < TopArea Then
                    TopArea = p1.Y
                End If
                'Memorizza BottomArea
                If p2.Y > BottomArea Then
                    BottomArea = p2.Y
                End If


                'Linea inferiore verticale
                p1.X = Med2
                p1.Y = Min - m_Dimension - 2
                p2.X = Med2
                p2.Y = Min - m_Dimension - CInt(m_Dimension / 4) - 2
                GraphToDraw.DrawLine(Penna, p1, p2)
                'Memorizza TopArea
                If p2.Y < TopArea Then
                    TopArea = p2.Y
                End If
                'Memorizza BottomArea
                If p1.Y > BottomArea Then
                    BottomArea = p1.Y
                End If

                'Linea orizz1
                p1.X = Med1
                p1.Y = Max + m_Dimension + CInt(m_Dimension / 4) + 2
                p2.X = MedMedi
                p2.Y = p1.Y
                GraphToDraw.DrawLine(Penna, p1, p2)

                'Linea orizz2
                p1.X = Med2
                p1.Y = Min - m_Dimension - CInt(m_Dimension / 4) - 2
                p2.X = MedMedi
                p2.Y = p1.Y
                GraphToDraw.DrawLine(Penna, p1, p2)

                'Linea verticale centrale
                p1.X = MedMedi
                p1.Y = Max + m_Dimension + CInt(m_Dimension / 4) + 2
                p2.X = MedMedi
                p2.Y = Min - m_Dimension - CInt(m_Dimension / 4) - 2
                GraphToDraw.DrawLine(Penna, p1, p2)
                Penna.Width = 2

                'Trattino della Transition
                p1.X = MedMedi - CInt(m_Dimension / 5)
                p1.Y = Med
                p2.X = MedMedi + CInt(m_Dimension / 5)
                p2.Y = p1.Y
                Penna.Width = 4
                GraphToDraw.DrawLine(Penna, p1, p2)

                'Memorizza la m_Position.x se è la prima volta che la disegna
                If m_Position.X = -1 Then
                    m_Position.X = MedMedi
                End If
                'Memorizza la m_Position.y 
                m_Position.Y = Med
                'Memorizza LeftArea
                If p1.X < LeftArea Then
                    LeftArea = p1.X
                End If

                'Testo nella transizione
                Dim Rect As Drawing.SizeF
                Rect = GraphToDraw.MeasureString(m_Number, Carattere)
                p1.X = m_Position.X + m_Dimension / 5 + 2
                p1.Y = Med - (Rect.Height / 2)
                Dim Br As New Drawing.SolidBrush(Col3)

                Dim Misure As Integer
                If FlagShowDetails Then
                    'Disegna il testo della condizione
                    Misure = GraphToDraw.MeasureString(m_Condition.GetExpressionString, Carattere).Width
                    'Misure = GraphToDraw.MeasureString(m_Name & " - " & m_Condition.GetExpressionString, Carattere).Width
                    GraphToDraw.DrawString(m_Name & " - " & m_Condition.GetExpressionString, Carattere, Br, p1.X, p1.Y)
                Else
                    Misure = GraphToDraw.MeasureString(m_Name, Carattere).Width
                    GraphToDraw.DrawString(m_Name, Carattere, Br, p1.X, p1.Y)
                End If
                'Aggiorna l'area dopo la scritta della transizione
                If p1.X + Misure > RightArea Then
                    RightArea = p1.X + Misure
                End If
                'Store RightArea
                If p1.X + Rect.Width + 1 > RightArea Then
                    RightArea = p1.X + Rect.Width + 1
                End If

                'StoreArea
                Area.X = LeftArea
                Area.Y = TopArea
                Area.Width = RightArea - LeftArea
                Area.Height = BottomArea - TopArea

            End If
        Catch ex As System.Exception
        Finally
            Monitor.Exit(GraphToDraw)
        End Try
        'GraphToDraw.DrawRectangle(Penna, LeftArea, TopArea, RightArea - LeftArea, BottomArea - TopArea)


    End Sub
    Public Sub DrawTransitionState()
        Try
            If Monitor.TryEnter(GraphToDraw, 2000) Then
                Dim p1, p2 As Drawing.Point
                Dim Penna As New Drawing.Pen(ColorConditionFalse)
                If m_ActualCondition Then
                    Penna.Color = ColorConditionTrue
                End If
                'Trattino della Transition
                p1.X = m_Position.X - CInt(m_Dimension / 5)
                p1.Y = m_Position.Y
                p2.X = m_Position.X + CInt(m_Dimension / 5)
                p2.Y = p1.Y
                Penna.Width = 4
                GraphToDraw.DrawLine(Penna, p1, p2)
            End If
        Catch ex As System.Exception
        Finally
            Monitor.Exit(GraphToDraw)
        End Try
    End Sub
    Private Sub CalculusTransitionPoints(ByRef Right1 As Integer, ByRef Left1 As Integer, ByRef Right2 As Integer, ByRef Left2 As Integer, ByRef Max As Integer, ByRef Min As Integer, ByRef Med As Integer, ByRef Med1 As Integer, ByRef Med2 As Integer, ByRef MedMedi As Integer, ByVal m_Dimension As Integer)
        Dim temp, OffsetSu, OffsetGiu As Integer

        'Calcola il max delle precedenti
        'Trova la fase più a sinistra e più a destra tra le precedenti
        Max = 0
        Left1 = Integer.MaxValue
        Right1 = 0
        For Each F As BaseGraphicalStep In m_PreviousGraphicalStepsList
            temp = F.ReadPosition.Y
            If Max < temp Then
                Max = temp
            End If
            temp = F.ReadPosition.X
            If Left1 > temp Then
                Left1 = temp
            End If
            If Right1 < temp Then
                Right1 = temp
            End If
        Next

        'Disegna la linee orizzontali superiori
        Dim p1 As New Drawing.Point
        Dim p2 As New Drawing.Point

        'Calcola il min delle successive
        'Trova la fase più a sinistra e più a destra tra le successive
        Min = Integer.MaxValue
        Left2 = Integer.MaxValue
        Right2 = 0
        For Each F As BaseGraphicalStep In m_NextGraphicalStepsList
            temp = F.ReadPosition.Y
            If Min > temp Then
                Min = temp
            End If
            temp = F.ReadPosition.X
            If Left2 > temp Then
                Left2 = temp
            End If
            If Right2 < temp Then
                Right2 = temp
            End If
        Next F



        'Trova i punti medi X
        Med1 = CInt((Right1 + Left1) / 2)
        Med2 = CInt((Right2 + Left2) / 2)

        'Se la m_Position.x non è impostata la calcola
        If m_Position.X = -1 Then
            'Muove la transizione a sinistra se è salente
            If Max >= Min Then
                MedMedi = Math.Min(Left1, Left2) - CInt(m_Dimension * 1.6)
                'se è < m_Dimension/5 lo porta a 0 m_Dimension
                If MedMedi < m_Dimension / 5 Then
                    MedMedi = CInt(m_Dimension / 5)
                End If
            Else
                MedMedi = (Med1 + Med2) / 2
            End If
        Else
            'altrimenti la legge
            MedMedi = m_Position.X
        End If

        'Calcola gli offset
        If Left1 = Right1 Then 'se c'è solo una fase precedente tira tutto più sù
            OffsetSu = CInt(m_Dimension / 2)
        End If
        If Left2 = Right2 Then 'se c'è solo una fase successiva tira tutto più giù
            OffsetGiu = CInt(m_Dimension / 2)
        End If

        Max = Max - OffsetSu
        Min = Min + OffsetGiu

        'Punto medio Y

        '  Try

        Med = CInt((Min / 2) + (Max / 2))

        'Catch ex As Exception

        ' End Try

    End Sub
    Public Function MyAreaIsHere(ByVal x As Integer, ByVal y As Integer) As Boolean
        Dim Right1, Left1, Right2, Left2, Max, Min, Med, Med1, Med2, MedMedi As Integer
        CalculusTransitionPoints(Right1, Left1, Right2, Left2, Max, Min, Med, Med1, Med2, MedMedi, m_Dimension)
        If Abs(x - m_Position.X) < CInt(m_Dimension / 5) And Abs(y - Med) < CInt(m_Dimension / 5) Then
            MyAreaIsHere = True
        End If
    End Function
    Private Function Snap(ByVal val As Integer, ByVal CoeffSnap As Integer) As Integer
        Snap = CInt(val / CoeffSnap) * CoeffSnap
    End Function
    Public Sub CalculateArea() Implements IGraphicalObject.CalculateArea
        CalculusArea()
    End Sub
    Public Sub CalculusArea()
        Dim Right1, Left1, Right2, Left2, Max, Min, Med, Med1, Med2, MedMedi, TopArea, BottomArea, LeftArea, RightArea As Integer
        Dim p1, p2 As Drawing.Point
        TopArea = 2147483647
        BottomArea = 0

        CalculusTransitionPoints(Right1, Left1, Right2, Left2, Max, Min, Med, Med1, Med2, MedMedi, m_Dimension)

        'Memorizza LeftArea
        If Left1 < Left2 Then
            LeftArea = Left1
        Else
            LeftArea = Left2
        End If

        'Memorizza RightArea
        If Right2 > Right1 Then
            RightArea = Right2
        Else
            RightArea = Right1
        End If

        p1.Y = Max + m_Dimension + 2
        p2.Y = Max + m_Dimension + CInt(m_Dimension / 4) + 2

        'Memorizza TopArea
        If p1.Y < TopArea Then
            TopArea = p1.Y
        End If
        'Memorizza BottomArea
        If p2.Y > BottomArea Then
            BottomArea = p2.Y
        End If

        p1.Y = Min - m_Dimension - 2
        p2.Y = Min - m_Dimension - CInt(m_Dimension / 4) - 2

        'Memorizza TopArea
        If p2.Y < TopArea Then
            TopArea = p2.Y
        End If
        'Memorizza BottomArea
        If p1.Y > BottomArea Then
            BottomArea = p1.Y
        End If

        'Trattino della Transition
        p1.X = MedMedi - CInt(m_Dimension / 5)

        'Memorizza LeftArea
        If p1.X < LeftArea Then
            LeftArea = p1.X
        End If

        'Testo nella Transition
        p1.X = m_Position.X + m_Dimension / 5 + 2

        'Memorizza RightArea
        If p1.X + m_Dimension + 1 > RightArea Then
            RightArea = p1.X + m_Dimension + 1
        End If

        'Memorizza Area
        Area.X = LeftArea - 1
        Area.Y = TopArea + 1
        Area.Width = RightArea - LeftArea + 2
        Area.Height = BottomArea - TopArea + 2

    End Sub
    Public Sub Move(ByVal dx As Integer)
        m_Position.X = m_Position.X + dx
        CalculusArea()
    End Sub
    Public Sub WriteFile(ByRef W As BinaryWriter)
        W.Write(m_Number)
        W.Write(m_Condition.GetExpressionString)
        W.Write(m_Position.X)

        'm_PreviousGraphicalStepsList
        W.Write(m_PreviousGraphicalStepsList.CountStepsList)
        For Each F As BaseGraphicalStep In m_PreviousGraphicalStepsList
            W.Write(F.Number)
        Next F
        'm_NextGraphicalStepsList
        W.Write(m_NextGraphicalStepsList.CountStepsList)
        For Each F As BaseGraphicalStep In m_NextGraphicalStepsList
            W.Write(F.Number)
        Next F

    End Sub
    Public Function GetIdentifier() As String Implements IDocumentable.GetIdentifier
        Return "Transition " + Me.Name
    End Function
    Public Function GetDescription() As String Implements IDocumentable.GetDescription
        Return GetSystemDescription() + IIf(Of String)(Documentation = "", "", " (" + Documentation + ")")
    End Function
    Public Function GetSystemDescription() As String Implements IDocumentable.GetSystemDescription
        Dim desc As String = GetIdentifier()
        desc += " from " + Me.m_PreviousGraphicalStepsList.ListSteps() + " to " + _
            Me.m_NextGraphicalStepsList.ListSteps()
        desc += " superable if " + Me.ReadCondition().GetExpressionString()
        Return desc
    End Function
End Class
