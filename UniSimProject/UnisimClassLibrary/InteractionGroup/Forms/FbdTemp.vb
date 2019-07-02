' Form temporaneo che simula l'editor FBD :-)
' 
Public Class FbdTemp
    Private m_Pou As pou
    Private m_pouBody As body
    Private m_VarToAdd As BaseVariable = Nothing
    Private WithEvents impl As FBD
    Public Sub New(ByVal p As pou, ByVal b As body)
        Me.InitializeComponent()
        m_Pou = p
        m_pouBody = b
        impl = CType(b.ReadImplementation(), FBD)
    End Sub

    Private Sub CycleDone() Handles impl.EndScan
        Panel1.Invalidate()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        m_VarToAdd = m_Pou.PouInterface.FindVariableByName(TextBox1.Text)
        If m_VarToAdd Is Nothing Then _
            m_VarToAdd = m_Pou.PouInterface.localVars.CreateAndAddVariable(TextBox1.Text, _
                "", "", "false", "BOOL")
    End Sub

    Private Sub Panel1_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseUp
        Static lastId As Integer = 1
        If m_VarToAdd IsNot Nothing Then
            Dim rect As New Drawing.Rectangle(e.Location, New Drawing.Size(44, 44))
            Dim gv As New GraphicalVariable(m_pouBody.ReadImplementation(), _
                lastId, rect, e.Location, Panel1.CreateGraphics(), FBDVariableType.InputOutput, m_VarToAdd)
            m_VarToAdd = Nothing
            gv.ResolveVariableLinks()
            impl.VariablesList.Add(gv)
            lastId += 1
        End If
        Panel1.Invalidate()
    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint
        impl.DrawElementsArea(New Drawing.Rectangle(Panel1.Location, Panel1.Size), True)
    End Sub
End Class