Imports System.IO
Imports System.Math
Imports System.Threading
Imports System.Drawing


Public Class GraphicalCoil

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

    Protected m_ActionBlock As ActionBlock
    Protected m_PulseActionExecuted As Boolean
    Protected m_NotPulseActionExecuted As Boolean
    Dim AcBolockPosition As New Drawing.Point

    Public Sub New(ByVal ContactNumber As Integer, ByRef ContactName As String, ByVal ContactDocumentation As String, ByRef RefLadder As Ladder, ByVal Ini As Boolean, ByVal Fin As Boolean, ByVal P As Drawing.Point, ByVal BackCol As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, ByRef Graph As Drawing.Graphics, ByVal DrState As Boolean, ByVal Dimen As Integer)
        m_Number = ContactNumber
        m_Name = ContactName
        m_documentation = ContactDocumentation
        m_Ladder = RefLadder
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
        m_ActionBlock = New ActionBlock(m_Dimension, RefLadder, AcBolockPosition, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, GraphToDraw)
        CalculusArea()
    End Sub

    Public Sub New(ByVal RefLadder As Ladder, ByVal BackCol As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, ByVal Graph As Drawing.Graphics, ByVal DrState As Boolean, ByVal Dimen As Integer)
        m_Ladder = RefLadder
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
        m_ActionBlock = New ActionBlock(m_Dimension, RefLadder, AcBolockPosition, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, GraphToDraw)
        CalculusArea()
    End Sub

    Public Sub DrawCoil(ByVal DrawSmallRectangels As Boolean)
        'viene chiamata solo alla fina il problema stà prima
        Draw(DrawSmallRectangels, SelectionColor, NotSelectionColor, TextColor, BackColor)
    End Sub
    Public Function ReadDimension() As Integer
        ReadDimension = m_Dimension
    End Function


    Public Sub Draw(ByVal DrawSmallRectangels As Boolean, ByVal Col1 As Drawing.Color, ByVal Col2 As Drawing.Color, ByVal Col3 As Drawing.Color, ByVal Col4 As Drawing.Color)

        Dim R As Integer = 38
        Try

            If Monitor.TryEnter(GraphToDraw, 2000) Then

                Dim Penna As New Drawing.Pen(Col1)
                Dim larg As Integer
                Dim alt As Integer
                Dim Br As Drawing.Brush
                If Selected Then
                    'Penna.Color = SelectionColor
                Else
                    Penna.Color = Col2
                End If

                GraphToDraw.DrawLine(Penna, Position.X - 20, Position.Y, Position.X - 7, Position.Y)
                GraphToDraw.DrawLine(Penna, Position.X + 7, Position.Y, Position.X + 20, Position.Y)
                GraphToDraw.DrawArc(Penna, (Position.X + 31) - R, Position.Y - R, R * 2, R * 2, 158, 44)
                GraphToDraw.DrawArc(Penna, (Position.X - 31) - R, Position.Y - R, R * 2, R * 2, 338, 44)

                'disrgna i reofori
                If DrawSmallRectangels Then
                    If TopRectSelected Then
                        Penna.Color = Col1
                    Else
                        Penna.Color = Col2
                    End If
                    larg = CInt(m_Dimension / 5)
                    alt = larg
                    Penna.Width = 2
                    GraphToDraw.DrawRectangle(Penna, Position.X - CInt(m_Dimension / 10), Position.Y - CInt(m_Dimension / 2) - CInt(m_Dimension / 5), larg, alt)
                    If BottomRectSelected Then
                        Penna.Color = Col1
                    Else
                        Penna.Color = Col2
                    End If
                    larg = CInt(m_Dimension / 5)
                    alt = larg
                    Penna.Width = 2
                    GraphToDraw.DrawRectangle(Penna, Position.X - CInt(m_Dimension / 10), Position.Y + CInt(m_Dimension / 2) + 1, larg, alt)
                End If

                '        'Rettangolo vuoto al centro
                '       'GraphToDraw.FillRectangle(Br, R.X + 1, R.Y + 1, R.Width - 1, R.Height - 1)
                '
                '           'Rectangle interno se iniziale
                '          If m_Initial Then
                '             Penna.Width = 1
                '            GraphToDraw.DrawRectangle(Penna, Position.X - CInt((m_Dimension - 8) / 2), Position.Y - CInt((m_Dimension - 8) / 2), m_Dimension - 8, m_Dimension - 8)
                '       End If
                '
                '           'Freccia se finale
                '          If m_Final Then
                '             Penna.Width = 1
                '            GraphToDraw.DrawLine(Penna, Position.X - CInt(m_Dimension / 4), Position.Y - CInt(m_Dimension / 4), Position.X + CInt(m_Dimension / 4), Position.Y - CInt(m_Dimension / 4))
                '           GraphToDraw.DrawLine(Penna, Position.X + CInt(m_Dimension / 6), Position.Y - CInt(m_Dimension / 4) - CInt(m_Dimension / 8), Position.X + CInt(m_Dimension / 4), Position.Y - CInt(m_Dimension / 4))
                '          GraphToDraw.DrawLine(Penna, Position.X + CInt(m_Dimension / 6), Position.Y - CInt(m_Dimension / 4) + CInt(m_Dimension / 8), Position.X + CInt(m_Dimension / 4), Position.Y - CInt(m_Dimension / 4))
                '     End If
                '
                '           'Quadratini se richiesto
                '          If DrawSmallRectangels Then
                '             If TopRectSelected Then
                '            Penna.Color = Col1
                '       Else
                '          Penna.Color = Col2
                '     End If
                '    larg = CInt(m_Dimension / 5)
                '   alt = larg
                '  Penna.Width = 2
                ' GraphToDraw.DrawRectangle(Penna, Position.X - CInt(m_Dimension / 10), Position.Y - CInt(m_Dimension / 2) - CInt(m_Dimension / 5), larg, alt)
                '           If BottomRectSelected Then
                '              Penna.Color = Col1
                '         Else
                '            Penna.Color = Col2
                '       End If
                '      larg = CInt(m_Dimension / 5)
                '     alt = larg
                '    Penna.Width = 2
                '   GraphToDraw.DrawRectangle(Penna, Position.X - CInt(m_Dimension / 10), Position.Y + CInt(m_Dimension / 2) + 1, larg, alt)
                '  End If
                '
                '           'Testo nella fase
                '          Dim Rect As Drawing.SizeF
                '         Br = New Drawing.SolidBrush(Col2)
                '        If Not IsNothing(m_Name) Then
                '           Rect = GraphToDraw.MeasureString(m_Name, Carattere)
                '
                '               'Controlla se il nome è più largo
                '              If Rect.Width < m_dimension Then
                '             GraphToDraw.DrawString(m_Name, Carattere, Br, Position.X - (Rect.Width / 2) + 1, Position.Y - (Rect.Height / 2))
                '        Else
                '           Dim NewRect As New Drawing.RectangleF(position.X - m_dimension / 2 + 4, position.Y - (Rect.Height / 2), m_dimension - 4, Rect.Height)
                '          GraphToDraw.DrawString(m_Name, Carattere, Br, NewRect)
                '     End If
                '    End If
            End If
        Catch ex As System.Exception
        Finally
            Monitor.Exit(GraphToDraw)
        End Try

    End Sub

    Public Sub SetGraphToDraw(ByRef Graph As Drawing.Graphics)
        GraphToDraw = Graph
    End Sub

    Public Function ReadArea() As Drawing.Rectangle
        ReadArea = Area
    End Function
    Public Function MyAreaIsHere(ByVal x As Integer, ByVal y As Integer) As Boolean
        If Abs(x - Position.X) <= (m_Dimension / 2) And Abs(y - Position.Y) <= (m_Dimension / 2) Then
            MyAreaIsHere = True
        End If
    End Function
    Public Function ReadSelected() As Boolean
        ReadSelected = Selected
    End Function
    Public Sub SetSelected(ByVal v As Boolean)
        Selected = v
    End Sub

    Public Sub Move(ByVal dx As Integer, ByVal dy As Integer)
        Position.X = Position.X + dx
        Position.Y = Position.Y + dy
        'Muove le azioni
        m_ActionBlock.Move(dx, dy)
        CalculusArea()
    End Sub

    Public Sub CalculusArea()
        'Mamorizza l'area
        Area.X = Position.X - CInt(m_Dimension / 2) - 2
        Area.Y = Position.Y - CInt(m_Dimension / 2) - CInt(m_Dimension / 5)
        Area.Width = m_Dimension + 4
        Area.Height = m_Dimension + 4
        'If m_ActionBlock.ActionList.Count > 0 Then
        'Area = Drawing.Rectangle.Union(Area, m_ActionBlock.CalculusArea)
        'End If
    End Sub

    Public Sub SetBottomRectSelected(ByVal v As Boolean)
        BottomRectSelected = v
    End Sub
    Public Sub SetTopRectSelected(ByVal v As Boolean)
        TopRectSelected = v
    End Sub
    Public Sub DeSelectActions()
        m_ActionBlock.DeSelectActions()
    End Sub

    Public Sub Cancell(ByVal CancellSmallRectangels As Boolean)
        Draw(CancellSmallRectangels, BackColor, BackColor, BackColor, BackColor)
        'Cancella lo stato se richiesto
        If DrawState Then
            DrawStepState(True)
        End If
        'Cancella le azioni
        'Disegna le azioni
        If m_ActionBlock.ActionList.Count > 0 Then
            Dim penna As New Drawing.Pen(BackColor)
            'Line tra fase e prima azione
            GraphToDraw.DrawLine(penna, Position.X + CInt((m_Dimension / 2)), Position.Y, Position.X + m_Dimension, Position.Y)
            m_ActionBlock.CalcelActions()
        End If
    End Sub
    Public Sub DisposeMe()
        m_ActionBlock.DisposeMe()
        Me.Finalize()
    End Sub
    Public Sub DrawStepState(ByVal Cancel As Boolean)

    End Sub


End Class
