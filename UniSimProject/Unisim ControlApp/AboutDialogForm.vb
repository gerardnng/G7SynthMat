Public Class AboutDialogForm
    Inherits System.Windows.Forms.Form










#Region " Codice generato da Progettazione Windows Form "

    Public Sub New()
        MyBase.New()
        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()
        'Assegna l'interfaccia
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
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Label2 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Navy
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(20, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(124, 24)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Unisim - Version 0.4"
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.ForeColor = System.Drawing.Color.Black
        Me.Button1.Location = New System.Drawing.Point(84, 256)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(96, 28)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Ok"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Navy
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(20, 40)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(224, 48)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Questo software è stato sviluppato da Pierangelo Di Sanzo in ambito della tesi di" & _
        " laurea :"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(20, 80)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(224, 32)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = """Un tool di sviluppo, validazione e controllo per progetti di automazione"""
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(20, 112)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(196, 16)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Relatore: Prof. Alfredo Pironti"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(20, 128)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(220, 15)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Correlatore: Ing. Giammaria De Tommasi"
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(20, 152)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(228, 20)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Università degli Studi di Napoli ""Federico II"""
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(20, 168)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(224, 16)
        Me.Label7.TabIndex = 11
        Me.Label7.Text = "Facoltà di Ingegneria"
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(20, 184)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(228, 20)
        Me.Label8.TabIndex = 12
        Me.Label8.Text = "Corso di Laurea in Ingegneria Informatica"
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(20, 216)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(228, 28)
        Me.Label9.TabIndex = 13
        Me.Label9.Text = "Per errori e segnalazioni: pierangelo.disanzo@tiscali.it"
        '
        'AboutDialogForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(258, 291)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label2)
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AboutDialogForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "About..."
        Me.ResumeLayout(False)

    End Sub

#End Region


    Private Sub ActionDialogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Dispose()

    End Sub
End Class
