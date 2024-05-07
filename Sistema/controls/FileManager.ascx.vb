Imports System.IO

Public Class FileManager
    Inherits System.Web.UI.UserControl

    Dim queryDelete As String
    Dim queryRetrieve As String
    Dim numeroDeEmpleado As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblUploadMessage.ForeColor = Drawing.Color.Red

        Try
            BindGridView()
            lblMessage.Text = "Archivos encontrados: " & $"{MyGridView.Rows.Count}"
        Catch ex As Exception
            lblMessage.Text = "Error al leer de la base de datos, por favor recargue la página : " & ex.Message
        End Try

        If Not IsPostBack Then

        End If

    End Sub

    Protected Sub MyGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        If e.CommandName = "DownloadFile" Then
            Dim args As String() = e.CommandArgument.ToString().Split("|"c)
            Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)

            Dim dataSource As DataTable = DirectCast(MyGridView.DataSource, DataTable)

            Dim documentPath As String = dataSource.Rows(rowIndex).Item("Ruta").ToString()

            Dim documentName As String = dataSource.Rows(rowIndex).Item("NombreDelArchivo").ToString()
            Dim handlerUrl As String = $"/Handlers/DownloadHandler.ashx?path={HttpUtility.UrlEncode(documentPath)}&name={HttpUtility.UrlEncode(documentName)}"
            Response.Redirect(handlerUrl)
        End If
    End Sub
    Protected Sub MyGridView_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs) Handles MyGridView.RowDeleting
        Dim rowIndex As Integer = e.RowIndex
        Dim id As String = MyGridView.DataKeys(rowIndex).Value.ToString()
        Dim dataSource As DataTable = DirectCast(MyGridView.DataSource, DataTable)

        Dim filePath As String = dataSource.Rows(rowIndex).Item("Ruta").ToString()

        Try
            DeleteRecordFromDatabase(id)
            DeleteFile(filePath)
            BindGridView()
        Catch ex As Exception

        End Try


    End Sub
    Private Sub DeleteRecordFromDatabase(id As String)
        Dim Usuario = Session("Usuario")
        Dim Clave = Session("Clave")
        Dim Servidor = Session("Servidor")
        Dim Bd = Session("Bd")
        Dim conf As New Configuracion(Usuario, Clave, "FUNAMOR", Servidor)
        Try
            queryDelete = " DELETE FROM [dbo].[DocumentosDeEmpleado] WHERE Id = " & id
            conf.EjecutaSql(queryDelete)
        Catch ex As Exception
            lblMessage.Text = "Error al eliminar registro de la base de datos, por favor vuelva a intentarlo : " & ex.Message

        End Try


    End Sub
    Private Sub DeleteFile(filePath As String)
        Try
            If File.Exists(filePath) Then
                File.Delete(filePath)
            End If

        Catch ex As Exception
            lblMessage.Text = "Error al eliminar archivo, por favor vuelva a intentarlo : " & ex.Message

        End Try

    End Sub


    Private Sub BindGridView()
        Dim Usuario = Session("Usuario")
        Dim Clave = Session("Clave")
        Dim Servidor = Session("Servidor")
        Dim Bd = Session("Bd")
        Dim conf As New Configuracion(Usuario, Clave, "FUNAMOR", Servidor)
        Try
            If Session("Codigo_Empleado") Then
                numeroDeEmpleado = Session("Codigo_Empleado")
                queryRetrieve = " SELECT * FROM [dbo].[DocumentosDeEmpleado] WHERE NumeroDeEmpleado = " & numeroDeEmpleado
                Dim Datos = conf.EjecutaSql(queryRetrieve)
                MyGridView.DataSource = Datos.Tables(0)
                MyGridView.DataBind()
                If MyGridView.Rows.Count = 0 Then
                    lblMessage.Text = "No se encontraron documentos"
                Else
                    lblMessage.Text = ""
                End If
            Else
            End If



        Catch ex As Exception
            lblMessage.Text = "Error, por favor vuelva a intentarlo : " & ex.Message

        End Try



    End Sub

    Protected Sub UploadFileButton_Click(sender As Object, e As EventArgs) Handles UploadFile.Click
        Try
            Dim Usuario = Session("Usuario")
            Dim Clave = Session("Clave")
            Dim Servidor = Session("Servidor")
            Dim Bd = Session("Bd")
            If usuario IsNot Nothing AndAlso clave IsNot Nothing AndAlso servidor IsNot Nothing AndAlso bd IsNot Nothing Then
                If Not File1.PostedFile Is Nothing Then
                    If File1.PostedFile.ContentLength > 0 Then
                        Dim conf As New Configuracion(Usuario, Clave, "FUNAMOR", Servidor)
                        Dim fileName As String = File1.PostedFile.FileName
                        Dim fileSize As Long = File1.PostedFile.ContentLength
                        Dim queryDocumentos As String
                        Dim numeroDeEmpleado As String
                        Dim relativePath As String = "Uploads/"
                        Dim path As String = Server.MapPath(relativePath)

                        If Not Directory.Exists(path) Then
                            Directory.CreateDirectory(path)
                        End If
                        If Not FileUploadHelper.LessThanFileSizeLimit(fileSize, 10485760) Then
                            Dim msg As String = "El archivo no puede tener un tamaño mayor a 10MB"
                            Dim alertType As String = "danger"
                            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))
                            Exit Sub
                        End If
                        If Session("Codigo_Empleado") IsNot Nothing Then
                            Try
                                numeroDeEmpleado = Session("Codigo_Empleado")
                                fileName = numeroDeEmpleado & "_" & DateString & "_" & fileName
                                Dim savePath As String = path & fileName
                                Dim completeRelativePath As String = relativePath & fileName
                                Dim descripcion As String
                                descripcion = TextBoxDescription.Text


                                If FileUploadHelper.CheckFileExists(savePath) Then
                                    RaiseEvent AlertGenerated(Me, New AlertEventArgs("Ya existe un archivo con ese nombre", "danger"))
                                    Exit Sub
                                Else
                                    File1.PostedFile.SaveAs(savePath)

                                    queryDocumentos = " INSERT INTO [dbo].[DocumentosDeEmpleado]
                                                    (
                                                        [NumeroDeEmpleado], [NombreDelArchivo],
                                                        [Ruta], [Descripcion]
                                                    )
                                                    VALUES
                                                    (
                                                        '" + numeroDeEmpleado + "', '" + fileName + "',
                                                        '" + completeRelativePath + "', '" + descripcion + "'
                                                    )"


                                    Dim Datos = conf.EjecutaSql(queryDocumentos)
                                    Dim msg As String = "Carga exitosa"
                                    Dim alertType As String = "success"
                                    RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))

                                    BindGridView()
                                End If



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

    Protected Sub MyGridView_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles MyGridView.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim sizeColumnIndex As Integer = 2
            Dim fileName As String = e.Row.Cells(sizeColumnIndex).Text
            Dim fileExtension As String = Path.GetExtension(fileName)

            Dim iconCell As TableCell = e.Row.Cells(3)
            Dim imageClass = "bi bi-image"
            Select Case fileExtension
                Case ".docx"
                    iconCell.Controls.Add(New LiteralControl("<i class=""bi bi-file-word-fill text-primary""></i>"))
                Case ".xls"
                    iconCell.Controls.Add(New LiteralControl("<i class=""bi bi-file-earmark-excel-fill text-success""></i>"))
                Case ".xlsx"
                    iconCell.Controls.Add(New LiteralControl("<i class=""bi bi-file-earmark-excel-fill text-success""></i>"))
                Case ".pdf"
                    iconCell.Controls.Add(New LiteralControl("<i class=""bi bi-file-earmark-pdf-fill text-danger""></i>"))
                Case ".jpg"
                    iconCell.Controls.Add(New LiteralControl("<i class=""bi bi-image""></i>"))
                Case ".jpg"
                    iconCell.Controls.Add(New LiteralControl("<i class=""bi bi-image""></i>"))
                Case ".jpeg"
                    iconCell.Controls.Add(New LiteralControl("<i class=""bi bi-image""></i>"))
                Case ".png"
                    iconCell.Controls.Add(New LiteralControl($"<i class=""bi bi-image""></i>"))
                Case Else
                    iconCell.Controls.Add(New LiteralControl("<i class=""bi bi-file-earmark-text-fill""></i>"))
            End Select
        End If
    End Sub

    Protected Sub TextBoxDescription_TextChanged(sender As Object, e As EventArgs)

        If TextBoxDescription.Text.Length > 100 Then
            TextBoxDescription.ForeColor = Drawing.Color.Red
            lblUploadMessage.Text = "Error: La descripción no puede ser mayor a 100 caracteres."
        Else
            TextBoxDescription.ForeColor = Drawing.Color.Black
        End If
    End Sub
    Public Event AlertGenerated As EventHandler(Of AlertEventArgs)

End Class
Public Class AlertEventArgs
    Inherits EventArgs

    Public Sub New(ByVal message As String, ByVal alertType As String)
        Me.Message = message
        Me.AlertType = alertType
    End Sub

    Public Property Message As String
    Public Property AlertType As String
End Class
