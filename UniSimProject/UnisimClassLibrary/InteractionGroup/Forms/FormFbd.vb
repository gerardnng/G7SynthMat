Imports System.Drawing
Imports System.Math
Imports System.Xml
Imports System.Threading
Imports System.Collections
Imports System.Collections.Generic
Imports System.Windows.Forms
Public Class FormFbd
    Inherits System.Windows.Forms.Form
    Implements IEditorForm

    Private m_Pou As pou
    Private WithEvents m_Body As body
    Private WithEvents m_Fbd As Fbd
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
    Private m_MonitorStatus As Hydra(Of Boolean, String, Object)

    ' la variabile da aggiungere
    Private boundVariable As BaseVariable
    Private boundVarType As GraphicalVariableType
    Private boundExecID As Integer
    Friend WithEvents MenuItem6 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSelOutgoingConns As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSelIncomingConns As System.Windows.Forms.MenuItem
    Friend WithEvents IncomingButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents OutgoingButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton4 As System.Windows.Forms.ToolBarButton
    Friend WithEvents DisconnectButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents MenuItem7 As System.Windows.Forms.MenuItem
    Friend WithEvents SwapParamsButton As System.Windows.Forms.ToolBarButton

    ' il blocco da aggiungere
    Private blockName As String

    Private Enum Operation
        DefiningVariable
        DefiningBlock
        DefiningConnection
        Selection
        MultipleSelection
        Selected
    End Enum
#Region " Codice generato da ProgettAction Windows Form "

    Public Sub New(ByRef RefPou As pou, ByRef RefBody As body, ByVal Editing As Boolean)

        MyBase.New()
        m_Pou = RefPou
        m_Body = RefBody '(in una macroazione il body non è quello della Pou)
        m_Fbd = m_Body.ReadFBD
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
        m_Fbd.SetGraphToDraw(m_GraphToDraw)
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
    Friend WithEvents NewTransButton As System.Windows.Forms.ToolBarButton
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormFbd))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.toolBar1 = New System.Windows.Forms.ToolBar
        Me.NewStepButton = New System.Windows.Forms.ToolBarButton
        Me.NewMacroStepButton = New System.Windows.Forms.ToolBarButton
        Me.NewTransButton = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton3 = New System.Windows.Forms.ToolBarButton
        Me.DelButton = New System.Windows.Forms.ToolBarButton
        Me.DisconnectButton = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton1 = New System.Windows.Forms.ToolBarButton
        Me.ShowCursor = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton4 = New System.Windows.Forms.ToolBarButton
        Me.IncomingButton = New System.Windows.Forms.ToolBarButton
        Me.SwapParamsButton = New System.Windows.Forms.ToolBarButton
        Me.OutgoingButton = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton2 = New System.Windows.Forms.ToolBarButton
        Me.VariablesMenu = New System.Windows.Forms.ToolBarButton
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.MenuItem4 = New System.Windows.Forms.MenuItem
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.MenuItem6 = New System.Windows.Forms.MenuItem
        Me.mnuSelOutgoingConns = New System.Windows.Forms.MenuItem
        Me.mnuSelIncomingConns = New System.Windows.Forms.MenuItem
        Me.MenuItem7 = New System.Windows.Forms.MenuItem
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
        Me.toolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.NewStepButton, Me.NewMacroStepButton, Me.NewTransButton, Me.ToolBarButton3, Me.DelButton, Me.DisconnectButton, Me.ToolBarButton1, Me.ShowCursor, Me.ToolBarButton4, Me.IncomingButton, Me.SwapParamsButton, Me.OutgoingButton, Me.ToolBarButton2, Me.VariablesMenu})
        Me.toolBar1.ButtonSize = New System.Drawing.Size(24, 24)
        Me.toolBar1.DropDownArrows = True
        Me.toolBar1.ImageList = Me.ImageList1
        Me.toolBar1.Location = New System.Drawing.Point(0, 0)
        Me.toolBar1.Name = "toolBar1"
        Me.toolBar1.ShowToolTips = True
        Me.toolBar1.Size = New System.Drawing.Size(546, 36)
        Me.toolBar1.TabIndex = 5
        '
        'NewStepButton
        '
        Me.NewStepButton.ImageIndex = 16
        Me.NewStepButton.Name = "NewStepButton"
        Me.NewStepButton.ToolTipText = "Variable"
        '
        'NewMacroStepButton
        '
        Me.NewMacroStepButton.ImageIndex = 17
        Me.NewMacroStepButton.Name = "NewMacroStepButton"
        Me.NewMacroStepButton.ToolTipText = "Block"
        '
        'NewTransButton
        '
        Me.NewTransButton.ImageIndex = 18
        Me.NewTransButton.Name = "NewTransButton"
        Me.NewTransButton.ToolTipText = "Connection"
        '
        'ToolBarButton3
        '
        Me.ToolBarButton3.Name = "ToolBarButton3"
        Me.ToolBarButton3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'DelButton
        '
        Me.DelButton.ImageIndex = 8
        Me.DelButton.Name = "DelButton"
        Me.DelButton.ToolTipText = "Cancel"
        '
        'DisconnectButton
        '
        Me.DisconnectButton.ImageIndex = 21
        Me.DisconnectButton.Name = "DisconnectButton"
        Me.DisconnectButton.ToolTipText = "Disconnect"
        '
        'ToolBarButton1
        '
        Me.ToolBarButton1.Name = "ToolBarButton1"
        Me.ToolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'ShowCursor
        '
        Me.ShowCursor.ImageIndex = 13
        Me.ShowCursor.Name = "ShowCursor"
        Me.ShowCursor.ToolTipText = "Cursor"
        '
        'ToolBarButton4
        '
        Me.ToolBarButton4.Name = "ToolBarButton4"
        Me.ToolBarButton4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'IncomingButton
        '
        Me.IncomingButton.ImageIndex = 20
        Me.IncomingButton.Name = "IncomingButton"
        Me.IncomingButton.ToolTipText = "Select incoming connections"
        '
        'SwapParamsButton
        '
        Me.SwapParamsButton.ImageIndex = 22
        Me.SwapParamsButton.Name = "SwapParamsButton"
        Me.SwapParamsButton.ToolTipText = "Reorder parameters"
        '
        'OutgoingButton
        '
        Me.OutgoingButton.ImageIndex = 19
        Me.OutgoingButton.Name = "OutgoingButton"
        Me.OutgoingButton.ToolTipText = "Select outgoing connections"
        '
        'ToolBarButton2
        '
        Me.ToolBarButton2.Name = "ToolBarButton2"
        Me.ToolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'VariablesMenu
        '
        Me.VariablesMenu.ImageIndex = 15
        Me.VariablesMenu.Name = "VariablesMenu"
        Me.VariablesMenu.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton
        Me.VariablesMenu.ToolTipText = "Variables"
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
        Me.ImageList1.Images.SetKeyName(18, "")
        Me.ImageList1.Images.SetKeyName(19, "ArrowRight.ico")
        Me.ImageList1.Images.SetKeyName(20, "ArrowLeft.ico")
        Me.ImageList1.Images.SetKeyName(21, "FBDDisconnectObject.ico")
        Me.ImageList1.Images.SetKeyName(22, "SwapParams.ico")
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem1, Me.MenuItem11})
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 0
        Me.MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem2, Me.MenuItem3, Me.MenuItem4, Me.MenuItem5, Me.MenuItem6, Me.MenuItem7})
        Me.MenuItem1.MergeOrder = 4
        Me.MenuItem1.Text = "&Fbd"
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 0
        Me.MenuItem2.Text = "Add &variable"
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 1
        Me.MenuItem3.Text = "Add &block"
        '
        'MenuItem4
        '
        Me.MenuItem4.Index = 2
        Me.MenuItem4.Text = "Add &connection"
        '
        'MenuItem5
        '
        Me.MenuItem5.Index = 3
        Me.MenuItem5.Text = "&Delete selected elements"
        '
        'MenuItem6
        '
        Me.MenuItem6.Index = 4
        Me.MenuItem6.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuSelOutgoingConns, Me.mnuSelIncomingConns})
        Me.MenuItem6.Text = "Select all connections"
        '
        'mnuSelOutgoingConns
        '
        Me.mnuSelOutgoingConns.Index = 0
        Me.mnuSelOutgoingConns.Text = "outgoing from current object"
        '
        'mnuSelIncomingConns
        '
        Me.mnuSelIncomingConns.Index = 1
        Me.mnuSelIncomingConns.Text = "incoming to current object"
        '
        'MenuItem7
        '
        Me.MenuItem7.Index = 5
        Me.MenuItem7.Text = "Reorder block parameters"
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
        Me.Panel2.Size = New System.Drawing.Size(546, 122)
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
        Me.Panel3.Location = New System.Drawing.Point(0, 158)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(546, 18)
        Me.Panel3.TabIndex = 0
        '
        'ExecuteBar1
        '
        Me.ExecuteBar1.Count = 0
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
        Me.Button1.Visible = False
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
        Me.stsComments.Location = New System.Drawing.Point(0, 176)
        Me.stsComments.Name = "stsComments"
        Me.stsComments.Size = New System.Drawing.Size(546, 22)
        Me.stsComments.TabIndex = 10
        '
        'lblComment
        '
        Me.lblComment.BackColor = System.Drawing.SystemColors.Info
        Me.lblComment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.lblComment.ForeColor = System.Drawing.SystemColors.InfoText
        Me.lblComment.Name = "lblComment"
        Me.lblComment.Size = New System.Drawing.Size(502, 17)
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
        'FormFbd
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(546, 198)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.toolBar1)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.stsComments)
        Me.ForeColor = System.Drawing.Color.Red
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.MainMenu1
        Me.MinimumSize = New System.Drawing.Size(320, 110)
        Me.Name = "FormFbd"
        Me.ShowInTaskbar = False
        Me.Text = "FormFBD"
        Me.Panel2.ResumeLayout(False)
        CType(Me.StatusBarPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.stsComments.ResumeLayout(False)
        Me.stsComments.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public Sub AddVariable()
        ResetCurrentOperation()
        'Cerca il numero della prima fase libera
        'e passa il valore alla finestra di dialogo
        Dim varsSelector As New FBDVariableBlockDialog(m_Body.ResGlobalVariables, m_Body.PouInterface, FBDVariableBlockDialogType.fvbdtFBD)
        Dim ResultDialog As System.Windows.Forms.DialogResult = varsSelector.ShowDialog()
        If ResultDialog = Windows.Forms.DialogResult.OK Then
            boundVariable = varsSelector.m_variable
            boundVarType = varsSelector.m_VariableType
            boundExecID = varsSelector.ExecutionOrderID
            ' Eseguiamo prima il controllo O(1) e poi quello O(n), evitandolo direttamente
            ' per tutte le variabili di ingresso
            If boundVarType = GraphicalVariableType.Output AndAlso m_Fbd.GraphicalVariablesList.IsAlreadyBoundToOutVariable(boundVariable) Then
                MsgBox( _
"A variable can only be used once as output." + vbCrLf + _
"Delete the existing output for this variable, or use an OR block to connect the two networks to the single output", MsgBoxStyle.Critical, _
                    UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
                Exit Sub
            End If
            CurrentOperation = Operation.DefiningVariable
        End If
        varsSelector.Dispose()
    End Sub
    Public Sub AddBlock()
        ResetCurrentOperation()
        'Cerca il numero della prima macrofase libera
        'e passa il valore alla finestra di dialogo
        Dim blockSelector As New FBDBlockChooser(m_Fbd.PousList)
        Dim ResultDialog As System.Windows.Forms.DialogResult = blockSelector.ShowDialog()
        If ResultDialog = Windows.Forms.DialogResult.OK Then
            blockName = blockSelector.BlockType
            CurrentOperation = Operation.DefiningBlock
        End If
        blockSelector.Dispose()
    End Sub
    Public Sub AddConnection()
        ResetCurrentOperation()
        CurrentOperation = Operation.DefiningConnection
        DeSelectAll()
        DrawVisibleArea()
    End Sub
    Public Sub RemoveSelectedElements()
        If m_Fbd.CountSelected = 0 Then
            MsgBox("Nothing selected to delete", MsgBoxStyle.Information, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
            Exit Sub
        End If
        Dim confirmed As Boolean = My.Computer.Keyboard.ShiftKeyDown OrElse _
            (MsgBox( _
"Do you really want to delete the selected elements?" + vbCrLf + _
"Removing connected objects may change the diagram's semantics", MsgBoxStyle.OkCancel Or _
                MsgBoxStyle.Question, UniSimVersion.VersionInfo.PrintableDescriptionForTool("")) = MsgBoxResult.Ok)
        If Not (confirmed) Then Exit Sub
        ' scolleghiamo prima blocchi o variabili
        If m_Fbd.GraphicalBlocksList.CountSelected > 0 OrElse m_Fbd.GraphicalVariablesList.CountSelected > 0 Then
            For Each GB As GraphicalFBDBlock In m_Fbd.GraphicalBlocksList
                If GB.Selected AndAlso m_Fbd.GraphicalConnectionsList.CountConnectionsFor(GB) > 0 Then
                    DisconnectObject(GB)
                    GB.SelectObject()
                End If
            Next
            For Each GV As GraphicalVariable In m_Fbd.GraphicalVariablesList
                If GV.Selected AndAlso m_Fbd.GraphicalConnectionsList.CountConnectionsFor(GV) > 0 Then
                    DisconnectObject(GV)
                    GV.SelectObject()
                End If
            Next
        End If
        m_Fbd.RemoveSelectedElements()
        CancelVisibleArea()
        DrawVisibleArea()
        CurrentOperation = Operation.Selection
    End Sub
    Public Sub WriteTitlePanel() Implements IEditorForm.WriteTitlePanel
        Me.Text = m_Fbd.Name
    End Sub
    Public Sub StartMonitor() Implements IEditorForm.StartMonitor
        m_Fbd.StartStateMonitor()
    End Sub
    Public Sub StopMonitor() Implements IEditorForm.StopMonitor
        m_Fbd.StopStateMonitor()
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
    Private Function StoreVariable(ByVal P As Drawing.Point) As Boolean
        m_Fbd.AddAndDrawGraphicalVariable(boundVariable, boundVarType, P, _
            New Drawing.Size(44, 44), boundExecID)
        DrawVisibleArea()
        Return True
    End Function
    Private Function StoreBlock(ByVal P As Drawing.Point) As Boolean
        m_Fbd.AddAndDrawGraphicalBlock(blockName, P, New Drawing.Size(44, 60))
        DrawVisibleArea()
        Return True
    End Function
    Private Sub ConfirmAddConnection()
        If ConnectionElementsOK() Then
            StoreConnection()
            If Not (Preferences.GetBoolean("FBDKeepConnectState", True)) Then _
                CurrentOperation = Operation.Selection
            CancelVisibleArea()
            DeSelectAll()
            DrawVisibleArea()
        Else
            MsgBox("You must select one source and one destination to connect and at least one must be a block" & vbCrLf & "The connection's semantic value shall only be evaluated at runtime", MsgBoxStyle.Information, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
        End If
    End Sub
    Private Function ConnectionElementsOK() As Boolean
        ' Se ci sono 2 oggetti selezionati...
        If (m_Fbd.FindObjectWithInRectangleSelected IsNot Nothing AndAlso _
            m_Fbd.FindObjectWithOutRectangleSelected IsNot Nothing) Then
            ' ...almeno uno deve essere un blocco
            Return Not (TypeOf (m_Fbd.FindObjectWithInRectangleSelected) Is GraphicalVariable AndAlso _
                TypeOf (m_Fbd.FindObjectWithOutRectangleSelected) Is GraphicalVariable)
        Else
            Return False
        End If
    End Function
    Private Function StoreConnection() As Boolean
        m_Fbd.AddAndDrawGraphicalConnection(m_Fbd.FindObjectWithOutRectangleSelected.Number, _
            m_Fbd.FindObjectWithInRectangleSelected.Number, My.Computer.Keyboard.ShiftKeyDown)
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
        m_Fbd.FindAndSelectElementsArea(Rect)
    End Sub
    Private Sub DrawArea(ByVal Rect As Drawing.Rectangle)
        Rect.X = Rect.X - 1
        Rect.Y = Rect.Y - 1
        Rect.Width = Rect.Width + 2
        Rect.Height = Rect.Height + 2
        If CurrentOperation = Operation.DefiningConnection Then
            m_Fbd.DrawElementsArea(Rect, True)
        Else
            m_Fbd.DrawElementsArea(Rect, False)
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
        m_Fbd.CancelSelection()
    End Sub
    Private Sub DeSelectAll()
        m_Fbd.DeSelectAll()
    End Sub
    Private Sub MoveSelection(ByVal dx As Integer, ByVal dy As Integer)
        CancelSelection()
        m_Fbd.MoveSelection(dx, dy)
        DrawVisibleArea()
    End Sub

    Private Sub NameChanged(ByVal Value As String) Handles m_Body.NameChanged 'Intercetta l'evento di cambiamento del nome
        WriteTitlePanel()
    End Sub
    Private Sub DisposingBody() Handles m_Body.Disposing    'Intercetta l'evento disposing del body
        Me.Dispose()
    End Sub
    Private Sub FormFbd_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        'Entra cou un monitor in m_GraphToDraw per aspettare che l'FBD finisca il disegno dello stato
        ' (codice preso da FormSfc)
        Monitor.Enter(Me.ReadBody.ReadFBD)
        Monitor.Enter(m_GraphToDraw)
        m_Fbd.StopStateMonitor()
        m_Fbd.SetGraphToDraw(Nothing)
        Monitor.Exit(m_GraphToDraw)
        Monitor.Exit(Me.ReadBody.ReadFBD)
        Me.Dispose()
    End Sub
    ' Impostiamo la dimensione del pannello ad almeno 1200*1200 pixels, ma
    ' se ci sono oggetti più in basso o più a destra di questo limite, aumentiamo
    ' la dimensione del pannello in modo da consentire di mostrare tutto il diagramma
    Private Sub InitialPanelSizing()
        Dim rightMost As IGraphicalObject = m_Fbd.GetRightmostObject
        Dim lowerMost As IGraphicalObject = m_Fbd.GetLowermostObject
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
                        Case Is = Operation.DefiningVariable
                            Dim P As New Drawing.Point(Snap(e.X), Snap(e.Y))
                            'Chiama AddAndDrawStep
                            If StoreVariable(P) Then  'quando ha fatto setta a true
                                CurrentOperation = Operation.Selection
                            End If
                        Case Is = Operation.DefiningBlock
                            Dim P As New Drawing.Point(Snap(e.X), Snap(e.Y))
                            'Chiama AddAndDrawMacroStep
                            If StoreBlock(P) Then
                                CurrentOperation = Operation.Selection
                            End If


                        Case Is = Operation.Selection
                            Dim Found As Boolean
                            If MultipleSelection Then
                                Found = m_Fbd.FindAndSelectElement(e.X, e.Y)
                            Else
                                Found = m_Fbd.FindElement(e.X, e.Y)
                                If Found Then
                                    'Ha trovato una Step o una Transition o un Action
                                    Dim GiaSelected As Boolean = m_Fbd.ReadIfElementIsSelected(e.X, e.Y)
                                    If Not GiaSelected Then
                                        DeSelectAll()
                                        m_Fbd.FindAndSelectElement(e.X, e.Y)
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

                        Case Is = Operation.DefiningConnection
                            m_Fbd.FindAndSelectSmallRectangle(e.X, e.Y)
                            DrawVisibleArea()
                    End Select

                Case Windows.Forms.MouseButtons.Middle, Windows.Forms.MouseButtons.Right
                    'Tasto desto o centrale del mouse
                    Select Case CurrentOperation
                        Case Operation.DefiningConnection
                            ConfirmAddConnection()
                    End Select
            End Select
        Else
            'Non è in modo di editing quindi seleziona solo la macrofasi per aprirle
            m_Fbd.FindAndSelectElement(e.X, e.Y)
        End If
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        AddConnection()
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
                        If Not m_Fbd.CheckForSelectionOutside(R, FuoriX, FuoriY) Then
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
                Case Operation.DefiningVariable
                    Panel1.Cursor = CursorStep
                Case Operation.DefiningBlock
                    Panel1.Cursor = CursorMacroStep
                Case Operation.DefiningConnection
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
    Private Sub FormFbd_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
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
                            m_Fbd.SetGraphToDraw(m_GraphToDraw)
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
    Private Sub FormFbd_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Select Case CurrentOperation
            Case Operation.DefiningConnection
                If e.KeyCode = 13 Then
                    ConfirmAddConnection()
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
    Private Sub FormFbd_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
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
            If CurrentOperation = Operation.Selected And m_Fbd.CountSelected = 1 And Not MultipleSelection Then
                Dim Obj As Object = m_Fbd.ReadObjectSelected
                Select Case Obj.GetType.Name
                    Case "GraphicalVariable"
                        Dim graphVar As GraphicalVariable = CType(Obj, GraphicalVariable)
                        If My.Computer.Keyboard.CtrlKeyDown AndAlso graphVar.BoundVariable.dataType = "BOOL" _
                            AndAlso graphVar.VariableType = GraphicalVariableType.Input Then
                            graphVar.BoundVariable.SetValue(Not graphVar.BoundVariable.ReadValue)
                            Exit Select
                        End If
                        Dim varsSelector As New FBDVariableBlockDialog(m_Body.ResGlobalVariables, _
                            m_Body.PouInterface, graphVar, FBDVariableBlockDialogType.fvbdtFBD)
                        If varsSelector.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            graphVar.BoundVariable = varsSelector.m_variable
                            graphVar.ExecutionOrderID = varsSelector.ExecutionOrderID
                            If (m_Fbd.GraphicalConnectionsList.FindAllConnectionsEndingWith(graphVar).Count > 0 _
                                OrElse m_Fbd.GraphicalConnectionsList.FindAllConnectionsStartingWith(graphVar).Count > 0) _
                                    AndAlso graphVar.VariableType <> varsSelector.m_VariableType Then
                                MsgBox("This variable is already connected to some other diagram entity and its type cannot be changed. Disconnect it before retrying", MsgBoxStyle.Critical, _
                                    UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
                            Else
                                graphVar.VariableType = varsSelector.m_VariableType
                            End If
                        End If
                    Case "GraphicalFBDBlock"
                        Dim block As GraphicalFBDBlock = CType(Obj, GraphicalFBDBlock)
                        Dim blockDialog As New FBDBlockChooser(m_Fbd.PousList, block, True)
                        If blockDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            block.BoundBlockType = blockDialog.BlockType
                            block.Name = blockDialog.BlockName
                        End If
                    Case "GraphicalFBDConnection"
                        Dim graphConn As GraphicalFBDConnection = CType(Obj, GraphicalFBDConnection)
                        Dim strMsg As String = "This connection is " & _
                            IIf(Of String)(graphConn.Negated, "negated. ", "affirmed. ") & _
                            "Do you want to invert its state?"
                        If My.Computer.Keyboard.ShiftKeyDown OrElse (MessageBox.Show(strMsg, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""), _
                         MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes) Then _
                            graphConn.Negated = Not (graphConn.Negated)
                    Case Else
                End Select
                CurrentOperation = Operation.Selection
                CancelVisibleArea()
                DrawVisibleArea()
            End If
        End If
    End Sub
    Private Sub FormFbd_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        'Entra cou un monitor in m_GraphToDraw per aspettare che l'Fbd finisca il disegno dello stato
        Monitor.Enter(Me.ReadBody.ReadFBD)
        Monitor.Enter(m_GraphToDraw)
        m_Fbd.StopStateMonitor()
        Monitor.Exit(m_GraphToDraw)
        Monitor.Exit(Me.ReadBody.ReadFBD)
        Me.Dispose()
    End Sub
    Private Sub toolBar1_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles toolBar1.ButtonClick
        If e.Button Is NewStepButton Then
            AddVariable()
        ElseIf e.Button Is NewMacroStepButton Then
            AddBlock()
        ElseIf e.Button Is DelButton Then
            RemoveSelectedElements()
        ElseIf e.Button Is NewTransButton Then
            AddConnection()
        ElseIf e.Button Is ShowCursor Then
            ResetCurrentOperation()
        ElseIf e.Button Is IncomingButton Then
            SelectIncomingConnections()
        ElseIf e.Button Is OutgoingButton Then
            SelectOutgoingConnections()
        ElseIf e.Button Is DisconnectButton Then
            DisconnectObject()
        ElseIf e.Button Is SwapParamsButton Then
            ReorderParameters()
        End If
    End Sub
    Private Sub StartFbdScanning() Handles m_Fbd.StartScan
        Try
            ExecuteBar1.NextValue()
            DrawVisibleArea()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub MenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem2.Click
        AddVariable()
    End Sub
    Private Sub MenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem3.Click
        AddBlock()
    End Sub
    Private Sub MenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem4.Click
        AddConnection()
    End Sub
    Private Sub MenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem5.Click
        RemoveSelectedElements()
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
                        m_Fbd.SetGraphToDraw(m_GraphToDraw)
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
        If m_Fbd.CountSelected() > 1 OrElse Me.MultipleSelection Then Return Nothing
        If m_Fbd.CountSelected() = 1 Then
            Dim Obj As Object = m_Fbd.ReadObjectSelected
            If TypeOf (Obj) Is IDocumentable Then
                Return CType(Obj, IDocumentable)
            Else
                Return Nothing
            End If
        End If
        If m_Fbd.CountSelected() = 0 Then Return m_Body
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
        ' Le connessioni non sono documentabili in quanto non vengono davvero salvate, ma ricreate
        ' a runtime e quindi non avremmo dove scrivere le informazioni
        If TypeOf (iComm) Is GraphicalFBDConnection Then Exit Sub
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
            myMenu.MenuItems.Add(m_Fbd.PouInterface.localVars.GetMenu())
            If m_Pou.PouType = EnumPouType.Function Then _
                myMenu.MenuItems.Add(m_Fbd.PouInterface.inputVars.GetMenu())
            If Not m_Fbd.ResGlobalVariables Is Nothing Then
                For Each VL As VariablesList In m_Fbd.ResGlobalVariables
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
        m_Fbd.PrintMe(imgGraphics, _
            imgSize)
        img.Save(destFile, Imaging.ImageFormat.Jpeg)
    End Sub

    Private Sub MenuItemExportJPEG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemExportJPEG.Click
        If sdgJPEGFile.ShowDialog() = Windows.Forms.DialogResult.OK Then _
            ExportAsJPEG(sdgJPEGFile.FileName)
    End Sub

    Private Sub DisconnectObject(ByVal Obj As IFBDConnectable)
        Obj.DeselectObject()
        For Each conn As GraphicalFBDConnection In m_Fbd.GraphicalConnectionsList.FindAllConnectionsStartingWith(Obj)
            m_Fbd.GraphicalConnectionsList.Remove(conn)
        Next
        For Each conn As GraphicalFBDConnection In m_Fbd.GraphicalConnectionsList.FindAllConnectionsEndingWith(Obj)
            m_Fbd.GraphicalConnectionsList.Remove(conn)
        Next
    End Sub

    Private Sub DisconnectObject()
        If Not EditingMode Then Exit Sub
        If CurrentOperation <> Operation.Selection Then Exit Sub
        If m_Fbd.CountSelected() > 1 OrElse Me.MultipleSelection Then Exit Sub
        If m_Fbd.CountSelected() = 1 Then
            Dim Obj As Object = m_Fbd.ReadObjectSelected
            If TypeOf (Obj) Is IFBDConnectable Then _
                DisconnectObject(Obj)
        End If
        DrawVisibleArea()
    End Sub

    Private Sub SelectOutgoingConnections()
        If Not EditingMode Then Exit Sub
        If CurrentOperation <> Operation.Selection Then Exit Sub
        If m_Fbd.CountSelected() > 1 OrElse Me.MultipleSelection Then Exit Sub
        If m_Fbd.CountSelected() = 1 Then
            Dim Obj As Object = m_Fbd.ReadObjectSelected
            If TypeOf (Obj) Is GraphicalVariable Or TypeOf (Obj) Is GraphicalFBDBlock Then
                Obj.DeselectObject()
                Dim connections As GraphicalFBDConnectionsList = m_Fbd.GraphicalConnectionsList.FindAllConnectionsStartingWith(Obj)
                For Each connection As GraphicalFBDConnection In connections
                    connection.DestinationObject.SelectObject()
                Next
            End If
        End If
        Me.DrawVisibleArea()
    End Sub
    Private Sub SelectIncomingConnections()
        If Not EditingMode Then Exit Sub
        If CurrentOperation <> Operation.Selection Then Exit Sub
        If m_Fbd.CountSelected() > 1 OrElse Me.MultipleSelection Then Exit Sub
        If m_Fbd.CountSelected() = 1 Then
            Dim Obj As Object = m_Fbd.ReadObjectSelected
            If TypeOf (Obj) Is GraphicalVariable Or TypeOf (Obj) Is GraphicalFBDBlock Then
                Obj.DeselectObject()
                Dim connections As GraphicalFBDConnectionsList = m_Fbd.GraphicalConnectionsList.FindAllConnectionsEndingWith(Obj)
                For Each connection As GraphicalFBDConnection In connections
                    connection.SourceObject.SelectObject()
                Next
            End If
        End If
        Me.DrawVisibleArea()
    End Sub
    Private Sub ReorderParameters()
        If Not EditingMode Then Exit Sub
        If CurrentOperation <> Operation.Selection Then Exit Sub
        If m_Fbd.CountSelected() > 1 OrElse Me.MultipleSelection Then Exit Sub
        If m_Fbd.CountSelected() = 1 Then
            Dim Obj As Object = m_Fbd.ReadObjectSelected
            If Not (TypeOf (Obj) Is GraphicalFBDBlock) Then Exit Sub
            Dim paramDialog As New FBDParametersReorderDialog(m_Fbd, Obj)
            paramDialog.ShowDialog()
        End If
    End Sub

    Private Sub mnuSelOutgoingConns_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSelOutgoingConns.Click
        SelectOutgoingConnections()
    End Sub

    Private Sub mnuSelIncomingConns_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSelIncomingConns.Click
        SelectIncomingConnections()
    End Sub

    Private Sub MenuItem7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem7.Click
        ReorderParameters()
    End Sub
End Class

