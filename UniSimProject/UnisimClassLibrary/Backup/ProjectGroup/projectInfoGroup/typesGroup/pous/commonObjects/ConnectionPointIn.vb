Imports System.Xml
Public Class ConnectionPointIn
    Implements IXMLSerializable
    Protected m_connections As ArrayList
    Public Property Connections() As ArrayList
        Get
            Connections = m_connections
        End Get
        Set(ByVal Value As ArrayList)
            m_connections = Value
        End Set
    End Property
    Public Sub New(ByRef ConnList As ArrayList)
        m_connections = ConnList
    End Sub
    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter) Implements IXMLSerializable.xmlExport
        'connectionPointIn
        RefXMLProjectWriter.WriteStartElement("connectionPointIn")
        'connection
        'Aggiunge un elemento Connections per ogni elemento in XmlPreviousConnectionsList
        For Each C As Object In m_connections
            'connection
            RefXMLProjectWriter.WriteStartElement("connection")
            'Attributi della connection
            RefXMLProjectWriter.WriteAttributeString("refLocalId", C.Number.ToString)
            RefXMLProjectWriter.WriteEndElement() 'connection
        Next C
        RefXMLProjectWriter.WriteEndElement() 'connectionPointIn
    End Sub
    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader) Implements IXMLSerializable.xmlImport
        'Memorizza la profondità del nodo
        Dim NodeDepth As Integer = RefXmlProjectReader.Depth
        'Se l'elemento non è vuoto si sposta sul nodo successivo
        If Not RefXmlProjectReader.IsEmptyElement Then
            RefXmlProjectReader.Read()

            'Scorre fino alla fine di Configuration
            While RefXmlProjectReader.Depth > NodeDepth
                Select Case RefXmlProjectReader.Name
                    Case "connection"
                        'Controlla se è l'inizio dell'elemento
                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                            'Legge il refLocalID di connection e lo aggiunge alla lista delle connessioni precedenti
                            If RefXmlProjectReader.MoveToAttribute("refLocalId") Then
                                m_connections.Add(CInt(RefXmlProjectReader.Value))
                            End If
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
