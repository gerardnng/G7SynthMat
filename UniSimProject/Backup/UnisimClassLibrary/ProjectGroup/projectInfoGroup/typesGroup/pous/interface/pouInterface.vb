Imports System.Xml
Imports System.Collections.Generic
Public Class pouInterface
    Implements IXMLSerializable

    Protected m_pou As pou    'Riferimento al proprio pou
    Protected m_variablesLists As VariablesLists    'Lista contenente le altre lista di variabili
    Protected m_returnType As String
    Protected m_localVars As VariablesList
    Protected m_tempVars As VariablesList
    Protected m_inputVars As VariablesList
    Protected m_outputVars As VariablesList
    Protected m_inOutVars As VariablesList
    Protected m_externalVars As VariablesList
    Protected m_globalVars As VariablesList
    Protected m_accesslVars As VariablesList

    Property variablesLists() As VariablesLists
        Get
            variablesLists = m_variablesLists
        End Get
        Set(ByVal Value As VariablesLists)
            m_variablesLists = Value
        End Set
    End Property
    Property localVars() As VariablesList
        Get
            localVars = m_localVars
        End Get
        Set(ByVal Value As VariablesList)
            m_localVars = Value
        End Set

    End Property
    Property tempVars() As VariablesList
        Get
            tempVars = m_tempVars
        End Get
        Set(ByVal Value As VariablesList)
            m_tempVars = Value
        End Set

    End Property
    Property inputVars() As VariablesList
        Get
            inputVars = m_inputVars
        End Get
        Set(ByVal Value As VariablesList)
            m_inputVars = Value
        End Set

    End Property
    Property outputVars() As VariablesList
        Get
            outputVars = m_outputVars
        End Get
        Set(ByVal Value As VariablesList)
            m_outputVars = Value
        End Set

    End Property
    Property inOutVars() As VariablesList
        Get
            inOutVars = m_inOutVars
        End Get
        Set(ByVal Value As VariablesList)
            m_inOutVars = Value
        End Set

    End Property
    Property externalVars() As VariablesList
        Get
            externalVars = m_externalVars
        End Get
        Set(ByVal Value As VariablesList)
            m_externalVars = Value
        End Set

    End Property
    Property accesslVars() As VariablesList
        Get
            accesslVars = m_accesslVars
        End Get
        Set(ByVal Value As VariablesList)
            m_accesslVars = Value
        End Set

    End Property
    Property globalVars() As VariablesList
        Get
            globalVars = m_globalVars
        End Get
        Set(ByVal Value As VariablesList)
            m_globalVars = Value
        End Set
    End Property
    Private Sub FillExternalVarsList()
        ' Produce documenti più compatti e funzionalmente equivalente non scrivendo
        ' l'elenco di variabili da "importare" nella <externalVars> della POU
        If Not (Preferences.GetBoolean("DeclareExternsUsed", True)) Then Exit Sub
        'Seleziona il tipo di body
        Select Case m_pou.Body.ReadBodyType
            Case EnumBodyType.tSFC
                Dim m_sfc As Sfc = m_pou.Body.ReadSfc
                FillExternalVarsList(m_sfc)
        End Select
    End Sub
    Private Sub FillExternalVarsList(ByVal RefSfc As Sfc)
        'Legge le variabili utilizzate nel singolo sfc e le memorizza nella lista delle
        'variabili esterne se non sono presenti nelle altre

        'Legge le variabili external dalle transizioni
        For Each T As GraphicalTransition In RefSfc.GraphicalTransitionsList
            Dim a As List(Of BaseVariable) = T.ReadCondition.GetUsedVariablesList
            For Each V As BaseVariable In T.ReadCondition.GetUsedVariablesList
                'Controlla se non è una variabile presente in altre liste e aggiunge la variabile nelle variabili esterne se non è già presente
                If m_localVars.IndexOf(V) = -1 And m_outputVars.IndexOf(V) = -1 And m_externalVars.IndexOf(V) = -1 Then
                    m_externalVars.Add(V)
                End If
            Next
        Next T
        'Legge le variabili external dalle azioni delle fasi
        For Each S As GraphicalStep In RefSfc.ReadStepList
            For Each A As GraphicalAction In S.ReadListActions
                'Controlla se non è una variabile presente in altre liste e aggiunge la variabile nelle variabili esterne se non è già presente
                If m_localVars.IndexOf(A.Variable) = -1 And m_outputVars.IndexOf(A.Variable) = -1 And m_externalVars.IndexOf(A.Variable) = -1 Then
                    m_externalVars.Add(A.Variable)
                End If
            Next A
        Next S
        'Legge ricorsivamente le variabili utilizzate nei body delle macroazioni
        For Each MS As GraphicalMacroStep In RefSfc.ReadMacroStepList
            'Chiamata ricorsiva
            FillExternalVarsList(MS.ReadBody.ReadSfc)
        Next MS
    End Sub
    Public Function FindVariableByName(ByVal Value As String) As BaseVariable
        FindVariableByName = m_localVars.FindVariableByName(Value)
        If Not IsNothing(FindVariableByName) Then
            Exit Function
        End If
        FindVariableByName = m_tempVars.FindVariableByName(Value)
        If Not IsNothing(FindVariableByName) Then
            Exit Function
        End If
        FindVariableByName = m_inputVars.FindVariableByName(Value)
        If Not IsNothing(FindVariableByName) Then
            Exit Function
        End If
        FindVariableByName = m_outputVars.FindVariableByName(Value)
        If Not IsNothing(FindVariableByName) Then
            Exit Function
        End If
        FindVariableByName = m_inOutVars.FindVariableByName(Value)
        If Not IsNothing(FindVariableByName) Then
            Exit Function
        End If
        'non si possono eseguire ricerche sulle variabili esterne poichè
        'UniSim salva le variabili globali usate da un SFC tra le sue
        'variabili esterne. Fino a prima della 0.4.6A, c'era un bug
        'per cui le variabili esterne non venivano mai caricate mentre
        'si apriva il progetto. Questo bug è stato corretto nella 0.4.6A
        'da Enrico Granata. Correggendo il bug si è dato luogo alla situazione
        'in cui gli SFC non rilevavano le modifiche fatte alle variabili
        'globali in quanto andavano a leggere le variabili esterne
        '(su cui l'utente non può intervenire) ==> le variabili esterne non
        'possono più essere utilizzate per retrocompatibilità
        'FindVariableByName = m_externalVars.FindVariableByName(Value)
        'If Not IsNothing(FindVariableByName) Then
        'Exit Function
        'End If
        FindVariableByName = m_globalVars.FindVariableByName(Value)
        If Not IsNothing(FindVariableByName) Then
            Exit Function
        End If
        FindVariableByName = m_accesslVars.FindVariableByName(Value)
        If Not IsNothing(FindVariableByName) Then
            Exit Function
        End If
    End Function
    Sub New(ByVal src As pouInterface)
        m_variablesLists = New VariablesLists()
        m_localVars = src.m_localVars.CreateInstance()
        m_tempVars = src.m_tempVars.CreateInstance()
        m_inputVars = src.m_inputVars.CreateInstance()
        m_outputVars = src.m_outputVars.CreateInstance()
        m_inOutVars = src.m_inOutVars.CreateInstance()
        m_externalVars = src.m_externalVars.CreateInstance()
        m_globalVars = src.m_globalVars.CreateInstance()
        m_accesslVars = src.m_accesslVars.CreateInstance()
        m_variablesLists.Add(m_localVars)
        m_variablesLists.Add(m_tempVars)
        m_variablesLists.Add(m_inputVars)
        m_variablesLists.Add(m_outputVars)
        m_variablesLists.Add(m_inOutVars)
        m_variablesLists.Add(m_externalVars)
        m_variablesLists.Add(m_globalVars)
        m_variablesLists.Add(m_accesslVars)
    End Sub
    Sub New(ByVal RefPou As pou)
        m_pou = RefPou
        m_variablesLists = New VariablesLists
        ' Imposta i nomi degli elenchi di variabili di modo che siano
        ' visibili anche prima di salvare e aprire un progetto
        m_localVars = New VariablesList(RefPou.Name & " - Local variables")
        m_tempVars = New VariablesList(RefPou.Name & " - Temporary variables")
        m_inputVars = New VariablesList(RefPou.Name & " - Input variables")
        m_outputVars = New VariablesList(RefPou.Name & " - Output variables")
        m_inOutVars = New VariablesList(RefPou.Name & " - InOut variables")
        m_externalVars = New VariablesList(RefPou.Name & " - External variables")
        m_globalVars = New VariablesList(RefPou.Name & " - Global variables")
        m_accesslVars = New VariablesList(RefPou.Name & " - Accessible variables")
        'Aggiunge le liste alla lista m_variablesLists
        m_variablesLists.Add(m_localVars)
        m_variablesLists.Add(m_tempVars)
        m_variablesLists.Add(m_inputVars)
        m_variablesLists.Add(m_outputVars)
        m_variablesLists.Add(m_inOutVars)
        m_variablesLists.Add(m_externalVars)
        m_variablesLists.Add(m_globalVars)
        m_variablesLists.Add(m_accesslVars)
    End Sub
    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter) Implements IXMLSerializable.xmlExport

        'Riempie le liste di variabili
        FillExternalVarsList()

        'interface
        RefXMLProjectWriter.WriteStartElement("interface")

        'localVars *
        RefXMLProjectWriter.WriteStartElement("localVars")
        m_localVars.xmlExport(RefXMLProjectWriter)
        RefXMLProjectWriter.WriteEndElement() 'inputVars()

        'tempVars *
        RefXMLProjectWriter.WriteStartElement("tempVars")
        m_tempVars.xmlExport(RefXMLProjectWriter)
        RefXMLProjectWriter.WriteEndElement() 'tempVars()

        'inputVars *
        RefXMLProjectWriter.WriteStartElement("inputVars")
        m_inputVars.xmlExport(RefXMLProjectWriter)
        RefXMLProjectWriter.WriteEndElement() 'inputVars()

        'outputVars *
        RefXMLProjectWriter.WriteStartElement("outputVars")
        m_outputVars.xmlExport(RefXMLProjectWriter)
        RefXMLProjectWriter.WriteEndElement() 'outputVars()

        'inOutVars *
        RefXMLProjectWriter.WriteStartElement("inOutVars")
        m_inOutVars.xmlExport(RefXMLProjectWriter)
        RefXMLProjectWriter.WriteEndElement() 'inOutVars()

        'externalVars *
        RefXMLProjectWriter.WriteStartElement("externalVars")
        m_externalVars.xmlExport(RefXMLProjectWriter)
        RefXMLProjectWriter.WriteEndElement() 'externalVars()

        'accesslVars *
        RefXMLProjectWriter.WriteStartElement("accessVars")
        m_accesslVars.xmlExport(RefXMLProjectWriter)
        RefXMLProjectWriter.WriteEndElement() 'accesslVars()

        'globalVars *
        RefXMLProjectWriter.WriteStartElement("globalVars")
        m_globalVars.xmlExport(RefXMLProjectWriter)
        RefXMLProjectWriter.WriteEndElement() 'accesslVars()

        RefXMLProjectWriter.WriteEndElement() 'interface
    End Sub
    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader) Implements IXMLSerializable.xmlImport
        'Memorizza la profondità del nodo
        Dim NodeDepth As Integer = RefXmlProjectReader.Depth
        'Si sposta sul nodo successivo
        RefXmlProjectReader.Read()

        'Scorre fino alla fine di pou
        While RefXmlProjectReader.Depth > NodeDepth
            Select Case RefXmlProjectReader.Name
                Case "localVars"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                        localVars.xmlImport(RefXmlProjectReader)
                        'Assegna il nome
                        localVars.Name = m_pou.Name & " - Local variables"
                    End If
                Case "inputVars"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                        inputVars.xmlImport(RefXmlProjectReader)
                        'Assegna il nome
                        inputVars.Name = m_pou.Name & " - Input variables"
                    End If
                Case "outputVars"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                        outputVars.xmlImport(RefXmlProjectReader)
                        'Assegna il nome
                        outputVars.Name = m_pou.Name & " - Output variables"
                    End If
                Case "externalVars"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                        externalVars.xmlImport(RefXmlProjectReader)
                        'Assegna il nome
                        externalVars.Name = m_pou.Name & " - External variables"
                    End If
                Case "tempVars"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                        tempVars.xmlImport(RefXmlProjectReader)
                        'Assegna il nome
                        tempVars.Name = m_pou.Name & " - Temporary variables"
                    End If
                Case "accessVars"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                        accesslVars.xmlImport(RefXmlProjectReader)
                        'Assegna il nome
                        accesslVars.Name = m_pou.Name & " - Accessible variables"
                    End If
                Case "inOutVars"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                        inOutVars.xmlImport(RefXmlProjectReader)
                        'Assegna il nome
                        inOutVars.Name = m_pou.Name & " - InOut variables"
                    End If
                Case "globalVars"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                        globalVars.xmlImport(RefXmlProjectReader)
                        'Assegna il nome
                        globalVars.Name = m_pou.Name & " - Global variables"
                    End If
            End Select
            'Si sposta sul nodo successivo
            RefXmlProjectReader.Read()

        End While
    End Sub


    Public Function CreateInstance() As pouInterface
        Return New pouInterface(Me)
    End Function

    ' Esegui il reset di tutte le variabili associate alla POU
    Private Sub ResetImpl(ByVal vl As VariablesList)
        If Not IsNothing(vl) Then vl.Reset()
    End Sub

    Public Sub Reset()
        ResetImpl(Me.accesslVars)
        ResetImpl(Me.externalVars)
        ResetImpl(Me.globalVars)
        ResetImpl(Me.inOutVars)
        ResetImpl(Me.inputVars)
        ResetImpl(Me.inputVars)
        ResetImpl(Me.localVars)
        ResetImpl(Me.outputVars)
        ResetImpl(Me.tempVars)
        If Not IsNothing(Me.variablesLists) Then Me.variablesLists.Reset()
    End Sub

    Public ReadOnly Property Variables(ByVal Name As String) As BaseVariable
        Get
            Return FindVariableByName(Name)
        End Get
    End Property

    Public Property pou() As pou
        Get
            Return m_pou
        End Get
        Friend Set(ByVal p As pou)
            m_pou = p
        End Set
    End Property

End Class

