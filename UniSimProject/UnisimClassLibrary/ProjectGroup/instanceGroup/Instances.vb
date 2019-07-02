Imports System.Xml
Public Class Instances
    Dim m_configurations As Collections.Generic.List(Of Configuration)
    Property configurations() As Collections.Generic.List(Of Configuration)
        Get
            configurations = m_configurations
        End Get
        Set(ByVal Value As Collections.Generic.List(Of Configuration))
            m_configurations = Value
        End Set
    End Property
    Public Sub New()
        m_configurations = New Collections.Generic.List(Of Configuration)
    End Sub
    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter)
        'instances
        RefXMLProjectWriter.WriteStartElement("instances")
        'Esporta configurations
        For Each Conf As configuration In m_configurations
            'configurations
            RefXMLProjectWriter.WriteStartElement("configurations")
            Conf.xmlExport(RefXMLProjectWriter)
            RefXMLProjectWriter.WriteEndElement() 'configurations
        Next Conf
        RefXMLProjectWriter.WriteEndElement() 'instances
    End Sub
    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader)
        'Memorizza la profondità del nodo
        Dim InstancesNodeDepth As Integer = RefXmlProjectReader.Depth
        'Si sposta sul nodo successivo
        RefXmlProjectReader.Read()
        'Scorre fino alla fine di Instances
        While RefXmlProjectReader.Depth >InstancesNodeDepth
            Select Case RefXmlProjectReader.Name
                Case "configurations"
                    'Controlla se è l'inizio dell'elemento e se l'elemento non è vuoto
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element And Not RefXmlProjectReader.IsEmptyElement Then
                        'Memorizza la profondità del nodo
                        Dim ConfigurationsNodeDepth As Integer = RefXmlProjectReader.Depth
                        'Si sposta sul nodo successivo
                        RefXmlProjectReader.Read()
                        'Scorre fino alla fine di configurations
                        While RefXmlProjectReader.Depth > ConfigurationsNodeDepth
                            Select Case RefXmlProjectReader.Name
                                Case "configuration"
                                    'Controlla se è l'inizio dell'elemento
                                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                                        Dim NewConfiguration As New Configuration
                                        NewConfiguration.xmlImport(RefXmlProjectReader)
                                        m_configurations.Add(NewConfiguration)
                                    End If
                            End Select
                            'Si sposta sul nodo successivo se non è la fine dell'elemento
                            If RefXmlProjectReader.Depth > ConfigurationsNodeDepth Then
                                RefXmlProjectReader.Read()
                            End If
                        End While
                    End If
            End Select
            'Si sposta sul nodo successivo se non è la fine dell'elemento
            If RefXmlProjectReader.Depth > InstancesNodeDepth Then
                RefXmlProjectReader.Read()
            End If
        End While
    End Sub
End Class

