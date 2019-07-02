Imports System.Drawing
Imports System.Math
Imports System.Xml
Imports System.Collections.Generic
Imports System.Threading
Imports System.Windows.Forms
Public Class FormLadder
    Inherits System.Windows.Forms.Form
    Implements IEditorForm

    Private Stato As Opzione = Opzione.Selection
    Private m_Pou As pou
    Private ShowDetails As Boolean
    Private WithEvents m_Body As body
    Private m_monitoring As Boolean
    Private EditingMode As Boolean
    Private NextCondition As BooleanExpression
    Private Foglio As Drawing.Rectangle
    Private InitialPoint As Drawing.Point
    Private Br, BrSfondo As Drawing.Brush
    Private MultipleSelection As Boolean
    Private m_GraphToDraw As Drawing.Graphics
    Private ColSfondo As Drawing.Color = Drawing.Color.White
    Private WithEvents m_Ladder As Ladder ' = New Ladder   'non va dichiarato qui pero...
    Private NextElementName As String
    Private CoeffSnap As Integer
    Private NextElementDocumentation As String
    Private TreeOpen As Boolean
    Private PreviousFinelPoint As Drawing.Point
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Private CursorStep As New Cursor(IO.Path.Combine(CursorsPath, "CursorStep.Cur"))
    Private CursorMacroStep As New Cursor(IO.Path.Combine(CursorsPath, "CursorMacroStep.Cur"))
    Private CursorTransition As New System.Windows.Forms.Cursor(IO.Path.Combine(CursorsPath, "CursorTransition.Cur"))
    Private CursorMove As New Cursor(IO.Path.Combine(CursorsPath, "CursorMove.cur"))
    Private CursorContact As New Cursor(IO.Path.Combine(CursorsPath, "CursorContact.Cur"))
    Private Counter As Integer = 0
    Public m_qualifier As String
    Public m_name As String
    Public m_variable As BaseVariable
    Public m_indicator As New BooleanVariable
    Friend WithEvents VariablesMenu As System.Windows.Forms.ToolBarButton
    Friend WithEvents MenuItem16 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem17 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem18 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ExecuteBar1 As UnisimClassLibrary.ExecuteBar
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents stsComments As System.Windows.Forms.StatusStrip
    Friend WithEvents lblComment As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents btnEdit As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
    Public m_time As TimeSpan
    Private m_ScanOrderEntries As Hydra(Of Boolean, String, String)
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemExportAsJPG As System.Windows.Forms.MenuItem
    Friend WithEvents sdgJPEGFile As System.Windows.Forms.SaveFileDialog
    Friend WithEvents MenuItem7 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem8 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem9 As System.Windows.Forms.MenuItem
    Private m_MonitorStatus As Hydra(Of Boolean, String, Object)
    Friend WithEvents ToolBarButton1 As System.Windows.Forms.ToolBarButton
    Friend WithEvents FBDVariable As System.Windows.Forms.ToolBarButton
    Friend WithEvents FBDBlock As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton2 As System.Windows.Forms.ToolBarButton
    Friend WithEvents MenuItem10 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem11 As System.Windows.Forms.MenuItem

    Private m_bType As String
    Private m_bVar As BaseVariable
    Friend WithEvents SwapParamsButton As System.Windows.Forms.ToolBarButton
    Private m_bVarType As GraphicalVariableType

    Private Enum Opzione
        Contatto
        Coil
        Connection
        Selection
        Selected
        MultipleSelection
        Rail
        Block
        Variable
    End Enum

#Region " Codice generato da Progettazione Windows Form "

    Public Sub New(ByRef RefPou As pou, ByRef RefBody As body, ByVal Editing As Boolean) 'vediamo dopo i parametri di scambio
        MyBase.New()
        m_Pou = RefPou
        m_Body = RefBody '(in una macroazione il body non è quello della Pou)
        m_Ladder = m_Body.ReadLadder
        EditingMode = Editing
        Stato = Opzione.Selection
        Foglio.Width = 800
        Foglio.Height = 1200
        CoeffSnap = 4
        InitializeComponent()
        m_GraphToDraw = Panel3.CreateGraphics()
        ShowDetails = True
        BrSfondo = New Drawing.SolidBrush(ColSfondo)
        InitialPoint = New Drawing.Point(0, 0)
        m_Ladder.SetGraphToDraw(m_GraphToDraw)
        'qui manca qualcosa
        If EditingMode Then
            ' Label2.Text = "Editing mode"
        Else
            '  Label2.Visible = False
            MenuItem1.Visible = False
            ToolBar1.Visible = False
        End If


        'Chiamata richiesta da Progettazione Windows Form.
        'InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()

        m_ScanOrderEntries = New Hydra(Of Boolean, String, String)
        m_ScanOrderEntries.AddValueToAllEntries("menuitem", New KeyValuePair(Of Boolean, String)(False, "RtL mode"), _
            New KeyValuePair(Of Boolean, String)(True, "LtR mode"))
        MenuItem4.Text = m_ScanOrderEntries(Ladder.RtL)("menuitem")

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

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form.
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents ToolBar1 As System.Windows.Forms.ToolBar
    Friend WithEvents Contact As System.Windows.Forms.ToolBarButton
    Friend WithEvents Coil As System.Windows.Forms.ToolBarButton
    Friend WithEvents Delete As System.Windows.Forms.ToolBarButton
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Connection As System.Windows.Forms.ToolBarButton
    Friend WithEvents Selection As System.Windows.Forms.ToolBarButton
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents MenuItem6 As System.Windows.Forms.MenuItem
    Friend WithEvents Rail As System.Windows.Forms.ToolBarButton
    Friend WithEvents CancelRail As System.Windows.Forms.ToolBarButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormLadder))
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.MenuItem6 = New System.Windows.Forms.MenuItem
        Me.MenuItem16 = New System.Windows.Forms.MenuItem
        Me.MenuItem17 = New System.Windows.Forms.MenuItem
        Me.MenuItem18 = New System.Windows.Forms.MenuItem
        Me.MenuItem4 = New System.Windows.Forms.MenuItem
        Me.MenuItem7 = New System.Windows.Forms.MenuItem
        Me.MenuItem8 = New System.Windows.Forms.MenuItem
        Me.MenuItem10 = New System.Windows.Forms.MenuItem
        Me.MenuItem9 = New System.Windows.Forms.MenuItem
        Me.MenuItem11 = New System.Windows.Forms.MenuItem
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.MenuItemExportAsJPG = New System.Windows.Forms.MenuItem
        Me.ToolBar1 = New System.Windows.Forms.ToolBar
        Me.Rail = New System.Windows.Forms.ToolBarButton
        Me.CancelRail = New System.Windows.Forms.ToolBarButton
        Me.Contact = New System.Windows.Forms.ToolBarButton
        Me.Coil = New System.Windows.Forms.ToolBarButton
        Me.Connection = New System.Windows.Forms.ToolBarButton
        Me.Delete = New System.Windows.Forms.ToolBarButton
        Me.Selection = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton1 = New System.Windows.Forms.ToolBarButton
        Me.FBDVariable = New System.Windows.Forms.ToolBarButton
        Me.FBDBlock = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton2 = New System.Windows.Forms.ToolBarButton
        Me.VariablesMenu = New System.Windows.Forms.ToolBarButton
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.ExecuteBar1 = New UnisimClassLibrary.ExecuteBar
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.stsComments = New System.Windows.Forms.StatusStrip
        Me.lblComment = New System.Windows.Forms.ToolStripStatusLabel
        Me.btnEdit = New System.Windows.Forms.ToolStripDropDownButton
        Me.sdgJPEGFile = New System.Windows.Forms.SaveFileDialog
        Me.SwapParamsButton = New System.Windows.Forms.ToolBarButton
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.stsComments.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem1, Me.MenuItem5})
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 0
        Me.MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem3, Me.MenuItem2, Me.MenuItem6, Me.MenuItem16, Me.MenuItem10})
        Me.MenuItem1.MergeOrder = 4
        Me.MenuItem1.Text = "&Ladder"
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 0
        Me.MenuItem3.Text = "Add Rail"
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 1
        Me.MenuItem2.Text = "Add Contact"
        '
        'MenuItem6
        '
        Me.MenuItem6.Index = 2
        Me.MenuItem6.Text = "Add &Coil"
        '
        'MenuItem16
        '
        Me.MenuItem16.Index = 3
        Me.MenuItem16.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem17, Me.MenuItem18, Me.MenuItem4, Me.MenuItem7, Me.MenuItem8})
        Me.MenuItem16.Text = "Debug"
        '
        'MenuItem17
        '
        Me.MenuItem17.Index = 0
        Me.MenuItem17.Text = "Hit breakpoint"
        '
        'MenuItem18
        '
        Me.MenuItem18.Index = 1
        Me.MenuItem18.Text = "Dump program as Rungs"
        '
        'MenuItem4
        '
        Me.MenuItem4.Index = 2
        Me.MenuItem4.Text = "LtR Mode"
        Me.MenuItem4.Visible = False
        '
        'MenuItem7
        '
        Me.MenuItem7.Index = 3
        Me.MenuItem7.Text = "Create a virtual PLC"
        '
        'MenuItem8
        '
        Me.MenuItem8.Index = 4
        Me.MenuItem8.Text = "Dump selected Rung"
        '
        'MenuItem10
        '
        Me.MenuItem10.Index = 4
        Me.MenuItem10.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem9, Me.MenuItem11})
        Me.MenuItem10.Text = "Add &FBD elements"
        '
        'MenuItem9
        '
        Me.MenuItem9.Index = 0
        Me.MenuItem9.Text = "Add block"
        '
        'MenuItem11
        '
        Me.MenuItem11.Index = 1
        Me.MenuItem11.Text = "Add variable"
        '
        'MenuItem5
        '
        Me.MenuItem5.Index = 1
        Me.MenuItem5.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemExportAsJPG})
        Me.MenuItem5.MergeOrder = 4
        Me.MenuItem5.Text = "Worksheet"
        '
        'MenuItemExportAsJPG
        '
        Me.MenuItemExportAsJPG.Index = 0
        Me.MenuItemExportAsJPG.Text = "Save as JPEG..."
        '
        'ToolBar1
        '
        Me.ToolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.Rail, Me.CancelRail, Me.Contact, Me.Coil, Me.Connection, Me.Delete, Me.Selection, Me.ToolBarButton1, Me.FBDVariable, Me.SwapParamsButton, Me.FBDBlock, Me.ToolBarButton2, Me.VariablesMenu})
        Me.ToolBar1.ButtonSize = New System.Drawing.Size(24, 24)
        Me.ToolBar1.DropDownArrows = True
        Me.ToolBar1.ImageList = Me.ImageList1
        Me.ToolBar1.Location = New System.Drawing.Point(0, 0)
        Me.ToolBar1.Name = "ToolBar1"
        Me.ToolBar1.ShowToolTips = True
        Me.ToolBar1.Size = New System.Drawing.Size(820, 28)
        Me.ToolBar1.TabIndex = 5
        '
        'Rail
        '
        Me.Rail.ImageIndex = 18
        Me.Rail.Name = "Rail"
        Me.Rail.ToolTipText = "Add Power Rail"
        '
        'CancelRail
        '
        Me.CancelRail.ImageIndex = 19
        Me.CancelRail.Name = "CancelRail"
        Me.CancelRail.ToolTipText = "Del Power Rail"
        '
        'Contact
        '
        Me.Contact.ImageIndex = 5
        Me.Contact.Name = "Contact"
        Me.Contact.ToolTipText = "Normal Open Contact"
        '
        'Coil
        '
        Me.Coil.ImageIndex = 0
        Me.Coil.Name = "Coil"
        Me.Coil.ToolTipText = "Coil"
        '
        'Connection
        '
        Me.Connection.ImageIndex = 16
        Me.Connection.Name = "Connection"
        Me.Connection.ToolTipText = "Connection"
        '
        'Delete
        '
        Me.Delete.ImageIndex = 14
        Me.Delete.Name = "Delete"
        Me.Delete.ToolTipText = "Delete"
        '
        'Selection
        '
        Me.Selection.ImageIndex = 15
        Me.Selection.Name = "Selection"
        Me.Selection.ToolTipText = "Selection"
        '
        'ToolBarButton1
        '
        Me.ToolBarButton1.Name = "ToolBarButton1"
        Me.ToolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'FBDVariable
        '
        Me.FBDVariable.ImageIndex = 22
        Me.FBDVariable.Name = "FBDVariable"
        Me.FBDVariable.ToolTipText = "Add a variable"
        '
        'FBDBlock
        '
        Me.FBDBlock.ImageIndex = 23
        Me.FBDBlock.Name = "FBDBlock"
        Me.FBDBlock.ToolTipText = "Add a block"
        '
        'ToolBarButton2
        '
        Me.ToolBarButton2.Name = "ToolBarButton2"
        Me.ToolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'VariablesMenu
        '
        Me.VariablesMenu.ImageIndex = 21
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
        Me.ImageList1.Images.SetKeyName(18, "")
        Me.ImageList1.Images.SetKeyName(19, "")
        Me.ImageList1.Images.SetKeyName(20, "")
        Me.ImageList1.Images.SetKeyName(21, "Data_Dataset.ico")
        Me.ImageList1.Images.SetKeyName(22, "FBDVariable.ico")
        Me.ImageList1.Images.SetKeyName(23, "FBDBlock.ico")
        Me.ImageList1.Images.SetKeyName(24, "SwapParams.ico")
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.White
        Me.Panel3.ForeColor = System.Drawing.Color.Red
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(800, 1200)
        Me.Panel3.TabIndex = 1
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.ForeColor = System.Drawing.Color.Red
        Me.Panel1.Location = New System.Drawing.Point(0, 28)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(820, 268)
        Me.Panel1.TabIndex = 6
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.ExecuteBar1)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.Button1)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.ForeColor = System.Drawing.Color.Black
        Me.Panel2.Location = New System.Drawing.Point(0, 278)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(820, 18)
        Me.Panel2.TabIndex = 8
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
        Me.stsComments.Location = New System.Drawing.Point(0, 296)
        Me.stsComments.Name = "stsComments"
        Me.stsComments.Size = New System.Drawing.Size(820, 22)
        Me.stsComments.TabIndex = 12
        '
        'lblComment
        '
        Me.lblComment.BackColor = System.Drawing.SystemColors.Info
        Me.lblComment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.lblComment.ForeColor = System.Drawing.SystemColors.InfoText
        Me.lblComment.Name = "lblComment"
        Me.lblComment.Size = New System.Drawing.Size(776, 17)
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
        'SwapParamsButton
        '
        Me.SwapParamsButton.ImageIndex = 24
        Me.SwapParamsButton.Name = "SwapParamsButton"
        Me.SwapParamsButton.ToolTipText = "Reorder"
        '
        'FormLadder
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(820, 318)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ToolBar1)
        Me.Controls.Add(Me.stsComments)
        Me.ForeColor = System.Drawing.Color.Red
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.MainMenu1
        Me.MinimumSize = New System.Drawing.Size(320, 110)
        Me.Name = "FormLadder"
        Me.ShowInTaskbar = False
        Me.Text = "FormLadder"
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.stsComments.ResumeLayout(False)
        Me.stsComments.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region


    Public Function ReadBody() As body Implements IEditorForm.ReadBody
        ReadBody = m_Body
    End Function


    'Add Contact

    Public Sub AddContact()
        ResetCurrentOperation()
        Dim CoilDialog As New ContactDialogForm(m_Ladder.ResGlobalVariables, m_Pou.PouInterface)
        CoilDialog.m_ContactName = m_Ladder.FirstAvailableContactName()
        Dim ResultDialog As System.Windows.Forms.DialogResult = CoilDialog.ShowDialog()


        If ResultDialog = Windows.Forms.DialogResult.OK Then
            'Controlla se il nome del contatto è già presente
            If Not IsNothing(m_Ladder.FindContactByName(CoilDialog.m_ContactName)) Then
                MsgBox("Contact " & CoilDialog.m_ContactName & " already exists", MsgBoxStyle.Critical, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
            Else
                NextElementName = CoilDialog.m_ContactName
                Stato = Opzione.Contatto
                SetLocalVariable(CoilDialog.m_name, CoilDialog.m_qualifier, CoilDialog.m_variable, CoilDialog.m_indicator, CoilDialog.m_time)
            End If
        End If


    End Sub
    Public Sub AddBlock()
        ResetCurrentOperation()
        Dim blockSelector As New LDBlockChooser(m_Ladder.PousList)
        Dim ResultDialog As System.Windows.Forms.DialogResult = blockSelector.ShowDialog()
        If ResultDialog = Windows.Forms.DialogResult.OK Then
            m_bType = blockSelector.BlockType
            NextElementName = m_Ladder.FirstAvailableBlockName()
            Stato = Opzione.Block
        End If
        blockSelector.Dispose()
    End Sub
    Public Sub AddVariable()
        ResetCurrentOperation()
        'Cerca il numero della prima fase libera
        'e passa il valore alla finestra di dialogo
        Dim varsSelector As New FBDVariableBlockDialog(m_Body.ResGlobalVariables, m_Body.PouInterface, _
            FBDVariableBlockDialogType.fvbdtLD)
        Dim ResultDialog As System.Windows.Forms.DialogResult = varsSelector.ShowDialog()
        If ResultDialog = Windows.Forms.DialogResult.OK Then
            m_bVar = varsSelector.m_variable
            m_bVarType = varsSelector.m_VariableType
            Stato = Opzione.Variable
        End If
        varsSelector.Dispose()
    End Sub
    'AddCoil

    Public Sub AddCoil()

        ResetCurrentOperation()
        Dim CoilDialog As New CoilDialogForm(m_Ladder.ResGlobalVariables, m_Pou.PouInterface)
        CoilDialog.m_CoilName = m_Ladder.FirstAvailableCoilName()
        Dim ResultDialog As System.Windows.Forms.DialogResult = CoilDialog.ShowDialog()

        If ResultDialog = Windows.Forms.DialogResult.OK Then
            'Controlla se il nome del contatto è già presente
            If Not IsNothing(m_Ladder.FindContactByName(CoilDialog.m_CoilName)) Then
                MsgBox("Contact " & CoilDialog.m_CoilName & " already exists", MsgBoxStyle.Critical, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
            Else
                NextElementName = CoilDialog.m_CoilName
                Stato = Opzione.Coil
                SetLocalVariable(CoilDialog.m_name, CoilDialog.m_qualifier, CoilDialog.m_variable, CoilDialog.m_indicator, CoilDialog.m_time)
            End If
        End If

    End Sub


    'AddRail
    Public Sub AddRail()

        Dim P1 As New Drawing.Point(10, 48 + Counter)
        StoreLeftRail(P1, 20, "", "", m_variable, m_indicator, m_time)

        Dim P2 As New Drawing.Point(780, 48 + Counter)
        StoreRightRail(P2, 21, "", "", m_variable, m_indicator, m_time)

        Counter = Counter + 60
        'Stato = Opzione.Rail

        If Counter > Panel3.Height Then
            ResizePanel(New Size(Panel3.Width, Counter + 108))
        End If

    End Sub

    Private Sub ToolBar1_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ToolBar1.ButtonClick
        'contact
        If e.Button Is Contact Then
            AddContact()

            'coil
        ElseIf e.Button Is Coil Then
            AddCoil()

            'connection
        ElseIf e.Button Is Connection Then
            Stato = Opzione.Connection
            DrawVisibleArea()

            'rail
        ElseIf e.Button Is Rail Then
            AddRail()

            'cancel
        ElseIf e.Button Is Delete Then
            RemoveSelectedElements()

        ElseIf e.Button Is Selection Then
            ResetCurrentOperation()

        ElseIf e.Button Is CancelRail Then
            RemoveRail()
        ElseIf e.Button Is FBDBlock Then
            AddBlock()
        ElseIf e.Button Is FBDVariable Then
            AddVariable()
        ElseIf e.Button Is SwapParamsButton Then
            ReorderParameters()
        End If
    End Sub
    Private Sub MoveSelection(ByVal dx As Integer, ByVal dy As Integer)
        CancelSelection()
        m_Ladder.MoveSelection(dx, dy)
        DrawVisibleArea()
    End Sub
    Private Sub CancelSelection()
        m_Ladder.CancelSelection(True)

    End Sub

    Private Sub Panel3_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel3.Paint
        DrawArea(e.ClipRectangle)
    End Sub
    Private Function Snap(ByVal val As Integer) As Integer
        Snap = CInt(val / CoeffSnap) * CoeffSnap

    End Function

    Private Sub Panel3_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel3.MouseDown
        Me.Activate()
        If EditingMode Then
            Dim P As New Drawing.Point(Snap(e.X), Snap(e.Y))
            'Legge il tasto del mouse premuto
            Select Case e.Button
                Case Windows.Forms.MouseButtons.Left
                    Select Case Stato

                        Case Is = Opzione.Contatto
                            If StoreContact(P, 1, NextElementName, m_qualifier, m_variable, _
                                m_indicator, m_time) Then
                                Stato = Opzione.Selection
                            End If
                        Case Is = Opzione.Coil
                            If StoreContact(P, 2, NextElementName, m_qualifier, m_variable, _
                                m_indicator, m_time) Then
                                Stato = Opzione.Selection
                            End If

                        Case Is = Opzione.Connection
                            m_Ladder.FindAndSelectSmallRectangleStep(Snap(e.X), Snap(e.Y))
                            DrawVisibleArea()

                        Case Is = Opzione.Block
                            If StoreBlock(P, 3, NextElementName, m_bType, Nothing, _
                                Nothing, Nothing) Then
                                Stato = Opzione.Selection
                            End If
                        Case Is = Opzione.Variable
                            StoreVariable(P)
                            Stato = Opzione.Selection

                        Case Is = Opzione.Selection
                            Dim Found As Boolean
                            Found = m_Ladder.FindElement(e.X, e.Y)
                            If Found Then
                                'Ha trovato un Contact
                                Dim GiaSelected As Boolean = m_Ladder.ReadIfElementIsSelected(e.X, e.Y)
                                If Not GiaSelected Then
                                    DeSelectAll()
                                    m_Ladder.FindAndSelectElement(e.X, e.Y)
                                End If
                            End If
                            If Found Then
                                'Ha trovato l'elemento e prepara per la selezione
                                Stato = Opzione.Selected
                                ' DrawVisibleArea(): inutile, tanto già lo fa sotto in ogni caso
                            Else
                                'Non ha trovato elementi e prepara per la selezione multipla
                                Stato = Opzione.MultipleSelection
                                DeSelectAll()
                            End If
                            InitialPoint.X = e.X
                            InitialPoint.Y = e.Y
                            DrawVisibleArea()
                    End Select

                    'case MouseButtons.Right 

                Case Windows.Forms.MouseButtons.Middle, Windows.Forms.MouseButtons.Right
                    'Tasto desto o centrale del mouse
                    Select Case Stato
                        Case Opzione.Connection
                            ConfirmAddConnection()
                    End Select
            End Select
        Else
            'Non è in modo di editing quindi seleziona solo la macrofasi (?? in Ladder ??) per aprirle
            m_Ladder.FindAndSelectElement(e.X, e.Y)
        End If

    End Sub

    Private Sub Panel3_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel3.MouseMove
        If EditingMode Then
            Select Case Stato
                Case Opzione.Selected
                    Panel3.Cursor = CursorMove
                    'Effettua lo Movemento solo se dopo lo snap....
                    '....è maggiore di 0
                    Dim dx As Integer = Snap(e.X - InitialPoint.X)
                    Dim dy As Integer = Snap(e.Y - InitialPoint.Y)
                    If dx <> 0 Or dy <> 0 Then
                        Dim R As New Drawing.Rectangle(-dx, -dy, Foglio.Width - dx, Foglio.Height - dy)
                        Dim FuoriX, FuoriY As Boolean
                        'Simula l'area del foglio Moveta nei versi opposti
                        If Not m_Ladder.IsSelectionOutside(R, FuoriX, FuoriY) Then
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
                Case Opzione.Contatto, Opzione.Coil, Opzione.Block, Opzione.Variable
                    Panel3.Cursor = CursorContact
                Case Opzione.Connection
                    Panel3.Cursor = CursorTransition
                Case Opzione.MultipleSelection
                    CancelMultipleSelectionRectangle(CreaRectangle(InitialPoint.X, InitialPoint.Y, PreviousFinelPoint.X, PreviousFinelPoint.Y))
                    DrawArea(CreaRectangle(InitialPoint.X, InitialPoint.Y, PreviousFinelPoint.X, PreviousFinelPoint.Y))
                    DrawMultipleSelectionRectangle(CreaRectangle(InitialPoint.X, InitialPoint.Y, e.X, e.Y))
                    PreviousFinelPoint.X = e.X
                    PreviousFinelPoint.Y = e.Y
                Case Else
                    Panel3.Cursor = System.Windows.Forms.Cursors.Default
            End Select
        End If
    End Sub
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

    Private Sub CancelMultipleSelectionRectangle(ByVal R As Drawing.Rectangle)
        Dim Penna As New Drawing.Pen(ColSfondo)
        If Monitor.TryEnter(m_GraphToDraw, 2000) Then
            ' Così andrebbe fatto questo codice in tutto UniSim (ma è troppo lungo
            ' ed error-prone correggere tutto)
            Try
                m_GraphToDraw.DrawRectangle(Penna, R)
            Finally
                Monitor.Exit(m_GraphToDraw)
            End Try
        End If
    End Sub
    Private Sub Panel3_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel3.MouseUp
        If EditingMode Then
            Select Case Stato
                Case Opzione.MultipleSelection
                    Stato = Opzione.Selection
                    DeSelectAll()
                    ' evitiamo di usare la selezione multipla per semplici "click"
                    If (InitialPoint.X <> e.X) OrElse (InitialPoint.Y <> e.Y) Then
                        FindAndSelectElementsArea(CreaRectangle(InitialPoint.X, InitialPoint.Y, e.X, e.Y))
                        Dim R As Drawing.Rectangle = CreaRectangle(InitialPoint.X, InitialPoint.Y, PreviousFinelPoint.X, PreviousFinelPoint.Y)
                        CancelMultipleSelectionRectangle(R)
                        DrawArea(R)
                    End If
                Case Opzione.Selected
                    Stato = Opzione.Selection
                    'DrawVisibleArea()
            End Select
        End If
        ' come in FormSfc pulisce lo schermo...
        Me.Refresh()
        ' imposta il tooltip di descrizione degli oggetti
        Me.SetToolTip()
    End Sub

    Private Sub FormLadder_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        'Entra cou un monitor in m_GraphToDraw per aspettare che il Ladder finisca il disegno dello stato
        ' (codice preso da FormSfc)
        Monitor.Enter(Me.ReadBody.ReadLadder)
        Monitor.Enter(m_GraphToDraw)
        m_Ladder.StopStateMonitor()
        m_Ladder.SetGraphToDraw(Nothing)
        Monitor.Exit(m_GraphToDraw)
        Monitor.Exit(Me.ReadBody.ReadLadder)
        Me.Dispose()
    End Sub
    Private Sub FormSfc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Select Case Stato
            'Case Operation.DefiningTransition
            ' If e.KeyCode = 13 Then
            '     ConfirmAddTransition()
            ' End If
            Case Opzione.Selection
                If e.KeyCode = 46 Then
                    CancelSelection()
                ElseIf e.KeyCode = Keys.ControlKey Or e.KeyCode = Keys.ShiftKey Then
                    MultipleSelection = True
                End If
        End Select
    End Sub

    Private Sub FindAndSelectElementsArea(ByVal P As Drawing.Point)
        m_Ladder.FindAndSelectElements(P.X, P.Y)
    End Sub
    Private Sub FindAndSelectElementsArea(ByVal Rect As Drawing.Rectangle)
        m_Ladder.FindAndSelectElementsArea(Rect)
    End Sub

    Private Sub DeSelectAll()
        m_Ladder.DeSelectAll()
    End Sub
    'rail
    Private Function StoreLeftRail(ByVal P As Drawing.Point, ByVal Id As Integer, ByVal Name As String, ByVal Qualy As String, ByVal Var As BaseVariable, ByVal Ind As BaseVariable, ByVal Time As TimeSpan) As Boolean
        StoreLeftRail = m_Ladder.AddAndDrawLeftRail(m_Ladder.FirstAvailableElementNumber, NextElementName, NextElementDocumentation, P, Id, Name, Qualy, Ind, Var, Time)
    End Function
    Private Function StoreRightRail(ByVal P As Drawing.Point, ByVal Id As Integer, ByVal Name As String, ByVal Qualy As String, ByVal Var As BaseVariable, ByVal Ind As BaseVariable, ByVal Time As TimeSpan) As Boolean
        StoreRightRail = m_Ladder.AddAndDrawRightRail(m_Ladder.FirstAvailableElementNumber, NextElementName, NextElementDocumentation, P, Id, Name, Qualy, Ind, Var, Time)
    End Function

    ' StoreBlock
    Private Function StoreBlock(ByVal P As Drawing.Point, ByVal Id As Integer, ByVal Name As String, ByVal Qualy As String, ByVal Var As BaseVariable, ByVal Ind As BaseVariable, ByVal Time As TimeSpan) As Boolean
        StoreBlock = m_Ladder.AddAndDrawBlock(m_Ladder.FirstAvailableElementNumber, NextElementName, NextElementDocumentation, P, Id, Name, Qualy, Var, Ind, Time)
    End Function

    'StoreContact
    Private Function StoreContact(ByVal P As Drawing.Point, ByVal Id As Integer, ByVal Name As String, ByVal Qualy As String, ByVal Var As BaseVariable, ByVal Ind As BaseVariable, ByVal Time As TimeSpan) As Boolean
        StoreContact = m_Ladder.AddAndDrawContact(m_Ladder.FirstAvailableElementNumber, _
            NextElementName, NextElementDocumentation, P, Id, Name, Qualy, Var, Ind, Time)
    End Function
    ' StoreVariable
    Private Function StoreVariable(ByVal P As Drawing.Point) As Boolean
        m_Ladder.AddAndDrawVariable(m_bVar, m_bVarType, P, _
            New Drawing.Size(44, 44))
        DrawVisibleArea()
        Return True
    End Function

    Private Sub ConfirmAddConnection()
        If ItemsSelectedForConnectionOK() Then
            StoreConnection()
            ' probabilmente si vorranno realizzare tutte le connessioni di un rung
            ' dopo aver inserito tutti i contatti
            If Not (Preferences.GetBoolean("KeepConnectState", True)) Then _
                Stato = Opzione.Selection
            CancelVisibleArea()
            DeSelectAll()
            DrawVisibleArea()
        Else
            If FBDConnectionOK() Then
                StoreFBDConnection()
                ' probabilmente si vorranno realizzare tutte le connessioni di un rung
                ' dopo aver inserito tutti i contatti
                If Not (Preferences.GetBoolean("KeepConnectState", True)) Then _
                    Stato = Opzione.Selection
                CancelVisibleArea()
                DeSelectAll()
                DrawVisibleArea()
            Else
                MsgBox("Not enough elements chosen for a connection or creating this connection would result in a loop." + vbCrLf + "At least an input and an output are required and feedback is not allowed in LD", MsgBoxStyle.Critical, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
            End If
        End If
    End Sub

    Private Function FBDConnectionOK() As Boolean
        Dim incOK As IPureConnectable = m_Ladder.FindFBDObjectAcceptingIncomingConnection()
        Dim outOK As IPureConnectable = m_Ladder.FindFBDObjectAcceptingOutgoingConnection()
        ' da perfezionare con le configurazioni ammesse:
        ' outOK = Input Variable, incOK = Block
        ' outOK = Block, incOK = Output Variable
        Return (incOK IsNot Nothing AndAlso outOK IsNot Nothing)
    End Function

    Private Sub StoreFBDConnection()
        Dim incOK As IPureConnectable = m_Ladder.FindFBDObjectAcceptingIncomingConnection()
        Dim outOK As IPureConnectable = m_Ladder.FindFBDObjectAcceptingOutgoingConnection()
        m_Ladder.AddAndDrawFBDConnection(incOK, outOK)
    End Sub

    Private Function StoreConnection() As Boolean
        m_Ladder.AddAndDrawConnection(m_Ladder.FirstAvailableElementNumber, NextElementName, _
            NextElementDocumentation, NextCondition)
        'If TreeOpen Then
        'TreeView1.Nodes(1).Nodes.Add(NextElementName)
        'TreeView1.Nodes(1).Expand()
        'End If
    End Function

    ' Controlla se un quaslsiasi elemento di l1 si trova anche in l2 o viceversa
    Private Function CheckForFeedback(ByVal l1 As GraphicalContactList, ByVal l2 As GraphicalContactList) As Boolean
        For Each X As BaseGraphicalContact In l1
            If l2.IndexOf(X) >= 0 Then Return True
        Next
        For Each Y As BaseGraphicalContact In l2
            If l1.IndexOf(Y) >= 0 Then Return True
        Next
    End Function

    Private Function ItemsSelectedForConnectionOK() As Boolean
        If (m_Ladder.GraphicalContactList.ReadBottomSelectedContactList.CountStepsList > 0) _
            And (m_Ladder.GraphicalContactList.ReadTopSelectedContactList.CountStepsList > 0) Then _
            Return Not (CheckForFeedback(m_Ladder.GraphicalContactList.ReadBottomSelectedContactList, _
                m_Ladder.GraphicalContactList.ReadTopSelectedContactList))
        Return False
    End Function

    ' Il parametro "silent" viene usato per controllare le cancellazioni pilotate in cui non c'è
    ' bisogno di conferma
    Public Sub RemoveSelectedElements(Optional ByVal silent As Boolean = False)
        If m_Ladder.GraphicalContactList.ReadSelected.CountSelected > 0 Or _
            m_Ladder.GraphicalConnectionList.ReadSelectedConnectionList.CountSelected > 0 Or _
            m_Ladder.GraphicalVariablesList.CountSelected > 0 Or _
            m_Ladder.GraphicalFBDConnectionsList.CountSelected > 0 Then
            If m_Ladder.GraphicalConnectionList.ControllaPresenzaStepsInTransizioni(m_Ladder.GraphicalContactList.ReadSelected) Then
                ' Evitiamo il problema di cancellare sequenze di contatti con connessioni (forse un giorno)
                ' In effetti il problema in questo tipo di operazione è che non si può usare un For Each
                ' (lo stato potrebbe essere modificato in modo imprevisto da cancellazioni precedenti)
                ' e bisognerebbe ricalcolare le tabelle contatti <--> connessioni dopo ogni iterazione
                If m_Ladder.GraphicalContactList.ReadSelected.Count > 1 Then
                    MsgBox("Sequences of contacts/coils can't be deleted in a row, unless all the contacts/coils are not connected to anything else", MsgBoxStyle.Critical, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
                    Exit Sub
                End If
                ' se stiamo cancellando connessioni non dovremmo trovarci qui
                If m_Ladder.GraphicalContactList.ReadSelected.Count = 0 Then Exit Sub
                Dim contact As BaseGraphicalContact = m_Ladder.GraphicalContactList.ReadSelected()(0)
                Dim theList As GraphicalConnectionList = m_Ladder.GraphicalConnectionList.FindConnectionsForObject(contact)
                If My.Computer.Keyboard.ShiftKeyDown OrElse (MsgBox("Do you really want to delete the selected elements?", MsgBoxStyle.OkCancel Or MsgBoxStyle.Question, UniSimVersion.VersionInfo.PrintableDescriptionForTool("")) = MsgBoxResult.Ok) Then
                    ' se stiamo cancellando un blocco, eliminiamo anche (e prima) le connessioni FBD
                    If contact.IsBlock Then
                        Dim theIncomingFBDList As GraphicalFBDConnectionsList = CType(contact, GraphicalContact).FindIncomingFBDConnections()
                        Dim theOutgoingFBDList As GraphicalFBDConnectionsList = CType(contact, GraphicalContact).FindOutgoingFBDConnections()
                        For Each ifc As GraphicalFBDConnection In theIncomingFBDList
                            m_Ladder.GraphicalFBDConnectionsList.Remove(ifc)
                        Next
                        For Each ofc As GraphicalFBDConnection In theOutgoingFBDList
                            m_Ladder.GraphicalFBDConnectionsList.Remove(ofc)
                        Next
                    End If
                    m_Ladder.GraphicalContactList.DeSelectAll()
                    For Each conn As GraphicalConnection In theList
                        conn.SetSelected(True)
                        Call RemoveSelectedElements(True)
                    Next
                    contact.SetSelected(True)
                    Call RemoveSelectedElements(True)
                End If
            Else
                If silent OrElse My.Computer.Keyboard.ShiftKeyDown OrElse (MsgBox("Do you really want to delete the selected elements?", MsgBoxStyle.OkCancel Or MsgBoxStyle.Question, UniSimVersion.VersionInfo.PrintableDescriptionForTool("")) = MsgBoxResult.Ok) Then
                    m_Ladder.StopStateMonitor()
                    m_Ladder.RemoveSelectedElements()
                    CancelVisibleArea()
                    DrawVisibleArea()
                    If m_monitoring Then
                        m_Ladder.StartStateMonitor()
                    End If
                    DrawVisibleArea()
                End If
                Stato = Opzione.Selection
            End If
        Else
            MsgBox("Nothing selected to delete", MsgBoxStyle.Information, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
        End If
    End Sub

    Public Sub RemoveRail()
        If My.Computer.Keyboard.CtrlKeyDown Then
            While RemoveRailImpl() : End While
        Else
            Call RemoveRailImpl()
        End If
    End Sub

    Private Function RemoveRailImpl() As Boolean

        If Counter > 0 Then
            m_Ladder.SelectRail()

            If m_Ladder.GraphicalConnectionList.ControllaPresenzaStepsInTransizioni(m_Ladder.GraphicalContactList.ReadSelected) Then
                MsgBox("This power rail is connected to something. Cannot remove it", MsgBoxStyle.Critical, _
                    UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
                Return False
            Else

                m_Ladder.RemoveSelectedRail()
                CancelRailVisibleArea()
                DrawVisibleArea()
                Stato = Opzione.Selection
                Counter = Counter - 60
                Return True
            End If
        Else
            MsgBox("No power rails found", MsgBoxStyle.Critical, _
                UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
            Return False
        End If

    End Function

    ' imposta correttamente l'altezza
    Private Sub SetGoodHeight()
        Dim minHeight As Integer = 1200
        Dim railsHeight As Integer = 60 * m_Ladder.GetRailsCount()
        Counter = railsHeight
        railsHeight += 168
        If railsHeight > minHeight Then _
            ResizePanel(New Size(Panel3.Width, railsHeight))
    End Sub

    Private Sub FormLadder_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        WriteTitlePanel()
        SetGoodHeight()
        Button1_Click(sender, e)
        Me.SetToolTip()
    End Sub


    Public Sub WriteTitlePanel() Implements IEditorForm.WriteTitlePanel
        Me.Text = m_Ladder.Name
    End Sub

    Public Sub ResetCurrentOperation() Implements IEditorForm.ResetCurrentOperation
        Stato = Opzione.Selection
        CancelVisibleArea()
        DrawVisibleArea()
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
    Private Sub CancelRailVisibleArea()
        Try
            If Monitor.TryEnter(m_GraphToDraw, 2000) Then
                m_GraphToDraw.Clear(ColSfondo)
            End If
        Catch ex As System.Exception
        Finally
            Monitor.Exit(m_GraphToDraw)
        End Try
    End Sub

    Private Sub DrawVisibleArea()
        Dim R As New Drawing.Rectangle(0 - Panel3.Left, 0 - Panel3.Top, Panel3.Width - Panel3.Left, Panel3.Height - Panel3.Top)
        DrawArea(R)
    End Sub

    Private Sub DrawArea(ByVal Rect As Drawing.Rectangle)
        Rect.X = Rect.X - 1
        Rect.Y = Rect.Y - 1
        Rect.Width = Rect.Width + 2
        Rect.Height = Rect.Height + 2
        If Stato = Opzione.Connection Then
            m_Ladder.DrawElementsArea(Rect, True)
        Else
            m_Ladder.DrawElementsArea(Rect, False)
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
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
    Public Sub StartMonitor() Implements IEditorForm.StartMonitor
        m_Ladder.StartStateMonitor()
    End Sub
    Public Sub SetDrawVariable()
        m_Ladder.SetDrawVariable()
        CancelVisibleArea()
        DrawVisibleArea()
    End Sub
    Public Sub StopMonitor() Implements IEditorForm.StopMonitor
        m_Ladder.StopStateMonitor()
        CancelVisibleArea()
        DrawVisibleArea()
    End Sub
    Private Sub SetLocalVariable(ByVal Name As String, ByVal Qualy As String, ByVal Var As BaseVariable, ByVal Ind As BaseVariable, ByVal Time As TimeSpan)
        m_qualifier = Qualy
        m_name = Name
        m_variable = Var
        m_indicator = Ind
        m_time = Time
    End Sub
    Private Sub ResetLocalVariable()
        m_qualifier = ""
        m_name = ""
    End Sub

    ' Questa subroutine aggiunge alle variabili locali del m_Ladder le seguenti variabili
    ' Ix, x = 1..IU
    ' Ux, x = 1..IU
    ' Wx, x = 1..W
    ' (è una versione scalata al ribasso del PLC virtuale presentato al corso di TSA)
    Private Sub AddVirtualPLCVariables(ByVal sender As Object, ByVal e As EventArgs) Handles MenuItem7.Click

        ' Questi parametri personalizzano il processo di creazione delle variabili
        ' indicando quante variabili I/U e quante variabili W si devono creare
        ' Assunzione: W >= IU
        Const IU As Integer = 5
        Const W As Integer = 7

        Dim i As Integer
        For i = 1 To IU
            m_Ladder.PouInterface.localVars.CreateAndAddVariable("I" + i.ToString(), "", "", "false", "BOOL")
            m_Ladder.PouInterface.localVars.CreateAndAddVariable("U" + i.ToString(), "", "", "false", "BOOL")
            m_Ladder.PouInterface.localVars.CreateAndAddVariable("W" + i.ToString(), "", "", "false", "BOOL")
        Next
        For i = IU + 1 To W
            m_Ladder.PouInterface.localVars.CreateAndAddVariable("W" + i.ToString(), "", "", "false", "BOOL")
        Next
    End Sub

    ' Crea il menu per l'editor variabili
    ' Differenze con il codice SFC:
    '  - Usa m_Ladder invece di m_Sfc
    Private Sub toolBar1_ButtonDropDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ToolBar1.ButtonDropDown
        If e.Button Is Me.VariablesMenu Then
            Dim myMenu As ContextMenu = New ContextMenu()
            Dim localVarsMenu As MenuItem = m_Ladder.PouInterface.localVars.GetMenu()
            If m_Pou.PouType = EnumPouType.Function Then _
                myMenu.MenuItems.Add(m_Ladder.PouInterface.inputVars.GetMenu())
            ' localVarsMenu.MenuItems.Add("Create a virtual PLC", AddressOf AddVirtualPLCVariables)
            myMenu.MenuItems.Add(localVarsMenu)
            If Not m_Ladder.ResGlobalVariables Is Nothing Then
                For Each VL As VariablesList In m_Ladder.ResGlobalVariables
                    If VL Is Nothing Then Continue For
                    myMenu.MenuItems.Add(VL.GetMenu())
                Next
            End If
            e.Button.DropDownMenu = myMenu
        End If
    End Sub


    Private Sub MenuItem17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem17.Click
        Stop
    End Sub

    Private Sub MenuItem18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem18.Click
        For Each r As Rung In m_Ladder.GetRungs()
            If r.CoilsCount = 0 Then Continue For
            MsgBox(r.ToString(), MsgBoxStyle.Information, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
        Next
    End Sub

    Private Sub MenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem2.Click
        Call AddContact()
    End Sub

    Private Sub MenuItem6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem6.Click
        Call AddCoil()
    End Sub

    Private Sub MenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem3.Click
        Call AddRail()
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
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

    Private Sub ScanCycle() Handles m_Ladder.StartScan
        Try
            Call ExecuteBar1.NextValue()
        Catch ex As Exception
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
        If Stato <> Opzione.Selection Then Return Nothing
        If m_Ladder.CountSelectedElement() > 1 OrElse Me.MultipleSelection Then Return Nothing
        If m_Ladder.CountSelectedElement() = 1 Then
            Dim obj As Object = m_Ladder.ReadObjectSelected()
            If TypeOf (obj) Is IDocumentable Then _
                Return CType(obj, IDocumentable)
            Return Nothing
        End If
        If m_Ladder.CountSelectedElement() = 0 Then Return m_Body
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
        ' Caso speciale: sebbene a rigore le connessioni sono IDocumentable, lo standard non prevede
        ' possibilità di salvare questi commenti. Siccome però è utile poter leggere la descrizione per
        ' una connessione, facciamo in modo che i commenti non possano essere associati alle connessioni
        ' ma manteniamo l'implementazione di IDocumentable
        If TypeOf (iComm) Is GraphicalConnection Then Return
        Dim commEditorWnd As New CommentsDialogForm(iComm)
        If commEditorWnd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            iComm.Documentation = commEditorWnd.CommentTypedIn
            SetToolTip()
        End If
    End Sub

    Private Sub ReorderParameters()
        If Not EditingMode Then Exit Sub
        If Stato <> Opzione.Selection Then Exit Sub
        If m_Ladder.CountSelectedElement() > 1 OrElse Me.MultipleSelection Then Exit Sub
        If m_Ladder.CountSelectedElement() = 1 Then
            Dim Obj As Object = m_Ladder.ReadObjectSelected
            If Not (TypeOf (Obj) Is GraphicalContact) Then Exit Sub
            Dim contact As GraphicalContact = CType(Obj, GraphicalContact)
            If Not (contact.IsBlock) Then Exit Sub
            Dim paramDialog As New LDParametersReorderDialog(m_Ladder, contact)
            paramDialog.ShowDialog()
        End If
    End Sub

    Private Sub Panel3_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Panel3.DoubleClick
        If Not EditingMode Then Exit Sub
        If Stato <> Opzione.Selected Then Exit Sub
        If m_Ladder.CountSelectedElement() <> 1 OrElse Me.MultipleSelection Then Exit Sub
        Dim obj As Object = m_Ladder.ReadObjectSelected()
        If TypeOf (obj) Is GraphicalContact Then
            Dim graphObj As GraphicalContact = CType(obj, GraphicalContact)
            If graphObj.IsContact Then
                ' edita come contatto
                If Not (My.Computer.Keyboard.CtrlKeyDown) Then
                    Dim editor As New ContactDialogForm(m_Ladder.ResGlobalVariables, m_Pou.PouInterface, graphObj)
                    If editor.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        graphObj.Qualy = editor.m_qualifier
                        graphObj.BoundVariable = editor.m_variable
                        graphObj.Name = editor.m_ContactName
                    End If
                Else
                    If graphObj.BoundVariable IsNot Nothing Then
                        graphObj.BoundVariable.SetValue(Not graphObj.BoundVariable.ReadValue())
                    End If
                    End If
            ElseIf graphObj.IsCoil Then
                    ' edita come bobina
                    Dim editor As New CoilDialogForm(m_Ladder.ResGlobalVariables, m_Pou.PouInterface, graphObj)
                    If editor.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        graphObj.Qualy = editor.m_qualifier
                        graphObj.BoundVariable = editor.m_variable
                        graphObj.Name = editor.m_CoilName
                    End If
            ElseIf graphObj.IsBlock Then
                    ' edita come blocco
                    Dim block As GraphicalContact = CType(obj, GraphicalContact)
                    Dim blockDialog As New LDBlockChooser(m_Ladder.PousList, block, True)
                    If blockDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        block.Qualy = blockDialog.BlockType
                        block.Name = blockDialog.BlockName
                    End If
            Else
                    MsgBox("Unknown, uneditable object", MsgBoxStyle.Critical, _
                        UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
            End If
        ElseIf TypeOf (obj) Is GraphicalVariable Then
            Dim graphVar As GraphicalVariable = CType(obj, GraphicalVariable)
            If My.Computer.Keyboard.CtrlKeyDown AndAlso graphVar.BoundVariable.dataType = "BOOL" _
                AndAlso graphVar.VariableType = GraphicalVariableType.Input Then
                graphVar.BoundVariable.SetValue(Not graphVar.BoundVariable.ReadValue)
            Else
                Dim varsSelector As New FBDVariableBlockDialog(m_Body.ResGlobalVariables, _
                    m_Body.PouInterface, graphVar, FBDVariableBlockDialogType.fvbdtLD)
                If varsSelector.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    graphVar.BoundVariable = varsSelector.m_variable
                    If (m_Ladder.GraphicalFBDConnectionsList.FindAllConnectionsEndingWith(graphVar).Count > 0 _
                        OrElse m_Ladder.GraphicalFBDConnectionsList.FindAllConnectionsStartingWith(graphVar).Count > 0) _
                            AndAlso graphVar.VariableType <> varsSelector.m_VariableType Then
                        MsgBox("This variable is already connected to some other diagram entity and its type cannot be changed. Disconnect it before retrying", MsgBoxStyle.Critical, _
                            UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
                    Else
                        graphVar.VariableType = varsSelector.m_VariableType
                    End If
                End If
            End If
        End If
        Refresh()
    End Sub

    Public Sub ResizePanel(ByVal newSize As Size) Implements IEditorForm.ResizePanel
        Panel3.Size = newSize
        Foglio.Width = newSize.Width
        Foglio.Height = newSize.Height
        Try
            If Monitor.TryEnter(m_GraphToDraw, 2000) Then
                Try
                    m_GraphToDraw = Panel3.CreateGraphics
                    If Monitor.TryEnter(m_GraphToDraw, 4000) Then
                        m_GraphToDraw.Clear(Color.White)
                        m_Ladder.SetGraphToDraw(m_GraphToDraw)
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

    Private Sub MenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem4.Click
        Ladder.RtL = Not (Ladder.RtL)
        MenuItem4.Text = m_ScanOrderEntries(Ladder.RtL)("menuitem")
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
        m_Ladder.PrintMe(imgGraphics, _
            imgSize)
        img.Save(destFile, Imaging.ImageFormat.Jpeg)
    End Sub

    Private Sub MenuItemExportAsJPG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemExportAsJPG.Click
        If sdgJPEGFile.ShowDialog() = Windows.Forms.DialogResult.OK Then _
            ExportAsJPEG(sdgJPEGFile.FileName)
    End Sub

    ' Se l'unica selezione è una bobina, e questa bobina è direttamente collegata ad una alimentazione destra
    ' stampa il rung associato
    Private Sub MenuItem8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem8.Click
        If Not EditingMode Then Exit Sub
        If m_Ladder.CountSelectedElement() <> 1 OrElse Me.MultipleSelection Then Exit Sub
        Dim obj As Object = m_Ladder.ReadObjectSelected()
        If TypeOf (obj) Is GraphicalContact Then
            Dim theContact As GraphicalContact = CType(obj, GraphicalContact)
            If Not (theContact.IsCoil) Then Exit Sub
            Dim theRight As GraphicalContactList = m_Ladder.GraphicalConnectionList.FindRightOf(theContact)
            If theRight.Count <> 1 Then Exit Sub
            obj = theRight(0)
            If TypeOf (obj) Is GraphicalRightRail Then
                Dim theRung As Rung = New Rung(CType(obj, GraphicalRightRail))
                MsgBox(theRung.ToString(), MsgBoxStyle.Information, UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
            End If
        End If
    End Sub

    Private Sub MenuItem9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem9.Click
        AddBlock()
    End Sub

    Private Sub MenuItem11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem11.Click
        AddVariable()
    End Sub
End Class

