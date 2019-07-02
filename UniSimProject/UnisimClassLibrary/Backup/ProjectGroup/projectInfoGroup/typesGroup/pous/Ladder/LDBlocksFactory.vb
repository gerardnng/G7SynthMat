' Analogamente all'FBD questa classe funge da factory per i blocchi usati in linguaggio a contatti
Imports System.Collections.Generic
Imports System

Public NotInheritable Class LDBlocksFactory

    Private Sub New()

    End Sub

    Public Shared Function ValidBlocks(ByVal plist As Pous) As String()
        Dim basicList As New List(Of String)( _
            New String() {"+", "-", "*", "/", ">", ">=", "<", "<=", "=", "!=", "IIf", "MOVE", "RAND"})
        If plist IsNot Nothing Then
            For Each p As pou In plist
                If p.PouType = EnumPouType.Function Then basicList.Add(p.Name)
            Next
        End If
        Return basicList.ToArray()
    End Function

    Public Shared Function CreateBlock(ByVal BlockName As String, _
        ByVal plist As Pous) As BlockLDTreeNode
        Select Case BlockName
            Case "+"
                Return New SumLDTreeNode()
            Case "-"
                Return New SubtractLDTreeNode()
            Case "*"
                Return New MultiplyLDTreeNode()
            Case "/"
                Return New DivideLDTreeNode()
            Case ">"
                Return New GreaterThanLDTreeNode()
            Case ">="
                Return New GreaterThanOrEqualLDTreeNode()
            Case "<"
                Return New LessThanLDTreeNode()
            Case "<="
                Return New LessThanOrEqualLDTreeNode()
            Case "="
                Return New EqualLDTreeNode()
            Case "!="
                Return New NotEqualLDTreeNode()
            Case "IIf"
                Return New IIfLDTreeNode()
            Case "MOVE"
                Return New MoveLDTreeNode()
            Case "RAND"
                Return New RandLDTreeNode()
            Case Else
                ' Promemoria: se abbiamo aggiunto qualche blocco lì e non qui...
                If Array.IndexOf(ValidBlocks(plist), BlockName) >= 0 Then
                    ' ...cerca tra le POU per prima cosa...
                    If plist.FindpouByName(BlockName) IsNot Nothing Then _
                        Return New POUBoundLDTreeNode(plist.FindpouByName(BlockName))
                    ' ...poi dai una eccezione
                    Throw New BlockNotBoundToClassException(BlockName)
                Else
                    ' altrimenti ritorna Nothing, non dovremmo neanche passare questo BlockName
                    Return Nothing
                End If
        End Select
    End Function

End Class
