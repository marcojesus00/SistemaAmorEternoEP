Imports System.Data.SqlClient

Public Class ConnectionManager

    Private _connectionString As String

    Public Sub New(ByVal server As String, ByVal database As String, ByVal username As String, ByVal password As String)
        _connectionString = "Server=" & server & ";Database=" & database & ";UID=" & username & ";PWD=" & password
    End Sub

    Public Function GetConnection() As SqlConnection
        Dim connection = New SqlConnection(_connectionString)
        connection.Open()
        Return connection
    End Function

    Public Sub CloseConnection(ByVal connection As SqlConnection)
        If connection.State = ConnectionState.Open Then
            connection.Close()
        End If
    End Sub

End Class