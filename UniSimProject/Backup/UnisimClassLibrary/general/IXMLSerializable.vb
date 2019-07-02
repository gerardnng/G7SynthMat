' Servono due interfacce export-only ed import-only perchè alcuni oggetti si esportano
' ma non si importano da soli. Si potrebbe fare tutto in due interfacce
' IXMLExportable ed IXMLSerializable (in cui mettere xmlImport), ma conviene una
' soluzione più generale
Public Interface IXMLImportable
    Sub xmlImport(ByRef RefXmlProjectReader As Xml.XmlTextReader)
End Interface

Public Interface IXMLExportable
    Sub xmlExport(ByRef RefXMLProjectWriter As Xml.XmlTextWriter)
End Interface

' Tutti gli oggetti che possono essere salvati su un documento XML e caricati
' da esso implementano questa interfaccia (o almeno dovrebbero, controllare!)
Public Interface IXMLSerializable
    Inherits IXMLExportable, IXMLImportable
End Interface
