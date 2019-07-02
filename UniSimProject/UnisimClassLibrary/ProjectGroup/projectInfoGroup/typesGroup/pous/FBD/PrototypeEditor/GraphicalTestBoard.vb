Public Class GraphicalTestBoard

    Private m_ObjectInStore As IGraphicalObject = Nothing

    Private m_ToDraw As New Collections.Generic.List(Of IGraphicalObject)

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        m_ObjectInStore = New GraphicalVariable(New BooleanVariable("ciao", "", "", "false"), _
            1, New Drawing.Size(44, 44), New Drawing.Point(1, 1), _
            IIf(Of GraphicalVariableType)(CheckBox1.Checked, _
             GraphicalVariableType.Input, GraphicalVariableType.Output))
    End Sub

    Private Sub Panel1_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseUp
        If Not (m_ObjectInStore Is Nothing) Then
            m_ObjectInStore.Position = e.Location
            m_ObjectInStore.SetGraphToDraw(Panel1.CreateGraphics())
            m_ToDraw.Add(m_ObjectInStore)
            m_ObjectInStore = Nothing
        Else
            For Each igo As IGraphicalObject In m_ToDraw
                Dim rect As New Drawing.Rectangle(igo.Position, igo.Size)
                igo.Selected = (rect.Contains(e.Location))
            Next
        End If
        Panel1.Invalidate(True)
    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint
        For Each igo As IGraphicalObject In m_ToDraw
            igo.Draw(True)
        Next
    End Sub
End Class