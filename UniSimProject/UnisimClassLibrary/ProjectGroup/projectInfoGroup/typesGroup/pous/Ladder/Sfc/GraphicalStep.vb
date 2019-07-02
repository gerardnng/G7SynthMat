Imports System.IO
Imports System.Math
Imports System.Xml
Imports System.Threading
Imports System.Collections.Generic
Imports System.Drawing

Public Class GraphicalStep
    Inherits BaseGraphicalStep
    Protected m_ActionBlock As ActionBlock
    Protected m_PulseActionExecuted As Boolean
    Protected m_NotPulseActionExecuted As Boolean
    Protected m_pouslist As Pous
    'Questo tipo di fase può contenere azioni
    'Funzioni per la fase
    Public Sub New(ByVal StepNumber As Integer, ByVal StepName As String, ByVal StepDocumentation As String, ByRef RefSfc As Sfc, ByVal Ini As Boolean, ByVal Fin As Boolean, ByVal P As Drawing.Point, ByVal BackCol As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, ByRef Graph As Drawing.Graphics, ByVal DrState As Boolean, ByVal Dimension As Integer, ByRef pouslist As Pous)
        MyBase.New(StepNumber, StepName, StepDocumentation, RefSfc, Ini, Fin, P, BackCol, SelectionCol, NotSelectionCol, TextCol, Car, ColActive, ColDeactive, ColPreActive, Graph, DrState, Dimension)
        'Calcola la posizione dell'actionBlock
        Dim AcBolockPosition As New Drawing.Point
        AcBolockPosition.X = m_Position.X + m_Dimension
        AcBolockPosition.Y = m_Position.Y
        m_ActionBlock = New ActionBlock(m_Dimension, RefSfc, AcBolockPosition, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, GraphToDraw, pouslist)
        'Aggiunge la fase ai collegamenti precedenti
        m_ActionBlock.XmlPreviousConnectionsList.Add(Me)
        CalculateArea()
        m_pouslist = pouslist

    End Sub
    Public Sub New(ByRef RefSfc As Sfc, ByVal BackCol As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, ByRef Graph As Drawing.Graphics, ByVal DrState As Boolean, ByVal Dimension As Integer, ByRef pouslist As Pous)
        MyBase.New(RefSfc, BackCol, SelectionCol, NotSelectionCol, TextCol, Car, ColActive, ColDeactive, ColPreActive, Graph, DrState, Dimension)
        'Calcola la posizione dell'actionBlock

        Dim AcBolockPosition As New Drawing.Point
        AcBolockPosition.X = m_Position.X + m_Dimension
        AcBolockPosition.Y = m_Position.Y
        m_ActionBlock = New ActionBlock(m_Dimension, RefSfc, AcBolockPosition, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, GraphToDraw, pouslist)
        CalculateArea()
        m_pouslist = pouslist

    End Sub
    Public Overrides Function CreateInstance(ByRef NewSfc As Sfc) As Object
        'd() 'im_ActionBlock.CreateInstance()
        CreateInstance = New GraphicalStep(m_Number, m_Name, m_Documentation, NewSfc, m_Initial, m_final, m_Position, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, ColorActive, ColorDeactive, ColorPreActive, Nothing, False, m_Dimension, m_pouslist)

    End Function
    Public Overrides Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter)
        'Esporta la fase
        RefXMLProjectWriter.WriteStartElement("step")
        'Attributi della fase
        RefXMLProjectWriter.WriteAttributeString("name", m_Name)
        RefXMLProjectWriter.WriteAttributeString("height", m_Dimension.ToString)
        RefXMLProjectWriter.WriteAttributeString("width", m_Dimension.ToString)
        RefXMLProjectWriter.WriteAttributeString("localId", m_Number.ToString)
        RefXMLProjectWriter.WriteAttributeString("initialStep", m_Initial.ToString.ToLower)
        If m_Final Then _
            RefXMLProjectWriter.WriteAttributeString("finalStep", m_Final.ToString.ToLower)
        RefXMLProjectWriter.WriteAttributeString("negated", m_negated.ToString.ToLower)

        'Position
        RefXMLProjectWriter.WriteStartElement("position")
        'Attributi di Position
        'Nella fase position si riferisce al centro quindi sottrae CInt(m_Dimension / 2)
        RefXMLProjectWriter.WriteAttributeString("x", (m_Position.X - CInt(m_Dimension / 2)).ToString)
        RefXMLProjectWriter.WriteAttributeString("y", (m_Position.Y - CInt(m_Dimension / 2)).ToString)

        RefXMLProjectWriter.WriteEndElement() 'Position

        'ConnectionPointIn
        Dim ConnectionsList As New ConnectionPointIn(XmlPreviousConnectionsList)
        ConnectionsList.xmlExport(RefXMLProjectWriter)

        'Esporta documentation
        If Documentation <> "" Then
            RefXMLProjectWriter.WriteElementString("documentation", Me.Documentation)  'documentation
        End If
        RefXMLProjectWriter.WriteEndElement() 'fase

        'Esporta l'actionBlock

        If m_ActionBlock.ActionList.Count > 0 Then
            m_ActionBlock.xmlExport(RefXMLProjectWriter)
        End If
    End Sub
    Public Overrides Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader)
        'Memorizza la profondità del nodo
        Dim NodeDepth As Integer = RefXmlProjectReader.Depth

        'Legge gli attributi
        If RefXmlProjectReader.MoveToAttribute("name") Then
            m_name = RefXmlProjectReader.Value
        End If
        If RefXmlProjectReader.MoveToAttribute("localId") Then
            m_number = RefXmlProjectReader.Value
        End If
        If RefXmlProjectReader.MoveToAttribute("initialStep") Then
            m_initial = RefXmlProjectReader.Value
        End If
        If RefXmlProjectReader.MoveToAttribute("finalStep") Then
            m_Final = RefXmlProjectReader.Value
        End If
        If RefXmlProjectReader.MoveToAttribute("negated") Then
            m_negated = RefXmlProjectReader.Value
        End If
        'Se l'elemento non è vuoto si sposta sul nodo successivo
        If Not RefXmlProjectReader.IsEmptyElement Then
            RefXmlProjectReader.Read()
            While RefXmlProjectReader.Depth > NodeDepth
                Select Case RefXmlProjectReader.Name
                    Case "position"
                        'Nella fase position si riferisce al centro quindi aggiunge CInt(m_Dimension / 2)
                        'Controlla se è l'inizio dell'elemento
                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                            'Legge gli attributi di position
                            If RefXmlProjectReader.MoveToAttribute("x") Then
                                m_Position.X = RefXmlProjectReader.Value + CInt(m_Dimension / 2)
                            End If
                            If RefXmlProjectReader.MoveToAttribute("y") Then
                                m_Position.Y = RefXmlProjectReader.Value + CInt(m_Dimension / 2)
                            End If
                        End If
                    Case "connectionPointIn"
                        'Controlla se è l'inizio dell'elemento
                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                            Dim ConnectionsList As New ConnectionPointIn(XmlPreviousConnectionsList)
                            ConnectionsList.xmlImport(RefXmlProjectReader)
                        End If
                        ' Supporta il tag <comment> per retrocompatibilità con la 0.4.5L
                        ' (versione non ufficiale usata nell'anno accademico 2006/2007)
                        ' (salva sempre usando il tag standard <documentation>)
                    Case "documentation", "comment"
                        'Controlla se è l'inizio dell'elemento
                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                            Documentation = RefXmlProjectReader.ReadString()
                        End If
                End Select
                'Si sposta sul nodo successivo se non è la fine dell'elemento
                If RefXmlProjectReader.Depth > NodeDepth Then
                    RefXmlProjectReader.Read()
                End If

            End While
            'Calcola la posizione dell'actionBlock
            Dim AcBolockPosition As New Drawing.Point
            AcBolockPosition.X = m_Position.X + m_Dimension
            AcBolockPosition.Y = m_Position.Y
            m_ActionBlock.Position = AcBolockPosition
            'Aggiunge all'actionBlock il collegamento precedente
            m_ActionBlock.XmlPreviousConnectionsList.Add(Me)
            'Aggiorna l'area della fase
            CalculateArea()
        End If
    End Sub
    Public Overrides Sub ResolveVariablesLinks()
        'Risolve i riferimenti dei nomi di variabili nella azioni
        m_ActionBlock.ResolveVariablesLinks()
    End Sub
    Public Overrides Sub DisposeMe()
        m_ActionBlock.DisposeMe()
        Me.Finalize()
    End Sub
    Public Sub ExecuteActions()
        If Not m_preactive And Not m_Active Then
            If m_NotPulseActionExecuted Then
                StopNotPulseActions()
                m_NotPulseActionExecuted = False
            End If
            m_PulseActionExecuted = False
        Else
            'Esegue le azioni impulsive o non impulsive se non era già eseguite
            If m_preactive And Not m_PulseActionExecuted Then
                ExecutePulseActions()
                m_PulseActionExecuted = True
            Else
                If m_active And Not m_NotPulseActionExecuted Then
                    ExecuteNotPulseActions()
                    m_NotPulseActionExecuted = True
                Else
                End If
            End If
        End If
    End Sub
    Public Overrides Sub Active()
        Dim StateChanged As Boolean = Not m_Active
        m_Active = True
        m_TimeActivation = Now  'Imposta l'istate di attivazione
        m_PreActive = False
        'Disegna lo stato se richiesto
        If StateChanged And DrawState Then
            DrawStepState(False)
        End If
    End Sub
    Public Overrides Sub Disactive()
        'Termina le azioni non impulsive se era ancora attiva
        m_Active = False
        m_preactive = False
        'Disegna lo stato se è cambiato e se richiesto
        If DrawState Then
            DrawStepState(False)
        End If
    End Sub
    Public Overrides Sub PreActive()
        Dim StateChanged As Boolean = Not m_PreActive
        m_PreActive = True
        'Disegna se richiesto
        If StateChanged And DrawState Then
            DrawStepState(False)
        End If
    End Sub
    Public Overloads Overrides Sub Draw(ByVal DrawSmallRectangels As Boolean)
        Draw(DrawSmallRectangels, SelectionColor, NotSelectionColor, TextColor, BackColor)
        'Disegna lo stato se richiesto
        If DrawState Then
            DrawStepState(False)
        End If
        'Disegna le azioni
        If m_ActionBlock.ActionList.Count > 0 Then
            Dim penna As New Drawing.Pen(NotSelectionColor)
            'Line tra fase e prima azione
            GraphToDraw.DrawLine(penna, m_Position.X + CInt((m_Dimension / 2)), m_Position.Y, m_Position.X + m_Dimension, m_Position.Y)
            m_ActionBlock.DrawActions()
        End If
    End Sub
    Public Overrides Sub Cancel(ByVal CancellSmallRectangels As Boolean)
        Draw(CancellSmallRectangels, BackColor, BackColor, BackColor, BackColor)
        'Cancella lo stato se richiesto
        If DrawState Then
            DrawStepState(True)
        End If
        'Cancella le azioni
        'Disegna le azioni
        If m_ActionBlock.ActionList.Count > 0 Then
            Dim penna As New Drawing.Pen(BackColor)
            'Line tra fase e prima azione
            GraphToDraw.DrawLine(penna, m_Position.X + CInt((m_Dimension / 2)), m_Position.Y, m_Position.X + m_Dimension, m_Position.Y)
            m_ActionBlock.CalcelActions()
        End If
    End Sub
    Public Overloads Sub Draw(ByVal DrawSmallRectangels As Boolean, ByVal Col1 As Drawing.Color, ByVal Col2 As Drawing.Color, ByVal Col3 As Drawing.Color, ByVal Col4 As Drawing.Color)
        Try
            If Monitor.TryEnter(Graphtodraw, 2000) Then
                Dim Penna As New Drawing.Pen(Col1)
                Dim larg As Integer
                Dim alt As Integer
                Dim Br As Drawing.Brush
                If m_Selected Then
                    'Penna.Color = SelectionColor
                Else
                    Penna.Color = Col2
                End If
                Penna.Width = 1
                Dim R As New Drawing.Rectangle(m_Position.X - CInt(m_Dimension / 2), m_Position.Y - CInt(m_Dimension / 2), m_Dimension, m_Dimension)
                GraphToDraw.DrawRectangle(Penna, R)
                'Rettangolo vuoto al centro
                'GraphToDraw.FillRectangle(Br, R.X + 1, R.Y + 1, R.Width - 1, R.Height - 1)
                'Rectangle interno se iniziale
                If m_Initial Then
                    Penna.Width = 1
                    GraphToDraw.DrawRectangle(Penna, m_Position.X - CInt((m_Dimension - 8) / 2), m_Position.Y - CInt((m_Dimension - 8) / 2), m_Dimension - 8, m_Dimension - 8)
                End If
                'Freccia se finale
                If m_Final Then
                    Penna.Width = 1
                    GraphToDraw.DrawLine(Penna, m_Position.X - CInt(m_Dimension / 4), m_Position.Y - CInt(m_Dimension / 4), m_Position.X + CInt(m_Dimension / 4), m_Position.Y - CInt(m_Dimension / 4))
                    GraphToDraw.DrawLine(Penna, m_Position.X + CInt(m_Dimension / 6), m_Position.Y - CInt(m_Dimension / 4) - CInt(m_Dimension / 8), m_Position.X + CInt(m_Dimension / 4), m_Position.Y - CInt(m_Dimension / 4))
                    GraphToDraw.DrawLine(Penna, m_Position.X + CInt(m_Dimension / 6), m_Position.Y - CInt(m_Dimension / 4) + CInt(m_Dimension / 8), m_Position.X + CInt(m_Dimension / 4), m_Position.Y - CInt(m_Dimension / 4))
                End If
                'Quadratini se richiesto
                If DrawSmallRectangels Then
                    If TopRectSelected Then
                        Penna.Color = Col1
                    Else
                        Penna.Color = Col2
                    End If
                    larg = CInt(m_Dimension / 5)
                    alt = larg
                    Penna.Width = 2
                    GraphToDraw.DrawRectangle(Penna, m_Position.X - CInt(m_Dimension / 10), m_Position.Y - CInt(m_Dimension / 2) - CInt(m_Dimension / 5), larg, alt)
                    If BottomRectSelected Then
                        Penna.Color = Col1
                    Else
                        Penna.Color = Col2
                    End If
                    larg = CInt(m_Dimension / 5)
                    alt = larg
                    Penna.Width = 2
                    GraphToDraw.DrawRectangle(Penna, m_Position.X - CInt(m_Dimension / 10), m_Position.Y + CInt(m_Dimension / 2) + 1, larg, alt)
                End If
                'Testo nella fase
                Dim Rect As Drawing.SizeF
                Br = New Drawing.SolidBrush(Col2)
                If Not IsNothing(m_Name) Then
                    Rect = GraphToDraw.MeasureString(m_Name, Carattere)
                    'Controlla se il nome è più largo
                    If Rect.Width < m_dimension Then
                        GraphToDraw.DrawString(m_Name, Carattere, Br, m_Position.X - (Rect.Width / 2) + 1, m_Position.Y - (Rect.Height / 2))
                    Else
                        Dim NewRect As New Drawing.RectangleF(m_Position.X - m_dimension / 2 + 4, m_Position.Y - (Rect.Height / 2), m_dimension - 4, Rect.Height)
                        GraphToDraw.DrawString(m_Name, Carattere, Br, NewRect)
                    End If
                End If

            End If
        Catch ex As System.Exception
        Finally
            Monitor.Exit(GraphToDraw)
        End Try
    End Sub
    Public Overrides Sub DrawStepState(ByVal Cancel As Boolean)
        Try
            If Monitor.TryEnter(Graphtodraw, 2000) Then
                Dim Lato As Integer = CInt(m_Dimension / 4)
                Dim Br As Drawing.Brush
                If Cancel Then
                    Br = New Drawing.SolidBrush(backcolor)
                Else
                    If m_PreActive And Not m_Active Then
                        Br = New Drawing.SolidBrush(ColorPreActive)
                    Else
                        If m_Active Then
                            Br = New Drawing.SolidBrush(ColorActive)
                        Else
                            Br = New Drawing.SolidBrush(ColorDeactive)
                        End If
                    End If
                End If
                Graphtodraw.FillEllipse(Br, m_Position.X - CInt(m_Dimension / 8), m_Position.Y + CInt(m_Dimension / 7), Lato, Lato)
            End If
        Catch ex As System.Exception
        Finally
            Monitor.Exit(GraphToDraw)
        End Try
    End Sub
    Public Overrides Sub SetGraphToDraw(ByRef Graph As Drawing.Graphics)
        GraphToDraw = Graph
        m_ActionBlock.SetGraphToDraw(Graph)
    End Sub
    Public Overrides Sub CalculateArea()
        'Mamorizza l'area
        Area.X = m_Position.X - CInt(m_Dimension / 2) - 2
        Area.Y = m_Position.Y - CInt(m_Dimension / 2) - CInt(m_Dimension / 5)
        Area.Width = m_Dimension + 4
        Area.Height = m_Dimension + 4
        If m_ActionBlock.ActionList.Count > 0 Then
            Area = Drawing.Rectangle.Union(Area, m_ActionBlock.CalculusArea)
        End If
    End Sub
    Public Overrides Sub Move(ByVal dx As Integer, ByVal dy As Integer)
        m_Position.X = m_Position.X + dx
        m_Position.Y = m_Position.Y + dy
        'Muove le azioni
        m_ActionBlock.Move(dx, dy)
        CalculateArea()
    End Sub
    'Funzioni per le azioni
    Public Sub SetActionBlock(ByRef AcBlock As ActionBlock)
        m_ActionBlock = AcBlock
        'Calcola la posizione dell'actionBlock
        Dim AcBolockPosition As New Drawing.Point
        AcBolockPosition.X = m_Position.X + m_Dimension
        AcBolockPosition.Y = m_Position.Y
        m_ActionBlock.Position = AcBolockPosition
        'Svuota le connesisoni dell'ActionBlock
        m_ActionBlock.XmlPreviousConnectionsList.Clear()
        'Aggiunge all'actionBlock il collegamento precedente
        m_ActionBlock.XmlPreviousConnectionsList.Add(Me)
        'Aggiorna l'area della fase
        CalculateArea()
    End Sub
    Public Sub AddAndDrawAction(ByVal Nom As String, ByVal Qual As String, ByRef Var As BaseVariable, ByRef VarInd As BaseVariable, ByVal T As TimeSpan, ByRef RefSfc As Sfc, ByVal StepsToForces As GraphicalStepsList, ByVal ArithExp As String)
        m_ActionBlock.AddAndDrawAction(Nom, Qual, Var, VarInd, m_Dimension, T, RefSfc, StepsToForces, ArithExp, BackColor, SelectionColor, NotSelectionColor, TextColor, Carattere, GraphToDraw)
        CalculateArea()
    End Sub
    Private Sub CancelActionBlock()
        If m_ActionBlock.ActionList.Count > 0 Then
            Dim penna As New Drawing.Pen(BackColor)
            'Line tra fase e prima azione
            GraphToDraw.DrawLine(penna, m_Position.X + CInt((m_Dimension / 2)), m_Position.Y, m_Position.X + m_Dimension, m_Position.Y)
            m_ActionBlock.CalcelActions()
        End If
    End Sub
    Public Function FindAction(ByVal x As Integer, ByVal y As Integer) As Boolean
        'Trova solo un'azione della fase
        If m_ActionBlock.ActionList.Count > 0 Then
            FindAction = m_ActionBlock.FindAction(x, y)
        End If
    End Function
    Public Function FindAndSelectAction(ByVal x As Integer, ByVal y As Integer) As Boolean
        'Trova solo un'azione della fase
        If m_ActionBlock.ActionList.Count > 0 Then
            FindAndSelectAction = m_ActionBlock.FindAndSelectAction(x, y)
        End If
    End Function
    Public Function ReadIfActionSelected(ByVal x As Integer, ByVal y As Integer) As Boolean
        'Trova la prima azione in x,y
        If m_ActionBlock.ActionList.Count > 0 Then
            ReadIfActionSelected = m_ActionBlock.ReadIfActionSelected(x, y)
        End If
    End Function
    Public Function FindAndSelectActions(ByVal Rect As Drawing.Rectangle) As Boolean
        'Seleziona tutte le azioni della fase che trova
        If m_ActionBlock.ActionList.Count > 0 Then
            FindAndSelectActions = m_ActionBlock.FindAndSelectActions(Rect)
        End If
    End Function
    Public Function ReadSelectedAction() As GraphicalAction
        'Legge solo la prima azione selezionata
        If m_ActionBlock.ActionList.Count > 0 Then
            Return m_ActionBlock.ReadSelectedAction()
        Else
            Return Nothing
        End If
    End Function
    Public Function CountActionsInList() As Integer
        Return m_ActionBlock.ActionList.Count
    End Function
    Public Function ReadListActions() As ArrayList
        Return m_ActionBlock.ActionList
    End Function
    Public Sub RemoveSelectedActions()
        m_ActionBlock.RemoveSelectedActions()
        CalculateArea()
    End Sub
    Public Function ReadSelectedActionsList() As List(Of GraphicalAction)
        Return m_ActionBlock.ReadSelectedActionsList()
    End Function
    Public Function CountSelectedActions() As Integer
        Return m_ActionBlock.CountSelectedActions()
    End Function
    Public Sub DeSelectActions()
        m_ActionBlock.DeSelectActions()
    End Sub
    Public Sub ExecutePulseActions()
        'Esegue le azioni impulsive
        m_ActionBlock.ExecutePulseActions()
    End Sub
    Public Sub ExecuteNotPulseActions()
        'Esegue le azione non impulsive
        m_ActionBlock.ExecuteNotPulseActions()
    End Sub
    Public Sub StopNotPulseActions()
        'Termina le azioni i non impulsive
        m_ActionBlock.StopNotPulseActions()
    End Sub
    Public Sub ResetActions()
        m_PreActive = False
        m_Active = False
        m_PulseActionExecuted = False
        m_NotPulseActionExecuted = False
        m_ActionBlock.ResetActions()
    End Sub
    Public Sub ResetActionsForSus()
        'Disattiva le azioni durante la sospensione
        m_ActionBlock.ResetActionsForSus()
    End Sub
    Public Sub ActiveActionsForFr()
        'Attiva le azioni delle fasi forzate
        m_ActionBlock.ActiveActionsForFr()
    End Sub
    Public Overrides Function GetIdentifier() As String
        Return "Step " + Me.Name
    End Function
    Public Overrides Function GetDescription() As String
        Return GetSystemDescription() + IIf(Of String)(Documentation = "", "", " (" + Documentation + ")")
    End Function
    Public Overrides Function GetSystemDescription() As String
        Dim desc As String = GetIdentifier()
        If Me.ReadInitial() Then
            desc += " is initial"
        End If
        If Me.ReadFinal() Then
            desc += " is final"
        End If
        Return desc
    End Function
End Class
