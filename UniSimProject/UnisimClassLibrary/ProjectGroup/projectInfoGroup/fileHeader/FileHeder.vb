Imports System.Xml
Public Class fileHeader
    Implements IXMLSerializable
    Dim m_companyName As String
    Dim m_companyURL As String
    Dim m_productName As String
    Dim m_productVersion As String
    Dim m_productRelease As String
    Dim m_creationDateTime As String
    Dim m_contentDescription As String
    Property companyName() As String
        Get
            companyName = m_companyName
        End Get
        Set(ByVal Value As String)
            m_companyName = Value
        End Set
    End Property
    Property companyURL() As String
        Get
            companyURL = m_companyURL
        End Get
        Set(ByVal Value As String)
            m_companyURL = Value
        End Set
    End Property
    Property productName() As String
        Get
            productName = m_productName
        End Get
        Set(ByVal Value As String)
            m_productName = Value
        End Set
    End Property
    Property productVersion() As String
        Get
            productVersion = m_productVersion
        End Get
        Set(ByVal Value As String)
            m_productVersion = Value
        End Set
    End Property
    Property productRelease() As String
        Get
            productRelease = m_productRelease
        End Get
        Set(ByVal Value As String)
            m_productRelease = Value
        End Set
    End Property
    Property creationDateTime() As String
        Get
            creationDateTime = m_creationDateTime
        End Get
        Set(ByVal Value As String)
            m_creationDateTime = Value
        End Set
    End Property
    Property contentDescription() As String
        Get
            contentDescription = m_contentDescription
        End Get
        Set(ByVal Value As String)
            m_contentDescription = Value
        End Set
    End Property
    Public Sub New()
    End Sub
    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter) Implements IXMLSerializable.xmlExport
        RefXMLProjectWriter.WriteStartElement("fileHeader")
        RefXMLProjectWriter.WriteAttributeString("companyName", m_companyName)
        RefXMLProjectWriter.WriteAttributeString("companyURL", m_companyURL)
        RefXMLProjectWriter.WriteAttributeString("productName", m_productName)
        RefXMLProjectWriter.WriteAttributeString("productVersion", m_productVersion)
        RefXMLProjectWriter.WriteAttributeString("productRelease", m_productRelease)
        RefXMLProjectWriter.WriteAttributeString("creationDateTime", m_creationDateTime)
        RefXMLProjectWriter.WriteAttributeString("contentDescription", m_contentDescription)
        RefXMLProjectWriter.WriteEndElement()
    End Sub
    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader) Implements IXMLSerializable.xmlImport
        'Legge gli attributi
        If RefXmlProjectReader.MoveToAttribute("companyName") Then
            m_companyName = RefXmlProjectReader.Value
        End If
        If RefXmlProjectReader.MoveToAttribute("companyURL") Then
            m_companyURL = RefXmlProjectReader.Value
        End If
        If RefXmlProjectReader.MoveToAttribute("productName") Then
            m_productName = RefXmlProjectReader.Value
        End If
        If RefXmlProjectReader.MoveToAttribute("productVersion") Then
            m_productVersion = RefXmlProjectReader.Value
        End If
        If RefXmlProjectReader.MoveToAttribute("productRelease") Then
            m_productRelease = RefXmlProjectReader.Value
        End If
        If RefXmlProjectReader.MoveToAttribute("creationDateTime") Then
            m_creationDateTime = RefXmlProjectReader.Value
        End If
        If RefXmlProjectReader.MoveToAttribute("contentDescription") Then
            m_contentDescription = RefXmlProjectReader.Value
        End If
    End Sub
    Public Sub DisposeMe()
        Me.Finalize()
    End Sub
End Class
