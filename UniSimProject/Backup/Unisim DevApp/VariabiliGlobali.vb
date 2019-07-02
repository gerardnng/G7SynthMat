Imports UnisimClassLibrary
Imports System.IO

Module GlobalVariables
    Public ApplicationProductName As String = UniSimVersion.VersionInfo.ProgramName
    Public ApplicationProductName1 As String = UniSimVersion.VersionInfo.ProgramName
    Public ApplicationProductVersion As String = UniSimVersion.VersionInfo.VersionString
    Public ApplicationCompanyName As String = UniSimVersion.VersionInfo.CorporateName
    Public ApplicationCompanyURL As String = UniSimVersion.VersionInfo.CorporateURL
    Public ApplicationCreationDateTime As String = UniSimVersion.VersionInfo.CreationMoment
    Public ApplicationRelease As String = UniSimVersion.VersionInfo.ProductRelease
    Public ApplicationContentDescription As String = "Application"
    Public ApplicationFileXMLExtension As String = "xml"
    Public CursorsPath As String = Path.Combine(System.Windows.Forms.Application.StartupPath, "cursors")
    Public XmlSchemaPath As String = Path.Combine(Path.Combine(System.Windows.Forms.Application.StartupPath, _
        "xml"), "TC6_XML_V10.xsd")
    Public m_Project As UnisimClassLibrary.Project     'Riferimento al progetto
    'Caratteri non validi per i nomi di variabili
    '  +|""'<>().,:; /\]^%@-*#?![]~{}=
    Public InvalidVariableChars As String = "+|""'<>().,:; /\]^%@*#?![]~{}"
    Public Null As New Object
End Module
