Imports System.IO
Imports System.Math
Imports System.Xml
Imports System.Threading
Public Class GraphicalMacroStep
    Inherits BaseGraphicalStep
    'La macrofase non deve essere attivata, se preattivata si attiva da sola quando il proprio body ha raggiunto una fase finale...
    '....quindi si Depreattiva e si attiva. Cosi nelle scansioni successive la macrofase
    'La macrofase, quando viene chiamato il proprio ciclo di scansione per il body lo effetua solo se è preattiva
    Protected m_Body As body
    Protected m_pouslist As Pous
    WriteOnly Property PouInterface() As PouInterface
        Set(ByVal Value As PouInterface)
            'Comunica al body l'interfaccia della pou
            m_Body.PouInterface = Value
        End Set
    End Property
    WriteOnly Property ResGlobalVariables() As VariablesLists
        Set(ByVal Value As VariablesLists)
            'Comunica al body l'interfaccia della POU
            m_Body.ResGlobalVariables = Value
        End Set
    End Property
    Public Sub New(ByVal StepNumber As Integer, ByVal StepName As String, ByVal StepDocumentation As String, ByRef RefSfc As Sfc, ByVal Ini As Boolean, ByVal Fin As Boolean, ByVal P As Drawing.Point, ByVal ColSfondo As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal TextColor As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, ByVal Graph As Drawing.Graphics, ByVal DrState As Boolean, ByVal Dimen As Integer, ByRef pouslist As Pous)
        MyBase.New(StepNumber, StepName, StepDocumentation, RefSfc, Ini, Fin, P, ColSfondo, SelectionCol, NotSelectionCol, TextColor, Car, ColActive, ColDeactive, ColPreActive, Graph, DrState, Dimen)
        m_Body = New body(m_Name & " body", EnumBodyType.tSFC, RefSfc.ResGlobalVariables, RefSfc.PouInterface, pouslist)
        CalculateArea()
        m_pouslist = pouslist
    End Sub
    Public Sub New(ByRef RefSfc As Sfc, ByVal BackCol As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color, ByRef Graph As Drawing.Graphics, ByVal DrState As Boolean, ByVal Dimen As Integer)
        MyBase.New(RefSfc, BackCol, SelectionCol, NotSelectionCol, TextCol, Car, ColActive, ColDeactive, ColPreActive, Graph, DrState, Dimen)
        CalculateArea()
    End Sub
    Public Sub New(ByRef R As BinaryReader, ByVal ColSfondo As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal ColTesto As Drawing.Color, ByVal Car As Drawing.Font, ByVal ColActive As Drawing.Color, ByVal ColDeactive As Drawing.Color, ByVal ColPreActive As Drawing.Color)
        MyBase.new()
        m_Name = R.ReadString
        m_Documentation = R.ReadString
        m_Number = R.ReadInt32
        m_Position.X = R.ReadInt32
        m_Position.Y = R.ReadInt32
        m_Initial = R.ReadBoolean
        m_Final = R.ReadBoolean
        BackColor = ColSfondo
        SelectionColor = SelectionCol
        NotSelectionColor = NotSelectionCol
        TextColor = ColTesto
        Carattere = Car
        ColorActive = ColActive
        ColorDeactive = ColDeactive
        ColorPreActive = ColPreActive
        Area = New Drawing.Rectangle
    End Sub
    Public Overrides Function CreateInstance(ByRef NewSfc As Sfc) As Object
        CreateInstance = New GraphicalMacroStep(m_Number, m_Name, m_Documentation, NewSfc, m_initial, m_final, m_Position, BackColor, SelectionColor, notSelectionColor, TextColor, Carattere, ColorActive, ColordeActive, ColorpreActive, Nothing, False, m_dimension, m_pouslist)
        CreateInstance.SetBody(m_Body.CreateInstance)
    End Function
    Public Overrides Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter)
        'Esporta la macrofase
        RefXMLProjectWriter.WriteStartElement("macroStep")
        'Attributi della macrofase
        RefXMLProjectWriter.WriteAttributeString("name", m_Name)
        RefXMLProjectWriter.WriteAttributeString("height", m_Dimension.ToString)
        RefXMLProjectWriter.WriteAttributeString("width", m_Dimension.ToString)
        RefXMLProjectWriter.WriteAttributeString("localId", m_Number.ToString)

        'Position
        RefXMLProjectWriter.WriteStartElement("position")
        'Attributi di Position
        'Nella macrofase position si riferisce al centro quindi sottrae CInt(m_Dimension / 2)
        RefXMLProjectWriter.WriteAttributeString("x", (m_Position.X - CInt(m_Dimension / 2)).ToString)
        RefXMLProjectWriter.WriteAttributeString("y", (m_Position.Y - CInt(m_Dimension / 2)).ToString)

        RefXMLProjectWriter.WriteEndElement() 'Position

        'ConnectionPointIn
        Dim ConnectionsList As New ConnectionPointIn(XmlPreviousConnectionsList)
        ConnectionsList.xmlExport(RefXMLProjectWriter)

        'Esporta il body
        m_Body.xmlExport(RefXMLProjectWriter)

        'Esporta documentation
        If m_documentation <> "" Then
            RefXMLProjectWriter.WriteElementString("documentation", m_documentation)  'documentation
        End If

        RefXMLProjectWriter.WriteEndElement() 'macrofase
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
        If RefXmlProjectReader.MoveToAttribute("negated") Then
            m_negated = RefXmlProjectReader.Value
        End If

        'Si sposta sul nodo successivo
        RefXmlProjectReader.Read()
        'Scorre fino alla fine della macrofase
        While RefXmlProjectReader.Depth > NodeDepth
            Select Case RefXmlProjectReader.Name
                Case "position"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                        'Legge gli attributi di position
                        'Nella macrofase position si riferisce al centro quindi aggiunge CInt(m_Dimension / 2)
                        If RefXmlProjectReader.MoveToAttribute("x") Then
                            m_Position.X = RefXmlProjectReader.Value + CInt(m_Dimension / 2)
                        End If
                        If RefXmlProjectReader.MoveToAttribute("y") Then
                            m_Position.Y = RefXmlProjectReader.Value + CInt(m_Dimension / 2)
                        End If
                    End If
                Case "connectionPointIn"
                    Dim ConnectionsList As New ConnectionPointIn(XmlPreviousConnectionsList)
                    ConnectionsList.xmlImport(RefXmlProjectReader)
                Case "body"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                        'Crea il body
                        m_Body = New body(m_name, m_Dimension, m_sfc.ResGlobalVariables, m_sfc.PouInterface, m_pouslist)
                        'Importa i dati del body
                        m_Body.xmlImport(RefXmlProjectReader)
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
        'Aggiorna l'area della fase
        CalculateArea()
    End Sub
    Public Overrides Sub ResolveVariablesLinks()
        'Risolve i riferimenti dei nomi di variabili nella azioni
        m_Body.ResolveVariablesLinks()
    End Sub
    Public Overrides Sub DisposeMe()
        m_Body.DisposeMe()
        Me.Finalize()
    End Sub
    Public Overrides Sub Active()
        'La macrofase non deve essere attivata, se preattivata si attiva da sola quando il proprio body ha raggiunto una fase finale...
        '....quindi si Depreattiva e si attiva. Cosi nelle scansioni successive la macrofase
        'La macrofase, quando viene chiamato il proprio ciclo di scansione per il body lo effetua solo se è preattiva
    End Sub
    Public Overrides Sub Disactive()
        'Poichè non può essere attivata (si attiva da sola), se viene disattivata....
        '....forzatamente resetta l'evoluzione del proprio body se era attiva o preattiva
        If m_Active Or m_PreActive Then
            m_Body.Reset()
            m_PreActive = False
            m_Active = False
            'Resetta lo stato del body
            ResetBodyState()
            'Disegna lo stato se richiesto
            If DrawState Then
                DrawStepState(False)
            End If
        End If
    End Sub
    Public Overrides Sub PreActive()
        If Not m_PreActive And Not m_Active Then
            m_TimeActivation = Now  'Imposta l'istate di attivazione
            'Se non era preattiva o attiva inizializza l'sfc del proprio body e disegna lo stato
            m_Body.ReadSfc.ExecuteInit()
            m_PreActive = True
            'Disegna lo stato se richiesto
            If DrawState Then
                DrawStepState(False)
            End If
        End If
    End Sub
    Public Overrides Sub Cancel(ByVal CancellSmallRectangels As Boolean)
        Draw(CancellSmallRectangels, BackColor, BackColor, BackColor, BackColor)
        'Cancella lo stato se richiesto
        If DrawState Then
            DrawStepState(True)
        End If
    End Sub
    Public Overrides Sub Move(ByVal dx As Integer, ByVal dy As Integer)
        m_Position.X = m_Position.X + dx
        m_Position.Y = m_Position.Y + dy
        CalculateArea()
    End Sub
    Public Overrides Sub SetGraphToDraw(ByRef Graph As Drawing.Graphics)
        GraphToDraw = Graph
    End Sub
    Overloads Overrides Sub Draw(ByVal DrawSmallRectangels As Boolean)
        Draw(DrawSmallRectangels, SelectionColor, NotSelectionColor, TextColor, BackColor)
        'Disegna lo stato se richiesto
        If DrawState Then
            DrawStepState(False)
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
                Penna.Width = 3
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
                    GraphToDraw.DrawLine(Penna, m_Position.X - CInt(m_Dimension / 4), m_Position.Y - CInt(m_Dimension / 4), m_Position.X - CInt(m_Dimension / 4), m_Position.Y + CInt(m_Dimension / 4))
                    GraphToDraw.DrawLine(Penna, m_Position.X, m_Position.Y - CInt(m_Dimension / 4) - CInt(m_Dimension / 6), m_Position.X - CInt(m_Dimension / 4), m_Position.Y + CInt(m_Dimension / 4))
                    GraphToDraw.DrawLine(Penna, m_Position.X, m_Position.Y - CInt(m_Dimension / 4) + CInt(m_Dimension / 6), m_Position.X - CInt(m_Dimension / 4), m_Position.Y + CInt(m_Dimension / 4))
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
                Dim Br As Drawing.Brush
                Dim Lato As Integer = CInt(m_Dimension / 4)
                If Cancel Then
                    Br = New Drawing.SolidBrush(BackColor)
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
    Public Overrides Sub CalculateArea()
        'Mamorizza l'area
        Area.X = m_Position.X - CInt(m_Dimension / 2) - 2
        Area.Y = m_Position.Y - CInt(m_Dimension / 2) - CInt(m_Dimension / 5)
        Area.Width = m_Dimension + 4 '+ m_Dimension * Sign(m_ListActions.Count) + m_ListActions.Count * m_Dimension * 3
        Area.Height = m_Dimension + 2 * CInt(m_Dimension / 5)
    End Sub
    Public Sub SetBody(ByRef Value As body)
        m_Body = Value
    End Sub
    Public Function ReadBody() As body
        ReadBody = m_Body
    End Function
    Public Sub ExecuteScanCycle()
        If m_PreActive Then
            'Se era preattivata effettua la scansione dell'sfc del proprio body. Se era attiva non deve effetuare...
            '....la scansione perchè si attiva quando il proprio sfc raggiunge una fase finale
            'Legge dalla funzione ExecuteScanCycle se è stata attivata almeno una fase finale,....
            '.... se si resetta l'sfc del proprio body e si attiva (rendendosi attiva per le transizioni uscenti)
            If m_Body.ReadSfc.ExecuteScanCycle() Then
                'Ha raggiunto una fase finale, quindi viene resattato
                m_PreActive = False
                m_Active = True 'Si rende attiva per le transizioni successive
                m_Body.Reset()
                'Disegna lo stato se richiesto
                If DrawState Then
                    DrawStepState(False)
                End If
            End If
        End If
    End Sub
    Public Sub ResetBodyState()
        'Resetta lo stato del suo body
        m_Body.Reset()
    End Sub
    Public Overrides Function GetIdentifier() As String
        Return "MacroStep " + Me.Name
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
