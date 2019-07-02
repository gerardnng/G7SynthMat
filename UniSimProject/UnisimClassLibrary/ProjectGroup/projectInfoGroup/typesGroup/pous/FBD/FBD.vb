Imports System.Collections.Generic
Imports System.Threading

Public Class FBD
    Implements IIEC61131LanguageImplementation

    Private m_pouName As String
    Private m_ResGlobalVariables As VariablesLists
    Private m_PouInterface As pouInterface
    Private m_GraphicalVariablesList As GraphicalVariablesList
    Private m_GraphicalBlocksList As GraphicalFBDBlocksList
    Private m_GraphicalConnectionsList As GraphicalFBDConnectionsList
    Private m_GraphToDraw As Drawing.Graphics
    Private m_pousList As Pous

    Public Sub New(ByVal pn As String, ByVal rgv As VariablesLists, ByVal pi As pouInterface, _
        ByVal pl As Pous)
        m_pouName = pn
        m_pousList = pl
        m_ResGlobalVariables = rgv
        m_PouInterface = pi
        m_GraphicalVariablesList = New GraphicalVariablesList(Me)
        m_GraphicalBlocksList = New GraphicalFBDBlocksList(Me)
        m_GraphicalConnectionsList = New GraphicalFBDConnectionsList(Me)
    End Sub

    Public Function CreateInstance() As IIEC61131LanguageImplementation Implements IIEC61131LanguageImplementation.CreateInstance
        Dim ret As New FBD(m_pouName, m_ResGlobalVariables, m_PouInterface, m_pousList)
        ret.GraphicalVariablesList = Me.GraphicalVariablesList.CreateInstance(ret)
        ret.GraphicalBlocksList = Me.GraphicalBlocksList.CreateInstance(ret)
        ret.m_GraphicalConnectionsList = Me.m_GraphicalConnectionsList.CreateInstance(ret)
        ret.m_pousList = m_pousList
        Return ret
    End Function

    Public Property GraphicalVariablesList() As GraphicalVariablesList
        Get
            Return m_GraphicalVariablesList
        End Get
        Set(ByVal value As GraphicalVariablesList)
            m_GraphicalVariablesList = value
        End Set
    End Property

    Public Property GraphicalBlocksList() As GraphicalFBDBlocksList
        Get
            Return m_GraphicalBlocksList
        End Get
        Set(ByVal value As GraphicalFBDBlocksList)
            m_GraphicalBlocksList = value
        End Set
    End Property

    Public Property GraphicalConnectionsList() As GraphicalFBDConnectionsList
        Get
            Return m_GraphicalConnectionsList
        End Get
        Set(ByVal value As GraphicalFBDConnectionsList)
            m_GraphicalConnectionsList = value
        End Set
    End Property

    Public ReadOnly Property PousList() As Pous
        Get
            Return m_pousList
        End Get
    End Property

    Public Event EndScan() Implements IIEC61131LanguageImplementation.EndScan

    Public Sub ExecuteInit() Implements IIEC61131LanguageImplementation.ExecuteInit
        RaiseEvent StartScan()
        Reset()
        RaiseEvent EndScan()
    End Sub

    Private Sub BuildTreeNode(ByVal graphNode As IFBDConnectable, _
        ByVal treeNode As FBDTreeNode)
        Dim previousConnections As GraphicalFBDConnectionsList = _
            GraphicalConnectionsList.FindAllConnectionsEndingWith(graphNode)
        For Each previousConnection As GraphicalFBDConnection In previousConnections
            Dim srcObject As IFBDConnectable = previousConnection.SourceObject
            Dim newNode As FBDTreeNode
            If TypeOf (srcObject) Is GraphicalVariable Then
                newNode = New VariableBoundFDBTreeNode(CType(srcObject, GraphicalVariable).BoundVariable)
            Else
                newNode = FBDBlocksFactory.CreateBlock(CType(srcObject, GraphicalFBDBlock).BoundBlockType, _
                    Me.m_pousList)
            End If
            newNode.Negated = previousConnection.Negated
            treeNode.AddNode(newNode)
            BuildTreeNode(srcObject, newNode)
        Next
    End Sub

    Private Function ExecutionOrderIDComparison(ByVal X As GraphicalVariable, ByVal Y As GraphicalVariable) As Integer
        Return X.ExecutionOrderID.CompareTo(Y.ExecutionOrderID)
    End Function

    ' Assumiamo che ogni variabile sia il terminale di una sola connessione
    ' e da qui partiamo per creare l'albero associato
    Private Function BuildTrees() As List(Of FBDTree)
        Dim evalTrees As New List(Of FBDTree)
        Dim outsList As GraphicalVariablesList = _
            GraphicalVariablesList.FindAllVariablesOfType(GraphicalVariableType.Output)
        outsList.Sort(AddressOf ExecutionOrderIDComparison)
        For Each GV As GraphicalVariable In outsList
            Dim tree As New FBDTree(GV.BoundVariable)
            Dim previousConnections As GraphicalFBDConnectionsList = _
                GraphicalConnectionsList.FindAllConnectionsEndingWith(GV)
            ' Se qualcosa non è collegato, saltalo nella creazione dell'albero
            ' (ottimo esempio, il blocco RAND, o una uscita ancora non collegata)
            If previousConnections.Count = 0 Then Continue For
            Dim previousConnection As GraphicalFBDConnection = previousConnections(0)
            Dim previousBlock As IFBDConnectable = previousConnection.SourceObject
            ' Se prima c'è una variabile assumiamo che l'utente voglia semplicemente "copiare"
            If TypeOf (previousBlock) Is GraphicalVariable Then
                tree.SetRootNode(New VariableBoundFDBTreeNode(CType(previousBlock, _
                    GraphicalVariable).BoundVariable))
            Else
                ' Dovrebbe esserci un blocco, creiamo l'albero
                Dim treeRoot As GraphicalFBDBlock = CType(previousBlock, GraphicalFBDBlock)
                tree.SetRootNode(FBDBlocksFactory.CreateBlock(treeRoot.BoundBlockType, _
                    Me.m_pousList))
                tree.GetRootNode.Negated = previousConnection.Negated
                BuildTreeNode(treeRoot, tree.GetRootNode)
            End If
            ' Se non c'è l'albero, inutile aggiungere
            If tree.GetRootNode() IsNot Nothing Then evalTrees.Add(tree)
        Next
        Return evalTrees
    End Function

    Public Function ExecuteScanCycle() As Boolean Implements IIEC61131LanguageImplementation.ExecuteScanCycle
        RaiseEvent StartScan()
        Dim trees As List(Of FBDTree) = BuildTrees()
        For Each t As FBDTree In trees
            Try
                t.Execute()
            Catch ex As FBDReturnException
                Exit For
            Catch ex2 As Exception
                Continue For
            End Try
        Next
        RaiseEvent EndScan()
    End Function

    Public Property Name() As String Implements IIEC61131LanguageImplementation.Name
        Get
            Return m_pouName
        End Get
        Set(ByVal value As String)
            m_pouName = value
        End Set
    End Property

    Public Property PouInterface() As pouInterface Implements IIEC61131LanguageImplementation.PouInterface
        Get
            Return m_PouInterface
        End Get
        Set(ByVal value As pouInterface)
            m_PouInterface = value
        End Set
    End Property

    Public Sub PrintMe(ByRef Graph As System.Drawing.Graphics, ByVal Rect As System.Drawing.Rectangle) Implements IIEC61131LanguageImplementation.PrintMe
        Dim OldGraphToDraw As Drawing.Graphics = Nothing
        If Not IsNothing(m_GraphToDraw) Then
            OldGraphToDraw = m_GraphToDraw
        End If
        Dim AreaToDraw As New Drawing.Rectangle(0, 0, Rect.Width, Rect.Height)
        SetGraphToDraw(Graph)
        DrawElementsArea(AreaToDraw, False)
        SetGraphToDraw(OldGraphToDraw)
    End Sub

    Public Sub Reset() Implements IIEC61131LanguageImplementation.Reset
        m_PouInterface.Reset()
    End Sub

    Public Property ResGlobalVariables() As VariablesLists Implements IIEC61131LanguageImplementation.ResGlobalVariables
        Get
            Return m_ResGlobalVariables
        End Get
        Set(ByVal value As VariablesLists)
            m_ResGlobalVariables = value
        End Set
    End Property

    Public Sub ResolveVariableLinks() Implements IIEC61131LanguageImplementation.ResolveVariableLinks
        m_GraphicalVariablesList.ResolveVariableLinks()
        m_GraphicalBlocksList.ResolveVariableLinks()
        m_GraphicalConnectionsList.ResolveVariableLinks()
    End Sub

    Public Sub SetGraphToDraw(ByRef Graph As System.Drawing.Graphics) Implements IIEC61131LanguageImplementation.SetGraphToDraw
        m_GraphToDraw = Graph
        m_GraphicalVariablesList.SetGraphToDraw(Graph)
        m_GraphicalBlocksList.SetGraphToDraw(Graph)
        m_GraphicalConnectionsList.SetGraphToDraw(Graph)
    End Sub

    Public Event StartScan() Implements IIEC61131LanguageImplementation.StartScan

    Public Sub xmlExport(ByRef writer As System.Xml.XmlTextWriter) Implements IXMLExportable.xmlExport
        writer.WriteStartElement("FBD")

        GraphicalVariablesList.xmlExport(writer)
        GraphicalBlocksList.xmlExport(writer)

        ' Non salviamo le connessioni, le possiamo ricreare manualmente :-)

        writer.WriteEndElement() ' FBD
    End Sub

    Private Sub ParseVariable(ByRef reader As System.Xml.XmlTextReader)
        Dim var As New GraphicalVariable(Me)
        var.xmlImport(reader)
        GraphicalVariablesList.Add(var)
    End Sub
    Public Sub ParseBlock(ByRef reader As System.Xml.XmlTextReader)
        Dim blk As New GraphicalFBDBlock(Me)
        blk.xmlImport(reader)
        GraphicalBlocksList.Add(blk)
    End Sub

    Public Sub xmlImport(ByRef reader As System.Xml.XmlTextReader) Implements IXMLImportable.xmlImport
        Dim NodeDepth As Integer = reader.Depth

        ' Lo scheletro di questo codice è tratto da GraphicalStep.vb (nella cartella SFC)
        If Not reader.IsEmptyElement Then
            reader.Read()
            While reader.Depth > NodeDepth
                Select Case reader.Name
                    Case "inVariable"
                        ParseVariable(reader)
                    Case "outVariable"
                        ParseVariable(reader)
                    Case "block"
                        ParseBlock(reader)
                End Select
                If reader.Depth > NodeDepth Then
                    reader.Read()
                End If
            End While
        End If
    End Sub

    Public Function FindElementByLocalId(ByVal id As Integer) As IHasLocalId Implements IIEC61131LanguageImplementation.FindElementByLocalId
        Dim ilid As IHasLocalId = m_GraphicalVariablesList.FindVariableByNumber(id)
        If ilid Is Nothing Then ilid = m_GraphicalBlocksList.FindBlockByNumber(id)
        Return ilid
    End Function

    Public Function FirstAvailableElementNumber() As Integer
        Dim i As Integer = 1
        While FindElementByLocalId(i) IsNot Nothing
            i += 1
        End While
        Return i
    End Function

    Public Sub DrawElementsArea(ByVal Rect As Drawing.Rectangle, ByVal DrawSmallRectangles As Boolean)
        Try
            If Monitor.TryEnter(Me, 2000) Then
                m_GraphicalVariablesList.DrawArea(Rect, DrawSmallRectangles)
                m_GraphicalBlocksList.DrawArea(Rect, DrawSmallRectangles)
                m_GraphicalConnectionsList.DrawArea(Rect, DrawSmallRectangles)
                Monitor.Exit(Me)
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
        End Try
    End Sub

    Public Sub CancelSelection()
        GraphicalVariablesList.CancelSelection(True)
        GraphicalBlocksList.CancelSelection(True)
        GraphicalConnectionsList.CancelSelection(True)
    End Sub
    Public Sub DeSelectAll()
        GraphicalVariablesList.DeselectAll()
        GraphicalBlocksList.DeselectAll()
        GraphicalConnectionsList.DeselectAll()
    End Sub
    Public Sub MoveSelection(ByVal dx As Integer, ByVal dy As Integer)
        GraphicalVariablesList.MoveSelection(dx, dy)
        GraphicalBlocksList.MoveSelection(dx, dy)
        GraphicalConnectionsList.MoveSelection(dx, dy)
    End Sub
    Public Function CountSelected() As Integer
        Return GraphicalVariablesList.CountSelected + _
            GraphicalBlocksList.CountSelected + _
                GraphicalConnectionsList.CountSelected
    End Function
    Public Sub RemoveSelectedElements()
        GraphicalVariablesList.RemoveSelectedElements()
        GraphicalBlocksList.RemoveSelectedElements()
        GraphicalConnectionsList.RemoveSelectedElements()
    End Sub
    Public Sub FindAndSelectElementsArea(ByVal Rect As Drawing.Rectangle)
        GraphicalVariablesList.FindAndSelectVariables(Rect)
        GraphicalBlocksList.FindAndSelectBlocks(Rect)
        GraphicalConnectionsList.FindAndSelectConnections(Rect)
    End Sub
    Public Function FindAndSelectElement(ByVal x As Integer, ByVal y As Integer) As Boolean
        Return GraphicalVariablesList.FindAndSelectVariable(x, y) OrElse _
            GraphicalBlocksList.FindAndSelectBlock(x, y) OrElse _
                GraphicalConnectionsList.FindAndSelectConnection(x, y)
    End Function
    Public Function FindElement(ByVal x As Integer, ByVal y As Integer) As Boolean
        Return GraphicalVariablesList.FindVariable(x, y) OrElse _
            GraphicalBlocksList.FindBlock(x, y) OrElse _
                GraphicalConnectionsList.FindConnection(x, y)
    End Function
    Public Function ReadIfElementIsSelected(ByVal x As Integer, ByVal y As Integer) As Boolean
        Return GraphicalVariablesList.ReadIfVariableSelected(x, y) OrElse _
            GraphicalBlocksList.ReadIfBlockSelected(x, y) OrElse _
                GraphicalConnectionsList.ReadIfConnectionSelected(x, y)
    End Function
    Public Function CheckForSelectionOutside(ByVal R As Drawing.Rectangle, ByRef FuoriX As Boolean, ByRef FuoriY As Boolean) As Boolean
        Return GraphicalBlocksList.CheckForSelectionOutside(R, FuoriX, FuoriY) Or GraphicalVariablesList.CheckForSelectionOutside(R, FuoriX, FuoriY)
    End Function
    Public Function FindAndSelectSmallRectangle(ByVal x As Integer, ByVal y As Integer) As Boolean
        Return GraphicalVariablesList.FindAndSelectSmallRectangleVariable(x, y) OrElse _
            GraphicalBlocksList.FindAndSelectSmallRectangleBlock(x, y)
    End Function

    Public Sub AddAndDrawGraphicalVariable(ByVal bVar As BaseVariable, ByVal vType As GraphicalVariableType, _
            ByVal pos As Drawing.Point, ByVal dimen As Drawing.Size, ByVal execID As Integer)
        Dim obj As New GraphicalVariable(Me, bVar, FirstAvailableElementNumber(), _
             dimen, pos, vType, execID)
        obj.SetGraphToDraw(Me.m_GraphToDraw)
        m_GraphicalVariablesList.Add(obj)
        obj.Draw(False)
    End Sub

    Public Sub AddAndDrawGraphicalBlock(ByVal bType As String, _
        ByVal pos As Drawing.Point, ByVal dimen As Drawing.Size)
        Dim obj As New GraphicalFBDBlock(Me, bType, FirstAvailableElementNumber(), _
            dimen, pos)
        obj.SetGraphToDraw(Me.m_GraphToDraw)
        obj.Name = GraphicalBlocksList.MakeUniqueNameForBlock(bType)
        m_GraphicalBlocksList.Add(obj)
        obj.Draw(False)
    End Sub

    Public Sub AddAndDrawGraphicalConnection(ByVal src As Integer, ByVal dst As Integer, _
        ByVal negated As Boolean, Optional ByVal offset As Integer = 0)
        Dim obj As New GraphicalFBDConnection(src, dst, Me, negated)
        obj.Offset = offset
        obj.SetGraphToDraw(Me.m_GraphToDraw)
        m_GraphicalConnectionsList.Add(obj)
        obj.Draw(False)
    End Sub

    Public Sub AddAndDrawGraphicalConnection(ByVal src As IHasLocalId, ByVal dst As IHasLocalId, _
        ByVal negated As Boolean)
        Me.AddAndDrawGraphicalConnection(src.Number, dst.Number, negated)
    End Sub

    ' Monitoriamo sempre e comunque, ma l'editor vuole questi due metodi e quindi glieli diamo
    Public Sub StartStateMonitor()
        Nop()
    End Sub
    Public Sub StopStateMonitor()
        Nop()
    End Sub

    Public Function FindObjectsWithInRectangleSelected() As List(Of IFBDConnectable)
        Dim ret As New List(Of IFBDConnectable)
        For Each GV As GraphicalVariable In GraphicalVariablesList
            If GV.VariableType = GraphicalVariableType.Output Then _
                If GV.RectangleSelected Then ret.Add(GV)
        Next
        For Each GB As GraphicalFBDBlock In GraphicalBlocksList
            If GB.InRectangleSelected Then ret.Add(GB)
        Next
        Return ret
    End Function

    Public Function FindObjectWithInRectangleSelected() As IFBDConnectable
        For Each GV As GraphicalVariable In GraphicalVariablesList
            If GV.VariableType = GraphicalVariableType.Output Then
                If GV.RectangleSelected Then
                    Return GV
                End If
            End If
        Next
        For Each GB As GraphicalFBDBlock In GraphicalBlocksList
            If GB.InRectangleSelected Then Return GB
        Next
        Return Nothing
    End Function

    Public Function FindObjectsWithOutRectangleSelected() As List(Of IFBDConnectable)
        Dim ret As New List(Of IFBDConnectable)
        For Each GV As GraphicalVariable In GraphicalVariablesList
            If GV.VariableType = GraphicalVariableType.Input Then _
                If GV.RectangleSelected Then ret.Add(GV)
        Next
        For Each GB As GraphicalFBDBlock In GraphicalBlocksList
            If GB.OutRectangleSelected Then ret.Add(GB)
        Next
        Return ret
    End Function

    Public Function FindObjectWithOutRectangleSelected() As IFBDConnectable
        For Each GV As GraphicalVariable In GraphicalVariablesList
            If GV.VariableType = GraphicalVariableType.Input Then
                If GV.RectangleSelected Then
                    Return GV
                End If
            End If
        Next
        For Each GB As GraphicalFBDBlock In GraphicalBlocksList
            If GB.OutRectangleSelected Then Return GB
        Next
        Return Nothing
    End Function

    Public Function ReadObjectSelected() As Object
        ReadObjectSelected = GraphicalBlocksList.ReadBlockSelected
        If ReadObjectSelected IsNot Nothing Then Exit Function
        ReadObjectSelected = GraphicalVariablesList.ReadVariableSelected
        If ReadObjectSelected IsNot Nothing Then Exit Function
        ReadObjectSelected = GraphicalConnectionsList.ReadConnectionSelected

    End Function

    ' Ritorna l'oggetto più a destra del diagramma
    Public Function GetRightmostObject() As IGraphicalObject
        Dim mostX As Integer = -1
        Dim mostXObject As IGraphicalObject = Nothing
        For Each GO As IGraphicalObject In GraphicalVariablesList
            If GO.Position.X > mostX Then
                mostX = GO.Position.X
                mostXObject = GO
            End If
        Next
        For Each GO As IGraphicalObject In GraphicalBlocksList
            If GO.Position.X > mostX Then
                mostX = GO.Position.X
                mostXObject = GO
            End If
        Next
        Return mostXObject
    End Function

    ' Ritorna l'oggetto più in basso del diagramma
    Public Function GetLowermostObject() As IGraphicalObject
        Dim mostY As Integer = -1
        Dim mostYObject As IGraphicalObject = Nothing
        For Each GO As IGraphicalObject In GraphicalVariablesList
            If GO.Position.Y > mostY Then
                mostY = GO.Position.Y
                mostYObject = GO
            End If
        Next
        For Each GO As IGraphicalObject In GraphicalBlocksList
            If GO.Position.Y > mostY Then
                mostY = GO.Position.Y
                mostYObject = GO
            End If
        Next
        Return mostYObject
    End Function

End Class