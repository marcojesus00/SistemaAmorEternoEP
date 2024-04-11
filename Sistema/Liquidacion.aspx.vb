Public Class Liquidacion1
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut, Cobrador, Liquida2 As String
    Public Informe As New CrystalDecisions.CrystalReports.Engine.ReportDocument

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Usuario = Session("Usuario")
        Clave = Session("Clave")
        Servidor = Session("Servidor")
        Bd = Session("Bd")
        Usuario_Aut = Session("Usuario_Aut")
        Clave_Aut = Session("Clave_Aut")
        Cobrador = Session("Cobrador")
        Liquida2 = Session("Liquida2")

        Informe.Dispose()

        Session.Timeout = 90

        If Session("Usuario") = "" Then
            Response.Redirect("inicio.aspx")
        End If

        If Session("Reporte") = "Liquida2" Then
            Informe = New Liquidacion2
            Informe.SetDatabaseLogon(Usuario, Clave)
            Informe.SetParameterValue("Cobrador", "'" + Cobrador + "'")
            Informe.SetParameterValue("Usuario", "'" + Usuario_Aut + "'")
            Informe.SetParameterValue("Liquida2", "'" + Liquida2 + "'")

            Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
            Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
            exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
            exportOpts.ExportFormatOptions = pdfOpts
            Informe.ExportToHttpResponse(exportOpts, Response, False, "Liquidacion")
            Informe.Dispose()
            Informe.Close()
        End If

        If Session("Reporte") = "Liquida" Then
            Informe = New Liquidacion
            Informe.SetDatabaseLogon(Usuario, Clave)
            Informe.SetParameterValue("Cobrador", "'" + Cobrador + "'")
            Informe.SetParameterValue("Usuario", "'" + Usuario_Aut + "'")

            Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
            Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
            exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
            exportOpts.ExportFormatOptions = pdfOpts
            Informe.ExportToHttpResponse(exportOpts, Response, False, "Liquidacion")
            Informe.Dispose()
            Informe.Close()
        End If

        If Session("Reporte") = "VentasLiquida2" Then
            Informe = New VentasLiquidacion2
            Informe.SetDatabaseLogon(Usuario, Clave)
            Informe.SetParameterValue("Cobrador", "'" + Cobrador + "'")
            Informe.SetParameterValue("Usuario", "'" + Usuario_Aut + "'")
            Informe.SetParameterValue("Liquida2", "'" + Liquida2 + "'")

            Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
            Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
            exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
            exportOpts.ExportFormatOptions = pdfOpts
            Informe.ExportToHttpResponse(exportOpts, Response, False, "Liquidacion")
            Informe.Dispose()
            Informe.Close()
        End If

        If Session("Reporte") = "VentasLiquida" Then
            Informe = New VentasLiquidacion
            Informe.SetDatabaseLogon(Usuario, Clave)
            Informe.SetParameterValue("Cobrador", "'" + Cobrador + "'")
            Informe.SetParameterValue("Usuario", "'" + Usuario_Aut + "'")

            Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
            Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
            exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
            exportOpts.ExportFormatOptions = pdfOpts
            Informe.ExportToHttpResponse(exportOpts, Response, False, "Liquidacion")
            Informe.Dispose()
            Informe.Close()
        End If

    End Sub

End Class