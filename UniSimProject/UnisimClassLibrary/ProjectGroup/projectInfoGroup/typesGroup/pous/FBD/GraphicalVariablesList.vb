Imports System.IO
Imports System.Xml
Imports System.Collections.Generic
Public Class GraphicalVariablesList
    Inherits List(Of GraphicalVariable)
    Implements IXMLSerializable

    Private MyBody As IIEC61131LanguageImplementation

    Public Sub New(ByVal bd As IIEC61131LanguageImplementation)
        MyBody = bd
    End Sub

    Public Property FBDBody() As IIEC61131LanguageImplementation
        Get
            Return MyBody
        End Get
        Set(ByVal value As IIEC61131LanguageImplementation)
            MyBody = value
        End Set
    End Property

    Public Function CreateInstance(ByVal body As IIEC61131LanguageImplementation) As GraphicalVariablesList
        Dim ret As New GraphicalVariablesList(body)
        For Each GV As GraphicalVariable In Me
            Dim var As BaseVariable = body.PouInterface.FindVariableByName(GV.BoundVariable.Name)
            If var Is Nothing Then var = body.ResGlobalVariables.FindVariableByName(GV.BoundVariable.Name)
            ret.Add(New GraphicalVariable(body, _
                var, GV.Number, GV.Dimension, GV.Position, _
                    GV.VariableType, GV.ExecutionOrderID))
        Next
        Return ret
    End Function

    Public Function FindAndSelectVariable(ByVal x As Integer, ByVal y As Integer) As Boolean
        For Each GV As GraphicalVariable In Me
            If GV.MyAreaIsHere(x, y) Then
                GV.SelectObject()
                Return True
            End If
        Next
        Return False
    End Function

    Public Function FindVariable(ByVal x As Integer, ByVal y As Integer) As Boolean
        For Each GV As GraphicalVariable In Me
            If GV.MyAreaIsHere(x, y) Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function ReadIfVariableSelected(ByVal x As Integer, ByVal y As Integer) As Boolean
        For Each GV As GraphicalVariable In Me
            If GV.MyAreaIsHere(x, y) Then Return GV.Selected
        Next
        Return False
    End Function
    Public Sub FindAndSelectVariables(ByVal Rect As Drawing.Rectangle)
        For Each GV As GraphicalVariable In Me
            If GV.ObjectRectangle.IntersectsWith(Rect) Then GV.SelectObject()
        Next
    End Sub
    Public Function FindVariableByNumber(ByVal n As Integer) As GraphicalVariable
        For Each GV As GraphicalVariable In Me
            If GV.Number = n Then Return GV
        Next
        Return Nothing
    End Function
    Function FirstAvailableVariableNumber() As Integer
        Dim i As Integer = 1
        While FindVariableByNumber(i) IsNot Nothing
            i += 1
        End While
        Return i
    End Function
    Private Function _IsSelectedVariable(ByVal V As GraphicalVariable) As Boolean
        Return V.Selected
    End Function
    Public Sub RemoveSelectedElements()
        ' Complessità: O(Count^2)
        While True
            Dim i As Integer = Me.FindIndex(AddressOf _IsSelectedVariable)
            If i < 0 Then Exit Sub
            Me(i).Dispose()
            Me.RemoveAt(i)
        End While
    End Sub
    Public Function FindAndSelectSmallRectangleVariable(ByVal x As Integer, ByVal y As Integer) As Boolean
        For Each GV As GraphicalVariable In Me
            If GV.MyRectangleIsHere(x, y) Then
                GV.RectangleSelected = Not (GV.RectangleSelected)
                Return True
            End If
        Next
        Return False
    End Function
    Public Sub DeselectAll()
        For Each GV As GraphicalVariable In Me
            GV.DeselectObject()
            GV.DeselectRectangle()
        Next
    End Sub
    Public Sub SelectAll()
        For Each GV As GraphicalVariable In Me
            GV.SelectObject()
        Next
    End Sub
    Public Sub DrawArea(ByVal Rect As Drawing.Rectangle, ByVal DrawSmallRectangles As Boolean)
        For Each GV As GraphicalVariable In Me
            If GV.ObjectRectangle.IntersectsWith(Rect) Then GV.Draw(DrawSmallRectangles)
        Next
    End Sub
    Public Sub DrawVariables()
        For Each GV As GraphicalVariable In Me
            GV.Draw(False)
        Next
    End Sub
    Public Sub DrawSelection(ByVal DrawSmallRectangles As Boolean)
        For Each GV As GraphicalVariable In Me
            If GV.Selected Then GV.Draw(DrawSmallRectangles)
        Next
    End Sub
    Public Sub MoveSelection(ByVal dx As Integer, ByVal dy As Integer)
        For Each GV As GraphicalVariable In Me
            If GV.Selected Then GV.Move(dx, dy)
        Next
    End Sub
    Public Sub CancelSelection(ByVal CancelSmallRectangles As Boolean)
        For Each GV As GraphicalVariable In Me
            If GV.Selected Then GV.Cancel(CancelSmallRectangles)
        Next
    End Sub
    Public Function CheckForSelectionOutside(ByVal R As Drawing.Rectangle, ByRef FuoriX As Boolean, ByRef FuoriY As Boolean) As Boolean
        'Controlla che l'area delle Steps selezionate....
        '....non sia fuori da R (se è fuori restituisce True)
        For Each GV As GraphicalVariable In Me
            If GV.Selected Then
                CheckForSelectionOutside = Outside(R, GV.ObjectRectangle, FuoriX, FuoriY)
                If CheckForSelectionOutside Then
                    Exit For
                End If
            End If
        Next
    End Function
    Public Function CountSelected() As Integer
        CountSelected = 0
        For Each GV As GraphicalVariable In Me
            ' CountSelected += IIf(Of Integer)(GV.Selected,1,0) ' prestazioni probabilmente pessime        
            If GV.Selected Then CountSelected += 1
        Next
    End Function
    Public Sub SetGraphToDraw(ByVal Graph As Drawing.Graphics)
        For Each GV As GraphicalVariable In Me
            GV.SetGraphToDraw(Graph)
        Next
    End Sub
    Public Sub ResolveVariableLinks()
        For Each GV As GraphicalVariable In Me
            GV.ResolveVariableLinks()
        Next
    End Sub

    Public Sub xmlExport(ByRef RefXMLProjectWriter As System.Xml.XmlTextWriter) Implements IXMLExportable.xmlExport
        For Each GV As GraphicalVariable In Me
            GV.xmlExport(RefXMLProjectWriter)
        Next
    End Sub

    Public Sub xmlImport(ByRef RefXmlProjectReader As System.Xml.XmlTextReader) Implements IXMLImportable.xmlImport

    End Sub

    Public Function FindAllVariablesOfType(Optional ByVal vType As GraphicalVariableType = GraphicalVariableType.Output) As GraphicalVariablesList
        Dim ret As New GraphicalVariablesList(Me.FBDBody)
        For Each GV As GraphicalVariable In Me
            If GV.VariableType = vType Then ret.Add(GV)
        Next
        Return ret
    End Function

    Public Function IsAlreadyBoundToOutVariable(ByVal bv As BaseVariable) As Boolean
        For Each GV As GraphicalVariable In FindAllVariablesOfType(GraphicalVariableType.Output)
            If GV.BoundVariable.Name = bv.Name Then Return True
        Next
        Return False
    End Function

    Public Function ReadVariableSelected() As GraphicalVariable
        For Each GV As GraphicalVariable In Me
            If GV.Selected Then Return GV
        Next
        Return Nothing
    End Function

End Class