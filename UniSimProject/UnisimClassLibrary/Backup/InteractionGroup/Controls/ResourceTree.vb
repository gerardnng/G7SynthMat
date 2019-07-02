Imports UnisimClassLibrary
Imports System.Windows.Forms
Public Class ResourceTree
    Inherits System.Windows.Forms.TreeView
    Private m_mdiParentForm As Form
    Private m_project As Project
    Private m_resource As Resource


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

    Friend WithEvents MenuNodeGlobalVars As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuGlobalVarsAdd As System.Windows.Forms.MenuItem

    Friend WithEvents MenuNodeTasks As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuTasksAdd As System.Windows.Forms.MenuItem

    Friend WithEvents MenuNodePouInstances As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuPouInstancesAdd As System.Windows.Forms.MenuItem

    Friend WithEvents MenuGlobalVars As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuGlobalVarsOpen As System.Windows.Forms.MenuItem
    Friend WithEvents MenuGlobalVarsRename As System.Windows.Forms.MenuItem
    Friend WithEvents MenuGlobalVarsRemove As System.Windows.Forms.MenuItem

    Friend WithEvents MenuTasks As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuTasksSet As System.Windows.Forms.MenuItem
    Friend WithEvents MenuTasksRemove As System.Windows.Forms.MenuItem

    Friend WithEvents MenuPouInstances As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuPouInstancesSet As System.Windows.Forms.MenuItem
    Friend WithEvents MenuPouInstancesRemove As System.Windows.Forms.MenuItem



    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.MenuNodeGlobalVars = New System.Windows.Forms.ContextMenu
        Me.MenuGlobalVarsAdd = New System.Windows.Forms.MenuItem
        Me.MenuNodeTasks = New System.Windows.Forms.ContextMenu
        Me.MenuTasksAdd = New System.Windows.Forms.MenuItem
        Me.MenuNodePouInstances = New System.Windows.Forms.ContextMenu
        Me.MenuPouInstancesAdd = New System.Windows.Forms.MenuItem
        Me.MenuGlobalVars = New System.Windows.Forms.ContextMenu
        Me.MenuGlobalVarsOpen = New System.Windows.Forms.MenuItem
        Me.MenuGlobalVarsRename = New System.Windows.Forms.MenuItem
        Me.MenuGlobalVarsRemove = New System.Windows.Forms.MenuItem
        Me.MenuTasks = New System.Windows.Forms.ContextMenu
        Me.MenuTasksSet = New System.Windows.Forms.MenuItem
        Me.MenuTasksRemove = New System.Windows.Forms.MenuItem
        Me.MenuPouInstances = New System.Windows.Forms.ContextMenu
        Me.MenuPouInstancesSet = New System.Windows.Forms.MenuItem
        Me.MenuPouInstancesRemove = New System.Windows.Forms.MenuItem
        Me.SuspendLayout()
        '
        'MenuNodeGlobalVars
        '
        Me.MenuNodeGlobalVars.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuGlobalVarsAdd})
        '
        'MenuGlobalVarsAdd
        '
        Me.MenuGlobalVarsAdd.Index = 0
        Me.MenuGlobalVarsAdd.Text = "Add list"
        '
        'MenuNodeTasks
        '
        Me.MenuNodeTasks.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuTasksAdd})
        '
        'MenuTasksAdd
        '
        Me.MenuTasksAdd.Index = 0
        Me.MenuTasksAdd.Text = "Add task"
        '
        'MenuNodePouInstances
        '
        Me.MenuNodePouInstances.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuPouInstancesAdd})
        '
        'MenuPouInstancesAdd
        '
        Me.MenuPouInstancesAdd.Index = 0
        Me.MenuPouInstancesAdd.Text = "Add Pou instances"
        '
        'MenuGlobalVars
        '
        Me.MenuGlobalVars.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuGlobalVarsOpen, Me.MenuGlobalVarsRename, Me.MenuGlobalVarsRemove})
        '
        'MenuGlobalVarsOpen
        '
        Me.MenuGlobalVarsOpen.Index = 0
        Me.MenuGlobalVarsOpen.Text = "Open list"
        '
        'MenuGlobalVarsRename
        '
        Me.MenuGlobalVarsRename.Index = 1
        Me.MenuGlobalVarsRename.Text = "Rename list"
        '
        'MenuGlobalVarsRemove
        '
        Me.MenuGlobalVarsRemove.Index = 2
        Me.MenuGlobalVarsRemove.Text = "Remove list"
        '
        'MenuTasks
        '
        Me.MenuTasks.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuTasksSet, Me.MenuTasksRemove})
        '
        'MenuTasksSet
        '
        Me.MenuTasksSet.Index = 0
        Me.MenuTasksSet.Text = "Open task"
        '
        'MenuTasksRemove
        '
        Me.MenuTasksRemove.Index = 1
        Me.MenuTasksRemove.Text = "Remove task"
        '
        'MenuPouInstances
        '
        Me.MenuPouInstances.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuPouInstancesSet, Me.MenuPouInstancesRemove})
        '
        'MenuPouInstancesSet
        '
        Me.MenuPouInstancesSet.Index = 0
        Me.MenuPouInstancesSet.Text = "Open Pou instance"
        '
        'MenuPouInstancesRemove
        '
        Me.MenuPouInstancesRemove.Index = 1
        Me.MenuPouInstancesRemove.Text = "Remove Pou instances"
        '
        'ResourceTree
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
                If Not IsNothing(m_project.Istances.configurations(0)) Then
                    If Not IsNothing(m_project.Istances.configurations(0).resources(0)) Then
                        m_resource = m_project.Istances.configurations(0).resources(0)
                    End If
                End If
            End If
        End Set
    End Property
    Public Sub RefreshTreeStruct()
        If Not IsNothing(m_resource) Then
            Me.SuspendLayout()
            Nodes.Clear()
            'Crea il nodo variabili globali
            Dim GlobalVariablesNode As New TreeNode("Global variables")
            'Aggiunge il nodo delle variabili globali
            Nodes.Add(GlobalVariablesNode)
            For Each List As VariablesList In m_resource.globalVars
                'Crea i nodi delle singole liste di variabili
                Dim ListNode As New TreeNode(List.Name)
                'Aggiunge la lista al nodo delle variabili globali
                GlobalVariablesNode.Nodes.Add(ListNode)
            Next List

            'Crea il nodo tasks e lo aggiune
            Dim TasksNode As New TreeNode("Tasks")
            Nodes.Add(TasksNode)
            For Each T As Task In m_resource.tasks
                'Crea il nodo task e lo aggiune
                Dim TaskNode As New TreeNode(T.name)
                TasksNode.Nodes.Add(TaskNode)
            Next T
            'Crea il nodo PouInstances e lo aggiunge
            Dim PouInstancesNode As New TreeNode("POUs instances")
            Nodes.Add(PouInstancesNode)
            For Each PI As pouInstance In m_resource.pouInstances
                Dim TaskPouInstanceNode As TreeNode = CreatePouInstanceNode(PI)
                PouInstancesNode.Nodes.Add(TaskPouInstanceNode)
            Next PI
            Me.ResumeLayout()
        End If
    End Sub
    Private Function CreatePouInstanceNode(ByVal PI As pouInstance) As TreeNode
        Return New TreeNode(PI.name)
    End Function
    Private Sub ShowGlobalVariablesList(ByRef List As VariablesList)
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
            Dim FrVariablesListControlPanel As New VariablesListControlPanel(m_resource.globalVars, List, True)
            FrVariablesListControlPanel.MdiParent = m_mdiParentForm
            FrVariablesListControlPanel.Show()
            'Attiva il monitoring delle variabili
            FrVariablesListControlPanel.StartMonitor()
        End If
    End Sub
    Private Sub AddGlobalVariablesList()
        Dim VariablesDialogDlg As New VariablesListDialogForm
        Dim ResultDialog As DialogResult = VariablesDialogDlg.ShowDialog()
        If ResultDialog = DialogResult.OK Then _
            If Not (AddGlobalVariablesList(VariablesDialogDlg.m_Name)) Then _
                MessageBox.Show("Global variables list " + VariablesDialogDlg.m_Name + " already exists", _
                    UniSimVersion.VersionInfo.PrintableDescriptionForTool(), MessageBoxButtons.OK, _
                     MessageBoxIcon.Stop)
        VariablesDialogDlg.Dispose()
    End Sub
    ' Ritorna True se ha creato la lista, False se esiste già
    Public Function AddGlobalVariablesList(ByVal name As String, _
        Optional ByVal showEditor As Boolean = True) As Boolean
        'Cerca prima
        If Not IsNothing(m_resource.globalVars.FindListByName(name)) Then Return False
        'Crea la nuova lista e lo aggiunge alla lista
        Dim NewVariablesList As New VariablesList(name)
        m_resource.globalVars.Add(NewVariablesList)
        If showEditor Then
            'Crea e visualizza il relativo form
            Dim FrVariablesListTemp As New VariablesListControlPanel(m_resource.globalVars, NewVariablesList, True)
            FrVariablesListTemp.MdiParent = m_mdiParentForm
            FrVariablesListTemp.Show()
            'Attiva il monitoring delle variabili
            FrVariablesListTemp.StartMonitor()
        End If
        RefreshTreeStruct()
        Return True
    End Function
    Private Sub SetGlobalVariablesList(ByRef List As VariablesList)
        Dim VariablesDialogDlg As New VariablesListDialogForm
        VariablesDialogDlg.m_Name = List.Name
        Dim ResultDialog As DialogResult = VariablesDialogDlg.ShowDialog()
        If ResultDialog = DialogResult.OK Then
            If List.Name <> VariablesDialogDlg.m_Name Then
                'Se il nome è cambiato controlla se una lista  con lo stesso nome esiste già
                If Not IsNothing(m_resource.globalVars.FindListByName(VariablesDialogDlg.m_Name)) Then
                    MessageBox.Show("List " + VariablesDialogDlg.m_Name + " already exists", _
                                        UniSimVersion.VersionInfo.PrintableDescriptionForTool(), MessageBoxButtons.OK, _
                                         MessageBoxIcon.Stop)
                Else
                    'Aggiorna i dati
                    List.Name = VariablesDialogDlg.m_Name
                    RefreshTreeStruct()
                End If
            End If
        End If
        VariablesDialogDlg.Dispose()
    End Sub
    Private Sub RemoveVariablesList(ByRef RefList As VariablesList)
        If MsgBox("Do you really want to delete " & RefList.Name & "?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, _
        UniSimVersion.VersionInfo.PrintableDescriptionForTool("")) = MsgBoxResult.Yes Then
            'Rimuove la lista dalla lista delle variabili
            m_resource.globalVars.Remove(RefList)
            RefreshTreeStruct()
        End If
    End Sub
    Private Sub AddTask()
        Dim TaskDialogDlg As New TaskDialogForm(m_resource, m_project.Types.pous)
        Dim ResultDialog As DialogResult = TaskDialogDlg.ShowDialog()
        If ResultDialog = DialogResult.OK Then
            'Controlla se il nome del Task è già presente
            If Not IsNothing(m_resource.FindTaskByName(TaskDialogDlg.m_name)) Then
                MessageBox.Show("Task " + TaskDialogDlg.m_name + " already exists", _
                                    UniSimVersion.VersionInfo.PrintableDescriptionForTool(), MessageBoxButtons.OK, _
                                     MessageBoxIcon.Stop)
            Else
                'Crea il task e lo aggiunge alla lista
                Dim NewTask As New Task(TaskDialogDlg.m_name, TaskDialogDlg.m_priority, TaskDialogDlg.m_interval, TaskDialogDlg.m_pouInstances)
                m_resource.AddTask(NewTask)
            End If
        End If
        TaskDialogDlg.Dispose()
        RefreshTreeStruct()
    End Sub
    Public Sub AddTask(ByRef RefPou As pou)
        Dim pouInstances As New System.Collections.Generic.List(Of pouInstance)
        pouInstances.Add(New pouInstance(RefPou))
        Dim NewTask As New Task(RefPou.Name + "Task", Task.MAX_PRIORITY, New TimeSpan(0), pouInstances)
        m_resource.AddTask(NewTask)
        RefreshTreeStruct()
    End Sub
    Private Sub SetTask(ByRef RefTask As Task)
        Dim TaskDialog As New TaskDialogForm(m_resource, m_project.Types.pous)
        TaskDialog.m_name = RefTask.name
        TaskDialog.m_priority = RefTask.priority
        TaskDialog.m_interval = RefTask.m_interval
        TaskDialog.m_pouInstances = RefTask.pouInstances
        Dim ResultDialog As DialogResult = TaskDialog.ShowDialog()
        If ResultDialog = DialogResult.OK Then
            'Se il nome è cambiato controlla se un task con lo stesso nome esiste già
            If RefTask.name <> TaskDialog.m_name Then
                If Not IsNothing(m_resource.FindTaskByName(TaskDialog.m_name)) Then
                    MessageBox.Show("Task " + TaskDialog.m_name + " already exists", _
                                        UniSimVersion.VersionInfo.PrintableDescriptionForTool(), MessageBoxButtons.OK, _
                                         MessageBoxIcon.Stop)
                Else
                    'Aggiorna i dati
                    RefTask.name = TaskDialog.m_name
                    RefTask.priority = TaskDialog.m_priority
                    RefTask.m_interval = TaskDialog.m_interval
                    RefTask.pouInstances = TaskDialog.m_pouInstances
                    RefreshTreeStruct()
                End If
            Else
                'Non aggiorna il nome
                RefTask.priority = TaskDialog.m_priority
                RefTask.m_interval = TaskDialog.m_interval
                RefTask.pouInstances = TaskDialog.m_pouInstances
                RefreshTreeStruct()
            End If
        End If
        TaskDialog.Dispose()
    End Sub
    Private Sub RemoveTask(ByRef RefTask As Task)
        If MsgBox("Do you really want to delete " & RefTask.name & "?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, _
        UniSimVersion.VersionInfo.PrintableDescriptionForTool("")) = MsgBoxResult.Yes Then
            'Rimuove il task
            m_resource.RemoveTask(RefTask)
            RefreshTreeStruct()
        End If
    End Sub
    Private Sub AddPouInstance()
        Dim PouListDialogDlg As New PouListDialogForm(m_resource, m_project.Types.pous)
        Dim ResultDialog As DialogResult = PouListDialogDlg.ShowDialog()
        If ResultDialog = DialogResult.OK Then
            'Controlla se l'istanza è già presente
            If Not IsNothing(m_resource.FindPouInstanceByName(PouListDialogDlg.m_pou.Name)) Then
                MessageBox.Show("POU instance " + PouListDialogDlg.m_pou.Name + " already exists", _
                                    UniSimVersion.VersionInfo.PrintableDescriptionForTool(), MessageBoxButtons.OK, _
                                     MessageBoxIcon.Stop)
            Else
                AddPouInstance(PouListDialogDlg.m_pou)
            End If
        End If
        PouListDialogDlg.Dispose()
        RefreshTreeStruct()
    End Sub
    Public Sub AddPouInstance(ByVal p As pou)
        Dim instance As New pouInstance(p)
        m_resource.pouInstances.Add(instance)
        RefreshTreeStruct()
    End Sub
    Private Sub RemovePouInstance(ByRef RefPouInstance As pouInstance)
        If MsgBox("Do you really want to delete " & RefPouInstance.name & "?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, _
        UniSimVersion.VersionInfo.PrintableDescriptionForTool("")) = MsgBoxResult.Yes Then
            'Rimuove il RefPouInstance
            m_resource.pouInstances.Remove(RefPouInstance)
            RefreshTreeStruct()
        End If
    End Sub
    Private Sub SetPouInstance(ByRef RefPouInstance As pouInstance)
        Dim PouListDialog As New PouListDialogForm(m_resource, m_project.Types.pous)
        PouListDialog.m_pou = RefPouInstance.pou
        Dim ResultDialog As DialogResult = PouListDialog.ShowDialog()
        If ResultDialog = DialogResult.OK Then
            'Controlla se un pouInstance con lo stesso nome esiste già
            If Not IsNothing(m_resource.FindPouInstanceByName(PouListDialog.m_pou.Name)) Then
                MessageBox.Show("POU instance " + PouListDialog.m_pou.Name + " already exists", _
                                    UniSimVersion.VersionInfo.PrintableDescriptionForTool(), MessageBoxButtons.OK, _
                                     MessageBoxIcon.Stop)
            Else
                'Aggiorna i dati
                RefPouInstance.pou = PouListDialog.m_pou
            End If
        End If
        PouListDialog.Dispose()
    End Sub
    Private Sub ResourceTree_DoubleClick(ByVal Obg As Object, ByVal e As System.EventArgs) Handles Me.DoubleClick
        If Not IsNothing(SelectedNode) Then
            If Not IsNothing(SelectedNode.Parent) Then
                'Verifica il tipo di nodo dal nome del nodo padre
                Select Case SelectedNode.Parent.Text
                    Case "Global variables"
                        Dim List As VariablesList = m_resource.globalVars.FindListByName(SelectedNode.Text)
                        If Not IsNothing(List) Then
                            ShowGlobalVariablesList(List)
                            Exit Sub
                        End If
                    Case "Tasks"
                        Dim Task As Task = m_resource.FindTaskByName(SelectedNode.Text)
                        If Not IsNothing(Task) Then
                            SetTask(Task)
                            Exit Sub
                        End If
                    Case "POUs instances"
                        Dim PI As pouInstance = m_resource.FindPouInstanceByName(SelectedNode.Text)
                        If Not IsNothing(PI) Then
                            SetPouInstance(PI)
                            Exit Sub
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
                        Select Case SelectedNode.Parent.Text
                            Case "Global variables"
                                ContextMenu = Me.MenuGlobalVars
                            Case "Tasks"
                                ContextMenu = Me.MenuTasks
                            Case "POUs instances"
                                ContextMenu = Me.MenuPouInstances
                            Case Else
                                ContextMenu = Nothing
                        End Select
                    Else
                        'Il nodo padre è nullo
                        Select Case SelectedNode.Text
                            Case "Global variables"
                                ContextMenu = Me.MenuNodeGlobalVars
                            Case "Tasks"
                                ContextMenu = Me.MenuNodeTasks
                            Case "POUs instances"
                                ContextMenu = Me.MenuNodePouInstances
                            Case Else
                                ContextMenu = Nothing
                        End Select
                    End If
                Else
                    ContextMenu = Nothing
                End If
            End If
        End If
    End Sub
    Private Sub MenuGlobalVarsOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuGlobalVarsOpen.Click
        Dim L As VariablesList = m_resource.globalVars.FindListByName(Me.SelectedNode.Text)
        If Not IsNothing(L) Then
            ShowGlobalVariablesList(L)
        End If
    End Sub
    Private Sub MenuGlobalVarsRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuGlobalVarsRename.Click
        Dim L As VariablesList = m_resource.globalVars.FindListByName(Me.SelectedNode.Text)
        If Not IsNothing(L) Then
            SetGlobalVariablesList(L)
        End If
    End Sub
    Private Sub MenuGlobalVarsRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuGlobalVarsRemove.Click
        Dim L As VariablesList = m_resource.globalVars.FindListByName(Me.SelectedNode.Text)
        If Not IsNothing(L) Then
            RemoveVariablesList(L)
        End If
    End Sub
    Private Sub MenuGlobalVarsAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuGlobalVarsAdd.Click
        AddGlobalVariablesList()
    End Sub
    Private Sub MenuTasksSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuTasksSet.Click
        Dim T As Task = m_resource.FindTaskByName(Me.SelectedNode.Text)
        If Not IsNothing(T) Then
            SetTask(T)
        End If
    End Sub
    Private Sub MenuTasksRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuTasksRemove.Click
        Dim T As Task = m_resource.FindTaskByName(Me.SelectedNode.Text)
        If Not IsNothing(T) Then
            RemoveTask(T)
        End If
    End Sub
    Private Sub MenuTasksAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuTasksAdd.Click
        AddTask()
    End Sub
    Private Sub MenuPouIstancesSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuPouInstancesSet.Click
        Dim PI As pouInstance = m_resource.FindPouInstanceByName(Me.SelectedNode.Text)
        If Not IsNothing(PI) Then
            SetPouInstance(PI)
        End If
    End Sub
    Private Sub MenuPouInstanceRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuPouInstancesRemove.Click
        Dim PI As pouInstance = m_resource.FindPouInstanceByName(Me.SelectedNode.Text)
        If Not IsNothing(PI) Then
            RemovePouInstance(PI)
        End If
    End Sub
    Private Sub MenuPouInstanceAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuPouInstancesAdd.Click
        AddPouInstance()
    End Sub
End Class
