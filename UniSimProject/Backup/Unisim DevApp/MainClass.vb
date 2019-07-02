Imports System.Collections.Generic
Imports System.IO

Public Class MainClass

#Region "Metodi interni"
    Private Shared Function Exists(ByVal path As String) As Boolean
        Return (New FileInfo(path)).Exists
    End Function

    Private Shared Files() As String = { _
                XmlSchemaPath, _
                Path.Combine(CursorsPath, "CursorContact.cur"), _
                Path.Combine(CursorsPath, "CursorMacroStep.cur"), _
                Path.Combine(CursorsPath, "CursorMove.cur"), _
                Path.Combine(CursorsPath, "CursorStep.cur"), _
                Path.Combine(CursorsPath, "CursorTransition.cur") _
            }

    ' Controlla che i file di supporto richiesti da UniSim siano presenti
    ' Se tutto OK ritorna (True,<unspecified>)
    ' Se manca un file ritorna (False,percorso del file)
    Shared Function CheckForRequiredFiles() As KeyValuePair(Of Boolean, String)
        For Each File As String In Files
            If Not Exists(File) Then _
                Return New KeyValuePair(Of Boolean, String)(False, File)
        Next
        Return New KeyValuePair(Of Boolean, String)(True, "")
    End Function

    Shared Sub Prepare()
        ' Verifica che sia tutto "in ordine". Per ora non controlliamo la
        ' guida in linea poichè non è ritenuta una funzione necessaria
        ' Quando passeremo alla guida CHM-based controlleremo anche
        ' il file dell'help
        Dim kvp As KeyValuePair(Of Boolean, String) = CheckForRequiredFiles()
        If Not (kvp.Key) Then
            MessageBox.Show( _
"A file required for UniSim to work has not been found. File path: " + vbCrLf + _
kvp.Value + vbCrLf + _
"You should obtain this file from a valid source or reinstall UniSim", _
UnisimClassLibrary.UniSimVersion.VersionInfo.PrintableDescriptionForTool(), _
MessageBoxButtons.OK, MessageBoxIcon.Error)
            Environment.Exit(1023)
        End If

        ' Questa riga serve ad evitare una eccezione all'avvio di UniSim
        ' in quanto il tool manipola controlli grafici in thread diversi
        ' da quello che li ha creati (e il framework, se questo flag non
        ' viene settato a False, impedisce tali manipolazioni)
        System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False

        ' Attiva gli stili visuali su XP/Vista/... se richiesti dall'utente
        If UnisimClassLibrary.Preferences.GetBoolean("WantVisualStyles", False) Then _
            System.Windows.Forms.Application.EnableVisualStyles()

        ' Aggiunge i gestori per le eccezioni non gestite
        AddHandler Application.ThreadException, AddressOf Threadexception
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf Unhandledexception
    End Sub

    ' Questo attributo serve a mettere in pausa il codice all'interno di Main()
    ' se si usa il pulsante sulla Toolbar. Se bisogna fare debugging di questa routine
    ' commentarlo (CTRL+K,CTRL+C)
    <DebuggerNonUserCode()> _
    Shared Sub Run()
        Application.Run(New MainForm())
    End Sub

    ' Mostra un messaggio all'utente
    Private Shared Sub HandleUnhandledException(ByVal thing As Exception)
        Dim uhed As New UnhandledExceptionDialogForm(thing)
        Dim ret As DialogResult = uhed.ShowDialog()
        ' Yes = continua
        ' No = esci
        If ret = DialogResult.Yes Then Exit Sub
        Environment.Exit(1024)
    End Sub

    Private Shared Sub Threadexception(ByVal sender As Object, ByVal e As System.Threading.ThreadExceptionEventArgs)
        HandleUnhandledException(e.Exception)
    End Sub

    Private Shared Sub Unhandledexception(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)
        HandleUnhandledException(e.ExceptionObject)
    End Sub
#End Region

    Shared Sub Main()

        ' Prepara l'esecuzione di UniSim
        Prepare()

        'Esegue UniSim
        Run()

    End Sub

End Class