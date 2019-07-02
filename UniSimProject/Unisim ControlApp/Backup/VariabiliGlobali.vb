Module GlobalVariables

    Public ApplicationProductName As String = "Unisim"
    Public ApplicationProductVersion As String = "v1.0"
    Public ApplicationCompanyName As String = "Application"
    Public ApplicationCompanyURL As String = "host.domain.org"
    Public ApplicationCreationDateTime As String = "2005-05-01T20:00:00Z"
    Public ApplicationRelease As String = "A"
    Public ApplicationContentDescription As String = "Application"
    Public ApplicationFileExtension As String = "psf"
    Public ApplicationFileXMLExtension As String = "xml"

    Public CursorsPath As String = System.Windows.Forms.Application.StartupPath & "\cursors"
    Public XmlSchemaPath As String = System.Windows.Forms.Application.StartupPath & "\xml\TC6_XML_V10.xsd"
    Public m_Project As UnisimClassLibrary.Project     'Riferimento al progetto
    'Caratteri non validi per i nomi di variabili
    '  +|""'<>().,:; /\]^%@-*#?![]~{}=
    Public InvalidVariableChars As String = "+|""'<>().,:; /\]^%@-*#?![]~{}"


End Module
