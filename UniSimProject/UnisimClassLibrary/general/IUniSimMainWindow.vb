' Un hack per consentire a parti della ClassLibrary di accedere alla finestra principale
' della DevApp. I prerequisiti sono un riferimento alla MainWindow (anche sotto forma
' di Object va bene). Per ora lo usa solo FormSFC per istanziare la POU Ladder
' "equivalente". Aggiungere membri secondo necessità
Public Interface IUniSimMainWindow

    Function GetPOUsTree() As PousTree
    Function GetResourceTree() As ResourceTree
    ReadOnly Property ApplicationName() As String

End Interface
