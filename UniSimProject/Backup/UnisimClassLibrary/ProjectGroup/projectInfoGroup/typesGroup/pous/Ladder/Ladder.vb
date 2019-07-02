Imports System.IO
Imports System.Threading
Imports System.Xml
Imports System.Drawing
Imports System.Collections.Generic
Imports UnisimClassLibrary

Public Class Ladder
    Implements IIEC61131LanguageImplementation

    Private m_GraphicalConnectionList As GraphicalConnectionList
    Private m_GraphicalContactList As GraphicalContactList
    Private m_GraphicalVariablesList As GraphicalVariablesList
    Private m_GraphicalFBDConnections As GraphicalFBDConnectionsList

    'Le seguenti liste servono solo ai fini dell'esportazione xml
    Private m_Name As String
    Private m_ConditionChanged As Boolean
    Private m_ActivatedPreactivedSteps As Boolean
    Private m_pousList As Pous
    Private m_StateMonitor As Boolean
    Private m_SetDrawVariable As Boolean = False
    Private m_pouInterface As pouInterface
    Private m_ResGlobalVariables As VariablesLists
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
    Private m_GraphToDraw As Drawing.Graphics
    Private Const m_Dimension As Integer = 44 ' metterci Const dovrebbe permettere al compilatore di ottimizzare
    Public Event StartScan() Implements IIEC61131LanguageImplementation.StartScan
    Public Event EndScan() Implements IIEC61131LanguageImplementation.EndScan
    ' Mappato sulla variabile temporanea first_scan nella interfaccia della POU
    Private m_FirstScan As BooleanVariable
    'A che servono in una POU Ladder?
    '----------------------------
    'Blocco aggiuntivo alla norma
    'Private Suspended As Boolean
    'Private Stopped As Boolean
    'Private Forced As Boolean
    '----------------------------


    ' Crea se necessario le variabili di utilità dell'implementazione UniSim
    ' del linguaggio a contatti:
    ' first_scan    BOOL    alta durante il primo ciclo di scansione dei rung
    ' false         BOOL    sempre falsa
    Private Sub CreateUtilityVariables()
        If IsNothing(m_pouInterface.Variables("first_scan")) Then _
            m_pouInterface.tempVars.AddVariable(New BooleanVariable("first_scan", "", "", "false"))
        m_FirstScan = m_pouInterface.Variables("first_scan")
        If IsNothing(m_pouInterface.Variables("false")) Then _
            m_pouInterface.tempVars.AddVariable(New BooleanVariable("false", "", "", "false"))
    End Sub


    Public Sub New(ByVal MyBodyName As String, ByRef RefResGlobalVariables As VariablesLists, _
        ByRef RefPouInterface As pouInterface, ByRef pList As Pous)
        m_Name = MyBodyName
        m_pousList = pList
        m_ResGlobalVariables = RefResGlobalVariables
        m_pouInterface = RefPouInterface
        m_GraphicalContactList = New GraphicalContactList(Me, m_BackColor, m_SelectionColor, m_NotSelectionColor, m_TextColor, m_Char, m_ColorActive, m_ColorDeactive, m_ColorPreactive, m_GraphToDraw, m_Dimension)
        m_GraphicalConnectionList = New GraphicalConnectionList(Me, m_CSnap, m_BackColor, m_SelectionColor, m_NotSelectionColor, m_TextColor, m_Char, m_ColTransitionConditionTrue, m_ColTransitionConditionFalse, m_ColorDeactive, m_ColorDeactive, m_ColorPreactive, m_GraphToDraw, m_Dimension)
        m_GraphicalVariablesList = New GraphicalVariablesList(Me)
        m_GraphicalFBDConnections = New GraphicalFBDConnectionsList(Me)
        Call CreateUtilityVariables()
    End Sub
    Public Sub New(ByVal MyBodyName As String, ByRef RefResGlobalVariables As VariablesLists, _
        ByRef RefPouInterface As pouInterface, ByRef GrapStepsList As GraphicalContactList, _
        ByRef GrapTransitionsList As GraphicalConnectionList, _
        ByRef GrafVarsList As GraphicalVariablesList, ByRef GrafFConnList As GraphicalFBDConnectionsList)
        m_Name = MyBodyName
        m_pousList = Me.m_pousList
        m_ResGlobalVariables = RefResGlobalVariables
        m_pouInterface = RefPouInterface
        m_GraphicalContactList = GrapStepsList
        m_GraphicalConnectionList = GrapTransitionsList
        m_GraphicalVariablesList = GrafVarsList
        m_GraphicalFBDConnections = GrafFConnList
        Call CreateUtilityVariables()
    End Sub
    Property Name() As String Implements IIEC61131LanguageImplementation.Name
        Get
            Name = m_Name
        End Get
        Set(ByVal Value As String)
            m_Name = Value
        End Set
    End Property
    Property GraphicalContactList() As GraphicalContactList
        Get
            GraphicalContactList = m_GraphicalContactList
        End Get
        Set(ByVal Value As GraphicalContactList)
            m_GraphicalContactList = Value
        End Set
    End Property
    Property GraphicalConnectionList() As GraphicalConnectionList
        Get
            GraphicalConnectionList = m_GraphicalConnectionList
        End Get
        Set(ByVal Value As GraphicalConnectionList)
            m_GraphicalConnectionList = Value
        End Set
    End Property
    Property GraphicalVariablesList() As GraphicalVariablesList
        Get
            Return m_GraphicalVariablesList
        End Get
        Set(ByVal Value As GraphicalVariablesList)
            m_GraphicalVariablesList = Value
        End Set
    End Property
    Property GraphicalFBDConnectionsList() As GraphicalFBDConnectionsList
        Get
            Return m_GraphicalFBDConnections
        End Get
        Set(ByVal value As GraphicalFBDConnectionsList)
            m_GraphicalFBDConnections = value
        End Set
    End Property
    Public ReadOnly Property PousList() As Pous
        Get
            Return m_pousList
        End Get
    End Property


    ' Quante alimentazioni ci sono nel diagramma?
    ' Serve al FormLadder per settare Panel3.Height
    Public Function GetRailsCount() As Integer
        Dim leftRails As Integer = GraphicalContactList.GetLeftRails().Count
        Dim rightRails As Integer = GraphicalContactList.GetRightRails().Count
        Return Math.Max(leftRails, rightRails)
    End Function

    Public Function RemoveSelectedElements() As Boolean
        'Tenta di entrare nel ladder
        Try
            If Monitor.TryEnter(Me, 2000) Then
                m_GraphicalFBDConnections.RemoveSelectedElements()
                m_GraphicalConnectionList.RemoveSelectedElements()
                m_GraphicalContactList.RemoveSelectedElements()
                m_GraphicalVariablesList.RemoveSelectedElements()
                Monitor.Exit(Me)
                RemoveSelectedElements = True
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
            RemoveSelectedElements = False
        End Try
    End Function

    Public Function RemoveSelectedRail() As Boolean
        'Tenta di entrare nel ladder
        Try
            If Monitor.TryEnter(Me, 2000) Then
                m_GraphicalContactList.RemoveSelectedRail()
                ' bug fix: qui mancava
                Monitor.Exit(Me)
                RemoveSelectedRail = True
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
            RemoveSelectedRail = False
        End Try
    End Function
    Public Sub SelectRail()
        ' deseleziona tutto
        m_GraphicalContactList.DeSelectAll()
        m_GraphicalVariablesList.DeselectAll()
        ' ora seleziona solo le piste
        m_GraphicalContactList.FindAndSelectRail()
    End Sub
    Public Function FirstAvailableContactName() As String
        FirstAvailableContactName = ""
        Dim i As Integer
        Dim Find As Boolean
        For i = 1 To m_GraphicalContactList.Count + 1
            Find = False
            For Each S As BaseGraphicalContact In m_GraphicalContactList
                If S.Name = "C" & i Then
                    Find = True
                    Exit For
                End If
            Next S
            If Not Find Then
                FirstAvailableContactName = "C" & i
                Exit For
            End If
        Next i
    End Function

    Public Function FirstAvailableCoilName() As String
        FirstAvailableCoilName = ""
        Dim i As Integer
        Dim Find As Boolean
        For i = 1 To m_GraphicalContactList.Count + 1
            Find = False
            For Each S As BaseGraphicalContact In m_GraphicalContactList
                If S.Name = "S" & i Then
                    Find = True
                    Exit For
                End If
            Next S
            If Not Find Then
                FirstAvailableCoilName = "S" & i
                Exit For
            End If
        Next i
    End Function
    Public Function FirstAvailableBlockName() As String
        FirstAvailableBlockName = "B"
        Dim i As Integer
        Dim Find As Boolean
        For i = 1 To GraphicalContactList.Count + 1
            Find = False
            For Each S As BaseGraphicalContact In GraphicalContactList
                If S.Name = "B" & i Then
                    Find = True
                    Exit For
                End If
            Next S
            If Not Find Then
                FirstAvailableBlockName = "B" & i
                Exit For
            End If
        Next i
    End Function
    Public Function FindContactByName(ByVal Value As String) As BaseGraphicalContact
        For Each S As BaseGraphicalContact In m_GraphicalContactList
            If S.Name = Value Then
                Return S
            End If
        Next S
        Return Nothing
    End Function
    Public Function FindBlockByName(ByVal Value As String) As BaseGraphicalContact
        Return FindContactByName(Value)
    End Function
    Public Sub DeSelectAll()
        m_GraphicalContactList.DeSelectAll()
        m_GraphicalConnectionList.DeSelectAll()
        m_GraphicalVariablesList.DeselectAll()
        m_GraphicalFBDConnections.DeselectAll()
    End Sub

    Public Function AddAndDrawLeftRail(ByVal Number As Integer, ByVal Name As String, ByVal Documentation As String, ByVal Position As Drawing.Point, ByVal Id As Integer, ByVal VarName As String, ByVal Qualy As String, ByVal Var As BaseVariable, ByVal Ind As BaseVariable, ByVal Time As TimeSpan) As Boolean
        Try
            If Monitor.TryEnter(Me, 2000) Then
                m_GraphicalContactList.AddAndDrawLeftRail(Number, Name, Documentation, Position, m_StateMonitor, Id, VarName, Qualy, Ind, Var, Time)
                AddAndDrawLeftRail = True
                Monitor.Exit(Me)
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
            AddAndDrawLeftRail = True
        End Try
    End Function

    Public Function AddAndDrawRightRail(ByVal Number As Integer, ByVal Name As String, ByVal Documentation As String, ByVal Position As Drawing.Point, ByVal Id As Integer, ByVal VarName As String, ByVal Qualy As String, ByVal Var As BaseVariable, ByVal Ind As BaseVariable, ByVal Time As TimeSpan) As Boolean
        Try
            If Monitor.TryEnter(Me, 2000) Then
                m_GraphicalContactList.AddAndDrawRightRail(Number, Name, Documentation, Position, m_StateMonitor, Id, VarName, Qualy, Ind, Var, Time)
                AddAndDrawRightRail = True
                Monitor.Exit(Me)
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
            AddAndDrawRightRail = True
        End Try
    End Function

    Public Function AddAndDrawContact(ByVal Number As Integer, ByVal Name As String, ByVal Documentation As String, ByVal Position As Drawing.Point, ByVal Id As Integer, ByVal VarName As String, ByVal Qualy As String, ByVal Var As BaseVariable, ByVal Ind As BaseVariable, ByVal Time As TimeSpan) As Boolean
        Try
            If Monitor.TryEnter(Me, 2000) Then

                m_GraphicalContactList.AddAndDrawContact(Number, Name, Documentation, Position, _
                    m_StateMonitor, Id, VarName, Qualy, Var, Ind, Time)
                AddAndDrawContact = True
                Monitor.Exit(Me)
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
            AddAndDrawContact = True
        End Try
    End Function

    Public Function AddAndDrawBlock(ByVal Number As Integer, ByVal Name As String, ByVal Documentation As String, ByVal Position As Drawing.Point, ByVal Id As Integer, ByVal VarName As String, ByVal Qualy As String, ByVal Var As BaseVariable, ByVal Ind As BaseVariable, ByVal Time As TimeSpan) As Boolean
        If Monitor.TryEnter(Me, 2000) Then
            Try
                m_GraphicalContactList.AddAndDrawContact(Number, Name, Documentation, _
                    Position, m_StateMonitor, Id, VarName, Qualy, Var, Ind, Time)
                Return True
            Finally
                Monitor.Exit(Me)
            End Try
        End If
    End Function

    Public Sub AddAndDrawVariable(ByVal bVar As BaseVariable, ByVal vType As GraphicalVariableType, _
        ByVal pos As Drawing.Point, ByVal dimen As Drawing.Size)
        Dim obj As New GraphicalVariable(Me, bVar, FirstAvailableElementNumber(), _
             dimen, pos, vType)
        obj.SetGraphToDraw(Me.m_GraphToDraw)
        m_GraphicalVariablesList.Add(obj)
        obj.Draw(False)
    End Sub

    Public Sub AddAndDrawFBDConnection(ByVal src As Integer, ByVal dst As Integer, _
        Optional ByVal offset As Integer = 0)
        AddAndDrawFBDConnection(CType(FindElementByLocalId(src), IPureConnectable), _
            CType(FindElementByLocalId(dst), IPureConnectable), offset)
    End Sub

    Public Sub AddAndDrawFBDConnection(ByVal src As IPureConnectable, ByVal dst As IPureConnectable, _
        Optional ByVal offset As Integer = 0)
        Dim fbdConn As New GraphicalFBDConnection(src.Number, dst.Number, Me)
        fbdConn.SetGraphToDraw(Me.m_GraphToDraw)
        fbdConn.Offset = offset
        m_GraphicalFBDConnections.Add(fbdConn)
    End Sub

    Public Function FindAndSelectSmallRectangleStep(ByVal x As Integer, ByVal y As Integer) As Boolean
        FindAndSelectSmallRectangleStep = _
            m_GraphicalContactList.FindAndSelectSmallRectangleStep(x, y)
        If Not FindAndSelectSmallRectangleStep Then
            FindAndSelectSmallRectangleStep = _
                GraphicalVariablesList.FindAndSelectSmallRectangleVariable(x, y)
        End If
    End Function

    Public Sub SetGraphToDraw(ByRef Graph As Drawing.Graphics) Implements IIEC61131LanguageImplementation.SetGraphToDraw
        m_GraphToDraw = Graph
        m_GraphicalContactList.SetGraphToDraw(m_GraphToDraw)
        m_GraphicalConnectionList.SetGraphToDraw(m_GraphToDraw)
        m_GraphicalVariablesList.SetGraphToDraw(m_GraphToDraw)
        m_GraphicalFBDConnections.SetGraphToDraw(m_GraphToDraw)
    End Sub

    Public Function CancelSelection(ByVal CancelSmallRectangles As Boolean) As Boolean
        'Cancella su GraphToDraw gli elementi selezionati
        'Tenta di entrare nel ladder e nel GraphToDraw
        Try
            If Monitor.TryEnter(Me, 2000) Then
                m_GraphicalContactList.CancelSelection(CancelSmallRectangles)
                m_GraphicalConnectionList.CancelTransitionWithSelectedSteps(m_FlagShowDetails)
                m_GraphicalConnectionList.CancelSelection(m_FlagShowDetails)
                m_GraphicalVariablesList.CancelSelection(CancelSmallRectangles)
                m_GraphicalFBDConnections.CancelSelection(CancelSmallRectangles)
                CancelSelection = True
                Monitor.Exit(Me)
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
            CancelSelection = False
        End Try
    End Function

    Public Sub MoveSelection(ByVal dx As Integer, ByVal dy As Integer)
        m_GraphicalContactList.MoveSelection(dx, dy)
        m_GraphicalConnectionList.MoveSelection(dx)
        m_GraphicalVariablesList.MoveSelection(dx, dy)
        m_GraphicalFBDConnections.MoveSelection(dx, dy)
    End Sub

    Public Sub FindAndSelectElementsArea(ByVal Rect As Drawing.Rectangle)
        m_GraphicalContactList.FindAndSelectSteps(Rect)
        m_GraphicalConnectionList.FindAndSelectConnection(Rect)
        m_GraphicalVariablesList.FindAndSelectVariables(Rect)
        m_GraphicalFBDConnections.FindAndSelectConnections(Rect)
    End Sub
    Public Sub FindAndSelectElements(ByVal x As Integer, ByVal y As Integer)
        m_GraphicalContactList.FindAndSelectContact(x, y)
        m_GraphicalConnectionList.FindAndSelectConnection(x, y)
        m_GraphicalVariablesList.FindAndSelectVariable(x, y)
        m_GraphicalFBDConnections.FindAndSelectConnection(x, y)
    End Sub

    Public Function IsSelectionOutside(ByVal R As Drawing.Rectangle, ByRef FuoriX As Boolean, ByRef FuoriY As Boolean) As Boolean
        'Verifica se gli Elements Selected sono fuori dal Rectangle R
        IsSelectionOutside = m_GraphicalContactList.IsSelectionOutside(R, FuoriX, FuoriY) Or _
            m_GraphicalConnectionList.IsSelectionOutside(R, FuoriX, FuoriY) Or _
                m_GraphicalVariablesList.CheckForSelectionOutside(R, FuoriX, FuoriY)
    End Function

        ' Un metodo abbastanza sicuro per ottenere numeri localId univoci. I tempi di esecuzione sono dell'ordine
        ' di O(n) alla prima esecuzione, O(1) per le successive. La precedente implementazione era sempre O(1)
        ' ma non era evidente che avrebbe generato numeri univoci in ogni circostanza (anche se non si è trovata
        ' una prova del contrario)
    Public Function FirstAvailableElementNumber() As Integer
        Static i As Integer = -1
        If (i < 0) Then
            For Each C As BaseGraphicalContact In GraphicalContactList
                i = Math.Max(i, C.Number)
            Next
            For Each V As GraphicalVariable In m_GraphicalVariablesList
                i = Math.Max(i, V.Number)
            Next
            ' Il minimo localId che vogliamo è 1. Se mettessimo qui 1, il minimo però sarebbe 2
            If (i < 1) Then i = 0
        End If
        i += 1
        Return i
    End Function


    Public Function FindAndSelectElement(ByVal x As Integer, ByVal y As Integer) As Boolean
        'cerca tra i contatti
        FindAndSelectElement = m_GraphicalContactList.FindAndSelectContact(x, y)
        If Not FindAndSelectElement Then
            'cerca tra le connessioni
            FindAndSelectElement = m_GraphicalConnectionList.FindAndSelectConnection(x, y)
            If Not FindAndSelectElement Then
                FindAndSelectElement = m_GraphicalVariablesList.FindAndSelectVariable(x, y)
                If Not FindAndSelectElement Then
                    FindAndSelectElement = m_GraphicalFBDConnections.FindAndSelectConnection(x, y)
                End If
            End If
        End If
    End Function

    Public Function FindElement(ByVal x As Integer, ByVal y As Integer) As Boolean
        FindElement = m_GraphicalContactList.FindContact(x, y)
        If Not FindElement Then
            FindElement = m_GraphicalConnectionList.FindConnection(x, y)
            If Not FindElement Then
                FindElement = m_GraphicalVariablesList.FindVariable(x, y)
                If Not FindElement Then
                    FindElement = m_GraphicalFBDConnections.FindConnection(x, y)
                End If
            End If
        End If
    End Function

    Public Function ReadIfElementIsSelected(ByVal x As Integer, ByVal y As Integer) As Boolean
        ReadIfElementIsSelected = m_GraphicalContactList.ReadIfContactSelected(x, y)
        If Not ReadIfElementIsSelected Then
            ReadIfElementIsSelected = m_GraphicalConnectionList.ReadIfConnectionSelected(x, y)
            If Not ReadIfElementIsSelected Then
                ReadIfElementIsSelected = m_GraphicalVariablesList.ReadIfVariableSelected(x, y)
                If Not ReadIfElementIsSelected Then
                    ReadIfElementIsSelected = m_GraphicalFBDConnections.ReadIfConnectionSelected(x, y)
                End If
            End If
        End If
    End Function

    Public Sub DrawElementsArea(ByVal Rect As Drawing.Rectangle, ByVal DrawSmallRectangles As Boolean)
        If Monitor.TryEnter(Me, 2000) Then
            Try
                m_GraphicalContactList.DrawArea(Rect, DrawSmallRectangles)
                m_GraphicalConnectionList.DrawArea(Rect, True)
                m_GraphicalVariablesList.DrawArea(Rect, DrawSmallRectangles)
                m_GraphicalFBDConnections.DrawArea(Rect, DrawSmallRectangles)
            Finally
                Monitor.Exit(Me)
            End Try
        End If
    End Sub

    Public Function FindFBDObjectAcceptingOutgoingConnection() As IPureConnectable
        For Each GC As BaseGraphicalContact In m_GraphicalContactList
            If GC.IsBlock Then
                If CType(GC, GraphicalContact).InFBDRectangleSelected Then Return GC
            End If
        Next
        For Each GV As GraphicalVariable In m_GraphicalVariablesList
            If GV.RectangleSelected AndAlso GV.VariableType = GraphicalVariableType.Output Then Return GV
        Next
        Return Nothing
    End Function

    Public Function FindFBDObjectAcceptingIncomingConnection() As IPureConnectable
        For Each GC As BaseGraphicalContact In m_GraphicalContactList
            If GC.IsBlock Then
                If CType(GC, GraphicalContact).OutFBDRectangleSelected Then Return GC
            End If
        Next
        For Each GV As GraphicalVariable In m_GraphicalVariablesList
            If GV.RectangleSelected AndAlso GV.VariableType = GraphicalVariableType.Input Then Return GV
        Next
        Return Nothing
    End Function

    Public Function AddAndDrawConnection(ByVal Number As Integer, ByVal Name As String, _
        ByVal Documentation As String, ByRef RefCondition As BooleanExpression) As Boolean
        'Tenta di entrare nell'sfc e nel GraphToDraw
        Try
            If Monitor.TryEnter(Me, 2000) Then
                m_GraphicalConnectionList.AddAndDrawConnection(Number, Name, Documentation, _
                    m_GraphicalContactList.ReadBottomSelectedStepList, _
                    m_GraphicalContactList.ReadTopSelectedContactList, RefCondition, m_FlagShowDetails, _
                    m_StateMonitor)
                Monitor.Exit(Me)
                AddAndDrawConnection = True
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
            AddAndDrawConnection = False
        End Try
    End Function
    Public Function CountSelectedElement() As Integer
        CountSelectedElement = m_GraphicalContactList.CountSelected
        CountSelectedElement = CountSelectedElement + m_GraphicalConnectionList.CountSelected
        CountSelectedElement = CountSelectedElement + m_GraphicalVariablesList.CountSelected
        CountSelectedElement = CountSelectedElement + m_GraphicalFBDConnections.CountSelected
    End Function

    Public Function ReadObjectSelected() As Object
        ReadObjectSelected = m_GraphicalContactList.ReadContactSelected
        If IsNothing(ReadObjectSelected) Then
            ReadObjectSelected = m_GraphicalConnectionList.ReadConnectionSelected
            If IsNothing(ReadObjectSelected) Then
                ReadObjectSelected = m_GraphicalVariablesList.ReadVariableSelected
                If IsNothing(ReadObjectSelected) Then
                    ReadObjectSelected = m_GraphicalFBDConnections.ReadConnectionSelected
                End If
            End If
        End If
    End Function

    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter) Implements IIEC61131LanguageImplementation.xmlExport

        ' niente di tutto questo esiste in linguaggio a contatti...

        'Riempie le liste di convergenze e divergenze e delle connessioni
        'Svuota le connessioni
        'm_GraphicalContactList.ClearXMLConnectionsLists()
        'm_GraphicalConnectionList.ClearXMLConnectionsLists()

        'm_XmlSimultaneousConvergences = New XmlSimultaneousConvergences
        'm_XmlSimultaneousDivergences = New XmlSimultaneousDivergences
        'm_XmlSelectionConvergences = New XmlSelectionConvergences
        'm_XmlSelectionDivergences = New XmlSelectionDivergences
        'Dim NextNumberAvaiable As Integer = FirstAvailableElementNumber()
        ''Riempie le liste di convergenze e divergenze
        'm_GraphicalTransitionsList.CreateXmlConvergencesAndDivergences(NextNumberAvaiable)

        '       'Riempie le liste di convergenze e divergenze selettive
        '        m_GraphicalStepsList.CreateXmlConvergencesAndDivergences(NextNumberAvaiable)

        'Inizio esportazione XML
        'LADDER
        RefXMLProjectWriter.WriteStartElement("LD")
        'Esporta tutto (contatti e connessioni)
        'Per come è concepito il formato XML di PLCOpen la cosa più saggia (e anche l'unica ragionevolmente
        'possibile) è di salvare sia i contatti sia le connessioni in un unico passaggio attraverso i dati
        'UniSim delega tutto alla lista dei contatti (sarebbe possibile anche la scelta inversa, ma risulterebbe
        'meno naturale)
        m_GraphicalContactList.xmlExport(RefXMLProjectWriter)
        m_GraphicalVariablesList.xmlExport(RefXMLProjectWriter)
        ' Inutile
        ' ==> m_GraphicalConnectionList.xmlExport(RefXMLProjectWriter) <==
        'Esporta le convergenze e divergenza (in Ladder? mah...)
        'm_XmlSimultaneousConvergences.xmlExport(RefXMLProjectWriter)
        'm_XmlSimultaneousDivergences.xmlExport(RefXMLProjectWriter)
        'm_XmlSelectionConvergences.xmlExport(RefXMLProjectWriter)
        'm_XmlSelectionDivergences.xmlExport(RefXMLProjectWriter)

        RefXMLProjectWriter.WriteEndElement() 'LD
        'Fine esportazione XML

        'm_XmlSimultaneousConvergences = Nothing
        'm_XmlSimultaneousDivergences = Nothing
        'm_XmlSelectionConvergences = Nothing
        'm_XmlSelectionDivergences = Nothing
    End Sub

    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader) Implements IIEC61131LanguageImplementation.xmlImport

        '    'Crea le liste per le convergenz e divergenze
        '   m_XmlSimultaneousConvergences = New XmlSimultaneousConvergences
        '  m_XmlSimultaneousDivergences = New XmlSimultaneousDivergences
        ' m_XmlSelectionConvergences = New XmlSelectionConvergences
        ' m_XmlSelectionDivergences = New XmlSelectionDivergences

        'Memorizza la profondità del nodo
        Dim NodeDepth As Integer = RefXmlProjectReader.Depth
        'Si sposta sul nodo successivo
        RefXmlProjectReader.Read()

        'Scorre fino alla fine del LADDER
        While RefXmlProjectReader.Depth > NodeDepth
            Select Case RefXmlProjectReader.Name
                Case "contact", "coil", "leftPowerRail", "rightPowerRail", "block"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                        'Aggiunge il contatto alla lista
                        m_GraphicalContactList.xmlImport(RefXmlProjectReader)
                    End If
                Case "inVariable", "outVariable"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                        ' importa la variabile
                        Dim gVar As New GraphicalVariable(Me)
                        gVar.xmlImport(RefXmlProjectReader)
                        m_GraphicalVariablesList.Add(gVar)
                    End If
            End Select
            'Si sposta sul nodo successivo
            RefXmlProjectReader.Read()

        End While

        ''''''''''''''  'Risolve i collegamenti delle transizioni eliminando convergenze e divergenza
        '''''''''  'Risolve i collegamenti precedenti nelle transizioni
        'm_GraphicalTransitionsList.ResolveXmlConvergencesAndDivergences()
        ''''''''''''Risolve i collegamenti precedenti nelle fasi
        ' m_GraphicalStepsList.ResolveXmlConvergencesAndDivergences()


        'per ora commento

        '''''''''Svuota le lista delle convergenze e divergenza
        'm_XmlSimultaneousConvergences.Clear()
        'm_XmlSimultaneousDivergences.Clear()
        'm_XmlSelectionConvergences.Clear()
        'm_XmlSelectionDivergences.Clear()
        'm_XmlSimultaneousConvergences = Nothing
        'm_XmlSimultaneousDivergences = Nothing
        'm_XmlSelectionConvergences = Nothing
        'm_XmlSelectionDivergences = Nothing
    End Sub
    Public Function CreateInstance() As IIEC61131LanguageImplementation Implements IIEC61131LanguageImplementation.CreateInstance
        Dim NewLadder As New Ladder(m_Name, m_ResGlobalVariables, _
            m_pouInterface, m_pousList)
        NewLadder.GraphicalContactList = GraphicalContactList.CreateInstance(NewLadder)
        NewLadder.GraphicalConnectionList = GraphicalConnectionList.CreateInstance(NewLadder, _
            NewLadder.GraphicalContactList)
        NewLadder.GraphicalVariablesList = GraphicalVariablesList.CreateInstance(NewLadder)
        NewLadder.GraphicalFBDConnectionsList = GraphicalFBDConnectionsList.CreateInstance(NewLadder)
        CreateInstance = NewLadder
    End Function
    Property ResGlobalVariables() As VariablesLists Implements IIEC61131LanguageImplementation.ResGlobalVariables
        Get
            ResGlobalVariables = m_ResGlobalVariables
        End Get
        Set(ByVal Value As VariablesLists)
            m_ResGlobalVariables = Value
            'Comunica alle Macrofasi le lista di variabili globali della risorsa
            m_GraphicalContactList.ResGlobalVariables = m_ResGlobalVariables
        End Set
    End Property
    Property PouInterface() As pouInterface Implements IIEC61131LanguageImplementation.PouInterface
        Get
            PouInterface = m_pouInterface
        End Get
        Set(ByVal Value As pouInterface)
            m_pouInterface = Value
            'Comunica alle Macrofasi l'interfaccia della pou
            'm_GraphicalContactList.PouInterface = m_pouInterface
        End Set
    End Property
    Public Sub StartStateMonitor()
        m_StateMonitor = True
        Try
            If Monitor.TryEnter(Me, 2000) Then
                m_GraphicalContactList.SetDrawState(True)
                'Disegna lo stato
                m_GraphicalContactList.DrawStepsState()
                Monitor.Exit(Me)
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
        End Try
    End Sub

    Public Sub SetDrawVariable()
        m_SetDrawVariable = Not m_SetDrawVariable
        Try
            If Monitor.TryEnter(Me, 2000) Then
                m_GraphicalContactList.SetDrawVariable(m_SetDrawVariable)
                Monitor.Exit(Me)
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
        End Try
    End Sub


    Public Sub StopStateMonitor()
        m_StateMonitor = False
        m_GraphicalContactList.SetDrawState(False)
    End Sub

    Private Function GetRungsRtL() As List(Of Rung)
        Dim ret As New List(Of Rung)
        Dim rightRails As GraphicalContactList = m_GraphicalContactList.GetRightRails()
        For Each rail As GraphicalRightRail In rightRails
            ret.Add(New Rung(rail))
        Next
        Return ret
    End Function
    Private Function GetRungsLtR() As List(Of Rung)
        Dim ret As New List(Of Rung)
        Dim leftRails As GraphicalContactList = m_GraphicalContactList.GetLeftRails()
        For Each rail As GraphicalLeftRail In leftRails
            ret.Add(New Rung(rail))
        Next
        Return ret
    End Function

    Public Shared RtL As Boolean = True
    Public Function GetRungs() As List(Of Rung)
        Try
            If RtL Then Return GetRungsRtL()
            Return GetRungsLtR()
            ' Per rendere utilizzabile l'editor si sono rimosse alcune sincronizzazioni
            ' relative a monitor che vengono acquisiti ma, per motivi ancora non chiari,
            ' mai rilasciati. Per come è concepito il framework .net, modificare una
            ' collezione invalida tutti gli enumeratori (iteratori) su di essa e genera
            ' una eccezione appena si cerca di usarli. Qui catturiamo questa eccezione,
            ' aspettiamo 0.1 secondi e poi riproviamo. L'unico problema è che potremmo
            ' finire per esaurire lo stack (e dover aspettare anche un pò di tempo prima
            ' di avere questo risultato) ma nella grande maggioranza dei casi questo
            ' spezzone di codice è un fix risolutivo (e inoltre esaurire lo stack ci da
            ' almeno una indicazione che qualcosa non va)
        Catch ex As Exception
            Thread.Sleep(100)
            Return GetRungs()
        End Try
    End Function

    Public Function ExecuteScanCycle() As Boolean Implements IIEC61131LanguageImplementation.ExecuteScanCycle

        RaiseEvent StartScan()

        Dim anyChange As Boolean = False

        Dim rungs As List(Of Rung) = GetRungs()
        ' bug fix esempio 2.5 "Chiacchio - Basile" Tecnologie informatiche x l'automazione
        GraphicalContactList.SetLastValues()

        For Each r As Rung In rungs
            anyChange = anyChange Or r.ExecuteScanCycle(Me)
        Next
        For Each r As Rung In rungs
            r.CommitChanges()
        Next

        If anyChange Then
            m_GraphicalContactList.DrawStepsState()
            m_GraphicalVariablesList.DrawVariables()
        End If

        RaiseEvent EndScan()

        m_FirstScan.SetValue(False)

        Return anyChange

    End Function

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
                m_GraphicalContactList.ExecuteInit()
                Monitor.Exit(Me)
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
        End Try
        'Genera l'evento di fine scansione
        m_FirstScan.SetValue(True)
        RaiseEvent EndScan()
    End Sub

    Public Sub Reset() Implements IIEC61131LanguageImplementation.Reset
        'Monitor sul LADDER e su GraphToDraw per disegnare lo stato
        Try
            If Monitor.TryEnter(Me, 2000) Then
                m_GraphicalContactList.ResetState()
                Monitor.Exit(Me)
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
        End Try
    End Sub
    Public Sub ResolveVariablesLinks() Implements IIEC61131LanguageImplementation.ResolveVariableLinks
        'Risolve i riferimenti alle variabili e crea le connessioni (anche se il nome del metodo non lo
        'farebbe immaginare è così)
        m_GraphicalContactList.ResolveVariablesLinks()
        m_GraphicalVariablesList.ResolveVariableLinks()
        m_GraphicalFBDConnections.ResolveVariableLinks()
    End Sub

    Public ReadOnly Property IsFirstScan() As Boolean
        Get
            Return m_FirstScan.ReadValue()
        End Get
    End Property

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
        FindElementByLocalId = GraphicalContactList.FindContactByNumber(id)
        If FindElementByLocalId Is Nothing Then
            FindElementByLocalId = m_GraphicalVariablesList.FindVariableByNumber(id)
        End If
    End Function
End Class