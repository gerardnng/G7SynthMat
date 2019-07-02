
Imports System
Imports System.IO
Imports System.Text
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Threading
Imports System.Runtime.Serialization.Formatters.Binary
Imports UnisimClassLibrary




Public Class MainForm
    Inherits System.Windows.Forms.Form
    Implements IUniSimMainWindow

    Private Shared parentWindow As MainForm
    Private components As System.ComponentModel.IContainer

    Private WithEvents m_SimulatorEngineHandles As SimulatorEngine
    Private m_Resource As Resource   'Riferimento all'unica risorsa
    Private m_SimulationState As String

    Private m_XMLprojectManager As XMLProjectManager

    Private m_TimerSplashForm As System.Threading.Timer
    'Private m_TimerSplashForm As System.Timers.Timer

    Friend WithEvents Separator1 As System.Windows.Forms.ToolBarButton
    Friend WithEvents RunTheSimButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents StopTheSimButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemExportWithName As System.Windows.Forms.MenuItem
    Private m_FrmSpash As SplashForm
    Private m_DisposeSplashForm As Boolean

    Friend WithEvents MenuItemEditPrefs As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemProjPanel As System.Windows.Forms.MenuItem
    Friend WithEvents PousTree1 As PousTree
    Friend WithEvents ResourceTree1 As ResourceTree
    Friend WithEvents MenuItemPOUsManager As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemAddPOU As System.Windows.Forms.MenuItem
    Friend WithEvents RefreshButton As System.Windows.Forms.ToolBarButton

    Private m_LastFileName As String
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents lblSimDuration As System.Windows.Forms.Label

    Friend WithEvents chkPreemptiveMultitasking As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label

#Region "Hydras"
    ' Questa Hydra è usata nel metodo RefreshMenuToolbar()
    Private m_MenuItemsState As New Hydra(Of Boolean, MenuItem, Boolean)
    Friend WithEvents MenuItemNewProjectHeader As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem6 As System.Windows.Forms.MenuItem
    ' Questa Hydra contiene le impostazioni del motore di simulazione
    Private m_SimulatorPerformance As New Hydra(Of Integer, String, Integer)
    Private Sub FillSimPerformanceHydra()
        With m_SimulatorPerformance
            .AddValueToAllEntries("UsePreemptiveScheduler", _
                New KeyValuePair(Of Integer, Integer)(1, False), _
                New KeyValuePair(Of Integer, Integer)(2, True), _
                New KeyValuePair(Of Integer, Integer)(3, True))
            .AddValueToAllEntries("SimulationPriority", _
                New KeyValuePair(Of Integer, Integer)(1, 0), _
                New KeyValuePair(Of Integer, Integer)(2, 50000), _
                New KeyValuePair(Of Integer, Integer)(3, 100000))
        End With
    End Sub
    Private Sub FillMenuItemsStateHydra()
        With m_MenuItemsState
            .AddValueToAllEntries(MenuItemProjectImport, _
                New KeyValuePair(Of Boolean, Boolean)(True, True), _
                New KeyValuePair(Of Boolean, Boolean)(False, True))
            .AddValueToAllEntries(MenuItemProjectExport, _
                New KeyValuePair(Of Boolean, Boolean)(True, False), _
                New KeyValuePair(Of Boolean, Boolean)(False, True))
            .AddValueToAllEntries(MenuItemExportWithName, _
                New KeyValuePair(Of Boolean, Boolean)(True, False), _
                New KeyValuePair(Of Boolean, Boolean)(False, True))
            .AddValueToAllEntries(MenuItemClose, _
                New KeyValuePair(Of Boolean, Boolean)(True, False), _
                New KeyValuePair(Of Boolean, Boolean)(False, True))
            .AddValueToAllEntries(MenuItemXmlView, _
                New KeyValuePair(Of Boolean, Boolean)(True, False), _
                New KeyValuePair(Of Boolean, Boolean)(False, True))
            .AddValueToAllEntries(MenuItemPrint, _
                New KeyValuePair(Of Boolean, Boolean)(True, False), _
                New KeyValuePair(Of Boolean, Boolean)(False, True))
            .AddValueToAllEntries(MenuItemInformationProject, _
                New KeyValuePair(Of Boolean, Boolean)(True, False), _
                New KeyValuePair(Of Boolean, Boolean)(False, True))
            .AddValueToAllEntries(MenuItemInformationProject, _
                New KeyValuePair(Of Boolean, Boolean)(True, False), _
                New KeyValuePair(Of Boolean, Boolean)(False, True))
            .AddValueToAllEntries(MenuItemPOUsManager, _
                New KeyValuePair(Of Boolean, Boolean)(True, False), _
                New KeyValuePair(Of Boolean, Boolean)(False, True))
            .AddValueToAllEntries(MenuItemSimulation, _
                New KeyValuePair(Of Boolean, Boolean)(True, False), _
                New KeyValuePair(Of Boolean, Boolean)(False, True))
            .AddValueToAllEntries(MenuItemWindow, _
                New KeyValuePair(Of Boolean, Boolean)(True, False), _
                New KeyValuePair(Of Boolean, Boolean)(False, True))
            .AddValueToAllEntries(MenuItemNewProjectHeader, _
                New KeyValuePair(Of Boolean, Boolean)(True, False), _
                New KeyValuePair(Of Boolean, Boolean)(False, True))
        End With
    End Sub
#End Region

#Region " Codice generato da Progettazione Windows Form "

    Public Sub New()
        parentWindow = Me
        InitializeComponent()
        Call SetTitleBar()
        Me.ResourceTree1.MdiParentForm = Me
        Me.PousTree1.MdiParentForm = Me
        m_SimulationState = "STOP"
        Me.FillMenuItemsStateHydra()
        Me.FillSimPerformanceHydra()
    End Sub 'New

    'Il form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub
    Private WithEvents SaveButton As System.Windows.Forms.ToolBarButton
    Private WithEvents OpenButton As System.Windows.Forms.ToolBarButton
    Private WithEvents NewButton As System.Windows.Forms.ToolBarButton
    Private WithEvents toolBar1 As System.Windows.Forms.ToolBar
    Private WithEvents PrintDoc As System.Drawing.Printing.PrintDocument
    Private WithEvents MenuItemAbout As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemStep7 As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemAffVert As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemToolbar As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemHelp As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemWindow As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemView As System.Windows.Forms.MenuItem
    Private WithEvents mdiClient1 As System.Windows.Forms.MdiClient
    Private WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents ToolBarButton1 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton2 As System.Windows.Forms.ToolBarButton
    Private WithEvents MenuItemCascade As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemAffOriz As System.Windows.Forms.MenuItem
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TrackBar1 As System.Windows.Forms.TrackBar
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ButtonResetSim As System.Windows.Forms.Button
    Friend WithEvents ButtonStepSim As System.Windows.Forms.Button
    Friend WithEvents ButtonStopSim As System.Windows.Forms.Button
    Friend WithEvents ButtonStartSim As System.Windows.Forms.Button
    Friend WithEvents MenuItemSimulation As System.Windows.Forms.MenuItem
    Friend WithEvents NewPOUButton As System.Windows.Forms.ToolBarButton
    Friend WithEvents MenuItemProject As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemInformationProject As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemProjectExport As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemProjectImport As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemSimulationSimulation As System.Windows.Forms.MenuItem
    Friend WithEvents ContextMenu1 As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuInteShowPOUBody As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemSetPOUInfo2 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemRemovePou2 As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemNew As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemClose As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemPrint As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemExit As System.Windows.Forms.MenuItem
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents ProjectPanel As System.Windows.Forms.TabControl
    Friend WithEvents ContextMenu3 As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuItem7 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem8 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem9 As System.Windows.Forms.MenuItem
    Friend WithEvents ContextMenu4 As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuItem10 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem12 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem11 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem13 As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemHelpTopics As System.Windows.Forms.MenuItem
    Friend WithEvents SimulationPanel As System.Windows.Forms.Panel
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemXmlView As System.Windows.Forms.MenuItem



    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.toolBar1 = New System.Windows.Forms.ToolBar()
        Me.NewButton = New System.Windows.Forms.ToolBarButton()
        Me.OpenButton = New System.Windows.Forms.ToolBarButton()
        Me.SaveButton = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButton1 = New System.Windows.Forms.ToolBarButton()
        Me.NewPOUButton = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButton2 = New System.Windows.Forms.ToolBarButton()
        Me.RefreshButton = New System.Windows.Forms.ToolBarButton()
        Me.Separator1 = New System.Windows.Forms.ToolBarButton()
        Me.RunTheSimButton = New System.Windows.Forms.ToolBarButton()
        Me.StopTheSimButton = New System.Windows.Forms.ToolBarButton()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.mdiClient1 = New System.Windows.Forms.MdiClient()
        Me.MenuItemAffOriz = New System.Windows.Forms.MenuItem()
        Me.MenuItemView = New System.Windows.Forms.MenuItem()
        Me.MenuItemToolbar = New System.Windows.Forms.MenuItem()
        Me.MenuItemProjPanel = New System.Windows.Forms.MenuItem()
        Me.MenuItemSimulationSimulation = New System.Windows.Forms.MenuItem()
        Me.PrintDoc = New System.Drawing.Printing.PrintDocument()
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.MenuItemProject = New System.Windows.Forms.MenuItem()
        Me.MenuItemNew = New System.Windows.Forms.MenuItem()
        Me.MenuItemProjectImport = New System.Windows.Forms.MenuItem()
        Me.MenuItemProjectExport = New System.Windows.Forms.MenuItem()
        Me.MenuItemExportWithName = New System.Windows.Forms.MenuItem()
        Me.MenuItemClose = New System.Windows.Forms.MenuItem()
        Me.MenuItemInformationProject = New System.Windows.Forms.MenuItem()
        Me.MenuItemNewProjectHeader = New System.Windows.Forms.MenuItem()
        Me.MenuItemXmlView = New System.Windows.Forms.MenuItem()
        Me.MenuItemPrint = New System.Windows.Forms.MenuItem()
        Me.MenuItem2 = New System.Windows.Forms.MenuItem()
        Me.MenuItemEditPrefs = New System.Windows.Forms.MenuItem()
        Me.MenuItem5 = New System.Windows.Forms.MenuItem()
        Me.MenuItemExit = New System.Windows.Forms.MenuItem()
        Me.MenuItemPOUsManager = New System.Windows.Forms.MenuItem()
        Me.MenuItemAddPOU = New System.Windows.Forms.MenuItem()
        Me.MenuItemSimulation = New System.Windows.Forms.MenuItem()
        Me.MenuItem1 = New System.Windows.Forms.MenuItem()
        Me.MenuItem3 = New System.Windows.Forms.MenuItem()
        Me.MenuItem4 = New System.Windows.Forms.MenuItem()
        Me.MenuItemWindow = New System.Windows.Forms.MenuItem()
        Me.MenuItemCascade = New System.Windows.Forms.MenuItem()
        Me.MenuItemAffVert = New System.Windows.Forms.MenuItem()
        Me.MenuItemHelp = New System.Windows.Forms.MenuItem()
        Me.MenuItemHelpTopics = New System.Windows.Forms.MenuItem()
        Me.MenuItem6 = New System.Windows.Forms.MenuItem()
        Me.MenuItemStep7 = New System.Windows.Forms.MenuItem()
        Me.MenuItemAbout = New System.Windows.Forms.MenuItem()
        Me.ContextMenu1 = New System.Windows.Forms.ContextMenu()
        Me.MenuItem11 = New System.Windows.Forms.MenuItem()
        Me.MenuInteShowPOUBody = New System.Windows.Forms.MenuItem()
        Me.MenuItemSetPOUInfo2 = New System.Windows.Forms.MenuItem()
        Me.MenuItemRemovePou2 = New System.Windows.Forms.MenuItem()
        Me.ButtonStopSim = New System.Windows.Forms.Button()
        Me.ButtonStartSim = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TrackBar1 = New System.Windows.Forms.TrackBar()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ButtonResetSim = New System.Windows.Forms.Button()
        Me.ButtonStepSim = New System.Windows.Forms.Button()
        Me.SimulationPanel = New System.Windows.Forms.Panel()
        Me.chkPreemptiveMultitasking = New System.Windows.Forms.CheckBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.lblSimDuration = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ProjectPanel = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.PousTree1 = New UnisimClassLibrary.PousTree()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.ResourceTree1 = New UnisimClassLibrary.ResourceTree()
        Me.ContextMenu4 = New System.Windows.Forms.ContextMenu()
        Me.MenuItem10 = New System.Windows.Forms.MenuItem()
        Me.MenuItem13 = New System.Windows.Forms.MenuItem()
        Me.MenuItem12 = New System.Windows.Forms.MenuItem()
        Me.ContextMenu3 = New System.Windows.Forms.ContextMenu()
        Me.MenuItem9 = New System.Windows.Forms.MenuItem()
        Me.MenuItem7 = New System.Windows.Forms.MenuItem()
        Me.MenuItem8 = New System.Windows.Forms.MenuItem()
        Me.Splitter1 = New System.Windows.Forms.Splitter()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SimulationPanel.SuspendLayout()
        Me.ProjectPanel.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'toolBar1
        '
        Me.toolBar1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.toolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.NewButton, Me.OpenButton, Me.SaveButton, Me.ToolBarButton1, Me.NewPOUButton, Me.ToolBarButton2, Me.RefreshButton, Me.Separator1, Me.RunTheSimButton, Me.StopTheSimButton})
        Me.toolBar1.ButtonSize = New System.Drawing.Size(24, 24)
        Me.toolBar1.DropDownArrows = True
        Me.toolBar1.ImageList = Me.ImageList1
        Me.toolBar1.Location = New System.Drawing.Point(0, 0)
        Me.toolBar1.Name = "toolBar1"
        Me.toolBar1.ShowToolTips = True
        Me.toolBar1.Size = New System.Drawing.Size(994, 37)
        Me.toolBar1.TabIndex = 1
        '
        'NewButton
        '
        Me.NewButton.ImageIndex = 0
        Me.NewButton.Name = "NewButton"
        Me.NewButton.ToolTipText = "New"
        '
        'OpenButton
        '
        Me.OpenButton.ImageIndex = 1
        Me.OpenButton.Name = "OpenButton"
        Me.OpenButton.ToolTipText = "Open"
        '
        'SaveButton
        '
        Me.SaveButton.ImageIndex = 2
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.ToolTipText = "Save"
        '
        'ToolBarButton1
        '
        Me.ToolBarButton1.Name = "ToolBarButton1"
        Me.ToolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'NewPOUButton
        '
        Me.NewPOUButton.ImageIndex = 8
        Me.NewPOUButton.Name = "NewPOUButton"
        Me.NewPOUButton.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton
        Me.NewPOUButton.ToolTipText = "New POU / POUs list"
        '
        'ToolBarButton2
        '
        Me.ToolBarButton2.Name = "ToolBarButton2"
        Me.ToolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'RefreshButton
        '
        Me.RefreshButton.ImageIndex = 18
        Me.RefreshButton.Name = "RefreshButton"
        Me.RefreshButton.ToolTipText = "Refresh"
        Me.RefreshButton.Visible = False
        '
        'Separator1
        '
        Me.Separator1.Name = "Separator1"
        Me.Separator1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'RunTheSimButton
        '
        Me.RunTheSimButton.ImageIndex = 16
        Me.RunTheSimButton.Name = "RunTheSimButton"
        Me.RunTheSimButton.ToolTipText = "Start simulation"
        '
        'StopTheSimButton
        '
        Me.StopTheSimButton.ImageIndex = 17
        Me.StopTheSimButton.Name = "StopTheSimButton"
        Me.StopTheSimButton.ToolTipText = "Stop simulation"
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
        Me.ImageList1.Images.SetKeyName(16, "servicerunning.ico")
        Me.ImageList1.Images.SetKeyName(17, "servicestopped.ico")
        Me.ImageList1.Images.SetKeyName(18, "reload.png")
        '
        'mdiClient1
        '
        Me.mdiClient1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.mdiClient1.Location = New System.Drawing.Point(152, 37)
        Me.mdiClient1.Name = "mdiClient1"
        Me.mdiClient1.Size = New System.Drawing.Size(842, 544)
        Me.mdiClient1.TabIndex = 0
        '
        'MenuItemAffOriz
        '
        Me.MenuItemAffOriz.Index = 2
        Me.MenuItemAffOriz.Text = "Tile &vertically"
        '
        'MenuItemView
        '
        Me.MenuItemView.Index = 3
        Me.MenuItemView.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemToolbar, Me.MenuItemProjPanel, Me.MenuItemSimulationSimulation})
        Me.MenuItemView.MergeOrder = 5
        Me.MenuItemView.Text = "&View"
        '
        'MenuItemToolbar
        '
        Me.MenuItemToolbar.Checked = True
        Me.MenuItemToolbar.Index = 0
        Me.MenuItemToolbar.Text = "&Toolbar"
        '
        'MenuItemProjPanel
        '
        Me.MenuItemProjPanel.Index = 1
        Me.MenuItemProjPanel.Shortcut = System.Windows.Forms.Shortcut.CtrlJ
        Me.MenuItemProjPanel.Text = "Pro&ject panel"
        '
        'MenuItemSimulationSimulation
        '
        Me.MenuItemSimulationSimulation.Index = 2
        Me.MenuItemSimulationSimulation.Shortcut = System.Windows.Forms.Shortcut.CtrlP
        Me.MenuItemSimulationSimulation.Text = "Simulation &panel"
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemProject, Me.MenuItemPOUsManager, Me.MenuItemSimulation, Me.MenuItemView, Me.MenuItemWindow, Me.MenuItemHelp})
        '
        'MenuItemProject
        '
        Me.MenuItemProject.Index = 0
        Me.MenuItemProject.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemNew, Me.MenuItemProjectImport, Me.MenuItemProjectExport, Me.MenuItemExportWithName, Me.MenuItemClose, Me.MenuItemInformationProject, Me.MenuItemNewProjectHeader, Me.MenuItemXmlView, Me.MenuItemPrint, Me.MenuItem2, Me.MenuItemEditPrefs, Me.MenuItem5, Me.MenuItemExit})
        Me.MenuItemProject.MergeOrder = 1
        Me.MenuItemProject.Text = "&Project"
        '
        'MenuItemNew
        '
        Me.MenuItemNew.Index = 0
        Me.MenuItemNew.Text = "&New"
        '
        'MenuItemProjectImport
        '
        Me.MenuItemProjectImport.Index = 1
        Me.MenuItemProjectImport.Text = "&Open..."
        '
        'MenuItemProjectExport
        '
        Me.MenuItemProjectExport.Index = 2
        Me.MenuItemProjectExport.Text = "&Save"
        '
        'MenuItemExportWithName
        '
        Me.MenuItemExportWithName.Index = 3
        Me.MenuItemExportWithName.Text = "S&ave as..."
        '
        'MenuItemClose
        '
        Me.MenuItemClose.Index = 4
        Me.MenuItemClose.Text = "&Close"
        '
        'MenuItemInformationProject
        '
        Me.MenuItemInformationProject.Index = 5
        Me.MenuItemInformationProject.Text = "Project &information"
        '
        'MenuItemNewProjectHeader
        '
        Me.MenuItemNewProjectHeader.Index = 6
        Me.MenuItemNewProjectHeader.Text = "Update project &header"
        '
        'MenuItemXmlView
        '
        Me.MenuItemXmlView.Index = 7
        Me.MenuItemXmlView.Text = "&View XML Format"
        '
        'MenuItemPrint
        '
        Me.MenuItemPrint.Index = 8
        Me.MenuItemPrint.Text = "&Print..."
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 9
        Me.MenuItem2.Text = "-"
        '
        'MenuItemEditPrefs
        '
        Me.MenuItemEditPrefs.Index = 10
        Me.MenuItemEditPrefs.Text = "Pre&ferences..."
        '
        'MenuItem5
        '
        Me.MenuItem5.Index = 11
        Me.MenuItem5.Text = "-"
        '
        'MenuItemExit
        '
        Me.MenuItemExit.Index = 12
        Me.MenuItemExit.Text = "&Exit"
        '
        'MenuItemPOUsManager
        '
        Me.MenuItemPOUsManager.Index = 1
        Me.MenuItemPOUsManager.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemAddPOU})
        Me.MenuItemPOUsManager.MergeOrder = 2
        Me.MenuItemPOUsManager.Text = "POUs"
        '
        'MenuItemAddPOU
        '
        Me.MenuItemAddPOU.Index = 0
        Me.MenuItemAddPOU.Text = "&Add POU..."
        '
        'MenuItemSimulation
        '
        Me.MenuItemSimulation.Index = 2
        Me.MenuItemSimulation.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem1, Me.MenuItem3, Me.MenuItem4})
        Me.MenuItemSimulation.MergeOrder = 3
        Me.MenuItemSimulation.Text = "Si&mulation"
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 0
        Me.MenuItem1.Shortcut = System.Windows.Forms.Shortcut.F5
        Me.MenuItem1.Text = "&Start simulation"
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 1
        Me.MenuItem3.Shortcut = System.Windows.Forms.Shortcut.CtrlIns
        Me.MenuItem3.Text = "S&top simulation"
        '
        'MenuItem4
        '
        Me.MenuItem4.Index = 2
        Me.MenuItem4.Shortcut = System.Windows.Forms.Shortcut.F10
        Me.MenuItem4.Text = "Step &by step"
        '
        'MenuItemWindow
        '
        Me.MenuItemWindow.Index = 4
        Me.MenuItemWindow.MdiList = True
        Me.MenuItemWindow.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemCascade, Me.MenuItemAffVert, Me.MenuItemAffOriz})
        Me.MenuItemWindow.MergeOrder = 6
        Me.MenuItemWindow.Text = "&Windows"
        '
        'MenuItemCascade
        '
        Me.MenuItemCascade.Index = 0
        Me.MenuItemCascade.Text = "&Cascade"
        '
        'MenuItemAffVert
        '
        Me.MenuItemAffVert.Index = 1
        Me.MenuItemAffVert.Text = "Tile &horizontally"
        '
        'MenuItemHelp
        '
        Me.MenuItemHelp.Index = 5
        Me.MenuItemHelp.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemHelpTopics, Me.MenuItem6, Me.MenuItemStep7, Me.MenuItemAbout})
        Me.MenuItemHelp.MergeOrder = 7
        Me.MenuItemHelp.Text = "&Help"
        '
        'MenuItemHelpTopics
        '
        Me.MenuItemHelpTopics.Index = 0
        Me.MenuItemHelpTopics.Text = "Content (Italian)"
        '
        'MenuItem6
        '
        Me.MenuItem6.Index = 1
        Me.MenuItem6.Text = "Content (English)"
        '
        'MenuItemStep7
        '
        Me.MenuItemStep7.Index = 2
        Me.MenuItemStep7.Text = "-"
        '
        'MenuItemAbout
        '
        Me.MenuItemAbout.Index = 3
        Me.MenuItemAbout.Text = "&About..."
        '
        'ContextMenu1
        '
        Me.ContextMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem11, Me.MenuInteShowPOUBody, Me.MenuItemSetPOUInfo2, Me.MenuItemRemovePou2})
        '
        'MenuItem11
        '
        Me.MenuItem11.Index = 0
        Me.MenuItem11.Text = "Add pou"
        '
        'MenuInteShowPOUBody
        '
        Me.MenuInteShowPOUBody.Index = 1
        Me.MenuInteShowPOUBody.Text = "Show body"
        '
        'MenuItemSetPOUInfo2
        '
        Me.MenuItemSetPOUInfo2.Index = 2
        Me.MenuItemSetPOUInfo2.Text = "Show info"
        '
        'MenuItemRemovePou2
        '
        Me.MenuItemRemovePou2.Index = 3
        Me.MenuItemRemovePou2.Text = "Remove"
        '
        'ButtonStopSim
        '
        Me.ButtonStopSim.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ButtonStopSim.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonStopSim.Enabled = False
        Me.ButtonStopSim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonStopSim.Image = CType(resources.GetObject("ButtonStopSim.Image"), System.Drawing.Image)
        Me.ButtonStopSim.Location = New System.Drawing.Point(677, 14)
        Me.ButtonStopSim.Name = "ButtonStopSim"
        Me.ButtonStopSim.Size = New System.Drawing.Size(56, 44)
        Me.ButtonStopSim.TabIndex = 19
        Me.ButtonStopSim.TabStop = False
        Me.ButtonStopSim.Text = "Stop"
        Me.ButtonStopSim.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage
        Me.ButtonStopSim.UseVisualStyleBackColor = False
        '
        'ButtonStartSim
        '
        Me.ButtonStartSim.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ButtonStartSim.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonStartSim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonStartSim.Image = CType(resources.GetObject("ButtonStartSim.Image"), System.Drawing.Image)
        Me.ButtonStartSim.Location = New System.Drawing.Point(621, 14)
        Me.ButtonStartSim.Name = "ButtonStartSim"
        Me.ButtonStartSim.Size = New System.Drawing.Size(56, 44)
        Me.ButtonStartSim.TabIndex = 1
        Me.ButtonStartSim.TabStop = False
        Me.ButtonStartSim.Text = "Start"
        Me.ButtonStartSim.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage
        Me.ButtonStartSim.UseVisualStyleBackColor = False
        '
        'Label7
        '
        Me.Label7.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(322, 7)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(40, 13)
        Me.Label7.TabIndex = 23
        Me.Label7.Text = "Priority"
        '
        'TrackBar1
        '
        Me.TrackBar1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TrackBar1.LargeChange = 1
        Me.TrackBar1.Location = New System.Drawing.Point(287, 23)
        Me.TrackBar1.Maximum = 100000
        Me.TrackBar1.Name = "TrackBar1"
        Me.TrackBar1.Size = New System.Drawing.Size(113, 45)
        Me.TrackBar1.TabIndex = 22
        Me.TrackBar1.TabStop = False
        Me.TrackBar1.TickFrequency = 10000
        Me.TrackBar1.Value = 2000
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(-1, 3)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(101, 13)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "Simulation Panel"
        '
        'Label6
        '
        Me.Label6.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label6.BackColor = System.Drawing.Color.White
        Me.Label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Red
        Me.Label6.Location = New System.Drawing.Point(510, 25)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(56, 16)
        Me.Label6.TabIndex = 21
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label5.BackColor = System.Drawing.Color.White
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(510, 5)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(56, 16)
        Me.Label5.TabIndex = 20
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(454, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 16)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "State:"
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(454, 25)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(48, 16)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Cycles/s:"
        '
        'ButtonResetSim
        '
        Me.ButtonResetSim.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ButtonResetSim.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonResetSim.Enabled = False
        Me.ButtonResetSim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonResetSim.Image = CType(resources.GetObject("ButtonResetSim.Image"), System.Drawing.Image)
        Me.ButtonResetSim.Location = New System.Drawing.Point(789, 14)
        Me.ButtonResetSim.Name = "ButtonResetSim"
        Me.ButtonResetSim.Size = New System.Drawing.Size(52, 44)
        Me.ButtonResetSim.TabIndex = 28
        Me.ButtonResetSim.TabStop = False
        Me.ButtonResetSim.Text = "Reset"
        Me.ButtonResetSim.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage
        Me.ButtonResetSim.UseVisualStyleBackColor = False
        '
        'ButtonStepSim
        '
        Me.ButtonStepSim.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ButtonStepSim.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonStepSim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonStepSim.Image = CType(resources.GetObject("ButtonStepSim.Image"), System.Drawing.Image)
        Me.ButtonStepSim.Location = New System.Drawing.Point(737, 14)
        Me.ButtonStepSim.Name = "ButtonStepSim"
        Me.ButtonStepSim.Size = New System.Drawing.Size(52, 44)
        Me.ButtonStepSim.TabIndex = 26
        Me.ButtonStepSim.TabStop = False
        Me.ButtonStepSim.Text = "Step"
        Me.ButtonStepSim.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage
        Me.ButtonStepSim.UseVisualStyleBackColor = False
        '
        'SimulationPanel
        '
        Me.SimulationPanel.BackColor = System.Drawing.Color.Navy
        Me.SimulationPanel.Controls.Add(Me.chkPreemptiveMultitasking)
        Me.SimulationPanel.Controls.Add(Me.ButtonStepSim)
        Me.SimulationPanel.Controls.Add(Me.ButtonStartSim)
        Me.SimulationPanel.Controls.Add(Me.ButtonStopSim)
        Me.SimulationPanel.Controls.Add(Me.Label13)
        Me.SimulationPanel.Controls.Add(Me.Label4)
        Me.SimulationPanel.Controls.Add(Me.Label5)
        Me.SimulationPanel.Controls.Add(Me.ButtonResetSim)
        Me.SimulationPanel.Controls.Add(Me.lblSimDuration)
        Me.SimulationPanel.Controls.Add(Me.Label3)
        Me.SimulationPanel.Controls.Add(Me.Label6)
        Me.SimulationPanel.Controls.Add(Me.Label2)
        Me.SimulationPanel.Controls.Add(Me.Label1)
        Me.SimulationPanel.Controls.Add(Me.Label7)
        Me.SimulationPanel.Controls.Add(Me.TrackBar1)
        Me.SimulationPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.SimulationPanel.Location = New System.Drawing.Point(148, 581)
        Me.SimulationPanel.Name = "SimulationPanel"
        Me.SimulationPanel.Size = New System.Drawing.Size(846, 74)
        Me.SimulationPanel.TabIndex = 24
        Me.SimulationPanel.Visible = False
        '
        'chkPreemptiveMultitasking
        '
        Me.chkPreemptiveMultitasking.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.chkPreemptiveMultitasking.AutoSize = True
        Me.chkPreemptiveMultitasking.ForeColor = System.Drawing.Color.White
        Me.chkPreemptiveMultitasking.Location = New System.Drawing.Point(201, 30)
        Me.chkPreemptiveMultitasking.Name = "chkPreemptiveMultitasking"
        Me.chkPreemptiveMultitasking.Size = New System.Drawing.Size(15, 14)
        Me.chkPreemptiveMultitasking.TabIndex = 29
        Me.chkPreemptiveMultitasking.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.White
        Me.Label13.Location = New System.Drawing.Point(454, 45)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(54, 16)
        Me.Label13.TabIndex = 14
        Me.Label13.Text = "Duration(s):"
        '
        'lblSimDuration
        '
        Me.lblSimDuration.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lblSimDuration.BackColor = System.Drawing.Color.White
        Me.lblSimDuration.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSimDuration.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSimDuration.ForeColor = System.Drawing.Color.Red
        Me.lblSimDuration.Location = New System.Drawing.Point(510, 45)
        Me.lblSimDuration.Name = "lblSimDuration"
        Me.lblSimDuration.Size = New System.Drawing.Size(56, 16)
        Me.lblSimDuration.TabIndex = 21
        Me.lblSimDuration.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(159, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(106, 14)
        Me.Label1.TabIndex = 23
        Me.Label1.Text = "Preemptive multitasking"
        '
        'ProjectPanel
        '
        Me.ProjectPanel.Controls.Add(Me.TabPage1)
        Me.ProjectPanel.Controls.Add(Me.TabPage2)
        Me.ProjectPanel.Dock = System.Windows.Forms.DockStyle.Left
        Me.ProjectPanel.Location = New System.Drawing.Point(0, 37)
        Me.ProjectPanel.Name = "ProjectPanel"
        Me.ProjectPanel.SelectedIndex = 0
        Me.ProjectPanel.Size = New System.Drawing.Size(148, 618)
        Me.ProjectPanel.TabIndex = 27
        Me.ProjectPanel.Visible = False
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.PousTree1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(140, 592)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Pous"
        '
        'PousTree1
        '
        Me.PousTree1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PousTree1.Location = New System.Drawing.Point(4, 5)
        Me.PousTree1.MdiParentForm = Nothing
        Me.PousTree1.Name = "PousTree1"
        Me.PousTree1.Project = Nothing
        Me.PousTree1.Size = New System.Drawing.Size(132, 520)
        Me.PousTree1.TabIndex = 1
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.ResourceTree1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(140, 592)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Resource"
        Me.TabPage2.Visible = False
        '
        'ResourceTree1
        '
        Me.ResourceTree1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ResourceTree1.Location = New System.Drawing.Point(4, 4)
        Me.ResourceTree1.MdiParentForm = Nothing
        Me.ResourceTree1.Name = "ResourceTree1"
        Me.ResourceTree1.Project = Nothing
        Me.ResourceTree1.Size = New System.Drawing.Size(132, 649)
        Me.ResourceTree1.TabIndex = 0
        '
        'ContextMenu4
        '
        Me.ContextMenu4.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem10, Me.MenuItem13, Me.MenuItem12})
        '
        'MenuItem10
        '
        Me.MenuItem10.Index = 0
        Me.MenuItem10.Text = "Add pou instance"
        '
        'MenuItem13
        '
        Me.MenuItem13.Index = 1
        Me.MenuItem13.Text = "Open"
        '
        'MenuItem12
        '
        Me.MenuItem12.Index = 2
        Me.MenuItem12.Text = "Remove"
        '
        'ContextMenu3
        '
        Me.ContextMenu3.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem9, Me.MenuItem7, Me.MenuItem8})
        '
        'MenuItem9
        '
        Me.MenuItem9.Index = 0
        Me.MenuItem9.Text = "Add task"
        '
        'MenuItem7
        '
        Me.MenuItem7.Index = 1
        Me.MenuItem7.Text = "Open"
        '
        'MenuItem8
        '
        Me.MenuItem8.Index = 2
        Me.MenuItem8.Text = "Remove"
        '
        'Splitter1
        '
        Me.Splitter1.Location = New System.Drawing.Point(148, 37)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(4, 544)
        Me.Splitter1.TabIndex = 28
        Me.Splitter1.TabStop = False
        Me.Splitter1.Visible = False
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(994, 655)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.SimulationPanel)
        Me.Controls.Add(Me.ProjectPanel)
        Me.Controls.Add(Me.toolBar1)
        Me.Controls.Add(Me.mdiClient1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.Menu = Me.MainMenu1
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "UniSim G7 Converter"
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SimulationPanel.ResumeLayout(False)
        Me.SimulationPanel.PerformLayout()
        Me.ProjectPanel.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub 'InitializeComponent
#End Region

    ' Imposta la barra del titolo del programma in base alla versione di UniSim
    ' in uso e all'eventuale progetto aperto
    Private Sub SetTitleBar()
        Try
            Dim title As String = UniSimVersion.VersionInfo.PrintableDescriptionForTool()
            If Not (m_Project Is Nothing) Then
                title &= (" - " & m_Project.ContentHeader.name)
                If Not (m_LastFileName Is Nothing) Then
                    If m_LastFileName <> "" Then _
                        title &= (" (saved as " & Path.GetFileName(m_LastFileName) & ")")
                End If
            End If
            Me.Text = title
        Catch ex As Exception
            Me.Text = "UniSim" ' fallback in caso di errore
        End Try
    End Sub

    Sub DisableSplashForm(ByVal Qual As Object)
        'Evento generato allo scadere del timer
        m_DisposeSplashForm = True
        m_TimerSplashForm.Dispose()

    End Sub

    'Funzioni di gestione progetto
    Private Sub NewProject()
        'Controlla se c'è un progeto aperto
        If Not IsNothing(m_Project) Then
            Dim Res As DialogResult = CloseSub()
            If Res = Windows.Forms.DialogResult.Cancel Then
                Exit Sub
            End If
        End If

        If CreateNewProject() Then
            'E' creato il progetto
            'Impostazioni della finestra
            RefreshPouPanel()
            RefreshResourceTree()
            RefreshMenuToolbar()
            ShowProjectPanel(True)
            Call SetTitleBar()

            ' Onora la preferenza di creare una nuova lista di variabili globali per ogni
            ' progetto (prima di qui non può farlo perchè ResourceTree1 non dispone di un
            ' puntatore alla risorsa)
            If Preferences.GetBoolean("AutoMakeGlobVarList", False) Then _
                ResourceTree1.AddGlobalVariablesList("shared", False)

        End If
    End Sub
    Private Function CreateNewProject() As Boolean
        Dim projectDlg As New projectDialogForm
        Dim ResultDialog As DialogResult = projectDlg.ShowDialog()
        If ResultDialog = Windows.Forms.DialogResult.OK Then
            'Se è stato premuto OK
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
            CreateProject()
            m_Project.ContentHeader.name = projectDlg.m_name
            m_Project.ContentHeader.version = projectDlg.m_version
            m_Project.ContentHeader.organization = projectDlg.m_organization
            m_Project.ContentHeader.author = projectDlg.m_author
            m_Project.ContentHeader.language = projectDlg.m_language
            m_Project.ContentHeader.comment = projectDlg.m_Comment
            CreateNewProject = True
        End If
        projectDlg.Dispose()
        m_LastFileName = ""
    End Function
    Public Sub CreateProject()
        'm_Simulation = New Simulation
        '////Creazione nuovo progetto
        'Crea il nuovo progetto
        m_Project = New Project

        'Riempie i dati del file header
        FillHeaderFile()

        'Crea l'istanza con la configurazione
        Dim NewConfiguration As New UnisimClassLibrary.Configuration("MainConfiguration", "")

        'Crea la risorsa
        Dim NewResource As New Resource("MainResource", "")
        NewConfiguration.resources.Add(NewResource)
        m_Project.Istances.configurations.Add(NewConfiguration)
        m_Resource = NewResource

        'Comunica alla lista dei pou la lista delle liste delle variabili globali della risorsa
        m_Project.Types.pous.ResourceGlobalVariables = m_Resource.globalVars
        'Comunica la lista dei pous alla risorsa per le funzioni di simulazione
        m_Resource.pouList = m_Project.Types.pous

        'Collega l'handle al SimulatorEngine
        m_SimulatorEngineHandles = m_Resource.SimulatorEngine
        m_SimulatorEngineHandles.PreemptiveScheduling = chkPreemptiveMultitasking.Checked

        '////Controllo grafico del Simulation
        'Collega i pannelli di controllo delle variabili con la memoria del Simulation
        m_SimulationState = "STOP"

    End Sub

    Public Sub CompleteXmlProject()
        'Controlla se esiste la prima configurazione con la prima risorsa, altrimenti le crea
        If m_Project.Istances.configurations.Count = 0 Then
            'Crea la configurazione e la risorsa e le aggiunge
            Dim NewConfiguration As New UnisimClassLibrary.Configuration("MainConfiguration", "Single configuration of this project")
            m_Project.Istances.configurations.Add(NewConfiguration)
            'Crea la risorsa
            Dim NewResource As New Resource("MainResource", "Single resource of this project")
            NewConfiguration.resources.Add(NewResource)
            m_Resource = NewResource
        Else
            'Se la configurazione esiste controlla se esiste la prima risorsa
            If m_Project.Istances.configurations(0).resources.Count = 0 Then
                'Crea la risorsa
                Dim NewResource As New Resource("MainResource", "Single resource of this project")
                m_Project.Istances.configurations(0).resources.Add(NewResource)
                m_Resource = NewResource
            Else
                'Imposta il riferimento alla prima risorsa
                m_Resource = m_Project.Istances.configurations(0).resources(0)
            End If
        End If

        'Comunica alla lista dei pou la lista delle liste delle variabili globali della risorsa
        m_Project.Types.pous.ResourceGlobalVariables = m_Resource.globalVars

        'Risolve i riferimenti dei nomi di variabili nella azioni e nelle condizioni
        m_Project.Types.pous.ResolveVariablesLinks()

        'Comunica la lista dei pous alla risorsa per le funzioni di simulazione
        m_Resource.pouList = m_Project.Types.pous

        'Risolve i riferimenti dei nomi dei pou nei pouInstance
        m_Project.Istances.configurations(0).resources(0).ResolvePouInstanceLinks()

        'Collega l'handle al SimulatorEngine
        m_SimulatorEngineHandles = m_Resource.SimulatorEngine
        m_SimulatorEngineHandles.PreemptiveScheduling = chkPreemptiveMultitasking.Checked

        m_SimulationState = "STOP"

        Dim warnings As List(Of ProjectIncompatibility) = _
            UniSimVersion.VersionInfo.TestProjectCompatibility(m_Project.fileHeader)

        For Each warning As ProjectIncompatibility In warnings
            Dim msgIcon As MessageBoxIcon
            If warning.Level = IncompatibilitySeriousnessLevel.Information Then msgIcon = MessageBoxIcon.Information
            If warning.Level = IncompatibilitySeriousnessLevel.Warning Then msgIcon = MessageBoxIcon.Warning
            If warning.Level = IncompatibilitySeriousnessLevel.Error Then msgIcon = MessageBoxIcon.Error
            MessageBox.Show("Problem: " + warning.Description + vbCrLf + "Solution: " + warning.Solution, _
                UniSimVersion.VersionInfo.PrintableDescriptionForTool(), _
                MessageBoxButtons.OK, msgIcon)
        Next

    End Sub

    Private Sub CloseDocument()
        'Chiude tutti i form aperti
        For Each Fr As Form In Me.MdiChildren
            Fr.Dispose()
        Next Fr

        'Distrugge il documento
        m_Project.DisposeMe()
        m_Project = Nothing
        'Distrugge il simulatore
        'm_Simulation.DisposeMe()
        'm_Simulation = Nothing

        Call SetTitleBar()
        RefreshMenuToolbar()
        CloseSimulation()
        ShowProjectPanel(False)

        m_LastFileName = ""

    End Sub
    Private Function CloseSub() As DialogResult
        If (m_Project Is Nothing) Then Return Windows.Forms.DialogResult.Yes
        Dim Res As DialogResult = MsgBox("Save changes in " & m_Project.ContentHeader.name & "?", MsgBoxStyle.Question Or MessageBoxButtons.YesNoCancel, UniSimVersion.VersionInfo.PrintableDescriptionForTool())
        If Res = Windows.Forms.DialogResult.Cancel Then Return Res
        If Res = Windows.Forms.DialogResult.Yes Then xmlExport()
        CloseDocument()
        Return Res
    End Function
    Private Function SetProjectInformation() As Boolean
        'Se è stato premuto OK restituisce TRUE
        Dim projectDlg As New projectDialogForm

        If Not IsNothing(m_Project) Then
            projectDlg.m_name = m_Project.ContentHeader.name
            projectDlg.m_version = m_Project.ContentHeader.version
            projectDlg.m_organization = m_Project.ContentHeader.organization
            projectDlg.m_author = m_Project.ContentHeader.author
            projectDlg.m_language = m_Project.ContentHeader.language
            projectDlg.m_Comment = m_Project.ContentHeader.comment
        Else
            projectDlg.m_name = "New Project"
        End If
        Dim ResultDialog As DialogResult = projectDlg.ShowDialog()
        If ResultDialog = Windows.Forms.DialogResult.OK Then
            'Se è stato premuto OK
            RefreshMenuToolbar()
            m_Project.ContentHeader.name = projectDlg.m_name
            m_Project.ContentHeader.version = projectDlg.m_version
            m_Project.ContentHeader.organization = projectDlg.m_organization
            m_Project.ContentHeader.author = projectDlg.m_author
            m_Project.ContentHeader.language = projectDlg.m_language
            m_Project.ContentHeader.comment = projectDlg.m_Comment
            SetProjectInformation = True
        End If
        projectDlg.Dispose()
    End Function
    Private Sub FillHeaderFile()
        m_Project.fileHeader.productName = ApplicationProductName
        m_Project.fileHeader.productVersion = ApplicationProductVersion
        m_Project.fileHeader.productRelease = ApplicationRelease
        m_Project.fileHeader.companyName = ApplicationCompanyName
        m_Project.fileHeader.companyURL = ApplicationCompanyURL
        m_Project.fileHeader.creationDateTime = ApplicationCreationDateTime
        m_Project.fileHeader.contentDescription = ApplicationContentDescription
    End Sub
    Private Sub Print()
        Dim PrintOptionFr As New PrintOptionForm(m_Project)
        PrintOptionFr.ShowDialog()
    End Sub
    Private Sub PrintPreview()
        Try
            Dim PrevDialog As New PrintPreviewDialog
            PrevDialog.Document = PrintDoc
            PrevDialog.Size = New System.Drawing.Size(600, 329)
            PrevDialog.ShowDialog()
        Catch Ex As System.Exception
            MsgBox(Ex.Message)
        End Try
    End Sub
    Private Sub AboutHelp()
        Try
            Dim AboutDialog As New AboutDialogForm
            AboutDialog.ShowDialog()
        Catch Ex As System.Exception
            MsgBox(Ex.Message)
        End Try
    End Sub
    Private Enum HelpLanguage
        Italian
        English
    End Enum
    Private Sub ShowHelpTopics(ByVal hl As HelpLanguage)
        Dim helpPath As String = "UniSim Help" & IIf(hl = HelpLanguage.Italian, "", " EN")
        Try
            Dim sInfo As New ProcessStartInfo(IO.Path.Combine(IO.Path.Combine( _
                System.Windows.Forms.Application.StartupPath, helpPath), "default.htm"))
            Process.Start(sInfo)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub Tile()
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub
    Private Sub Cascade()
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub
    Private Sub ExitApp()
        Dim Res As DialogResult = CloseSub()
        If Res = Windows.Forms.DialogResult.No Or Res = Windows.Forms.DialogResult.Yes Then
            'Chiude se non è State Cancelto il comando durante il salvataggio
            'Environment.Exit(0)
            'Me.Enabled = False
            Me.Visible = False

        End If
    End Sub
    Private Sub ClosingMainAppHander(ByVal Sender As [Object], ByVal e As CancelEventArgs) 'Gestisce MyBase.CloLeftgMainAppHander
        Me.ExitApp()
    End Sub 'CloLeftgMainAppHander

    'Funzioni di XML import/export progetto
    Private Sub xmlImport()
        'Controlla se è già aperto un altro
        If Not IsNothing(m_Project) Then
            Dim Res As DialogResult = CloseSub()
            If Res = Windows.Forms.DialogResult.Cancel Then
                Exit Sub
            End If
        End If

        'Apre la finestra di dialogo per l'apeture del file xml
        Dim OpenDialog As New OpenFileDialog
        OpenDialog.Filter = "File " & ApplicationFileXMLExtension & " (*." & ApplicationFileXMLExtension & ")|*." & ApplicationFileXMLExtension & "|Tutti i file (*.*)|*.*"
        OpenDialog.FileName = ""
        OpenDialog.DefaultExt = "." & ApplicationFileXMLExtension
        OpenDialog.CheckFileExists = True
        OpenDialog.CheckPathExists = True
        Dim ResultDialog As DialogResult = OpenDialog.ShowDialog()
        If ResultDialog = Windows.Forms.DialogResult.OK Then
            'Se è stato selezionato OK controlla se non ci sono errori
            'qual è l'utilità di far selezionare solo file .xml?
            'If Not OpenDialog.FileName.ToLower(Globalization.CultureInfo.InvariantCulture).EndsWith("." & ApplicationFileXMLExtension) Then
            'MsgBox("Bad format file!", MsgBoxStyle.OkOnly)
            'Else
            CreateNewProjectFromXmlFile(OpenDialog.FileName)
            'End If
        End If
    End Sub
    Private Sub CreateNewProjectFromXmlFile(ByVal FileName As String)
        'Controlla se il file schema esiste
        If Not File.Exists(XmlSchemaPath) Then
            MsgBox("Schema file missing. UniSim cannot load projects without this file." + vbCrLf + "Reinstall UniSim or obtain a copy of TC6_XML_V10.xsd and store it into " & XmlSchemaPath, _
             MsgBoxStyle.Critical, UniSimVersion.VersionInfo.PrintableDescriptionForTool())
            Exit Sub
        End If
        'Crea il nuovo XMLProjectManager
        m_XMLprojectManager = New XMLProjectManager
        'Memorizza il contenuto del file in ImportString
        Dim ImportString As String = ""
        'Legge il progetto xml dal file tramite l'XMLProjectManager, ottiene false se c'è un errore di aperture
        If m_XMLprojectManager.OpenXmlFile(FileName, ImportString) Then

            m_LastFileName = FileName

            'Importa il file


            '////Barra di progresso
            Dim ImportingProgress As New ProgressForm
            ImportingProgress.TopMost = True
            ImportingProgress.Show()
            'Crea il nuovo progetto
            m_Project = New Project

            '////Incrementa la barra di progresso
            ImportingProgress.NextStep()
            '////

            'Se il file non contiene un progetto valido ottiene false
            Dim Result As Boolean = m_XMLprojectManager.xmlImport(ImportString, _
                XmlSchemaPath, m_Project, Not (Preferences.GetBoolean("NoProjValidate", _
                                                False)))

            '////Incrementa la barra di progresso
            ImportingProgress.NextStep()
            '////

            ' Il progetto non è valido secondo le specifiche IEC 61131-3
            ' Chiediamo all'utente se intende procedere nell'importazione (avvisando
            ' che UniSim potrebbe andare in crash ecc ecc) o fermare il processo
            If Not Result Then
                'Distrugge il progetto
                ' m_Project = Nothing
                'La politica di UniSim è il "consenso informato"
                Dim ErrorsList As New StringBuilder()
                For Each S As String In m_XMLprojectManager.ErrorsList
                    ErrorsList.AppendLine(S)
                    ErrorsList.AppendLine()
                Next S

                ImportingProgress.Close()
                ImportingProgress.Dispose()

                Dim errForm As New OpenErrorsForm(ErrorsList.ToString())
                Dim wantToGoOn As DialogResult = errForm.ShowDialog()
                If wantToGoOn = Windows.Forms.DialogResult.No Then
                    m_Project = Nothing
                    Return
                Else
                    m_Project = New Project
                    m_XMLprojectManager.xmlImport(ImportString, XmlSchemaPath, m_Project, False)
                End If
            End If
            'Else    'Progetto valido e caricato
            'Completa il progetto
            CompleteXmlProject()

            '////Incrementa la barra di progresso
            ImportingProgress.NextStep()
            '////

            'Aggiorna gli elementi del form
            RefreshPouPanel()
            RefreshResourceTree()

            '////Incrementa la barra di progresso
            ImportingProgress.NextStep()
            '////

            RefreshMenuToolbar()
            ShowProjectPanel(True)
            Call SetTitleBar()
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

            '////Incrementa la barra di progresso
            ImportingProgress.NextStep()
            '////

            ImportingProgress.Close()
            ImportingProgress.Dispose()
            'End If
        Else
            'C'è ststo un errore di apertura del file
            MsgBox( _
    "The file could not be opened and read-in. This may mean that the file is" + vbCrLf + _
    "not valid XML or does not exist. Please check the file with a text editor" + vbCrLf + _
    "and try again.", MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, UniSimVersion.VersionInfo.PrintableDescriptionForTool())
            m_XMLprojectManager = Nothing
        End If

    End Sub

    ' Regole per chiedere o meno un nome con cui salvare
    ' 1) Se AskForFileName = True chiedi sempre
    ' 2) Se m_LastFileName = "" chiedi
    ' 3) Altrimenti non chiedere e usa m_LastFileName
    Private Sub xmlExport(Optional ByVal AskForFileName As Boolean = False)
        If Not IsNothing(m_Project) Then
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
            m_XMLprojectManager = New XMLProjectManager
            'Memorizza il contenuto xml in ExportString
            Dim ExportString As StringBuilder = m_XMLprojectManager.xmlExport(m_Project)
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
            If AskForFileName OrElse (m_LastFileName = "") Then
                Dim SaveDialog As New SaveFileDialog
                SaveDialog.Filter = "File " & ApplicationFileXMLExtension & " (*." & ApplicationFileXMLExtension & ")|*." & ApplicationFileXMLExtension & "|Tutti i file (*.*)|*.*"
                SaveDialog.DefaultExt = "." & ApplicationFileXMLExtension
                SaveDialog.FileName = m_Project.ContentHeader.name & "." & ApplicationFileXMLExtension
                Dim ResultDialog As DialogResult = SaveDialog.ShowDialog()
                If ResultDialog <> Windows.Forms.DialogResult.OK Then Exit Sub
                m_LastFileName = SaveDialog.FileName
            End If
            If Not m_XMLprojectManager.WriteXmlFile(ExportString, m_LastFileName) Then
                Dim ret As System.Windows.Forms.DialogResult = MsgBox( _
"The project could not be saved. Do you want to try and save the project" + vbCrLf + _
"to a new file or cancel the save process?", MsgBoxStyle.Question Or MsgBoxStyle.RetryCancel, _
UniSimVersion.VersionInfo.PrintableDescriptionForTool())
                If ret = Windows.Forms.DialogResult.Retry Then Call xmlExport(True)
            End If
        End If
        RefreshMenuToolbar()
        m_XMLprojectManager = Nothing
        Me.SetTitleBar()
    End Sub
    Private Sub XmlView()
        'Visualizza il progetto in formato XML
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        m_XMLprojectManager = New XMLProjectManager
        'Memorizza il contenuto xml in ExportString
        Dim ExportString As StringBuilder = m_XMLprojectManager.xmlExport(m_Project)
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        'Visualizza il contenuto
        Dim ExportDialog As New xmlProjectViewForm
        ExportDialog.TextBuffer = ExportString.ToString
        Dim ResultDialog As DialogResult = ExportDialog.ShowDialog()
    End Sub

    'Funzioni di gestione POU
    Private Sub Newpou()
        Dim pouDlg As New pouDialogForm
        Dim ResultDialog As DialogResult = pouDlg.ShowDialog()
        If ResultDialog = Windows.Forms.DialogResult.OK Then
            'Controlla se il nome dell'SFC è già presente
            If Not IsNothing(m_Project.Types.pous.FindpouByName(pouDlg.m_Name)) Then
                MsgBox("POU '" & pouDlg.m_Name & "' already exists!")
            Else
                'Crea il nuovo POU e lo aggiunge alla lista
                Dim Newpou As pou = m_Project.Types.pous.AddPou(pouDlg.m_Name, "", _
                    pouDlg.m_pouType, pouDlg.m_bodyType)

                ' Se stiamo creando una funzione obbliga l'utente a definire una uscita
                ' Per convenzione in UniSim l'unica uscita di una funzione ha nome OUT
                If pouDlg.m_pouType = EnumPouType.Function Then
                    Dim varDialog As New VariableDialogForm()
                    varDialog.BlockNameTo("OUT")
                    varDialog.ShowDialog() ' stiamo usando forceOk quindi è sicuro che l'utente dirà sì
                    Newpou.PouInterface.outputVars.CreateAndAddVariable("OUT", "", _
                        "", varDialog.m_InitialValue, varDialog.m_Type)
                End If

                'Crea e visualizza il relativo form
                If pouDlg.m_bodyType = EnumBodyType.tSFC Then
                    'crea e visualizza formSFC
                    Dim FrSfcTemp As New FormSfc(Newpou, Newpou.Body, True)
                    FrSfcTemp.MdiParent = Me
                    FrSfcTemp.Show()
                    RefreshPouPanel()
                ElseIf pouDlg.m_bodyType = EnumBodyType.tLD Then
                    'crea e visualizza formLADDER
                    'dopo si devono vedere i parametri di scambio....
                    Dim FrLadderTemp As New FormLadder(Newpou, Newpou.Body, True)  '..anche se non va dichiarato qui
                    FrLadderTemp.MdiParent = Me
                    FrLadderTemp.Show()
                    RefreshPouPanel()

                ElseIf pouDlg.m_bodyType = EnumBodyType.tFBD Then
                    Dim FrFbdTemp As New FormFbd(Newpou, Newpou.Body, True)
                    FrFbdTemp.MdiParent = Me
                    If Newpou.PouType = EnumPouType.Function AndAlso _
                        Preferences.GetBoolean("AutoMakeOUT", False) Then
                        Newpou.Body.ReadFBD.AddAndDrawGraphicalVariable(Newpou.PouInterface.outputVars.FindVariableByName("OUT"), _
                             GraphicalVariableType.Output, New Drawing.Point(100, 100), New Drawing.Size(44, 44), 0)
                    End If
                    FrFbdTemp.Show()
                    RefreshPouPanel()
                End If

                ' Usa gli stessi codici numerici della preferenza "ActionOnNewPOU"
                Dim newPOUBehaviour As Integer = pouDlg.WhatToCreate
                Select Case newPOUBehaviour
                    Case 1
                        Me.ResourceTree1.AddPouInstance(Newpou)
                        Me.ResourceTree1.RefreshTreeStruct()
                    Case 2
                        Me.ResourceTree1.AddTask(Newpou)
                        Me.ResourceTree1.RefreshTreeStruct()
                    Case 3
                End Select

            End If
        End If
        pouDlg.Dispose()
        RefreshMenuToolbar()
    End Sub
    Private Sub RemovePOU(ByRef RefPou As pou)
        If MsgBox("Delete POU " & RefPou.Name & "?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            'Rimuove il POU dalla lista
            m_Project.Types.pous.RemovePou(RefPou)
            RefreshPouPanel()
            RefreshMenuToolbar()
        End If
    End Sub
    Private Sub ShowBody(ByRef RefPou As pou)
        'Se è in stato di simulazione fa partire il monitor
        'Controlla se il form esiste già
        Dim Find As Boolean
        For Each Fr As Form In Me.MdiChildren
            If Fr.GetType.Name = "FormSfc" Then
                Dim FrSfc As FormSfc = Fr
                If FrSfc.ReadBody Is RefPou.Body Then
                    Fr.Activate()
                    Find = True
                    Exit For
                End If
            End If
        Next Fr
        'Se non lo ha trovato lo crea e lo visualizza
        If Not Find Then
            Dim EditingFlag As Boolean = True
            'Visualizza il form
            Dim FrSfcTemp As New FormSfc(RefPou, RefPou.Body, EditingFlag)
            FrSfcTemp.MdiParent = Me
            FrSfcTemp.Show()
        End If
    End Sub

    Private Sub SetPOUInfo(ByRef RefPou As pou)
        Dim pouDialog As New pouDialogForm
        pouDialog.m_Name = RefPou.Name
        Dim ResultDialog As DialogResult = pouDialog.ShowDialog()
        If ResultDialog = Windows.Forms.DialogResult.OK Then
            'Se il nome è cambiato controlla se un pou con lo stesso nome esiste già
            If RefPou.Name <> pouDialog.m_Name Then
                If Not IsNothing(m_Project.Types.pous.FindpouByName(pouDialog.m_Name)) Then
                    MsgBox("Pou " & pouDialog.m_Name & " already exist!")
                Else
                    'Aggiorna i dati
                    RefPou.Name = pouDialog.m_Name
                    RefreshPouPanel()
                End If
            Else
                'Non aggiorna il nome
                RefreshPouPanel()
            End If
        End If
        pouDialog.Dispose()
    End Sub
    Private Sub RefreshPouPanel()
        'Collega il pannello all'oggetto m_Project
        PousTree1.Project = m_Project
        PousTree1.RefreshTreeStruct()
    End Sub
    Private Sub ShowProjectPanel(ByVal value As Boolean)
        Splitter1.Visible = value
        ProjectPanel.Visible = value
    End Sub



    'Funzioni di gestione risorsa
    Private Sub RefreshResourceTree()
        'Collega il pannello all'oggetto m_Project
        ResourceTree1.Project = m_Project
        ResourceTree1.RefreshTreeStruct()
    End Sub

    Private Sub ClosingHandler(ByVal Sender As Object, ByVal e As CancelEventArgs) Handles MyBase.Closing
        'Controlla se c'è un documento aperto
        If IsNothing(m_Project) Then
            Exit Sub
        End If
        Dim Res As DialogResult = CloseSub()
        If Res = Windows.Forms.DialogResult.Cancel Then
            'Altrimenti non chiude
            e.Cancel = True
        End If

    End Sub


    'Funzioni simulazione
    Private Sub ShowSimulationPanel()
        ShowSimulationState()
        Me.SuspendLayout()
        SimulationPanel.Visible = True
        SimulationPanel.BringToFront()
        Me.ResumeLayout()
    End Sub
    Private Sub StartSimulation()
        If m_SimulationState = "STOP" Then
            SetSimulationWait()
            m_SimulatorEngineHandles.Start()
            m_SimulationState = "RUN"
        End If
    End Sub
    Private Sub StopSimulation()
        m_SimulatorEngineHandles.StopSimulation()
        m_SimulationState = "STOP"
        Label6.Text = ""
        lblSimDuration.Text = ""
    End Sub
    Private Sub NextSimulationStep()
        m_SimulatorEngineHandles.ExecuteSimulationNextStep()
    End Sub
    Private Sub ResetSimulation()
        m_SimulatorEngineHandles.ResetSimulation()
    End Sub
    Private Sub SetPreemptiveScheduling(ByVal Value As Boolean)
        If m_SimulatorEngineHandles IsNot Nothing Then _
            m_SimulatorEngineHandles.PreemptiveScheduling = Value
    End Sub
    Private Function CloseSimulationPanel() As Boolean
        If m_SimulationState = "STOP" Then
            ResetSimulation()
            CloseSimulationPanel = True
        Else
            If MsgBox("Simulation running!. Terminate?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                StopSimulation()
                ResetSimulation()
                CloseSimulationPanel = True
            End If
        End If
    End Function
    Private Sub ShowSimulationState()
        Label5.Text = m_SimulationState
    End Sub
    Private Sub SetSimulationWait()
        Dim Tempo As Integer = CInt(100000 - TrackBar1.Value)
        'Set Wait nel simulatore
        If Not IsNothing(m_SimulatorEngineHandles) Then
            m_SimulatorEngineHandles.SetWait(Tempo)
        End If
    End Sub
    Private Sub WriteSimulatorEngineCycles(ByVal NumCycles As Integer) Handles m_SimulatorEngineHandles.NewCycles
        If m_SimulationState = "RUN" Then
            Label6.Text = NumCycles
            lblSimDuration.Text = m_SimulatorEngineHandles.SimulationDuration.ToString()
        End If
    End Sub
    Private Sub CloseSimulation()
        SimulationPanel.Visible = False
        Me.MenuItemSimulationSimulation.Checked = False
        'Ripulisce la memoria hardware
    End Sub

    ' Usa un Hydra e alcune ottimizzazioni booleane per rendere il codice molto più coinciso
    Public Sub RefreshMenuToolbar()

        Dim noProject As Boolean = IsNothing(m_Project)
        Dim simOff As Boolean = m_SimulationState = "STOP"

        Me.MenuItemProjectImport.Enabled = Me.m_MenuItemsState(noProject)(MenuItemProjectImport)
        Me.MenuItemProjectExport.Enabled = Me.m_MenuItemsState(noProject)(MenuItemProjectExport)
        Me.MenuItemExportWithName.Enabled = Me.m_MenuItemsState(noProject)(MenuItemExportWithName)
        Me.MenuItemClose.Enabled = Me.m_MenuItemsState(noProject)(MenuItemClose)
        Me.MenuItemXmlView.Enabled = Me.m_MenuItemsState(noProject)(MenuItemXmlView)
        Me.MenuItemPrint.Enabled = Me.m_MenuItemsState(noProject)(MenuItemPrint)
        Me.MenuItemInformationProject.Enabled = Me.m_MenuItemsState(noProject)(MenuItemInformationProject)
        Me.MenuItemPOUsManager.Enabled = Me.m_MenuItemsState(noProject)(MenuItemPOUsManager)
        Me.MenuItemSimulation.Enabled = Me.m_MenuItemsState(noProject)(MenuItemSimulation)
        Me.MenuItemWindow.Enabled = Me.m_MenuItemsState(noProject)(MenuItemWindow)
        Me.MenuItemNewProjectHeader.Enabled = Me.m_MenuItemsState(noProject)(MenuItemNewProjectHeader)

        Me.toolBar1.Buttons(0).Enabled = True
        Me.toolBar1.Buttons(1).Enabled = True
        Me.toolBar1.Buttons(2).Enabled = Not noProject
        Me.toolBar1.Buttons(4).Enabled = Not noProject
        Me.toolBar1.Buttons(6).Enabled = Not noProject
        Me.toolBar1.Buttons(8).Enabled = Not noProject
        Me.toolBar1.Buttons(9).Enabled = Not noProject

        If Not noProject Then
            Me.MenuItemNew.Enabled = simOff
            Me.MenuItemProjectImport.Enabled = simOff
            Me.MenuItemClose.Enabled = simOff
            Me.MenuItemPrint.Enabled = simOff
            Me.toolBar1.Buttons(0).Enabled = simOff
            Me.toolBar1.Buttons(1).Enabled = simOff
        End If

    End Sub

    Private Sub Cascata()
        'Finestre sovrapposte.
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub
    Private Sub TileHorizontal()
        'Finestre affiancate oriz.
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub
    Private Sub TileVertical()
        'Finestre affiancate vert.
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub MainForm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ExitApp()
    End Sub
    Private Sub MainForm_MdiChildActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.MdiChildActivate
        RefreshMenuToolbar()
    End Sub
    Private Sub ToolBar1_ButtonClick(ByVal Sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles toolBar1.ButtonClick
        If e.Button Is NewButton Then
            NewProject()
        ElseIf e.Button Is OpenButton Then
            xmlImport()
        ElseIf e.Button Is SaveButton Then
            xmlExport()
        ElseIf e.Button Is NewPOUButton Then
            Newpou()
        ElseIf e.Button Is RefreshButton Then
            PousTree1.RefreshTreeStruct()
            ResourceTree1.RefreshTreeStruct()
        ElseIf e.Button Is RunTheSimButton Then
            Call ShowPanelAndRunSimulation()
        ElseIf e.Button Is StopTheSimButton Then
            Call StopSimulationAndHidePanel()
        End If

    End Sub 'ToolBar1_ButtonClick
    Private Sub MenuItemNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemNew.Click
        NewProject()
    End Sub
    Private Sub MenuItemClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemClose.Click
        CloseSub()
    End Sub
    Private Sub MenuItemAffOriz_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemAffOriz.Click
        TileVertical()
    End Sub
    Private Sub MenuItemPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemPrint.Click
        Print()
    End Sub
    Private Sub MenuItemExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemExit.Click
        ExitApp()
    End Sub
    Private Sub MenuItemToolbar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemToolbar.Click
        If MenuItemToolbar.Checked = True Then
            MenuItemToolbar.Checked = False
            toolBar1.Visible = False
        Else
            MenuItemToolbar.Checked = True
            toolBar1.Visible = True
        End If
    End Sub
    Private Sub MenuItemCascade_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemCascade.Click
        Cascata()
    End Sub
    Private Sub MenuItemAffVert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemAffVert.Click
        TileHorizontal()
    End Sub
    Private Sub MenuItemHelpTopics_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemHelpTopics.Click
        ShowHelpTopics(HelpLanguage.Italian)
    End Sub
    Private Sub MenuItemAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemAbout.Click
        AboutHelp()
    End Sub
    Private Sub DisplaySplashForm()
        m_FrmSpash = New SplashForm
        Me.Enabled = False
        m_FrmSpash.Show()
    End Sub
    Private Sub DisposeSplashForm()
        Try
            m_FrmSpash.Close()
            m_FrmSpash.Dispose()
        Catch ex As System.Exception

        End Try
    End Sub

    Private Sub MainWindow_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DisplaySplashForm()
        'Callback function and Thread
        Dim m_TimerDelegate As TimerCallback
        m_TimerDelegate = New TimerCallback(AddressOf DisableSplashForm)
        m_TimerSplashForm = New Threading.Timer(m_TimerDelegate, Null, 1400, -1)

        m_DisposeSplashForm = False
        While (Not m_DisposeSplashForm)
            'Do nothing until m_FrmSpash_close becomes true
        End While

        DisposeSplashForm()
        Me.Enabled = True

        RefreshMenuToolbar()
        Me.chkPreemptiveMultitasking.Checked = CBool(m_SimulatorPerformance(Preferences.GetInteger("PerformanceProfile", 2))("UsePreemptiveScheduler"))
        Me.TrackBar1.Value = m_SimulatorPerformance(Preferences.GetInteger("PerformanceProfile", 2))("SimulationPriority")
        Call ParseCommandLine()
    End Sub
    Private Sub ParseCommandLine()
        Dim args() As String = Environment.GetCommandLineArgs()
        For i As Integer = 1 To args.Length - 1
            Me.CreateNewProjectFromXmlFile(args(i))
        Next
    End Sub
    Private Sub MenuItemAffOriz_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        TileVertical()
    End Sub
    Private Sub MenuItemInformationProject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemInformationProject.Click
        SetProjectInformation()
    End Sub
    Private Sub ButtonStartSim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonStartSim.Click
        StartSimulation()
        ShowSimulationState()
        ButtonStartSim.Enabled = False
        ButtonStopSim.Enabled = True
        ButtonStepSim.Enabled = False
        ButtonResetSim.Enabled = False
        RefreshMenuToolbar()
    End Sub
    Private Sub ButtonStepSim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonStepSim.Click
        NextSimulationStep()
        ButtonStartSim.Enabled = True
        ButtonStopSim.Enabled = False
        ButtonStepSim.Enabled = True
        ButtonResetSim.Enabled = True
        RefreshMenuToolbar()
    End Sub
    Private Sub ButtonStopSim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonStopSim.Click
        StopSimulation()
        ShowSimulationState()
        ButtonStartSim.Enabled = True
        ButtonStopSim.Enabled = False
        ButtonStepSim.Enabled = True
        ButtonResetSim.Enabled = True
        RefreshMenuToolbar()
    End Sub
    Private Sub ButtonResetSim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonResetSim.Click
        ResetSimulation()
        ButtonStartSim.Enabled = True
        ButtonStopSim.Enabled = False
        ButtonStepSim.Enabled = True
    End Sub
    Private Sub ButtonChiudiSim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        SetSimulationWait()
    End Sub
    Private Sub MenuItemSimulationSimulation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemSimulationSimulation.Click
        If MenuItemSimulationSimulation.Checked = True Then
            MenuItemSimulationSimulation.Checked = False
            SimulationPanel.Visible = False
        Else
            MenuItemSimulationSimulation.Checked = True
            Me.SuspendLayout()
            SimulationPanel.BringToFront()
            SimulationPanel.Visible = True
            Me.ResumeLayout()
        End If
    End Sub
    Private Sub ButtonSimPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ShowSimulationPanel()
        RefreshMenuToolbar()
    End Sub
    Private Sub ButtonContrPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If CloseSimulationPanel() Then
            ButtonStartSim.Enabled = True
            ButtonStopSim.Enabled = False
            ButtonStepSim.Enabled = True
            SimulationPanel.Visible = False
            RefreshMenuToolbar()
        End If
    End Sub
    Private Sub MenuItemProjectExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemProjectExport.Click
        xmlExport(False)
    End Sub
    Private Sub MenuItemProjectImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemProjectImport.Click
        xmlImport()
    End Sub
    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Newpou()
    End Sub
    Private Sub MenuItem11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem11.Click
        Newpou()
    End Sub
    Private Sub MenuItemXmlView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemXmlView.Click
        XmlView()
    End Sub

    Private Sub ShowPanelAndRunSimulation()
        If Not (MenuItemSimulationSimulation.Checked) Then _
            Call MenuItemSimulationSimulation_Click(Me, New EventArgs())
        If m_SimulationState <> "RUN" Then _
            Call ButtonStartSim_Click(Me, New EventArgs())
    End Sub

    Private Sub StopSimulationAndHidePanel()
        If m_SimulationState <> "STOP" Then _
            Call ButtonStopSim_Click(Me, New EventArgs())
        If MenuItemSimulationSimulation.Checked Then _
            Call MenuItemSimulationSimulation_Click(Me, New EventArgs())
    End Sub

    Private Sub MenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem1.Click
        Call ShowPanelAndRunSimulation()
    End Sub

    Private Sub MenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem3.Click
        Call StopSimulationAndHidePanel()
    End Sub

    Private Sub MenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem4.Click
        If Not (MenuItemSimulationSimulation.Checked) Then _
            Call MenuItemSimulationSimulation_Click(Me, New EventArgs())
        If m_SimulationState = "STOP" Then Call ButtonStepSim_Click(sender, e)
    End Sub

    Private Sub MenuItemExportWithName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemExportWithName.Click
        xmlExport(True)
    End Sub

    ' Mostra la schermata di visualizzazione preferenze
    Private Sub MenuItemEditPrefs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemEditPrefs.Click
        Dim pref As New PreferencesForm()
        pref.ShowDialog()
    End Sub

    Private Sub MenuItemProjPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemProjPanel.Click
        ProjectPanel.Visible = Not (ProjectPanel.Visible)
        MenuItemProjPanel.Checked = ProjectPanel.Visible
    End Sub

    ' Siccome a volte il ProjectPanel viene mostrato a schermo forzatamente conviene
    ' aggiornare lo stato del relativo MenuItem prima di mostrarlo all'utente
    Private Sub MenuItemView_Popup(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemView.Popup
        MenuItemProjPanel.Checked = ProjectPanel.Visible
    End Sub

    Private Sub MenuItemAddPOU_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemAddPOU.Click
        Call Newpou()
    End Sub

    ' Gestisce il drop-down: apre il corpo e le variabili locali di una POU
    ' messa nel tag di un MenuItem (così da evitare le ricerche)
    Private Sub OpenPOUBodyClicked(ByVal sender As System.Object, ByVal e As EventArgs)
        Dim targetPOU As pou = CType(CType(sender, MenuItem).Tag, pou)
        PousTree1.OpenPOUBody(targetPOU)
    End Sub
    Private Sub OpenPOUVarsClicked(ByVal sender As System.Object, ByVal e As EventArgs)
        Dim targetPOU As pou = CType(CType(sender, MenuItem).Tag, pou)
        PousTree1.OpenPOUVarsList(targetPOU)
    End Sub
    Private Sub OpenPOUInputVarsClicked(ByVal sender As System.Object, ByVal e As EventArgs)
        Dim targetPOU As pou = CType(CType(sender, MenuItem).Tag, pou)
        PousTree1.OpenPOUInputVarsList(targetPOU)
    End Sub

    Private Sub toolBar1_ButtonDropDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles toolBar1.ButtonDropDown
        If e.Button Is Me.NewPOUButton Then
            Dim myMenu As New ContextMenu()
            For Each P As pou In m_Project.Types.pous
                Dim pouItem As New MenuItem(P.Name)
                Dim bodyItem As New MenuItem("Body", AddressOf OpenPOUBodyClicked)
                Dim varsItem As New MenuItem("Local variables", AddressOf OpenPOUVarsClicked)
                Dim inputVarsItem As New MenuItem("Input variables", AddressOf OpenPOUInputVarsClicked)
                bodyItem.Tag = P
                varsItem.Tag = P
                inputVarsItem.Tag = P
                pouItem.MenuItems.Add(bodyItem)
                pouItem.MenuItems.Add(varsItem)
                If P.PouType = EnumPouType.Function Then pouItem.MenuItems.Add(inputVarsItem)
                myMenu.MenuItems.Add(pouItem)
            Next
            e.Button.DropDownMenu = myMenu
        End If
    End Sub

    Public Function GetPOUsTree() As UnisimClassLibrary.PousTree Implements UnisimClassLibrary.IUniSimMainWindow.GetPOUsTree
        Return Me.PousTree1
    End Function

    Public Function GetResourceTree() As UnisimClassLibrary.ResourceTree Implements UnisimClassLibrary.IUniSimMainWindow.GetResourceTree
        Return Me.ResourceTree1
    End Function

    Private Sub chkPreemptiveMultitasking_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPreemptiveMultitasking.CheckedChanged
        SetPreemptiveScheduling(chkPreemptiveMultitasking.Checked)
    End Sub

    Private Sub SimulationPanel_VisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimulationPanel.VisibleChanged
        UnisimClassLibrary.Macintoshize(SimulationPanel)
    End Sub

    Public ReadOnly Property ApplicationName() As String Implements UnisimClassLibrary.IUniSimMainWindow.ApplicationName
        Get
            Return "DevApp"
        End Get
    End Property

    Private Sub MenuItemNewProjectHeader_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemNewProjectHeader.Click
        Dim retVal As DialogResult
        retVal = MessageBox.Show( _
"Updating a project's header file is required to avoid compliancy warnings when opening it" + vbCrLf + _
"However, doing this without having truly edited a project to ensure that" + vbCrLf + _
"everything works as expected, may cause issues when transferring the" + vbCrLf + _
"project to a third party unaware of the issues it contains." + vbCrLf + "Do you want to continue?", _
    UniSimVersion.VersionInfo.PrintableDescriptionForTool(), _
    MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If retVal <> Windows.Forms.DialogResult.Yes Then Exit Sub
        FillHeaderFile()
    End Sub

    Private Sub MenuItem6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem6.Click
        ShowHelpTopics(HelpLanguage.English)
    End Sub

End Class

