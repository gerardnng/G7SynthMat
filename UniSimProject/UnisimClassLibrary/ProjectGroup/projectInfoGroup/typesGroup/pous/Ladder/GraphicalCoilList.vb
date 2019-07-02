Imports System.IO
Imports System.Drawing

Public Class GraphicalCoilList
    Inherits ArrayList

    Private MyLadder As Ladder
    Private Dimension As Integer
    Private SelectionColor As Drawing.Color
    Private NotSelectionColor As Drawing.Color
    Private TextColor As Drawing.Color
    Private Carattere As Drawing.Font
    Private BackColor As Drawing.Color
    Private ActiveColor As Drawing.Color
    Private PreActiveColor As Drawing.Color
    Private DeactiveColor As Drawing.Color
    Private GraphToDraw As Drawing.Graphics
    Private DrawState As Boolean

    Sub New(ByRef RefLadder As Ladder, ByVal BackCol As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal ColTesto As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, ByRef Graph As Drawing.Graphics, ByVal Dimen As Integer)
        MyBase.new()
        MyLadder = RefLadder
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
    End Sub
    Public Sub New(ByRef R As BinaryReader, ByVal BackCol As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal ColTesto As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, ByRef Graph As Drawing.Graphics)
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
        Dim i As Integer
        Dim NumSteps As Integer = R.ReadInt32
        ' For i = 1 To NumSteps
        'Dim S As New GraphicalStep(R, BackCol, SelectionCol, NotSelectionCol, ColTesto, Car, ColActive, ColDeactive, ColPreActive)
        'Add(S)
        ' Next i
    End Sub
    Public Sub New()
        MyBase.new()
    End Sub

    Public Sub AddAndDrawCoil(ByVal StepNumber As Integer, ByVal StepName As String, ByVal StepDocumentation As String, ByVal Position As Drawing.Point, ByVal DrawState As Boolean)
        Dim S As New GraphicalCoil1(StepNumber, StepName, StepDocumentation, MyLadder, False, False, Position, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, GraphToDraw, DrawState, Dimension)
        Add(S)
        S.DrawCoil(False)
    End Sub

    Public Sub AddAndDrawNegativeCoil(ByVal StepNumber As Integer, ByVal StepName As String, ByVal StepDocumentation As String, ByVal Position As Drawing.Point, ByVal DrawState As Boolean)
        Dim S As New GraphicalCoil2(StepNumber, StepName, StepDocumentation, MyLadder, False, False, Position, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, GraphToDraw, DrawState, Dimension)
        Add(S)
        S.DrawCoil(False)
    End Sub

    '3
    Public Sub AddAndDrawSetCoil(ByVal StepNumber As Integer, ByVal StepName As String, ByVal StepDocumentation As String, ByVal Position As Drawing.Point, ByVal DrawState As Boolean)
        Dim S As New GraphicalCoil3(StepNumber, StepName, StepDocumentation, MyLadder, False, False, Position, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, GraphToDraw, DrawState, Dimension)
        Add(S)
        S.DrawCoil(False)
    End Sub
    '4
    Public Sub AddAndDrawResetCoil(ByVal StepNumber As Integer, ByVal StepName As String, ByVal StepDocumentation As String, ByVal Position As Drawing.Point, ByVal DrawState As Boolean)
        Dim S As New GraphicalCoil4(StepNumber, StepName, StepDocumentation, MyLadder, False, False, Position, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, GraphToDraw, DrawState, Dimension)
        Add(S)
        S.DrawCoil(False)
    End Sub
    '5
    Public Sub AddAndDrawRetensiveCoil(ByVal StepNumber As Integer, ByVal StepName As String, ByVal StepDocumentation As String, ByVal Position As Drawing.Point, ByVal DrawState As Boolean)
        Dim S As New GraphicalCoil5(StepNumber, StepName, StepDocumentation, MyLadder, False, False, Position, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, GraphToDraw, DrawState, Dimension)
        Add(S)
        S.DrawCoil(False)
    End Sub
    '6
    Public Sub AddAndDrawSetRetensiveCoil(ByVal StepNumber As Integer, ByVal StepName As String, ByVal StepDocumentation As String, ByVal Position As Drawing.Point, ByVal DrawState As Boolean)
        Dim S As New GraphicalCoil6(StepNumber, StepName, StepDocumentation, MyLadder, False, False, Position, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, GraphToDraw, DrawState, Dimension)
        Add(S)
        S.DrawCoil(False)
    End Sub
    '7
    Public Sub AddAndDrawResetRetensiveCoil(ByVal StepNumber As Integer, ByVal StepName As String, ByVal StepDocumentation As String, ByVal Position As Drawing.Point, ByVal DrawState As Boolean)
        Dim S As New GraphicalCoil7(StepNumber, StepName, StepDocumentation, MyLadder, False, False, Position, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, GraphToDraw, DrawState, Dimension)
        Add(S)
        S.DrawCoil(False)
    End Sub
    '8
    Public Sub AddAndDrawPTSCoil(ByVal StepNumber As Integer, ByVal StepName As String, ByVal StepDocumentation As String, ByVal Position As Drawing.Point, ByVal DrawState As Boolean)
        Dim S As New GraphicalCoil8(StepNumber, StepName, StepDocumentation, MyLadder, False, False, Position, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, GraphToDraw, DrawState, Dimension)
        Add(S)
        S.DrawCoil(False)
    End Sub
    '9
    Public Sub AddAndDrawNTSCoil(ByVal StepNumber As Integer, ByVal StepName As String, ByVal StepDocumentation As String, ByVal Position As Drawing.Point, ByVal DrawState As Boolean)
        Dim S As New GraphicalCoil9(StepNumber, StepName, StepDocumentation, MyLadder, False, False, Position, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, GraphToDraw, DrawState, Dimension)
        Add(S)
        S.DrawCoil(False)
    End Sub


    Public Sub SetGraphToDraw(ByRef Graph As Drawing.Graphics)
        GraphToDraw = Graph
        For Each T As BaseGraphicalCoil In Me
            T.SetGraphToDraw(GraphToDraw)
        Next T
    End Sub
    Public Sub AddExistingStep(ByVal S As BaseGraphicalCoil)
        Add(S)
    End Sub
    Public Sub DrawArea(ByVal Rect As Drawing.Rectangle, ByVal DrawSmallRectangels As Boolean)
        'ok questa viene chiamata col paint
        'e ad ogni movimento...

        For Each S As BaseGraphicalCoil In Me
            'La disegna se si interseca con il rectanglolo da disegnare
            If S.ReadArea.IntersectsWith(Rect) Then
                'qui invece viene chiamata solo alla fine una volta 
                S.DrawCoil(DrawSmallRectangels)
            End If
        Next S
    End Sub

    Public Function FindCoil(ByVal x As Integer, ByVal y As Integer) As Boolean
        'Find la prima Transition in x,y
        For Each T As BaseGraphicalCoil In Me
            If T.MyAreaIsHere(x, y) Then
                FindCoil = True
                Exit Function
            End If
        Next T
    End Function

    Public Function CountSelected() As Integer
        CountSelected = 0
        For Each S As BaseGraphicalCoil In Me
            If S.ReadSelected Then
                CountSelected = CountSelected + 1
            End If
        Next S
    End Function

    Public Function ReadIfCoilSelected(ByVal x As Integer, ByVal y As Integer) As Boolean
        'Trova la prima fase in x,y
        For Each S As BaseGraphicalCoil In Me
            If S.MyAreaIsHere(x, y) And S.ReadSelected Then
                ReadIfCoilSelected = True
                Exit Function
            End If
        Next S
    End Function


    Public Sub MoveSelection(ByVal dx, ByVal dy)
        'Muove le fasi selezionate
        For Each S As BaseGraphicalCoil In Me
            If S.ReadSelected Then
                S.Move(dx, dy)
            End If
        Next S
    End Sub

    Public Sub DeSelectAll()
        For Each S As Object In Me
            S.SetSelected(False)
            S.SetBottomRectSelected(False)
            S.SetTopRectSelected(False)
            If S.GetType.Name = "GraphicalCoil" Then
                S.DeSelectActions()
            End If
        Next S
    End Sub


    Public Function FindAndSelectCoil(ByVal x As Integer, ByVal y As Integer) As Boolean
        'seleziona solo il primo contatto che trova
        For Each T As BaseGraphicalCoil In Me
            If T.MyAreaIsHere(x, y) = True Then
                FindAndSelectCoil = True
                T.SetSelected(True)
                Exit Function
            End If
        Next T
    End Function

    Public Sub CancellSelection(ByVal CancellSmallRectangels As Boolean)
        For Each S As BaseGraphicalCoil In Me
            If S.ReadSelected Then
                S.Cancell(CancellSmallRectangels)
            End If
        Next S
    End Sub

    Public Function ReadSelected() As GraphicalCoilList
        ReadSelected = New GraphicalCoilList
        For Each S As BaseGraphicalCoil In Me
            If S.ReadSelected = True Then
                ReadSelected.AddExistingStep(S)
            End If
        Next S
    End Function
    Public Function CountStepsList() As Integer
        CountStepsList = Count
    End Function



    Public Sub RemoveSelectedElements()

        Dim i, j As Integer
        j = 0
        For i = 0 To Count - 1
            'Cancella le azioni della fase
            If Me(i - j).GetType.Name = "GraphicalStep" Then
                Me(i - j).RemoveSelectedActions()
            End If
            'Cancella la fase se selezionata 
            If Me(i - j).ReadSelected = True Then
                Me(i - j).DisposeMe()
                RemoveAt(i - j)
                j = j + 1
            End If
        Next i
    End Sub

    Public Sub FindAndSelectCoil(ByVal Rect As Drawing.Rectangle)
        'Seleziona tutte le fasi che trova
        Dim x, y, Passo As Integer
        For Each S As BaseGraphicalCoil In Me
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

    Public Function FindAndSelectSmallRectangleStep(ByVal x As Integer, ByVal y As Integer) As Boolean

        For Each T As BaseGraphicalCoil In Me
            'Controlla il ret sup
            If T.CeIlMioRetLeft(x, y) Then
                'Inverte il valore selezionato del ret sup
                T.SetTopRectSelected(Not T.ReadTopRectSelected)
                FindAndSelectSmallRectangleStep = True
                Exit Function
            End If
            'Controlla il valore selezionato
            If T.CeIlMioRetRight(x, y) = True Then
                'Inverte il Value Selected del ret sup
                T.SetBottomRectSelected(Not T.ReadBottomRectSelected)
                FindAndSelectSmallRectangleStep = True
                Exit Function
            End If
        Next T
    End Function

    Public Function ReadBottomSelectedContactList() As GraphicalCoilList
        ReadBottomSelectedContactList = New GraphicalCoilList
        For Each S As BaseGraphicalCoil In Me
            If S.ReadBottomRectSelected = True Then
                ReadBottomSelectedContactList.AddExistingStep(S)
            End If
        Next S
    End Function
    Public Function ReadTopSelectedStepList() As GraphicalCoilList
        ReadTopSelectedStepList = New GraphicalCoilList
        For Each S As BaseGraphicalCoil In Me
            If S.ReadTopRectSelected = True Then
                ReadTopSelectedStepList.AddExistingStep(S)
            End If
        Next S
    End Function


End Class