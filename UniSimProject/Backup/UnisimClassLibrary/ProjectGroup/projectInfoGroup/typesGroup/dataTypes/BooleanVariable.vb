Imports System.Threading
Imports System.Xml
Public Class BooleanVariable
    Inherits BaseVariable
    Protected m_Value As Boolean
    ' Usato per i riconoscitori di fronti
    Protected m_OldValue As Boolean
    Public Sub New(ByVal Name As String, ByVal Documentation As String, ByVal Address As String, ByVal IniValue As String)
        MyBase.New(Name, Documentation, Address, IniValue)
        m_Value = CBool(IniValue)
        OldValue = m_Value
        m_dataType = "BOOL"
    End Sub
    Public Sub New()
        MyBase.New()
    End Sub
    Public Overrides Sub SetValue(ByVal Value As Object)
        If Locked Then Exit Sub
        If m_Value <> Value Then
            ' Quale colore??
            'Il valore è cambiato quindi lo memorizza e cambia il colore se richiesto
            Try
                ResourceTab.AddResource(Me, Thread.CurrentThread)
                If Monitor.TryEnter(Me) Then
                    'Dim sw As New Stopwatch()
                    Dim a As Integer = 0
                    'sw.Start()
                    ' Questa attesa dura circa 61ms (e non è poco in UniSim)
                    ' Serve per risolvere alcuni problemi di sincronizzazione verso il Monitor variabili
                    ' esempio il seguente programma Ladder (una XOR):
                    ' 
                    ' ---|X|---|/Y|------
                    '                    ------(PZ) ' (PZ) = Positive-Transition Sensing Coil su Z
                    ' ---|/X|---|Y|------
                    '
                    ' senza questa attesa molti fronti vengono ignorati, inserendola tutto funziona
                    While a < 10000000
                        a = a + 1
                    End While
                    'sw.Stop()
                    'MsgBox(sw.ElapsedMilliseconds)
                    m_OldValue = m_Value
                    m_Value = Value
                    Monitor.Exit(Me)
                    ResourceTab.RemoveThread(Me, Thread.CurrentThread)
                Else
                    ResourceTab.RaiseVarLock(Me)
                    ResourceTab.AddResource(Me, Thread.CurrentThread)
                    Monitor.Enter(Me)
                    m_OldValue = m_Value
                    m_Value = Value
                    Monitor.Exit(Me)
                    ResourceTab.RemoveThread(Me, Thread.CurrentThread)
                End If
            Catch ex As System.Exception
                ResourceTab.RemoveThread(Me, Thread.CurrentThread)
                Monitor.Exit(Me)
            End Try
            'Chiama la funzione per generare l'evento di notifica cambio valore
            RaiseValueChanged()
        End If
    End Sub
    Public Overrides Function ReadValue() As Object
        If Monitor.TryEnter(Me, 500) Then
            Try
                Return m_Value
            Finally
                Monitor.Exit(Me)
            End Try
        Else
            Return VariablesManager.DefaultValue("BOOL")
        End If
    End Function
    Public Overrides Sub SetInitialValue(ByVal Value As Object)
        m_InitialValue = Value
    End Sub
    Public Overrides Function ReadInitialValue() As Object
        ReadInitialValue = m_InitialValue
    End Function
    Public Overrides Function ReadActValue() As Object
        Return ReadValue()
    End Function
    Public Overrides Function CheckValue(ByVal Value As String) As Boolean
        Try
            Dim NewValue As Boolean = CBool(Value)
            CheckValue = True
        Catch ex As System.Exception
            CheckValue = False
        End Try
    End Function
    Public Overrides Sub IncreaseValue(ByVal Value As Object)

    End Sub
    Public Overrides Sub DecreaseValue(ByVal Value As Object)

    End Sub
    Public Overrides Sub ResetValue()
        SetValue(ReadInitialValue())
        OldValue = ReadValue()
    End Sub
    Public Overrides Sub SetActValue(ByVal Value As Object)
        SetValue(Value)
    End Sub

    Private Sub ToggleVariable(ByVal sender As Object, ByVal e As EventArgs)
        Me.SetValue(Not (Me.ReadValue()))
    End Sub

    Public Overrides Function GetMenu() As System.Windows.Forms.MenuItem
        Dim myMenu As New System.Windows.Forms.MenuItem(Me.Name, AddressOf ToggleVariable)
        myMenu.Checked = Me.ReadValue()
        Return myMenu
    End Function

    Public Property OldValue() As Boolean
        Get
            Return m_oldvalue
        End Get
        Private Set(ByVal value As Boolean)
            m_oldvalue = value
        End Set
    End Property

End Class

