Imports System.Drawing.Printing
Imports System.Windows.Forms
Public Class PrintOptionForm
    Inherits System.Windows.Forms.Form
    Private m_Project As Project
    Private m_NumberPouPrinting As Integer
    Private m_NumberListPrinting As Integer
    Private m_Font1 As New drawing.Font("Arial", 12)
    Private m_Font2 As New drawing.Font("Arial", 10)
    Private WithEvents m_PrintDocument As PrintDocument

#Region " Codice generato da Progettazione Windows Form "

    Public Sub New(ByRef Proj As Project)
        MyBase.New()
        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()
        m_Project = Proj
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
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ListBox2 As System.Windows.Forms.ListBox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(PrintOptionForm))
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button6 = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.ListBox2 = New System.Windows.Forms.ListBox
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.Location = New System.Drawing.Point(20, 184)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(72, 32)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Print"
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.Control
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.Location = New System.Drawing.Point(236, 184)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(72, 32)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "Cancel"
        '
        'Button6
        '
        Me.Button6.BackColor = System.Drawing.SystemColors.Control
        Me.Button6.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button6.Location = New System.Drawing.Point(92, 184)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(72, 32)
        Me.Button6.TabIndex = 7
        Me.Button6.Text = "Preview"
        '
        'Label1
        '
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(24, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 16)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Pous"
        '
        'Label6
        '
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(176, 8)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(100, 16)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Variables lists"
        '
        'ListBox2
        '
        Me.ListBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBox2.DisplayMember = "Name"
        Me.ListBox2.Location = New System.Drawing.Point(176, 32)
        Me.ListBox2.Name = "ListBox2"
        Me.ListBox2.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.ListBox2.Size = New System.Drawing.Size(132, 134)
        Me.ListBox2.TabIndex = 12
        Me.ListBox2.ValueMember = "Name"
        '
        'ListBox1
        '
        Me.ListBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBox1.DisplayMember = "Name"
        Me.ListBox1.Location = New System.Drawing.Point(24, 32)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.ListBox1.Size = New System.Drawing.Size(132, 134)
        Me.ListBox1.TabIndex = 13
        Me.ListBox1.ValueMember = "Name"
        '
        'PrintOptionForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(326, 235)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.ListBox2)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PrintOptionForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Print option"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Dispose()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Print()
    End Sub

    Private Sub SfcDialogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        FillListBox()
        '
    End Sub
    Private Sub FillListBox()
        For Each P As pou In m_Project.Types.pous
            ListBox1.Items.Add(P)
        Next P
        For Each L As VariablesList In m_Project.Istances.configurations(0).resources(0).globalVars
            ListBox2.Items.Add(L)
        Next L
    End Sub
    Private Sub Print()
        'Memorizza il numero di oggetti da stampare
        Try
            m_PrintDocument = New PrintDocument
            m_PrintDocument.Print()
        Catch ex As system.exception
            MsgBox(ex.Message)
        Finally
        End Try
    End Sub
    Private Sub PrintPage(ByVal sender As Object, ByVal ev As PrintPageEventArgs) Handles m_PrintDocument.PrintPage

        Dim leftMargin As Single = ev.MarginBounds.Left
        Dim topMargin As Single = ev.MarginBounds.Top
        Dim Br As New drawing.SolidBrush(drawing.Color.Black)
        Dim P As New Drawing.Pen(Drawing.Color.Black)

        If m_NumberPouPrinting <= ListBox1.Items.Count - 1 Then
            If ListBox1.SelectedIndices.Contains(m_NumberPouPrinting) Then    'Cotrolla se l'indice è selezionato
                'Trasla l'oggetto Graphics per far disegnare dai margini sinistro e alto
                ev.Graphics.TranslateTransform(leftMargin, topMargin)

                'Stampa intestazioni e righe
                ev.Graphics.DrawString("Project: " & m_Project.ContentHeader.name & " - Pou: " & ListBox1.Items(m_NumberPouPrinting).name, m_Font1, Br, 4, 4)
                ev.Graphics.DrawLine(P, 4, 24, ev.MarginBounds.Width - 4, 24)
                ev.Graphics.DrawLine(P, 4, ev.MarginBounds.Height - 4, ev.MarginBounds.Width - 4, ev.MarginBounds.Height - 4)
                'Stampa il pou riducendo l'area occupata dell'intestazione
                ev.Graphics.TranslateTransform(0, 30)
                Dim Rect As new drawing.rectangle(0, 0, ev.MarginBounds.Width - leftMargin, ev.MarginBounds.Height - topMargin - 30)
                ListBox1.Items(m_NumberPouPrinting).PrintMe(ev.Graphics, Rect)
            End If
            m_NumberPouPrinting = m_NumberPouPrinting + 1
        End If
        If m_NumberPouPrinting = ListBox1.Items.Count - 1 Then
            ev.HasMorePages = True
        Else
            ev.HasMorePages = False
        End If
    End Sub
    Private Sub PrintPreview()
        Try
            Dim PrevDialog As New PrintPreviewDialog
            m_PrintDocument = New PrintDocument
            PrevDialog.Document = m_PrintDocument
            PrevDialog.Size = New System.Drawing.Size(600, 400)
            Dim Result As System.Windows.Forms.DialogResult = PrevDialog.ShowDialog()
            m_PrintDocument.Print()
            If Result = DialogResult.Cancel Then
                m_PrintDocument.Dispose()
            End If
        Catch Ex As system.exception
            MsgBox(Ex.Message)
        End Try
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        PrintPreview()
    End Sub
End Class
