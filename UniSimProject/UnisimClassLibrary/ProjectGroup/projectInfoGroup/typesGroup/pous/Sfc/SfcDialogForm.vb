Public Class SfcDialogForm
    Inherits System.Windows.Forms.Form
    Public Nome As String
    Public Descrizione As String


#Region " Codice generato da Progettazione Windows Form "

    Public Sub New()
        MyBase.New()
        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()

    End Sub

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(16, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 16)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Nome:"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(16, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 16)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Descrizione: "
        '
        'Button1
        '
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button1.Location = New System.Drawing.Point(64, 104)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(80, 32)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Ok"
        '
        'Button2
        '
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.Location = New System.Drawing.Point(144, 104)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(80, 32)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "Annulla"
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(88, 64)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(184, 20)
        Me.TextBox3.TabIndex = 6
        Me.TextBox3.Text = ""
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(88, 24)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(184, 20)
        Me.TextBox1.TabIndex = 7
        Me.TextBox1.Text = ""
        '
        'SfcDialogForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(292, 143)
        Me.ControlBox = False
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SfcDialogForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Nuovo SFC"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Dispose()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Nome = TextBox1.Text
        Descrizione = TextBox3.Text
    End Sub

    Private Sub SfcDialogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.AcceptButton = Button1
        Button1.NotifyDefault(True)
        TextBox1.Text = Nome
        TextBox3.Text = Descrizione
    End Sub



End Class
