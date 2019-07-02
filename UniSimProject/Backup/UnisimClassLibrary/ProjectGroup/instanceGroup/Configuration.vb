Imports System.Xml
Public Class Configuration
    Dim m_resources As Collections.Generic.List(Of Resource)
    Dim m_globalVars As VariablesLists
    Dim m_name As String
    Dim m_documentation As String
    Property resources() As Collections.Generic.List(Of Resource)
        Get
            resources = m_resources
        End Get
        Set(ByVal Value As Collections.Generic.List(Of Resource))
            m_resources = Value
        End Set
    End Property
    Property globalVars() As VariablesLists
        Get
            globalVars = m_globalVars
        End Get
        Set(ByVal Value As VariablesLists)
            m_globalVars = Value
        End Set
    End Property
    Property name() As String
        Get
            name = m_name
        End Get
        Set(ByVal Value As String)
            m_name = Value
        End Set
    End Property
    Property documentation() As String
        Get
            documentation = m_documentation
        End Get
        Set(ByVal Value As String)
            m_documentation = Value
        End Set
    End Property
    Public Sub New(ByVal name As String, ByVal documentation As String)
        m_name = name
        m_documentation = documentation
        m_resources = New Collections.Generic.List(Of Resource)
        m_globalVars = New VariablesLists
    End Sub
    Public Sub New()
        m_resources = New Collections.Generic.List(Of Resource)
        m_globalVars = New VariablesLists
    End Sub
    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter)
        'configuration
        RefXMLProjectWriter.WriteStartElement("configuration")
        'Attributi
        RefXMLProjectWriter.WriteAttributeString("name", m_name)

        'Esporta globalVars
        For Each VarList As VariablesList In globalVars

        Next VarList
        'Esporta resources
        For Each Res As Resource In m_resources
            Res.xmlExport(RefXMLProjectWriter)
        Next Res
        RefXMLProjectWriter.WriteEndElement() 'configuration
        'Esporta documentation
        If m_documentation <> "" Then
            RefXMLProjectWriter.WriteElementString("documentation", m_documentation)  'documentation
        End If
    End Sub
    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader)

        'Memorizza la profondità del nodo
        Dim NodeDepth As Integer = RefXmlProjectReader.Depth

        'Legge gli attributi
        If RefXmlProjectReader.MoveToAttribute("name") Then
            m_name = RefXmlProjectReader.Value
        End If
        'Se l'elemento non è vuoto si sposta sul nodo successivo
        If Not RefXmlProjectReader.IsEmptyElement Then
            RefXmlProjectReader.Read()
            'Scorre fino alla fine di Configuration
            While RefXmlProjectReader.Depth > NodeDepth
                Select Case RefXmlProjectReader.Name
                    Case "resource"
                        'Controlla se è l'inizio dell'elemento
                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                            Dim NewResouce As New Resource
                            NewResouce.xmlImport(RefXmlProjectReader)
                            m_resources.Add(NewResouce)
                        End If
                    Case "globalVars"
                        'Controlla se è l'inizio dell'elemento
                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

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
        End If
    End Sub
End Class



