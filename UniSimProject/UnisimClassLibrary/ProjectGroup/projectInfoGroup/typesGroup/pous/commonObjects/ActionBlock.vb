Imports System.Xml
Imports System.Collections.Generic
Public Class ActionBlock
    Implements IXMLSerializable

    Protected m_documentation As String
    Protected m_Sfc As Sfc
    Protected m_Dimension As Integer
    Protected m_Position As Drawing.Point
    Protected m_Area As Drawing.Rectangle
    Protected m_BackColor As Drawing.Color
    Protected m_SelectionColor As Drawing.Color
    Protected m_NotSelectionColor As Drawing.Color
    Protected m_TextColor As Drawing.Color
    Protected m_Car As Drawing.Font
    Protected ColorActive As Drawing.Color
    Protected ColorPreActive As Drawing.Color
    Protected ColorDeactive As Drawing.Color
    Protected m_GraphToDraw As Drawing.Graphics
    Protected m_ActionList As ArrayList
    Protected m_pouslist As Pous
    Public XmlPreviousConnectionsList As ArrayList
    Public Property ActionList() As ArrayList
        Get
            ActionList = m_ActionList
        End Get
        Set(ByVal Value As ArrayList)
            m_ActionList = Value
        End Set
    End Property
    Public Property Position() As Drawing.Point
        Get
            Position = m_Position

        End Get
        Set(ByVal Value As Drawing.Point)
            m_Position = Value
            'Ricalcola la posizione delle azioni
            Dim ActionPos As Drawing.Point
            Dim i As Integer = 0
            For Each A As GraphicalAction In m_ActionList
                ActionPos.X = m_Position.X
                ActionPos.Y = m_Position.Y + i * CInt(m_Dimension * (3 / 4))
                A.Position = ActionPos
                i = i + 1
            Next A
        End Set
    End Property
    Property Documentation() As String
        Get
            Documentation = m_documentation
        End Get
        Set(ByVal Value As String)
            m_documentation = Value
        End Set
    End Property
    Public Sub New(ByVal Dimension As Integer, ByRef RefSfc As Sfc, ByVal Position As Drawing.Point, ByVal BackCol As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, ByRef Graph As Drawing.Graphics, ByRef pouslist As Pous)
        m_Dimension = Dimension
        m_Position = Position
        m_Sfc = RefSfc
        m_BackColor = BackCol
        m_SelectionColor = SelectionCol
        m_NotSelectionColor = NotSelectionCol
        m_TextColor = TextCol
        m_Car = Car
        m_GraphToDraw = Graph
        m_ActionList = New ArrayList
        XmlPreviousConnectionsList = New ArrayList
        m_pouslist = pouslist
    End Sub

    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter) Implements IXMLSerializable.xmlExport
        If m_ActionList.Count > 0 Then
            'actionBlock
            RefXMLProjectWriter.WriteStartElement("actionBlock")
            'Position
            RefXMLProjectWriter.WriteStartElement("position")
            'Attributi di Position
            RefXMLProjectWriter.WriteAttributeString("x", (m_Position.X).ToString)
            RefXMLProjectWriter.WriteAttributeString("y", (m_Position.Y - CInt(m_Dimension * (3 / 8))).ToString)
            RefXMLProjectWriter.WriteEndElement() 'Position
            'ConnectionPointIn
            Dim ConnectionsList As New ConnectionPointIn(XmlPreviousConnectionsList)
            ConnectionsList.xmlExport(RefXMLProjectWriter)

            For Each A As GraphicalAction In m_ActionList
                A.xmlExport(RefXMLProjectWriter)
            Next A
            RefXMLProjectWriter.WriteEndElement() 'actionBlock
        End If
    End Sub
    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader) Implements IXMLSerializable.xmlImport
        'Memorizza la profondità del nodo
        Dim NodeDepth As Integer = RefXmlProjectReader.Depth

        'Se l'elemento non è vuoto si sposta sul nodo successivo
        If Not RefXmlProjectReader.IsEmptyElement Then
            RefXmlProjectReader.Read()
            While RefXmlProjectReader.Depth > NodeDepth
                Select Case RefXmlProjectReader.Name
                    Case "position"
                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then


                        End If
                    Case "connectionPointIn"
                        'Controlla se è l'inizio dell'elemento
                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                            Dim ConnectionsList As New ConnectionPointIn(XmlPreviousConnectionsList)
                            ConnectionsList.xmlImport(RefXmlProjectReader)
                        End If
                    Case "action"
                        'Controlla se è l'inizio dell'elemento
                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                            Dim NewAction As New GraphicalAction(m_Dimension, Nothing, m_Sfc, m_BackColor, m_SelectionColor, m_NotSelectionColor, m_TextColor, m_Car, m_GraphToDraw, m_pouslist)
                            NewAction.xmlImport(RefXmlProjectReader)

                            m_ActionList.Add(NewAction)
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
    Public Sub AddAndDrawAction(ByVal Name As String, ByVal Qual As String, ByVal Var As BaseVariable, ByVal VarInd As BaseVariable, ByVal Dimension As Integer, ByVal Time As TimeSpan, ByVal RefSfc As Sfc, ByVal StepsToForces As List(Of BaseGraphicalStep), ByVal ArithExp As String, ByVal m_BackColor As Drawing.Color, ByVal m_SelectionColor As Drawing.Color, ByVal m_NotSelectionColor As Drawing.Color, ByVal m_TextColor As Drawing.Color, ByVal Car As Drawing.Font, ByVal m_GraphToDraw As Drawing.Graphics)
        'Calcola la posizione dell'azione
        Dim ActionPos As Drawing.Point
        ActionPos.X = m_Position.X
        ActionPos.Y = m_Position.Y + m_ActionList.Count * CInt(m_Dimension * (3 / 4))
        Dim A As New GraphicalAction(Name, Qual, Var, VarInd, Dimension, Time, RefSfc, StepsToForces, ArithExp, ActionPos, m_BackColor, m_SelectionColor, m_NotSelectionColor, m_TextColor, Car, m_GraphToDraw)
        m_ActionList.Add(A)
        A.Draw()
    End Sub
    Public Sub SetGraphToDraw(ByRef Graph As Drawing.Graphics)
        For Each A As GraphicalAction In m_ActionList
            A.SetGraphToDraw(Graph)
        Next A
    End Sub
    Public Function CalculusArea() As Drawing.Rectangle
        'Calcola l'area delle azioni
        CalculusArea.X = m_Position.X
        CalculusArea.Y = m_Position.Y
        For Each A As GraphicalAction In m_ActionList
            CalculusArea = Drawing.Rectangle.Union(CalculusArea, A.CalculusArea)
        Next A
    End Function
    Public Sub DrawActions()
        For Each A As GraphicalAction In m_ActionList
            A.Draw()
        Next A
    End Sub
    Public Sub CalcelActions()
        For Each A As GraphicalAction In m_ActionList
            A.Cancel()
        Next A
    End Sub
    Public Sub ResolveVariablesLinks()
        'Risolve i riferimenti dei nomi di variabili nella azioni
        For Each A As GraphicalAction In m_ActionList
            A.ResolveVariablesLinks()
        Next A
    End Sub
    Public Sub Move(ByVal dx As Integer, ByVal dy As Integer)
        m_Position.X = m_Position.X + dx
        m_Position.Y = m_Position.Y + dy
        'Muove le azioni
        For Each A As GraphicalAction In m_ActionList
            A.Move(dx, dy)
        Next A
    End Sub
    Public Sub DisposeMe()
        'Distrugge tutte le azioni
        For Each A As GraphicalAction In m_ActionList
            A.DisposeMe()
        Next A
        Me.Finalize()
    End Sub
    Public Function FindAction(ByVal x As Integer, ByVal y As Integer) As Boolean
        'Trova solo un'azione del blocco
        For Each A As GraphicalAction In m_ActionList
            If A.MyAreaIsHere(x, y) Then
                FindAction = True
                Exit Function
            End If
        Next A
    End Function
    Public Function FindAndSelectAction(ByVal x As Integer, ByVal y As Integer) As Boolean
        'Seleziona solo la prima azione della fase che trova
        For Each A As GraphicalAction In m_ActionList
            If A.MyAreaIsHere(x, y) = True Then
                FindAndSelectAction = True
                A.Selected = True
                Exit Function
            End If
        Next A
    End Function
    Public Function ReadIfActionSelected(ByVal x As Integer, ByVal y As Integer) As Boolean
        'Trova la prima azione in x,y
        For Each A As GraphicalAction In m_ActionList
            If A.MyAreaIsHere(x, y) And A.Selected Then
                ReadIfActionSelected = True
                Exit Function
            End If
        Next A
    End Function
    Public Function FindAndSelectActions(ByVal Rect As Drawing.Rectangle) As Boolean
        'Seleziona tutte le azioni della fase che trova
        Dim x, y, Passo As Integer
        For Each A As GraphicalAction In m_ActionList
            Passo = A.Heigth()
            For x = Rect.X To Rect.X + Rect.Width Step Passo
                For y = Rect.Y To Rect.Y + Rect.Height Step Passo
                    If A.MyAreaIsHere(x, y) = True Then
                        A.Selected = True
                    End If
                Next y
            Next x
            'Ricerca sul bordo verticale destro
            x = Rect.X + Rect.Width
            For y = Rect.Y To Rect.Y + Rect.Height Step Passo
                If A.MyAreaIsHere(x, y) = True Then
                    A.Selected = True
                End If
            Next y
            'Ricerca sul bordo orizzontale sinistro
            y = Rect.Y + Rect.Height
            For x = Rect.X To Rect.X + Rect.Width Step Passo
                If A.MyAreaIsHere(x, y) = True Then
                    A.Selected = True
                End If
            Next x
            'Ricerca sull'angolo inferiore destro 
            x = Rect.X + Rect.Width
            y = Rect.Y + Rect.Height
            If A.MyAreaIsHere(x, y) = True Then
                A.Selected = True
            End If
        Next A
    End Function
    Public Function ReadSelectedAction() As GraphicalAction
        'Legge solo la prima azione selezionata
        For Each A As GraphicalAction In m_ActionList
            If A.Selected Then
                Return A
                Exit Function
            End If
        Next A
        Return Nothing
    End Function
    Public Sub RemoveSelectedActions()
        Dim i, j As Integer
        j = 0
        For i = 0 To m_ActionList.Count - 1
            If m_ActionList(i - j).Selected = True Then
                m_ActionList(i - j).DisposeMe()
                m_ActionList.RemoveAt(i - j)
                j = j + 1
            End If
        Next i
        'Ricalcola la posizione delle azioni
        Dim ActionPos As Drawing.Point
        i = 0
        For Each A As GraphicalAction In m_ActionList
            ActionPos.X = m_Position.X
            ActionPos.Y = m_Position.Y + i * CInt(m_Dimension * (3 / 4))
            A.Position = ActionPos
            i = i + 1
        Next A
    End Sub
    Public Function ReadSelectedActionsList() As List(Of GraphicalAction)
        ReadSelectedActionsList = New List(Of GraphicalAction)()
        For Each A As GraphicalAction In m_ActionList
            If A.Selected = True Then
                ReadSelectedActionsList.Add(A)
            End If
        Next A
    End Function
    Public Function CountSelectedActions() As Integer
        CountSelectedActions = 0
        For Each A As GraphicalAction In m_ActionList
            If A.Selected Then
                CountSelectedActions = CountSelectedActions + 1
            End If
        Next
    End Function
    Public Sub DeSelectActions()
        For Each A As GraphicalAction In m_ActionList
            If A.Selected Then
                A.Selected = False
            End If
        Next
    End Sub
    Public Sub ExecutePulseActions()
        'Esegue le azioni impulsive
        For Each A As GraphicalAction In m_ActionList
            A.ExecuteIfIsPulseAction()
        Next
    End Sub
    Public Sub ExecuteNotPulseActions()
        'Esegue le azione non impulsive
        For Each A As GraphicalAction In m_ActionList
            A.ExecuteIfIsNotPulseAction()
        Next
    End Sub
    Public Sub StopNotPulseActions()
        'Termina le azioni i non impulsive
        For Each A As GraphicalAction In m_ActionList
            A.StopIfIsNotPulseAction()
        Next A
    End Sub
    Public Sub ResetActions()
        For Each A As GraphicalAction In m_ActionList
            A.ResetActions()
        Next A
    End Sub
    Public Sub ResetActionsForSus()
        'Disattiva le azioni durante la sospensione
        For Each A As GraphicalAction In m_ActionList
            A.ResetActionsForSus()
        Next A
    End Sub
    Public Sub ActiveActionsForFr()
        'Attiva le azioni delle fasi forzate
        For Each A As GraphicalAction In m_ActionList
            A.ActiveActionsForFr()
        Next A
    End Sub
End Class
