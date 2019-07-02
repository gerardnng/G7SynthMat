Public Class TransitionDialogForm
    Inherits System.Windows.Forms.Form
    Public m_Name As String
    Public m_Documentation As String
    Public m_Sfc As Sfc
    Public m_Condition As BooleanExpression
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Private m_ExpressionValidated As Boolean

#Region " Codice generato da Progettazione Windows Form "

    Public Sub New()
        MyBase.New()
        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()

        Macintoshize(Me)

        Me.AcceptButton = Button1

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
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button5 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TransitionDialogForm))
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Button5 = New System.Windows.Forms.Button
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.Label5 = New System.Windows.Forms.Label
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button1.Location = New System.Drawing.Point(165, 126)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(72, 32)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Ok"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.Control
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.Location = New System.Drawing.Point(316, 127)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(72, 32)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(92, 20)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(320, 22)
        Me.TextBox1.TabIndex = 2
        Me.TextBox1.Text = "true"
        '
        'Label2
        '
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(20, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 16)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Condition:"
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.SystemColors.Control
        Me.Button5.ForeColor = System.Drawing.Color.Black
        Me.Button5.Location = New System.Drawing.Point(440, 19)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(96, 20)
        Me.Button5.TabIndex = 32
        Me.Button5.Text = "Add variable..."
        Me.Button5.UseVisualStyleBackColor = False
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(89, 54)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(318, 52)
        Me.Label5.TabIndex = 35
        Me.Label5.Text = "Use + as Or, * as And, ! as Not" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "[S0.X] is true if step S0 is active" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "[S0.t>1s] i" & _
            "s true if step S0 has been active for more than 1 second" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Type arithmetic compar" & _
            "isons in the form {variable<=>expression}"
        '
        'TransitionDialogForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(548, 170)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TransitionDialogForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Transition"
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Private Sub SfcDialogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.AcceptButton = Button1
        If Not IsNothing(m_Condition) Then
            TextBox1.Text = m_Condition.GetExpressionString
        End If
    End Sub
    Private Sub TryEnableOkButton()
        ErrorProvider1.Clear()
        'Crea un oggetto condizione temporaneo
        Dim CondTemp As New BooleanExpression(m_Sfc)
        ' Cerca di settare l'espressione
        Dim validExpr As Boolean = CondTemp.SetExpression(TextBox1.Text)
        ' Se non è valida segnala l'errore
        If Not (validExpr) Then _
            ErrorProvider1.SetError(TextBox1, CondTemp.ReadError)
        CondTemp = Nothing
        Button1.Enabled = validExpr
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Crea l'espressione (assicurarsi che sia validata prima di procedere, questo
        ' costruttore genera una ArgumentException per espressioni non valide)
        m_Condition = New BooleanExpression(m_Sfc, TextBox1.Text)
    End Sub
    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        TryEnableOkButton()
    End Sub
    Private Sub TextBox2_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'Limita l'inserimento di caratteri per i nomi di variabili
        Dim NewChar As Char = e.KeyChar
        If Not InvalidVariableChars.IndexOf(e.KeyChar) = -1 Then
            e.Handled = True
        End If
    End Sub
    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim VariablesSelectorDialog As New VariablesSelectorDialogForm(m_Sfc.ResGlobalVariables, m_Sfc.PouInterface)
        Dim ResultDialog As System.Windows.Forms.DialogResult = VariablesSelectorDialog.ShowDialog(Me)
        If ResultDialog = Windows.Forms.DialogResult.OK Then
            Dim Init As Integer
            Dim NameLength As Integer
            If Not IsNothing(VariablesSelectorDialog.m_SelectedVar) Then
                Init = TextBox1.SelectionStart
                NameLength = VariablesSelectorDialog.m_SelectedVar.name.length
                TextBox1.Text = TextBox1.Text.Insert(TextBox1.SelectionStart, VariablesSelectorDialog.m_SelectedVar.name)
            End If
            TextBox1.Focus()
            TextBox1.SelectionStart = Init + NameLength
            TextBox1.SelectionLength = 0
        End If
        TryEnableOkButton()
    End Sub
End Class
