Imports System.Windows.Forms
Imports System.Collections.Generic

' Gestisce le preferenze utente per UniSim. E' un modulo non banale come si potrebbe
' pensare che sia un gestore di preferenze. Usa pesantemente le funzioni di Generics
' del .net Framework 2.0

' Implementa il concetto di un repository permanente per coppie (chiave,valore)
Public Interface IPreferencesBackEnd
    Function GetValue(ByVal name As String, ByVal defaultValue As Object) As Object
    Sub SetValue(ByVal name As String, ByVal value As Object)
End Interface

' Memorizza le impostazioni nel Registro di Configurazione
Public Class RegistryPreferencesBackEnd
    Implements IPreferencesBackEnd

    Private Shared KeyName As String = "HKEY_CURRENT_USER\Software\unina\UniSim"

    Public Function GetValue(ByVal name As String, ByVal defaultValue As Object) As Object Implements IPreferencesBackEnd.GetValue
        Dim ret As Object = _
            My.Computer.Registry.GetValue(KeyName, name, defaultValue)
        If IsNothing(ret) Then ret = defaultValue
        Return ret
    End Function

    Public Sub SetValue(ByVal name As String, ByVal value As Object) Implements IPreferencesBackEnd.SetValue
        My.Computer.Registry.SetValue(KeyName, name, value.ToString())
    End Sub
End Class

' Classe centrale per le preferenze: consente di leggere e scrivere valori di tipo
' arbitrario e contiene degli "shortcuts" per leggere Interi, Stringhe e Booleani
Public Class Preferences

    Private Shared m_BackEnd As IPreferencesBackEnd = New RegistryPreferencesBackEnd()

    Public Shared Property BackEnd() As IPreferencesBackEnd
        Get
            Return m_BackEnd
        End Get
        Set(ByVal value As IPreferencesBackEnd)
            m_BackEnd = value
        End Set
    End Property

    Public Shared Sub SetPreference(Of T)(ByVal name As String, ByVal value As T)
        m_BackEnd.SetValue(name, value)
    End Sub
    Public Shared Function GetPreference(Of T)(ByVal name As String, ByVal defaultValue As T) As T
        Return CType(m_BackEnd.GetValue(name, defaultValue), T)
    End Function

    Public Shared Function GetBoolean(ByVal name As String, _
        Optional ByVal defaultValue As Boolean = False) As Boolean
        Return GetPreference(Of Boolean)(name, defaultValue)
    End Function
    Public Shared Function GetInteger(ByVal name As String, _
        Optional ByVal defaultValue As Integer = 0) As Integer
        Return GetPreference(Of Integer)(name, defaultValue)
    End Function
    Public Shared Function GetString(ByVal name As String, _
        Optional ByVal defaultValue As String = "") As String
        Return GetPreference(Of String)(name, defaultValue)
    End Function

    Public Shared Sub SetBoolean(ByVal name As String, ByVal value As Boolean)
        SetPreference(Of Boolean)(name, value)
    End Sub
    Public Shared Sub SetInteger(ByVal name As String, ByVal value As Integer)
        SetPreference(Of Integer)(name, value)
    End Sub
    Public Shared Sub SetString(ByVal name As String, ByVal value As String)
        SetPreference(Of String)(name, value)
    End Sub

End Class

' Questa interfaccia e la classe PrefenceMapper gestiscono il concetto che alcuni
' controlli nella interfaccia grafica utente siano in qualche modo biunivocamente
' associati con alcune impostazioni utente e sia possibile passare dallo stato 
' dei controlli al valore delle impostazioni e viceversa
Public Interface IPreferenceGUIBinder(Of T)
    Sub WritePreferenceToGUI(ByVal value As T)
    Function ReadPreferenceFromGUI() As T
End Interface

' Sarebbe inutile se si potesse dichiarare una variabile di tipo
' List(Of PreferenceMapper(Of ?)) o una qualsiasi altra sintassi per rendersi 
' indipendenti dal tipo del parametro. Non essendo possibile si deve usare l'hack di
' scrivere una interfaccia da implementare e poi dichiarare la lista come
' List(Of IPreferenceMapper)
Public Interface IPreferenceMapper
    Sub SetToGUI() ' Preferences => GUI
    Sub SetFromGUI() ' GUI => Preferences
End Interface

Public Delegate Sub PreferenceValueChangedHandler(Of T)(ByVal sender As PreferenceMapper(Of T), ByVal oldValue As T, ByVal newValue As T)

Public Class PreferenceMapper(Of T)
    Implements IPreferenceMapper
    Private m_PrefName As String
    Private m_DefaultValue As T
    Private m_Binder As IPreferenceGUIBinder(Of T)
    Public Event PreferenceValueChanged As PreferenceValueChangedHandler(Of T)
    Public Sub New(ByVal name As String, _
        ByVal defaultValue As T, _
        ByVal binder As IPreferenceGUIBinder(Of T))
        m_PrefName = name
        m_DefaultValue = defaultValue
        m_Binder = binder
    End Sub
    Public Sub New(ByVal name As String, _
        ByVal defaultValue As T, _
        ByVal binder As IPreferenceGUIBinder(Of T), _
        ByVal prefChangeHandler As PreferenceValueChangedHandler(Of T))
        Call Me.New(name, defaultValue, binder)
        AddHandler PreferenceValueChanged, prefChangeHandler
    End Sub
    Public Sub SetToGUI() Implements IPreferenceMapper.SetToGUI
        m_Binder.WritePreferenceToGUI(Preferences.GetPreference(Of T)(m_PrefName, m_DefaultValue))
    End Sub
    Public Sub SetFromGUI() Implements IPreferenceMapper.SetFromGUI
        Dim oldValue As T = Preferences.GetPreference(Of T)(m_PrefName, m_DefaultValue)
        Preferences.SetPreference(Of T)(m_PrefName, m_Binder.ReadPreferenceFromGUI())
        Dim newValue As T = Preferences.GetPreference(Of T)(m_PrefName, m_DefaultValue)
        If Not (oldValue.Equals(newValue)) Then RaiseEvent PreferenceValueChanged(Me, oldValue, newValue)
    End Sub
End Class

' Un'impostazione booleana associata ad una CheckBox
Public Class CheckBoxPreferenceBinder
    Implements IPreferenceGUIBinder(Of Boolean)

    Private m_CheckBox As CheckBox

    Public Sub New(ByVal cb As CheckBox)
        m_CheckBox = cb
    End Sub

    Public Function ReadPreferenceFromGUI() As Boolean Implements IPreferenceGUIBinder(Of Boolean).ReadPreferenceFromGUI
        Return m_CheckBox.Checked
    End Function

    Public Sub WritePreferenceToGUI(ByVal value As Boolean) Implements IPreferenceGUIBinder(Of Boolean).WritePreferenceToGUI
        m_CheckBox.Checked = value
    End Sub
End Class

' Un'impostazione intera associata ad un array di RadioButton
Public Class RadioButtonsPreferenceBinder
    Implements IPreferenceGUIBinder(Of Integer)

    Private m_Buttons As List(Of RadioButton)

    ' lowIndex è un offset di cui "scalare" gli indici dei RadioButton. In particolare, nel caso di
    ' UniSim si sceglie di contare gli indici a partire da 1 e quindi si pone lowIndex = 1
    Public Sub New(ByVal lowIndex As Integer, ByVal ParamArray buttons() As RadioButton)
        m_Buttons = New List(Of RadioButton)
        While lowIndex > 0
            m_Buttons.Add(Nothing)
            lowIndex -= 1
        End While
        For Each b As RadioButton In buttons
            m_Buttons.Add(b)
        Next
    End Sub

    Public Function ReadPreferenceFromGUI() As Integer Implements IPreferenceGUIBinder(Of Integer).ReadPreferenceFromGUI
        For i As Integer = 0 To m_Buttons.Count - 1
            If m_Buttons(i) Is Nothing Then Continue For
            If m_Buttons(i).Checked Then Return i
        Next
        Return (-1)
    End Function

    Public Sub WritePreferenceToGUI(ByVal value As Integer) Implements IPreferenceGUIBinder(Of Integer).WritePreferenceToGUI
        For i As Integer = 0 To m_Buttons.Count - 1
            If m_Buttons(i) Is Nothing Then Continue For
            m_Buttons(i).Checked = (i = value)
        Next
    End Sub
End Class

' Un'impostazione intera associata da una TrackBar
Public Class TrackBarPreferenceBinder
    Implements IPreferenceGUIBinder(Of Integer)

    Private m_TrackBar As TrackBar

    Public Sub New(ByVal tb As TrackBar)
        m_TrackBar = tb
    End Sub

    Public Function ReadPreferenceFromGUI() As Integer Implements IPreferenceGUIBinder(Of Integer).ReadPreferenceFromGUI
        Return m_TrackBar.Value
    End Function

    Public Sub WritePreferenceToGUI(ByVal value As Integer) Implements IPreferenceGUIBinder(Of Integer).WritePreferenceToGUI
        m_TrackBar.Value = value
    End Sub
End Class

' Un insieme di preferenze utente da caricare/salvare in blocco
Public Class PreferenceSet
    Inherits List(Of IPreferenceMapper)
    Implements IPreferenceMapper

    Public Sub SetFromGUI() Implements IPreferenceMapper.SetFromGUI
        For Each X As IPreferenceMapper In Me
            X.SetFromGUI()
        Next
    End Sub

    Public Sub SetToGUI() Implements IPreferenceMapper.SetToGUI
        For Each X As IPreferenceMapper In Me
            X.SetToGUI()
        Next
    End Sub
End Class