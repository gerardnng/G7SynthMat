Public Class VariablesSelectorDialogForm
    Inherits System.Windows.Forms.Form
    Public m_SelectedVar As Object
    Public m_Type As String
    Public m_Value As String
    Private m_ResGlobalVariables As VariablesLists
    Private m_PouInterface As pouInterface

#Region " Codice generato da Progettazione Windows Form "

    Public Sub New(ByRef RefResGlobalVariables As VariablesLists, _
        ByRef RefPouInterface As pouInterface)
        MyBase.New()
        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()
        m_ResGlobalVariables = RefResGlobalVariables
        m_PouInterface = RefPouInterface
        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()

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
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents NumericUpDown2 As System.Windows.Forms.NumericUpDown
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ListView2 As System.Windows.Forms.ListView
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader10 As System.Windows.Forms.ColumnHeader

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim ListViewItem1 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("")
        Dim ListViewItem2 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("")
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(VariablesSelectorDialogForm))
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader10 = New System.Windows.Forms.ColumnHeader
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.ListView2 = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.Label1 = New System.Windows.Forms.Label
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button1.Location = New System.Drawing.Point(152, 212)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(72, 32)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Ok"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.Control
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.Location = New System.Drawing.Point(224, 212)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(72, 32)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(8, 16)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(474, 180)
        Me.TabControl1.TabIndex = 2
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.ListView1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(466, 154)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "POU-level variables"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'ListView1
        '
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8, Me.ColumnHeader9, Me.ColumnHeader10})
        Me.ListView1.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem1})
        Me.ListView1.Location = New System.Drawing.Point(0, 4)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(470, 148)
        Me.ListView1.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.ListView1.TabIndex = 3
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Name"
        Me.ColumnHeader6.Width = 138
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Type"
        Me.ColumnHeader7.Width = 82
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Address"
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "Actual value"
        Me.ColumnHeader9.Width = 86
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "Initial/Final value"
        Me.ColumnHeader10.Width = 97
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.ListView2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(466, 154)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Resource-level variables"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'ListView2
        '
        Me.ListView2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5})
        Me.ListView2.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem2})
        Me.ListView2.Location = New System.Drawing.Point(0, 4)
        Me.ListView2.MultiSelect = False
        Me.ListView2.Name = "ListView2"
        Me.ListView2.Size = New System.Drawing.Size(470, 148)
        Me.ListView2.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.ListView2.TabIndex = 4
        Me.ListView2.UseCompatibleStateImageBehavior = False
        Me.ListView2.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 138
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Type"
        Me.ColumnHeader2.Width = 82
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Address"
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Actual value"
        Me.ColumnHeader4.Width = 86
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Initial/Final value"
        Me.ColumnHeader5.Width = 97
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(252, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(230, 28)
        Me.Label1.TabIndex = 4
        '
        'VariablesSelectorDialogForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(494, 251)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "VariablesSelectorDialogForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Variables selector"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Dispose()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim dismiss As Boolean = False
        Select Case TabControl1.SelectedIndex
            Case 0  'Scheda variabili locali
                If ListView1.SelectedItems.Count > 0 Then
                    Dim TempVar As BaseVariable = m_PouInterface.localVars.FindVariableByName(ListView1.SelectedItems(0).Text)
                    If Not IsNothing(TempVar) Then
                        m_SelectedVar = TempVar
                        m_Type = TempVar.dataType
                        m_Value = TempVar.ReadValue
                        dismiss = True
                    End If
                End If
            Case 1  'Scheda variabili globali di risorsa
                If ListView2.SelectedItems.Count > 0 Then
                    Dim TempVar As BaseVariable = m_ResGlobalVariables.FindVariableByName(ListView2.SelectedItems(0).Text)
                    If Not IsNothing(TempVar) Then
                        m_SelectedVar = TempVar
                        m_Type = TempVar.dataType
                        m_Value = TempVar.ReadValue
                        dismiss = True
                    End If
                End If
        End Select
        If dismiss Then
            DialogResult = Windows.Forms.DialogResult.OK
            Me.Hide()
        End If
    End Sub
    Private Sub TryEnableOkButton()
        'Disattiva i pulsanti di conferma se non c'e una variabile selezionata
        Select Case TabControl1.SelectedIndex
            Case 0  'Scheda variabili locali
                If ListView1.SelectedItems.Count > 0 Then
                    Button1.Enabled = True
                Else
                    Button1.Enabled = False
                End If
            Case 1  'Scheda variabili globali di risorsa
                If ListView2.SelectedItems.Count > 0 Then
                    Button1.Enabled = True
                Else
                    Button1.Enabled = False
                End If
        End Select
    End Sub
    Private Sub FillVariableDetails(ByVal lstView As Windows.Forms.ListView, _
        ByVal V As BaseVariable)
        Dim i As Integer = lstView.Items.Add(V.Name).Index
        lstView.Items(i).SubItems.Add(V.dataType)
        lstView.Items(i).SubItems.Add(V.Address)
        lstView.Items(i).SubItems.Add(V.ValueToUniversalString())
        lstView.Items(i).SubItems.Add(V.InitialValueToUniversalString())
    End Sub
    Private Sub FillVariablesList()
        'Aggiunge le variabili
        ListView1.Items.Clear()
        For Each V As BaseVariable In m_PouInterface.localVars
            FillVariableDetails(ListView1, V)
        Next V
        If m_PouInterface.pou.PouType = EnumPouType.Function Then
            For Each V As BaseVariable In m_PouInterface.inputVars
                FillVariableDetails(ListView1, V)
            Next
            For Each V As BaseVariable In m_PouInterface.outputVars
                FillVariableDetails(ListView1, V)
            Next
        End If
        ListView2.Items.Clear()
        For Each L As VariablesList In m_ResGlobalVariables
            For Each V As BaseVariable In L
                FillVariableDetails(ListView2, V)
            Next V
        Next L
    End Sub
    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        'Deseleziona tutte le variabili selezionate
        TryEnableOkButton()
    End Sub
    Private Sub VariablesSelectorDialogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        FillVariablesList()
    End Sub
    Private Sub ListView1_MouseDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseDown
        TryEnableOkButton()
    End Sub
    Private Sub ListView2_MouseDown2(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView2.MouseDown
        TryEnableOkButton()
    End Sub
    Private Sub ListView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        TryEnableOkButton()
    End Sub
    Private Sub ListView2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView2.SelectedIndexChanged
        TryEnableOkButton()
    End Sub

    Private Sub ListView1_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        If ListView1.SelectedIndices.Count = 0 Then Exit Sub
        Call Button1_Click(sender, e)
    End Sub

    Private Sub ListView2_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView2.DoubleClick
        If ListView2.SelectedIndices.Count = 0 Then Exit Sub
        Call Button1_Click(sender, e)
    End Sub

End Class
