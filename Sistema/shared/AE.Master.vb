﻿Public Class AE
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub Salir_Click(sender As Object, e As EventArgs) Handles salir.Click
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

        'Session.Clear()
        Response.Redirect("inicio.aspx")
    End Sub
End Class