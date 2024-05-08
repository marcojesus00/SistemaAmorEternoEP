Imports System.IO

Public Class ProfilePicture
    Inherits System.Web.UI.UserControl

    Dim queryDelete As String
    Dim queryRetrieve As String
    Dim numeroDeEmpleado As String
    Public Event AlertGenerated As EventHandler(Of AlertEventArgs)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblUploadMessage.ForeColor = Drawing.Color.Red
        If Session("Codigo_Empleado") Then

            BindCard()

        End If


        If Not IsPostBack Then

        End If

    End Sub

    Private Sub DeleteRecordFromDatabase(employeeId As String)
        Dim Usuario = Session("Usuario")
        Dim Clave = Session("Clave")
        Dim Servidor = Session("Servidor")
        Dim Bd = Session("Bd")
        Dim conf As New Configuracion(Usuario, Clave, "FUNAMOR", Servidor)
        Try
            queryDelete = " DELETE FROM [dbo].[FotoDeEmpleado] WHERE NumeroDeEmpleado = " & employeeId
            conf.EjecutaSql(queryDelete)
        Catch ex As Exception

        End Try


    End Sub

    Private Function RetrievePathFromDatabase()
        Dim Usuario = Session("Usuario")
        Dim Clave = Session("Clave")
        Dim Servidor = Session("Servidor")
        Dim Bd = Session("Bd")
        Dim conf As New Configuracion(Usuario, Clave, "FUNAMOR", Servidor)
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
                If dbPath.Length > 0 Then
                    imgProfile.ImageUrl = "~/" & dbPath
                Else
                    imgProfile.ImageUrl = "~/FotosDePerfil/no.png"
                End If
            Else
                imgProfile.ImageUrl = "~/FotosDePerfil/no.png"
            End If

        Catch ex As Exception
            Dim msg = "Error al cargar archivo: " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))

        End Try



    End Sub

    Protected Sub Delete(employeeId As String)
        Dim priorFilePath As String = RetrievePathFromDatabase()

        priorFilePath = Server.MapPath(priorFilePath)
        Try
            Dim fileDeleted As Boolean = FileHelper.DeleteFile(priorFilePath)
            If fileDeleted Then
                DeleteRecordFromDatabase(employeeId)
                BindCard()

            Else
                RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error al cambiar la imagen", "danger"))
            End If

        Catch ex As Exception

        End Try


    End Sub
    Protected Sub UploadFileButton_Click(sender As Object, e As EventArgs) Handles UploadFile.Click
        Try
            Dim Usuario = Session("Usuario")
            Dim Clave = Session("Clave")
            Dim Servidor = Session("Servidor")
            Dim Bd = Session("Bd")
            If Usuario IsNot Nothing AndAlso Clave IsNot Nothing AndAlso Servidor IsNot Nothing AndAlso Bd IsNot Nothing Then
                If Not File1.PostedFile Is Nothing Then
                    If File1.PostedFile.ContentLength > 0 Then
                        Dim conf As New Configuracion(Usuario, Clave, "FUNAMOR", Servidor)
                        Dim fileName As String = File1.PostedFile.FileName
                        Dim fileSize As Long = File1.PostedFile.ContentLength
                        Dim queryDocumentos As String
                        Dim numeroDeEmpleado As String
                        Dim relativePath As String = "FotosDePerfil/"
                        Dim path As String = Server.MapPath(relativePath)

                        If Not Directory.Exists(path) Then
                            Directory.CreateDirectory(path)
                        End If
                        If Not FileHelper.LessThanFileSizeLimit(fileSize, 10485760) Then
                            Dim msg As String = "El archivo no puede tener un tamaño mayor a 10MB"
                            Dim alertType As String = "danger"
                            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))
                            Exit Sub
                        End If
                        If Not FileHelper.ValidateFileExtension(fileName, {".png", ".jpg", ".jpeg"}) Then
                            Dim msg As String = "Solo se admiten imagenes con formato png, jpeg o jpg"
                            Dim alertType As String = "danger"
                            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))
                            Exit Sub
                        End If
                        If Session("Codigo_Empleado") IsNot Nothing Then
                            Try
                                numeroDeEmpleado = Session("Codigo_Empleado")
                                fileName = numeroDeEmpleado & "_" & fileName
                                Dim savePath As String = path & fileName
                                Dim completeRelativePath As String = relativePath & fileName

                                Delete(numeroDeEmpleado)
                                File1.PostedFile.SaveAs(savePath)

                                queryDocumentos = " INSERT INTO [dbo].[FotoDeEmpleado]
                                                    (
                                                        [NumeroDeEmpleado], 
                                                        [Ruta]
                                                    )
                                                    VALUES
                                                    (
                                                        '" + numeroDeEmpleado + "', 
                                                        '" + completeRelativePath + "'
                                                    )"


                                Dim Datos = conf.EjecutaSql(queryDocumentos)
                                Dim msg As String = "Carga exitosa"
                                Dim alertType As String = "success"
                                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))

                                BindCard()



                            Catch ex As Exception
                                Dim msg = "Error al cargar archivos: " & ex.Message
                                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))

                            End Try
                        Else
                            Dim msg = "Error: No se pudo identificar el empleado para subir archivos."
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

