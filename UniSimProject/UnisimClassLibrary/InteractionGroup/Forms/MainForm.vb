
Imports System
Imports System.IO
Imports System.Text
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Threading
Imports System.Runtime.Serialization.Formatters.Binary


    _

Public Class MainForm
    Inherits System.Windows.Forms.Form

    Private Shared parentWindow As MainForm
    Private components As System.ComponentModel.IContainer

    Private WithEvents m_SimulatorEngineHandles As SimulatorEngine
    Private m_Resource As Resource   'Riferimento all'unica risorsa
    Private m_SimulationState As String

    Private m_XMLprojectManager As XMLProjectManager

    Private m_TimerSplashForm As System.Threading.Timer
    Private m_TimerDelegate As TimerCallback
    Private m_FrmSpash As SplashForm
#Region " Codice generato da Progettazione Windows Form "


    Public Sub New()
        parentWindow = Me
        Me.Text = ProductName
        m_SimulationState = "STOP"
        InitializeComponent()


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
    Private HelpProvider1 As System.Windows.Forms.HelpProvider
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
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
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
    Friend WithEvents MenuItemNewPou As System.Windows.Forms.MenuItem
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
    Friend WithEvents MenuItemPOU As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemNew As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemClose As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemPrint As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemExit As System.Windows.Forms.MenuItem
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GlobalVariablesListBox As System.Windows.Forms.ListBox
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents TasksListBox As System.Windows.Forms.ListBox
    Friend WithEvents PouInstancesListBox As System.Windows.Forms.ListBox
    Friend WithEvents ProjectPanel As System.Windows.Forms.TabControl
    Friend WithEvents PouListBox As System.Windows.Forms.ListBox
    Friend WithEvents ContextMenu2 As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents ContextMenu3 As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuItem7 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem8 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem6 As System.Windows.Forms.MenuItem
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.toolBar1 = New System.Windows.Forms.ToolBar
        Me.NewButton = New System.Windows.Forms.ToolBarButton
        Me.OpenButton = New System.Windows.Forms.ToolBarButton
        Me.SaveButton = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton1 = New System.Windows.Forms.ToolBarButton
        Me.NewPOUButton = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton2 = New System.Windows.Forms.ToolBarButton
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.mdiClient1 = New System.Windows.Forms.MdiClient
        Me.MenuItemAffOriz = New System.Windows.Forms.MenuItem
        Me.MenuItemView = New System.Windows.Forms.MenuItem
        Me.MenuItemToolbar = New System.Windows.Forms.MenuItem
        Me.PrintDoc = New System.Drawing.Printing.PrintDocument
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.MenuItemProject = New System.Windows.Forms.MenuItem
        Me.MenuItemNew = New System.Windows.Forms.MenuItem
        Me.MenuItemProjectImport = New System.Windows.Forms.MenuItem
        Me.MenuItemProjectExport = New System.Windows.Forms.MenuItem
        Me.MenuItemClose = New System.Windows.Forms.MenuItem
        Me.MenuItemInformationProject = New System.Windows.Forms.MenuItem
        Me.MenuItemXmlView = New System.Windows.Forms.MenuItem
        Me.MenuItemPrint = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.MenuItemExit = New System.Windows.Forms.MenuItem
        Me.MenuItemPOU = New System.Windows.Forms.MenuItem
        Me.MenuItemNewPou = New System.Windows.Forms.MenuItem
        Me.MenuItemSimulation = New System.Windows.Forms.MenuItem
        Me.MenuItemSimulationSimulation = New System.Windows.Forms.MenuItem
        Me.MenuItemWindow = New System.Windows.Forms.MenuItem
        Me.MenuItemCascade = New System.Windows.Forms.MenuItem
        Me.MenuItemAffVert = New System.Windows.Forms.MenuItem
        Me.MenuItemHelp = New System.Windows.Forms.MenuItem
        Me.MenuItemHelpTopics = New System.Windows.Forms.MenuItem
        Me.MenuItemStep7 = New System.Windows.Forms.MenuItem
        Me.MenuItemAbout = New System.Windows.Forms.MenuItem
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider
        Me.ContextMenu1 = New System.Windows.Forms.ContextMenu
        Me.MenuItem11 = New System.Windows.Forms.MenuItem
        Me.MenuInteShowPOUBody = New System.Windows.Forms.MenuItem
        Me.MenuItemSetPOUInfo2 = New System.Windows.Forms.MenuItem
        Me.MenuItemRemovePou2 = New System.Windows.Forms.MenuItem
        Me.ButtonStopSim = New System.Windows.Forms.Button
        Me.ButtonStartSim = New System.Windows.Forms.Button
        Me.Label7 = New System.Windows.Forms.Label
        Me.TrackBar1 = New System.Windows.Forms.TrackBar
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.ButtonResetSim = New System.Windows.Forms.Button
        Me.ButtonStepSim = New System.Windows.Forms.Button
        Me.SimulationPanel = New System.Windows.Forms.Panel
        Me.ProjectPanel = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.PouListBox = New System.Windows.Forms.ListBox
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.PouInstancesListBox = New System.Windows.Forms.ListBox
        Me.ContextMenu4 = New System.Windows.Forms.ContextMenu
        Me.MenuItem10 = New System.Windows.Forms.MenuItem
        Me.MenuItem13 = New System.Windows.Forms.MenuItem
        Me.MenuItem12 = New System.Windows.Forms.MenuItem
        Me.TasksListBox = New System.Windows.Forms.ListBox
        Me.ContextMenu3 = New System.Windows.Forms.ContextMenu
        Me.MenuItem9 = New System.Windows.Forms.MenuItem
        Me.MenuItem7 = New System.Windows.Forms.MenuItem
        Me.MenuItem8 = New System.Windows.Forms.MenuItem
        Me.Label24 = New System.Windows.Forms.Label
        Me.Label23 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.GlobalVariablesListBox = New System.Windows.Forms.ListBox
        Me.ContextMenu2 = New System.Windows.Forms.ContextMenu
        Me.MenuItem6 = New System.Windows.Forms.MenuItem
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.MenuItem4 = New System.Windows.Forms.MenuItem
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.Splitter1 = New System.Windows.Forms.Splitter
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SimulationPanel.SuspendLayout()
        Me.ProjectPanel.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'toolBar1
        '
        Me.toolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.NewButton, Me.OpenButton, Me.SaveButton, Me.ToolBarButton1, Me.NewPOUButton, Me.ToolBarButton2})
        Me.toolBar1.ButtonSize = New System.Drawing.Size(24, 24)
        Me.toolBar1.DropDownArrows = True
        Me.toolBar1.ImageList = Me.ImageList1
        Me.toolBar1.Location = New System.Drawing.Point(0, 0)
        Me.toolBar1.Name = "toolBar1"
        Me.toolBar1.ShowToolTips = True
        Me.toolBar1.Size = New System.Drawing.Size(792, 30)
        Me.toolBar1.TabIndex = 1
        '
        'NewButton
        '
        Me.NewButton.ImageIndex = 0
        Me.NewButton.ToolTipText = "New"
        '
        'OpenButton
        '
        Me.OpenButton.ImageIndex = 1
        Me.OpenButton.ToolTipText = "Open"
        '
        'SaveButton
        '
        Me.SaveButton.ImageIndex = 2
        Me.SaveButton.ToolTipText = "Save"
        '
        'ToolBarButton1
        '
        Me.ToolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'NewPOUButton
        '
        Me.NewPOUButton.ImageIndex = 8
        Me.NewPOUButton.ToolTipText = "New Pou"
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
        'mdiClient1
        '
        Me.mdiClient1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.mdiClient1.Location = New System.Drawing.Point(128, 30)
        Me.mdiClient1.Name = "mdiClient1"
        Me.mdiClient1.TabIndex = 0
        '
        'MenuItemAffOriz
        '
        Me.MenuItemAffOriz.Index = 2
        Me.MenuItemAffOriz.Text = "Affianca verticalmente"
        '
        'MenuItemView
        '
        Me.MenuItemView.Index = 3
        Me.MenuItemView.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemToolbar})
        Me.MenuItemView.MergeOrder = 5
        Me.MenuItemView.Text = "View"
        '
        'MenuItemToolbar
        '
        Me.MenuItemToolbar.Checked = True
        Me.MenuItemToolbar.Index = 0
        Me.MenuItemToolbar.Text = "Toolbar"
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemProject, Me.MenuItemPOU, Me.MenuItemSimulation, Me.MenuItemView, Me.MenuItemWindow, Me.MenuItemHelp})
        '
        'MenuItemProject
        '
        Me.MenuItemProject.Index = 0
        Me.MenuItemProject.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemNew, Me.MenuItemProjectImport, Me.MenuItemProjectExport, Me.MenuItemClose, Me.MenuItemInformationProject, Me.MenuItemXmlView, Me.MenuItemPrint, Me.MenuItem2, Me.MenuItemExit})
        Me.MenuItemProject.MergeOrder = 1
        Me.MenuItemProject.Text = "Project"
        '
        'MenuItemNew
        '
        Me.MenuItemNew.Index = 0
        Me.MenuItemNew.Text = "New"
        '
        'MenuItemProjectImport
        '
        Me.MenuItemProjectImport.Index = 1
        Me.MenuItemProjectImport.Text = "Open"
        '
        'MenuItemProjectExport
        '
        Me.MenuItemProjectExport.Index = 2
        Me.MenuItemProjectExport.Text = "Save"
        '
        'MenuItemClose
        '
        Me.MenuItemClose.Index = 3
        Me.MenuItemClose.Text = "Close"
        '
        'MenuItemInformationProject
        '
        Me.MenuItemInformationProject.Index = 4
        Me.MenuItemInformationProject.Text = "Project information"
        '
        'MenuItemXmlView
        '
        Me.MenuItemXmlView.Index = 5
        Me.MenuItemXmlView.Text = "View XML Format"
        '
        'MenuItemPrint
        '
        Me.MenuItemPrint.Index = 6
        Me.MenuItemPrint.Text = "Print..."
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 7
        Me.MenuItem2.Text = "-"
        '
        'MenuItemExit
        '
        Me.MenuItemExit.Index = 8
        Me.MenuItemExit.Text = "Exit"
        '
        'MenuItemPOU
        '
        Me.MenuItemPOU.Index = 1
        Me.MenuItemPOU.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemNewPou})
        Me.MenuItemPOU.MergeOrder = 2
        Me.MenuItemPOU.Text = "POU"
        '
        'MenuItemNewPou
        '
        Me.MenuItemNewPou.Index = 0
        Me.MenuItemNewPou.Text = "Add POU"
        '
        'MenuItemSimulation
        '
        Me.MenuItemSimulation.Index = 2
        Me.MenuItemSimulation.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemSimulationSimulation})
        Me.MenuItemSimulation.MergeOrder = 3
        Me.MenuItemSimulation.Text = "Simulation"
        '
        'MenuItemSimulationSimulation
        '
        Me.MenuItemSimulationSimulation.Index = 0
        Me.MenuItemSimulationSimulation.Text = "Simulation panel"
        '
        'MenuItemWindow
        '
        Me.MenuItemWindow.Index = 4
        Me.MenuItemWindow.MdiList = True
        Me.MenuItemWindow.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemCascade, Me.MenuItemAffVert, Me.MenuItemAffOriz})
        Me.MenuItemWindow.MergeOrder = 6
        Me.MenuItemWindow.Text = "Windows"
        '
        'MenuItemCascade
        '
        Me.MenuItemCascade.Index = 0
        Me.MenuItemCascade.Text = "Sovrapponi"
        '
        'MenuItemAffVert
        '
        Me.MenuItemAffVert.Index = 1
        Me.MenuItemAffVert.Text = "Affianca orizzontalmente"
        '
        'MenuItemHelp
        '
        Me.MenuItemHelp.Index = 5
        Me.MenuItemHelp.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemHelpTopics, Me.MenuItemStep7, Me.MenuItemAbout})
        Me.MenuItemHelp.MergeOrder = 7
        Me.MenuItemHelp.Text = "?"
        '
        'MenuItemHelpTopics
        '
        Me.MenuItemHelpTopics.Index = 0
        Me.MenuItemHelpTopics.Text = "Guida in linea"
        '
        'MenuItemStep7
        '
        Me.MenuItemStep7.Index = 1
        Me.MenuItemStep7.Text = "-"
        '
        'MenuItemAbout
        '
        Me.MenuItemAbout.Index = 2
        Me.MenuItemAbout.Text = "About..."
        '
        'HelpProvider1
        '
        Me.HelpProvider1.HelpNamespace = "..\Scribble\help\scribble.chm"
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
        Me.ButtonStopSim.Location = New System.Drawing.Point(348, 48)
        Me.ButtonStopSim.Name = "ButtonStopSim"
        Me.ButtonStopSim.Size = New System.Drawing.Size(80, 28)
        Me.ButtonStopSim.TabIndex = 19
        Me.ButtonStopSim.TabStop = False
        Me.ButtonStopSim.Text = "Stop"
        '
        'ButtonStartSim
        '
        Me.ButtonStartSim.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ButtonStartSim.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonStartSim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonStartSim.Location = New System.Drawing.Point(268, 48)
        Me.ButtonStartSim.Name = "ButtonStartSim"
        Me.ButtonStartSim.Size = New System.Drawing.Size(80, 28)
        Me.ButtonStartSim.TabIndex = 1
        Me.ButtonStartSim.TabStop = False
        Me.ButtonStartSim.Text = "Start"
        '
        'Label7
        '
        Me.Label7.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(96, 56)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(66, 16)
        Me.Label7.TabIndex = 23
        Me.Label7.Text = "Priority"
        '
        'TrackBar1
        '
        Me.TrackBar1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TrackBar1.LargeChange = 1
        Me.TrackBar1.Location = New System.Drawing.Point(176, 48)
        Me.TrackBar1.Maximum = 100000
        Me.TrackBar1.Name = "TrackBar1"
        Me.TrackBar1.Size = New System.Drawing.Size(84, 42)
        Me.TrackBar1.TabIndex = 22
        Me.TrackBar1.TabStop = False
        Me.TrackBar1.TickFrequency = 10000
        Me.TrackBar1.Value = 2000
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(96, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 16)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "Simulation"
        '
        'Label6
        '
        Me.Label6.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label6.BackColor = System.Drawing.Color.White
        Me.Label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label6.ForeColor = System.Drawing.Color.Red
        Me.Label6.Location = New System.Drawing.Point(364, 12)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(64, 16)
        Me.Label6.TabIndex = 21
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label5.BackColor = System.Drawing.Color.White
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(240, 12)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(64, 16)
        Me.Label5.TabIndex = 20
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(180, 12)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 16)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "State:"
        '
        'Label9
        '
        Me.Label9.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(228, 32)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(32, 16)
        Me.Label9.TabIndex = 25
        Me.Label9.Text = "Max"
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(312, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(52, 16)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Cycles/s:"
        '
        'Label8
        '
        Me.Label8.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(180, 32)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(24, 16)
        Me.Label8.TabIndex = 24
        Me.Label8.Text = "Min"
        '
        'ButtonResetSim
        '
        Me.ButtonResetSim.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ButtonResetSim.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonResetSim.Enabled = False
        Me.ButtonResetSim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonResetSim.Location = New System.Drawing.Point(512, 48)
        Me.ButtonResetSim.Name = "ButtonResetSim"
        Me.ButtonResetSim.Size = New System.Drawing.Size(72, 28)
        Me.ButtonResetSim.TabIndex = 28
        Me.ButtonResetSim.TabStop = False
        Me.ButtonResetSim.Text = "Reset"
        '
        'ButtonStepSim
        '
        Me.ButtonStepSim.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ButtonStepSim.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonStepSim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ButtonStepSim.Location = New System.Drawing.Point(432, 48)
        Me.ButtonStepSim.Name = "ButtonStepSim"
        Me.ButtonStepSim.Size = New System.Drawing.Size(76, 28)
        Me.ButtonStepSim.TabIndex = 26
        Me.ButtonStepSim.TabStop = False
        Me.ButtonStepSim.Text = "Step"
        '
        'SimulationPanel
        '
        Me.SimulationPanel.BackColor = System.Drawing.Color.Navy
        Me.SimulationPanel.Controls.Add(Me.ButtonStepSim)
        Me.SimulationPanel.Controls.Add(Me.ButtonStartSim)
        Me.SimulationPanel.Controls.Add(Me.ButtonStopSim)
        Me.SimulationPanel.Controls.Add(Me.Label4)
        Me.SimulationPanel.Controls.Add(Me.Label8)
        Me.SimulationPanel.Controls.Add(Me.Label5)
        Me.SimulationPanel.Controls.Add(Me.ButtonResetSim)
        Me.SimulationPanel.Controls.Add(Me.Label3)
        Me.SimulationPanel.Controls.Add(Me.Label6)
        Me.SimulationPanel.Controls.Add(Me.Label2)
        Me.SimulationPanel.Controls.Add(Me.Label9)
        Me.SimulationPanel.Controls.Add(Me.Label7)
        Me.SimulationPanel.Controls.Add(Me.TrackBar1)
        Me.SimulationPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.SimulationPanel.Location = New System.Drawing.Point(124, 470)
        Me.SimulationPanel.Name = "SimulationPanel"
        Me.SimulationPanel.Size = New System.Drawing.Size(668, 83)
        Me.SimulationPanel.TabIndex = 24
        Me.SimulationPanel.Visible = False
        '
        'ProjectPanel
        '
        Me.ProjectPanel.Controls.Add(Me.TabPage1)
        Me.ProjectPanel.Controls.Add(Me.TabPage2)
        Me.ProjectPanel.Dock = System.Windows.Forms.DockStyle.Left
        Me.ProjectPanel.Location = New System.Drawing.Point(0, 30)
        Me.ProjectPanel.Name = "ProjectPanel"
        Me.ProjectPanel.SelectedIndex = 0
        Me.ProjectPanel.Size = New System.Drawing.Size(124, 523)
        Me.ProjectPanel.TabIndex = 27
        Me.ProjectPanel.Visible = False
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.PouListBox)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(116, 497)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Pous type"
        '
        'PouListBox
        '
        Me.PouListBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PouListBox.ContextMenu = Me.ContextMenu1
        Me.PouListBox.DisplayMember = "name"
        Me.PouListBox.IntegralHeight = False
        Me.PouListBox.Location = New System.Drawing.Point(4, 4)
        Me.PouListBox.Name = "PouListBox"
        Me.PouListBox.Size = New System.Drawing.Size(108, 488)
        Me.PouListBox.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.PouInstancesListBox)
        Me.TabPage2.Controls.Add(Me.TasksListBox)
        Me.TabPage2.Controls.Add(Me.Label24)
        Me.TabPage2.Controls.Add(Me.Label23)
        Me.TabPage2.Controls.Add(Me.Label1)
        Me.TabPage2.Controls.Add(Me.GlobalVariablesListBox)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(116, 497)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Resource"
        Me.TabPage2.Visible = False
        '
        'PouInstancesListBox
        '
        Me.PouInstancesListBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PouInstancesListBox.ContextMenu = Me.ContextMenu4
        Me.PouInstancesListBox.DisplayMember = "Name"
        Me.PouInstancesListBox.Location = New System.Drawing.Point(4, 332)
        Me.PouInstancesListBox.Name = "PouInstancesListBox"
        Me.PouInstancesListBox.Size = New System.Drawing.Size(108, 160)
        Me.PouInstancesListBox.TabIndex = 11
        Me.PouInstancesListBox.ValueMember = "Name"
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
        'TasksListBox
        '
        Me.TasksListBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TasksListBox.ContextMenu = Me.ContextMenu3
        Me.TasksListBox.DisplayMember = "Name"
        Me.TasksListBox.Location = New System.Drawing.Point(4, 176)
        Me.TasksListBox.Name = "TasksListBox"
        Me.TasksListBox.Size = New System.Drawing.Size(108, 134)
        Me.TasksListBox.TabIndex = 8
        Me.TasksListBox.ValueMember = "Name"
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
        'Label24
        '
        Me.Label24.Location = New System.Drawing.Point(8, 316)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(80, 16)
        Me.Label24.TabIndex = 5
        Me.Label24.Text = "Pou instances"
        '
        'Label23
        '
        Me.Label23.Location = New System.Drawing.Point(4, 4)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(112, 16)
        Me.Label23.TabIndex = 4
        Me.Label23.Text = "Global variables"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(4, 160)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 16)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Tasks"
        '
        'GlobalVariablesListBox
        '
        Me.GlobalVariablesListBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GlobalVariablesListBox.ContextMenu = Me.ContextMenu2
        Me.GlobalVariablesListBox.DisplayMember = "Name"
        Me.GlobalVariablesListBox.Location = New System.Drawing.Point(4, 20)
        Me.GlobalVariablesListBox.Name = "GlobalVariablesListBox"
        Me.GlobalVariablesListBox.Size = New System.Drawing.Size(108, 134)
        Me.GlobalVariablesListBox.TabIndex = 0
        Me.GlobalVariablesListBox.ValueMember = "Name"
        '
        'ContextMenu2
        '
        Me.ContextMenu2.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem6, Me.MenuItem3, Me.MenuItem4, Me.MenuItem5})
        '
        'MenuItem6
        '
        Me.MenuItem6.Index = 0
        Me.MenuItem6.Text = "Add list"
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 1
        Me.MenuItem3.Text = "Open"
        '
        'MenuItem4
        '
        Me.MenuItem4.Index = 2
        Me.MenuItem4.Text = "Rename"
        '
        'MenuItem5
        '
        Me.MenuItem5.Index = 3
        Me.MenuItem5.Text = "Remove"
        '
        'Splitter1
        '
        Me.Splitter1.Location = New System.Drawing.Point(124, 30)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(4, 440)
        Me.Splitter1.TabIndex = 28
        Me.Splitter1.TabStop = False
        Me.Splitter1.Visible = False
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(792, 553)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.SimulationPanel)
        Me.Controls.Add(Me.ProjectPanel)
        Me.Controls.Add(Me.toolBar1)
        Me.Controls.Add(Me.mdiClient1)
        Me.IsMdiContainer = True
        Me.Menu = Me.MainMenu1
        Me.Name = "MainForm"
        Me.HelpProvider1.SetShowHelp(Me, True)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SimulationPanel.ResumeLayout(False)
        Me.ProjectPanel.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub 'InitializeComponent
#End Region

    Sub DisposeSplashForm(ByVal Qual As Object)
        'Evento generato allo scadere del timer
        Try
            'Chiude il form splash se non è già chiuso
            Me.Enabled = True
            m_FrmSpash.Close()
        Catch ex As System.Exception

        End Try
        m_FrmSpash.Dispose()
        m_TimerSplashForm.Dispose()
    End Sub

    'Funzioni di gestione progetto
    Private Sub NewProject()
        'Controlla se c'è un progeto aperto
        If Not IsNothing(m_Project) Then
            Dim Res As DialogResult = CloseSub()
            If Res = DialogResult.Cancel Then
                Exit Sub
            End If
        End If

        If CreateNewProject() Then
            'E' creato il progetto
            'Impostazioni della finestra
            RefreshPouPanel()
            RefreshResourcePanel()
            RefreshMenuToolbar()
            ShowProjectPanel(True)
            Me.Text = m_Project.ContentHeader.name & " - " & ProductName
        End If
    End Sub
    Private Function CreateNewProject() As Boolean
        Dim projectDlg As New projectDialogForm
        Dim ResultDialog As DialogResult = projectDlg.ShowDialog()
        If ResultDialog = DialogResult.OK Then
            'Se è stato premuto OK
            Me.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
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
    End Function
    Public Sub CreateProject()
        'm_Simulation = New Simulation
        '////Creazione nuovo progetto
        'Crea il nuovo progetto
        m_Project = New Project

        'Riempie i dati del file header
        FillHeaderFile()

        'Crea l'istanza con la configurazione
        Dim NewConfiguration As New Configuration("MainConfiguration", "")

        'Crea la risorsa
        Dim NewResource As New Resource("MainResource", "")
        NewConfiguration.resources.Add(NewResource)
        m_Project.Istances.configurations.Add(NewConfiguration)
        m_Resource = NewResource

        'Comunica alla lista dei pou la lista delle liste delle variabili globali della risorsa
        m_Project.Types.pous.ResourceGlobalVariables = m_Resource.globalVars
        'Comunica la lista dei pous alla risorsa per le funzioni di simulazione
        m_Resource.SetPouList(m_Project.Types.pous)

        'Collega l'handle al SimulatorEngine
        m_SimulatorEngineHandles = m_Resource.SimulatorEngine

        '////Controllo grafico del Simulation
        'Collega i pannelli di controllo delle variabili con la memoria del Simulation
        m_SimulationState = "STOP"
    End Sub

    Public Sub CompleteXmlProject()
        'Controlla se esiste la prima configurazione con la prima risorsa, altrimenti le crea
        If m_Project.Istances.configurations.Count = 0 Then
            'Crea la configurazione e la risorsa e le aggiunge
            Dim NewConfiguration As New Configuration("MainConfiguration", "Single configuration of this project")
            m_Project.Istances.configurations.Add(NewConfiguration)
            'Crea la risorsa
            Dim NewResource As New Resource("MainResource", "Single resource of this project")
            NewConfiguration.resources.Add(NewResource)
            m_Resource = NewResource
        Else
            'Se la configurazione esiste controlla se esiste la prima risorsa
            If m_Project.Istances.configurations(0).resources.count = 0 Then
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
        m_Resource.SetPouList(m_Project.Types.pous)

        'Risolve i riferimenti dei nomi dei pou nei pouInstance
        m_Project.Istances.configurations(0).resources(0).ResolvePouInstanceLinks()

        'Collega l'handle al SimulatorEngine
        m_SimulatorEngineHandles = m_Resource.SimulatorEngine

        m_SimulationState = "STOP"
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

        Me.Text = ProductName
        RefreshMenuToolbar()
        CloseSimulation()
        ShowProjectPanel(False)
    End Sub
    Private Function CloseSub() As DialogResult
        Dim Res As DialogResult = MsgBox("Save change in " & m_Project.ContentHeader.name & "?", MessageBoxButtons.YesNoCancel)
        If Res = DialogResult.Yes Then
            Dim SaveDialog As New SaveFileDialog
            SaveDialog.Filter = "File " & ApplicationFileExtension & " (*." & ApplicationFileExtension & ")|*." & ApplicationFileExtension & "|Tutti i file (*.*)|*.*"
            SaveDialog.DefaultExt = "." & ApplicationFileExtension
            SaveDialog.FileName = ApplicationFileExtension & "File1." & ApplicationFileExtension
            Dim ResultDialog As DialogResult = SaveDialog.ShowDialog()
            If ResultDialog = DialogResult.OK Then
                'Salva e distrugge il documento e i FormSFC
                Me.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
                ''''m_Project.SaveProject(SaveDialog.FileName)
                CloseDocument()
                Me.Cursor.Current = System.Windows.Forms.Cursors.Default
            Else
                CloseSub = ResultDialog
                Exit Function
            End If
        Else
            If Res = DialogResult.No Then
                'Il risultato è NO quindi chiude senza salvare
                CloseDocument()
            End If
        End If
        CloseSub = Res
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
        If ResultDialog = DialogResult.OK Then
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
        MsgBox("p")
    End Sub
    Private Sub ShowHelpTopics()
        'Dim filePath As String = Path.Combine(Directory.GetCurrentDirectory(), "..\help\scribble.chm")
        'Help.ShowHelp(Me, filePath)
    End Sub
    Private Sub Tile()
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub
    Private Sub Cascade()
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub
    Private Sub ExitApp()
        Dim Res As DialogResult = CloseSub()
        If Res = DialogResult.No Or Res = DialogResult.Yes Then
            'Chiude se non è State Cancelto il comando durante il salvataggio
            Application.Exit()
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
            If Res = DialogResult.Cancel Then
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
        If ResultDialog = DialogResult.OK Then
            'Se è stato selezionato OK controlla se non ci sono errori
            If Not OpenDialog.FileName.ToLower(Globalization.CultureInfo.InvariantCulture).EndsWith("." & ApplicationFileXMLExtension) Then
                MsgBox("Bad format file!", MsgBoxStyle.OKOnly)
            Else
                CreateNewProjectFromXmlFile(OpenDialog.FileName)
            End If
        End If
    End Sub
    Private Sub CreateNewProjectFromXmlFile(ByVal FileName As String)
        'Controlla se il file schema esiste
        If Not File.Exists(XmlSchemaPath) Then
            MsgBox("Xml schema file do not found! (path: " & XmlSchemaPath & ")")
            Exit Sub
        End If
        'Crea il nuovo XMLProjectManager
        m_XMLprojectManager = New XMLProjectManager
        'Memorizza il contenuto del file in ImportString
        Dim ImportString As String
        'Legge il progetto xml dal file tramite l'XMLProjectManager, ottiene false se c'è un errore di aperture
        If m_XMLprojectManager.OpenXmlFile(FileName, ImportString) Then
            'Importa il file


            '////Barra di progresso
            Dim ImportingProgress As New ProgressForm
            ImportingProgress.LabelText = "Validaing file..."
            ImportingProgress.TopMost = True
            ImportingProgress.Show()
            'Crea il nuovo progetto
            m_Project = New Project

            '////Incrementa la barra di progresso
            ImportingProgress.ProgressBar1.PerformStep()
            '////

            'Se il file non contiene un progetto valido ottiene false
            Dim Result As Boolean = m_XMLprojectManager.xmlImport(ImportString, XmlSchemaPath, m_Project)

            '////Incrementa la barra di progresso
            ImportingProgress.ProgressBar1.PerformStep()
            '////

            If Not Result Then  'Progetto xml non valido
                'Distrugge il progetto
                m_Project = Nothing
                'Mostra gli errori
                Dim ErrorsList As String
                For Each S As String In m_XMLprojectManager.ErrorsList
                    ErrorsList = ErrorsList & S
                Next S

                ImportingProgress.Close()
                ImportingProgress.Dispose()


                MsgBox("Error reading xml file!" & vbCrLf & ErrorsList)
            Else    'Progetto valido e caricato
                'Completa il progetto
                ImportingProgress.LabelText = "Loading project..."
                CompleteXmlProject()

                '////Incrementa la barra di progresso
                ImportingProgress.ProgressBar1.PerformStep()
                '////

                'Aggiorna gli elementi del form
                RefreshPouPanel()
                RefreshResourcePanel()

                '////Incrementa la barra di progresso
                ImportingProgress.ProgressBar1.PerformStep()
                '////

                RefreshMenuToolbar()
                ShowProjectPanel(True)
                Me.Text = m_Project.ContentHeader.name & " - " & ProductName
                Me.Cursor.Current = System.Windows.Forms.Cursors.Default

                '////Incrementa la barra di progresso
                ImportingProgress.ProgressBar1.PerformStep()
                '////

                ImportingProgress.Close()
                ImportingProgress.Dispose()


                ''''Apre tutti i form del progetto
                For Each L As VariablesList In GlobalVariablesListBox.Items
                    ShowVariablesList(L)
                Next L
                For Each P As pou In PouListBox.Items
                    ShowBody(P)
                Next P
                'Allinea i form
                TileVertical()


            End If
        Else
            'C'è ststo un errore di apertura del file
            MsgBox("Error opening file!")
            m_XMLprojectManager = Nothing
        End If

    End Sub
    Private Sub xmlExport()
        If Not IsNothing(m_Project) Then
            Me.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
            m_XMLprojectManager = New XMLProjectManager
            'Memorizza il contenuto xml in ExportString
            Dim ExportString As StringBuilder = m_XMLprojectManager.xmlExport(m_Project)
            Me.Cursor.Current = System.Windows.Forms.Cursors.Default
            Dim SaveDialog As New SaveFileDialog
            SaveDialog.Filter = "File " & ApplicationFileXMLExtension & " (*." & ApplicationFileXMLExtension & ")|*." & ApplicationFileXMLExtension & "|Tutti i file (*.*)|*.*"
            SaveDialog.DefaultExt = "." & ApplicationFileXMLExtension
            SaveDialog.FileName = m_Project.ContentHeader.name & "." & ApplicationFileXMLExtension
            Dim ResultDialog As DialogResult = SaveDialog.ShowDialog()
            If ResultDialog = DialogResult.OK Then
                'Ottiene true se il file è stato salvato
                If Not m_XMLprojectManager.WriteXmlFile(ExportString, SaveDialog.FileName) Then
                    MsgBox("Error writing file!")
                End If
            End If
            RefreshMenuToolbar()
        End If
        m_XMLprojectManager = Nothing
    End Sub
    Private Sub XmlView()
        'Visualizza il progetto in formato XML
        Me.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        m_XMLprojectManager = New XMLProjectManager
        'Memorizza il contenuto xml in ExportString
        Dim ExportString As StringBuilder = m_XMLprojectManager.xmlExport(m_Project)
        Me.Cursor.Current = System.Windows.Forms.Cursors.Default
        'Visualizza il contenuto
        Dim ExportDialog As New xmlProjectViewForm
        ExportDialog.TextBuffer = ExportString.ToString
        ExportDialog.FlagExport = True
        Dim ResultDialog As DialogResult = ExportDialog.ShowDialog()
    End Sub

    'Funzioni di gestione POU
    Private Sub Newpou()
        Dim pouDlg As New pouDialogForm
        Dim ResultDialog As DialogResult = pouDlg.ShowDialog()
        If ResultDialog = DialogResult.OK Then
            'Controlla se il nome dell'SFC è già presente
            If Not IsNothing(m_Project.Types.pous.FindpouByName(pouDlg.m_Name)) Then
                MsgBox("POU '" & pouDlg.m_Name & "' already exists!")
            Else
                'Crea il nuovo POU e lo aggiunge alla lista
                Dim Newpou As pou = m_Project.Types.pous.AddPou(pouDlg.m_Name, "", pouDlg.m_pouType, pouDlg.m_bodyType)
                'Crea e visualizza il relativo form
                Dim FrSfcTemp As New FormSfc(Newpou.Body, True)
                FrSfcTemp.MdiParent = Me
                FrSfcTemp.Show()
                RefreshPouPanel()
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
            Dim FrSfcTemp As New FormSfc(RefPou.Body, EditingFlag)
            FrSfcTemp.MdiParent = Me
            FrSfcTemp.Show()
        End If
    End Sub
    Private Sub SetPOUInfo(ByRef RefPou As pou)
        Dim pouDialog As New pouDialogForm
        pouDialog.m_Name = RefPou.Name
        Dim ResultDialog As DialogResult = pouDialog.ShowDialog()
        If ResultDialog = DialogResult.OK Then
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
        PouListBox.Items.Clear()
        'Aggiunge gli SFC
        If Not IsNothing(m_Project) Then
            For Each P As pou In m_Project.Types.pous
                PouListBox.Items.Add(P)
            Next P
            PouListBox.Sorted = True
        End If
    End Sub
    Private Sub ShowProjectPanel(ByVal value As Boolean)
        Splitter1.Visible = value
        ProjectPanel.Visible = value
    End Sub



    'Funzioni di gestione risorsa
    Private Sub RefreshResourcePanel()
        GlobalVariablesListBox.Items.Clear()
        'Aggiunge le lista variabili globali
        If Not IsNothing(m_Project) Then
            For Each VL As VariablesList In m_Project.Istances.configurations(0).resources(0).globalVars
                GlobalVariablesListBox.Items.Add(VL)
            Next VL
        End If
        Me.TasksListBox.Items.Clear()
        'Aggiunge i task
        For Each T As Task In m_Project.Istances.configurations(0).resources(0).tasks()
            TasksListBox.Items.Add(T)
        Next T
        Me.PouInstancesListBox.Items.Clear()
        'Aggiunge i pouInstance
        For Each PI As pouInstance In m_Project.Istances.configurations(0).resources(0).pouInstances()
            PouInstancesListBox.Items.Add(PI)
        Next PI
    End Sub
    Private Sub AddGlobalVariablesList()
        Dim VariablesDialogDlg As New VariablesListDialogForm
        Dim ResultDialog As DialogResult = VariablesDialogDlg.ShowDialog()
        If ResultDialog = DialogResult.OK Then
            'Controlla se il nome della lista è già presente
            If Not IsNothing(m_Resource.globalVars.FindListByName(VariablesDialogDlg.m_Name)) Then
                MsgBox("Variables list '" & VariablesDialogDlg.m_Name & "' already exists!")
            Else
                'Crea la nuova lista e lo aggiunge alla lista
                Dim NewVariablesList As New VariablesList(VariablesDialogDlg.m_Name)
                m_Resource.globalVars.Add(NewVariablesList)
                'Crea e visualizza il relativo form
                Dim FrVariablesListTemp As New VariablesListControlPanel(m_Resource.globalVars, NewVariablesList, True)
                FrVariablesListTemp.MdiParent = Me
                FrVariablesListTemp.Show()
                'Attiva il monitoring delle variabili
                FrVariablesListTemp.StartMonitor()
                RefreshResourcePanel()
            End If
        End If
        VariablesDialogDlg.Dispose()
        RefreshMenuToolbar()
    End Sub
    Private Sub SetGlobalVariablesList(ByRef List As VariablesList)
        Dim VariablesDialogDlg As New VariablesListDialogForm
        VariablesDialogDlg.m_Name = List.Name
        Dim ResultDialog As DialogResult = VariablesDialogDlg.ShowDialog()
        If ResultDialog = DialogResult.OK Then
            If List.Name <> VariablesDialogDlg.m_Name Then
                'Se il nome è cambiato controlla se una lista  con lo stesso nome esiste già
                If Not IsNothing(m_Resource.globalVars.FindListByName(VariablesDialogDlg.m_Name)) Then
                    MsgBox("List " & VariablesDialogDlg.m_Name & " already exist!")
                Else
                    'Aggiorna i dati
                    List.Name = VariablesDialogDlg.m_Name
                    RefreshResourcePanel()
                End If
            End If
        End If
        VariablesDialogDlg.Dispose()
    End Sub
    Private Sub ShowVariablesList(ByRef List As VariablesList)
        'Controlla se il form esiste già
        Dim Find As Boolean
        For Each Fr As Form In Me.MdiChildren
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
            Dim FrVariablesListControlPanel As New VariablesListControlPanel(m_Resource.globalVars, List, True)
            FrVariablesListControlPanel.MdiParent = Me
            FrVariablesListControlPanel.Show()
            'Attiva il monitoring delle variabili
            FrVariablesListControlPanel.StartMonitor()
        End If
    End Sub
    Private Sub RemoveVariablesList(ByRef RefList As VariablesList)
        If MsgBox("Delete variables list " & RefList.Name & "?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            'Rimuove la lista dalla lista delle variabili
            m_Resource.globalVars.Remove(RefList)
            RefreshResourcePanel()
        End If
    End Sub
    Private Sub AddTask()
        Dim TaskDialogDlg As New TaskDialogForm(m_Resource, m_Project.Types.pous)
        Dim ResultDialog As DialogResult = TaskDialogDlg.ShowDialog()
        If ResultDialog = DialogResult.OK Then
            'Controlla se il nome del Task è già presente
            If Not IsNothing(m_Resource.FindTaskByName(TaskDialogDlg.m_name)) Then
                MsgBox("Task '" & TaskDialogDlg.m_name & "' already exists!")
            Else
                'Crea il task e lo aggiunge alla lista
                Dim NewTask As New Task(TaskDialogDlg.m_name, TaskDialogDlg.m_priority, TaskDialogDlg.m_interval, TaskDialogDlg.m_pouInstances)
                m_Resource.AddTask(NewTask)
            End If
        End If
        TaskDialogDlg.Dispose()
        RefreshResourcePanel()
    End Sub
    Private Sub SetTask(ByRef RefTask As Task)
        Dim TaskDialog As New TaskDialogForm(m_Resource, m_Project.Types.pous)
        TaskDialog.m_name = RefTask.name
        TaskDialog.m_priority = RefTask.priority
        TaskDialog.m_interval = RefTask.m_interval
        TaskDialog.m_pouInstances = RefTask.pouInstances
        Dim ResultDialog As DialogResult = TaskDialog.ShowDialog()
        If ResultDialog = DialogResult.OK Then
            'Se il nome è cambiato controlla se un task con lo stesso nome esiste già
            If RefTask.name <> TaskDialog.m_name Then
                If Not IsNothing(m_Resource.FindTaskByName(TaskDialog.m_name)) Then
                    MsgBox("Task " & TaskDialog.m_name & " already exist!")
                Else
                    'Aggiorna i dati
                    RefTask.name = TaskDialog.m_name
                    RefTask.priority = TaskDialog.m_priority
                    RefTask.m_interval = TaskDialog.m_interval
                    RefTask.pouInstances = TaskDialog.m_pouInstances
                    RefreshResourcePanel()
                End If
            Else
                'Non aggiorna il nome
                RefTask.priority = TaskDialog.m_priority
                RefTask.m_interval = TaskDialog.m_interval
                RefTask.pouInstances = TaskDialog.m_pouInstances
                RefreshResourcePanel()
            End If
        End If
        TaskDialog.Dispose()
    End Sub
    Private Sub RemoveTask(ByRef RefTask As Task)
        If MsgBox("Remove task " & RefTask.name & "?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            'Rimuove il task
            m_Resource.RemoveTask(RefTask)
            RefreshResourcePanel()
        End If
    End Sub
    Private Sub AddPouInstance()
        Dim PouListDialogDlg As New PouListDialogForm(m_Resource, m_Project.Types.pous)
        Dim ResultDialog As DialogResult = PouListDialogDlg.ShowDialog()
        If ResultDialog = DialogResult.OK Then
            'Controlla se l'istanza è già presente
            If Not IsNothing(m_Resource.FindPouInstanceByName(PouListDialogDlg.m_pou.Name)) Then
                MsgBox("Pou instance '" & PouListDialogDlg.m_pou.Name & "' already exists!")
            Else
                'Lo aggiunge alla lista
                Dim NewPouInstance As New pouInstance(PouListDialogDlg.m_pou)
                m_Resource.pouInstances.Add(NewPouInstance)
            End If
        End If
        PouListDialogDlg.Dispose()
        RefreshResourcePanel()
    End Sub
    Private Sub RemovePouInstance(ByRef RefPouInstance As pouInstance)
        If MsgBox("Remove pou instance " & RefPouInstance.name & "?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            'Rimuove il RefPouInstance
            m_Resource.pouInstances.Remove(RefPouInstance)
            RefreshResourcePanel()
        End If
    End Sub
    Private Sub SetPouInstance(ByRef RefPouInstance As pouInstance)
        Dim PouListDialog As New PouListDialogForm(m_Resource, m_Project.Types.pous)
        PouListDialog.m_pou = RefPouInstance.pou
        Dim ResultDialog As DialogResult = PouListDialog.ShowDialog()
        If ResultDialog = DialogResult.OK Then
            'Controlla se un pouInstance con lo stesso nome esiste già
            If Not IsNothing(m_Resource.FindPouInstanceByName(PouListDialog.m_pou.Name)) Then
                MsgBox("Pou instance " & PouListDialog.m_pou.Name & " already exist!")
            Else
                'Aggiorna i dati
                RefPouInstance.pou = PouListDialog.m_pou
                RefreshResourcePanel()
            End If
        End If
        PouListDialog.Dispose()
    End Sub
    Private Sub ClosingHandler(ByVal Sender As [Object], ByVal e As CancelEventArgs) Handles MyBase.Closing
        'Controlla se c'è un documento aperto
        If IsNothing(m_Project) Then
            Exit Sub
        End If
        Dim Res As DialogResult = CloseSub()
        If Res = DialogResult.Cancel Then
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
    End Sub
    Private Sub NextSimulationStep()
        m_SimulatorEngineHandles.ExecuteSimulationNextStep()
    End Sub
    Private Sub ResetSimulation()
        m_SimulatorEngineHandles.ResetSimulation()
    End Sub
    Private Function CloseSimulationPanel() As Boolean
        If m_SimulationState = "STOP" Then
            ResetSimulation()
            CloseSimulationPanel = True
        Else
            If MsgBox("Simulation running!. Terminate?", MsgBoxStyle.OKCancel) = MsgBoxResult.OK Then
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
        End If
    End Sub
    Private Sub CloseSimulation()
        SimulationPanel.Visible = False
        Me.MenuItemSimulationSimulation.Checked = False
        'Ripulisce la memoria hardware
    End Sub

    Public Sub RefreshMenuToolbar()
        'Controlla se non c'è un documento aperto
        If IsNothing(m_Project) Then
            'Menu Project
            Me.MenuItemProjectImport.Enabled = True
            Me.MenuItemProjectExport.Enabled = False
            Me.MenuItemClose.Enabled = False
            Me.MenuItemXmlView.Enabled = False
            Me.MenuItemPrint.Enabled = False
            Me.MenuItemInformationProject.Enabled = False

            Me.MenuItemPOU.Enabled = False
            Me.MenuItemSimulation.Enabled = False
            Me.MenuItemView.Enabled = False
            Me.MenuItemWindow.Enabled = False

            'Toolbar
            Me.toolBar1.Buttons(0).Enabled = True
            Me.toolBar1.Buttons(1).Enabled = True
            Me.toolBar1.Buttons(2).Enabled = False
            Me.toolBar1.Buttons(4).Enabled = False

        Else    'C'è un documento aperto
            'Menu file
            'Se è in Simulation non permette di chiudere o aprire file ecc...
            If m_SimulationState = "STOP" Then
                Me.MenuItemNew.Enabled = True
                Me.MenuItemProjectImport.Enabled = True
                Me.MenuItemClose.Enabled = True
                Me.MenuItemPrint.Enabled = True
                'Toolbar
                Me.toolBar1.Buttons(0).Enabled = True
                Me.toolBar1.Buttons(1).Enabled = True
            Else
                Me.MenuItemNew.Enabled = False
                Me.MenuItemProjectImport.Enabled = False
                Me.MenuItemClose.Enabled = False
                Me.MenuItemPrint.Enabled = False
                'Toolbar
                Me.toolBar1.Buttons(0).Enabled = False
                Me.toolBar1.Buttons(1).Enabled = False

            End If
            Me.MenuItemProjectExport.Enabled = True
            Me.MenuItemInformationProject.Enabled = True
            Me.MenuItemXmlView.Enabled = True
            Me.toolBar1.Buttons(2).Enabled = True
            Me.toolBar1.Buttons(4).Enabled = True


            'Altri menù
            Me.MenuItemPOU.Enabled = True
            Me.MenuItemSimulation.Enabled = True
            Me.MenuItemView.Enabled = True
            Me.MenuItemWindow.Enabled = True

            'Toolbar
            Me.toolBar1.Buttons(2).Enabled = True
            Me.toolBar1.Buttons(4).Enabled = True
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
    Private Sub MenuItemNewPou_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemNewPou.Click
        Newpou()
    End Sub
    Private Sub MenuItemCascade_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemCascade.Click
        Cascata()
    End Sub
    Private Sub MenuItemAffVert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemAffVert.Click
        TileHorizontal()
    End Sub
    Private Sub MenuItemHelpTopics_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemHelpTopics.Click
        ShowHelpTopics()
    End Sub
    Private Sub MenuItemAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemAbout.Click
        AboutHelp()
    End Sub
    Private Sub MainWindow_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_FrmSpash = New SplashForm
        Me.Enabled = False
        m_FrmSpash.Show()
        m_TimerDelegate = New TimerCallback(AddressOf DisposeSplashForm)
        Dim Null As New Object
        m_TimerSplashForm = New Threading.Timer(m_TimerDelegate, Null, 4000, -1)
        RefreshMenuToolbar()
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
    Private Sub MenuInteShowPOUBody_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuInteShowPOUBody.Click
        If Not IsNothing(PouListBox.SelectedItem) Then
            ShowBody(PouListBox.SelectedItem)
        End If
    End Sub
    Private Sub MenuItemSetPOUInfo2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemSetPOUInfo2.Click
        If Not IsNothing(PouListBox.SelectedItem) Then
            SetPOUInfo(PouListBox.SelectedItem)
        End If
    End Sub
    Private Sub MenuItemRemovePou2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRemovePou2.Click
        If Not IsNothing(PouListBox.SelectedItem) Then
            RemovePOU(PouListBox.SelectedItem)
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
        xmlExport()
    End Sub
    Private Sub MenuItemProjectImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemProjectImport.Click
        xmlImport()
    End Sub
    Private Sub PouListBox_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles PouListBox.DoubleClick
        If Not IsNothing(PouListBox.SelectedItem) Then
            ShowBody(PouListBox.SelectedItem)
        End If
    End Sub
    Private Sub GlobalVariablesListBox_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GlobalVariablesListBox.DoubleClick
        If Not IsNothing(GlobalVariablesListBox.SelectedItem) Then
            ShowVariablesList(GlobalVariablesListBox.SelectedItem)
        End If
    End Sub
    Private Sub MenuItem6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem6.Click
        AddGlobalVariablesList()
    End Sub
    Private Sub MenuItem3_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem3.Click
        If Not IsNothing(GlobalVariablesListBox.SelectedItem) Then
            ShowVariablesList(GlobalVariablesListBox.SelectedItem)
        End If
    End Sub
    Private Sub MenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem4.Click
        If Not IsNothing(GlobalVariablesListBox.SelectedItem) Then
            SetGlobalVariablesList(GlobalVariablesListBox.SelectedItem)
        End If
    End Sub
    Private Sub MenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem5.Click
        If Not IsNothing(GlobalVariablesListBox.SelectedItem) Then
            RemoveVariablesList(GlobalVariablesListBox.SelectedItem)
        End If
    End Sub
    Private Sub MenuItem9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem9.Click
        AddTask()
    End Sub
    Private Sub MenuItem7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem7.Click
        If Not IsNothing(TasksListBox.SelectedItem) Then
            SetTask(TasksListBox.SelectedItem)
        End If
    End Sub
    Private Sub MenuItem8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem8.Click
        If Not IsNothing(TasksListBox.SelectedItem) Then
            RemoveTask(TasksListBox.SelectedItem)
        End If
    End Sub
    Private Sub MenuItem10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem10.Click
        AddPouInstance()
    End Sub
    Private Sub MenuItem12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem12.Click
        If Not IsNothing(Me.PouInstancesListBox.SelectedItem) Then
            RemovePouInstance(PouInstancesListBox.SelectedItem)
        End If
    End Sub
    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Newpou()
    End Sub
    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not IsNothing(PouListBox.SelectedItem) Then
            RemovePOU(PouListBox.SelectedItem.Name)
        End If
    End Sub
    Private Sub MenuItem11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem11.Click
        Newpou()
    End Sub
    Private Sub TasksListBox_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TasksListBox.DoubleClick
        If Not IsNothing(TasksListBox.SelectedItem) Then
            SetTask(TasksListBox.SelectedItem)
        End If
    End Sub
    Private Sub PouInstancesListBox_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles PouInstancesListBox.DoubleClick
        If Not IsNothing(PouInstancesListBox.SelectedItem) Then
            SetPouInstance(PouInstancesListBox.SelectedItem)
        End If
    End Sub
    Private Sub MenuItem13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem13.Click
        If Not IsNothing(PouInstancesListBox.SelectedItem) Then
            SetPouInstance(PouInstancesListBox.SelectedItem)
        End If
    End Sub

    Private Sub MenuItemXmlView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemXmlView.Click
        XmlView()
    End Sub

End Class

