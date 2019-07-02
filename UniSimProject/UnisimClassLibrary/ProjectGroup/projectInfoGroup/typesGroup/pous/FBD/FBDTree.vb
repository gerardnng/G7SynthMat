Imports System
Imports System.Collections.Generic

Public Class FBDTree
    Private m_Target As BaseVariable
    Private m_RootNode As FBDTreeNode

    Public Sub New(ByVal tg As BaseVariable, Optional ByVal rn As FBDTreeNode = Nothing)
        m_Target = tg
        SetRootNode(rn)
    End Sub

    Public Sub SetRootNode(ByVal rn As FBDTreeNode)
        m_RootNode = rn
    End Sub

    Public Function GetRootNode() As FBDTreeNode
        Return m_RootNode
    End Function

    Public Sub Execute()
        m_Target.SetActValue(m_RootNode.GetNodeValue())
    End Sub

End Class