Imports System.Collections.Generic
Imports System.Xml
Imports System.Drawing
Imports System
Imports System.Threading

Public Class GraphicalLDBlock
    Implements IMovableObject, IHasName, IHasLocalId

    Private m_Body As Ladder
    Private m_BoundBlockType As String
    Private m_LocalId As Integer
    Private m_Dimension As Size
    Private m_Position As Point
    Private m_Name As String
    Private m_GraphToDraw As Drawing.Graphics
    Private m_MeSelected As New BasicSelectionManager()
    ' Prevediamo due coppie di rettangolini di selezione: LD ed FBD
    ' La coppia InLD/OutLD corrisponde ai rettangolini di selezione usati da bobine e contatti e serve ad
    ' inserire il blocco in un rung
    ' La coppia InFBD/OutFBD corrisponde ai rettangolini di selezione usati dai blocchi FBD e serve a collegare
    ' il blocco ai suoi ingressi ed uscite
    Private m_InLDRectangleSelected As New BasicSelectionManager()
    Private m_OutLDRectangleSelected As New BasicSelectionManager()
    Private m_InFBDRectangleSelected As New BasicSelectionManager()
    Private m_OutFBDRectangleSelected As New BasicSelectionManager()
    Private m_Documentation As String

    Public Sub New(ByVal myLD As Ladder)
        m_Body = myLD
    End Sub

    Public Sub New(ByVal myLD As Ladder, ByVal blockType As String, ByVal id As Integer, _
        ByVal dimen As Size, ByVal pos As Point)
        Me.New(myLD)
        BoundBlockType = blockType
        Number = id
        Dimension = dimen
        Position = pos
    End Sub

    Public Overloads Property Number() As Integer Implements IHasLocalId.Number
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

    Public ReadOnly Property GraphToDraw() As Drawing.Graphics
        Get
            Return m_GraphToDraw
        End Get
    End Property

    Public ReadOnly Property ObjectRectangle() As Drawing.Rectangle
        Get
            Return New Drawing.Rectangle(Position, Size)
        End Get
    End Property

    Private ReadOnly Property SmallRectSize() As Drawing.Size
        Get
            Return New Drawing.Size(Dimension.Width / 5, Dimension.Height / 5)
        End Get
    End Property

    ' Rettangolino di connessione entrante LD
    Public ReadOnly Property InLDConnectionRectangle() As Drawing.Rectangle
        Get
            Return New Drawing.Rectangle(New Drawing.Point(Position.X - SmallRectSize.Width, Position.Y), _
                SmallRectSize)
        End Get
    End Property

    ' Rettangolino di connessione uscente LD
    Public ReadOnly Property OutLDConnectionRectangle() As Drawing.Rectangle
        Get
            Return New Drawing.Rectangle(New Drawing.Point(Position.X + Size.Width, Position.Y), _
                SmallRectSize)
        End Get
    End Property

    ' Rettangolino di connessione entrante FBD
    Public ReadOnly Property InFBDConnectionRectangle() As Drawing.Rectangle
        Get
            Return New Drawing.Rectangle(New Drawing.Point(Position.X - SmallRectSize.Width, Position.Y + _
                Size.Height - SmallRectSize.Height), _
                SmallRectSize)
        End Get
    End Property

    ' Rettangolino di connessione uscente FBD
    Public ReadOnly Property OutFBDConnectionRectangle() As Drawing.Rectangle
        Get
            Return New Drawing.Rectangle(New Drawing.Point(Position.X + Size.Width, Position.Y + _
                Size.Height - SmallRectSize.Height), _
                SmallRectSize)
        End Get
    End Property

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
                        IIf(Of Drawing.Color)(InLDRectangleSelected, SelColor, NotSelColor)
                    GraphToDraw.DrawRectangle(Penna, InLDConnectionRectangle)

                    Penna.Color = _
                        IIf(Of Drawing.Color)(OutLDRectangleSelected, SelColor, NotSelColor)
                    GraphToDraw.DrawRectangle(Penna, OutLDConnectionRectangle)

                    Penna.Color = _
                        IIf(Of Drawing.Color)(InFBDRectangleSelected, SelColor, NotSelColor)
                    GraphToDraw.DrawRectangle(Penna, InFBDConnectionRectangle)

                    Penna.Color = _
                        IIf(Of Drawing.Color)(OutFBDRectangleSelected, SelColor, NotSelColor)
                    GraphToDraw.DrawRectangle(Penna, OutFBDConnectionRectangle)

                End If
                Dim strToDraw As String = BoundBlockType
                Dim textSize As SizeF = GraphToDraw.MeasureString(strToDraw, TextFont)
                Dim textPoint As New Point(middlePoint.X - textSize.Width / 2, _
                    middlePoint.Y - textSize.Height / 2)
                ' Soluzione elegante: intersechiamo il rettangolo che contiene il testo con il
                ' rettangolo che delimita la "nostra" regione di disegno, e scriviamo in questo
                ' risultato :-)
                Dim textRectangle As New RectangleF(textPoint, textSize)
                textRectangle.Intersect(New Drawing.RectangleF(Me.Position.X, Me.Position.Y, _
                    Me.Size.Width, Me.Size.Height))
                GraphToDraw.DrawString(strToDraw, TextFont, New SolidBrush(TextColor), textRectangle)
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

    Public Sub DeselectObject() Implements ISelectableObject.DeselectObject
        m_MeSelected.DeselectObject()
    End Sub

    Public Property Selected() As Boolean Implements ISelectableObject.Selected
        Get
            Return m_MeSelected.Selected
        End Get
        Set(ByVal value As Boolean)
            m_MeSelected.Selected = value
        End Set
    End Property

    Public Sub SelectInLDRectangle()
        m_InLDRectangleSelected.SelectObject()
    End Sub

    Public Sub DeselectInLDRectangle()
        m_InLDRectangleSelected.DeselectObject()
    End Sub

    Public Property InLDRectangleSelected() As Boolean
        Get
            Return m_InLDRectangleSelected.Selected
        End Get
        Set(ByVal value As Boolean)
            m_InLDRectangleSelected.Selected = value
        End Set
    End Property

    Public Sub SelectOutLDRectangle()
        m_OutLDRectangleSelected.SelectObject()
    End Sub

    Public Sub DeselectOutLDRectangle()
        m_OutLDRectangleSelected.DeselectObject()
    End Sub

    Public Property OutLDRectangleSelected() As Boolean
        Get
            Return m_OutLDRectangleSelected.Selected
        End Get
        Set(ByVal value As Boolean)
            m_OutLDRectangleSelected.Selected = value
        End Set
    End Property

    Public Sub SelectInFBDRectangle()
        m_InFBDRectangleSelected.SelectObject()
    End Sub

    Public Sub DeselectInFBDRectangle()
        m_InFBDRectangleSelected.DeselectObject()
    End Sub

    Public Property InFBDRectangleSelected() As Boolean
        Get
            Return m_InFBDRectangleSelected.Selected
        End Get
        Set(ByVal value As Boolean)
            m_InFBDRectangleSelected.Selected = value
        End Set
    End Property

    Public Sub SelectOutFBDRectangle()
        m_OutFBDRectangleSelected.SelectObject()
    End Sub

    Public Sub DeselectOutFBDRectangle()
        m_OutFBDRectangleSelected.DeselectObject()
    End Sub

    Public Property OutFBDRectangleSelected() As Boolean
        Get
            Return m_OutFBDRectangleSelected.Selected
        End Get
        Set(ByVal value As Boolean)
            m_OutFBDRectangleSelected.Selected = value
        End Set
    End Property

    Public Sub SelectObject() Implements ISelectableObject.SelectObject
        m_MeSelected.SelectObject()
    End Sub

    Public Overloads Property Name() As String Implements IHasName.Name
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

    Public Function InLDRectangleIsHere(ByVal x As Integer, ByVal y As Integer) As Boolean
        Return InLDConnectionRectangle.Contains(x, y)
    End Function
    Public Function OutLDRectangleIsHere(ByVal x As Integer, ByVal y As Integer) As Boolean
        Return OutLDConnectionRectangle.Contains(x, y)
    End Function

    Public Function InFBDRectangleIsHere(ByVal x As Integer, ByVal y As Integer) As Boolean
        Return InFBDConnectionRectangle.Contains(x, y)
    End Function
    Public Function OutFBDRectangleIsHere(ByVal x As Integer, ByVal y As Integer) As Boolean
        Return OutFBDConnectionRectangle.Contains(x, y)
    End Function

    Public Sub ResolveVariableLinks()

    End Sub

End Class