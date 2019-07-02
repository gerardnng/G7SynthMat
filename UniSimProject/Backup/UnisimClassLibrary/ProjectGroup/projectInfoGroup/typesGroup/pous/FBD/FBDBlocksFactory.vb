Imports System.Collections.Generic
Imports System

Public Class BlockNotBoundToClassException
    Inherits InvalidOperationException

    Public Sub New(ByVal blockName As String)
        MyBase.New("Block " & blockName & "  has been registered as a valid block name, but was not bound to any implementation")
    End Sub

End Class

' Una classe che descrive un tipo di blocco in linguaggio FBD
Public Class FBDBlockDescriptor

    Private m_BlockName As String
    Private m_BlockType As Type
    Private m_MaxInputs As Integer

    Public Sub New(ByVal n As String, ByVal t As Type, ByVal i As Integer)
        m_BlockName = n
        m_BlockType = t
        m_MaxInputs = i
    End Sub

    Public ReadOnly Property BlockName() As String
        Get
            Return m_BlockName
        End Get
    End Property

    Public ReadOnly Property BlockType() As Type
        Get
            Return m_BlockType
        End Get
    End Property

    Public ReadOnly Property MaxInputs() As Integer
        Get
            Return m_MaxInputs
        End Get
    End Property

    Public Function CreateInstance() As BlockFBDTreeNode
        Dim defaultConstructor As Reflection.ConstructorInfo = _
            BlockType.GetConstructor(Reflection.BindingFlags.CreateInstance, Nothing, Reflection.CallingConventions.Any, _
             New Type() {}, Nothing)
        Dim ret As BlockFBDTreeNode = CType(defaultConstructor.Invoke(Nothing, _
            Reflection.BindingFlags.CreateInstance, Nothing, Nothing, Nothing), BlockFBDTreeNode)
        Return ret
    End Function

End Class

' Una classe che fa da Factory per i vari blocchi dell'FBD (non è codice di cui essere
' particolarmente fieri dal punto di vista della OOP, ma funziona bene)
Public NotInheritable Class FBDBlocksFactory

    Private Sub New()

    End Sub

    Public Shared Function ValidBlocks(ByVal plist As Pous) As String()
        Dim basicList As New List(Of String)( _
            New String() {"AND", "NAND", "OR", "NOR", "XOR", "RAND", "EQU", _
            "+", "-", "*", "/", ">", "<", "MOVE", "IIf", "RETURN"})
        If plist IsNot Nothing Then
            For Each p As pou In plist
                If p.PouType = EnumPouType.Function Then basicList.Add(p.Name)
            Next
        End If
        Return basicList.ToArray()
    End Function

    Public Shared Function CreateBlock(ByVal BlockName As String, _
        ByVal plist As Pous) As BlockFBDTreeNode
        Select Case BlockName
            Case "AND"
                Return New AndFBDBlock()
            Case "NAND"
                Return New NandFBDBlock()
            Case "OR"
                Return New OrFBDBlock()
            Case "NOR"
                Return New NorFBDBlock()
            Case "XOR"
                Return New XorFBDBlock()
            Case "RAND"
                Return New RandFBDBlock()
            Case "EQU"
                Return New EquFBDBlock()
            Case "+"
                Return New SumFBDBlock()
            Case "-"
                Return New SubtractFBDBlock()
            Case ">"
                Return New GreaterThanFBDBlock()
            Case "<"
                Return New LessThanFBDBlock()
            Case "*"
                Return New MultiplyFBDBlock()
            Case "/"
                Return New DivideFBDBlock()
            Case "MOVE"
                Return New MoveFBDBlock()
            Case "IIf"
                Return New IIfFBDBlock()
            Case "RETURN"
                Return New ReturnFBDBlock()
            Case Else
                ' Promemoria: se abbiamo aggiunto qualche blocco lì e non qui...
                If Array.IndexOf(ValidBlocks(plist), BlockName) >= 0 Then
                    ' ...cerca tra le POU per prima cosa...
                    If plist.FindpouByName(BlockName) IsNot Nothing Then _
                        Return New POUBoundFBDBlock(plist.FindpouByName(BlockName))
                    ' ...poi dai una eccezione
                    Throw New BlockNotBoundToClassException(BlockName)
                Else
                    ' altrimenti ritorna Nothing, non dovremmo neanche passare questo BlockName
                    Return Nothing
                End If
        End Select
    End Function

End Class
