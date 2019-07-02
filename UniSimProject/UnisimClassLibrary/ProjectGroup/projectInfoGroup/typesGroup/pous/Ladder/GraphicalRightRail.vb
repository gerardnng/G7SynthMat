Imports System.IO
Imports System.Math
Imports System.Xml
Imports System.Threading
Imports System.Drawing


Public Class GraphicalRightRail
    Inherits BaseGraphicalContact

    Public Overloads Overrides Sub DrawContact(ByVal DrawSmallRectangels As Boolean, ByVal Id As Integer)

    End Sub
    Public Overloads Overrides Sub Cancel(ByVal CancellSmallRectangels As Boolean, ByVal Id As Integer, _
        ByVal Name As String, ByVal Qualy As String, ByVal Var As BaseVariable, _
        ByVal Ind As BooleanVariable, ByVal Time As TimeSpan)

    End Sub

    Public Overloads Overrides Sub DrawStepState(ByVal Cancel As Boolean, ByVal Var As BaseVariable)

    End Sub
    Public Overloads Overrides Sub DrawContact(ByVal DrawSmallRectangels As Boolean, ByVal Id As Integer, _
        ByVal Name As String, ByVal Qualy As String, _
        ByVal Var As BaseVariable, ByVal Ind As BooleanVariable, ByVal Time As TimeSpan)
        Draw(DrawSmallRectangels, SelectionColor, NotSelectionColor, TextColor, BackColor, Id)
    End Sub
    Public Overrides Sub SetFinalValue()

    End Sub

    Public Overloads Sub Draw(ByVal DrawSmallRectangels As Boolean, ByVal Col1 As Drawing.Color, _
        ByVal Col2 As Drawing.Color, ByVal Col3 As Drawing.Color, ByVal Col4 As Drawing.Color, _
        ByVal Id As Integer)

        Try
            If Monitor.TryEnter(Graphtodraw, 2000) Then
                Dim Penna As New Drawing.Pen(Color.Black)
                Penna.Width = 2
                Dim larg As Integer
                Dim alt As Integer
                If m_Selected Then
                    'Penna.Color = SelectionColor
                Else
                    Penna.Color = Color.Black
                End If

                GraphToDraw.DrawLine(Penna, Position.X - 20, Position.Y - 30, Position.X - 20, Position.Y + 30)              'orizz

                'disrgna i reofori
                If DrawSmallRectangels Then
                    If TopRectSelected Then
                        Penna.Color = Col1
                    Else
                        Penna.Color = Col2
                    End If
                    larg = CInt(m_Dimension.Height / 5)
                    alt = larg
                    Penna.Width = 2
                    GraphToDraw.DrawRectangle(Penna, Position.X - CInt(m_Dimension.Width / 2) - _
                        CInt(m_Dimension.Width / 5), Position.Y - CInt(m_Dimension.Height / 10), _
                        larg, alt)

                    If BottomRectSelected Then
                        Penna.Color = Col1
                    Else
                        Penna.Color = Col2
                    End If
                    larg = CInt(m_Dimension.Height / 5)
                    alt = larg
                    Penna.Width = 2
                    'GraphToDraw.DrawRectangle(Penna, Position.X + CInt(m_Dimension / 2) + 1, Position.Y - CInt(m_Dimension / 10), larg, alt)
                End If
            End If
        Catch ex As System.Exception
        Finally
            Monitor.Exit(GraphToDraw)
        End Try

    End Sub

    Public Overrides Sub DisposeMe()
        Me.Finalize()
    End Sub

    Public Sub New(ByRef RefLadder As Ladder, ByVal BackCol As Drawing.Color, _
        ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, _
        ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, _
        ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, _
        ByRef Graph As Drawing.Graphics, ByVal DrState As Boolean, ByVal Dimension As Integer)
        MyBase.New(RefLadder, BackCol, SelectionCol, NotSelectionCol, TextCol, Car, ColActive, ColDeactive, ColPreActive, Graph, DrState, Dimension)
        Me.m_Id = 20
        CalculusArea()
    End Sub
    Public Sub New(ByVal StepNumber As Integer, ByVal StepName As String, ByVal StepDocumentation As String, _
        ByRef RefLadder As Ladder, ByVal Ini As Boolean, ByVal Fin As Boolean, ByVal P As Drawing.Point, _
        ByVal BackCol As Drawing.Color, ByVal SelectionCol As Drawing.Color, _
        ByVal NotSelectionCol As Drawing.Color, ByVal TextCol As Drawing.Color, _
        ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, _
        ByVal ColPreActive As Drawing.Color, ByRef Graph As Drawing.Graphics, ByVal DrState As Boolean, _
        ByVal Dimension As Integer, ByVal Id As Integer, ByVal Name As String, _
        ByVal Qualy As String, ByVal Var As BaseVariable, ByVal Ind As BooleanVariable, ByVal Time As TimeSpan)
        MyBase.New(StepNumber, StepName, StepDocumentation, RefLadder, Ini, Fin, P, BackCol, SelectionCol, NotSelectionCol, TextCol, Car, ColActive, ColDeactive, ColPreActive, Graph, DrState, Dimension, Id, Name, Qualy, Var, Ind, Time)
        Me.m_Id = 20
        CalculusArea()
    End Sub


    

    Public Overrides Sub CalculusArea()
        'Mamorizza l'area
        Area.X = Position.X - CInt(m_Dimension.Width / 2) - 2
        Area.Y = Position.Y - CInt(m_Dimension.Height / 2) - CInt(m_Dimension.Height / 5)
        Area.Width = m_Dimension.Width + 4
        Area.Height = m_Dimension.Height + 4
    End Sub

    Public Overloads Overrides Sub Cancel(ByVal CancellSmallRectangels As Boolean, ByVal Id As Integer)

    End Sub
    Public Overloads Sub Cancell(ByVal CancellSmallRectangels As Boolean)

    End Sub

    Public Overrides Function CreateInstance(ByRef NewLadder As Ladder) As Object
        CreateInstance = New GraphicalRightRail(NewLadder, Me.BackColor, Me.SelectionColor, Me.NotSelectionColor, _
            Me.TextColor, Me.Carattere, Me.ColorActive, Me.ColorDeactive, Me.ColorPreActive, _
                Me.GraphToDraw, Me.DrawState, 44)
        CreateInstance.Number = Me.Number
    End Function


    Public Overloads Overrides Sub DrawStepState(ByVal Cancel As Boolean)

    End Sub

    Public Overrides Sub Move(ByVal dx As Integer, ByVal dy As Integer)

    End Sub

    Public Overrides Sub ResolveVariablesLinks()

    End Sub

    Public Overrides Sub SetGraphToDraw(ByRef Graph As System.Drawing.Graphics)
        GraphToDraw = Graph
    End Sub

    Public Overrides Sub xmlExport(ByRef RefXMLProjectWriter As System.Xml.XmlTextWriter)
        RefXMLProjectWriter.WriteStartElement("rightPowerRail")
        RefXMLProjectWriter.WriteAttributeString("localId", m_Number.ToString())
        RefXMLProjectWriter.WriteAttributeString("height", m_Dimension.Height.ToString())
        RefXMLProjectWriter.WriteAttributeString("width", m_Dimension.Width.ToString())
        RefXMLProjectWriter.WriteStartElement("position")
        RefXMLProjectWriter.WriteAttributeString("x", Position.X.ToString())
        RefXMLProjectWriter.WriteAttributeString("y", Position.Y.ToString())
        RefXMLProjectWriter.WriteEndElement() 'position
        RefXMLProjectWriter.WriteEndElement() 'rightPowerRail
    End Sub

    Public Overrides Sub xmlImport(ByRef reader As System.Xml.XmlTextReader)
        Dim NodeDepth As Integer = reader.Depth
        If reader.MoveToAttribute("localId") Then
            m_Number = Integer.Parse(reader.Value)
        End If
        m_Dimension = New Size(0, 0)
        If reader.MoveToAttribute("height") Then
            m_Dimension.Height = Integer.Parse(reader.Value)
        End If
        If reader.MoveToAttribute("width") Then
            m_Dimension.Width = Integer.Parse(reader.Value)
        End If
        If Not reader.IsEmptyElement Then
            reader.Read()
            While reader.Depth > NodeDepth
                Select Case reader.Name
                    Case "position"
                        If reader.NodeType = XmlNodeType.Element Then
                            If reader.MoveToAttribute("x") Then
                                Position.X = reader.Value
                            End If
                            If reader.MoveToAttribute("y") Then
                                Position.Y = reader.Value
                            End If
                        End If
                End Select
                If reader.Depth > NodeDepth Then
                    reader.Read()
                End If
            End While
        End If
        CalculusArea()
    End Sub
End Class

