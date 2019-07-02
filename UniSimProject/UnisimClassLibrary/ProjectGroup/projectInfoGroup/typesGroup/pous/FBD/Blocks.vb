' Questo file contiene i blocchi del linguaggio FBD

Public Class AndBlock
    Inherits GraphicalBlock

    Public Sub New(ByVal body As IIEC61131LanguageImplementation, ByVal id As Integer, _
        ByVal dimen As Drawing.Rectangle, ByVal post As Drawing.Point, _
        ByVal gtd As Drawing.Graphics)
        MyBase.New(body, id, dimen, post, gtd)
    End Sub

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "AND"
        End Get
    End Property

    ' Segnatura attesa: OUT := AND(IN)
    Public Overrides Sub ExecuteBlock()
        Dim output As GraphicalVariable = MyBase.GetVariableByFormalParameter("OUT", FBDVariableType.Output)
        Dim andresult As Boolean = True
        For Each B As BlockParameter In MyBase.InputVariables
            andresult = andresult AndAlso _
                MyBase.GetVariableByFormalParameter(B.FormalName, FBDVariableType.Input).BoundVariable.ReadValue()
        Next
        output.BoundVariable.SetValue(andresult)
    End Sub
End Class

Public Class OrBlock
    Inherits GraphicalBlock

    Public Sub New(ByVal body As IIEC61131LanguageImplementation, ByVal id As Integer, _
        ByVal dimen As Drawing.Rectangle, ByVal post As Drawing.Point, _
        ByVal gtd As Drawing.Graphics)
        MyBase.New(body, id, dimen, post, gtd)
    End Sub

    Public Overrides ReadOnly Property BlockName() As String
        Get
            Return "OR"
        End Get
    End Property

    ' Segnatura attesa: OUT := OR(IN)
    Public Overrides Sub ExecuteBlock()
        Dim output As GraphicalVariable = MyBase.GetVariableByFormalParameter("OUT", FBDVariableType.Output)
        Dim andresult As Boolean = False
        For Each B As BlockParameter In MyBase.InputVariables
            andresult = andresult OrElse _
                MyBase.GetVariableByFormalParameter(B.FormalName, FBDVariableType.Input).BoundVariable.ReadValue()
        Next
        output.BoundVariable.SetValue(andresult)
    End Sub
End Class