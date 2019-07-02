' Una interfaccia che definisce ci� che ci si aspetta di poter fare con una espressione
' La classe BooleanExpression e ArithmeticExpression la implementano (vedere note su
' quest'ultima, per�). Se si aggiungono nuove espressioni, farne implementazioni
' di IExpression

Public Interface IExpression(Of ResultType)
    Function Calculate() As ResultType
    Function TryParse(ByVal expr As String) As Boolean
    Function GetExpressionString() As String
End Interface
