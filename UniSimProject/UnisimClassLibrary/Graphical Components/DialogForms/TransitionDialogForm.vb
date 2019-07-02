Public Class TransitionDialogForm
    Inherits System.Windows.Forms.Form
    Public m_Name As String
    Public m_Documentation As String
    Public m_Sfc As Sfc
    Public m_Condition As BooleanExpression
    Private m_ExpressionValidated As Boolean

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
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Button3 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(TransitionDialogForm))
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Button3 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button1.Location = New System.Drawing.Point(216, 132)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(72, 32)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Ok"
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.Control
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.Location = New System.Drawing.Point(288, 132)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(72, 32)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "Cancel"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(92, 40)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(300, 20)
        Me.TextBox1.TabIndex = 2
        Me.TextBox1.Text = "true"
        '
        'Label2
        '
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(20, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 16)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Condition:"
        '
        'Label3
        '
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(92, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(208, 32)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Sintax:  + (Or) ,  * (And),  ! (Not) Example ref. to maker of fase [101.t>2s]"
        '
        'ListBox1
        '
        Me.ListBox1.DisplayMember = "Name"
        Me.ListBox1.Location = New System.Drawing.Point(408, 40)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(144, 82)
        Me.ListBox1.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(408, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(120, 16)
        Me.Label4.TabIndex = 19
        Me.Label4.Text = "Variables selector"
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.SystemColors.Window
        Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(92, 96)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(300, 20)
        Me.Label7.TabIndex = 23
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.SystemColors.Control
        Me.Button3.Location = New System.Drawing.Point(20, 96)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(60, 20)
        Me.Button3.TabIndex = 3
        Me.Button3.Text = "Validate"
        '
        'TransitionDialogForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(574, 175)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label3)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TransitionDialogForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Transition"
        Me.ResumeLayout(False)

    End Sub

#End Region
    Private Sub SfcDialogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.AcceptButton = Button1
        FillVariablesList()
        If Not IsNothing(m_Condition) Then
            TextBox1.Text = m_Condition.GetExpressionString
        End If
    End Sub
    Private Sub FillVariablesList()
        'Aggiunge le variabili
        For Each L As VariablesList In m_Sfc.ResGlobalVariables
            For Each V As BaseVariable In L
                ListBox1.Items.Add(V)
            Next V
        Next L
        ListBox1.Sorted = True
    End Sub
    Private Sub TryEnableOkButton()
        If m_ExpressionValidated Then
            Button1.Enabled = True
        Else
            Button1.Enabled = False
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Crea l'espressione
        m_Condition = New BooleanExpression(m_Sfc)
        'Imposta l'espressione
        m_Condition.SetExpression(TextBox1.Text)
    End Sub
    Private Sub ListBox1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.DoubleClick
        If Not IsNothing(ListBox1.SelectedItem) Then
            TextBox1.Text = TextBox1.Text & ListBox1.SelectedItem.Name
            TextBox1.Focus()
            TextBox1.SelectionStart = TextBox1.TextLength
        End If
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        'Effettua il test della condizione
        Dim Result As String
        'Crea un oggetto condizione temporaneo
        Dim CondTemp As New BooleanExpression(m_Sfc)
        If CondTemp.SetExpression(TextBox1.Text) Then
            Result = "Syntax Ok. Actual value: " & CondTemp.Evaluate()
            m_ExpressionValidated = True
        Else
            Result = CondTemp.ReadError
            'Disattiva il tasto OK
            Button1.Enabled = False
        End If
        CondTemp = Nothing
        Label7.Text = Result
        TryEnableOkButton()
    End Sub
    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        TryEnableOkButton()
    End Sub
    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        m_ExpressionValidated = False
        TryEnableOkButton()
    End Sub
    Private Sub TextBox2_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'Limita l'inserimento di caratteri per i nomi di variabili
        Dim NewChar As Char = e.KeyChar
        If Not InvalidVariableChars.IndexOf(e.KeyChar) = -1 Then
            e.Handled = True
        End If
    End Sub
End Class
