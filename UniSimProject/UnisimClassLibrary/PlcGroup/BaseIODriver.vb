Public MustInherit Class BaseIODriver
    Protected ErrorsList As ArrayList
    Public Sub New()
        ErrorsList = New ArrayList
    End Sub
    Public MustOverride Function TestDevice() As Boolean
    Public MustOverride Function GetDigitalInputNumber() As Integer
    Public MustOverride Function GetDigitalOutputNumber() As Integer
    Public MustOverride Function GetDigitalInputs(ByRef DigitalData As Boolean()) As Boolean
    Public MustOverride Function WriteDigitalOutputs(ByRef DigitalData As Boolean()) As Boolean
    Public Function GetErrors() As ArrayList
        GetErrors = ErrorsList
    End Function
End Class
