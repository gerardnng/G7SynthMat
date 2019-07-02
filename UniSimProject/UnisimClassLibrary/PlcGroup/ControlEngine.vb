Imports UnisimClassLibrary
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Threading
Public Enum ControlState
    ControlRUN
    ControlSTOP
End Enum
Public Enum Errors
    NothingIOInterface
    ReadError
    WriteError
    InternalCycleError
    InitError
    ResettingErrors
    BuiltingErrors
End Enum
Public Class ControlEngine
    Private m_resource As Resource  'Risorsa gestita dal simulatore
    Private m_taskList As List(Of Task)   'Riferimento diretto alla lista dei task
    Private m_ProcessThread As Thread
    Private TaskToExecute As Task
    Private m_TimerDelegate As TimerCallback
    Private m_ControlState As ControlState = ControlState.ControlSTOP
    Private m_Cycles As Long            'Contatore del numero di cicli
    Private m_CyclesTimer As Timer      'Timer che azzera i cicli
    Private m_Init As Boolean           'Variabile che indica l'inizializzazione
    Private m_Stopped As Boolean        'Variabile che informa il thread di arrestarsi
    Private m_NowTime As DateTime       'Variabile per la funzione di scheduling
    Private m_IOInterface As IOInterface 'Interfaccia dispositivi fisici
    Private m_DeviceConnected As Boolean 'Indica se il Device di I/O è connesso
    Private m_ErrorsList As ArrayList   'Lista degli errori
    'Proprietà pubbliche
    Public ReadOnly Property State() As ControlState
        Get
            State = m_ControlState
        End Get
    End Property
    Public ReadOnly Property ErrorsList() As ArrayList
        Get
            ErrorsList = m_ErrorsList
        End Get
    End Property
    Public Property IOInterface() As IOInterface
        Get
            IOInterface = m_IOInterface
        End Get
        Set(ByVal Value As IOInterface)
            m_IOInterface = Value
        End Set
    End Property
    'Metodi pubblici
    Public Sub New(ByVal RefResource As Resource)
        m_TimerDelegate = New TimerCallback(AddressOf CyclesCount)
        m_resource = RefResource
        m_taskList = m_resource.tasks
        m_ErrorsList = New ArrayList
    End Sub
    Public Sub DisposeMe()
        Me.Finalize()
    End Sub
    Public Function Start() As Boolean
        m_ErrorsList.Clear()
        Start = True
        If Not IsNothing(m_IOInterface) Then
            m_Cycles = 0
            m_Stopped = False
            If Not m_resource.Built Then
                m_ProcessThread = New Thread(AddressOf StartControl)
            m_ProcessThread.Start()
            m_CyclesTimer = New Timer(m_TimerDelegate, m_Cycles, 0, 1000)
            m_ControlState = ControlState.ControlRUN
                Start = True
            Else
                'Errore di costruzione del progetto
                m_ErrorsList.Add(Errors.BuiltingErrors)
                Start = False
            End If
        Else
            'Errore di interfaccia
            m_ErrorsList.Add(Errors.NothingIOInterface)
            Start = False
        End If
    End Function
    Public Sub StopControl()
        m_Stopped = True     '(Il thread del processo legge la variabile m_SimStopped ad ogni ciclo)
        If Not IsNothing(m_ProcessThread) Then
            'Attende che termini il thread del processo
            m_ProcessThread.Join()
            m_CyclesTimer.Dispose()
        End If
        m_ControlState = ControlState.ControlSTOP
    End Sub
    Public Sub ResetResource()
        m_ErrorsList.Clear()
        Try
            m_resource.globalVars.Reset()
            For Each T As Task In m_resource.tasks
                For Each P As pouInstance In T.pouInstances
                    P.pou.Reset()
                Next P
            Next T
            For Each P As pouInstance In m_resource.pouInstances
                P.pou.Reset()
            Next P
            m_Init = False
        Catch ex As System.Exception
            'Aggiunge l'errore alla lista
            m_ErrorsList.Add(Errors.ResettingErrors)
        End Try
    End Sub
    'Eventi pubblici
    Public Event NewCycles(ByVal Value As Integer)
    'Funzioni di Controllo
    Private Function ScheduleTask() As Task
        'Seleziona il task da eseguire
        ScheduleTask = Nothing
        Dim LastTaskPrioriry As Integer = Integer.MaxValue
        For Each T As Task In m_taskList
            'Cerca se c'è un task pronto e seleziona il task con priorità più alta(0 è la massima)
            If Now.Subtract(T.m_LastActivation).TotalMilliseconds > T.m_interval.TotalMilliseconds Then
                If T.priority < LastTaskPrioriry Then
                    ScheduleTask = T
                    LastTaskPrioriry = T.priority
                End If
            End If
        Next T
    End Function
    Private Sub StartControl()
        'Effettua l'inizializzazione se non è gia stata effettuata
        If Not m_Init Then
            ExecuteControlInit()
        End If
        'Se non c'è stato un errore di inizializzazione inizia il ciclo
        If m_Init Then
            ExecuteControl()
        Else
            'Aggiunge l'errore alla lista
            m_ErrorsList.Add(Errors.InitError)
            m_ControlState = ControlState.ControlSTOP
            m_Init = False
        End If
    End Sub
    Private Sub ExecuteControl()
        'Ciclo di controllo
        While Not (m_Stopped)
            Try
                'Legge i dati dagli ingressi fisici
                If Not m_IOInterface.GetDigitalInputs() Then
                    'Aggiunge l'errore alla lista
                    m_ErrorsList.Add(Errors.ReadError)
                End If
                'Ciclo di scansione
                ExecuteControlCycle()
                'Scrive i dati sugli ingressi fisici
                If Not m_IOInterface.WriteDigitalOutputs() Then
                    'Aggiunge l'errore alla lista
                    m_ErrorsList.Add(Errors.WriteError)
                End If
                m_Cycles = m_Cycles + 1
            Catch ex As System.Exception
                'Aggiunge l'errore alla lista
                m_ErrorsList.Add(Errors.InternalCycleError)
            End Try
        End While
    End Sub
    Private Sub ExecuteControlCycle()
        'Seleziona il task da eseguire
        TaskToExecute = ScheduleTask()
        'Esegue il task selezionato
        If Not IsNothing(TaskToExecute) Then
            TaskToExecute.ExecutePousInstance()
        Else
            'Se non lo trova esegue i pou senza task
            For Each P As pouInstance In m_resource.pouInstances
                P.ExecutePouInstance()
            Next P
        End If
    End Sub
    Private Sub ExecuteControlInit()
        'Inizializza direttamente i pou
        Try
            'Inizializza i pou dei task
            For Each T As Task In m_resource.tasks
                For Each P As pouInstance In T.pouInstances
                    P.ExecutePouInit()
                Next P
            Next T
            'Inizializza i pou senza task
            For Each P As pouInstance In m_resource.pouInstances
                P.ExecutePouInit()
            Next P
            m_Init = True
        Catch ex As System.Exception

        End Try
    End Sub
    Private Sub CyclesCount(ByVal Obj As Object)
        RaiseEvent NewCycles(m_Cycles)
        Thread.VolatileWrite(m_Cycles, 0)
    End Sub

End Class
