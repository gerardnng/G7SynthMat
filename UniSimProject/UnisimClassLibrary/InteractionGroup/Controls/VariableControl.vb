Imports System.Threading
Imports System.Drawing
Imports System.Collections.Generic
Imports System.Windows.Forms
Public Class VariableControl

    Inherits System.Windows.Forms.UserControl
    Protected WithEvents m_Var As BaseVariable
    Protected m_Value As Boolean
    Protected m_NumericValue As Object
    Protected m_PrevValue As Boolean
    Protected m_monitoring As Boolean
    'Variabili per il time monitor
    Protected m_GrapValue As drawing.Graphics
    Protected m_Pen1 As Drawing.Pen
    Protected m_Pen2 As Drawing.Pen
    Protected m_Brush As Drawing.SolidBrush
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents Button2 As System.Windows.Forms.Button
    Public WithEvents Button3 As System.Windows.Forms.Button
    Public WithEvents Button4 As System.Windows.Forms.Button
    Protected m_Span As Single
    Dim colorScheme As Hydra(Of Integer, String, Color)
    Dim myColorScheme As Integer



#Region " Codice generato da Progettazione Windows Form "

    Public Sub New(ByVal Var As BaseVariable, ByVal ContMenu As ContextMenu, ByVal Editing As Boolean)
        MyBase.New()

        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        colorScheme = New Hydra(Of Integer, String, Color)
        colorScheme.AddValueToAllEntries("BackColor", _
            New KeyValuePair(Of Integer, Color)(1, Color.White), _
            New KeyValuePair(Of Integer, Color)(2, Color.Black)).AddValueToAllEntries("InverseColor", _
            New KeyValuePair(Of Integer, Color)(1, Color.Black), _
            New KeyValuePair(Of Integer, Color)(2, Color.White)).AddValueToAllEntries("ForeColor", _
            New KeyValuePair(Of Integer, Color)(1, Color.Blue), _
            New KeyValuePair(Of Integer, Color)(2, Color.Chartreuse))

        myColorScheme = Preferences.GetInteger("VarMonitorColorSet", 1)

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()
        m_Var = Var
        RefreshControl()
        Label5.BackColor = colorScheme(myColorScheme)("BackColor")
        m_GrapValue = Label5.CreateGraphics
        If Not Editing Then
            Button1.Enabled = False
        Else
            Me.ContextMenu = ContMenu
        End If
    End Sub

    'UserControl esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        Try
            MyBase.Dispose(disposing)
        Finally
            Nop()
        End Try
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue � richiesta da Progettazione Windows Form.
    'Pu� essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Public WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label0 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label0 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Location = New System.Drawing.Point(64, 28)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(509, 3)
        Me.Panel1.TabIndex = 13
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.Blue
        Me.Button1.Location = New System.Drawing.Point(2, 16)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(20, 20)
        Me.Button1.TabIndex = 0
        '
        'Label5
        '
        Me.Label5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.BackColor = System.Drawing.Color.White
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(62, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(513, 19)
        Me.Label5.TabIndex = 12
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.White
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(62, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(180, 16)
        Me.Label1.TabIndex = 1
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.White
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(242, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.White
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(326, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(84, 16)
        Me.Label3.TabIndex = 3
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.White
        Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(408, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(84, 16)
        Me.Label4.TabIndex = 4
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label0
        '
        Me.Label0.BackColor = System.Drawing.SystemColors.Control
        Me.Label0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label0.Location = New System.Drawing.Point(2, 2)
        Me.Label0.Name = "Label0"
        Me.Label0.Size = New System.Drawing.Size(60, 14)
        Me.Label0.TabIndex = 11
        Me.Label0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.White
        Me.Label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(490, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(84, 16)
        Me.Label6.TabIndex = 14
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.Transparent
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Location = New System.Drawing.Point(2, 15)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(20, 20)
        Me.Button2.TabIndex = 15
        Me.Button2.Text = "I"
        Me.Button2.Visible = False
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.Color.Transparent
        Me.Button3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button3.Location = New System.Drawing.Point(22, 15)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(20, 20)
        Me.Button3.TabIndex = 16
        Me.Button3.Text = "D"
        Me.Button3.Visible = False
        '
        'Button4
        '
        Me.Button4.BackColor = System.Drawing.Color.Transparent
        Me.Button4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button4.Location = New System.Drawing.Point(42, 15)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(20, 20)
        Me.Button4.TabIndex = 17
        Me.Button4.Text = "R"
        Me.Button4.Visible = False
        '
        'VariableControl
        '
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label0)
        Me.Name = "VariableControl"
        Me.Size = New System.Drawing.Size(575, 39)
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public Sub SetVariableName(ByVal Value As String)
        'Aggiorna il nome della variabile
        m_Var.Name = Value
        Label1.Text = Value
    End Sub
    Public Function ReadVariable() As BaseVariable
        'Restituisce la variabile controllata
        ReadVariable = m_Var
    End Function
    Public Sub ResetControl()
        'Resetta lo stato della variabile e del controllo
        m_Var.SetValue(False)
    End Sub
    Public Sub CallFocus()
        Label1.Focus()
    End Sub
    Public Sub SetMonitoring(ByVal Value As Boolean)
        m_monitoring = Value
        If m_monitoring Then
            'Se deve monitorare legge la prima volta lo stato del pulsante
            If m_Var.ReadValue Then
                Button1.BackColor = Drawing.color.Red
            Else
                Button1.BackColor = Drawing.color.Blue
            End If
        Else
            'altrimenti lo disattiva
            Button1.BackColor = Drawing.color.Blue
        End If
    End Sub
    Public Sub RefreshControl()
        Label0.Text = m_Var.dataType.ToString
        Label1.Text = m_Var.Name
        Label2.Text = m_Var.ReadValue
        Label3.Text = m_Var.InitialValueToUniversalString()
        Label4.Text = m_Var.ValueToUniversalString()
        Label6.Text = m_Var.Address
        If Label0.Text = "INT" Or Label0.Text = "REAL" Then
            Button1.Visible = False
            Button2.Visible = True
            Button3.Visible = True
            Button4.Visible = True
            Label5.Visible = False
            Panel1.Visible = False
            ' Si pu� sempre incrementare il valore degli interi ora
            Button2.Enabled = True
            ' I REAL possono anche essere < 0. Gli interi no
            Button3.Enabled = m_Var.dataType = "REAL" OrElse (m_Var.ReadActValue > 0)
        End If
    End Sub
    Public Sub VariableControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_Value = CBool(m_Var.ReadValue)
        If m_Var.dataType = "INT" Or m_Var.dataType = "REAL" Then
            m_NumericValue = m_Var.ReadActValue
        End If
        m_Pen1 = New Drawing.Pen(colorScheme(myColorScheme)("ForeColor"))
        m_Pen2 = New Drawing.Pen(colorScheme(myColorScheme)("BackColor"))
    End Sub
    Public Sub DrawNextTimeValue(ByRef PositionValue As Integer)
        If Not IsNothing(m_GrapValue) Then
            If PositionValue + 3 >= Label5.Width Then
                PositionValue = 0
            End If
            Try
                If Monitor.TryEnter(m_GrapValue, 30) Then
                    'Cancella le linee precedenti
                    m_GrapValue.DrawLine(m_Pen2, PositionValue, 2, PositionValue + 3, 2)
                    m_GrapValue.DrawLine(m_Pen2, PositionValue, 9, PositionValue + 3, 9)
                    m_GrapValue.DrawLine(m_Pen2, PositionValue, 2, PositionValue, 10)
                    If m_Value Then
                        'Nuova linea
                        m_GrapValue.DrawLine(m_Pen1, PositionValue, 2, PositionValue + 1, 2)
                    Else
                        m_GrapValue.DrawLine(m_Pen1, PositionValue, 9, PositionValue + 1, 9)
                    End If
                    If m_PrevValue <> m_Value Then
                        'Linea verticale se il valore � cambiato
                        m_GrapValue.DrawLine(m_Pen1, PositionValue, 2, PositionValue, 9)
                    End If
                    Monitor.Exit(m_GrapValue)
                End If
                m_PrevValue = m_Value
            Catch ex As System.Exception
                Monitor.Exit(m_GrapValue)
            End Try
        End If
    End Sub
    Public Sub ClearTimeValue()
        If Not IsNothing(m_GrapValue) Then
            Try
                If Monitor.TryEnter(m_GrapValue, 2000) Then
                    m_GrapValue.Clear(colorScheme(myColorScheme)("BackColor"))
                    Monitor.Exit(m_GrapValue)
                End If
            Catch ex As System.Exception
                Monitor.Exit(m_GrapValue)
            End Try
        End If
    End Sub
    Public Sub RefreshScale(ByVal Span As Integer)
        m_Span = Span
        ReDrawScale()
    End Sub
    Private Sub ReDrawScale()
        If m_Span > 0 Then
            Dim ActualXPosition As Single = m_Span
            Dim PenTemp As New Drawing.Pen(colorScheme(myColorScheme)("InverseColor"))
            Dim m_GraphicScale As drawing.Graphics = Panel1.CreateGraphics
            m_GraphicScale.Clear(colorScheme(myColorScheme)("BackColor"))
            While (ActualXPosition < Panel1.Width)
                m_GraphicScale.DrawLine(PenTemp, ActualXPosition, 0, ActualXPosition, Panel1.Height)
                ActualXPosition = ActualXPosition + m_Span
            End While
        End If
    End Sub
    Private Sub ValueChanged() Handles m_Var.ValueChanged
        m_Value = CBool(m_Var.ReadValue)
        If m_monitoring Then
            Label2.Text = m_Value
            If m_Value Then
                Button1.BackColor = Drawing.color.Red
            Else
                Button1.BackColor = Drawing.color.Blue
            End If
        End If
    End Sub
    Private Sub IntValueChanged() Handles m_Var.ActValueChanged
        ' previene alcuni errori nel ridisegno che non fanno vedere le modifiche
        m_NumericValue = m_Var.ReadActValue
        Label4.Text = m_Var.ValueToUniversalString()
        Me.Refresh()
        Label4.Refresh()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Inverte il valore della variabile se � attivo il monitoraggio
        If m_monitoring Then
            m_Var.SetValue(Not m_Var.ReadValue)
        End If
    End Sub
    Private Sub VariableControl_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        If Not IsNothing(m_GrapValue) Then
            If Monitor.TryEnter(m_GrapValue, 100) Then
                m_GrapValue = Label5.CreateGraphics
            End If
        End If
        ReDrawScale()
    End Sub
    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint
        ReDrawScale()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        m_Var.IncreaseValue(m_Var.ReadActValue)
        RefreshControl()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        m_Var.DecreaseValue(m_Var.ReadActValue)
        RefreshControl()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        m_Var.ResetValue()
        RefreshControl()
    End Sub
End Class
