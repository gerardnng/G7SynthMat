' Questo modulo incorpora le funzioni di gestione della versione di UniSim e della verifica sulla compatibilità
' dei progetti rispetto alle diverse versioni di UniSim e ai diversi software compatibili con l'XML Formats

Public Enum ReleaseKind
    ' Release molto instabile: sviluppo attivo in corso
    Alpha
    ' Release instabile: sviluppo e correzione errori
    Beta
    ' Release quasi stabile: correzione errori
    ReleaseCandidate
    ' Release stabile: manutenzione
    Final
End Enum

Public Enum IncompatibilitySeriousnessLevel
    ' Una informazione aggiuntiva di poca importanza: il progetto può essere aperto, simulato e salvato
    Information
    ' Un problema che potrebbe impedire la simulazione o l'editing di parti e che rende poco consigliabile il salvataggio
    Warning
    ' Un problema che rende anche la visualizzazione e l'editing del progetto impossibile
    [Error]
End Enum

Public Class ProjectIncompatibility

    Private m_Level As IncompatibilitySeriousnessLevel
    Private m_Description As String
    Private m_Solution As String

    Public Sub New(ByVal l As IncompatibilitySeriousnessLevel, ByVal d As String, _
        ByVal s As String)
        m_Level = l
        m_Description = d
        m_Solution = s
    End Sub

    Public ReadOnly Property Level() As IncompatibilitySeriousnessLevel
        Get
            Return m_Level
        End Get
    End Property
    Public ReadOnly Property Description() As String
        Get
            Return m_Description
        End Get
    End Property
    Public ReadOnly Property Solution() As String
        Get
            Return m_Solution
        End Get
    End Property

End Class

Public Class UniSimVersion

    Private m_ProgramName As String = "UniSim"
    Private m_ReleaseData As String = "B"
    Private m_Version As New Version(0, 5, 0, Asc(m_ReleaseData))
    Private m_AuthorName As String = "Pierangelo Di Sanzo"
    Private m_CorporateName As String = "Universita di Napoli Federico II"
    Private m_CorporateURL As String = "www.unina.it"
    Private m_CreationDateTime As String = "2007-12-31T23:59:59Z"
    Private m_ReleaseClass As ReleaseKind = ReleaseKind.Final

    Private Shared UniSimVerSingleton As New UniSimVersion()

    Private Sub New()
    End Sub

    Public ReadOnly Property ProgramName() As String
        Get
            Return m_ProgramName
        End Get
    End Property

    Public ReadOnly Property VersionString() As String
        Get
            Return m_Version.ToString(3)
        End Get
    End Property

    Public ReadOnly Property AuthorName() As String
        Get
            Return m_AuthorName
        End Get
    End Property

    Public ReadOnly Property CorporateName() As String
        Get
            Return m_CorporateName
        End Get
    End Property

    Public ReadOnly Property CorporateURL() As String
        Get
            Return m_CorporateURL
        End Get
    End Property

    Public ReadOnly Property CreationMoment() As String
        Get
            Return m_CreationDateTime
        End Get
    End Property

    Public ReadOnly Property ProductRelease() As String
        Get
            Return m_ReleaseData
        End Get
    End Property

    Public ReadOnly Property ReleaseStabilityLevel() As ReleaseKind
        Get
            Return m_ReleaseClass
        End Get
    End Property

    Public Shared ReadOnly Property VersionInfo() As UniSimVersion
        Get
            Return UniSimVersion.UniSimVerSingleton
        End Get
    End Property

    ' Ritorna una descrizione in formato standard della versione di UniSim in uso
    ' adatta per le finestre di About e per le barre del titolo
    ' ToolName è il tool in uso (nel 90% dei casi "DevApp")
    ' Passare "" se si è nella ClassLibrary
    Public Function PrintableDescriptionForTool(Optional ByVal ToolName As String = "DevApp") As String
        Dim ret As String = ProgramName + " " + ToolName + New String(" ", IIf(ToolName <> "", 1, 0))
        ret += VersionString + ProductRelease
        Select Case Me.ReleaseStabilityLevel
            Case ReleaseKind.Alpha
                ret += " (devel)"
            Case ReleaseKind.Beta
                ret += " (beta)"
            Case ReleaseKind.ReleaseCandidate
                ret += " (rc)"
        End Select
        Return ret
    End Function

    Public Function TestProjectCompatibility(ByVal header As fileHeader) As System.Collections.Generic.List(Of ProjectIncompatibility)
        Dim ret As New System.Collections.Generic.List(Of ProjectIncompatibility)
        If IsNothing(header) Then Return Nothing
        With header
            ' Se non è opera nostra...
            If .productName <> "UniSim" Then
                ' ...non possiamo garantire
                ret.Add(New ProjectIncompatibility(IncompatibilitySeriousnessLevel.Information, _
    "This project was not created by UniSim", _
    "There should be no problems opening and editing the project in UniSim, unless the original content producer used some extensions"))
                Return ret
            End If
            '  Da qui in poi, è un progetto UniSim
            ' -------------------------------------
            ' L'indicatore di versione v1.0 è usato da tutte le versioni di UniSim
            ' fino alla 0.4.5L e ancora dalla versione di La Brocca. Tutto ciò che
            ' è generato da quelle versioni funziona tranquillamente
            If .productVersion = "v1.0" Then Return ret
            ' L'indicatore 0.4.5 è usato da una fork non ufficiale e parzialmente
            ' incompatibile con le versioni ufficiali
            If .productVersion = "0.4.5" Then _
                ret.Add(New ProjectIncompatibility(IncompatibilitySeriousnessLevel.Error, _
    "This project was created by a non official and non standard version of UniSim", _
    "Forcing, stoppage and suspension actions will not work under this version of UniSim and projects with any of these inside will cause all kind of erratic behaviour. If you save the project, comments will not be available in the old release"))
            Try
                Dim projVersion As New Version(.productVersion)
                ' Incompatibilità sugli interi
                If (projVersion < New Version(0, 4, 7)) OrElse _
                   (projVersion = New Version(0, 4, 7) AndAlso .productRelease < "B") Then _
                    ret.Add(New ProjectIncompatibility(IncompatibilitySeriousnessLevel.Error, _
    "Integer variables no longer work as a counter since version 0.4.7B Final values will now be used as initial values", _
    "Any SFC transitions or Ladder contacts that use an integer variable will no longer work as expected" + vbCrLf + _
    "Use an explicit comparison in SFC, or a comparison block in LD to overcome this change" + vbCrLf + _
    "Arithmetic operations will return unexpected values unless you set a meaningful initial values and reset all variables"))
                ' Non sappiamo dire cosa succederà nel futuro. Avvisiamo che versioni successive
                ' potrebbero essere parzialmente incompatibili. Il confronto avviene tramite gli indicatori
                ' di versione e se questi sono uguali tramite il confronto della release
                If projVersion > m_Version OrElse _
                    (projVersion = m_Version AndAlso Asc(.productRelease.ToLower()) > Asc(m_ReleaseData.ToLower())) Then _
                    ret.Add(New ProjectIncompatibility(IncompatibilitySeriousnessLevel.Warning, _
        "This project was created by a later version of UniSim than the one you're using", _
        "Don't save this project unless you know for sure that it will work under this earlier release of Unisim"))
            Catch anything As Exception
            Finally
            End Try
            Return ret
        End With
    End Function

End Class
