Imports System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder
Imports System.Data.SqlClient
Imports System.IO

Public Class FileManager
    Inherits System.Web.UI.UserControl

    Dim queryDelete As String
    Dim queryRetrieve As String
    Dim numeroDeEmpleado As String
    Dim ServerPath As String = ConfigurationManager.AppSettings("ServerPath")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblUploadMessage.ForeColor = Drawing.Color.Red
        lblUploadMessage.Text = "Archivo y descripción son obligatorios"

        If Session("Codigo_Empleado") Then



            If Not IsPostBack Then
                PopulateDropdown()
                Try

                    BindGridView()
                Catch ex As Exception
                    Dim msg = "Error al leer de la base de datos, por favor recargue la página : " & ex.Message
                    RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
                End Try
            End If

        End If

    End Sub
    Private Sub PopulateDropdown()
        ddlAreArchived.Items.Clear()
        ddlAreArchived.Items.Add(New ListItem("Documentos", "False"))
        ddlAreArchived.Items.Add(New ListItem("Documentos archivados", "True"))
    End Sub
    Protected Sub MyGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        Session("tabSelected") = "DocsTab"

        If e.CommandName = "DownloadFile" Then

            Try

                Dim args As String() = e.CommandArgument.ToString().Split("|"c)
                Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = MyGridView.Rows(rowIndex)
                Dim documentName As String = row.Cells(2).Text
                Using dbContext As New MyDbContext
                    Dim DocId As Integer = MyGridView.DataKeys(rowIndex).Value.ToString()
                    Dim record As DocumentoDeEmpleado = dbContext.DocumentosDeEmpleados.Find(DocId)
                    Dim documentPath = record.Ruta
                    Dim handlerUrl As String = $"/Handlers/DownloadHandler.ashx?path={HttpUtility.UrlEncode(documentPath)}&name={HttpUtility.UrlEncode(documentName)}"
                    Response.Redirect(handlerUrl)
                End Using

            Catch ex As IOException
                Dim msg = "Error al procesar  archivo: " & ex.Message
                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            Catch ex As Exception
                Dim msg = "Error de descarga: " & ex.Message
                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            End Try

        End If
        If e.CommandName = "RestoreFile" Then
            Try
                Using dbContext As New MyDbContext
                    Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
                    Dim DocId As Integer = MyGridView.DataKeys(rowIndex).Value.ToString()
                    Dim recordToUpdate As DocumentoDeEmpleado = dbContext.DocumentosDeEmpleados.Find(DocId)
                    recordToUpdate.Archivado = False
                    dbContext.SaveChanges()
                    BindGridView()

                    RaiseEvent AlertGenerated(Me, New AlertEventArgs("Archvo restaurado", "success"))

                End Using
            Catch ex As SqlException
                Dim msg As String = "Error de base de datos: " & vbCrLf & ex.Message
                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))

            Catch ex As Exception
                Dim msg As String = "Error inesperado: " & vbCrLf & ex.Message
                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            End Try


        End If

    End Sub
    Protected Sub MyGridView_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs) Handles MyGridView.RowDeleting

        Try
            Session("tabSelected") = "DocsTab"

            Dim documentId As Integer = Convert.ToInt32(MyGridView.DataKeys(e.RowIndex).Value)
            Using dbcontext As New MyDbContext()
                Dim document As DocumentoDeEmpleado = dbcontext.DocumentosDeEmpleados.SingleOrDefault(Function(d) d.Id = documentId)
                Dim creationDate As DateTime = document.FechaDeCreacion
                'Dim relativePath As String = 

                Dim utcTime As DateTime = DateTime.UtcNow
                Dim targetTimeZoneOffset As Integer = -6
                Dim localTime As DateTime = utcTime.AddHours(targetTimeZoneOffset)
                Dim time24HoursLater As DateTime = creationDate.AddHours(24)
                Dim filePath As String = document.Ruta
                If utcTime > time24HoursLater Then
                    Try
                        document.Archivado = True
                        dbcontext.SaveChanges()
                        RaiseEvent AlertGenerated(Me, New AlertEventArgs("Documento archivado correctamente", "success"))

                        BindGridView()

                    Catch ex As Exception
                        RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error al archivar: " & ex.Message, "danger"))

                    End Try

                Else
                    Dim fileIsDeleted = FileHelper.DeleteFile(filePath)

                    If fileIsDeleted Then
                        If DeleteRecordFromDatabase(document.Id) Then
                            RaiseEvent AlertGenerated(Me, New AlertEventArgs("Archivo eliminado correctamente", "success"))
                            BindGridView()
                        Else
                            RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error al eliminar archivo", "danger"))
                        End If


                    Else
                        RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error al eliminar archivo", "danger"))
                    End If

                End If

            End Using




        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error de borrado o archivado: " & ex.Message, "danger"))
        End Try


    End Sub
    Private Function DeleteRecordFromDatabase(documentId As String)
        Try
            Using dbContext As New MyDbContext()
                Dim recordsToDelete = dbContext.DocumentosDeEmpleados.Where(Function(f) f.Id = documentId)
                dbContext.DocumentosDeEmpleados.RemoveRange(recordsToDelete)
                dbContext.SaveChanges()
                Return True
            End Using
        Catch ex As SqlException
            Dim msg As String = "Error: Database error occurred." & vbCrLf & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error al eliminar registro de la base de datos, por favor vuelva a intentarlo : " & ex.Message, "danger"))
        End Try
        Return False
    End Function


    Private Sub BindGridView()


        Try

            If Session("Codigo_Empleado") Then
                numeroDeEmpleado = Session("Codigo_Empleado")
                Dim selectedValue As String = ddlAreArchived.SelectedValue
                Dim isArchived As Boolean = False
                If selectedValue = "True" Then
                    isArchived = True
                End If
                Dim dataList As List(Of DocumentoDeEmpleado) = GetEmployeeDocs(isArchived, numeroDeEmpleado)

                MyGridView.DataSource = dataList
                MyGridView.DataBind()

                If MyGridView.Rows.Count = 0 Then
                    lblMessage.Text = "No se encontraron documentos"
                Else
                    lblMessage.Text = "Archivos encontrados: " & $"{dataList.Count}"
                End If

            Else

            End If

        Catch ex As Exception
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
        End Try



    End Sub
    Protected Sub UpdateButtonVisibility(eraArchived As Boolean)



    End Sub
    Protected Function GetEmployeeDocs(areArechived As Boolean, employeeId As Integer) As List(Of DocumentoDeEmpleado)
        Using dbContext As New MyDbContext

            Dim data = dbContext.DocumentosDeEmpleados.Where(Function(d) d.NumeroDeEmpleado = employeeId And d.Archivado = areArechived)
            Return data.ToList()
        End Using
    End Function

    Protected Sub UploadFileButton_Click(sender As Object, e As EventArgs) Handles UploadFile.Click
        Session("tabSelected") = "DocsTab"

        Try

            If Not File1.PostedFile Is Nothing And Page.IsValid Then
                Dim fileName As String = File1.PostedFile.FileName
                Dim fileSize As Long = File1.PostedFile.ContentLength
                Dim numeroDeEmpleado As String
                Dim directoryRelativePath As String = "Musica\Empleados\Documentos\"
                Dim directoryAbsolutePath As String = Path.Combine(ServerPath, directoryRelativePath)

                If Not Directory.Exists(directoryAbsolutePath) Then
                    Directory.CreateDirectory(directoryAbsolutePath)
                End If

                If File1.PostedFile.ContentLength < 1 Then
                    RaiseEvent AlertGenerated(Me, New AlertEventArgs("Seleccione un archivo por favor.", "danger"))
                    Exit Sub
                End If

                If Not FileHelper.LessThanFileSizeLimit(fileSize, 10485760) Then
                    Dim msg As String = "El archivo no puede tener un tamaño mayor a 10MB"
                    Dim alertType As String = "danger"
                    RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))
                    Exit Sub
                End If

                If Not FileHelper.ValidateFileExtension(fileName, {".doc", ".docx", ".pdf", ".xls", ".xlsx", ".png", ".jpg", ".jpeg", ".odt", ".zip", ".rar", ".7z", ".mp4"}) Then
                    Dim msg As String = "Solo se admiten documentos, archivos comprimidos, fotos o videos cortos"
                    Dim alertType As String = "danger"
                    RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, alertType))
                    Exit Sub
                End If

                If TextBoxDescription.Text.Length < 1 Then
                    RaiseEvent AlertGenerated(Me, New AlertEventArgs("La descripción es obligatoria.", "danger"))
                    Exit Sub
                End If

                If Session("Codigo_Empleado") IsNot Nothing Then

                    Try
                        numeroDeEmpleado = Session("Codigo_Empleado")
                        fileName = numeroDeEmpleado & "_" & DateString & "_" & fileName
                        Dim fileAbsolutePath As String = directoryAbsolutePath & fileName
                        'Dim fileRelativePath As String = directoryRelativePath & fileName
                        Dim description As String
                        description = TextBoxDescription.Text
                        Dim utcTime As DateTime = DateTime.UtcNow
                        Dim targetTimeZoneOffset As Integer = -6
                        Dim localTime As DateTime = utcTime.AddHours(targetTimeZoneOffset)
                        If FileHelper.CheckFileExists(fileAbsolutePath) Then
                            RaiseEvent AlertGenerated(Me, New AlertEventArgs("Ya existe un archivo con ese nombre", "danger"))
                            Exit Sub
                        Else
                            File1.PostedFile.SaveAs(fileAbsolutePath)
                            Using dbContext As New MyDbContext
                                Dim newDoc As New DocumentoDeEmpleado With
                                    {
                                    .NumeroDeEmpleado = numeroDeEmpleado,
                                    .NombreDelArchivo = fileName,
                                    .Ruta = fileAbsolutePath,
                                    .Descripcion = description,
                                    .FechaDeCreacion = localTime,
                                    .Archivado = False}
                                dbContext.DocumentosDeEmpleados.Add(newDoc)
                                dbContext.SaveChanges()
                            End Using
                            Dim msg As String = "Carga exitosa"
                            Dim alertType As String = "success"
                            TextBoxDescription.Text = ""
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
                RaiseEvent AlertGenerated(Me, New AlertEventArgs("Por favor seleccione un archivo para subir.", "danger"))
            End If


        Catch ex As Exception
            Dim msg = "Error" & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
        End Try

    End Sub

    Protected Sub MyGridView_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles MyGridView.RowDataBound

        Try

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim sizeColumnIndex As Integer = 2
                Dim fileName As String = e.Row.Cells(sizeColumnIndex).Text
                Dim fileExtension As String = Path.GetExtension(fileName)
                Dim iconCell As TableCell = e.Row.Cells(3)
                Dim nameCell As TableCell = e.Row.Cells(2)
                Dim imageClass = "bi bi-image"
                Dim btnDownload As LinkButton = CType(e.Row.FindControl("btnDownload"), LinkButton)
                Dim btnRestore As LinkButton = CType(e.Row.FindControl("btnRestore"), LinkButton)
                Dim lnkDelete As LinkButton = CType(e.Row.FindControl("lnkDelete"), LinkButton)

                If btnDownload IsNot Nothing AndAlso btnRestore IsNot Nothing AndAlso lnkDelete IsNot Nothing Then
                    If ddlAreArchived.SelectedValue = "True" Then
                        btnDownload.Visible = False
                        btnRestore.Visible = True
                        lnkDelete.Visible = False
                    Else
                        btnDownload.Visible = True
                        btnRestore.Visible = False
                        lnkDelete.Visible = True
                    End If
                End If
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

                If fileName.Length > 32 Then
                    fileName = fileName.Substring(Math.Max(fileName.Length - 32, 0))
                End If

                nameCell.Text = "..." & fileName
            End If

        Catch ex As Exception
            Dim msg = "Error inesperado : " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
        End Try

    End Sub
    Protected Sub ddlDocsState_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAreArchived.SelectedIndexChanged
        Session("tabSelected") = "DocsTab"

        BindGridView()
    End Sub

    Protected Sub MyGridView_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles MyGridView.PageIndexChanging
        MyGridView.PageIndex = e.NewPageIndex
        Session("tabSelected") = "DocsTab"

    End Sub
    Public Event AlertGenerated As EventHandler(Of AlertEventArgs)

End Class
