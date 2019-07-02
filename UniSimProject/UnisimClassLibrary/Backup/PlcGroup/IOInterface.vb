Public Class IOInterface
    'Riferimento al driver
    Protected m_IODriver As BaseIODriver
    'Array di bit per la lettura degli inputs
    Protected m_DigitalDataInputs() As Boolean
    'Array di bit per la scrittura degli output
    Protected m_DigitalDataOutputs() As Boolean
    Public Structure DriverList
        Dim m_Number As Integer
        Dim m_Name As String
    End Structure
    Protected m_DriversList As ArrayList
    Private Structure LinkVariableLine
        Dim LineNumber As Integer
        Dim Var As BaseVariable
    End Structure
    'Liste che contengono i riferimenti diretti alle variabili e la linea di I o O
    Dim m_DigitalInputVariablesList() As LinkVariableLine
    Dim m_DigitalOutputVariablesList() As LinkVariableLine
    Public Sub New()
    End Sub
    Public Function ConnectDriver(ByVal DevID As String) As Boolean
        'Se non ci sono errori e supera il test restituisce true
        'Si collega al device
        'm_IODriver = New NI_PCI_6527_Driver(DevID)
        'Controlla che non ci sono errori
        If m_IODriver.GetErrors.Count = 0 Then
            'Effettua un test
            If m_IODriver.TestDevice Then
                'Se non ci sono errori dimensiona gli array di bit e supera il test restituisce true 
                ReDim m_DigitalDataInputs(m_IODriver.GetDigitalInputNumber - 1)
                ReDim m_DigitalDataOutputs(m_IODriver.GetDigitalOutputNumber - 1)
                ConnectDriver = True
            End If
        End If
    End Function
    Public Sub VariablesMapping(ByRef RefVariablesLists As VariablesLists)
        Try
            'Scansione delle liste di variabili per mappare le variabili sulle linee fisiche
            For Each VL As VariablesList In RefVariablesLists
                For Each V As BaseVariable In VL
                    If V.Address <> "" Then
                        AssignVariableToList(V)
                    End If
                Next V
            Next VL
        Catch ex As System.Exception
            ' MsgBox(ex.Message)
            Throw
        End Try
    End Sub
    Private Function AssignVariableToList(ByVal V As BaseVariable) As Boolean
        'Legge il massimo numero di input e output del device
        Dim MaxInput As Integer = m_IODriver.GetDigitalInputNumber
        Dim MaxOutput As Integer = m_IODriver.GetDigitalOutputNumber

        'Seleziona tra I o O
        Dim CurrentInputNumber As Integer
        Dim CurrentOutputNumber As Integer
        Select Case Mid(V.Address, 1, 1)
            Case "I" 'E' un input
                'Cotrolla che l'indirizzo non faccia riferimento ad un numero de input maggiore di MaxInput
                If Not Mid(V.Address, 2, 1) * 7 + Mid(V.Address, 4, 1) > MaxInput - 1 Then
                    'Aumenta la dimensione della matrice
                    ReDim Preserve m_DigitalInputVariablesList(CurrentInputNumber)
                    'Calcola la porta e la linea (Effettua uno shift di 7 posti nella lista per ogni porta)
                    m_DigitalInputVariablesList(CurrentInputNumber).LineNumber = (Mid(V.Address, 2, 1) * 7 + Mid(V.Address, 4, 1))
                    m_DigitalInputVariablesList(CurrentInputNumber).Var = V
                    CurrentInputNumber = CurrentInputNumber + 1
                End If
            Case "O" 'E' un output
                'Cotrolla che l'indirizzo non faccia riferimento ad un numero di output maggiore di MaxOuput
                If Not Mid(V.Address, 2, 1) * 7 + Mid(V.Address, 4, 1) > MaxOutput - 1 Then
                    'Aumenta la dimensione della matrice
                    ReDim Preserve m_DigitalOutputVariablesList(CurrentOutputNumber)
                    'Calcola la porta e la linea (Effettua uno shift di 7 posti nella lista per ogni porta)
                    m_DigitalOutputVariablesList(CurrentOutputNumber).LineNumber = (Mid(V.Address, 2, 1) * 7 + Mid(V.Address, 4, 1))
                    m_DigitalOutputVariablesList(CurrentOutputNumber).Var = V
                    CurrentOutputNumber = CurrentOutputNumber + 1
                End If
        End Select
    End Function
    Public Function TestDevice() As Boolean

    End Function
    Public Function WriteDigitalOutputs() As Boolean
        'Aggiorna la matrice di bit m_DigitalDataOutputs
        LoadDigitalDataToWrite()
        'Scrive gli output attraverso il driver
        Try
            m_IODriver.WriteDigitalOutputs(m_DigitalDataOutputs)
        Catch ex As System.Exception
            ' MsgBox(ex.Message)
            Throw
        End Try
    End Function
    Public Function GetDigitalInputs() As Boolean
        'Legge gli input attraverso il driver
        Try
            m_IODriver.GetDigitalInputs(m_DigitalDataInputs)
            'Aggiorna le variabili
            StoreDigitalData()
        Catch ex As System.Exception
            ' MsgBox(ex.Message)
            Throw
        End Try
    End Function
    Private Function LoadDigitalDataToWrite() As Boolean
        'Riempie la matrice di bit m_DigitalDataOutputs leggedo i valori delle variabili di output
        Dim i As Integer
        'Per ogni elemento in m_DigitalOutputVariablesList assegna il valore della variabile dell'elemento....
        '....al bit specificato in LineNumber dello stesso elemento
        For i = 0 To m_DigitalOutputVariablesList.GetLength(0) - 1
            m_DigitalDataOutputs(m_DigitalOutputVariablesList(i).LineNumber) = -m_DigitalOutputVariablesList(i).Var.ReadValue
        Next i
    End Function
    Private Sub StoreDigitalData()
        'Aggiorna le variabili di input
        Dim i As Integer
        'Per ogni elemento in m_DigitalInputVariablesList legge il valore da assegnare alla variabile dell'elemento....
        '....dal bit specificato in LineNumber dello stesso elemento
        For i = 0 To m_DigitalInputVariablesList.GetLength(0) - 1
            m_DigitalInputVariablesList(i).Var.SetValue(m_DigitalDataInputs(m_DigitalInputVariablesList(i).LineNumber))
        Next i
    End Sub
    Public Sub Dispose()

    End Sub
End Class
