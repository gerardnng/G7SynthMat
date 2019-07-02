Imports System
Imports System.Collections.Generic
Public Class OpenErrorsForm

    Public Sub New(ByVal errors As String)

        Me.InitializeComponent()
        Me.txtErrors.Text = errors

        UnisimClassLibrary.Macintoshize(Me)

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Hide()
    End Sub

End Class