Imports System
Imports System.Threading
Imports System.Collections

' Una ArrayList che può tenere traccia dell'essere stata modificata. E' utile nei casi in cui bisogna
' sapere se un documento ha subito modifiche, oppure se bisogna rigenerare delle informazioni a partire
' da una lista oppure quelle in cache sono ancora valide, ...
' La gestione del flag Dirty è thread-safe, quella della lista in sè no, come riportato dall'MSDN:
'
' Thread Safety
' Public static (Shared in Visual Basic) members of this type are thread safe. Any instance members are not
' guaranteed to be thread safe.
' An ArrayList can support multiple readers concurrently, as long as the collection is not modified.
' To guarantee the thread safety of the ArrayList, all operations must be done through the wrapper returned
' by the Synchronized method.
' Enumerating through a collection is intrinsically not a thread safe procedure. Even when a collection is
' synchronized, other threads can still modify the collection, which causes the enumerator to throw an exception.
' To guarantee thread safety during enumeration, you can either lock the collection during the entire enumeration
' or catch the exceptions resulting from changes made by other threads.

<Serializable()> _
Public Class DirtyFlaggableArrayList
    Inherits ArrayList
    Implements IDirtyFlaggable

    Private m_Dirty As Boolean

    Public Sub New()
        MyBase.New()
        m_Dirty = False
    End Sub
    Public Sub New(ByVal ic As ICollection)
        MyBase.New(ic)
        m_Dirty = False
    End Sub
    Public Sub New(ByVal i As Int32)
        MyBase.New(i)
        m_Dirty = False
    End Sub

    Public ReadOnly Property Dirty() As Boolean Implements IDirtyFlaggable.Dirty
        Get
            Return m_Dirty
        End Get
    End Property
    Public Function ClearDirtyFlag() As Boolean Implements IDirtyFlaggable.ClearDirtyFlag
        If Monitor.TryEnter(MyBase.SyncRoot) Then
            Try
                Dim d As Boolean = Dirty
                m_Dirty = False
                Return d
            Finally
                Monitor.Exit(MyBase.SyncRoot)
            End Try
        End If
    End Function
    Private Sub SetDirtyFlag()
        If Monitor.TryEnter(MyBase.SyncRoot) Then
            Try
                If Not Dirty Then m_Dirty = False
            Finally
                Monitor.Exit(MyBase.SyncRoot)
            End Try
        End If
    End Sub

    Public Overrides Function Add(ByVal value As Object) As Integer
        SetDirtyFlag()
        Return MyBase.Add(value)
    End Function
    Public Overrides Sub AddRange(ByVal c As System.Collections.ICollection)
        SetDirtyFlag()
        MyBase.AddRange(c)
    End Sub
    Public Overrides Sub Clear()
        SetDirtyFlag()
        MyBase.Clear()
    End Sub
    Public Overrides Sub Insert(ByVal index As Integer, ByVal value As Object)
        SetDirtyFlag()
        MyBase.Insert(index, value)
    End Sub
    Public Overrides Sub InsertRange(ByVal index As Integer, ByVal c As System.Collections.ICollection)
        SetDirtyFlag()
        MyBase.InsertRange(index, c)
    End Sub
    Default Public Overrides Property Item(ByVal index As Integer) As Object
        Get
            Return MyBase.Item(index)
        End Get
        Set(ByVal value As Object)
            SetDirtyFlag()
            MyBase.Item(index) = value
        End Set
    End Property
    Public Overrides Sub Remove(ByVal obj As Object)
        SetDirtyFlag()
        MyBase.Remove(obj)
    End Sub
    Public Overrides Sub RemoveAt(ByVal index As Integer)
        SetDirtyFlag()
        MyBase.RemoveAt(index)
    End Sub
    Public Overrides Sub RemoveRange(ByVal index As Integer, ByVal count As Integer)
        SetDirtyFlag()
        MyBase.RemoveRange(index, count)
    End Sub
    Public Overrides Sub Reverse()
        SetDirtyFlag()
        MyBase.Reverse()
    End Sub
    Public Overrides Sub Reverse(ByVal index As Integer, ByVal count As Integer)
        SetDirtyFlag()
        MyBase.Reverse(index, count)
    End Sub
    Public Overrides Sub SetRange(ByVal index As Integer, ByVal c As System.Collections.ICollection)
        SetDirtyFlag()
        MyBase.SetRange(index, c)
    End Sub
    Public Overrides Sub Sort()
        SetDirtyFlag()
        MyBase.Sort()
    End Sub
    Public Overrides Sub Sort(ByVal index As Integer, ByVal count As Integer, ByVal comparer As System.Collections.IComparer)
        SetDirtyFlag()
        MyBase.Sort(index, count, comparer)
    End Sub
    Public Overrides Sub Sort(ByVal comparer As System.Collections.IComparer)
        SetDirtyFlag()
        MyBase.Sort(comparer)
    End Sub
    Public Overrides Sub TrimToSize()
        SetDirtyFlag()
        MyBase.TrimToSize()
    End Sub

End Class
