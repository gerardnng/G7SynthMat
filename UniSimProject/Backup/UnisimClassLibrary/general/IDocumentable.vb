' Interfaccia degli oggetti dotati di un localId
Public Interface IHasLocalId
    Property Number() As Integer
End Interface

Public Interface IHasName
    Property Name() As String
End Interface

Public Interface IHasDocumentation
    Inherits IHasName
    Property Documentation() As String
End Interface

' Indivdua gli oggetti che possono essere commentati dall'utente
' (sostanzialmente tutti gli oggetti di interesse nei linguaggi dello standard)
Public Interface IDocumentable
    Inherits IHasDocumentation

    ' descrizione dettagliata dell'oggetto
    Function GetDescription() As String
    ' descrizione "di sistema" dell'oggetto (senza commenti utente)
    Function GetSystemDescription() As String
    ' tipo ed identificativo dell'oggetto
    Function GetIdentifier() As String

End Interface