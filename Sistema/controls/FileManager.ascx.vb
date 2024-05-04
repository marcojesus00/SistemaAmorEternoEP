Imports System.IO

Public Class FileManager
    Inherits System.Web.UI.UserControl

    Dim queryDelete As String
    Dim queryRetrieve As String
    Dim numeroDeEmpleado As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
            If Not File1.PostedFile Is Nothing Then
                If File1.PostedFile.ContentLength > 0 Then
                    Dim Usuario = Session("Usuario")
                    Dim Clave = Session("Clave")
                    Dim Servidor = Session("Servidor")
                    Dim Bd = Session("Bd")
                    Dim fileName As String = File1.PostedFile.FileName
                    Dim fileSize As Long = File1.PostedFile.ContentLength
                    Dim conf As New Configuracion(Usuario, Clave, "FUNAMOR", Servidor)
                    Dim queryDocumentos As String
                    Dim numeroDeEmpleado As String
                    Dim relativePath As String = "Uploads/"
                    Dim path As String = Server.MapPath(relativePath)
                    Dim savePath As String = path & fileName
                    Dim completeRelativePath As String = relativePath & fileName
                    Dim descripcion As String
                    If Not Directory.Exists(path) Then
                        Directory.CreateDirectory(path)
                    End If
                    If fileSize > 21485760 Then ' 
                        lblUploadMessage.Text = "Error: El archivo no puede tener un tamaño mayor a 20MB."
                        Exit Sub
                    End If
                    If Session("Codigo_Empleado") IsNot Nothing Then
                        Try

                            numeroDeEmpleado = Session("Codigo_Empleado")
                            File1.PostedFile.SaveAs(savePath)
                            descripcion = "Contrato"
                            queryDocumentos = " INSERT INTO [dbo].[DocumentosDeEmpleado]
                    (
                        [NumeroDeEmpleado],
                        [NombreDelArchivo],
                        [Ruta],
                        [Descripcion]
                    )
                VALUES
                    ('" + numeroDeEmpleado + "',
                    '" + fileName + "',
                    '" + completeRelativePath + "',
                    '" + descripcion + "')"
                            Dim Datos = conf.EjecutaSql(queryDocumentos)
                            lblUploadMessage.Text = "¡Carga exitosa!"
                            BindGridView()

                        Catch ex As Exception
                            lblUploadMessage.Text = "Error al cargar archivos: " & ex.Message
                        End Try
                    Else
                        lblUploadMessage.Text = "Error: No se pudo identificar el empleado para subir archivos." ' Inform user about missing session variable
                    End If
                Else
                    lblUploadMessage.Text = "Por favor seleccione un archivo para subir."

                End If

            Else
                lblUploadMessage.Text = "Por favor seleccione un archivo para subir."
            End If

        Catch ex As Exception
            lblUploadMessage.Text = "Error" & ex.Message

        End Try
    End Sub

    Protected Sub FileExtensionManager()
        Dim fileName As String = "mydocument.docx"
        Dim fileExtension As String = Path.GetExtension(fileName)

        If fileExtension = ".docx" Then
            Console.WriteLine("The file is a Microsoft Word document.")
        ElseIf fileExtension = ".txt" Then
            Console.WriteLine("The file is a plain text file.")
            ' Add more conditions for other extensions
        End If
    End Sub

End Class