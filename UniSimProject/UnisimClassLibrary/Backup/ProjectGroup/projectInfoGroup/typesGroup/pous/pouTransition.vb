' In apparenza questa classe non ha più alcuna utilità...
' Può essere rimossa?

<Obsolete("Non usata da alcun elemento del codice")> _
Public Class pouTransition
    Dim m_body As body
    Dim m_documentation As String
    Property Body() As body
        Get
            Body = m_body
        End Get
        Set(ByVal Value As body)
            m_body = Value
        End Set
    End Property
    Property Documentation() As String
        Get
            Documentation = m_documentation
        End Get
        Set(ByVal Value As String)
            m_documentation = Value
        End Set
    End Property
End Class
