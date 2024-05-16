﻿Imports System
Imports System.IO
Imports System.Data.SqlClient
Imports System.IO.Path

Public Class FileHelper



    Public Shared Function GetTempFilePath(fileName As String) As String
        Dim tempDirectory As String = Path.GetTempPath()
        Dim filePath As String = Path.Combine(tempDirectory, fileName)
        Return filePath
    End Function
    Public Function SaveToTemporaryFile(fileData As Byte(), fileExtension As String) As String
        Dim filePath As String = GetTempFilePath(Guid.NewGuid().ToString() & fileExtension)
        File.WriteAllBytes(filePath, fileData)
        Return filePath
    End Function


    Public Shared Function LessThanFileSizeLimit(fileSize As Long, sizeAllowed As Long) As Boolean
        If fileSize > sizeAllowed Then
            Return False
        End If
        Return True
    End Function

    Public Shared Function ValidateFileExtension(fileName As String, allowedExtensions() As String) As Boolean
        Dim fileExtension As String = Path.GetExtension(fileName)

        For Each ext As String In allowedExtensions
            If String.Equals(ext, fileExtension, StringComparison.OrdinalIgnoreCase) Then
                Return True
            End If
        Next

        Return False
    End Function
    Public Shared Function CheckFileExists(filePath As String) As Boolean
        CheckFileExists = System.IO.File.Exists(filePath)
    End Function

    Public Shared Sub ExecuteQuery(query As String, numeroDeEmpleado As String, fileName As String, completeRelativePath As String, descripcion As String, connectionString As String)
        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@NumeroDeEmpleado", numeroDeEmpleado)
                command.Parameters.AddWithValue("@NombreDelArchivo", fileName)
                command.Parameters.AddWithValue("@Ruta", completeRelativePath)
                command.Parameters.AddWithValue("@Descripcion", descripcion)
                connection.Open()
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub
    Public Shared Function DeleteFile(filePath As String)

        If File.Exists(filePath) Then
            File.Delete(filePath)
            Return True
        Else
            Return False
        End If
    End Function
End Class
