Imports System.Xml
Public Class ContentHeader
    Implements IXMLSerializable
    Dim m_name As String
    Dim m_version As String
    Dim m_modificationDateTime As String
    Dim m_organization As String
    Dim m_author As String
    Dim m_language As String
    Dim m_comment As String

    Property name() As String
        Get
            name = m_name
        End Get
        Set(ByVal Value As String)
            m_name = Value
        End Set
    End Property
    Property version() As String
        Get
            version = m_version
        End Get
        Set(ByVal Value As String)
            m_version = Value
        End Set
    End Property
    Property modificationDateTime() As String
        Get
            modificationDateTime = m_modificationDateTime
        End Get
        Set(ByVal Value As String)
            m_modificationDateTime = Value
        End Set
    End Property
    Property organization() As String
        Get
            organization = m_organization
        End Get
        Set(ByVal Value As String)
            m_organization = Value
        End Set
    End Property
    Property author() As String
        Get
            author = m_author
        End Get
        Set(ByVal Value As String)
            m_author = Value
        End Set
    End Property
    Property language() As String
        Get
            language = m_language
        End Get
        Set(ByVal Value As String)
            m_language = Value
        End Set
    End Property
    Property comment() As String
        Get
            comment = m_comment
        End Get
        Set(ByVal Value As String)
            m_comment = Value
        End Set
    End Property
    Public Sub New(ByVal name As String, ByVal version As String, ByVal modificationDateTime As String, ByVal organization As String, ByVal _author As String, ByVal language As String)
        m_name = name
        m_version = version
        m_modificationDateTime = modificationDateTime
        m_organization = organization
        m_author = author
        m_language = language
    End Sub
    Public Sub New()
    End Sub
    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter) Implements IXMLSerializable.xmlExport
        'contentHeader
        RefXMLProjectWriter.WriteStartElement("contentHeader")

        'attributi di contentHeader
        RefXMLProjectWriter.WriteAttributeString("name", m_name)
        If Not IsNothing(m_version) Then
            If m_version.Length > 0 Then
                RefXMLProjectWriter.WriteAttributeString("version", m_version)
            End If
        End If
        If Not IsNothing(m_organization) Then
            If m_organization.Length > 0 Then
                RefXMLProjectWriter.WriteAttributeString("organization", m_organization)
            End If
        End If
        If Not IsNothing(m_author) Then
            If m_author.Length > 0 Then
                RefXMLProjectWriter.WriteAttributeString("author", m_author)
            End If
        End If
        If Not IsNothing(m_language) Then
            If m_language.Length > 0 Then
                RefXMLProjectWriter.WriteAttributeString("language", m_language)
            End If
        End If
        'Comment
        RefXMLProjectWriter.WriteElementString("Comment", m_comment)
        'coordinateInfo
        RefXMLProjectWriter.WriteStartElement("coordinateInfo")
        'pageSize
        RefXMLProjectWriter.WriteStartElement("pageSize")
        RefXMLProjectWriter.WriteAttributeString("x", "0")
        RefXMLProjectWriter.WriteAttributeString("y", "0")
        RefXMLProjectWriter.WriteEndElement() 'pageSize
        'fbd
        RefXMLProjectWriter.WriteStartElement("fbd")
        'scaling
        RefXMLProjectWriter.WriteStartElement("scaling")
        RefXMLProjectWriter.WriteAttributeString("x", "1")
        RefXMLProjectWriter.WriteAttributeString("y", "1")
        RefXMLProjectWriter.WriteEndElement() 'scaling
        RefXMLProjectWriter.WriteEndElement() 'fbd
        'ld
        RefXMLProjectWriter.WriteStartElement("ld")
        'scaling
        RefXMLProjectWriter.WriteStartElement("scaling")
        RefXMLProjectWriter.WriteAttributeString("x", "1")
        RefXMLProjectWriter.WriteAttributeString("y", "1")
        RefXMLProjectWriter.WriteEndElement() 'scaling
        RefXMLProjectWriter.WriteEndElement() 'ld
        'sfc
        RefXMLProjectWriter.WriteStartElement("sfc")
        'scaling
        RefXMLProjectWriter.WriteStartElement("scaling")
        RefXMLProjectWriter.WriteAttributeString("x", "1")
        RefXMLProjectWriter.WriteAttributeString("y", "1")
        RefXMLProjectWriter.WriteEndElement() 'scaling
        RefXMLProjectWriter.WriteEndElement() 'sfc

        RefXMLProjectWriter.WriteEndElement() 'coordinateInfo

        RefXMLProjectWriter.WriteEndElement() 'contentHeader
    End Sub
    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader) Implements IXMLSerializable.xmlImport
        'Memorizza la profondità del nodo
        Dim NodeDepth As Integer = RefXmlProjectReader.Depth
        'Legge gli attributi
        If RefXmlProjectReader.MoveToAttribute("name") Then
            m_name = RefXmlProjectReader.Value
        End If
        If RefXmlProjectReader.MoveToAttribute("version") Then
            m_version = RefXmlProjectReader.Value
        End If
        If RefXmlProjectReader.MoveToAttribute("organization") Then
            m_organization = RefXmlProjectReader.Value
        End If
        If RefXmlProjectReader.MoveToAttribute("author") Then
            m_author = RefXmlProjectReader.Value
        End If
        If RefXmlProjectReader.MoveToAttribute("language") Then
            m_language = RefXmlProjectReader.Value
        End If
        'Se l'elemento non è vuoto si sposta sul nodo successivo
        If Not RefXmlProjectReader.IsEmptyElement Then
            RefXmlProjectReader.Read()
            'Scorre fino alla fine di ContentHeader
            While RefXmlProjectReader.Depth > NodeDepth
                Select Case RefXmlProjectReader.Name
                    Case "Comment"
                        'Controlla se è l'inizio dell'elemento
                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                            'Se l'elemento non è vuoto si sposta sul nodo successivo
                            If Not RefXmlProjectReader.IsEmptyElement Then
                                RefXmlProjectReader.Read()
                                m_comment = RefXmlProjectReader.Value
                            End If
                        End If
                    Case "coordinateInfo"
                        'Se l'elemento non è vuoto si sposta sul nodo successivo
                        If Not RefXmlProjectReader.IsEmptyElement Then
                            RefXmlProjectReader.Read()
                            'Scorre fino alla fine di coordinateInfo
                            While RefXmlProjectReader.Name <> "coordinateInfo"
                                Select Case RefXmlProjectReader.Name
                                    Case "pageSize"
                                        'Controlla se è l'inizio dell'elemento
                                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                                        End If
                                    Case "fbd"
                                        'Controlla se è l'inizio dell'elemento
                                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                                            'Si sposta sul nodo scaling
                                            RefXmlProjectReader.Read()
                                        End If
                                    Case "ld"
                                        'Controlla se è l'inizio dell'elemento
                                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                                            'Si sposta sul nodo scaling
                                            RefXmlProjectReader.Read()
                                        End If
                                    Case "sfc"
                                        'Controlla se è l'inizio dell'elemento
                                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                                            'Si sposta sul nodo scaling
                                            RefXmlProjectReader.Read()
                                        End If
                                End Select
                                'Si sposta sul nodo successivo se non è la fine dell'elemento
                                If RefXmlProjectReader.Depth > NodeDepth Then
                                    RefXmlProjectReader.Read()
                                End If
                            End While
                        End If
                End Select
                'Si sposta sul nodo successivo se non è la fine dell'elemento
                If RefXmlProjectReader.Depth > NodeDepth Then
                    RefXmlProjectReader.Read()
                End If
            End While
        End If

    End Sub
    Public Sub DisposeMe()
        Me.Finalize()
    End Sub
End Class
