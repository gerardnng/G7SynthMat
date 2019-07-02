Imports System.IO
Imports System.Math
Imports System.Xml
Imports System.Threading
Imports System.Drawing

Public Class GraphicalCoil5
    Inherits BaseGraphicalCoil
    Protected m_ActionBlock As ActionBlock
    Protected m_PulseActionExecuted As Boolean
    Protected m_NotPulseActionExecuted As Boolean

    Public Overloads Overrides Sub DrawCoil(ByVal DrawSmallRectangels As Boolean)
        Draw(DrawSmallRectangels, SelectionColor, NotSelectionColor, TextColor, BackColor)
    End Sub
    Public Overloads Sub Draw(ByVal DrawSmallRectangels As Boolean, ByVal Col1 As Drawing.Color, ByVal Col2 As Drawing.Color, ByVal Col3 As Drawing.Color, ByVal Col4 As Drawing.Color)
        Try

            If Monitor.TryEnter(Graphtodraw, 2000) Then
                Dim Penna As New Drawing.Pen(Col1)
                Dim larg As Integer
                Dim alt As Integer
                Dim Br As Drawing.Brush
                If Selected Then
                    'Penna.Color = SelectionColor
                Else
                    Penna.Color = Col2
                End If

                GraphToDraw.DrawLine(Penna, Position.X - 20, Position.Y, Position.X - 6, Position.Y)                          'orizz
                GraphToDraw.DrawLine(Penna, Position.X + 6, Position.Y, Position.X + 20, Position.Y)          'orizz
                GraphToDraw.DrawLine(Penna, Position.X - 6, Position.Y + 14, Position.X - 6, Position.Y - 14) 'vert
                GraphToDraw.DrawLine(Penna, Position.X + 6, Position.Y + 14, Position.X + 6, Position.Y - 14) 'vert

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



    Public Sub DeSelectActions()
        m_ActionBlock.DeSelectActions()
    End Sub


    Public Overrides Sub DisposeMe()
        m_ActionBlock.DisposeMe()
        Me.Finalize()
    End Sub

    Public Sub New(ByRef RefLadder As Ladder, ByVal BackCol As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, ByRef Graph As Drawing.Graphics, ByVal DrState As Boolean, ByVal Dimension As Integer)
        MyBase.New(RefLadder, BackCol, SelectionCol, NotSelectionCol, TextCol, Car, ColActive, ColDeactive, ColPreActive, Graph, DrState, Dimension)
        'Calcola la posizione dell'actionBlock

        Dim AcBolockPosition As New Drawing.Point
        AcBolockPosition.X = position.X + m_Dimension
        AcBolockPosition.Y = position.Y
        m_ActionBlock = New ActionBlock(m_Dimension, RefLadder, AcBolockPosition, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, GraphToDraw)
        CalculusArea()
    End Sub
    Public Sub New(ByVal StepNumber As Integer, ByVal StepName As String, ByVal StepDocumentation As String, ByRef RefLadder As Ladder, ByVal Ini As Boolean, ByVal Fin As Boolean, ByVal P As Drawing.Point, ByVal BackCol As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, ByRef Graph As Drawing.Graphics, ByVal DrState As Boolean, ByVal Dimension As Integer)
        MyBase.New(StepNumber, StepName, StepDocumentation, RefLadder, Ini, Fin, P, BackCol, SelectionCol, NotSelectionCol, TextCol, Car, ColActive, ColDeactive, ColPreActive, Graph, DrState, Dimension)
        'Calcola la posizione dell'actionBlock
        Dim AcBolockPosition As New Drawing.Point
        AcBolockPosition.X = position.X + m_Dimension
        AcBolockPosition.Y = position.Y
        m_ActionBlock = New ActionBlock(m_Dimension, RefLadder, AcBolockPosition, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, GraphToDraw)
        'Aggiunge la fase ai collegamenti precedenti
        'm_ActionBlock.XmlPreviousConnectionsList.Add(Me)
        CalculusArea()
    End Sub


    Public Overrides Sub Active()
        Dim StateChanged As Boolean = Not m_Active
        m_Active = True
        m_TimeActivation = Now  'Imposta l'istate di attivazione
        m_PreActive = False
        'Disegna lo stato se richiesto
        If StateChanged And DrawState Then
            DrawStepState(False)
        End If

    End Sub

    Public Overrides Sub CalculusArea()
        'Mamorizza l'area
        Area.X = Position.X - CInt(m_Dimension / 2) - 2
        Area.Y = Position.Y - CInt(m_Dimension / 2) - CInt(m_Dimension / 5)
        Area.Width = m_Dimension + 4
        Area.Height = m_Dimension + 4
        'If m_ActionBlock.ActionList.Count > 0 Then
        'Area = Drawing.Rectangle.Union(Area, m_ActionBlock.CalculusArea)
        'End If
    End Sub

    Public Overrides Sub Cancell(ByVal CancellSmallRectangels As Boolean)
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
            GraphToDraw.DrawLine(penna, Position.X + CInt((m_Dimension / 2)), position.Y, position.X + m_Dimension, position.Y)
            m_ActionBlock.CalcelActions()
        End If
    End Sub

    Public Overrides Function CreateInstance(ByRef NewLadder As Ladder) As Object
        'd() 'im_ActionBlock.CreateInstance()
        'CreateInstance = New GraphicalContact(m_Number, m_Name, m_Documentation, NewLadder, m_Initial, m_final, Position, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ColorActive, ColorDeactive, ColorPreActive, Nothing, False, m_Dimension)

    End Function

    Public Overrides Sub Disactive()
        'Termina le azioni non impulsive se era ancora attiva
        m_Active = False
        m_preactive = False
        'Disegna lo stato se è cambiato e se richiesto
        If DrawState Then
            DrawStepState(False)
        End If
    End Sub


    Public Overrides Sub DrawStepState(ByVal Cancel As Boolean)

    End Sub

    Public Overrides Sub Move(ByVal dx As Integer, ByVal dy As Integer)
        Position.X = Position.X + dx
        Position.Y = Position.Y + dy
        'Muove le azioni
        m_ActionBlock.Move(dx, dy)
        CalculusArea()
    End Sub

    Public Overrides Sub PreActive()

    End Sub

    Public Overrides Sub ResolveVariablesLinks()

    End Sub

    Public Overrides Sub SetGraphToDraw(ByRef Graph As System.Drawing.Graphics)

    End Sub

    Public Overrides Sub xmlExport(ByRef RefXMLProjectWriter As System.Xml.XmlTextWriter)

    End Sub

    Public Overrides Sub xmlImport(ByRef RefXMLProjectReader As System.Xml.XmlTextReader)

    End Sub
End Class
