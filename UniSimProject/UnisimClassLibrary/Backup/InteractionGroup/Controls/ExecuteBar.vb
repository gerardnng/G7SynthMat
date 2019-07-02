Public Class ExecuteBar
    Inherits Windows.Forms.UserControl

    Protected m_Count As Integer

#Region " Codice generato da Progettazione Windows Form "

    Public Sub New()
        MyBase.New()

        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()

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
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox6 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox7 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox8 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox9 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox10 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.TextBox5 = New System.Windows.Forms.TextBox
        Me.TextBox6 = New System.Windows.Forms.TextBox
        Me.TextBox7 = New System.Windows.Forms.TextBox
        Me.TextBox8 = New System.Windows.Forms.TextBox
        Me.TextBox9 = New System.Windows.Forms.TextBox
        Me.TextBox10 = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.Color.Gray
        Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Left
        Me.TextBox1.Enabled = False
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(0, 0)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(8, 17)
        Me.TextBox1.TabIndex = 8
        Me.TextBox1.Text = ""
        '
        'TextBox2
        '
        Me.TextBox2.BackColor = System.Drawing.Color.Gray
        Me.TextBox2.Dock = System.Windows.Forms.DockStyle.Left
        Me.TextBox2.Enabled = False
        Me.TextBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(8, 0)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(8, 17)
        Me.TextBox2.TabIndex = 9
        Me.TextBox2.Text = ""
        '
        'TextBox3
        '
        Me.TextBox3.BackColor = System.Drawing.Color.Gray
        Me.TextBox3.Dock = System.Windows.Forms.DockStyle.Left
        Me.TextBox3.Enabled = False
        Me.TextBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox3.Location = New System.Drawing.Point(16, 0)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(8, 17)
        Me.TextBox3.TabIndex = 10
        Me.TextBox3.Text = ""
        '
        'TextBox4
        '
        Me.TextBox4.BackColor = System.Drawing.Color.Gray
        Me.TextBox4.Dock = System.Windows.Forms.DockStyle.Left
        Me.TextBox4.Enabled = False
        Me.TextBox4.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox4.Location = New System.Drawing.Point(24, 0)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(8, 17)
        Me.TextBox4.TabIndex = 11
        Me.TextBox4.Text = ""
        '
        'TextBox5
        '
        Me.TextBox5.BackColor = System.Drawing.Color.Gray
        Me.TextBox5.Dock = System.Windows.Forms.DockStyle.Left
        Me.TextBox5.Enabled = False
        Me.TextBox5.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox5.Location = New System.Drawing.Point(32, 0)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(8, 17)
        Me.TextBox5.TabIndex = 12
        Me.TextBox5.Text = ""
        '
        'TextBox6
        '
        Me.TextBox6.BackColor = System.Drawing.Color.Gray
        Me.TextBox6.Dock = System.Windows.Forms.DockStyle.Left
        Me.TextBox6.Enabled = False
        Me.TextBox6.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox6.Location = New System.Drawing.Point(40, 0)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.Size = New System.Drawing.Size(8, 17)
        Me.TextBox6.TabIndex = 13
        Me.TextBox6.Text = ""
        '
        'TextBox7
        '
        Me.TextBox7.BackColor = System.Drawing.Color.Gray
        Me.TextBox7.Dock = System.Windows.Forms.DockStyle.Left
        Me.TextBox7.Enabled = False
        Me.TextBox7.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox7.Location = New System.Drawing.Point(48, 0)
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.Size = New System.Drawing.Size(8, 17)
        Me.TextBox7.TabIndex = 14
        Me.TextBox7.Text = ""
        '
        'TextBox8
        '
        Me.TextBox8.BackColor = System.Drawing.Color.Gray
        Me.TextBox8.Dock = System.Windows.Forms.DockStyle.Left
        Me.TextBox8.Enabled = False
        Me.TextBox8.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox8.Location = New System.Drawing.Point(56, 0)
        Me.TextBox8.Name = "TextBox8"
        Me.TextBox8.Size = New System.Drawing.Size(8, 17)
        Me.TextBox8.TabIndex = 15
        Me.TextBox8.Text = ""
        '
        'TextBox9
        '
        Me.TextBox9.BackColor = System.Drawing.Color.Gray
        Me.TextBox9.Dock = System.Windows.Forms.DockStyle.Left
        Me.TextBox9.Enabled = False
        Me.TextBox9.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox9.Location = New System.Drawing.Point(64, 0)
        Me.TextBox9.Name = "TextBox9"
        Me.TextBox9.Size = New System.Drawing.Size(8, 17)
        Me.TextBox9.TabIndex = 16
        Me.TextBox9.Text = ""
        '
        'TextBox10
        '
        Me.TextBox10.BackColor = System.Drawing.Color.Gray
        Me.TextBox10.Dock = System.Windows.Forms.DockStyle.Left
        Me.TextBox10.Enabled = False
        Me.TextBox10.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox10.Location = New System.Drawing.Point(72, 0)
        Me.TextBox10.Name = "TextBox10"
        Me.TextBox10.Size = New System.Drawing.Size(8, 17)
        Me.TextBox10.TabIndex = 17
        Me.TextBox10.Text = ""
        '
        'ExecuteBar
        '
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.TextBox4)
        Me.Controls.Add(Me.TextBox5)
        Me.Controls.Add(Me.TextBox6)
        Me.Controls.Add(Me.TextBox7)
        Me.Controls.Add(Me.TextBox8)
        Me.Controls.Add(Me.TextBox9)
        Me.Controls.Add(Me.TextBox10)
        Me.Name = "ExecuteBar"
        Me.Size = New System.Drawing.Size(80, 18)
        Me.ResumeLayout(False)

    End Sub


#End Region

    Public Property Count() As Integer
        Get
            Return m_Count
        End Get
        Set(ByVal value As Integer)
            m_Count = value
        End Set
    End Property

    Public Sub NextValue()
        Try

            Me.Controls(Count).BackColor = Drawing.Color.Gray
            Count = Count - 1
            If Count = -1 Then
                Count = 9
            End If
            Me.Controls(Count).BackColor = Drawing.Color.Red

        Catch ex As Exception
        End Try
    End Sub

End Class
