Imports System
Imports System.Collections
Imports System.Collections.Generic

Public Class ConstrainedArrayList
    Inherits ArrayList

    Private m_Constraint As Predicate(Of Object)

    Public Sub New(ByVal pred As Predicate(Of Object))
        m_Constraint = pred
    End Sub

    Public Shadows Function Add(ByVal value As Object) As Integer
        If Not (m_Constraint(value)) Then Throw New ArgumentException()
        MyBase.Add(value)
    End Function
    Public Shadows Sub AddRange(ByVal c As System.Collections.ICollection)
        For Each o As Object In c
            Add(o)
        Next
    End Sub
    Public Shadows Sub Insert(ByVal index As Integer, ByVal value As Object)
        If Not (m_Constraint(value)) Then Throw New ArgumentException()
        MyBase.Insert(index, value)
    End Sub
    Public Shadows Sub InsertRange(ByVal index As Integer, ByVal c As System.Collections.ICollection)
        For Each o As Object In c
            Insert(index, o)
            index += 1
        Next
    End Sub

End Class