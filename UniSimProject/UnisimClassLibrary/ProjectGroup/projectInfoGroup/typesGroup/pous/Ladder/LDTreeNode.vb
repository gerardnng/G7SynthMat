Imports System
Imports System.Collections.Generic

' Se dovesse servire l'inclusione di un concetto di stato nell'LD di UniSim, la scelta prevista è quella
' di inserire le informazioni di stato nella connessione e di estendere la GetNodeValue() per usare un oggetto
' di questo tipo, che adatti il valore corrente dell'oggetto in base allo stato (la rationale è vedere la
' connessione come un vero e proprio "filo" in cui far passare i valori e che pertanto può vedere tutta la
' successione degli stati)
Public Interface ILDAdapter
    Function Adapt(ByVal input As Object) As Object
End Interface

Public MustInherit Class LDTreeNode
    Private m_SubNodes As List(Of LDTreeNode)
    Private m_Parent As LDTreeNode
    Private m_Negated As Boolean
    Public Sub New()
        m_SubNodes = New List(Of LDTreeNode)
        m_Parent = Nothing
    End Sub

    ' Calculate() ritorna il valore associato al nodo in quanto tale
    ' Serve anche GetNodeValue() per garantire che sarà rispettata la
    ' negazione associata alle connessioni (si potrebbe fare in modo che sia
    ' il nodo a negarsi se necessario, ma è duplicazione di codice ed è
    ' un approccio più soggetto ad errori)
    Public MustOverride Function Calculate() As Object

    Public Function GetNodeValue() As Object
        Return Calculate()
    End Function
    Public Sub AddNode(ByVal newNode As LDTreeNode)
        m_SubNodes.Add(newNode)
        newNode.Parent = Me
    End Sub
    Public Function GetEnumerator() As IEnumerator(Of LDTreeNode)
        Return m_SubNodes.GetEnumerator()
    End Function
    Public Sub ClearNodes()
        For Each SN As LDTreeNode In Me
            SN.Parent = Nothing
        Next
        m_SubNodes.Clear()
    End Sub
    Public Property Parent() As LDTreeNode
        Get
            Return m_Parent
        End Get
        Set(ByVal value As LDTreeNode)
            m_Parent = value
        End Set
    End Property
    Public ReadOnly Property Count() As Integer
        Get
            Return m_SubNodes.Count
        End Get
    End Property
    Default Public Property Item(ByVal Index As Integer) As LDTreeNode
        Get
            Return m_SubNodes.Item(Index)
        End Get
        Set(ByVal value As LDTreeNode)
            m_SubNodes.Item(Index) = value
        End Set
    End Property
    Public Sub RemoveNode(ByVal nodeToDrop As LDTreeNode)
        If m_SubNodes.Remove(nodeToDrop) Then nodeToDrop.Parent = Nothing
    End Sub
End Class

Public Class VariableBoundLDTreeNode
    Inherits LDTreeNode

    Private m_Variable As BaseVariable

    Public Sub New(ByVal var As BaseVariable, Optional ByVal inv As Boolean = False)
        MyBase.New()
        m_Variable = var
    End Sub

    Public Overrides Function Calculate() As Object
        Return m_Variable.ReadActValue()
    End Function
End Class

Public MustInherit Class BlockLDTreeNode
    Inherits LDTreeNode

    Public MustOverride ReadOnly Property BlockName() As String

End Class