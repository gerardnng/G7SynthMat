Imports System.IO
Imports System.Math
Imports System.Xml
Imports System.Threading

Module ActionDef
    '----------------------------
    'Blocco aggiuntivo alla norma
    ' "fr", "ifr", "sus", "st" nella seguente dichiarazione
    '----------------------------
    Public ActionType() As String = {"N", "S", "R", "L", "D", "P", "SD", "DS", "SL", "fr", "ifr", "sus", "st"}
    ' Non so perchè questi due array siano qui...
    Public ContactType() As String = {"Normal Open Contact", "Normal Closed Contact", "Positive Transition-sensing Contact", "Negative Transition-sensing Contact"}
    Public CoilType() As String = {"Normal Coil", "Negative Coil", "Set Coil", "Reset Coil", "Positive Transition-sensing Coil", "Negative Transition-sensing Coil"}
End Module
Public Class GraphicalAction
    Implements IDocumentable, IXMLSerializable

    Protected m_Name As String
    Protected m_Documentation As String
    Protected WithEvents m_Var As BaseVariable
    Protected WithEvents m_Indicator As BaseVariable
    Protected m_Body As body
    Protected m_Qualifier As String
    Protected m_Type As String
    Protected m_Action As String
    Protected m_Time As TimeSpan
    Protected m_Height As Integer
    Protected m_Width As Integer
    Protected m_ActionTimer As System.Threading.Timer
    Protected m_TimerDelegate As TimerCallback
    Protected m_Sfc As Sfc
    Protected m_position As Drawing.Point
    Protected m_Selected As Boolean
    Protected m_Area As Drawing.Rectangle
    Protected m_BackColor As Drawing.Color
    Protected m_SelectionColor As Drawing.Color
    Protected m_NotSelectionColor As Drawing.Color
    Protected m_TextColor As Drawing.Color
    Protected m_TypeChar As Drawing.Font
    Protected m_GraphToDraw As Drawing.Graphics
    Protected m_xmlVarName As String            'Memorizza temporaneamente il nome della variabile prima di essere risolto
    Protected m_xmlIndicatorName As String       'Memorizza temporaneamente il nome della variabile indicatore prima di essere risolto


    '----------------------------
    'Blocco aggiuntivo alla norma
    Protected m_pouslist As Pous
    Protected m_StepToForces As GraphicalStepsList
    Protected m_xmlSfcName As String        'Memorizza temporaneamente il nome del sfc prima di essere risolto
    Protected m_xmlStepToForce As String    'Memorizza temporaneamente il nome dello step da forzare prima di essere risolto
    '----------------------------


    Protected m_xmlOpIntegerVars As String
    Protected m_ArithExp As String

    Public Sub New(ByVal Name As String, ByVal Qualifier As String, ByRef Var As BaseVariable, ByRef VarInd As BaseVariable, ByVal Dimension As Integer, ByVal T As TimeSpan, ByRef RefSfc As Sfc, ByVal StepToForces As GraphicalStepsList, ByVal ArithExp As String, ByVal P As Drawing.Point, ByVal BackCol As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, ByRef Graph As Drawing.Graphics)
        m_Name = Name
        m_Qualifier = Qualifier
        m_Var = Var
        If Not m_Var Is Nothing Then ' se l'azione  è del tipo non a norma m_Var è nullo
            m_Type = m_Var.dataType
        End If
        m_Indicator = VarInd
        m_Time = T
        m_Sfc = RefSfc
        m_TimerDelegate = New TimerCallback(AddressOf OnEndTimer)
        m_Width = Dimension * 3
        m_Height = CInt(Dimension * 3 / 4)
        m_position = P
        m_BackColor = BackCol
        m_SelectionColor = SelectionCol
        m_NotSelectionColor = NotSelectionCol
        m_TextColor = TextCol
        m_TypeChar = Car
        m_GraphToDraw = Graph
        m_Area = New Drawing.Rectangle
        m_ArithExp = ArithExp

        '----------------------------
        'Blocco aggiuntivo alla norma
        m_StepToForces = StepToForces
        m_xmlSfcName = ""
        m_xmlStepToForce = ""
        '----------------------------


    End Sub
    Public Function CreateInstance() As GraphicalAction
        CreateInstance = New GraphicalAction(m_Name, m_Qualifier, m_Var, m_Indicator, CInt(m_Width / 3), m_Time, m_Sfc, m_StepToForces, m_ArithExp, m_position, m_BackColor, m_SelectionColor, m_NotSelectionColor, m_TextColor, m_TypeChar, Nothing)
    End Function
    Public Property Name() As String Implements IDocumentable.Name
        Get
            Name = m_Name
        End Get
        Set(ByVal Value As String)
            m_Name = Value
        End Set
    End Property
    Public Property Documentation() As String Implements IDocumentable.Documentation
        Get
            Return m_Documentation
        End Get
        Set(ByVal Value As String)
            m_Documentation = Value
        End Set
    End Property
    Public Property Type() As String
        Get
            Type = m_Type
        End Get
        Set(ByVal Value As String)
            m_Type = Value
        End Set
    End Property
    Public Property Qualifier() As String
        Get
            Qualifier = m_Qualifier
        End Get
        Set(ByVal Value As String)
            m_Qualifier = Value
        End Set
    End Property
    Public Property Time() As TimeSpan
        Get
            Time = m_Time
        End Get
        Set(ByVal Value As TimeSpan)
            m_Time = Value
        End Set
    End Property
    Public Property Variable() As BaseVariable
        Get
            Variable = m_Var
        End Get
        Set(ByVal Value As BaseVariable)
            m_Var = Value
        End Set
    End Property
    Public Property Indicator() As BaseVariable
        Get
            Indicator = m_Indicator
        End Get
        Set(ByVal Value As BaseVariable)
            m_Indicator = Value
        End Set
    End Property
    Public Property Heigth() As Integer
        Get
            Heigth = m_Height
        End Get
        Set(ByVal Value As Integer)
            m_Height = Value
        End Set
    End Property
    Public Property Width() As Integer
        Get
            Width = m_Width
        End Get
        Set(ByVal Value As Integer)
            m_Width = Value
        End Set
    End Property
    Public Property Position() As Drawing.Point
        Get
            Position = m_position
        End Get
        Set(ByVal Value As Drawing.Point)
            m_position = Value
        End Set
    End Property
    Public Property Selected() As Boolean
        Get
            Selected = m_Selected
        End Get
        Set(ByVal Value As Boolean)
            m_Selected = Value
        End Set
    End Property
    Public Sub New(ByVal Dimension As Integer, ByVal P As Drawing.Point, ByRef RefSfc As Sfc, ByVal BackCol As Drawing.Color, ByVal SelectionCol As Drawing.Color, ByVal NotSelectionCol As Drawing.Color, ByVal TextCol As Drawing.Color, ByVal Car As Drawing.Font, ByVal Graph As Drawing.Graphics, ByRef pouslist As Pous)
        m_TimerDelegate = New TimerCallback(AddressOf OnEndTimer)
        m_Sfc = RefSfc
        m_Width = Dimension * 3
        m_Height = CInt(Dimension * 3 / 4)
        m_position = P
        m_BackColor = BackCol
        m_SelectionColor = SelectionCol
        m_NotSelectionColor = NotSelectionCol
        m_TextColor = TextCol
        m_TypeChar = Car
        m_GraphToDraw = Graph
        m_Area = New Drawing.Rectangle
        m_pouslist = pouslist
        m_xmlSfcName = ""
        m_xmlStepToForce = ""
    End Sub
    Public Sub WriteFile(ByRef W As BinaryWriter)
        W.Write(m_Qualifier)
        W.Write(m_Name)
        W.Write(m_Var.Name)
        W.Write(m_Time.TotalMilliseconds)
    End Sub
    Public Sub New(ByRef R As BinaryReader)
        m_Qualifier = R.ReadString
        m_Name = R.ReadString
        'Legge il nome della Variable e la converte
        Dim VarString As String = R.ReadString
        'Var = FindIndirizzoVariable(VarString)
        m_Time = TimeSpan.FromMilliseconds(R.ReadDouble)
        m_TimerDelegate = New TimerCallback(AddressOf OnEndTimer)
    End Sub
    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter) Implements IXMLSerializable.xmlExport
        'action
        RefXMLProjectWriter.WriteStartElement("action")
        'Attributi dell'azione
        RefXMLProjectWriter.WriteAttributeString("qualifier", m_Qualifier)
        RefXMLProjectWriter.WriteAttributeString("duration", m_Time.ToString().ToString) ' m_Time.Days.ToString & ":" & m_Time.Hours.ToString & ":" & m_Time.Minutes.ToString & ":" & m_Time.Seconds.ToString & ":" & m_Time.Milliseconds.ToString)
        If Not IsNothing(m_Indicator) Then
            RefXMLProjectWriter.WriteAttributeString("indicator", m_Indicator.Name)
        End If


        If m_ArithExp <> "" Then
            RefXMLProjectWriter.WriteAttributeString("arithExp", m_ArithExp)
        End If





        '--------------------
        'Blocco aggiuntivo alla norma
        If (Not IsNothing(m_Sfc)) And (m_Qualifier = "fr" Or m_Qualifier = "ifr" Or m_Qualifier = "sus" Or m_Qualifier = "st") Then
            RefXMLProjectWriter.WriteAttributeString("refsfc", m_Sfc.Name)
        End If
        Dim steps As String
        steps = ""
        If Not IsNothing(m_StepToForces) Then
            For Each s As GraphicalStep In m_StepToForces
                steps = steps & s.Name & " "
            Next s
            RefXMLProjectWriter.WriteAttributeString("stepstoforces", steps)
        End If

        '----------

        ' If m_Qualifier <> "fr" And m_Qualifier <> "ifr" And m_Qualifier <> "sus" And m_Qualifier <> "st" Then
        'reference  (Riferimento al nome della variabile booleana che controlla
        RefXMLProjectWriter.WriteStartElement("reference")
        'Attributi di reference
        Try
            RefXMLProjectWriter.WriteAttributeString("name", m_Var.Name)
        Catch ex As Exception
            RefXMLProjectWriter.WriteAttributeString("name", "")
        End Try
        RefXMLProjectWriter.WriteEndElement() 'reference
        'End If

        'Esporta documentation
        If Documentation <> "" Then
            RefXMLProjectWriter.WriteElementString("documentation", Documentation)  'documentation
        End If
        RefXMLProjectWriter.WriteEndElement() 'action


    End Sub
    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader) Implements IXMLSerializable.xmlImport

        'Memorizza la profondità del nodo
        Dim NodeDepth As Integer = RefXmlProjectReader.Depth

        'Legge gli attributi
        If RefXmlProjectReader.MoveToAttribute("duration") Then
            m_Time = TimeSpan.Parse(RefXmlProjectReader.Value)
        End If
        If RefXmlProjectReader.MoveToAttribute("qualifier") Then
            m_Qualifier = RefXmlProjectReader.Value
        Else
            m_Qualifier = "N"
        End If
        If RefXmlProjectReader.MoveToAttribute("indicator") Then
            m_xmlIndicatorName = (RefXmlProjectReader.Value)
        End If


        '----------------------
        'Blocco aggiuntivo alla norma
        If RefXmlProjectReader.MoveToAttribute("refsfc") Then
            m_xmlSfcName = (RefXmlProjectReader.Value)
            'Converte la stringa letta in un oggetto di tipo sfc
            ' For Each P As pou In m_pousList
            'MsgBox(P.Name)
            'If P.Name = m_xmlSfcName Then
            'MsgBox("entrato") ;-)
            'm_Sfc = P.Body.ReadSfc
            'End If
            '   Next P
        End If


        If RefXmlProjectReader.MoveToAttribute("stepstoforces") Then
            m_xmlStepToForce = (RefXmlProjectReader.Value)

        End If
        '----------------------

        If RefXmlProjectReader.MoveToAttribute("arithExp") Then
            m_ArithExp = (RefXmlProjectReader.Value)
        End If

        'Se l'elemento non è vuoto si sposta sul nodo successivo
        If Not RefXmlProjectReader.IsEmptyElement Then
            RefXmlProjectReader.Read()

            'Scorre fino alla fine di Configuration
            While RefXmlProjectReader.Depth > NodeDepth
                Select Case RefXmlProjectReader.Name
                    Case "reference"
                        'Controlla se è l'inizio dell'elemento
                        If RefXmlProjectReader.NodeType = XmlNodeType.Element Then

                            'Legge il nome della variabile
                            If RefXmlProjectReader.MoveToAttribute("name") Then
                                m_xmlVarName = (RefXmlProjectReader.Value)
                            End If
                        End If
                    Case "documentation"
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
        End If
    End Sub

    Private Sub createNameForNotStandardActions()

        Try
            If m_Qualifier = "sus" Then
                m_Name = m_Sfc.Name & " : {}"
            ElseIf m_Qualifier = "st" Then
                m_Name = m_Sfc.Name & " : {*}"
            ElseIf (m_Qualifier = "fr" Or m_Qualifier = "ifr") Then
                m_Name = m_Sfc.Name & " : {" 'a
                Dim n As Integer = m_StepToForces.Count 'a
                For i As Integer = 0 To (n - 1) 'a
                    m_Name = m_Name & m_StepToForces.Item(i).Name & ";" 'a
                Next 'a
                m_Name = m_Name & "}" 'a
            Else
                m_Name = m_Qualifier
            End If
        Catch ex As System.NullReferenceException
        End Try

    End Sub

    Public Sub ResolveVariablesLinks()


        If Not IsNothing(m_pouslist) And m_xmlSfcName <> "" Then
            For Each P As pou In m_pouslist

                If P.Name = m_xmlSfcName Then
                    m_Sfc = P.Body.ReadSfc

                End If
            Next P

            If (m_xmlStepToForce <> "") Then
                m_StepToForces = New GraphicalStepsList

                ' 'Converte la stringa letta in un oggetto di tipo GraphicalStepsList

                Dim i As Integer
                Dim tmp As String = " "
                While (tmp <> "")
                    tmp = Mid(m_xmlStepToForce, 1, m_xmlStepToForce.IndexOf(" "))
                    For Each S As GraphicalStep In m_Sfc.GraphicalStepsList
                        If tmp.Equals(S.Name) Then
                            m_StepToForces.Add(S)
                        End If
                    Next S
                    i = m_xmlStepToForce.IndexOf(" ")
                    m_xmlStepToForce = m_xmlStepToForce.Substring(i + 1)
                    If m_xmlStepToForce.IndexOf(" ") < 0 Then
                        Exit While
                    End If
                End While
            End If


        End If


        ' If m_xmlOpIntegerVars <> "" Then
        ' m_opIntegerVars = New VariablesList
        ' Dim firstVar As IntegerVariable
        ' Dim secondVar As IntegerVariable
        ' firstVar = m_Sfc.PouInterface.FindVariableByName(Mid(m_xmlOpIntegerVars, 1, m_xmlOpIntegerVars.IndexOf(" ")))
        'If IsNothing(firstVar) Then
        'firstVar = m_Sfc.ResGlobalVariables.FindVariableByName(Mid(m_xmlOpIntegerVars, 1, m_xmlOpIntegerVars.IndexOf(" ")))
        'End If
        'secondVar = m_Sfc.PouInterface.FindVariableByName(Mid(m_xmlOpIntegerVars, m_xmlOpIntegerVars.IndexOf(" ") + 1).Trim)
        'If IsNothing(secondVar) Then
        'secondVar = m_Sfc.ResGlobalVariables.FindVariableByName(Mid(m_xmlOpIntegerVars, m_xmlOpIntegerVars.IndexOf(" ") + 1).Trim)
        'End If
        'm_opIntegerVars.AddVariable(firstVar)
        'm_opIntegerVars.AddVariable(secondVar)
        'End If



        Try


            m_Var = m_Sfc.PouInterface.FindVariableByName(m_xmlVarName)
            If IsNothing(m_Var) Then
                m_Var = m_Sfc.ResGlobalVariables.FindVariableByName(m_xmlVarName)
            End If

            m_Name = m_xmlVarName

            m_Indicator = m_Sfc.ResGlobalVariables.FindVariableByName(m_xmlIndicatorName)
            If IsNothing(m_Indicator) Then
                m_Indicator = m_Sfc.PouInterface.FindVariableByName(m_xmlIndicatorName)
            End If
            m_Type = m_Var.dataType
        Catch ex As Exception
            createNameForNotStandardActions()
        End Try
    End Sub
    Public Function ReadArea() As Drawing.Rectangle
        ReadArea = m_Area
    End Function
    Public Sub DeleteBody()
        m_Body.DisposeMe()
    End Sub
    Public Sub DisposeMe()
        If Not IsNothing(m_ActionTimer) Then
            m_ActionTimer.Dispose()
        End If
        Me.Finalize()
    End Sub

    '----------------------------
    'Blocco aggiuntivo alla norma
    Public Sub SetRefSfc(ByRef mSfc As Sfc)
        m_Sfc = mSfc
    End Sub
    Public Function ReadRefSfc() As Sfc
        ReadRefSfc = m_Sfc
    End Function
    Public Sub SetStepToForces(ByRef StepToForces As GraphicalStepsList)
        m_StepToForces = StepToForces
    End Sub
    Public Function ReadStepToForces() As GraphicalStepsList
        ReadStepToForces = m_StepToForces
    End Function
    '----------------------------
    Public Function ReadArithExp() As String
        ReadArithExp = m_ArithExp
    End Function
    Public Sub SetArithExp(ByRef ArithExp As String)
        m_ArithExp = ArithExp
    End Sub

    Public Sub ExecuteIfIsNotPulseAction()
        'Esegue l'azione se è  N, L ,SL, D, DS o SD
        If Not IsNothing(m_Var) Then 'La variabile può essere stata eliminata
            Try
                Select Case m_Qualifier
                    Case "N"   'N
                        m_Var.SetValue(True)
                        'Variabile indicatore
                        If Not IsNothing(m_Indicator) Then
                            m_Indicator.SetValue(True)
                        End If
                    Case "L", "SL"    'Time limited, Set and time limited
                        m_Var.SetValue(True)
                        'Variabile indicatore
                        If Not IsNothing(m_Indicator) Then
                            m_Indicator.SetValue(True)
                        End If
                        m_ActionTimer = New Timer(m_TimerDelegate, m_Qualifier, CInt(m_Time.TotalMilliseconds), -1)
                    Case "D", "SD", "DS"  'Time delay,  Set and delay, Delayed and Set
                        m_ActionTimer = New Timer(m_TimerDelegate, m_Qualifier, CInt(m_Time.TotalMilliseconds), -1)

                End Select
            Catch ex As System.Exception

            End Try
        End If

        Select Case m_Qualifier
            '----------------------------
            'Blocco aggiuntivo alla norma
            Case "fr"
                If Not IsNothing(m_Sfc) Then
                    m_Sfc.ForceMe(m_StepToForces)
                End If
            Case "sus"
                If Not IsNothing(m_Sfc) Then
                    m_Sfc.SuspendMe()
                End If
            Case "st"
                If Not IsNothing(m_Sfc) Then
                    m_Sfc.StopMe()
                End If
                '----------------------------
        End Select


    End Sub
    Public Sub ExecuteIfIsPulseAction()
        'Esegue l'azione se è  S, R o P
        If Not IsNothing(m_Var) Then 'La variabile può essere stata eliminata
            Try
                Select Case m_Qualifier
                    Case "S"    'Set
                        m_Var.SetValue(True)
                    Case "R"    'Reset
                        m_Var.SetValue(False)
                    Case "P"    'Pulse
                        m_Var.SetValue(True)
                        m_Var.SetValue(False)
                    Case "PI"
                        m_Var.IncreaseValue(m_Var.ReadActValue)
                    Case "PD"
                        m_Var.DecreaseValue(m_Var.ReadActValue)
                    Case "PR"
                        m_Var.ResetValue()
                        'Da rivedere
                    Case "PO"
                        Dim exp As New ArithmeticExpression(m_Sfc.ResGlobalVariables, _
                             m_Sfc.PouInterface)
                        m_Var.SetActValue(Exp.calculateExp(m_ArithExp))


                End Select
            Catch ex As System.Exception
            End Try
        End If

        Select Case m_Qualifier
            '----------------------------
            'Blocco aggiuntivo alla norma

            Case "ifr"    'Forzature impulsiva
                If Not IsNothing(m_Sfc) Then
                    m_Sfc.ForceMeImpulsively(m_StepToForces)
                End If 'Forzatura impulsiva
                '-------------------------
        End Select


    End Sub
    Public Sub StopIfIsNotPulseAction()
        'Termina l'azione se è N, L , D o DS
        If Not IsNothing(m_Var) Then 'La variabile può essere stata eliminata
            Try
                Select Case m_Qualifier
                    Case "N"
                        m_Var.SetValue(False)
                        'Variabile indicatore
                        If Not IsNothing(m_Indicator) Then
                            m_Indicator.SetValue(False)
                        End If
                    Case "L", "D", "DS"  'Time limited, Time delay, Delayed and Set
                        m_Var.SetValue(False)
                        If Not IsNothing(m_Indicator) Then
                            m_Indicator.SetValue(False)
                        End If
                        'Distrugge il timer se non è stato ancora distrutto
                        If Not IsNothing(m_ActionTimer) Then
                            m_ActionTimer.Dispose()
                        End If


                End Select
            Catch ex As System.Exception

            End Try
        End If

        Select Case m_Qualifier

            '----------------------------
            ''Blocco aggiuntivo alla norma
            Case "fr"
                If Not IsNothing(m_Sfc) Then
                    m_Sfc.UnForceMe()
                End If
            Case "sus"
                If Not IsNothing(m_Sfc) Then
                    m_Sfc.ResumeMe()
                End If
            Case "st"
                If Not IsNothing(m_Sfc) Then
                    m_Sfc.ReStartMe()
                End If
                '----------------------------



        End Select


    End Sub
    Public Sub ResetActions()
        'Distrugge il timer se non è stato ancora distrutto
        If Not IsNothing(m_ActionTimer) Then
            m_ActionTimer.Dispose()
        End If
    End Sub
    Public Sub ResetActionsForSus()
        'Disattiva le azioni durante la sospensione
        If Not IsNothing(m_Var) Then
            Try
                m_Var.SetValue(False)
            Catch ex As System.Exception
            End Try
        End If
        If Not IsNothing(m_Indicator) Then
            Try
                m_Indicator.SetValue(False)
            Catch ex As System.Exception
            End Try
        End If
    End Sub
    Public Sub ActiveActionsForFr()
        'Attiva le azioni delle fasi forzate
        If Not IsNothing(m_Var) Then
            Try
                m_Var.SetValue(True)
            Catch ex As System.Exception
            End Try
        End If
        If Not IsNothing(m_Indicator) Then
            Try
                m_Indicator.SetValue(True)
            Catch ex As System.Exception
            End Try
        End If
    End Sub

    Private Sub OnEndTimer(ByVal Qual As Object)
        If Not IsNothing(m_Var) Then 'La variabile può essere stata eliminata
            Try
                'Evento generato allo scadere del timer
                Select Case m_Qualifier
                    Case "L", "SL"    'Time limited, Set and time limited
                        m_Var.SetValue(False)
                        If Not IsNothing(m_Indicator) Then
                            m_Indicator.SetValue(False)
                        End If
                    Case "D", "SD", "DS"  'Time delay, Set and delay, Delayed and Set
                        m_Var.SetValue(True)
                        If Not IsNothing(m_Indicator) Then
                            m_Indicator.SetValue(True)
                        End If
                End Select
                'Distrugge il timer se non è stato ancora distrutto
                If Not IsNothing(m_ActionTimer) Then
                    m_ActionTimer.Dispose()
                End If
            Catch ex As System.Exception

            End Try
        End If
    End Sub
    Public Sub Move(ByVal dx As Integer, ByVal dy As Integer)
        m_position.X = m_position.X + dx
        m_position.Y = m_position.Y + dy
    End Sub
    Public Sub Draw()
        Draw(m_SelectionColor, m_NotSelectionColor, m_TextColor)
    End Sub
    Public Sub Cancel()
        Draw(m_BackColor, m_BackColor, m_BackColor)
    End Sub
    Private Sub Draw(ByVal Col1 As Drawing.Color, ByVal Col2 As Drawing.Color, ByVal Col3 As Drawing.Color)
        Dim Penna As New Drawing.Pen(m_NotSelectionColor)
        Dim StartXPoint As Integer = m_position.X
        Dim Br As New Drawing.SolidBrush(Col3)
        Penna.Width = 1
        'Rettangolo principale
        If m_Selected Then
            Penna.Color = Col1
        Else
            Penna.Color = Col2
        End If
        m_GraphToDraw.DrawRectangle(Penna, StartXPoint, m_position.Y - CInt(m_Height / 2), m_Width, m_Height)
        Penna.Color = Col2
        'Qualificatore
        Dim Rect As Drawing.SizeF
        If IsNothing(m_Type) Then
            Rect = m_GraphToDraw.MeasureString(m_Qualifier, m_TypeChar)
            m_GraphToDraw.DrawString(m_Qualifier, m_TypeChar, Br, StartXPoint + (m_Width / 8) - (Rect.Width / 2), m_position.Y - (Rect.Height / 2))
        End If
        Select Case m_Type
            Case "BOOL"
                Rect = m_GraphToDraw.MeasureString(m_Qualifier, m_TypeChar)
                m_GraphToDraw.DrawString(m_Qualifier, m_TypeChar, Br, StartXPoint + (m_Width / 8) - (Rect.Width / 2), m_position.Y - (Rect.Height / 2))
            Case "INT", "REAL"
                Rect = m_GraphToDraw.MeasureString("P", m_TypeChar)
                m_GraphToDraw.DrawString("P", m_TypeChar, Br, StartXPoint + (m_Width / 8) - (Rect.Width / 2), m_position.Y - (Rect.Height / 2))
        End Select
        'Retta verticale che divide il qualificatore
        Penna.Width = 1
        StartXPoint = CInt(StartXPoint + m_Width * 0.22)
        m_GraphToDraw.DrawLine(Penna, StartXPoint, m_position.Y - CInt(m_Height / 2), StartXPoint, m_position.Y + CInt(m_Height / 2))
        'Nome azione
        If Not IsNothing(m_Name) Then
            If IsNothing(m_Type) Then
                Rect = m_GraphToDraw.MeasureString(m_Name, m_TypeChar)
                'Controlla se il nome è più largo
                If Rect.Width < m_Width / 2 Then
                    m_GraphToDraw.DrawString(m_Name, m_TypeChar, Br, StartXPoint + m_Width / 4 - (Rect.Width / 2) + 1, m_position.Y - (Rect.Height / 2))
                Else
                    Dim NewRect As New Drawing.RectangleF(StartXPoint + m_Width / 20, CInt(m_position.Y - (Rect.Height / 2)), m_Width / 2 - (m_Width / 10), Rect.Height)
                    m_GraphToDraw.DrawString(m_Name, m_TypeChar, Br, NewRect)
                End If
            End If
            Select Case m_Type
                Case "BOOL"
                    Rect = m_GraphToDraw.MeasureString(m_Name, m_TypeChar)
                    'Controlla se il nome è più largo
                    If Rect.Width < m_Width / 2 Then
                        m_GraphToDraw.DrawString(m_Name, m_TypeChar, Br, StartXPoint + m_Width / 4 - (Rect.Width / 2) + 1, m_position.Y - (Rect.Height / 2))
                    Else
                        Dim NewRect As New Drawing.RectangleF(StartXPoint + m_Width / 20, CInt(m_position.Y - (Rect.Height / 2)), m_Width / 2 - (m_Width / 10), Rect.Height)
                        m_GraphToDraw.DrawString(m_Name, m_TypeChar, Br, NewRect)
                    End If
                Case "INT", "REAL"
                    Select Case m_Qualifier
                        Case "PI"
                            Rect = m_GraphToDraw.MeasureString(m_Name & "++", m_TypeChar)
                            'Controlla se il nome è più largo
                            If Rect.Width < m_Width / 2 Then
                                m_GraphToDraw.DrawString(m_Name & "++", m_TypeChar, Br, StartXPoint + m_Width / 4 - (Rect.Width / 2) + 1, m_position.Y - (Rect.Height / 2))
                            Else
                                Dim NewRect As New Drawing.RectangleF(StartXPoint + m_Width / 20, CInt(m_position.Y - (Rect.Height / 2)), m_Width / 2 - (m_Width / 10), Rect.Height)
                                m_GraphToDraw.DrawString(m_Name & "++", m_TypeChar, Br, NewRect)
                            End If
                        Case "PD"
                            Rect = m_GraphToDraw.MeasureString(m_Name & "--", m_TypeChar)
                            'Controlla se il nome è più largo
                            If Rect.Width < m_Width / 2 Then
                                m_GraphToDraw.DrawString(m_Name & "--", m_TypeChar, Br, StartXPoint + m_Width / 4 - (Rect.Width / 2) + 1, m_position.Y - (Rect.Height / 2))
                            Else
                                Dim NewRect As New Drawing.RectangleF(StartXPoint + m_Width / 20, CInt(m_position.Y - (Rect.Height / 2)), m_Width / 2 - (m_Width / 10), Rect.Height)
                                m_GraphToDraw.DrawString(m_Name & "--", m_TypeChar, Br, NewRect)
                            End If
                        Case "PR"
                            Rect = m_GraphToDraw.MeasureString("R " & m_Name, m_TypeChar)
                            'Controlla se il nome è più largo
                            If Rect.Width < m_Width / 2 Then
                                m_GraphToDraw.DrawString("R " & m_Name, m_TypeChar, Br, StartXPoint + m_Width / 4 - (Rect.Width / 2) + 1, m_position.Y - (Rect.Height / 2))
                            Else
                                Dim NewRect As New Drawing.RectangleF(StartXPoint + m_Width / 20, CInt(m_position.Y - (Rect.Height / 2)), m_Width / 2 - (m_Width / 10), Rect.Height)
                                m_GraphToDraw.DrawString("R " & m_Name, m_TypeChar, Br, NewRect)
                            End If
                        Case "PO"
                            Dim tmpString As String
                            tmpString = m_Name & ":=" & m_ArithExp
                            Rect = m_GraphToDraw.MeasureString(tmpString, m_TypeChar)
                            'Controlla se il nome è più largo
                            If Rect.Width < m_Width / 2 Then
                                m_GraphToDraw.DrawString(tmpString, m_TypeChar, Br, StartXPoint + m_Width / 4 - (Rect.Width / 2) + 1, m_position.Y - (Rect.Height / 2))
                            Else
                                Dim NewRect As New Drawing.RectangleF(StartXPoint + m_Width / 20, CInt(m_position.Y - (Rect.Height / 2)), m_Width / 2 - (m_Width / 10), Rect.Height)
                                m_GraphToDraw.DrawString(tmpString, m_TypeChar, Br, NewRect)
                            End If
                    End Select
            End Select
        End If
        'Retta verticale che divide il nome
        Penna.Width = 1
        StartXPoint = StartXPoint + m_Width / 2
        m_GraphToDraw.DrawLine(Penna, StartXPoint, m_position.Y - CInt(m_Height / 2), StartXPoint, m_position.Y + CInt(m_Height / 2))
        'Indicatore
        If Not IsNothing(m_Indicator) Then
            Rect = m_GraphToDraw.MeasureString(m_Indicator.Name, m_TypeChar)
            'Controlla se il nome è più largo
            If Rect.Width < m_Width / 4 Then
                m_GraphToDraw.DrawString(m_Indicator.Name, m_TypeChar, Br, StartXPoint + (m_Width / 8) - (Rect.Width / 2) + 1, m_position.Y - (Rect.Height / 2))
            Else
                Dim NewRect As New Drawing.RectangleF(StartXPoint + m_Width / 20, CInt(m_position.Y - (Rect.Height / 2)), m_Width / 4 - (m_Width / 10), Rect.Height)
                m_GraphToDraw.DrawString(m_Indicator.Name, m_TypeChar, Br, NewRect)
            End If
        End If
    End Sub
    Public Sub SetGraphToDraw(ByRef Graph As Drawing.Graphics)
        m_GraphToDraw = Graph
    End Sub
    Public Function MyAreaIsHere(ByVal x As Integer, ByVal y As Integer) As Boolean
        If x >= m_position.X And x < m_position.X + m_Width And Abs(y - m_position.Y) < m_Height / 2 Then
            MyAreaIsHere = True
        End If
    End Function
    Public Function CalculusArea() As Drawing.Rectangle
        'Calcola l'area dell'azione
        CalculusArea.X = m_position.X - 1
        CalculusArea.Y = m_position.Y - m_Height / 2 - 1
        CalculusArea.Width = m_Width + 2
        CalculusArea.Height = m_Height + 2
    End Function
    Private Sub HandlesVarNameChanging(ByVal Name As String) Handles m_Var.NameChanging
        If Not IsNothing(m_GraphToDraw) Then
            Cancel()
        End If
    End Sub
    Private Sub HandlesVarNameChanged(ByVal Name As String) Handles m_Var.NameChanged
        m_Name = Name
        If Not IsNothing(m_GraphToDraw) Then
            Draw()
        End If
    End Sub
    Private Sub HandlesIndicatorNameChanging(ByVal Name As String) Handles m_Indicator.NameChanging
        If Not IsNothing(m_GraphToDraw) Then
            Cancel()
        End If
    End Sub
    Private Sub HandlesIndicatorNameChanged(ByVal Name As String) Handles m_Indicator.NameChanged
        If Not IsNothing(m_GraphToDraw) Then
            Draw()
        End If
    End Sub
    Public Function GetIdentifier() As String Implements IDocumentable.GetIdentifier
        Return ""
    End Function
    Public Function GetDescription() As String Implements IDocumentable.GetDescription
        Return GetSystemDescription() + IIf(Of String)(Documentation = "", "", " (" + Documentation + ")")
    End Function
    Public Function GetSystemDescription() As String Implements IDocumentable.GetSystemDescription
        Dim desc As String = ""
        Select Case Me.Qualifier
            Case "N"
                desc = "Continuously set " + m_Var.Name
            Case "S"
                desc = "Set " + m_Var.Name
            Case "R"
                desc = "Reset BOOL " + m_Var.Name
            Case "PR"
                desc = "Reset INT " + m_Var.Name
            Case "P"
                desc = "Pulse " + m_Var.Name
            Case "PI"
                desc = "Increase " + m_Var.Name
            Case "PD"
                desc = "Decrease " + m_Var.Name
            Case "PO"
                desc = "Set " + m_Var.Name + " to " + m_ArithExp
            Case "sus"
                desc = "Suspend " + m_Sfc.Name
            Case "st"
                desc = "Stop " + m_Sfc.Name
            Case "fr"
                desc = "Continuously activate " + m_StepToForces.ListSteps() + " on " + m_Sfc.Name
            Case "ifr"
                desc = "Impulsively activate " + m_StepToForces.ListSteps() + " on " + m_Sfc.Name
        End Select
        Return desc
    End Function
End Class

