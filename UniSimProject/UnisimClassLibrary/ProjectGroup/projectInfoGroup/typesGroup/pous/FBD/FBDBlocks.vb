Public Class AndFBDBlock
    Inherits BlockFBDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "AND"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Dim value As Boolean = True
        For Each n As FBDTreeNode In Me
            value = value AndAlso n.GetNodeValue()
        Next
        Return value
    End Function
End Class

Public Class NandFBDBlock
    Inherits BlockFBDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "NAND"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Dim value As Boolean = True
        For Each n As FBDTreeNode In Me
            value = value AndAlso n.GetNodeValue()
        Next
        Return Not (value)
    End Function
End Class

Public Class OrFBDBlock
    Inherits BlockFBDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "OR"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Dim value As Boolean = False
        For Each n As FBDTreeNode In Me
            value = value OrElse n.GetNodeValue()
        Next
        Return value
    End Function
End Class

Public Class NorFBDBlock
    Inherits BlockFBDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "NOR"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Dim value As Boolean = False
        For Each n As FBDTreeNode In Me
            value = value OrElse n.GetNodeValue()
        Next
        Return Not (value)
    End Function
End Class

Public Class XorFBDBlock
    Inherits BlockFBDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "XOR"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Dim value As Boolean = False
        For Each n As FBDTreeNode In Me
            value = value Xor n.GetNodeValue()
        Next
        Return value
    End Function
End Class

Public Class EquFBDBlock
    Inherits BlockFBDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "EQU"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Dim value As Object = Me(0).GetNodeValue()
        Dim result As Boolean = True
        For i As Integer = 1 To Count - 1
            result = result AndAlso (value = Me(i).GetNodeValue())
        Next
        Return result
    End Function
End Class

Public Class SumFBDBlock
    Inherits BlockFBDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "+"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Dim value As Object = 0
        For Each n As FBDTreeNode In Me
            value += (n.GetNodeValue())
        Next
        Return value
    End Function
End Class

Public Class MultiplyFBDBlock
    Inherits BlockFBDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "*"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Dim value As Object = 1
        For Each n As FBDTreeNode In Me
            value *= (n.GetNodeValue())
        Next
        Return value
    End Function
End Class

Public Class SubtractFBDBlock
    Inherits BlockFBDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "-"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Dim value As Object = (Me(0).GetNodeValue())
        For i As Integer = 1 To Count - 1
            value -= (Me(i).GetNodeValue())
        Next
        Return value
    End Function
End Class

Public Class DivideFBDBlock
    Inherits BlockFBDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "/"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Dim value As Object = (Me(0).GetNodeValue())
        For i As Integer = 1 To Count - 1
            Dim v As Object = Me(i).GetNodeValue()
            ' Si dovrebbe discriminare sul segno di value e ritornare uno tra +oo, NaN, -oo
            ' ma (in fin dei conti) a che pro?
            If (v = 0) Then Return Double.NaN
            value /= v
        Next
        Return value
    End Function
End Class

Public Class GreaterThanFBDBlock
    Inherits BlockFBDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return ">"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Dim value As Object = Me(0).GetNodeValue()
        Dim result As Boolean = True
        For i As Integer = 1 To Count - 1
            result = result AndAlso (value > Me(i).GetNodeValue())
        Next
        Return result
    End Function
End Class

Public Class LessThanFBDBlock
    Inherits BlockFBDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "<"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Dim value As Object = Me(0).GetNodeValue()
        Dim result As Boolean = True
        For i As Integer = 1 To Count - 1
            result = result AndAlso (value < Me(i).GetNodeValue())
        Next
        Return result
    End Function
End Class


Public Class RandFBDBlock
    Inherits BlockFBDTreeNode

    Private Shared m_Randomizer As New Random()

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "RAND"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Return (m_Randomizer.Next(0, 2) = 1)
    End Function
End Class

Public Class MoveFBDBlock
    Inherits BlockFBDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "MOVE"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Return Me(0).GetNodeValue()
    End Function
End Class

Public Class IIfFBDBlock
    Inherits BlockFBDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "IIf"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Return IIf(CBool(Me(0).GetNodeValue()), Me(1).GetNodeValue(), Me(2).GetNodeValue())
    End Function
End Class

Public Class POUBoundFBDBlock
    Inherits BlockFBDTreeNode

    Private m_pou As pou

    Public Sub New(ByVal p As pou)
        m_pou = p
    End Sub

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return m_pou.Name
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        m_pou.ExecuteInit()
        Dim vars As VariablesList = m_pou.PouInterface.inputVars
        vars.Reset()
        Dim i As Integer = 0
        Dim max As Integer = Math.Min(Me.Count, vars.Count)
        While i < max
            vars(i).SetActValue(Me(i).GetNodeValue())
            i += 1
        End While
        m_pou.ExecuteScanCycle()
        Return m_pou.PouInterface.outputVars.FindVariableByName("OUT").ReadActValue()
    End Function
End Class

' Il blocco RETURN solleva una eccezione se la Or dei suoi ingressi ha valore alto
' L'interprete FBD cattura questa eccezione e la gestisce smettendo di valutare le
' reti del diagramma. Questa classe è l'apposita eccezione. In effetti la scelta di
' usare eccezioni per comunicare tra strati software potrebbe essere opinabile, ma
' l'eventualità che si voglia fare un RETURN è sufficientemente rara e sufficientemente
' speciale rispetto all'ordinaria semantica dell'FBD che questo modus operandi risulta
' ammissibile. Si dovrebbe evitare l'uso di eccezioni per "comunicazioni" meno sporadiche
Public Class FBDReturnException
    Inherits Exception

    Public Sub New()

    End Sub
End Class

Public Class ReturnFBDBlock
    Inherits BlockFBDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "RETURN"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Dim value As Boolean = False
        For Each n As FBDTreeNode In Me
            value = value OrElse n.GetNodeValue()
        Next
        If value Then Throw New FBDReturnException()
        Return False
    End Function
End Class