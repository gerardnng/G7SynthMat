<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ResizePanelDialogForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ResizePanelDialogForm))
        Me.rdbIncreaseSize = New System.Windows.Forms.RadioButton
        Me.rdbDecreaseSize = New System.Windows.Forms.RadioButton
        Me.chkX = New System.Windows.Forms.CheckBox
        Me.chkY = New System.Windows.Forms.CheckBox
        Me.tkbAmount = New System.Windows.Forms.TrackBar
        Me.lblReadAmount = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rdbPixels = New System.Windows.Forms.RadioButton
        Me.rdbPercentage = New System.Windows.Forms.RadioButton
        CType(Me.tkbAmount, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'rdbIncreaseSize
        '
        Me.rdbIncreaseSize.AutoSize = True
        Me.rdbIncreaseSize.Checked = True
        Me.rdbIncreaseSize.Location = New System.Drawing.Point(12, 23)
        Me.rdbIncreaseSize.Name = "rdbIncreaseSize"
        Me.rdbIncreaseSize.Size = New System.Drawing.Size(147, 17)
        Me.rdbIncreaseSize.TabIndex = 0
        Me.rdbIncreaseSize.TabStop = True
        Me.rdbIncreaseSize.Text = "Increase workseet size by"
        Me.rdbIncreaseSize.UseVisualStyleBackColor = True
        '
        'rdbDecreaseSize
        '
        Me.rdbDecreaseSize.AutoSize = True
        Me.rdbDecreaseSize.Location = New System.Drawing.Point(12, 46)
        Me.rdbDecreaseSize.Name = "rdbDecreaseSize"
        Me.rdbDecreaseSize.Size = New System.Drawing.Size(152, 17)
        Me.rdbDecreaseSize.TabIndex = 0
        Me.rdbDecreaseSize.Text = "Decrease workseet size by"
        Me.rdbDecreaseSize.UseVisualStyleBackColor = True
        '
        'chkX
        '
        Me.chkX.AutoSize = True
        Me.chkX.Checked = True
        Me.chkX.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkX.Location = New System.Drawing.Point(95, 256)
        Me.chkX.Name = "chkX"
        Me.chkX.Size = New System.Drawing.Size(83, 17)
        Me.chkX.TabIndex = 2
        Me.chkX.Text = "along X axis"
        Me.chkX.UseVisualStyleBackColor = True
        '
        'chkY
        '
        Me.chkY.AutoSize = True
        Me.chkY.Checked = True
        Me.chkY.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkY.Location = New System.Drawing.Point(95, 279)
        Me.chkY.Name = "chkY"
        Me.chkY.Size = New System.Drawing.Size(83, 17)
        Me.chkY.TabIndex = 2
        Me.chkY.Text = "along Y axis"
        Me.chkY.UseVisualStyleBackColor = True
        '
        'tkbAmount
        '
        Me.tkbAmount.Location = New System.Drawing.Point(95, 83)
        Me.tkbAmount.Maximum = 100
        Me.tkbAmount.Name = "tkbAmount"
        Me.tkbAmount.Size = New System.Drawing.Size(354, 45)
        Me.tkbAmount.TabIndex = 3
        Me.tkbAmount.TickFrequency = 10
        Me.tkbAmount.Value = 10
        '
        'lblReadAmount
        '
        Me.lblReadAmount.AutoSize = True
        Me.lblReadAmount.Location = New System.Drawing.Point(92, 146)
        Me.lblReadAmount.Name = "lblReadAmount"
        Me.lblReadAmount.Size = New System.Drawing.Size(19, 13)
        Me.lblReadAmount.TabIndex = 4
        Me.lblReadAmount.Text = "10"
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button1.Location = New System.Drawing.Point(12, 335)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.Control
        Me.Button2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button2.Location = New System.Drawing.Point(409, 335)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rdbPixels)
        Me.Panel1.Controls.Add(Me.rdbPercentage)
        Me.Panel1.Location = New System.Drawing.Point(95, 175)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(200, 51)
        Me.Panel1.TabIndex = 7
        '
        'rdbPixels
        '
        Me.rdbPixels.AutoSize = True
        Me.rdbPixels.Location = New System.Drawing.Point(8, 26)
        Me.rdbPixels.Name = "rdbPixels"
        Me.rdbPixels.Size = New System.Drawing.Size(51, 17)
        Me.rdbPixels.TabIndex = 3
        Me.rdbPixels.Text = "pixels"
        Me.rdbPixels.UseVisualStyleBackColor = True
        '
        'rdbPercentage
        '
        Me.rdbPercentage.AutoSize = True
        Me.rdbPercentage.Checked = True
        Me.rdbPercentage.Location = New System.Drawing.Point(8, 3)
        Me.rdbPercentage.Name = "rdbPercentage"
        Me.rdbPercentage.Size = New System.Drawing.Size(61, 17)
        Me.rdbPercentage.TabIndex = 2
        Me.rdbPercentage.TabStop = True
        Me.rdbPercentage.Text = "percent"
        Me.rdbPercentage.UseVisualStyleBackColor = True
        '
        'ResizePanelDialogForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(496, 378)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.lblReadAmount)
        Me.Controls.Add(Me.tkbAmount)
        Me.Controls.Add(Me.chkY)
        Me.Controls.Add(Me.chkX)
        Me.Controls.Add(Me.rdbDecreaseSize)
        Me.Controls.Add(Me.rdbIncreaseSize)
        Me.ForeColor = System.Drawing.Color.White
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ResizePanelDialogForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Resize worksheet"
        CType(Me.tkbAmount, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents rdbIncreaseSize As System.Windows.Forms.RadioButton
    Friend WithEvents rdbDecreaseSize As System.Windows.Forms.RadioButton
    Friend WithEvents chkX As System.Windows.Forms.CheckBox
    Friend WithEvents chkY As System.Windows.Forms.CheckBox
    Friend WithEvents tkbAmount As System.Windows.Forms.TrackBar
    Friend WithEvents lblReadAmount As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rdbPixels As System.Windows.Forms.RadioButton
    Friend WithEvents rdbPercentage As System.Windows.Forms.RadioButton
End Class
