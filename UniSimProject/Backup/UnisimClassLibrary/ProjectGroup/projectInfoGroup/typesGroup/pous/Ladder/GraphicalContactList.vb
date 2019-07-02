Imports System.IO
Imports System.Drawing
Imports System.Xml
Imports System.Collections.Generic


Public Class GraphicalContactList
    Inherits List(Of BaseGraphicalContact)
    Implements IXMLSerializable

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
    Private DrawVariable As Boolean

    WriteOnly Property ResGlobalVariables() As VariablesLists
        Set(ByVal Value As VariablesLists)
            'Comunica al contatto le lista di variabili globali della risorsa
            For Each S As BaseGraphicalContact In Me
                If S.GetType.Name = "GraphicalContact" Then
                    Dim MS As GraphicalContact = S
                    'MS.ResGlobalVariables = Value
                End If
            Next
        End Set
    End Property

    Public Function FindContactByNumber(ByVal n As Integer) As BaseGraphicalContact
        For Each S As BaseGraphicalContact In Me
            If S.Number = n Then
                Return S
            End If
        Next S
        Return Nothing
    End Function


    WriteOnly Property PouInterface() As PouInterface
        Set(ByVal Value As PouInterface)
            'Comunica al contatto l'interfaccia della pou
            'come e perchè?
        End Set
    End Property

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
        Dim NumSteps As Integer = R.ReadInt32
        ' For i = 1 To NumSteps
        'Dim S As New GraphicalStep(R, BackCol, SelectionCol, NotSelectionCol, ColTesto, Car, ColActive, ColDeactive, ColPreActive)
        'Add(S)
        ' Next i
    End Sub
    Public Sub New()
        MyBase.new()
    End Sub
    Public Sub New(ByVal ParamArray Contacts() As BaseGraphicalContact)
        MyBase.New(Contacts)
    End Sub
    Public Sub AddAndDrawLeftRail(ByVal StepNumber As Integer, ByVal StepName As String, _
        ByVal StepDocumentation As String, ByVal Position As Drawing.Point, _
        ByVal DrawState As Boolean, ByVal Id As Integer, _
        ByVal Name As String, ByVal Qualy As String, ByVal Var As BaseVariable, _
        ByVal Ind As BaseVariable, ByVal Time As TimeSpan)
        Dim S As New GraphicalLeftRail(StepNumber, StepName, StepDocumentation, MyLadder, False, False, Position, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, GraphToDraw, DrawState, Dimension, Id, Name, Qualy, Var, Ind, Time)
        Add(S)
        S.DrawContact(False, Id, Name, Qualy, Var, Ind, Time)
    End Sub

    Public Sub AddAndDrawRightRail(ByVal StepNumber As Integer, ByVal StepName As String, _
    ByVal StepDocumentation As String, ByVal Position As Drawing.Point, ByVal DrawState As Boolean, _
    ByVal Id As Integer, ByVal Name As String, ByVal Qualy As String, ByVal Var As BaseVariable, _
    ByVal Ind As BaseVariable, ByVal Time As TimeSpan)
        Dim T As New GraphicalRightRail(StepNumber, StepName, StepDocumentation, MyLadder, False, False, Position, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, GraphToDraw, DrawState, Dimension, Id, Name, Qualy, Var, Ind, Time)
        Add(T)
        T.DrawContact(False, Id, Name, Qualy, Var, Ind, Time)
    End Sub

    Public Sub AddAndDrawContact(ByVal StepNumber As Integer, ByVal StepName As String, ByVal StepDocumentation As String, ByVal Position As Drawing.Point, ByVal DrawState As Boolean, ByVal Id As Integer, ByVal Name As String, ByVal Qualy As String, ByVal Var As BaseVariable, ByVal Ind As BaseVariable, ByVal Time As TimeSpan)
        Dim S As GraphicalContact
        If Id <> 3 Then
            S = New GraphicalContact(StepNumber, StepName, StepDocumentation, MyLadder, _
                False, False, Position, BackColor, SelectionColor, NotSelectionColor, _
                    TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, GraphToDraw, _
                        DrawState, Dimension, Id, Name, Qualy, _
                        Var, Ind, Time)
        Else
            S = New GraphicalContact(StepNumber, StepName, StepDocumentation, MyLadder, _
                False, False, Position, BackColor, SelectionColor, NotSelectionColor, _
                 TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, GraphToDraw, _
                    DrawState, New Size(44, 60), Id, Name, Qualy, _
                        Var, Ind, Time)
        End If
        Add(S)
        S.DrawContact(False, Id, Name, Qualy, Var, Ind, Time)
    End Sub
   
    Public Function FindAndSelectContact(ByVal x As Integer, ByVal y As Integer) As Boolean
        'seleziona solo il primo contatto che trova
        For Each T As BaseGraphicalContact In Me
            If T.MyAreaIsHere(x, y) = True Then
                FindAndSelectContact = True
                T.SetSelected(True)
                Exit Function
            End If
        Next T
    End Function

    Public Sub FindAndSelectRail()
        '   'cancella l'ultimo rail disegnato'
        Dim MaxPos As Integer = 0

        'sinistra
        For Each T As BaseGraphicalContact In Me
            If T.ReadId = 20 Then 'è una RAIL (...che sistema...)
                If T.ReadPosition.Y > MaxPos Then
                    MaxPos = T.ReadPosition.Y
                End If
            End If
        Next T

        If MaxPos <> 0 Then

            For Each sT As BaseGraphicalContact In Me
                If sT.ReadId = 20 And (sT.ReadPosition.Y = MaxPos) Then
                    sT.SetSelected(True)
                End If
            Next sT

            For Each dT As BaseGraphicalContact In Me
                If dT.ReadId = 21 And (dT.ReadPosition.Y = MaxPos) Then
                    dT.SetSelected(True)
                End If
            Next dT

        End If


    End Sub


    Public Sub AddExistingContact(ByVal S As BaseGraphicalContact)
        Add(S)
    End Sub

    Public Sub RemoveSelectedElements()
        Dim i, j As Integer
        j = 0
        For i = 0 To Count - 1
            'Cancella il contatto se selezionato
            If Me(i - j).ReadSelected = True Then
                Me(i - j).DisposeMe()
                RemoveAt(i - j)
                j = j + 1
            End If
        Next i
    End Sub
    <Obsolete("Usare RemoveSelectedElements()")> _
    Public Sub RemoveSelectedRail()
        Dim i, j As Integer
        j = 0
        For i = 0 To Count - 1
            'Cancella il contatto se selezionato
            If Me(i - j).ReadSelected = True Then
                Me(i - j).DisposeMe()
                RemoveAt(i - j)
                j = j + 1
            End If
        Next i


        '   Dim MaxPos As Integer = 0
        '   'cancella l'ultimo rail disegnato'
        '
        '       For Each T As BaseGraphicalContact In Me
        '      If T.ReadId = 20 Then 'è una RAIL
        '     If T.ReadPosition.Y > MaxPos Then
        '    MaxPos = T.ReadPosition.Y
        '   End If
        '  End If
        'Next T

        '  MsgBox(MaxPos)
        ' 'disegna power rail in bianco!!

    End Sub


    Public Function CountSelected() As Integer
        CountSelected = 0
        For Each S As BaseGraphicalContact In Me
            If S.ReadSelected And S.ReadId <> 20 And S.ReadId <> 21 Then
                CountSelected = CountSelected + 1
            End If
        Next S
    End Function

    <Obsolete("Non ha molto senso per 2 o più contatti in lista")> _
    Public Function GetPosition() As Integer
        For Each S As BaseGraphicalContact In Me
            GetPosition = S.ReadPosition.X()
        Next S
    End Function

    <Obsolete("Non ha molto senso per 2 o più contatti in lista")> _
    Public Function GetVariableValue() As Boolean
        For Each S As BaseGraphicalContact In Me
            GetVariableValue = S.ReadVar.ReadValue
        Next S
    End Function

    <Obsolete("Non ha molto senso per 2 o più contatti in lista")> _
    Public Function ReadFinalValue() As Boolean
        For Each S As BaseGraphicalContact In Me
            ReadFinalValue = S.ReadFinal
        Next S
    End Function

    Public Sub SetInitialValue(ByVal Val As Boolean) 'setta solo quelli a 1
        For Each S As BaseGraphicalContact In Me
            'Dim Id As Integer = S.ReadId
            'Dim Qualy As String = S.ReadQualy
            'Dim Initial As Boolean = S.ReadActive
            'S.SetFinalValue(Id, Qualy, Initial)
            S.SetInitial(Val)
        Next S
    End Sub

    Public Sub SetFinal()
        For Each S As BaseGraphicalContact In Me
            'Dim Id As Integer = S.ReadId
            'Dim Qualy As String = S.ReadQualy
            'Dim Initial As Boolean = S.ReadInitial
            'MsgBox("SetFinal" & Id)
            S.SetFinalValue()

            'S.SetInitial(Val)
            'S.ResetInitial()
        Next S
    End Sub




    Public Sub SetValue(ByVal Val As Boolean)
        For Each S As BaseGraphicalContact In Me
            S.SetFinal(Val)
        Next S
    End Sub


    Public Function ReadSelected() As GraphicalContactList
        ReadSelected = New GraphicalContactList
        For Each S As BaseGraphicalContact In Me
            If S.ReadSelected = True Then 'And S.ReadId <> 20 And S.ReadId <> 21 Then
                ReadSelected.AddExistingContact(S)
            End If
        Next S
    End Function

    Public Function IndexOfStep(ByRef S As BaseGraphicalContact) As Integer
        IndexOfStep = IndexOf(S)
        If IsNothing(IndexOfStep) Then
            IndexOfStep = -1
        End If
    End Function

    Public Function IsSelectionOutside(ByVal R As Drawing.Rectangle, ByRef FuoriX As Boolean, ByRef FuoriY As Boolean) As Boolean
        'Controlla che l'area delle Steps selezionate....
        '....non sia fuori da R (se è fuori restituisce True)
        Dim Area As Drawing.Rectangle
        For Each S As BaseGraphicalContact In Me
            If S.ReadSelected Then
                Area = S.ReadArea
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
        Next S
    End Function


    Public Sub MoveSelection(ByVal dx As Integer, ByVal dy As Integer)
        'Muove le fasi selezionate
        For Each S As BaseGraphicalContact In Me
            If S.ReadSelected Then
                S.Move(dx, dy)
            End If
        Next S
    End Sub

    Public Sub FindAndSelectSteps(ByVal Rect As Drawing.Rectangle)
        'Seleziona tutte le fasi che trova
        Dim x, y, Passo As Integer
        For Each S As BaseGraphicalContact In Me
            Passo = S.ReadDimension().Width
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


    Public Sub DeSelectAll()
        For Each S As BaseGraphicalContact In Me
            S.SetSelected(False)
            S.SetBottomRectSelected(False)
            S.SetTopRectSelected(False)
            If S.IsBlock Then
                CType(S, GraphicalContact).DeselectOutFBDREctangle()
                CType(S, GraphicalContact).DeselectInFBDREctangle()
            End If
        Next S
    End Sub


    Public Sub SetGraphToDraw(ByRef Graph As Drawing.Graphics)
        GraphToDraw = Graph
        For Each S As BaseGraphicalContact In Me
            S.SetGraphToDraw(GraphToDraw)
        Next
    End Sub
    Public Function ReadIfContactSelected(ByVal x As Integer, ByVal y As Integer) As Boolean
        'Trova la prima fase in x,y
        For Each S As BaseGraphicalContact In Me
            If S.MyAreaIsHere(x, y) And S.ReadSelected Then
                ReadIfContactSelected = True
                Exit Function
            End If
        Next S
    End Function

    Public Sub CancelSelection(ByVal CancelSmallRectangels As Boolean)
        For Each S As BaseGraphicalContact In Me
            If S.ReadSelected Then
                Dim Id As Integer = S.ReadId
                Dim Var As BaseVariable = S.ReadVar
                Dim Name As String = S.ReadVarName
                Dim Qualy As String = S.ReadQualy
                Dim Ind As BaseVariable = S.ReadInd
                Dim Time As TimeSpan = S.ReadTime
                S.Cancel(CancelSmallRectangels, Id, Name, Qualy, Var, Ind, Time)
            End If
        Next S
    End Sub


    Public Sub DrawArea(ByVal Rect As Drawing.Rectangle, ByVal DrawSmallRectangels As Boolean)
        For Each S As BaseGraphicalContact In Me
            'La disegna se si interseca con il rectanglolo da disegnare
            If S.ReadArea.IntersectsWith(Rect) Then
                Dim Id As Integer = S.ReadId
                Dim Var As BaseVariable = S.ReadVar
                Dim Name As String = S.Name
                Dim Qualy As String = S.ReadQualy
                Dim Ind As BaseVariable = S.ReadInd
                Dim Time As TimeSpan = S.ReadTime
                S.DrawContact(DrawSmallRectangels, Id, Name, Qualy, Var, Ind, Time)
            End If
        Next S
    End Sub
    Public Sub DrawAreaNC(ByVal Rect As Drawing.Rectangle, ByVal DrawSmallRectangels As Boolean, ByVal Name As String, ByVal Qualy As String, ByVal Var As BaseVariable, ByVal Ind As BaseVariable, ByVal Time As TimeSpan)
        'ok questa viene chiamata col paint
        For Each S As BaseGraphicalContact In Me
            'La disegna se si interseca con il rectanglolo da disegnare
            If S.ReadArea.IntersectsWith(Rect) Then
                Dim Id As Integer = S.ReadId
                'S.DrawContact(DrawSmallRectangels)
                S.DrawContact(DrawSmallRectangels, Id, Name, Qualy, Var, Ind, Time)
            End If
        Next S
    End Sub

    Public Function FindContact(ByVal x As Integer, ByVal y As Integer) As Boolean

        For Each T As BaseGraphicalContact In Me
            If T.MyAreaIsHere(x, y) Then
                FindContact = True
                Exit Function
            End If
        Next T
    End Function

    Public Function FindAndSelectSmallRectangleStep(ByVal x As Integer, ByVal y As Integer) As Boolean
        For Each S As BaseGraphicalContact In Me
            'Controlla il ret sup
            If S.CeIlMioRetLeft(x, y) Then
                'Inverte il valore selezionato del ret sup
                S.SetTopRectSelected(Not S.ReadTopRectSelected)
                FindAndSelectSmallRectangleStep = True
                Exit Function
            End If
            'Controlla il valore selezionato
            If S.CeIlMioRetRight(x, y) = True Then
                'Inverte il Value Selected del ret sup
                S.SetBottomRectSelected(Not S.ReadBottomRectSelected)
                FindAndSelectSmallRectangleStep = True
                Exit Function
            End If
            If S.IsBlock Then
                Dim GC As GraphicalContact = CType(S, GraphicalContact)
                If GC.IsInFBDRectangleHere(x, y) Then GC.InFBDRectangleSelected = Not (GC.InFBDRectangleSelected)
                If GC.IsOutFBDRectangleHere(x, y) Then GC.OutFBDRectangleSelected = Not (GC.OutFBDRectangleSelected)
            End If
        Next S
    End Function


    Public Function ReadBottomSelectedContactList() As GraphicalContactList
        ReadBottomSelectedContactList = New GraphicalContactList
        For Each S As BaseGraphicalContact In Me
            If S.ReadBottomRectSelected = True Then
                ReadBottomSelectedContactList.AddExistingContact(S)
            End If
        Next S
    End Function

    Public Function CountStepsList() As Integer
        CountStepsList = Count
    End Function

    Public Function ReadTopSelectedContactList() As GraphicalContactList
        ReadTopSelectedContactList = New GraphicalContactList
        For Each S As BaseGraphicalContact In Me
            If S.ReadTopRectSelected = True Then
                ReadTopSelectedContactList.AddExistingContact(S)
            End If
        Next S
    End Function

    Public Function ReadBottomSelectedStepList() As GraphicalContactList
        ReadBottomSelectedStepList = New GraphicalContactList
        For Each S As BaseGraphicalContact In Me
            If S.ReadBottomRectSelected = True Then
                ReadBottomSelectedStepList.AddExistingContact(S)
            End If
        Next S
    End Function

    Public Sub ReadLeftSelectedContact()
        'ReadLeftSelectedContact = New GraphicalContact
        For Each S As BaseGraphicalContact In Me
            If S.ReadBottomRectSelected = True Then
                ReadBottomSelectedStepList.AddExistingContact(S)
            End If
        Next S
    End Sub

    Public Function ReadContactSelected() As GraphicalContact
        For Each S As BaseGraphicalContact In Me
            If S.ReadSelected And TypeOf (S) Is GraphicalContact Then
                Return S
            End If
        Next S
        Return Nothing
    End Function

    Public Sub ClearXMLConnectionsLists()
        Nop()
    End Sub
    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter) Implements IXMLSerializable.xmlExport
        For Each S As BaseGraphicalContact In Me
            S.xmlExport(RefXMLProjectWriter)
        Next S
    End Sub

    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader) Implements IXMLSerializable.xmlImport
        Select Case RefXmlProjectReader.Name
            Case "contact", "coil", "block" 'vale per contatti, bobine e blocchi
                Dim S As New GraphicalContact(MyLadder, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, GraphToDraw, DrawState, Dimension)
                S.xmlImport(RefXmlProjectReader)
                Add(S)
            Case "leftPowerRail" 'alimentazione sx
                Dim L As New GraphicalLeftRail(MyLadder, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, GraphToDraw, DrawState, Dimension)
                L.xmlImport(RefXmlProjectReader)
                Add(L)
            Case "rightPowerRail" 'alimentazione dx
                Dim R As New GraphicalRightRail(MyLadder, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, GraphToDraw, DrawState, Dimension)
                R.xmlImport(RefXmlProjectReader)
                Add(R)
        End Select
    End Sub
    Public Function CreateInstance(ByRef NewLadder As Ladder) As GraphicalContactList
        CreateInstance = New GraphicalContactList(NewLadder, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ActiveColor, DeactiveColor, PreActiveColor, Nothing, Dimension)
        For Each S As BaseGraphicalContact In Me
            CreateInstance.Add(S.CreateInstance(NewLadder))
        Next S
    End Function
    Public Sub SetDrawState(ByVal DrState As Boolean)
        DrawState = DrState
        For Each F As BaseGraphicalContact In Me
            F.SetDrawState(DrState)
        Next F
    End Sub
    Public Sub SetDrawVariable(ByVal DrVariable As Boolean)
        'DrawVariable = Not DrVariable
        For Each F As BaseGraphicalContact In Me
            F.SetInitial(DrVariable)
        Next F
    End Sub
    Public Sub DrawStepsState()
        For Each F As BaseGraphicalContact In Me
            F.DrawStepState(False)
        Next F
    End Sub
    Public Sub ExecuteInit()
      
    End Sub

    Public Sub ResetState()
        For Each S As BaseGraphicalContact In Me
            If TypeOf (S) Is GraphicalContact Then _
                S.ResetInitial()
        Next S
    End Sub

    Public Sub ResolveVariablesLinks()
        'Risolve i riferimenti dei nomi di variabili
        For Each S As BaseGraphicalContact In Me
            S.ResolveVariablesLinks()
        Next S
    End Sub

    ' Ritorna una lista dei contatti inseriti in questa lista in forma stringa
    Public Function ListSteps() As String
        ' Sarebbe più ottimizzato usare un System.Text.StringBuilder, ma questo metodo non viene chiamato
        ' abbastanza spesso e su abbastanza dati da essere un vero collo di bottiglia per l'ambiente
        Dim ret As String = ""
        For Each C As BaseGraphicalContact In Me
            If TypeOf (C) Is GraphicalLeftRail Then
                ret += "left rail, "
            ElseIf TypeOf (C) Is GraphicalRightRail Then
                ret += "right rail, "
            Else
                ret += C.Name + ", "
            End If
        Next
        ret = ret.TrimEnd(" ").TrimEnd(",")
        Return ret
    End Function

    Public Sub SetLastValues()
        For Each A As BaseGraphicalContact In Me
            A.SetLastValue()
        Next
    End Sub

    Public Function GetLeftRails() As GraphicalContactList
        Dim ret As New GraphicalContactList()
        For Each C As BaseGraphicalContact In Me
            If TypeOf (C) Is GraphicalLeftRail Then ret.Add(C)
        Next
        Return ret
    End Function

    Public Function GetRightRails() As GraphicalContactList
        Dim ret As New GraphicalContactList()
        For Each C As BaseGraphicalContact In Me
            If TypeOf (C) Is GraphicalRightRail Then ret.Add(C)
        Next
        Return ret
    End Function

End Class
