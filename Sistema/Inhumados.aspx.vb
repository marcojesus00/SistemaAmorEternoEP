Public Class Inhumados

    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut As String
    Private Datos, Datos1, Datos2, DatosCR As DataSet
    Private Liquida, Liquida2, Sql As String
    Public Tabla As DataTable = New DataTable

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

        If Not IsPostBack Then
            'Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            'Sql = "SELECT SEG_ARCHIVO FROM DETSEG WHERE SEG_USUARIO = '" + Usuario_Aut + "'"
            'Datos = conf.EjecutaSql(Sql)

            'Tabla = Datos.Tables(0)
            'HABILITAR()
            llenarJardines()
        End If

    End Sub
    'Sub HABILITAR()

    '    Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'INH_DET'"
    '    If Tabla.DefaultView.Count > 0 Then
    '        CommandName.Visible = True
    '    Else
    '        detalleEstado.Visible = False
    '    End If

    'End Sub

    Sub llenarJardines()
        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String
        Sql = "Select JARD_CODIGO Codigo, rtrim(ltrim(JARD_NOMBRE)) Descripcion from FUNAMOR..JARDINES"

        Datos = conf.EjecutaSql(Sql)

        dlJardin.DataSource = Datos.Tables(0)
        dlJardin.DataValueField = "Codigo"
        dlJardin.DataTextField = "Descripcion"
        dlJardin.DataBind()

    End Sub
    Protected Sub btnSalir_Click(sender As Object, e As ImageClickEventArgs) Handles btnSalir.Click
        Response.Redirect("principal.aspx")
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Dim conf As New Configuracion(Usuario, Clave, "FUNAMOR", Servidor)
        Dim Sql As String

        If txtFecha1.ToString.Length > 0 Then
            Session.Add("Fecha1I", txtFecha1.Text)
        Else
            Session.Add("Fecha1I", "1990-01-01")
        End If

        If txtfecha2.Text.Length > 0 Then
            Session.Add("Fecha2I", txtfecha2.Text)
        Else
            Session.Add("Fecha2I", DateTime.Now.ToString("yyyy-MM-dd"))
        End If

        'Sql = "SELECT A.Codigo_clie Codigo,c.cont_numero Contrato ,A.Nombre_clie Nombre_Cliente,convert(date,c1.cont_dfecha) Fecha,ISNULL(A.Saldo_actua,0) Saldo,cont_dnombr [Persona Inhumado], isnull(C1.CONT_JARDIN,'-') Jardin,isnull(c1.CONT_LETRA,'-')+'-'+isnull(c1.CONT_NNUM,'-') Ubicacion "
        'Sql += " From CLIENTES A inner join CONTRATO C ON C.CODIGO_CLIE = A.CODIGO_CLIE "
        'Sql += " inner join CONTRAT1 c1 on c1.cont_numero = c.cont_numero "
        'Sql += " inner join SERVICIO ser on ser.serv_codigo = c.CONT_SERVI "
        'Sql += " Where convert(date,c1.cont_dfecha) BETWEEN '" + Session("Fecha1I") + "' and '" + Session("Fecha2I") + "' AND a.CODIGO_CLIE + c1.cont_dnombr + a.IDENTIDAD Like '%" + txtCliente.Text + "%' "
        'Sql += " and ser.serv_codigo in (01  ,02  ,03  ,04  ,05  ,06 ,100) and c1.cont_dnumer >= '01' and a.CODIGO_CLIE + c1.cont_dnombr + a.IDENTIDAD Like '%" + TxtCliente2.Text + "%'"
        'Sql += " ORDER BY A.Codigo_clie desc "


        Sql = " SELECT distinct A.Codigo_clie Codigo,c.cont_numero Contrato ,A.Nombre_clie Nombre_Cliente,convert(date,c1.cont_dfecha) Fecha,
		ISNULL(A.Saldo_actua,0) Saldo,cont_dnombr [Persona Inhumado], isnull(C1.CONT_JARDIN,'-') Jardin,isnull(c1.CONT_LETRA,'-')+'-'+isnull(c1.CONT_NNUM,'-') Ubicacion,  CONCAT('Linea ', ub.ubij_linea , '- Col ', ub.ubij_column) Localidad 		
        From CLIENTES A 
		inner join CONTRATO C ON C.CODIGO_CLIE = A.CODIGO_CLIE 
        inner join CONTRAT1 c1 on c1.CONT_NUMERO = c.CONT_NUMERO
        inner join ubijard ub on ub.ubij_letra = c1.cont_letra and ub.ubij_numero = c1.cont_nnum
        Where c1.cont_dfecha BETWEEN '" + Session("Fecha1I") + "' and '" + Session("Fecha2I") + "' 
		and isnull(a.IDENTIDAD,'') Like '%" + txtidentidad.Text + "%'  
		and c.CONT_DESCRI like '%lote%' and a.CODIGO_CLIE like '%" + txtcodigo.Text + "%'	
        and a.nombre_clie like '%" + TxtCliente2.Text + "%'	
        and a.nombre_clie like '%" + TxtCliente1.Text + "%'	
        and isnull(C1.CONT_JARDIN,'-') like '%" + dlJardin.SelectedItem.Text.TrimEnd + "%'
        and cont_dnombr like '%" + txtBenef1.Text + "%' and cont_dnombr like '%" + txtbenef2.Text + "%'
        ORDER BY A.Codigo_clie desc "
        Datos = conf.EjecutaSql(Sql)



        gvClientes.DataSource = Datos.Tables(0)
        gvClientes.DataBind()



    End Sub


    Private Sub gvClientes_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvClientes.Sorting
        Dim Orden As String

        If Session("Orden") = "0" Then
            Orden = "Asc"
            Session.Add("Orden", "1")
        Else
            Orden = "Desc"
            Session.Add("Orden", "0")
        End If

        Session("GV").DefaultView.Sort = e.SortExpression.ToString + " " + Orden
        gvClientes.DataSource = Session("GV")
        gvClientes.DataBind()
    End Sub

    'Private Sub BtnabrirPDF_Click(sender As Object, e As EventArgs) Handles BtnabrirPDF.Click
    '    Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
    '    Dim Sql As String

    '    If fecha1Inh.ToString.Length > 0 Then
    '        Session.Add("Fecha1I", fecha1Inh.Text)
    '    Else
    '        Session.Add("Fecha1I", "1990-01-01")
    '    End If

    '    If Fecha2Inh.Text.Length > 0 Then
    '        Session.Add("Fecha2I", Fecha2Inh.Text)
    '    Else
    '        Session.Add("Fecha2I", DateTime.Now.ToString("yyyy-MM-dd"))
    '    End If

    '    Sql = "SELECT A.Codigo_clie Codigo,c.cont_numero Contrato ,A.Nombre_clie Nombre_Cliente,convert(date,c1.cont_dfecha) Fecha, '" + Session("Fecha1I") + "'Fecha1,'" + Session("Fecha2I") + "' Fecha2,ISNULL(A.Saldo_actua,0) Saldo,cont_dnombr [Persona Inhumado], isnull(C1.CONT_JARDIN,'-') Jardin,isnull(c1.CONT_LETRA,'-')+'-'+isnull(c1.CONT_NNUM,'-') Ubicacion "
    '    Sql += " From CLIENTES A inner join CONTRATO C ON C.CODIGO_CLIE = A.CODIGO_CLIE "
    '    Sql += " inner join CONTRAT1 c1 on c1.cont_numero = c.cont_numero and c1.CONT_DCODIG = a.Codigo_clie "
    '    Sql += " inner join SERVICIO ser on ser.serv_codigo = c.CONT_SERVI "
    '    Sql += " Where convert(date,c1.cont_dfecha) BETWEEN '" + Session("Fecha1I") + "' and '" + Session("Fecha2I") + "' AND a.CODIGO_CLIE + c1.cont_dnombr + a.IDENTIDAD Like '%" + txtCliente.Text + "%' "
    '    Sql += " and ser.serv_codigo in (01  ,02  ,03  ,04  ,05  ,06 ,100) and c1.cont_dnumer >= '01' and a.CODIGO_CLIE + c1.cont_dnombr + a.IDENTIDAD Like '%" + TxtCliente2.Text + "%'"
    '    Sql += " ORDER BY A.Codigo_clie desc "
    '    Datos = conf.EjecutaSql(Sql)

    '    Session.Add("DATOSCR", Datos)
    '    Session.Add("ReporteCR", "DescargarInh")

    '    Dim javaScript As String = "window.open('ReportesCR.aspx');"
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)

    'End Sub


    Sub ReporteCRS(Informe As CrystalDecisions.CrystalReports.Engine.ReportDocument, Nombre_Archivo As String, DatosCR As DataSet)
        Informe.SetDataSource(DatosCR.Tables(1))
        Informe.Subreports(0).SetDataSource(DatosCR.Tables(0))
        'Informe.Subreports(1).SetDataSource(DatosCR.Tables(0))

        Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
        Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions

        exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        exportOpts.ExportFormatOptions = pdfOpts
        Informe.ExportToHttpResponse(exportOpts, Response, False, Nombre_Archivo)
        Informe.Close()
        Informe.Dispose()
    End Sub

    Sub EstadoDeCuenta()
        Dim Informe As New Movimiento_Clientes

        Informe.SetDatabaseLogon(Usuario, Clave)
        Informe.SetParameterValue("Cliente", Session("CodigoCliente").TrimEnd)


        Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
        Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
        exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        exportOpts.ExportFormatOptions = pdfOpts
        Informe.ExportToHttpResponse(exportOpts, Response, False, "Movimiento_Cliente")
        Informe.Close()
        Informe.Dispose()

    End Sub
    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub

    Private Sub gvClientes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvClientes.RowCommand
        Dim Informe As New Movimiento_Clientes
        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        ' Dim Sql As String

        If e.CommandName = "Detalle" Then
            Session.Add("CodigoCliente", gvClientes.Rows(e.CommandArgument.ToString).Cells(1).Text.ToString.TrimEnd)

        End If



        'Sql = " Exec AMORETERNO..SP_CR_VS_MovimientoClineteSubRPT '" + Session("CodigoCliente").TrimEnd + "' EXEC AMORETERNO..CR_ESTADO_DE_CUENTA   '" + Session("CodigoCliente").TrimEnd + "'"
        'DatosCR = conf.EjecutaSql(Sql)

        Dim javaScript As String = "window.open('monitorclientes.aspx','_blank','scrollbars=yes,resizable=yes,top=5,left=5,width=700,height=700');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)

        'Informe = New Movimiento_Clientes
        'ReporteCRS(Informe, "Movimiento_Cliente", DatosCR)

        EstadoDeCuenta()

    End Sub

    Private Sub gvMonitor_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvClientes.Sorting
        Dim Orden As String

        If Session("Orden") = "0" Then
            Orden = "Asc"
            Session.Add("Orden", "1")
        Else
            Orden = "Desc"
            Session.Add("Orden", "0")
        End If

        Session("GV").DefaultView.Sort = e.SortExpression.ToString + " " + Orden
        gvClientes.DataSource = Session("GV")
        gvClientes.DataBind()
    End Sub

End Class