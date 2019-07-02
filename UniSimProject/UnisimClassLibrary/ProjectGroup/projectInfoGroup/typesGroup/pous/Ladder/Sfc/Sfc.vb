Imports System.IO
Imports System.Threading
Imports System.Xml
Imports UnisimClassLibrary

Public Class Sfc
    Implements IIEC61131LanguageImplementation

    Private m_GraphicalStepsList As GraphicalStepsList
    Private m_GraphicalTransitionsList As GraphicalTransitionsList
    'Le seguenti liste servono solo ai fini dell'esportazione xml
    Private m_XmlSimultaneousConvergences As XmlSimultaneousConvergences
    Private m_XmlSimultaneousDivergences As XmlSimultaneousDivergences
    Private m_XmlSelectionConvergences As XmlSelectionConvergences
    Private m_XmlSelectionDivergences As XmlSelectionDivergences

    Private m_Name As String
    Private m_ConditionChanged As Boolean
    Private m_ActivatedPreactivedSteps As Boolean

    Private m_pouInterface As pouInterface
    Private m_ResGlobalVariables As VariablesLists

    Private m_pouslist As Pous

    '----------------------------
    'Blocco aggiuntivo alla norma
    Private Suspended As Boolean
    Private Stopped As Boolean
    Private Forced As Boolean
    '----------------------------



    Private m_BackColor As Drawing.Color = Drawing.Color.White
    Private m_SelectionColor As Drawing.Color = Drawing.Color.Blue
    Private m_NotSelectionColor As Drawing.Color = Drawing.Color.Black
    Private m_TextColor As Drawing.Color = Drawing.Color.Black
    Private m_Char As Drawing.Font = New Drawing.Font("Arial", 8)
    Private m_ColorActive As Drawing.Color = Drawing.Color.Red
    Private m_ColorDeactive As Drawing.Color = m_BackColor
    Private m_ColorPreactive As Drawing.Color = Drawing.Color.Pink
    Private m_CSnap As Integer = 4
    Private m_ColTransitionConditionTrue As Drawing.Color = Drawing.Color.Green
    Private m_ColTransitionConditionFalse As Drawing.Color = Drawing.Color.Brown
    Private m_FlagShowDetails As Boolean = True
    Private m_StateMonitor As Boolean
    Private m_GraphToDraw As Drawing.Graphics
    Private m_Dimension As Integer = 44
    Public Event StartScan() Implements IIEC61131LanguageImplementation.StartScan
    Public Event EndScan() Implements IIEC61131LanguageImplementation.EndScan
    Property Name() As String Implements IIEC61131LanguageImplementation.Name
        Get
            Name = m_Name
        End Get
        Set(ByVal Value As String)
            m_Name = Value
        End Set
    End Property
    Property ResGlobalVariables() As VariablesLists Implements IIEC61131LanguageImplementation.ResGlobalVariables
        Get
            ResGlobalVariables = m_ResGlobalVariables
        End Get
        Set(ByVal Value As VariablesLists)
            m_ResGlobalVariables = Value
            'Comunica alle Macrofasi le lista di variabili globali della risorsa
            m_GraphicalStepsList.ResGlobalVariables = m_ResGlobalVariables
        End Set
    End Property
    Property PouInterface() As pouInterface Implements IIEC61131LanguageImplementation.PouInterface
        Get
            PouInterface = m_pouInterface
        End Get
        Set(ByVal Value As pouInterface)
            m_pouInterface = Value
            'Comunica alle Macrofasi l'interfaccia della pou
            m_GraphicalStepsList.PouInterface = m_pouInterface
        End Set
    End Property
    Property GraphicalStepsList() As GraphicalStepsList
        Get
            GraphicalStepsList = m_GraphicalStepsList
        End Get
        Set(ByVal Value As GraphicalStepsList)
            m_GraphicalStepsList = Value
        End Set
    End Property
    Property GraphicalTransitionsList() As GraphicalTransitionsList
        Get
            GraphicalTransitionsList = m_GraphicalTransitionsList
        End Get
        Set(ByVal Value As GraphicalTransitionsList)
            m_GraphicalTransitionsList = Value
        End Set
    End Property
    Property XmlSimultaneousConvergences() As XmlSimultaneousConvergences
        Get
            XmlSimultaneousConvergences = m_XmlSimultaneousConvergences
        End Get
        Set(ByVal Value As XmlSimultaneousConvergences)
            m_XmlSimultaneousConvergences = Value
        End Set
    End Property
    Property XmlSimultaneousDivergences() As XmlSimultaneousDivergences
        Get
            XmlSimultaneousDivergences = m_XmlSimultaneousDivergences
        End Get
        Set(ByVal Value As XmlSimultaneousDivergences)
            m_XmlSimultaneousDivergences = Value
        End Set
    End Property
    Property XmlSelectionConvergences() As XmlSelectionConvergences
        Get
            XmlSelectionConvergences = m_XmlSelectionConvergences
        End Get
        Set(ByVal Value As XmlSelectionConvergences)
            m_XmlSelectionConvergences = Value
        End Set
    End Property
    Property XmlSelectionDivergences() As XmlSelectionDivergences
        Get
            XmlSelectionDivergences = m_XmlSelectionDivergences
        End Get
        Set(ByVal Value As XmlSelectionDivergences)
            m_XmlSelectionDivergences = Value
        End Set
    End Property
    Public Sub New(ByVal MyBodyName As String, ByRef RefResGlobalVariables As VariablesLists, ByRef RefPouInterface As pouInterface, ByVal pouslist As Pous)
        m_Name = MyBodyName
        m_ResGlobalVariables = RefResGlobalVariables
        m_pouInterface = RefPouInterface
        m_GraphicalStepsList = New GraphicalStepsList(Me, m_BackColor, m_SelectionColor, m_NotSelectionColor, m_TextColor, m_Char, m_ColorActive, m_ColorDeactive, m_ColorPreactive, m_GraphToDraw, m_Dimension, pouslist)
        m_GraphicalTransitionsList = New GraphicalTransitionsList(Me, m_CSnap, m_BackColor, m_SelectionColor, m_NotSelectionColor, m_TextColor, m_Char, m_ColTransitionConditionTrue, m_ColTransitionConditionFalse, m_ColorDeactive, m_ColorDeactive, m_ColorPreactive, m_GraphToDraw, m_Dimension)
        m_pouslist = pouslist
    End Sub
    Public Sub New(ByVal MyBodyName As String, ByRef RefResGlobalVariables As VariablesLists, ByRef RefPouInterface As pouInterface, ByRef GrapStepsList As GraphicalStepsList, ByRef GrapTransitionsList As GraphicalTransitionsList)
        m_Name = MyBodyName
        m_ResGlobalVariables = RefResGlobalVariables
        m_pouInterface = RefPouInterface
        m_GraphicalStepsList = GrapStepsList
        m_GraphicalTransitionsList = GrapTransitionsList
    End Sub
    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter) Implements IIEC61131LanguageImplementation.xmlExport

        'Riempie le liste di convergenze e divergenze e delle connessioni
        'Svuota le connessioni
        m_GraphicalStepsList.ClearXMLConnectionsLists()
        m_GraphicalTransitionsList.ClearXMLConnectionsLists()

        m_XmlSimultaneousConvergences = New XmlSimultaneousConvergences
        m_XmlSimultaneousDivergences = New XmlSimultaneousDivergences
        m_XmlSelectionConvergences = New XmlSelectionConvergences
        m_XmlSelectionDivergences = New XmlSelectionDivergences
        Dim NextNumberAvaiable As Integer = FirstAvaiableElementNumber()
        'Riempie le liste di convergenze e divergenze
        m_GraphicalTransitionsList.CreateXmlConvergencesAndDivergences(NextNumberAvaiable)

        'Riempie le liste di convergenze e divergenze selettive
        m_GraphicalStepsList.CreateXmlConvergencesAndDivergences(NextNumberAvaiable)

        'Inizio esportazione XML
        'SFC
        RefXMLProjectWriter.WriteStartElement("SFC")
        'Esporta le fasi
        m_GraphicalStepsList.xmlExport(RefXMLProjectWriter)
        'Esporta le transizioni
        m_GraphicalTransitionsList.xmlExport(RefXMLProjectWriter)
        'Esporta le convergenze e divergenza
        m_XmlSimultaneousConvergences.xmlExport(RefXMLProjectWriter)
        m_XmlSimultaneousDivergences.xmlExport(RefXMLProjectWriter)
        m_XmlSelectionConvergences.xmlExport(RefXMLProjectWriter)
        m_XmlSelectionDivergences.xmlExport(RefXMLProjectWriter)

        RefXMLProjectWriter.WriteEndElement() 'SFC
        'Fine esportazione XML

        m_XmlSimultaneousConvergences = Nothing
        m_XmlSimultaneousDivergences = Nothing
        m_XmlSelectionConvergences = Nothing
        m_XmlSelectionDivergences = Nothing
    End Sub
    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader) Implements IIEC61131LanguageImplementation.xmlImport

        'Crea le liste per le convergenz e divergenze
        m_XmlSimultaneousConvergences = New XmlSimultaneousConvergences
        m_XmlSimultaneousDivergences = New XmlSimultaneousDivergences
        m_XmlSelectionConvergences = New XmlSelectionConvergences
        m_XmlSelectionDivergences = New XmlSelectionDivergences

        'Memorizza la profondità del nodo
        Dim NodeDepth As Integer = RefXmlProjectReader.Depth
        'Si sposta sul nodo successivo
        RefXmlProjectReader.Read()

        'Scorre fino alla fine dell'SFC
        While RefXmlProjectReader.Depth > NodeDepth
            Select Case RefXmlProjectReader.Name
                Case "step", "macroStep"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                        'Aggiunge la fase alla lista
                        m_GraphicalStepsList.xmlImport(RefXmlProjectReader)
                    End If
                Case "transition"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                        'Aggiunge la fase alla lista
                        m_GraphicalTransitionsList.xmlImport(RefXmlProjectReader)
                    End If
                Case "actionBlock"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                        'Aggiunge l'actionBlock
                        m_GraphicalStepsList.xmlImport(RefXmlProjectReader)
                    End If
                Case "selectionDivergence"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                        'La aggiunge alla lista
                        m_XmlSelectionDivergences.xmlImport(RefXmlProjectReader)
                    End If
                Case "selectionConvergence"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                        'La aggiunge alla lista
                        m_XmlSelectionConvergences.xmlImport(RefXmlProjectReader)
                    End If
                Case "simultaneousDivergence"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                        'La aggiunge alla lista
                        m_XmlSimultaneousDivergences.xmlImport(RefXmlProjectReader)
                    End If
                Case "simultaneousConvergence"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                        'La aggiunge alla lista
                        m_XmlSimultaneousConvergences.xmlImport(RefXmlProjectReader)
                    End If
            End Select
            'Si sposta sul nodo successivo
            RefXmlProjectReader.Read()

        End While

        'Risolve i collegamenti delle transizioni eliminando convergenze e divergenza
        'Risolve i collegamenti precedenti nelle transizioni
        m_GraphicalTransitionsList.ResolveXmlConvergencesAndDivergences()
        'Risolve i collegamenti precedenti nelle fasi
        m_GraphicalStepsList.ResolveXmlConvergencesAndDivergences()

        'Svuota le lista delle convergenze e divergenza
        m_XmlSimultaneousConvergences.Clear()
        m_XmlSimultaneousDivergences.Clear()
        m_XmlSelectionConvergences.Clear()
        m_XmlSelectionDivergences.Clear()
        m_XmlSimultaneousConvergences = Nothing
        m_XmlSimultaneousDivergences = Nothing
        m_XmlSelectionConvergences = Nothing
        m_XmlSelectionDivergences = Nothing
    End Sub
    Public Sub ResolveVariablesLinks() Implements IIEC61131LanguageImplementation.ResolveVariableLinks
        'Risolve i riferimenti dei nomi di variabili nella azioni e nelle condizioni
        m_GraphicalStepsList.ResolveVariablesLinks()
        m_GraphicalTransitionsList.ResolveVariablesLinks()
    End Sub
    Public Function CreateInstance() As IIEC61131LanguageImplementation Implements IIEC61131LanguageImplementation.CreateInstance
        Dim NewSfc As New Sfc(m_Name, m_ResGlobalVariables, m_pouInterface, m_pouslist)
        Dim NewGraphicalStepsList As GraphicalStepsList = m_GraphicalStepsList.CreateInstance(NewSfc)
        Dim NewGraphicalTransitionsList As GraphicalTransitionsList = m_GraphicalTransitionsList.CreateInstance(NewSfc, NewGraphicalStepsList)
        NewSfc.GraphicalStepsList = NewGraphicalStepsList
        NewSfc.GraphicalTransitionsList = NewGraphicalTransitionsList
        CreateInstance = NewSfc
    End Function
    Public Sub DisposeMe()     'Distruttore
        m_GraphicalStepsList.DisposeMe()
        m_GraphicalTransitionsList.DisposeMe()
        Me.Finalize()
    End Sub
    Public Sub SetGraphToDraw(ByRef Graph As Drawing.Graphics) Implements IIEC61131LanguageImplementation.SetGraphToDraw
        m_GraphToDraw = Graph
        m_GraphicalStepsList.SetGraphToDraw(m_GraphToDraw)
        m_GraphicalTransitionsList.SetGraphToDraw(m_GraphToDraw)
    End Sub
    Public Function RemoveSelectedElements() As Boolean
        'Tenta di entrare nell'sfc
        Try
            If Monitor.TryEnter(Me, 2000) Then
                m_GraphicalStepsList.RemoveSelectedElements()
                m_GraphicalTransitionsList.RemoveSelectedElements()
                Monitor.Exit(Me)
                RemoveSelectedElements = True
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
            RemoveSelectedElements = False
        End Try
    End Function
    Public Sub DeSelectAll()
        m_GraphicalStepsList.DeSelectAll()
        m_GraphicalTransitionsList.DeSelectAll()
    End Sub
    Public Function FindElementByNumber(ByVal n As Integer) As Object
        'Cerca tra le fasi
        FindElementByNumber = m_GraphicalStepsList.FindStepByNumber(n)
        If IsNothing(FindElementByNumber) Then
            'Cerca tra le transizioni
            FindElementByNumber = m_GraphicalTransitionsList.FindTransitionByNumber(n)
        End If
    End Function
    Public Function FindStepByNumber(ByVal n As Integer) As BaseGraphicalStep
        FindStepByNumber = m_GraphicalStepsList.FindStepByNumber(n)
    End Function
    Public Function FindStepByName(ByVal Value As String) As BaseGraphicalStep
        For Each S As BaseGraphicalStep In m_GraphicalStepsList
            If S.Name = Value Then
                Return S
                Exit For
            End If
        Next S
        Return Nothing
    End Function
    Public Function FirstAvaiableStepName() As String
        FirstAvaiableStepName = ""
        Dim i As Integer
        Dim Find As Boolean
        For i = 0 To m_GraphicalStepsList.Count
            Find = False
            For Each S As BaseGraphicalStep In m_GraphicalStepsList
                If S.Name = "S" & i Then
                    Find = True
                    Exit For
                End If
            Next S
            If Not Find Then
                FirstAvaiableStepName = "S" & i
                Exit For
            End If
        Next i
    End Function
    Public Function FindTransitionByName(ByVal Value As String) As GraphicalTransition
        For Each T As GraphicalTransition In m_GraphicalTransitionsList
            If T.Name = Value Then
                Return T
            End If
        Next T
        Return Nothing
    End Function
    Public Function FirstAvaiableTransitionName() As String
        ' Potrebbe non essere davvero il primo nome disponibile
        ' (ma troverà comunque un nome valido)
        ' L'algoritmo precedente falliva se c'erano più transizioni che fasi e ritornava
        ' nomi inizianti con "S" invece che con "T"
        Dim num As Integer = Me.GraphicalTransitionsList.Count + 1
LoopIn:
        For Each T As GraphicalTransition In Me.GraphicalTransitionsList
            If Me.Name.Equals("T" + num.ToString()) Then
                num += 1
                GoTo LoopIn
            End If
        Next
        Return "T" + num.ToString()
    End Function
    Public Function FirstAvaiableElementNumber() As Integer
        Dim j As Integer = 0
        Dim NumberAvaiable As Boolean
        While j < m_GraphicalStepsList.Count + m_GraphicalTransitionsList.Count + 1
            j = j + 1
            NumberAvaiable = True
            If IsNothing(m_GraphicalStepsList.FindStepByNumber(j)) And IsNothing(m_GraphicalTransitionsList.FindTransitionByNumber(j)) Then
                Exit While
            End If
        End While
        FirstAvaiableElementNumber = j
    End Function
    Public Function ControllaPresenzaTransition(ByVal n As Integer) As Boolean
        ControllaPresenzaTransition = m_GraphicalTransitionsList.ControllaPresenzaTransition(n)
    End Function
    Public Function AddAndDrawStep(ByVal Number As Integer, ByVal Name As String, ByVal Documentation As String, ByVal Position As Drawing.Point) As Boolean
        'Tenta di entrare nell'sfc e nel GraphToDraw
        Try
            If Monitor.TryEnter(Me, 2000) Then
                m_GraphicalStepsList.AddAndDrawStep(Number, Name, Documentation, Position, m_StateMonitor)
                AddAndDrawStep = True
                Monitor.Exit(Me)
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
            AddAndDrawStep = False
        End Try
    End Function
    Public Function AddAndDrawMacroStep(ByVal Number As Integer, ByVal Name As String, ByVal Documentation As String, ByVal Position As Drawing.Point) As Boolean
        'Tenta di entrare nell'sfc e nel GraphToDraw
        Try
            If Monitor.TryEnter(Me, 2000) Then
                m_GraphicalStepsList.AddAndDrawMacroStep(Number, Name, Documentation, m_ResGlobalVariables, Position, m_StateMonitor)
                AddAndDrawMacroStep = True
                Monitor.Exit(Me)
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
            AddAndDrawMacroStep = False
        End Try
    End Function
    Public Function AddAndDrawTransition(ByVal Number As Integer, ByVal Name As String, ByVal Documentation As String, ByRef RefCondition As BooleanExpression) As Boolean
        'Tenta di entrare nell'sfc e nel GraphToDraw
        Try
            If Monitor.TryEnter(Me, 2000) Then
                'Crea la transizione inviando la lista delle fasi selezionate superiormente....
                '....e la lista delle fasi selezionate inferiormente leggendole dalla lista delle fasi
                m_GraphicalTransitionsList.AddAndDrawTransition(Number, Name, Documentation, m_GraphicalStepsList.ReadBottomSelectedStepList, m_GraphicalStepsList.ReadTopSelectedStepList, RefCondition, m_FlagShowDetails, m_StateMonitor)
                Monitor.Exit(Me)
                AddAndDrawTransition = True
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
            AddAndDrawTransition = False
        End Try
    End Function
    Public Function AddActionToSelectedSteps(ByVal Nome As String, ByVal Qual As String, ByVal Var As BaseVariable, ByVal VarInd As BaseVariable, ByVal Time As TimeSpan, ByRef RefSfc As Sfc, ByVal StepsToForces As GraphicalStepsList, ByVal ArithExp As String) As Boolean
        'Tenta di entrare nell'sfc e nel GraphToDraw
        Try
            If Monitor.TryEnter(Me, 2000) Then
                m_GraphicalStepsList.AddActionToSelectedSteps(Nome, Qual, Var, VarInd, Time, RefSfc, StepsToForces, ArithExp)
                Monitor.Exit(Me)
                AddActionToSelectedSteps = True
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
            AddActionToSelectedSteps = False
        End Try
    End Function
    Public Sub ShowDetails(ByVal value As Boolean)
        m_FlagShowDetails = value
    End Sub
    Private Function FindAndSelectStep(ByVal x As Integer, ByVal y As Integer) As Boolean
        FindAndSelectStep = m_GraphicalStepsList.FindAndSelectStep(x, y)
    End Function
    Private Function FindAndSelectTransition(ByVal x As Integer, ByVal y As Integer) As Boolean
        FindAndSelectTransition = m_GraphicalTransitionsList.FindAndSelectTransition(x, y)
    End Function
    Private Function FindAndSelectAction(ByVal x As Integer, ByVal y As Integer) As Boolean
        'Cerca tra le Actions
        FindAndSelectAction = m_GraphicalStepsList.FindAndSelectAction(x, y)
    End Function
    Public Sub FindAndSelectElementsArea(ByVal Rect As Drawing.Rectangle)
        m_GraphicalStepsList.FindAndSelectSteps(Rect)
        m_GraphicalTransitionsList.FindAndSelectTransitions(Rect)
        m_GraphicalStepsList.FindAndSelectActions(Rect)
    End Sub
    Public Function FindAndSelectElement(ByVal x As Integer, ByVal y As Integer) As Boolean
        'Cerca tra le transizioni
        FindAndSelectElement = m_GraphicalTransitionsList.FindAndSelectTransition(x, y)
        If Not FindAndSelectElement Then
            'Cerca tra le fasi
            FindAndSelectElement = m_GraphicalStepsList.FindAndSelectStep(x, y)
            If Not FindAndSelectElement Then
                'Cerca tra le azioni
                FindAndSelectElement = m_GraphicalStepsList.FindAndSelectAction(x, y)
            End If
        End If
    End Function
    Public Function FindElement(ByVal x As Integer, ByVal y As Integer) As Boolean
        FindElement = m_GraphicalTransitionsList.FindTransition(x, y)
        If Not FindElement Then
            FindElement = m_GraphicalStepsList.FindStep(x, y)
            If Not FindElement Then
                FindElement = m_GraphicalStepsList.FindAction(x, y)
            End If
        End If
    End Function
    Public Function ReadIfElementIsSelected(ByVal x As Integer, ByVal y As Integer) As Boolean
        ReadIfElementIsSelected = m_GraphicalStepsList.ReadIfStepSelected(x, y)
        If Not ReadIfElementIsSelected Then
            ReadIfElementIsSelected = m_GraphicalTransitionsList.ReadIfTransitionSelected(x, y)
            If Not ReadIfElementIsSelected Then
                ReadIfElementIsSelected = m_GraphicalStepsList.ReadIfActionSelected(x, y)
            End If
        End If
    End Function
    Public Function FindAndSelectSmallRectangleStep(ByVal x As Integer, ByVal y As Integer) As Boolean
        FindAndSelectSmallRectangleStep = m_GraphicalStepsList.FindAndSelectSmallRectangleStep(x, y)
    End Function
    Public Sub DrawElementsArea(ByVal Rect As Drawing.Rectangle, ByVal DrawSmallRectangels As Boolean)
        Try
            If Monitor.TryEnter(Me, 2000) Then
                m_GraphicalStepsList.DrawArea(Rect, DrawSmallRectangels)
                m_GraphicalTransitionsList.DrawArea(Rect, m_FlagShowDetails)
                Monitor.Exit(Me)
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
        End Try
    End Sub
    Public Function ReadStepList() As GraphicalStepsList
        'Legge solo le fasi
        ReadStepList = New GraphicalStepsList
        For Each S As BaseGraphicalStep In GraphicalStepsList
            If S.GetType.Name = "GraphicalStep" Then
                ReadStepList.Add(S)
            End If
        Next S
    End Function
    Public Function ReadMacroStepList() As GraphicalStepsList
        'Legge solo le macrofasi
        ReadMacroStepList = New GraphicalStepsList
        For Each S As BaseGraphicalStep In GraphicalStepsList
            If S.GetType.Name = "GraphicalMacroStep" Then
                ReadStepList.Add(S)
            End If
        Next S
    End Function
    Public Function SetInitialSteps() As Boolean
        SetInitialSteps = m_GraphicalStepsList.SetInitialSteps()
    End Function
    Public Function SetFinalSteps() As Boolean
        SetFinalSteps = m_GraphicalStepsList.SetFinalSteps()
    End Function
    Public Function CancelSelection(ByVal CancellSmallRectangels As Boolean) As Boolean
        'Cancella su GraphToDraw gli elementi selezionati
        'Tenta di entrare nell'sfc e nel GraphToDraw
        Try
            If Monitor.TryEnter(Me, 2000) Then
                m_GraphicalStepsList.CancelSelection(CancellSmallRectangels)
                m_GraphicalTransitionsList.CancellTransitionWithSelectedSteps(m_FlagShowDetails)
                m_GraphicalTransitionsList.CancellSelection(m_FlagShowDetails)
                CancelSelection = True
                Monitor.Exit(Me)
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
            CancelSelection = False
        End Try
    End Function
    Public Sub MoveSelection(ByVal dx As Integer, ByVal dy As Integer)
        m_GraphicalStepsList.MoveSelection(dx, dy)
        m_GraphicalTransitionsList.MoveSelection(dx)
    End Sub
    Public Function ControllaSelectionFuoriArea(ByVal R As Drawing.Rectangle, ByRef FuoriX As Boolean, ByRef FuoriY As Boolean) As Boolean
        'Verifica se gli Elements Selected sono fuori dal Rectangle R
        ControllaSelectionFuoriArea = m_GraphicalStepsList.ControllaSelectionFuoriArea(R, FuoriX, FuoriY) Or m_GraphicalTransitionsList.ControllaSelectionFuoriArea(R, FuoriX, FuoriY)
    End Function

    Public Sub DrawSelection(ByVal DrawSmallRectangels As Boolean)
        'Disegna su GraphToDraw gli elementi selezionati
        'Tenta di entrare nell'sfc e nel GraphToDraw
        Try
            If Monitor.TryEnter(Me, 1000) Then
                m_GraphicalStepsList.DrawSelection(DrawSmallRectangels)
                m_GraphicalTransitionsList.DrawSelection(m_FlagShowDetails)
                Monitor.Exit(Me)
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
        End Try
    End Sub
    Public Function CountSelectedElement() As Integer
        CountSelectedElement = m_GraphicalStepsList.CountSelected
        CountSelectedElement = CountSelectedElement + m_GraphicalTransitionsList.CountSelected
        CountSelectedElement = CountSelectedElement + m_GraphicalStepsList.CountSelectedActions
    End Function
    Public Function ReadObjectSelected() As Object
        ReadObjectSelected = m_GraphicalStepsList.ReadStepSelected
        If IsNothing(ReadObjectSelected) Then
            ReadObjectSelected = m_GraphicalTransitionsList.ReadTransitionSelected
            If IsNothing(ReadObjectSelected) Then
                ReadObjectSelected = m_GraphicalStepsList.ReadSelectedAction
            End If
        End If
    End Function
    Public Sub StartStateMonitor()
        m_StateMonitor = True
        Try
            If Monitor.TryEnter(Me, 2000) Then
                m_GraphicalStepsList.SetDrawState(True)
                m_GraphicalTransitionsList.SetDrawState(True)
                'Disegna lo stato
                m_GraphicalStepsList.DrawStepsState()
                m_GraphicalTransitionsList.DrawTransitionsState()
                Monitor.Exit(Me)
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
        End Try
    End Sub
    Public Sub StopStateMonitor()
        m_StateMonitor = False
        m_GraphicalStepsList.SetDrawState(False)
        m_GraphicalTransitionsList.SetDrawState(False)
    End Sub
    Public Sub ExecuteInit() Implements IIEC61131LanguageImplementation.ExecuteInit
        'Genera l'evento di inizio scansione
        RaiseEvent StartScan()
        Reset()
        '----------------------------
        'Blocco aggiuntivo alla norma
        'If Not Suspended And Not Stopped And Not forced Then    'Se è sospeso o bloccato non può essere inizializzato
        '----------------------------
        Try
            If Monitor.TryEnter(Me, 2000) Then
                m_GraphicalStepsList.ExecuteInit()
                Monitor.Exit(Me)
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
        End Try
        'Genera l'evento di fine scansione
        RaiseEvent EndScan()
    End Sub
    Public Function ExecuteScanCycle() As Boolean Implements IIEC61131LanguageImplementation.ExecuteScanCycle
        'Genera l'evento di inizio scansione
        RaiseEvent StartScan()
        '----------------------------
        'Blocco aggiuntivo alla norma
        If Not Stopped And Not Suspended And Not Forced Then    'Se è sospeso, bloccato o forzato non può essere inizializzato
            '----------------------------


            'Effettua la scansione dell'SFC
            'Se viene attivata almeno una fase finale restituisce True
            'Monitor sull'SFC e su GraphToDraw per disegnare lo stato
            Try
                If Monitor.TryEnter(Me, 100) Then
                    'Inizio ciclo di scansione (le  macrofasi precedenti devono essere attive(non basta preattive) per permettere di superare la condizione
                    m_ConditionChanged = m_GraphicalTransitionsList.ExecuteScanCycle
                    If m_ConditionChanged Then
                        'Se la condizione è cambiata preattiva e depreattiva le fasi
                        m_GraphicalTransitionsList.ExecutePreactivationCycle()
                        m_ActivatedPreactivedSteps = False
                        'Disegna lo stato sul form se richiesto
                    Else
                        'se la condizione non è cambiata la prima volta attiva le fasi (non macrofasi) preattive
                        'Se viene attivata almeno una fase finale memorizza a restituisce True
                        If Not m_ActivatedPreactivedSteps Then
                            ExecuteScanCycle = m_GraphicalStepsList.ActivePreactivedSteps()
                            m_ActivatedPreactivedSteps = True
                        End If
                    End If
                    'Cilo di scansione per le macrofasi
                    m_GraphicalStepsList.ExecuteMacroStepScanCycle()
                    'Esecuzione delle azioni
                    m_GraphicalStepsList.ExecuteAction()
                    'Fine ciclo di scansione
                    Monitor.Exit(Me)
                End If
            Catch ex As System.Exception
                Monitor.Exit(Me)
            End Try
        End If
        'Genera l'evento di fine scansione
        RaiseEvent EndScan()
    End Function
    Public Sub Reset() Implements IIEC61131LanguageImplementation.Reset
        'Monitor sull'SFC e su GraphToDraw per disegnare lo stato
        Try
            If Monitor.TryEnter(Me, 2000) Then
                m_GraphicalStepsList.ResetState()
                m_GraphicalTransitionsList.ResetState()
                ' questa riga impedisce su alcuni sistemi l'avvio della
                ' simulazione. poichè una chiamata del tutto analoga si
                ' trova alla fine dell'if, si rimuove questa
                ' Monitor.Exit(Me)
                m_ConditionChanged = False
                m_ActivatedPreactivedSteps = False
            End If
            Monitor.Exit(Me)
        Catch ex As System.Exception
            Monitor.Exit(Me)
        End Try
    End Sub

    '----------------------------
    'Blocco aggiuntivo alla norma
    Public Sub ForceMeImpulsively(ByRef StepToForces As GraphicalStepsList)
        'Effettua la forzatura impulsiva
        Try
            If Monitor.TryEnter(Me, 2000) Then
                'Disattiva tutte le fasi
                m_GraphicalStepsList.ResetState()
                For Each gst As GraphicalStep In m_GraphicalStepsList
                    'Disattiva le azioni delle fasi non forzate
                    gst.ResetActionsForSus()
                Next
                'Attiva le fasi in lista
                m_GraphicalStepsList.ActiveSteps(StepToForces)
                For Each gst As GraphicalStep In StepToForces
                    'Attiva le azioni delle fasi forzate
                    gst.ActiveActionsForFr()
                Next
                Monitor.Exit(Me)
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
        End Try
    End Sub
    Public Sub ForceMe(ByRef StepToForces As GraphicalStepsList)
        'Effettua la forzatura per tutta la durata della fase
        Try
            If Monitor.TryEnter(Me, 2000) Then
                If Suspended Then
                    ResumeMe()
                End If
                'Disattiva tutte le fasi
                m_GraphicalStepsList.ResetState()
                For Each gst As GraphicalStep In m_GraphicalStepsList
                    'Disattiva le azioni delle fasi non forzate
                    gst.ResetActionsForSus()
                Next
                'Attiva le fasi in lista
                m_GraphicalStepsList.ActiveSteps(StepToForces)
                For Each gst As GraphicalStep In StepToForces
                    'Attiva le azioni delle fasi forzate
                    gst.ActiveActionsForFr()
                Next
                Forced = True
                Monitor.Exit(Me)
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
        End Try
    End Sub
    Public Sub SuspendMe()
        'Monitor sull'SFC
        Try
            If Monitor.TryEnter(Me, 2000) Then
                'Disattiva tutte le fasi
                m_GraphicalStepsList.ResetState()
                For Each gst As GraphicalStep In m_GraphicalStepsList
                    'Disattiva le azioni durante la sospensione
                    gst.ResetActionsForSus()
                Next
                Suspended = True
                Monitor.Exit(Me)
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
        End Try

    End Sub
    Public Sub StopMe()
        Stopped = True
    End Sub
    Public Sub UnForceMe()
        For Each gst As GraphicalStep In m_GraphicalStepsList
            'Disattiva le azioni per iniziare il nuovo ciclo di scansione
            gst.ResetActionsForSus()
        Next
        Forced = False
    End Sub
    Public Sub ResumeMe()
        Suspended = False
    End Sub
    Public Sub ReStartMe()
        Stopped = False
    End Sub
    '----------------------------


    Public Sub PrintMe(ByRef Graph As Drawing.Graphics, ByVal Rect As Drawing.Rectangle) Implements IIEC61131LanguageImplementation.PrintMe
        Dim OldGraphToDraw As Drawing.Graphics = Nothing
        If Not IsNothing(m_GraphToDraw) Then
            OldGraphToDraw = m_GraphToDraw
        End If
        Dim AreaToDraw As New Drawing.Rectangle(0, 0, Rect.Width, Rect.Height)
        SetGraphToDraw(Graph)
        DrawElementsArea(AreaToDraw, False)
        ' ??? sembra inutile e ridondante
        'If Not IsNothing(m_GraphToDraw) Then
        'SetGraphToDraw(OldGraphToDraw)
        'End If
        SetGraphToDraw(OldGraphToDraw)
    End Sub

    Public Function FindElementByLocalId(ByVal id As Integer) As IHasLocalId Implements IIEC61131LanguageImplementation.FindElementByLocalId
        Dim ret As IHasLocalId = m_GraphicalStepsList.FindStepByNumber(id)
        If ret Is Nothing Then ret = m_GraphicalTransitionsList.FindTransitionByNumber(id)
        Return ret
    End Function

    ' Ritorna l'oggetto più a destra del diagramma
    Public Function GetRightmostObject() As IGraphicalObject
        Dim mostX As Integer = -1
        Dim mostXObject As IGraphicalObject = Nothing
        For Each GO As IGraphicalObject In GraphicalStepsList
            If GO.Position.X > mostX Then
                mostX = GO.Position.X
                mostXObject = GO
            End If
        Next
        Return mostXObject
    End Function

    ' Ritorna l'oggetto più in basso del diagramma
    Public Function GetLowermostObject() As IGraphicalObject
        Dim mostY As Integer = -1
        Dim mostYObject As IGraphicalObject = Nothing
        For Each GO As IGraphicalObject In GraphicalStepsList
            If GO.Position.Y > mostY Then
                mostY = GO.Position.Y
                mostYObject = GO
            End If
        Next
        Return mostYObject
    End Function

End Class
