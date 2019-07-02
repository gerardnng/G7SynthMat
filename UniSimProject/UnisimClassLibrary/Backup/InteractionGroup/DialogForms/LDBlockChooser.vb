Public Class LDBlockChooser


    Private Const FullHeight As Integer = 175

    Public Sub New(ByVal plist As Pous)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Macintoshize(Me)

        ComboBox1.Items.Clear()
        ComboBox1.Items.AddRange(LDBlocksFactory.ValidBlocks(plist))

    End Sub

    Public Sub New(ByVal plist As Pous, _
        ByVal existingBlock As GraphicalContact, Optional ByVal useNameEditor As Boolean = False)
        Me.New(plist)
        If Not (existingBlock.IsBlock) Then Throw New System.ArgumentException("Trying to edit a non block")
        BlockType = existingBlock.Qualy
        If useNameEditor Then
            BlockName = existingBlock.Name
            Label2.Visible = True
            TextBox1.Visible = True
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Button1.Enabled = (ComboBox1.SelectedIndex >= 0 AndAlso ComboBox1.SelectedIndex < ComboBox1.Items.Count)
    End Sub

    Public Property BlockType() As String
        Get
            Return ComboBox1.SelectedItem.ToString()
        End Get
        Set(ByVal value As String)
            Dim i As Integer = ComboBox1.Items.IndexOf(value)
            ComboBox1.SelectedIndex = IIf(Of Integer)(i >= 0, i, 0)
        End Set
    End Property

    Public Property BlockName() As String
        Get
            Return TextBox1.Text
        End Get
        Set(ByVal value As String)
            TextBox1.Text = value
        End Set
    End Property

End Class