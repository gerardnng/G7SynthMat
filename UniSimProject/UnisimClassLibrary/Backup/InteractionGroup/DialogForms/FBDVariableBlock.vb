Imports System.Windows.Forms
Imports System.Collections.Generic

Public Enum FBDVariableBlockDialogType
    fvbdtFBD
    fvbdtLD
End Enum

Public Class FBDVariableBlockDialog
    Inherits Form
    Public m_qualifier As String
    Public m_name As String
    Public m_variable As BaseVariable
    Public m_indicator As BaseVariable
    Public m_time As TimeSpan
    Private m_ResGlobalVariables As VariablesLists
    Private m_pouInterface As pouInterface
    Public m_VariableType As GraphicalVariableType
    Private m_DialogType As FBDVariableBlockDialogType
    Private m_DialogVariantData As New Hydra(Of FBDVariableBlockDialogType, String, String)

    Friend WithEvents rdbInput As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents nmrExecID As System.Windows.Forms.NumericUpDown
    Friend WithEvents rdbOutput As System.Windows.Forms.RadioButton

    Private Sub FillHydra()
        m_DialogVariantData.AddValueToAllEntries("dialogText", _
            New KeyValuePair(Of FBDVariableBlockDialogType, String)(FBDVariableBlockDialogType.fvbdtFBD, _
"The variable you specified does not exist. Do you want to create it?" + vbCrLf + _
"If you click No, the dialog won't be closed so you can correct the " + vbCrLf + _
"name and try again. Use the ""Select variable"" command if you can't" + vbCrLf + _
"remember the variable you need to use here" + vbCrLf + _
"Even though FBD supports BOOL and INT variable types, the variable" + vbCrLf + _
"shall be created as a BOOL. If you need an INT variable, click no" + vbCrLf + _
" and create the variable yourself"), New KeyValuePair(Of FBDVariableBlockDialogType, String)(FBDVariableBlockDialogType.fvbdtLD, _
"The variable you specified does not exist. Do you want to create it?" + vbCrLf + _
"If you click No, the dialog won't be closed so you can correct the " + vbCrLf + _
"name and try again. Use the ""Select variable"" command if you can't" + vbCrLf + _
"remember the variable you need to use here" + vbCrLf + _
"Even though LD supports BOOL and INT variable types, the variable" + vbCrLf + _
"shall be created as an INT. If you need a BOOL variable, click no" + vbCrLf + _
" and create the variable yourself")).AddValueToAllEntries("valueType", _
    New KeyValuePair(Of FBDVariableBlockDialogType, String)(FBDVariableBlockDialogType.fvbdtFBD, "BOOL"), _
    New KeyValuePair(Of FBDVariableBlockDialogType, String)(FBDVariableBlockDialogType.fvbdtLD, _
        "INT")).AddValueToAllEntries("languageName", New KeyValuePair(Of FBDVariableBlockDialogType, _
            String)(FBDVariableBlockDialogType.fvbdtFBD, "FBD"), _
    New KeyValuePair(Of FBDVariableBlockDialogType, String)(FBDVariableBlockDialogType.fvbdtLD, _
        "Ladder"))
    End Sub


#Region " Codice generato da Progettazione Windows Form "

    Public Sub New(ByRef RefResGlobalm_variableiables As VariablesLists, _
        ByRef RefPouInterface As pouInterface, ByVal dt As FBDVariableBlockDialogType)
        MyBase.New()
        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        Label1.Visible = False
        nmrExecID.Visible = False

        Me.m_DialogType = dt

        Me.FillHydra()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()
        'Assegna l'interfaccia
        m_ResGlobalVariables = RefResGlobalm_variableiables
        m_pouInterface = RefPouInterface

        Macintoshize(Me)
        Me.Text = String.Format(Me.Text, m_DialogVariantData(m_DialogType)("languageName"))

        Me.AcceptButton = Button1

    End Sub

    Public Sub New(ByRef RefResGlobalm_variableiables As VariablesLists, _
        ByRef RefPouInterface As pouInterface, ByVal var As GraphicalVariable, _
            ByVal dt As FBDVariableBlockDialogType)
        Me.New(RefResGlobalm_variableiables, RefPouInterface, dt)
        Me.m_variable = var.BoundVariable
        Me.m_VariableType = var.VariableType
        Me.TextBox1.Text = m_variable.Name
        If m_VariableType = GraphicalVariableType.Input Then
            rdbInput.Checked = True
        Else
            rdbOutput.Checked = True
        End If
        Me.nmrExecID.Value = CDec(var.ExecutionOrderID)
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
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Button5 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Button5 = New System.Windows.Forms.Button
        Me.rdbInput = New System.Windows.Forms.RadioButton
        Me.rdbOutput = New System.Windows.Forms.RadioButton
        Me.Label1 = New System.Windows.Forms.Label
        Me.nmrExecID = New System.Windows.Forms.NumericUpDown
        CType(Me.nmrExecID, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.ForeColor = System.Drawing.Color.Black
        Me.Button1.Location = New System.Drawing.Point(149, 135)
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
        Me.Button2.Location = New System.Drawing.Point(229, 135)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(76, 32)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Navy
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(12, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 16)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Reference:"
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.Color.White
        Me.TextBox1.Location = New System.Drawing.Point(82, 12)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(242, 20)
        Me.TextBox1.TabIndex = 1
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.SystemColors.Control
        Me.Button5.ForeColor = System.Drawing.Color.Black
        Me.Button5.Location = New System.Drawing.Point(330, 11)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(96, 20)
        Me.Button5.TabIndex = 31
        Me.Button5.Text = "select variable..."
        Me.Button5.UseVisualStyleBackColor = False
        '
        'rdbInput
        '
        Me.rdbInput.AutoSize = True
        Me.rdbInput.Checked = True
        Me.rdbInput.Location = New System.Drawing.Point(116, 52)
        Me.rdbInput.Name = "rdbInput"
        Me.rdbInput.Size = New System.Drawing.Size(89, 17)
        Me.rdbInput.TabIndex = 32
        Me.rdbInput.TabStop = True
        Me.rdbInput.Text = "Input variable"
        Me.rdbInput.UseVisualStyleBackColor = True
        '
        'rdbOutput
        '
        Me.rdbOutput.AutoSize = True
        Me.rdbOutput.Location = New System.Drawing.Point(116, 75)
        Me.rdbOutput.Name = "rdbOutput"
        Me.rdbOutput.Size = New System.Drawing.Size(97, 17)
        Me.rdbOutput.TabIndex = 33
        Me.rdbOutput.Text = "Output variable"
        Me.rdbOutput.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 112)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(98, 13)
        Me.Label1.TabIndex = 34
        Me.Label1.Text = "Execution order ID:"
        '
        'nmrExecID
        '
        Me.nmrExecID.Location = New System.Drawing.Point(116, 105)
        Me.nmrExecID.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nmrExecID.Name = "nmrExecID"
        Me.nmrExecID.Size = New System.Drawing.Size(208, 20)
        Me.nmrExecID.TabIndex = 35
        Me.nmrExecID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nmrExecID.ThousandsSeparator = True
        '
        'FBDVariableBlockDialog
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(434, 179)
        Me.Controls.Add(Me.nmrExecID)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.rdbOutput)
        Me.Controls.Add(Me.rdbInput)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label2)
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FBDVariableBlockDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "{0} Variable"
        CType(Me.nmrExecID, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click
        Me.Dispose()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If IsNothing(m_variable) AndAlso TextBox1.Text <> "" Then
            Dim dlgOut As Windows.Forms.DialogResult = _
                MessageBox.Show(m_DialogVariantData(m_DialogType)("dialogText"), _
UniSimVersion.VersionInfo.PrintableDescriptionForTool(""), _
 MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If dlgOut = Windows.Forms.DialogResult.No Then Exit Sub
            Dim varType As String = m_DialogVariantData(m_DialogType)("valueType")
            m_variable = _
                m_pouInterface.localVars.CreateAndAddVariable(TextBox1.Text, "", "", _
                    VariablesManager.DefaultValue(varType), varType)
        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Hide()
    End Sub
    Private Sub m_variableiablesSelectorDialogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.AcceptButton = Button1
        TextBox1.Focus()

        If TextBox1.Text = "" Then
            Button1.Enabled = False
        Else
            Button1.Enabled = True
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
        Button1.Enabled = (TextBox1.Text <> "") AndAlso IsValidVariableName(TextBox1.Text)
        If Button1.Enabled Then
            m_variable = m_pouInterface.Variables(TextBox1.Text)
            If IsNothing(m_variable) Then
                m_variable = m_ResGlobalVariables.FindVariableByName(TextBox1.Text)
            End If
        End If
        ' Button1.Enabled = (m_variable IsNot Nothing)
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        TryEnableOkButton()
    End Sub

    Private Sub ComboBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        TryEnableOkButton()
    End Sub

    Public ReadOnly Property ExecutionOrderID() As Integer
        Get
            Return CInt(nmrExecID.Value)
        End Get
    End Property

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim VariablesSelectorDialog As New VariablesSelectorDialogForm(m_ResGlobalVariables, m_pouInterface)
        Dim ResultDialog As System.Windows.Forms.DialogResult = VariablesSelectorDialog.ShowDialog(Me)
        If ResultDialog = Windows.Forms.DialogResult.OK Then
            m_variable = VariablesSelectorDialog.m_SelectedVar
            TextBox1.Text = m_variable.Name
        End If
        TryEnableOkButton()
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        Call TryEnableOkButton()
    End Sub

    Private Sub rdbInput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbInput.Click
        If rdbInput.Checked Then m_VariableType = GraphicalVariableType.Input
    End Sub

    Private Sub rdbOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbOutput.Click
        If rdbOutput.Checked Then m_VariableType = GraphicalVariableType.Output
    End Sub

    Private Sub rdbOutput_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbOutput.CheckedChanged
        If Me.m_DialogType = FBDVariableBlockDialogType.fvbdtFBD Then
            Label1.Visible = rdbOutput.Checked
            nmrExecID.Visible = rdbOutput.Checked
        Else
            Label1.Visible = False
            nmrExecID.Visible = False
        End If
    End Sub
End Class
