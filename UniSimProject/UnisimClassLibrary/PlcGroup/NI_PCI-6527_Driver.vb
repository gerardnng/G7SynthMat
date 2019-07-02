Imports NationalInstruments.DAQmx
Public Class NI_PCI_6527_Driver
    Inherits BaseIODriver
    'Task
    Dim DigitalReaderTask As NationalInstruments.DAQmx.Task
    Dim DigitalWriteTask As NationalInstruments.DAQmx.Task
    'Geter
    Dim Reader As DigitalSingleChannelReader
    'Writer
    Dim Writer As DigitalSingleChannelWriter
    Public Sub New(ByVal DeviceID As String)
        MyBase.New()
        ErrorsList.Clear()
        Try
            'Crea i canali di I/O e assegna le linee
            DigitalReaderTask = New NationalInstruments.DAQmx.Task("DigitalGeterTask")
            DigitalReaderTask.DIChannels.CreateChannel(DeviceID & "/Port0:2", "", ChannelLineGrouping.OneChannelForAllLines)
            Reader = New DigitalSingleChannelReader(DigitalReaderTask.Stream)

            DigitalWriteTask = New NationalInstruments.DAQmx.Task("DigitalWriteTask")
            DigitalWriteTask.DOChannels.CreateChannel(DeviceID & "/Port3:5", "", ChannelLineGrouping.OneChannelForAllLines)
            Writer = New DigitalSingleChannelWriter(DigitalWriteTask.Stream)

        Catch exception As DaqException
            ErrorsList.Add(exception.Message)
            'Distrugge i task se creati
            If Not IsNothing(DigitalReaderTask) Then
                DigitalReaderTask.Dispose()
            End If
            If Not IsNothing(DigitalWriteTask) Then
                DigitalWriteTask.Dispose()
            End If
        End Try
    End Sub
    Public Overrides Function GetDigitalInputNumber() As Integer
        GetDigitalInputNumber = 24
    End Function
    Public Overrides Function GetDigitalOutputNumber() As Integer
        GetDigitalOutputNumber = 24
    End Function
    Public Overrides Function TestDevice() As Boolean
        'fa un tentativo di lettura e scrittura
        Dim DigitalData(23) As Boolean
        If WriteDigitalOutputs(DigitalData) And GetDigitalInputs(DigitalData) Then
            TestDevice = True
        Else
            TestDevice = False
        End If
    End Function
    Public Overrides Function GetDigitalInputs(ByRef DigitalData() As Boolean) As Boolean
        'Restituisce False se è stato generato un errore e lo memorizza in ErrorsList
        Try
            DigitalData = Reader.ReadSingleSampleMultiLine()
            GetDigitalInputs = True
        Catch exception As DaqException
            ErrorsList.Add(exception.Message)
            GetDigitalInputs = False
        End Try
    End Function
    Public Overrides Function WriteDigitalOutputs(ByRef DigitalData() As Boolean) As Boolean
        'Restituisce False se è stato generato un errore e lo memorizza in ErrorsList
        Try
            Writer.WriteSingleSampleMultiLine(True, DigitalData)
            WriteDigitalOutputs = True
        Catch exception As DaqException
            ErrorsList.Add(exception.Message)
            WriteDigitalOutputs = False
        End Try
    End Function
End Class
