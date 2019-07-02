Imports System.Math
Imports System.IO
Imports System.Drawing
Imports System.Xml
Imports System.Threading.Timer
Public MustInherit Class BaseGraphicalContact
    Implements IDocumentable, IXMLSerializable, IHasLocalId, IPositionable, ISelectable

    Protected WithEvents m_Var As BaseVariable
    Protected m_Name As String
    Protected m_VarName As String
    Protected m_Ind As BooleanVariable
    Protected m_quly As String
    Protected m_time As TimeSpan
    Protected m_varvalue As String
    Protected m_Documentation As String
    Protected m_Number As Integer
    Protected m_Initial As Boolean
    Protected m_Negated As Boolean
    Protected m_Id As Integer
    Protected m_Final As Boolean
    Protected m_Ladder As Ladder
    Protected Position As Drawing.Point
    Protected Area As Drawing.Rectangle
    Protected m_Selected As Boolean
    Protected TopRectSelected As Boolean
    Protected BottomRectSelected As Boolean
    Protected BackColor As Drawing.Color
    Protected SelectionColor As Drawing.Color
    Protected NotSelectionColor As Drawing.Color
    Protected TextColor As Drawing.Color
    Protected m_Dimension As Drawing.Size
    Protected Carattere As Drawing.Font
    Protected ColorActive As Drawing.Color
    Protected ColorPreActive As Drawing.Color
    Protected ColorDeactive As Drawing.Color
    Protected GraphToDraw As Drawing.Graphics
    Protected DrawState As Boolean
    ' Usato per i riconoscitori di fronte
    Private m_LastRungValue As Boolean
    Protected Shared m_DrawByPowerFlow As Boolean = Preferences.GetBoolean("PowerFlowColor", False)
    Protected m_MyOutgoingConnections As ArrayList

    Public Property Name() As String Implements IDocumentable.Name
        Get
            Name = m_Name
        End Get
        Set(ByVal Value As String)
            m_Name = Value
        End Set
    End Property
    Property VarValue() As String
        Get
            VarValue = m_varvalue
        End Get
        Set(ByVal Value As String)
            m_varvalue = Value
        End Set
    End Property

    Property Documentation() As String Implements IDocumentable.Documentation
        Get
            Return m_Documentation
        End Get
        Set(ByVal Value As String)
            m_Documentation = Value
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
    Property Initial() As Boolean
        Get
            Initial = m_Initial
        End Get
        Set(ByVal Value As Boolean)
            m_Initial = Value
        End Set
    End Property
    Property Negated() As Boolean
        Get
            Negated = m_Negated
        End Get
        Set(ByVal Value As Boolean)
            m_Negated = Value
        End Set
    End Property

    Public MustOverride Sub DisposeMe()
    Public MustOverride Sub Cancel(ByVal CancelSmallRectangels As Boolean, ByVal Id As Integer)
    Public MustOverride Sub Cancel(ByVal CancelSmallRectangels As Boolean, ByVal Id As Integer, _
        ByVal Name As String, ByVal Qualy As String, ByVal Var As BaseVariable , ByVal Ind As BooleanVariable, _
        ByVal Time As TimeSpan)
    Public MustOverride Sub DrawContact(ByVal CancelSmallRectangels As Boolean, ByVal Id As Integer)
    Public MustOverride Sub DrawContact(ByVal DrawSmallRectangels As Boolean, ByVal Id As Integer, _
        ByVal Name As String, ByVal Qualy As String, ByVal Var As BaseVariable, ByVal Ind As BooleanVariable, _
        ByVal Time As TimeSpan)
    Public MustOverride Sub SetFinalValue()
    Public MustOverride Sub CalculusArea()
    Public MustOverride Sub DrawStepState(ByVal Cancel As Boolean)
    Public MustOverride Sub DrawStepState(ByVal Cancel As Boolean, ByVal Var As BaseVariable)
    Public MustOverride Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter) _
        Implements IXMLSerializable.xmlExport
    Public MustOverride Sub xmlImport(ByRef RefXMLProjectReader As XmlTextReader) _
        Implements IXMLSerializable.xmlImport
    Public MustOverride Sub ResolveVariablesLinks()
    Public MustOverride Sub Move(ByVal dx As Integer, ByVal dy As Integer)
    Public MustOverride Sub SetGraphToDraw(ByRef Graph As Drawing.Graphics)
    Public MustOverride Function CreateInstance(ByRef NewLadder As Ladder) As Object

    Public Sub New(ByVal ContactNumber As Integer, ByRef ContactName As String, _
        ByVal ContactDocumentation As String, ByRef RefSfc As Ladder, ByVal Ini As Boolean, _
        ByVal Fin As Boolean, ByVal P As Drawing.Point, ByVal BackCol As Drawing.Color, _
        ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, _
        ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, _
        ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, _
        ByVal ColPreActive As Drawing.Color, ByRef Graph As Drawing.Graphics, _
        ByVal DrState As Boolean, ByVal Dimen As Size, ByVal Id As Integer, _
        ByVal Name As String, ByVal Qualy As String, ByVal Var As BaseVariable, _
        ByVal Ind As BooleanVariable, ByVal Time As TimeSpan)
        m_Number = ContactNumber
        m_Name = ContactName
        m_Documentation = ContactDocumentation
        m_Ladder = RefSfc
        m_Id = Id
        m_VarName = Name
        m_quly = Qualy
        m_Var = Var
        m_Ind = Ind
        m_time = Time
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
    End Sub

    Public Sub New(ByVal ContactNumber As Integer, ByRef ContactName As String, _
        ByVal ContactDocumentation As String, ByRef RefSfc As Ladder, ByVal Ini As Boolean, _
        ByVal Fin As Boolean, ByVal P As Drawing.Point, ByVal BackCol As Drawing.Color, _
        ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, _
        ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, _
        ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, _
        ByVal ColPreActive As Drawing.Color, ByRef Graph As Drawing.Graphics, _
        ByVal DrState As Boolean, ByVal Dimen As Integer, ByVal Id As Integer, _
        ByVal Name As String, ByVal Qualy As String, ByVal Var As BaseVariable, _
        ByVal Ind As BooleanVariable, ByVal Time As TimeSpan)
        m_Number = ContactNumber
        m_Name = ContactName
        m_Documentation = ContactDocumentation
        m_Ladder = RefSfc
        m_Id = Id
        m_VarName = Name
        m_quly = Qualy
        m_Var = Var
        m_Ind = Ind
        m_time = Time
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
        m_Dimension = New Size(Dimen, Dimen)
        Area = New Drawing.Rectangle
    End Sub

    Public Sub New(ByVal RefSfc As Ladder, ByVal BackCol As Drawing.Color, _
        ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, _
        ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, _
        ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, _
        ByVal ColPreActive As Drawing.Color, ByVal Graph As Drawing.Graphics, _
        ByVal DrState As Boolean, ByVal Dimen As Size)
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
    End Sub

    Public Sub New(ByVal RefSfc As Ladder, ByVal BackCol As Drawing.Color, _
        ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, _
        ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, _
        ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, _
        ByVal ColPreActive As Drawing.Color, ByVal Graph As Drawing.Graphics, _
        ByVal DrState As Boolean, ByVal Dimen As Integer)
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
        m_Dimension = New Size(Dimen, Dimen)
        Area = New Drawing.Rectangle
    End Sub
    Public Sub New()
    End Sub
    Public Sub SetDrawState(ByRef Value As Boolean)
        DrawState = Value
    End Sub
    Public Sub SetFinal(ByVal Value As Boolean)
        m_Var.SetValue(Value)
    End Sub
    Public Function ReadFinal() As Boolean
        ReadFinal = m_Final
    End Function
    Public Sub SetInitial(ByVal Value As Boolean)
        m_Initial = Value
    End Sub
    Public Sub ResetInitial()
        m_Initial = False
        m_LastRungValue = False
    End Sub
    Public Function ReadInitial() As Boolean
        ReadInitial = m_Initial
    End Function
    Public Sub SetPosition(ByVal p As Drawing.Point) Implements IPositionable.SetPosition
        Position = p
        CalculusArea()
    End Sub
    Public Function ReadPosition() As Drawing.Point Implements IPositionable.ReadPosition
        ReadPosition = Position
    End Function
    Public Sub SetSelected(ByVal v As Boolean)
        m_Selected = v
    End Sub
    Public Function ReadSelected() As Boolean
        ReadSelected = m_Selected
    End Function
    Public Sub SelectObject() Implements ISelectable.SelectObject
        SetSelected(True)
    End Sub
    Public Sub DeselectObject() Implements ISelectable.DeselectObject
        SetSelected(False)
    End Sub
    Public Property Selected() As Boolean Implements ISelectable.Selected
        Get
            Return ReadSelected()
        End Get
        Set(ByVal value As Boolean)
            SetSelected(value)
        End Set
    End Property
    Public Sub SetDimension(ByVal Dimen As Integer)
        SetDimension(New Size(Dimen, Dimen))
    End Sub
    Public Sub SetDimension(ByVal Dimen As Size)
        m_Dimension = Dimen
    End Sub
    Public Function ReadDimension() As Size
        ReadDimension = m_Dimension
    End Function
    Public Sub SetTopRectSelected(ByVal v As Boolean)
        TopRectSelected = v
    End Sub
    Public Function ReadTopRectSelected() As Boolean
        ReadTopRectSelected = TopRectSelected
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
    Public Function ReadId() As Integer
        ReadId = m_Id
    End Function
    Public Function ReadVar() As BaseVariable
        ReadVar = m_Var
    End Function
    Public Sub SetVar(ByVal v As BaseVariable)
        m_Var = v
    End Sub
    Public Property BoundVariable() As BaseVariable
        Get
            Return ReadVar()
        End Get
        Set(ByVal value As BaseVariable)
            SetVar(value)
        End Set
    End Property
    Public Function ReadInd() As BooleanVariable
        ReadInd = m_Ind
    End Function
    Public Function ReadQualy() As String
        ReadQualy = m_quly
    End Function
    Public Function ReadVarName() As String
        ReadVarName = m_VarName
    End Function
    Public Function ReadTime() As TimeSpan
        ReadTime = m_time
    End Function
    Public ReadOnly Property ObjectRectangle() As Rectangle
        Get
            Dim p As Point = Position
            Dim s As Size = ReadDimension()
            p.Offset(-s.Width / 2, -s.Height / 2)
            Return New Rectangle(p, s)
        End Get
    End Property

    Public Function CeIlMioRetLeft(ByVal x As Integer, ByVal y As Integer) As Boolean
        If Abs(y - Position.Y) <= CInt(m_Dimension.Height / 10) And _
                Abs(x - (Position.X - CInt(m_Dimension.Width / 2) - CInt(m_Dimension.Width / 10))) _
                    <= (m_Dimension.Width / 10) Then
            CeIlMioRetLeft = True
        End If
    End Function
    Public Function CeIlMioRetRight(ByVal x As Integer, ByVal y As Integer) As Boolean
        If Abs(y - Position.Y) <= CInt(m_Dimension.Height / 10) And _
            Abs(x - (Position.X + CInt(m_Dimension.Width / 2) + CInt(m_Dimension.Width / 10))) _
                <= (m_Dimension.Width / 10) Then
            CeIlMioRetRight = True
        End If
    End Function
    Public Function CeIlMioRetLeftRail(ByVal x As Integer, ByVal y As Integer) As Boolean
        If Abs(y - Position.Y) <= CInt(m_Dimension.Height / 10) And _
                Abs(x - (Position.X - 18)) <= (m_Dimension.Width / 10) Then
            CeIlMioRetLeftRail = True
        End If
    End Function
    Public Function CeIlMioRetRightRail(ByVal x As Integer, ByVal y As Integer) As Boolean
        If Abs(y - (Position.Y)) <= CInt(m_Dimension.Height / 10) And _
                Abs(x - (Position.X + 742)) <= (m_Dimension.Width / 10) Then
            CeIlMioRetRightRail = True
        End If
    End Function
    Public Function MyAreaIsHere(ByVal x As Integer, ByVal y As Integer) As Boolean
        Return ObjectRectangle.Contains(x, y)
    End Function
    Public Property Id() As Integer
        Get
            Return m_Id
        End Get
        Protected Set(ByVal value As Integer)
            m_Id = value
        End Set
    End Property
    Public Property Qualy() As String
        Get
            Return m_quly
        End Get
        Set(ByVal value As String)
            m_quly = value
        End Set
    End Property
    Public ReadOnly Property IsContact() As Boolean
        Get
            Return (TypeOf (Me) Is GraphicalContact) AndAlso (Me.Id = 1)
        End Get
    End Property
    Public ReadOnly Property IsCoil() As Boolean
        Get
            Return (TypeOf (Me) Is GraphicalContact) AndAlso (Me.Id = 2)
        End Get
    End Property
    Public ReadOnly Property IsBlock() As Boolean
        Get
            Return (TypeOf (Me) Is GraphicalContact) AndAlso (Me.Id = 3)
        End Get
    End Property

    Public ReadOnly Property Container() As Ladder
        Get
            Return m_Ladder
        End Get
    End Property

    Public Sub SetLastValue()
        If Not IsContact Then Return
        Dim var As BaseVariable = Me.ReadVar()
        If Not IsNothing(var) AndAlso Not IsNothing(var.Name) Then _
            m_LastRungValue = var.ReadValue()
    End Sub

    Protected Function CalculatePowerFlowThroughMe() As Boolean
        If IsCoil Then
            Return ReadVar.ReadValue()
        ElseIf IsBlock Then
            Return True
        Else
            Select Case Qualy
                Case "Normal Open Contact"
                    Return ReadVar().ReadValue()
                Case "Normal Closed Contact"
                    Return Not (ReadVar().ReadValue())
                Case "Positive Transition-sensing Contact"
                    If m_LastRungValue Then
                        Return False
                    Else
                        Return ReadVar().ReadValue()
                    End If
                Case "Negative Transition-sensing Contact"
                    If m_LastRungValue Then
                        Return Not (ReadVar().ReadValue())
                    Else
                        Return False
                    End If
            End Select
        End If
    End Function

    ' Ritorna un termine di espressione equivalente a questo contatto
    Public Function GetTerm() As String
        If Not IsContact Then Return ""
        Dim var As BaseVariable = Me.ReadVar()
        If IsNothing(var) OrElse IsNothing(var.Name) Then Return ""
        If m_Ladder.IsFirstScan Then m_LastRungValue = m_Var.ReadValue()
        Dim ret As String = ""
        Select Case Qualy
            Case "Normal Open Contact"
                ret = var.Name
            Case "Normal Closed Contact"
                ret = "!" + var.Name
            Case "Positive Transition-sensing Contact"
                If m_LastRungValue Then
                    ret = "false"
                Else
                    ret = var.Name
                End If
            Case "Negative Transition-sensing Contact"
                If m_LastRungValue Then
                    ret = "!" + var.Name
                Else
                    ret = "false"
                End If
            Case Else
                ' I blocchi non devono interferire con la valutazione complessiva del Rung
                ' In effetti la versione attuale del parser RtL evita di inserire termini true ridondanti
                ' per i blocchi, ma meglio essere previdenti in caso di future modifiche
                If IsBlock Then Return "true"
        End Select
        ' bug fix esempio 2.5 "Chiacchio - Basile" Tecnologie informatiche x l'automazione
        'm_LastRungValue = var.ReadValue()
        Return ret
    End Function

    ' Imposta il coil ad un valore appropriato in base al valore del rung
    ' Ritorna True se il valore è cambiato
    Public Function SetCoilValue(ByVal rng As Rung, _
        ByVal rungValue As Boolean) As Boolean
        If m_Ladder.IsFirstScan Then m_LastRungValue = rungValue
        Dim anyChange As Boolean = False
        If Not IsCoil Then Return False
        Dim var As BaseVariable = Me.ReadVar()
        ' Non dovrebbe succedere...
        If IsNothing(var) OrElse IsNothing(var.Name) Then Return False
        Select Case Qualy
            Case "Normal Coil"
                anyChange = var.ReadValue <> rungValue
                rng.SetValue(var, rungValue)
            Case "Negative Coil"
                anyChange = var.ReadValue = rungValue
                rng.SetValue(var, Not (rungValue))
            Case "Reset Coil"
                anyChange = var.ReadValue And rungValue
                If rungValue Then rng.SetValue(var, False)
            Case "Set Coil"
                anyChange = Not (var.ReadValue) And rungValue
                If rungValue Then rng.SetValue(var, True)
            Case "Positive Transition-sensing Coil"
                Dim mustSet As Boolean = rungValue And Not (m_LastRungValue)
                anyChange = mustSet Xor var.ReadValue
                rng.SetValue(var, mustSet)
            Case "Negative Transition-sensing Coil"
                Dim mustSet As Boolean = Not (rungValue) And m_LastRungValue
                anyChange = mustSet Xor var.ReadValue
                rng.SetValue(var, mustSet)
        End Select
        m_LastRungValue = rungValue
        Return anyChange
    End Function

    ' sarebbe bello avere una Enum per descrivere i vari tipi di contatto/bobina invece di stringhe
    Public Function GetDescription() As String Implements IDocumentable.GetDescription
        Return GetSystemDescription() + IIf(Of String)(Documentation = "", "", " (" + Documentation + ")")
    End Function

    Public Function GetSystemDescription() As String Implements IDocumentable.GetSystemDescription
        Dim desc As String = GetIdentifier()
        Select Case Qualy
            ' Bobine
            Case "Normal Coil"
                desc += " will set " + ReadVar().Name + " if the rung is true (and reset it otherwise)"
            Case "Negative Coil"
                desc += " will set " + ReadVar().Name + " if the rung is false (and reset it otherwise)"
            Case "Reset Coil"
                desc += " will reset " + ReadVar().Name + " if the rung is true"
            Case "Set Coil"
                desc += " will set " + ReadVar().Name + " if the rung is true"
            Case "Positive Transition-sensing Coil"
                desc += " will set " + ReadVar().Name + " if the rung goes high"
            Case "Negative Transition-sensing Coil"
                desc += " will set " + ReadVar().Name + " if the rung goes low"
                ' Contatti
            Case "Normal Open Contact"
                desc += " will be true if " + ReadVar().Name + " is true"
            Case "Normal Closed Contact"
                desc += " will be true if " + ReadVar().Name + " is false"
            Case "Positive Transition-sensing Contact"
                desc += " will be true if " + ReadVar().Name + " goes high"
            Case "Negative Transition-sensing Contact"
                desc += " will be true if " + ReadVar().Name + " goes low"
                ' blocco?
            Case Else
                If IsBlock Then desc += " of type " + Qualy
        End Select
        Return desc
    End Function

    Public Function GetIdentifier() As String Implements IDocumentable.GetIdentifier
        If IsContact Then Return "Contact " + Name
        If IsCoil Then Return "Coil " + Name
        If IsBlock Then Return "Block " + Name
        Return Name
    End Function
End Class
