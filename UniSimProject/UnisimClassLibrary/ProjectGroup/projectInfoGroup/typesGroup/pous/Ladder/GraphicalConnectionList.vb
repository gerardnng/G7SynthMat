Imports System.IO
Imports System.Xml
Imports System.Collections.Generic
Public Class GraphicalConnectionList
    Inherits List(Of GraphicalConnection)
    Implements IXMLExportable

    ' Era Protected MySfc As Ladder :-(
    Protected MyLadder As Ladder  'Ladder cui appartiene
    Dim NotSelectionColor As Drawing.Color
    Dim SelectionColor As Drawing.Color
    Dim CoeffSnap As Integer
    Dim Carattere As Drawing.Font
    Dim BackColor As Drawing.Color
    Dim TextColor As Drawing.Color
    Dim ColorConditionTrue As Drawing.Color
    Dim ColorConditionFalse As Drawing.Color
    Dim ColorActive As Drawing.Color
    Dim ColorPreActive As Drawing.Color
    Dim ColorDeactive As Drawing.Color
    Private GraphToDraw As Drawing.Graphics
    Private DrawState As Boolean
    Private Dimension As Integer

    Sub New(ByRef RefSfc As Ladder, ByVal CSnap As Integer, ByVal ColSfondo As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal ColTesto As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColConditionTrue As Drawing.Color, ByVal ColConditionFalse As Drawing.Color, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, ByRef Graph As Drawing.Graphics, ByVal Dimen As Integer)
        MyBase.New()
        MyLadder = RefSfc
        NotSelectionColor = NotSelectionCol
        SelectionColor = SelectionCol
        CoeffSnap = CSnap
        Carattere = Car
        BackColor = ColSfondo
        TextColor = ColTesto
        ColorActive = ColActive
        ColorPreActive = ColPreActive
        ColorDeactive = Drawing.Color.Brown
        ColorConditionTrue = ColConditionTrue
        ColorConditionFalse = ColConditionFalse
        GraphToDraw = Graph
        Dimension = Dimen
    End Sub
    Public Sub New(ByRef R As BinaryReader, ByRef RefSfc As Ladder, ByRef m_GraphicalStepsList As GraphicalContactList, ByVal CSnap As Integer, ByVal ColSfondo As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal ColTesto As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColConditionTrue As Drawing.Color, ByVal ColConditionFalse As Drawing.Color, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, ByRef Graph As Drawing.Graphics)
        MyLadder = RefSfc
        NotSelectionColor = NotSelectionCol
        SelectionColor = SelectionCol
        CoeffSnap = CSnap
        Carattere = Car
        BackColor = ColSfondo
        TextColor = ColTesto
        ColorActive = ColActive
        ColorPreActive = ColPreActive
        ColorDeactive = ColDeactive
        ColorConditionTrue = ColConditionTrue
        ColorConditionFalse = ColConditionFalse
        GraphToDraw = Graph
        Dim i As Integer
        Dim NumTransizions As Integer = R.ReadInt32
        For i = 1 To NumTransizions
            'Dim T As New GraphicalTransition
            'Add(T)
        Next i
    End Sub
    Public Sub New()
        MyBase.new()
    End Sub

    Public Sub AddAndDrawConnection(ByVal Number As Integer, ByVal Name As String, _
        ByVal Documentation As String, ByRef LPreviousSteps As GraphicalContactList, _
        ByRef LNextSteps As GraphicalContactList, ByRef RefCondition As BooleanExpression, _
        ByVal FlagShowDetails As Boolean, ByVal DrawState As Boolean)
        For Each srcContact As BaseGraphicalContact In LPreviousSteps
            For Each dstContact As BaseGraphicalContact In LNextSteps
                Dim srcContacts As New GraphicalContactList(srcContact)
                Dim dstContacts As New GraphicalContactList(dstContact)
                Dim T As New GraphicalConnection(Number, Name, Documentation, srcContacts, dstContacts, _
                    RefCondition, MyLadder, -1, CoeffSnap, BackColor, SelectionColor, NotSelectionColor, _
                    TextColor, Carattere, ColorConditionTrue, ColorConditionFalse, ColorActive, _
                    ColorPreActive, ColorDeactive, GraphToDraw, DrawState, Dimension)
                Add(T)
                T.Draw(FlagShowDetails)
            Next
        Next
    End Sub

    

    Public Sub SetGraphToDraw(ByRef Graph As Drawing.Graphics)
        GraphToDraw = Graph
        For Each T As GraphicalConnection In Me
            T.SetGraphToDraw(GraphToDraw)
        Next T
    End Sub
    Public Sub DrawArea(ByVal Rect As Drawing.Rectangle, ByVal ShowDetails As Boolean)
        For Each T As GraphicalConnection In Me
            'La Draw se si interseca con il Rectangle da Drawre
            If T.ReadArea.IntersectsWith(Rect) Then
                T.Draw(ShowDetails)
            End If
        Next T
    End Sub
    Public Sub DeSelectAll()
        For Each T As GraphicalConnection In Me
            T.SetSelected(False)
        Next T
    End Sub
    Public Sub CancelSelection(ByVal FlagShowDetails As Boolean)
        'Cancell le transizioni selezionate
        For Each T As GraphicalConnection In Me
            If T.ReadSelected Then
                T.Cancel(FlagShowDetails)
            End If
        Next T
    End Sub
    Public Function FindConnection(ByVal obj As BaseGraphicalContact) As Integer
        For i As Integer = 0 To Me.Count - 1
            Dim C As GraphicalConnection = Me(i)
            If C.ReadPreviousGraphicalStepsList.IndexOf(obj) >= 0 OrElse _
                C.ReadNextsGraphicalStepsList.IndexOf(obj) >= 0 Then Return i
        Next
        Return -1
    End Function
    Public Function FindConnection(ByVal x As Integer, ByVal y As Integer) As Boolean
        'Find la prima Transition in x,y
        For Each T As GraphicalConnection In Me
            If T.MyAreaIsHere(x, y) Then
                FindConnection = True
                Exit Function
            End If
        Next T
    End Function
    Public Function FindAndSelectConnection(ByVal x As Integer, ByVal y As Integer) As Boolean
        'Seleziona solo la prima cinnessione che trova
        For Each T As GraphicalConnection In Me
            If T.MyAreaIsHere(x, y) = True Then
                FindAndSelectConnection = True
                T.SetSelected(True)
                Exit Function
            End If
        Next T
    End Function

    Public Sub RemoveSelectedElements()
        Dim i, j As Integer
        j = 0
        For i = 0 To Count - 1
            If Me(i - j).ReadSelected = True Then
                Me(i - j).DisposeMe()
                RemoveAt(i - j)
                j = j + 1
            End If
        Next i
    End Sub

    Public Function ReadSelectedConnectionList() As GraphicalConnectionList
        ReadSelectedConnectionList = New GraphicalConnectionList
        For Each T As GraphicalConnection In Me
            If T.ReadSelected = True Then
                ReadSelectedConnectionList.Add(T)
            End If
        Next T
    End Function


    Public Sub CancelTransitionWithSelectedSteps(ByVal FlagShowDetails As Boolean)
        'Cancell le transizioni che hanno Steps selezionate
        For Each T As GraphicalConnection In Me
            If T.ReadPreviousGraphicalStepsList.ReadSelected.CountStepsList > 0 Or T.ReadNextsGraphicalStepsList.ReadSelected.CountStepsList > 0 Then
                T.Cancel(FlagShowDetails)
            End If
        Next T
    End Sub

    Public Function IsSelectionOutside(ByVal R As Drawing.Rectangle, ByRef FuoriX As Boolean, ByRef FuoriY As Boolean) As Boolean
        'Controlla che l'area delle transizioni selezionate....
        '....non sia fuori da R (se è fuori restituisce True)
        Dim Area As Drawing.Rectangle
        For Each T As GraphicalConnection In Me
            If T.ReadSelected Then
                Area = T.ReadArea
                If Area.X <= R.X Or Area.X + Area.Width >= R.X + R.Width Then
                    IsSelectionOutside = True
                    FuoriX = True
                End If
                If Area.Y <= R.Y Or Area.Y + Area.Height >= R.Y + R.Height Then
                    IsSelectionOutside = True
                    FuoriY = True
                End If
                If IsSelectionOutside Then
                    Exit For
                End If
            End If
        Next T
    End Function
    Public Function ReadIfConnectionSelected(ByVal x As Integer, ByVal y As Integer) As Boolean
        'Find la prima Step in x,y
        For Each T As GraphicalConnection In Me
            If T.MyAreaIsHere(x, y) And T.ReadSelected Then
                ReadIfConnectionSelected = True
                Exit Function
            End If
        Next T
    End Function
    Public Sub FindAndSelectConnection(ByVal Rect As Drawing.Rectangle)
        ' Un nuovo algoritmo basato sulla intersezione di rettangoli
        ' evitiamo i rettangoli con una delle dimensioni <= 0
        Rect.Width = Math.Max(Rect.Width, 1)
        Rect.Height = Math.Max(Rect.Height, 1)
        For Each T As GraphicalConnection In Me
            ' breakpoint cond: T.ReadNextsGraphicalStepsList()(0).Name = "S1" And T.ReadPreviousGraphicalStepsList()(0).Name = "C2"
            ' T.ReadNextsGraphicalStepsList()(0).Name = "S2" And T.ReadPreviousGraphicalStepsList()(0).Name = "C3"
            For Each R As Drawing.Rectangle In T.GetDrawnRectangles()
                R.Intersect(Rect)
                If Not (R.IsEmpty) Then T.SetSelected(True)
            Next
        Next
    End Sub

    'Public Sub FindAndSelectConnection(ByVal Rect As Drawing.Rectangle)
    '    'Seleziona tutte le transizioni che trova
    '    Dim x, y, Passo As Integer
    '    For Each T As GraphicalConnection In Me
    '        Passo = CInt(T.ReadDimension() / 5)
    '        For x = Rect.X To Rect.X + Rect.Width Step Passo
    '            For y = Rect.Y To Rect.Y + Rect.Height Step Passo
    '                If T.MyAreaIsHere(x, y) = True Then
    '                    T.SetSelected(True)
    '                End If
    '            Next y
    '        Next x
    '        'Ricerca sul bordo verticale destro
    '        x = Rect.X + Rect.Width
    '        For y = Rect.Y To Rect.Y + Rect.Height Step Passo
    '            If T.MyAreaIsHere(x, y) = True Then
    '                T.SetSelected(True)
    '            End If
    '        Next y
    '        'Ricerca sul bordo orizzontale sinistro
    '        y = Rect.Y + Rect.Height
    '        For x = Rect.X To Rect.X + Rect.Width Step Passo
    '            If T.MyAreaIsHere(x, y) = True Then
    '                T.SetSelected(True)
    '            End If
    '        Next x
    '        'Ricerca sull'angolo inferiore destro 
    '        x = Rect.X + Rect.Width
    '        y = Rect.Y + Rect.Height
    '        If T.MyAreaIsHere(x, y) = True Then
    '            T.SetSelected(True)
    '        End If
    '    Next T
    'End Sub

    Public Sub MoveSelection(ByVal dx As Integer)
        'Move le transizioni selezionate
        For Each T As GraphicalConnection In Me
            If T.ReadSelected Then
                T.Move(dx)
            End If
        Next T
    End Sub

    Public Sub MoveSelectedTransitions(ByVal OffsetX As Integer)
        For Each T As GraphicalConnection In Me
            If T.ReadSelected Then
                Dim x As Integer
                x = T.ReadPosition
                x = x + OffsetX
                T.SetPosition(x)
            End If
        Next T
    End Sub

    Public Function CountSelected() As Integer
        CountSelected = 0
        For Each T As GraphicalConnection In Me
            If T.ReadSelected Then
                CountSelected = CountSelected + 1
            End If
        Next
    End Function

    Public Function ControllaPresenzaStepsInTransizioni(ByVal LSteps As GraphicalContactList) As Boolean
        For Each T As GraphicalConnection In Me
            For Each F As BaseGraphicalContact In LSteps
                If T.ReadPreviousGraphicalStepsList.IndexOfStep(F) >= 0 _
                    Then Return True
                If T.ReadNextsGraphicalStepsList.IndexOfStep(F) >= 0 _
                    Then Return True
            Next F
        Next T
        Return False
    End Function

    Public Function ReadConnectionSelected() As GraphicalConnection
        For Each T As GraphicalConnection In Me
            If T.ReadSelected Then
                Return T
            End If
        Next T
        Return Nothing
    End Function
    Public Sub ClearXMLConnectionsLists()
        'Svuota le liste delle connessioni delle tansizioni per l'esportazione xml
        For Each T As GraphicalConnection In Me
            T.ReadXmlPreviousConnectionsList.Clear()
        Next T
    End Sub
    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter) Implements IXMLExportable.xmlExport
        For Each T As GraphicalConnection In Me
            T.xmlExport(RefXMLProjectWriter)
        Next T
    End Sub

    Public Function CreateInstance(ByRef NewLadder As Ladder, ByRef StepsList As GraphicalContactList) As GraphicalConnectionList
        CreateInstance = New GraphicalConnectionList(NewLadder, CoeffSnap, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ColorConditionTrue, ColorConditionFalse, ColorActive, ColorDeactive, ColorPreActive, Nothing, Dimension)
        For Each T As GraphicalConnection In Me
            CreateInstance.Add(T.CreateInstance(NewLadder, StepsList))
        Next T
    End Function

    ' Tutte le connessioni che hanno per primo terminale un left rail
    ' sono i pezzi iniziali dei rung
    <Obsolete("Usare GetLeftRails() in GraphicalContactList")> _
    Public Function GetLeftRails() As GraphicalContactList        
        Return MyLadder.GraphicalContactList.GetLeftRails()
    End Function

    ' Tutte le connessioni che hanno per secondo terminale un right rail
    ' sono i pezzi finali dei rung
    <Obsolete("Usare GetLeftRails() in GraphicalContactList")> _
    Public Function GetRightRails() As GraphicalContactList
        Return MyLadder.GraphicalContactList.GetRightRails()
    End Function

    Public Function MakeConnectionPointOutForObject(ByVal obj As BaseGraphicalContact) As ConnectionPointOut
        Dim lst As New ArrayList
        For Each S As GraphicalConnection In Me
            If S.ReadPreviousGraphicalStepsList.IndexOf(obj) >= 0 Then
                lst.AddRange(S.ReadNextsGraphicalStepsList)
            End If
        Next
        Return New ConnectionPointOut(lst)
    End Function

    ' Trova i contatti alla destra di obj
    Public Function FindRightOf(ByVal obj As BaseGraphicalContact) As GraphicalContactList
        Dim ret As New GraphicalContactList()
        For Each S As GraphicalConnection In Me
            Dim left As GraphicalContactList = S.ReadPreviousGraphicalStepsList
            If left.IndexOf(obj) >= 0 Then
                ret.AddRange(S.ReadNextsGraphicalStepsList())
            End If
        Next
        Return ret
    End Function

    ' Trova i contatti alla sinistra di obj
    Public Function FindLeftOf(ByVal obj As BaseGraphicalContact) As GraphicalContactList
        Dim ret As New GraphicalContactList()
        For Each S As GraphicalConnection In Me
            Dim right As GraphicalContactList = S.ReadNextsGraphicalStepsList
            If right.IndexOf(obj) >= 0 Then
                ret.AddRange(S.ReadPreviousGraphicalStepsList)
            End If
        Next
        Return ret
    End Function

    ' Trova tutte le connessioni in cui (alla destra o alla sinistra) c'è obj
    Public Function FindConnectionsForObject(ByVal obj As BaseGraphicalContact) As GraphicalConnectionList
        Dim ret As New GraphicalConnectionList()
        For Each S As GraphicalConnection In Me
            If S.ReadPreviousGraphicalStepsList().IndexOf(obj) >= 0 _
                OrElse S.ReadNextsGraphicalStepsList.IndexOf(obj) >= 0 Then _
                    ret.Add(S)
        Next
        Return ret
    End Function

    ' Crea espressioni in maniera ricorsiva usando lo schema LtR (cioè partendo dai
    ' left rails e andando verso i right rails)
    Private Function MakeExpressionPieceLtR(ByVal root As BaseGraphicalContact, _
        ByRef rng As Rung) As String
        Dim nextOnes As GraphicalContactList = FindRightOf(root)
        Dim myself As String = ""
        If TypeOf (root) Is GraphicalLeftRail Or TypeOf (root) Is GraphicalRightRail Then
            myself = ""
        Else
            If root.IsContact Then
                myself = root.GetTerm + "*"
            ElseIf root.IsCoil Then
                rng.AddCoil(root)
            End If
        End If
        ' Se ci sono nodi dopo questo...e non siamo in una bobina...
        If Not IsNothing(nextOnes) AndAlso Not (root.IsCoil) Then
            ' esegui la scansione del resto del rung
            If nextOnes.Count = 1 Then
                myself = myself + MakeExpressionPieceLtR(nextOnes(0), rng)
            Else
                ' Verifica se il resto del rung ha qualcosa da aggiungere
                ' esempio in cui non lo avrà:
                '                  ---( )---
                ' -| |-------------
                '                  ---( )---
                Dim inneroutput As String = ""
                For Each n As BaseGraphicalContact In nextOnes
                    inneroutput += MakeExpressionPieceLtR(n, rng)
                    inneroutput = inneroutput.TrimEnd("*") + "+"
                Next
                ' Una stringa di nextOnes.Count segni +
                Dim comparison As New String("+", nextOnes.Count)
                ' Se non ci sono solo i segni più nella stringa inneroutput
                ' cioè se almeno una MakeExpressionPiece() ha aggiunto qualcosa
                If inneroutput <> comparison Then _
                    myself = myself + "(" + inneroutput.TrimEnd("+") + ")"
            End If
        End If
        Return myself
    End Function

    ' Questo codice è stato adattato dall'FBD
    Private Sub BuildTreeNode(ByVal graphNode As GraphicalContact, _
        ByVal treeNode As LDTreeNode)
        Dim previousConnections As GraphicalFBDConnectionsList = _
            graphNode.FindIncomingFBDConnections()
        For Each previousConnection As GraphicalFBDConnection In previousConnections
            Dim srcObject As ILDConnectable = previousConnection.SourceObject
            Dim newNode As LDTreeNode
            If TypeOf (srcObject) Is GraphicalVariable Then
                newNode = New VariableBoundLDTreeNode(CType(srcObject, GraphicalVariable).BoundVariable)
            Else
                Dim newObject As GraphicalContact = TryCast(srcObject, GraphicalContact)
                ' siamo alla fine?
                If newObject Is Nothing OrElse Not (newObject.IsBlock) Then Continue For
                newNode = LDBlocksFactory.CreateBlock(newObject.Qualy, MyLadder.PousList)
            End If
            treeNode.AddNode(newNode)
            If TypeOf (newNode) Is BlockLDTreeNode Then _
                BuildTreeNode(srcObject, newNode)
        Next
    End Sub

    Private Function BuildTree(ByVal root As GraphicalContact) As LDTree
        Dim GV As GraphicalVariable
        Dim outgoingConnections As GraphicalFBDConnectionsList = root.FindOutgoingFBDConnections()
        If outgoingConnections.Count = 0 Then Return Nothing
        GV = TryCast(outgoingConnections(0).DestinationObject, GraphicalVariable)
        ' non c'è un'uscita???
        If GV Is Nothing Then Return Nothing
        ' all'uscita c'è una variable d'ingresso?!?!?!...
        If Not (GV.VariableType = GraphicalVariableType.Output) Then Return Nothing
        Dim tree As New LDTree(GV.BoundVariable)
        tree.SetRootNode(LDBlocksFactory.CreateBlock(root.Qualy, MyLadder.PousList))
        BuildTreeNode(root, tree.GetRootNode)
        Return tree
    End Function

    ' Crea espressioni in maniera ricorsiva usando lo schema RtL (cioè partendo dai
    ' right rails e andando verso i left rails)
    Private Function MakeExpressionPieceRtL(ByVal root As BaseGraphicalContact, _
        ByRef rng As Rung) As String
        Dim prevOnes As GraphicalContactList = FindLeftOf(root)
        Dim myself As String = ""
        ' Se siamo in una pista di alimentazione, ignora...
        If TypeOf (root) Is GraphicalLeftRail Or TypeOf (root) Is GraphicalRightRail Then
            myself = ""
        Else
            ' Per i contatti, calcola il termine e inseriscilo
            If root.IsContact Then
                myself = "*" + root.GetTerm()
                ' per le bobine, aggiungile all'elenco delle uscite
            ElseIf root.IsCoil Then
                rng.AddCoil(root)
                ' per i blocchi, crea l'albero FBD associato
            ElseIf root.IsBlock Then
                Dim ldTree As LDTree = BuildTree(root)
                ' Se non c'è l'albero FBD non aggiungerlo
                ' Per come è congegnato il metodo BuildTree() verranno ignorati i blocchi che fanno parte
                ' di altri alberi FBD (in particolare quelli che non hanno una variabile come uscita) e
                ' quindi il loro attraversamento non influirà sul rung
                If ldTree IsNot Nothing Then rng.AddFBDTree(ldTree)
            End If
        End If
        ' Se ci sono nodi prima di questo...
        If Not IsNothing(prevOnes) Then
            ' se ce n'è uno solo bisogna farne la And (il segno "*" è stato inserito
            ' prima se necessario)
            If prevOnes.Count = 1 Then
                myself = MakeExpressionPieceRtL(prevOnes(0), rng) + myself
            Else
                ' Ci sono più nodi precedenti, bisogna farne una Or e poi metterla
                ' in And (come prima, il "*" se necessario è già presente)
                Dim inneroutput As String = ""
                For Each n As BaseGraphicalContact In prevOnes
                    inneroutput = MakeExpressionPieceRtL(n, rng) + inneroutput
                    inneroutput = "+" + inneroutput.TrimStart("*")
                Next
                ' Una stringa di nextOnes.Count segni +
                Dim comparison As New String("+", prevOnes.Count)
                ' Se non ci sono solo i segni più nella stringa inneroutput
                ' cioè se almeno una MakeExpressionPiece() ha aggiunto qualcosa
                If inneroutput <> comparison Then
                    ' aggiungi la Or all'espressione totale
                    myself = "(" + inneroutput.TrimStart("+") + ")" + myself
                End If
            End If
        End If
        Return myself
    End Function

    ' Crea l'espressione equivalente al rung che inizia in root
    ' N.B. lo schema LtR è il più semplice modo di scansionare un diagramma ladder,
    ' e corrisponde all'ordine di esecuzione e all'ordine naturale di lettura per
    ' un europeo. Nonstante ciò lo schema LtR non sempre funziona correttamente
    ' ad esempio fallisce in
    ' |-----|a|-----
    '               ----(c)----|
    ' |-----|b|-----
    ' in quanto produce le due espressioni
    ' c = a e c = b, che sono equivalenti a c = a*b invece che a c = a | b
    <Obsolete("Lo schema LtR non è in grado di produrre sempre Rung corretti ed inoltre non è aggiornato " _
        & "per gestire i blocchi. Usare lo schema RtL")> _
    Public Function BuildExpressionLtR(ByVal root As GraphicalLeftRail) As Rung
        Dim ret As New Rung()
        Dim expr As String = MakeExpressionPieceLtR(root, ret)
        expr = expr.TrimEnd("*")
        ret.Expression = expr
        Return ret
    End Function

    ' Crea l'espressione equivalente al rung che termina in root
    ' Lo schema RtL è meno naturale alla lettura (esclusi coloro che parlano arabo)
    ' e la routine MakeExpressionPieceRtL deve adottare alcuni artifici per garantire
    ' che il flusso di esecuzione sia da sinistra verso destra in ogni caso
    ' Ad oggi non si conoscono esempi che l'algoritmo RtL non scansioni correttamente
    Public Function BuildExpressionRtL(ByVal root As GraphicalRightRail) As Rung
        Dim ret As New Rung()
        Dim expr As String = MakeExpressionPieceRtL(root, ret)
        expr = expr.TrimStart("*")
        expr = Globals.TrimDuplicates(expr, "+"c)
        ret.Expression = expr
        Return ret
    End Function

End Class

