' In apparenza questa classe non ha più alcuna utilità...
' Probabilmente era usata prima che nascesse la classe BooleanExpression
' Può essere rimossa?

Imports System.CodeDom
Imports System.IO
Imports System.CodeDom.Compiler
Imports Microsoft.VisualBasic

<Obsolete("Non usata da alcun elemento del codice")> _
Public Class LogicalInterpreter

    Private m_CodeStream As StringWriter
    Private m_expression As String
    Private m_Assembly As Reflection.Assembly   'Punta all'assembly compilato in memoria

    Private Sub WriteCode()
        m_CodeStream = New StringWriter()
        m_CodeStream.Write("Public Class Evaluator")
        m_CodeStream.WriteLine()
        m_CodeStream.Write("Public Function Evaluate() As Boolean")
        m_CodeStream.WriteLine()
        m_CodeStream.Write("Return " & m_expression)
        m_CodeStream.WriteLine()
        m_CodeStream.Write("End Function")
        m_CodeStream.WriteLine()
        m_CodeStream.Write("End Class")
    End Sub

    Private Function CompileCode() As Compiler.CompilerErrorCollection
        Dim Provider As New VBCodeProvider
        Dim CompParameters As New CompilerParameters
        CompParameters.GenerateInMemory = True
        CompParameters.ReferencedAssemblies.Add("s.dll") ' cos'è s.dll ?
        Dim CompResult As CompilerResults = Provider.CompileAssemblyFromSource(CompParameters, m_CodeStream.ToString())
        'Restituisce gli eventuali errori
        CompileCode = CompResult.Errors
        If CompileCode.Count > 0 Then 'Compilazione fallita
            m_Assembly = Nothing
        Else
            m_Assembly = CompResult.CompiledAssembly()
        End If
    End Function

    Private Function ExecuteCode() As Boolean
        If Not IsNothing(m_Assembly) Then
            Dim params(0) As Object
            Dim target As Object = m_Assembly.CreateInstance("Evaluator")
            Return CBool(m_Assembly.GetType("Evaluator").InvokeMember("Evaluate", Reflection.BindingFlags.InvokeMethod Or Reflection.BindingFlags.Public, Nothing, target, params))
        Else
            If CompileCode().Count > 0 Then Throw New ArgumentException("Error in LogicalInterpreter: code compilation failed")
            Return ExecuteCode()
        End If
    End Function

    Public Property Expression() As String
        Get
            Return m_expression
        End Get
        Set(ByVal value As String)
            m_expression = value
            WriteCode()
        End Set
    End Property

    Public Function Evaluate() As Boolean
        Evaluate = ExecuteCode()
    End Function

End Class
