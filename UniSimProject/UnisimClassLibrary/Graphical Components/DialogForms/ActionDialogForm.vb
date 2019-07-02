Public Class ActionDialogForm
    Inherits System.Windows.Forms.Form
    Public Qualifier As String
    Public Nome As String
    Public Var As BaseVariable
    Public Indicator As BaseVariable
    Public Time As TimeSpan
    Private m_ResGlobalVariables As VariablesLists



    '----------------------------
    'Blocco aggiuntivo alla norma
    Public RefSfc As Sfc
    Public StepsToForces As GraphicalStepsList
    '----------------------------





#Region " Codice generato da Progettazione Windows Form "

    Public Sub New(ByVal ResGlobalVariables As VariablesLists)
        MyBase.New()
        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()
        'Assegna l'interfaccia
        m_ResGlobalVariables = ResGlobalVariables
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
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents NumericUpDown1 As System.Windows.Forms.NumericUpDown
    Friend WithEvents NumericUpDown2 As System.Windows.Forms.NumericUpDown
    Friend WithEvents NumericUpDown3 As System.Windows.Forms.NumericUpDown
    Friend WithEvents NumericUpDown4 As System.Windows.Forms.NumericUpDown
    Friend WithEvents NumericUpDown5 As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox3 As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ActionDialogForm))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.NumericUpDown1 = New System.Windows.Forms.NumericUpDown
        Me.NumericUpDown2 = New System.Windows.Forms.NumericUpDown
        Me.NumericUpDown3 = New System.Windows.Forms.NumericUpDown
        Me.NumericUpDown4 = New System.Windows.Forms.NumericUpDown
        Me.NumericUpDown5 = New System.Windows.Forms.NumericUpDown
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboBox2 = New System.Windows.Forms.ComboBox
        Me.ComboBox3 = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(236, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 16)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Qualifier:"
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button1.ForeColor = System.Drawing.Color.Black
        Me.Button1.Location = New System.Drawing.Point(184, 104)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(76, 32)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Ok"
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.Control
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.ForeColor = System.Drawing.Color.Black
        Me.Button2.Location = New System.Drawing.Point(260, 104)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(76, 32)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "Cancel"
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.Location = New System.Drawing.Point(296, 16)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(52, 21)
        Me.ComboBox1.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(16, 68)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 16)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Timer:"
        '
        'NumericUpDown1
        '
        Me.NumericUpDown1.Location = New System.Drawing.Point(96, 64)
        Me.NumericUpDown1.Maximum = New Decimal(New Integer() {30, 0, 0, 0})
        Me.NumericUpDown1.Name = "NumericUpDown1"
        Me.NumericUpDown1.Size = New System.Drawing.Size(48, 20)
        Me.NumericUpDown1.TabIndex = 5
        Me.NumericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'NumericUpDown2
        '
        Me.NumericUpDown2.Location = New System.Drawing.Point(164, 64)
        Me.NumericUpDown2.Maximum = New Decimal(New Integer() {23, 0, 0, 0})
        Me.NumericUpDown2.Name = "NumericUpDown2"
        Me.NumericUpDown2.Size = New System.Drawing.Size(48, 20)
        Me.NumericUpDown2.TabIndex = 6
        Me.NumericUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'NumericUpDown3
        '
        Me.NumericUpDown3.Location = New System.Drawing.Point(236, 64)
        Me.NumericUpDown3.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
        Me.NumericUpDown3.Name = "NumericUpDown3"
        Me.NumericUpDown3.Size = New System.Drawing.Size(48, 20)
        Me.NumericUpDown3.TabIndex = 7
        Me.NumericUpDown3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'NumericUpDown4
        '
        Me.NumericUpDown4.Location = New System.Drawing.Point(304, 64)
        Me.NumericUpDown4.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
        Me.NumericUpDown4.Name = "NumericUpDown4"
        Me.NumericUpDown4.Size = New System.Drawing.Size(48, 20)
        Me.NumericUpDown4.TabIndex = 8
        Me.NumericUpDown4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'NumericUpDown5
        '
        Me.NumericUpDown5.Location = New System.Drawing.Point(380, 64)
        Me.NumericUpDown5.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
        Me.NumericUpDown5.Name = "NumericUpDown5"
        Me.NumericUpDown5.Size = New System.Drawing.Size(48, 20)
        Me.NumericUpDown5.TabIndex = 9
        Me.NumericUpDown5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label5
        '
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(80, 68)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(16, 16)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "d:"
        '
        'Label6
        '
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(148, 68)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(16, 16)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "h:"
        '
        'Label7
        '
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(216, 68)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(20, 16)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "m:"
        '
        'Label8
        '
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(288, 68)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(16, 16)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "s:"
        '
        'Label9
        '
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(356, 68)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(24, 16)
        Me.Label9.TabIndex = 17
        Me.Label9.Text = "ms:"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Navy
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(16, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 16)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Reference:"
        '
        'ComboBox2
        '
        Me.ComboBox2.DisplayMember = "Name"
        Me.ComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox2.Location = New System.Drawing.Point(80, 16)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(148, 21)
        Me.ComboBox2.TabIndex = 24
        Me.ComboBox2.ValueMember = "Name"
        '
        'ComboBox3
        '
        Me.ComboBox3.DisplayMember = "Name"
        Me.ComboBox3.Location = New System.Drawing.Point(424, 16)
        Me.ComboBox3.Name = "ComboBox3"
        Me.ComboBox3.Size = New System.Drawing.Size(76, 21)
        Me.ComboBox3.TabIndex = 25
        Me.ComboBox3.ValueMember = "Name"
        '
        'Label3
        '
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(360, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 16)
        Me.Label3.TabIndex = 26
        Me.Label3.Text = "Indicator:"
        '
        'ActionDialogForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(514, 151)
        Me.Controls.Add(Me.ComboBox3)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ComboBox2)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.NumericUpDown5)
        Me.Controls.Add(Me.NumericUpDown4)
        Me.Controls.Add(Me.NumericUpDown3)
        Me.Controls.Add(Me.NumericUpDown2)
        Me.Controls.Add(Me.NumericUpDown1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ActionDialogForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Azione"
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click
        Me.Dispose()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Qualifier = ComboBox1.Text
        Nome = ComboBox2.Text
        'Se nel ComboBox2 è stata selezionata una variabile la assegna a Var
        If Not IsNothing(ComboBox2.SelectedItem) Then
            Var = ComboBox2.SelectedItem
        Else
            Var = Nothing
        End If
        'Se nel ComboBox3 è stata selezionata una variabile la assegna a Indicator
        If Not IsNothing(ComboBox2.SelectedItem) Then
            Indicator = ComboBox3.SelectedItem
        Else
            Indicator = Nothing
        End If
        Time = New TimeSpan(NumericUpDown1.Value, NumericUpDown2.Value, NumericUpDown3.Value, NumericUpDown4.Value, NumericUpDown5.Value)
    End Sub
    Private Sub SfcDialogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.AcceptButton = Button1
        'Riempie il ComboBox e richiama i dati dell'azione
        Dim i As Integer
        For i = 0 To ActionType.Length - 5 '(c'è -5 per eliminare quelli fuori norma, altrimenti -1)
            ComboBox1.Items.Add(ActionType(i))
        Next i
        If Qualifier <> "" Then
            ComboBox1.Text = Qualifier
        Else
            ComboBox1.Text = "N"
        End If
        FillVariablesList()
        ComboBox2.Text = Nome
        'Se la variabile c'è assegna il nome al ComboBox2
        If Not IsNothing(Var) Then
            ComboBox2.SelectedIndex = ComboBox2.FindString(Var.Name)
        End If
        'Se l'indicatore c'è assegna il nome al ComboBox3
        If Not IsNothing(Indicator) Then
            ComboBox3.SelectedIndex = ComboBox3.FindString(Indicator.Name)
        End If
        NumericUpDown1.Value = Val(Time.Days)
        NumericUpDown2.Value = Val(Time.Hours)
        NumericUpDown3.Value = Val(Time.Minutes)
        NumericUpDown4.Value = Val(Time.Seconds)
        NumericUpDown5.Value = Val(Time.Milliseconds)


        'Disattiva i pulsanti di conferma se non c'e il nome o non è selezionata una variabile o un sfc
        If ComboBox2.Text = "" Then
            Button1.Enabled = False
        Else
            Button1.Enabled = True
        End If

    End Sub
    Private Sub NumericUpDown5_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles NumericUpDown5.KeyUp
        If NumericUpDown5.Value > 999 Then
            NumericUpDown5.Value = 999
        End If
        If NumericUpDown5.Value < 0 Then
            NumericUpDown5.Value = 0
        End If
    End Sub
    Private Sub NumericUpDown4_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles NumericUpDown4.KeyUp
        If NumericUpDown4.Value > 59 Then
            NumericUpDown4.Value = 59
        End If
        If NumericUpDown4.Value < 0 Then
            NumericUpDown4.Value = 0
        End If
    End Sub
    Private Sub NumericUpDown3_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles NumericUpDown3.KeyUp
        If NumericUpDown3.Value > 59 Then
            NumericUpDown3.Value = 59
        End If
        If NumericUpDown3.Value < 0 Then
            NumericUpDown3.Value = 0
        End If
    End Sub
    Private Sub NumericUpDown2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles NumericUpDown2.KeyUp
        If NumericUpDown2.Value > 23 Then
            NumericUpDown2.Value = 23
        End If
        If NumericUpDown2.Value < 0 Then
            NumericUpDown2.Value = 0
        End If
    End Sub
    Private Sub NumericUpDown1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles NumericUpDown1.KeyUp
        If NumericUpDown1.Value > 30 Then
            NumericUpDown1.Value = 30
        End If
        If NumericUpDown1.Value < 0 Then
            NumericUpDown1.Value = 0
        End If
    End Sub
    Private Sub FillVariablesList()
        'Aggiunge le variabili
        For Each L As VariablesList In m_ResGlobalVariables
            For Each V As BaseVariable In L
                ComboBox2.Items.Add(V)
                ComboBox3.Items.Add(V)
            Next V
        Next L
        ComboBox2.Sorted = True
        ComboBox3.Sorted = True
    End Sub
    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        TryEnableOkButton()
    End Sub
    Private Sub TextBox1_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'Limita l'inserimento di caratteri per i nomi di variabili
        Dim NewChar As Char = e.KeyChar
        If Not InvalidVariableChars.IndexOf(e.KeyChar) = -1 Then
            e.Handled = True
        End If
    End Sub
    Private Sub TryEnableOkButton()
        If ComboBox2.Text = "" Then
            Button1.Enabled = False
        Else
            Button1.Enabled = True
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        TryEnableOkButton()
    End Sub

    Private Sub ComboBox2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox2.TextChanged
        TryEnableOkButton()
    End Sub
End Class
