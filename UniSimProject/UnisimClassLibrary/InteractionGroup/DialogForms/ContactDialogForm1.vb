Public Class ContactDialogForm1
    Inherits System.Windows.Forms.Form
    Public m_Name As String
    Public m_Documentation As String
    Public m_Ladder As Ladder
    Private m_Pou As pou
    Public m_variable As BaseVariable
    Public m_Condition As BooleanExpression
    Private m_ExpressionValidated As Boolean
    Public ContactName As String
    Public StepDocumentation As String
    Public m_qualifier As String
    Public VarValue As String
    Public m_indicator As BaseVariable
    Public m_time As TimeSpan
    Public StatoVariabile As Boolean
    Public NomeVariabile As String
    Private m_ResGlobalVariables As VariablesLists
    Private m_pouInterface As pouInterface



#Region " Codice generato da Progettazione Windows Form "

    Public Sub New(ByVal LD As Ladder, ByVal POU As pou)
        MyBase.New()
        m_Ladder = LD
        m_Pou = POU


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
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents NumericUpDown2 As System.Windows.Forms.NumericUpDown
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Button2 = New System.Windows.Forms.Button
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Button6 = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.Control
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.Location = New System.Drawing.Point(104, 80)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(80, 40)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "Cancel"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(96, 16)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(88, 20)
        Me.TextBox1.TabIndex = 0
        Me.TextBox1.Text = ""
        '
        'Label5
        '
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(16, 20)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(56, 16)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Name:"
        '
        'Button6
        '
        Me.Button6.BackColor = System.Drawing.SystemColors.Control
        Me.Button6.Location = New System.Drawing.Point(184, 80)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(80, 40)
        Me.Button6.TabIndex = 7
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button1.Location = New System.Drawing.Point(24, 80)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(80, 40)
        Me.Button1.TabIndex = 8
        Me.Button1.Text = "Ok"
        '
        'TextBox2
        '
        Me.TextBox2.Enabled = False
        Me.TextBox2.Location = New System.Drawing.Point(96, 48)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(88, 20)
        Me.TextBox2.TabIndex = 9
        Me.TextBox2.Text = ""
        '
        'Label1
        '
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(8, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 24)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Variable Name"
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(56, 136)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(112, 20)
        Me.TextBox3.TabIndex = 11
        Me.TextBox3.Text = ""
        '
        'ContactDialogForm1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(290, 176)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Button2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ContactDialogForm1"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Contact"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Dispose()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ContactName = TextBox1.Text
    End Sub

    Private Sub SfcDialogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' AcceptButton = Button1
        TextBox1.Text = ContactName
        TextBox2.Text = NomeVariabile

        TextBox1.Focus()
        TextBox2.Focus()
        TryEnableOkButton()

        If TextBox2.Text <> "" Then
            Button6.Text = "Change Var"
        Else
            Button6.Text = "Add Var"
        End If

    End Sub
    Private Sub TryEnableOkButton()
        'Disattiva i pulsanti di conferma se non c'e il nome
        If TextBox1.Text = "" Then
            '    Button1.Enabled = False
        Else
            '   Button1.Enabled = True
        End If
    End Sub
    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        'Limita l'inserimento di caratteri per i nomi di variabili
        Dim NewChar As Char = e.KeyChar
        If Not InvalidVariableChars.IndexOf(e.KeyChar) = -1 Then
            e.Handled = True
        End If
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        TryEnableOkButton()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Dim VariablesSelectorDialog As New VariablesSelectorDialogForm(m_Ladder.ResGlobalVariables, m_Pou.PouInterface)
        Dim ResultDialog As System.Windows.Forms.DialogResult = VariablesSelectorDialog.ShowDialog(Me)
        If ResultDialog = DialogResult.OK Then
            m_variable = VariablesSelectorDialog.m_SelectedVar
            TextBox2.Text = m_variable.Name
            NomeVariabile = m_variable.Name
            StatoVariabile = CBool(m_variable.ReadValue) 'legge lo stato della variabile
            TextBox3.Text = StatoVariabile 'lo riporta nella casella di testo
        End If
        VariablesSelectorDialog.Dispose()
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        'modifica solo il nome
        ContactName = TextBox1.Text
        m_Documentation = TextBox2.Text

    End Sub
End Class
