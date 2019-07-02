Imports UnisimClassLibrary
Imports System.Drawing

Public Class AboutDialogForm
    Inherits System.Windows.Forms.Form

    Dim colorProducer As New UnisimClassLibrary.CyclicProducer(Of Drawing.Color)( _
        Color.Red, Color.Blue, Color.Green, Color.Violet, Color.Yellow, Color.DeepPink, Color.Cyan)

#Region " Codice generato da Progettazione Windows Form "

    Public Sub New()
        MyBase.New()
        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()
        'Assegna l'interfaccia

        UnisimClassLibrary.Macintoshize(Me)
        lblUniSimVer.ForeColor = Color.Red

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
    Friend WithEvents lblUniSimVer As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form.
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    Friend WithEvents Button1 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AboutDialogForm))
        Me.Button1 = New System.Windows.Forms.Button
        Me.lblUniSimVer = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.ForeColor = System.Drawing.Color.Black
        Me.Button1.Location = New System.Drawing.Point(195, 341)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(96, 28)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Ok"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'lblUniSimVer
        '
        Me.lblUniSimVer.AutoSize = True
        Me.lblUniSimVer.Font = New System.Drawing.Font("Times New Roman", 20.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUniSimVer.ForeColor = System.Drawing.Color.Red
        Me.lblUniSimVer.Location = New System.Drawing.Point(12, 23)
        Me.lblUniSimVer.Name = "lblUniSimVer"
        Me.lblUniSimVer.Size = New System.Drawing.Size(279, 31)
        Me.lblUniSimVer.TabIndex = 6
        Me.lblUniSimVer.Tag = ""
        Me.lblUniSimVer.Text = "This version of UniSim"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 67)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(460, 262)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = resources.GetString("Label1.Text")
        '
        'Timer1
        '
        '
        'AboutDialogForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(484, 381)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblUniSimVer)
        Me.Controls.Add(Me.Button1)
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AboutDialogForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "About..."
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region


#Region "Codice speciale"
    Private Sub LoadSpecial()
        Dim dt As DateTime = DateTime.Now
        If dt.Day = 25 AndAlso dt.Month = 12 Then Me.Text = "Merry Christmas"
        If dt.Day = 1 AndAlso dt.Month = 1 Then Me.Text = "Happy New Year"
        If dt.Day = 1 AndAlso dt.Month = 5 Then Me.Text = "Happy Labor Day"
    End Sub

    Private Sub lblUniSimVer_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblUniSimVer.DoubleClick
        ' CTRL+ALT+doppio clic qui attiva le due uova di Pasqua
        If My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.AltKeyDown Then
            ' se sono accesi tutti e 3 i LED scatena un'eccezione non gestita
            If My.Computer.Keyboard.ScrollLock AndAlso My.Computer.Keyboard.CapsLock AndAlso My.Computer.Keyboard.NumLock Then
                Me.Hide()
                Throw New OutOfMemoryException()
                ' se sono spenti tutti e 3 i LED attiva l'effetto arcobaleno
            ElseIf Not (My.Computer.Keyboard.ScrollLock) AndAlso Not (My.Computer.Keyboard.CapsLock) AndAlso Not (My.Computer.Keyboard.NumLock) Then
                Timer1.Start()
            End If
        End If
    End Sub

    ' L'uovo di Pasqua: cambia il colore del testo "UniSim DevApp ecc ecc" ogni 100ms :-)
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Not colorProducer.Advance() Then colorProducer.Reset()
        lblUniSimVer.ForeColor = colorProducer.CurrentItem
    End Sub

#End Region

    Private Sub ActionDialogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblUniSimVer.Text = UnisimClassLibrary.UniSimVersion.VersionInfo.PrintableDescriptionForTool()
        Me.LoadSpecial()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Dispose()
    End Sub

End Class
