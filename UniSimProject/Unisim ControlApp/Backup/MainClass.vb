Public Class MainClass

    Shared Sub main()

        Control.CheckForIllegalCrossThreadCalls = False

        Dim Main As New MainForm
        AddHandler Application.ThreadException, AddressOf Threadexception
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf Unhandledexception
        'Try

        Application.Run(Main)
        'Catch ex As system.exception
        '    MsgBox(ex.Message)
        'End Try

    End Sub

    Private Shared Sub Threadexception(ByVal sender As Object, ByVal e As System.Threading.ThreadExceptionEventArgs)
        MsgBox(e.Exception.Message)
    End Sub

    Private Shared Sub Unhandledexception(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)
        MsgBox(e.ExceptionObject.ToString, MsgBoxStyle.Information, "Unhandled error")
    End Sub



End Class

