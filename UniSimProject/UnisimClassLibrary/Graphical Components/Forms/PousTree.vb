Public Class PousTree
    Inherits System.Windows.Forms.TreeView
    Dim m_mdiParentForm As System.Windows.Forms.Form
    Dim m_project As Project


#Region " Codice generato da Progettazione Windows Form "

    Public Sub New(ByRef mdiParentForm As System.Windows.Forms.Form, ByRef refProject As Project)
        MyBase.New()

        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()
        m_mdiParentForm = mdiParentForm
        RefreshTreeStruct()
    End Sub

    'UserControl esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form.
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        '
        'PousTree
        '
        Me.Name = "PousTree"

    End Sub

#End Region

    Public Sub RefreshTreeStruct()
        Me.SuspendLayout()
        Nodes.Clear()
        Nodes.Add("Data types")
        Dim PousNode As System.Windows.Forms.TreeNode = Nodes.Add("POUs")
        '        For Each pouTemp As pou In m_mdiParentForm.



    End Sub
End Class
