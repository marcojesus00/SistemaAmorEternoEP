Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Public Class Configuracion
    Private _Usuario As String
    Private _Password As String
    Private _Db As String
    Private _Server As String
    Public _MSG As String
    Private Conector As New SqlConnection
    Private Adaptador As New SqlDataAdapter
    Private Datos As New Data.DataSet
    Private sql As String

    Public Sub New(ByVal user As String, ByVal pwd As String, ByVal db As String, ByVal server As String)
        _Usuario = user
        _Password = pwd
        _Db = db
        _Server = server
    End Sub

    Public ReadOnly Property Usuario As String
        Get
            Return _Usuario
        End Get
    End Property

    Public ReadOnly Property Password As String
        Get
            Return _Password
        End Get
    End Property

    Public ReadOnly Property Db As String
        Get
            Return _Db
        End Get
    End Property

    Public ReadOnly Property server As String
        Get
            Return _Server
        End Get
    End Property

    Public Function EjecutaSql(ByVal SQL As String)
        Dim rs As New Data.DataSet
        If (Conector.State <> ConnectionState.Open) Then
            Conector = New SqlConnection("Server=" + server + "; Database=" + Db + "; UID=" + Usuario + "; PWD=" + Password)
            Try
                Conector.Open()
            Catch ex As Exception
                If (Conector.State = ConnectionState.Open) Then
                    Conector.Close()
                End If
                _MSG = ex.Message
            End Try
        End If
        Adaptador = New SqlDataAdapter(SQL, Conector)
        Datos.Clear()
        Try
            Adaptador.Fill(rs)
            Datos = rs
            If (Conector.State = ConnectionState.Open) Then
                Conector.Close()
            End If
        Catch ex As SqlException
            If (Conector.State = ConnectionState.Open) Then
                Conector.Close()
            End If

            For i As Integer = 0 To ex.Errors.Count - 1
                If ex.Errors(i).Number = 2627 Then
                    Dim msg As String = "No se puede insertar un registro duplicado, " + " Codigo de Error: " + ex.Errors(i).Number.ToString + " " + ex.Errors(i).Message
                    _MSG = msg
                Else
                    _MSG = ex.Errors(i).Message
                End If
            Next
        End Try

        Return Datos
    End Function

End Class
