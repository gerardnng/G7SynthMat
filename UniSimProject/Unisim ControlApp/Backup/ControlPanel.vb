Imports UnisimClassLibrary
Imports System.Text

Public Class ControlPanel
    Inherits System.Windows.Forms.UserControl
    Private m_Resource As Resource
    Private m_IOInterface As IOInterface
    Private WithEvents m_ControlEngineHandles As ControlEngine
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
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents StartButton As System.Windows.Forms.Button
    Friend WithEvents StopButton As System.Windows.Forms.Button
    Friend WithEvents ResetButton As System.Windows.Forms.Button
    Friend WithEvents LabelCycles As System.Windows.Forms.Label
    Friend WithEvents LabelState As System.Windows.Forms.Label
    Friend WithEvents DeviceSettingsButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.StartButton = New System.Windows.Forms.Button
        Me.StopButton = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.LabelState = New System.Windows.Forms.Label
        Me.ResetButton = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.LabelCycles = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.DeviceSettingsButton = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'StartButton
        '
        Me.StartButton.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.StartButton.BackColor = System.Drawing.SystemColors.Control
        Me.StartButton.ForeColor = System.Drawing.Color.Black
        Me.StartButton.Location = New System.Drawing.Point(220, 12)
        Me.StartButton.Name = "StartButton"
        Me.StartButton.Size = New System.Drawing.Size(80, 44)
        Me.StartButton.TabIndex = 1
        Me.StartButton.TabStop = False
        Me.StartButton.Text = "Start"
        '
        'StopButton
        '
        Me.StopButton.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.StopButton.BackColor = System.Drawing.SystemColors.Control
        Me.StopButton.Enabled = False
        Me.StopButton.ForeColor = System.Drawing.Color.Black
        Me.StopButton.Location = New System.Drawing.Point(300, 12)
        Me.StopButton.Name = "StopButton"
        Me.StopButton.Size = New System.Drawing.Size(80, 44)
        Me.StopButton.TabIndex = 19
        Me.StopButton.TabStop = False
        Me.StopButton.Text = "Stop"
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(92, 36)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(52, 16)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Cycles/s:"
        '
        'LabelState
        '
        Me.LabelState.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.LabelState.BackColor = System.Drawing.Color.White
        Me.LabelState.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelState.ForeColor = System.Drawing.Color.Red
        Me.LabelState.Location = New System.Drawing.Point(148, 12)
        Me.LabelState.Name = "LabelState"
        Me.LabelState.Size = New System.Drawing.Size(64, 16)
        Me.LabelState.TabIndex = 20
        Me.LabelState.Text = "Stop"
        Me.LabelState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ResetButton
        '
        Me.ResetButton.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ResetButton.BackColor = System.Drawing.SystemColors.Control
        Me.ResetButton.Enabled = False
        Me.ResetButton.ForeColor = System.Drawing.Color.Black
        Me.ResetButton.Location = New System.Drawing.Point(388, 12)
        Me.ResetButton.Name = "ResetButton"
        Me.ResetButton.Size = New System.Drawing.Size(48, 44)
        Me.ResetButton.TabIndex = 28
        Me.ResetButton.TabStop = False
        Me.ResetButton.Text = "Reset"
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(92, 12)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 16)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "State:"
        '
        'LabelCycles
        '
        Me.LabelCycles.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.LabelCycles.BackColor = System.Drawing.Color.White
        Me.LabelCycles.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelCycles.ForeColor = System.Drawing.Color.Red
        Me.LabelCycles.Location = New System.Drawing.Point(148, 36)
        Me.LabelCycles.Name = "LabelCycles"
        Me.LabelCycles.Size = New System.Drawing.Size(64, 16)
        Me.LabelCycles.TabIndex = 21
        Me.LabelCycles.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(12, 4)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 16)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "Control"
        '
        'DeviceSettingsButton
        '
        Me.DeviceSettingsButton.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.DeviceSettingsButton.BackColor = System.Drawing.SystemColors.Control
        Me.DeviceSettingsButton.ForeColor = System.Drawing.Color.Black
        Me.DeviceSettingsButton.Location = New System.Drawing.Point(12, 24)
        Me.DeviceSettingsButton.Name = "DeviceSettingsButton"
        Me.DeviceSettingsButton.Size = New System.Drawing.Size(68, 32)
        Me.DeviceSettingsButton.TabIndex = 29
        Me.DeviceSettingsButton.Text = "Device settings"
        '
        'ControlPanel
        '
        Me.BackColor = System.Drawing.Color.Brown
        Me.Controls.Add(Me.DeviceSettingsButton)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.LabelCycles)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ResetButton)
        Me.Controls.Add(Me.LabelState)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.StopButton)
        Me.Controls.Add(Me.StartButton)
        Me.Name = "ControlPanel"
        Me.Size = New System.Drawing.Size(456, 64)
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public WriteOnly Property Resource() As Resource
        Set(ByVal Value As Resource)
            If Not IsNothing(Value) Then
                m_Resource = Value
                m_ControlEngineHandles = m_Resource.ControlEngine
            End If
        End Set
    End Property
    Private Sub ControlPanel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
    Private Sub StartButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartButton.Click
        m_ControlEngineHandles.Start()
        'Controlla se la partenza è avvenuta
        If m_ControlEngineHandles.State = ControlState.ControlRUN Then
            StartButton.Enabled = False
            StopButton.Enabled = True
            DeviceSettingsButton.Enabled = False
        Else
            If m_ControlEngineHandles.ErrorsList.Count > 0 Then
                'Mostra gli errori
                ShowControlErrors()
            End If

        End If
        RefreshLabelState()
    End Sub
    Private Sub RefreshCycles(ByVal Value As Integer) Handles m_ControlEngineHandles.NewCycles
        LabelCycles.Text = Value.ToString
    End Sub
    Private Sub ResetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetButton.Click
        m_ControlEngineHandles.ResetResource()
        ResetButton.Enabled = False
    End Sub
    Private Sub StopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StopButton.Click
        m_ControlEngineHandles.StopControl()
        StartButton.Enabled = True
        StopButton.Enabled = False
        ResetButton.Enabled = True
        DeviceSettingsButton.Enabled = True
        LabelCycles.Text = ""
    End Sub
    Private Sub RefreshLabelState()
        Select Case m_ControlEngineHandles.State
            Case ControlState.ControlRUN
                LabelState.Text = "Run"
            Case ControlState.ControlSTOP
                LabelState.Text = "Stop"
        End Select
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeviceSettingsButton.Click
        'Effetua in sequenza:
        '-Creazione dell' IOInterface
        '-Collegamento del device tramite il driver
        '-Mapping delle variabili
        m_IOInterface = New IOInterface

        Dim DeviceIDDlg As New DeviceIdDialogForm
        Dim ResultDialog As DialogResult = DeviceIDDlg.ShowDialog
        'Chiede di inserire l'id del device
        If ResultDialog = DialogResult.OK Then
            'Collega il device ed ottiene true se è stato superato il test
            If m_IOInterface.ConnectDriver(DeviceIDDlg.IdDevice) Then
                'Effettue il mapping
                m_IOInterface.VariablesMapping(m_Resource.globalVars)
                'Comunica l'interfaccia al ControlEngine
                m_ControlEngineHandles.IOInterface = m_IOInterface
                MsgBox("Device tests OK", MsgBoxStyle.Information, UniSimVersion.VersionInfo.PrintableDescriptionForTool("ControlApp"))
                'Pronto per il controllo
                UnLockControl()
            Else
                'Il test non è passato
                MsgBox("Device tests failed", MsgBoxStyle.Information, UniSimVersion.VersionInfo.PrintableDescriptionForTool("ControlApp"))
                LockControl()
            End If
        End If
    End Sub
    Public Sub ShowControlErrors()
        If m_ControlEngineHandles.ErrorsList.Count > 0 Then
            'Mostra gli errori
            Dim i As Integer
            Dim ErrorString As New StringBuilder
            For i = 0 To m_ControlEngineHandles.ErrorsList.Count - 1
                Select Case m_ControlEngineHandles.ErrorsList(i)
                    Case Is = Errors.NothingIOInterface
                        ErrorString.Append("Interface error")
                    Case Is = Errors.InternalCycleError
                        ErrorString.Append("Interface error")
                    Case Is = Errors.InitError
                        ErrorString.Append("Error of device")
                    Case Is = Errors.ReadError
                        ErrorString.Append("Read error")
                    Case Is = Errors.ResettingErrors
                        ErrorString.Append("Reset error")
                    Case Is = Errors.WriteError
                        ErrorString.Append("Write error")
                End Select
            Next i
            MsgBox(ErrorString.ToString)
        End If
    End Sub

    Public Sub LockControl()
        'Non permette di iniziare il controllo
        StartButton.Enabled = False
    End Sub
    Private Sub UnLockControl()
        'Permette di iniziare il controllo
        StartButton.Enabled = True
    End Sub
End Class
