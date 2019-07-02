Imports System.Xml
Public Class pouInstance
    Protected m_pou As pou  'Puntatore diretto al tipo
    Protected m_pouInstance As pou  'Pntatore diretto all'istanza
    Protected xmlPouName As String
    Protected xmlPouType As String
    Property pou() As pou
        Get
            pou = m_pou
        End Get
        Set(ByVal Value As pou)
            m_pou = Value
        End Set
    End Property
    ReadOnly Property pouInstance() As pou
        Get
            pouInstance = m_pouInstance
        End Get
    End Property
    ReadOnly Property name() As String
        Get
            name = m_pou.Name
        End Get
    End Property
    ReadOnly Property type() As EnumPouType
        Get
            type = m_pou.PouType
        End Get
    End Property
    Public Sub New(ByVal RefPou As pou)
        m_pou = RefPou
    End Sub
    Public Sub New()
    End Sub
    Public Sub ResolvePouInstanceLinks(ByRef RefPouList As Pous)
        m_pou = RefPouList.FindpouByName(xmlPouName)
    End Sub
    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter)
        RefXMLProjectWriter.WriteStartElement("pouInstance")
        'attributi di pouInstance
        RefXMLProjectWriter.WriteAttributeString("name", m_pou.Name)
        RefXMLProjectWriter.WriteAttributeString("type", m_pou.PouType.ToString.ToLower())
        RefXMLProjectWriter.WriteEndElement() 'pouInstances
    End Sub
    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader)
        'Memorizza la profondità del nodo
        Dim NodeDepth As Integer = RefXmlProjectReader.Depth
        'Legge gli attributi
        If RefXmlProjectReader.MoveToAttribute("name") Then
            xmlPouName = RefXmlProjectReader.Value
        End If
        If RefXmlProjectReader.MoveToAttribute("type") Then
            Select Case RefXmlProjectReader.Value
                Case "program"
                    xmlPouType = EnumPouType.Program
                Case "function"
                    xmlPouType = EnumPouType.Function
                Case "functionBlock"
                    xmlPouType = EnumPouType.FunctionBlock
            End Select
        End If
    End Sub
    Public Sub ExecutePou()
        m_pou.Execute()
    End Sub
    Public Sub ExecutePouInstance()
        m_pouInstance.Execute()
    End Sub
    Public Sub ExecutePouInit()
        m_pou.ExecuteInit()
    End Sub
    Public Sub ExecutePouInstanceInit()
        m_pouInstance.ExecuteInit()
    End Sub
    Public Sub CreateInstance()
        'Crea l'istanza del POU
        m_pouInstance = m_pou.CreateInstance
    End Sub
End Class
