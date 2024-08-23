Imports CrystalDecisions.Shared
Public Class Reportes

    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Reporte As String
    Private Datos, Datos1 As DataSet
    Public Informe As New CrystalDecisions.CrystalReports.Engine.ReportDocument

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario") = "" Then
            Response.Redirect("inicio.aspx")
        End If

        Informe.Close()
        Informe.Dispose()

        lblMsg.Text = ""

        Reporte = Session("Reporte")
        Usuario = Session("Usuario")
        Clave = Session("Clave")
        Servidor = Session("Servidor")
        Bd = Session("Bd")

        If Reporte = "Recibos Nulos" Then
            Label3.Text = "Nombre Lider"
        End If

        If Reporte = "Ventas" Or Reporte = "Estadistica Cobradores" Or Reporte = "Ventas_Rendimiento" Then
            'txtCobrador.Visible = False
            Label3.Text = "Lider"
        End If

        If Reporte = "Recibos Nulos" Then
            txtlider.Visible = True
            Label4.Visible = True
        End If

        If Reporte = "Ventas_Resindida" Then
            txtCobrador.Visible = False
            Label3.Visible = False
            txtFecha2.Visible = False
            Label2.Visible = False
        End If

        If Reporte = "Estadistica de Visitas" Then
            txtCobrador.Visible = False
            Label3.Visible = False

            If (Not Page.IsPostBack) Then
                Label5.Visible = True
                dlAgrupa.Visible = True
                dlAgrupa.Items.Add("Lider")
                dlAgrupa.Items.Add("Zona")
                dlAgrupa.Items.Add("Cobrador")
                dlAgrupa.SelectedIndex = 0
            End If
        End If

    End Sub

    Private Sub btnEjecutar_Click(sender As Object, e As EventArgs) Handles btnEjecutar.Click
        If Reporte = "Estadistica de Visitas" Then
            If dlAgrupa.SelectedIndex = 0 Then
                pnlDialog.Visible = False

                Estadistica_Visitas()
            End If

            If dlAgrupa.SelectedIndex = 1 Then
                pnlDialog.Visible = False

                Estadistica_Visitas_Zona()
            End If
            If dlAgrupa.SelectedIndex = 2 Then
                Estadistica_Visitas_Cobrador()
            End If

        End If
        If Reporte = "Clientes Sin Visitas" Then
            Clientes_Sin_Visitas()
        End If

        If Reporte = "Clientes Sin Cobro" Then
            Clientes_Sin_Cobro()
        End If

        If Reporte = "No cobro Visitas" Then
            No_cobro_Visitas()
        End If

        If Reporte = "Recibos Nulos" Then
            Recibos_Nulos()
        End If

        If Reporte = "No Estaba" Then
            No_Cobro()
        End If

        If Reporte = "Ventas" Then
            Ventas()
        End If

        If Reporte = "Ventas_Rendimiento" Then
            VentasRendLider()
        End If

        If Reporte = "Ventas_Resindida" Then
            VentasRensindidas()
        End If


    End Sub

    Private Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Response.Redirect("principal.aspx")
    End Sub

    'Funcion de Reporte para enviar todos los DataSet
    'Nueva Funcion Agregada 
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

    Sub ReporteCR(Informe As CrystalDecisions.CrystalReports.Engine.ReportDocument, Nombre_Archivo As String)
        Informe.SetDatabaseLogon(Usuario, Clave)
        Informe.SetParameterValue("F1", txtFecha1.Text)
        Informe.SetParameterValue("F2", txtFecha2.Text)

        Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
        Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
        exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        exportOpts.ExportFormatOptions = pdfOpts
        Informe.ExportToHttpResponse(exportOpts, Response, False, Nombre_Archivo)
        Informe.Close()
        Informe.Dispose()

    End Sub

    Sub Estadistica_Visitas()
        Informe = New Estadistica_Visitas

        Informe.SetDatabaseLogon(Usuario, Clave)
        Informe.SetParameterValue("F1", txtFecha1.Text)
        Informe.SetParameterValue("F2", txtFecha2.Text)

        Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
        Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
        exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        exportOpts.ExportFormatOptions = pdfOpts
        Informe.ExportToHttpResponse(exportOpts, Response, False, "Estadistica_Visitas")
        Informe.Close()
        Informe.Dispose()
    End Sub
    Sub Estadistica_Visitas_Cobrador()
        pnlDialog.Visible = True


    End Sub
    Sub Estadistica_Visitas_Zona()
        Informe = New Estadistica_Visitas_Zona

        Informe.SetDatabaseLogon(Usuario, Clave)
        Informe.SetParameterValue("F1", txtFecha1.Text)
        Informe.SetParameterValue("F2", txtFecha2.Text)

        Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
        Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
        exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        exportOpts.ExportFormatOptions = pdfOpts
        Informe.ExportToHttpResponse(exportOpts, Response, False, "Estadistica_Visitas")
        Informe.Close()
        Informe.Dispose()
    End Sub

    Sub Clientes_Sin_Visitas()
        Informe = New Clientes_Sin_Visitas

        Informe.SetDatabaseLogon(Usuario, Clave)
        Informe.SetParameterValue("F1", txtFecha1.Text)
        Informe.SetParameterValue("F2", txtFecha2.Text)
        Informe.SetParameterValue("Cobr", txtCobrador.Text)

        Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
        Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
        exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        exportOpts.ExportFormatOptions = pdfOpts
        Informe.ExportToHttpResponse(exportOpts, Response, False, "Clientes_Sin_Visitas")
        Informe.Close()
        Informe.Dispose()
    End Sub

    Sub Clientes_Sin_Cobro()
        Informe = New Clientes_Sin_Cobro

        Informe.SetDatabaseLogon(Usuario, Clave)
        Informe.SetParameterValue("F1", txtFecha1.Text)
        Informe.SetParameterValue("F2", txtFecha2.Text)
        Informe.SetParameterValue("Cobr", txtCobrador.Text)

        Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
        Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
        exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        exportOpts.ExportFormatOptions = pdfOpts
        Informe.ExportToHttpResponse(exportOpts, Response, False, "Clientes_Sin_Cobro")
        Informe.Close()
        Informe.Dispose()
    End Sub

    Sub No_cobro_Visitas()
        Informe = New No_Cobro_Visitas

        Informe.SetDatabaseLogon(Usuario, Clave)
        Informe.SetParameterValue("F1", txtFecha1.Text)
        Informe.SetParameterValue("F2", txtFecha2.Text)
        Informe.SetParameterValue("Cobrador", txtCobrador.Text)

        Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
        Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
        exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        exportOpts.ExportFormatOptions = pdfOpts
        Informe.ExportToHttpResponse(exportOpts, Response, False, "No_cobro_Visitas")
        Informe.Close()
        Informe.Dispose()
    End Sub

    Sub Recibos_Nulos()
        Informe = New Recibos_Nulos

        Informe.SetDatabaseLogon(Usuario, Clave)
        Informe.SetParameterValue("F1", txtFecha1.Text)
        Informe.SetParameterValue("F2", txtFecha2.Text)
        Informe.SetParameterValue("Lider", "'" + txtCobrador.Text + "'")
        Informe.SetParameterValue("Cobr", "'" + txtlider.Text + "'")

        Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
        Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
        exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        exportOpts.ExportFormatOptions = pdfOpts
        Informe.ExportToHttpResponse(exportOpts, Response, False, "Recibos_Nulos")
        Informe.Close()
        Informe.Dispose()
    End Sub

    Sub No_Cobro()
        Informe = New NoCobro

        Informe.SetDatabaseLogon(Usuario, Clave)
        Informe.SetParameterValue("F1", txtFecha1.Text)
        Informe.SetParameterValue("F2", txtFecha2.Text)
        Informe.SetParameterValue("Cobrador", txtCobrador.Text)

        Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
        Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
        exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        exportOpts.ExportFormatOptions = pdfOpts
        Informe.ExportToHttpResponse(exportOpts, Response, False, "NO_Estaba")
        Informe.Close()
        Informe.Dispose()
    End Sub

    Sub Ventas()
        Informe = New ventas

        Informe.SetDatabaseLogon(Usuario, Clave)
        Informe.SetParameterValue("F1", txtFecha1.Text)
        Informe.SetParameterValue("F2", txtFecha2.Text)

        Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
        Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
        exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        exportOpts.ExportFormatOptions = pdfOpts
        Informe.ExportToHttpResponse(exportOpts, Response, False, "Ventas")
        Informe.Close()
        Informe.Dispose()
    End Sub

    Sub VentasRendLider()
        Dim conf1 As New Configuracion(Usuario, Clave, "AEVENTAS", Servidor)
        Dim SQL As String

        If txtCobrador.Text.Length > 0 Then
            SQL = " EXEC CR_COB_REND '" + txtFecha1.Text + "', '" + txtFecha2.Text + "', '" + txtCobrador.Text + "' "
            Datos = conf1.EjecutaSql(SQL)
        Else
            SQL = " CR_REND_LIDER '" + txtFecha1.Text + "', '" + txtFecha2.Text + "'"
            Datos = conf1.EjecutaSql(SQL)
        End If

        If Datos.Tables(0).Rows.Count < 0 Then
            MsgBox("NO hay registros")
            Exit Sub
        End If

        Dim reporte As New Lider_Rend
        reporte.SetParameterValue(0, txtFecha1.Text)
        reporte.SetParameterValue(1, txtFecha1.Text)
        reporte.SetDataSource(Datos.Tables(0))

        Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
        Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
        exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        exportOpts.ExportFormatOptions = pdfOpts
        reporte.ExportToHttpResponse(exportOpts, Response, False, "VentasRendLider")
        reporte.Close()
        reporte.Dispose()
    End Sub

    Protected Sub dlAgrupa_TextChanged(sender As Object, e As EventArgs)
        If dlAgrupa.SelectedIndex <> 2 Then
            pnlDialog.Visible = False


        End If
    End Sub

    Sub VentasRensindidas()
        Dim conf1 As New Configuracion(Usuario, Clave, "FUNAMOR", Servidor)
        Dim SQL As String
        SQL = " EXEC SP_CR_VENTAS_P_REACTIV '" + txtFecha1.Text + "'"
        Datos = conf1.EjecutaSql(SQL)

        If Datos.Tables(0).Rows.Count < 0 Then
            MsgBox("NO hay registros")
            Exit Sub
        End If

        Dim reporte As New VentasResindidas
        reporte.SetDataSource(Datos.Tables(0))

        Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
        Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
        exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        exportOpts.ExportFormatOptions = pdfOpts
        reporte.ExportToHttpResponse(exportOpts, Response, False, "VentasResindidas")
        reporte.Close()
        reporte.Dispose()
    End Sub


    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = " < Script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub

    Private Sub reportes_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        Informe.Close()
        Informe.Dispose()
    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs)
        lblError.ForeColor = Drawing.Color.Red
        lblError.Text = ""
        Dim userInput As String = txtCodigoCObrador.Text
        Dim fechaInicial As String = txtFecha1.Text
        Dim fechaFinal As String = txtFecha2.Text
        Dim convertedDate As DateTime

        Dim esValidaFechaInicial As Boolean = DateTime.TryParse(fechaInicial, convertedDate)
        Dim esValidaFechaFinal As Boolean = DateTime.TryParse(fechaFinal, convertedDate)


        If userInput.Trim().Length < 4 OrElse userInput.Trim().Length > 4 Then
            lblError.Text = "Ingrese un código de 4 dígitos"
            Exit Sub
        End If
        If Not esValidaFechaInicial Then
            lblError.Text = "Ingrese fechas válidas"

            Exit Sub
        End If
        If Not esValidaFechaFinal Then
            lblError.Text = "Ingrese fechas válidas"

            Exit Sub

        End If
        Informe = New Estadistica_Visitas_Cobrador

        Informe.SetDatabaseLogon(Usuario, Clave)
        Informe.SetParameterValue("F1", txtFecha1.Text)
        Informe.SetParameterValue("F2", txtFecha2.Text)
        Informe.SetParameterValue("COBRADOR", userInput.Trim())


        Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
        Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
        exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        exportOpts.ExportFormatOptions = pdfOpts
        Informe.ExportToHttpResponse(exportOpts, Response, False, $"Estadistica_Visitas__{userInput}__{fechaInicial}_{fechaFinal}")
        Informe.Close()
        Informe.Dispose()
        pnlDialog.Visible = False
    End Sub
    Protected Sub btnCancel_Click()
        pnlDialog.Visible = False

    End Sub


End Class