Public Class ActionDialogForm
    Inherits System.Windows.Forms.Form
    Public m_qualifier As String
    Public m_type As String
    Public m_name As String
    Public m_actvalue As String
    Public m_action As String
    Public m_variable As BaseVariable
    Public m_indicator As BaseVariable
    Public m_time As TimeSpan
    Private m_ResGlobalVariables As VariablesLists
    Private m_pouInterface As pouInterface



    '----------------------------
    'Blocco aggiuntivo alla norma
    Private m_pousList As Pous
    Public RefSfc As Sfc
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Public StepsToForces As GraphicalStepsList
    Private thisSfc As String
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    '----------------------------
    Public ArithExp As String = ""


#Region " Codice generato da Progettazione Windows Form "

    Public Sub New(ByRef RefResGlobalm_variableiables As VariablesLists, ByRef RefPouInterface As pouInterface, ByRef pousList As Pous, ByRef thisSfcName As String)
        MyBase.New()
        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()
        'Assegna l'interfaccia
        m_ResGlobalVariables = RefResGlobalm_variableiables
        m_pouInterface = RefPouInterface
        m_pousList = pousList
        thisSfc = thisSfcName

        Macintoshize(Me)

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
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents ComboBox3 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox4 As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Button8 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ActionDialogForm))
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
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.Button5 = New System.Windows.Forms.Button
        Me.Button6 = New System.Windows.Forms.Button
        Me.ComboBox2 = New System.Windows.Forms.ComboBox
        Me.ComboBox3 = New System.Windows.Forms.ComboBox
        Me.ComboBox4 = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.Button7 = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Button8 = New System.Windows.Forms.Button
        Me.Button9 = New System.Windows.Forms.Button
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(12, 56)
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
        Me.Button1.Location = New System.Drawing.Point(128, 408)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(76, 32)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Ok"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.Control
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.ForeColor = System.Drawing.Color.Black
        Me.Button2.Location = New System.Drawing.Point(224, 408)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(76, 32)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.Location = New System.Drawing.Point(72, 52)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(56, 21)
        Me.ComboBox1.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(16, 368)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 16)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "m_timer:"
        '
        'NumericUpDown1
        '
        Me.NumericUpDown1.Location = New System.Drawing.Point(88, 368)
        Me.NumericUpDown1.Maximum = New Decimal(New Integer() {30, 0, 0, 0})
        Me.NumericUpDown1.Name = "NumericUpDown1"
        Me.NumericUpDown1.Size = New System.Drawing.Size(48, 20)
        Me.NumericUpDown1.TabIndex = 5
        Me.NumericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'NumericUpDown2
        '
        Me.NumericUpDown2.Location = New System.Drawing.Point(160, 368)
        Me.NumericUpDown2.Maximum = New Decimal(New Integer() {23, 0, 0, 0})
        Me.NumericUpDown2.Name = "NumericUpDown2"
        Me.NumericUpDown2.Size = New System.Drawing.Size(48, 20)
        Me.NumericUpDown2.TabIndex = 6
        Me.NumericUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'NumericUpDown3
        '
        Me.NumericUpDown3.Location = New System.Drawing.Point(232, 368)
        Me.NumericUpDown3.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
        Me.NumericUpDown3.Name = "NumericUpDown3"
        Me.NumericUpDown3.Size = New System.Drawing.Size(48, 20)
        Me.NumericUpDown3.TabIndex = 7
        Me.NumericUpDown3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'NumericUpDown4
        '
        Me.NumericUpDown4.Location = New System.Drawing.Point(296, 368)
        Me.NumericUpDown4.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
        Me.NumericUpDown4.Name = "NumericUpDown4"
        Me.NumericUpDown4.Size = New System.Drawing.Size(48, 20)
        Me.NumericUpDown4.TabIndex = 8
        Me.NumericUpDown4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'NumericUpDown5
        '
        Me.NumericUpDown5.Location = New System.Drawing.Point(376, 368)
        Me.NumericUpDown5.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
        Me.NumericUpDown5.Name = "NumericUpDown5"
        Me.NumericUpDown5.Size = New System.Drawing.Size(48, 20)
        Me.NumericUpDown5.TabIndex = 9
        Me.NumericUpDown5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label5
        '
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(72, 368)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(16, 16)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "d:"
        '
        'Label6
        '
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(144, 368)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(16, 16)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "h:"
        '
        'Label7
        '
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(208, 368)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(20, 16)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "m:"
        '
        'Label8
        '
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(280, 368)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(16, 16)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "s:"
        '
        'Label9
        '
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(352, 368)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(24, 16)
        Me.Label9.TabIndex = 17
        Me.Label9.Text = "ms:"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Navy
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(12, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 16)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Reference:"
        '
        'Label3
        '
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(136, 56)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 16)
        Me.Label3.TabIndex = 26
        Me.Label3.Text = "m_indicator:"
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.Color.White
        Me.TextBox1.Location = New System.Drawing.Point(72, 16)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(244, 20)
        Me.TextBox1.TabIndex = 27
        '
        'TextBox2
        '
        Me.TextBox2.BackColor = System.Drawing.Color.White
        Me.TextBox2.Location = New System.Drawing.Point(192, 52)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(120, 20)
        Me.TextBox2.TabIndex = 28
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.SystemColors.Control
        Me.Button5.ForeColor = System.Drawing.Color.Black
        Me.Button5.Location = New System.Drawing.Point(328, 16)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(96, 20)
        Me.Button5.TabIndex = 31
        Me.Button5.Text = "select variable..."
        Me.Button5.UseVisualStyleBackColor = False
        '
        'Button6
        '
        Me.Button6.BackColor = System.Drawing.SystemColors.Control
        Me.Button6.ForeColor = System.Drawing.Color.Black
        Me.Button6.Location = New System.Drawing.Point(328, 52)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(96, 20)
        Me.Button6.TabIndex = 32
        Me.Button6.Text = "select variable..."
        Me.Button6.UseVisualStyleBackColor = False
        '
        'ComboBox2
        '
        Me.ComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox2.Items.AddRange(New Object() {"Increase", "Decrease", "Reset", "Operation"})
        Me.ComboBox2.Location = New System.Drawing.Point(192, 51)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox2.TabIndex = 33
        Me.ComboBox2.Visible = False
        '
        'ComboBox3
        '
        Me.ComboBox3.DisplayMember = "Name"
        Me.ComboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox3.Enabled = False
        Me.ComboBox3.Location = New System.Drawing.Point(64, 128)
        Me.ComboBox3.Name = "ComboBox3"
        Me.ComboBox3.Size = New System.Drawing.Size(96, 21)
        Me.ComboBox3.TabIndex = 46
        Me.ComboBox3.ValueMember = "Name"
        '
        'ComboBox4
        '
        Me.ComboBox4.DisplayMember = "name"
        Me.ComboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox4.Enabled = False
        Me.ComboBox4.Location = New System.Drawing.Point(64, 96)
        Me.ComboBox4.Name = "ComboBox4"
        Me.ComboBox4.Size = New System.Drawing.Size(247, 21)
        Me.ComboBox4.TabIndex = 47
        Me.ComboBox4.ValueMember = "name"
        '
        'Label10
        '
        Me.Label10.ForeColor = System.Drawing.Color.White
        Me.Label10.Location = New System.Drawing.Point(24, 96)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(32, 16)
        Me.Label10.TabIndex = 48
        Me.Label10.Text = "POU"
        '
        'Label11
        '
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.Location = New System.Drawing.Point(24, 128)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(36, 16)
        Me.Label11.TabIndex = 49
        Me.Label11.Text = "Steps:"
        '
        'ListBox1
        '
        Me.ListBox1.DisplayMember = "Name"
        Me.ListBox1.Location = New System.Drawing.Point(216, 184)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(188, 147)
        Me.ListBox1.TabIndex = 53
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(16, 192)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(52, 16)
        Me.Label12.TabIndex = 52
        Me.Label12.Text = "Step List:"
        '
        'Button7
        '
        Me.Button7.BackColor = System.Drawing.SystemColors.Control
        Me.Button7.ForeColor = System.Drawing.Color.Black
        Me.Button7.Location = New System.Drawing.Point(40, 272)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(136, 23)
        Me.Button7.TabIndex = 51
        Me.Button7.Text = "REMOVE selected Step"
        Me.Button7.UseVisualStyleBackColor = False
        '
        'Button4
        '
        Me.Button4.BackColor = System.Drawing.SystemColors.Control
        Me.Button4.ForeColor = System.Drawing.Color.Black
        Me.Button4.Location = New System.Drawing.Point(40, 224)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(136, 23)
        Me.Button4.TabIndex = 50
        Me.Button4.Text = "ADD selected Step -->"
        Me.Button4.UseVisualStyleBackColor = False
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(328, 88)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(96, 20)
        Me.Button3.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Button8)
        Me.Panel1.Controls.Add(Me.Button9)
        Me.Panel1.Controls.Add(Me.TextBox4)
        Me.Panel1.Controls.Add(Me.Label15)
        Me.Panel1.Location = New System.Drawing.Point(19, 93)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(406, 86)
        Me.Panel1.TabIndex = 55
        Me.Panel1.Visible = False
        '
        'Button8
        '
        Me.Button8.BackColor = System.Drawing.SystemColors.Control
        Me.Button8.ForeColor = System.Drawing.Color.Black
        Me.Button8.Location = New System.Drawing.Point(18, 58)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(82, 23)
        Me.Button8.TabIndex = 11
        Me.Button8.Text = "validate"
        Me.Button8.UseVisualStyleBackColor = False
        Me.Button8.Visible = False
        '
        'Button9
        '
        Me.Button9.BackColor = System.Drawing.SystemColors.Control
        Me.Button9.ForeColor = System.Drawing.Color.Black
        Me.Button9.Location = New System.Drawing.Point(326, 28)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(72, 33)
        Me.Button9.TabIndex = 10
        Me.Button9.Text = "add variable"
        Me.Button9.UseVisualStyleBackColor = False
        '
        'TextBox4
        '
        Me.TextBox4.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox4.Location = New System.Drawing.Point(17, 32)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(277, 22)
        Me.TextBox4.TabIndex = 9
        '
        'Label15
        '
        Me.Label15.ForeColor = System.Drawing.Color.White
        Me.Label15.Location = New System.Drawing.Point(11, 11)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(72, 16)
        Me.Label15.TabIndex = 8
        Me.Label15.Text = "Expression"
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink
        Me.ErrorProvider1.ContainerControl = Me
        '
        'ActionDialogForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(434, 456)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.ComboBox3)
        Me.Controls.Add(Me.ComboBox4)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label3)
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
        Me.Controls.Add(Me.ComboBox2)
        Me.Controls.Add(Me.TextBox2)
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
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click
        Me.Dispose()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        m_qualifier = ComboBox1.Text
        m_name = TextBox1.Text
        If m_type = "INT" Or m_type = "REAL" Then
            Select Case ComboBox2.Text
                Case "Increase"
                    m_qualifier = "PI"
                Case "Decrease"
                    m_qualifier = "PD"
                Case "Reset"
                    m_qualifier = "PR"
                Case "Operation"
                    m_qualifier = "PO"
                    ArithExp = TextBox4.Text
            End Select
        End If


        If ComboBox4.Enabled = True Then
            Try
                RefSfc = ComboBox4.SelectedItem.Body.readSfc
            Catch ex As System.NullReferenceException
                MsgBox("POU not found or not selected. Cannot proceed", MsgBoxStyle.Critical, _
                    UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
                Me.Show()
            Finally
                'cancella la variabile
                m_variable = Nothing
            End Try
        End If
        If ComboBox3.Enabled = True Then
            Try
                'StepsToForces.Clear()
                StepsToForces = New GraphicalStepsList
                If ListBox1.Items.Count = 0 Then 'a
                    StepsToForces.Add(ComboBox3.SelectedItem)
                Else 'a
                    For Each S As GraphicalStep In ListBox1.Items 'a
                        StepsToForces.Add(S) 'a
                    Next 'a
                End If 'a
            Catch ex As System.NullReferenceException
                MsgBox("Step not found or not selected. Cannot proceed", MsgBoxStyle.Critical, _
                    UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
                Me.Show()
            End Try
        End If

        Try
            If ComboBox1.Text = "sus" Then
                m_name = RefSfc.Name & " : {}"
            ElseIf ComboBox1.Text = "st" Then
                m_name = RefSfc.Name & " : {*}"
            ElseIf (ComboBox1.Text = "fr" Or ComboBox1.Text = "ifr") And ListBox1.Items.Count = 0 Then 'And listbox1.items.count=0
                m_name = RefSfc.Name & " : {" & StepsToForces.Item(0).Name & "}"
            ElseIf (ComboBox1.Text = "fr" Or ComboBox1.Text = "ifr") And ListBox1.Items.Count > 0 Then 'a
                m_name = RefSfc.Name & " : {" 'a
                Dim n As Integer = ListBox1.Items.Count 'a
                Dim i As Integer 'a
                For i = 0 To (n - 1) 'a
                    m_name = m_name & StepsToForces.Item(i).Name 'a
                    If i < n - 1 Then
                        m_name = m_name & ";"
                    End If
                Next 'a
                m_name = m_name & "}" 'a
            End If
        Catch ex As System.NullReferenceException
        End Try



        m_time = New TimeSpan(NumericUpDown1.Value, NumericUpDown2.Value, NumericUpDown3.Value, NumericUpDown4.Value, NumericUpDown5.Value)
    End Sub
    Private Sub m_variableiablesSelectorDialogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.AcceptButton = Button1
        'Riempie il ComboBox e richiama i dati dell'azione
        Dim i As Integer
        For i = 0 To ActionType.Length - 1 '(c'è -5 per eliminare quelli fuori norma, altrimenti -1)
            ComboBox1.Items.Add(ActionType(i))
        Next i
        For Each P As pou In m_pousList
            ' Se è una POU SFC e non è la stessa POU in cui metteremo l'azione
            If P.Name <> thisSfc AndAlso P.Body.ReadBodyType = EnumBodyType.tSFC Then
                ComboBox4.Items.Add(P)
            End If

        Next P
        If m_qualifier <> "" Then
            ComboBox1.Text = m_qualifier
        Else
            ComboBox1.Text = "N"
        End If
        'Se la m_variableiabile c'è assegna il m_Name al TextBox1
        If Not IsNothing(m_variable) Then
            TextBox1.Text = m_variable.Name
            m_type = m_variable.dataType
            CheckType()
        End If
        'Se l'm_indicatore c'è assegna il m_Name al TextBox2
        If Not IsNothing(m_indicator) Then
            TextBox2.Text = m_indicator.Name
        End If
        'Se RefSfc c'è, assegna il m_Name al ComboBox3
        If Not IsNothing(RefSfc) Then
            ComboBox4.Text = RefSfc.Name
        End If
        'Se StepsToForces c'è, assegna il m_Name al ComboBox2 o alla ListBox1
        If Not IsNothing(StepsToForces) Then
            If StepsToForces.Count = 1 Then 'aggiusta
                ComboBox3.Text = StepsToForces.Item(0).Name
            ElseIf StepsToForces.Count > 1 Then
                For Each S As GraphicalStep In StepsToForces
                    ListBox1.Items.Add(S)
                Next
            End If
        End If
        If m_action <> "" Then
            ComboBox2.Text = m_action
        Else
            ComboBox2.Text = "Increase"
        End If
        If ArithExp <> "" Then
            ComboBox2.Text = "Operation"
            ComboBox1.Text = "P"
            TextBox4.Text = ArithExp
            Panel1.Enabled = True
        End If
        NumericUpDown1.Value = Val(m_time.Days)
        NumericUpDown2.Value = Val(m_time.Hours)
        NumericUpDown3.Value = Val(m_time.Minutes)
        NumericUpDown4.Value = Val(m_time.Seconds)
        NumericUpDown5.Value = Val(m_time.Milliseconds)

        'Disattiva i pulsanti di conferma se non c'e il m_Name o non è selezionata una m_variableiabile o un sfc
        If TextBox1.Text = "" Then
            Button1.Enabled = False
        Else
            Button1.Enabled = True
        End If
        If ComboBox1.Text = "fr" Or ComboBox1.Text = "ifr" Or ComboBox1.Text = "sus" Or ComboBox1.Text = "st" Then
            If ComboBox4.Text <> "" Then
                Button1.Enabled = True
            End If
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
    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        TryEnableOkButton()
    End Sub
    Private Sub TextBox1_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'Limita l'inserimento di caratteri per i nomi di m_variableiabili
        Dim NewChar As Char = e.KeyChar
        If Not InvalidVariableChars.IndexOf(e.KeyChar) = -1 Then
            e.Handled = True
        End If
    End Sub
    Private Sub TryEnableOkButton()
        ErrorProvider1.Clear()
        Button1.Enabled = False
        If TextBox1.Text <> "" Then
            m_variable = m_pouInterface.Variables(TextBox1.Text)
            If Not IsNothing(m_variable) Then
                Button1.Enabled = True
            Else
                m_variable = m_ResGlobalVariables.FindVariableByName(TextBox1.Text)
                If Not IsNothing(m_variable) Then Button1.Enabled = True
            End If
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        TryEnableOkButton()
    End Sub

    Private Sub ComboBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        TryEnableOkButton()
    End Sub

    Private Sub CopyVariableDetails(ByVal var As BaseVariable, Optional ByVal toTextField As Boolean = False)
        m_variable = var
        m_type = var.dataType
        m_actvalue = var.ValueToUniversalString()
        If toTextField Then TextBox1.Text = var.Name
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim VariablesSelectorDialog As New VariablesSelectorDialogForm(m_ResGlobalVariables, m_pouInterface)
        Dim ResultDialog As System.Windows.Forms.DialogResult = VariablesSelectorDialog.ShowDialog(Me)
        If ResultDialog = Windows.Forms.DialogResult.OK Then _
            CopyVariableDetails(VariablesSelectorDialog.m_SelectedVar, True)
        CheckType()
        TryEnableOkButton()
    End Sub
    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Dim VariablesSelectorDialog As New VariablesSelectorDialogForm(m_ResGlobalVariables, m_pouInterface)
        Dim ResultDialog As System.Windows.Forms.DialogResult = VariablesSelectorDialog.ShowDialog()
        If ResultDialog = Windows.Forms.DialogResult.OK Then
            m_indicator = VariablesSelectorDialog.m_SelectedVar
            TextBox2.Text = m_indicator.Name
        End If
        TryEnableOkButton()
    End Sub
    Public Sub CheckType()
        Select Case m_type
            Case "INT", "REAL"
                Button6.Visible = False
                Button4.Visible = False
                Button7.Visible = False
                NumericUpDown1.Visible = False
                NumericUpDown2.Visible = False
                NumericUpDown3.Visible = False
                NumericUpDown4.Visible = False
                NumericUpDown5.Visible = False
                Label10.Visible = False
                Label11.Visible = False
                Label12.Visible = False
                Label4.Visible = False
                Label5.Visible = False
                Label6.Visible = False
                Label7.Visible = False
                Label8.Visible = False
                Label9.Visible = False
                ComboBox4.Visible = False
                ComboBox3.Visible = False
                ListBox1.Visible = False
                Label3.Text = "Action:"
                ComboBox1.SelectedItem = "P"
                ComboBox1.Enabled = False
                ComboBox2.Visible = True
                Panel1.Visible = True
                Panel1.Enabled = False
                TextBox4.Text = ""
                ComboBox2.Text = "Increase"
            Case "BOOL"
                Button6.Visible = True
                Button4.Visible = True
                Button7.Visible = True
                NumericUpDown1.Visible = True
                NumericUpDown2.Visible = True
                NumericUpDown3.Visible = True
                NumericUpDown4.Visible = True
                NumericUpDown5.Visible = True
                Label4.Visible = True
                Label5.Visible = True
                Label6.Visible = True
                Label7.Visible = True
                Label8.Visible = True
                Label9.Visible = True
                Label10.Visible = True
                Label11.Visible = True
                Label12.Visible = True
                Label3.Text = "m_indicator:"
                ComboBox1.Enabled = True
                ComboBox2.Visible = False
                ComboBox4.Visible = True
                ComboBox3.Visible = True
                ListBox1.Visible = True
                Panel1.Visible = False


        End Select
    End Sub

    'attiva o disattiva gli elementi a seconda del tipo di azione scelto

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged

        If ComboBox1.Text = "sus" Or ComboBox1.Text = "st" Then
            ComboBox4.Enabled = True
            ComboBox3.Enabled = False
            TextBox1.Text = ""
            TextBox1.Enabled = False
            Button5.Enabled = False
            Button4.Enabled = False
            Button7.Enabled = False
            ListBox1.Enabled = False
        ElseIf ComboBox1.Text = "fr" Or ComboBox1.Text = "ifr" Then
            ComboBox4.Enabled = True
            ComboBox3.Enabled = True
            TextBox1.Text = ""
            TextBox1.Enabled = False
            Button5.Enabled = False
            Button4.Enabled = True
            Button7.Enabled = True
            ListBox1.Enabled = True
        Else
            ComboBox4.Enabled = False
            ComboBox3.Enabled = False
            TextBox1.Enabled = True
            Button5.Enabled = True
            Button4.Enabled = False
            Button7.Enabled = False
            ListBox1.Enabled = False
        End If

    End Sub

    Private Sub ComboBox4_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox4.SelectedIndexChanged

        Button1.Enabled = True
        If ComboBox1.Text = "fr" Or ComboBox1.Text = "ifr" Then
            ComboBox3.Items.Clear()

            For Each S As GraphicalStep In ComboBox4.SelectedItem.Body.readSfc.GraphicalStepsList

                ComboBox3.Items.Add(S)
            Next S
        End If
    End Sub

   

    Private Sub Button4_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Try
            If Not ListBox1.Items.Contains(ComboBox3.SelectedItem) Then
                ListBox1.Items.Add(ComboBox3.SelectedItem)
            Else
                MsgBox("Step already selected", MsgBoxStyle.Critical, _
                    UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
            End If
        Catch ex As System.ArgumentNullException
            MsgBox("Step not selected", MsgBoxStyle.Critical, _
                UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
            Me.Show()
        End Try
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        If IsNothing(ListBox1.SelectedItem) Then
            MsgBox("Step not selected", MsgBoxStyle.Critical, _
                UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
        End If
        ListBox1.Items.Remove(ListBox1.SelectedItem)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        ' cambia la variabile
        Call TryEnableOkButton()
        ' esegui il riconoscimento del tipo se c'è qualcosa di valido
        If Button1.Enabled Then
            Call CopyVariableDetails(m_variable, False)
            Call CheckType()
            Call TryEnableOkButton() ' ripeti l'abilitazione...forse è inutile
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.Text = "Operation" Then
            Panel1.Enabled = True
            Button1.Enabled = False
        Else
            Panel1.Enabled = False
            Button1.Enabled = True
            TextBox4.Text = ""
        End If
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        Dim VariablesSelectorDialog As New VariablesSelectorDialogForm(m_ResGlobalVariables, m_pouInterface)
        Dim ResultDialog As System.Windows.Forms.DialogResult = VariablesSelectorDialog.ShowDialog(Me)
        If ResultDialog = Windows.Forms.DialogResult.OK Then
            If (VariablesSelectorDialog.m_Type = "BOOL") Then
                Button1.Enabled = False
                MsgBox(VariablesSelectorDialog.m_SelectedVar.Name & " is not a numeric (INT or REAL) variable", MsgBoxStyle.Critical, _
                    UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
                Exit Sub
            End If
            TextBox4.Text += VariablesSelectorDialog.m_SelectedVar.Name
        End If
    End Sub

    ' Esegue la validazione di una espressione aritmetica
    Private Sub ArithExpValidate()
        ErrorProvider1.Clear()
        Dim arithExp As ArithmeticExpression = New ArithmeticExpression(m_ResGlobalVariables, m_pouInterface)
        Try
            If (arithExp.Parse(TextBox4.Text)) Then
                arithExp.calculateExp(TextBox4.Text)
                Button1.Enabled = True
            Else
                ErrorProvider1.SetError(TextBox4, arithExp.m_Error)
                Button1.Enabled = False
            End If
        Catch ex As Exception
            ' se non sono errori presumibilmente dovuti a variabili non
            ' ancora settate...
            If Not (TypeOf (ex) Is DivideByZeroException OrElse _
                TypeOf (ex) Is OverflowException) Then
                Button1.Enabled = False
                ErrorProvider1.SetError(TextBox4, ex.Message)
            Else
                Button1.Enabled = True
            End If
        End Try
        ' Se l'espressione è valida, controlla che lo sia anche il target
        If Button1.Enabled Then _
            Call TryEnableOkButton()
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        Call ArithExpValidate()
    End Sub

    ' La validazione esplicita (cliccando su "Validate") non è più prevista
    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged
        Call ArithExpValidate()
    End Sub
End Class
