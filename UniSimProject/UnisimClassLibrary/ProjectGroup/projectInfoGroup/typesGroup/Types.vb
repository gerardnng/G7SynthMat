Imports System.Xml

Public Class Types
    Private m_dataTypes As ArrayList
    Private m_pous As pous
    Property dataTypes() As ArrayList
        Get
            dataTypes = m_dataTypes
        End Get
        Set(ByVal Value As ArrayList)
            m_dataTypes = dataTypes
        End Set
    End Property
    Property pous() As pous
        Get
            pous = m_pous
        End Get
        Set(ByVal Value As pous)
            m_pous = Value
        End Set
    End Property
    Public Sub New()
        m_dataTypes = New ArrayList
        m_pous = New Pous
    End Sub
    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter)
        'types
        RefXMLProjectWriter.WriteStartElement("types")
        'dataTypes
        RefXMLProjectWriter.WriteStartElement("dataTypes")
        RefXMLProjectWriter.WriteEndElement() 'dataTypes
        'pous
        m_pous.xmlExport(RefXMLProjectWriter)

        RefXMLProjectWriter.WriteEndElement() 'types
    End Sub
    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader)
        'Memorizza la profondità del nodo
        Dim NodeDepth As Integer = RefXmlProjectReader.Depth
        'Si sposta sul nodo successivo
        RefXmlProjectReader.Read()

        'Scorre fino alla fine di types
        While RefXmlProjectReader.Depth > NodeDepth
            Select Case RefXmlProjectReader.Name
                Case "dataTypes"
                    'Controlla se è l'inizio dell'elemento e se non è vuoto
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element And Not RefXmlProjectReader.IsEmptyElement Then

                    End If
                Case "pous"
                    'Controlla se è l'inizio dell'elemento e se non è vuoto
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element And Not RefXmlProjectReader.IsEmptyElement Then
                        m_pous.xmlImport(RefXmlProjectReader)
                    End If
            End Select
            'Si sposta sul nodo successivo
            RefXmlProjectReader.Read()

        End While
    End Sub
    Public Sub DisposeMe()
        pous.DisposeMe()
    End Sub
End Class
