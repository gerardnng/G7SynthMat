Imports System.Math
Imports System.IO
Imports System.Xml
Imports System.Threading.Timer
Public MustInherit Class BaseGraphicalContant

    Protected m_Name As String
    Protected m_documentation As String
    Protected m_Number As Integer
    Protected m_Initial As Boolean
    Protected m_Negated As Boolean
    Protected m_Active As Boolean
    Protected m_PreActive As Boolean
    Protected m_TimeActivation As DateTime
    Protected m_Final As Boolean
    Protected m_Ladder As Ladder
    Protected Position As Drawing.Point
    Protected Area As Drawing.Rectangle
    Protected Selected As Boolean
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
    Property Name() As String
        Get
            Name = m_Name
        End Get
        Set(ByVal Value As String)
            m_Name = Value
        End Set
    End Property
    Property Documentation() As String
        Get
            Documentation = m_documentation
        End Get
        Set(ByVal Value As String)
            m_documentation = Value
        End Set
    End Property
    Property Number() As String
        Get
            Number = m_Number
        End Get
        Set(ByVal Value As String)
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
    Public MustOverride Sub Cancell(ByVal CancellSmallRectangels As Boolean)
    Public MustOverride Sub DrawContact(ByVal DrawSmallRectangels As Boolean)
    'Public MustOverride Sub DrawNegativeContact(ByVal DrawSmallRectangels As Boolean)
    Public MustOverride Sub CalculusArea()
    Public MustOverride Sub DrawStepState(ByVal Cancel As Boolean)
    Public MustOverride Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter)
    Public MustOverride Sub xmlImport(ByRef RefXMLProjectReader As XmlTextReader)
    Public MustOverride Sub ResolveVariablesLinks()
    Public MustOverride Sub Move(ByVal dx As Integer, ByVal dy As Integer)
    Public MustOverride Sub SetGraphToDraw(ByRef Graph As Drawing.Graphics)
    Public MustOverride Function CreateInstance(ByRef NewLadder As Ladder) As Object

    Public Sub New(ByVal ContactNumber As Integer, ByRef ContactName As String, ByVal ContactDocumentation As String, ByRef RefSfc As Ladder, ByVal Ini As Boolean, ByVal Fin As Boolean, ByVal P As Drawing.Point, ByVal BackCol As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, ByRef Graph As Drawing.Graphics, ByVal DrState As Boolean, ByVal Dimen As Integer)
        m_Number = ContactNumber
        m_Name = ContactName
        m_documentation = ContactDocumentation
        m_Ladder = RefSfc
        m_Initial = Ini
        m_Final = Fin
        Position = P
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

    Public Sub New(ByVal RefSfc As Ladder, ByVal BackCol As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, ByVal Graph As Drawing.Graphics, ByVal DrState As Boolean, ByVal Dimen As Integer)
        m_Ladder = RefSfc
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

    'mancano le classi di XML


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
        Position = p
        CalculusArea()
    End Sub
    Public Function ReadPosition() As Drawing.Point
        ReadPosition = Position
    End Function
    Public Sub SetSelected(ByVal v As Boolean)
        Selected = v
    End Sub
    Public Function ReadSelected() As Boolean
        ReadSelected = Selected
    End Function
    Public Sub SetDimension(ByVal Dimen As Integer)
        m_Dimension = Dimen
    End Sub
    Public Function ReadDimension() As Integer
        ReadDimension = m_Dimension
    End Function
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
    Public Function CeIlMioRetLeft(ByVal x As Integer, ByVal y As Integer) As Boolean
        If Abs(y - Position.Y) <= CInt(m_Dimension / 10) And Abs(x - (Position.X - CInt(m_Dimension / 2) - CInt(m_Dimension / 10))) <= (m_Dimension / 10) Then
            CeIlMioRetLeft = True
        End If
    End Function
    Public Function CeIlMioRetRight(ByVal x As Integer, ByVal y As Integer) As Boolean
        If Abs(y - Position.Y) <= CInt(m_Dimension / 10) And Abs(x - (Position.X + CInt(m_Dimension / 2) + CInt(m_Dimension / 10))) <= (m_Dimension / 10) Then
            CeIlMioRetRight = True
        End If
    End Function
    Public Function CeIlMioRetLeftRail(ByVal x As Integer, ByVal y As Integer) As Boolean
        'If Abs(y - (Position.Y)) <= CInt(m_Dimension / 10) And Abs(x - (Position.X + 742)) <= (m_Dimension / 10) Then


        If Abs(y - Position.Y) <= CInt(m_Dimension / 10) And Abs(x - (Position.X - 18)) <= (m_Dimension / 10) Then
            CeIlMioRetLeftRail = True
        End If
    End Function
    Public Function CeIlMioRetRightRail(ByVal x As Integer, ByVal y As Integer) As Boolean
        'If Abs(y - Position.Y) <= CInt(m_Dimension / 10) And Abs(x - (Position.X - 18)) <= (m_Dimension / 10) Then
        If Abs(y - (Position.Y)) <= CInt(m_Dimension / 10) And Abs(x - (Position.X + 742)) <= (m_Dimension / 10) Then
            CeIlMioRetRightRail = True
        End If
    End Function


    Public Function MyAreaIsHere(ByVal x As Integer, ByVal y As Integer) As Boolean
        If Abs(x - Position.X) <= (m_Dimension / 2) And Abs(y - Position.Y) <= (m_Dimension / 2) Then
            MyAreaIsHere = True
        End If
    End Function

End Class
