Imports System.Xml
Public Class Project
    Private m_fileHeader As fileHeader
    Private m_ContentHeader As ContentHeader
    Private m_Types As Types
    Private m_Instances As Instances
    Private m_PlcMemory As VariablesList
    Property fileHeader() As fileHeader
        Get
            fileHeader = m_fileHeader
        End Get
        Set(ByVal Value As fileHeader)
            m_fileHeader = Value
        End Set
    End Property
    Property ContentHeader() As ContentHeader
        Get
            ContentHeader = m_ContentHeader
        End Get
        Set(ByVal Value As ContentHeader)
            m_ContentHeader = Value
        End Set
    End Property
    Property Types() As Types
        Get
            Types = m_Types
        End Get
        Set(ByVal Value As Types)
            m_Types = Value
        End Set
    End Property
    Property Istances() As Instances
        Get
            Istances = m_Instances
        End Get
        Set(ByVal Value As Instances)
            m_Instances = Value
        End Set
    End Property
    Public ReadOnly Property DefaultResource() As Resource
        Get
            Return Istances.configurations(0).resources(0)
        End Get
    End Property
    Public Sub New()
        m_fileHeader = New fileHeader
        m_ContentHeader = New ContentHeader
        m_Types = New Types
        m_Instances = New Instances
    End Sub
    Public Sub xmlExport(ByRef RefXmlProjectWriter As XmlTextWriter)
        'Scrive l'elemento root (project)
        RefXmlProjectWriter.WriteStartElement("project")
        'Informazioni sullo spazio dei nomi, prefissi, schema
        RefXmlProjectWriter.WriteAttributeString("xmlns", "http://www.plcopen.org/xml/tc6.xsd")
        RefXmlProjectWriter.WriteAttributeString("xmlns:xhtml", "http://www.w3.org/1999/xhtml")
        RefXmlProjectWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance")
        'RefXmlProjectWriter.WriteAttributeString("xsi:schemaLocation", "http://www.plcopen.org/xml/tc6.xsd TC6_XML_V099Valid.xml")
        'Esporta le variabili
        m_fileHeader.xmlExport(RefXmlProjectWriter)
        m_ContentHeader.xmlExport(RefXmlProjectWriter)
        m_Types.xmlExport(RefXmlProjectWriter)
        m_Instances.xmlExport(RefXmlProjectWriter)
        RefXmlProjectWriter.WriteEndElement() 'project


    End Sub
    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader)
        'Si sposta sul nodo fileHeader
        RefXmlProjectReader.Read()
        m_fileHeader.xmlImport(RefXmlProjectReader)
        'Si sposta sul nodo ContentHeader
        RefXmlProjectReader.Read()
        m_ContentHeader.xmlImport(RefXmlProjectReader)
        'Si sposta sul nodo Types
        RefXmlProjectReader.Read()
        m_Types.xmlImport(RefXmlProjectReader)
        'Si sposta sul nodo Instances
        RefXmlProjectReader.Read()
        m_Instances.xmlImport(RefXmlProjectReader)
    End Sub
    Public Sub DisposeMe()
        m_fileHeader.DisposeMe()
        m_ContentHeader.DisposeMe()
        m_Types.DisposeMe()
    End Sub

End Class
