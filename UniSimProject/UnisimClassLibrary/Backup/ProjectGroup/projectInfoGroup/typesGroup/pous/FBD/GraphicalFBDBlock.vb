Imports System.Collections.Generic
Imports System.Xml
Imports System.Drawing
Imports System
Imports System.Threading

Public Class GraphicalFBDBlock
    Implements IXMLSerializable, IMovableObject, IHasLocalId, _
        IHasName, IFBDConnectable, IDocumentable

    Private m_Body As FBD
    Private m_BoundBlockType As String
    Private m_LocalId As Integer
    Private m_Dimension As Size
    Private m_Position As Point
    Private m_Name As String
    Private m_GraphToDraw As Drawing.Graphics
    Private m_MeSelected As New BasicSelectionManager()
    Private m_InRectangleSelected As New BasicSelectionManager()
    Private m_OutRectangleSelected As New BasicSelectionManager()
    Private m_Documentation As String

    ' Liste temporanee per memorizzare gli ingressi e le uscite letti dall'XML
    ' ResolveVariableLinks() li trasforma in connessioni
    Class ConnectionDescriptor
        Public OtherPartyID As Integer = 0
        Public Negated As Boolean = False
        Public Offset As Integer = 0
        Public Sub New()

        End Sub
    End Class
    Private m_Inputs As New List(Of ConnectionDescriptor)
    Private m_Outputs As New List(Of ConnectionDescriptor)

    Public Sub New(ByVal myFbd As FBD)
        m_Body = myFbd
    End Sub

    Public Sub New(ByVal myFBD As FBD, ByVal blockType As String, ByVal id As Integer, _
        ByVal dimen As Size, ByVal pos As Point)
        Me.New(myFBD)
        BoundBlockType = blockType
        Number = id
        Dimension = dimen
        Position = pos
    End Sub

    Public Property Number() As Integer Implements IHasLocalId.Number
        Get
            Return m_LocalId
        End Get
        Set(ByVal value As Integer)
            m_LocalId = value
        End Set
    End Property

    Public Property BoundBlockType() As String
        Get
            Return m_BoundBlockType
        End Get
        Set(ByVal value As String)
            m_BoundBlockType = value
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
    Public Function ReadPosition() As Point Implements IPureConnectable.ReadPosition
        Return Position
    End Function
    Public Sub SetPosition(ByVal p As Point) Implements IPureConnectable.SetPosition
        Position = p
    End Sub

    Public ReadOnly Property GraphToDraw() As Drawing.Graphics
        Get
            Return m_GraphToDraw
        End Get
    End Property

    ' Rettangolo entrante
    Public ReadOnly Property InConnectionRectangle() As Drawing.Rectangle Implements IFBDConnectable.InConnectionRectangle
        Get
            Dim middlePoint As New Point(Position.X + Size.Width / 2, Position.Y + Size.Height / 2)
            Dim smallRectSize As New Drawing.Size(Dimension.Width / 5, Dimension.Height / 5)
            Dim smallRectPos As New Drawing.Point(Position.X - smallRectSize.Width, _
                middlePoint.Y - smallRectSize.Height / 2)
            Return New Drawing.Rectangle(smallRectPos, smallRectSize)
        End Get
    End Property

    ' Rettangolo uscente
    Public ReadOnly Property OutConnectionRectangle() As Drawing.Rectangle Implements IFBDConnectable.OutConnectionRectangle
        Get
            Dim middlePoint As New Point(Position.X + Size.Width / 2, Position.Y + Size.Height / 2)
            Dim smallRectSize As New Drawing.Size(Dimension.Width / 5, Dimension.Height / 5)
            Dim smallRectPos As New Drawing.Point(Position.X + Size.Width + 1, _
                middlePoint.Y - smallRectSize.Height / 2)
            Return New Drawing.Rectangle(smallRectPos, smallRectSize)
        End Get
    End Property

    Public ReadOnly Property ObjectRectangle() As Drawing.Rectangle
        Get
            Return New Drawing.Rectangle(Position, Size)
        End Get
    End Property

    Public Sub xmlExport(ByRef writer As System.Xml.XmlTextWriter) Implements IXMLExportable.xmlExport
        writer.WriteStartElement("block")

        writer.WriteAttributeString("height", Dimension.Height)
        writer.WriteAttributeString("width", Dimension.Width)
        writer.WriteAttributeString("localId", Number)
        writer.WriteAttributeString("typeName", BoundBlockType)
        writer.WriteAttributeString("instanceName", Name)

        writer.WriteStartElement("position")

        writer.WriteAttributeString("x", Position.X)
        writer.WriteAttributeString("y", Position.Y)

        writer.WriteEndElement() ' position

        writer.WriteStartElement("inputVariables")
        ' leggi tutte le connessioni che hanno Me come terminale
        Dim connections As GraphicalFBDConnectionsList = _
            m_Body.GraphicalConnectionsList.FindAllConnectionsEndingWith(Me)
        Dim i As Integer = 1
        For Each connection As GraphicalFBDConnection In connections
            writer.WriteStartElement("variable")
            writer.WriteAttributeString("formalParameter", "IN" & i.ToString())
            If connection.Negated Then writer.WriteAttributeString("negated", "true")
            writer.WriteStartElement("connectionPointIn")
            writer.WriteStartElement("relPosition")
            writer.WriteAttributeString("x", Me.InConnectionRectangle.X)
            writer.WriteAttributeString("y", Me.InConnectionRectangle.Y + connection.Offset)
            writer.WriteEndElement() ' relPosition
            writer.WriteStartElement("connection")
            writer.WriteAttributeString("refLocalId", connection.SourceId.ToString())
            writer.WriteEndElement() ' connection
            writer.WriteEndElement() ' connectionPointIn
            writer.WriteEndElement() ' variable
            i += 1
        Next
        writer.WriteEndElement() ' inputVariables

        ' non supportiamo le variabili di ingresso/uscita per ora, ma ci mettiamo
        ' il tag (in lettura lo saltiamo)
        writer.WriteStartElement("inOutVariables")
        writer.WriteEndElement() ' inOutVariables

        ' lo standard prevede di memorizzare dentro di noi solo le uscite verso
        ' variabili, quelle verso blocchi saranno gestite come ingressi dal blocco
        i = 1
        writer.WriteStartElement("outputVariables")
        connections = m_Body.GraphicalConnectionsList.FindAllConnectionsStartingWith(Me)
        For Each connection As GraphicalFBDConnection In connections
            writer.WriteStartElement("variable")
            writer.WriteAttributeString("formalParameter", "OUT" & i.ToString())
            If connection.Negated Then writer.WriteAttributeString("negated", "true")
            writer.WriteStartElement("connectionPointOut")
            writer.WriteStartElement("relPosition")
            writer.WriteAttributeString("x", Me.OutConnectionRectangle.X)
            writer.WriteAttributeString("y", Me.OutConnectionRectangle.Y + connection.Offset)
            writer.WriteEndElement() ' relPosition
            ' programmazione strutturata 101% :-)
            If TypeOf (connection.DestinationObject) Is GraphicalFBDBlock Then GoTo CloseBlock
            ' Dobbiamo scrivere in una variable
            Dim tgtVar As GraphicalVariable = CType(connection.DestinationObject, GraphicalVariable)
            writer.WriteStartElement("connection")
            writer.WriteAttributeString("refLocalId", connection.DestinationId.ToString())
            writer.WriteEndElement() ' connection
CloseBlock:
            writer.WriteEndElement() ' connectionPointOut
            writer.WriteEndElement() ' variable
            i += 1
        Next
        writer.WriteEndElement() ' outputVariables

        If Documentation IsNot Nothing AndAlso Documentation <> "" Then _
            writer.WriteElementString("documentation", Documentation)

        writer.WriteEndElement() ' block
    End Sub

    Private Sub ParseInputVariable(ByRef reader As System.Xml.XmlTextReader, ByVal neg As Boolean)
        Dim NodeDepth As Integer = reader.Depth

        If reader.MoveToAttribute("formalParameter") Then
            Nop()
        End If

        If Not reader.IsEmptyElement Then
            reader.Read()
            Dim cond As New ConnectionDescriptor()
            cond.Negated = neg
            While reader.Depth > NodeDepth
                Select Case reader.Name
                    Case "connectionPointIn"
                    Case "relPosition"
                        If reader.MoveToAttribute("y") Then
                            cond.Offset = Int32.Parse(reader.Value) - Me.InConnectionRectangle.Y
                        End If
                    Case "connection"
                        Dim lid As Integer = 0
                        If reader.MoveToAttribute("refLocalId") Then
                            cond.OtherPartyID = Int32.Parse(reader.Value)
                        End If
                        'MsgBox("I" & lid)
                        m_Inputs.Add(cond)
                End Select
                If reader.Depth > NodeDepth Then
                    reader.Read()
                End If
            End While
        End If
    End Sub

    Private Sub ParseOutputVariable(ByRef reader As System.Xml.XmlTextReader, ByVal neg As Boolean)
        Dim NodeDepth As Integer = reader.Depth

        If reader.MoveToAttribute("formalParameter") Then
            Nop()
        End If

        If Not reader.IsEmptyElement Then
            reader.Read()
            Dim cond As New ConnectionDescriptor()
            cond.Negated = neg
            While reader.Depth > NodeDepth
                Select Case reader.Name
                    Case "connectionPointOut"
                    Case "relPosition"
                        If reader.MoveToAttribute("y") Then
                            cond.Offset = Int32.Parse(reader.Value) - Me.OutConnectionRectangle.Y
                        End If
                    Case "connection"
                        Dim lid As Integer = 0
                        If reader.MoveToAttribute("refLocalId") Then
                            cond.OtherPartyID = Int32.Parse(reader.Value)
                        End If
                        'MsgBox("O" & lid)
                        m_Outputs.Add(cond)
                End Select
                If reader.Depth > NodeDepth Then
                    reader.Read()
                End If
            End While
        End If
    End Sub

    Private Sub ParseInputVariables(ByRef reader As System.Xml.XmlTextReader)
        Dim NodeDepth As Integer = reader.Depth
        If Not reader.IsEmptyElement Then
            reader.Read()
            While reader.Depth > NodeDepth
                Select Case reader.Name
                    Case "variable"
                        Dim neg As Boolean = reader.MoveToAttribute("negated")
                        If neg Then
                            neg = reader.Value.Equals("true")
                            reader.MoveToElement()
                        End If
                        ParseInputVariable(reader, neg)
                End Select
                If reader.Depth > NodeDepth Then
                    reader.Read()
                End If
            End While
        End If
    End Sub

    ' Il codice sia qui sia in importBlock() in GraphicalContact e' *SBAGLIATO* e funziona solo
    ' perchè questo elemento è vuoto... correggere non appena questa assunzione non
    ' sarà più verificata
    Private Sub ParseInOutVariables(ByRef reader As System.Xml.XmlTextReader)
        While (reader.NodeType = XmlNodeType.EndElement AndAlso reader.Name = "inOutVariables")
            reader.Read()
        End While
    End Sub

    Private Sub ParseOutputVariables(ByRef reader As System.Xml.XmlTextReader)
        Dim NodeDepth As Integer = reader.Depth
        If Not reader.IsEmptyElement Then
            reader.Read()
            While reader.Depth > NodeDepth
                Select Case reader.Name
                    Case "variable"
                        Dim neg As Boolean = reader.MoveToAttribute("negated")
                        If neg Then
                            neg = reader.Value.Equals("true")
                            reader.MoveToElement()
                        End If
                        ParseOutputVariable(reader, neg)
                End Select
                If reader.Depth > NodeDepth Then
                    reader.Read()
                End If
            End While
        End If
    End Sub

    Public Sub xmlImport(ByRef reader As System.Xml.XmlTextReader) Implements IXMLImportable.xmlImport

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

        If reader.MoveToAttribute("typeName") Then
            BoundBlockType = reader.Value
        End If

        If reader.MoveToAttribute("instanceName") Then
            Name = reader.Value
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
                    Case "inputVariables"
                        ParseInputVariables(reader)
                    Case "inOutVariables"
                        ParseInOutVariables(reader)
                    Case "outputVariables"
                        ParseOutputVariables(reader)
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
    End Sub

    Public Sub Draw(ByVal DrawSmallRectangles As Boolean) Implements IGraphicalObject.Draw
        DrawImpl(DrawSmallRectangles, SelectedColor, NotSelectedColor, _
            TextColor)
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
                GraphToDraw.DrawPath(Penna, GetRoundedRect(ObjectRectangle))
                Dim middlePoint As New Point(Position.X + Size.Width / 2, Position.Y + Size.Height / 2)
                'Quadratini se richiesto
                If DrawSmallRectangles Then
                    Penna.Width = 2

                    Penna.Color = _
                        IIf(Of Drawing.Color)(InRectangleSelected, SelColor, NotSelColor)
                    GraphToDraw.DrawRectangle(Penna, InConnectionRectangle)

                    Penna.Color = _
                        IIf(Of Drawing.Color)(OutRectangleSelected, SelColor, NotSelColor)
                    GraphToDraw.DrawRectangle(Penna, OutConnectionRectangle)

                End If
                Dim strToDraw As String = BoundBlockType
                Globals.DrawString(strToDraw, GraphToDraw, TextColor, Me)
            Finally
                Monitor.Exit(m_GraphToDraw)
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

    Public Sub SelectInRectangle()
        m_InRectangleSelected.SelectObject()
    End Sub

    Public Sub DeselectInRectangle()
        m_InRectangleSelected.DeselectObject()
    End Sub

    Public Property InRectangleSelected() As Boolean
        Get
            Return m_InRectangleSelected.Selected
        End Get
        Set(ByVal value As Boolean)
            m_InRectangleSelected.Selected = value
        End Set
    End Property

    Public Sub SelectOutRectangle()
        m_OutRectangleSelected.SelectObject()
    End Sub

    Public Sub DeselectOutRectangle()
        m_OutRectangleSelected.DeselectObject()
    End Sub

    Public Property OutRectangleSelected() As Boolean
        Get
            Return m_OutRectangleSelected.Selected
        End Get
        Set(ByVal value As Boolean)
            m_OutRectangleSelected.Selected = value
        End Set
    End Property

    Public Property Name() As String Implements IHasName.Name
        Get
            Return m_Name
        End Get
        Set(ByVal value As String)
            m_Name = value
        End Set
    End Property

    Public Function MyAreaIsHere(ByVal x As Integer, ByVal y As Integer) As Boolean
        Return ObjectRectangle.Contains(x, y)
    End Function

    Public Function MyInRectangleIsHere(ByVal x As Integer, ByVal y As Integer) As Boolean
        Return InConnectionRectangle.Contains(x, y)
    End Function

    Public Function MyOutRectangleIsHere(ByVal x As Integer, ByVal y As Integer) As Boolean
        Return OutConnectionRectangle.Contains(x, y)
    End Function

    Public Sub ResolveVariableLinks()
        For Each i As ConnectionDescriptor In m_Inputs
            Me.m_Body.AddAndDrawGraphicalConnection(i.OtherPartyID, Number, i.Negated, i.Offset)
        Next
        For Each j As ConnectionDescriptor In m_Outputs
            Me.m_Body.AddAndDrawGraphicalConnection(Number, j.OtherPartyID, j.Negated, j.Offset)
        Next
    End Sub

    Public Function GetDescription() As String Implements IDocumentable.GetDescription
        Dim ret As String = GetSystemDescription()
        If Documentation IsNot Nothing AndAlso Documentation <> "" Then _
            ret &= " (" & Documentation & ")"
        Return ret
    End Function

    Public Function GetIdentifier() As String Implements IDocumentable.GetIdentifier
        Return "Block"
    End Function

    Public Function GetSystemDescription() As String Implements IDocumentable.GetSystemDescription
        Return Name & " : " & BoundBlockType & " block"
    End Function

    Public Property Documentation() As String Implements IHasDocumentation.Documentation
        Get
            Return m_Documentation
        End Get
        Set(ByVal value As String)
            m_Documentation = value
        End Set
    End Property

    ' Ritorna gli indici in GraphicalConnectionsList delle connessioni entranti qui
    ' (nell'ordine in cui verranno poi valutate quando si esegue il diagramma)
    Public Function GetMyArgumentsIndexes() As List(Of Integer)
        Dim ret As New List(Of Integer)
        For Each connection As GraphicalFBDConnection In m_Body.GraphicalConnectionsList.FindAllConnectionsEndingWith(Me)
            ret.Add(m_Body.GraphicalConnectionsList.IndexOf(connection))
        Next
        Return ret
    End Function
End Class