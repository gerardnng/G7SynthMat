Imports System.Xml
Public Enum EnumPouType
    [Function]
    FunctionBlock
    Program
End Enum
Public Class pou
    Implements IHasName, IExecutable, IBindToResource

    Dim m_name As String
    Dim m_pouType As EnumPouType
    Dim m_interface As pouInterface
    Dim m_actions As ArrayList
    Dim m_transitions As ArrayList
    Dim m_body As body
    Dim m_documentation As String
    Dim m_ResGlobalVariables As VariablesLists
    Dim m_pouslist As Pous
    Property Name() As String Implements IHasName.Name
        Get
            Name = m_name
        End Get
        Set(ByVal Value As String)
            m_name = Value
            m_body.Name = m_name
        End Set
    End Property
    Property PouType() As EnumPouType
        Get
            PouType = m_pouType
        End Get
        Set(ByVal Value As EnumPouType)
            m_pouType = Value
        End Set
    End Property
    Property PouInterface() As pouInterface Implements IBindToResource.PouInterface
        Get
            PouInterface = m_interface
        End Get
        Set(ByVal Value As pouInterface)
            m_interface = Value
        End Set
    End Property
    Property Pous() As Pous
        Get
            Pous = m_pouslist
        End Get
        Set(ByVal Value As Pous)
            m_pouslist = Value
        End Set
    End Property
    Property Actions() As ArrayList
        Get
            Actions = m_actions
        End Get
        Set(ByVal Value As ArrayList)
            m_actions = Value
        End Set
    End Property
    Property Transitions() As ArrayList
        Get
            Transitions = m_transitions
        End Get
        Set(ByVal Value As ArrayList)
            m_transitions = Value
        End Set
    End Property
    Property Body() As body
        Get
            Body = m_body
        End Get
        Set(ByVal Value As body)
            m_body = Value
        End Set
    End Property
    Property Documentation() As String
        Get
            Documentation = m_documentation
        End Get
        Set(ByVal Value As String)
            m_documentation = Value
        End Set
    End Property
    Property ResGlobalVariables() As VariablesLists Implements IBindToResource.ResGlobalVariables
        Get
            ResGlobalVariables = m_ResGlobalVariables
        End Get
        Set(ByVal Value As VariablesLists)
            m_ResGlobalVariables = Value
            'Comunica al body le lista di variabili globali della risorsa
            m_body.ResGlobalVariables = m_ResGlobalVariables
        End Set
    End Property
    Public Sub New(ByVal name As String, ByVal documentation As String, _
        ByVal pouType As EnumPouType, ByRef ResGlobalVariables As VariablesLists, _
        ByVal BodyType As EnumBodyType, ByRef pousList As Pous)
        m_name = name
        m_documentation = documentation
        m_ResGlobalVariables = ResGlobalVariables
        'Imposta il tipo di POU
        m_pouType = pouType
        'Crea l'interfaccia
        m_interface = New pouInterface(Me)
        'Crea il body
        m_body = New body(m_name, BodyType, m_ResGlobalVariables, m_interface, pousList)
        m_pouslist = pousList
    End Sub
    Public Sub New(ByVal name As String, ByVal documentation As String, _
        ByRef pouType As EnumPouType, ByRef ResGlobalVariables As VariablesLists, _
        ByRef RefBody As body, ByRef pousList As Pous, ByRef pouIntf As pouInterface)
        m_name = name
        m_documentation = documentation
        m_ResGlobalVariables = ResGlobalVariables
        m_pouType = pouType
        m_interface = pouIntf
        m_body = RefBody
        m_pouslist = pousList
    End Sub
    Public Sub New(ByRef ResGlobalVariables As VariablesLists, ByRef pousList As Pous)
        m_ResGlobalVariables = ResGlobalVariables
        'Crea l'interfaccia
        m_interface = New pouInterface(Me)
        m_pouslist = pousList
    End Sub
    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter)
        'pou
        RefXMLProjectWriter.WriteStartElement("pou")
        'attributi del pou
        RefXMLProjectWriter.WriteAttributeString("name", m_name)
        RefXMLProjectWriter.WriteAttributeString("pouType", m_pouType.ToString.ToLower())

        'esporta interface
        m_interface.xmlExport(RefXMLProjectWriter)
        'esporta body
        m_body.xmlExport(RefXMLProjectWriter)

        RefXMLProjectWriter.WriteEndElement() 'pou
    End Sub
    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader)
        'Memorizza la profondità del nodo
        Dim NodeDepth As Integer = RefXmlProjectReader.Depth
        'Legge gli attributi
        If RefXmlProjectReader.MoveToAttribute("name") Then
            m_name = RefXmlProjectReader.Value
        End If
        If RefXmlProjectReader.MoveToAttribute("pouType") Then
            Select Case RefXmlProjectReader.Value
                Case "program"
                    m_pouType = EnumPouType.Program
                Case "function"
                    m_pouType = EnumPouType.Function
                Case "functionBlock"
                    m_pouType = EnumPouType.FunctionBlock
            End Select
        End If

        'Si sposta sul nodo successivo
        RefXmlProjectReader.Read()

        'Scorre fino alla fine di pou
        While RefXmlProjectReader.Depth > NodeDepth
            Select Case RefXmlProjectReader.Name
                Case "interface"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                        m_interface.xmlImport(RefXmlProjectReader)
                    End If
                Case "actions"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                    End If
                Case "transitions"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                    End If
                Case "body"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                        'Crea il body
                        m_body = New body(m_name, m_ResGlobalVariables, m_interface, m_pouslist)
                        'Importa i dati del body
                        m_body.xmlImport(RefXmlProjectReader)
                    End If
                Case "documentation"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                        m_documentation = RefXmlProjectReader.Value
                    End If

            End Select
            'Si sposta sul nodo successivo se non è la fine dell'elemento
            If RefXmlProjectReader.Depth > NodeDepth Then
                RefXmlProjectReader.Read()
            End If

        End While
    End Sub
    Public Sub ResolveVariablesLinks()
        'Risolve i riferimenti dei nomi di variabili nella azioni e nelle condizioni
        m_body.ResolveVariablesLinks()
    End Sub
    Public Function CreateInstance() As pou
        Dim BodyInstance As body = m_body.CreateInstance
        CreateInstance = New pou(m_name, m_documentation, m_pouType, m_ResGlobalVariables, _
            BodyInstance, m_pouslist, BodyInstance.PouInterface)
        CreateInstance.m_interface.pou = CreateInstance
    End Function
    Public Sub DisposeMe()
        m_body.DisposeMe()
        m_body = Nothing
        Me.Finalize()
    End Sub
    Public Sub Execute()
        Body.Execute()
    End Sub
    Public Sub ExecuteInit() Implements IExecutable.ExecuteInit
        Body.ExecuteInit()
    End Sub
    Public Sub Reset() Implements IExecutable.Reset
        m_body.Reset()
        If Not IsNothing(Me.PouInterface) Then PouInterface.Reset()
    End Sub
    Public Sub PrintMe(ByVal Graph As Drawing.Graphics, ByVal Rect As Drawing.Rectangle)
        m_body.PrintMe(Graph, Rect)
    End Sub

    Public Function ExecuteScanCycle() As Boolean Implements IExecutable.ExecuteScanCycle
        Me.Execute()
        Return True
    End Function

End Class
