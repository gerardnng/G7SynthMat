Imports System
Imports System.Collections.Generic
Imports UnisimClassLibrary

Public Class Rung

    Private m_Expression As String
    Private m_Coils As GraphicalContactList
    Private m_FBDNets As List(Of LDTree)
    Private m_WorkingStorageSet As Dictionary(Of BaseVariable, Object)

    Public Sub New()
        m_Expression = "true"
        m_Coils = New GraphicalContactList
        m_FBDNets = New List(Of LDTree)
        m_WorkingStorageSet = New Dictionary(Of BaseVariable, Object)
    End Sub

    Private Sub Copy(ByVal other As Rung)
        Me.m_Expression = other.m_Expression
        Me.m_Coils = other.m_Coils
        Me.m_FBDNets = other.m_FBDNets
        Me.m_WorkingStorageSet = other.m_WorkingStorageSet
    End Sub

    Public Sub New(ByVal root As GraphicalRightRail)
        Dim other As Rung = root.Container.GraphicalConnectionList.BuildExpressionRtL(root)
        Me.Copy(other)
    End Sub

    Public Sub New(ByVal root As GraphicalLeftRail)
        Dim other As Rung = root.Container.GraphicalConnectionList.BuildExpressionLtR(root)
        Me.Copy(other)
    End Sub

    Public ReadOnly Property CoilsCount() As Integer
        Get
            Return m_Coils.Count
        End Get
    End Property

    Public ReadOnly Property FBDNetsCount() As Integer
        Get
            Return m_FBDNets.Count
        End Get
    End Property

    Public Property Expression() As String
        Get
            Return m_Expression
        End Get
        Set(ByVal value As String)
            m_Expression = value
        End Set
    End Property

    Public Sub AddCoil(ByVal c As BaseGraphicalContact)
        If Not (c.IsCoil) Then Throw New ArgumentException("only coils can be added to the right side of a rung")
        If Not (m_Coils.Contains(c)) Then m_Coils.Add(c)
    End Sub
    Public Sub AddFBDTree(ByVal t As LDTree)
        If Not (m_FBDNets.Contains(t)) Then m_FBDNets.Add(t)
    End Sub

    Public Overrides Function ToString() As String
        Dim ret As String = "{"
        For Each c As BaseGraphicalContact In m_Coils
            ret += c.Qualy + "(" + c.ReadVar().Name + "),"
        Next
        ret = ret.TrimEnd(",") + "}"
        ret += " := " + Expression
        Return ret
    End Function

    Public Function ExecuteScanCycle(ByVal ld As Ladder) As Boolean
        Dim newValue As Boolean = Calculate(ld)
        Dim anyChange As Boolean = False
        For Each c As GraphicalContact In m_Coils
            anyChange = anyChange Or c.SetCoilValue(Me, newValue)
        Next
        ' se il rung ha continuità elettrica...
        If newValue Then
            ' ...esegui le parti FBD associate
            For Each t As LDTree In m_FBDNets
                anyChange = anyChange Or t.Execute()
            Next
        End If
        Return anyChange
    End Function

    Public Sub SetValue(ByVal b As BaseVariable, ByVal v As Object)
        m_WorkingStorageSet(b) = v
    End Sub

    Public Sub CommitChanges()
        For Each kvp As KeyValuePair(Of BaseVariable, Object) In Me.m_WorkingStorageSet
            kvp.Key.SetValue(kvp.Value)
        Next
    End Sub

    Public Function Calculate(ByVal ld As Ladder) As Boolean
        Dim boolExpr As New BooleanExpression(ld)
        boolExpr.SetExpression(Expression)
        Return boolExpr.Evaluate()
    End Function

End Class
