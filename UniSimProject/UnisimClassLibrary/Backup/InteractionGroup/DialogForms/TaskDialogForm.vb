Imports System
Imports System.Collections.Generic
Public Class TaskDialogForm
    Inherits System.Windows.Forms.Form
    Public m_name As String
    Public m_Single As String
    Public m_interval As TimeSpan
    Public m_priority As Integer
    Public m_pouInstances As List(Of pouInstance)
    Private v_resource As Resource
    Private v_pous As Pous


#Region " Codice generato da Progettazione Windows Form "

    Public Sub New(ByRef RefResource As Resource, ByRef PousList As Pous)
        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()
        v_resource = RefResource
        v_pous = PousList

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
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents NumericUpDown8 As System.Windows.Forms.NumericUpDown
    Friend WithEvents NumericUpDown4 As System.Windows.Forms.NumericUpDown
    Friend WithEvents NumericUpDown3 As System.Windows.Forms.NumericUpDown
    Friend WithEvents NumericUpDown2 As System.Windows.Forms.NumericUpDown
    Friend WithEvents NumericUpDown1 As System.Windows.Forms.NumericUpDown
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(TaskDialogForm))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.NumericUpDown8 = New System.Windows.Forms.NumericUpDown
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.NumericUpDown4 = New System.Windows.Forms.NumericUpDown
        Me.NumericUpDown3 = New System.Windows.Forms.NumericUpDown
        Me.NumericUpDown2 = New System.Windows.Forms.NumericUpDown
        Me.NumericUpDown1 = New System.Windows.Forms.NumericUpDown
        CType(Me.NumericUpDown8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(12, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 16)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Name: "
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.Control
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.Location = New System.Drawing.Point(188, 216)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(80, 32)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "Cancel"
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button1.Enabled = False
        Me.Button1.Location = New System.Drawing.Point(108, 216)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(80, 32)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Ok"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(100, 16)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(188, 20)
        Me.TextBox1.TabIndex = 0
        Me.TextBox1.Text = ""
        '
        'Label2
        '
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(12, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 16)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Interval:"
        '
        'Label3
        '
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(12, 84)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 16)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Priority:"
        '
        'Label4
        '
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(12, 120)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(84, 16)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Pous instances:"
        '
        'ListBox1
        '
        Me.ListBox1.DisplayMember = "Name"
        Me.ListBox1.Location = New System.Drawing.Point(100, 116)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.ListBox1.Size = New System.Drawing.Size(188, 82)
        Me.ListBox1.Sorted = True
        Me.ListBox1.TabIndex = 3
        Me.ListBox1.ValueMember = "Name"
        '
        'NumericUpDown8
        '
        Me.NumericUpDown8.Location = New System.Drawing.Point(100, 84)
        Me.NumericUpDown8.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
        Me.NumericUpDown8.Name = "NumericUpDown8"
        Me.NumericUpDown8.Size = New System.Drawing.Size(76, 20)
        Me.NumericUpDown8.TabIndex = 15
        Me.NumericUpDown8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label9
        '
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(292, 56)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(24, 16)
        Me.Label9.TabIndex = 28
        Me.Label9.Text = "ms:"
        '
        'Label8
        '
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(224, 56)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(16, 16)
        Me.Label8.TabIndex = 27
        Me.Label8.Text = "s:"
        '
        'Label7
        '
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(152, 56)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(20, 16)
        Me.Label7.TabIndex = 26
        Me.Label7.Text = "m:"
        '
        'Label6
        '
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(84, 56)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(16, 16)
        Me.Label6.TabIndex = 25
        Me.Label6.Text = "h:"
        '
        'NumericUpDown4
        '
        Me.NumericUpDown4.Location = New System.Drawing.Point(316, 52)
        Me.NumericUpDown4.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
        Me.NumericUpDown4.Name = "NumericUpDown4"
        Me.NumericUpDown4.Size = New System.Drawing.Size(48, 20)
        Me.NumericUpDown4.TabIndex = 23
        Me.NumericUpDown4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'NumericUpDown3
        '
        Me.NumericUpDown3.Location = New System.Drawing.Point(240, 52)
        Me.NumericUpDown3.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
        Me.NumericUpDown3.Name = "NumericUpDown3"
        Me.NumericUpDown3.Size = New System.Drawing.Size(48, 20)
        Me.NumericUpDown3.TabIndex = 21
        Me.NumericUpDown3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'NumericUpDown2
        '
        Me.NumericUpDown2.Location = New System.Drawing.Point(172, 52)
        Me.NumericUpDown2.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
        Me.NumericUpDown2.Name = "NumericUpDown2"
        Me.NumericUpDown2.Size = New System.Drawing.Size(48, 20)
        Me.NumericUpDown2.TabIndex = 20
        Me.NumericUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'NumericUpDown1
        '
        Me.NumericUpDown1.Location = New System.Drawing.Point(100, 52)
        Me.NumericUpDown1.Maximum = New Decimal(New Integer() {23, 0, 0, 0})
        Me.NumericUpDown1.Name = "NumericUpDown1"
        Me.NumericUpDown1.Size = New System.Drawing.Size(48, 20)
        Me.NumericUpDown1.TabIndex = 19
        Me.NumericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TaskDialogForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(374, 263)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.NumericUpDown4)
        Me.Controls.Add(Me.NumericUpDown3)
        Me.Controls.Add(Me.NumericUpDown2)
        Me.Controls.Add(Me.NumericUpDown1)
        Me.Controls.Add(Me.NumericUpDown8)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TaskDialogForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Task"
        CType(Me.NumericUpDown8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub VariableDialogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        AcceptButton = Button1
        TextBox1.Text = m_name
        NumericUpDown1.Value = Val(m_interval.Hours)
        NumericUpDown2.Value = Val(m_interval.Minutes)
        NumericUpDown3.Value = Val(m_interval.Seconds)
        NumericUpDown4.Value = Val(m_interval.Milliseconds)
        NumericUpDown8.Value = m_priority
        'Aggiunge i pous non presenti nell'istanza
        For Each P As pou In v_pous
            If P.PouType <> EnumPouType.Program Then Continue For
            If IsNothing(v_resource.FindPouInstanceByName(P.Name)) Then ListBox1.Items.Add(P)
        Next
        'Aggiunge i propri pou e li seleziona
        If Not IsNothing(m_pouInstances) Then
            For Each P As pouInstance In m_pouInstances
                ' metti solo le POU di tipo program
                If P.pou.PouType <> EnumPouType.Program Then Continue For
                Dim i As Integer = ListBox1.Items.Add(P.pou)
                ListBox1.SetSelected(i, True)
            Next
        End If
        TextBox1.Focus()
        TryEnableOkButton()
    End Sub
    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        TryEnableOkButton()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        m_name = TextBox1.Text
        m_interval = New TimeSpan(0, NumericUpDown1.Value, NumericUpDown2.Value, NumericUpDown3.Value, NumericUpDown4.Value)
        m_priority = NumericUpDown8.Value
        m_pouInstances = New List(Of pouInstance)
        'Aggiunge le PouInstance con il riferimento al pou
        For Each P As pou In ListBox1.SelectedItems
            Dim NewPouInstance As New pouInstance(P)
            m_pouInstances.Add(NewPouInstance)
        Next P
    End Sub
    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        'Limita l'inserimento di caratteri per i nomi di variabili
        Dim NewChar As Char = e.KeyChar
        If Not InvalidVariableChars.IndexOf(e.KeyChar) = -1 Then
            e.Handled = True
        End If
    End Sub
    Private Sub TryEnableOkButton()
        'Controlla initial value e in nome
        If TextBox1.Text = "" Or ListBox1.SelectedItems.Count = 0 Then
            Button1.Enabled = False
        Else
            Button1.Enabled = True
        End If
    End Sub
    Private Sub NumericUpDown8_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles NumericUpDown8.KeyUp
        If NumericUpDown8.Value > 65535 Then
            NumericUpDown8.Value = 65535
        End If
        If NumericUpDown8.Value < 0 Then
            NumericUpDown8.Value = 0
        End If
    End Sub
    Private Sub NumericUpDown4_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles NumericUpDown4.KeyUp
        If NumericUpDown4.Value > 999 Then
            NumericUpDown4.Value = 999
        End If
        If NumericUpDown4.Value < 0 Then
            NumericUpDown4.Value = 0
        End If
    End Sub
    Private Sub NumericUpDown3_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles NumericUpDown3.KeyUp
        If NumericUpDown3.Value > 59 Then
            NumericUpDown3.Value = 59
        End If
        If NumericUpDown3.Value < 0 Then
            NumericUpDown3.Value = 0
        End If
    End Sub
    Private Sub NumericUpDown2_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles NumericUpDown2.KeyUp
        If NumericUpDown2.Value > 59 Then
            NumericUpDown2.Value = 59
        End If
        If NumericUpDown2.Value < 0 Then
            NumericUpDown2.Value = 0
        End If
    End Sub
    Private Sub NumericUpDown1_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles NumericUpDown1.KeyUp
        If NumericUpDown1.Value > 23 Then
            NumericUpDown1.Value = 23
        End If
        If NumericUpDown1.Value < 0 Then
            NumericUpDown1.Value = 0
        End If
    End Sub
    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        TryEnableOkButton()
    End Sub
End Class
