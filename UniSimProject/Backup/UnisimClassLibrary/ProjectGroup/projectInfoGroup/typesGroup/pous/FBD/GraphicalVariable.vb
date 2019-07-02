Imports System.Drawing
Imports System.Xml
Imports System
Imports System.Threading

Public Enum GraphicalVariableType
    Input
    Output
End Enum

Public Class GraphicalVariable
    Implements IHasLocalId, IXMLSerializable, IMovableObject, _
        IFBDConnectable, IDocumentable, ILDConnectable

    Private m_Body As IIEC61131LanguageImplementation
    Private WithEvents m_BoundVariable As BaseVariable
    Private m_LocalId As Integer
    Private m_Dimension As Size
    Private m_Position As Point
    Private m_VarType As GraphicalVariableType
    Private m_GraphToDraw As Drawing.Graphics
    Private m_MeSelected As New BasicSelectionManager()
    Private m_RectangleSelected As New BasicSelectionManager()
    Private m_BoundVarName As String
    Private m_Documentation As String
    Private m_ExecutionOrderID As Integer

    Private Sub BoundVariableChangedHandler() Handles m_BoundVariable.ActValueChanged, m_BoundVariable.ValueChanged
        Me.DrawVariableValue(False)
    End Sub

    Public Sub New(ByVal myFbd As IIEC61131LanguageImplementation)
        m_Body = myFbd
    End Sub

    Public Sub Dispose()
        RemoveHandler m_BoundVariable.ActValueChanged, AddressOf Me.BoundVariableChangedHandler
        RemoveHandler m_BoundVariable.ValueChanged, AddressOf Me.BoundVariableChangedHandler
    End Sub

    Public Sub New(ByVal myFBD As IIEC61131LanguageImplementation, ByVal bVar As BaseVariable, ByVal id As Integer, _
        ByVal dimen As Size, ByVal pos As Point, ByVal vType As GraphicalVariableType, Optional ByVal execOrderID As Integer = 0)
        Me.New(myFBD)
        BoundVariable = bVar
        Number = id
        Dimension = dimen
        Position = pos
        VariableType = vType
        ExecutionOrderID = execOrderID
    End Sub

    Public Property Number() As Integer Implements IHasLocalId.Number
        Get
            Return m_LocalId
        End Get
        Set(ByVal value As Integer)
            m_LocalId = value
        End Set
    End Property

    Public Property ExecutionOrderID() As Integer
        Get
            Return m_ExecutionOrderID
        End Get
        Set(ByVal value As Integer)
            m_ExecutionOrderID = value
        End Set
    End Property

    Public Property BoundVariable() As BaseVariable
        Get
            Return m_BoundVariable
        End Get
        Set(ByVal value As BaseVariable)
            m_BoundVariable = value
        End Set
    End Property

    Public Property Name() As String Implements IHasDocumentation.Name
        Get
            If BoundVariable Is Nothing Then Return Nothing
            Return BoundVariable.Name
        End Get
        Set(ByVal value As String)
            Nop()
        End Set
    End Property

    Public Property Dimension() As Size
        Get
            Return m_Dimension
        End Get
        Set(ByVal value As Size)
            m_Dimension = value
        End Set
    End Property

    Public Property Size() As Size Implements IMovableObject.Size
        Get
            Return Dimension
        End Get
        Set(ByVal value As Size)
            Dimension = value
        End Set
    End Property

    Public Property Position() As Point Implements IMovableObject.Position
        Get
            Return m_Position
        End Get
        Set(ByVal value As Point)
            m_Position = value
        End Set
    End Property
    Public Function ReadPosition() As Point Implements ILDConnectable.ReadPosition
        Return Position
    End Function
    Public Sub SetPosition(ByVal p As Point) Implements ILDConnectable.SetPosition
        Position = p
    End Sub

    Public Property VariableType() As GraphicalVariableType
        Get
            Return m_VarType
        End Get
        Set(ByVal value As GraphicalVariableType)
            m_VarType = value
        End Set
    End Property

    Public ReadOnly Property GraphToDraw() As Drawing.Graphics
        Get
            Return m_GraphToDraw
        End Get
    End Property

    ' Ritorna il rettangolo di connessione. Sebbene sia una computazione più che un accesso
    ' a variabile, scegliamo di usare una proprietà poichè il tempo del calcolo è O(1)
    Public ReadOnly Property ConnectionRectangle() As Drawing.Rectangle
        Get
            Dim middlePoint As New Point(Position.X + Size.Width / 2, Position.Y + Size.Height / 2)
            Dim smallRectSize As New Drawing.Size(Dimension.Width / 5, Dimension.Height / 5)
            ' La coordinata Y è uguale in entrambi i casi, ciò che cambia è la X
            ' e sempre di un offset rispetto a Position.X
            Dim smallRectPos As New Drawing.Point(Position.X, _
                middlePoint.Y - smallRectSize.Height / 2)
            If VariableType = GraphicalVariableType.Output Then
                smallRectPos.X -= smallRectSize.Width
            Else
                ' empiricamente viene meglio con quest'altro pixel aggiunto
                smallRectPos.X += Size.Width + 1
            End If
            Return New Drawing.Rectangle(smallRectPos, smallRectSize)
        End Get
    End Property

    ' Rettangolo entrante
    Public ReadOnly Property InConnectionRectangle() As Drawing.Rectangle Implements IFBDConnectable.InConnectionRectangle
        Get
            If VariableType = GraphicalVariableType.Output Then Return ConnectionRectangle
        End Get
    End Property

    ' Rettangolo uscente
    Public ReadOnly Property OutConnectionRectangle() As Drawing.Rectangle Implements IFBDConnectable.OutConnectionRectangle
        Get
            If VariableType = GraphicalVariableType.Input Then Return ConnectionRectangle
        End Get
    End Property

    Public ReadOnly Property ObjectRectangle() As Drawing.Rectangle
        Get
            Return New Drawing.Rectangle(Position, Size)
        End Get
    End Property

    '- <inVariable height="16" localId="1" width="80">
    '  <position x="80" y="32" /> 
    '- <connectionPointOut>
    '  <relPosition x="80" y="8" /> 
    '  </connectionPointOut>
    '  <expression>Var4</expression> 
    '  </inVariable>


    Public Sub xmlExport(ByRef writer As System.Xml.XmlTextWriter) Implements IXMLExportable.xmlExport
        writer.WriteStartElement(IIf(Of String)(VariableType = GraphicalVariableType.Input, _
            "inVariable", "outVariable"))

        writer.WriteAttributeString("height", Dimension.Height)
        writer.WriteAttributeString("width", Dimension.Width)
        writer.WriteAttributeString("localId", Number)
        writer.WriteAttributeString("executionOrderId", ExecutionOrderID)

        writer.WriteStartElement("position")

        writer.WriteAttributeString("x", Position.X)
        writer.WriteAttributeString("y", Position.Y)

        writer.WriteEndElement() ' position

        writer.WriteElementString("expression", BoundVariable.Name)

        If Documentation IsNot Nothing AndAlso Documentation <> "" Then _
            writer.WriteElementString("documentation", Documentation)

        writer.WriteEndElement() ' variable
    End Sub

    Public Sub xmlImport(ByRef reader As System.Xml.XmlTextReader) Implements IXMLImportable.xmlImport

        If reader.Name = "inVariable" Then
            VariableType = GraphicalVariableType.Input
        Else
            VariableType = GraphicalVariableType.Output
        End If

        Dim NodeDepth As Integer = reader.Depth

        Dim w, h As Integer

        If reader.MoveToAttribute("height") Then
            h = Integer.Parse(reader.Value)
        End If
        If reader.MoveToAttribute("width") Then
            w = Integer.Parse(reader.Value)
        End If

        Dimension = New Size(w, h)

        If reader.MoveToAttribute("localId") Then
            m_LocalId = Integer.Parse(reader.Value)
        End If

        If reader.MoveToAttribute("executionOrderId") Then
            m_ExecutionOrderID = Integer.Parse(reader.Value)
        End If

        Dim x, y As Integer

        ' Lo scheletro di questo codice è tratto da GraphicalStep.vb (nella cartella SFC)
        If Not reader.IsEmptyElement Then
            reader.Read()
            While reader.Depth > NodeDepth
                Select Case reader.Name
                    Case "position"
                        If reader.NodeType = XmlNodeType.Element Then
                            If reader.MoveToAttribute("x") Then
                                x = reader.Value
                            End If
                            If reader.MoveToAttribute("y") Then
                                y = reader.Value
                            End If
                            Position = New Point(x, y)
                        End If
                    Case "documentation"
                        If reader.NodeType = XmlNodeType.Element Then
                            Me.Documentation = reader.ReadString()
                        End If
                    Case "expression"
                        If reader.NodeType = XmlNodeType.Element Then
                            Me.m_BoundVarName = reader.ReadString()
                        End If
                End Select
                If reader.Depth > NodeDepth Then
                    reader.Read()
                End If
            End While
        End If

    End Sub

    Public Sub CalculateArea() Implements IGraphicalObject.CalculateArea
        Nop() ' :-)
    End Sub

    Public Sub Cancel(ByVal DrawSmallRectangles As Boolean) Implements IGraphicalObject.Cancel
        DrawImpl(DrawSmallRectangles, BackColor, BackColor, _
            BackColor)
        DrawVariableValue(True)
    End Sub

    Public Sub Draw(ByVal DrawSmallRectangles As Boolean) Implements IGraphicalObject.Draw
        DrawImpl(DrawSmallRectangles, SelectedColor, NotSelectedColor, _
            TextColor)
        DrawVariableValue(False)
    End Sub

    Private Sub DrawImpl(ByVal DrawSmallRectangles As Boolean, _
        ByVal SelColor As Color, ByVal NotSelColor As Color, _
        ByVal TextColor As Color)
        If m_GraphToDraw Is Nothing Then Exit Sub
        If Monitor.TryEnter(m_GraphToDraw, 2000) Then
            Try
                ' Codice di disegno
                Dim Penna As New Drawing.Pen( _
                    IIf(Of Drawing.Color)(Selected, SelColor, NotSelColor))
                Penna.Width = 1
                Dim R As New Drawing.Rectangle(Position.X, Position.Y, m_Dimension.Width, m_Dimension.Height)
                GraphToDraw.DrawPath(Penna, GetRoundedRect(R))
                'Dim middlePoint As New Point(Position.X + Size.Width / 2, Position.Y + Size.Height / 2)
                'Quadratini se richiesto
                If DrawSmallRectangles Then
                    Penna.Color = _
                    IIf(Of Drawing.Color)(RectangleSelected, SelColor, NotSelColor)
                    Penna.Width = 2
                    GraphToDraw.DrawRectangle(Penna, ConnectionRectangle)
                End If
                Dim strToDraw As String = Name
                Globals.DrawString(strToDraw, GraphToDraw, TextColor, Me)
            Finally
                Monitor.Exit(m_GraphToDraw)
            End Try
        End If
    End Sub
    Private Sub DrawVariableValue(ByVal Cancel As Boolean)
        If BoundVariable().dataType = "BOOL" Then
            DrawBoolVariableValue(Cancel)
        Else
            DrawNumericVariableValue(Cancel)
        End If
    End Sub
    Private Sub DrawNumericVariableValue(ByVal Cancel As Boolean)
        Static LastDrawn As String = ""
        If IsNothing(GraphToDraw) Then Exit Sub
        If Monitor.TryEnter(GraphToDraw, 2000) Then
            Try
                Dim Br As Drawing.Brush = New Drawing.SolidBrush(IIf(Of Drawing.Color)(Cancel, BackColor, _
                            IIf(Of Drawing.Color)(BoundVariable.ReadValue(), TrueColor, FalseColor)))
                Dim strToDraw As String = BoundVariable.ValueToUniversalString()
                If LastDrawn <> "" Then
                    Globals.DrawString(LastDrawn, GraphToDraw, Globals.BackColor, Me, , 1)
                    Globals.DrawString(LastDrawn, GraphToDraw, Globals.BackColor, Me, , 1)
                End If
                Globals.DrawString(strToDraw, GraphToDraw, Br, Me, , 1)
                LastDrawn = strToDraw
            Finally
                Monitor.Exit(GraphToDraw)
            End Try
        End If
    End Sub
    Private Sub DrawBoolVariableValue(ByVal Cancel As Boolean)
        If IsNothing(GraphToDraw) Then Exit Sub
        If Monitor.TryEnter(GraphToDraw, 2000) Then
            Try
                Dim Lato As Integer = CInt(m_Dimension.Width / 4)
                Dim Br As Drawing.Brush
                If Cancel Then
                    Br = New Drawing.SolidBrush(BackColor)
                Else
                    Br = New Drawing.SolidBrush( _
                        IIf(Of Drawing.Color)(BoundVariable.ReadValue(), _
                            TrueColor, FalseColor))
                End If
                GraphToDraw.FillEllipse(Br, Position.X + CInt(m_Dimension.Width / 3), Position.Y + CInt(m_Dimension.Height / 2) + CInt(m_Dimension.Height / 7), Lato, Lato)
            Finally
                Monitor.Exit(GraphToDraw)
            End Try
        End If
    End Sub


    Public Sub SetGraphToDraw(ByRef Graph As System.Drawing.Graphics) Implements IGraphicalObject.SetGraphToDraw
        m_GraphToDraw = Graph
    End Sub

    Public Sub Move(ByVal dx As Integer, ByVal dy As Integer) Implements IMovableObject.Move
        m_Position.Offset(dx, dy)
    End Sub

    Public Sub DeselectObject() Implements ISelectable.DeselectObject
        m_MeSelected.DeselectObject()
    End Sub

    Public Property Selected() As Boolean Implements ISelectable.Selected
        Get
            Return m_MeSelected.Selected
        End Get
        Set(ByVal value As Boolean)
            m_MeSelected.Selected = value
        End Set
    End Property

    Public Sub SelectObject() Implements ISelectable.SelectObject
        m_MeSelected.SelectObject()
    End Sub

    Public Sub SelectRectangle()
        m_RectangleSelected.SelectObject()
    End Sub

    Public Sub DeselectRectangle()
        m_RectangleSelected.DeselectObject()
    End Sub

    Public Property RectangleSelected() As Boolean
        Get
            Return m_RectangleSelected.Selected
        End Get
        Set(ByVal value As Boolean)
            m_RectangleSelected.Selected = value
        End Set
    End Property

    Public Function MyAreaIsHere(ByVal x As Integer, ByVal y As Integer) As Boolean
        Return ObjectRectangle.Contains(x, y)
    End Function
    Public Function MyRectangleIsHere(ByVal x As Integer, ByVal y As Integer) As Boolean
        Return ConnectionRectangle.Contains(x, y)
    End Function

    Public Sub ResolveVariableLinks()
        BoundVariable = m_Body.PouInterface.FindVariableByName(m_BoundVarName)
        If BoundVariable Is Nothing Then
            BoundVariable = m_Body.ResGlobalVariables.FindVariableByName(m_BoundVarName)
        End If
    End Sub

    Public Function GetDescription() As String Implements IDocumentable.GetDescription
        Dim ret As String = GetSystemDescription()
        If Documentation IsNot Nothing AndAlso Documentation <> "" Then _
            ret &= " (" & Documentation & ")"
        Return ret
    End Function

    Public Function GetIdentifier() As String Implements IDocumentable.GetIdentifier
        Return "Variable"
    End Function

    Public Function GetSystemDescription() As String Implements IDocumentable.GetSystemDescription
        Return IIf(Of String)(Me.VariableType = GraphicalVariableType.Input, "Input ", "Output ") & _
            GetIdentifier() & " " & BoundVariable.Name & " : " & BoundVariable.dataType
    End Function

    Public Property Documentation() As String Implements IHasDocumentation.Documentation
        Get
            Return m_Documentation
        End Get
        Set(ByVal value As String)
            m_Documentation = value
        End Set
    End Property

End Class