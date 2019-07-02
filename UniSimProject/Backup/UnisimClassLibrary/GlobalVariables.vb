Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Structure LineSegment
    Private m_Point1 As Drawing.Point
    Private m_Point2 As Drawing.Point
    Public Sub New(ByVal p1 As Drawing.Point, ByVal p2 As Drawing.Point)
        m_Point1 = p1
        m_Point2 = p2
    End Sub
    Public Property FirstPoint() As Drawing.Point
        Get
            Return m_Point1
        End Get
        Set(ByVal value As Drawing.Point)
            m_Point1 = value
        End Set
    End Property
    Public Property SecondPoint() As Drawing.Point
        Get
            Return m_Point2
        End Get
        Set(ByVal value As Drawing.Point)
            m_Point2 = value
        End Set
    End Property
    Public Sub Draw(ByVal GraphToDraw As Drawing.Graphics, ByVal Color As Drawing.Color)
        GraphToDraw.DrawLine(New Drawing.Pen(Color), FirstPoint, SecondPoint)
    End Sub
End Structure

Module MultiExecutors

    Public Delegate Sub UnaryFunctor(Of ArgumentType)(ByVal Arg As ArgumentType)
    Public Delegate Function UnaryFunctor(Of ArgumentType, ReturnType)(ByVal Arg As ArgumentType) As ReturnType

    Public Sub ExecuteUnaryFunctor(Of ArgumentType)(ByVal f As UnaryFunctor(Of ArgumentType), _
                   ByVal ParamArray params() As ArgumentType)
        For Each param As ArgumentType In params
            f(param)
        Next
    End Sub

    Public Sub ExecuteUnaryFunctor(Of ArgumentType, ReturnType)(ByVal f As UnaryFunctor(Of ArgumentType, ReturnType), _
                   ByVal ParamArray params() As ArgumentType)
        For Each param As ArgumentType In params
            f(param)
        Next
    End Sub

    ' Un vero trionfo del late binding :-)
    Public Function ExecuteMethod(ByVal metName As String, ByVal tgtObject As Object, _
        ByVal ParamArray params() As Object) As Object
        Dim tgtType As Type = tgtObject.GetType()
        Dim tgtMethod As Reflection.MethodInfo = _
            tgtType.GetMethod(metName, Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public, Nothing, _
             Reflection.CallingConventions.Standard, Type.GetTypeArray(params), Nothing)
        If tgtMethod Is Nothing Then Throw New InvalidProgramException()
        Return tgtMethod.Invoke(tgtObject, Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public, _
             Nothing, params, Nothing)
    End Function

End Module

Module Globals
    Public CursorsPath As String = IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "cursors")
    Public XmlSchemaPath As String = IO.Path.Combine(IO.Path.Combine( _
        System.Windows.Forms.Application.StartupPath, "xml"), "TC6_XML_V10.xsd")
    Public InvalidVariableChars As String = "+|""'<>().,:; /\]^%@-*#?![]~{}"
    Public WithEvents ResourceTab As New ResourceTable
    Public Null As New Object

    ' Sostituisce l'istruzione vuota ;)
    Public Sub Nop()

    End Sub

    ' Operatore ternario C ? IfTrue : IfFalse
    ' Nota: Non si pu� scrivere un'istruzione come
    ' IIf(Of T)(X Is Nothing,T.DefaultValue(),X.GetT())
    ' poich� IfTrue e IfFalse vengono entrambi valutati prima di essere passati a questa routine
    Public Function IIf(Of OutType)(ByVal C As Boolean, ByVal IfTrue As OutType, ByVal IfFalse As OutType) As OutType
        If C Then Return IfTrue
        Return IfFalse
    End Function

    Public Function IsValidVariableName(ByVal name As String) As Boolean
        Return name.IndexOfAny(InvalidVariableChars.ToCharArray()) < 0
    End Function

    Public Function MiddlePoint(ByVal r As Drawing.Rectangle) As Drawing.Point
        Return New Drawing.Point(r.Location.X + r.Size.Width / 2, _
            r.Location.Y + r.Size.Height / 2)
    End Function

    Public Function MiddlePoint(ByVal p As Drawing.Point, ByVal s As Drawing.Size) As Drawing.Point
        Return MiddlePoint(New Drawing.Rectangle(p, s))
    End Function

    Public Function ParseInvariantDouble(ByVal param As String) As Double
        Return Double.Parse(param, System.Globalization.CultureInfo.InvariantCulture.NumberFormat)
    End Function



    ' Gli offset sono espressi in frazione di textSize.Width e textSize.Height
    Public Sub DrawString(ByVal strToDraw As String, ByVal GraphToDraw As Graphics, ByVal Col As Color, _
        ByVal BaseObject As IGraphicalObject, _
            Optional ByVal xOffset As Single = 0, Optional ByVal yOffset As Single = 0)
        DrawString(strToDraw, GraphToDraw, New SolidBrush(Col), BaseObject, xOffset, yOffset)
    End Sub

    Public Sub DrawString(ByVal strToDraw As String, ByVal GraphToDraw As Graphics, ByVal Br As Brush, _
        ByVal BaseObject As IGraphicalObject, _
            Optional ByVal xOffset As Single = 0, Optional ByVal yOffset As Single = 0)
        If GraphToDraw Is Nothing Then Exit Sub
        If System.Threading.Monitor.TryEnter(GraphToDraw, 2000) Then
            Try
                Dim middlePoint As New Point(BaseObject.Position.X + BaseObject.Size.Width / 2, _
                BaseObject.Position.Y + BaseObject.Size.Height / 2)
                Dim textSize As SizeF = GraphToDraw.MeasureString(strToDraw, TextFont)
                Dim textPoint As New Point(middlePoint.X - textSize.Width / 2 + CInt(xOffset * textSize.Width), _
                    middlePoint.Y - textSize.Height / 2 + CInt(yOffset * textSize.Height))
                Dim textRectangle As New RectangleF(textPoint, textSize)
                textRectangle.Intersect(New Drawing.RectangleF(BaseObject.Position.X, BaseObject.Position.Y, _
                    BaseObject.Size.Width, BaseObject.Size.Height))
                Try
                    GraphToDraw.DrawString(strToDraw, TextFont, Br, textRectangle)
                Catch ex As Exception
                End Try
            Finally
                System.Threading.Monitor.Exit(GraphToDraw)
            End Try
        End If
    End Sub


    ' Controlla che, su nessuno dei due assi, l'area Area sia al di fuori del rettangolo R
    ' (Outside = X OrElse Y)
    Public Function Outside(ByVal R As Drawing.Rectangle, ByVal Area As Drawing.Rectangle, _
        ByRef X As Boolean, ByRef Y As Boolean) As Boolean
        If Area.X <= R.X Or Area.X + Area.Width >= R.X + R.Width Then
            X = True
            Outside = True
        End If
        If Area.Y <= R.Y Or Area.Y + Area.Height >= R.Y + R.Height Then
            Y = True
            Outside = True
        End If
        ' Return Outside
    End Function

    ' Crea una copia di s in cui le sequenze di due o pi� c di seguito vengono ridotte ad una sola istanza
    ' esempio: TrimDuplicates("a+++++b*c","+"c) = "a+b*c". La funzione � implementata come un automa a stati
    ' finiti (in particolare due, gestiti dalla variabile isTrimming). Si potrebbe ottimizzare la parte d = c
    ' ma conviene mantenere l'aderenza al modello standard di FSA
    Public Function TrimDuplicates(ByVal s As String, ByVal c As Char) As String
        Dim builder As New System.Text.StringBuilder()
        Dim isTrimming As Boolean = False
        Dim i As Integer = 0
        Dim l As Integer = s.Length
        While i < l
            Dim d As Char = s(i)
            If isTrimming Then
                If d = c Then GoTo Cont
                isTrimming = False
                builder.Append(c)
                builder.Append(d)
            Else
                If d = c Then
                    isTrimming = True
                    GoTo Cont
                End If
                builder.Append(d)
            End If
Cont:
            i += 1
        End While
        ' Se abbiamo finito la stringa contando i c, aggiungine uno a mano
        If isTrimming Then builder.Append(c)
        Return builder.ToString()
    End Function

    ' Costanti globali associate alla grafica
    Public ReadOnly BackColor As Drawing.Color = Drawing.Color.White
    Public ReadOnly SelectedColor As Drawing.Color = Drawing.Color.Blue
    Public ReadOnly NotSelectedColor As Drawing.Color = Drawing.Color.Black
    Public ReadOnly TextColor As Drawing.Color = Drawing.Color.Black
    Public ReadOnly TextFont As Drawing.Font = New Drawing.Font("Arial", 8)
    Public ReadOnly ActiveColor As Drawing.Color = Drawing.Color.Red
    Public ReadOnly DeactiveColor As Drawing.Color = BackColor
    Public ReadOnly PreactiveColor As Drawing.Color = Drawing.Color.Pink
    Public ReadOnly TrueColor As Drawing.Color = Drawing.Color.Green
    Public ReadOnly FalseColor As Drawing.Color = Drawing.Color.Red

    ' Swap generiche e non generiche, e basate su oggetti o su indici di array
#Region "Swap"

    Public Sub Swap(Of T As Class)(ByRef Obj1 As T, ByRef Obj2 As T)
        Dim temp As T = Obj1
        Obj1 = Obj2
        Obj2 = temp
    End Sub

    Public Sub Swap(Of T As Class)(ByRef Objects As T(), ByVal Idx1 As Integer, ByVal Idx2 As Integer)
        Swap(Of T)(Objects(Idx1), Objects(Idx2))
    End Sub

    Public Sub Swap(ByRef Obj1 As Object, ByRef Obj2 As Object)
        Dim temp As Object = Obj1
        Obj1 = Obj2
        Obj2 = temp
    End Sub

    Public Sub Swap(ByRef Objects As Object(), ByVal Idx1 As Integer, ByVal Idx2 As Integer)
        Swap(Objects(Idx1), Objects(Idx2))
    End Sub

#End Region

#Region "Codice di microsoft.public.dotnet.framework.drawing"
    ' Il codice in questa regione compare nel forum http://www.dotnet247.com/247reference/msgs/10/51694.aspx
    ' ed � tratto dal newsgroup microsoft.public.dotnet.framework.drawing


    Public Function GetRoundedRect(ByVal BaseRect As Rectangle, _
        Optional ByVal Radius As Single = 5) As GraphicsPath
        Dim rectF As New RectangleF(BaseRect.X, BaseRect.Y, _
            BaseRect.Width, BaseRect.Height)
        Return GetRoundedRect(rectF, Radius)
    End Function

    Private Function GetRoundedRect(ByVal BaseRect As RectangleF, ByVal _
            Radius As Single) As Drawing2D.GraphicsPath
        ' If corner radius is less than or equal to zero, return the
        ' original(Rectangle)
        If Radius <= 0 Then Return Nothing
        ' If corner radius is greater than or equal to half the width or
        'height (whichever is shorter) then
        ' return a capsule instead of a lozenge.
        If Radius >= (Math.Min(BaseRect.Width, BaseRect.Height) / 2.0) Then _
            Return GetCapsule(BaseRect)

        Dim Diameter As Single = Radius + Radius
        Dim ArcRect As New RectangleF(BaseRect.Location, New SizeF(Diameter, _
Diameter))
        Dim RR As New Drawing2D.GraphicsPath()

        With RR
            ' top left arc
            .AddArc(ArcRect, 180, 90)

            ' top right arc
            ArcRect.X = BaseRect.Right - Diameter
            .AddArc(ArcRect, 270, 90)

            ' bottom right arc
            ArcRect.Y = BaseRect.Bottom - Diameter
            .AddArc(ArcRect, 0, 90)

            ' bottom left arc
            ArcRect.X = BaseRect.Left
            .AddArc(ArcRect, 90, 90)

            .CloseFigure()
        End With

        Return RR
    End Function

    ' .... and here's the function that returns a capsule region:

    Private Function GetCapsule(ByVal BaseRect As RectangleF) As Drawing2D.GraphicsPath
        Dim Diameter As Single
        Dim ArcRect As RectangleF
        Dim RR As New Drawing2D.GraphicsPath()

        With RR
            Try
                If BaseRect.Width > BaseRect.Height Then
                    ' Return horizontal capsule
                    Diameter = BaseRect.Height
                    ArcRect = New RectangleF(BaseRect.Location, New _
SizeF(Diameter, Diameter))
                    .AddArc(ArcRect, 90, 180)
                    ArcRect.X = BaseRect.Right - Diameter
                    .AddArc(ArcRect, 270, 180)

                ElseIf BaseRect.Height > BaseRect.Width Then
                    ' Return vertical capsule
                    Diameter = BaseRect.Width
                    ArcRect = New RectangleF(BaseRect.Location, New _
SizeF(Diameter, Diameter))
                    .AddArc(ArcRect, 180, 180)
                    ArcRect.Y = BaseRect.Bottom - Diameter
                    .AddArc(ArcRect, 0, 180)

                Else
                    ' return circle
                    .AddEllipse(BaseRect)
                End If

            Catch e As Exception
                .AddEllipse(BaseRect)
            Finally
                .CloseFigure()
            End Try
        End With

        Return RR
    End Function

#End Region

End Module
