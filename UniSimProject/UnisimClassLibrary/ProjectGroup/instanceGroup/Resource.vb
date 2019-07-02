Imports System.Xml
Imports System
Imports System.Collections.Generic
Imports System.Threading
Public Class Resource
    Implements IXMLSerializable, IHasDocumentation

    Protected m_name As String
    Protected m_tasks As List(Of Task)
    Protected m_globalVars As VariablesLists
    Protected m_pouInstances As List(Of pouInstance)
    Protected m_documentation As String
    Private m_SimulatorEngine As SimulatorEngine
    Private m_ControlEngine As ControlEngine
    Private m_PouList As Pous  'Questa lista occorre per le funzioni di simulazione per il monitor sulla lista dei pous
    Property tasks() As List(Of Task)
        Get
            tasks = m_tasks
        End Get
        Set(ByVal Value As List(Of Task))
            m_tasks = Value
        End Set
    End Property
    Property globalVars() As VariablesLists
        Get
            globalVars = m_globalVars
        End Get
        Set(ByVal Value As VariablesLists)
            m_globalVars = Value
        End Set
    End Property
    Property pouInstances() As List(Of pouInstance)
        Get
            pouInstances = m_pouInstances
        End Get
        Set(ByVal Value As List(Of pouInstance))
            m_pouInstances = Value
        End Set
    End Property
    Property pouList() As Pous
        Get
            pouList = m_PouList
        End Get
        Set(ByVal Value As Pous)
            m_PouList = Value
        End Set
    End Property
    Property name() As String Implements IHasDocumentation.Name
        Get
            name = m_name
        End Get
        Set(ByVal Value As String)
            m_name = Value
        End Set
    End Property
    Property documentation() As String Implements IHasDocumentation.Documentation
        Get
            documentation = m_documentation
        End Get
        Set(ByVal Value As String)
            m_documentation = Value
        End Set
    End Property
    ReadOnly Property ControlEngine() As ControlEngine
        Get
            ControlEngine = m_ControlEngine
        End Get
    End Property
    ReadOnly Property SimulatorEngine() As SimulatorEngine
        Get
            SimulatorEngine = m_SimulatorEngine
        End Get
    End Property
    Public Sub New(ByVal name As String, ByVal documentation As String)
        m_name = name
        m_documentation = documentation
        m_tasks = New List(Of Task)
        m_globalVars = New VariablesLists
        m_pouInstances = New List(Of pouInstance)
        m_SimulatorEngine = New SimulatorEngine(Me)
        m_ControlEngine = New ControlEngine(Me)

    End Sub
    Public Sub New()
        m_tasks = New List(Of Task)
        m_globalVars = New VariablesLists
        m_pouInstances = New List(Of pouInstance)
        m_SimulatorEngine = New SimulatorEngine(Me)
        m_ControlEngine = New ControlEngine(Me)
    End Sub
    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter) Implements IXMLSerializable.xmlExport
        'resource
        RefXMLProjectWriter.WriteStartElement("resource")
        'Attributi
        RefXMLProjectWriter.WriteAttributeString("name", m_name)

        'Esporta tasks
        For Each Ts As Task In tasks
            Ts.xmlExport(RefXMLProjectWriter)
        Next Ts

        'Esporta globalVars
        For Each VarList As VariablesList In globalVars
            'globalVars
            RefXMLProjectWriter.WriteStartElement("globalVars")
            VarList.xmlExport(RefXMLProjectWriter)
            RefXMLProjectWriter.WriteEndElement() 'globalVars
        Next VarList

        'Esporta pouInstances
        For Each ProgInst As pouInstance In pouInstances
            ProgInst.xmlExport(RefXMLProjectWriter)
        Next ProgInst
        'Esporta documentation
        If m_documentation <> "" Then
            RefXMLProjectWriter.WriteElementString("documentation", m_documentation)  'documentation
        End If

        RefXMLProjectWriter.WriteEndElement() 'resource
    End Sub
    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader) Implements IXMLSerializable.xmlImport

        'Memorizza la profondità del nodo
        Dim NodeDepth As Integer = RefXmlProjectReader.Depth

        'Legge gli attributi
        If RefXmlProjectReader.MoveToAttribute("name") Then
            m_name = RefXmlProjectReader.Value
        End If
        'Se l'elemento non è vuoto si sposta sul nodo successivo
        If Not RefXmlProjectReader.IsEmptyElement Then
            RefXmlProjectReader.Read()
            'Scorre fino alla fine di Configuration
            While RefXmlProjectReader.Depth > NodeDepth
                Select Case RefXmlProjectReader.Name
                    Case "task"
                        'Controlla se è l'inizio dell'elemento
                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                            Dim NewTask As New Task
                            NewTask.xmlImport(RefXmlProjectReader)
                            m_tasks.Add(NewTask)
                        End If
                    Case "globalVars"
                        'Controlla se è l'inizio dell'elemento
                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                            Dim NewVarList As New VariablesList
                            NewVarList.xmlImport(RefXmlProjectReader)
                            m_globalVars.Add(NewVarList)
                        End If
                    Case "pouInstance"
                        'Controlla se è l'inizio dell'elemento
                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                            Dim NewProgramInstance As New pouInstance
                            NewProgramInstance.xmlImport(RefXmlProjectReader)
                            m_pouInstances.Add(NewProgramInstance)
                        End If
                    Case "documentation"
                        'Controlla se è l'inizio dell'elemento
                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                            m_documentation = RefXmlProjectReader.Value
                        End If
                End Select
                'Si sposta sul nodo successivo se non è la fine dell'elemento
                If RefXmlProjectReader.Depth > NodeDepth Then
                    RefXmlProjectReader.Read()
                End If

            End While
        End If
    End Sub
    Public Sub ResolvePouInstanceLinks()
        'Risolve i riferimenti dei nomi dei pou nei pouInstance
        For Each T As Task In m_tasks
            T.ResolveLinks(m_globalVars, m_PouList)
        Next T

        For Each PI As pouInstance In m_pouInstances
            PI.ResolvePouInstanceLinks(m_PouList)
        Next PI
    End Sub
    Public Function FindTaskByName(ByVal Name As String) As Task
        'Cerca tra i tasks
        For Each T As Task In m_tasks
            If T.name = Name Then _
                Return T
        Next T
        Return Nothing
    End Function
    Public Function FindPouInstanceByName(ByVal Name As String) As pouInstance
        'Cerca tra i pouInstance dei task della risorsa
        For Each T As Task In m_tasks
            For Each PI As pouInstance In T.pouInstances
                If PI.name = Name Then Return PI
            Next PI
        Next T
        'Cerca tra i pouInstance della risorsa
        For Each PI As pouInstance In m_pouInstances
            If PI.name = Name Then Return PI
        Next PI
        Return Nothing
    End Function
    Public Function Built() As Boolean
        'Genera le instanze dei pou
        Built = True
        Try
            'Pou con task
            For Each T As Task In m_tasks
                For Each PI As pouInstance In T.pouInstances
                    PI.CreateInstance()
                Next PI
            Next T
            'Pou senza task
            For Each PI As pouInstance In m_pouInstances
                PI.CreateInstance()
            Next PI
        Catch ex As System.Exception
            Built = False
        End Try
    End Function
    Public Sub AddTask(ByRef NewTask As Task)
        Try
            If Monitor.TryEnter(m_tasks, 2000) Then
                m_tasks.Add(NewTask)
                Monitor.Exit(m_tasks)
            End If
        Catch ex As System.Exception
            Monitor.Exit(m_tasks)
        End Try
    End Sub
    Public Sub RemoveTask(ByRef RefTask As Task)
        Try
            If Monitor.TryEnter(m_tasks, 2000) Then
                m_tasks.Remove(RefTask)
                Monitor.Exit(m_tasks)
            End If
        Catch ex As System.Exception
            Monitor.Exit(m_tasks)
        End Try
    End Sub
End Class


