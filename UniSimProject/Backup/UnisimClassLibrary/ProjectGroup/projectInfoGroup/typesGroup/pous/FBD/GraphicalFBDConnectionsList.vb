Imports System.IO
Imports System.Xml
Imports System.Collections.Generic
Public Class GraphicalFBDConnectionsList
    Inherits List(Of GraphicalFBDConnection)
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

    Public Function CreateInstance(ByVal body As IIEC61131LanguageImplementation) As GraphicalFBDConnectionsList
        Dim ret As New GraphicalFBDConnectionsList(body)
        For Each GC As GraphicalFBDConnection In Me
            ret.Add(New GraphicalFBDConnection(GC.SourceId, GC.DestinationId, body, GC.Negated))
        Next
        Return ret
    End Function

    Public Function FindAndSelectConnection(ByVal x As Integer, ByVal y As Integer) As Boolean
        For Each GV As GraphicalFBDConnection In Me
            If GV.MyAreaIsHere(x, y) Then
                GV.SelectObject()
                Return True
            End If
        Next
        Return False
    End Function

    Public Function FindConnection(ByVal x As Integer, ByVal y As Integer) As Boolean
        For Each GV As GraphicalFBDConnection In Me
            If GV.MyAreaIsHere(x, y) Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function ReadIfConnectionSelected(ByVal x As Integer, ByVal y As Integer) As Boolean
        For Each GV As GraphicalFBDConnection In Me
            If GV.MyAreaIsHere(x, y) Then Return GV.Selected
        Next
        Return False
    End Function
    Public Sub FindAndSelectConnections(ByVal Rect As Drawing.Rectangle)
        For Each GV As GraphicalFBDConnection In Me
            If GV.IntersectsWith(Rect) Then GV.SelectObject()
        Next
    End Sub
    Private Function _IsSelectedConnection(ByVal V As GraphicalFBDConnection) As Boolean
        Return V.Selected
    End Function
    Public Sub RemoveSelectedElements()
        ' Complessità: O(Count^2)
        While True
            Dim i As Integer = Me.FindIndex(AddressOf _IsSelectedConnection)
            If i < 0 Then Exit Sub
            Me.RemoveAt(i)
        End While
    End Sub
    Public Sub DeselectAll()
        For Each GV As GraphicalFBDConnection In Me
            GV.DeselectObject()
        Next
    End Sub
    Public Sub SelectAll()
        For Each GV As GraphicalFBDConnection In Me
            GV.SelectObject()
        Next
    End Sub
    Public Sub MoveSelection(ByVal dx As Integer, ByVal dy As Integer)
        For Each GC As GraphicalFBDConnection In Me
            If GC.Selected Then GC.Move(dx, dy)
        Next
    End Sub
    Public Sub DrawArea(ByVal Rect As Drawing.Rectangle, ByVal DrawSmallRectangles As Boolean)
        For Each GV As GraphicalFBDConnection In Me
            If GV.IntersectsWith(Rect) Then GV.Draw(DrawSmallRectangles)
        Next
    End Sub
    Public Sub DrawSelection(ByVal DrawSmallRectangles As Boolean)
        For Each GV As GraphicalFBDConnection In Me
            If GV.Selected Then GV.Draw(DrawSmallRectangles)
        Next
    End Sub
    Public Sub CancelSelection(ByVal CancelSmallRectangles As Boolean)
        For Each GV As GraphicalFBDConnection In Me
            If GV.Selected Then GV.Cancel(CancelSmallRectangles)
        Next
    End Sub
    Public Function CountSelected() As Integer
        CountSelected = 0
        For Each GV As GraphicalFBDConnection In Me
            ' CountSelected += IIf(Of Integer)(GV.Selected,1,0) ' prestazioni probabilmente pessime        
            If GV.Selected Then CountSelected += 1
        Next
    End Function
    Public Function CountConnectionsFor(ByVal obj As IFBDConnectable) As Integer
        Return FindAllConnectionsEndingWith(obj).Count + _
                FindAllConnectionsStartingWith(obj).Count
    End Function
    Public Sub SetGraphToDraw(ByVal Graph As Drawing.Graphics)
        For Each GV As GraphicalFBDConnection In Me
            GV.SetGraphToDraw(Graph)
        Next
    End Sub
    Public Sub ResolveVariableLinks()
        For Each GV As GraphicalFBDConnection In Me
            GV.ResolveVariableLinks()
        Next
    End Sub

    Public Function FindAllConnectionsStartingWith(ByVal src As IFBDConnectable) As GraphicalFBDConnectionsList
        Dim ret As New GraphicalFBDConnectionsList(Me.FBDBody)
        For Each GC As GraphicalFBDConnection In Me
            If src.Number = GC.SourceId Then ret.Add(GC)
        Next
        Return ret
    End Function

    Public Function FindAllConnectionsEndingWith(ByVal dst As IFBDConnectable) As GraphicalFBDConnectionsList
        Dim ret As New GraphicalFBDConnectionsList(Me.FBDBody)
        For Each GC As GraphicalFBDConnection In Me
            If dst.Number = GC.DestinationId Then ret.Add(GC)
        Next
        Return ret
    End Function

    Public Function ReadConnectionSelected() As GraphicalFBDConnection
        For Each GC As GraphicalFBDConnection In Me
            If GC.Selected Then Return GC
        Next
        Return Nothing
    End Function

End Class