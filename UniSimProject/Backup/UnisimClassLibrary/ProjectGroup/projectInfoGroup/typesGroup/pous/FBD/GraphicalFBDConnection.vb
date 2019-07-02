Imports System.Drawing
Imports System
Imports System.Threading
Imports System.Collections.Generic

Public Class GraphicalFBDConnection
    Implements IMovableObject, IDocumentable

    Private m_SourceId As Integer
    Private m_DestinationId As Integer
    Private m_Body As IIEC61131LanguageImplementation
    Private m_Negated As Boolean
    Private m_GraphToDraw As Drawing.Graphics
    Private m_Position As New Point(-1, -1)
    Private ReadOnly CoeffSnap As Integer = 4
    Private m_Dimension As Integer = 44
    Private m_SelectionManager As New BasicSelectionManager
    Private m_Documentation As String
    Private m_Offset As Integer = 0

    Public Sub New(ByVal sId As Integer, ByVal dId As Integer, ByVal bdy As IIEC61131LanguageImplementation, _
        Optional ByVal inv As Boolean = False)
        m_SourceId = sId
        m_DestinationId = dId
        m_Body = bdy
        m_Negated = inv
    End Sub

    Public ReadOnly Property SourceId() As Integer
        Get
            Return m_SourceId
        End Get
    End Property

    Public ReadOnly Property DestinationId() As Integer
        Get
            Return m_DestinationId
        End Get
    End Property

    Public ReadOnly Property SourceObject() As IPureConnectable
        Get
            Return CType(m_Body.FindElementByLocalId(SourceId), IPureConnectable)
        End Get
    End Property

    Public ReadOnly Property DestinationObject() As IPureConnectable
        Get
            Return CType(m_Body.FindElementByLocalId(DestinationId), IPureConnectable)
        End Get
    End Property

    Public Property Negated() As Boolean
        Get
            Return m_Negated
        End Get
        Set(ByVal value As Boolean)
            m_Negated = value
        End Set
    End Property

    Public Sub SetGraphToDraw(ByRef Graph As Drawing.Graphics) Implements IGraphicalObject.SetGraphToDraw
        m_GraphToDraw = Graph
    End Sub

    Public ReadOnly Property GraphToDraw() As Drawing.Graphics
        Get
            Return m_GraphToDraw
        End Get
    End Property

    Public Sub Cancel(ByVal DrawSmallRectangles As Boolean) Implements IGraphicalObject.Cancel
        DrawImpl(DrawSmallRectangles, BackColor, BackColor, _
            BackColor)
        DrawTruthood(BackColor, BackColor)
    End Sub

    Public Sub Draw(ByVal DrawSmallRectangles As Boolean) Implements IGraphicalObject.Draw
        DrawImpl(DrawSmallRectangles, SelectedColor, NotSelectedColor, _
            TextColor)
        DrawTruthood(SelectedColor, NotSelectedColor)
    End Sub

    Private Sub GetCardinalPoints(ByRef S As Point, ByRef Med1 As Point, ByRef Med2 As Point, ByRef D As Point)
        S = SourceObject.OutConnectionRectangle.Location
        S.Y += CInt(SourceObject.OutConnectionRectangle.Height / 2)
        S.Y += m_Offset
        D = DestinationObject.InConnectionRectangle.Location
        D.X += DestinationObject.InConnectionRectangle.Width
        D.Y += CInt(DestinationObject.InConnectionRectangle.Height / 2)
        Med1 = New Drawing.Point(CInt((S.X + D.X) / 2), S.Y)
        Med2 = New Drawing.Point(Med1.X, D.Y)
    End Sub

    Private Function GetDrawnRectangles() As List(Of Rectangle)
        Dim ret As New List(Of Rectangle)
        Dim S As Point, Med1 As Point, Med2 As Point, D As Point
        Call GetCardinalPoints(S, Med1, Med2, D)
        Dim rect1, rect2, rect3 As Drawing.Rectangle
        ' Alcuni ragionamenti di ordine geometrico e algebrico dicono che, se la sorgente è più a sinistra
        ' della destinazione, i rettangoli rect1 e rect3 sono giusti così come sono (per giusti intendiamo
        ' con Height > 0 *E* Width > 0). rect2 potrebbe invece avere l'Height < 0. Per evitarlo, dobbiamo
        ' scoprire quale dei due punti è più "in alto" dell'altro, e aggiustare di conseguenza la formula
        rect1 = New Drawing.Rectangle(S.X, S.Y, Med1.X - S.X, 3)
        If Med2.Y > Med1.Y Then
            rect2 = New Drawing.Rectangle(Med1.X, Med1.Y, 3, Med2.Y - Med1.Y)
        Else
            rect2 = New Drawing.Rectangle(Med2.X, Med2.Y, 3, Med1.Y - Med2.Y)
        End If
        rect3 = New Drawing.Rectangle(Med2.X, Med2.Y, D.X - Med2.X, 3)
        ret.Add(rect1)
        ret.Add(rect2)
        ret.Add(rect3)
        Return ret
    End Function

    ' Questo codice è stato "progettato" a lavagna, definendo i punti necessari
    ' per disegnare la connessione. Non va bene il codice delle transizioni poichè
    ' là i collegamenti sono alto/basso e non sinistra/destra
    Private Sub DrawImpl(ByVal DrawSmallRectangles As Boolean, _
        ByVal SelColor As Color, ByVal NotSelColor As Color, _
        ByVal TextColor As Color)
        If m_GraphToDraw Is Nothing Then Exit Sub
        If Monitor.TryEnter(m_GraphToDraw, 2000) Then
            Try
                Dim pen As New Drawing.Pen(IIf(Of Drawing.Color)(Selected, _
                    SelColor, NotSelColor))
                Dim S As Point, Med1 As Point, Med2 As Point, D As Point
                Call GetCardinalPoints(S, Med1, Med2, D)
                GraphToDraw.DrawLine(pen, S, Med1)
                GraphToDraw.DrawLine(pen, Med1, Med2)
                GraphToDraw.DrawLine(pen, Med2, D)
            Finally
                Monitor.Exit(m_GraphToDraw)
            End Try
        End If
    End Sub

    ' Se la connessione è negata disegniamo un pallino alla sorgente, altrimenti
    ' nulla (si sarebbe dovuto fare a destinazione, è più pratico farlo qui visto
    ' che abbiamo un solo rettangolo target per blocco)
    Private Sub DrawTruthood(ByVal SelColor As Color, ByVal NotSelColor As Color)
        If IsNothing(m_GraphToDraw) Then Exit Sub
        If Not Negated Then Exit Sub
        If Monitor.TryEnter(GraphToDraw, 2000) Then
            Try
                Dim srcRect As Drawing.Rectangle = SourceObject.OutConnectionRectangle
                srcRect.Y += Offset
                GraphToDraw.DrawEllipse(New Drawing.Pen(IIf(Of Color)(Selected, SelColor, NotSelColor)), srcRect)
            Finally
                Monitor.Exit(GraphToDraw)
            End Try
        End If
    End Sub

    Public Sub SelectObject() Implements IGraphicalObject.SelectObject
        Selected = True
    End Sub
    Public Sub DeselectObject() Implements IGraphicalObject.DeselectObject
        Selected = False
    End Sub
    Public Property Selected() As Boolean Implements IGraphicalObject.Selected
        Get
            Return m_SelectionManager.Selected
        End Get
        Set(ByVal value As Boolean)
            m_SelectionManager.Selected = value
        End Set
    End Property

    Public Sub CalculateArea() Implements IGraphicalObject.CalculateArea
        Nop()
    End Sub

    Public Sub Move(ByVal dx As Integer, ByVal dy As Integer) Implements IMovableObject.Move
        ' Non spostiamo la connessione se c'è un blocco o una variabile selezionata oppure
        ' se ci sono più connessioni selezionate (evita l'effetto grafico in cui spostiamo un
        ' pezzo di diagramma e tutte le connessioni si ritrovano slittate in alto o in basso,
        ' mentre così facendo c'è un effetto "inerzia" in cui la connessione segue passivamente
        ' i suoi terminali)
        If TypeOf (m_Body) Is FBD Then
            Dim fbd As FBD = CType(m_Body, FBD)
            If fbd.GraphicalBlocksList.CountSelected <> 0 OrElse _
                fbd.GraphicalVariablesList.CountSelected <> 0 OrElse _
                    fbd.GraphicalConnectionsList.CountSelected <> 1 Then Exit Sub
        ElseIf TypeOf (m_Body) Is Ladder Then
            Dim ld As Ladder = CType(m_Body, Ladder)
            If ld.CountSelectedElement() - ld.GraphicalFBDConnectionsList.CountSelected > 0 Then Exit Sub
        End If
        ' Ignora dx, sposta solamente verso l'alto e verso il basso...
        m_Offset += dy
        ' di non oltre la metà della dimensione dell'elemento sorgente (così evitiamo di
        ' uscire fuori)
        m_Offset = Math.Sign(m_Offset) * Math.Min(Math.Abs(m_Offset), SourceObject.Size.Height / 2)
    End Sub


    Public Property Position() As System.Drawing.Point Implements IGraphicalObject.Position
        Get
            Return m_Position
        End Get
        Set(ByVal value As System.Drawing.Point)
            m_Position = value
        End Set
    End Property

    Public Property Offset() As Integer
        Get
            Return m_Offset
        End Get
        Friend Set(ByVal value As Integer)
            m_Offset = value
        End Set
    End Property

    Public Property Size() As System.Drawing.Size Implements IGraphicalObject.Size
        Get
            Return New Drawing.Size(44, 44)
        End Get
        Set(ByVal value As System.Drawing.Size)
            m_Dimension = Math.Max(value.Width, value.Height)
        End Set
    End Property

    ' Come in GraphicalConnection (LD)
    Public Function MyAreaIsHere(ByVal x As Integer, ByVal y As Integer) As Boolean
        For Each R As Rectangle In GetDrawnRectangles()
            If R.Contains(x, y) Then Return True
        Next
        Return False
    End Function

    Public Function IntersectsWith(ByVal Rect As Rectangle) As Boolean
        For Each R As Rectangle In GetDrawnRectangles()
            If R.IntersectsWith(Rect) Then Return True
        Next
        Return False
    End Function

    Public Sub ResolveVariableLinks()

    End Sub

    Public Function GetDescription() As String Implements IDocumentable.GetDescription
        Return GetSystemDescription()
    End Function

    Public Function GetIdentifier() As String Implements IDocumentable.GetIdentifier
        Return "Connection"
    End Function

    Public Function GetSystemDescription() As String Implements IDocumentable.GetSystemDescription
        Dim ret As String = GetIdentifier() & " connects " & _
            IIf(Of String)(Negated, "the inverse of ", "") & _
                CType(SourceObject, IDocumentable).GetSystemDescription() & " to " _
                    & CType(DestinationObject, IDocumentable).GetSystemDescription()
        Return ret
    End Function

    Public Property Documentation() As String Implements IHasDocumentation.Documentation
        Get
            Return ""
        End Get
        Set(ByVal value As String)
            Nop()
        End Set
    End Property

    Public Property Name() As String Implements IHasName.Name
        Get
            Return ""
        End Get
        Set(ByVal value As String)
            Nop()
        End Set
    End Property
End Class