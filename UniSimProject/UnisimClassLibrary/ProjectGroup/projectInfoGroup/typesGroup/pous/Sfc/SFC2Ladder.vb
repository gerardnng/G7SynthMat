' Questo modulo esporta le funzioni di conversione del Sequential Functional Chart verso il Ladder
' Alcuni ricercatori hanno sviluppato tecniche per effettuare la traduzione inversa usando sistemi
' di equazioni ma ad oggi questo non riguarda noi. Una vecchia versione includeva il supporto per
' impostare una POU Ladder equivalente ad una SFC data, ma poi si è deciso di effettuare la
' conversione completa
' Ciò che non supportiamo:
' * macrofasi e macroazioni
' * azioni ritardate ed eritmetiche
' * condizioni aritmetiche e temporali nelle transizioni
' Non ci sono stringenti vincoli temporali e di ottimizzazione: questo codice sarà eseguito sporadicamente
' e quindi, se anche dovesse richiedere 1 minuto di computazione per una POU di media complessità, non
' sarebbe un problema

Module SFC2Ladder

    Class RailsObject
        Public leftRail As GraphicalLeftRail
        Public rightRail As GraphicalRightRail
        Public Sub New(ByVal l As GraphicalLeftRail, ByVal r As GraphicalRightRail)
            leftRail = l
            rightRail = r
        End Sub
    End Class

    Class SFC2LDConversionData
        Public source As Sfc
        Public target As Ladder
        Private m_RailsCounter As Integer
        Shared ReadOnly P As New Drawing.Point(100, 100)
        Public Sub New(ByVal S As Sfc, ByVal L As Ladder)
            source = S
            target = L
            m_RailsCounter = 0
        End Sub
        Public Function AddRails() As RailsObject
            Dim P1 As New Drawing.Point(10, 48 + m_RailsCounter)
            Dim leftid As Integer = target.FirstAvailableElementNumber
            target.AddAndDrawLeftRail(leftid, "", "", P1, 20, "", _
                "", Nothing, Nothing, Nothing)
            Dim P2 As New Drawing.Point(780, 48 + m_RailsCounter)
            Dim rightid As Integer = target.FirstAvailableElementNumber
            target.AddAndDrawRightRail(rightid, "", "", P2, 20, "", "", _
                Nothing, Nothing, Nothing)
            m_RailsCounter += 60
            Return New RailsObject(target.FindElementByLocalId(leftid), target.FindElementByLocalId(rightid))
        End Function
        Public Function AddContact(ByVal qualy As String, ByVal boundVar As BaseVariable) As GraphicalContact
            Return AddContact(qualy, boundVar, SFC2LDConversionData.P)
        End Function
        Public Function AddContact(ByVal qualy As String, ByVal boundVar As BaseVariable, _
            ByVal P As Drawing.Point) As GraphicalContact
            Dim id As Integer = target.FirstAvailableElementNumber
            target.AddAndDrawContact(id, _
                target.FirstAvailableContactName(), "", P, 1, boundVar.Name, qualy, boundVar, Nothing, Nothing)
            Return CType(target.FindElementByLocalId(id), GraphicalContact)
        End Function
        Public Function AddCoil(ByVal qualy As String, ByVal boundVar As BaseVariable) As GraphicalContact
            Return AddCoil(qualy, boundVar, SFC2LDConversionData.P)
        End Function
        Public Function AddCoil(ByVal qualy As String, ByVal boundVar As BaseVariable, _
            ByVal P As Drawing.Point) As GraphicalContact
            Dim id As Integer = target.FirstAvailableElementNumber
            target.AddAndDrawContact(id, _
                target.FirstAvailableContactName(), "", P, 2, boundVar.Name, qualy, boundVar, Nothing, Nothing)
            Return CType(target.FindElementByLocalId(id), GraphicalContact)
        End Function
        ' Late binding a go-go...ma per lo meno questo blocco di codice non è critico e fa parte
        ' di un modulo molto specifico
        Public Sub ConnectEntries(ByVal srcEntry As Object, ByVal dstEntry As Object)
            srcEntry.SetBottomRectSelected(True)
            dstEntry.SetTopRectSelected(True)
            ConnectSelectedEntries()
            srcEntry.SetBottomRectSelected(False)
            dstEntry.SetTopRectSelected(False)
        End Sub
        Public Sub ConnectEntriesLists(ByVal srcEntries As GraphicalContactList, _
            ByVal dstEntries As GraphicalContactList)
            For Each s As Object In srcEntries
                For Each d As Object In dstEntries
                    ConnectEntries(s, d)
                Next
            Next
        End Sub
        Public Sub ConnectSelectedEntries()
            target.AddAndDrawConnection(target.FirstAvailableElementNumber, "", "", Nothing)
        End Sub
        Public Function GetStepMarkerVariable(ByVal S0 As BaseGraphicalStep) As BaseVariable
            Return target.PouInterface.localVars.FindVariableByName(S0.Name + "X")
        End Function
    End Class

    ' Traduciamo le variabili
    ' 1) Ricopiamo le variabili locali dell'SFC
    ' 2) Creiamo le variabili marker di fase
    Private Sub TranslateVariables(ByVal params As SFC2LDConversionData)
        For Each V As BaseVariable In params.source.PouInterface.localVars
            params.target.PouInterface.localVars.CreateAndAddVariable(V.Name, V.Documentation, V.Address, _
            V.ReadInitialValue(), V.dataType)
        Next
        For Each S As BaseGraphicalStep In params.source.ReadStepList()
            params.target.PouInterface.localVars.CreateAndAddVariable(S.Name + "X", _
                "Marker for step " + S.Name, "", "false", "BOOL")
        Next
    End Sub

    ' Crea la fase di inizializzazione
    ' 1) Prepara i rung del tipo
    ' |---|first_scan|----(set S.X)---|
    Private Sub PrepareFirstScan(ByVal params As SFC2LDConversionData)
        Dim initial_steps As New GraphicalStepsList
        Dim ld As Ladder = params.target
        Dim first_scan As BaseVariable = ld.PouInterface.FindVariableByName("first_scan")
        For Each S0 As BaseGraphicalStep In params.source.ReadStepList()
            If Not (TypeOf (S0) Is GraphicalStep) Then Continue For
            Dim S As GraphicalStep = DirectCast(S0, GraphicalStep)
            If Not (S.Initial) Then Continue For
            Dim fs As GraphicalContact = _
                params.AddContact("Normal Open Contact", first_scan)
            Dim sx As GraphicalContact = _
                params.AddCoil("Set Coil", params.GetStepMarkerVariable(S))
            Dim rails As RailsObject = params.AddRails()
            Dim railpos As Drawing.Point = rails.leftRail.ReadPosition
            fs.SetPosition(New Drawing.Point(200, railpos.Y))
            sx.SetPosition(New Drawing.Point(600, railpos.Y))
            params.ConnectEntries(rails.leftRail, fs)
            params.ConnectEntries(fs, sx)
            params.ConnectEntries(sx, rails.rightRail)
        Next
    End Sub

    ' Prepariamo le azioni impulsive associate a ciascuna fase
    ' 1) Crea il contatto a fronte di salita per l'azione
    ' 2) Ignora le azioni non gestite o le continue
    ' 3) Crea il contatto che esegue l'azione
    ' 4) Collega il rung
    Private Sub TranslatePulseActions(ByVal params As SFC2LDConversionData)
        Dim ld As Ladder = params.target
        For Each S0 As BaseGraphicalStep In params.source.ReadStepList()
            If Not (TypeOf (S0) Is GraphicalStep) Then Continue For
            Dim S As GraphicalStep = CType(S0, GraphicalStep)
            Dim marker As BaseVariable = params.GetStepMarkerVariable(S)
            For Each A0 As Object In S.ReadListActions()
                If Not (TypeOf (A0) Is GraphicalAction) Then Continue For
                Dim A As GraphicalAction = DirectCast(A0, GraphicalAction)
                If A.Qualifier <> "S" AndAlso A.Qualifier <> "R" Then Continue For
                Dim actor As GraphicalContact = Nothing
                Dim targetVar As BaseVariable = ld.PouInterface.FindVariableByName(A.Variable.Name)
                If targetVar Is Nothing Then targetVar = ld.ResGlobalVariables.FindVariableByName(A.Variable.Name)
                If A.Qualifier = "S" Then actor = _
                    params.AddCoil("Set Coil", targetVar)
                If A.Qualifier = "R" Then actor = _
                    params.AddCoil("Reset Coil", targetVar)
                Dim cond As GraphicalContact = _
                    params.AddContact("Positive Transition-sensing Contact", marker)
                Dim rails As RailsObject = params.AddRails()
                Dim railpos As Drawing.Point = rails.leftRail.ReadPosition
                cond.SetPosition(New Drawing.Point(200, railpos.Y))
                actor.SetPosition(New Drawing.Point(600, railpos.Y))
                params.ConnectEntries(rails.leftRail, cond)
                params.ConnectEntries(cond, actor)
                params.ConnectEntries(actor, rails.rightRail)
            Next
        Next
    End Sub

    ' Prepariamo le azioni impulsive associate a ciascuna fase
    ' 1) Crea il contatto per l'azione
    ' 2) Ignora le azioni non gestite o le impulsive
    ' 3) Crea il contatto che esegue l'azione
    ' 4) Collega il rung
    Private Sub TranslateContinuousActions(ByVal params As SFC2LDConversionData)
        Dim ld As Ladder = params.target
        For Each S0 As BaseGraphicalStep In params.source.ReadStepList()
            If Not (TypeOf (S0) Is GraphicalStep) Then Continue For
            Dim S As GraphicalStep = CType(S0, GraphicalStep)
            Dim marker As BaseVariable = params.GetStepMarkerVariable(S)
            For Each A0 As Object In S.ReadListActions()
                If Not (TypeOf (A0) Is GraphicalAction) Then Continue For
                Dim A As GraphicalAction = DirectCast(A0, GraphicalAction)
                If A.Qualifier <> "N" Then Continue For
                Dim actor As GraphicalContact = Nothing
                Dim targetVar As BaseVariable = ld.PouInterface.FindVariableByName(A.Variable.Name)
                If targetVar Is Nothing Then targetVar = ld.ResGlobalVariables.FindVariableByName(A.Variable.Name)
                If A.Qualifier = "N" Then actor = _
                    params.AddCoil("Normal Coil", targetVar)
                Dim cond As GraphicalContact = _
                    params.AddContact("Normal Open Contact", marker)
                Dim rails As RailsObject = params.AddRails()
                Dim railpos As Drawing.Point = rails.leftRail.ReadPosition
                cond.SetPosition(New Drawing.Point(200, railpos.Y))
                actor.SetPosition(New Drawing.Point(600, railpos.Y))
                params.ConnectEntries(rails.leftRail, cond)
                params.ConnectEntries(cond, actor)
                params.ConnectEntries(actor, rails.rightRail)
            Next
        Next
    End Sub

    ' Questo algoritmo è (RtL)^-1, ovvero l'inverso semantico dell'RtL
    ' Ovviamente, ogni espressione booleana è convertibile in tanti modi
    ' diversi in LD, quindi non c'è da aspettarsi una rispondenza grafica assoluta
    Private Function TranslateTransitionPiece(ByVal params As SFC2LDConversionData, _
        ByVal T As GraphicalTransition, ByVal condPiece As BoolExpressionNode, _
        ByVal hookingPoint As GraphicalContactList) As GraphicalContactList

        If condPiece Is Nothing Then Return Nothing

        Dim condPieceObj As Object = CObj(condPiece)

        Select Case condPiece.GetType().Name
            Case "PlusNode"
                If condPiece.Neg Then
                    ' !(A+B)=!A*!B
                    Dim newNode As New MultNode()
                    newNode.Neg = False
                    newNode.NextNodes = condPieceObj.NextNodes
                    For Each node As BoolExpressionNode In newNode.NextNodes
                        node.Neg = Not (node.Neg)
                    Next
                    TranslateTransitionPiece = TranslateTransitionPiece(params, T, newNode, hookingPoint)
                    ' rimetti i nodi a posto
                    For Each node As BoolExpressionNode In newNode.NextNodes
                        node.Neg = Not (node.Neg)
                    Next
                    Exit Function
                End If
                Dim newHookingPoint As New GraphicalContactList()
                For Each node As BoolExpressionNode In condPieceObj.NextNodes
                    Dim rails As RailsObject = params.AddRails()
                    Dim formalHP As New GraphicalContactList(rails.leftRail)
                    newHookingPoint.AddRange( _
                        TranslateTransitionPiece(params, T, node, _
                        IIf(Of GraphicalContactList)(condPieceObj.NextNodes.Count = 1, _
                        hookingPoint, formalHP)))
                Next
                Return newHookingPoint
            Case "MultNode"
                If condPiece.Neg Then
                    ' !(A*B)=!A+!B
                    Dim newNode As New PlusNode()
                    newNode.Neg = False
                    newNode.NextNodes = condPieceObj.NextNodes
                    For Each node As BoolExpressionNode In newNode.NextNodes
                        node.Neg = Not (node.Neg)
                    Next
                    TranslateTransitionPiece = TranslateTransitionPiece(params, T, newNode, hookingPoint)
                    ' rimetti i nodi a posto
                    For Each node As BoolExpressionNode In newNode.NextNodes
                        node.Neg = Not (node.Neg)
                    Next
                    Exit Function
                End If
                Dim newHP As GraphicalContactList = hookingPoint
                For Each node As BoolExpressionNode In condPieceObj.NextNodes
                    newHP = _
                        SFC2Ladder.TranslateTransitionPiece(params, T, _
                            node, newHP)
                Next
                Return newHP
            Case "CompareNode"
                Exit Select
            Case "VariableNode"
                Dim varCopy As BaseVariable = params.target.PouInterface.localVars.FindVariableByName(condPieceObj.Var.Name)
                If varCopy Is Nothing Then varCopy = params.target.ResGlobalVariables.FindVariableByName(condPieceObj.Var.Name)
                Dim varNode As GraphicalContact = params.AddContact( _
                    IIf(Of String)(condPiece.Neg, "Normal Closed Contact", "Normal Open Contact"), _
                        varCopy)
                Dim hpPosition As Drawing.Point = hookingPoint(0).ReadPosition
                hpPosition.X += 2 * varNode.Size.Width
                varNode.SetPosition(hpPosition)
                Dim theList As New GraphicalContactList(varNode)
                params.ConnectEntriesLists(hookingPoint, theList)
                Return theList
            Case "StepMakerConditionNode"
                If condPieceObj.Type Then Exit Select
                Dim theStep As BaseGraphicalStep = condPieceObj.StepMaker
                Dim boundVar As BaseVariable = params.GetStepMarkerVariable(theStep)
                Dim varNode As GraphicalContact = params.AddContact( _
                    IIf(Of String)(condPiece.Neg, "Normal Closed Contact", "Normal Open Contact"), _
                        boundVar)
                Dim theList As New GraphicalContactList(varNode)
                params.ConnectEntriesLists(hookingPoint, theList)
                Return theList
        End Select
        Return Nothing
    End Function


    ' Spezza la transizione in più rungs
    ' 1) |----|prev_step0|----|prev_step1|----..---|prev_stepn|---(TPRECOND)---|
    ' 2) |----<valutazione della condizione di transizione>---(TCOND)----|
    ' 2.2 |----|TPRECOND|----|TCOND|---------------------(TENABLE)------|
    ' 3.1) |----|TENABLE|---------------------------(R:prev_step0)------|
    ' ...
    ' 3.n) |----|TENABLE|---------------------------(R:prev_stepn)------|
    ' 4.1) |----|TENABLE|---------------------------(S:next_step0)------|
    ' ...
    ' 4.m) |----|TENABLE|---------------------------(S:next_stepm)------|
    Private Sub TranslateTransition(ByVal params As SFC2LDConversionData, ByVal T As GraphicalTransition)
        Dim ld As Ladder = params.target
        Dim prevSteps As GraphicalStepsList = T.ReadPreviousGraphicalStepsList()
        Dim precond As BaseVariable = params.target.PouInterface.localVars.CreateAndAddVariable( _
            params.target.PouInterface.localVars.MakeUniqueName("TPRECOND"), "", "", _
                "false", "BOOL")
        Dim rails As RailsObject = params.AddRails()
        Dim pos As Drawing.Point = rails.leftRail.ReadPosition()
        Dim prevOne As BaseGraphicalContact = rails.leftRail
        Dim contact As GraphicalContact
        pos.X = 200
        For Each S0 As BaseGraphicalStep In prevSteps
            If Not (TypeOf (S0) Is GraphicalStep) Then Continue For
            contact = params.AddContact("Normal Open Contact", _
                params.GetStepMarkerVariable(S0))
            contact.SetPosition(pos)
            params.ConnectEntries(prevOne, contact)
            prevOne = contact
            pos.X += 60
        Next
        Dim coil As GraphicalContact = params.AddCoil("Normal Coil", precond)
        coil.SetPosition(New Drawing.Point(Math.Max(600, pos.X), pos.Y))
        params.ConnectEntries(prevOne, coil)
        params.ConnectEntries(coil, rails.rightRail)
        ' Inizio codice temporaneo
        rails = params.AddRails()
        pos = rails.leftRail.ReadPosition()
        pos.X = 200
        Dim condVar As BaseVariable = params.target.PouInterface.localVars.CreateAndAddVariable( _
            params.target.PouInterface.localVars.MakeUniqueName("TCOND"), "", "", _
                "false", "BOOL")
        coil = params.AddCoil("Normal Coil", condVar)
        Dim transen As BaseVariable = params.target.PouInterface.localVars.CreateAndAddVariable( _
            params.target.PouInterface.localVars.MakeUniqueName("TEN"), "", "", _
                "false", "BOOL")
        Dim contact2 As GraphicalContactList = TranslateTransitionPiece(params, T, _
            T.ReadCondition().RootNode, New GraphicalContactList(rails.leftRail))
        pos.X = 400
        pos.X = 600
        coil.SetPosition(pos)
        If contact2 IsNot Nothing Then
            params.ConnectEntriesLists(New GraphicalContactList(rails.leftRail), contact2)
            params.ConnectEntriesLists(contact2, New GraphicalContactList(coil))
        Else
            params.ConnectEntries(rails.leftRail, coil)
        End If
        params.ConnectEntries(coil, rails.rightRail)
        rails = params.AddRails()
        pos = rails.leftRail.ReadPosition
        contact = params.AddContact("Normal Open Contact", precond)
        pos.X = 200
        contact.SetPosition(pos)
        params.ConnectEntries(rails.leftRail, contact)
        Dim contactC As GraphicalContact = params.AddContact("Normal Open Contact", condVar)
        pos.X = 400
        contactC.SetPosition(pos)
        params.ConnectEntries(contact, contactC)
        coil = params.AddCoil("Normal Coil", transen)
        pos.X = 600
        coil.SetPosition(pos)
        params.ConnectEntries(contactC, coil)
        params.ConnectEntries(coil, rails.rightRail)
        ' Fine codice temporaneo: in TEN c'è true se dobbiamo eseguire la transizione
        Dim nextSteps As GraphicalStepsList = T.ReadNextsGraphicalStepsList
        contact = params.AddContact("Normal Open Contact", transen)
        rails = params.AddRails()
        pos = rails.leftRail.ReadPosition
        pos.X = 200
        contact.SetPosition(pos)
        params.ConnectEntries(rails.leftRail, contact)
        For Each S0 As BaseGraphicalStep In prevSteps
            If Not (TypeOf (S0) Is GraphicalStep) Then Continue For
            coil = params.AddCoil("Reset Coil", params.GetStepMarkerVariable(S0))
            rails = params.AddRails()
            pos = rails.leftRail.ReadPosition
            pos.X = 600
            coil.SetPosition(pos)
            params.ConnectEntries(contact, coil)
            params.ConnectEntries(coil, rails.rightRail)
        Next
        For Each S0 As BaseGraphicalStep In nextSteps
            If Not (TypeOf (S0) Is GraphicalStep) Then Continue For
            coil = params.AddCoil("Set Coil", params.GetStepMarkerVariable(S0))
            rails = params.AddRails()
            pos = rails.leftRail.ReadPosition
            pos.X = 600
            coil.SetPosition(pos)
            params.ConnectEntries(contact, coil)
            params.ConnectEntries(coil, rails.rightRail)
        Next
    End Sub

    Private Sub TranslateTransitions(ByVal params As SFC2LDConversionData)
        Dim ld As Ladder = params.target
        For Each T As GraphicalTransition In params.source.GraphicalTransitionsList
            TranslateTransition(params, T)
        Next
    End Sub

    Public Sub Translate(ByVal source As Sfc, ByVal target As Ladder)
        Dim convParams As New SFC2LDConversionData(source, target)
        TranslateVariables(convParams)
        convParams.AddRails() ' separa le diverse porzioni logiche del diagramma
        PrepareFirstScan(convParams)
        convParams.AddRails() ' separa le diverse porzioni logiche del diagramma
        TranslatePulseActions(convParams)
        convParams.AddRails() ' separa le diverse porzioni logiche del diagramma
        TranslateContinuousActions(convParams)
        convParams.AddRails() ' separa le diverse porzioni logiche del diagramma
        TranslateTransitions(convParams)
    End Sub

End Module
