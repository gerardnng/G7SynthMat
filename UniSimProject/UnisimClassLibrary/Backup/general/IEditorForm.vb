' L'uso di 3 interfacce è dovuto al fatto che alcuni linguaggi, ad esempio IL e ST,
' potrebbero non aver bisogno di implementare le funzioni di natura grafica o
' di monitoraggio

' Una versione molto basilare di un form di editing
Public Interface IBasicEditorForm

    Function ReadBody() As body
    Sub WriteTitlePanel()

End Interface

' Un form di editing che include anche un Monitor
Public Interface IMonitoringEditorForm
    Inherits IBasicEditorForm

    Sub StartMonitor()
    Sub StopMonitor()

End Interface

' La versione completa dei form per l'editing
Public Interface IEditorForm
    Inherits IMonitoringEditorForm

    Sub ResetCurrentOperation()
    Sub SetToolTip()
    Function GetCurrentIDocumentable() As IDocumentable
    Sub ResizePanel(ByVal newSize As System.Drawing.Size)

End Interface
