' Una interfaccia che indica che un oggetto può essere "sporcato" da modifiche ed è in grado di tenere
' traccia del suo stato di modificato/non modificato. L'operazione che assume il significato logico
' di "salvataggio", cioè un acknolwedgement delle modifiche da parte di una entità a livello gerarchico
' superiore dovrebbe includere il settaggio allo stato di non modificato tramite ClearDirtyFlag
' che ritorna, per contratto, il valore di Dirty prima del settaggio
' Il tipo di operazioni da ritenersi modifica, e che quindi devono portare Dirty a True dipendono dalla
' specifica classe

Public Interface IDirtyFlaggable
    ReadOnly Property Dirty() As Boolean
    Function ClearDirtyFlag() As Boolean
End Interface
