Imports UnisimClassLibrary
Imports System.Windows.Forms
Public Class PousTree
    Inherits System.Windows.Forms.TreeView
    Private m_mdiParentForm As Form
    Private m_project As Project
    Private m_pouslist As Pous


#Region " Codice generato da Progettazione Windows Form "

    Public Sub New()
        MyBase.New()

        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()
    End Sub

    'UserControl esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
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

    Friend WithEvents MenuNodePous As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuPousSet As System.Windows.Forms.MenuItem
    Friend WithEvents MenuPousRemove As System.Windows.Forms.MenuItem

    Friend WithEvents MenuBody As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuPousShowBody As System.Windows.Forms.MenuItem

    Friend WithEvents MenuVarsList As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuVarsListOpen As System.Windows.Forms.MenuItem



    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.MenuNodePous = New System.Windows.Forms.ContextMenu
        Me.MenuPousSet = New System.Windows.Forms.MenuItem
        Me.MenuPousRemove = New System.Windows.Forms.MenuItem
        Me.MenuBody = New System.Windows.Forms.ContextMenu
        Me.MenuPousShowBody = New System.Windows.Forms.MenuItem
        Me.MenuVarsList = New System.Windows.Forms.ContextMenu
        Me.MenuVarsListOpen = New System.Windows.Forms.MenuItem
        Me.SuspendLayout()
        '
        'MenuNodePous
        '
        Me.MenuNodePous.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuPousSet, Me.MenuPousRemove})
        '
        'MenuPousSet
        '
        Me.MenuPousSet.Index = 0
        Me.MenuPousSet.Text = "Show info"
        '
        'MenuPousRemove
        '
        Me.MenuPousRemove.Index = 1
        Me.MenuPousRemove.Text = "Remove pou"
        '
        'MenuBody
        '
        Me.MenuBody.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuPousShowBody})
        '
        'MenuPousShowBody
        '
        Me.MenuPousShowBody.Index = 0
        Me.MenuPousShowBody.Text = "Show body"
        '
        'MenuVarsList
        '
        Me.MenuVarsList.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuVarsListOpen})
        '
        'MenuVarsListOpen
        '
        Me.MenuVarsListOpen.Index = 0
        Me.MenuVarsListOpen.Text = "Open list"
        '
        'PousTree
        '
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public Property MdiParentForm() As Form
        Get
            MdiParentForm = m_mdiParentForm
        End Get
        Set(ByVal Value As Form)
            m_mdiParentForm = Value
        End Set
    End Property
    Public Property Project() As Project
        Get
            Project = m_project
        End Get
        Set(ByVal Value As Project)
            If Not IsNothing(Value) Then
                m_project = Value
                m_pouslist = m_project.Types.pous
            End If
        End Set
    End Property
    Public Sub RefreshTreeStruct()
        If Not IsNothing(m_pouslist) Then
            Me.SuspendLayout()
            Nodes.Clear()
            'Aggiunge i nodi delle Pous
            For Each P As pou In m_pouslist
                'Crea il nodo Pou
                Dim PouNode As New TreeNode(P.Name)
                'Aggiunge il nodo Pou
                Me.Nodes.Add(PouNode)
                'Riempie il nodo Pou
                PouNode.Nodes.Add(New TreeNode("Local variables"))
                If P.PouType = EnumPouType.Function Then _
                    PouNode.Nodes.Add(New TreeNode("Input variables"))
                PouNode.Nodes.Add(New TreeNode("Body"))
            Next P
            Me.ResumeLayout()
        End If
    End Sub
    Private Sub ShowVariablesList(ByVal Lists As VariablesLists, ByRef List As VariablesList)
        'Controlla se il form esiste già
        Dim Find As Boolean
        For Each Fr As Form In m_mdiParentForm.MdiChildren
            'Controlla se è un form VariablesListControlPanel
            If Fr.GetType.Name = "VariablesListControlPanel" Then
                Dim FrVariablesListControlPanel As VariablesListControlPanel = Fr
                If FrVariablesListControlPanel.ReadList Is List Then
                    Fr.Activate()
                    Find = True
                    Exit For
                End If
            End If
        Next Fr
        'Se non lo ha trovato lo crea e lo visualizza
        If Not Find Then
            Dim FrVariablesListControlPanel As New VariablesListControlPanel(Lists, List, True)
            FrVariablesListControlPanel.MdiParent = m_mdiParentForm
            FrVariablesListControlPanel.Show()
            'Attiva il monitoring delle variabili
            FrVariablesListControlPanel.StartMonitor()
        End If
    End Sub
    'Funzioni di gestione POU
    Private Sub RemovePOU(ByRef RefPou As pou)
        If MsgBox("Do you really want to delete " & RefPou.Name & "?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, _
        UniSimVersion.VersionInfo.PrintableDescriptionForTool("")) = MsgBoxResult.Yes Then
            ' Se c'è una istanza della POU cancella anche lei
            If Not IsNothing(m_project.DefaultResource.FindPouInstanceByName(RefPou.Name)) Then
                m_project.DefaultResource.pouInstances.Remove(m_project.DefaultResource.FindPouInstanceByName(RefPou.Name))
                ' in questo caso l'uso dell'hack IUniSimMainWindow è meno "pesante" in quanto il PousTree contiene
                ' già un riferimento al Form che lo contiene
                CType(Me.MdiParentForm, IUniSimMainWindow).GetResourceTree().RefreshTreeStruct()
            End If
            'Rimuove il POU dalla lista
            m_project.Types.pous.RemovePou(RefPou)
            RefreshTreeStruct()
        End If
    End Sub
    Public Sub OpenPOUBody(ByRef RefPou As pou)
        ShowBody(RefPou)
    End Sub
    Public Sub OpenPOUVarsList(ByRef RefPou As pou)
        ShowVariablesList(RefPou.PouInterface.variablesLists, RefPou.PouInterface.localVars)
    End Sub
    Public Sub OpenPOUInputVarsList(ByRef RefPou As pou)
        ShowVariablesList(RefPou.PouInterface.variablesLists, RefPou.PouInterface.inputVars)
    End Sub
    Private Sub ShowBody(ByRef RefPou As pou)
        'Se è in stato di simulazione fa partire il monitor
        'Controlla se il form esiste già
        Dim Find As Boolean = False
        For Each Fr As Form In m_mdiParentForm.MdiChildren
            If TypeOf (Fr) Is IEditorForm Then
                If CType(Fr, IEditorForm).ReadBody Is RefPou.Body Then
                    Fr.Activate()
                    Find = True
                    Exit For
                End If
            End If
        Next Fr

        'Se non lo ha trovato lo crea e lo visualizza
        If Not Find Then
            'Visualizza il form
            If RefPou.Body.ReadBodyType = EnumBodyType.tLD Then
                Dim FrLddTemp As New FormLadder(RefPou, RefPou.Body, True)
                FrLddTemp.MdiParent = m_mdiParentForm
                FrLddTemp.Show()
            ElseIf RefPou.Body.ReadBodyType = EnumBodyType.tSFC Then
                Dim FrSfcTemp As New FormSfc(RefPou, RefPou.Body, True)
                FrSfcTemp.MdiParent = m_mdiParentForm
                FrSfcTemp.Show()
            ElseIf RefPou.Body.ReadBodyType = EnumBodyType.tFBD Then
                Dim FrFbdTemp As New FormFBD(RefPou, RefPou.Body, True)
                FrFbdTemp.MdiParent = m_mdiParentForm
                FrFbdTemp.Show()
            End If
        End If
    End Sub
    Private Sub SetPOUInfo(ByRef RefPou As pou)
        Dim pouDialog As New pouDialogForm(RefPou)
        Dim ResultDialog As DialogResult = pouDialog.ShowDialog()
        If ResultDialog = DialogResult.OK Then
            'Se il nome è cambiato controlla se un pou con lo stesso nome esiste già
            If RefPou.Name <> pouDialog.m_Name Then
                If Not IsNothing(m_project.Types.pous.FindpouByName(pouDialog.m_Name)) Then
                    MsgBox("POU " & pouDialog.m_Name & " already exists", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, _
                    UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
                Else
                    'Aggiorna i dati
                    RefPou.Name = pouDialog.m_Name
                    RefreshTreeStruct()
                End If
            Else
                'Non aggiorna il nome
                RefreshTreeStruct()
            End If
        End If
        pouDialog.Dispose()
    End Sub
    Private Sub PousTree_NodeDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) _
        Handles Me.NodeMouseDoubleClick
        Dim selNode As TreeNode = e.Node
        If Not IsNothing(selNode) AndAlso Not IsNothing(selNode.Parent) Then
            'Verifica il tipo di nodo dal nome del nodo padre
            Select Case selNode.Text
                Case "Local variables"
                    Dim P As pou = m_pouslist.FindpouByName(selNode.Parent.Text)
                    If Not IsNothing(P) Then
                        Dim List As VariablesList = P.PouInterface.localVars
                        Me.ShowVariablesList(P.PouInterface.variablesLists, List)
                    End If
                Case "Input variables"
                    Dim P As pou = m_pouslist.FindpouByName(selNode.Parent.Text)
                    If Not IsNothing(P) Then
                        Dim List As VariablesList = P.PouInterface.inputVars
                        Me.ShowVariablesList(P.PouInterface.variablesLists, List)
                    End If
                Case "Body"
                    Dim P As pou = m_pouslist.FindpouByName(selNode.Parent.Text)
                    If Not IsNothing(P) Then
                        Me.ShowBody(P)
                    End If
            End Select
        End If
    End Sub
    Private Sub PousTree_DoubleClick(ByVal Obg As Object, ByVal e As System.EventArgs) Handles Me.DoubleClick
        If Not IsNothing(SelectedNode) Then
            If Not IsNothing(SelectedNode.Parent) Then
                'Verifica il tipo di nodo dal nome del nodo padre
                Select Case SelectedNode.Text
                    Case "Local variables"
                        Dim P As pou = m_pouslist.FindpouByName(SelectedNode.Parent.Text)
                        If Not IsNothing(P) Then
                            Dim List As VariablesList = P.PouInterface.localVars
                            Me.ShowVariablesList(P.PouInterface.variablesLists, List)
                        End If
                    Case "Input variables"
                        Dim P As pou = m_pouslist.FindpouByName(SelectedNode.Parent.Text)
                        If Not IsNothing(P) Then
                            Dim List As VariablesList = P.PouInterface.inputVars
                            Me.ShowVariablesList(P.PouInterface.variablesLists, List)
                        End If
                    Case "Body"
                        Dim P As pou = m_pouslist.FindpouByName(SelectedNode.Parent.Text)
                        If Not IsNothing(P) Then
                            Me.ShowBody(P)
                        End If
                End Select
            End If
        End If
    End Sub
    Private Sub TreeView1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        If Not IsNothing(Me.GetNodeAt(e.X, e.Y)) Then
            Me.SelectedNode = Me.GetNodeAt(e.X, e.Y)
            'Se è il tasto destro associa il menu contestuale
            If e.Button = Windows.Forms.MouseButtons.Right Then
                If Not IsNothing(SelectedNode) Then
                    If Not IsNothing(SelectedNode.Parent) Then
                        'Verifica il tipo di nodo dal nome del nodo padre
                        Select Case SelectedNode.Text
                            Case "Local variables", "Input variables"
                                ContextMenu = Me.MenuVarsList
                            Case "Body"
                                ContextMenu = Me.MenuBody
                            Case Else
                                ContextMenu = Nothing
                        End Select
                    Else
                        'Il nodo padre è nullo
                        ContextMenu = Me.MenuNodePous
                    End If
                Else
                    ContextMenu = Nothing
                End If
            End If
        End If
    End Sub
    Private Sub MenuShowBody_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuPousShowBody.Click
        Dim P As pou = m_pouslist.FindpouByName(Me.SelectedNode.Parent.Text)
        If Not IsNothing(P) Then
            ShowBody(P)
        End If
    End Sub
    Private Sub MenuPousRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuPousRemove.Click
        Dim P As pou = m_pouslist.FindpouByName(Me.SelectedNode.Text)
        If Not IsNothing(P) Then
            RemovePOU(P)
        End If
    End Sub
    Private Sub MenuPousSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuPousSet.Click
        Dim P As pou = m_pouslist.FindpouByName(Me.SelectedNode.Text)
        If Not IsNothing(P) Then
            SetPOUInfo(P)
        End If
    End Sub
    Private Sub MenuVarsListOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuVarsListOpen.Click
        Dim L As VariablesList
        Select Case Me.SelectedNode.Text
            'Seleziona la lista di variabili da aprire
            Case "Local variables"
                Dim P As pou = m_pouslist.FindpouByName(Me.SelectedNode.Parent.Text)
                If Not IsNothing(P) Then
                    L = P.PouInterface.localVars
                    If Not IsNothing(L) Then
                        ShowVariablesList(P.PouInterface.variablesLists, L)
                    End If
                End If
            Case "Input variables"
                Dim P As pou = m_pouslist.FindpouByName(Me.SelectedNode.Parent.Text)
                If Not IsNothing(P) Then
                    L = P.PouInterface.inputVars
                    If Not IsNothing(L) Then
                        ShowVariablesList(P.PouInterface.variablesLists, L)
                    End If
                End If
        End Select
    End Sub
End Class
