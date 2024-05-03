Imports System.IO

Public Class SubirDocumentos
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Protected Sub UploadDocumentsButton_Click(sender As Object, e As EventArgs) Handles UploadDocuments.Click
        If Not File1.PostedFile Is Nothing And File1.PostedFile.ContentLength > 0 Then
            ' Get file details
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
                lblMessage.Text = "Error: El archivo no puede tener un tamaño mayor a 20MB."
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
                    lblMessage.Text = "¡Carga exitosa!"

                Catch ex As Exception
                    lblMessage.Text = "Error al cargar archivos: " & ex.Message
                End Try
            Else
                lblMessage.Text = "Error: No se pudo identificar el empleado para subir archivos." ' Inform user about missing session variable
            End If
        Else
                lblMessage.Text = "Por favor seleccione un archivo para subir."
        End If
    End Sub
End Class