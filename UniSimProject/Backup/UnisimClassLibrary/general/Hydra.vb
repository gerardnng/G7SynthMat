Imports System
Imports System.Collections
Imports System.Collections.Generic

' Idra è una figura mitologica con nove teste di serpente
' Questa classe è un doppio dizionario, o dizionario indiretto o dizionario parametrico
' che dir si voglia. In sostanza, la sua utilità è poter contenere diversi dizionari
' al suo interno, e poter esporre l'uno o l'altro verso l'utilizzatore in base ad
' un parametro. In tal modo, si rende più facile modificare le configurazioni in base
' alle opzioni utente (o voci di menu in base a flag booleani)
Public Class Hydra(Of ParamType, KeyType, ValueType)
    ' implementare un IDictionary generico è un lavoro gravoso che richiede di
    ' supportare anche metodi non parametrici non di interesse, e non se ne
    ' vedono i vantaggi per l'uso che ci si aspetta di questa classe
    'Implements IDictionary(Of ParamType, Dictionary(Of KeyType, ValueType))

    Private m_Dictionaries As Dictionary(Of ParamType, _
        Dictionary(Of KeyType, ValueType))

    Public Sub New()
        m_Dictionaries = New Dictionary(Of ParamType, Dictionary(Of KeyType, ValueType))
    End Sub

    Default Public Property Item(ByVal key As ParamType) As Dictionary(Of KeyType, ValueType)
        Get
            If Not (m_Dictionaries.ContainsKey(key)) Then _
                m_Dictionaries.Add(key, New Dictionary(Of KeyType, ValueType))
            Return m_Dictionaries(key)
        End Get
        Set(ByVal value As Dictionary(Of KeyType, ValueType))
            m_Dictionaries(key) = value
        End Set
    End Property

    Public Sub Clear()
        m_Dictionaries.Clear()
    End Sub

    Public ReadOnly Property Count() As Integer
        Get
            Return m_Dictionaries.Count
        End Get
    End Property

    Public Function ContainsKey(ByVal key As ParamType) As Boolean
        Return m_Dictionaries.ContainsKey(key)
    End Function

    Public ReadOnly Property Keys() As System.Collections.Generic.ICollection(Of ParamType)
        Get
            Return m_Dictionaries.Keys
        End Get
    End Property

    Public Function Remove(ByVal key As ParamType) As Boolean
        Return m_Dictionaries.Remove(key)
    End Function

    Public ReadOnly Property Values() As System.Collections.Generic.ICollection(Of System.Collections.Generic.Dictionary(Of KeyType, ValueType))
        Get
            Return m_Dictionaries.Values
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of System.Collections.Generic.KeyValuePair(Of ParamType, System.Collections.Generic.Dictionary(Of KeyType, ValueType)))
        Return m_Dictionaries.GetEnumerator()
    End Function

    Public Function AddValueToAllEntries(ByVal valueName As KeyType, ByVal ParamArray values() As KeyValuePair(Of ParamType, ValueType)) _
        As Hydra(Of ParamType, KeyType, ValueType)
        For Each valObject As KeyValuePair(Of ParamType, ValueType) In values
            Me(valObject.Key)(valueName) = valObject.Value
        Next
        ' Usiamo la stessa tecnica degli oggetti ostream del C++, così possiamo fare
        ' una cosa tipo hydra.AddValue(...).AddValue(...) in una sola riga di codice
        Return Me
    End Function

End Class
