Public Class VariableDialogForm
    Inherits System.Windows.Forms.Form
    Public m_Name As String
    Public m_Documentation As String
    Public m_Address As String
    Public m_InitialValue As String = "false"
    Public m_Type As String
    ' True per creare una nuova variabile, False per modificarne una esistente
    Public m_ChangeType As Boolean
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox

#Region " Codice generato da Progettazione Windows Form "

    Public Sub New()
        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()

        Macintoshize(Me)

    End Sub
    Public Sub New(ByVal var As BaseVariable)
        Me.New()
        m_Name = var.Name
        m_Documentation = var.Documentation
        m_Address = var.Address
        m_InitialValue = var.InitialValueToUniversalString()
        m_Type = var.dataType
        m_ChangeType = False
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
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(VariableDialogForm))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(12, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 16)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Name: "
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.Control
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.Location = New System.Drawing.Point(216, 123)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(80, 32)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button1.Enabled = False
        Me.Button1.Location = New System.Drawing.Point(136, 123)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(80, 32)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Ok"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(84, 24)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(180, 20)
        Me.TextBox1.TabIndex = 0
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(84, 60)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(104, 20)
        Me.TextBox3.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(12, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 16)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Address:"
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(304, 60)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(108, 20)
        Me.TextBox4.TabIndex = 3
        Me.TextBox4.Text = "false"
        '
        'Label4
        '
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(228, 64)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(68, 16)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Initial value:"
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.Items.AddRange(New Object() {"BOOL", "INT", "REAL"})
        Me.ComboBox1.Location = New System.Drawing.Point(336, 24)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(76, 21)
        Me.ComboBox1.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(280, 28)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(52, 16)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "Type:"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.ForeColor = System.Drawing.Color.White
        Me.CheckBox1.Location = New System.Drawing.Point(15, 97)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(234, 17)
        Me.CheckBox1.TabIndex = 16
        Me.CheckBox1.Text = "Open it again so I can create more variables"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'VariableDialogForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(426, 167)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.TextBox4)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "VariableDialogForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Variable"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    ' Usata per obbligare l'utente a creare una variabile con il nome prefissato
    Public Sub BlockNameTo(ByVal name As String, Optional ByVal forceOK As Boolean = True)
        m_Name = name
        TextBox1.Text = m_Name
        TextBox1.Enabled = False
        Button2.Visible = Not (forceOK)
        m_ChangeType = True
    End Sub

    Private Sub VariableDialogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TextBox1.Text = m_Name
        TextBox3.Text = m_Address
        If m_Type <> "" Then
            ComboBox1.Text = m_Type
        Else
            ComboBox1.SelectedIndex = 0
        End If
        If Not m_ChangeType Then
            ComboBox1.Enabled = False
            CheckBox1.Visible = False
        End If
        TextBox4.Text = m_InitialValue
        TextBox1.Focus()
        TryEnableOkButton()
        Me.AcceptButton = Button1
    End Sub
    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        TryEnableOkButton()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If (IsNumeric(Mid(TextBox1.Text, 1, 1))) Then
            MsgBox("Variable name can't start with a number", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
        Else
            m_Name = TextBox1.Text
            m_Address = TextBox3.Text
            m_InitialValue = TextBox4.Text
            If m_ChangeType Then
                m_Type = ComboBox1.Text
            End If
        End If
    End Sub
    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        'Limita l'inserimento di caratteri per i nomi di variabili
        Dim NewChar As Char = e.KeyChar
        If Not InvalidVariableChars.IndexOf(e.KeyChar) = -1 Then
            e.Handled = True
        End If
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        TextBox4.Text = VariablesManager.DefaultValue(ComboBox1.SelectedItem)
        TryEnableOkButton()
    End Sub
    Private Sub TryEnableOkButton()
        'Controlla initial value e in nome
        If VariablesManager.CheckValue(TextBox4.Text, ComboBox1.Text) And TextBox1.Text <> "" Then
            Button1.Enabled = True
        Else
            Button1.Enabled = False
        End If
    End Sub
    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged
        TryEnableOkButton()
    End Sub
    Public ReadOnly Property AgainOn() As Boolean
        Get
            Return (m_ChangeType AndAlso CheckBox1.Checked AndAlso Me.DialogResult = Windows.Forms.DialogResult.OK)
        End Get
    End Property
End Class
