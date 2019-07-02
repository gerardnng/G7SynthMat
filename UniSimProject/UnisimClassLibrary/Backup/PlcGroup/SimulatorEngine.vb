Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Threading
Public Enum SimulationState
    SimRUN
    SimSTOP
End Enum

Public NotInheritable Class SimulatorEngine
    Implements IExecutable

    Private m_resource As Resource  'Risorsa gestita dal simulatore
    Private m_taskList As List(Of Task)   'Riferimento diretto alla lista dei task
    Private m_SimulationThread As Thread
    Private m_TaskExecuted As Task
    Private WithEvents m_ResourceTab As ResourceTable = ResourceTab
    Private m_TimerDelegate As TimerCallback
    Private m_simulationState As SimulationState
    Private m_TaskExecutedPrioriry As Integer = 65536
    Private m_ResoursePouInstanceTask As Task   'Task a priotità minima per i pou della risorsa
    Private m_PreemptiveScheduling As Boolean   'Variabile che indica il tipo di scheduling da effettuare
    Private m_TaskThreadsKilled As Boolean      'Variabile che indica se i task in sospeso avviati dall'algoritmo preemptive sono stati distrutti
    Private m_Cycles As Long            'Contatore del numero di cicli
    Private m_CyclesTimer As Timer      'Timer che azzera i cicli
    Private m_Wait As Integer           'Tempo di attesa tra cicli
    Private m_Init As Boolean           'Variabile che indica l'inizializzazione
    Private m_Jumps As Long             'Variabile che conta i cicli non effettuati
    Private m_Stopped As Boolean        'Variabile che informa il thread di arrestarsi
    Private m_NowTime As DateTime       'Variabile per la funzione di scheduling
    Private m_SimDuration As Integer    'Numero di secondi in simulazione
    Public Property State() As SimulationState
        Get
            State = m_simulationState
        End Get
        Set(ByVal Value As SimulationState)
            m_simulationState = Value
        End Set
    End Property
    Public Property PreemptiveScheduling() As Boolean
        Get
            PreemptiveScheduling = m_PreemptiveScheduling
        End Get
        Set(ByVal Value As Boolean)
            m_PreemptiveScheduling = Value
            If Value Then
                m_TaskThreadsKilled = False
            End If
        End Set
    End Property
    Public Sub New(ByVal RefResource As Resource)
        m_TimerDelegate = New TimerCallback(AddressOf CyclesCount)
        m_Wait = 0
        m_Jumps = 0
        m_resource = RefResource
        m_taskList = m_resource.tasks
        m_TaskThreadsKilled = True
        'Configura il task per i pou della risorsa
        m_ResoursePouInstanceTask = New Task("", 65636, New TimeSpan(0), m_resource.pouInstances)
        m_TaskExecuted = m_ResoursePouInstanceTask
        PreemptiveScheduling = False
    End Sub
    Public Sub DisposeMe()
        Me.Finalize()
    End Sub
    Public Function Start() As Boolean
        m_Cycles = 0
        m_Jumps = 0
        m_Stopped = False
        SimulationDuration = 0
        m_SimulationThread = New Thread(AddressOf StartSimulation)
        m_SimulationThread.Start()
        m_CyclesTimer = New Timer(m_TimerDelegate, m_Cycles, 0, 1000)
        State = SimulationState.SimRUN
    End Function
    Public Sub StopSimulation()
        m_Stopped = True     '(Il thread del processo legge la variabile m_SimStopped ad ogni ciclo)
        If Not IsNothing(m_SimulationThread) Then
            'Attende che termini il thread del processo
            m_SimulationThread.Join()
            'Attende che terminino gli eventuale thread dell'algoritmo preemptive
            If Not m_TaskThreadsKilled Then
                KillsTaskThreads()
            End If
            m_CyclesTimer.Dispose()
        End If
        State = SimulationState.SimSTOP
    End Sub
    Private Sub KillsTaskThreads()
        'Distrugge gli eventuali thread in sospeso dell'algoritmo preemptive
        For Each T As Task In m_resource.tasks
            T.ResumeThread()
            T.Join()
        Next T
        m_ResoursePouInstanceTask.ResumeThread()
        m_ResoursePouInstanceTask.Join()
        m_TaskThreadsKilled = True
    End Sub
    Public Sub ExecuteSimulationNextStep()
        'Permette di eseguire un passo di simulazione
        If Not m_Init Then
            ExecuteSimulationInit()
            m_Init = True
        Else
            ExecuteNotPreemptiveSimulationScanCycle()
        End If
    End Sub
    Public Sub SetWait(ByVal Value As Integer)
        m_Wait = Value
    End Sub
    Private Function ScheduleTask() As Task
        'Seleziona il task da eseguire
        ScheduleTask = Nothing
        Try
            Dim LastTaskPrioriry As Integer = Integer.MaxValue
            If Monitor.TryEnter(m_taskList, 500) Then
                'Memorizza l'istante di tempo attuale
                m_NowTime = Now
                For Each T As Task In m_taskList
                    'Cerca ci sono task pronti o sospesi seleziona quello con priorità più alta(0 è la massima)
                    If (m_NowTime.Subtract(T.m_LastActivation).TotalMilliseconds > T.m_interval.TotalMilliseconds And T.Ready) Or T.ExecutionThread.IsAlive Then
                        If T.priority < LastTaskPrioriry Then
                            ScheduleTask = T
                            LastTaskPrioriry = T.priority
                        End If
                    End If
                Next T
                Monitor.Exit(m_taskList)
            End If
        Catch ex As System.Exception
            Monitor.Exit(m_taskList)
        End Try
    End Function
    'Funzioni di simulazione
    Private Sub StartSimulation()
        'Nel modo simulazione controlla direttamente i pous nella lista dei types....
        '....quindi effettua il monitor sulla lista dei pous in types

        'Effettua l'inizializzazione se non è gia stata effettuata
        If Not m_Init Then
            ExecuteSimulationInit()
            m_Init = True
        End If
        'Assegna come task da eseguire quello dei pou della risorsa (ha priorità minima)
        m_TaskExecuted = m_ResoursePouInstanceTask
        m_TaskExecutedPrioriry = m_TaskExecuted.priority
        'Esegue la simulazione
        ExecuteSimulation()
    End Sub
    Private Sub ExecuteSimulation()
        'Nel modo simulazione controlla direttamente i pous nella lista dei types....
        '....quindi effettua il monitor sulla lista dei pous e sulla risorsa
        'Ciclo di simulazione
        While Not (m_Stopped)
            Try
                'Seleziona tra l'algoritmo Preemptive e NotPreemptive
                If m_PreemptiveScheduling Then
                    If Monitor.TryEnter(m_resource.pouList, 100) Then
                        If Monitor.TryEnter(m_resource, 100) Then
                            ExecutePreemptiveSimulationScanCycle()
                        End If
                        Monitor.Exit(m_resource)
                    End If
                    Monitor.Exit(m_resource.pouList)
                Else
                    If Monitor.TryEnter(m_resource.pouList, 100) Then
                        If Monitor.TryEnter(m_resource, 100) Then
                            'Controlla se sono stadi distrutti i thread dell'algoritmo preemptive
                            If Not m_TaskThreadsKilled Then
                                KillsTaskThreads()
                            End If
                            ExecuteNotPreemptiveSimulationScanCycle()
                            Monitor.Exit(m_resource)
                        End If
                        Monitor.Exit(m_resource.pouList)
                    End If
                End If
                m_Cycles = m_Cycles + 1
                If m_Wait > 0 Then
                    ' Implementato in Rotor (Shared Source CLI) versione 2.0 come
                    ' REP NOP
                    ' mettendo ECX = m_Wait (non dovrebbe essere molto diverso su .net)
                    ' In pratica i valori di m_Wait che rendono "utile" questo SpinWait() dipendono
                    ' dalla macchina e dal workload (non sembra esserci un modo di calibrare
                    ' cioè di trovare min({X : durata di SpinWait(X) >= N ms}) e anche ammesso che esista
                    ' (lo si chiami SW(N)) i dati sperimentali dicono che SW(2*N) >> 2*SW(N)
                    ' ovvero, detto X = SW(1), un'attesa di 2 millisecondi non è data da SpinWait(2*X)
                    ' I risultati dell'escursione di questo valore sono (progetto con 3 POU SFC):
                    ' --- L'analisi che segue è stata condotta su un Prescott (HT) 3GHz 800Mhz
                    ' 1 GB di RAM 400Mhz, con sistema operativo Windows Vista ---
                    ' Modalità non preemptive:
                    ' m_Wait = 100000                   cicli: 250/sec  uso CPU: 55%
                    ' m_Wait = 0                        cicli: 400/sec  uso CPU: 55%
                    ' Modalità preemptive:
                    ' m_Wait = 10000                    cicli: 400/sec  uso CPU: 90%
                    ' m_Wait = 0                        cicli: 3500/sec uso CPU: 100%
                    Thread.SpinWait(m_Wait)
                End If
            Catch ex As System.Exception
                Monitor.Exit(m_resource)
                Monitor.Exit(m_resource.pouList)
            End Try
        End While
        m_Stopped = False
    End Sub
    Private Sub ExecuteNotPreemptiveSimulationScanCycle()

        'Controlla direttamente i pou
        Dim TaskToExecute As Task
        'Seleziona il task da eseguire
        TaskToExecute = ScheduleTask()
        'Esegue il task selezionato
        If Not IsNothing(TaskToExecute) Then
            TaskToExecute.ExecutePous()
        Else
            'Se non lo trova esegue i pou senza task
            For Each P As pouInstance In m_resource.pouInstances
                P.pou.Execute()
            Next P
        End If
    End Sub
    Private Sub ExecutePreemptiveSimulationScanCycle()
        'Controlla direttamente i pou
        Dim TaskToExecute As Task
        'Seleziona il task da eseguire
        TaskToExecute = ScheduleTask()
        'Se ottiene un task
        If Not IsNothing(TaskToExecute) Then
            'Esegue il task se è a priorità maggiore di m_TaskExecuted 
            If TaskToExecute.priority > m_TaskExecutedPrioriry Or Not m_TaskExecuted.ExecutionThread.IsAlive Then
                If m_TaskExecuted.ExecutionThread.IsAlive Then
                    m_TaskExecuted.Suspend()
                End If
                'Esegue il task selezionato
                TaskToExecute.Run()
                'Memorizza il task in esecuzione
                m_TaskExecuted = TaskToExecute
                m_TaskExecutedPrioriry = m_TaskExecuted.priority
            End If
        Else
            'Se non riceve nessun task e m_TaskExecuted è terminato esegue i pou senza task
            TaskToExecute = m_ResoursePouInstanceTask
            'Esegue il task 
            TaskToExecute.Run()
        End If
    End Sub
    Private Sub EsecutePou()
        For Each P As pouInstance In m_resource.pouInstances
            P.pou.Execute()
        Next P
    End Sub
    Private Sub ExecuteSimulationInit()
        'Inizializza direttamente i pou
        Try
            'Monitor sulla lista dei POU (è condivisa con types) e sulla risorsa
            If Monitor.TryEnter(m_resource.pouList, 2000) Then
                If Monitor.TryEnter(m_resource, 2000) Then
                    'Inizializza i pou dei task
                    For Each T As Task In m_resource.tasks
                        For Each P As pouInstance In T.pouInstances
                            P.pou.ExecuteInit()
                        Next P
                    Next T
                    'Inizializza i pou senza task
                    For Each P As pouInstance In m_ResoursePouInstanceTask.pouInstances
                        P.pou.ExecuteInit()
                    Next P
                    Monitor.Exit(m_resource)
                End If
                Monitor.Exit(m_resource.pouList)
            End If

        Catch ex As System.Exception
            Monitor.Exit(m_resource)
            Monitor.Exit(m_resource.pouList)
        End Try
    End Sub
    Public Sub ResetSimulation()
        Try
            'Monitor sulla lista dei POU (è condivisa con types) e sulla risorsa
            If Monitor.TryEnter(m_resource.pouList, 2000) Then
                If Monitor.TryEnter(m_resource, 2000) Then
                    'Resetta le variabili globali della risorsa
                    m_resource.globalVars.Reset()
                    'Resetta direttamente i pou
                    For Each T As Task In m_resource.tasks
                        For Each P As pouInstance In T.pouInstances
                            P.pou.Reset()
                        Next P
                    Next T
                    For Each P As pouInstance In m_ResoursePouInstanceTask.pouInstances
                        P.pou.Reset()
                    Next P
                    m_Init = False
                    Monitor.Exit(m_resource)
                End If
                Monitor.Exit(m_resource.pouList)
            End If

        Catch ex As System.Exception
            Monitor.Exit(m_resource)
            Monitor.Exit(m_resource.pouList)
        End Try
    End Sub
    Private Sub EventVarLocked(ByRef T As Thread) Handles m_ResourceTab.VarLocked
        Try
            If T.IsAlive Then
                T.Resume()
                T.Join()
            End If
        Catch ex As System.Exception
        End Try
    End Sub
    Private Sub CyclesCount(ByVal Obj As Object)
        If State = SimulationState.SimRUN Then _
            SimulationDuration = SimulationDuration + 1
        RaiseEvent NewCycles(m_Cycles)
        Thread.VolatileWrite(m_Cycles, 0)
    End Sub
    Public Event NewCycles(ByVal Value As Integer)

    Public Property SimulationDuration() As Integer
        Get
            Return Thread.VolatileRead(m_SimDuration)
        End Get
        Private Set(ByVal value As Integer)
            Thread.VolatileWrite(m_SimDuration, value)
        End Set
    End Property

    Public Sub ExecuteInit() Implements IExecutable.ExecuteInit
        Me.ExecuteSimulationInit()
    End Sub

    Public Function ExecuteScanCycle() As Boolean Implements IExecutable.ExecuteScanCycle
        Me.ExecuteSimulationNextStep()
        Return True
    End Function

    Public Sub Reset() Implements IExecutable.Reset
        Me.ResetSimulation()
    End Sub
End Class
