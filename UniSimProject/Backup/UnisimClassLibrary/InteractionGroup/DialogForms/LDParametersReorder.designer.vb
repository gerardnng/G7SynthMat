<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LDParametersReorderDialog
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FBDParametersReorderDialog))
        Me.Label1 = New System.Windows.Forms.Label
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.btnUp = New System.Windows.Forms.Button
        Me.btnDown = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(394, 31)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select the parameter you want to put in a new order and move the arrow buttons ti" & _
            "l it occupies the wanted position"
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(15, 53)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(358, 277)
        Me.ListBox1.TabIndex = 1
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button1.Location = New System.Drawing.Point(175, 344)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Close"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'btnUp
        '
        Me.btnUp.BackColor = System.Drawing.SystemColors.Control
        Me.btnUp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnUp.Image = CType(resources.GetObject("btnUp.Image"), System.Drawing.Image)
        Me.btnUp.Location = New System.Drawing.Point(379, 53)
        Me.btnUp.Name = "btnUp"
        Me.btnUp.Size = New System.Drawing.Size(38, 42)
        Me.btnUp.TabIndex = 3
        Me.btnUp.UseVisualStyleBackColor = False
        '
        'btnDown
        '
        Me.btnDown.BackColor = System.Drawing.SystemColors.Control
        Me.btnDown.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnDown.Image = CType(resources.GetObject("btnDown.Image"), System.Drawing.Image)
        Me.btnDown.Location = New System.Drawing.Point(378, 101)
        Me.btnDown.Name = "btnDown"
        Me.btnDown.Size = New System.Drawing.Size(39, 38)
        Me.btnDown.TabIndex = 4
        Me.btnDown.UseVisualStyleBackColor = False
        '
        'FBDParametersReorder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(426, 379)
        Me.Controls.Add(Me.btnDown)
        Me.Controls.Add(Me.btnUp)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.Label1)
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FBDParametersReorder"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Parameters reorder"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btnUp As System.Windows.Forms.Button
    Friend WithEvents btnDown As System.Windows.Forms.Button
End Class
