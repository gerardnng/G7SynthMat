Public Class SumLDTreeNode
    Inherits BlockLDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "+"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Dim ret As Object = 0
        For Each input As LDTreeNode In Me
            ret += input.GetNodeValue()
        Next
        Return ret
    End Function
End Class

Public Class SubtractLDTreeNode
    Inherits BlockLDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "-"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Dim ret As Object = Me(0).GetNodeValue()
        For i As Integer = 1 To Me.Count - 1
            ret -= Me(i).GetNodeValue()
        Next
        Return ret
    End Function
End Class

Public Class MultiplyLDTreeNode
    Inherits BlockLDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "*"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Dim ret As Object = 1
        For Each input As LDTreeNode In Me
            ret *= input.GetNodeValue()
        Next
        Return ret
    End Function
End Class

Public Class DivideLDTreeNode
    Inherits BlockLDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "/"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Dim ret As Object = Me(0).GetNodeValue()
        For i As Integer = 1 To Me.Count - 1
            Dim v As Object = Me(i).GetNodeValue()
            ' Si dovrebbe discriminare sul segno di value e ritornare uno tra +oo, NaN, -oo
            ' ma (in fin dei conti) a che pro?
            If (v = 0) Then Return Double.NaN
            ret /= v
        Next
        Return ret
    End Function
End Class

Public Class GreaterThanLDTreeNode
    Inherits BlockLDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return ">"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Return Me(0).GetNodeValue() > Me(1).GetNodeValue()
    End Function
End Class

Public Class GreaterThanOrEqualLDTreeNode
    Inherits BlockLDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return ">="
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Return Me(0).GetNodeValue() >= Me(1).GetNodeValue()
    End Function
End Class

Public Class LessThanLDTreeNode
    Inherits BlockLDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "<"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Return Me(0).GetNodeValue() < Me(1).GetNodeValue()
    End Function
End Class

Public Class LessThanOrEqualLDTreeNode
    Inherits BlockLDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "<="
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Return Me(0).GetNodeValue() <= Me(1).GetNodeValue()
    End Function
End Class

Public Class EqualLDTreeNode
    Inherits BlockLDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "="
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Return Me(0).GetNodeValue() = Me(1).GetNodeValue()
    End Function
End Class

Public Class NotEqualLDTreeNode
    Inherits BlockLDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "!="
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Return Me(0).GetNodeValue() <> Me(1).GetNodeValue()
    End Function
End Class

Public Class IIfLDTreeNode
    Inherits BlockLDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "IIf"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Return IIf(Me(0).GetNodeValue(), Me(1).GetNodeValue(), Me(2).GetNodeValue())
    End Function
End Class

Public Class MoveLDTreeNode
    Inherits BlockLDTreeNode

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "MOVE"
        End Get
    End Property

    Public Overrides Function Calculate() As Object
        Return Me(0).GetNodeValue()
    End Function
End Class

Public Class RandLDTreeNode
    Inherits BlockLDTreeNode

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

Public Class POUBoundLDTreeNode
    Inherits BlockLDTreeNode

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