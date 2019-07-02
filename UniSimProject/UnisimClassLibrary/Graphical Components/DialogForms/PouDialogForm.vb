Public Class pouDialogForm
    Inherits System.Windows.Forms.Form
    Public m_Name As String
    Public m_pouType As EnumPouType
    Public m_bodyType As EnumBodyType


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
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents RadioButton6 As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents RadioButton7 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton8 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton9 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton10 As System.Windows.Forms.RadioButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(pouDialogForm))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.RadioButton1 = New System.Windows.Forms.RadioButton
        Me.RadioButton2 = New System.Windows.Forms.RadioButton
        Me.RadioButton3 = New System.Windows.Forms.RadioButton
        Me.Label2 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.RadioButton6 = New System.Windows.Forms.RadioButton
        Me.Label4 = New System.Windows.Forms.Label
        Me.RadioButton7 = New System.Windows.Forms.RadioButton
        Me.RadioButton8 = New System.Windows.Forms.RadioButton
        Me.RadioButton9 = New System.Windows.Forms.RadioButton
        Me.RadioButton10 = New System.Windows.Forms.RadioButton
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(8, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 16)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Name:"
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button1.Location = New System.Drawing.Point(72, 216)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(80, 32)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Ok"
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.Control
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.Location = New System.Drawing.Point(152, 216)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(80, 32)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "Cancel"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(64, 16)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(192, 20)
        Me.TextBox1.TabIndex = 0
        Me.TextBox1.Text = ""
        '
        'RadioButton1
        '
        Me.RadioButton1.Checked = True
        Me.RadioButton1.ForeColor = System.Drawing.Color.White
        Me.RadioButton1.Location = New System.Drawing.Point(16, 24)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.TabIndex = 8
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "Program"
        '
        'RadioButton2
        '
        Me.RadioButton2.Enabled = False
        Me.RadioButton2.ForeColor = System.Drawing.Color.White
        Me.RadioButton2.Location = New System.Drawing.Point(16, 48)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.TabIndex = 9
        Me.RadioButton2.Text = "Function"
        '
        'RadioButton3
        '
        Me.RadioButton3.Enabled = False
        Me.RadioButton3.ForeColor = System.Drawing.Color.White
        Me.RadioButton3.Location = New System.Drawing.Point(16, 72)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.TabIndex = 10
        Me.RadioButton3.Text = "Functional block"
        '
        'Label2
        '
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(8, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 16)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "POU type:"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.RadioButton1)
        Me.Panel1.Controls.Add(Me.RadioButton2)
        Me.Panel1.Controls.Add(Me.RadioButton3)
        Me.Panel1.Location = New System.Drawing.Point(56, 56)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(120, 152)
        Me.Panel1.TabIndex = 14
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.RadioButton6)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.RadioButton7)
        Me.Panel2.Controls.Add(Me.RadioButton8)
        Me.Panel2.Controls.Add(Me.RadioButton9)
        Me.Panel2.Controls.Add(Me.RadioButton10)
        Me.Panel2.Location = New System.Drawing.Point(184, 56)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(96, 152)
        Me.Panel2.TabIndex = 15
        '
        'RadioButton6
        '
        Me.RadioButton6.Enabled = False
        Me.RadioButton6.ForeColor = System.Drawing.Color.White
        Me.RadioButton6.Location = New System.Drawing.Point(24, 120)
        Me.RadioButton6.Name = "RadioButton6"
        Me.RadioButton6.Size = New System.Drawing.Size(48, 24)
        Me.RadioButton6.TabIndex = 12
        Me.RadioButton6.Text = "IL"
        '
        'Label4
        '
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(8, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(64, 16)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Body type:"
        '
        'RadioButton7
        '
        Me.RadioButton7.Checked = True
        Me.RadioButton7.ForeColor = System.Drawing.Color.White
        Me.RadioButton7.Location = New System.Drawing.Point(24, 24)
        Me.RadioButton7.Name = "RadioButton7"
        Me.RadioButton7.Size = New System.Drawing.Size(48, 24)
        Me.RadioButton7.TabIndex = 8
        Me.RadioButton7.TabStop = True
        Me.RadioButton7.Text = "SFC"
        '
        'RadioButton8
        '
        Me.RadioButton8.Enabled = False
        Me.RadioButton8.ForeColor = System.Drawing.Color.White
        Me.RadioButton8.Location = New System.Drawing.Point(24, 48)
        Me.RadioButton8.Name = "RadioButton8"
        Me.RadioButton8.Size = New System.Drawing.Size(48, 24)
        Me.RadioButton8.TabIndex = 9
        Me.RadioButton8.Text = "LD"
        '
        'RadioButton9
        '
        Me.RadioButton9.Enabled = False
        Me.RadioButton9.ForeColor = System.Drawing.Color.White
        Me.RadioButton9.Location = New System.Drawing.Point(24, 72)
        Me.RadioButton9.Name = "RadioButton9"
        Me.RadioButton9.Size = New System.Drawing.Size(48, 24)
        Me.RadioButton9.TabIndex = 10
        Me.RadioButton9.Text = "FDB"
        '
        'RadioButton10
        '
        Me.RadioButton10.Enabled = False
        Me.RadioButton10.ForeColor = System.Drawing.Color.White
        Me.RadioButton10.Location = New System.Drawing.Point(24, 96)
        Me.RadioButton10.Name = "RadioButton10"
        Me.RadioButton10.Size = New System.Drawing.Size(48, 24)
        Me.RadioButton10.TabIndex = 11
        Me.RadioButton10.Text = "ST"
        '
        'pouDialogForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(306, 271)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "pouDialogForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "POU"
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Dispose()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        m_Name = TextBox1.Text
        m_pouType = EnumPouType.program
        m_bodyType = EnumBodyType.SFC
    End Sub

    Private Sub SfcDialogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.AcceptButton = Button1
        TextBox1.Text = m_Name
        'Disattiva i pulsanti di conferma se non c'e il nome
        If TextBox1.Text = "" Then
            Button1.Enabled = False
        Else
            Button1.Enabled = True
        End If
        TextBox1.Focus()
    End Sub



    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        'Disattiva i pulsanti di conferma se non c'e il nome
        If TextBox1.Text = "" Then
            Button1.Enabled = False
        Else
            Button1.Enabled = True
        End If
    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        'Limita l'inserimento di caratteri per i nomi di variabili
        Dim NewChar As Char = e.KeyChar
        If Not InvalidVariableChars.IndexOf(e.KeyChar) = -1 Then
            e.Handled = True
        End If
    End Sub
End Class
