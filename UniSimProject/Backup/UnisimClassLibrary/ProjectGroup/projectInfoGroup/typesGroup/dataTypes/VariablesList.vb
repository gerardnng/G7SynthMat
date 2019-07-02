Imports System.Collections.Generic
Imports System
Imports System.Xml
Public Class VariablesList
    Inherits List(Of BaseVariable)
    Protected m_Name As String
    Protected m_Constant As Boolean
    Protected m_Retain As Boolean
    Protected m_NonRetain As Boolean
    Protected m_Persistent As Boolean
    Protected m_NonPersistent As Boolean
    Property Name() As String
        Get
            Name = m_Name
        End Get
        Set(ByVal Value As String)
            m_Name = Value
            RaiseEvent NameChanged(Value)
        End Set
    End Property
    Public Event NameChanged(ByVal NewName As String)     'Notifica il cambiamento del nome
    Public Event Disposing()    'Notifica la distruzione della lista
    Sub New(ByVal Name As String)
        MyBase.New()
        m_Name = Name
    End Sub
    Sub New()
        MyBase.New()
    End Sub
    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter)
        'Attributi
        Try


        If m_Name <> "" Then
            RefXMLProjectWriter.WriteAttributeString("name", m_Name)
        End If
        For Each V As BaseVariable In Me
            V.xmlExport(RefXMLProjectWriter)
        Next V


        Catch ex As Exception
        End Try

    End Sub
    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader)
        Try
            'Memorizza la profondità del nodo
            Dim ListNodeDepth As Integer = RefXmlProjectReader.Depth

            ' correzione di un bug: se l'XmlReader punta ad un attributo
            ' invece che ad un elemento IsEmptyElement ritorna False
            ' Bisogna controllare se l'elemento è vuoto quando il Reader punta al nodo
            ' (come da documentazione Microsoft):
            ' Property Value
            ' true if the current node is an element
            ' (NodeType equals XmlNodeType.Element) that ends with />;
            ' otherwise, false. 
            Dim isEmpty As Boolean = RefXmlProjectReader.IsEmptyElement

            'Legge gli attributi
            If RefXmlProjectReader.MoveToAttribute("name") Then
                m_Name = RefXmlProjectReader.Value
            End If

            'Se l'elemento non è vuoto si sposta sul nodo successivo
            If Not isEmpty Then
                RefXmlProjectReader.Read()
                'Scorre fino alla fine di varlist
                While RefXmlProjectReader.Depth > ListNodeDepth
                    Select Case RefXmlProjectReader.Name
                        Case "variable"
                            'Memorizza la profondità del nodo
                            Dim VeriableNodeDepth As Integer = RefXmlProjectReader.Depth
                            'Legge gli attributi
                            Dim vName As String = ""
                            Dim vDocumentation As String = ""
                            Dim vAddress As String = ""
                            Dim vInitialValue As String = ""
                            Dim vType As String = ""
                            'Legge gli attributi
                            If RefXmlProjectReader.MoveToAttribute("name") Then
                                vName = RefXmlProjectReader.Value
                            End If
                            If RefXmlProjectReader.MoveToAttribute("address") Then
                                vAddress = RefXmlProjectReader.Value
                            End If
                            'Si sposta sul nodo successivo
                            RefXmlProjectReader.Read()
                            While RefXmlProjectReader.Depth > VeriableNodeDepth
                                Select Case RefXmlProjectReader.Name
                                    Case "type"
                                        'Si sposta sul nodo successivo
                                        RefXmlProjectReader.Read()
                                        vType = RefXmlProjectReader.Name
                                        RefXmlProjectReader.Read()
                                    Case "initialValue"
                                        'Si sposta sul nodo successivo 
                                        RefXmlProjectReader.Read()
                                        Select Case RefXmlProjectReader.Name
                                            Case "simpleValue"
                                                If RefXmlProjectReader.MoveToAttribute("value") Then
                                                    vInitialValue = RefXmlProjectReader.Value
                                                End If
                                        End Select
                                        'Si sposta sul nodo successivo 
                                        RefXmlProjectReader.Read()
                                End Select
                                'Si sposta sul nodo successivo
                                RefXmlProjectReader.Read()
                            End While
                            'Aggiunge la variabile
                            CreateAndAddVariable(vName, vDocumentation, vAddress, vInitialValue, vType)
                    End Select
                    RefXmlProjectReader.Read()
                End While
            End If
        Catch ex As system.exception

        End Try
    End Sub
    Public Event NewVariable(ByVal Var As BaseVariable)
    Public Event VariableDropped(ByVal Var As BaseVariable)
    Public Sub AddVariable(ByRef Var As BaseVariable)
        MyBase.Add(Var)
        RaiseEvent NewVariable(Var)
    End Sub
    Public Sub RemoveVariable(ByRef Var As BaseVariable)
        MyBase.Remove(Var)
        RaiseEvent VariableDropped(Var)
    End Sub
    Public Function FindVariableByName(ByVal VarName As String) As BaseVariable
        For Each V As BaseVariable In Me
            If V.Name = VarName Then
                Return V
            End If
        Next V
        Return Nothing
    End Function
    Public Sub Reset()
        'Resetta al valore iniziale tutte le variabili della lista
        For Each V As BaseVariable In Me
            ' In questo modo le variabili intere si resettano correttamente
            ' (in realtà il problema è a monte in come è stata pensata l'implementazione
            ' di IntegerVariable, ma è tardi per cambiarla)
            V.ResetValue()
        Next V
    End Sub
    WriteOnly Property Locked() As Boolean
        Set(ByVal value As Boolean)
            For Each V As BaseVariable In Me
                V.Locked = value
            Next
        End Set
    End Property
    Public Sub Lock()
        Locked = True
    End Sub
    Public Sub Unlock()
        Locked = False
    End Sub
    Public Overloads Sub RemoveAll()
        For Each V As BaseVariable In Me
            V.DisposeMe()
            Remove(V)
        Next V
    End Sub
    Public Sub DisposeMe()
        RaiseEvent Disposing()
        Dim i As Integer
        'Cancella tutte le variabili
        For i = 0 To Count - 1
            Me(0).DisposeMe()
            RemoveAt(0)
        Next i
        Me.Finalize()
    End Sub
    Public Function CreateAndAddVariable(ByVal Name As String, ByVal Documentation As String, ByVal Address As String, ByVal InitialValue As String, ByVal Type As String) As BaseVariable
        'Crea e aggiunge la variabile
        'Restituisce un riferimento nullo se c'è un errore
        CreateAndAddVariable = Nothing
        Dim NewVariable As BaseVariable = VariablesManager.CreateVariable(Name, Documentation, Address, InitialValue, Type)
        If Not IsNothing(NewVariable) Then
            AddVariable(NewVariable)
            Return NewVariable
        End If
    End Function

    ' Gestisce la richiesta di creazione nuova variabile dal menu
    Private Sub HandleNewVarWantedClick(ByVal sender As Object, _
    ByVal e As EventArgs)
        Dim senderAsMenuItem As Windows.Forms.MenuItem = _
            CType(sender, Windows.Forms.MenuItem)
        Dim VariableDialog As New VariableDialogForm
        VariableDialog.m_ChangeType = True
        Do
            VariableDialog.m_Name = ""
            Dim ResultDialog As System.Windows.Forms.DialogResult = VariableDialog.ShowDialog()
            If ResultDialog = Windows.Forms.DialogResult.OK Then _
                Call CreateAndAddVariable(VariableDialog.m_Name, VariableDialog.m_Documentation, VariableDialog.m_Address, VariableDialog.m_InitialValue, VariableDialog.m_Type)
        Loop While VariableDialog.AgainOn
    End Sub

    Public Function GetMenu() As System.Windows.Forms.MenuItem
        Dim myMenu As New System.Windows.Forms.MenuItem(Me.Name)
        For Each V As BaseVariable In Me
            If V.Hidden Then Continue For
            myMenu.MenuItems.Add(V.GetMenu())
        Next
        myMenu.MenuItems.Add("Add variable", AddressOf HandleNewVarWantedClick)
        Return myMenu
    End Function

    ' Crea un nome del tipo nameRoot<nn> dove nn è un numero e il nome è univoco
    ' all'interno di questa lista di variabili
    Public Function MakeUniqueName(ByVal nameRoot As String) As String
        Dim fmtString As String = nameRoot & "{0}"
        Dim index As Integer = 0
        Dim outString As String = ""
        While True
            outString = String.Format(fmtString, index)
            If Me.FindVariableByName(outString) Is Nothing Then Return outString
            index += 1
        End While
        ' Dovremmo trovare un nome prima o poi, ma per far contento VB fingiamo che questo
        ' percorso vada coperto
        Return Nothing
    End Function

    ' Crea un duplicato di questo elenco variabili. Se si mette useActualValues a True vengono
    ' usati i valori correnti di ogni variabile nel duplicato, se si mette a False vengono usati
    ' i valori iniziali
    Public Function CreateInstance(Optional ByVal useActualValues As Boolean = False) As VariablesList
        Dim vList As New VariablesList()
        For Each V As BaseVariable In Me
            Dim vDup As BaseVariable = vList.CreateAndAddVariable(V.Name.Clone(), _
                V.Documentation.Clone(), V.Address.Clone(), _
                    V.InitialValueToUniversalString().Clone(), _
                    V.dataType.Clone())
            If useActualValues Then vDup.SetActValue(V.ValueToUniversalString().Clone())
        Next
        Return vList
    End Function

End Class
