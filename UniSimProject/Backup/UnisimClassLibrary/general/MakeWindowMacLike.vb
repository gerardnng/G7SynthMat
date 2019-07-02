Imports System
Imports System.Drawing
Imports System.Windows.Forms

' Questo modulo applica una "texture" in stile Mac OS X ad una finestra Windows
Public Module MakeWindowMacLike

    ' Aggiungere i tipi dei controlli che vanno ricolorati
    Private ControlTypesToRecolor() As System.Type = { _
        GetType(Label), _
        GetType(RadioButton), _
        GetType(CheckBox), _
        GetType(Panel), _
        GetType(TabControl), _
        GetType(TabPage)}

    ' Aggiungere i tipi dei controlli che ricevono RGB(236,236,236) come sfondo
    ' (non supportano Color.Transparent come sfondo)
    Private ControlTypesToCustomColor() As System.Type = { _
        GetType(TrackBar)}

    Private Sub NormalColorize(ByVal ctrl As Control)
        ctrl.BackColor = Color.Transparent
        ctrl.ForeColor = Color.Black
    End Sub

    Private Sub CustomColorize(ByVal ctrl As Control)
        ctrl.BackColor = Color.FromArgb(255, 236, 236, 236)
        ctrl.ForeColor = Color.Black
    End Sub

    ' Usiamo tre marker speciali come hack, da inserire in Control.Tag
    ' {MC_LEAVE_ALONE}: non tocare questo controllo
    ' {MC_CUSTOM}: usa i colori personalizzati
    ' {MC_HANDLE_ME}: usa i colori standard
    ' questi marker permettono di forzare la gestione di un particolare controllo ignorando i tipi predefiniti
    Private Sub MacintoshizeControl(ByVal ctrl As Control)
        For Each c As Control In ctrl.Controls
            Dim typeOfc As Type = c.GetType()
            If c.Tag IsNot Nothing AndAlso c.Tag.ToString().Equals("{MC_LEAVE_ALONE}") Then _
                GoTo 1
            If (c.Tag IsNot Nothing AndAlso c.Tag.ToString().Equals("{MC_CUSTOM}")) OrElse _
                Array.IndexOf(ControlTypesToCustomColor, typeOfc) >= 0 Then _
                CustomColorize(c)
            If (c.Tag IsNot Nothing AndAlso c.Tag.ToString().Equals("{MC_HANDLE_ME}")) OrElse _
                Array.IndexOf(ControlTypesToRecolor, typeOfc) >= 0 Then _
                    NormalColorize(c)
1:          MacintoshizeControl(c)
        Next
    End Sub

    ' Necessario usare ScrollableControl per supportare sia Panel che Form
    Public Sub Macintoshize(ByVal wnd As ScrollableControl)
        If Preferences.GetPreference(Of Boolean)("Macintoshize", False) = False Then Exit Sub
        wnd.BackgroundImage = My.Resources.Mac_Background
        MacintoshizeControl(wnd)
    End Sub

End Module
