Imports System.Drawing
Imports System.Math
Imports System.Xml
Imports System.Threading
Imports System.Windows.Forms
Public Class FormSfc
    Inherits System.Windows.Forms.Form
    Private WithEvents m_Body As body
    Private WithEvents m_Sfc As Sfc
    Private m_GraphToDraw As drawing.Graphics
    Private EditingMode As Boolean
    Private m_monitoring As Boolean
    Private Dimension As Integer
    Private Foglio as Drawing.rectangle
    Private ColGriglia as Drawing.Color
    Private Br, BrSfondo As Drawing.Brush
    Private OffsetXMove As Integer
    Private OffsetYMove As Integer
    Private CoeffSnap As Integer
    Private NextCondition As BooleanExpression
    Private NextElementName As String
    Private NextElementDocumentation As String
    Private CurrentOperation As Operation
    Private DistanzaDaSelectionX As Integer
    Private DistanzaDaSelectiony As Integer
    Private MultipleSelection As Boolean
    Private InitialPoint as Drawing.point
    Private PreviousFinelPoint as Drawing.point
    Private ColSfondo As Drawing.Color = Drawing.color.White
    Private TreeOpen As Boolean
    Private ShowDetails As Boolean
    Private CursorStep As New Cursor(CursorsPath & "\CursorStep.Cur")
    Private CursorMacroStep As New Cursor(CursorsPath & "\CursorMacroStep.Cur")
    Private CursorTransition As New System.Windows.Forms.Cursor(CursorsPath & "\CursorTransition.Cur")
    Private CursorMove As New Cursor(CursorsPath & "\CursorMove.cur")
    Private Enum Operation
        DefiningStep
        DefiningMacroStep
        DefiningTransition
        Selection
        MultipleSelection
        Selected
    End Enum
#Region " Codice generato da ProgettAction Windows Form "

    Public Sub New(ByRef RefBody As body, ByVal Editing As Boolean)

        MyBase.New()
        m_Body = RefBody
        m_Sfc = m_Body.ReadSfc
        EditingMode = Editing
        CurrentOperation = Operation.Selection
        Foglio.Width = 800
        Foglio.Height = 1200
        CoeffSnap = 4
        BrSfondo = New Drawing.SolidBrush(ColSfondo)
        InitialPoint = new Drawing.point(0, 0)



        'Chiamata richiesta da ProgettAction Windows Form.
        InitializeComponent()
        'm_GraphToDraw = Panel1.CreateGraphics
        'm_GraphToDraw = Panel1.CreateGraphics
        'Aggiungere le eventuali istruzioni di InitialzzAction dopo la chiamata a InitializeComponent()
        m_GraphToDraw = Panel1.CreateGraphics
        ShowDetails = True
        m_Sfc.SetGraphToDraw(m_GraphToDraw)
        'Disattiva i controlli se non è in modo Editing
        If EditingMode Then
            Label2.Text = "Editing mode"
        Else
            Label2.Visible = False
            MenuItem1.Visible = False
            toolBar1.Visible = False
        End If
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

    'Richiesto da ProgettAction Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da ProgettAction Windows Form.
    'Può essere modificata in ProgettAction Windows Form.  
    'Non modificarla nell'editor del codice.
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents toolBar1 As System.Windows.Forms.ToolBar
    Friend WithEvents NewStepButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents NewMacroStepButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents NewActionButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents NewTransButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents SetIniButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents SetFinButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton3 As System.Windows.Forms.ToolBarButton
    Friend WithEvents DelButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton4 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ShowTreeButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents ShowTransitionDetailButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem6 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem7 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem8 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem9 As System.Windows.Forms.MenuItem
    Friend WithEvents ToolBarButton1 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ShowCursor As System.Windows.Forms.ToolBarButton
    Friend WithEvents MenuItem10 As System.Windows.Forms.MenuItem
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ToolBarButton2 As System.Windows.Forms.ToolBarButton
    Friend WithEvents StatusBarPanel1 As System.Windows.Forms.StatusBarPanel
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ExecuteBar1 As ExecuteBar
    Friend WithEvents Button1 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(FormSfc))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.toolBar1 = New System.Windows.Forms.ToolBar
        Me.NewStepButton = New System.Windows.Forms.ToolBarButton
        Me.NewMacroStepButton = New System.Windows.Forms.ToolBarButton
        Me.NewActionButton = New System.Windows.Forms.ToolBarButton
        Me.NewTransButton = New System.Windows.Forms.ToolBarButton
        Me.SetIniButton = New System.Windows.Forms.ToolBarButton
        Me.SetFinButton = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton3 = New System.Windows.Forms.ToolBarButton
        Me.DelButton = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton4 = New System.Windows.Forms.ToolBarButton
        Me.ShowTreeButton = New System.Windows.Forms.ToolBarButton
        Me.ShowTransitionDetailButton = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton1 = New System.Windows.Forms.ToolBarButton
        Me.ShowCursor = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton2 = New System.Windows.Forms.ToolBarButton
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.MenuItem4 = New System.Windows.Forms.MenuItem
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.MenuItem6 = New System.Windows.Forms.MenuItem
        Me.MenuItem7 = New System.Windows.Forms.MenuItem
        Me.MenuItem8 = New System.Windows.Forms.MenuItem
        Me.MenuItem9 = New System.Windows.Forms.MenuItem
        Me.MenuItem10 = New System.Windows.Forms.MenuItem
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.StatusBarPanel1 = New System.Windows.Forms.StatusBarPanel
        Me.TreeView1 = New System.Windows.Forms.TreeView
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.ExecuteBar1 = New SiValProClassLibrary.ExecuteBar
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Panel2.SuspendLayout()
        CType(Me.StatusBarPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(800, 1200)
        Me.Panel1.TabIndex = 1
        '
        'Splitter1
        '
        Me.Splitter1.Location = New System.Drawing.Point(88, 30)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(4, 305)
        Me.Splitter1.TabIndex = 4
        Me.Splitter1.TabStop = False
        '
        'toolBar1
        '
        Me.toolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.NewStepButton, Me.NewMacroStepButton, Me.NewActionButton, Me.NewTransButton, Me.SetIniButton, Me.SetFinButton, Me.ToolBarButton3, Me.DelButton, Me.ToolBarButton4, Me.ShowTreeButton, Me.ShowTransitionDetailButton, Me.ToolBarButton1, Me.ShowCursor, Me.ToolBarButton2})
        Me.toolBar1.ButtonSize = New System.Drawing.Size(24, 24)
        Me.toolBar1.DropDownArrows = True
        Me.toolBar1.ImageList = Me.ImageList1
        Me.toolBar1.Location = New System.Drawing.Point(0, 0)
        Me.toolBar1.Name = "toolBar1"
        Me.toolBar1.ShowToolTips = True
        Me.toolBar1.Size = New System.Drawing.Size(492, 30)
        Me.toolBar1.TabIndex = 5
        '
        'NewStepButton
        '
        Me.NewStepButton.ImageIndex = 6
        Me.NewStepButton.ToolTipText = "Step"
        '
        'NewMacroStepButton
        '
        Me.NewMacroStepButton.ImageIndex = 9
        Me.NewMacroStepButton.ToolTipText = "MacroStep"
        '
        'NewActionButton
        '
        Me.NewActionButton.ImageIndex = 12
        Me.NewActionButton.ToolTipText = "Action"
        '
        'NewTransButton
        '
        Me.NewTransButton.ImageIndex = 5
        Me.NewTransButton.ToolTipText = "Transition"
        '
        'SetIniButton
        '
        Me.SetIniButton.ImageIndex = 10
        Me.SetIniButton.ToolTipText = "InitialStep (yes/no)"
        '
        'SetFinButton
        '
        Me.SetFinButton.ImageIndex = 15
        Me.SetFinButton.ToolTipText = "FinalStep (yes/No)"
        '
        'ToolBarButton3
        '
        Me.ToolBarButton3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'DelButton
        '
        Me.DelButton.ImageIndex = 11
        Me.DelButton.ToolTipText = "Cancel"
        '
        'ToolBarButton4
        '
        Me.ToolBarButton4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'ShowTreeButton
        '
        Me.ShowTreeButton.ImageIndex = 14
        Me.ShowTreeButton.Text = "Show tree"
        '
        'ShowTransitionDetailButton
        '
        Me.ShowTransitionDetailButton.ImageIndex = 13
        Me.ShowTransitionDetailButton.Text = "Show transitions condition (yes/no)"
        '
        'ToolBarButton1
        '
        Me.ToolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'ShowCursor
        '
        Me.ShowCursor.ImageIndex = 16
        Me.ShowCursor.ToolTipText = "Cursor"
        '
        'ToolBarButton2
        '
        Me.ToolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'ImageList1
        '
        Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem1})
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 0
        Me.MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem2, Me.MenuItem3, Me.MenuItem4, Me.MenuItem5, Me.MenuItem6, Me.MenuItem7, Me.MenuItem8, Me.MenuItem9, Me.MenuItem10})
        Me.MenuItem1.MergeOrder = 4
        Me.MenuItem1.Text = "Sfc"
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 0
        Me.MenuItem2.Text = "Add Step"
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 1
        Me.MenuItem3.Text = "Add Action"
        '
        'MenuItem4
        '
        Me.MenuItem4.Index = 2
        Me.MenuItem4.Text = "Add Transition"
        '
        'MenuItem5
        '
        Me.MenuItem5.Index = 3
        Me.MenuItem5.Text = "Cancel selection"
        '
        'MenuItem6
        '
        Me.MenuItem6.Index = 4
        Me.MenuItem6.Text = "Initia step (Yes/no)"
        '
        'MenuItem7
        '
        Me.MenuItem7.Index = 5
        Me.MenuItem7.Text = "Final step  (Yes/no)"
        '
        'MenuItem8
        '
        Me.MenuItem8.Index = 6
        Me.MenuItem8.Text = "Show tree (Yes/No)"
        '
        'MenuItem9
        '
        Me.MenuItem9.Index = 7
        Me.MenuItem9.Text = "Show transitions details (Yes/No)"
        '
        'MenuItem10
        '
        Me.MenuItem10.Index = 8
        Me.MenuItem10.Text = "Cursor"
        '
        'Panel2
        '
        Me.Panel2.AutoScroll = True
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(224, Byte), CType(224, Byte), CType(224, Byte))
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel2.Controls.Add(Me.Panel1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(92, 30)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(400, 305)
        Me.Panel2.TabIndex = 6
        '
        'StatusBarPanel1
        '
        Me.StatusBarPanel1.Text = "Mode:"
        '
        'TreeView1
        '
        Me.TreeView1.Dock = System.Windows.Forms.DockStyle.Left
        Me.TreeView1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TreeView1.ImageIndex = -1
        Me.TreeView1.Location = New System.Drawing.Point(0, 30)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.SelectedImageIndex = -1
        Me.TreeView1.Size = New System.Drawing.Size(88, 305)
        Me.TreeView1.TabIndex = 9
        Me.TreeView1.Visible = False
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.ExecuteBar1)
        Me.Panel3.Controls.Add(Me.Label1)
        Me.Panel3.Controls.Add(Me.Button1)
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.ForeColor = System.Drawing.Color.Black
        Me.Panel3.Location = New System.Drawing.Point(0, 335)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(492, 18)
        Me.Panel3.TabIndex = 0
        '
        'ExecuteBar1
        '
        Me.ExecuteBar1.Dock = System.Windows.Forms.DockStyle.Left
        Me.ExecuteBar1.Location = New System.Drawing.Point(230, 0)
        Me.ExecuteBar1.Name = "ExecuteBar1"
        Me.ExecuteBar1.Size = New System.Drawing.Size(82, 18)
        Me.ExecuteBar1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(154, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 18)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Execution steps:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.Gray
        Me.Button1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(64, 0)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(90, 18)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "Monitor (On/Off)"
        '
        'Label2
        '
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(0, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 18)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Editing mode"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FormSfc
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(492, 353)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.TreeView1)
        Me.Controls.Add(Me.toolBar1)
        Me.Controls.Add(Me.Panel3)
        Me.ForeColor = System.Drawing.Color.Red
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.MainMenu1
        Me.MinimumSize = New System.Drawing.Size(320, 110)
        Me.Name = "FormSfc"
        Me.ShowInTaskbar = False
        Me.Text = "ProgettAction"
        Me.Panel2.ResumeLayout(False)
        CType(Me.StatusBarPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public Sub AddStep()
        ResetCurrentOperation()
        'Cerca il numero della prima fase libera
        'e passa il valore alla finestra di dialogo
        Dim StepDialog As New StepDialogForm
        StepDialog.StepName = m_Sfc.FirstAvaiableStepName
        Dim ResultDialog As System.Windows.Forms.DialogResult = StepDialog.ShowDialog()
        If ResultDialog = DialogResult.OK Then
            'Controlla se il nome della fase è già presente
            If Not IsNothing(m_Sfc.FindStepByName(StepDialog.StepName)) Then
                MsgBox("Step " & StepDialog.StepName & " already exist!")
            Else
                NextElementName = StepDialog.StepName
                CurrentOperation = Operation.DefiningStep
            End If
        End If
        StepDialog.Dispose()
    End Sub
    Public Sub AddMacroStep()
        ResetCurrentOperation()
        'Cerca il numero della prima macrofase libera
        'e passa il valore alla finestra di dialogo
        Dim MacroStepDialog As New MacroStepDialogForm
        MacroStepDialog.StepName = m_Sfc.FirstAvaiableStepName
        'Disabilita il tasto per mostrare il body
        MacroStepDialog.Button3.Enabled = False
        Dim ResultDialog As System.Windows.Forms.DialogResult = MacroStepDialog.ShowDialog()
        If ResultDialog = DialogResult.OK Then
            'Controlla se il nome è già presente
            If Not IsNothing(m_Sfc.FindStepByName(MacroStepDialog.StepName)) = True Then
                MsgBox("Macro step " & MacroStepDialog.StepName & " already exist!")
            Else
                NextElementName = MacroStepDialog.StepName
                CurrentOperation = Operation.DefiningMacroStep
            End If
        End If
        MacroStepDialog.Dispose()
    End Sub
    Public Sub AddAction()
        ResetCurrentOperation()
        If m_Sfc.ReadStepList.ReadSelected.CountStepsList > 0 Then
            Dim ActionDialog As New ActionDialogForm(m_Sfc.ResGlobalVariables)
            Dim ResultDialog As System.Windows.Forms.DialogResult = ActionDialog.ShowDialog()
            If ResultDialog = DialogResult.OK Then
                StoreActions(ActionDialog.Nome, ActionDialog.Qualifier, ActionDialog.Var, ActionDialog.Indicator, ActionDialog.Time, ActionDialog.RefSfc, ActionDialog.StepsToForces)
                DrawVisibleArea()
            End If
        Else
            MsgBox("Step not selected")
        End If
    End Sub
    Public Sub AddTransition()
        ResetCurrentOperation()
        'Cerca il numero della prima transizione libera
        'e passa il valore alla finestra di dialogo
        Dim TransitionDialog As New TransitionDialogForm
        TransitionDialog.m_Name = m_Sfc.FirstAvaiableTransitionName
        TransitionDialog.m_Sfc = m_Sfc
        Dim ResultDialog As System.Windows.Forms.DialogResult = TransitionDialog.ShowDialog()
        If ResultDialog = DialogResult.OK Then
            NextCondition = TransitionDialog.m_Condition
            CurrentOperation = Operation.DefiningTransition
            DeSelectAll()
            DrawVisibleArea()
        End If
        TransitionDialog.Dispose()
    End Sub
    Public Sub SetInitial()
        If CurrentOperation = Operation.DefiningTransition Then
            m_Sfc.CancelSelection(True)
        Else
            m_Sfc.CancelSelection(False)
        End If
        Dim Find As Boolean = m_Sfc.SetInitialSteps()
        DrawVisibleArea()
        If Not Find Then
            MsgBox("No steps selected!")
        End If
    End Sub
    Public Sub SetFinal()
        If CurrentOperation = Operation.DefiningTransition Then
            m_Sfc.CancelSelection(True)
        Else
            m_Sfc.CancelSelection(False)
        End If
        Dim Find As Boolean = m_Sfc.SetFinalSteps
        DrawVisibleArea()
        If Not Find Then
            MsgBox("No steps selected!")
        End If
    End Sub
    Public Sub RemoveSelectedElements()
        If m_Sfc.GraphicalStepsList.ReadSelected.CountSelected > 0 Or m_Sfc.GraphicalStepsList.ReadSelectedStepsActionsList.Count > 0 Or m_Sfc.GraphicalTransitionsList.ReadSelectedTransitionList.CountSelected > 0 Then
            If m_Sfc.GraphicalTransitionsList.ControllaPresenzaStepsInTransizioni(m_Sfc.GraphicalStepsList.ReadSelected) Then
                MsgBox("It is't possible delete a step with a link to a transition!")
            Else
                If MsgBox("Confirm delete?", MsgBoxStyle.OKCancel) = MsgBoxResult.OK Then
                    m_Sfc.RemoveSelectedElements()
                    CancelVisibleArea()
                    DrawVisibleArea()
                    If TreeOpen Then
                        RefreshTreeStruct()
                    End If
                End If
                CurrentOperation = Operation.Selection
            End If
        Else
            MsgBox("No selected elements!")
        End If
    End Sub
    Public Sub WriteTitlePanel()
        Me.Text = m_Sfc.Name
    End Sub
    Public Sub StartMonitor()
        m_Sfc.StartStateMonitor()
    End Sub
    Public Sub StopMonitor()
        m_Sfc.StopStateMonitor()
        CancelVisibleArea()
        DrawVisibleArea()
    End Sub
    Public Function ReadBody() As body
        ReadBody = m_Body
    End Function
    Public Sub SetShowdetail()
        ShowDetails = Not ShowDetails
        m_Sfc.ShowDetails(ShowDetails)
        CancelVisibleArea()
        DrawVisibleArea()
    End Sub
    Public Sub ResetCurrentOperation()
        CurrentOperation = Operation.Selection
        CancelVisibleArea()
        DrawVisibleArea()
    End Sub

    Private Function StoreStep(ByVal P As Drawing.Point) As Boolean
        StoreStep = m_Sfc.AddAndDrawStep(m_Sfc.FirstAvaiableElementNumber, NextElementName, NextElementDocumentation, P)
        If TreeOpen Then
            TreeView1.Nodes(0).Nodes.Add(NextElementName)
            TreeView1.Nodes(0).Expand()
        End If
    End Function
    Private Function StoreMacroStep(ByVal P As Drawing.Point) As Boolean
        StoreMacroStep = m_Sfc.AddAndDrawMacroStep(m_Sfc.FirstAvaiableElementNumber, NextElementName, NextElementDocumentation, P)
        If TreeOpen Then
            TreeView1.Nodes(0).Nodes.Add(NextElementName)
            TreeView1.Nodes(0).Expand()
        End If
    End Function
    Private Sub ConfirmAddTransition()
        If StepsSelectedForTransitionOk() Then
            StoreTransition()
            CurrentOperation = Operation.Selection
            CancelVisibleArea()
            DeSelectAll()
            DrawVisibleArea()
        Else
            MsgBox("A transition needs almost a output step and a input step!")
        End If
    End Sub
    Private Function StepsSelectedForTransitionOk()
        If m_Sfc.GraphicalStepsList.ReadBottomSelectedStepList.CountStepsList > 0 And m_Sfc.GraphicalStepsList.ReadTopSelectedStepList.CountStepsList > 0 Then
            StepsSelectedForTransitionOk = True
        End If
    End Function
    Private Function StoreTransition() As Boolean
        m_Sfc.AddAndDrawTransition(m_Sfc.FirstAvaiableElementNumber, NextElementName, NextElementDocumentation, NextCondition)
        If TreeOpen Then
            TreeView1.Nodes(1).Nodes.Add(NextElementName)
            TreeView1.Nodes(1).Expand()
        End If
    End Function
    Private Function StoreActions(ByVal Nome As String, ByVal Qual As String, ByVal Var As BaseVariable, ByVal VarInd As BaseVariable, ByVal Time As TimeSpan, ByRef RefSfc As Sfc, ByVal StepToForces As GraphicalStepsList)
        m_Sfc.AddActionToSelectedSteps(Nome, Qual, Var, VarInd, Time, RefSfc, StepToForces)
        RefreshTreeStruct()
    End Function
    Private Function Snap(ByVal val As Integer) As Integer
        Snap = CInt(val / CoeffSnap) * CoeffSnap

    End Function
    Private Sub DrawMultipleSelectionRectangle(ByVal R As Drawing.Rectangle)
        Dim Penna As New Drawing.Pen(Color.Black)
        Penna.DashStyle = Drawing2D.DashStyle.Dot
        Try
            If Monitor.TryEnter(m_GraphToDraw, 2000) Then
                m_GraphToDraw.DrawRectangle(Penna, R)
            End If
        Catch ex As System.Exception
        Finally
            Monitor.Exit(m_GraphToDraw)
        End Try
    End Sub
    Private Sub CancelMultipleSelectionRectangle(ByVal R As Drawing.Rectangle)
        Dim Penna As New Drawing.Pen(ColSfondo)
        Try
            If Monitor.TryEnter(m_GraphToDraw, 2000) Then
                m_GraphToDraw.DrawRectangle(Penna, R)
            End If
        Catch ex As System.Exception
        Finally
            Monitor.Exit(m_GraphToDraw)
        End Try
    End Sub
    Private Sub FindAndSelectElementsArea(ByVal Rect As Drawing.Rectangle)
        m_Sfc.FindAndSelectElementsArea(Rect)
    End Sub
    Private Sub DrawArea(ByVal Rect As Drawing.Rectangle)
        Rect.X = Rect.X - 1
        Rect.Y = Rect.Y - 1
        Rect.Width = Rect.Width + 2
        Rect.Height = Rect.Height + 2
        If CurrentOperation = Operation.DefiningTransition Then
            m_Sfc.DrawElementsArea(Rect, True)
        Else
            m_Sfc.DrawElementsArea(Rect, False)
        End If
    End Sub
    Private Sub DrawVisibleArea()
        Dim R As New Drawing.Rectangle(0 - Panel1.Left, 0 - Panel1.Top, Panel1.Width - Panel1.Left, Panel1.Height - Panel1.Top)
        DrawArea(R)
    End Sub
    Private Sub CancelVisibleArea()
        Try
            If Monitor.TryEnter(m_GraphToDraw, 2000) Then
                m_GraphToDraw.Clear(ColSfondo)
            End If
        Catch ex As System.Exception
        Finally
            Monitor.Exit(m_GraphToDraw)
        End Try

    End Sub
    Private Sub CancelSelection()
        m_Sfc.CancelSelection(True)

    End Sub
    Private Sub DeSelectAll()
        m_Sfc.DeSelectAll()
    End Sub
    Private Function MoveSelection(ByVal dx As Integer, ByVal dy As Integer)
        CancelSelection()
        m_Sfc.MoveSelection(dx, dy)
        DrawVisibleArea()
    End Function
    Public Sub SwitchTreeShow()
        If Not TreeOpen Then
            RefreshTreeStruct()
            TreeView1.Visible = True
            TreeOpen = True
        Else
            TreeView1.Visible = False
            TreeOpen = False
        End If
    End Sub
    Private Sub RefreshTreeStruct()
        TreeView1.Nodes.Clear()
        TreeView1.Nodes.Add("Steps")
        TreeView1.Nodes.Add("Transitions")
        For Each S As BaseGraphicalStep In m_Sfc.GraphicalStepsList()
            If S.GetType.Name = "GraphicalStep" Then
                TreeView1.Nodes(0).Nodes.Add(S.Name)
            Else
                If S.GetType.Name = "GraphicalMacroStep" Then
                    TreeView1.Nodes(0).Nodes.Add(S.Name)
                End If
            End If
        Next S
        For Each T As GraphicalTransition In m_Sfc.GraphicalTransitionsList()
            TreeView1.Nodes(1).Nodes.Add(T.ReadCondition.GetExpressionString)
        Next T
        TreeView1.Nodes(0).Expand()
        TreeView1.Nodes(1).Expand()
    End Sub
    Private Sub ShowMacroStepBody(ByRef RefBody As body)
        'Mostra un FormSfc per visualizzare il body di una macrofase
        'Controlla se il form esiste già
        Dim Find As Boolean
        For Each Fr As System.Windows.Forms.Form In Me.MdiParent.MdiChildren
            If Fr.GetType.Name = "FormSfc" Then
                Dim FrSfc As FormSfc = Fr
                If FrSfc.ReadBody.Name = RefBody.Name Then
                    Fr.Activate()
                    Find = True
                    Exit For
                End If
            End If
        Next Fr
        'Se non lo ha trovato lo crea e lo visualizza
        If Not Find Then
            Dim FrSfcTemp As New FormSfc(RefBody, EditingMode)
            FrSfcTemp.MdiParent = Me.MdiParent
            FrSfcTemp.Show()
        End If

    End Sub
    Private Sub NameChanged(ByVal Value As String) Handles m_Body.NameChanged 'Intercetta l'evento di cambiamento del nome
        WriteTitlePanel()
    End Sub
    Private Sub DisposingBody() Handles m_Body.Disposing    'Intercetta l'evento disposing del body
        Me.Dispose()
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        WriteTitlePanel()
        RefreshTreeStruct()
    End Sub
    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint
        DrawArea(e.ClipRectangle)
    End Sub
    Private Sub Panel1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseDown
        Me.Activate()
        If EditingMode Then
            'Legge il tasto del mouse premuto
            Select Case e.Button
                Case MouseButtons.Left
                    'Tasto sinistro del mouse
                    Select Case CurrentOperation
                        Case Is = Operation.DefiningStep
                            Dim P As New Drawing.Point(Snap(e.X), Snap(e.Y))
                            'Chiama AddAndDrawStep
                            If StoreStep(P) Then
                                CurrentOperation = Operation.Selection
                            End If
                        Case Is = Operation.DefiningMacroStep
                            Dim P As New Drawing.Point(Snap(e.X), Snap(e.Y))
                            'Chiama AddAndDrawMacroStep
                            If StoreMacroStep(P) Then
                                CurrentOperation = Operation.Selection
                            End If
                        Case Is = Operation.Selection
                            Dim Found As Boolean
                            If MultipleSelection Then
                                Found = m_Sfc.FindAndSelectElement(e.X, e.Y)
                            Else
                                Found = m_Sfc.FindElement(e.X, e.Y)
                                If Found Then
                                    'Ha trovato una Step o una Transition o un Action
                                    Dim GiaSelected As Boolean = m_Sfc.ReadIfElementIsSelected(e.X, e.Y)
                                    If Not GiaSelected Then
                                        DeSelectAll()
                                        m_Sfc.FindAndSelectElement(e.X, e.Y)
                                    End If
                                End If
                            End If

                            If Found Then
                                'Ha trovato l'elemento e prepara per la selezione
                                CurrentOperation = Operation.Selected
                                DrawVisibleArea()
                            Else
                                'Non ha trovato elementi e prepara per la selezione multipla
                                CurrentOperation = Operation.MultipleSelection
                                DeSelectAll()
                            End If
                            InitialPoint.X = e.X
                            InitialPoint.Y = e.Y
                            DrawVisibleArea()
                        Case Is = Operation.DefiningTransition
                            m_Sfc.FindAndSelectSmallRectangleStep(e.X, e.Y)
                            DrawVisibleArea()
                    End Select

                Case MouseButtons.Middle, MouseButtons.Right
                    'Tasto desto o centrale del mouse
                    Select Case CurrentOperation
                        Case Operation.DefiningTransition
                            ConfirmAddTransition()
                    End Select
            End Select
        Else
            'Non è in modo di editing quindi seleziona solo la macrofasi per aprirle
            m_Sfc.FindAndSelectElement(e.X, e.Y)
        End If
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        AddTransition()
    End Sub
    Private Sub Panel1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseUp
        If EditingMode Then
            Select Case CurrentOperation
                Case Operation.MultipleSelection
                    CurrentOperation = Operation.Selection
                    DeSelectAll()
                    FindAndSelectElementsArea(CreaRectangle(InitialPoint.X, InitialPoint.Y, e.X, e.Y))
                    Dim R As Drawing.Rectangle = CreaRectangle(InitialPoint.X, InitialPoint.Y, PreviousFinelPoint.X, PreviousFinelPoint.Y)
                    CancelMultipleSelectionRectangle(R)
                    DrawArea(R)
                Case Operation.Selected
                    CurrentOperation = Operation.Selection
                    'DrawVisibleArea()
            End Select
        End If
    End Sub
    Private Sub Panel1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseMove
        If EditingMode Then
            Select Case CurrentOperation
                Case Operation.Selected
                    Panel1.Cursor = CursorMove
                    'Effettua lo Movemento solo se dopo lo snap....
                    '....è maggiore di 0
                    Dim dx As Integer = Snap(e.X - InitialPoint.X)
                    Dim dy As Integer = Snap(e.Y - InitialPoint.Y)
                    If dx <> 0 Or dy <> 0 Then
                        Dim R As New Drawing.Rectangle(-dx, -dy, Foglio.Width - dx, Foglio.Height - dy)
                        Dim FuoriX, FuoriY As Boolean
                        'Simula l'area del foglio Moveta nei versi opposti
                        If Not m_Sfc.ControllaSelectionFuoriArea(R, FuoriX, FuoriY) Then
                            'Move la Selection se ci riesce
                            MoveSelection(dx, dy)
                            InitialPoint.X = Snap(e.X)
                            InitialPoint.Y = Snap(e.Y)
                        Else
                            If Not FuoriX And dx <> 0 Then
                                'Move la Selection solo di dx se ci riesce
                                MoveSelection(dx, 0)
                                InitialPoint.X = Snap(e.X)
                            End If
                            If Not FuoriY And dy <> 0 Then
                                'Move la Selection solo di dy se ci riesce
                                MoveSelection(0, dy)
                                InitialPoint.Y = Snap(e.Y)
                            End If
                        End If
                    End If
                Case Operation.DefiningStep
                    Panel1.Cursor = CursorStep
                Case Operation.DefiningMacroStep
                    Panel1.Cursor = CursorMacroStep
                Case Operation.DefiningTransition
                    Panel1.Cursor = CursorTransition
                Case Operation.MultipleSelection
                    CancelMultipleSelectionRectangle(CreaRectangle(InitialPoint.X, InitialPoint.Y, PreviousFinelPoint.X, PreviousFinelPoint.Y))
                    DrawArea(CreaRectangle(InitialPoint.X, InitialPoint.Y, PreviousFinelPoint.X, PreviousFinelPoint.Y))
                    DrawMultipleSelectionRectangle(CreaRectangle(InitialPoint.X, InitialPoint.Y, e.X, e.Y))
                    PreviousFinelPoint.X = e.X
                    PreviousFinelPoint.Y = e.Y
                Case Else
                    Panel1.Cursor = System.Windows.Forms.Cursors.Default
            End Select
        End If
    End Sub
    Private Sub FormSfc_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        'Try
        'Dim R As new drawing.rectangle(0, 0, Panel1.Size.Width, Panel1.Size.Height)
        'Dim Reg As New Region(R)
        ''Il monitor è su m_GraphToDraw perche può conteso dalla sub ExecuteScanState e dalle Barre di scorrimento
        'If Monitor.TryEnter(m_GraphToDraw, 1000) Then
        'm_GraphToDraw.Clip = Reg
        'm_GraphToDraw.Clip = Reg
        'DrawVisibleArea()
        'Monitor.Exit(m_GraphToDraw)
        'End If
        'Catch ex As system.exception
        'Monitor.Exit(m_GraphToDraw)
        'End Try
        If Not IsNothing(m_GraphToDraw) Then
            Try
                If Monitor.TryEnter(m_GraphToDraw, 2000) Then
                    Try
                        m_GraphToDraw = Panel1.CreateGraphics
                        If Monitor.TryEnter(m_GraphToDraw, 4000) Then
                            m_GraphToDraw.Clear(Color.White)
                            m_Sfc.SetGraphToDraw(m_GraphToDraw)
                            Monitor.Exit(m_GraphToDraw)
                            Me.DrawVisibleArea()
                        End If
                    Catch ex As System.Exception
                        Monitor.Exit(m_GraphToDraw)
                    End Try
                End If
            Catch ex As System.Exception
                If Not IsNothing(m_GraphToDraw) Then
                    Monitor.Exit(m_GraphToDraw)
                End If
            End Try
        End If
    End Sub
    Private Sub FormSfc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

    End Sub
    Private Sub FormSfc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Select Case CurrentOperation
            Case Operation.DefiningTransition
                If e.KeyCode = 13 Then
                    ConfirmAddTransition()
                End If
            Case Operation.Selection
                If e.KeyCode = 46 Then
                    CancelSelection()
                End If
            Case Operation.Selection
                If e.KeyCode = Keys.ControlKey Or e.KeyCode = Keys.ShiftKey Then
                    MultipleSelection = True
                End If
        End Select
    End Sub
    Private Sub FormSfc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        Select Case CurrentOperation
            Case Operation.Selection
                If e.KeyCode = Keys.ControlKey Or e.KeyCode = Keys.ShiftKey Then
                    MultipleSelection = False
                End If
        End Select
    End Sub
    Private Function CreaRectangle(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer) As Drawing.Rectangle
        Dim a, b, c, d As Integer
        If x1 < x2 Then
            a = x1
            c = x2
        Else
            a = x2
            c = x1
        End If
        If y1 < y2 Then
            b = y1
            d = y2
        Else
            b = y2
            d = y1
        End If
        CreaRectangle = New Drawing.Rectangle(a, b, c - a, d - b)
    End Function
    Private Sub Panel1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel1.DoubleClick
        If EditingMode Then
            If CurrentOperation = Operation.Selected And m_Sfc.CountSelectedElement = 1 And Not MultipleSelection Then
                Dim Obj As Object = m_Sfc.ReadObjectSelected
                Select Case Obj.GetType.Name

                    Case "GraphicalStep"
                        Dim StepDialog As New StepDialogForm
                        StepDialog.StepName = Obj.Name
                        Dim ResultDialog As System.Windows.Forms.DialogResult = StepDialog.ShowDialog()
                        If ResultDialog = DialogResult.OK Then
                            'Controlla se il mome della fase è già presente e se il numero è stato cambiato
                            If StepDialog.StepName <> Obj.Name Then
                                If Not IsNothing(m_Sfc.FindStepByName(StepDialog.StepName)) Then
                                    MsgBox("Step " & StepDialog.StepName & " already exist!")
                                Else
                                    'Imposta il nuovo nome
                                    Obj.Name = StepDialog.StepName
                                End If
                            End If
                        End If
                        StepDialog.Dispose()

                    Case "GraphicalMacroStep"
                        Dim MacroStepDialog As New MacroStepDialogForm
                        MacroStepDialog.StepName = Obj.Name
                        Dim ResultDialog As System.Windows.Forms.DialogResult = MacroStepDialog.ShowDialog()
                        If ResultDialog = DialogResult.OK Or ResultDialog = DialogResult.Yes Then
                            'Controlla se il nome della fase è già presente e se il numero è stato cambiato
                            If MacroStepDialog.StepName <> Obj.Name Then
                                If Not IsNothing(m_Sfc.FindStepByName(MacroStepDialog.StepName)) Then
                                    MsgBox("Step " & MacroStepDialog.StepName & " already exist!")
                                Else
                                    'Imposta il nuovo nome
                                    Obj.Name = MacroStepDialog.StepName
                                End If
                            End If
                            If ResultDialog = DialogResult.Yes Then
                                'Mostra il body se è stato premuto Show body (Resituisce Yes)
                                ShowMacroStepBody(Obj.ReadBody)
                            End If
                        End If
                        MacroStepDialog.Dispose()

                    Case "GraphicalTransition"
                        Dim TransitionDlgForm As New TransitionDialogForm
                        TransitionDlgForm.m_Name = Obj.Name
                        TransitionDlgForm.m_Condition = Obj.ReadCondition
                        TransitionDlgForm.m_Sfc = m_Sfc
                        Dim ResultDialog As System.Windows.Forms.DialogResult = TransitionDlgForm.ShowDialog()
                        If ResultDialog = DialogResult.OK Then
                            'Controlla se il nome della transizione è già presente se il numero è stato cambiato
                            If TransitionDlgForm.m_Name <> Obj.Name Then
                                'Controlla se il nome non è gia presente
                                If Not IsNothing(m_Sfc.FindTransitionByName(TransitionDlgForm.m_Name)) Then
                                    MsgBox("Transition " & TransitionDlgForm.m_Name & " already exist!")
                                Else
                                    'Imposta il nuovo nome
                                    Obj.Name = TransitionDlgForm.m_Name
                                End If
                            End If
                            'Imposta il nuovo body
                            Obj.SetCondition(TransitionDlgForm.m_Condition)
                        End If
                        TransitionDlgForm.Dispose()

                    Case "GraphicalAction"
                        Dim ActionDlgForm As New ActionDialogForm(m_Sfc.ResGlobalVariables)
                        Dim a As GraphicalAction
                        ActionDlgForm.Nome = Obj.Name
                        ActionDlgForm.Qualifier = Obj.Qualifier
                        ActionDlgForm.Time = Obj.Time

                        '----------------------------
                        'Blocco aggiuntivo alla norma
                        Select Case Obj.Qualifier
                            Case ActionType(9), ActionType(10) 'Forzature
                                ActionDlgForm.RefSfc = Obj.ReadRefSfc
                                ActionDlgForm.StepsToForces = Obj.ReadStepToForces
                            Case ActionType(11), ActionType(12) 'Bloc, Sosp
                                ActionDlgForm.RefSfc = Obj.ReadRefSfc
                            Case Else
                                '----------------------------
                                If Not IsNothing(Obj.Variable) Then
                                    ActionDlgForm.Var = Obj.Variable
                                End If
                                If Not IsNothing(Obj.Indicator) Then
                                    ActionDlgForm.Indicator = Obj.Indicator
                                End If

                                '----------------------------
                                'Blocco aggiuntivo alla norma
                        End Select


                        '----------------------------
                        Dim ResultDialog As System.Windows.Forms.DialogResult = ActionDlgForm.ShowDialog()
                        If ResultDialog = DialogResult.OK Then
                            Obj.Qualifier = ActionDlgForm.Qualifier
                            Obj.Name = ActionDlgForm.Nome
                            Obj.Time = ActionDlgForm.Time

                            '----------------------------
                            'Blocco aggiuntivo alla norma
                            Select Case ActionDlgForm.Qualifier
                                Case ActionType(9), ActionType(10)  'Forzature
                                    Obj.SetRefSfc(ActionDlgForm.RefSfc)
                                    Obj.SetStepToForces(ActionDlgForm.StepsToForces)
                                Case ActionType(11), ActionType(12) 'Bloc, Sosp
                                    Obj.SetRefSfc(ActionDlgForm.RefSfc)
                                Case Else
                                    '----------------------------
                                    If Not IsNothing(ActionDlgForm.Var) Then
                                        Obj.Variable = ActionDlgForm.Var
                                    Else
                                        Obj.Variable = Nothing
                                    End If
                                    If Not IsNothing(ActionDlgForm.Var) Then
                                        Obj.Indicator = ActionDlgForm.Indicator
                                    Else
                                        Obj.Indicator = Nothing
                                    End If
                                    '----------------------------
                                    'Blocco aggiuntivo alla norma
                            End Select
                            '----------------------------



                        End If
                End Select
                CurrentOperation = Operation.Selection
                CancelVisibleArea()
                DrawVisibleArea()
            End If

        Else
            'Non è in modo di editing quindi apre solo le macrofasi
            'Controlla se è selezionata
            If m_Sfc.CountSelectedElement = 1 Then
                Dim Obj As Object = m_Sfc.ReadObjectSelected
                Select Case Obj.GetType.Name
                    Case "GraphicalMacroStep"
                        ShowMacroStepBody(Obj.ReadBody)
                        m_Sfc.DeSelectAll()
                End Select

            End If
        End If
        RefreshTreeStruct()
    End Sub
    Private Sub FormSfc_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        'Entra cou un monitor in m_GraphToDraw per aspettare che l'sfc finisca il disegno dello stato
        Monitor.Enter(Me.ReadBody.ReadSfc)
        Monitor.Enter(m_GraphToDraw)
        m_Sfc.StopStateMonitor()
        Monitor.Exit(m_GraphToDraw)
        Monitor.Exit(Me.ReadBody.ReadSfc)
        m_Sfc = Nothing
    End Sub
    Private Sub toolBar1_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles toolBar1.ButtonClick
        If e.Button Is NewStepButton Then
            AddStep()
        ElseIf e.Button Is NewMacroStepButton Then
            AddMacroStep()
        ElseIf e.Button Is NewActionButton Then
            AddAction()
        ElseIf e.Button Is SetIniButton Then
            SetInitial()
        ElseIf e.Button Is SetFinButton Then
            SetFinal()
        ElseIf e.Button Is DelButton Then
            RemoveSelectedElements()
        ElseIf e.Button Is NewTransButton Then
            AddTransition()
        ElseIf e.Button Is ShowTreeButton Then
            SwitchTreeShow()
        ElseIf e.Button Is ShowTransitionDetailButton Then
            SetShowdetail()
        ElseIf e.Button Is ShowCursor Then
            ResetCurrentOperation()
        End If
    End Sub
    Private Sub StartSfcScanning() Handles m_Sfc.StartScan
        'Inverte i led di esecuzione
        ExecuteBar1.NextValue()
    End Sub
    Private Sub MenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem2.Click
        AddStep()
    End Sub
    Private Sub MenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem3.Click
        AddAction()
    End Sub
    Private Sub MenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem4.Click
        AddTransition()
    End Sub
    Private Sub MenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem5.Click
        RemoveSelectedElements()
    End Sub
    Private Sub MenuItem6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem6.Click
        SetInitial()
    End Sub
    Private Sub MenuItem7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem7.Click
        SetFinal()
    End Sub
    Private Sub MenuItem8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem8.Click
        SwitchTreeShow()
    End Sub
    Private Sub MenuItem9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem9.Click
        SetShowdetail()
    End Sub
    Private Sub MenuItem10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem10.Click
        ResetCurrentOperation()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Select Case m_monitoring
            Case False
                StartMonitor()
                m_monitoring = True
                Button1.BackColor = Color.Brown
            Case True
                StopMonitor()
                m_monitoring = False
                Button1.BackColor = Color.Gray
        End Select
    End Sub
End Class

