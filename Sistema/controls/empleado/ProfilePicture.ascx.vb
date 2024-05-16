Imports System.IO

Public Class ProfilePicture
    Inherits System.Web.UI.UserControl

    Dim queryDelete As String
    Dim queryRetrieve As String
    Dim numeroDeEmpleado As String
    Public Event AlertGenerated As EventHandler(Of AlertEventArgs)
    Dim fileTypesAllowed As String() = {".png", ".jpg", ".jpeg"}

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblUploadMessage.ForeColor = Drawing.Color.Red
        If Session("Codigo_Empleado") Then

            BindCard()

        End If


        If Not IsPostBack Then

        End If

    End Sub

    Private Function DeleteRecordFromDatabase(employeeId As String)
        Dim usuario = Session("Usuario")
        Dim clave = Session("Clave")
        Dim servidor = Session("Servidor")
        Dim bd = Session("Bd")
        Dim conf As New Configuracion(usuario, clave, "FUNAMOR", servidor)
        Try
            queryDelete = " DELETE FROM [dbo].[FotoDeEmpleado] WHERE NumeroDeEmpleado = " & employeeId
            conf.EjecutaSql(queryDelete)
            Return True
        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error al cambiar la foto de perfil: " & ex.Message, "danger"))

        End Try

        Return False
    End Function

    Private Function RetrievePathFromDatabase()
        Dim usuario = Session("Usuario")
        Dim clave = Session("Clave")
        Dim servidor = Session("Servidor")
        Dim bd = Session("Bd")
        Dim conf As New Configuracion(usuario, clave, "FUNAMOR", servidor)
        Try
            numeroDeEmpleado = Session("Codigo_Empleado")
            queryRetrieve = " SELECT top 1 * FROM [dbo].[FotoDeEmpleado] WHERE NumeroDeEmpleado = " & numeroDeEmpleado
            Dim Datos = conf.EjecutaSql(queryRetrieve)
            Dim dTable = Datos.Tables(0)
            If dTable Is Nothing Then
                Return ""
            Else
                If dTable.Rows.Count = 0 Then
                    Return ""
                Else
                    Return dTable.Rows(0).Item("Ruta").ToString

                End If


            End If

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

    Protected Function Delete(employeeId As String) As Boolean
        Dim priorFilePath As String = RetrievePathFromDatabase()

        priorFilePath = Server.MapPath(priorFilePath)
        Try
            Dim fileDeleted As Boolean = FileHelper.DeleteFile(priorFilePath)
            If fileDeleted Then
                If DeleteRecordFromDatabase(employeeId) Then
                    Return True

                End If
            Else
                RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error al cambiar la imagen", "danger"))
            End If

        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error al cambiar la foto: " & ex.Message, "danger"))

        End Try
        Return False


    End Function
    Protected Sub PreviewButton_Click(sender As Object, e As EventArgs) Handles PreviewButton0.Click
        Dim msg As String
        Dim alertType As String
        Dim fileName As String = File1.PostedFile.FileName
        Dim fileSize As Long = File1.PostedFile.ContentLength

        Try
            If Not FileHelper.LessThanFileSizeLimit(fileSize, 10485760) Then
                msg = "El archivo no puede tener un tamaño mayor a 10MB"
                alertType = "danger"
                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))
                Exit Sub
            End If
            If Not FileHelper.ValidateFileExtension(fileName, fileTypesAllowed) Then
                msg = "Solo se admiten archivos con formato png, jpeg o jpg"
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

            imgProfile.ImageUrl = dataUrl
            changePhotoButton.Visible = False
            UploadFile.Visible = True
            CancelUpload.Visible = True

        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(ex.Message, "danger"))

        End Try


    End Sub

    Protected Sub UploadFileButton_Click(sender As Object, e As EventArgs) Handles UploadFile.Click
        Try
            Dim usuario = Session("Usuario")
            Dim clave = Session("Clave")
            Dim servidor = Session("Servidor")
            Dim bd = Session("Bd")
            If usuario IsNot Nothing AndAlso clave IsNot Nothing AndAlso servidor IsNot Nothing AndAlso bd IsNot Nothing Then
                If Not File1.PostedFile Is Nothing Then
                    If File1.PostedFile.ContentLength > 0 Then
                        Dim conf As New Configuracion(usuario, clave, "FUNAMOR", servidor)
                        Dim fileName As String = File1.PostedFile.FileName
                        Dim fileSize As Long = File1.PostedFile.ContentLength
                        Dim insertRecord As String
                        Dim numeroDeEmpleado As String
                        Dim relativePath As String = "FotosDePerfil/"
                        Dim path As String = Server.MapPath(relativePath)
                        Dim msg As String
                        Dim alertType As String

                        If Not Directory.Exists(path) Then
                            Directory.CreateDirectory(path)
                        End If

                        If Session("Codigo_Empleado") IsNot Nothing Then
                            Try
                                numeroDeEmpleado = Session("Codigo_Empleado")
                                fileName = numeroDeEmpleado & "_" & fileName
                                Dim savePath As String = path & fileName
                                Dim completeRelativePath As String = relativePath & fileName
                                UploadFile.Visible = False
                                CancelUpload.Visible = False
                                If Delete(numeroDeEmpleado) Then
                                    File1.PostedFile.SaveAs(savePath)

                                    insertRecord = " INSERT INTO [dbo].[FotoDeEmpleado]
                                                    (
                                                        [NumeroDeEmpleado], 
                                                        [Ruta]
                                                    )
                                                    VALUES
                                                    (
                                                        '" + numeroDeEmpleado + "', 
                                                        '" + completeRelativePath + "'
                                                    )"


                                    Dim Datos = conf.EjecutaSql(insertRecord)


                                    If FileHelper.CheckFileExists(Server.MapPath(completeRelativePath)) Then
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
                        lblUploadMessage.Text = "Por favor seleccione un archivo para subir."


                    End If

                Else
                    lblUploadMessage.Text = "Por favor seleccione un archivo para subir."
                End If
            End If


        Catch ex As Exception
            Dim msg = "Error" & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))

        End Try
    End Sub


End Class

