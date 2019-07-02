Public Interface IExecutable
    Sub ExecuteInit()
    Function ExecuteScanCycle() As Boolean
    Sub Reset()
End Interface

Public Interface IExecutableWithSignalling
    Inherits IExecutable

    Event StartScan()
    Event EndScan()
End Interface

Public Interface IPrintable
    Sub PrintMe(ByRef Graph As Drawing.Graphics, ByVal Rect As Drawing.Rectangle)
End Interface

' Una interfaccia per gli oggetti che si possono "collegare" alle risorse
' dell'ambiente (le variabili)
Public Interface IBindToResource
    Property PouInterface() As pouInterface
    Property ResGlobalVariables() As VariablesLists
End Interface

' Nome lunghissimo per un concetto molto semplice:
' Questa interfaccia astrae gli elementi comuni a tutte le implementazioni
' dei linguaggi IEC61131 fornite da UniSim (ad oggi SFC, Ladder ed FBD)
' N.B: non c'è tutto, solo ciò che è servito finora. Aggiungere pure
Public Interface IIEC61131LanguageImplementation
    Inherits IXMLSerializable, IHasName, _
    IExecutableWithSignalling, IPrintable, IBindToResource

    Sub ResolveVariableLinks()
    Sub SetGraphToDraw(ByRef Graph As Drawing.Graphics)
    Function CreateInstance() As IIEC61131LanguageImplementation
    Function FindElementByLocalId(ByVal id As Integer) As IHasLocalId

End Interface
