Public Class UnhandledExceptionDialogForm

    Private Function GetExceptionReport(ByVal except As Exception) As String
        Dim retVal As String = _
"Exception message: '" + except.Message + "'" + vbCrLf + _
"Exception source: " + except.Source + vbCrLf + _
"Exception target site: " + except.TargetSite.ToString() + vbCrLf + _
"Stack trace:" + vbCrLf + except.StackTrace + vbCrLf
        If Not IsNothing(except.Data) AndAlso except.Data.Count > 0 Then
            retVal += "Exception data:" + vbCrLf
            For Each key As Object In except.Data.Keys
                retVal += key.ToString() + " ==> " + except.Data.Item(key) + vbCrLf
            Next
        End If
        If Not IsNothing(except.InnerException) Then
            retVal += _
"-------------------------------------------" + vbCrLf + _
"Inner exception details follow" + vbCrLf + _
"-------------------------------------------" + _
    GetExceptionReport(except.InnerException)
        End If
        Return retVal
    End Function

    Public Sub New(ByVal except As Exception)
        Me.InitializeComponent()
        Me.txtDetails.Text = GetExceptionReport(except)
        UnisimClassLibrary.Macintoshize(Me)
    End Sub

End Class