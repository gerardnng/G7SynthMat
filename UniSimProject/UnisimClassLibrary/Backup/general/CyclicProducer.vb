' Una classe che restituisce ciclicamente tutti gli elementi di un array

Public Class CyclicProducer(Of T)

    Private m_Step As Integer
    Private m_Items() As T

    Public Sub New(ByVal ParamArray i() As T)
        m_Items = CType(Array.CreateInstance(GetType(T), i.Length), T())
        Array.Copy(i, m_Items, i.Length)
        Reset()
    End Sub

    ' Ritorna True finchè non termina un ciclo
    ' Il modo corretto di usare questa classe è
    ' Do
    '   Consume(CurrentItem)
    ' Loop While Advance()
    Public Function Advance() As Boolean
        CurrentStep += 1
        If CurrentStep = Length Then
            CurrentStep = 0
            Return False
        Else
            Return True
        End If
    End Function

    Public Sub Reset()
        CurrentStep = 0
    End Sub

    Public Property CurrentStep() As Integer
        Get
            Return m_Step
        End Get
        Private Set(ByVal s As Integer)
            m_Step = s
        End Set
    End Property

    Public ReadOnly Property CurrentItem() As T
        Get
            Return m_Items(CurrentStep)
        End Get
    End Property

    Public ReadOnly Property Length() As Integer
        Get
            Return m_Items.Length
        End Get
    End Property

End Class
