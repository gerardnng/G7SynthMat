Imports System.IO
Imports System.Xml
Imports System.Collections.Generic
Public Class GraphicalBlocksList
    Inherits List(Of GraphicalBlock)
    Implements IXMLSerializable

    Private MyBody As FBD

    Public Sub New(ByVal bd As FBD)
        MyBody = bd
    End Sub

    Public Property FBDBody() As FBD
        Get
            Return MyBody
        End Get
        Set(ByVal value As FBD)
            MyBody = value
        End Set
    End Property

    Public Function FindAndSelectBlock(ByVal x As Integer, ByVal y As Integer) As Boolean
        For Each GV As GraphicalBlock In Me
            If GV.MyAreaIsHere(x, y) Then
                GV.SelectObject()
                Return True
            End If
        Next
        Return False
    End Function

    Public Function FindBlock(ByVal x As Integer, ByVal y As Integer) As Boolean
        For Each GV As GraphicalBlock In Me
            If GV.MyAreaIsHere(x, y) Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function ReadIfBlockSelected(ByVal x As Integer, ByVal y As Integer) As Boolean
        For Each GV As GraphicalBlock In Me
            If GV.MyAreaIsHere(x, y) Then Return GV.Selected
        Next
        Return False
    End Function
    Public Sub FindAndSelectBlocks(ByVal Rect As Drawing.Rectangle)
        For Each GV As GraphicalBlock In Me
            If GV.ObjectRectangle.IntersectsWith(Rect) Then GV.SelectObject()
        Next
    End Sub
    Public Function FindBlockByNumber(ByVal n As Integer) As GraphicalBlock
        For Each GV As GraphicalBlock In Me
            If GV.Number = n Then Return GV
        Next
        Return Nothing
    End Function
    Function FirstAvailableBlockeNumber() As Integer
        Dim i As Integer = 1
        While FindBlockByNumber(i) IsNot Nothing
            i += 1
        End While
        Return i
    End Function
    Private Function _IsSelectedBlock(ByVal V As GraphicalBlock) As Boolean
        Return V.Selected
    End Function
    Public Sub RemoveSelectedElements()
        ' Complessit�: O(Count^2)
        While True
            Dim i As Integer = Me.FindIndex(AddressOf _IsSelectedBlock)
            If i < 0 Then Exit Sub
            Me.RemoveAt(i)
        End While
    End Sub
    Public Function FindAndSelectSmallRectangleBlock(ByVal x As Integer, ByVal y As Integer) As Boolean
        For Each GV As GraphicalBlock In Me
            If GV.MyInRectangleIsHere(x, y) Then
                GV.InRectangleSelected = Not (GV.InRectangleSelected)
                Return True
            End If
            If GV.MyOutRectangleIsHere(x, y) Then
                GV.OutRectangleSelected = Not (GV.OutRectangleSelected)
                Return True
            End If
        Next
        Return False
    End Function
    Public Sub DeselectAll()
        For Each GV As GraphicalBlock In Me
            GV.DeselectObject()
            GV.DeselectInRectangle()
            GV.DeselectOutRectangle()
        Next
    End Sub
    Public Sub SelectAll()
        For Each GV As GraphicalBlock In Me
            GV.SelectObject()
        Next
    End Sub
    Public Sub DrawArea(ByVal Rect As Drawing.Rectangle, ByVal DrawSmallRectangles As Boolean)
        For Each GV As GraphicalBlock In Me
            If GV.ObjectRectangle.IntersectsWith(Rect) Then GV.Draw(DrawSmallRectangles)
        Next
    End Sub
    Public Sub DrawSelection(ByVal DrawSmallRectangles As Boolean)
        For Each GV As GraphicalBlock In Me
            If GV.Selected Then GV.Draw(DrawSmallRectangles)
        Next
    End Sub
    Public Sub MoveSelection(ByVal dx As Integer, ByVal dy As Integer)
        For Each GV As GraphicalBlock In Me
            If GV.Selected Then GV.Move(dx, dy)
        Next
    End Sub
    Public Sub CancelSelection(ByVal CancelSmallRectangles As Boolean)
        For Each GV As GraphicalBlock In Me
            If GV.Selected Then GV.Cancel(CancelSmallRectangles)
        Next
    End Sub
    Public Function CheckForSelectionOutside(ByVal R As Drawing.Rectangle, ByRef FuoriX As Boolean, ByRef FuoriY As Boolean) As Boolean
        'Controlla che l'area delle Steps selezionate....
        '....non sia fuori da R (se � fuori restituisce True)
        For Each GV As GraphicalBlock In Me
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
        For Each GV As GraphicalBlock In Me
            ' CountSelected += IIf(Of Integer)(GV.Selected,1,0) ' prestazioni probabilmente pessime        
            If GV.Selected Then CountSelected += 1
        Next
    End Function
    Public Sub SetGraphToDraw(ByVal Graph As Drawing.Graphics)
        For Each GV As GraphicalBlock In Me
            GV.SetGraphToDraw(Graph)
        Next
    End Sub
    Public Sub ResolveVariableLinks()
        For Each GV As GraphicalBlock In Me
            GV.ResolveVariableLinks()
        Next
    End Sub

    Public Function ReadBlockSelected() As GraphicalBlock
        For Each GB As GraphicalBlock In Me
            If GB.Selected Then Return GB
        Next
        Return Nothing
    End Function

    Public Sub xmlExport(ByRef RefXMLProjectWriter As System.Xml.XmlTextWriter) Implements IXMLExportable.xmlExport
        For Each GB As GraphicalBlock In Me
            GB.xmlExport(RefXMLProjectWriter)
        Next
    End Sub

    Public Sub xmlImport(ByRef RefXmlProjectReader As System.Xml.XmlTextReader) Implements IXMLImportable.xmlImport

    End Sub
End Class