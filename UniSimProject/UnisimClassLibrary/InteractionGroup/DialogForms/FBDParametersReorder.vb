Public Class FBDParametersReorderDialog

    Class ParameterListBoxItem
        Public Body As FBD
        Public ParameterEntry As IFBDConnectable
        Public ParameterIndex As Integer
        Public Sub New(ByVal b As FBD, ByVal e As IFBDConnectable, ByVal i As Integer)
            Body = b
            ParameterEntry = e
            ParameterIndex = i
        End Sub
        Public Overrides Function ToString() As String
            Return ParameterEntry.GetIdentifier() + " " + ParameterEntry.Name
        End Function
    End Class

    Private m_Body As FBD
    Private m_Block As GraphicalFBDBlock

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(ByVal body As FBD, ByVal block As GraphicalFBDBlock)
        Me.New()
        m_Body = body
        m_Block = block
    End Sub

    Private Sub LoadParameters()
        ListBox1.Items.Clear()
        For Each i As Integer In m_Block.GetMyArgumentsIndexes()
            ListBox1.Items.Add(New ParameterListBoxItem(m_Body, m_Body.GraphicalConnectionsList.Item(i).SourceObject, i))
        Next
        btnUp.Enabled = False
        btnDown.Enabled = False
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Hide()
    End Sub

    Private Sub FBDParametersReorder_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadParameters()
    End Sub

    Private Sub ListBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.Click
        btnUp.Enabled = (ListBox1.SelectedIndex > 0)
        btnDown.Enabled = (ListBox1.SelectedIndex >= 0) AndAlso (ListBox1.SelectedIndex < ListBox1.Items.Count - 1)
    End Sub

    Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click
        ' Nessuno di questi indici può andare off-by-one perchè non permettiamo di cliccare i bottoni
        ' se l'elemento selezionato non può essere spostato in alto
        Dim curItem As ParameterListBoxItem = CType(ListBox1.SelectedItem, ParameterListBoxItem)
        Dim prevItem As ParameterListBoxItem = CType(ListBox1.Items(ListBox1.SelectedIndex - 1), _
            ParameterListBoxItem)
        Swap(Of GraphicalFBDConnection)(m_Body.GraphicalConnectionsList(curItem.ParameterIndex), _
            m_Body.GraphicalConnectionsList(prevItem.ParameterIndex))
        LoadParameters()
    End Sub

    Private Sub btnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDown.Click
        ' Nessuno di questi indici può andare off-by-one perchè non permettiamo di cliccare i bottoni
        ' se l'elemento selezionato non può essere spostato in basso
        Dim curItem As ParameterListBoxItem = CType(ListBox1.SelectedItem, ParameterListBoxItem)
        Dim nextItem As ParameterListBoxItem = CType(ListBox1.Items(ListBox1.SelectedIndex + 1), _
            ParameterListBoxItem)
        Swap(Of GraphicalFBDConnection)(m_Body.GraphicalConnectionsList(curItem.ParameterIndex), _
            m_Body.GraphicalConnectionsList(nextItem.ParameterIndex))
        LoadParameters()
    End Sub
End Class