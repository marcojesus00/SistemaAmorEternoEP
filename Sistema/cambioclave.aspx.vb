Public Class cambioclave
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut As String
    Private Datos As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario") = "" Then
            Response.Redirect("inicio.aspx")
        End If

        Usuario = Session("Usuario")
        Clave = Session("Clave")
        Servidor = Session("Servidor")
        Bd = Session("Bd")
        Usuario_Aut = Session("Usuario_Aut")
        Clave_Aut = Session("Clave_Aut")
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As ImageClickEventArgs) Handles btnSalir.Click
        Response.Redirect("principal.aspx")
    End Sub


    Private Sub btnCambiar_Click(sender As Object, e As EventArgs) Handles btnCambiar.Click
        If txtAnterior.Text <> Clave_Aut Then
            Msg("Contraseña Anterior incorrecta")
            Exit Sub
        End If

        If txtConfirmar.Text.Length > 10 Then
            Msg("Contraseña debe tener maximo 10 caracter")
            Exit Sub
        End If

        If txtNueva.Text = txtConfirmar.Text Then
            Dim conf As New Configuracion(Usuario, Clave, "FUNAMOR", Servidor)
            Dim Sql As String

            Sql = " UPDATE CABSEG SET SEG_PASSWD = '" + txtConfirmar.Text + "'"
            Sql += " WHERE SEG_USUARIO = '" + Usuario_Aut + "'"
            Datos = conf.EjecutaSql(Sql)

            Session.Clear()
            Response.Redirect("inicio.aspx")
        End If

        Msg("Las contraseñas no coinciden")
    End Sub

    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub

End Class