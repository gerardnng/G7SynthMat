' Una interfaccia che definisce ciò che ci si aspetta di poter fare con una espressione
' La classe BooleanExpression e ArithmeticExpression la implementano (vedere note su
' quest'ultima, però). Se si aggiungono nuove espressioni, farne implementazioni
' di IExpression

Public Interface IExpression(Of ResultType)
    Function Calculate() As ResultType
    Function TryParse(ByVal expr As String) As Boolean
    Function GetExpressionString() As String
End Interface
