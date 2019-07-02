Imports UnisimClassLibrary

Public Class PreferencesForm

    Private m_PreferencesSet As PreferenceSet = Nothing
    Private m_MustReboot As Boolean = False

    Private Sub SetMustRebootBoolean(ByVal sender As PreferenceMapper(Of Boolean), ByVal oldValue As Boolean, ByVal newValue As Boolean)
        m_MustReboot = True
    End Sub
    Private Sub SetMustRebootInteger(ByVal sender As PreferenceMapper(Of Integer), ByVal oldValue As Integer, ByVal newValue As Integer)
        m_MustReboot = True
    End Sub

    Private Sub PreparePreferencesSet()
        If Not (m_PreferencesSet) Is Nothing Then Exit Sub
        m_PreferencesSet = New PreferenceSet()
        With m_PreferencesSet
            .Add(New PreferenceMapper(Of Boolean)("NoProjValidate", False, _
                New CheckBoxPreferenceBinder(chkNoValidation)))
            .Add(New PreferenceMapper(Of Boolean)("AutoMakeGlobVarList", False, _
                New CheckBoxPreferenceBinder(chkAutomakeGlobVarList)))
            .Add(New PreferenceMapper(Of Integer)("NewPOULang", 1, _
                New RadioButtonsPreferenceBinder(1, rdbDefaultSFC, rdbDefaultLD, rdbDefaultFBD)))
            .Add(New PreferenceMapper(Of Boolean)("Step0IsInitial", True, _
                New CheckBoxPreferenceBinder(chkFirstStepInitial)))
            .Add(New PreferenceMapper(Of Boolean)("DeclareExternsUsed", True, _
                New CheckBoxPreferenceBinder(chkDeclareExternsUsage)))
            .Add(New PreferenceMapper(Of Boolean)("KeepConnectState", True, _
                New CheckBoxPreferenceBinder(chkKeepConnectState)))
            .Add(New PreferenceMapper(Of Boolean)("CoolPNGUI", False, _
                New CheckBoxPreferenceBinder(chkUseArrowsPN)))
            .Add(New PreferenceMapper(Of Boolean)("PowerFlowColor", False, _
                New CheckBoxPreferenceBinder(chkPowerFlowColor), AddressOf SetMustRebootBoolean))
            .Add(New PreferenceMapper(Of Boolean)("FBDKeepConnectState", True, _
                New CheckBoxPreferenceBinder(chkFBDKeepConnectState)))
            .Add(New PreferenceMapper(Of Boolean)("AutoMakeOUT", False, _
                New CheckBoxPreferenceBinder(chkAutoMakeOUT)))
            .Add(New PreferenceMapper(Of Boolean)("WantVisualStyles", False, _
                New CheckBoxPreferenceBinder(chkVisualStyles), AddressOf SetMustRebootBoolean))
            .Add(New PreferenceMapper(Of Boolean)("Macintoshize", False, _
                New CheckBoxPreferenceBinder(chkMacintoshize)))
            .Add(New PreferenceMapper(Of Integer)("VarMonitorColorSet", 1, _
                New RadioButtonsPreferenceBinder(1, rdbStyleUniSim, rdbStyleSpice)))
            .Add(New PreferenceMapper(Of Boolean)("SamplingMonitorOn", False, _
                New CheckBoxPreferenceBinder(chkAutoSample)))
            .Add(New PreferenceMapper(Of Integer)("DefaultSamplingFreq", 100, _
                New TrackBarPreferenceBinder(tkbSampleFreq)))
            .Add(New PreferenceMapper(Of Integer)("ActionOnNewPOU", 1, _
                New RadioButtonsPreferenceBinder(1, rbdNewPOUInstance, rdbNewTask, rdbIgnoreNewPOUs)))
            .Add(New PreferenceMapper(Of Integer)("PerformanceProfile", 2, _
                New RadioButtonsPreferenceBinder(1, rdbMinimizeWorkload, rdbBalancePerformance, rdbMaximizePerformance), AddressOf SetMustRebootInteger))
        End With
    End Sub

    Private Sub PreferencesForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PreparePreferencesSet()
        m_PreferencesSet.SetToGUI()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Hide()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        m_PreferencesSet.SetFromGUI()
        If m_MustReboot Then MessageBox.Show( _
"One or more preferences you changed will only be effective once you restart UniSim", _
UniSimVersion.VersionInfo.PrintableDescriptionForTool(), MessageBoxButtons.OK, _
MessageBoxIcon.Exclamation)
        Me.Hide()
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        UnisimClassLibrary.Macintoshize(Me)

    End Sub
End Class