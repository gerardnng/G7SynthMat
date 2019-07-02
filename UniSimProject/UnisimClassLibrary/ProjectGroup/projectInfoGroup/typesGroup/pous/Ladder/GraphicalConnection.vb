Imports System.Math
Imports System.IO
Imports System.Xml
Imports System.Drawing
Imports System.Threading

Public Class GraphicalConnection
    Implements IDocumentable, IXMLExportable

    Protected m_Name As String
    Protected m_documentation As String
    Protected m_Number As Integer
    Protected m_Ladder As Ladder     'Ladder cui appartiene
    Protected m_Condition As BooleanExpression
    Protected m_ConditionString As String     'Memorizza dopo l'importazione xml temporaneamente la condizione
    Protected ConnValue As Boolean
    Protected m_PreviousGraphicalStepsList As GraphicalContactList
    Protected m_NextGraphicalStepsList As GraphicalContactList
    Protected m_PreviousGraphicalCoilList As GraphicalContactList
    Protected m_NextGraphicalCoilList As GraphicalContactList
    Protected m_Dimension As Integer
    Protected m_Position As Drawing.Point
    Protected Area As Drawing.Rectangle
    Protected m_Selected As Boolean
    Protected NotSelectionColor As Drawing.Color
    Protected SelectionColor As Drawing.Color
    Protected CoeffSnap As Integer
    Protected Carattere As Drawing.Font
    Protected BackColor As Drawing.Color
    Protected TextColor As Drawing.Color
    Protected ColorConditionTrue As Drawing.Color
    Protected ColorConditionFalse As Drawing.Color
    Protected ColorActive As Drawing.Color
    Protected ColorPreActive As Drawing.Color
    Protected ColorDeactive As Drawing.Color
    Protected GraphToDraw As Drawing.Graphics
    Protected DrawState As Boolean
    'La seguenta lista serve solo ai fini dell'esportazione xml
    Protected XmlPreviousConnectionsList As ArrayList

    Property Documentation() As String Implements IDocumentable.Documentation
        Get
            Documentation = m_documentation
        End Get
        Set(ByVal Value As String)
            m_documentation = Value
        End Set
    End Property
    Public Property Name() As String Implements IDocumentable.Name
        Get
            Name = m_Name
        End Get
        Set(ByVal Value As String)
            m_Name = Value
        End Set
    End Property
    Property Number() As String
        Get
            Number = m_Number
        End Get
        Set(ByVal Value As String)
            m_Number = Value
        End Set
    End Property
    'viene chiamato questo

    Public Sub New(ByVal N As Integer, ByVal Name As String, ByVal Documentation As String, _
        ByRef LPreviousSteps As GraphicalContactList, ByRef LNextSteps As GraphicalContactList, _
        ByRef Condition As BooleanExpression, ByRef RefLadder As Ladder, ByVal PoxX As Integer, _
        ByVal CSnap As Integer, ByVal ColSfondo As Drawing.Color, ByVal SelectionCol As Drawing.Color, _
        ByVal NotSelectionCol As Drawing.Color, ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, _
        ByVal ColConditionTrue As Drawing.Color, ByVal ColConditionFalse As Drawing.Color, _
        ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, _
        ByVal ColPreActive As Drawing.Color, ByVal Graph As Drawing.Graphics, _
        ByVal DrState As Boolean, ByVal Dimen As Integer)
        m_Number = N
        m_Name = Name
        m_documentation = Documentation
        m_PreviousGraphicalStepsList = LPreviousSteps
        m_NextGraphicalStepsList = LNextSteps
        m_Condition = Condition
        m_Ladder = RefLadder
        m_Condition = Condition
        m_Position.X = PoxX
        NotSelectionColor = NotSelectionCol
        SelectionColor = SelectionCol
        CoeffSnap = CSnap
        Carattere = Car
        BackColor = ColSfondo
        TextColor = TextCol
        ColorActive = ColActive
        ColorPreActive = ColPreActive
        ColorDeactive = Drawing.Color.Brown
        ColorConditionTrue = ColConditionTrue
        ColorConditionFalse = ColConditionFalse
        GraphToDraw = Graph
        DrawState = DrState
        m_Dimension = Dimen
        Area = New Drawing.Rectangle
        XmlPreviousConnectionsList = New ArrayList
        CalculusArea()
    End Sub
    Public Sub New(ByRef RefLadder As Ladder, ByVal CSnap As Integer, ByVal ColSfondo As Drawing.Color, _
        ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, _
        ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColConditionTrue As Drawing.Color, _
        ByVal ColConditionFalse As Drawing.Color, ByVal ColActive As Drawing.Color, _
        ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, _
        ByVal Graph As Drawing.Graphics, ByVal DrState As Boolean, ByVal Dimen As Integer)
        m_PreviousGraphicalStepsList = New GraphicalContactList
        m_NextGraphicalStepsList = New GraphicalContactList
        m_PreviousGraphicalCoilList = New GraphicalContactList
        m_NextGraphicalCoilList = New GraphicalContactList
        m_Ladder = RefLadder
        'Crea il body e imposta il tipo di body (EnumBodyType.ST)
        m_Position.X = -1
        NotSelectionColor = NotSelectionCol
        SelectionColor = SelectionCol
        CoeffSnap = CSnap
        Carattere = Car
        BackColor = ColSfondo
        TextColor = TextCol
        ColorActive = ColActive
        ColorPreActive = ColPreActive
        ColorDeactive = Drawing.Color.Brown
        ColorConditionTrue = ColConditionTrue
        ColorConditionFalse = ColConditionFalse
        GraphToDraw = Graph
        DrawState = DrState
        m_Dimension = Dimen
        Area = New Drawing.Rectangle
        XmlPreviousConnectionsList = New ArrayList
    End Sub
    Public Function CreateInstance(ByRef NewSfc As Ladder, ByRef StepsList As GraphicalContactList) As GraphicalConnection
        Dim NewPreviousGraphicalContactList As New GraphicalContactList
        Dim NewNextGraphicalStepsList As New GraphicalContactList
        For Each S As BaseGraphicalContact In m_PreviousGraphicalStepsList
            NewPreviousGraphicalContactList.Add(StepsList.FindContactByNumber(S.Number))
        Next S
        For Each S As BaseGraphicalContact In m_NextGraphicalStepsList
            NewNextGraphicalStepsList.Add(StepsList.FindContactByNumber(S.Number))
        Next S
        Return New GraphicalConnection(m_Number, m_Name, m_documentation, _
            NewPreviousGraphicalContactList, NewNextGraphicalStepsList, m_Condition, NewSfc, _
                m_Position.X, CoeffSnap, BackColor, SelectionColor, NotSelectionColor, TextColor, _
                    Carattere, ColorConditionTrue, ColorConditionFalse, ColorActive, ColorDeactive, _
                        ColorPreActive, Nothing, False, m_Dimension)
    End Function

    Public Function ReadLeftContact() As GraphicalContactList
        ReadLeftContact = m_PreviousGraphicalStepsList
    End Function

    Public Function ReadRigthContact() As GraphicalContactList
        ReadRigthContact = m_NextGraphicalStepsList
    End Function

    Public Sub DisposeMe()
        Me.Finalize()
    End Sub
    Public Function ReadDimension() As Integer
        ReadDimension = m_Dimension
    End Function
    Public Sub SetConnValue(ByVal v As Boolean)
        ConnValue = v
    End Sub
    Public Function ReadConnValue() As Boolean
        ReadConnValue = ConnValue
    End Function

    Public Sub SetPosition(ByVal x As Integer)
        m_Position.X = x
    End Sub
    Public Function ReadPosition() As Integer
        ReadPosition = m_Position.X
    End Function

    Public Sub SetGraphToDraw(ByRef Graph As Drawing.Graphics)
        GraphToDraw = Graph
    End Sub
    Public Function ReadSelected() As Boolean
        ReadSelected = m_Selected
    End Function

    Public Sub Draw(ByVal FlagShowDetails As Boolean)
        Draw(SelectionColor, NotSelectionColor, TextColor, FlagShowDetails)
    End Sub

    Public Sub Cancel(ByVal FlagShowDetails As Boolean)
        Draw(BackColor, BackColor, BackColor, FlagShowDetails)
    End Sub

    Public Sub SetSelected(ByVal v As Boolean)
        m_Selected = v

    End Sub
    Public Function ReadNextsGraphicalStepsList() As GraphicalContactList
        ReadNextsGraphicalStepsList = m_NextGraphicalStepsList
    End Function

    Public Function ReadPreviousGraphicalStepsList() As GraphicalContactList
        ReadPreviousGraphicalStepsList = m_PreviousGraphicalStepsList
    End Function

    Friend Function GetDrawnRectangles() As System.Collections.Generic.List(Of Rectangle)
        ' Codice tratto da Draw(). La migliore scelta sarebbe un metodo "back-end" che fornisca
        ' a noi un elenco di rettangoli e a Draw() un elenco di coppie di punti...
        ' (in realtà, l'idea che una connessione sia disegnata come un insieme di rettangoli è un'astrazione
        ' che noi usiamo, ponendo ad un valore molto basso la dimensione che altrimenti sarebbe -> 0)

        Const NullDimension As Integer = 1

        Dim ret As New System.Collections.Generic.List(Of Rectangle)
        Dim Right1, Left1, Right2, Left2, Max, Min, Med, Med1, Med2, MedMedi, TopArea, BottomArea, LeftArea, RightArea As Integer
        TopArea = 2147483647
        BottomArea = 0

        CalculusTransitionPoints(Right1, Left1, Right2, Left2, Max, Min, Med, Med1, Med2, MedMedi, m_Dimension)

        'Store LeftArea
        If Left1 < Left2 Then
            LeftArea = Left1
        Else
            LeftArea = Left2
        End If

        'Store RightArea
        If Right2 > Right1 Then
            RightArea = Right2
        Else
            RightArea = Right1
        End If

        'Snappa i coefficienti
        MedMedi = Snap(MedMedi, CoeffSnap)
        Med = Snap(Med, CoeffSnap)
        Med1 = Snap(Med1, CoeffSnap)
        Med2 = Snap(Med2, CoeffSnap)

        Dim p1, p2 As Drawing.Point


        'Linea orozzontale destra
        p1.X = Med1 + (m_Dimension / 2) - 2   'OK
        p1.Y = Max + (m_Dimension / 2)        'OK

        p2.X = MedMedi                        'OK
        p2.Y = Max + (m_Dimension / 2)        'OK
        If (p2.X > p1.X) Then
            ret.Add(New Rectangle(New Point(p1.X, p1.Y + NullDimension), New Size(p2.X - p1.X, NullDimension)))
        Else
            ret.Add(New Rectangle(New Point(p2.X, p2.Y + NullDimension), New Size(p1.X - p2.X, NullDimension)))
        End If

        'Store TopArea
        If p1.Y < TopArea Then
            TopArea = p1.Y
        End If
        'Memorizza BottomArea
        If p2.Y > BottomArea Then
            BottomArea = p2.Y
        End If

        'Linea orizzontale sinistra
        p1.X = Med2 - (m_Dimension / 2) + 2  'OK
        p1.Y = Min - (m_Dimension / 2)
        p2.X = MedMedi                       'OK
        p2.Y = Min - (m_Dimension / 2)
        If p1.X > p2.X Then
            ret.Add(New Rectangle(New Point(p2.X, p2.Y + NullDimension), New Size(p1.X - p2.X, NullDimension)))
        Else
            ret.Add(New Rectangle(New Point(p1.X, p1.Y + NullDimension), New Size(p2.X - p1.X, NullDimension)))
        End If

        'Memorizza TopArea
        If p2.Y < TopArea Then
            TopArea = p2.Y
        End If
        'Memorizza BottomArea
        If p1.Y > BottomArea Then
            BottomArea = p1.Y
        End If


        'Linea verticale centrale
        p1.X = MedMedi
        p1.Y = Max + (m_Dimension / 2)
        p2.X = MedMedi
        p2.Y = Min - (m_Dimension / 2)
        If p1.Y > p2.Y Then
            ret.Add(New Rectangle(New Point(p2.X + NullDimension, p2.Y), New Size(NullDimension, p1.Y - p2.Y)))
        Else
            ret.Add(New Rectangle(New Point(p1.X + NullDimension, p1.Y), New Size(NullDimension, p2.Y - p1.Y)))
        End If

        Return ret

    End Function

    Public Sub Draw(ByVal Col1 As Drawing.Color, ByVal Col2 As Drawing.Color, ByVal Col3 As Drawing.Color, ByVal FlagShowDetails As Boolean)
        If GraphToDraw Is Nothing Then Exit Sub
        If Monitor.TryEnter(GraphToDraw, 2000) Then
            Try
                Dim Right1, Left1, Right2, Left2, Max, Min, Med, Med1, Med2, MedMedi, TopArea, BottomArea, LeftArea, RightArea As Integer
                TopArea = 2147483647
                BottomArea = 0

                CalculusTransitionPoints(Right1, Left1, Right2, Left2, Max, Min, Med, Med1, Med2, MedMedi, m_Dimension)

                'Store LeftArea
                If Left1 < Left2 Then
                    LeftArea = Left1
                Else
                    LeftArea = Left2
                End If

                'Store RightArea
                If Right2 > Right1 Then
                    RightArea = Right2
                Else
                    RightArea = Right1
                End If

                'Snappa i coefficienti
                MedMedi = Snap(MedMedi, CoeffSnap)
                Med = Snap(Med, CoeffSnap)
                Med1 = Snap(Med1, CoeffSnap)
                Med2 = Snap(Med2, CoeffSnap)

                Dim p1, p2 As Drawing.Point
                Dim Penna As New Drawing.Pen(Col1)
                If m_Selected Then
                    'Penna.Color = SelectionColor
                Else
                    Penna.Color = Col2
                End If
                Penna.Width = 1

                'Controlla se sono una o più fasi precedenti


                'Linea orozzontale destra
                p1.X = Med1 + (m_Dimension / 2) - 2   'OK
                p1.Y = Max + (m_Dimension / 2)        'OK

                p2.X = MedMedi                        'OK
                p2.Y = Max + (m_Dimension / 2)        'OK
                GraphToDraw.DrawLine(Penna, p1, p2)

                'Store TopArea
                If p1.Y < TopArea Then
                    TopArea = p1.Y
                End If
                'Memorizza BottomArea
                If p2.Y > BottomArea Then
                    BottomArea = p2.Y
                End If

                'Linea orizzontale sinistra
                p1.X = Med2 - (m_Dimension / 2) + 2  'OK
                p1.Y = Min - (m_Dimension / 2)
                p2.X = MedMedi                       'OK
                p2.Y = Min - (m_Dimension / 2)
                GraphToDraw.DrawLine(Penna, p1, p2)

                'Memorizza TopArea
                If p2.Y < TopArea Then
                    TopArea = p2.Y
                End If
                'Memorizza BottomArea
                If p1.Y > BottomArea Then
                    BottomArea = p1.Y
                End If


                'Linea verticale centrale
                p1.X = MedMedi
                p1.Y = Max + (m_Dimension / 2)
                p2.X = MedMedi
                p2.Y = Min - (m_Dimension / 2)
                GraphToDraw.DrawLine(Penna, p1, p2)
                Penna.Width = 2


                'Memorizza la m_Position.x se è la prima volta che la disegna
                If m_Position.X = -1 Then
                    m_Position.X = MedMedi
                End If
                'Memorizza la m_Position.y 
                m_Position.Y = Med
                'Memorizza LeftArea
                If p1.X < LeftArea Then
                    LeftArea = p1.X
                End If

                'Testo nella transizione
                Dim Rect As Drawing.SizeF
                Rect = GraphToDraw.MeasureString(m_Number, Carattere)
                p1.X = m_Position.X + m_Dimension / 5 + 2
                p1.Y = Med - (Rect.Height / 2)
                Dim Br As New Drawing.SolidBrush(Col3)

                Dim Misure As Integer
                'If FlagShowDetails And m_Condition IsNot Nothing Then
                '    'Disegna il testo della condizione
                '    Misure = GraphToDraw.MeasureString(m_Condition.GetExpressionString, Carattere).Width
                '    'Misure = GraphToDraw.MeasureString(m_Name & " - " & m_Condition.GetExpressionString, Carattere).Width
                '    GraphToDraw.DrawString(m_Name & " - " & m_Condition.GetExpressionString, Carattere, Br, p1.X, p1.Y)
                'Else
                '    Misure = GraphToDraw.MeasureString(m_Name, Carattere).Width
                '    GraphToDraw.DrawString(m_Name, Carattere, Br, p1.X, p1.Y)
                'End If
                'Aggiorna l'area dopo la scritta della transizione
                If p1.X + Misure > RightArea Then
                    RightArea = p1.X + Misure
                End If
                'Store RightArea
                If p1.X + Rect.Width + 1 > RightArea Then
                    RightArea = p1.X + Rect.Width + 1
                End If

                'StoreArea
                Area.X = LeftArea
                Area.Y = TopArea
                Area.Width = RightArea - LeftArea
                Area.Height = BottomArea - TopArea

            Finally
                If GraphToDraw IsNot Nothing Then Monitor.Exit(GraphToDraw)
            End Try
        End If
    End Sub

    Private Sub CalculusTransitionPoints(ByRef Right1 As Integer, ByRef Left1 As Integer, ByRef Right2 As Integer, ByRef Left2 As Integer, ByRef Max As Integer, ByRef Min As Integer, ByRef Med As Integer, ByRef Med1 As Integer, ByRef Med2 As Integer, ByRef MedMedi As Integer, ByVal m_Dimension As Integer)
        Dim temp, OffsetSu, OffsetGiu As Integer

        'Calcola il max delle precedenti
        'Trova la fase più a sinistra e più a destra tra le precedenti
        Max = 0
        Left1 = Integer.MaxValue
        Right1 = 0
        For Each F As BaseGraphicalContact In m_PreviousGraphicalStepsList
            temp = F.ReadPosition.Y
            If Max < temp Then
                Max = temp
            End If

            temp = F.ReadPosition.X

            If Left1 > temp Then
                Left1 = temp
            End If
            If Right1 < temp Then
                Right1 = temp
            End If
        Next

        'Disegna la linee orizzontali superiori
        Dim p1 As New Drawing.Point
        Dim p2 As New Drawing.Point

        'Calcola il min delle successive
        'Trova la fase più a sinistra e più a destra tra le successive
        Min = Integer.MaxValue
        Left2 = Integer.MaxValue
        Right2 = 0
        For Each F As BaseGraphicalContact In m_NextGraphicalStepsList
            temp = F.ReadPosition.Y
            If Min > temp Then
                Min = temp
            End If
            temp = F.ReadPosition.X
            If Left2 > temp Then
                Left2 = temp
            End If
            If Right2 < temp Then
                Right2 = temp
            End If
        Next F

        'Trova i punti medi X
        Med1 = CInt((Right1 + Left1) / 2)
        Med2 = CInt((Right2 + Left2) / 2)

        'Se la m_Position.x non è impostata la calcola
        If m_Position.X = -1 Then

            MedMedi = (Med1 + Med2) / 2

        Else
            'altrimenti la legge
            MedMedi = m_Position.X
        End If

        'Calcola gli offset
        If Left1 = Right1 Then 'se c'è solo una fase precedente tira tutto più sù
            OffsetSu = CInt(m_Dimension / 2)
        End If
        If Left2 = Right2 Then 'se c'è solo una fase successiva tira tutto più giù
            OffsetGiu = CInt(m_Dimension / 2)
        End If

        Max = Max - OffsetSu
        Min = Min + OffsetGiu

        'Punto medio Y
        Med = CInt((Min + Max) / 2)


    End Sub


    Public Sub CalculusArea()

        Dim Right1, Left1, Right2, Left2, Max, Min, Med, Med1, Med2, MedMedi, TopArea, BottomArea, LeftArea, RightArea As Integer
        Dim p1, p2 As Drawing.Point
        TopArea = 2147483647
        BottomArea = 0

        CalculusTransitionPoints(Right1, Left1, Right2, Left2, Max, Min, Med, Med1, Med2, MedMedi, m_Dimension)

        'Memorizza LeftArea
        If Left1 < Left2 Then
            LeftArea = Left1
        Else
            LeftArea = Left2
        End If

        'Memorizza RightArea
        If Right2 > Right1 Then
            RightArea = Right2
        Else
            RightArea = Right1
        End If

        p1.Y = Max + m_Dimension + 2
        p2.Y = Max + m_Dimension + CInt(m_Dimension / 4) + 2

        'Memorizza TopArea
        If p1.Y < TopArea Then
            TopArea = p1.Y
        End If
        'Memorizza BottomArea
        If p2.Y > BottomArea Then
            BottomArea = p2.Y
        End If

        p1.Y = Min - m_Dimension - 2
        p2.Y = Min - m_Dimension - CInt(m_Dimension / 4) - 2

        'Memorizza TopArea
        If p2.Y < TopArea Then
            TopArea = p2.Y
        End If
        'Memorizza BottomArea
        If p1.Y > BottomArea Then
            BottomArea = p1.Y
        End If

        'Trattino della Transition
        p1.X = MedMedi - CInt(m_Dimension / 5)

        'Memorizza LeftArea
        If p1.X < LeftArea Then
            LeftArea = p1.X
        End If

        'Testo nella Transition
        p1.X = m_Position.X + m_Dimension / 5 + 2

        'Memorizza RightArea
        If p1.X + m_Dimension + 1 > RightArea Then
            RightArea = p1.X + m_Dimension + 1
        End If

        'Memorizza Area
        Area.X = LeftArea - 1
        Area.Y = TopArea + 1
        Area.Width = RightArea - LeftArea + 2
        Area.Height = BottomArea - TopArea + 2


    End Sub

    Private Function Snap(ByVal val As Integer, ByVal CoeffSnap As Integer) As Integer
        Snap = CInt(val / CoeffSnap) * CoeffSnap

    End Function

    Public Sub Move(ByVal dx As Integer)
        m_Position.X = m_Position.X + dx
        CalculusArea()
    End Sub

    Public Function ReadArea() As Drawing.Rectangle
        ReadArea = Area
    End Function
    Public Function MyAreaIsHere(ByVal x As Integer, ByVal y As Integer) As Boolean
        For Each R As Rectangle In GetDrawnRectangles()
            If R.Contains(x, y) Then Return True
        Next
        Return False
    End Function
    Public Function ReadXmlPreviousConnectionsList() As ArrayList
        ReadXmlPreviousConnectionsList = XmlPreviousConnectionsList
    End Function

    Public Sub xmlExport(ByRef RefXmlProjectWriter As XmlTextWriter) Implements IXMLExportable.xmlExport
        'Esporta la connessione
        RefXmlProjectWriter.WriteStartElement("connector")
        'Attributi della transizione
        RefXmlProjectWriter.WriteAttributeString("height", m_Dimension.ToString)
        RefXmlProjectWriter.WriteAttributeString("width", m_Dimension.ToString)
        RefXmlProjectWriter.WriteAttributeString("localId", m_Number.ToString)

        'm_Position
        RefXmlProjectWriter.WriteStartElement("position")
        'Attributi di m_Position
        'Nella transizione m_Position si riferisce al centro del trattino quindi sottrae CInt(m_Dimension / 10) a m_Position.X
        RefXmlProjectWriter.WriteAttributeString("x", (m_Position.X - CInt(m_Dimension / 10)).ToString)
        RefXmlProjectWriter.WriteAttributeString("y", (m_Position.Y).ToString)
        RefXmlProjectWriter.WriteEndElement() 'm_Position

        'ConnectionPointIn
        Dim ConnectionsList As New ConnectionPointIn(XmlPreviousConnectionsList)
        ConnectionsList.xmlExport(RefXmlProjectWriter)

        'condition  (Riferimento al nome della variabile booleana che controlla
        '   RefXmlProjectWriter.WriteStartElement("condition")
        'reference
        '  RefXmlProjectWriter.WriteStartElement("reference")

        'Attributi di reference
        'RefXmlProjectWriter.WriteAttributeString("name", m_Condition.GetExpressionString)

        ' RefXmlProjectWriter.WriteEndElement() 'reference
        ' RefXmlProjectWriter.WriteEndElement() 'condition

        RefXmlProjectWriter.WriteEndElement() 'connector
    End Sub


    Public Function GetSystemDescription() As String Implements IDocumentable.GetSystemDescription
        Dim desc As String = GetIdentifier()
        desc += " from " + Me.ReadPreviousGraphicalStepsList.ListSteps() + " to " + _
            Me.ReadNextsGraphicalStepsList.ListSteps()
        Return desc
    End Function

    Public Function GetDescription() As String Implements IDocumentable.GetDescription
        Return GetSystemDescription() + IIf(Of String)(Documentation = "", "", " (" + Documentation + ")")
    End Function

    Public Function GetIdentifier() As String Implements IDocumentable.GetIdentifier
        Return "Connection "
    End Function
End Class
