<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnhandledExceptionDialogForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnhandledExceptionDialogForm))
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtDetails = New System.Windows.Forms.TextBox
        Me.btnContinue = New System.Windows.Forms.Button
        Me.btnQuit = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(69, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(448, 62)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = resources.GetString("Label1.Text")
        '
        'txtDetails
        '
        Me.txtDetails.BackColor = System.Drawing.SystemColors.Info
        Me.txtDetails.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetails.ForeColor = System.Drawing.SystemColors.InfoText
        Me.txtDetails.Location = New System.Drawing.Point(15, 94)
        Me.txtDetails.Multiline = True
        Me.txtDetails.Name = "txtDetails"
        Me.txtDetails.ReadOnly = True
        Me.txtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDetails.Size = New System.Drawing.Size(502, 260)
        Me.txtDetails.TabIndex = 1
        '
        'btnContinue
        '
        Me.btnContinue.BackColor = System.Drawing.SystemColors.Control
        Me.btnContinue.DialogResult = System.Windows.Forms.DialogResult.Yes
        Me.btnContinue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnContinue.Location = New System.Drawing.Point(15, 372)
        Me.btnContinue.Name = "btnContinue"
        Me.btnContinue.Size = New System.Drawing.Size(95, 25)
        Me.btnContinue.TabIndex = 2
        Me.btnContinue.Text = "Continue"
        Me.btnContinue.UseVisualStyleBackColor = False
        '
        'btnQuit
        '
        Me.btnQuit.BackColor = System.Drawing.SystemColors.Control
        Me.btnQuit.DialogResult = System.Windows.Forms.DialogResult.No
        Me.btnQuit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnQuit.Location = New System.Drawing.Point(422, 372)
        Me.btnQuit.Name = "btnQuit"
        Me.btnQuit.Size = New System.Drawing.Size(95, 25)
        Me.btnQuit.TabIndex = 3
        Me.btnQuit.Text = "Quit"
        Me.btnQuit.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(12, 412)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(505, 60)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = resources.GetString("Label2.Text")
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(15, 18)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(48, 48)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 5
        Me.PictureBox1.TabStop = False
        '
        'UnhandledExceptionDialogForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(529, 479)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnQuit)
        Me.Controls.Add(Me.btnContinue)
        Me.Controls.Add(Me.txtDetails)
        Me.Controls.Add(Me.Label1)
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "UnhandledExceptionDialogForm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Error"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtDetails As System.Windows.Forms.TextBox
    Friend WithEvents btnContinue As System.Windows.Forms.Button
    Friend WithEvents btnQuit As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
End Class
