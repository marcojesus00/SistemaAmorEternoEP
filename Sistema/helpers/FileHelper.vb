Imports System.Data.SqlClient
Imports System.IO
Imports System.Reflection

Public Class FileHelper


    Public Shared Function MoveFile(sourceFileAbsolutePath As String, destinationDirectoryAbsolutePath As String) As Boolean
        Try
            If Not CheckFileExists(sourceFileAbsolutePath) Then
                Throw New FileNotFoundException("File not found: " & sourceFileAbsolutePath)
            End If

            Dim filename As String = Path.GetFileName(sourceFileAbsolutePath)
            Dim newDestinationPath As String = Path.Combine(destinationDirectoryAbsolutePath, filename)

            createFolderIfNotExists(destinationDirectoryAbsolutePath)
            DeleteFile(newDestinationPath)
            File.Move(sourceFileAbsolutePath, newDestinationPath)
            If CheckFileExists(newDestinationPath) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception

            Throw ex
        End Try
    End Function



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
    Public Shared Sub createFolderIfNotExists(path)
        If Not Directory.Exists(path) Then
            Directory.CreateDirectory(path)
        End If
    End Sub

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
            If File.Exists(filePath) Then
                Return False
            Else
                Return True

            End If
        Else
            Return True

        End If
    End Function

    Public Sub ExportDataTableToHtml(dataTable As DataTable, filePath As String)
        Using writer As New StreamWriter(filePath)
            ' Write the HTML header
            writer.WriteLine("<html>")
            writer.WriteLine("<head><title>DataTable Export</title></head>")
            writer.WriteLine("<body>")
            writer.WriteLine("<table border='1'>")

            ' Write the table header
            writer.WriteLine("<tr>")
            For Each column As DataColumn In dataTable.Columns
                writer.WriteLine($"<th>{column.ColumnName}</th>")
            Next
            writer.WriteLine("</tr>")

            ' Write the table rows
            For Each row As DataRow In dataTable.Rows
                writer.WriteLine("<tr>")
                For Each cell As Object In row.ItemArray
                    writer.WriteLine($"<td>{cell.ToString()}</td>")
                Next
                writer.WriteLine("</tr>")
            Next

            ' Close the table and HTML tags
            writer.WriteLine("</table>")
            writer.WriteLine("</body>")
            writer.WriteLine("</html>")
        End Using
    End Sub

    Public Function ExportListToHtml(Of T)(list As List(Of T)) As String
        If list Is Nothing OrElse list.Count = 0 Then
            Return String.Empty
        End If

        Dim type As Type = GetType(T)
        Dim properties As PropertyInfo() = type.GetProperties()

        Dim htmlBuilder As New StringBuilder()

        ' Write the HTML header
        htmlBuilder.AppendLine("<html>")
        htmlBuilder.AppendLine("<head><title>List Export</title></head>")
        htmlBuilder.AppendLine("<body>")
        htmlBuilder.AppendLine("<table border='1'>")

        ' Write the table header
        htmlBuilder.AppendLine("<tr>")
        For Each prop As PropertyInfo In properties
            htmlBuilder.AppendLine($"<th>{prop.Name}</th>")
        Next
        htmlBuilder.AppendLine("</tr>")

        ' Write the table rows
        For Each item As T In list
            htmlBuilder.AppendLine("<tr>")
            For Each prop As PropertyInfo In properties
                Dim value As Object = prop.GetValue(item, Nothing)
                htmlBuilder.AppendLine($"<td>{value}</td>")
            Next
            htmlBuilder.AppendLine("</tr>")
        Next

        ' Close the table and HTML tags
        htmlBuilder.AppendLine("</table>")
        htmlBuilder.AppendLine("</body>")
        htmlBuilder.AppendLine("</html>")

        Return htmlBuilder.ToString()
    End Function

    'Public Function ExportListToHtml(Of T)(list As List(Of T), filePath As String) As Boolean
    '    If list Is Nothing OrElse list.Count = 0 Then
    '        Return False
    '    End If

    '    Dim type As Type = GetType(T)
    '    Dim properties As Reflection.PropertyInfo() = type.GetProperties()

    '    Using writer As New StreamWriter(filePath)
    '        ' Write the HTML header
    '        writer.WriteLine("<html>")
    '        writer.WriteLine("<head><title>List Export</title></head>")
    '        writer.WriteLine("<body>")
    '        writer.WriteLine("<table border='1'>")

    '        ' Write the table header
    '        writer.WriteLine("<tr>")
    '        For Each prop As Reflection.PropertyInfo In properties
    '            writer.WriteLine($"<th>{prop.Name}</th>")
    '        Next
    '        writer.WriteLine("</tr>")

    '        ' Write the table rows
    '        For Each item As T In list
    '            writer.WriteLine("<tr>")
    '            For Each prop As Reflection.PropertyInfo In properties
    '                Dim value As Object = prop.GetValue(item, Nothing)
    '                writer.WriteLine($"<td>{value}</td>")
    '            Next
    '            writer.WriteLine("</tr>")
    '        Next

    '        ' Close the table and HTML tags
    '        writer.WriteLine("</table>")
    '        writer.WriteLine("</body>")
    '        writer.WriteLine("</html>")
    '    End Using

    '    Return True
    'End Function


End Class
