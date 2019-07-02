Public Class pouAction
    Dim m_body As body
    Dim m_documentation As String
    Property Body() As body
        Get
            body = m_body
        End Get
        Set(ByVal Value As body)
            m_body = Value
        End Set
    End Property
    Property Documentation() As String
        Get
            documentation = m_documentation
        End Get
        Set(ByVal Value As String)
            m_documentation = Value
        End Set
    End Property
End Class
