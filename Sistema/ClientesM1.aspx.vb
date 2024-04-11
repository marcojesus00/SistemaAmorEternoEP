Imports System.IO

Public Class ClientesM1
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut As String
    Private Datos, Datos1, Datos2, DatosCR As DataSet
    Private Liquida, Liquida2 As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("Usuario") = "" Or Session("Destino") <> "ClientesM1.aspx" Then
            Response.Redirect("inicio.aspx")
        End If

        Usuario = Session("Usuario")
        Clave = Session("Clave")
        Servidor = Session("Servidor")
        Bd = Session("Bd")
        Usuario_Aut = Session("Usuario_Aut")
        Clave_Aut = Session("Clave_Aut")

        If Not IsPostBack Then
            Session.Timeout = 90
            Session.Add("Orden", "0")
            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim SQL As String


            txtCliente.Text = Session("CodigoCliente")
            LlenarDatos()
            ObtenerListaDeImagenes()

            Dim dtImagenes As DataTable = Session("DatosImg")

            ' Dim listaImagenes As List(Of Imagen) = ObtenerListaDeImagenes() ' Reemplaza con tu lógica de obtención de datos

            ' Enlazar la lista al control Repeater
            rptPDFList.DataSource = dtImagenes
            rptPDFList.DataBind()

        End If

    End Sub

    Public Class Imagen
        Public Property RutaFile As String
        Public Property Comentario As String

    End Class
    Sub ObtenerListaDeImagenes()
        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim SQL As String

        SQL = " exec FUNAMOR..SP_VS_BuscarCorrelativoArchivoCliente '" + Session("CodigoCliente") + "'"


        Datos = conf.EjecutaSql(SQL)
        Session.Add("DatosImg", Datos.Tables(0))
    End Sub
    Protected Sub btnSalir_Click(sender As Object, e As ImageClickEventArgs) Handles btnSalir.Click
        Response.Redirect("monitorclientes.aspx")

    End Sub



    Sub LlenarDatos()
        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim SQL As String

        SQL = "Select c.codigo_clie CodigoCliente,c.nombre_clie Nombre, co.cont_valor, co.cont_descri Producto1, 
                co.cont_canti Cantidad1, co.cont_valcuo ValorCuota, co.cont_numcuo NumeroCuotas from 
                FUNAMOR..CLIENTES C
                inner join FUNAMOR..CONTRATO CO on co.codigo_clie = c.codigo_clie 
                where c.codigo_clie = '" + txtCliente.Text.TrimEnd + "'"
        Datos = conf.EjecutaSql(SQL)

        'If Datos.Tables(0).Rows.Count > 0 Then

        'End If

    End Sub



    Protected Sub btnGuardarFile_Click(sender As Object, e As EventArgs) Handles btnGuardarFile.Click
        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim SQL As String, SqlInsert As String

        SQL = "if not exists(select  convert(varchar(2), count(*))  Correlativo from FUNAMOR..urlclientes 
                where codigo_clie =  '" + txtCliente.Text.TrimEnd + "'  )begin select '1' Correlativo
				end else begin 
				select convert(varchar(2), count(*)) Correlativo from FUNAMOR..urlclientes 
                where codigo_clie = '" + txtCliente.Text.TrimEnd + "' 
				end "
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Correlativo", Datos.Tables(0).Rows(0).Item("Correlativo"))

        If FileUpload.HasFile Then
            Try
                ' Obtener el nombre del archivo
                Dim fileName As String = Path.GetFileName(FileUpload.FileName)

                ' Obtener la ruta donde se guardará el archivo 
                Dim filePath As String = "\\192.168.20.226\Musica"

                ' Combinar la ruta y el nombre del archivo

                Dim fullPath As String = Path.Combine(filePath, Session("CodigoCliente") + "-" + Session("Correlativo") + "-" + fileName)

                Dim FileCorrelativo As String = Path.Combine(Session("CodigoCliente") + "-" + Session("Correlativo") + "-" + fileName)

                ' Guardar el archivo en la ubicación deseada
                FileUpload.SaveAs(fullPath)

                ' Mostrar un mensaje de éxito
                lblMensaje.Text = "Archivo guardado con éxito."

                SqlInsert = " exec FUNAMOR..SP_VS_GuardarImagenRuta '" +
                    Session("CodigoCliente") + "','" + System.DateTime.Now.ToShortTimeString + "','" + TxtComentario.Text.Trim.TrimEnd + "','" + fullPath + "','" + Session("Usuario_Aut") + "','1', '" + fileName + "','" + filePath + "','" + FileCorrelativo + "'"

                Datos1 = conf1.EjecutaSql(SqlInsert)
                TxtComentario.Text = ""
                '
                ObtenerListaDeImagenes()
                Dim dtImagenes As DataTable = Session("DatosImg")

                ' Dim listaImagenes As List(Of Imagen) = ObtenerListaDeImagenes() ' Reemplaza con tu lógica de obtención de datos

                ' Enlazar la lista al control Repeater
                rptPDFList.DataSource = dtImagenes
                rptPDFList.DataBind()

            Catch ex As Exception
                ' Manejar cualquier error que pueda ocurrir durante la operación de guardado
                lblMensaje.Text = "Error al guardar el archivo: " & ex.Message
            End Try
        Else
            lblMensaje.Text = "Por favor, seleccione un archivo para guardar."
        End If

    End Sub

    Private Sub rptPDFList_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rptPDFList.ItemCommand
        Dim ooo As String
        ''If e.CommandName = "Abrir" Then
        Dim rutaDelPDF As String = e.CommandArgument.ToString()
        ' Aquí puedes usar la variable 'rutaDelPDF' para descargar el PDF o realizar alguna otra acción relacionada con el elemento.
        ''End If
        ooo = rutaDelPDF
    End Sub

    'Protected Sub btnEliminar_Click(sender As Object, e As CommandEventArgs)
    '    If e.CommandName = "EliminarItem" Then
    '        Dim idDelItem As String = e.CommandArgument.ToString()
    '        ' Ahora tienes el ID del elemento que se desea eliminar, puedes usarlo para eliminar ese elemento específico.
    '        ' EliminarElemento(idDelItem)
    '    End If
    'End Sub


End Class





