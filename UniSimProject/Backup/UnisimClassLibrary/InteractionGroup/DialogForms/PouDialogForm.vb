Public Class pouDialogForm
    Inherits System.Windows.Forms.Form
    Public m_Name As String
    Public m_pouType As EnumPouType
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents rdbPOUInstance As System.Windows.Forms.RadioButton
    Friend WithEvents rdbTask As System.Windows.Forms.RadioButton
    Friend WithEvents rdbNothing As System.Windows.Forms.RadioButton
    Public m_bodyType As EnumBodyType
    ' Settare a False se stiamo editando le Info di una POU esistente     
    Private m_Load As Boolean = True

#Region " Codice generato da Progettazione Windows Form "

    Public Sub New()
        MyBase.New()
        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()

        Macintoshize(Me)

    End Sub
    Public Sub New(ByVal P As pou)
        Me.New()
        TextBox1.Text = P.Name
        Select Case P.Body.ReadBodyType
            Case EnumBodyType.tSFC
                rdbSFC.Checked = True
            Case EnumBodyType.tLD
                rdbLD.Checked = True
            Case EnumBodyType.tFBD
                rdbFBD.Checked = True
            Case EnumBodyType.tST
                rdbST.Checked = True
            Case EnumBodyType.tIL
                rdbIL.Checked = True
        End Select
        Select Case P.PouType
            Case EnumPouType.Function
                rdbFunction.Checked = True
            Case EnumPouType.FunctionBlock
                rdbFunctionBlock.Checked = True
            Case EnumPouType.Program
                rdbProgram.Checked = True
        End Select
        Panel3.Visible = False
        m_Load = False
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
    Friend WithEvents rdbProgram As System.Windows.Forms.RadioButton
    Friend WithEvents rdbFunction As System.Windows.Forms.RadioButton
    Friend WithEvents rdbFunctionBlock As System.Windows.Forms.RadioButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rdbIL As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents rdbSFC As System.Windows.Forms.RadioButton
    Friend WithEvents rdbLD As System.Windows.Forms.RadioButton
    Friend WithEvents rdbFBD As System.Windows.Forms.RadioButton
    Friend WithEvents rdbST As System.Windows.Forms.RadioButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(pouDialogForm))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.rdbProgram = New System.Windows.Forms.RadioButton
        Me.rdbFunction = New System.Windows.Forms.RadioButton
        Me.rdbFunctionBlock = New System.Windows.Forms.RadioButton
        Me.Label2 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.rdbIL = New System.Windows.Forms.RadioButton
        Me.Label4 = New System.Windows.Forms.Label
        Me.rdbSFC = New System.Windows.Forms.RadioButton
        Me.rdbLD = New System.Windows.Forms.RadioButton
        Me.rdbFBD = New System.Windows.Forms.RadioButton
        Me.rdbST = New System.Windows.Forms.RadioButton
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Label3 = New System.Windows.Forms.Label
        Me.rdbPOUInstance = New System.Windows.Forms.RadioButton
        Me.rdbTask = New System.Windows.Forms.RadioButton
        Me.rdbNothing = New System.Windows.Forms.RadioButton
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
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
        Me.Button1.Location = New System.Drawing.Point(96, 350)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(80, 32)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Ok"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.Control
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.Location = New System.Drawing.Point(215, 350)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(80, 32)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(56, 16)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(316, 20)
        Me.TextBox1.TabIndex = 0
        '
        'rdbProgram
        '
        Me.rdbProgram.AutoSize = True
        Me.rdbProgram.Checked = True
        Me.rdbProgram.ForeColor = System.Drawing.Color.White
        Me.rdbProgram.Location = New System.Drawing.Point(16, 24)
        Me.rdbProgram.Name = "rdbProgram"
        Me.rdbProgram.Size = New System.Drawing.Size(64, 17)
        Me.rdbProgram.TabIndex = 8
        Me.rdbProgram.TabStop = True
        Me.rdbProgram.Text = "Program"
        '
        'rdbFunction
        '
        Me.rdbFunction.AutoSize = True
        Me.rdbFunction.ForeColor = System.Drawing.Color.White
        Me.rdbFunction.Location = New System.Drawing.Point(16, 48)
        Me.rdbFunction.Name = "rdbFunction"
        Me.rdbFunction.Size = New System.Drawing.Size(66, 17)
        Me.rdbFunction.TabIndex = 9
        Me.rdbFunction.Text = "Function"
        '
        'rdbFunctionBlock
        '
        Me.rdbFunctionBlock.AutoSize = True
        Me.rdbFunctionBlock.Enabled = False
        Me.rdbFunctionBlock.ForeColor = System.Drawing.Color.White
        Me.rdbFunctionBlock.Location = New System.Drawing.Point(16, 72)
        Me.rdbFunctionBlock.Name = "rdbFunctionBlock"
        Me.rdbFunctionBlock.Size = New System.Drawing.Size(103, 17)
        Me.rdbFunctionBlock.TabIndex = 10
        Me.rdbFunctionBlock.Text = "Functional block"
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
        Me.Panel1.BackColor = System.Drawing.Color.Transparent
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.rdbProgram)
        Me.Panel1.Controls.Add(Me.rdbFunction)
        Me.Panel1.Controls.Add(Me.rdbFunctionBlock)
        Me.Panel1.Location = New System.Drawing.Point(56, 56)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(161, 152)
        Me.Panel1.TabIndex = 14
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.Transparent
        Me.Panel2.Controls.Add(Me.rdbIL)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.rdbSFC)
        Me.Panel2.Controls.Add(Me.rdbLD)
        Me.Panel2.Controls.Add(Me.rdbFBD)
        Me.Panel2.Controls.Add(Me.rdbST)
        Me.Panel2.Location = New System.Drawing.Point(223, 56)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(149, 152)
        Me.Panel2.TabIndex = 15
        '
        'rdbIL
        '
        Me.rdbIL.AutoSize = True
        Me.rdbIL.Enabled = False
        Me.rdbIL.ForeColor = System.Drawing.Color.White
        Me.rdbIL.Location = New System.Drawing.Point(24, 120)
        Me.rdbIL.Name = "rdbIL"
        Me.rdbIL.Size = New System.Drawing.Size(34, 17)
        Me.rdbIL.TabIndex = 12
        Me.rdbIL.Text = "IL"
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
        'rdbSFC
        '
        Me.rdbSFC.AutoSize = True
        Me.rdbSFC.Checked = True
        Me.rdbSFC.ForeColor = System.Drawing.Color.White
        Me.rdbSFC.Location = New System.Drawing.Point(24, 24)
        Me.rdbSFC.Name = "rdbSFC"
        Me.rdbSFC.Size = New System.Drawing.Size(45, 17)
        Me.rdbSFC.TabIndex = 8
        Me.rdbSFC.TabStop = True
        Me.rdbSFC.Text = "SFC"
        '
        'rdbLD
        '
        Me.rdbLD.AutoSize = True
        Me.rdbLD.ForeColor = System.Drawing.Color.White
        Me.rdbLD.Location = New System.Drawing.Point(24, 48)
        Me.rdbLD.Name = "rdbLD"
        Me.rdbLD.Size = New System.Drawing.Size(39, 17)
        Me.rdbLD.TabIndex = 9
        Me.rdbLD.Text = "LD"
        '
        'rdbFBD
        '
        Me.rdbFBD.AutoSize = True
        Me.rdbFBD.ForeColor = System.Drawing.Color.White
        Me.rdbFBD.Location = New System.Drawing.Point(24, 72)
        Me.rdbFBD.Name = "rdbFBD"
        Me.rdbFBD.Size = New System.Drawing.Size(46, 17)
        Me.rdbFBD.TabIndex = 10
        Me.rdbFBD.Text = "FDB"
        '
        'rdbST
        '
        Me.rdbST.AutoSize = True
        Me.rdbST.Enabled = False
        Me.rdbST.ForeColor = System.Drawing.Color.White
        Me.rdbST.Location = New System.Drawing.Point(24, 96)
        Me.rdbST.Name = "rdbST"
        Me.rdbST.Size = New System.Drawing.Size(39, 17)
        Me.rdbST.TabIndex = 11
        Me.rdbST.Text = "ST"
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.Transparent
        Me.Panel3.Controls.Add(Me.Label3)
        Me.Panel3.Controls.Add(Me.rdbPOUInstance)
        Me.Panel3.Controls.Add(Me.rdbTask)
        Me.Panel3.Controls.Add(Me.rdbNothing)
        Me.Panel3.Location = New System.Drawing.Point(56, 223)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(316, 103)
        Me.Panel3.TabIndex = 14
        '
        'Label3
        '
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(8, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(64, 16)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Instancing:"
        '
        'rdbPOUInstance
        '
        Me.rdbPOUInstance.AutoSize = True
        Me.rdbPOUInstance.Checked = True
        Me.rdbPOUInstance.ForeColor = System.Drawing.Color.White
        Me.rdbPOUInstance.Location = New System.Drawing.Point(16, 24)
        Me.rdbPOUInstance.Name = "rdbPOUInstance"
        Me.rdbPOUInstance.Size = New System.Drawing.Size(134, 17)
        Me.rdbPOUInstance.TabIndex = 8
        Me.rdbPOUInstance.TabStop = True
        Me.rdbPOUInstance.Text = "Create a POU instance"
        '
        'rdbTask
        '
        Me.rdbTask.AutoSize = True
        Me.rdbTask.ForeColor = System.Drawing.Color.White
        Me.rdbTask.Location = New System.Drawing.Point(16, 47)
        Me.rdbTask.Name = "rdbTask"
        Me.rdbTask.Size = New System.Drawing.Size(88, 17)
        Me.rdbTask.TabIndex = 9
        Me.rdbTask.Text = "Create a task"
        '
        'rdbNothing
        '
        Me.rdbNothing.AutoSize = True
        Me.rdbNothing.ForeColor = System.Drawing.Color.White
        Me.rdbNothing.Location = New System.Drawing.Point(16, 70)
        Me.rdbNothing.Name = "rdbNothing"
        Me.rdbNothing.Size = New System.Drawing.Size(77, 17)
        Me.rdbNothing.TabIndex = 10
        Me.rdbNothing.Text = "Do nothing"
        '
        'pouDialogForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(384, 403)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel3)
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
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Dispose()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        m_Name = TextBox1.Text
        If rdbProgram.Checked Then
            m_pouType = EnumPouType.Program
        ElseIf rdbFunction.Checked Then
            m_pouType = EnumPouType.Function
        End If
        'm_bodyType = EnumBodyType.tSFC
        If rdbSFC.Checked Then
            m_bodyType = EnumBodyType.tSFC
        ElseIf rdbLD.Checked Then
            m_bodyType = EnumBodyType.tLD
        ElseIf rdbFBD.Checked Then
            m_bodyType = EnumBodyType.tFBD
        End If
    End Sub

    Private Sub SfcDialogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not (m_Load) Then Exit Sub
        Me.AcceptButton = Button1
        TextBox1.Text = m_Name
        'Disattiva i pulsanti di conferma se non c'e il nome
        If TextBox1.Text = "" Then
            Button1.Enabled = False
        Else
            Button1.Enabled = True
        End If
        TextBox1.Focus()
        Dim defaultInstancing As Integer = Preferences.GetInteger("ActionOnNewPOU", 1)
        Select Case defaultInstancing
            Case 1
                rdbPOUInstance.Checked = True
                rdbTask.Checked = False
                rdbNothing.Checked = False
            Case 2
                rdbPOUInstance.Checked = False
                rdbTask.Checked = True
                rdbNothing.Checked = False
            Case 3
                rdbPOUInstance.Checked = False
                rdbTask.Checked = False
                rdbNothing.Checked = True
        End Select
        Dim langPOU As Integer = Preferences.GetInteger("NewPOULang", 1)
        If langPOU = 1 Then rdbSFC.Checked = True
        If langPOU = 2 Then rdbLD.Checked = True
        If langPOU = 3 Then rdbFBD.Checked = True
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

    Public ReadOnly Property WhatToCreate() As Integer
        Get
            If rdbPOUInstance.Checked Then Return 1
            If rdbTask.Checked Then Return 2
            Return 3
        End Get
    End Property

    ' Questo gestore di eventi assicura che non possiamo creare POU function in SFC
    ' e che una POU function non possa avere un'istanza nè un task associati alla creazione
    Private Sub rdbFunction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles rdbFunction.Click, rdbFunction.CheckedChanged
        rdbSFC.Enabled = Not (rdbFunction.Checked)
        rdbPOUInstance.Enabled = Not (rdbFunction.Checked)
        rdbTask.Enabled = Not (rdbFunction.Checked)
        If rdbSFC.Checked And Not (rdbSFC.Enabled) Then rdbLD.Checked = True
        If (rdbPOUInstance.Checked And Not (rdbPOUInstance.Enabled)) OrElse _
            ((rdbTask.Checked) And Not (rdbTask.Enabled)) Then rdbNothing.Checked = True
    End Sub
End Class


