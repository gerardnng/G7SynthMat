Imports System.Threading
Imports System.Windows.Forms
Public Class VariablesListControlPanel
    Inherits System.Windows.Forms.Form
    Protected m_Lists As VariablesLists 'Lista contenente la lista delle variabili
    Protected WithEvents m_List As VariablesList 'Lista delle variabili che controlla
    Protected m_Editing As Boolean
    Protected m_Monitoring As Boolean
    Protected m_TimeMonitoring As Boolean
    'Variabili per time monitor
    Protected m_showTimeMonitor As Boolean
    Protected m_PositionValue As Integer
    Protected m_TimeSample As Integer
    Protected m_ChangeTimeSample As Boolean
    Protected m_PrositionValue As Integer
    Protected m_TimerGrapValue As System.Threading.Timer
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Protected m_TimerDelegate As TimerCallback
#Region " Codice generato da Progettazione Windows Form "

    Public Sub New(ByRef Lists As VariablesLists, ByRef List As VariablesList, ByVal Editing As Boolean)
        MyBase.New()

        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()
        m_Lists = Lists
        m_List = List
        m_TimeSample = CInt(1000 / TrackBar1.Value)
        m_TimerDelegate = New TimerCallback(AddressOf OnEndTimer)
        m_Editing = Editing
        If Not m_Editing Then
            Button1.Visible = False
        End If
        RefreshVariableControlsList()
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
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ContextMenu1 As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuItemSettingsVar As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemRemoveVar As System.Windows.Forms.MenuItem
    Friend WithEvents Label0 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents TrackBar1 As System.Windows.Forms.TrackBar
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(VariablesListControlPanel))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ContextMenu1 = New System.Windows.Forms.ContextMenu
        Me.MenuItemSettingsVar = New System.Windows.Forms.MenuItem
        Me.MenuItemRemoveVar = New System.Windows.Forms.MenuItem
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label0 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Button2 = New System.Windows.Forms.Button
        Me.TrackBar1 = New System.Windows.Forms.TrackBar
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.AutoScroll = True
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Location = New System.Drawing.Point(4, 24)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(575, 215)
        Me.Panel1.TabIndex = 0
        '
        'ContextMenu1
        '
        Me.ContextMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemSettingsVar, Me.MenuItemRemoveVar})
        '
        'MenuItemSettingsVar
        '
        Me.MenuItemSettingsVar.Index = 0
        Me.MenuItemSettingsVar.Text = "Settings"
        '
        'MenuItemRemoveVar
        '
        Me.MenuItemRemoveVar.Index = 1
        Me.MenuItemRemoveVar.Text = "Remove"
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!)
        Me.Button1.Location = New System.Drawing.Point(4, 247)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(84, 32)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Add variable"
        '
        'Label0
        '
        Me.Label0.BackColor = System.Drawing.SystemColors.Control
        Me.Label0.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!)
        Me.Label0.Location = New System.Drawing.Point(4, 4)
        Me.Label0.Name = "Label0"
        Me.Label0.Size = New System.Drawing.Size(62, 14)
        Me.Label0.TabIndex = 17
        Me.Label0.Text = "Type"
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!)
        Me.Label5.Location = New System.Drawing.Point(416, 4)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(84, 16)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "Numeric value"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!)
        Me.Label3.Location = New System.Drawing.Point(332, 4)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(87, 16)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "Initial value"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!)
        Me.Label2.Location = New System.Drawing.Point(248, 4)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 16)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Boolean value"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.SystemColors.Control
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!)
        Me.Label6.Location = New System.Drawing.Point(68, 4)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(180, 16)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Name"
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!)
        Me.Button2.Location = New System.Drawing.Point(273, 247)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(88, 32)
        Me.Button2.TabIndex = 20
        Me.Button2.Text = "Sampling monitor (on/off)"
        '
        'TrackBar1
        '
        Me.TrackBar1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TrackBar1.LargeChange = 10
        Me.TrackBar1.Location = New System.Drawing.Point(433, 255)
        Me.TrackBar1.Maximum = 100
        Me.TrackBar1.Minimum = 10
        Me.TrackBar1.Name = "TrackBar1"
        Me.TrackBar1.Size = New System.Drawing.Size(124, 42)
        Me.TrackBar1.TabIndex = 21
        Me.TrackBar1.TickFrequency = 25
        Me.TrackBar1.Value = 100
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!)
        Me.Label4.Location = New System.Drawing.Point(367, 249)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 28)
        Me.Label4.TabIndex = 22
        Me.Label4.Text = "Sampling frequency"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!)
        Me.Label1.Location = New System.Drawing.Point(433, 243)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(36, 12)
        Me.Label1.TabIndex = 23
        Me.Label1.Text = "10Hz"
        '
        'Label7
        '
        Me.Label7.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!)
        Me.Label7.Location = New System.Drawing.Point(526, 243)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(39, 12)
        Me.Label7.TabIndex = 24
        Me.Label7.Text = "100Hz"
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.SystemColors.Control
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!)
        Me.Label8.Location = New System.Drawing.Point(495, 4)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(84, 16)
        Me.Label8.TabIndex = 25
        Me.Label8.Text = "Address"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'VariablesListControlPanel
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(587, 284)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label0)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.TrackBar1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(350, 100)
        Me.Name = "VariablesListControlPanel"
        Me.Text = "Variables list"
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Private Sub VariablesControlPanel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = m_List.Name
        TrackBar1.Value = Preferences.GetInteger("DefaultSamplingFreq", 100)
        m_TimeSample = 1000 / TrackBar1.Value
        If Preferences.GetBoolean("SamplingMonitorOn", True) _
            And Not (m_TimeMonitoring) Then Call SwitchShowTimeMonitor()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Aggiunge una nuova variabile con il relativo controllo
        AddVariableAndControl()
    End Sub
    Public Sub SetVariablesList(ByRef List As VariablesList)
        m_List = List
        RefreshVariableControlsList()
    End Sub
    Public Sub AddVariableAndControl()
        'Crea la nuova variabile con il controllo e la inserisce nalla lista
        Dim VariableDialog As New VariableDialogForm
        VariableDialog.m_ChangeType = True
        Do
            VariableDialog.m_Name = ""
            Dim ResultDialog As System.Windows.Forms.DialogResult = VariableDialog.ShowDialog()
            If ResultDialog = Windows.Forms.DialogResult.OK Then
                'Controlla se una viariabile con lo stesso nome esiste già nella lista delle liste di variabili
                If Not IsNothing(m_Lists.FindVariableByName(VariableDialog.m_Name)) Then
                    MsgBox("Variable " & VariableDialog.m_Name & " already exist!", MsgBoxStyle.Exclamation, _
                        UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
                Else
                    'Aggiunge una nuova variabile con il relativo controllo
                    Dim NewVariable As BaseVariable = m_List.CreateAndAddVariable(VariableDialog.m_Name, VariableDialog.m_Documentation, VariableDialog.m_Address, VariableDialog.m_InitialValue, VariableDialog.m_Type)
                    ' non serve più.. gestito da VariableAdded()
                    'If Not IsNothing(NewVariable) Then
                    'Dim ContVariable As New VariableControl(NewVariable, ContextMenu1, True)
                    'ContVariable.SetMonitoring(m_Monitoring)
                    'Me.SuspendLayout()
                    'Me.Panel1.Controls.Add(ContVariable)
                    'ContVariable.Dock = DockStyle.Top
                    'ContVariable.BringToFront()
                    'ContVariable.CallFocus()
                    'ContVariable.RefreshScale(1000 / m_TimeSample)
                    'Me.ResumeLayout()
                End If
            End If
        Loop While VariableDialog.AgainOn
    End Sub
    Public Sub ResetControls()
        For Each ContrVariable As Object In Panel1.Controls
            ContrVariable.ResetControl()
        Next ContrVariable
    End Sub
    Public Sub RemoveAllVariableAndControl()
        'Rimuove tutte i controlli variabili
        m_List.RemoveAll()
    End Sub
    Public Sub SettingVariable(ByRef VC As VariableControl)
        'Cambia i dati della variabile del controllo VC
        Dim Var As BaseVariable = VC.ReadVariable
        Dim VariableDialog As New VariableDialogForm(VC.ReadVariable())

        Dim ResultDialog As DialogResult = VariableDialog.ShowDialog
        If ResultDialog = Windows.Forms.DialogResult.OK Then
            'Se il nome è cambiato controlla se una viariabile con lo stesso nome esiste già se in nome è cambiato
            If VC.ReadVariable.Name <> VariableDialog.m_Name Then
                If Not IsNothing(m_List.FindVariableByName(VariableDialog.m_Name)) Then
                    MsgBox("Variable " & VariableDialog.m_Name & " already exist!", MsgBoxStyle.Exclamation, _
                        UniSimVersion.VersionInfo.PrintableDescriptionForTool(""))
                Else
                    'Aggiorna i dati
                    Var.Name = VariableDialog.m_Name
                    Var.Address = VariableDialog.m_Address
                    Var.SetInitialValue(VariableDialog.m_InitialValue)
                    VC.RefreshControl()
                End If
            Else
                'Non aggiorna il nome
                Var.Address = VariableDialog.m_Address
                Var.SetInitialValue(VariableDialog.m_InitialValue)
            End If
            VC.RefreshControl()
        End If
    End Sub
    Public Sub RemoveVariable(ByRef VC As VariableControl)
        'Rimuove il controllo VC a la relativa variabile
        If MsgBox("Remove variable '" & VC.ReadVariable.Name & "'?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, _
            UniSimVersion.VersionInfo.PrintableDescriptionForTool("")) = MsgBoxResult.Yes Then
            Dim V As Object = VC.ReadVariable
            m_List.Remove(V)
            'Rimuove il controllo
            Panel1.Controls.Remove(VC)
        End If
    End Sub
    Public Function ReadList() As VariablesList
        ReadList = m_List
    End Function
    Public Sub StartMonitor()
        m_Monitoring = True
        For Each VC As VariableControl In Panel1.Controls
            VC.SetMonitoring(True)
        Next VC
        'Attiva il pulsante Show time monitor
        Button2.Enabled = True
    End Sub
    Public Sub StopMonitor()
        m_Monitoring = False
        For Each VC As VariableControl In Panel1.Controls
            VC.SetMonitoring(False)
        Next VC
        StopTimeMonitor()
        'Disattiva il pulsante Show time monitor
        Button2.Enabled = False
    End Sub
    Private Sub RefreshVariableControlsList()
        Me.SuspendLayout()
        Me.Panel1.Controls.Clear()
        For Each V As BaseVariable In m_List
            If V.Hidden Then Continue For
            'Crea il controllo variabile
            Dim ContVariable As New VariableControl(V, ContextMenu1, m_Editing)
            ContVariable.SetMonitoring(m_Monitoring)
            'Aggiunge il controllo
            Me.Panel1.Controls.Add(ContVariable)
            ContVariable.Dock = DockStyle.Top
            ContVariable.BringToFront()
            ContVariable.RefreshControl()
            ContVariable.RefreshScale(1000 / m_TimeSample)
        Next V
        Me.ResumeLayout()
    End Sub
    Private Sub MenuItemRemoveVar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemRemoveVar.Click
        RemoveVariable(ContextMenu1.SourceControl)
    End Sub
    Private Sub MenuItemSettingsVar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemSettingsVar.Click
        SettingVariable(ContextMenu1.SourceControl)
    End Sub
    Private Sub NameChanged(ByVal Value As String) Handles m_List.NameChanged 'Intercetta il camciamento del nome
        Me.Text = m_List.Name
    End Sub
    Private Sub VariableAdded(ByVal Var As BaseVariable) Handles m_List.NewVariable
        Call RefreshVariableControlsList()
    End Sub
    Private Sub VariableDeleted(ByVal Var As BaseVariable) Handles m_List.VariableDropped
        Call RefreshVariableControlsList()
    End Sub
    Private Sub DisposingList() Handles m_List.Disposing    'Intercetta la distruzione della lista
        Me.Dispose()
    End Sub
    Public Sub SwitchShowTimeMonitor()
        If m_TimeMonitoring Then
            StopTimeMonitor()
        Else
            StartTimeMonitor()
        End If
    End Sub
    Private Sub RefreshScale()
        For Each C As VariableControl In Panel1.Controls
            C.RefreshScale(1000 / m_TimeSample)
        Next C
    End Sub
    Private Sub StartTimeMonitor()
        'Ripulisce i grafici
        For Each C As VariableControl In Panel1.Controls
            C.ClearTimeValue()
        Next C
        m_PositionValue = 0
        'Fa partire il timer
        m_TimerGrapValue = New Threading.Timer(m_TimerDelegate, Nothing, 0, m_TimeSample)
        m_TimeMonitoring = True
    End Sub
    Private Sub StopTimeMonitor()
        'Distrugge il timer
        If Not IsNothing(m_TimerGrapValue) Then
            m_TimerGrapValue.Dispose()
        End If
        m_TimeMonitoring = False
    End Sub
    Private Sub OnEndTimer(ByVal Obj As Object)
        If m_ChangeTimeSample Then   'Controlla se deve cambiare il tempo di campionamento
            ChangeSampleTime()
        Else
            For Each C As VariableControl In Panel1.Controls
                'Stampa un valore temporale
                C.DrawNextTimeValue(m_PositionValue)
            Next C
            m_PositionValue = m_PositionValue + 1
        End If
    End Sub
    Private Sub ChangeSampleTime()
        'Distrugge il timer se ancora esiste
        If Not IsNothing(m_TimerGrapValue) Then
            m_TimerGrapValue.Dispose()
            'Ripulisce i grafici
            For Each C As VariableControl In Panel1.Controls
                C.ClearTimeValue()
            Next C
            m_PositionValue = 0
            'Fa partire il timer con il nuovo valore
            m_TimerGrapValue = New Threading.Timer(m_TimerDelegate, Nothing, 0, m_TimeSample)
            m_ChangeTimeSample = False
        End If
    End Sub
    Private Sub VariablesListControlPanel_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Not IsNothing(m_TimerGrapValue) Then
            m_TimerGrapValue.Dispose()
        End If
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        SwitchShowTimeMonitor()
    End Sub
    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        m_TimeSample = CInt(1000 / TrackBar1.Value)
        m_ChangeTimeSample = True
        'Aggiorna la scala 
        RefreshScale()
    End Sub
End Class
