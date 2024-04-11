Imports System.Data.SqlClient
Imports System.Data

Public Class inicioOld1
    Inherits System.Web.UI.Page
    Private er As Boolean
    Private sql, Usuario, clave, Servidor, bd As String
    Private Datos As New Data.DataSet

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
    End Sub

    Private Sub Button_Click(sender As Object, e As EventArgs) Handles Button.Click

        Servidor = "192.168.20.225"
        'bd = dlCompania.SelectedValue
        Usuario = "sistema.web"
        clave = "$$Eterno4321."

        Dim conector As New SqlConnection("Server=" + Servidor + "; Database=" + bd + "; UID=" + Usuario + "; PWD=" + clave)
        Try
            conector.Open()
            If conector.State <> ConnectionState.Open Then
                Msg("No se pudo conectar. - ")
                Exit Sub
            End If

        Catch ex As SqlException

            For i As Integer = 0 To ex.Errors.Count - 1
                If ex.Errors(i).Number = 2 Or ex.Errors(i).Number = 53 Then
                    Msg("El Servidor " + Servidor + " no existe")
                    limpiar_campos()
                ElseIf ex.Errors(i).Number = 4060 Then
                    Msg("La Base de Datos " + bd + " no existe")
                    limpiar_campos()
                ElseIf ex.Errors(i).Number = 18456 Then
                    Msg("Usuario o Clave de sql incorrectos")
                    limpiar_campos()
                Else
                    Msg(ex.Errors(i).Message & " Error- " & ex.Errors(i).Number)
                    limpiar_campos()
                End If
            Next
            Exit Sub

        End Try

        If conector.State = ConnectionState.Open Then
            conector.Close()
        End If

        Dim conf As New Configuracion(Usuario, clave, bd, Servidor)
        sql = "SELECT A.CODIGO_SUCU FROM FUNAMOR..CABSEG A WHERE A.SEG_USUARIO = '" & txtUsuario.Text & "' AND A.SEG_PASSWD = '" + txtPassword.Text + "'"
        Datos = conf.EjecutaSql(sql)

        If Datos.Tables(0).Rows.Count.ToString = "0" Then
            Msg("Usuario o Clave incorrecto ")
            Exit Sub
        End If

        Session.Timeout = 90
        Session.Add("Usuario_Aut", txtUsuario.Text)
        Session.Add("Clave_Aut", txtPassword.Text)
        Session.Add("Usuario", Usuario)
        Session.Add("Clave", clave)
        Session.Add("Bd", bd)
        Session.Add("Servidor", Servidor)
        Session.Add("Sucursal", Datos.Tables(0).Rows(0).Item("CODIGO_SUCU"))

        Response.Redirect("principal.aspx")
    End Sub

    Sub limpiar_campos()
        txtUsuario.Text = ""
        txtPassword.Text = ""
    End Sub

    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub
End Class
