Public Class NavBar
    Inherits System.Web.UI.UserControl
    Public Usuario, clave, Servidor, Bd, Usuario_Aut, Clave_Aut As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.Timeout = 90
        Usuario = Session("Usuario")
        clave = Session("Clave")
        Servidor = Session("Servidor")
        Bd = Session("Bd")
        Usuario_Aut = Session("Usuario_Aut")
        Clave_Aut = Session("Clave_Aut")


    End Sub
    Private Sub LogOut_Click(sender As Object, e As EventArgs) Handles logOut.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String

        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Salir del Sistema" + "')"
        Dim Datos = conf.EjecutaSql(SQL)

        Session.Clear()
        Response.Redirect("~/inicio.aspx")
    End Sub
    Private Sub Back_Click(sender As Object, e As EventArgs) Handles back.Click

        If Session("BackPageUrl") IsNot Nothing Then
            If Session("BackPageUrl").ToString().Length > 0 Then
                Response.Redirect(Session("BackPageUrl"))
            Else
                Response.Redirect("~/principal.aspx")

            End If
        Else
                Response.Redirect("~/principal.aspx")

        End If
    End Sub
End Class