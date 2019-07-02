Imports System.Collections.Generic
Public Class VariablesLists
    Inherits List(Of VariablesList)
    Public Sub New()
        MyBase.new()
    End Sub
    Public Shadows Function Add(ByRef VarList As VariablesList) As Integer
        MyBase.Add(VarList)
        Return MyBase.IndexOf(VarList)
    End Function
    Public Shadows Sub Remove(ByRef VarList As VariablesList)
        MyBase.Remove(VarList)
        VarList.DisposeMe()
    End Sub
    Public Sub RemoveListByName(ByVal Value As String)
        For Each L As VariablesList In Me
            If L.Name = Value Then
                MyBase.Remove(L)
                Exit For
            End If
        Next L
    End Sub
    Public Function FindListByName(ByVal Value As String) As VariablesList
        For Each L As VariablesList In Me
            If L.Name = Value Then
                Return L
            End If
        Next L
        Return Nothing
    End Function
    Public Function FindVariableByName(ByVal Value As String) As BaseVariable
        FindVariableByName = Nothing
        For Each L As VariablesList In Me
            FindVariableByName = L.FindVariableByName(Value)
            If Not IsNothing(FindVariableByName) Then
                Exit For
            End If
        Next L
    End Function
    Public Sub Reset()
        'Resetta al valore iniziale tutte le variabili delle liste
        For Each L As VariablesList In Me
            L.Reset()
        Next L
    End Sub
End Class
