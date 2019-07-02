Imports System.Xml
Imports System.Threading
Public Enum EnumBodyType
    tSFC
    tST
    tLD
    tIL
    tFBD
End Enum
Public Class body
    Implements IDocumentable, IExecutable

    Protected m_Name As String
    Protected m_BodyType As EnumBodyType
    Protected m_ResGlobalVariables As VariablesLists
    Protected m_pouInterface As pouInterface
    Protected m_Documentation As String
    Protected m_Implementation As IIEC61131LanguageImplementation
    'Per i body di tipo SFC 
    'Protected m_Sfc As Sfc
    'Per i body di tipo LADDER
    'Protected m_Ladder As Ladder
    'Eventi
    Public Event NameChanged(ByVal NewName As String)     'Notifica il cambiamento del nome
    Protected m_xmlSTBodyString As String

    Protected m_pouslist As Pous

    Public Property Name() As String Implements IDocumentable.Name
        Get
            Return m_Implementation.Name
        End Get
        Set(ByVal Value As String)
            m_Name = Value
            m_Implementation.Name = m_Name
            RaiseEvent NameChanged(m_Name)      'Notifica il cambiamento del nome
        End Set
    End Property
    Property ResGlobalVariables() As VariablesLists
        Get
            ResGlobalVariables = m_ResGlobalVariables
        End Get
        Set(ByVal Value As VariablesLists)
            m_ResGlobalVariables = Value
            'Comunica al contenuto le lista di variabili globali della risorsa
            m_Implementation.ResGlobalVariables = m_ResGlobalVariables
        End Set
    End Property
    Property PouInterface() As pouInterface
        Get
            PouInterface = m_pouInterface
        End Get
        Set(ByVal Value As pouInterface)
            m_pouInterface = Value
            'Comunica al contenuto l'interfaccia della pou
            m_Implementation.PouInterface = m_pouInterface
        End Set
    End Property
    Public Event Disposing()
    Public Sub New(ByVal PouName As String, ByVal Type As EnumBodyType, ByRef RefResGlobalVariables As VariablesLists, ByRef RefPouInterface As pouInterface, ByVal pouslist As Pous)
        m_BodyType = Type
        m_ResGlobalVariables = RefResGlobalVariables
        m_pouInterface = RefPouInterface
        Select Case Type
            Case EnumBodyType.tSFC
                m_Implementation = New Sfc(PouName, m_ResGlobalVariables, m_pouInterface, pouslist)
            Case EnumBodyType.tLD
                m_Implementation = New Ladder(PouName, m_ResGlobalVariables, m_pouInterface, pouslist)
            Case EnumBodyType.tFBD
                m_Implementation = New FBD(PouName, m_ResGlobalVariables, m_pouInterface, pouslist)
        End Select
        m_pouslist = pouslist
    End Sub
    Public Sub New(ByVal PouName As String, ByRef RefResGlobalVariables As VariablesLists, ByRef RefPouInterface As pouInterface, ByRef pouslist As Pous)
        m_Name = PouName
        m_ResGlobalVariables = RefResGlobalVariables
        m_pouInterface = RefPouInterface
        m_pouslist = pouslist
    End Sub
    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter)
        'body
        RefXMLProjectWriter.WriteStartElement("body")
        ' esporta il contenuto
        m_Implementation.xmlExport(RefXMLProjectWriter)
        If Documentation <> "" Then
            RefXMLProjectWriter.WriteElementString("documentation", Me.Documentation)  'documentation
        End If
        RefXMLProjectWriter.WriteEndElement() 'body
    End Sub
    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader)
        'Memorizza la profondità del nodo
        Dim NodeDepth As Integer = RefXmlProjectReader.Depth
        'Si sposta sul nodo successivo
        RefXmlProjectReader.Read()


        'Scorre fino alla fine del body
        While RefXmlProjectReader.Depth > NodeDepth
            Select Case RefXmlProjectReader.Name
                Case "documentation"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                        Documentation = RefXmlProjectReader.ReadString()
                    End If
                Case "IL"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                    End If
                Case "ST"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                    End If
                Case "FBD"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                        ' Imposta il tipo di body
                        m_BodyType = EnumBodyType.tFBD
                        ' Importa il contenuto
                        m_Implementation = New FBD(m_Name, m_ResGlobalVariables, m_pouInterface, m_pouslist)
                        ' Se non è vuoto lo legge
                        If Not RefXmlProjectReader.IsEmptyElement() Then
                            m_Implementation.xmlImport(RefXmlProjectReader)
                        End If
                    End If
                Case "LD"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                        'Importa il tipo di body
                        m_BodyType = EnumBodyType.tLD
                        'crea il ladder
                        m_Implementation = New Ladder(m_Name, m_ResGlobalVariables, m_pouInterface, m_pouslist)
                        If Not RefXmlProjectReader.IsEmptyElement() Then
                            m_Implementation.xmlImport(RefXmlProjectReader)
                        End If

                    End If
                Case "SFC"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                        'Imposta i tipo di body
                        m_BodyType = EnumBodyType.tSFC
                        'Crea l'SFC
                        m_Implementation = New Sfc(m_Name, m_ResGlobalVariables, m_pouInterface, m_pouslist)
                        'Se non è vuoto lo legge
                        If Not RefXmlProjectReader.IsEmptyElement() Then
                            m_Implementation.xmlImport(RefXmlProjectReader)
                        End If
                    End If
            End Select
            'Si sposta sul nodo successivo
            RefXmlProjectReader.Read()

        End While
    End Sub
    Public Function CreateInstance() As body
        Dim intf As pouInterface = m_pouInterface.CreateInstance()
        CreateInstance = New body(m_Name, m_BodyType, m_ResGlobalVariables, intf, m_pouslist)
        Dim oldIntf As pouInterface = m_Implementation.PouInterface
        Try
            m_Implementation.PouInterface = intf
            CreateInstance.SetImplementation(m_Implementation.CreateInstance)
        Finally
            m_Implementation.PouInterface = oldIntf
        End Try
    End Function
    Public Function ReadBodyType() As EnumBodyType
        ReadBodyType = m_BodyType
    End Function
    Public Function ReadResGlobalVariables() As VariablesLists
        ReadResGlobalVariables = m_ResGlobalVariables
    End Function
    Public Sub DisposeMe()
        RaiseEvent Disposing()  'Genera l'evento
        'm_Sfc.DisposeMe()
        'm_Sfc = Nothing
        Me.Finalize()
    End Sub
    'Funzioni body Type SFC
    Public Sub ResolveVariablesLinks()
        Call m_Implementation.ResolveVariableLinks()
    End Sub
    Public Sub Execute()
        m_Implementation.ExecuteScanCycle()
    End Sub
    Public Sub ExecuteInit() Implements IExecutable.ExecuteInit
        m_Implementation.ExecuteInit()
    End Sub
    Public Sub Reset() Implements IExecutable.Reset
        m_Implementation.Reset()
    End Sub

    Private Function ReadImplAs(Of T As IIEC61131LanguageImplementation)() As T
        Return CType(ReadImplementation(), T)
    End Function
    Public Function ReadSfc() As Sfc
        Return ReadImplAs(Of Sfc)()
    End Function
    Public Function ReadLadder() As Ladder
        Return ReadImplAs(Of Ladder)()
    End Function
    Public Function ReadFBD() As FBD
        Return ReadImplAs(Of FBD)()
    End Function

    Public Function ReadImplementation() As IIEC61131LanguageImplementation
        Return m_Implementation
    End Function
    <Obsolete("Usare SetImplementation")> _
    Public Sub SetSfc(ByVal RefSfc As Sfc)
        SetImplementation(RefSfc)
    End Sub
    <Obsolete("Usare SetImplementation")> _
    Public Sub SetLadder(ByVal RefLadder As Ladder)
        SetImplementation(RefLadder)
    End Sub
    Public Sub SetImplementation(ByVal impl As IIEC61131LanguageImplementation)
        m_Implementation = impl
    End Sub

    'Funzioni di stampa
    Public Sub PrintMe(ByVal Graph As Drawing.Graphics, ByVal Rect As Drawing.Rectangle)
        m_Implementation.PrintMe(Graph, Rect)
    End Sub

    Public Property Documentation() As String Implements IDocumentable.Documentation
        Get
            Return m_Documentation
        End Get
        Set(ByVal value As String)
            m_Documentation = value
        End Set
    End Property

    Public Function GetSystemDescription() As String Implements IDocumentable.GetSystemDescription
        Dim desc As String = GetIdentifier() + " is "
        Select Case m_BodyType
            Case EnumBodyType.tSFC
                desc += "SFC"
            Case EnumBodyType.tLD
                desc += "Ladder"
                ' Per ora questi linguaggi non sono implementati, ma meglio essere previdenti
            Case EnumBodyType.tFBD
                desc += "FBD"
            Case EnumBodyType.tIL
                desc += "Instruction List"
            Case EnumBodyType.tST
                desc += "Structured Text"
        End Select
        Return desc
    End Function

    Public Function GetDescription() As String Implements IDocumentable.GetDescription
        Return GetSystemDescription() + IIf(Of String)(Documentation = "", "", " (" + Documentation + ")")
    End Function

    Public Function GetIdentifier() As String Implements IDocumentable.GetIdentifier
        Return "POU " + Me.Name
    End Function

    Public Function ExecuteScanCycle() As Boolean Implements IExecutable.ExecuteScanCycle
        Me.Execute()
        Return True
    End Function
End Class
