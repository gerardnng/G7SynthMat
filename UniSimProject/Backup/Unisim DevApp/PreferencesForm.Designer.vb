<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PreferencesForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PreferencesForm))
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.chkMacintoshize = New System.Windows.Forms.CheckBox
        Me.chkVisualStyles = New System.Windows.Forms.CheckBox
        Me.chkAutomakeGlobVarList = New System.Windows.Forms.CheckBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rdbDefaultFBD = New System.Windows.Forms.RadioButton
        Me.rdbDefaultLD = New System.Windows.Forms.RadioButton
        Me.rdbDefaultSFC = New System.Windows.Forms.RadioButton
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.chkNoValidation = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.chkDeclareExternsUsage = New System.Windows.Forms.CheckBox
        Me.chkFirstStepInitial = New System.Windows.Forms.CheckBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.chkUseArrowsPN = New System.Windows.Forms.CheckBox
        Me.chkKeepConnectState = New System.Windows.Forms.CheckBox
        Me.chkPowerFlowColor = New System.Windows.Forms.CheckBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.TabPage6 = New System.Windows.Forms.TabPage
        Me.chkFBDKeepConnectState = New System.Windows.Forms.CheckBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.TabPage4 = New System.Windows.Forms.TabPage
        Me.Label8 = New System.Windows.Forms.Label
        Me.tkbSampleFreq = New System.Windows.Forms.TrackBar
        Me.Label7 = New System.Windows.Forms.Label
        Me.chkAutoSample = New System.Windows.Forms.CheckBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.rdbStyleSpice = New System.Windows.Forms.RadioButton
        Me.rdbStyleUniSim = New System.Windows.Forms.RadioButton
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.TabPage5 = New System.Windows.Forms.TabPage
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.rdbMaximizePerformance = New System.Windows.Forms.RadioButton
        Me.rdbBalancePerformance = New System.Windows.Forms.RadioButton
        Me.rdbMinimizeWorkload = New System.Windows.Forms.RadioButton
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.rdbIgnoreNewPOUs = New System.Windows.Forms.RadioButton
        Me.rdbNewTask = New System.Windows.Forms.RadioButton
        Me.rbdNewPOUInstance = New System.Windows.Forms.RadioButton
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.RadioButton1 = New System.Windows.Forms.RadioButton
        Me.RadioButton2 = New System.Windows.Forms.RadioButton
        Me.chkAutoMakeOUT = New System.Windows.Forms.CheckBox
        Me.TabControl1.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        CType(Me.tkbSampleFreq, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage6)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.HotTrack = True
        Me.TabControl1.ImageList = Me.ImageList1
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(466, 385)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage3
        '
        Me.TabPage3.BackColor = System.Drawing.Color.Navy
        Me.TabPage3.Controls.Add(Me.Panel5)
        Me.TabPage3.Controls.Add(Me.chkAutomakeGlobVarList)
        Me.TabPage3.Controls.Add(Me.Panel1)
        Me.TabPage3.Controls.Add(Me.Label12)
        Me.TabPage3.Controls.Add(Me.Label4)
        Me.TabPage3.Controls.Add(Me.chkNoValidation)
        Me.TabPage3.Controls.Add(Me.Label1)
        Me.TabPage3.ImageIndex = 0
        Me.TabPage3.Location = New System.Drawing.Point(4, 23)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(458, 358)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "General"
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.chkMacintoshize)
        Me.Panel5.Controls.Add(Me.chkVisualStyles)
        Me.Panel5.Location = New System.Drawing.Point(16, 115)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(419, 80)
        Me.Panel5.TabIndex = 8
        '
        'chkMacintoshize
        '
        Me.chkMacintoshize.AutoSize = True
        Me.chkMacintoshize.Location = New System.Drawing.Point(28, 48)
        Me.chkMacintoshize.Name = "chkMacintoshize"
        Me.chkMacintoshize.Size = New System.Drawing.Size(138, 17)
        Me.chkMacintoshize.TabIndex = 8
        Me.chkMacintoshize.Text = "Macintosh-like windows"
        Me.chkMacintoshize.UseVisualStyleBackColor = True
        '
        'chkVisualStyles
        '
        Me.chkVisualStyles.AutoSize = True
        Me.chkVisualStyles.Location = New System.Drawing.Point(28, 25)
        Me.chkVisualStyles.Name = "chkVisualStyles"
        Me.chkVisualStyles.Size = New System.Drawing.Size(231, 17)
        Me.chkVisualStyles.TabIndex = 8
        Me.chkVisualStyles.Text = "Modern GUI style on Windows XP and later"
        Me.chkVisualStyles.UseVisualStyleBackColor = True
        '
        'chkAutomakeGlobVarList
        '
        Me.chkAutomakeGlobVarList.AutoSize = True
        Me.chkAutomakeGlobVarList.Location = New System.Drawing.Point(16, 64)
        Me.chkAutomakeGlobVarList.Name = "chkAutomakeGlobVarList"
        Me.chkAutomakeGlobVarList.Size = New System.Drawing.Size(234, 17)
        Me.chkAutomakeGlobVarList.TabIndex = 5
        Me.chkAutomakeGlobVarList.Text = "Create a global variables list for each project"
        Me.chkAutomakeGlobVarList.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rdbDefaultFBD)
        Me.Panel1.Controls.Add(Me.rdbDefaultLD)
        Me.Panel1.Controls.Add(Me.rdbDefaultSFC)
        Me.Panel1.Location = New System.Drawing.Point(16, 231)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(152, 105)
        Me.Panel1.TabIndex = 4
        '
        'rdbDefaultFBD
        '
        Me.rdbDefaultFBD.AutoSize = True
        Me.rdbDefaultFBD.Location = New System.Drawing.Point(28, 62)
        Me.rdbDefaultFBD.Name = "rdbDefaultFBD"
        Me.rdbDefaultFBD.Size = New System.Drawing.Size(46, 17)
        Me.rdbDefaultFBD.TabIndex = 1
        Me.rdbDefaultFBD.TabStop = True
        Me.rdbDefaultFBD.Text = "FBD"
        Me.rdbDefaultFBD.UseVisualStyleBackColor = True
        '
        'rdbDefaultLD
        '
        Me.rdbDefaultLD.AutoSize = True
        Me.rdbDefaultLD.Location = New System.Drawing.Point(28, 39)
        Me.rdbDefaultLD.Name = "rdbDefaultLD"
        Me.rdbDefaultLD.Size = New System.Drawing.Size(58, 17)
        Me.rdbDefaultLD.TabIndex = 0
        Me.rdbDefaultLD.Text = "Ladder"
        Me.rdbDefaultLD.UseVisualStyleBackColor = True
        '
        'rdbDefaultSFC
        '
        Me.rdbDefaultSFC.AutoSize = True
        Me.rdbDefaultSFC.Checked = True
        Me.rdbDefaultSFC.Location = New System.Drawing.Point(28, 16)
        Me.rdbDefaultSFC.Name = "rdbDefaultSFC"
        Me.rdbDefaultSFC.Size = New System.Drawing.Size(45, 17)
        Me.rdbDefaultSFC.TabIndex = 0
        Me.rdbDefaultSFC.TabStop = True
        Me.rdbDefaultSFC.Text = "SFC"
        Me.rdbDefaultSFC.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(13, 99)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(72, 13)
        Me.Label12.TabIndex = 3
        Me.Label12.Text = "Look and feel"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(13, 215)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(160, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Default language for new POUs:"
        '
        'chkNoValidation
        '
        Me.chkNoValidation.AutoSize = True
        Me.chkNoValidation.Location = New System.Drawing.Point(16, 41)
        Me.chkNoValidation.Name = "chkNoValidation"
        Me.chkNoValidation.Size = New System.Drawing.Size(131, 17)
        Me.chkNoValidation.TabIndex = 1
        Me.chkNoValidation.Text = "Don't validate projects"
        Me.chkNoValidation.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(342, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Preferences you set here will affect the behaviour of UniSim as a whole"
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.Navy
        Me.TabPage1.Controls.Add(Me.chkDeclareExternsUsage)
        Me.TabPage1.Controls.Add(Me.chkFirstStepInitial)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.ImageIndex = 2
        Me.TabPage1.Location = New System.Drawing.Point(4, 23)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(458, 358)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "SFC"
        '
        'chkDeclareExternsUsage
        '
        Me.chkDeclareExternsUsage.AutoSize = True
        Me.chkDeclareExternsUsage.Location = New System.Drawing.Point(16, 79)
        Me.chkDeclareExternsUsage.Name = "chkDeclareExternsUsage"
        Me.chkDeclareExternsUsage.Size = New System.Drawing.Size(174, 17)
        Me.chkDeclareExternsUsage.TabIndex = 3
        Me.chkDeclareExternsUsage.Text = "Fill <externalVars> when saving"
        Me.chkDeclareExternsUsage.UseVisualStyleBackColor = True
        '
        'chkFirstStepInitial
        '
        Me.chkFirstStepInitial.AutoSize = True
        Me.chkFirstStepInitial.Checked = True
        Me.chkFirstStepInitial.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkFirstStepInitial.Location = New System.Drawing.Point(16, 44)
        Me.chkFirstStepInitial.Name = "chkFirstStepInitial"
        Me.chkFirstStepInitial.Size = New System.Drawing.Size(171, 17)
        Me.chkFirstStepInitial.TabIndex = 1
        Me.chkFirstStepInitial.Text = "Mark first step created as initial"
        Me.chkFirstStepInitial.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(329, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Preferences you set here will only affect the SFC editor and simulator"
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.Color.Navy
        Me.TabPage2.Controls.Add(Me.chkUseArrowsPN)
        Me.TabPage2.Controls.Add(Me.chkKeepConnectState)
        Me.TabPage2.Controls.Add(Me.chkPowerFlowColor)
        Me.TabPage2.Controls.Add(Me.Label3)
        Me.TabPage2.ImageIndex = 1
        Me.TabPage2.Location = New System.Drawing.Point(4, 23)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(458, 358)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Ladder"
        '
        'chkUseArrowsPN
        '
        Me.chkUseArrowsPN.AutoSize = True
        Me.chkUseArrowsPN.Location = New System.Drawing.Point(16, 114)
        Me.chkUseArrowsPN.Name = "chkUseArrowsPN"
        Me.chkUseArrowsPN.Size = New System.Drawing.Size(314, 17)
        Me.chkUseArrowsPN.TabIndex = 11
        Me.chkUseArrowsPN.Text = "Use arrows instead of P/N for edge-driven coils and contacts"
        Me.chkUseArrowsPN.UseVisualStyleBackColor = True
        '
        'chkKeepConnectState
        '
        Me.chkKeepConnectState.AutoSize = True
        Me.chkKeepConnectState.Checked = True
        Me.chkKeepConnectState.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkKeepConnectState.Location = New System.Drawing.Point(16, 81)
        Me.chkKeepConnectState.Name = "chkKeepConnectState"
        Me.chkKeepConnectState.Size = New System.Drawing.Size(190, 17)
        Me.chkKeepConnectState.TabIndex = 10
        Me.chkKeepConnectState.Text = "Allow multiple connections in a row"
        Me.chkKeepConnectState.UseVisualStyleBackColor = True
        '
        'chkPowerFlowColor
        '
        Me.chkPowerFlowColor.AutoSize = True
        Me.chkPowerFlowColor.Location = New System.Drawing.Point(16, 47)
        Me.chkPowerFlowColor.Name = "chkPowerFlowColor"
        Me.chkPowerFlowColor.Size = New System.Drawing.Size(329, 17)
        Me.chkPowerFlowColor.TabIndex = 2
        Me.chkPowerFlowColor.Text = "Color circles according to power flow rather than variable's value"
        Me.chkPowerFlowColor.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(13, 12)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(342, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Preferences you set here will only affect the Ladder editor and simulator"
        '
        'TabPage6
        '
        Me.TabPage6.BackColor = System.Drawing.Color.Navy
        Me.TabPage6.Controls.Add(Me.chkAutoMakeOUT)
        Me.TabPage6.Controls.Add(Me.chkFBDKeepConnectState)
        Me.TabPage6.Controls.Add(Me.Label13)
        Me.TabPage6.ImageIndex = 5
        Me.TabPage6.Location = New System.Drawing.Point(4, 23)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Size = New System.Drawing.Size(458, 358)
        Me.TabPage6.TabIndex = 5
        Me.TabPage6.Text = "FBD"
        '
        'chkFBDKeepConnectState
        '
        Me.chkFBDKeepConnectState.AutoSize = True
        Me.chkFBDKeepConnectState.Checked = True
        Me.chkFBDKeepConnectState.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkFBDKeepConnectState.Location = New System.Drawing.Point(16, 50)
        Me.chkFBDKeepConnectState.Name = "chkFBDKeepConnectState"
        Me.chkFBDKeepConnectState.Size = New System.Drawing.Size(190, 17)
        Me.chkFBDKeepConnectState.TabIndex = 12
        Me.chkFBDKeepConnectState.Text = "Allow multiple connections in a row"
        Me.chkFBDKeepConnectState.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(13, 12)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(330, 13)
        Me.Label13.TabIndex = 11
        Me.Label13.Text = "Preferences you set here will only affect the FBD editor and simulator"
        '
        'TabPage4
        '
        Me.TabPage4.BackColor = System.Drawing.Color.Navy
        Me.TabPage4.Controls.Add(Me.Label8)
        Me.TabPage4.Controls.Add(Me.tkbSampleFreq)
        Me.TabPage4.Controls.Add(Me.Label7)
        Me.TabPage4.Controls.Add(Me.chkAutoSample)
        Me.TabPage4.Controls.Add(Me.Panel2)
        Me.TabPage4.Controls.Add(Me.Label6)
        Me.TabPage4.Controls.Add(Me.Label5)
        Me.TabPage4.ImageIndex = 3
        Me.TabPage4.Location = New System.Drawing.Point(4, 23)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(458, 358)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Variables windows"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(165, 224)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(282, 13)
        Me.Label8.TabIndex = 10
        Me.Label8.Text = "10 Hz                                                                       100 H" & _
            "z"
        '
        'tkbSampleFreq
        '
        Me.tkbSampleFreq.LargeChange = 10
        Me.tkbSampleFreq.Location = New System.Drawing.Point(157, 192)
        Me.tkbSampleFreq.Maximum = 100
        Me.tkbSampleFreq.Minimum = 10
        Me.tkbSampleFreq.Name = "tkbSampleFreq"
        Me.tkbSampleFreq.Size = New System.Drawing.Size(298, 42)
        Me.tkbSampleFreq.TabIndex = 9
        Me.tkbSampleFreq.TickFrequency = 10
        Me.tkbSampleFreq.Value = 10
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(13, 210)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(138, 13)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "Default sampling frequency:"
        '
        'chkAutoSample
        '
        Me.chkAutoSample.AutoSize = True
        Me.chkAutoSample.Checked = True
        Me.chkAutoSample.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAutoSample.Location = New System.Drawing.Point(16, 169)
        Me.chkAutoSample.Name = "chkAutoSample"
        Me.chkAutoSample.Size = New System.Drawing.Size(156, 17)
        Me.chkAutoSample.TabIndex = 7
        Me.chkAutoSample.Text = "Start sampling automatically"
        Me.chkAutoSample.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Controls.Add(Me.rdbStyleSpice)
        Me.Panel2.Controls.Add(Me.rdbStyleUniSim)
        Me.Panel2.Location = New System.Drawing.Point(16, 71)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(260, 76)
        Me.Panel2.TabIndex = 6
        Me.Panel2.Tag = "{MC_LEAVE_ALONE}"
        '
        'rdbStyleSpice
        '
        Me.rdbStyleSpice.AutoSize = True
        Me.rdbStyleSpice.BackColor = System.Drawing.Color.Black
        Me.rdbStyleSpice.ForeColor = System.Drawing.Color.Chartreuse
        Me.rdbStyleSpice.Location = New System.Drawing.Point(28, 39)
        Me.rdbStyleSpice.Name = "rdbStyleSpice"
        Me.rdbStyleSpice.Size = New System.Drawing.Size(141, 17)
        Me.rdbStyleSpice.TabIndex = 0
        Me.rdbStyleSpice.Tag = "{MC_LEAVE_ALONE}"
        Me.rdbStyleSpice.Text = "Green on black (PSpice)"
        Me.rdbStyleSpice.UseVisualStyleBackColor = False
        '
        'rdbStyleUniSim
        '
        Me.rdbStyleUniSim.AutoSize = True
        Me.rdbStyleUniSim.Checked = True
        Me.rdbStyleUniSim.ForeColor = System.Drawing.Color.Blue
        Me.rdbStyleUniSim.Location = New System.Drawing.Point(28, 16)
        Me.rdbStyleUniSim.Name = "rdbStyleUniSim"
        Me.rdbStyleUniSim.Size = New System.Drawing.Size(131, 17)
        Me.rdbStyleUniSim.TabIndex = 0
        Me.rdbStyleUniSim.TabStop = True
        Me.rdbStyleUniSim.Tag = "{MC_LEAVE_ALONE}"
        Me.rdbStyleUniSim.Text = "Blue on white (UniSim)"
        Me.rdbStyleUniSim.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(13, 45)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(73, 13)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Drawing style:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(13, 12)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(308, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Preferences you set here will affect the variables editor windows"
        '
        'TabPage5
        '
        Me.TabPage5.BackColor = System.Drawing.Color.Navy
        Me.TabPage5.Controls.Add(Me.Panel4)
        Me.TabPage5.Controls.Add(Me.Panel3)
        Me.TabPage5.Controls.Add(Me.Label11)
        Me.TabPage5.Controls.Add(Me.Label10)
        Me.TabPage5.Controls.Add(Me.Label9)
        Me.TabPage5.ImageIndex = 4
        Me.TabPage5.Location = New System.Drawing.Point(4, 23)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(458, 358)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "Simulation"
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.rdbMaximizePerformance)
        Me.Panel4.Controls.Add(Me.rdbBalancePerformance)
        Me.Panel4.Controls.Add(Me.rdbMinimizeWorkload)
        Me.Panel4.Location = New System.Drawing.Point(16, 205)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(439, 95)
        Me.Panel4.TabIndex = 6
        '
        'rdbMaximizePerformance
        '
        Me.rdbMaximizePerformance.AutoSize = True
        Me.rdbMaximizePerformance.Location = New System.Drawing.Point(28, 62)
        Me.rdbMaximizePerformance.Name = "rdbMaximizePerformance"
        Me.rdbMaximizePerformance.Size = New System.Drawing.Size(301, 17)
        Me.rdbMaximizePerformance.TabIndex = 0
        Me.rdbMaximizePerformance.Text = "High performance (preemptive simulation, maximum priority)"
        Me.rdbMaximizePerformance.UseVisualStyleBackColor = True
        '
        'rdbBalancePerformance
        '
        Me.rdbBalancePerformance.AutoSize = True
        Me.rdbBalancePerformance.Location = New System.Drawing.Point(28, 39)
        Me.rdbBalancePerformance.Name = "rdbBalancePerformance"
        Me.rdbBalancePerformance.Size = New System.Drawing.Size(381, 17)
        Me.rdbBalancePerformance.TabIndex = 0
        Me.rdbBalancePerformance.Text = "Balance workload and performance (preemptive simulation, average priority)"
        Me.rdbBalancePerformance.UseVisualStyleBackColor = True
        '
        'rdbMinimizeWorkload
        '
        Me.rdbMinimizeWorkload.AutoSize = True
        Me.rdbMinimizeWorkload.Checked = True
        Me.rdbMinimizeWorkload.Location = New System.Drawing.Point(28, 16)
        Me.rdbMinimizeWorkload.Name = "rdbMinimizeWorkload"
        Me.rdbMinimizeWorkload.Size = New System.Drawing.Size(301, 17)
        Me.rdbMinimizeWorkload.TabIndex = 0
        Me.rdbMinimizeWorkload.TabStop = True
        Me.rdbMinimizeWorkload.Text = "Low workload (non preemptive simulation, minimum priority)"
        Me.rdbMinimizeWorkload.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.rdbIgnoreNewPOUs)
        Me.Panel3.Controls.Add(Me.rdbNewTask)
        Me.Panel3.Controls.Add(Me.rbdNewPOUInstance)
        Me.Panel3.Location = New System.Drawing.Point(16, 67)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(222, 95)
        Me.Panel3.TabIndex = 6
        '
        'rdbIgnoreNewPOUs
        '
        Me.rdbIgnoreNewPOUs.AutoSize = True
        Me.rdbIgnoreNewPOUs.Location = New System.Drawing.Point(28, 62)
        Me.rdbIgnoreNewPOUs.Name = "rdbIgnoreNewPOUs"
        Me.rdbIgnoreNewPOUs.Size = New System.Drawing.Size(77, 17)
        Me.rdbIgnoreNewPOUs.TabIndex = 0
        Me.rdbIgnoreNewPOUs.Text = "Do nothing"
        Me.rdbIgnoreNewPOUs.UseVisualStyleBackColor = True
        '
        'rdbNewTask
        '
        Me.rdbNewTask.AutoSize = True
        Me.rdbNewTask.Location = New System.Drawing.Point(28, 39)
        Me.rdbNewTask.Name = "rdbNewTask"
        Me.rdbNewTask.Size = New System.Drawing.Size(111, 17)
        Me.rdbNewTask.TabIndex = 0
        Me.rdbNewTask.Text = "Create a new task"
        Me.rdbNewTask.UseVisualStyleBackColor = True
        '
        'rbdNewPOUInstance
        '
        Me.rbdNewPOUInstance.AutoSize = True
        Me.rbdNewPOUInstance.Checked = True
        Me.rbdNewPOUInstance.Location = New System.Drawing.Point(28, 16)
        Me.rbdNewPOUInstance.Name = "rbdNewPOUInstance"
        Me.rbdNewPOUInstance.Size = New System.Drawing.Size(157, 17)
        Me.rbdNewPOUInstance.TabIndex = 0
        Me.rbdNewPOUInstance.TabStop = True
        Me.rbdNewPOUInstance.Text = "Create a new POU instance"
        Me.rbdNewPOUInstance.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(13, 179)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(199, 13)
        Me.Label11.TabIndex = 5
        Me.Label11.Text = "Simulation performance/workload profile:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(13, 41)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(79, 13)
        Me.Label10.TabIndex = 5
        Me.Label10.Text = "For new POUs:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(13, 12)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(400, 13)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "Preferences you set here will affect tasks, POU instances and the simulation engi" & _
            "ne"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "SVPIcon.ico")
        Me.ImageList1.Images.SetKeyName(1, "LD.ico")
        Me.ImageList1.Images.SetKeyName(2, "SFC.ico")
        Me.ImageList1.Images.SetKeyName(3, "VarList.ico")
        Me.ImageList1.Images.SetKeyName(4, "servicerunning.ico")
        Me.ImageList1.Images.SetKeyName(5, "FBD.ico")
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button1.Location = New System.Drawing.Point(16, 403)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.Control
        Me.Button2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button2.Location = New System.Drawing.Point(403, 403)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Location = New System.Drawing.Point(28, 39)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(58, 17)
        Me.RadioButton1.TabIndex = 0
        Me.RadioButton1.Text = "Ladder"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Checked = True
        Me.RadioButton2.Location = New System.Drawing.Point(28, 16)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(45, 17)
        Me.RadioButton2.TabIndex = 0
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "SFC"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'chkAutoMakeOUT
        '
        Me.chkAutoMakeOUT.AutoSize = True
        Me.chkAutoMakeOUT.Location = New System.Drawing.Point(16, 82)
        Me.chkAutoMakeOUT.Name = "chkAutoMakeOUT"
        Me.chkAutoMakeOUT.Size = New System.Drawing.Size(307, 17)
        Me.chkAutoMakeOUT.TabIndex = 13
        Me.chkAutoMakeOUT.Text = "Immediately create an OUT variable block in function POUs"
        Me.chkAutoMakeOUT.UseVisualStyleBackColor = True
        '
        'PreferencesForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Navy
        Me.ClientSize = New System.Drawing.Size(503, 439)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TabControl1)
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PreferencesForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Preferences"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage6.ResumeLayout(False)
        Me.TabPage6.PerformLayout()
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage4.PerformLayout()
        CType(Me.tkbSampleFreq, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage5.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkNoValidation As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rdbDefaultLD As System.Windows.Forms.RadioButton
    Friend WithEvents rdbDefaultSFC As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents chkFirstStepInitial As System.Windows.Forms.CheckBox
    Friend WithEvents chkAutomakeGlobVarList As System.Windows.Forms.CheckBox
    Friend WithEvents chkDeclareExternsUsage As System.Windows.Forms.CheckBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rdbStyleSpice As System.Windows.Forms.RadioButton
    Friend WithEvents rdbStyleUniSim As System.Windows.Forms.RadioButton
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents chkAutoSample As System.Windows.Forms.CheckBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents tkbSampleFreq As System.Windows.Forms.TrackBar
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents rdbNewTask As System.Windows.Forms.RadioButton
    Friend WithEvents rbdNewPOUInstance As System.Windows.Forms.RadioButton
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents rdbIgnoreNewPOUs As System.Windows.Forms.RadioButton
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents rdbMaximizePerformance As System.Windows.Forms.RadioButton
    Friend WithEvents rdbBalancePerformance As System.Windows.Forms.RadioButton
    Friend WithEvents rdbMinimizeWorkload As System.Windows.Forms.RadioButton
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents chkMacintoshize As System.Windows.Forms.CheckBox
    Friend WithEvents chkVisualStyles As System.Windows.Forms.CheckBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents chkPowerFlowColor As System.Windows.Forms.CheckBox
    Friend WithEvents rdbDefaultFBD As System.Windows.Forms.RadioButton
    Friend WithEvents chkUseArrowsPN As System.Windows.Forms.CheckBox
    Friend WithEvents chkKeepConnectState As System.Windows.Forms.CheckBox
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents chkFBDKeepConnectState As System.Windows.Forms.CheckBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents chkAutoMakeOUT As System.Windows.Forms.CheckBox
End Class
