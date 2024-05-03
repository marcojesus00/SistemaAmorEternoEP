Imports System.IO

Public Class VerDocumentos
    Inherits System.Web.UI.UserControl

    Dim queryDelete As String
    Dim queryRetrieve As String
    Dim numeroDeEmpleado As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            BindGridView()
            lblMessage.Text = "Cantidad de archivos para este usuario: " & $"{MyGridView.Rows.Count}"
        Catch ex As Exception
            lblMessage.Text = "Error al leer de la base de datos, por favor recargue la página : " & ex.Message
        End Try

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
        ''Dim id As Integer = Convert.ToInt32(MyGridView.DataKeys(e.RowIndex).Values("Id"))
        Dim rowIndex As Integer = e.RowIndex
        Dim id As String = MyGridView.DataKeys(rowIndex).Value.ToString() ' Assuming you have set DataKeyNames in GridView
        Dim dataSource As DataTable = DirectCast(MyGridView.DataSource, DataTable)
        'Dim id As Integer = Convert.ToInt32(dataSource.Rows(rowIndex).Item("Id"))

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



End Class