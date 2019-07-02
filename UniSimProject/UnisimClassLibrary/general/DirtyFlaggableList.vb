Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Threading

' Per i dettagli sulla logica, vedere DirtyFlaggableArrayList.vb
' Questa è la versione lista generica
<Serializable()> _
Public Class DirtyFlaggableList(Of T)
    Inherits List(Of T)
    Implements IDirtyFlaggable

    Private m_Dirty As Boolean
    ' List(Of T) non fornisce un SyncRoot, e quindi bisogna "farselo in casa"
    Private SyncRoot As New Object()

    Public ReadOnly Property Dirty() As Boolean Implements IDirtyFlaggable.Dirty
        Get
            Return m_Dirty
        End Get
    End Property
    Public Function ClearDirtyFlag() As Boolean Implements IDirtyFlaggable.ClearDirtyFlag
        If Monitor.TryEnter(SyncRoot) Then
            Try
                Dim d As Boolean = Dirty
                m_Dirty = False
                Return d
            Finally
                Monitor.Exit(SyncRoot)
            End Try
        End If
    End Function
    Private Sub SetDirtyFlag()
        If Monitor.TryEnter(SyncRoot) Then
            Try
                If Not Dirty Then m_Dirty = False
            Finally
                Monitor.Exit(SyncRoot)
            End Try
        End If
    End Sub

    Public Sub New()
        MyBase.New()
        m_Dirty = False
    End Sub
    Public Sub New(ByVal ic As IEnumerable(Of T))
        MyBase.New(ic)
        m_Dirty = False
    End Sub
    Public Sub New(ByVal i As Int32)
        MyBase.New(i)
        m_Dirty = False
    End Sub

    Public Shadows Sub Add(ByVal value As T)
        SetDirtyFlag()
        MyBase.Add(value)
    End Sub
    Public Shadows Sub AddRange(ByVal c As IEnumerable(Of T))
        SetDirtyFlag()
        MyBase.AddRange(c)
    End Sub
    Public Shadows Sub Clear()
        SetDirtyFlag()
        MyBase.Clear()
    End Sub
    Public Shadows Sub Insert(ByVal index As Integer, ByVal value As T)
        SetDirtyFlag()
        MyBase.Insert(index, value)
    End Sub
    Public Shadows Sub InsertRange(ByVal index As Integer, ByVal c As IEnumerable(Of T))
        SetDirtyFlag()
        MyBase.InsertRange(index, c)
    End Sub
    Default Public Shadows Property Item(ByVal index As Integer) As T
        Get
            Return MyBase.Item(index)
        End Get
        Set(ByVal value As T)
            SetDirtyFlag()
            MyBase.Item(index) = value
        End Set
    End Property
    Public Shadows Sub Remove(ByVal obj As T)
        SetDirtyFlag()
        MyBase.Remove(obj)
    End Sub
    Public Shadows Sub RemoveAt(ByVal index As Integer)
        SetDirtyFlag()
        MyBase.RemoveAt(index)
    End Sub
    Public Shadows Sub RemoveRange(ByVal index As Integer, ByVal count As Integer)
        SetDirtyFlag()
        MyBase.RemoveRange(index, count)
    End Sub
    Public Shadows Sub Reverse()
        SetDirtyFlag()
        MyBase.Reverse()
    End Sub
    Public Shadows Sub Reverse(ByVal index As Integer, ByVal count As Integer)
        SetDirtyFlag()
        MyBase.Reverse(index, count)
    End Sub
    Public Shadows Sub Sort()
        SetDirtyFlag()
        MyBase.Sort()
    End Sub
    Public Shadows Sub Sort(ByVal comparison As Comparison(Of T))
        SetDirtyFlag()
        MyBase.Sort(comparison)
    End Sub
    Public Shadows Sub Sort(ByVal comparer As IComparer(Of T))
        SetDirtyFlag()
        MyBase.Sort(comparer)
    End Sub
    Public Shadows Sub Sort(ByVal index As Integer, ByVal count As Integer, ByVal comparer As IComparer(Of T))
        SetDirtyFlag()
        MyBase.Sort(index, count, comparer)
    End Sub
    Public Shadows Sub TrimExcess()
        SetDirtyFlag()
        MyBase.TrimExcess()
    End Sub
End Class
