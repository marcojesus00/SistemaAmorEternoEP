Imports System.Data.SqlClient
Imports System.IO

Public Class ProfilePicture
    Inherits System.Web.UI.UserControl
    Dim queryDelete As String
    Dim queryRetrieve As String
    Dim employeeId As String
    Public Event AlertGenerated As EventHandler(Of AlertEventArgs)
    Dim fileTypesAllowed As String() = {".jpg", ".jpeg"}
    Dim thePostedImage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblUploadMessage.ForeColor = Drawing.Color.Red
        If Session("Codigo_Empleado") Then
            employeeId = Session("Codigo_Empleado")
            BindCard()

        End If


        If Not IsPostBack Then

        End If

    End Sub

    Private Function DeleteRecordFromDatabase(employeeId As String)
        Try
            Using dbContext As New MyDbContext()

                Dim recordsToDelete = dbContext.FotosDeEmpleados.Where(Function(f) f.NumeroDeEmpleado = employeeId)
                dbContext.FotosDeEmpleados.RemoveRange(recordsToDelete)
                dbContext.SaveChanges()
            End Using
        Catch ex As SqlException
            Dim msg As String = "Error: Database error occurred." & vbCrLf & ex.Message

            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))

        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error al cambiar la foto de perfil: " & ex.Message, "danger"))

        End Try

        Return False
    End Function
    Private Function InsertRecordIntoDatabase(employeeId As Integer, completeRelativePath As String)
        Try
            Using dbContext As New MyDbContext()

                Dim newProfilePicture As New FotoDeEmpleado
                newProfilePicture.NumeroDeEmpleado = employeeId
                newProfilePicture.Ruta = completeRelativePath
                dbContext.FotosDeEmpleados.Add(newProfilePicture)
                dbContext.SaveChanges()
            End Using
        Catch ex As SqlException
            Dim msg As String = "Error: Database error occurred." & vbCrLf & ex.Message

            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))

        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error al cambiar la foto de perfil: " & ex.Message, "danger"))

        End Try

        Return False
    End Function
    Private Function RetrievePathFromDatabase()


        Try
            employeeId = Session("Codigo_Empleado")

            Using dbContext As New MyDbContext()
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

    Private Sub BindCard()

        Dim employeeIsSelected = Session("Codigo_Empleado").ToString.Length > 0
        Dim dbPath As String
        Try

            If employeeIsSelected Then
                dbPath = RetrievePathFromDatabase()
                Dim fileExists As Boolean = FileHelper.CheckFileExists(Server.MapPath(dbPath))
                If dbPath.Length > 0 And fileExists Then
                    imgProfile.ImageUrl = "~/" & dbPath
                    Session("currentDBPath") = dbPath
                Else
                    imgProfile.ImageUrl = "~/FotosDePerfil/no.png"
                End If
            Else
                imgProfile.ImageUrl = "~/FotosDePerfil/no.png"
            End If

        Catch ex As Exception
            Dim msg = "Error al leer archivo: " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))

        End Try



    End Sub



    Protected Sub PreviewButton_Click(sender As Object, e As EventArgs) Handles PreviewButton0.Click
        Dim msg As String
        Dim alertType As String
        Dim fileName As String = File1.PostedFile.FileName
        Dim fileSize As Long = File1.PostedFile.ContentLength
        Dim savePath As String
        Dim relativePath As String = "FotosDePerfil/"
        Dim abolutePath As String = Server.MapPath(relativePath)
        Dim fileExtension As String = Path.GetExtension(fileName)
        employeeId = Session("Codigo_Empleado")
        fileName = employeeId & fileExtension
        Dim completeRelativePath = relativePath & fileName
        Dim combinedString As String = Strings.Join(fileTypesAllowed, " *")

        savePath = abolutePath & fileName
        Session("savePath") = savePath

        Session("relativePath") = completeRelativePath
        Try
            If Not FileHelper.LessThanFileSizeLimit(fileSize, 10485760) Then
                msg = "El archivo no puede tener un tamaño mayor a 10MB"
                alertType = "danger"
                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))
                Exit Sub
            End If
            If Not FileHelper.ValidateFileExtension(fileName, fileTypesAllowed) Then
                msg = "Solo se admiten archivos con los formatos: " & combinedString
                alertType = "danger"
                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))
                Exit Sub
            End If

            Dim postedFile As HttpPostedFile = File1.PostedFile

            ' Read the file content
            Dim reader As New BinaryReader(postedFile.InputStream)
            Dim fileContent As Byte() = reader.ReadBytes(CInt(postedFile.ContentLength))

            ' Create data URL
            Dim dataUrl As String = "data:" & postedFile.ContentType & ";base64," & Convert.ToBase64String(fileContent)
            Session("UploadedFileContentLength") = File1.PostedFile.ContentLength
            File1.PostedFile.SaveAs(savePath)

            imgProfile.ImageUrl = dataUrl
            changePhotoButton.Visible = False
            UploadFile.Visible = True
            CancelUpload.Visible = True

        Catch ex As HttpException ' Handle HTTP exceptions for file upload
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
                    Dim fileName As String = Session("UploadedFileName")
                    Dim fileData As Stream = Session("UploadedFileData")
                    Dim relativePath As String = "FotosDePerfil/"
                    Dim completeRelativePath As String = Session("relativePath")
                    Dim path As String = MyBase.Server.MapPath(relativePath)
                    Dim msg As String
                    Dim alertType As String
                    Dim currentDbPath As String
                    If Not Directory.Exists(path) Then
                        Directory.CreateDirectory(path)
                    End If

                    If Session("Codigo_Empleado") IsNot Nothing Then
                        Try

                            If True Then


                                DeleteRecordFromDatabase(Session("Codigo_Empleado").ToString())
                                If Session("currentDBPath") IsNot Nothing Then
                                    currentDbPath = Session("currentDBPath")
                                    If currentDbPath.Length > 0 Then
                                        Dim filePath = MyBase.Server.MapPath(currentDbPath)

                                        FileHelper.DeleteFile(filePath)

                                    End If
                                End If
                                InsertRecordIntoDatabase(Session("Codigo_Empleado"), completeRelativePath)

                                If FileHelper.CheckFileExists(MyBase.Server.MapPath(completeRelativePath)) Then
                                    msg = "Carga exitosa"
                                    alertType = "success"

                                    BindCard()
                                Else
                                    msg = "Error al cambiar la foto"
                                    alertType = "danger"


                                End If
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
            FileHelper.DeleteFile(Session("savePath").ToString)

        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs("" & ex.Message, "danger"))

        End Try

    End Sub

End Class

