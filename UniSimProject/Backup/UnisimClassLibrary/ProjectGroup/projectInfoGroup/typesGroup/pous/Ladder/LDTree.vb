Imports System
Imports System.Collections.Generic

Public Class LDTree
    Private m_Target As BaseVariable
    Private m_RootNode As LDTreeNode

    Public Sub New(ByVal tg As BaseVariable, Optional ByVal rn As LDTreeNode = Nothing)
        m_Target = tg
        SetRootNode(rn)
    End Sub

    Public Sub SetRootNode(ByVal rn As LDTreeNode)
        m_RootNode = rn
    End Sub

    Public Function GetRootNode() As LDTreeNode
        Return m_RootNode
    End Function

    Public Function Execute() As Object
        Dim oldValue As Object = m_Target.ReadActValue()
        m_Target.SetActValue(m_RootNode.GetNodeValue())
        Return oldValue <> m_Target.ReadActValue()
    End Function

End Class