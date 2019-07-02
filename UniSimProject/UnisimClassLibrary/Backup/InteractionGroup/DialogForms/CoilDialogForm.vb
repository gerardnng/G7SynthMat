Imports System.Windows.Forms

Public Class CoilDialogForm
    Inherits Form
    Public m_qualifier As String
    Public m_name As String
    Public m_variable As BaseVariable
    Public m_indicator As BaseVariable
    Public m_time As TimeSpan
    Private m_ResGlobalVariables As VariablesLists
    Private m_pouInterface As pouInterface
    Public m_CoilName As String


    '----------------------------
    'Blocco aggiuntivo alla norma
    Public RefSfc As Sfc
    Public StepsToForces As GraphicalStepsList
    '----------------------------


#Region " Codice generato da Progettazione Windows Form "

    Public Sub New(ByRef RefResGlobalm_variableiables As VariablesLists, ByRef RefPouInterface As pouInterface)
        MyBase.New()
        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()
        'Assegna l'interfaccia
        m_ResGlobalVariables = RefResGlobalm_variableiables
        m_pouInterface = RefPouInterface

        Macintoshize(Me)

        Me.AcceptButton = Button1

    End Sub

    Public Sub New(ByRef RefResGlobalm_variableiables As VariablesLists, ByRef RefPouInterface As pouInterface, ByVal coil As BaseGraphicalContact)
        Me.New(RefResGlobalm_variableiables, RefPouInterface)
        m_CoilName = coil.Name
        'TextBox2.Text = m_ContactName
        m_variable = coil.ReadVar()
        'TextBox1.Text = m_variable.Name
        m_qualifier = coil.Qualy
        'ComboBox1.SelectedIndex = ComboBox1.Items.IndexOf(m_qualifier)
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
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Button5 = New System.Windows.Forms.Button
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(8, 96)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 16)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Qualifier:"
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.ForeColor = System.Drawing.Color.Black
        Me.Button1.Location = New System.Drawing.Point(144, 136)
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
        Me.Button2.Location = New System.Drawing.Point(224, 136)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(76, 32)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.Location = New System.Drawing.Point(72, 88)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(244, 21)
        Me.ComboBox1.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Navy
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(8, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 16)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Reference:"
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.Color.White
        Me.TextBox1.Location = New System.Drawing.Point(72, 56)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(244, 20)
        Me.TextBox1.TabIndex = 1
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.SystemColors.Control
        Me.Button5.ForeColor = System.Drawing.Color.Black
        Me.Button5.Location = New System.Drawing.Point(328, 56)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(96, 20)
        Me.Button5.TabIndex = 31
        Me.Button5.Text = "select variable..."
        Me.Button5.UseVisualStyleBackColor = False
        '
        'TextBox2
        '
        Me.TextBox2.BackColor = System.Drawing.Color.White
        Me.TextBox2.Location = New System.Drawing.Point(74, 24)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(244, 20)
        Me.TextBox2.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Navy
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(8, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 16)
        Me.Label3.TabIndex = 34
        Me.Label3.Text = "Name:"
        '
        'CoilDialogForm2
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(434, 179)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CoilDialogForm2"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Coil"
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
        m_CoilName = TextBox2.Text
        If IsNothing(m_variable) AndAlso TextBox1.Text <> "" Then
            Dim dlgOut As Windows.Forms.DialogResult = _
                MessageBox.Show( _
"The variable you specified does not exist. Do you want to create it?" + vbCrLf + _
"If you click No, the dialog won't be closed so you can correct the " + vbCrLf + _
"name and try again. Use the ""Select variable"" command if you can't" + vbCrLf + _
"remember the variable you need to use here", UniSimVersion.VersionInfo.PrintableDescriptionForTool(""), _
 MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If dlgOut = Windows.Forms.DialogResult.No Then Exit Sub
            m_variable = _
                m_pouInterface.localVars.CreateAndAddVariable(TextBox1.Text, "", "", _
                    "false", "BOOL")
        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Hide()
        ' m_time = New TimeSpan(NumericUpDown1.Value, NumericUpDown2.Value, NumericUpDown3.Value, NumericUpDown4.Value, NumericUpDown5.Value)
    End Sub
    Private Sub m_variableiablesSelectorDialogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.AcceptButton = Button1
        'Riempie il ComboBox e richiama i dati dell'azione
        Dim i As Integer
        For i = 0 To CoilType.Length - 1 '(c'è -5 per eliminare quelli fuori norma, altrimenti -1)
            ComboBox1.Items.Add(CoilType(i))
        Next i

        If m_qualifier <> "" Then
            ComboBox1.Text = m_qualifier
        Else
            ComboBox1.Text = "Normal Coil"
        End If

        TextBox2.Text = m_CoilName
        TextBox1.Focus()

        'Se la m_variableiabile c'è assegna il m_Name al TextBox1
        If Not IsNothing(m_variable) Then
            TextBox1.Text = m_variable.Name
        End If
        'Se l'm_indicatore c'è assegna il m_Name al TextBox2
        If Not IsNothing(m_indicator) Then
            TextBox2.Text = m_indicator.Name
        End If
        ' NumericUpDown1.Value = Val(m_time.Days)
        ' NumericUpDown2.Value = Val(m_time.Hours)
        ' NumericUpDown3.Value = Val(m_time.Minutes)
        ' NumericUpDown4.Value = Val(m_time.Seconds)
        '  NumericUpDown5.Value = Val(m_time.Milliseconds)

        'Disattiva i pulsanti di conferma se non c'e il m_Name o non è selezionata una m_variableiabile o un sfc

        If TextBox1.Text = "" Or TextBox2.Text = "" Then
            Button1.Enabled = False
        Else
            Button1.Enabled = True
        End If

    End Sub
    Private Sub NumericUpDown5_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        '  If NumericUpDown5.Value > 999 Then
        '       NumericUpDown5.Value = 999
        '   End If
        '   If NumericUpDown5.Value < 0 Then
        '   NumericUpDown5.Value = 0
        '   End If
    End Sub
    Private Sub NumericUpDown4_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        '    If NumericUpDown4.Value > 59 Then
        '    NumericUpDown4.Value = 59
        '    End If
        '     If NumericUpDown4.Value < 0 Then
        '     NumericUpDown4.Value = 0
        '     End If
    End Sub
    Private Sub NumericUpDown3_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        '    If NumericUpDown3.Value > 59 Then
        '    NumericUpDown3.Value = 59
        '    End If
        '    If NumericUpDown3.Value < 0 Then
        '    NumericUpDown3.Value = 0
        '    End If
    End Sub
    Private Sub NumericUpDown2_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        '      If NumericUpDown2.Value > 23 Then
        '     NumericUpDown2.Value = 23
        '     End If
        '     If NumericUpDown2.Value < 0 Then
        '     NumericUpDown2.Value = 0
        '     End If
    End Sub
    Private Sub NumericUpDown1_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        '     If NumericUpDown1.Value > 30 Then
        '    NumericUpDown1.Value = 30
        '    End If
        '    If NumericUpDown1.Value < 0 Then
        '    NumericUpDown1.Value = 0
        '    End If
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
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        TryEnableOkButton()
    End Sub

    Private Sub ComboBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        TryEnableOkButton()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim VariablesSelectorDialog As New VariablesSelectorDialogForm(m_ResGlobalVariables, m_pouInterface)
        Dim ResultDialog As System.Windows.Forms.DialogResult = VariablesSelectorDialog.ShowDialog(Me)
        If ResultDialog = Windows.Forms.DialogResult.OK Then
            m_variable = VariablesSelectorDialog.m_SelectedVar
            TextBox1.Text = m_variable.Name
        End If
        TryEnableOkButton()
    End Sub
    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim VariablesSelectorDialog As New VariablesSelectorDialogForm(m_ResGlobalVariables, m_pouInterface)
        Dim ResultDialog As System.Windows.Forms.DialogResult = VariablesSelectorDialog.ShowDialog()
        If ResultDialog = Windows.Forms.DialogResult.OK Then
            m_indicator = VariablesSelectorDialog.m_SelectedVar
            TextBox2.Text = m_indicator.Name
        End If
        TryEnableOkButton()
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        Call TryEnableOkButton()
    End Sub
End Class
