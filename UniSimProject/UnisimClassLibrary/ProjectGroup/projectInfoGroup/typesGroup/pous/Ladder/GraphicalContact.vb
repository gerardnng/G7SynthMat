Imports System.Collections.Generic
Imports System.IO
Imports System.Math
Imports System.Xml
Imports System.Threading
Imports System.Drawing

' Questa classe rappresenta sia contatti e bobine che blocchi
' Sarebbe meglio spezzare tutto in almeno 3 classi diverse, ma per come è stato a suo tempo
' progettato il supporto LD in UniSim, il tutto richiede troppo lavoro...
Public Class GraphicalContact
    Inherits BaseGraphicalContact
    Implements ILDConnectable

    Protected m_Body As body
    'Protected m_PulseActionExecuted As Boolean
    'Protected m_NotPulseActionExecuted As Boolean
    Protected m_ContactId As Integer
    Protected m_ContactQualy As String
    Protected WithEvents m_Indicator As BaseVariable
    ' Queste variabili vengono usate se IsBlock = True
    ' ------------------------------------------------------
    Protected m_InFBDSelMan As New BasicSelectionManager
    Protected m_OutFBDSelMan As New BasicSelectionManager
    ' Liste temporanee per memorizzare gli ingressi e le uscite letti
    ' dall'XML. ResolveVariableLinks() li trasforma in connessioni
    ' (il Boolean andrà a finire in Negated)
    Class ConnectionDescriptor
        Public OtherPartyID As Integer = 0
        Public Negated As Boolean = False
        Public Offset As Integer = 0
        Public Sub New()

        End Sub
    End Class
    Private m_Inputs As New List(Of ConnectionDescriptor)
    Private m_Outputs As New List(Of ConnectionDescriptor)
    ' ------------------------------------------------------

    Public Sub New(ByRef RefLadder As Ladder, ByVal BackCol As Drawing.Color, _
        ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, _
        ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, _
        ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, _
        ByVal ColPreActive As Drawing.Color, ByRef Graph As Drawing.Graphics, _
        ByVal DrState As Boolean, ByVal Dimension As Size)
        MyBase.New(RefLadder, BackCol, SelectionCol, NotSelectionCol, TextCol, Car, ColActive, _
            ColDeactive, ColPreActive, Graph, DrState, Dimension)
        CalculusArea()
    End Sub
    Public Sub New(ByVal StepNumber As Integer, ByVal StepName As String, _
    ByVal StepDocumentation As String, ByRef RefLadder As Ladder, ByVal Ini As Boolean, _
    ByVal Fin As Boolean, ByVal P As Drawing.Point, ByVal BackCol As Drawing.Color, _
    ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, _
    ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, _
    ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, _
    ByRef Graph As Drawing.Graphics, ByVal DrState As Boolean, ByVal Dimension As Size, _
    ByVal Id As Integer, ByVal Name As String, ByVal Qualy As String, _
    ByVal Var As BaseVariable, ByVal Ind As BooleanVariable, ByVal Time As TimeSpan)
        MyBase.New(StepNumber, StepName, StepDocumentation, RefLadder, Ini, Fin, P, BackCol, _
            SelectionCol, NotSelectionCol, TextCol, Car, ColActive, ColDeactive, ColPreActive, _
                Graph, DrState, Dimension, Id, Name, Qualy, Var, Ind, Time)
        CalculusArea()
    End Sub

    Public Sub New(ByRef RefLadder As Ladder, ByVal BackCol As Drawing.Color, _
        ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, _
        ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, _
        ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, _
        ByVal ColPreActive As Drawing.Color, ByRef Graph As Drawing.Graphics, _
        ByVal DrState As Boolean, ByVal Dimension As Integer)
        MyBase.New(RefLadder, BackCol, SelectionCol, NotSelectionCol, TextCol, Car, ColActive, _
            ColDeactive, ColPreActive, Graph, DrState, Dimension)
        CalculusArea()
    End Sub
    Public Sub New(ByVal StepNumber As Integer, ByVal StepName As String, _
    ByVal StepDocumentation As String, ByRef RefLadder As Ladder, ByVal Ini As Boolean, _
    ByVal Fin As Boolean, ByVal P As Drawing.Point, ByVal BackCol As Drawing.Color, _
    ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, _
    ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, _
    ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, _
    ByRef Graph As Drawing.Graphics, ByVal DrState As Boolean, ByVal Dimension As Integer, _
    ByVal Id As Integer, ByVal Name As String, ByVal Qualy As String, _
    ByVal Var As BaseVariable, ByVal Ind As BooleanVariable, ByVal Time As TimeSpan)
        MyBase.New(StepNumber, StepName, StepDocumentation, RefLadder, Ini, Fin, P, BackCol, _
            SelectionCol, NotSelectionCol, TextCol, Car, ColActive, ColDeactive, ColPreActive, _
                Graph, DrState, Dimension, Id, Name, Qualy, Var, Ind, Time)
        CalculusArea()
    End Sub

    Public Overrides Sub SetFinalValue()
    End Sub
    Public Overloads Overrides Sub DrawContact(ByVal DrawSmallRectangels As Boolean, ByVal Id As Integer)
    End Sub

    Public Overloads Overrides Sub DrawContact(ByVal DrawSmallRectangels As Boolean, ByVal Id As Integer, _
        ByVal Name As String, ByVal Qualy As String, ByVal Var As BaseVariable, _
        ByVal Ind As BooleanVariable, ByVal Time As TimeSpan)
        Draw(DrawSmallRectangels, SelectionColor, NotSelectionColor, _
        TextColor, BackColor, Id, Name, Qualy, Var, Ind, Time)
    End Sub
    Public Sub DrawContactStateValueChanched() Handles m_Var.ValueChanged
        If IsNothing(GraphToDraw) Then Exit Sub
        If Monitor.TryEnter(GraphToDraw, 2000) Then
            Try
                If DrawState Then
                    DrawStepState(False, m_Var)
                End If
            Finally
                Monitor.Exit(GraphToDraw)
            End Try
        End If
    End Sub
    Public Sub DrawContactStateValueCambiato() Handles m_Var.ValueChanging
        If IsNothing(GraphToDraw) Then Exit Sub
        If Monitor.TryEnter(GraphToDraw, 2000) Then
            Try
                If DrawState Then
                    DrawStepState(False, m_Var)
                End If
            Finally
                Monitor.Exit(GraphToDraw)
            End Try
        End If
    End Sub

    Public ReadOnly Property InConnectionRectangle() As Rectangle Implements ILDConnectable.InConnectionRectangle
        Get
            Dim smallRectSize As New Drawing.Size(m_Dimension.Width / 5, m_Dimension.Height / 5)
            Return New Rectangle(ObjectRectangle.X - smallRectSize.Width, _
                ObjectRectangle.Y + ObjectRectangle.Height - smallRectSize.Height, smallRectSize.Width, _
                    smallRectSize.Height)
        End Get
    End Property
    Public Function IsInFBDRectangleHere(ByVal x As Integer, ByVal y As Integer) As Boolean
        Return InConnectionRectangle.Contains(x, y)
    End Function
    Public Property InFBDRectangleSelected() As Boolean
        Get
            Return m_InFBDSelMan.Selected
        End Get
        Set(ByVal value As Boolean)
            m_InFBDSelMan.Selected = value
        End Set
    End Property
    Public Sub SelectInFBDRectangle()
        InFBDRectangleSelected = True
    End Sub
    Public Sub DeselectInFBDREctangle()
        InFBDRectangleSelected = False
    End Sub

    Public ReadOnly Property OutConnectionRectangle() As Rectangle Implements ILDConnectable.OutConnectionRectangle
        Get
            Dim smallRectSize As New Drawing.Size(m_Dimension.Width / 5, m_Dimension.Height / 5)
            Return New Rectangle(ObjectRectangle.X + ObjectRectangle.Width, _
                ObjectRectangle.Y + ObjectRectangle.Height - smallRectSize.Height, smallRectSize.Width, _
                    smallRectSize.Height)
        End Get
    End Property
    Public Function IsOutFBDRectangleHere(ByVal x As Integer, ByVal y As Integer) As Boolean
        Return OutConnectionRectangle.Contains(x, y)
    End Function
    Public Property OutFBDRectangleSelected() As Boolean
        Get
            Return m_OutFBDSelMan.Selected
        End Get
        Set(ByVal value As Boolean)
            m_OutFBDSelMan.Selected = value
        End Set
    End Property
    Public Sub SelectOutFBDRectangle()
        OutFBDRectangleSelected = True
    End Sub
    Public Sub DeselectOutFBDREctangle()
        OutFBDRectangleSelected = False
    End Sub

    Private Sub DrawImplContact(ByVal DrawSmallRectangles As Boolean, ByVal Col1 As Drawing.Color, _
    ByVal Col2 As Drawing.Color, ByVal Col3 As Drawing.Color, ByVal Col4 As Drawing.Color, _
    ByVal Id As Integer, ByVal Name As String, ByVal Qualy As String, ByVal Var As BaseVariable , _
    ByVal Ind As BooleanVariable, ByVal Time As TimeSpan, ByVal Penna As Pen)

        Dim Br As Brush

        GraphToDraw.DrawLine(Penna, Position.X - 20, Position.Y, Position.X - 6, Position.Y) 'orizz
        GraphToDraw.DrawLine(Penna, Position.X + 6, Position.Y, Position.X + 20, Position.Y) 'orizz
        GraphToDraw.DrawLine(Penna, Position.X - 6, Position.Y + 14, Position.X - 6, Position.Y - 14) 'vert
        GraphToDraw.DrawLine(Penna, Position.X + 6, Position.Y + 14, Position.X + 6, Position.Y - 14) 'vert


        If Qualy = "Normal Open Contact" Then
        ElseIf Qualy = "Normal Closed Contact" Then
            GraphToDraw.DrawLine(Penna, Position.X - 4, Position.Y + 14, Position.X + 4, Position.Y - 14) 'diagonale
        ElseIf Qualy = "Positive Transition-sensing Contact" Then
            'lettera P
            Dim Rect1 As Drawing.SizeF
            Dim testo As String = IIf(Of String)(Preferences.GetBoolean("CoolPNGUI", False), "↑", "P")
            Br = New Drawing.SolidBrush(Col2)
            Rect1 = GraphToDraw.MeasureString(testo, Carattere)
            GraphToDraw.DrawString(testo, Carattere, Br, Position.X - (Rect1.Width / 2) + 1, Position.Y - 6)

        ElseIf Qualy = "Negative Transition-sensing Contact" Then
            'lettera N
            Dim Rect1 As Drawing.SizeF
            Dim testo As String = IIf(Of String)(Preferences.GetBoolean("CoolPNGUI", False), "↓", "N")
            Br = New Drawing.SolidBrush(Col2)
            Rect1 = GraphToDraw.MeasureString(testo, Carattere)
            GraphToDraw.DrawString(testo, Carattere, Br, Position.X - (Rect1.Width / 2) + 1, Position.Y - 6)
        End If
    End Sub

    Private Sub DrawImplCoil(ByVal DrawSmallRectangles As Boolean, ByVal Col1 As Drawing.Color, _
    ByVal Col2 As Drawing.Color, ByVal Col3 As Drawing.Color, ByVal Col4 As Drawing.Color, _
    ByVal Id As Integer, ByVal Name As String, ByVal Qualy As String, ByVal Var As BaseVariable, _
    ByVal Ind As BooleanVariable, ByVal Time As TimeSpan, ByVal Penna As Pen)

        Dim Br As Brush
        Const R As Integer = 38 ' Dichiarandola Const permettiamo al compilatore di ottimizzare

        GraphToDraw.DrawLine(Penna, Position.X - 20, Position.Y, Position.X - 7, Position.Y)
        GraphToDraw.DrawLine(Penna, Position.X + 7, Position.Y, Position.X + 20, Position.Y)
        GraphToDraw.DrawArc(Penna, (Position.X + 31) - R, Position.Y - R, R * 2, R * 2, 158, 44)
        GraphToDraw.DrawArc(Penna, (Position.X - 31) - R, Position.Y - R, R * 2, R * 2, 338, 44)

        If Qualy = "Normal Coil" Then
        ElseIf Qualy = "Negative Coil" Then
            GraphToDraw.DrawLine(Penna, Position.X - 4, Position.Y + 14, Position.X + 4, Position.Y - 14) 'diagonale
        ElseIf Qualy = "Reset Coil" Then
            'lettera R
            Dim Rect1 As Drawing.SizeF
            Dim testo As String = "R"
            Br = New Drawing.SolidBrush(Col2)
            Rect1 = GraphToDraw.MeasureString(testo, Carattere)
            GraphToDraw.DrawString(testo, Carattere, Br, Position.X - (Rect1.Width / 2) + 1, Position.Y - 6)
        ElseIf Qualy = "Set Coil" Then
            'lettera N
            Dim Rect1 As Drawing.SizeF
            Dim testo As String = "S"
            Br = New Drawing.SolidBrush(Col2)
            Rect1 = GraphToDraw.MeasureString(testo, Carattere)
            GraphToDraw.DrawString(testo, Carattere, Br, Position.X - (Rect1.Width / 2) + 1, Position.Y - 6)
        ElseIf Qualy = "Positive Transition-sensing Coil" Then
            'lettera P
            Dim Rect1 As Drawing.SizeF
            Dim testo As String = IIf(Of String)(Preferences.GetBoolean("CoolPNGUI", False), "↑", "P")
            Br = New Drawing.SolidBrush(Col2)
            Rect1 = GraphToDraw.MeasureString(testo, Carattere)
            GraphToDraw.DrawString(testo, Carattere, Br, Position.X - (Rect1.Width / 2) + 1, Position.Y - 6)
        ElseIf Qualy = "Negative Transition-sensing Coil" Then
            'lettera N
            Dim Rect1 As Drawing.SizeF
            Dim testo As String = IIf(Of String)(Preferences.GetBoolean("CoolPNGUI", False), "↓", "N")
            Br = New Drawing.SolidBrush(Col2)
            Rect1 = GraphToDraw.MeasureString(testo, Carattere)
            GraphToDraw.DrawString(testo, Carattere, Br, Position.X - (Rect1.Width / 2) + 1, Position.Y - 6)
        End If
    End Sub

    Private Sub DrawImplBlock(ByVal DrawSmallRectangles As Boolean, ByVal SelColor As Color, _
    ByVal NotSelColor As Color, ByVal TextColor As Color)
        ' Codice di disegno
        Dim Penna As New Drawing.Pen( _
            IIf(Of Drawing.Color)(m_Selected, SelColor, NotSelColor))
        Dim Size As New Size(44, 60)
        Penna.Width = 1
        GraphToDraw.DrawPath(Penna, GetRoundedRect(ObjectRectangle))
        Dim topLeft As New Point(Position.X - Size.Width / 2, Position.Y - Size.Height / 2)
        Dim strToDraw As String = Qualy
        Dim textSize As SizeF = GraphToDraw.MeasureString(strToDraw, TextFont)
        Dim textPoint As New Point(Position.X - textSize.Width / 2, _
            Position.Y - textSize.Height / 2)
        Dim textRectangle As New RectangleF(textPoint, textSize)
        textRectangle.Intersect(New Drawing.RectangleF(topLeft.X, topLeft.Y, _
            Size.Width, Size.Height))
        GraphToDraw.DrawString(strToDraw, TextFont, New SolidBrush(TextColor), textRectangle)
        ' Disegna i rettangoli di connessione FBD
        If DrawSmallRectangles Then
            Penna.Width = 2

            Penna.Color = _
                IIf(Of Drawing.Color)(InFBDRectangleSelected, SelColor, NotSelColor)
            GraphToDraw.DrawRectangle(Penna, InConnectionRectangle)

            Penna.Color = _
                IIf(Of Drawing.Color)(OutFBDRectangleSelected, SelColor, NotSelColor)
            GraphToDraw.DrawRectangle(Penna, OutConnectionRectangle)
        End If
    End Sub


    Public Overloads Sub Draw(ByVal DrawSmallRectangles As Boolean, ByVal Col1 As Drawing.Color, _
    ByVal Col2 As Drawing.Color, ByVal Col3 As Drawing.Color, ByVal Col4 As Drawing.Color, _
    ByVal Id As Integer, ByVal Name As String, ByVal Qualy As String, ByVal Var As BaseVariable, _
    ByVal Ind As BooleanVariable, ByVal Time As TimeSpan)
        If GraphToDraw Is Nothing Then Exit Sub
        If Monitor.TryEnter(GraphToDraw, 2000) Then
            Try
                'GraphToDraw.DrawRectangle(Pens.Brown, ObjectRectangle)

                Dim Penna As New Drawing.Pen(Col1)
                'Penna.Width = 2
                Dim larg As Integer
                Dim alt As Integer
                Dim Br As Drawing.Brush
                If m_Selected Then
                    'Penna.Color = SelectionColor
                Else
                    Penna.Color = Col2
                End If

                If IsContact Then
                    Me.DrawImplContact(DrawSmallRectangles, Col1, Col2, Col3, Col4, _
                        Id, Name, Qualy, Var, Ind, Time, Penna)
                ElseIf IsCoil Then
                    Me.DrawImplCoil(DrawSmallRectangles, Col1, Col2, Col3, Col4, _
                        Id, Name, Qualy, Var, Ind, Time, Penna)
                ElseIf IsBlock Then
                    Me.DrawImplBlock(DrawSmallRectangles, Col1, Col2, Col3)
                End If

                'disegna i reofori
                If DrawSmallRectangles Then
                    If TopRectSelected Then
                        Penna.Color = Col1
                    Else
                        Penna.Color = Col2
                    End If
                    larg = CInt(m_Dimension.Width / 5)
                    alt = larg
                    Penna.Width = 2
                    GraphToDraw.DrawRectangle(Penna, Position.X - CInt(m_Dimension.Width / 2) - _
                        CInt(m_Dimension.Width / 5), Position.Y - CInt(m_Dimension.Height / 10), _
                        larg, alt)

                    If BottomRectSelected Then
                        Penna.Color = Col1
                    Else
                        Penna.Color = Col2
                    End If
                    larg = CInt(m_Dimension.Width / 5)
                    alt = larg
                    Penna.Width = 2
                    GraphToDraw.DrawRectangle(Penna, Position.X + CInt(m_Dimension.Width / 2) + 1, _
                        Position.Y - CInt(m_Dimension.Height / 10), larg, alt)
                End If

                'nome del contact / coil
                Dim Rect As Drawing.SizeF
                Br = New Drawing.SolidBrush(Col2)
                If Not IsNothing(Name) Then
                    Rect = GraphToDraw.MeasureString(Name, Carattere)
                    ' se stiamo disegnando un blocco, alziamo di 3 pixels il nome (è un accorgimento
                    ' grafico hard-coded senza il quale il nome viene in parte sovrapposto al rettangolo)
                    GraphToDraw.DrawString(Name, Carattere, Br, Position.X - (Rect.Width / 2) + 1, _
                        Position.Y - (m_Dimension.Height / 1.5) - IIf(Of Integer)(IsBlock, 3, 0))
                End If

                'nome della variabile associata
                Dim Rect11 As Drawing.SizeF
                Br = New Drawing.SolidBrush(Col2)
                If Var IsNot Nothing AndAlso Var.Name IsNot Nothing Then
                    Rect11 = GraphToDraw.MeasureString(Var.Name, Carattere)
                    GraphToDraw.DrawString(Var.Name, Carattere, Br, Position.X - _
                        (Rect11.Width / 2), Position.Y + (m_Dimension.Height / 2.5))
                End If

                'Disegna lo stato se richiesto
                If DrawState Then
                    DrawStepState(False, m_Var)
                End If
            Finally
                Monitor.Exit(GraphToDraw)
            End Try
        End If
    End Sub

    Public Overrides Sub DisposeMe()
        Me.Finalize()
    End Sub

    Public Overrides Sub CalculusArea()
        'Mamorizza l'area
        Area.X = Position.X - CInt(m_Dimension.Width / 2) - 2
        Area.Y = Position.Y - CInt(m_Dimension.Height / 2) - CInt(m_Dimension.Height / 5)
        Area.Width = m_Dimension.Width + 4
        Area.Height = m_Dimension.Height + 4
    End Sub

    Public Overloads Overrides Sub Cancel(ByVal CancellSmallRectangels As Boolean, ByVal Id As Integer, _
        ByVal Name As String, ByVal Qualy As String, ByVal Var As BaseVariable, _
        ByVal Ind As BooleanVariable, ByVal Time As TimeSpan)

        Draw(CancellSmallRectangels, BackColor, BackColor, BackColor, BackColor, Id, Name, Qualy, Var, Ind, Time)
        If DrawState Then
            DrawStepState(True)
        End If

    End Sub

    Public Overloads Overrides Sub Cancel(ByVal CancellSmallRectangels As Boolean, ByVal Id As Integer)

    End Sub

    Public Overrides Function CreateInstance(ByRef NewLadder As Ladder) As Object
        Return New GraphicalContact(m_Number, m_Name, m_Documentation, _
                NewLadder, m_Initial, m_Final, Position, BackColor, SelectionColor, _
                    NotSelectionColor, TextColor, Carattere, ColorActive, ColorDeactive, _
                        ColorPreActive, Nothing, False, m_Dimension, Me.Id, Me.Name, Me.Qualy, _
                            Me.m_Var, Me.m_Ind, Me.m_time)
    End Function

    Private Function GetMyColor() As Drawing.Color
        Dim itemIsGreen As Boolean
        If m_DrawByPowerFlow Then
            ' un qualche algoritmo di decisione sul flusso di energia attraverso questo oggetto
            itemIsGreen = CalculatePowerFlowThroughMe()
        Else
            If ReadVar() Is Nothing Then Return Globals.BackColor
            itemIsGreen = ReadVar().ReadValue
        End If
        If itemIsGreen Then
            Return Color.Green
        Else
            Return Color.Red
        End If
    End Function

    Public Overloads Overrides Sub DrawStepState(ByVal Cancel As Boolean, ByVal Var As BaseVariable)
        If Monitor.TryEnter(GraphToDraw, 2000) Then
            Try
                Dim Lato As Integer = CInt(m_Dimension.Width / 4)
                Dim Br As Drawing.Brush
                If Cancel Then
                    Br = New Drawing.SolidBrush(Color.White)
                Else
                    Br = New Drawing.SolidBrush(GetMyColor())
                End If
                GraphToDraw.FillEllipse(Br, Position.X - CInt(m_Dimension.Width / 8), _
                    Position.Y + CInt(m_Dimension.Height / 7), Lato, Lato)
            Finally
                Monitor.Exit(GraphToDraw)
            End Try
        End If
    End Sub
    Public Overloads Overrides Sub DrawStepState(ByVal Cancel As Boolean)
        If GraphToDraw Is Nothing Then Exit Sub
        If Monitor.TryEnter(GraphToDraw, 2000) Then
            Try
                Dim Lato As Integer = CInt(m_Dimension.Width / 4)
                Dim Br As Drawing.Brush
                If Cancel Then
                    Br = New Drawing.SolidBrush(Color.White)
                Else
                    Br = New Drawing.SolidBrush(GetMyColor())
                End If
                GraphToDraw.FillEllipse(Br, Position.X - CInt(m_Dimension.Width / 8), _
                    Position.Y + CInt(m_Dimension.Height / 7), Lato, Lato)
            Finally
                Monitor.Exit(GraphToDraw)
            End Try
        End If
    End Sub

    Public Overrides Sub Move(ByVal dx As Integer, ByVal dy As Integer)
        Position.X = Position.X + dx
        Position.Y = Position.Y + dy
        CalculusArea()
    End Sub

    Public Overrides Sub ResolveVariablesLinks()
        m_Var = Me.m_Ladder.PouInterface.FindVariableByName(m_VarName)
        If IsNothing(m_Var) Then _
            m_Var = Me.m_Ladder.ResGlobalVariables.FindVariableByName(m_VarName)
        If m_MyOutgoingConnections IsNot Nothing AndAlso m_MyOutgoingConnections.Count > 0 Then
            Dim prevList As New GraphicalContactList(Me)
            'Salviamo tutte le connessioni uscenti da un contatto nello stesso tag <connectionPointOut>
            'ma le ricreiamo una alla volta (in effetti UniSim *non* gestisce per nulla una connessione
            'con più sorgenti o più destinazioni in Ladder, ma chissà perchè il codice sembra permetterle)
            'non conviene cercare di correggere i problemi sulle connessioni multiple, in quanto è facile
            'simularle usando più connessioni singole
            For Each tgtIndex As Integer In m_MyOutgoingConnections
                Dim tgtObject As BaseGraphicalContact = m_Ladder.GraphicalContactList.FindContactByNumber(tgtIndex)
                Dim nextList As New GraphicalContactList(tgtObject)
                ' parametri hardcoded presi da quelli usati da FormLadder
                m_Ladder.GraphicalConnectionList.AddAndDrawConnection(m_Ladder.FirstAvailableElementNumber, _
                    "C1", "", prevList, nextList, Nothing, True, True)
            Next
        End If
        If Me.IsBlock Then
            For Each i As ConnectionDescriptor In m_Inputs
                Me.m_Ladder.AddAndDrawFBDConnection(i.OtherPartyID, Number, i.Offset)
            Next
            For Each j As ConnectionDescriptor In m_Outputs
                Me.m_Ladder.AddAndDrawFBDConnection(Number, j.OtherPartyID, j.Offset)
            Next
        End If
    End Sub

    Public Overrides Sub SetGraphToDraw(ByRef Graph As System.Drawing.Graphics)
        GraphToDraw = Graph
    End Sub

    Private Sub exportContact(ByRef writer As XmlTextWriter)
        writer.WriteStartElement("contact")
        writer.WriteAttributeString("localId", m_Number.ToString())
        writer.WriteAttributeString("height", m_Dimension.Height.ToString())
        writer.WriteAttributeString("width", m_Dimension.Width.ToString())
        If Qualy = "Normal Closed Contact" Then _
            writer.WriteAttributeString("negated", "true")
        If Qualy = "Positive Transition-sensing Contact" Then _
            writer.WriteAttributeString("edge", "rising")
        If Qualy = "Negative Transition-sensing Contact" Then _
            writer.WriteAttributeString("edge", "falling")
        writer.WriteAttributeString("name", m_Name)
        writer.WriteStartElement("position")
        writer.WriteAttributeString("x", Me.Position.X.ToString())
        writer.WriteAttributeString("y", Me.Position.Y.ToString())
        writer.WriteEndElement() 'position
        writer.WriteElementString("variable", m_Var.Name)
        If Not (IsNothing(Documentation) OrElse Documentation = "") Then _
            writer.WriteElementString("documentation", Documentation)
        Me.m_Ladder.GraphicalConnectionList.MakeConnectionPointOutForObject(Me).xmlExport(writer)
        writer.WriteEndElement() 'contact
    End Sub
    Private Sub exportCoil(ByRef writer As XmlTextWriter)
        writer.WriteStartElement("coil")
        writer.WriteAttributeString("localId", m_Number.ToString())
        writer.WriteAttributeString("height", m_Dimension.Height.ToString())
        writer.WriteAttributeString("width", m_Dimension.Width.ToString())
        If Qualy = "Negative Coil" Then _
            writer.WriteAttributeString("negated", "true")
        If Qualy = "Positive Transition-sensing Coil" Then _
            writer.WriteAttributeString("edge", "rising")
        If Qualy = "Negative Transition-sensing Coil" Then _
            writer.WriteAttributeString("edge", "falling")
        If Qualy = "Set Coil" Then _
            writer.WriteAttributeString("storage", "set")
        If Qualy = "Reset Coil" Then _
            writer.WriteAttributeString("storage", "reset")
        writer.WriteAttributeString("name", m_Name)
        writer.WriteStartElement("position")
        writer.WriteAttributeString("x", Me.Position.X.ToString())
        writer.WriteAttributeString("y", Me.Position.Y.ToString())
        writer.WriteEndElement() 'position
        writer.WriteElementString("variable", m_Var.Name)
        If Not (IsNothing(Documentation) OrElse Documentation = "") Then _
            writer.WriteElementString("documentation", Documentation)
        Me.m_Ladder.GraphicalConnectionList.MakeConnectionPointOutForObject(Me).xmlExport(writer)
        writer.WriteEndElement() 'coil
    End Sub

    ' Codice adattato da GraphicalFBDBlock
    Private Sub exportBlock(ByRef writer As XmlTextWriter)
        writer.WriteStartElement("block")

        writer.WriteAttributeString("height", Size.Height)
        writer.WriteAttributeString("width", Size.Width)
        writer.WriteAttributeString("localId", Number)
        writer.WriteAttributeString("typeName", Qualy)
        writer.WriteAttributeString("instanceName", Name)

        writer.WriteStartElement("position")

        writer.WriteAttributeString("x", Position.X)
        writer.WriteAttributeString("y", Position.Y)

        writer.WriteEndElement() ' position

        writer.WriteStartElement("inputVariables")
        ' leggi tutte le connessioni che hanno Me come terminale
        Dim connections As GraphicalFBDConnectionsList = _
            Me.FindIncomingFBDConnections()
        Dim i As Integer = 1
        For Each connection As GraphicalFBDConnection In connections
            writer.WriteStartElement("variable")
            writer.WriteAttributeString("formalParameter", "IN" & i.ToString())
            If connection.Negated Then writer.WriteAttributeString("negated", "true")
            writer.WriteStartElement("connectionPointIn")
            writer.WriteStartElement("relPosition")
            writer.WriteAttributeString("x", Me.InConnectionRectangle.X)
            writer.WriteAttributeString("y", Me.InConnectionRectangle.Y + connection.Offset)
            writer.WriteEndElement() ' relPosition
            writer.WriteStartElement("connection")
            writer.WriteAttributeString("refLocalId", connection.SourceId.ToString())
            writer.WriteEndElement() ' connection
            writer.WriteEndElement() ' connectionPointIn
            writer.WriteEndElement() ' variable
            i += 1
        Next
        writer.WriteEndElement() ' inputVariables

        ' non supportiamo le variabili di ingresso/uscita per ora, ma ci mettiamo
        ' il tag (in lettura lo saltiamo)
        writer.WriteStartElement("inOutVariables")
        writer.WriteEndElement() ' inOutVariables

        ' lo standard prevede di memorizzare dentro di noi solo le uscite verso
        ' variabili, quelle verso blocchi saranno gestite come ingressi dal blocco
        i = 1
        writer.WriteStartElement("outputVariables")
        connections = Me.FindOutgoingFBDConnections()
        For Each connection As GraphicalFBDConnection In connections
            writer.WriteStartElement("variable")
            writer.WriteAttributeString("formalParameter", "OUT" & i.ToString())
            If connection.Negated Then writer.WriteAttributeString("negated", "true")
            writer.WriteStartElement("connectionPointOut")
            writer.WriteStartElement("relPosition")
            writer.WriteAttributeString("x", Me.OutConnectionRectangle.X)
            writer.WriteAttributeString("y", Me.OutConnectionRectangle.Y + connection.Offset)
            writer.WriteEndElement() ' relPosition
            ' programmazione strutturata 101% :-)
            If TypeOf (connection.DestinationObject) Is GraphicalContact Then GoTo CloseBlock
            Dim tgtVar As GraphicalVariable = CType(connection.DestinationObject, GraphicalVariable)
            writer.WriteStartElement("connection")
            writer.WriteAttributeString("refLocalId", connection.DestinationId.ToString())
            writer.WriteEndElement() ' connection
CloseBlock:
            writer.WriteEndElement() ' connectionPointOut
            writer.WriteEndElement() ' variable
            i += 1
        Next
        writer.WriteEndElement() ' outputVariables

        Me.m_Ladder.GraphicalConnectionList.MakeConnectionPointOutForObject(Me).xmlExport(writer)

        If Documentation IsNot Nothing AndAlso Documentation <> "" Then _
            writer.WriteElementString("documentation", Documentation)

        writer.WriteEndElement() ' block
    End Sub

    Public Overrides Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter)
        'Esporta l'oggetto
        If IsContact Then
            exportContact(RefXMLProjectWriter)
        ElseIf IsCoil Then
            exportCoil(RefXMLProjectWriter)
        ElseIf IsBlock Then
            exportBlock(RefXMLProjectWriter)
        End If
    End Sub

    Private Sub importContact(ByRef reader As XmlTextReader)
        Dim NodeDepth As Integer = reader.Depth
        If reader.MoveToAttribute("localId") Then
            m_Number = Integer.Parse(reader.Value)
        End If
        m_Dimension = New Size(0, 0)
        If reader.MoveToAttribute("height") Then
            m_Dimension.Height = Integer.Parse(reader.Value)
        End If
        If reader.MoveToAttribute("width") Then
            m_Dimension.Width = Integer.Parse(reader.Value)
        End If
        ' ricostruiamo il tipo di contatto in base agli attributi
        ' (non è una tecnica elegantissima, ma lo standard non offre di meglio, e l'uso di stringhe non
        ' aiuta particolarmente)
        If reader.MoveToAttribute("negated") Then
            If reader.Value = "true" Then Qualy = "Normal Closed Contact"
        ElseIf reader.MoveToAttribute("edge") Then
            If reader.Value = "rising" Then
                Qualy = "Positive Transition-sensing Contact"
            Else
                Qualy = "Negative Transition-sensing Contact"
            End If
        Else
            Qualy = "Normal Open Contact"
        End If
        If reader.MoveToAttribute("name") Then
            m_Name = reader.Value
        End If
        ' Lo scheletro di questo codice è tratto da GraphicalStep.vb (nella cartella SFC)
        If Not reader.IsEmptyElement Then
            reader.Read()
            While reader.Depth > NodeDepth
                Select Case reader.Name
                    Case "position"
                        If reader.NodeType = XmlNodeType.Element Then
                            If reader.MoveToAttribute("x") Then
                                Position.X = reader.Value
                            End If
                            If reader.MoveToAttribute("y") Then
                                Position.Y = reader.Value
                            End If
                        End If
                    Case "documentation"
                        If reader.NodeType = XmlNodeType.Element Then
                            Me.Documentation = reader.ReadString()
                        End If
                    Case "variable"
                        If reader.NodeType = XmlNodeType.Element Then
                            Me.m_VarName = reader.ReadString()
                        End If
                    Case "connectionPointOut"
                        If reader.NodeType = XmlNodeType.Element Then
                            m_MyOutgoingConnections = New ArrayList()
                            Dim outconn As New ConnectionPointOut(m_MyOutgoingConnections)
                            outconn.xmlImport(reader)
                            m_MyOutgoingConnections = outconn.Connections
                        End If
                End Select
                If reader.Depth > NodeDepth Then
                    reader.Read()
                End If
            End While
        End If
    End Sub

    Private Sub importCoil(ByRef reader As XmlTextReader)
        Dim NodeDepth As Integer = reader.Depth
        If reader.MoveToAttribute("localId") Then
            m_Number = Integer.Parse(reader.Value)
        End If
        m_Dimension = New Size(0, 0)
        If reader.MoveToAttribute("height") Then
            m_Dimension.Height = Integer.Parse(reader.Value)
        End If
        If reader.MoveToAttribute("width") Then
            m_Dimension.Width = Integer.Parse(reader.Value)
        End If
        ' ricostruiamo il tipo di bobina in base agli attributi
        ' (non è una tecnica elegantissima, ma lo standard non offre di meglio, e l'uso di stringhe non
        ' aiuta particolarmente)
        If reader.MoveToAttribute("negated") Then
            If reader.Value = "true" Then Qualy = "Negative Coil"
        ElseIf reader.MoveToAttribute("edge") Then
            If reader.Value = "rising" Then
                Qualy = "Positive Transition-sensing Coil"
            Else
                Qualy = "Negative Transition-sensing Coil"
            End If
        ElseIf reader.MoveToAttribute("storage") Then
            If reader.Value = "set" Then
                Qualy = "Set Coil"
            Else
                Qualy = "Reset Coil"
            End If
        Else
            Qualy = "Normal Coil"
        End If
        If reader.MoveToAttribute("name") Then
            m_Name = reader.Value
        End If
        ' Lo scheletro di questo codice è tratto da GraphicalStep.vb (nella cartella SFC)
        If Not reader.IsEmptyElement Then
            reader.Read()
            While reader.Depth > NodeDepth
                Select Case reader.Name
                    Case "position"
                        If reader.NodeType = XmlNodeType.Element Then
                            If reader.MoveToAttribute("x") Then
                                Position.X = reader.Value
                            End If
                            If reader.MoveToAttribute("y") Then
                                Position.Y = reader.Value
                            End If
                        End If
                    Case "documentation"
                        If reader.NodeType = XmlNodeType.Element Then
                            Me.Documentation = reader.ReadString()
                        End If
                    Case "variable"
                        If reader.NodeType = XmlNodeType.Element Then
                            Me.m_VarName = reader.ReadString()
                        End If
                    Case "connectionPointOut"
                        If reader.NodeType = XmlNodeType.Element Then
                            m_MyOutgoingConnections = New ArrayList()
                            Dim outconn As New ConnectionPointOut(m_MyOutgoingConnections)
                            outconn.xmlImport(reader)
                            m_MyOutgoingConnections = outconn.Connections
                        End If
                End Select
                If reader.Depth > NodeDepth Then
                    reader.Read()
                End If
            End While
        End If
    End Sub

    ' Codice adattato da GraphicalFBDBlock

    Private Sub ParseInputVariable(ByRef reader As System.Xml.XmlTextReader, ByVal neg As Boolean)
        Dim NodeDepth As Integer = reader.Depth

        If reader.MoveToAttribute("formalParameter") Then
            Nop()
        End If

        If Not reader.IsEmptyElement Then
            reader.Read()
            Dim cond As New ConnectionDescriptor()
            cond.Negated = neg
            While reader.Depth > NodeDepth
                Select Case reader.Name
                    Case "connectionPointIn"
                    Case "relPosition"
                        If reader.MoveToAttribute("y") Then
                            cond.Offset = Int32.Parse(reader.Value) - Me.InConnectionRectangle.Y
                        End If
                    Case "connection"
                        Dim lid As Integer = 0
                        If reader.MoveToAttribute("refLocalId") Then
                            cond.OtherPartyID = Int32.Parse(reader.Value)
                        End If
                        'MsgBox("I" & lid)
                        m_Inputs.Add(cond)
                End Select
                If reader.Depth > NodeDepth Then
                    reader.Read()
                End If
            End While
        End If
    End Sub

    Private Sub ParseOutputVariable(ByRef reader As System.Xml.XmlTextReader, ByVal neg As Boolean)
        Dim NodeDepth As Integer = reader.Depth

        If reader.MoveToAttribute("formalParameter") Then
            Nop()
        End If

        If Not reader.IsEmptyElement Then
            reader.Read()
            Dim cond As New ConnectionDescriptor()
            cond.Negated = neg
            While reader.Depth > NodeDepth
                Select Case reader.Name
                    Case "connectionPointOut"
                    Case "relPosition"
                        If reader.MoveToAttribute("y") Then
                            cond.Offset = Int32.Parse(reader.Value) - Me.OutConnectionRectangle.Y
                        End If
                    Case "connection"
                        Dim lid As Integer = 0
                        If reader.MoveToAttribute("refLocalId") Then
                            cond.OtherPartyID = Int32.Parse(reader.Value)
                        End If
                        'MsgBox("O" & lid)
                        m_Outputs.Add(cond)
                End Select
                If reader.Depth > NodeDepth Then
                    reader.Read()
                End If
            End While
        End If
    End Sub

    Private Sub ParseInputVariables(ByRef reader As System.Xml.XmlTextReader)
        Dim NodeDepth As Integer = reader.Depth
        If Not reader.IsEmptyElement Then
            reader.Read()
            While reader.Depth > NodeDepth
                Select Case reader.Name
                    Case "variable"
                        Dim neg As Boolean = reader.MoveToAttribute("negated")
                        If neg Then
                            neg = reader.Value.Equals("true")
                            reader.MoveToElement()
                        End If
                        ParseInputVariable(reader, neg)
                End Select
                If reader.Depth > NodeDepth Then
                    reader.Read()
                End If
            End While
        End If
    End Sub

    ' Il codice sia qui sia in GraphicalFBDBlock e' *SBAGLIATO* e funziona solo
    ' perchè questo elemento è vuoto... correggere non appena questa assunzione non
    ' sarà più verificata
    Private Sub ParseInOutVariables(ByRef reader As System.Xml.XmlTextReader)
        While (reader.NodeType = XmlNodeType.EndElement AndAlso reader.Name = "inOutVariables")
            reader.Read()
        End While
    End Sub

    Private Sub ParseOutputVariables(ByRef reader As System.Xml.XmlTextReader)
        Dim NodeDepth As Integer = reader.Depth
        If Not reader.IsEmptyElement Then
            reader.Read()
            While reader.Depth > NodeDepth
                Select Case reader.Name
                    Case "variable"
                        Dim neg As Boolean = reader.MoveToAttribute("negated")
                        If neg Then
                            neg = reader.Value.Equals("true")
                            reader.MoveToElement()
                        End If
                        ParseOutputVariable(reader, neg)
                End Select
                If reader.Depth > NodeDepth Then
                    reader.Read()
                End If
            End While
        End If
    End Sub

    Private Sub importBlock(ByRef reader As XmlTextReader)
        Dim NodeDepth As Integer = reader.Depth

        Dim w, h As Integer

        If reader.MoveToAttribute("height") Then
            h = Integer.Parse(reader.Value)
        End If
        If reader.MoveToAttribute("width") Then
            w = Integer.Parse(reader.Value)
        End If

        Size = New Size(w, h)

        If reader.MoveToAttribute("localId") Then
            Number = Integer.Parse(reader.Value)
        End If

        If reader.MoveToAttribute("typeName") Then
            Qualy = reader.Value
        End If

        If reader.MoveToAttribute("instanceName") Then
            Name = reader.Value
        End If

        Dim x, y As Integer

        ' Lo scheletro di questo codice è tratto da GraphicalStep.vb (nella cartella SFC)
        If Not reader.IsEmptyElement Then
            reader.Read()
            While reader.Depth > NodeDepth
                Select Case reader.Name
                    Case "position"
                        If reader.NodeType = XmlNodeType.Element Then
                            If reader.MoveToAttribute("x") Then
                                x = reader.Value
                            End If
                            If reader.MoveToAttribute("y") Then
                                y = reader.Value
                            End If
                            Position = New Point(x, y)
                        End If
                    Case "documentation"
                        If reader.NodeType = XmlNodeType.Element Then
                            Me.Documentation = reader.ReadString()
                        End If
                    Case "inputVariables"
                        ParseInputVariables(reader)
                    Case "inOutVariables"
                        ParseInOutVariables(reader)
                    Case "outputVariables"
                        ParseOutputVariables(reader)
                    Case "connectionPointOut"
                        If reader.NodeType = XmlNodeType.Element Then
                            m_MyOutgoingConnections = New ArrayList()
                            Dim outconn As New ConnectionPointOut(m_MyOutgoingConnections)
                            outconn.xmlImport(reader)
                            m_MyOutgoingConnections = outconn.Connections
                        End If
                End Select
                If reader.Depth > NodeDepth Then
                    reader.Read()
                End If
            End While
        End If
    End Sub


    Public Overrides Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader)
        ' solo i tag <contact> <coil> e <block> devono "finire" in questa classe
        If RefXmlProjectReader.Name = "contact" Then
            Id = 1
            importContact(RefXmlProjectReader)
        ElseIf RefXmlProjectReader.Name = "coil" Then
            Id = 2
            importCoil(RefXmlProjectReader)
        ElseIf RefXmlProjectReader.Name = "block" Then
            Id = 3
            importBlock(RefXmlProjectReader)
        End If
        CalculusArea()
    End Sub
    WriteOnly Property ResGlobalVariables() As VariablesLists
        Set(ByVal Value As VariablesLists)
            'Comunica al body l'interfaccia della POU
            'm_Body.ResGlobalVariables = Value
        End Set
    End Property

    Public Property Size() As Size Implements ISizable.Size
        Get
            Return ReadDimension()
        End Get
        Set(ByVal value As Size)
            SetDimension(value)
        End Set
    End Property

    Public Function FindIncomingFBDConnections() As GraphicalFBDConnectionsList
        Dim ret As New GraphicalFBDConnectionsList(m_Ladder)
        For Each conn As GraphicalFBDConnection In m_Ladder.GraphicalFBDConnectionsList
            If conn.DestinationId = Me.Number Then ret.Add(conn)
        Next
        Return ret
    End Function

    Public Function FindOutgoingFBDConnections() As GraphicalFBDConnectionsList
        Dim ret As New GraphicalFBDConnectionsList(m_Ladder)
        For Each conn As GraphicalFBDConnection In m_Ladder.GraphicalFBDConnectionsList
            If conn.SourceId = Me.Number Then ret.Add(conn)
        Next
        Return ret
    End Function

    ' Ritorna gli indici in GraphicalConnectionsList delle connessioni entranti qui
    ' (nell'ordine in cui verranno poi valutate quando si esegue il diagramma)
    Public Function GetMyArgumentsIndexes() As List(Of Integer)
        Dim ret As New List(Of Integer)
        For Each connection As GraphicalFBDConnection In Me.FindIncomingFBDConnections()
            ret.Add(m_Ladder.GraphicalFBDConnectionsList.IndexOf(connection))
        Next
        Return ret
    End Function

End Class