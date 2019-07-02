Imports System.Drawing
Imports System.Math
Imports System.Xml
Imports System.Threading
Imports System.Collections
Imports System.Collections.Generic
Imports System.Windows.Forms
Public Class FormSfc
    Inherits System.Windows.Forms.Form
    Implements IEditorForm

    Private m_Pou As pou
    Private WithEvents m_Body As body
    Private WithEvents m_Sfc As Sfc
    Private m_GraphToDraw As Drawing.Graphics
    Private EditingMode As Boolean
    Private m_monitoring As Boolean
    Private Dimension As Integer
    Private Foglio As Drawing.Rectangle
    Private ColGriglia As Drawing.Color
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
    Private InitialPoint As Drawing.Point
    Private PreviousFinelPoint As Drawing.Point
    Private ColSfondo As Drawing.Color = Drawing.Color.White
    Private TreeOpen As Boolean = False
    Private ShowDetails As Boolean = True
    Private CursorStep As New Cursor(IO.Path.Combine(CursorsPath, "CursorStep.Cur"))
    Private CursorMacroStep As New Cursor(IO.Path.Combine(CursorsPath, "CursorMacroStep.Cur"))
    Private CursorTransition As New System.Windows.Forms.Cursor(IO.Path.Combine(CursorsPath, "CursorTransition.Cur"))
    Private CursorMove As New Cursor(IO.Path.Combine(CursorsPath, "CursorMove.cur"))
    Friend WithEvents stsComments As System.Windows.Forms.StatusStrip
    Friend WithEvents lblComment As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents btnEdit As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents VariablesMenu As System.Windows.Forms.ToolBarButton
    Friend WithEvents MenuItemExportJPEG As System.Windows.Forms.MenuItem
    Friend WithEvents sdgJPEGFile As System.Windows.Forms.SaveFileDialog

    Dim WithEvents SollevaEvento As New BooleanVariable
    Friend WithEvents MenuItemToLadder As System.Windows.Forms.MenuItem
    Private m_MonitorStatus As Hydra(Of Boolean, String, Object)

    Private Enum Operation
        DefiningStep
        DefiningMacroStep
        DefiningTransition
        Selection
        MultipleSelection
        Selected
    End Enum
#Region " Codice generato da ProgettAction Windows Form "

    Public Sub New(ByRef RefPou As pou, ByRef RefBody As body, ByVal Editing As Boolean)

        MyBase.New()
        m_Pou = RefPou
        m_Body = RefBody '(in una macroazione il body non è quello della Pou)
        m_Sfc = m_Body.ReadSfc
        EditingMode = Editing
        CurrentOperation = Operation.Selection
        Foglio.Width = 800
        Foglio.Height = 1200
        CoeffSnap = 4
        BrSfondo = New Drawing.SolidBrush(ColSfondo)
        InitialPoint = New Drawing.Point(0, 0)



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

        m_MonitorStatus = New Hydra(Of Boolean, String, Object)
        m_MonitorStatus.AddValueToAllEntries("color", New KeyValuePair(Of Boolean, Object)(True, Color.Brown), _
            New KeyValuePair(Of Boolean, Object)(False, Color.Gray))

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
    Friend WithEvents toolBar1 As System.Windows.Forms.ToolBar
    Friend WithEvents NewStepButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents NewMacroStepButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents NewActionButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents NewTransButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents SetIniButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents SetFinButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton3 As System.Windows.Forms.ToolBarButton
    Friend WithEvents DelButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents ToolBarButton1 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ShowCursor As System.Windows.Forms.ToolBarButton
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ToolBarButton2 As System.Windows.Forms.ToolBarButton
    Friend WithEvents StatusBarPanel1 As System.Windows.Forms.StatusBarPanel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ExecuteBar1 As ExecuteBar
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents MenuItem11 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemResize As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormSfc))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.toolBar1 = New System.Windows.Forms.ToolBar
        Me.NewStepButton = New System.Windows.Forms.ToolBarButton
        Me.NewMacroStepButton = New System.Windows.Forms.ToolBarButton
        Me.NewActionButton = New System.Windows.Forms.ToolBarButton
        Me.NewTransButton = New System.Windows.Forms.ToolBarButton
        Me.SetIniButton = New System.Windows.Forms.ToolBarButton
        Me.SetFinButton = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton3 = New System.Windows.Forms.ToolBarButton
        Me.DelButton = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton1 = New System.Windows.Forms.ToolBarButton
        Me.ShowCursor = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton2 = New System.Windows.Forms.ToolBarButton
        Me.VariablesMenu = New System.Windows.Forms.ToolBarButton
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.MenuItem4 = New System.Windows.Forms.MenuItem
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.MenuItem11 = New System.Windows.Forms.MenuItem
        Me.MenuItemResize = New System.Windows.Forms.MenuItem
        Me.MenuItemExportJPEG = New System.Windows.Forms.MenuItem
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.StatusBarPanel1 = New System.Windows.Forms.StatusBarPanel
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.ExecuteBar1 = New UnisimClassLibrary.ExecuteBar
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.stsComments = New System.Windows.Forms.StatusStrip
        Me.lblComment = New System.Windows.Forms.ToolStripStatusLabel
        Me.btnEdit = New System.Windows.Forms.ToolStripDropDownButton
        Me.sdgJPEGFile = New System.Windows.Forms.SaveFileDialog
        Me.MenuItemToLadder = New System.Windows.Forms.MenuItem
        Me.Panel2.SuspendLayout()
        CType(Me.StatusBarPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.stsComments.SuspendLayout()
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
        'toolBar1
        '
        Me.toolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.NewStepButton, Me.NewMacroStepButton, Me.NewActionButton, Me.NewTransButton, Me.SetIniButton, Me.SetFinButton, Me.ToolBarButton3, Me.DelButton, Me.ToolBarButton1, Me.ShowCursor, Me.ToolBarButton2, Me.VariablesMenu})
        Me.toolBar1.ButtonSize = New System.Drawing.Size(24, 24)
        Me.toolBar1.DropDownArrows = True
        Me.toolBar1.ImageList = Me.ImageList1
        Me.toolBar1.Location = New System.Drawing.Point(0, 0)
        Me.toolBar1.Name = "toolBar1"
        Me.toolBar1.ShowToolTips = True
        Me.toolBar1.Size = New System.Drawing.Size(492, 36)
        Me.toolBar1.TabIndex = 5
        '
        'NewStepButton
        '
        Me.NewStepButton.ImageIndex = 6
        Me.NewStepButton.Name = "NewStepButton"
        Me.NewStepButton.ToolTipText = "Step"
        '
        'NewMacroStepButton
        '
        Me.NewMacroStepButton.ImageIndex = 9
        Me.NewMacroStepButton.Name = "NewMacroStepButton"
        Me.NewMacroStepButton.ToolTipText = "MacroStep"
        '
        'NewActionButton
        '
        Me.NewActionButton.ImageIndex = 12
        Me.NewActionButton.Name = "NewActionButton"
        Me.NewActionButton.ToolTipText = "Action"
        '
        'NewTransButton
        '
        Me.NewTransButton.ImageIndex = 5
        Me.NewTransButton.Name = "NewTransButton"
        Me.NewTransButton.ToolTipText = "Transition"
        '
        'SetIniButton
        '
        Me.SetIniButton.ImageIndex = 10
        Me.SetIniButton.Name = "SetIniButton"
        Me.SetIniButton.ToolTipText = "InitialStep (yes/no)"
        '
        'SetFinButton
        '
        Me.SetFinButton.ImageIndex = 15
        Me.SetFinButton.Name = "SetFinButton"
        Me.SetFinButton.ToolTipText = "FinalStep (yes/No)"
        '
        'ToolBarButton3
        '
        Me.ToolBarButton3.Name = "ToolBarButton3"
        Me.ToolBarButton3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'DelButton
        '
        Me.DelButton.ImageIndex = 11
        Me.DelButton.Name = "DelButton"
        Me.DelButton.ToolTipText = "Cancel"
        '
        'ToolBarButton1
        '
        Me.ToolBarButton1.Name = "ToolBarButton1"
        Me.ToolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'ShowCursor
        '
        Me.ShowCursor.ImageIndex = 16
        Me.ShowCursor.Name = "ShowCursor"
        Me.ShowCursor.ToolTipText = "Cursor"
        '
        'ToolBarButton2
        '
        Me.ToolBarButton2.Name = "ToolBarButton2"
        Me.ToolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'VariablesMenu
        '
        Me.VariablesMenu.ImageIndex = 18
        Me.VariablesMenu.Name = "VariablesMenu"
        Me.VariablesMenu.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "")
        Me.ImageList1.Images.SetKeyName(1, "")
        Me.ImageList1.Images.SetKeyName(2, "")
        Me.ImageList1.Images.SetKeyName(3, "")
        Me.ImageList1.Images.SetKeyName(4, "")
        Me.ImageList1.Images.SetKeyName(5, "")
        Me.ImageList1.Images.SetKeyName(6, "")
        Me.ImageList1.Images.SetKeyName(7, "")
        Me.ImageList1.Images.SetKeyName(8, "")
        Me.ImageList1.Images.SetKeyName(9, "")
        Me.ImageList1.Images.SetKeyName(10, "")
        Me.ImageList1.Images.SetKeyName(11, "")
        Me.ImageList1.Images.SetKeyName(12, "")
        Me.ImageList1.Images.SetKeyName(13, "")
        Me.ImageList1.Images.SetKeyName(14, "")
        Me.ImageList1.Images.SetKeyName(15, "")
        Me.ImageList1.Images.SetKeyName(16, "")
        Me.ImageList1.Images.SetKeyName(17, "")
        Me.ImageList1.Images.SetKeyName(18, "Data_Dataset.ico")
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem1, Me.MenuItem11})
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 0
        Me.MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem2, Me.MenuItem3, Me.MenuItem4, Me.MenuItem5, Me.MenuItemToLadder})
        Me.MenuItem1.MergeOrder = 4
        Me.MenuItem1.Text = "&Sfc"
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 0
        Me.MenuItem2.Text = "Add &step"
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 1
        Me.MenuItem3.Text = "Add &action"
        '
        'MenuItem4
        '
        Me.MenuItem4.Index = 2
        Me.MenuItem4.Text = "Add &transition"
        '
        'MenuItem5
        '
        Me.MenuItem5.Index = 3
        Me.MenuItem5.Text = "&Delete selected elements"
        '
        'MenuItem11
        '
        Me.MenuItem11.Index = 1
        Me.MenuItem11.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemResize, Me.MenuItemExportJPEG})
        Me.MenuItem11.MergeOrder = 4
        Me.MenuItem11.Text = "&Worksheet"
        '
        'MenuItemResize
        '
        Me.MenuItemResize.Index = 0
        Me.MenuItemResize.Text = "&Resize..."
        '
        'MenuItemExportJPEG
        '
        Me.MenuItemExportJPEG.Index = 1
        Me.MenuItemExportJPEG.Text = "Save as JPEG..."
        '
        'Panel2
        '
        Me.Panel2.AutoScroll = True
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel2.Controls.Add(Me.Panel1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 36)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(492, 109)
        Me.Panel2.TabIndex = 6
        '
        'StatusBarPanel1
        '
        Me.StatusBarPanel1.Name = "StatusBarPanel1"
        Me.StatusBarPanel1.Text = "Mode:"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.ExecuteBar1)
        Me.Panel3.Controls.Add(Me.Label1)
        Me.Panel3.Controls.Add(Me.Button1)
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.ForeColor = System.Drawing.Color.Black
        Me.Panel3.Location = New System.Drawing.Point(0, 145)
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
        Me.Button1.UseVisualStyleBackColor = False
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
        'stsComments
        '
        Me.stsComments.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblComment, Me.btnEdit})
        Me.stsComments.Location = New System.Drawing.Point(0, 163)
        Me.stsComments.Name = "stsComments"
        Me.stsComments.Size = New System.Drawing.Size(492, 22)
        Me.stsComments.TabIndex = 10
        '
        'lblComment
        '
        Me.lblComment.BackColor = System.Drawing.SystemColors.Info
        Me.lblComment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.lblComment.ForeColor = System.Drawing.SystemColors.InfoText
        Me.lblComment.Name = "lblComment"
        Me.lblComment.Size = New System.Drawing.Size(448, 17)
        Me.lblComment.Spring = True
        '
        'btnEdit
        '
        Me.btnEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEdit.Image = CType(resources.GetObject("btnEdit.Image"), System.Drawing.Image)
        Me.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(29, 20)
        Me.btnEdit.Text = "ToolStripDropDownButton1"
        '
        'sdgJPEGFile
        '
        Me.sdgJPEGFile.Filter = "JPEG(*.jpg)|*.jpg"
        Me.sdgJPEGFile.Title = "Save as JPEG"
        '
        'MenuItemToLadder
        '
        Me.MenuItemToLadder.Index = 4
        Me.MenuItemToLadder.Text = "Set up &Ladder equivalent"
        '
        'FormSfc
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(492, 185)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.toolBar1)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.stsComments)
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
        Me.stsComments.ResumeLayout(False)
        Me.stsComments.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Public Sub AddStep()
        ResetCurrentOperation()
        'Cerca il numero della prima fase libera
        'e passa il valore alla finestra di dialogo
        Dim StepDialog As New StepDialogForm
        StepDialog.StepName = m_Sfc.FirstAvaiableStepName
        Dim ResultDialog As System.Windows.Forms.DialogResult = StepDialog.ShowDialog()
        If ResultDialog = Windows.Forms.DialogResult.OK Then
            'Controlla se il nome della fase è già presente
            If Not IsNothing(m_Sfc.FindStepByName(StepDialog.StepName)) Then
                MsgBox("Step " & StepDialog.StepName & " already exists", MsgBoxStyle.Critical, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
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
        If ResultDialog = Windows.Forms.DialogResult.OK Then
            'Controlla se il nome è già presente
            If Not IsNothing(m_Sfc.FindStepByName(MacroStepDialog.StepName)) = True Then
                MsgBox("Macro step " & MacroStepDialog.StepName & " already exists", MsgBoxStyle.Critical, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
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
            Dim ActionDialog As New ActionDialogForm(m_Sfc.ResGlobalVariables, m_Pou.PouInterface, m_Pou.Pous, m_Pou.Name)
            Dim ResultDialog As System.Windows.Forms.DialogResult = ActionDialog.ShowDialog()
            If ResultDialog = Windows.Forms.DialogResult.OK Then
                ' bug fix per consentire alle azioni di tipo "operazione aritmetica"
                ' di funzionare prima di salvare il progetto
                Dim RefSfc As Sfc = ActionDialog.RefSfc
                If IsNothing(RefSfc) And ActionDialog.m_qualifier = "PO" Then _
                    RefSfc = Me.m_Sfc
                StoreActions(ActionDialog.m_name, ActionDialog.m_qualifier, ActionDialog.m_variable, ActionDialog.m_indicator, ActionDialog.m_time, RefSfc, ActionDialog.StepsToForces, ActionDialog.ArithExp)
                DrawVisibleArea()
            End If
        Else
            MsgBox("No step selected", MsgBoxStyle.Critical, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
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
        If ResultDialog = Windows.Forms.DialogResult.OK Then
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
            MsgBox("No steps selected", MsgBoxStyle.Information, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
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
            MsgBox("No steps selected", MsgBoxStyle.Information, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
        End If
    End Sub
    Public Sub RemoveSelectedElements()
        If m_Sfc.GraphicalStepsList.ReadSelected.CountSelected > 0 Or m_Sfc.GraphicalStepsList.ReadSelectedStepsActionsList.Count > 0 Or m_Sfc.GraphicalTransitionsList.ReadSelectedTransitionList.CountSelected > 0 Then
            If m_Sfc.GraphicalTransitionsList.ControllaPresenzaStepsInTransizioni(m_Sfc.GraphicalStepsList.ReadSelected) Then
                MsgBox("Steps that are source or destination of a transition can't be deleted", MsgBoxStyle.Critical, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
            Else
                If My.Computer.Keyboard.ShiftKeyDown OrElse (MsgBox("Do you really want to delete the selected elements?", MsgBoxStyle.OkCancel Or MsgBoxStyle.Question, UniSimVersion.VersionInfo.PrintableDescriptionForTool("")) = MsgBoxResult.Ok) Then
                    m_Sfc.RemoveSelectedElements()
                    CancelVisibleArea()
                    DrawVisibleArea()
                End If
                CurrentOperation = Operation.Selection
            End If
        Else
            MsgBox("Nothing selected to delete", MsgBoxStyle.Information, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
        End If
    End Sub
    Public Sub WriteTitlePanel() Implements IEditorForm.WriteTitlePanel
        Me.Text = m_Sfc.Name
    End Sub
    Public Sub StartMonitor() Implements IEditorForm.StartMonitor
        m_Sfc.StartStateMonitor()
    End Sub
    Public Sub StopMonitor() Implements IEditorForm.StopMonitor
        m_Sfc.StopStateMonitor()
        CancelVisibleArea()
        DrawVisibleArea()
    End Sub
    Public Function ReadBody() As body Implements IEditorForm.ReadBody
        ReadBody = m_Body
    End Function
    Public Sub ResetCurrentOperation() Implements IEditorForm.ResetCurrentOperation
        CurrentOperation = Operation.Selection
        CancelVisibleArea()
        DrawVisibleArea()
    End Sub
    Private Function StoreStep(ByVal P As Drawing.Point) As Boolean
        StoreStep = m_Sfc.AddAndDrawStep(m_Sfc.FirstAvaiableElementNumber, NextElementName, NextElementDocumentation, P)
        ' Se c'è una sola fase nel diagramma...
        If m_Sfc.GraphicalStepsList.Count = 1 Then
            Dim step0 As BaseGraphicalStep = _
                CType(m_Sfc.GraphicalStepsList.Item(0), _
                    BaseGraphicalStep)
            ' e non è una macrofase allora rendila fase iniziale
            ' (se la preferenza Step0IsInitial = True)
            If TypeOf (step0) Is GraphicalStep Then step0.SetInitial( _
                Preferences.GetBoolean("Step0IsInitial", True))
        End If
        DrawVisibleArea()
    End Function
    Private Function StoreMacroStep(ByVal P As Drawing.Point) As Boolean
        StoreMacroStep = m_Sfc.AddAndDrawMacroStep(m_Sfc.FirstAvaiableElementNumber, NextElementName, NextElementDocumentation, P)
    End Function
    Private Sub ConfirmAddTransition()
        If StepsSelectedForTransitionOk() Then
            StoreTransition()
            CurrentOperation = Operation.Selection
            CancelVisibleArea()
            DeSelectAll()
            DrawVisibleArea()
        Else
            MsgBox("You must select source and destination steps before adding a transition" + vbCrLf + "If you choose more than one source and one destination, divergences and convergences shall be created as appropriate", MsgBoxStyle.Information, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
        End If
    End Sub
    Private Function StepsSelectedForTransitionOk() As Boolean
        If m_Sfc.GraphicalStepsList.ReadBottomSelectedStepList.CountStepsList > 0 And m_Sfc.GraphicalStepsList.ReadTopSelectedStepList.CountStepsList > 0 Then
            Return True
        End If
        Return False
    End Function
    Private Function StoreTransition() As Boolean
        Dim tCount As Integer = m_Sfc.GraphicalTransitionsList.Count + 1
        m_Sfc.AddAndDrawTransition(m_Sfc.FirstAvaiableElementNumber, m_Sfc.FirstAvaiableTransitionName(), NextElementDocumentation, NextCondition)
    End Function
    Private Sub StoreActions(ByVal Nome As String, ByVal Qual As String, ByVal Var As BaseVariable, ByVal VarInd As BaseVariable, ByVal Time As TimeSpan, ByRef RefSfc As Sfc, ByVal StepToForces As GraphicalStepsList, ByVal ArithExp As String)
        m_Sfc.AddActionToSelectedSteps(Nome, Qual, Var, VarInd, Time, RefSfc, StepToForces, ArithExp)
    End Sub
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
    Private Sub MoveSelection(ByVal dx As Integer, ByVal dy As Integer)
        CancelSelection()
        m_Sfc.MoveSelection(dx, dy)
        DrawVisibleArea()
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
            Dim FrSfcTemp As New FormSfc(m_Pou, RefBody, EditingMode)
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
    ' Impostiamo la dimensione del pannello ad almeno 1200*1200 pixels, ma
    ' se ci sono oggetti più in basso o più a destra di questo limite, aumentiamo
    ' la dimensione del pannello in modo da consentire di mostrare tutto il diagramma
    Private Sub InitialPanelSizing()
        Dim rightMost As IGraphicalObject = m_Sfc.GetRightmostObject
        Dim lowerMost As IGraphicalObject = m_Sfc.GetLowermostObject
        Dim myWantedSize As New Drawing.Size(1200, 1200)
        Dim rmX As Integer = -1
        If rightMost IsNot Nothing Then rmX = rightMost.Position.X + rightMost.Size.Width
        Dim lmY As Integer = -1
        If lowerMost IsNot Nothing Then lmY = lowerMost.Position.Y + lowerMost.Size.Height
        rmX = (110 * rmX) / 100 ' 110%
        lmY = (110 * lmY) / 100 ' 110%
        myWantedSize.Width = Math.Max(myWantedSize.Width, rmX)
        myWantedSize.Height = Math.Max(myWantedSize.Height, lmY)
        Me.ResizePanel(myWantedSize)
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        WriteTitlePanel()
        ' Attiva la modalità "Monitoraggio" all'apertura della finestra
        Button1_Click(sender, e)
        ' Mostra i commenti sin dall'inizio
        Call Me.SetToolTip()
        Call Me.InitialPanelSizing()
    End Sub
    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint
        DrawArea(e.ClipRectangle)
    End Sub
    Private Sub Panel1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseDown
        Me.Activate()

        If EditingMode Then
            'Legge il tasto del mouse premuto
            Select Case e.Button
                Case Windows.Forms.MouseButtons.Left
                    'Tasto sinistro del mouse
                    Select Case CurrentOperation
                        Case Is = Operation.DefiningStep
                            Dim P As New Drawing.Point(Snap(e.X), Snap(e.Y))
                            'Chiama AddAndDrawStep
                            If StoreStep(P) Then  'quando ha fatto setta a true
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

                Case Windows.Forms.MouseButtons.Middle, Windows.Forms.MouseButtons.Right
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
        ' elimina le sbavature presenti su alcuni sistemi
        Panel1.Refresh()
        ' imposta il tooltip di descrizione degli oggetti
        Me.SetToolTip()
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
                        If ResultDialog = Windows.Forms.DialogResult.OK Then
                            'Controlla se il mome della fase è già presente e se il numero è stato cambiato
                            If StepDialog.StepName <> Obj.Name Then
                                If Not IsNothing(m_Sfc.FindStepByName(StepDialog.StepName)) Then
                                    MsgBox("Step " & StepDialog.StepName & " already exists", MsgBoxStyle.Critical, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
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
                        If ResultDialog = Windows.Forms.DialogResult.OK Or ResultDialog = Windows.Forms.DialogResult.Yes Then
                            'Controlla se il nome della fase è già presente e se il numero è stato cambiato
                            If MacroStepDialog.StepName <> Obj.Name Then
                                If Not IsNothing(m_Sfc.FindStepByName(MacroStepDialog.StepName)) Then
                                    MsgBox("Macro step " & MacroStepDialog.StepName & " already exists", MsgBoxStyle.Critical, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
                                Else
                                    'Imposta il nuovo nome
                                    Obj.Name = MacroStepDialog.StepName
                                End If
                            End If
                            If ResultDialog = Windows.Forms.DialogResult.Yes Then
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
                        If ResultDialog = Windows.Forms.DialogResult.OK Then
                            'Controlla se il nome della transizione è già presente se il numero è stato cambiato
                            If TransitionDlgForm.m_Name <> Obj.Name Then
                                'Controlla se il nome non è gia presente
                                If Not IsNothing(m_Sfc.FindTransitionByName(TransitionDlgForm.m_Name)) Then
                                    MsgBox("Transition " & TransitionDlgForm.m_Name & " already exists", MsgBoxStyle.Critical, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
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
                        Dim ActionDlgForm As New ActionDialogForm(m_Sfc.ResGlobalVariables, m_Pou.PouInterface, m_Pou.Pous, m_Pou.Name)
                        ActionDlgForm.m_name = Obj.Name
                        ActionDlgForm.m_qualifier = Obj.Qualifier
                        ActionDlgForm.m_time = Obj.Time

                        If Obj.Qualifier = "PO" Then
                            ActionDlgForm.ArithExp = Obj.ReadArithExp()
                        Else : ActionDlgForm.ArithExp = ""
                        End If


                        '----------------------------
                        'Blocco aggiuntivo alla norma
                        Select Case CStr(Obj.Qualifier)
                            Case ActionType(9), ActionType(10) 'Forzature
                                ActionDlgForm.RefSfc = Obj.ReadRefSfc
                                ActionDlgForm.StepsToForces = Obj.ReadStepToForces
                            Case ActionType(11), ActionType(12) 'Bloc, Sosp
                                ActionDlgForm.RefSfc = Obj.ReadRefSfc
                            Case Else

                                '----------------------------
                                If Not IsNothing(Obj.Variable) Then
                                    ActionDlgForm.m_variable = Obj.Variable
                                End If
                                If Not IsNothing(Obj.Indicator) Then
                                    ActionDlgForm.m_indicator = Obj.Indicator
                                End If

                                '----------------------------
                                'Blocco aggiuntivo alla norma
                        End Select


                        '----------------------------
                        Dim ResultDialog As System.Windows.Forms.DialogResult = ActionDlgForm.ShowDialog()
                        If ResultDialog = Windows.Forms.DialogResult.OK Then
                            Obj.Qualifier = ActionDlgForm.m_qualifier
                            Obj.Name = ActionDlgForm.m_name
                            Obj.Time = ActionDlgForm.m_time
                            Obj.SetArithExp(ActionDlgForm.ArithExp)
                            Obj.Type = ActionDlgForm.m_type

                            '----------------------------
                            'Blocco aggiuntivo alla norma
                            Select Case ActionDlgForm.m_qualifier
                                Case ActionType(9), ActionType(10)  'Forzature
                                    Obj.SetRefSfc(ActionDlgForm.RefSfc)
                                    Obj.SetStepToForces(ActionDlgForm.StepsToForces)
                                    Obj.Variable = Nothing
                                Case ActionType(11), ActionType(12) 'Bloc, Sosp
                                    Obj.SetRefSfc(ActionDlgForm.RefSfc)
                                    Obj.Variable = Nothing
                                Case Else
                                    '----------------------------
                                    If Not IsNothing(ActionDlgForm.m_variable) Then
                                        Obj.Variable = ActionDlgForm.m_variable
                                    Else
                                        Obj.Variable = Nothing
                                    End If
                                    If Not IsNothing(ActionDlgForm.m_variable) Then
                                        Obj.Indicator = ActionDlgForm.m_indicator
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
    End Sub
    Private Sub FormSfc_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        'Entra cou un monitor in m_GraphToDraw per aspettare che l'sfc finisca il disegno dello stato
        Monitor.Enter(Me.ReadBody.ReadSfc)
        Monitor.Enter(m_GraphToDraw)
        m_Sfc.StopStateMonitor()
        Monitor.Exit(m_GraphToDraw)
        Monitor.Exit(Me.ReadBody.ReadSfc)
        Me.Dispose()
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
        ElseIf e.Button Is ShowCursor Then
            ResetCurrentOperation()
        End If
    End Sub
    Private Sub StartSfcScanning() Handles m_Sfc.StartScan
        Try
            ExecuteBar1.NextValue()
        Catch ex As Exception
        End Try
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
    Private Sub MenuItem6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SetInitial()
    End Sub
    Private Sub MenuItem7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SetFinal()
    End Sub
    Private Sub MenuItem10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ResetCurrentOperation()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Select Case m_monitoring
            Case False
                StartMonitor()
                m_monitoring = True
            Case True
                StopMonitor()
                m_monitoring = False
        End Select
        Button1.BackColor = CType(m_MonitorStatus(m_monitoring)("color"), Color)
    End Sub

    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs)

    End Sub

    Private Sub MenuItem12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemResize.Click
        Dim resizeDialog As New ResizePanelDialogForm(Panel1.Size)
        If resizeDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then _
            ResizePanel(resizeDialog.NewSize)
        resizeDialog.Dispose()
    End Sub

    Private Sub ResizePanel(ByVal newSize As Size) Implements IEditorForm.ResizePanel
        Panel1.Size = newSize
        Foglio.Width = newSize.Width
        Foglio.Height = newSize.Height
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
    End Sub

    ' Ritorna l'oggetto che in questo momento si ritiene essere il target
    ' per la gestione dei commenti in base alle seguenti regole:
    ' 0) Solo in modo editing e in fase di selezione c'è un target
    ' 1) Se ci sono più oggetti selezionati, nessuno è il target
    ' 2) Se c'è un solo oggetto selezionato ed è IDocumentable è il target
    ' 2bis) Se l'unico oggetto selezionato non è IDocumentabile nessuno è il target
    ' 3) Se non ci sono oggetti selezionati il body è il target
    Public Function GetCurrentIDocumentable() As IDocumentable Implements IEditorForm.GetCurrentIDocumentable
        If Not EditingMode Then Return Nothing
        If CurrentOperation <> Operation.Selection Then Return Nothing
        If m_Sfc.CountSelectedElement() > 1 OrElse Me.MultipleSelection Then Return Nothing
        If m_Sfc.CountSelectedElement() = 1 Then
            Dim Obj As Object = m_Sfc.ReadObjectSelected
            If TypeOf (Obj) Is BaseGraphicalStep OrElse _
                TypeOf (Obj) Is GraphicalAction OrElse _
                TypeOf (Obj) Is GraphicalTransition Then Return CType(Obj, IDocumentable)
            Return Nothing
        End If
        If m_Sfc.CountSelectedElement() = 0 Then Return m_Body
        ' fa contento Visual Basic ma è inutile, i percorsi sono tutti coperti
        Return Nothing
    End Function

    ' Imposta il tooltip da mostrare in base alla selezione corrente
    Public Sub SetToolTip() Implements IEditorForm.SetToolTip
        Dim iComm As IDocumentable = GetCurrentIDocumentable()
        If Not (iComm Is Nothing) Then
            Dim fullText As String = iComm.GetDescription
            Dim sysText As String = iComm.GetSystemDescription + " ->"
            If lblComment.GetCurrentParent().CreateGraphics().MeasureString(fullText, lblComment.Font).Width > lblComment.Width Then
                lblComment.Text = sysText
            Else
                lblComment.Text = fullText
            End If
        Else
            lblComment.Text = "(no selection)"
        End If
    End Sub

    ' Edita il commento attuale
    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        Dim iComm As IDocumentable = GetCurrentIDocumentable()
        If iComm Is Nothing Then Return
        Dim commEditorWnd As New CommentsDialogForm(iComm)
        If commEditorWnd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            iComm.Documentation = commEditorWnd.CommentTypedIn
            SetToolTip()
        End If
    End Sub

    ' Crea il menu per l'editor variabili
    Private Sub toolBar1_ButtonDropDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles toolBar1.ButtonDropDown
        If e.Button Is Me.VariablesMenu Then
            Dim myMenu As ContextMenu = New ContextMenu()
            myMenu.MenuItems.Add(m_Sfc.PouInterface.localVars.GetMenu())
            If Not m_Sfc.ResGlobalVariables Is Nothing Then
                For Each VL As VariablesList In m_Sfc.ResGlobalVariables
                    If VL Is Nothing Then Continue For
                    myMenu.MenuItems.Add(VL.GetMenu())
                Next
            End If
            e.Button.DropDownMenu = myMenu
        End If
    End Sub

    ' Esporta il diagramma in formato JPEG (utile per le tesine di TSA)
    Private Sub ExportAsJPEG(ByVal destFile As String)
        Dim imgSize As New Rectangle(Foglio.X, Foglio.Y, _
            Foglio.Width, Foglio.Height)
        Dim img As New Bitmap(imgSize.Width, imgSize.Height)
        Dim imgGraphics As Graphics = _
            Graphics.FromImage(img)
        imgGraphics.FillRectangle(Brushes.White, _
            imgSize)
        m_Sfc.PrintMe(imgGraphics, _
            imgSize)
        img.Save(destFile, Imaging.ImageFormat.Jpeg)
    End Sub

    Private Sub MenuItemExportJPEG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemExportJPEG.Click
        If sdgJPEGFile.ShowDialog() = Windows.Forms.DialogResult.OK Then _
            ExportAsJPEG(sdgJPEGFile.FileName)
    End Sub

    Private Sub MenuItemToLadder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemToLadder.Click
        Dim ladder As pou = Me.m_Pou.Pous.AddPou(m_Sfc.Name + "LD", "", EnumPouType.Program, EnumBodyType.tLD)
        SFC2Ladder.Translate(m_Sfc, ladder.Body.ReadLadder())
        Dim myparent As IUniSimMainWindow = CType(Me.MdiParent, IUniSimMainWindow)
        myparent.GetResourceTree().AddPouInstance(ladder)
        myparent.GetPOUsTree().RefreshTreeStruct()
        myparent.GetResourceTree().RefreshTreeStruct()
    End Sub
End Class

