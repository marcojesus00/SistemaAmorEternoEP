Imports System.Data.SqlClient
Imports System.IO

Public Class ProfilePicture
    Inherits System.Web.UI.UserControl
    Dim employeeId As Integer
    Public Event AlertGenerated As EventHandler(Of AlertEventArgs)
    Dim fileTypesAllowed As String() = {".jpg", ".jpeg"}
    Dim thePostedImage
    Dim noProfilePictureRelativePath = "imagenes/no_profile_picture.png"
    Dim ServerPath As String = ConfigurationManager.AppSettings("ServerPath")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblUploadMessage.ForeColor = Drawing.Color.Red

        If Session("Codigo_Empleado") Then

            employeeId = CInt(Session("Codigo_Empleado"))

            If Not IsPostBack Then

                Try
                    BindCard(employeeId)
                Catch ex As InvalidCastException
                    RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error de conversión: " & ex.Message, "danger"))
                Catch ex As Exception
                    RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error inesperado: " & ex.Message, "danger"))
                End Try

            End If
        End If

    End Sub

    Private Function DeleteRecordFromDatabase(employeeId As Integer)

        Try
            Using dbContext As New FunamorContext()
                Dim recordsToDelete = dbContext.FotosDeEmpleados.Where(Function(f) f.NumeroDeEmpleado = employeeId)
                dbContext.FotosDeEmpleados.RemoveRange(recordsToDelete)
                dbContext.SaveChanges()
            End Using
            Session("tabSelected") = "ProfilePicturaTab"

        Catch ex As SqlException
            Dim msg As String = "Error en la base de datos: " & vbCrLf & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error al cambiar la foto de perfil: " & ex.Message, "danger"))
        End Try

        Return False
    End Function
    Private Function InsertRecordIntoDatabase(employeeId As Integer, completeRelativePath As String)

        Try

            Using dbContext As New FunamorContext()
                Dim newProfilePicture As New FotoDeEmpleado With {
                    .NumeroDeEmpleado = employeeId,
                    .Ruta = completeRelativePath
                }
                dbContext.FotosDeEmpleados.Add(newProfilePicture)
                dbContext.SaveChanges()
            End Using
            Session("tabSelected") = "ProfilePicturaTab"

        Catch ex As SqlException
            Dim msg As String = "Error en la base de datos: ." & vbCrLf & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error al cambiar la foto de perfil: " & ex.Message, "danger"))

        End Try

        Return False
    End Function
    Private Function RetrievePathFromDatabase(employeeId As Integer)

        Try

            Using dbContext As New FunamorContext()
                Dim record As FotoDeEmpleado = dbContext.FotosDeEmpleados.Where(Function(f) f.NumeroDeEmpleado = employeeId).FirstOrDefault()

                If record Is Nothing Then
                    Return ""
                Else
                    Return record.Ruta
                End If

            End Using

        Catch ex As SqlException
            Dim msg As String = "Error al cargar archivo: Database error occurred." & vbCrLf & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
        Catch ex As Exception
            Dim msg = "Error al cargar archivo: " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
        End Try

        Return ""

    End Function

    Private Sub BindCard(employeeId As Integer)
        Dim employeeIsSelected = Session("Codigo_Empleado").ToString.Length > 0
        Dim dbPath As String

        Try

            If employeeIsSelected Then
                dbPath = RetrievePathFromDatabase(employeeId)

                Dim fileExists As Boolean = FileHelper.CheckFileExists(Path.Combine(ServerPath, dbPath))

                If dbPath.Length > 0 And fileExists Then
                    imgProfile.ImageUrl = "http:" & ServerPath & dbPath
                    Session("currentDBPath") = dbPath
                Else
                    imgProfile.ImageUrl = Path.Combine("~/", noProfilePictureRelativePath)
                End If

            Else
                imgProfile.ImageUrl = Path.Combine("~/", noProfilePictureRelativePath)
            End If

        Catch ex As ArgumentNullException
            Dim msg = "Error: falta un argumento. " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))

        Catch ex As InvalidOperationException
            Dim msg = "Error: Invalid operation. " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))

        Catch ex As IOException
            Dim msg = "Error al acceder al sistema de archivos. " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))

        Catch ex As Exception
            Dim msg = "Error al leer archivo: " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
        End Try

    End Sub

    Protected Sub PreviewButton_Click(sender As Object, e As EventArgs) Handles PreviewButton0.Click
        Dim msg As String
        Dim alertType As String
        Dim fileSize As Long = File1.PostedFile.ContentLength
        Dim directoryTempRelativePath As String = "Musica\Empleados\Temp\"
        Dim directoryTempAbsolutePath As String = Path.Combine(ServerPath, directoryTempRelativePath)
        Dim fileExtension As String = Path.GetExtension(File1.PostedFile.FileName)
        Dim randomNumer As String = NumberHelper.GenerateRandomNumber(3).ToString()
        Dim newFileName = employeeId & "_" & randomNumer & fileExtension
        Dim fileTempRelativePath = directoryTempRelativePath & newFileName
        Dim fileTempAbsolutePath = directoryTempAbsolutePath & newFileName
        Dim combinedFileTypesString As String = Strings.Join(fileTypesAllowed, " ")

        Session("fileTempAbsolutePath") = fileTempAbsolutePath
        Session("fileTempRelativePath") = fileTempRelativePath

        Try
            Session("tabSelected") = "ProfilePicturaTab"

            msg = "Por favor seleccione un documento."
            alertType = "danger"

            If File1.PostedFile Is Nothing Then
                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))
                Exit Sub
            End If

            Dim postedFile As HttpPostedFile = File1.PostedFile

            If postedFile.ContentLength < 1 Then
                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))
                Exit Sub
            End If

            If Not FileHelper.LessThanFileSizeLimit(fileSize, 10485760) Then
                msg = "El archivo no puede tener un tamaño mayor a 10MB"
                alertType = "danger"
                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))
                Exit Sub
            End If

            If Not FileHelper.ValidateFileExtension(newFileName, fileTypesAllowed) Then
                msg = "Solo se admiten archivos con los formatos: " & combinedFileTypesString
                alertType = "danger"
                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))
                Exit Sub
            End If


            ' Read the file content
            Dim reader As New BinaryReader(postedFile.InputStream)
            Dim fileContent As Byte() = reader.ReadBytes(CInt(fileSize))

            ' Create data URL
            Dim dataUrl As String = "data:" & postedFile.ContentType & ";base64," & Convert.ToBase64String(fileContent)
            Session("UploadedFileContentLength") = fileSize
            FileHelper.createFolderIfNotExists(directoryTempAbsolutePath)
            File1.PostedFile.SaveAs(fileTempAbsolutePath)
            imgProfile.ImageUrl = dataUrl
            changePhotoButton.Visible = False
            UploadFile.Visible = True
            CancelUpload.Visible = True
        Catch ex As UnauthorizedAccessException
            ' Handle access permissions issue
            msg = "Acceso denegado: " & ex.Message
            alertType = "danger"
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))

        Catch ex As ArgumentException
            ' Handle invalid path issue
            alertType = "danger"
            msg = "Ruta inválida: " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))

        Catch ex As HttpException
            msg = "Error al cargar el archivo: " & ex.Message
            alertType = "danger"
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))
        Catch ex As IOException
            msg = "Error al procesar el archivo: " & ex.Message
            alertType = "danger"
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))
        Catch ex As Exception
            msg = "Error inesperado: " & ex.Message
            alertType = "danger"
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))
        End Try

    End Sub
    Protected Sub PreviewButton1_Click(sender As Object, e As EventArgs) Handles PreviewButton1.Click
        Dim msg As String
        Dim alertType As String
        'Dim fileSize As Long = File1.PostedFile.ContentLength
        Dim directoryTempRelativePath As String = "Musica\Empleados\Temp\"
        Dim directoryTempAbsolutePath As String = Path.Combine(ServerPath, directoryTempRelativePath)
        'Dim fileExtension As String = Path.GetExtension(File1.PostedFile.FileName)
        Dim randomNumer As String = NumberHelper.GenerateRandomNumber(3).ToString()
        Dim newFileName = employeeId & "_" & randomNumer & ".jpg"
        Dim fileTempRelativePath = directoryTempRelativePath & newFileName
        Dim fileTempAbsolutePath = directoryTempAbsolutePath & newFileName
        Dim combinedFileTypesString As String = Strings.Join(fileTypesAllowed, " ")

        Session("fileTempAbsolutePath") = fileTempAbsolutePath
        Session("fileTempRelativePath") = fileTempRelativePath

        Try
            Session("tabSelected") = "ProfilePicturaTab"

            msg = "Por favor toma una fotografía."
            alertType = "danger"
            Dim imageData As String = HiddenFieldImageData.Value

            If String.IsNullOrEmpty(imageData) Then
                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))
                Exit Sub
            End If


            ' Read the file content
            Dim base64Data As String = imageData.Replace("data:image/jpeg;base64,", String.Empty)
            Dim bytes As Byte() = Convert.FromBase64String(base64Data)

            Session("UploadedFileContentLength") = "1000"
            FileHelper.createFolderIfNotExists(directoryTempAbsolutePath)
            File.WriteAllBytes(fileTempAbsolutePath, bytes)

            imgProfile.ImageUrl = imageData
            changePhotoButton.Visible = False
            UploadFile.Visible = True
            CancelUpload.Visible = True
        Catch ex As UnauthorizedAccessException
            ' Handle access permissions issue
            msg = "Acceso denegado: " & ex.Message
            alertType = "danger"
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))

        Catch ex As ArgumentException
            ' Handle invalid path issue
            alertType = "danger"
            msg = "Ruta inválida: " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))

        Catch ex As HttpException
            msg = "Error al cargar el archivo: " & ex.Message
            alertType = "danger"
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))
        Catch ex As IOException
            msg = "Error al procesar el archivo: " & ex.Message
            alertType = "danger"
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))
        Catch ex As Exception
            msg = "Error inesperado: " & ex.Message
            alertType = "danger"
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))
        End Try

    End Sub
    Protected Sub UploadFileButton_Click(sender As Object, e As EventArgs) Handles UploadFile.Click

        Try
            changePhotoButton.Visible = True
            UploadFile.Visible = False
            CancelUpload.Visible = False

            If Session("UploadedFileContentLength") IsNot Nothing Then

                If Session("UploadedFileContentLength").ToString().Length > 0 Then
                    Dim relativeDirectoryPath As String = "Musica\Empleados\FotosDePerfil\"
                    Dim directoryAbsolutePath As String = Path.Combine(ServerPath, relativeDirectoryPath)
                    Dim fileTempAbsolutePath = Session("fileTempAbsolutePath")
                    Dim newFileName = Path.GetFileName(fileTempAbsolutePath)
                    Dim fileRelativePath As String = Path.Combine(relativeDirectoryPath, newFileName)
                    Dim msg As String
                    Dim alertType As String
                    Dim priorFileRelativePath As String
                    If Session("Codigo_Empleado") IsNot Nothing Then

                        Try
                            Dim fileIsMoved = FileHelper.MoveFile(fileTempAbsolutePath, directoryAbsolutePath)
                            If fileIsMoved Then
                                DeleteRecordFromDatabase(employeeId)
                                InsertRecordIntoDatabase(employeeId, fileRelativePath)
                                If Session("currentDBPath") IsNot Nothing Then
                                    priorFileRelativePath = Session("currentDBPath").ToString()
                                    If Path.GetFileName(priorFileRelativePath) <> newFileName Then
                                        FileHelper.DeleteFile(Path.Combine(ServerPath, priorFileRelativePath))
                                    End If
                                End If

                                If FileHelper.CheckFileExists(Path.Combine(ServerPath, fileRelativePath)) Then
                                    msg = "Carga exitosa"
                                    alertType = "success"
                                    BindCard(employeeId)
                                    RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))

                                Else

                                    msg = "Error al cambiar la foto: Archivo no subido correctamente"
                                    alertType = "danger"
                                    RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))

                                End If


                            Else
                                msg = "Error al cambiar la foto: Archivo no subido correctamente"
                                alertType = "danger"
                                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))

                            End If

                        Catch ex As Exception
                            msg = "Error al cargar archivos: " & ex.Message
                            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
                        End Try

                    Else
                        msg = "Error: No se pudo identificar el empleado para subir archivos."
                        RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
                    End If

                Else
                    RaiseEvent AlertGenerated(Me, New AlertEventArgs("Por favor seleccione un archivo para subir.", "danger"))
                End If

            Else
                RaiseEvent AlertGenerated(Me, New AlertEventArgs("Por favor seleccione un archivo para subir.", "danger"))
            End If
            Session("tabSelected") = "ProfilePicturaTab"


        Catch ex As Exception
            Dim msg = "Error" & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
        End Try

    End Sub

    Protected Sub CancelUploadButton_Click(sender As Object, e As EventArgs) Handles CancelUpload.Click

        Try
            changePhotoButton.Visible = True
            UploadFile.Visible = False
            CancelUpload.Visible = False
            FileHelper.DeleteFile(Session("fileTempAbsolutePath").ToString)
            Session("tabSelected") = "ProfilePicturaTab"

            BindCard(employeeId)

        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs("" & ex.Message, "danger"))
        End Try

    End Sub

End Class

