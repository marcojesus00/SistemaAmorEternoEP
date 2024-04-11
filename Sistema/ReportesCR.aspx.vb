Imports CrystalDecisions.Shared
Public Class ReportesCR
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, ReporteCR As String
    Public fecha1, fecha2 As Date
    Private Datos, Datos1 As DataSet
    Public Informe As New CrystalDecisions.CrystalReports.Engine.ReportDocument

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Session("Usuario") = "" Then
            Response.Redirect("inicio.aspx")
        End If

        Informe.Close()
        Informe.Dispose()

        lblMsg.Text = ""

        ReporteCR = Session("ReporteCR")
        Usuario = Session("Usuario")
        Clave = Session("Clave")
        Servidor = Session("Servidor")
        Bd = Session("Bd")
        Datos = Session("DatosCR")

        Dim fecha1 = Session("Fecha1I")
        Dim Fecha2 = Session("Fecha2I")


        If Session("ReporteCR") = "FirmaClientes" Then
            Informe = New FirmaClientes
            Datos = Session("DATOSCR")
            ReporteCrystal(Informe, "FirmaClientes", Datos)
        End If

        'If Session("ReporteCR") = "DiasLaborados" Then
        '    Informe = New DiasLaborados
        '    Datos = Session("DatosCR")
        '    ReporteCrystal(Informe, "DiasLaborados", Datos)
        'End If

        'reportes de Dias de Cobradores
        If Session("ReporteCR") = "DiasLaborados" And Session("Valor") = "Resumido" Then
            Informe = New DiasLaboradosResumido
            Datos = Session("DatosCR")
            ReporteCrystal(Informe, "DiasLaboradosResumido", Datos)

        ElseIf Session("ReporteCR") = "DiasLaborados" And Session("Valor") = "" Then
            Informe = New DiasLaborados
            Datos = Session("DatosCR")
            ReporteCrystal(Informe, "DiasLaborados", Datos)
        End If

        If Session("ReporteCR") = "Rendimiento" Then
            Informe = New Lider_RendMod
            Datos = Session("DatosCR")
            ReporteCrystal(Informe, "Lider_RendMod", Datos)
        End If

        If Session("ReporteCR") = "EntregasPrg" Then
            Informe = New EntregasPrg
            Datos = Session("DatosCR")
            ReporteCrystal(Informe, "EntregasPrg", Datos)
        End If

        'Reporte Cartera Cobador Clientes
        If Session("ReporteCR") = "CarteraCobradorCL" Then
            Informe = New DetalleCarteraCobrador
            Datos = Session("DatosCR")
            ReporteCrystal(Informe, "DetalleCarteraCobrador", Datos)
        End If

        'Reporte de Notas de Duelo
        If Session("ReporteCR") = "RptNotaDuelo" Then
            Informe = New RptNotaDuelo
            ReporteCrystal(Informe, "RptNotaDuelo", Session("DatosCR"))
        End If


        'Reporte de movimiento de clientes visitas.
        If Session("ReporteCR") = "RptMovClienteM" Then
            Informe = New Movimiento_ClientesVisitas
            Datos = Session("DatosCR")
            ReporteCrystal(Informe, "Movimiento_ClientesVisitas", Datos)
        End If


        'Reporte de Ventas Por Fecha.
        If Session("ReporteCR") = "RptVentasXFecha" Then
            Informe = New RPTVentasPorFechaX
            Datos = Session("DatosCR")
            ReporteCrystal(Informe, "RPTVentasPorFechaX", Datos)
        End If

    End Sub

    Sub ReporteCrystal(Informe As CrystalDecisions.CrystalReports.Engine.ReportDocument, Nombre_Archivo As String, DatosCR As DataSet)
        Informe.SetDataSource(DatosCR.Tables(0))

        Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
        Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
        exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        exportOpts.ExportFormatOptions = pdfOpts
        Informe.ExportToHttpResponse(exportOpts, Response, False, Nombre_Archivo)
        Informe.Close()
        Informe.Dispose()
    End Sub

    Sub ReporteCrystalTable(Informe As CrystalDecisions.CrystalReports.Engine.ReportDocument, Nombre_Archivo As String, DatosCR As DataTable)
        Informe.SetDataSource(DatosCR)

        Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
        Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
        exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        exportOpts.ExportFormatOptions = pdfOpts
        Informe.ExportToHttpResponse(exportOpts, Response, False, Nombre_Archivo)
        Informe.Close()
        Informe.Dispose()
    End Sub



End Class