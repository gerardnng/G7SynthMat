
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
Imports UnisimClassLibrary


    _

Public Class MainForm
    Inherits System.Windows.Forms.Form
    Implements IUniSimMainWindow

    Private Shared parentWindow As MainForm
    Private components As System.ComponentModel.IContainer
    Private m_Resource As Resource   'Riferimento all'unica risorsa
    Private m_ControlState As ControlState

    Private m_XMLprojectManager As XMLProjectManager

    Private m_TimerSplashForm As System.Threading.Timer
    Private m_TimerDelegate As TimerCallback
    Friend WithEvents ResourceTree1 As UnisimClassLibrary.ResourceTree
    Friend WithEvents ControlPanel1 As UniSim.ControlPanel
    Private m_FrmSpash As SplashForm
#Region " Codice generato da Progettazione Windows Form "


    Public Sub New()
        parentWindow = Me
        InitializeComponent()
        Me.ResourceTree1.MdiParentForm = Me
        Me.Text = ProductName
        m_ControlState = ControlState.ControlSTOP


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
    Private WithEvents OpenButton As System.Windows.Forms.ToolBarButton
    Private WithEvents toolBar1 As System.Windows.Forms.ToolBar
    Private WithEvents MenuItemAbout As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemStep7 As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemAffVert As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemToolbar As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemHelp As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemWindow As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemView As System.Windows.Forms.MenuItem
    Private WithEvents mdiClient1 As System.Windows.Forms.MdiClient
    Private WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Private WithEvents MenuItemCascade As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemAffOriz As System.Windows.Forms.MenuItem
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents MenuItemProject As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemInformationProject As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemProjectImport As System.Windows.Forms.MenuItem
    Friend WithEvents ContextMenu1 As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuInteShowPOUBody As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemSetPOUInfo2 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemRemovePou2 As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemClose As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemExit As System.Windows.Forms.MenuItem
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
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
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemXmlView As System.Windows.Forms.MenuItem
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label



    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.toolBar1 = New System.Windows.Forms.ToolBar
        Me.OpenButton = New System.Windows.Forms.ToolBarButton
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.mdiClient1 = New System.Windows.Forms.MdiClient
        Me.MenuItemAffOriz = New System.Windows.Forms.MenuItem
        Me.MenuItemView = New System.Windows.Forms.MenuItem
        Me.MenuItemToolbar = New System.Windows.Forms.MenuItem
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.MenuItemProject = New System.Windows.Forms.MenuItem
        Me.MenuItemProjectImport = New System.Windows.Forms.MenuItem
        Me.MenuItemClose = New System.Windows.Forms.MenuItem
        Me.MenuItemInformationProject = New System.Windows.Forms.MenuItem
        Me.MenuItemXmlView = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.MenuItemExit = New System.Windows.Forms.MenuItem
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
        Me.ContextMenu4 = New System.Windows.Forms.ContextMenu
        Me.MenuItem10 = New System.Windows.Forms.MenuItem
        Me.MenuItem13 = New System.Windows.Forms.MenuItem
        Me.MenuItem12 = New System.Windows.Forms.MenuItem
        Me.ContextMenu3 = New System.Windows.Forms.ContextMenu
        Me.MenuItem9 = New System.Windows.Forms.MenuItem
        Me.MenuItem7 = New System.Windows.Forms.MenuItem
        Me.MenuItem8 = New System.Windows.Forms.MenuItem
        Me.ContextMenu2 = New System.Windows.Forms.ContextMenu
        Me.MenuItem6 = New System.Windows.Forms.MenuItem
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.MenuItem4 = New System.Windows.Forms.MenuItem
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.ControlPanel1 = New UniSim.ControlPanel
        Me.ResourceTree1 = New UnisimClassLibrary.ResourceTree
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'toolBar1
        '
        Me.toolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.OpenButton})
        Me.toolBar1.ButtonSize = New System.Drawing.Size(24, 24)
        Me.toolBar1.DropDownArrows = True
        Me.toolBar1.ImageList = Me.ImageList1
        Me.toolBar1.Location = New System.Drawing.Point(0, 0)
        Me.toolBar1.Name = "toolBar1"
        Me.toolBar1.ShowToolTips = True
        Me.toolBar1.Size = New System.Drawing.Size(792, 28)
        Me.toolBar1.TabIndex = 1
        '
        'OpenButton
        '
        Me.OpenButton.ImageIndex = 1
        Me.OpenButton.Name = "OpenButton"
        Me.OpenButton.ToolTipText = "Open"
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
        '
        'mdiClient1
        '
        Me.mdiClient1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.mdiClient1.Location = New System.Drawing.Point(164, 28)
        Me.mdiClient1.Name = "mdiClient1"
        Me.mdiClient1.Size = New System.Drawing.Size(628, 419)
        Me.mdiClient1.TabIndex = 0
        '
        'MenuItemAffOriz
        '
        Me.MenuItemAffOriz.Index = 2
        Me.MenuItemAffOriz.Text = "Tile &vertically"
        '
        'MenuItemView
        '
        Me.MenuItemView.Index = 1
        Me.MenuItemView.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemToolbar})
        Me.MenuItemView.MergeOrder = 5
        Me.MenuItemView.Text = "&View"
        '
        'MenuItemToolbar
        '
        Me.MenuItemToolbar.Checked = True
        Me.MenuItemToolbar.Index = 0
        Me.MenuItemToolbar.Text = "&Toolbar"
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemProject, Me.MenuItemView, Me.MenuItemWindow, Me.MenuItemHelp})
        '
        'MenuItemProject
        '
        Me.MenuItemProject.Index = 0
        Me.MenuItemProject.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemProjectImport, Me.MenuItemClose, Me.MenuItemInformationProject, Me.MenuItemXmlView, Me.MenuItem2, Me.MenuItemExit})
        Me.MenuItemProject.MergeOrder = 1
        Me.MenuItemProject.Text = "&Project"
        '
        'MenuItemProjectImport
        '
        Me.MenuItemProjectImport.Index = 0
        Me.MenuItemProjectImport.Text = "&Open"
        '
        'MenuItemClose
        '
        Me.MenuItemClose.Index = 1
        Me.MenuItemClose.Text = "&Close"
        '
        'MenuItemInformationProject
        '
        Me.MenuItemInformationProject.Index = 2
        Me.MenuItemInformationProject.Text = "Project &information"
        '
        'MenuItemXmlView
        '
        Me.MenuItemXmlView.Index = 3
        Me.MenuItemXmlView.Text = "&View XML Format"
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 4
        Me.MenuItem2.Text = "-"
        '
        'MenuItemExit
        '
        Me.MenuItemExit.Index = 5
        Me.MenuItemExit.Text = "&Exit"
        '
        'MenuItemWindow
        '
        Me.MenuItemWindow.Index = 2
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
        Me.MenuItemHelp.Index = 3
        Me.MenuItemHelp.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemHelpTopics, Me.MenuItemStep7, Me.MenuItemAbout})
        Me.MenuItemHelp.MergeOrder = 7
        Me.MenuItemHelp.Text = "&Help"
        '
        'MenuItemHelpTopics
        '
        Me.MenuItemHelpTopics.Index = 0
        Me.MenuItemHelpTopics.Text = "&Content"
        '
        'MenuItemStep7
        '
        Me.MenuItemStep7.Index = 1
        Me.MenuItemStep7.Text = "-"
        '
        'MenuItemAbout
        '
        Me.MenuItemAbout.Index = 2
        Me.MenuItemAbout.Text = "&About..."
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
        Me.Splitter1.Location = New System.Drawing.Point(160, 28)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(4, 483)
        Me.Splitter1.TabIndex = 28
        Me.Splitter1.TabStop = False
        Me.Splitter1.Visible = False
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.ResourceTree1)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel1.Location = New System.Drawing.Point(0, 28)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(160, 483)
        Me.Panel1.TabIndex = 29
        Me.Panel1.Visible = False
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(156, 28)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Resource configuration"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ControlPanel1
        '
        Me.ControlPanel1.BackColor = System.Drawing.Color.Brown
        Me.ControlPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ControlPanel1.Location = New System.Drawing.Point(164, 447)
        Me.ControlPanel1.Name = "ControlPanel1"
        Me.ControlPanel1.Size = New System.Drawing.Size(628, 64)
        Me.ControlPanel1.TabIndex = 30
        '
        'ResourceTree1
        '
        Me.ResourceTree1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ResourceTree1.Location = New System.Drawing.Point(0, 28)
        Me.ResourceTree1.MdiParentForm = Nothing
        Me.ResourceTree1.Name = "ResourceTree1"
        Me.ResourceTree1.Project = Nothing
        Me.ResourceTree1.Size = New System.Drawing.Size(156, 451)
        Me.ResourceTree1.TabIndex = 2
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(792, 511)
        Me.Controls.Add(Me.ControlPanel1)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.toolBar1)
        Me.Controls.Add(Me.mdiClient1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.Menu = Me.MainMenu1
        Me.Name = "MainForm"
        Me.HelpProvider1.SetShowHelp(Me, True)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub 'InitializeComponent
#End Region

    Public ReadOnly Property ApplicationName() As String Implements IUniSimMainWindow.ApplicationName
        Get
            Return "ApplicationName" 'ou bien simplement Return "UniSim"
        End Get
    End Property


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
    Private Sub CloseProject()
        'Chiude tutti i form aperti
        For Each Fr As Form In Me.MdiChildren
            Fr.Dispose()
        Next Fr

        'Distrugge il progetto
        m_Project.DisposeMe()
        m_Project = Nothing

        Me.Text = ProductName
        RefreshMenuToolbar()
        ShowResourcePanel(False)
        ShowControlPanel(False)
    End Sub
    Private Function ViewProjectInformation() As Boolean
        'Se è stato premuto OK restituisce TRUE
        Dim projectDlg As New projectInfoDialogForm(m_Project)
        projectDlg.ShowDialog()
    End Function
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

    'Funzioni di XML import progetto
    Private Sub xmlImport()
        'Controlla se è già aperto un altro
        If Not IsNothing(m_Project) Then
            CloseProject()
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
            If Not OpenDialog.FileName.ToLower(Globalization.CultureInfo.InvariantCulture).EndsWith("." & ApplicationFileXMLExtension) Then
                MsgBox("Bad format file!", MsgBoxStyle.OkOnly)
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
            ImportingProgress.TopMost = True
            ImportingProgress.Show()
            'Crea il nuovo progetto
            m_Project = New Project

            '////Incrementa la barra di progresso
            ImportingProgress.NextStep()
            '////

            'Se il file non contiene un progetto valido ottiene false
            Dim Result As Boolean = m_XMLprojectManager.xmlImport(ImportString, XmlSchemaPath, m_Project)

            '////Incrementa la barra di progresso
            ImportingProgress.NextStep()
            '////

            If Not Result Then  'Progetto xml non valido
                'Distrugge il progetto
                m_Project = Nothing
                'Mostra gli errori
                Dim ErrorsList As String
                For Each S As String In m_XMLprojectManager.ErrorsList
                    ErrorsList = ErrorsList & S
                Next S

                MsgBox("Error reading xml file!" & vbCrLf & ErrorsList)
            Else    'Progetto valido e caricato
                'Completa il progetto
                If CompleteXmlProject() Then
                    'Se non ci sono stati errori
                    '////Incrementa la barra di progresso
                    ImportingProgress.NextStep()
                    '////


                    '////Incrementa la barra di progresso
                    ImportingProgress.NextStep()
                    '////

                    RefreshMenuToolbar()
                    Me.Text = m_Project.ContentHeader.name & " - " & ProductName
                    Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

                    '////Incrementa la barra di progresso
                    ImportingProgress.NextStep()
                    RefreshResourcePanel()
                    ShowResourcePanel(True)
                    ShowControlPanel(True)
                    'Blocca il controllo
                    ControlPanel1.LockControl()
                    '////
                Else
                    'Distrugge il progetto
                    m_Project = Nothing
                End If
            End If
            ImportingProgress.Close()
            ImportingProgress.Dispose()
        Else
            'C'è ststo un errore di apertura del file
            MsgBox("Error opening file!")
            m_XMLprojectManager = Nothing
        End If

    End Sub
    Public Function CompleteXmlProject() As Boolean
        'Controlla se esiste la prima configurazione con la prima risorsa, altrimenti le crea
        If m_Project.Istances.configurations.Count = 0 Then
            MsgBox("Configurations don't exist in this project!")
            CompleteXmlProject = False
        Else
            'Se la configurazione esiste controlla se esiste la prima risorsa
            If m_Project.Istances.configurations(0).resources.Count = 0 Then
                MsgBox("Resource of first configuration don't exist in this project!")
                CompleteXmlProject = False
            Else
                'Imposta il riferimento alla prima risorsa
                m_Resource = m_Project.Istances.configurations(0).resources(0)

                'Comunica alla lista dei pou la lista delle liste delle variabili globali della risorsa
                m_Project.Types.pous.ResourceGlobalVariables = m_Resource.globalVars

                'Risolve i riferimenti dei nomi di variabili nella azioni e nelle condizioni
                m_Project.Types.pous.ResolveVariablesLinks()

                'Comunica la lista dei pous alla risorsa
                m_Resource.pouList = m_Project.Types.pous

                'Risolve i riferimenti dei nomi dei pou nei pouInstance
                m_Project.Istances.configurations(0).resources(0).ResolvePouInstanceLinks()

                'Crea le instanze dei POUS
                m_Project.Istances.configurations(0).resources(0).Built()

                'Comunica al ControlPanel il riferimento alla risorsa
                ControlPanel1.Resource = m_Project.Istances.configurations(0).resources(0)
                CompleteXmlProject = True
            End If
        End If

    End Function


    'Funzioni di gestione controlli grafici
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
    Private Sub ShowResourcePanel(ByVal value As Boolean)
        Splitter1.Visible = value
        Panel1.Visible = value
    End Sub
    Private Sub ShowControlPanel(ByVal value As Boolean)
        Me.SuspendLayout()
        ControlPanel1.Visible = value
        ControlPanel1.BringToFront()
        Me.ResumeLayout()
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
            Dim FrVariablesListControlPanel As New VariablesListControlPanel(m_Resource.globalVars, List, False)
            FrVariablesListControlPanel.MdiParent = Me
            FrVariablesListControlPanel.Show()
            'Attiva il monitoring delle variabili
            FrVariablesListControlPanel.StartMonitor()
        End If
    End Sub
    Private Sub ShowTask(ByRef RefTask As Task)
        Dim TaskDialog As New TaskDialogForm(m_Resource, m_Project.Types.pous)
        TaskDialog.m_name = RefTask.name
        TaskDialog.m_priority = RefTask.priority
        TaskDialog.m_interval = RefTask.m_interval
        TaskDialog.m_pouInstances = RefTask.pouInstances
        TaskDialog.ShowDialog()
    End Sub
    Private Sub ShowPouInstance(ByRef RefPouInstance As pouInstance)
        Dim PouListDialog As New PouListDialogForm(m_Resource, m_Project.Types.pous)
        PouListDialog.m_pou = RefPouInstance.pou
        PouListDialog.ShowDialog()
    End Sub
    Public Sub RefreshMenuToolbar()
        'Controlla se non c'è un documento aperto
        If IsNothing(m_Project) Then
            'Menu Project
            Me.MenuItemProjectImport.Enabled = True
            Me.MenuItemClose.Enabled = False
            Me.MenuItemXmlView.Enabled = False
            Me.MenuItemInformationProject.Enabled = False

            Me.MenuItemView.Enabled = False
            Me.MenuItemWindow.Enabled = False

            'Toolbar
            Me.toolBar1.Buttons(0).Enabled = True

        Else    'C'è un documento aperto
            'Menu file
            'Se è in Simulation non permette di chiudere o aprire file ecc...
            If m_ControlState = ControlState.ControlSTOP Then
                Me.MenuItemProjectImport.Enabled = True
                Me.MenuItemClose.Enabled = True
                'Toolbar
                Me.toolBar1.Buttons(0).Enabled = True
            Else
                Me.MenuItemProjectImport.Enabled = False
                Me.MenuItemClose.Enabled = False
                'Toolbar
                Me.toolBar1.Buttons(0).Enabled = False
            End If
            Me.MenuItemInformationProject.Enabled = True
            Me.MenuItemXmlView.Enabled = True


            'Altri menù
            Me.MenuItemView.Enabled = True
            Me.MenuItemWindow.Enabled = True
        End If
    End Sub
    Private Sub RefreshResourcePanel()
        ResourceTree1.Project = m_Project
        ResourceTree1.RefreshTreeStruct()
    End Sub

    Private Sub Tile()
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub
    Private Sub Cascade()
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub
    Private Sub ExitApp()
        Application.Exit()
    End Sub
    Private Sub ClosingMainAppHander(ByVal Sender As [Object], ByVal e As CancelEventArgs) 'Gestisce MyBase.CloLeftgMainAppHander
        Me.ExitApp()
    End Sub 'CloLeftgMainAppHander
    Private Sub ClosingHandler(ByVal Sender As [Object], ByVal e As CancelEventArgs) Handles MyBase.Closing
        'Controlla se c'è un documento aperto
        If IsNothing(m_Project) Then
            Exit Sub
        End If
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
        If e.Button Is OpenButton Then
            xmlImport()
        End If

    End Sub 'ToolBar1_ButtonClick
    Private Sub MenuItemClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemClose.Click
        CloseProject()
    End Sub
    Private Sub MenuItemAffOriz_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemAffOriz.Click
        TileVertical()
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
        Cascade()
    End Sub
    Private Sub MenuItemAffVert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemAffVert.Click
        TileHorizontal()
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
        ViewProjectInformation()
    End Sub
    Private Sub MenuItemXmlView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemXmlView.Click
        XmlView()
    End Sub

    Private Sub MenuItemProjectImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemProjectImport.Click
        xmlImport()
    End Sub

    Private Sub MenuItemHelpTopics_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemHelpTopics.Click
        Try
            Dim sInfo As New ProcessStartInfo(System.Windows.Forms.Application.StartupPath & "\Unisim Help\Default.htm")
            Process.Start(sInfo)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub MenuItemAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemAbout.Click
        Try
            Dim AboutDialog As New AboutDialogForm
            AboutDialog.ShowDialog()
        Catch Ex As System.Exception
            MsgBox(Ex.Message)
        End Try
    End Sub

    ' La ControlApp non ha un POUsTree
    Public Function GetPOUsTree() As UnisimClassLibrary.PousTree Implements UnisimClassLibrary.IUniSimMainWindow.GetPOUsTree
        Return Nothing
    End Function

    Public Function GetResourceTree() As UnisimClassLibrary.ResourceTree Implements UnisimClassLibrary.IUniSimMainWindow.GetResourceTree
        Return Me.ResourceTree1
    End Function
End Class

