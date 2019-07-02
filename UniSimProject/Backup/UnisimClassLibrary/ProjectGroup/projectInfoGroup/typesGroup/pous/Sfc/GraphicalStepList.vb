Imports System.IO
Imports System.Xml
Imports System.Collections.Generic
Public Class GraphicalStepsList
    Inherits List(Of BaseGraphicalStep)
    Implements IXMLSerializable

    Private MySfc As Sfc
    Private Dimension As Integer
    Private SelectionColor as Drawing.Color
    Private NotSelectionColor as Drawing.Color
    Private TextColor as Drawing.Color
    Private Carattere As drawing.Font
    Private BackColor as Drawing.Color
    Private ActiveColor as Drawing.Color
    Private PreActiveColor as Drawing.Color
    Private DeactiveColor as Drawing.Color
    Private GraphToDraw As drawing.Graphics
    Private DrawState As Boolean
    Private m_pouslist As Pous
    WriteOnly Property ResGlobalVariables() As VariablesLists
        Set(ByVal Value As VariablesLists)
            'Comunica alle Macrofasi le lista di variabili globali della risorsa
            For Each S As BaseGraphicalStep In Me
                If S.GetType.Name = "GraphicalMacroStep" Then
                    Dim MS As GraphicalMacroStep = S
                    MS.ResGlobalVariables = Value
                End If
            Next
        End Set
    End Property
    WriteOnly Property PouInterface() As PouInterface
        Set(ByVal Value As PouInterface)
            'Comunica alle Macrofasi l'interfaccia della pou
            For Each S As BaseGraphicalStep In Me
                If S.GetType.Name = "GraphicalMacroStep" Then
                    Dim MS As GraphicalMacroStep = S
                    MS.PouInterface = Value
                End If
            Next
        End Set
    End Property
    Sub New(ByRef RefSfc As Sfc, ByVal BackCol As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal ColTesto As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, ByRef Graph As Drawing.Graphics, ByVal Dimen As Integer, ByRef pouslist As Pous)
        MyBase.new()
        MySfc = RefSfc
        SelectionColor = SelectionCol
        NotSelectionColor = NotSelectionCol
        BackColor = BackCol
        TextColor = ColTesto
        Carattere = Car
        ActiveColor = ColActive
        DeactiveColor = ColDeactive
        PreActiveColor = ColPreActive
        GraphToDraw = Graph
        Dimension = Dimen
        m_pouslist = pouslist
    End Sub
    Public Sub New(ByRef R As BinaryReader, ByVal BackCol As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal ColTesto As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, ByRef Graph As Drawing.Graphics, ByRef pouslist As Pous)
        MyBase.new()
        SelectionColor = SelectionCol
        NotSelectionColor = NotSelectionCol
        BackColor = BackCol
        TextColor = ColTesto
        Carattere = Car
        ActiveColor = ColActive
        DeactiveColor = ColDeactive
        PreActiveColor = ColPreActive
        GraphToDraw = Graph
        Dim NumSteps As Integer = R.ReadInt32
        ' For i = 1 To NumSteps
        'Dim S As New GraphicalStep(R, BackCol, SelectionCol, NotSelectionCol, ColTesto, Car, ColActive, ColDeactive, ColPreActive)
        'Add(S)
        ' Next i
        m_pouslist = pouslist
    End Sub
    Public Sub New()
        MyBase.new()
    End Sub
    Public Function CreateInstance(ByRef NewSfc As Sfc) As GraphicalStepsList
        CreateInstance = New GraphicalStepsList(NewSfc, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, Nothing, Dimension, m_pouslist)
        For Each S As BaseGraphicalStep In Me
            CreateInstance.Add(S.CreateInstance(NewSfc))
        Next S
    End Function
    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter) Implements IXMLSerializable.xmlExport
        For Each S As BaseGraphicalStep In Me
            S.xmlExport(RefXMLProjectWriter)
        Next S
    End Sub
    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader) Implements IXMLSerializable.xmlImport
        'Seleziona se è una fase o macrofase
        Select Case RefXmlProjectReader.Name
            Case "step"
                'Crea la fase
                Dim S As New GraphicalStep(MySfc, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, GraphToDraw, DrawState, Dimension, m_pouslist)
                'Legge i dati xml
                S.xmlImport(RefXmlProjectReader)
                'Aggiunge la fase alla lista
                Add(S)
            Case "macroStep"
                'Crea la fase
                Dim MS As New GraphicalMacroStep(MySfc, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, GraphToDraw, DrawState, Dimension)
                'Legge i dati xml
                MS.xmlImport(RefXmlProjectReader)
                'Aggiunge la fase alla lista
                Add(MS)
            Case "actionBlock"
                'Crea la fase
                Dim AB As New ActionBlock(Dimension, MySfc, Nothing, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, GraphToDraw, m_pouslist)
                'Legge i dati xml
                AB.xmlImport(RefXmlProjectReader)
                'Aggiunge l'ActionBlock alla fase
                If AB.XmlPreviousConnectionsList.Count > 0 Then
                    Dim S As BaseGraphicalStep = FindStepByNumber(AB.XmlPreviousConnectionsList(0))
                    If S.GetType.Name = "GraphicalStep" Then
                        Dim NS As GraphicalStep = S
                        NS.SetActionBlock(AB)
                    End If
                End If
        End Select
    End Sub
    Public Sub CreateXmlConvergencesAndDivergences(ByRef NextNumberAvaiable As Integer)
        'Crea le convergenze e divergenze selettive e le memorizza
        For Each A As BaseGraphicalStep In Me
            A.CreateXmlConvergencesAndDivergences(NextNumberAvaiable)
        Next A
    End Sub
    Public Sub ResolveXmlConvergencesAndDivergences()
        For Each S As BaseGraphicalStep In Me
            S.ResolveXmlConvergencesAndDivergences()
        Next S
    End Sub
    Public Sub ClearXMLConnectionsLists()
        'Svuota le liste delle connessioni delle fase per l'esportazione xml
        For Each F As BaseGraphicalStep In Me
            F.ReadXmlPreviousConnectionsList.Clear()
            F.ReadXmlNextConnectionsList.Clear()
        Next F
    End Sub
    Public Sub ResolveVariablesLinks()
        'Risolve i riferimenti dei nomi di variabili nella azioni
        For Each S As BaseGraphicalStep In Me
            S.ResolveVariablesLinks()
        Next S
    End Sub
    Public Sub DisposeMe()
        'Distrugge tutte le fasi
        For Each S As BaseGraphicalStep In Me
            S.DisposeMe()
        Next S
        Me.Finalize()
    End Sub
    Public Sub SetGraphToDraw(ByRef Graph As Drawing.Graphics)
        GraphToDraw = Graph
        For Each S As BaseGraphicalStep In Me
            S.SetGraphToDraw(GraphToDraw)
        Next
    End Sub
    Public Sub SetDrawState(ByVal DrState As Boolean)
        DrawState = DrState
        For Each F As BaseGraphicalStep In Me
            F.SetDrawState(DrawState)
        Next F
    End Sub
    Public Sub DrawStepsState()
        For Each F As BaseGraphicalStep In Me
            F.DrawStepState(False)
        Next F
    End Sub
    Public Sub AddExistingStep(ByVal S As BaseGraphicalStep)
        Add(S)
    End Sub
    Public Sub AddAndDrawStep(ByVal StepNumber As Integer, ByVal StepName As String, ByVal StepDocumentation As String, ByVal Position As Drawing.Point, ByVal DrawState As Boolean)
        Dim S As New GraphicalStep(StepNumber, StepName, StepDocumentation, MySfc, False, False, Position, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, GraphToDraw, DrawState, Dimension, m_pouslist)
        Add(S)
        S.Draw(False)
    End Sub
    Public Sub AddAndDrawMacroStep(ByVal StepNumber As Integer, ByVal StepName As String, ByVal StepDocumentation As String, ByRef ResGlobalVariables As VariablesLists, ByVal Position As Drawing.Point, ByVal DrState As Boolean)
        Dim S As New GraphicalMacroStep(StepNumber, StepName, StepDocumentation, MySfc, False, False, Position, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, GraphToDraw, DrState, Dimension, m_pouslist)
        Me.Add(S)
        S.Draw(False)
    End Sub
    Public Function FindAndSelectStep(ByVal x As Integer, ByVal y As Integer) As Boolean
        'Seleziona solo la prima fase che trova
        For Each S As BaseGraphicalStep In Me
            If S.MyAreaIsHere(x, y) = True Then
                FindAndSelectStep = True
                S.SetSelected(True)
                Exit Function
            End If
        Next S
    End Function
    Public Function FindStep(ByVal x As Integer, ByVal y As Integer) As Boolean
        'Trova la prima fase in x,y
        For Each S As BaseGraphicalStep In Me
            If S.MyAreaIsHere(x, y) Then
                FindStep = True
                Exit Function
            End If
        Next S
    End Function
    Public Function ReadIfStepSelected(ByVal x As Integer, ByVal y As Integer) As Boolean
        'Trova la prima fase in x,y
        For Each S As BaseGraphicalStep In Me
            If S.MyAreaIsHere(x, y) And S.ReadSelected Then
                ReadIfStepSelected = True
                Exit Function
            End If
        Next S
    End Function
    Public Sub FindAndSelectSteps(ByVal Rect As Drawing.Rectangle)
        'Seleziona tutte le fasi che trova
        Dim x, y, Passo As Integer
        For Each S As BaseGraphicalStep In Me
            Passo = S.ReadDimension()
            For x = Rect.X To Rect.X + Rect.Width Step Passo
                For y = Rect.Y To Rect.Y + Rect.Height Step Passo
                    If S.MyAreaIsHere(x, y) = True Then
                        S.SetSelected(True)
                    End If
                Next y
            Next x
            'Ricerca sul bordo verticale destro
            x = Rect.X + Rect.Width
            For y = Rect.Y To Rect.Y + Rect.Height Step Passo
                If S.MyAreaIsHere(x, y) = True Then
                    S.SetSelected(True)
                End If
            Next y
            'Ricerca sul bordo orizzontale sinistro
            y = Rect.Y + Rect.Height
            For x = Rect.X To Rect.X + Rect.Width Step Passo
                If S.MyAreaIsHere(x, y) = True Then
                    S.SetSelected(True)
                End If
            Next x
            'Ricerca sull'angolo inferiore destro 
            x = Rect.X + Rect.Width
            y = Rect.Y + Rect.Height
            If S.MyAreaIsHere(x, y) = True Then
                S.SetSelected(True)
            End If
        Next S
    End Sub
    Public Function FindStepByNumber(ByVal n As Integer) As BaseGraphicalStep
        For Each S As BaseGraphicalStep In Me
            If S.Number = n Then
                Return S
            End If
        Next S
        Return Nothing
    End Function
    Function FirstAvaiableStepNumber() As Integer
        Dim j As Integer = 0
        Dim NumberAvaiable As Boolean
        While j < Count + 1
            j = j + 1
            NumberAvaiable = True
            For Each S As BaseGraphicalStep In Me
                If TypeOf (S) Is GraphicalStep Then
                    If S.Number = j Then
                        NumberAvaiable = False
                        Exit For 'Ha trovato una fase co il numero j
                    End If
                End If
            Next S
            If NumberAvaiable Then
                FirstAvaiableStepNumber = j
                Exit While
            End If
        End While
    End Function
    Function FirstAvaiableMacroStepNumber() As Integer
        Dim j As Integer = 0
        Dim NumberAvaiable As Boolean
        While j < Count + 1
            j = j + 1
            NumberAvaiable = True
            For Each S As BaseGraphicalStep In Me
                If S.GetType.Name = "GraphicalMacroStep" Then
                    If S.Number = j Then
                        NumberAvaiable = False
                        Exit For 'Ha trovato una fase co il numero j
                    End If
                End If
            Next S
            If NumberAvaiable Then
                FirstAvaiableMacroStepNumber = j
                Exit While
            End If

        End While
    End Function
    Public Function ReadStep(ByVal n As Integer) As BaseGraphicalStep
        ReadStep = Me(IndexOfStep(n))
    End Function
    Public Function IndexOfStep(ByVal n As Integer) As Integer
        Dim i As Integer
        IndexOfStep = -1
        For i = 0 To Count - 1
            If Me(i).Number = n Then
                IndexOfStep = i
                Exit For
            End If
        Next
    End Function
    Public Function IndexOfStep(ByRef S As BaseGraphicalStep) As Integer
        IndexOfStep = IndexOf(S)
        If IsNothing(IndexOfStep) Then
            IndexOfStep = -1
        End If
    End Function
    Public Function CountStepsList() As Integer
        CountStepsList = Count
    End Function
    Public Sub RemoveSelectedElements()
        Dim i, j As Integer
        j = 0
        For i = 0 To Count - 1
            'Cancella le azioni della fase
            If TypeOf (Me(i - j)) Is GraphicalStep Then
                CType(Me(i - j), GraphicalStep).RemoveSelectedActions()
            End If
            'Cancella la fase se selezionata
            If Me(i - j).ReadSelected = True Then
                Me(i - j).DisposeMe()
                RemoveAt(i - j)
                j = j + 1
            End If
        Next i
    End Sub
    Public Function ReadSelected() As GraphicalStepsList
        ReadSelected = New GraphicalStepsList
        For Each S As BaseGraphicalStep In Me
            If S.ReadSelected = True Then
                ReadSelected.AddExistingStep(S)
            End If
        Next S
    End Function
    Public Function ReadTopSelectedStepList() As GraphicalStepsList
        ReadTopSelectedStepList = New GraphicalStepsList
        For Each S As BaseGraphicalStep In Me
            If S.ReadTopRectSelected = True Then
                ReadTopSelectedStepList.AddExistingStep(S)
            End If
        Next S
    End Function
    Public Function ReadBottomSelectedStepList() As GraphicalStepsList
        ReadBottomSelectedStepList = New GraphicalStepsList
        For Each S As BaseGraphicalStep In Me
            If S.ReadBottomRectSelected = True Then
                ReadBottomSelectedStepList.AddExistingStep(S)
            End If
        Next S
    End Function
    Public Function ReadSelectedStepsActionsList() As ArrayList
        ReadSelectedStepsActionsList = New ArrayList
        For Each S As Object In Me
            If S.GetType.Name = "GraphicalStep" Then
                If S.ReadSelectedActionsList.Count > 0 Then
                    ReadSelectedStepsActionsList.AddRange(S.ReadSelectedActionsList)
                End If
            End If
        Next S
    End Function
    Public Function FindAndSelectAction(ByVal x As Integer, ByVal y As Integer) As Boolean
        'Seleziona solo la prima azione che trova
        For Each S As Object In Me
            If S.GetType.Name = "GraphicalStep" Then
                FindAndSelectAction = S.FindAndSelectAction(x, y)
                If FindAndSelectAction Then
                    Exit Function
                End If
            End If
        Next S
    End Function
    Public Function FindAction(ByVal x As Integer, ByVal y As Integer) As Boolean
        For Each S As Object In Me
            If S.GetType.Name = "GraphicalStep" Then
                FindAction = S.FindAction(x, y)
                If FindAction Then
                    Exit Function
                End If
            End If
        Next S
    End Function
    Public Function ReadIfActionSelected(ByVal x As Integer, ByVal y As Integer) As Boolean
        For Each S As Object In Me
            If S.GetType.Name = "GraphicalStep" Then
                ReadIfActionSelected = S.ReadIfActionSelected(x, y)
                If ReadIfActionSelected Then
                    Exit Function
                End If
            End If
        Next S
    End Function
    Public Function ReadIfTransitionSelected(ByVal x As Integer, ByVal y As Integer) As Boolean
        'Trova la prima fase in x,y
        For Each T As BaseGraphicalStep In Me
            If T.MyAreaIsHere(x, y) And T.ReadSelected Then
                ReadIfTransitionSelected = True
                Exit Function
            End If
        Next T
    End Function
    Public Sub FindAndSelectActions(ByVal Rect As Drawing.Rectangle)
        'Seleziona tutte le azioni che trova
        For Each S As Object In Me
            If S.GetType.Name = "GraphicalStep" Then
                S.FindAndSelectActions(Rect)
            End If
        Next S
    End Sub
    Public Function FindAndSelectSmallRectangleStep(ByVal x As Integer, ByVal y As Integer) As Boolean
        For Each S As BaseGraphicalStep In Me
            'Controlla il ret sup
            If S.CeIlMioRetSup(x, y) Then
                'Inverte il valore selezionato del ret sup
                S.SetTopRectSelected(Not S.ReadTopRectSelected)
                FindAndSelectSmallRectangleStep = True
                Exit Function
            End If
            'Controlla il valore selezionato
            If S.CeIlMioRetInf(x, y) = True Then
                'Inverte il Value Selected del ret sup
                S.SetBottomRectSelected(Not S.ReadBottomRectSelected)
                FindAndSelectSmallRectangleStep = True
                Exit Function
            End If
        Next S
    End Function
    Public Sub DeSelectAll()
        For Each S As Object In Me
            S.SetSelected(False)
            S.SetBottomRectSelected(False)
            S.SetTopRectSelected(False)
            If S.GetType.Name = "GraphicalStep" Then
                S.DeSelectActions()
            End If
        Next S
    End Sub
    Public Sub SelezionaAll()
        For Each S As Object In Me
            S.SetSelected(True)
        Next
    End Sub
    Public Sub DrawArea(ByVal Rect As Drawing.Rectangle, ByVal DrawSmallRectangels As Boolean)
        For Each S As BaseGraphicalStep In Me
            'La disegna se si interseca con il rectanglolo da disegnare
            If S.ReadArea.IntersectsWith(Rect) Then
                S.Draw(DrawSmallRectangels)
            End If
        Next S
    End Sub
    Public Sub DrawSelection(ByVal DrawSmallRectangles As Boolean)
        For Each S As BaseGraphicalStep In Me
            'Disegna le fasi selezionate
            If S.ReadSelected Then
                S.Draw(DrawSmallRectangles)
            End If
        Next S
    End Sub
    Public Sub MoveSelection(ByVal dx As Integer, ByVal dy As Integer)
        'Muove le fasi selezionate
        For Each S As BaseGraphicalStep In Me
            If S.ReadSelected Then
                S.Move(dx, dy)
            End If
        Next S
    End Sub
    Public Sub AddActionToSelectedSteps(ByVal Nom As String, ByVal Qual As String, ByRef Var As BaseVariable, ByRef VarInd As BaseVariable, ByVal T As TimeSpan, ByRef RefSfc As Sfc, ByVal StepsToForces As GraphicalStepsList, ByVal ArithExp As String)
        For Each S As Object In Me
            If S.ReadSelected And S.GetType.Name = "GraphicalStep" Then
                S.AddAndDrawAction(Nom, Qual, Var, VarInd, T, RefSfc, StepsToForces, ArithExp)
            End If
        Next S
    End Sub
    Public Function SetInitialSteps() As Boolean
        'Fa lo switch della variabile Initial per ogni fase selezionata In Mea
        'Restituisce True se ha trovato almeno una fase selezionata
        For Each S As BaseGraphicalStep In Me
            If S.GetType.Name = "GraphicalStep" And S.ReadSelected Then
                SetInitialSteps = True
                S.SetInitial(Not S.ReadInitial)
            End If
        Next S
    End Function
    Public Function SetFinalSteps() As Boolean
        'Fa lo switch della variabile Final per ogni fase selezionata In Mea
        'Restituisce True se ha trovato almeno una fase selezionata
        For Each S As BaseGraphicalStep In Me
            If S.GetType.Name = "GraphicalStep" And S.ReadSelected Then
                SetFinalSteps = True
                S.SetFinal(Not S.ReadFinal)
            End If
        Next S
    End Function
    Public Sub CancelSelection(ByVal CancelSmallRectangles As Boolean)
        For Each S As BaseGraphicalStep In Me
            If S.ReadSelected Then
                S.Cancel(CancelSmallRectangles)
            End If
        Next S
    End Sub
    Public Function ControllaSelectionFuoriArea(ByVal R As Drawing.Rectangle, ByRef FuoriX As Boolean, ByRef FuoriY As Boolean) As Boolean
        'Controlla che l'area delle Steps selezionate....
        '....non sia fuori da R (se è fuori restituisce True)
        For Each S As BaseGraphicalStep In Me
            If S.ReadSelected Then
                ControllaSelectionFuoriArea = Outside(R, S.ReadArea, FuoriX, FuoriY)
                If ControllaSelectionFuoriArea Then
                    Exit For
                End If
            End If
        Next S
    End Function
    Public Function CountSelected() As Integer
        CountSelected = 0
        For Each S As BaseGraphicalStep In Me
            If S.ReadSelected Then
                CountSelected = CountSelected + 1
            End If
        Next S
    End Function
    Public Function CountSelectedActions() As Integer
        CountSelectedActions = 0
        For Each S As Object In Me
            If S.GetType.Name = "GraphicalStep" Then
                CountSelectedActions = CountSelectedActions + S.CountSelectedActions()
            End If
        Next S
    End Function
    Public Function ReadStepSelected() As BaseGraphicalStep
        For Each S As BaseGraphicalStep In Me
            If S.ReadSelected Then
                Return S
            End If
        Next S
        Return Nothing
    End Function
    Public Function ReadSelectedAction() As GraphicalAction
        ReadSelectedAction = Nothing
        For Each S As Object In Me
            If S.GetType.Name = "GraphicalStep" Then
                ReadSelectedAction = S.ReadSelectedAction
                If Not IsNothing(ReadSelectedAction) Then
                    Exit Function
                End If
            End If
        Next S
    End Function
    Public Sub ExecuteInit()
        For Each S As BaseGraphicalStep In Me
            If S.ReadInitial Then
                S.PreActive()
            End If
        Next
    End Sub
    Public Sub DeactiveAllSteps()
        'Disattiva tutte le fasi
        For Each S As BaseGraphicalStep In Me
            S.Disactive()
        Next S
    End Sub
    Public Sub ActiveSteps(ByRef StepsList As GraphicalStepsList)
        Dim i As Integer
        For Each S As BaseGraphicalStep In StepsList
            i = IndexOf(S)
            Me(i).Active()
        Next S
    End Sub
    Public Function ActivePreactivedSteps() As Boolean
        'Esegue le azioni non impulsive delle fasi ....'
        '.... da attivare se non erano preattive e le attiva

        'Se è viene attivata almeno una fase finale restituisce True

        For Each S As BaseGraphicalStep In Me
            'Imposta la Step Active se era preActive
            If S.ReadPreActive Then
                S.Active()
                If S.ReadFinal Then
                    ActivePreactivedSteps = True
                End If
            End If
        Next S
    End Function
    Public Sub PreActiveSteps()
        For Each S As BaseGraphicalStep In Me
            'Imposta la Step preActive
            S.PreActive()
        Next S
    End Sub
    Public Sub ResetState()
        DeactiveAllSteps()
        For Each S As BaseGraphicalStep In Me
            If S.GetType.Name = "GraphicalMacroStep" Then
                Dim MS As GraphicalMacroStep = S
                MS.ResetBodyState()
            Else
                If S.GetType.Name = "GraphicalStep" Then
                    Dim St As GraphicalStep = S
                    St.ResetActions()
                End If
            End If
        Next S
    End Sub
    Public Function ReadIfAllStepsActive() As Boolean
        'Le macrofasi devono essere attive (non basta preattive come le fasi)
        ReadIfAllStepsActive = True
        For Each S As BaseGraphicalStep In Me
            If S.GetType.Name = "GraphicalMacroStep" Then
                'E' una macrofase
                If Not S.ReadActive Then
                    ReadIfAllStepsActive = False
                    Exit For
                End If
            Else
                'E' una fase
                If S.GetType.Name = "GraphicalStep" Then
                    If Not S.ReadActive And Not S.ReadPreActive Then
                        ReadIfAllStepsActive = False
                        Exit For
                    End If
                End If
            End If
        Next S
    End Function
    Public Sub ExecuteMacroStepScanCycle()
        'Cilo di scansione per le macrofasi
        For Each S As Object In Me
            If S.GetType.Name = "GraphicalMacroStep" Then
                S.ExecuteScanCycle()
            End If
        Next S
    End Sub
    Public Sub ExecuteAction()
        Dim GS As GraphicalStep
        For Each S As BaseGraphicalStep In Me
            If S.GetType.Name = "GraphicalStep" Then
                GS = S
                GS.ExecuteActions()
            End If
        Next S
    End Sub

    ' Ritorna una lista delle fasi inserite in questa lista in forma stringa
    Public Function ListSteps() As String
        Dim ret As String = ""
        For Each S As BaseGraphicalStep In Me
            ret += S.Name + ", "
        Next
        ret = ret.TrimEnd(" ").TrimEnd(",")
        Return ret
    End Function
End Class

