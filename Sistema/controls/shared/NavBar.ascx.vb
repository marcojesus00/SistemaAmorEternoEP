Public Class NavBar
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub LogOut_Click(sender As Object, e As EventArgs) Handles logOut.Click
        'Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String

        'SQL = " Insert into FUNAMOR..LogAccesoApp 
        '        (Usuario, Fecha,Hora,NombreReporte) 
        '         values
        '        ('" + Session("Usuario_Aut") + "',
        '        '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
        '        '" + Format(DateTime.Now, "HH:mm:ss") + "',
        '        '" + "Salir del Sistema" + "')"
        'Datos = conf.EjecutaSql(SQL)

        Session.Clear()
        Response.Redirect("~/inicio.aspx")
    End Sub
    Private Sub Back_Click(sender As Object, e As EventArgs) Handles back.Click


        Response.Redirect("~/principal.aspx")
    End Sub
End Class