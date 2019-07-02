' Una interfaccia di base per oggetti collegabili (usata in FBD e nei blocchi del Ladder)

Public Interface IPureConnectable
    Inherits IPositionable, IHasLocalId, IHasName, ISizable, ISelectable, IDocumentable

    ReadOnly Property InConnectionRectangle() As Drawing.Rectangle
    ReadOnly Property OutConnectionRectangle() As Drawing.Rectangle
End Interface