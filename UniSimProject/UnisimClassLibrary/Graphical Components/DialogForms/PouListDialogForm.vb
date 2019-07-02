Public Class PouListDialogForm
    Inherits System.Windows.Forms.Form
    Public m_pou As pou
    Private v_resource As Resource
    Private v_pous As Pous


#Region " Codice generato da Progettazione Windows Form "

    Public Sub New(ByRef RefResource As Resource, ByRef PousList As Pous)
        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()
        v_resource = RefResource
        v_pous = PousList
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
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(PouListDialogForm))
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.SuspendLayout()
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.Control
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.Location = New System.Drawing.Point(104, 172)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(80, 32)
        Me.Button2.TabIndex = 7
        Me.Button2.Text = "Cancel"
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button1.Enabled = False
        Me.Button1.Location = New System.Drawing.Point(24, 172)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(80, 32)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Add"
        '
        'ListBox1
        '
        Me.ListBox1.DisplayMember = "Name"
        Me.ListBox1.Location = New System.Drawing.Point(12, 12)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(192, 147)
        Me.ListBox1.Sorted = True
        Me.ListBox1.TabIndex = 14
        Me.ListBox1.ValueMember = "Name"
        '
        'PouListDialogForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(218, 215)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PouListDialogForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Pou instance"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub VariableDialogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim a As Integer
        AcceptButton = Button1
        'Aggiunge i pous non presenti nell'istanza
        For Each P As pou In v_pous
            If IsNothing(v_resource.FindPouInstanceByName(P.Name)) Then
                ListBox1.Items.Add(P)
            End If
        Next P
        'Aggiunge il proprio pou e lo seleziona
        If Not IsNothing(m_pou) Then
            Dim i As Integer = ListBox1.Items.Add(m_pou)
            ListBox1.SetSelected(i, True)
        End If
        TryEnableOkButton()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        m_pou = ListBox1.SelectedItem
        'Aggiunge le PouInstance con il riferimento al pou
        For Each P As pou In ListBox1.SelectedItems
            Dim NewPouInstance As New pouInstance(P)
        Next P
    End Sub
    Private Sub TryEnableOkButton()
        If Not IsNothing(ListBox1.SelectedItem) Then
            Button1.Enabled = True
        Else
            Button1.Enabled = False
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        TryEnableOkButton()
    End Sub
End Class
