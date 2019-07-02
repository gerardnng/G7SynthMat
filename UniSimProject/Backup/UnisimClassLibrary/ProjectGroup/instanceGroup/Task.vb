Imports System
Imports System.Collections.Generic
Imports System.Threading
Imports System.Xml
Public Class Task
    Implements IXMLSerializable, IHasName

    Protected m_name As String
    Protected m_Single As String
    Protected m_priority As Integer
    Protected m_pouInstances As List(Of pouInstance)
    Protected m_ResGlobalVariables As VariablesLists
    Protected m_ExecutionThread As Thread
    Protected m_Ready As Boolean
    Public m_interval As TimeSpan
    Public m_LastActivation As DateTime

    Public Shared ReadOnly MAX_PRIORITY As Integer = 0
    Public Shared ReadOnly MIN_PRIORITY As Integer = 65535

    Public Property name() As String Implements IHasName.Name
        Get
            name = m_name
        End Get
        Set(ByVal Value As String)
            m_name = Value
        End Set
    End Property
    Public Property mSingle() As String
        Get
            mSingle = m_Single
        End Get
        Set(ByVal Value As String)
            m_Single = Value
        End Set
    End Property
    Public Property priority() As Integer
        Get
            priority = m_priority
        End Get
        Set(ByVal Value As Integer)
            m_priority = Value
        End Set
    End Property
    Public Property pouInstances() As List(Of pouInstance)
        Get
            pouInstances = m_pouInstances
        End Get
        Set(ByVal Value As List(Of pouInstance))
            m_pouInstances = Value
        End Set
    End Property
    Public ReadOnly Property Ready() As Boolean
        Get
            Ready = m_Ready
        End Get
    End Property
    Public ReadOnly Property ExecutionThread() As Thread
        Get
            ExecutionThread = m_ExecutionThread
        End Get
    End Property
    Public Sub New(ByVal Name As String, ByVal Priority As Integer, ByVal Interval As TimeSpan, ByRef PouInstances As List(Of pouInstance))
        m_name = Name
        m_interval = Interval
        m_priority = Priority
        m_pouInstances = PouInstances
        m_Ready = True
        m_ExecutionThread = New Thread(AddressOf ExecutePous)
    End Sub
    Public Sub New()
        pouInstances = New List(Of pouInstance)
        m_ExecutionThread = New Thread(AddressOf ExecutePous)
        m_Ready = True
    End Sub
    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter) Implements IXMLSerializable.xmlExport
        'task
        RefXMLProjectWriter.WriteStartElement("task")

        'attributi di task
        RefXMLProjectWriter.WriteAttributeString("name", m_name)
        RefXMLProjectWriter.WriteAttributeString("priority", m_priority.ToString)
        If Not IsNothing(m_interval) Then
            RefXMLProjectWriter.WriteAttributeString("interval", m_interval.ToString())
        End If
        If Not IsNothing(m_Single) Then
            RefXMLProjectWriter.WriteAttributeString("version", m_Single)
        End If

        'Esporta pouInstances
        For Each P As pouInstance In pouInstances
            P.xmlExport(RefXMLProjectWriter)
        Next P

        RefXMLProjectWriter.WriteEndElement() 'task
    End Sub
    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader) Implements IXMLSerializable.xmlImport
        'Memorizza la profondità del nodo
        Dim NodeDepth As Integer = RefXmlProjectReader.Depth

        'Legge gli attributi
        If RefXmlProjectReader.MoveToAttribute("name") Then
            m_name = RefXmlProjectReader.Value
        End If
        If RefXmlProjectReader.MoveToAttribute("single") Then
            m_Single = RefXmlProjectReader.Value
        End If
        If RefXmlProjectReader.MoveToAttribute("priority") Then
            m_priority = CInt(RefXmlProjectReader.Value)
        End If
        If RefXmlProjectReader.MoveToAttribute("interval") Then
            m_interval = TimeSpan.Parse(RefXmlProjectReader.Value)
        End If
        'Se l'elemento non è vuoto si sposta sul nodo successivo
        If Not RefXmlProjectReader.IsEmptyElement Then
            RefXmlProjectReader.Read()
            'Scorre fino alla fine di Configuration
            While RefXmlProjectReader.Depth > NodeDepth
                Select Case RefXmlProjectReader.Name
                    Case "pouInstance"
                        'Controlla se è l'inizio dell'elemento
                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                            Dim NewProgramInstance As New pouInstance
                            NewProgramInstance.xmlImport(RefXmlProjectReader)
                            m_pouInstances.Add(NewProgramInstance)
                        End If
                End Select
                'Si sposta sul nodo successivo se non è la fine dell'elemento
                If RefXmlProjectReader.Depth > NodeDepth Then
                    RefXmlProjectReader.Read()
                End If
            End While
        End If
    End Sub
    Public Sub ResolveLinks(ByRef RefglobalVars As VariablesLists, ByRef RefPouList As Pous)
        'Risolve i riferimenti dei nomi dei pou nei pouInstance
        For Each PI As pouInstance In m_pouInstances
            PI.ResolvePouInstanceLinks(RefPouList)
        Next PI
    End Sub
    Public Sub Run()
        Try
            If m_ExecutionThread.IsAlive Then
                m_ExecutionThread.Resume()
            Else
                m_ExecutionThread = New Thread(AddressOf ExecutePous)
                m_ExecutionThread.Start()
            End If
        Catch ex As System.Exception
        End Try
    End Sub
    Public Sub ResumeThread()
        Try
            If m_ExecutionThread.IsAlive Then
                m_ExecutionThread.Resume()
            End If
        Catch ex As System.Exception
        End Try
    End Sub
    Public Sub Suspend()
        Try
            If Not IsNothing(m_ExecutionThread) Then
                m_ExecutionThread.Suspend()
            End If
        Catch ex As System.Exception
        End Try
    End Sub
    Public Sub Join()
        Try
            If Not IsNothing(m_ExecutionThread) Then
                m_ExecutionThread.Join()
            End If
        Catch ex As System.Exception
        End Try
    End Sub
    Public Sub ExecutePous()
        'Memorizza l'istante di ultima attivazione
        m_LastActivation = Now
        m_Ready = False
        'Esegue le POU
        For Each P As pouInstance In m_pouInstances
            P.ExecutePou()
        Next P
        m_Ready = True
    End Sub
    Public Sub ExecutePousInstance()
        'Memorizza l'istante di ultima attivazione
        m_LastActivation = Now
        m_Ready = False
        'Esegue le PouInstances
        For Each P As pouInstance In m_pouInstances
            P.ExecutePouInstance()
        Next P
        m_Ready = True
    End Sub
    Public Sub ExecutePousInit()
        For Each P As pouInstance In m_pouInstances
            P.ExecutePouInit()
        Next P
    End Sub
    Public Sub ExecutePouInstanceInit()
        For Each P As pouInstance In m_pouInstances
            P.ExecutePouInstanceInit()
        Next P
    End Sub

End Class


