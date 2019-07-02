Imports System.Text
Imports System.io
Imports System.Xml
Imports System.Xml.Schema
Public Class XMLProjectManager
    'Variabili per l'import
    Dim WithEvents m_XmlValidatingReader As XmlValidatingReader 'Lettore per la validazione
    Public ErrorsList As ArrayList          'Lista degli errori di validazione
    Public Response As Boolean              'Esito della validazione
    'Variabili per l'export
    Dim m_xmlProjectWriter As XmlTextWriter 'Scrittore del file progetto
    Public Function xmlImport(ByRef xmlContentString As String, ByVal FileSchemaName As String, ByRef RefProject As Project, _
                              Optional ByVal doValidate As Boolean = True) As Boolean
        'Valida prima il progetto
        If Not (doValidate) OrElse xmlValidate(xmlContentString, FileSchemaName) Then
            'Il progetto è valido e lo importa
            Dim xmlImportStringReader As New StringReader(xmlContentString)     'Lettore della stringa xmlContentString
            Dim m_xmlProjectReader As New XmlTextReader(xmlImportStringReader)   'Lettore della stringa contenente il progetto
            m_xmlProjectReader.WhitespaceHandling = WhitespaceHandling.None     'Ignora gli spazi vuoti
            'Inizia la lettura fino a trovare l'elemento project
            m_xmlProjectReader.Read()
            While Not (m_xmlProjectReader.NodeType = XmlNodeType.Element And m_xmlProjectReader.Name = "project")
                m_xmlProjectReader.Read()
            End While
            RefProject.xmlImport(m_xmlProjectReader)
            xmlImport = True
        Else
            'Il progetto non è valido e resituisce false
            xmlImport = False
        End If
    End Function
    Public Function xmlValidate(ByRef xmlContentString As String, ByVal FileSchemaName As String) As Boolean
        Try
            Dim xmlImportStringReader As New StringReader(xmlContentString)     'Lettore della stringa xmlContentString
            Dim m_xmlProjectReader As New XmlTextReader(xmlImportStringReader)   'Lettore della stringa contenente il progetto
            m_xmlProjectReader.WhitespaceHandling = WhitespaceHandling.None     'Ignora gli spazi vuoti
            m_XmlValidatingReader = New XmlValidatingReader(m_xmlProjectReader)    'Lettore per la validazione
            m_XmlValidatingReader.ValidationType = ValidationType.Schema
            m_xmlProjectReader.WhitespaceHandling = WhitespaceHandling.None
            Dim m_xmlSchenaReader As New XmlTextReader(FileSchemaName)    'Lettore dello schema
            'Aggiunge lo schema al lettore per la validazione
            m_XmlValidatingReader.Schemas.Add(Nothing, m_xmlSchenaReader)
            ErrorsList = New ArrayList
            StartValidate()
            'Se è valido notifica al chiamante
            If Response Then
                xmlValidate = True
            End If
            xmlImportStringReader.Close()
            m_xmlProjectReader.Close()
            m_XmlValidatingReader.Close()
            m_xmlSchenaReader.Close()
        Catch ex As system.exception
            ' MsgBox(ex.Message)
            xmlValidate = False
        End Try
    End Function
    Public Function xmlExport(ByRef RefProject As Project) As StringBuilder
        Dim xmlExportStringWriter As New StringWriter
        ' Dim s As New StreamWriter
        m_xmlProjectWriter = New XmlTextWriter(xmlExportStringWriter)
        'Imposta il tipo di formattazione
        m_xmlProjectWriter.Formatting = Formatting.Indented
        'Scrive l'inizio del documento
        m_xmlProjectWriter.WriteRaw("<?xml version=""1.0"" encoding=""utf-8""?>")
        'Effettua l'esportazione del progetto
        RefProject.xmlExport(m_xmlProjectWriter)
        'Chiude li scrittore
        m_xmlProjectWriter.Close()
        'Restituisce la stringa
        xmlExport = xmlExportStringWriter.GetStringBuilder
    End Function
    Public Sub StartValidate()
        'Effettua la lettura del file per la validazione
        ErrorsList.Clear()  'Svuota la lista deggli error
        Response = True
        Try
            While (m_XmlValidatingReader.Read())
                'Scorre il documento per validarlo
            End While
        Catch ex As system.exception
            'E un errore non di conformità allo schema ma di sintassi quindi cancella gli eventuali altri errori ed esce
            Response = False
            ErrorsList.Clear()
            ErrorsList.Add(ex.Message)
        End Try
    End Sub
    Private Sub ErrorEventSub(ByVal Sender As Object, ByVal e As ValidationEventArgs) Handles m_XmlValidatingReader.ValidationEventHandler
        'Questa sub viene chiamata in caso di errore durante la validazione
        'E un errore di conformità allo schema
        'Aggiunge l'errore alla stringa
        Response = False
        ErrorsList.Add(e.Message)
    End Sub
    Public Function OpenXmlFile(ByVal FileName As String, ByRef xmlContent As String) As Boolean
        'Legge un file e lo restituisce in una stringa
        Try
            If File.Exists(FileName) Then
                Dim sr As StreamReader = File.OpenText(FileName)
                xmlContent = sr.ReadToEnd
                sr.Close()
                OpenXmlFile = True
            End If
        Catch ex As system.exception
            OpenXmlFile = False
        End Try
    End Function
    Public Function WriteXmlFile(ByRef mStringBuilder As StringBuilder, ByVal FileName As String) As Boolean
        'Salva il contenuto di xmlContent in un file
        Try
            Dim xmlFileWriter As New StreamWriter(FileName)
            xmlFileWriter.Write(mStringBuilder)
            xmlFileWriter.Close()
        Catch ex As system.exception
            WriteXmlFile = False
            Exit Function
        End Try
        WriteXmlFile = True
    End Function
End Class
