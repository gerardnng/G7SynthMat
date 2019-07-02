Imports System.Drawing
Imports System.Math
Imports System.Xml
Imports System.Threading

Imports System.Windows.Forms


Public Class FormLd
    Inherits System.Windows.Forms.Form
    Private CurrentOperation As Operation
    Private m_GraphToDraw As Drawing.Graphics
    Private CoeffSnap As Integer
    Private EditingMode As Boolean
    Private TreeOpen As Boolean
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Private Foglio As Drawing.Rectangle
    Private InitialPoint As Drawing.Point
    Private WithEvents m_Ladder As Ladder = New Ladder       'l'instanza non va però qui, va fatta in body !!
    Private NextElementName As String
    Private NextElementDocumentation As String
    Private CursorStep As New Cursor(CursorsPath & "\CursorStep.Cur")
    Private ColSfondo As Drawing.Color = Drawing.Color.White
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Private CursorMove As New Cursor(CursorsPath & "\CursorMove.cur")


    Private Enum Operation
        DefiningContact
        Contact
        Coil
        Selection
        Selected
    End Enum
#Region " Codice generato da Progettazione Windows Form "

    Public Sub New()
        MyBase.New()
        CoeffSnap = 4

        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()

    End Sub

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form.
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents ToolBar1 As System.Windows.Forms.ToolBar
    Friend WithEvents contact As System.Windows.Forms.ToolBarButton
    Friend WithEvents coil As System.Windows.Forms.ToolBarButton
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents TreeView2 As System.Windows.Forms.TreeView
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.ToolBar1 = New System.Windows.Forms.ToolBar
        Me.contact = New System.Windows.Forms.ToolBarButton
        Me.coil = New System.Windows.Forms.ToolBarButton
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.TreeView2 = New System.Windows.Forms.TreeView
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem1})
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 0
        Me.MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem2, Me.MenuItem3})
        Me.MenuItem1.Text = "Ladder"
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 0
        Me.MenuItem2.Text = "Contact"
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 1
        Me.MenuItem3.Text = "Coil"
        '
        'ToolBar1
        '
        Me.ToolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.contact, Me.coil})
        Me.ToolBar1.ButtonSize = New System.Drawing.Size(24, 24)
        Me.ToolBar1.DropDownArrows = True
        Me.ToolBar1.Location = New System.Drawing.Point(0, 0)
        Me.ToolBar1.Name = "ToolBar1"
        Me.ToolBar1.ShowToolTips = True
        Me.ToolBar1.Size = New System.Drawing.Size(492, 30)
        Me.ToolBar1.TabIndex = 5
        '
        'contact
        '
        Me.contact.Text = "contact"
        Me.contact.ToolTipText = "contact"
        '
        'coil
        '
        Me.coil.Text = "coil"
        Me.coil.ToolTipText = "coil"
        '
        'Panel3
        '
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.ForeColor = System.Drawing.Color.Black
        Me.Panel3.Location = New System.Drawing.Point(0, 335)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(492, 18)
        Me.Panel3.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.AutoScroll = True
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(224, Byte), CType(224, Byte), CType(224, Byte))
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel2.Controls.Add(Me.Panel4)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 30)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(492, 305)
        Me.Panel2.TabIndex = 6
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.White
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(800, 1200)
        Me.Panel4.TabIndex = 1
        '
        'TreeView2
        '
        Me.TreeView2.Dock = System.Windows.Forms.DockStyle.Left
        Me.TreeView2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.25!)
        Me.TreeView2.ImageIndex = -1
        Me.TreeView2.Location = New System.Drawing.Point(0, 30)
        Me.TreeView2.Name = "TreeView2"
        Me.TreeView2.SelectedImageIndex = -1
        Me.TreeView2.Size = New System.Drawing.Size(88, 305)
        Me.TreeView2.TabIndex = 9
        Me.TreeView2.Visible = False
        '
        'FormLd
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(492, 353)
        Me.Controls.Add(Me.TreeView2)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.ToolBar1)
        Me.ForeColor = System.Drawing.Color.Red
        Me.Menu = Me.MainMenu1
        Me.MinimumSize = New System.Drawing.Size(320, 110)
        Me.Name = "FormLd"
        Me.ShowInTaskbar = False
        Me.Text = "FormLd"
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub ToolBar1_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ToolBar1.ButtonClick
        If e.Button Is contact Then
            Test("premuto contact")
            AddContact()

        ElseIf e.Button Is coil Then
            Test("premuto coil")
        End If
    End Sub

    Public Sub AddContact()

        'ResetCurrentOperation()
        'Cerca il numero della prima fase libera
        'e  passa il valore alla finestra di dialogo

        Dim StepDialog As New StepDialogForm

        'StepDialog.StepName = m_Ladder.FirstAvaiableStepName

        Dim ResultDialog As System.Windows.Forms.DialogResult = StepDialog.ShowDialog()
        If ResultDialog = DialogResult.OK Then

            'Controlla se il nome della fase è già presente
            'If Not IsNothing(m_Ladder.FindStepByName(StepDialog.StepName)) Then
            ' MsgBox("Step " & StepDialog.StepName & " already exist!")
            'Else
            ' NextElementName = StepDialog.StepName
            ' CurrentOperation = Operation.DefiningContact
            'End If
        End If
        CurrentOperation = Operation.DefiningContact
        StepDialog.Dispose()
    End Sub



    Public Sub ResetCurrentOperation()
        CurrentOperation = Operation.Selection
        CancelVisibleArea()
        DrawVisibleArea()
    End Sub
    Private Function MoveSelection(ByVal dx As Integer, ByVal dy As Integer)
        CancelSelection()
        m_Ladder.MoveSelection(dx, dy)
        DrawVisibleArea()
    End Function
    Private Sub CancelSelection()
        m_Ladder.CancelSelection(True)

    End Sub

    Private Sub CancelVisibleArea()
        Try
            If Monitor.TryEnter(m_GraphToDraw, 2000) Then
                m_GraphToDraw.Clear(ColSfondo)
            End If
        Catch ex As System.Exception
        Finally
            Monitor.Exit(m_GraphToDraw)
        End Try

    End Sub
    Private Function Snap(ByVal val As Integer) As Integer
        Snap = CInt(val / CoeffSnap) * CoeffSnap

    End Function
    Private Sub DrawVisibleArea()
        Dim R As New Drawing.Rectangle(0 - Panel1.Left, 0 - Panel1.Top, Panel1.Width - Panel1.Left, Panel1.Height - Panel1.Top)
        DrawArea(R)
    End Sub

    Private Sub DrawArea(ByVal Rect As Drawing.Rectangle)
        Rect.X = Rect.X - 1
        Rect.Y = Rect.Y - 1
        Rect.Width = Rect.Width + 2
        Rect.Height = Rect.Height + 2
        ''If CurrentOperation = Operation.DefiningTransition Then
        'm_Sfc.DrawElementsArea(Rect, True)
        'Else
        '    m_Sfc.DrawElementsArea(Rect, False)
        'End If
    End Sub
    Private Sub Panel4_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        'If EditingMode Then
        Select Case CurrentOperation
            'Case Operation.Selected
            '   Panel2.Cursor = CursorMove
            'Effettua lo Movemento solo se dopo lo snap....
            '....è maggiore di 0
            '           Dim dx As Integer = Snap(e.X - InitialPoint.X)
            '          Dim dy As Integer = Snap(e.Y - InitialPoint.Y)
            '         If dx <> 0 Or dy <> 0 Then
            '        Dim R As New Drawing.Rectangle(-dx, -dy, Foglio.Width - dx, Foglio.Height - dy)
            '       Dim FuoriX, FuoriY As Boolean
            '      'Simula l'area del foglio Moveta nei versi opposti
            '     If Not m_Ladder.ControllaSelectionFuoriArea(R, FuoriX, FuoriY) Then
            '     'Move la Selection se ci riesce
            '     MoveSelection(dx, dy)
            '     InitialPoint.X = Snap(e.X)
            '         InitialPoint.Y = Snap(e.Y)
            '         Else
            '            If Not FuoriX And dx <> 0 Then
            '        ''Move la Selection solo di dx se ci riesce
            '        MoveSelection(dx, 0)
            '        InitialPoint.X = Snap(e.X)
            '            End If
            '            If Not FuoriY And dy <> 0 Then
            '        ''Move la Selection solo di dy se ci riesce
            '        MoveSelection(0, dy)
            '        InitialPoint.Y = Snap(e.Y)
            '           End If
            '       End If
            '       End If
        Case Operation.DefiningContact
                Panel4.Cursor = CursorStep
                'Case Operation.DefiningMacroStep
                '   Panel1.Cursor = CursorMacroStep
                'Case Operation.DefiningTransition
                '   Panel1.Cursor = CursorTransition
                'Case Operation.MultipleSelection
                '    CancelMultipleSelectionRectangle(CreaRectangle(InitialPoint.X, InitialPoint.Y, PreviousFinelPoint.X, PreviousFinelPoint.Y))
                '   DrawArea(CreaRectangle(InitialPoint.X, InitialPoint.Y, PreviousFinelPoint.X, PreviousFinelPoint.Y))
                '  DrawMultipleSelectionRectangle(CreaRectangle(InitialPoint.X, InitialPoint.Y, e.X, e.Y))
                ' PreviousFinelPoint.X = e.X
                'PreviousFinelPoint.Y = e.Y
            Case Else
                Panel4.Cursor = System.Windows.Forms.Cursors.Default
        End Select
        ' End If
    End Sub
    Private Function StoreStep(ByVal P As Drawing.Point) As Boolean
        'qui ci arriva!!
        StoreStep = m_Ladder.AddAndDrawStep(m_Ladder.FirstAvaiableElementNumber, NextElementName, NextElementDocumentation, P)
        'qui pure ci arriva però sembra che non fa nulla!!
        If TreeOpen Then
            TreeView1.Nodes(0).Nodes.Add(NextElementName)
            TreeView1.Nodes(0).Expand()
        End If
    End Function

    Private Sub Panel4_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel4.MouseDown
        Me.Activate()

        'If EditingMode Then    ''manca la funzione editing

        'Legge il tasto del mouse premuto
        Select Case e.Button
            Case MouseButtons.Left
                Test("premuto il tasto sinistro del mause di ladder")
                'Tasto sinistro del mouse
                Select Case CurrentOperation
                    Case Is = Operation.DefiningContact                      'qui ci arriva
                        Dim P As New Drawing.Point(Snap(e.X), Snap(e.Y))
                        'Chiama AddAndDrawStep
                        If StoreStep(P) Then
                            CurrentOperation = Operation.Selection
                        Else
                            'Test("non sono entrato")
                        End If

                End Select

                'Case MouseButtons.Middle, MouseButtons.Right
                '    'Tasto desto o centrale del mouse
                '     Select Case CurrentOperation
                ' Case Operation.DefiningTransition
                '      ConfirmAddTransition()
                ' End Select
        End Select
        ' Else
        'Non è in modo di editing quindi seleziona solo la macrofasi per aprirle
        '   m_Ladder.FindAndSelectElement(e.X, e.Y)
        'End If
    End Sub

    Private Sub FormLd_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Panel4_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs)
        DrawArea(e.ClipRectangle)
    End Sub

    Private Sub Panel3_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel3.Paint

    End Sub

    Private Sub Panel4_Paint_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel4.Paint

    End Sub
End Class
