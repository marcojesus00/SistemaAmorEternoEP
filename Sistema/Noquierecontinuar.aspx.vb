Imports CrystalDecisions.Shared
Public Class Noquierecontinuar

    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Reporte As String
    Private Datos, Datos1 As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario") = "" Then
            Response.Redirect("inicio.aspx")
        End If

        lblMsg.Text = ""

        Reporte = Session("Reporte")
        Usuario = Session("Usuario")
        Clave = Session("Clave")
        Servidor = Session("Servidor")
        Bd = Session("Bd")

        If (Not Page.IsPostBack) Then
            Label5.Visible = True
            dlAgrupa.Visible = True
            dlAgrupa.Items.Add("Resumido")
            dlAgrupa.Items.Add("Detallado")
            dlAgrupa.SelectedIndex = 0
        End If

    End Sub

    Private Sub btnEjecutar_Click(sender As Object, e As EventArgs) Handles btnEjecutar.Click
        If dlAgrupa.SelectedIndex = 0 Then
            NoquiereContinuar1()
        End If

        If dlAgrupa.SelectedIndex = 1 Then
            NoquiereContinuar2()
        End If
    End Sub

    Private Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Response.Redirect("principal.aspx")
    End Sub

    Sub NoquiereContinuar1()
        Dim Informe As New NoQuiereContinuar1

        Informe.SetDatabaseLogon(Usuario, Clave)
        Informe.SetParameterValue("F1", txtFecha1.Text)
        Informe.SetParameterValue("F2", txtFecha2.Text)

        Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
        Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
        exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        exportOpts.ExportFormatOptions = pdfOpts
        Informe.ExportToHttpResponse(exportOpts, Response, False, "Resumido")
        Informe.Close()
        Informe.Dispose()
    End Sub

    Sub NoquiereContinuar2()
        Dim Informe As New NoQuiereContinuar2

        Informe.SetDatabaseLogon(Usuario, Clave)
        Informe.SetParameterValue("F1", txtFecha1.Text)
        Informe.SetParameterValue("F2", txtFecha2.Text)
        Informe.SetParameterValue("Cobrador", "'" + txtCobrador.Text + "'")

        Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
        Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
        exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        exportOpts.ExportFormatOptions = pdfOpts
        Informe.ExportToHttpResponse(exportOpts, Response, False, "Detallado")
        Informe.Close()
        Informe.Dispose()
    End Sub

    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = " < Script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub

End Class