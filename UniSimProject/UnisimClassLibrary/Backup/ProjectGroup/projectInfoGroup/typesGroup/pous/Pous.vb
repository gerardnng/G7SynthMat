Imports System.Xml
Imports System.Threading
Imports System
Imports System.Collections.Generic
Public Class Pous
    Inherits List(Of pou)
    Protected m_ResourceGlobalVariables As VariablesLists
    Property ResourceGlobalVariables() As VariablesLists
        Get
            ResourceGlobalVariables = m_ResourceGlobalVariables
        End Get
        Set(ByVal Value As VariablesLists)
            m_ResourceGlobalVariables = Value
            'Comunica ai pou le lista di variabili globali della risorsa
            For Each pouTemp As pou In Me
                pouTemp.ResGlobalVariables = m_ResourceGlobalVariables
            Next pouTemp
        End Set
    End Property
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub xmlExport(ByRef RefXMLProjectWriter As XmlTextWriter)
        'pous
        RefXMLProjectWriter.WriteStartElement("pous")
        For Each PouTemp As pou In Me
            'pou
            PouTemp.xmlExport(RefXMLProjectWriter)
        Next PouTemp
        RefXMLProjectWriter.WriteEndElement() 'pous

    End Sub
    Public Sub xmlImport(ByRef RefXmlProjectReader As XmlTextReader)
        'Memorizza la profondità del nodo
        Dim NodeDepth As Integer = RefXmlProjectReader.Depth
        'Si sposta sul nodo successivo
        RefXmlProjectReader.Read()

        'Scorre fino alla fine di pous
        While RefXmlProjectReader.Depth > NodeDepth
            Select Case RefXmlProjectReader.Name
                Case "pou"
                    'Controlla se è l'inizio dell'elemento
                    If RefXmlProjectReader.NodeType = XmlNodeType.Element Then
                        'Crea il POU
                        Dim NewPou As New pou(m_ResourceGlobalVariables, Me)
                        'Importa i dati del pou
                        NewPou.xmlImport(RefXmlProjectReader)
                        'Aggiunge il pou alla lista
                        Me.Add(NewPou)
                    End If
            End Select
            'Si sposta sul nodo successivo se non è la fine dell'elemento
            If RefXmlProjectReader.Depth > NodeDepth Then
                RefXmlProjectReader.Read()
            End If

        End While
    End Sub
    Public Function AddPou(ByVal PouName As String, ByVal PouDocumentation As String, ByVal PoupouType As EnumPouType, ByVal BodyType As EnumBodyType) As pou
        AddPou = Nothing
        Try
            'Monitor sulla lista dei pou
            Monitor.TryEnter(Me, 2000)
            Dim Newpou As New pou(PouName, PouDocumentation, PoupouType, _
                m_ResourceGlobalVariables, BodyType, Me)
            MyBase.Add(Newpou)
            Monitor.Exit(Me)
            AddPou = Newpou
        Catch ex As System.Exception
            Monitor.Exit(Me)
        End Try
    End Function
    Public Function FindpouByName(ByVal mName As String) As pou
        'Verifica se il POU con il nome specificato esiste
        For Each pouTemp As pou In Me
            If pouTemp.Name = mName Then
                Return pouTemp
            End If
        Next pouTemp
        Return Nothing
    End Function
    Public Sub RemovePou(ByRef RefPou As pou)
        Try
            'Monitor sulla lista dei pou
            If Monitor.TryEnter(Me, 2000) Then
                For Each pouTemp As pou In Me
                    If pouTemp Is RefPou Then
                        'Rimuove il POU dalla lista
                        MyBase.Remove(pouTemp)
                        'Distrugge il POU
                        pouTemp.DisposeMe()
                        Exit For
                    End If
                Next pouTemp
                Monitor.Exit(Me)
            End If
        Catch ex As System.Exception
            Monitor.Exit(Me)
        End Try
    End Sub
    Public Function ReadResGlobalVariables() As VariablesLists
        ReadResGlobalVariables = m_ResourceGlobalVariables
    End Function
    Public Sub ResolveVariablesLinks()
        'Risolve i riferimenti dei nomi di variabili nella azioni e nelle condizioni
        For Each PouTemp As pou In Me
            'pou
            PouTemp.ResolveVariablesLinks()
        Next PouTemp
    End Sub
    Public Sub DisposeMe()
        For Each P As pou In Me
            P.DisposeMe()
        Next P
    End Sub
End Class

