Imports System.Windows.Forms

Public Class FBDProtoEditor

    Private m_pou As pou
    Private m_body As body
    Private WithEvents m_FBD As FBD

    Public Sub New()

        ' Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().

    End Sub

    Public Sub New(ByVal p As pou, ByVal b As body)
        Me.New()
        m_pou = p
        m_body = b
        m_FBD = CType(b.ReadImplementation(), FBD)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim selectTarget As New VariablesSelectorDialogForm(m_FBD.ResGlobalVariables, m_FBD.PouInterface)
        If selectTarget.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim boundVar As BaseVariable = selectTarget.m_SelectedVar
            Dim tvrootNode As New TreeNode(boundVar.Name)
            Dim fbdrootNode As New FBDTree(boundVar)
            tvrootNode.Tag = fbdrootNode
            tw.Nodes.Add(tvrootNode)
            m_FBD.AddTree(fbdrootNode)
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim blockChooser As New FBDProtoBlockCreator()
        blockChooser.ShowDialog()
        Dim thetvNode As TreeNode = tw.SelectedNode
        Dim theNewtvNode As New TreeNode(blockChooser.BlockType)
        Dim theNewFbdNode As FBDTreeNode = Nothing
        Select Case blockChooser.BlockType
            Case "AND"
                theNewFbdNode = New AndFBDBlock()
            Case "OR"
                theNewFbdNode = New OrFBDBlock()
            Case "XOR"
                theNewFbdNode = New XorFBDBlock()
            Case "RAND"
                theNewFbdNode = New RandFBDBlock()
        End Select
        theNewtvNode.Tag = theNewFbdNode
        ' Siamo alla radice o in un nodo?
        If TypeOf (thetvNode.Tag) Is FBDTree Then
            ' radice
            Dim theFbdTree As FBDTree = CType(thetvNode.Tag, FBDTree)
            ' il nuovo elemento può andare solo come figlio
            thetvNode.Nodes.Add(theNewtvNode)
            theFbdTree.SetRootNode(theNewFbdNode)
        Else
            ' nodo
            Dim theFbdBlock As FBDTreeNode = CType(thetvNode.Tag, FBDTreeNode)
            ' l'elemento può andare come figlio o come pari
            Dim wantChild As DialogResult = MessageBox.Show("Create as child? Yes for child, No for peer", "UniSim", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If wantChild Then
                thetvNode.Nodes.Add(theNewtvNode)
                theFbdBlock.AddNode(theNewFbdNode)
            Else
                thetvNode.Parent.Nodes.Add(theNewtvNode)
                CType(thetvNode.Parent.Tag, FBDTreeNode).AddNode(theNewFbdNode)
            End If
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim selectTarget As New VariablesSelectorDialogForm(m_FBD.ResGlobalVariables, m_FBD.PouInterface)
        If selectTarget.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim boundVar As BaseVariable = selectTarget.m_SelectedVar
            Dim thetvNode As TreeNode = tw.SelectedNode
            Dim theNewtvNode As New TreeNode(boundVar.Name)
            Dim wantNegated As Boolean = MessageBox.Show("Negate?", "UniSim", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes
            Dim theNewFbdNode As New VariableBoundFDBTreeNode(boundVar, wantNegated)
            If wantNegated Then theNewtvNode.Text = "!" + theNewtvNode.Text
            theNewtvNode.Tag = theNewFbdNode
            ' Siamo alla radice o in un nodo?
            If TypeOf (thetvNode.Tag) Is FBDTree Then
                ' radice
                Dim theFbdTree As FBDTree = CType(thetvNode.Tag, FBDTree)
                ' il nuovo elemento può andare solo come figlio
                thetvNode.Nodes.Add(theNewtvNode)
                theFbdTree.SetRootNode(theNewFbdNode)
            Else
                ' nodo
                Dim theFbdBlock As FBDTreeNode = CType(thetvNode.Tag, FBDTreeNode)
                ' l'elemento può andare come figlio o come pari
                Dim wantChild As DialogResult = MessageBox.Show("Create as child? Yes for child, No for peer", "UniSim", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If wantChild Then
                    thetvNode.Nodes.Add(theNewtvNode)
                    theFbdBlock.AddNode(theNewFbdNode)
                Else
                    thetvNode.Parent.Nodes.Add(theNewtvNode)
                    CType(thetvNode.Parent.Tag, FBDTreeNode).AddNode(theNewFbdNode)
                End If
            End If

        End If
    End Sub

End Class