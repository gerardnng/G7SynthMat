<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FBDProtoEditor
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.tw = New System.Windows.Forms.TreeView
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'tw
        '
        Me.tw.Location = New System.Drawing.Point(12, 12)
        Me.tw.Name = "tw"
        Me.tw.Size = New System.Drawing.Size(524, 522)
        Me.tw.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 540)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(111, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Create Root Node"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(129, 540)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(161, 23)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "Create variable-bound node"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(129, 569)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(127, 23)
        Me.Button3.TabIndex = 3
        Me.Button3.Text = "Create block node"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'FBDProtoEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(548, 626)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.tw)
        Me.MaximizeBox = False
        Me.Name = "FBDProtoEditor"
        Me.Text = "FBDProtoEditor"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tw As System.Windows.Forms.TreeView
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
End Class
