Imports CrystalDecisions.Shared
Imports System.IO
Imports System.Drawing
Public Class monitorventas1
    Inherits System.Web.UI.Page


    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut, Status, SuperUser, Funcion As String
    Private Datos, Datos1, Datos2, Datos3 As DataSet
    Private TotalDias As Decimal = 0
    Private TotalDocumento As Decimal = 0
    Private TotalVenta As Decimal = 0
    Private TotalCosto As Decimal = 0
    Private TotalGanacia As Decimal = 0
    Private Total As Decimal = 0
    Private Visitados As Decimal = 0
    Private Ventas As Decimal = 0
    Private Verdes As Decimal = 0
    Private Recibos As Decimal = 0
    Private Cobradores As Decimal = 0
    Private Liquida, Liquida2 As String


    Private Tabla As New DataTable
    Private TablaPago As New DataTable
    'Private Conector As SqlConnection
    'Private Adaptador As SqlDataAdapter
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        'Dim Sql As String = ""

        'Session.Add("Bd", "FUNAMOR")

        'If Session("Usuario") = "" Then
        '    Response.Redirect("inicio.aspx")
        'End If


        'Sql = "SELECT SEG_ARCHIVO FROM FUNAMOR..DETSEG WHERE SEG_USUARIO = '" + Usuario_Aut + "'"
        'Datos = conf.EjecutaSql(Sql)

        'Tabla = Datos.Tables(0)

        Usuario = "Mmejia" ''Session("Usuario")
        Clave = "Mm%%4567" 'Session("Clave")
        Servidor = "192.168.20.225" 'Session("Servidor")
        Bd = "Funamor" ''Session("DB")
        Usuario_Aut = Session("Usuario_Aut")
        Clave_Aut = Session("Clave_Aut")

        If Not IsPostBack Then
            LlenarGrafica()
            Session.Add("Orden", "0")
            dlMostrar.Items.Add("POR DIA DE TRABAJO")
            dlMostrar.Items.Add("POR LIQUIDACION")
            dlMostrar.Items.Add("SIN LIQUIDACION")
            dlMostrar.Items.Add("SIN ACTIVIDAD")
            dlMostrar.SelectedIndex = 0


            dlRun.Items.Add("False")
            dlRun.Items.Add("True")
            dlRun.SelectedIndex = 0

            If Session("Reporte") = "Caja Ventas" Then
                dlMostrar.Items.Add("SIN PROCESAR")
                dlMostrar.Items.Add("POR PROCESADOS")
                dlMostrar.SelectedIndex = 4
            End If
            dlMostrar.Items.Add("VERDES")

            DropMode.Items.Add("Simple")
            DropMode.Items.Add("Advanced")

            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim SQL As String
            SQL = " SELECT B.NOMBRE_VEND FROM FUNAMOR..VENDEDOR A INNER JOIN FUNAMOR..VENDEDOR B ON A.VEND_LIDER = B.Cod_vendedo WHERE A.VEND_STATUS = 'A' GROUP BY B.NOMBRE_VEND "
            Datos2 = conf.EjecutaSql(SQL)

            dllider.Items.Add("")
            dllider.Items.Add("TODOS")
            For I As Integer = 0 To Datos2.Tables(0).Rows.Count - 1
                dllider.Items.Add(Datos2.Tables(0).Rows(I).Item("NOMBRE_VEND"))
            Next

            Dim conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim SQL1 As String
            SQL1 = " Select B.vzon_nombre From FUNAMOR..VENDEDOR A Left Join FUNAMOR..VZONA B ON A.vzon_codigo = B.vzon_codigo WHERE A.VEND_STATUS = 'A' Group BY B.vzon_nombre "
            Datos = conf1.EjecutaSql(SQL1)

            dlzona.Items.Add("")
            For I As Integer = 0 To Datos.Tables(0).Rows.Count - 1
                dlzona.Items.Add(Datos.Tables(0).Rows(I).Item("vzon_nombre"))
            Next

            'Dim conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
            'Dim SQL2 As String
            'SQL2 = " SELECT LTRIM(RTRIM(A.Nombre_vend)) Nombre_vend FROM FUNAMOR..VENDEDOR A WHERE A.Cod_vendedo = '" + Usuario_Aut + "' "
            'Datos1 = conf2.EjecutaSql(SQL2)

            'If Datos1.Tables(0).Rows.Count > 0 Then
            '    If Datos1.Tables(0).Rows(0).Item("Nombre_vend").ToString.Length > 0 Then
            '        dllider.SelectedItem.Text = Datos1.Tables(0).Rows(0).Item("Nombre_vend")
            '        dllider.Enabled = False
            '        dlRun.Items.Add("False")
            '        dlRun.Enabled = False
            '    End If
            'End If

            'Dim conf3 As New Configuracion(Usuario, Clave, Bd, Servidor)
            'Dim SQL3 As String
            'SQL3 = " if exists (SELECT LTRIM(RTRIM(A.cod_vendedo)) Lider FROM FUNAMOR..VENDEDOR A WHERE  cod_vendedo <> vend_suplid and A.Cod_vendedo = '" + Usuario_Aut + "') select 1 ExisteLider else select 0 ExisteLider "
            'Datos3 = conf3.EjecutaSql(SQL3)

            'If Datos3.Tables(0).Rows.Count > 0 Then
            '    If Datos3.Tables(0).Rows(0).Item("ExisteLider").ToString.Length > 0 Then
            '  Session.Add("UsuarioLider", Datos3.Tables(0).Rows(0).Item("ExisteLider"))
            'End If



        End If

        ' HABILITAR()


    End Sub


    Sub LlenarGrafica()
        Dim Sql As String
        Dim conf As New Configuracion(Usuario, Clave, "Aeventas", Servidor)


        Sql = "	      DECLARE @FECHA DATE 
         SET @FECHA = '" + Session("F1") + "'

	   Select  SUBSTRING(C.Nombre_vend,1,16) Vendedor 
		,SUM(A.Por_lempira) [Cobrado]
		,SUM(A.Ventas) Ventas
        FROM ( 
        Select DISTINCT A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Ventas, 0 Verdes FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.RCODVEND = B.CONT_VENDED WHERE CONVERT(DATE,A.Fecha_recib) = @FECHA AND A.MARCA = 'N' 
        UNION ALL 
        Select A.VENDEDOR, 0, SUBSTRING(A.HORA,1,2) +':'+ SUBSTRING(A.HORA,3,2) +':00', 0 Recibos, 1 Visitados, 0 Ventas, 0 Verdes FROM NOVENTA A WHERE CONVERT(DATE,A.FECHA) = @FECHA 
        UNION ALL 
        Select A.CL_VENDEDOR, 0, CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)), 0 Recibos, 1 Visitados, 0 Ventas, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Verdes FROM CLIENTESN A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.CL_VENDEDOR = B.CONT_VENDED WHERE CONVERT(DATE,A.cl_fecha) = @FECHA AND (A.tempo = 'N' or a.tempo='A')
        ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS 
        LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo 
        LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo 
        Group BY C.Nombre_vend
order by sum(a.ventas), sum(Por_lempira)"


        Datos = conf.EjecutaSql(Sql)
        Dim Filas As Integer
        Filas = Datos.Tables(0).Rows.Count

        If Filas >= 0 Then
            Dim A As Integer
            If Filas > 10 Then
                A = 10
            Else
                A = Filas - 1
            End If

            For i As Integer = 0 To A
                GraTopVen.Series(0).Points.AddXY(Datos.Tables(0).Rows(i).Item("Vendedor"), Datos.Tables(0).Rows(i).Item("Ventas"))
                GraTopVen.Series(0).Points(i).Label = Datos.Tables(0).Rows(i).Item("Ventas")
            Next

            GraTopVen.Series(0).Font = New Drawing.Font(GraTopVen.Series(0).Font.Name, 12)
            'GraTopVen.ChartAreas(0).Area3DStyle.Enable3D = True
            'GraTopVen.Series(0).LabelForeColor = Drawing.Color.DarkBlue
            GraTopVen.ChartAreas(0).AxisX.LabelStyle.Font = New Drawing.Font(GraTopVen.Series(0).Font.Name, 10)
            GraTopVen.Series(0).Palette = DataVisualization.Charting.ChartColorPalette.BrightPastel
            GraTopVen.Series(0).SmartLabelStyle.Enabled = False
            'GraTopVen.Series(0).LabelAngle = -45
            GraTopVen.ChartAreas(0).AxisX.Interval = 1
            GraTopVen.Width = "500"
            GraTopVen.Height = "220"
        End If
    End Sub


    Protected Sub DropMode_TextChanged(sender As Object, e As EventArgs)


        If DropMode.SelectedIndex = 0 Then
            Fecha2.Visible = False
            GraTopVen.Visible = False
            rowclienteIdend.Visible = False
            rowClienteNombre.Visible = False
            RowGrafico.Visible = False
            RowFecha2.Visible = False
        Else
            Fecha2.Visible = True
            GraTopVen.Visible = True
            rowclienteIdend.Visible = True
            rowClienteNombre.Visible = True
            RowGrafico.Visible = True
            RowFecha2.Visible = True
        End If
        'txtCodClienteApp.Visible = False
        'txtidentidadClienteApp.Visible = False
        'TxtCliente1.Visible = False
        'TxtCliente2.Visible = False
        LlenarGrafica()

    End Sub
    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    If Session("Usuario_Aut") = "" Then 'Or Session("Destino") <> "monitorventas1.aspx" 
    '        Response.Redirect("inicio.aspx")
    '    End If

    '    Usuario = Session("Usuario")
    '    Clave = Session("Clave")
    '    Bd = Session("Bd")
    '    Servidor = Session("Sevidor")
    '    Funcion = Session("Funcion")
    '    Session.Timeout = Session("Tiempo")

    '    If Not IsPostBack Then
    '        Session.Add("Orden", "0")
    '        ' LlenarVendedores()

    '        txtBuscar_TextChanged(sender, e)
    '    End If
    'End Sub

    'Sub LlenarVendedores()
    '    Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
    '    Dim Sql As String = ""

    '    Sql = "	SELECT CASE CONVERT(VARCHAR,A.EncarCod) WHEN '-1' THEN '' ELSE CONVERT(VARCHAR,A.EncarCod) END  EncarCod, A.EncarNom 
    '            FROM (SELECT '-1' EncarCod, 'Todos' EncarNom UNION ALL
    '            SELECT A.EncarCod, A.EncarNom
    '      FROM Encargado A
    '      WHERE A.Activo = 'Y') A"
    '    Datos = Conf.EjecutaSql(Sql)

    '    dlVendedor.DataSource = Datos.Tables(0)
    '    dlVendedor.DataTextField = "EncarNom"
    '    dlVendedor.DataValueField = "EncarCod"
    '    dlVendedor.DataBind()
    'End Sub

    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub

    Private Sub GVtoExcel(dt As Data.DataTable, Nombre As String)
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=" + Nombre + ".xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        gvExcel.DataSource = dt
        gvExcel.DataBind()

        Using sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)

            gvExcel.RenderControl(hw)

            Response.Output.Write(sw.ToString())
            Response.Flush()
            Response.End()
        End Using
    End Sub

    Protected Sub txtBuscar_TextChanged(sender As Object, e As EventArgs)
        Dim conf, Conf1 As New Configuracion(Usuario, Clave, "AEVENTAS", Servidor)
        Dim Sql, Sql1 As String



        If DropMode.SelectedIndex = 0 Then


            Session.Add("LiderCodigo", dllider.SelectedValue)


            Session.Add("txtCobrador", txtCobrador.Value)
            Session.Add("ZonaVendedor", dlzona.SelectedValue)
            Session.Add("NombreCliente1", TxtCliente1.Text.TrimEnd())
            Session.Add("NombreCliente2", TxtCliente2.Text.Trim + txtidentidadClienteApp.Text.Trim)

            If txtFecha.Text.Length > 0 Then
                Session.Add("F1", txtFecha.Text)
            Else
                Session.Add("F1", DateTime.Now.ToString("yyyy-MM-dd"))
            End If

            If Fecha2.Text.Length > 0 Then
                Session.Add("F2", Fecha2.Text)
            Else
                Session.Add("F2", DateTime.Now.ToString("yyyy-MM-dd"))
            End If

        End If

        If dlMostrar.SelectedIndex = 0 And dllider.SelectedValue = "TODOS" Then
            GvPrincipal.Columns(1).Visible = False
            GvPrincipal.Columns(2).Visible = False
            lblVerdes.Visible = True
            Sql = " DECLARE @FECHA DATE, @FECHA2 Date 
                   SET @FECHA = '" + Session("F1") + "'
                   SET @FECHA2 = '" + Session("F2") + "'
                    exec SP_MonitorVentasXDiaEnc @Fecha,@Fecha2 '%" + dllider.SelectedValue + "%','%" + txtCobrador.Value + "%','" + dlzona.SelectedValue + "','%" + TxtCliente1.Text.TrimEnd.TrimStart + "%','%" + TxtCliente2.Text.Trim + "%','%" + txtidentidadClienteApp.Text.Trim + "%','%" + txtCodClienteApp.Text.Trim + "%'"

            If dlRun.SelectedIndex = 0 Then

                Sql1 = "  DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                Sql1 += " exec [SP_MONITOR_VENTAS_l] @Fecha "
            ElseIf dlRun.SelectedIndex = 1 Then

                Sql1 = "  DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                Sql1 += " exec SP_MONITOR_VENTAS @Fecha "


            End If

        End If

        'lblMsg.Text = Sql1
        'Exit Sub


        If dlMostrar.SelectedIndex = 0 And dllider.SelectedValue <> "TODOS" Then
            'gvMonitor.Columns(1).Visible = False
            'gvMonitor.Columns(2).Visible = False
            'lblVerdes.Visible = True

            'Sql = " DECLARE @FECHA DATE, @FECHA2 Date 
            '       SET @FECHA = '" + Session("F1") + "'
            '       SET @FECHA2 = '" + Session("F2") + "'
            '        exec SP_MonitorVentasXDiaEnc @Fecha,@Fecha2 '%" + dllider.SelectedValue + "%','%" + txtCobrador.Value + "%','" + dlzona.SelectedValue + "','%" + TxtCliente1.Text.TrimEnd.TrimStart + "%','%" + TxtCliente2.Text.Trim + "%','%" + txtidentidadClienteApp.Text.Trim + "%','%" + txtCodClienteApp.Text.Trim + "%'"

            'If Usuario_Aut = Session("UsuarioLider") Then
            '    gvMonitor.Columns(8).Visible = False
            'End If
            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("F1") + "'"
            Sql += " Select  A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUM(A.Ventas) Ventas, SUM(A.Verdes) Verdes, SUBSTRING(C.Nombre_vend,1,16) [Lider], D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
            Sql += " ,'-'Liquida,'-'Procesado  FROM ( "
            Sql += " Select DISTINCT A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Ventas, 0 Verdes FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.RCODVEND = B.CONT_VENDED WHERE CONVERT(DATE,A.Fecha_recib) = @FECHA AND A.MARCA = 'N' "
            Sql += " UNION ALL "
            Sql += " Select A.VENDEDOR, 0, SUBSTRING(A.HORA,1,2) +':'+ SUBSTRING(A.HORA,3,2) +':00', 0 Recibos, 1 Visitados, 0 Ventas, 0 Verdes FROM NOVENTA A WHERE CONVERT(DATE,A.FECHA) = @FECHA "
            Sql += " UNION ALL "
            Sql += " Select A.CL_VENDEDOR, 0, CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)), 0 Recibos, 1 Visitados, 0 Ventas, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Verdes FROM CLIENTESN A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.CL_VENDEDOR = B.CONT_VENDED WHERE CONVERT(DATE,A.cl_fecha) = @FECHA AND (A.tempo = 'N' or a.tempo='A')"
            Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
            Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
            Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
            Sql += " WHERE C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
            Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
            Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
            Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, D.vzon_nombre, B.Tel_vendedo "




            If dlRun.SelectedIndex = 0 Then

                Sql1 = "  DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                Sql1 += " exec [SP_MONITOR_VENTAS_l] @Fecha "
            ElseIf dlRun.SelectedIndex = 1 Then

                'Sql1 = "  DECLARE @FECHA DATE "
                'Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                'Sql1 += " exec SP_MONITOR_VENTAS @Fecha "


                Sql1 = " DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                Sql1 += " SELECT DISTINCT A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + '
                MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono, A.LONGITUD, A.LATITUD,cf.ClienteSistema   "
                Sql1 += " FROM( "
                Sql1 += " SELECT A.Num_doc, A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4, A.LONGITUD, A.LATITUD FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.RCODVEND = B.cont_vended WHERE CONVERT(DATE,A.Fecha_recib) = @FECHA "
                Sql1 += " UNION ALL "
                Sql1 += " SELECT 'NO VENTA', 0, SUBSTRING(A.HORA,1,2) +':'+ SUBSTRING(A.HORA,3,2) +':00', '', A.VENDEDOR, '', CONVERT(DATE,A.FECHA), A.liquida, 'N', 0, 0, 0, NULL, 0, 0, NULL, 0, 0, NULL, 0, 0, NULL, 0, 0, A.LONGITUD, A.LATITUD FROM NOVENTA A WHERE CONVERT(DATE,A.FECHA) = @FECHA "
                Sql1 += " UNION ALL "
                Sql1 += " SELECT 'PROSPECTO', 0, CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)), '', A.CL_VENDEDOR, A.Nombre_clie, CONVERT(DATE,A.cl_fecha), A.liquida, 'N', B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4, A.LATITUD, A.LONGITUD FROM CLIENTESN A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.CL_VENDEDOR = B.cont_vended WHERE CONVERT(DATE,A.cl_fecha) = @FECHA AND A.tempo = 'N' "
                Sql1 += " ) A "
                Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
                Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "
                Sql1 += " left join (select  cli.identidad,cli.Nombre_clie, "
                Sql1 += " 	 stuff((Select top 6 ', ' + Codigo_clie "
                Sql1 += "  from FUNAMOR..CLIENTES cl "
                Sql1 += "  Where  CL.identidad  =  CLI.IDENTIDAD and cl.Nombre_clie = cli.Nombre_clie "
                Sql1 += " For xml PATH('')),1, 2,'' "
                Sql1 += " 	  )	as ClientesSistema "
                Sql1 += "  From FUNAMOR..CLIENTES cli "
                Sql1 += " Group By identidad, nombre_clie "
                Sql1 += " )Cf on replace(cf.identidad,'-','') = c.identidad COLLATE Modern_Spanish_CI_AS or cf.Nombre_clie = replace(c.Nombre_clie,'.','') COLLATE Modern_Spanish_CI_AS "


            End If
        End If
        If dlMostrar.SelectedIndex = 1 Then
            GvPrincipal.Columns(1).Visible = False
            GvPrincipal.Columns(2).Visible = False

            lblVerdes.Visible = False
            'If Usuario_Aut = Session("UsuarioLider") Then
            '    gvMonitor.Columns(8).Visible = False
            ''End If

            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("F1") + "'"
            Sql += " SELECT A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUM(A.Ventas) Ventas, SUBSTRING(C.Nombre_vend,1,16) [Lider], SUBSTRING(A.liquida,1,4) +'/'+ SUBSTRING(A.liquida,5,2) +'/'+ SUBSTRING(A.liquida,7,2) +' '+ SUBSTRING(A.liquida,9,2) +':'+ SUBSTRING(A.liquida,11,2) Liquida, CASE WHEN A.liquida2 != 'N' THEN SUBSTRING(A.liquida2,1,4) +'/'+ SUBSTRING(A.liquida2,5,2) +'/'+ SUBSTRING(A.liquida2,7,2) +' '+ SUBSTRING(A.liquida2,9,2) +':'+ SUBSTRING(A.liquida2,11,2) ELSE 'N' END Procesado, D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
            Sql += " FROM ( "
            Sql += " Select DISTINCT A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Ventas, A.liquida, A.liquida2 FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.RCODVEND = B.CONT_VENDED WHERE CONVERT(DATE,SUBSTRING(A.liquida,1,8)) = @FECHA AND A.MARCA = 'N' "
            Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
            Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
            Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
            Sql += " WHERE C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
            Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
            Sql += " AND A.liquida != 'N' "
            Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
            Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, A.Liquida, A.liquida2, D.vzon_nombre, B.Tel_vendedo "



            If dlRun.SelectedIndex = 0 Then ''Sin Consulta Cliente Sistema


                Sql1 = "  DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("F1") + "'"
                Sql1 += " EXEC [SP_MONITOR_VENTAS_X_LIQUID] @Fecha "

            ElseIf dlRun.SelectedIndex = 1 Then


                Sql1 = "  DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                Sql1 += " EXEC SP_MONITOR_VENTAS_X_LIQUID_Sistema  @Fecha "

            End If
            'lblMsg.Text = Sql1
            'Exit Sub
        End If



        If dlMostrar.SelectedIndex = 2 Then
            GvPrincipal.Columns(1).Visible = False
            GvPrincipal.Columns(2).Visible = False
            lblVerdes.Visible = False
            'If Usuario_Aut.ToUpper = "MANAGER" Then
            '    gvMonitor.Columns(1).Visible = True
            'End If


            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("F1") + "'"
            Sql += " SELECT A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUM(A.Ventas) Ventas, SUBSTRING(C.Nombre_vend,1,16) [Lider], A.Liquida, A.Liquida2 Procesado, D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
            Sql += " FROM ( "
            Sql += " Select DISTINCT A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Ventas, A.liquida, A.liquida2 FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.RCODVEND = B.CONT_VENDED WHERE A.MARCA = 'N' AND A.liquida = 'N' AND CONVERT(DATE,A.Fecha_recib) <= @FECHA "
            Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
            Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
            Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
            Sql += " WHERE C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
            Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
            Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
            Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, A.Liquida, A.liquida2, D.vzon_nombre, B.Tel_vendedo "


            If dlRun.SelectedIndex = 0 Then

                Sql1 = "  DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                Sql1 += " exec SP_MONITOR_VENTAS_SIN_LIQUID @Fecha "

            ElseIf dlRun.SelectedIndex = 1 Then


                Sql1 = "  DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                Sql1 += " exec [SP_MONITOR_VENTAS_SIN_LIQUID_SISTEMA] @Fecha "

            End If


        End If

        If dlMostrar.SelectedIndex = 3 Then
            GvPrincipal.Columns(1).Visible = False
            GvPrincipal.Columns(2).Visible = False
            lblVerdes.Visible = False

            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("F1") + "'"
            Sql += " SELECT A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUBSTRING(C.Nombre_vend,1,16) [Lider], A.Liquida, A.Liquida2 Procesado, D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
            Sql += " FROM ( "
            Sql += " SELECT A.cveage RCODVEND, 0 Por_lempira, '00:00:00' rhora, 0 Recibos, 0 Visitados, 'N' liquida, 'N' liquida2 FROM MVSAGEN A WHERE A.cveage COLLATE Modern_Spanish_CI_AS NOT IN (SELECT A.RCODVEND FROM RECIBOS A WHERE CONVERT(DATE,A.Fecha_recib) = @FECHA GROUP BY A.RCODVEND UNION ALL SELECT A.VENDEDOR FROM NOVENTA A WHERE CONVERT(DATE,A.FECHA) = @FECHA GROUP BY A.VENDEDOR UNION ALL SELECT A.CL_VENDEDOR FROM CLIENTESN A WHERE CONVERT(DATE,A.cl_fecha) = @FECHA AND A.tempo = 'N' GROUP BY A.CL_VENDEDOR ) "
            Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS AND B.VEND_STATUS = 'A' "
            Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
            Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
            Sql += " WHERE B.VEND_STATUS = 'A'"
            Sql += " AND C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
            Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
            Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
            Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, A.Liquida, A.liquida2, D.vzon_nombre, B.Tel_vendedo "


            If dlRun.SelectedIndex = 1 Then

                Sql1 = "  DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                Sql1 += " SELECT DISTINCT TOP 1 A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono  "
                Sql1 += " ,cf.ClientesSistema FROM( "
                Sql1 += " SELECT A.Num_doc, A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4 FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.RCODVEND = B.cont_vended WHERE A.Liquida = 'N' AND CONVERT(DATE,A.Fecha_recib) = @FECHA "
                Sql1 += " ) A "
                Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
                Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "
                Sql1 += " left join (select  cli.identidad,cli.Nombre_clie, "
                Sql1 += " 	 stuff((Select top 6 ', ' + Codigo_clie "
                Sql1 += "  from FUNAMOR..CLIENTES cl "
                Sql1 += "  Where  CL.identidad  =  CLI.IDENTIDAD and cl.Nombre_clie = cli.Nombre_clie "
                Sql1 += " For xml PATH('')),1, 2,'' "
                Sql1 += " 	  )	as ClientesSistema "
                Sql1 += "  From FUNAMOR..CLIENTES cli "
                Sql1 += " Group By identidad, nombre_clie "
                Sql1 += " )Cf on replace(cf.identidad,'-','') = c.identidad COLLATE Modern_Spanish_CI_AS or replace(cf.Nombre_clie,'.','') = replace(c.Nombre_clie,'.','') COLLATE Modern_Spanish_CI_AS "

            ElseIf dlRun.SelectedIndex = 0 Then
                Sql1 = "  DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                Sql1 += " SELECT DISTINCT TOP 1 A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono  "
                Sql1 += " ,cf.ClientesSistema FROM( "
                Sql1 += " SELECT A.Num_doc, A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4 FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.RCODVEND = B.cont_vended WHERE A.Liquida = 'N' AND CONVERT(DATE,A.Fecha_recib) = @FECHA "
                Sql1 += " ) A "
                Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
                Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "
                Sql1 += " left join (select  cli.identidad,cli.Nombre_clie, "
                Sql1 += " 	 stuff((Select top 6 ', ' + Codigo_clie "
                Sql1 += "  from FUNAMOR..CLIENTES cl "
                Sql1 += "  Where  CL.identidad  =  CLI.IDENTIDAD and cl.Nombre_clie = cli.Nombre_clie "
                Sql1 += " For xml PATH('')),1, 2,'' "
                Sql1 += " 	  )	as ClientesSistema "
                Sql1 += "  From FUNAMOR..CLIENTES cli "
                Sql1 += " Group By identidad, nombre_clie "
                Sql1 += " )Cf on replace(cf.identidad,'-','') = c.identidad COLLATE Modern_Spanish_CI_AS or cf.Nombre_clie = replace(c.Nombre_clie,'.','') COLLATE Modern_Spanish_CI_AS "
            End If
        End If

        If dlMostrar.SelectedIndex = 4 Then
            GvPrincipal.Columns(1).Visible = True
            GvPrincipal.Columns(2).Visible = False
            lblVerdes.Visible = False

            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("F1") + "'"
            Sql += " SELECT A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUBSTRING(C.Nombre_vend,1,16) [Lider], SUBSTRING(A.liquida,1,4) +'/'+ SUBSTRING(A.liquida,5,2) +'/'+ SUBSTRING(A.liquida,7,2) +' '+ SUBSTRING(A.liquida,9,2) +':'+ SUBSTRING(A.liquida,11,2) Liquida, CASE WHEN A.liquida2 != 'N' THEN SUBSTRING(A.liquida2,1,4) +'/'+ SUBSTRING(A.liquida2,5,2) +'/'+ SUBSTRING(A.liquida2,7,2) +' '+ SUBSTRING(A.liquida2,9,2) +':'+ SUBSTRING(A.liquida2,11,2) ELSE 'N' END Procesado, D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
            Sql += " FROM ( "
            Sql += " Select A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, A.liquida, A.liquida2 FROM RECIBOS A WHERE A.MARCA = 'N' AND A.liquida <> 'N' AND A.liquida2 = 'N' AND CONVERT(DATE,A.Fecha_recib) <= @FECHA "
            Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
            Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
            Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
            Sql += " WHERE C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
            Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
            Sql += " AND A.liquida != 'N' "
            Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
            Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, A.Liquida, A.liquida2, D.vzon_nombre, B.Tel_vendedo "


            If dlRun.SelectedIndex = 1 Then


                Sql1 = "  DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                Sql1 += " exec  [SP_MONITOR_VENTAS_SinProcesar]  @Fecha "


            ElseIf dlRun.SelectedIndex = 0 Then

                Sql1 = " DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                Sql1 += " SELECT DISTINCT A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono "
                Sql1 += " ,''clientesSistema FROM( "
                Sql1 += " SELECT  distinct  case when B.SERVI1DES like '%lote%' or B.SERVI1DES like '%crem%' or B.SERVI1DES like '%placa%' "
                Sql1 += " then 'P01'  +''+SUBSTRING(Num_doc,4,14) "
                Sql1 += " when (B.SERVI1DES not like '%lote%' or B.SERVI1DES not like '%crem%' or B.SERVI1DES  not like  '%placa%'  ) AND SUBSTRING(Num_doc,1,3) = 'P01' 			 "
                Sql1 += " then rtrim(ltrim(codzona))+''+SUBSTRING(Num_doc,4,14) "
                Sql1 += " when (B.SERVI1DES not like '%lote%' or B.SERVI1DES not like '%crem%' or  B.SERVI1DES not like  '%placa%' ) AND SUBSTRING(Num_doc,1,3) <> 'P01' "
                Sql1 += " THEN rtrim(ltrim(codzona))+''+SUBSTRING(Num_doc,4,14) ELSE A.Num_doc end Num_Doc, "
                Sql1 += " A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4 "
                Sql1 += " FROM RECIBOS A "
                Sql1 += " LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie And A.RCODVEND = B.cont_vended "
                Sql1 += " LEFT join CLIENTESN Cc ON CC.Codigo_clie = A.Codigo_clie And a.RCODVEND = cc.CL_VENDEDOR "
                Sql1 += " INNER JOIN DEPTOZONA DP ON DP.desdepto = CC.departa And dp.desmuni = Cc.municipio "
                Sql1 += " WHERE A.Liquida <> 'N' AND A.Liquida2 = 'N' AND CONVERT(DATE,A.Fecha_recib) <= @FECHA "
                Sql1 += " ) A "
                Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
                Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "
                'Sql1 += " left join (select cli.identidad,cli.Nombre_clie, "
                'Sql1 += " 	 stuff((Select top 6 ', ' + Codigo_clie "
                'Sql1 += "  from FUNAMOR..CLIENTES cl "
                'Sql1 += "  Where  CL.identidad  =  CLI.IDENTIDAD and cl.Nombre_clie = cli.Nombre_clie "
                'Sql1 += " For xml PATH('')),1, 2,'' "
                'Sql1 += " 	  )	as ClientesSistema "
                'Sql1 += "  From FUNAMOR..CLIENTES cli "
                'Sql1 += " Group By identidad, nombre_clie "
                'Sql1 += " )Cf on replace(replace(cf.identidad,'-',''),'.','') = c.identidad COLLATE Modern_Spanish_CI_AS 	or replace(replace(replace(lTRIM(RTRIM(cf.Nombre_clie)),'.',''),'á','a'),'É','E') = REPLACE(c.Nombre_clie,'.','') COLLATE Modern_Spanish_CI_AS  "
                'lblMsg.Text = Sql1
                'Exit Sub
            End If

        End If

        If dlMostrar.SelectedIndex = 5 Then
            GvPrincipal.Columns(2).Visible = True
            GvPrincipal.Columns(1).Visible = False
            lblVerdes.Visible = False

            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("F1") + "'"
            Sql += " SELECT A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUBSTRING(C.Nombre_vend,1,16) [Lider], SUBSTRING(A.liquida,1,4) +'/'+ SUBSTRING(A.liquida,5,2) +'/'+ SUBSTRING(A.liquida,7,2) +' '+ SUBSTRING(A.liquida,9,2) +':'+ SUBSTRING(A.liquida,11,2) Liquida, CASE WHEN A.liquida2 != 'N' THEN SUBSTRING(A.liquida2,1,4) +'/'+ SUBSTRING(A.liquida2,5,2) +'/'+ SUBSTRING(A.liquida2,7,2) +' '+ SUBSTRING(A.liquida2,9,2) +':'+ SUBSTRING(A.liquida2,11,2) ELSE 'N' END Procesado, D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
            Sql += " FROM ( "
            Sql += " Select A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, A.liquida, A.liquida2 FROM RECIBOS A WHERE CONVERT(DATE,SUBSTRING(A.liquida2,1,8)) = @FECHA AND A.MARCA = 'N' "
            Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
            Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
            Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
            Sql += " WHERE C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
            Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
            Sql += " AND A.liquida2 != 'N' "
            Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
            Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, A.Liquida, A.liquida2, D.vzon_nombre, B.Tel_vendedo "

            Sql1 = " DECLARE @FECHA DATE "
            Sql1 += " SET @FECHA = '" + Session("F1") + "' "
            Sql1 += " SELECT DISTINCT A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono  "
            Sql1 += " ,'' ClientesSistema FROM( "
            Sql1 += " SELECT  distinct  case when B.SERVI1DES like '%lote%' or B.SERVI1DES like '%crem%' or B.SERVI1DES like '%placa%' "
            Sql1 += " then 'P01'  +''+SUBSTRING(Num_doc,4,14) "
            Sql1 += " when (B.SERVI1DES not like '%lote%' or B.SERVI1DES not like '%crem%' or B.SERVI1DES not like '%placa%') AND SUBSTRING(Num_doc,1,3) = 'P01' 			 "
            Sql1 += " then rtrim(ltrim(codzona))+''+SUBSTRING(Num_doc,4,14) "
            Sql1 += " when (B.SERVI1DES not like '%lote%' or B.SERVI1DES not like '%crem%' not like B.SERVI1DES like '%placa%') AND SUBSTRING(Num_doc,1,3) <> 'P01' "
            Sql1 += " THEN rtrim(ltrim(codzona))+''+SUBSTRING(Num_doc,4,14) ELSE A.Num_doc end Num_Doc, "
            Sql1 += " A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4 "
            Sql1 += " FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.RCODVEND = B.cont_vended "
            Sql1 += " LEFT join CLIENTESN Cc ON CC.Codigo_clie = A.Codigo_clie And a.RCODVEND = cc.CL_VENDEDOR "
            Sql1 += " INNER JOIN DEPTOZONA DP ON DP.desdepto = CC.departa And dp.desmuni = Cc.municipio "
            Sql1 += " WHERE CONVERT(DATE,SUBSTRING(A.liquida2,1,8)) = @FECHA "
            Sql1 += " ) A "
            Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
            Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "
            'Sql1 += " left join (select cli.identidad,cli.Nombre_clie, "
            'Sql1 += " 	 stuff((Select top 6 ', ' + Codigo_clie "
            'Sql1 += "  from FUNAMOR..CLIENTES cl "
            'Sql1 += "  Where  CL.identidad  =  CLI.IDENTIDAD and cl.Nombre_clie = cli.Nombre_clie "
            'Sql1 += " For xml PATH('')),1, 2,'' "
            'Sql1 += " 	  )	as ClientesSistema "
            'Sql1 += "  From FUNAMOR..CLIENTES cli "
            'Sql1 += " Group By identidad, nombre_clie "
            'Sql1 += " )Cf on replace(replace(cf.identidad,'-',''),'.','') = c.identidad COLLATE Modern_Spanish_CI_AS or REPLACE(REPLACE(replace(replace(replace(replace(lTRIM(RTRIM(cf.Nombre_clie)),'.',''),'á','a'),'É','E'),'Í','I'),'Ó','O'),'Ú','U')  = REPLACE(REPLACE(replace(replace(replace(replace(lTRIM(RTRIM(c.Nombre_clie)),'.',''),'á','a'),'É','E'),'Í','I'),'Ó','O'),'Ú','U') COLLATE Modern_Spanish_CI_AS "
            Sql1 += " WHERE A.liquida2 != 'N' "
        End If

        'lblMsg.Text = Sql1
        'Exit Sub

        If dlMostrar.SelectedValue = "VERDES" Then
            GvPrincipal.Columns(1).Visible = False
            GvPrincipal.Columns(2).Visible = False

            Sql = " DECLARE @FECHA DATE "
            'Sql += " SET @FECHA = '" + Session("F1") + "'"
            Sql += " SELECT A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Verdes, SUM(A.Ventas) Ventas, SUBSTRING(C.Nombre_vend,1,16) [Lider], D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
            Sql += " FROM ( "
            'Sql += " Select DISTINCT A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Ventas, A.liquida, A.liquida2 FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.RCODVEND = B.CONT_VENDED WHERE A.MARCA = 'N' AND A.liquida = 'N' AND CONVERT(DATE,A.Fecha_recib) <= @FECHA "
            Sql += " Select A.CL_VENDEDOR RCODVEND, 0 Por_lempira, CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)) rhora, 0 Recibos, 1 Visitados, 0 Ventas FROM CLIENTESN A WHERE A.tempo = 'N'"
            Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
            Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
            Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
            Sql += " WHERE C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
            Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
            Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
            Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, D.vzon_nombre, B.Tel_vendedo "

            Sql1 = " DECLARE @FECHA DATE "
            Sql1 += " SET @FECHA = '" + Session("F1") + "' "
            Sql1 += " SELECT DISTINCT A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono "
            Sql1 += " ,'-' ClientesSistema FROM( "
            Sql1 += " SELECT 'PROSPECTO' Num_doc, 0 Por_lempira, CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)) rhora, A.Codigo_clie, A.CL_VENDEDOR RCODVEND, '' Concepto, A.Nombre_clie, CONVERT(DATE,A.cl_fecha) Fecha, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4, A.LONGITUD, A.LATITUD FROM CLIENTESN A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.CL_VENDEDOR = B.cont_vended WHERE A.tempo = 'N'  "
            Sql1 += " ) A "
            Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
            Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "

        End If

        'lblMsg.Text = Sql1
        'Exit Sub

        'Datos = conf.EjecutaSql(Sql)

        'If Usuario_Aut = "JULIOCAJA" Or Usuario_Aut = "MANAGER" Or Usuario_Aut = "MDERAS" Or Usuario_Aut = "ABLANDON" Or Usuario_Aut = "JULIO" And (Datos.Tables(0).Rows.Count = 1 And dlMostrar.SelectedIndex = 1) Then
        '    Datos1 = Conf1.EjecutaSql(Sql1)
        '    Session.Add("GVDetalle", Datos1.Tables(0))

        '    Session("GVDetalle").DefaultView.RowFilter = "RCODVEND ='" + txtCobrador.Value + "'"
        '    Session("GVDetalle").DefaultView.Sort = "Fecha Asc, Hora Asc"

        '    gvDetalle2.DataSource = Session("GVDetalle")
        '    gvDetalle2.DataBind()

        '    Datos = conf.EjecutaSql(Sql)
        '    Datos.Tables(0).DefaultView.Sort = "Cobrado Desc"
        '    Session.Add("GV", Datos.Tables(0))
        '    gvMonitor2.DataSource = Session("GV")
        '    gvMonitor2.DataBind()

        '    gvMonitor2.Visible = True
        '    gvDetalle2.Visible = True
        '    gvMonitor.Visible = False

        'ElseIf dlMostrar.SelectedIndex = 2 And Datos.Tables(0).Rows.Count = 1 Then
        '    Datos1 = Conf1.EjecutaSql(Sql1)
        '    Session.Add("GVDetalle", Datos1.Tables(0))

        '    Session("GVDetalle").DefaultView.RowFilter = "RCODVEND ='" + txtCobrador.Value + "'"
        '    Session("GVDetalle").DefaultView.Sort = "Fecha Asc, Hora Asc"

        '    gvDetalle2.DataSource = Session("GVDetalle")
        '    gvDetalle2.DataBind()

        '    Datos = conf.EjecutaSql(Sql)
        '    Datos.Tables(0).DefaultView.Sort = "Cobrado Desc"
        '    Session.Add("GV", Datos.Tables(0))
        '    gvMonitor2.DataSource = Session("GV")
        '    gvMonitor2.DataBind()

        '    gvMonitor2.Visible = True
        '    gvDetalle2.Visible = True
        '    gvMonitor.Visible = False
        '    gvDetalle2.Columns(2).Visible = True
        '    gvDetalle2.Columns(1).Visible = False

        'ElseIf dlMostrar.SelectedIndex = 0 And Datos.Tables(0).Rows.Count = 1 Then
        '    Datos1 = Conf1.EjecutaSql(Sql1)
        '    Session.Add("GVDetalle", Datos1.Tables(0))

        '    Session("GVDetalle").DefaultView.RowFilter = "RCODVEND ='" + txtCobrador.Value + "'"
        '    Session("GVDetalle").DefaultView.Sort = "Fecha Asc, Hora Asc"

        '    gvDetalle2.DataSource = Session("GVDetalle")
        '    gvDetalle2.DataBind()

        '    Datos = conf.EjecutaSql(Sql)
        '    Datos.Tables(0).DefaultView.Sort = "Cobrado Desc"
        '    Session.Add("GV", Datos.Tables(0))
        '    gvMonitor2.DataSource = Session("GV")
        '    gvMonitor2.DataBind()

        '    gvMonitor2.Visible = True
        '    gvDetalle2.Visible = True
        '    gvMonitor.Visible = False
        '    gvDetalle2.Columns(0).Visible = False
        '    gvDetalle2.Columns(1).Visible = True

        'Else
        '    Datos1 = Conf1.EjecutaSql(Sql1)
        '    Session.Add("GVDetalle", Datos1.Tables(0))

        '    Datos = conf.EjecutaSql(Sql)
        '    Datos.Tables(0).DefaultView.Sort = "Cobrado Desc"
        '    Session.Add("GV", Datos.Tables(0))
        '    gvMonitor.DataSource = Session("GV")
        '    gvMonitor.DataBind()
        '    gvMonitor2.Visible = False
        '    gvDetalle2.Visible = False
        '    gvMonitor.Visible = True
        'End If

        ' lblHora.Text = "Actualizado: " + System.DateTime.Now.ToShortTimeString


        Datos1 = Conf1.EjecutaSql(Sql1)
        Session.Add("GridDetalle", Datos1.Tables(0))

        Datos = Conf.EjecutaSql(Sql)
        Session.Add("GV", Datos)
        GvPrincipal.DataSource = Session("GV").Tables(0)
        GvPrincipal.DataBind()

    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        'MyBase.VerifyRenderingInServerForm(control)
    End Sub

    Private Sub GvPrincipal_Sorting(sender As Object, e As GridViewSortEventArgs) Handles GvPrincipal.Sorting
        Dim Orden As String

        If Session("Orden") = "0" Then
            Orden = "Asc"
            Session.Add("Orden", "1")
        Else
            Orden = "Desc"
            Session.Add("Orden", "0")
        End If

        Session("GV").Tables(0).DefaultView.Sort = e.SortExpression.ToString + " " + Orden
        GvPrincipal.DataSource = Session("GV").Tables(0)
        GvPrincipal.DataBind()
    End Sub



    'Protected Sub GvPrincipal_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles GvPrincipal.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        Dim gvDetalle As GridView = DirectCast(e.Row.FindControl("GVDetalle"), GridView)
    '        Dim Fila As System.Data.DataRowView = e.Row.DataItem
    '        Dim Codigo As String = Fila.Item("Codigo").ToString
    '        Dim id As Integer = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Codigo"))
    '        'Dim detalles As List(Of Detalle) = ObtenerDetallesPorId(id)
    '        Session("GridDetalle").DefaultView.RowFilter = "Codigo='" + Codigo + "'"
    '        gvDetalle.DataSource = Session("GridDetalle")
    '        gvDetalle.DataBind()
    '    End If
    'End Sub

    Private Sub GvPrincipal_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GvPrincipal.RowDataBound
        If ((e.Row.RowType = DataControlRowType.DataRow) And (e.Row.RowType <> DataControlRowType.EmptyDataRow)) Then
            Dim Fila As System.Data.DataRowView = e.Row.DataItem
            Dim Codigo As String = Fila.Item("Codigo").ToString
            Dim gvDetalle As GridView = TryCast(e.Row.FindControl("GVDetalle"), GridView)

            Session("GridDetalle").DefaultView.RowFilter = "rcodvend ='" + Codigo + "'"
            gvDetalle.DataSource = Session("GridDetalle")
            gvDetalle.DataBind()

            'TotalDocumento += Convert.ToDecimal(Fila.Item("Saldo"))
            'TotalVenta += Convert.ToDecimal(Replace(Fila.Item("Corriente"), "-", "0"))
            'TotalCosto += Convert.ToDecimal(Fila.Item("Saldo")) - Convert.ToDecimal(Replace(Fila.Item("Corriente"), "-", "0"))

            'lblTotalDocumento.Text = "Saldo: " + Format(TotalDocumento, "#,##0.00")
            'lblTotalVenta.Text = "Corriente: " + Format(TotalVenta, "#,##0.00")
            'lblTotalCosto.Text = "Vencido: " + Format(TotalCosto, "#,##0.00")
        End If
    End Sub


    'Private Sub GvPrincipal_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GvPrincipal.RowDataBound
    '  If ((e.Row.RowType = DataControlRowType.DataRow) And (e.Row.RowType <> DataControlRowType.EmptyDataRow)) Then
    '        Dim Fila As System.Data.DataRowView = e.Row.DataItem
    '        Dim Codigo As String = Fila.Item("Codigo").ToString
    '        Dim gvDetalle As GridView = TryCast(e.Row.FindControl("GVDetalle"), GridView)

    '        Session("GvDetalle").DefaultView.RowFilter = "Codigo='" + Codigo + "'"
    '        gvDetalle.DataSource = Session("GvDetalle")
    '        gvDetalle.DataBind()

    'TotalDocumento += Convert.ToDecimal(Fila.Item("Saldo"))
    'TotalVenta += Convert.ToDecimal(Replace(Fila.Item("Corriente"), "-", "0"))
    'TotalCosto += Convert.ToDecimal(Fila.Item("Saldo")) - Convert.ToDecimal(Replace(Fila.Item("Corriente"), "-", "0"))

    'lblTotalDocumento.Text = "Saldo: " + Format(TotalDocumento, "#,##0.00")
    'lblTotalVenta.Text = "Corriente: " + Format(TotalVenta, "#,##0.00")
    'lblTotalCosto.Text = "Vencido: " + Format(TotalCosto, "#,##0.00")
    '    End If
    'End Sub
    Private Sub gvMonitor_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GvPrincipal.RowDataBound
        Try
            If ((e.Row.RowType = DataControlRowType.DataRow) And (e.Row.RowType <> DataControlRowType.EmptyDataRow)) Then
                Dim Fila As System.Data.DataRowView = e.Row.DataItem
                Total += Convert.ToDecimal(Fila.Item(2))
                Visitados += Convert.ToDecimal(Fila.Item(6))
                Recibos += Convert.ToDecimal(Fila.Item(5))
                Cobradores += 1
                If dlMostrar.SelectedIndex = 1 Or dlMostrar.SelectedIndex = 5 Then
                    Liquida = Fila.Item(9)
                    Liquida2 = Fila.Item(10)
                End If

                If dlMostrar.SelectedIndex = 0 Then
                    Verdes += Convert.ToDecimal(Fila.Item(8))
                    lblVerdes.Text = "Verdes: " + Format(Verdes, "#,##0")
                End If

                If dlMostrar.SelectedIndex = 0 Or dlMostrar.SelectedIndex = 1 Or dlMostrar.SelectedIndex = 2 Then
                    Ventas += Convert.ToDecimal(Fila.Item(7))
                    lblVentas.Text = "Ventas: " + Format(Ventas, "#,##0")
                End If

                If dlMostrar.SelectedIndex = 3 Then
                    lblVentas.Text = "Ventas: 0"
                End If

                lblTotal.Text = "Cobrado: " + Format(Total, "#,##0.00")
                lblRecibos.Text = "Recibos: " + Format(Recibos, "#,##0")
                lblVisitados.Text = "Visitados: " + Format(Visitados, "#,##0")
                lblCobradores.Text = "Vendedores: " + Cobradores.ToString
                e.Row.Cells(6).Text = Format(Fila.Item(2), "#,##0.00")
            End If
        Catch ex As Exception
            Msg(ex.Message.ToString() + " - " + ex.Source.ToString())
        End Try

        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    Dim Codigo As String = GvPrincipal.DataKeys(e.Row.RowIndex).Value.ToString()
        '    Dim gvDetalle As GridView = TryCast(e.Row.FindControl("gvDetalle"), GridView)

        '    If dlMostrar.SelectedIndex = 1 Or dlMostrar.SelectedIndex = 5 Then
        '        Session("GVDetalle").DefaultView.RowFilter = "Liquida='" + Liquida.Replace("/", "").Replace(" ", "").Replace(":", "") + "' AND Liquida2='" + Liquida2.Replace("/", "").Replace(" ", "").Replace(":", "") + "' AND RCODVEND='" + Codigo + "'"
        '    ElseIf dlMostrar.SelectedIndex = 0 And dllider.SelectedValue = "TODOS" Then
        '        Session("GVDetalle").DefaultView.RowFilter = "Lider='" + Codigo + "'"
        '    Else
        '        Session("GVDetalle").DefaultView.RowFilter = "RCODVEND='" + Codigo + "'"
        '    End If
        '    Session("GVDetalle").DefaultView.Sort = "Fecha Asc, Hora Asc"

        '    gvDetalle.DataSource = Session("GVDetalle")
        '    gvDetalle.DataBind()

        '    For i = 0 To gvDetalle.Rows.Count - 1
        '        Select Case gvDetalle.Rows(i).Cells(0).Text
        '            Case "NO VENTA"
        '                gvDetalle.Rows(i).ControlStyle.ForeColor = System.Drawing.Color.Red
        '            Case "PROSPECTO"
        '                gvDetalle.Rows(i).ControlStyle.ForeColor = System.Drawing.Color.Green
        '        End Select

        '        If gvDetalle.Rows(i).Cells(10).Text.TrimEnd = "ANULADO" Then
        '            Session("GVDetalle").DefaultView.RowFilter = "Codigo ='" + gvDetalle.Rows(i).Cells(0).Text.TrimEnd + "'"
        '            If Session("GVDetalle").DefaultView.Count > 0 Then
        '                gvDetalle.Rows(i).Cells(10).ToolTip = Session("GVDetalle").DefaultView.Item(0).Item("Motivo").ToString
        '            End If
        '        End If

        '        'If gvDetalle.Rows(i).Cells(5).Text.Length > 0 Then
        '        '    gvDetalle.Rows(i).Cells(5).ToolTip = Session("GVDetalle").DefaultView.Item(0).Item("identidad").ToString
        '        'End If

        '        Session("GVDetalle").DefaultView.RowFilter = "Codigo='" + gvDetalle.Rows(i).Cells(0).Text.TrimEnd + "'"
        '        If Session("GVDetalle").DefaultView.Count > 0 And Session("GVDetalle").DefaultView.Item(0).Item("SERVI2DES").ToString.Length > 0 Then
        '            gvDetalle.Rows(i).Cells(9).ToolTip = Session("GVDetalle").DefaultView.Item(0).Item("SERVI2DES").ToString + "
        '" + Session("GVDetalle").DefaultView.Item(0).Item("SERVI3DES").ToString + " 
        '" + Session("GVDetalle").DefaultView.Item(0).Item("SERVI4DES").ToString
        '            gvDetalle.Rows(i).Cells(9).ControlStyle.ForeColor = System.Drawing.Color.DarkBlue
        '        End If

        '    Next
        'End If

    End Sub
    Private Sub btnBuscarv_Click(sender As Object, e As EventArgs) Handles btnBuscarv.Click
        Dim conf, Conf1 As New Configuracion(Usuario, Clave, "AEVENTAS", Servidor)
        Dim Sql, Sql1 As String



        If DropMode.SelectedIndex = 0 Then


            Session.Add("LiderCodigo", dllider.SelectedValue)


            Session.Add("txtCobrador", txtCobrador.Value)
            Session.Add("ZonaVendedor", dlzona.SelectedValue)
            Session.Add("NombreCliente1", TxtCliente1.Text.TrimEnd())
            Session.Add("NombreCliente2", TxtCliente2.Text.Trim + txtidentidadClienteApp.Text.Trim)

            If txtFecha.Text.Length > 0 Then
                Session.Add("F1", txtFecha.Text)
            Else
                Session.Add("F1", DateTime.Now.ToString("yyyy-MM-dd"))
            End If

            If Fecha2.Text.Length > 0 Then
                Session.Add("F2", Fecha2.Text)
            Else
                Session.Add("F2", DateTime.Now.ToString("yyyy-MM-dd"))
            End If

        End If

        'If dlMostrar.SelectedIndex = 0 And dllider.SelectedValue = "TODOS" Then
        GvPrincipal.Columns(1).Visible = False
        GvPrincipal.Columns(2).Visible = False
        lblVerdes.Visible = True

        Sql = " DECLARE @FECHA DATE "
        Sql += " SET @FECHA = '" + Session("F1") + "'"
        Sql += " Select  A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUM(A.Ventas) Ventas, SUM(A.Verdes) Verdes, SUBSTRING(C.Nombre_vend,1,16) [Lider], D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
        Sql += " ,'-'Liquida,'-'Procesado  FROM ( "
        Sql += " Select DISTINCT A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Ventas, 0 Verdes FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.RCODVEND = B.CONT_VENDED WHERE CONVERT(DATE,A.Fecha_recib) = @FECHA AND A.MARCA = 'N' "
        Sql += " UNION ALL "
        Sql += " Select A.VENDEDOR, 0, SUBSTRING(A.HORA,1,2) +':'+ SUBSTRING(A.HORA,3,2) +':00', 0 Recibos, 1 Visitados, 0 Ventas, 0 Verdes FROM NOVENTA A WHERE CONVERT(DATE,A.FECHA) = @FECHA "
        Sql += " UNION ALL "
        Sql += " Select A.CL_VENDEDOR, 0, CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)), 0 Recibos, 1 Visitados, 0 Ventas, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Verdes FROM CLIENTESN A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.CL_VENDEDOR = B.CONT_VENDED WHERE CONVERT(DATE,A.cl_fecha) = @FECHA AND (A.tempo = 'N' or a.tempo='A')"
        Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
        Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
        Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
        Sql += " WHERE C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
        Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
        Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
        Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, D.vzon_nombre, B.Tel_vendedo "



        'Sql = " DECLARE @FECHA DATE, @FECHA2 Date 
        '       SET @FECHA = '" + Session("F1") + "'
        '       SET @FECHA2 = '" + Session("F2") + "'
        '        exec SP_MonitorVentasXDiaEnc @Fecha,@Fecha2 '%" + dllider.SelectedValue + "%','%" + txtCobrador.Value + "%','" + dlzona.SelectedValue + "','%" + TxtCliente1.Text.TrimEnd.TrimStart + "%','%" + TxtCliente2.Text.Trim + "%','%" + txtidentidadClienteApp.Text.Trim + "%','%" + txtCodClienteApp.Text.Trim + "%'"

        If dlRun.SelectedIndex = 0 Then

                Sql1 = "  DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                Sql1 += " exec [SP_MONITOR_VENTAS_l] @Fecha "
            ElseIf dlRun.SelectedIndex = 1 Then

                Sql1 = "  DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                Sql1 += " exec SP_MONITOR_VENTAS @Fecha "


            End If

        Datos1 = Conf1.EjecutaSql(Sql1)
        Session.Add("GridDetalle", Datos1.Tables(0))

        Datos = conf.EjecutaSql(Sql)
        Session.Add("GV", Datos)
        GvPrincipal.DataSource = Session("GV").Tables(0)
        GvPrincipal.DataBind()
        'End If
        'txtBuscar_TextChanged(sender, e)
    End Sub

    'Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.ServerClick
    '    Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
    '    Dim Sql As String

    '    Sql = " SELECT 
    '            A.CodigoM Codigo, 
    '            B.NombreM Cliente, 
    '            A.FechaDoc Fecha, 
    '            A.FechaVen Vencimiento, 
    '            A.NumDoc Documento, 
    '            C.EncarNom Vendedor, 
    '            A.TotalDoc - A.ValorApli Saldo,
    '            CASE 
    '             WHEN DATEDIFF(DAY,A.FechaVen,GETDATE()) < 30 THEN A.TotalDoc - A.ValorApli
    '             ELSE 0
    '            END Corriente,
    '            CASE 		
    '             WHEN DATEDIFF(DAY,A.FechaVen,GETDATE()) >= 30 AND DATEDIFF(DAY,A.FechaVen,GETDATE()) < 60 THEN A.TotalDoc - A.ValorApli
    '             ELSE 0
    '            END [30 a 60],
    '            CASE 		
    '             WHEN DATEDIFF(DAY,A.FechaVen,GETDATE()) >= 60 AND DATEDIFF(DAY,A.FechaVen,GETDATE()) < 90 THEN A.TotalDoc - A.ValorApli
    '             ELSE 0
    '            END [60 a 90],
    '            CASE 		
    '             WHEN DATEDIFF(DAY,A.FechaVen,GETDATE()) >= 90 AND DATEDIFF(DAY,A.FechaVen,GETDATE()) < 120 THEN A.TotalDoc - A.ValorApli
    '             ELSE 0
    '            END [90 a 120],
    '            CASE 		
    '             WHEN DATEDIFF(DAY,A.FechaVen,GETDATE()) > 120 THEN A.TotalDoc - A.ValorApli
    '             ELSE 0
    '            END [120+]
    '            FROM VenFacEnc A
    '            INNER JOIN Maestro B ON A.CodigoM = B.CodigoM
    '            LEFT JOIN Encargado C ON A.EncarCod = C.EncarCod
    '            WHERE A.Estado = 'O'
    '            AND A.EncarCod  LIKE '%" + dlVendedor.SelectedValue + "%'
    '            AND A.CodigoM + A.NombreM LIKE '%" + txtBuscar.Text + "%'
    '            ORDER BY A.CodigoM "
    '    Datos = Conf.EjecutaSql(Sql)
    '    Session.Add("GVR", Datos)

    '    GVtoExcel(Session("GVR").Tables(0), "CXC " + DateTime.Now.ToString("yyyy-MM-dd"))
    'End Sub



End Class
'    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut As String
'    Private Datos, Datos1, Datos2, Datos3 As DataSet
'    Private Total As Decimal = 0
'    Private Visitados As Decimal = 0
'    Private Ventas As Decimal = 0
'    Private Verdes As Decimal = 0
'    Private Recibos As Decimal = 0
'    Private Cobradores As Decimal = 0
'    Private Liquida, Liquida2 As String
'    ' Private Tabla As DataTable

'    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
'        If Session("Usuario") = "" Then
'            Response.Redirect("inicio.aspx")
'        End If

'        Usuario = Session("Usuario")
'        Clave = Session("Clave")
'        Servidor = Session("Servidor")
'        Bd = "AEVentas"
'        Usuario_Aut = Session("Usuario_Aut")
'        Clave_Aut = Session("Clave_Aut")
'        Session.Timeout = 90

'        'Tabla.Columns.Add("CodigoVendedor")
'        'Tabla.Columns.Add("Producto")
'        'Tabla.Columns.Add("Cantidad")
'        'Tabla.Columns.Add("ValorProducto")
'        'Tabla.Columns.Add("Costo")
'        'Tabla.Columns.Add("Vendedor")
'        'Tabla.Columns.Add("nombreVend")
'        'Tabla.Columns.Add("NombreCliente")
'        'Tabla.Columns.Add("CodigoCliente")
'        'Tabla.Columns.Add("Letra")
'        'Tabla.Columns.Add("Cuota")
'        'Tabla.Columns.Add("Valor")
'        'Tabla.Columns.Add("Total")

'        If Not IsPostBack Then
'            Session.Add("Orden", "0")
'            dlMostrar.Items.Add("POR DIA DE TRABAJO")
'            dlMostrar.Items.Add("POR LIQUIDACION")
'            dlMostrar.Items.Add("SIN LIQUIDACION")
'            dlMostrar.Items.Add("SIN ACTIVIDAD")
'            dlMostrar.SelectedIndex = 0

'            dlRun.Items.Add("False")
'            dlRun.Items.Add("True")
'            dlRun.SelectedIndex = 0

'            If Session("Reporte") = "Caja Ventas" Then
'                dlMostrar.Items.Add("SIN PROCESAR")
'                dlMostrar.Items.Add("POR PROCESADOS")
'                dlMostrar.SelectedIndex = 4
'            End If
'            dlMostrar.Items.Add("VERDES")



'            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
'            Dim SQL As String
'            SQL = " SELECT B.NOMBRE_VEND FROM FUNAMOR..VENDEDOR A INNER JOIN FUNAMOR..VENDEDOR B ON A.VEND_LIDER = B.Cod_vendedo WHERE A.VEND_STATUS = 'A' GROUP BY B.NOMBRE_VEND "
'            Datos2 = conf.EjecutaSql(SQL)

'            dllider.Items.Add("")
'            dllider.Items.Add("TODOS")
'            For I As Integer = 0 To Datos2.Tables(0).Rows.Count - 1
'                dllider.Items.Add(Datos2.Tables(0).Rows(I).Item("NOMBRE_VEND"))
'            Next

'            Dim conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
'            Dim SQL1 As String
'            SQL1 = " Select B.vzon_nombre From FUNAMOR..VENDEDOR A Left Join FUNAMOR..VZONA B ON A.vzon_codigo = B.vzon_codigo WHERE A.VEND_STATUS = 'A' Group BY B.vzon_nombre "
'            Datos = conf1.EjecutaSql(SQL1)

'            dlzona.Items.Add("")
'            For I As Integer = 0 To Datos.Tables(0).Rows.Count - 1
'                dlzona.Items.Add(Datos.Tables(0).Rows(I).Item("vzon_nombre"))
'            Next

'            Dim conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
'            Dim SQL2 As String
'            SQL2 = " SELECT LTRIM(RTRIM(A.Nombre_vend)) Nombre_vend FROM FUNAMOR..VENDEDOR A WHERE A.Cod_vendedo = '" + Usuario_Aut + "' "
'            Datos1 = conf2.EjecutaSql(SQL2)

'            If Datos1.Tables(0).Rows.Count > 0 Then
'                If Datos1.Tables(0).Rows(0).Item("Nombre_vend").ToString.Length > 0 Then
'                    dllider.SelectedItem.Text = Datos1.Tables(0).Rows(0).Item("Nombre_vend")
'                    dllider.Enabled = False
'                    dlRun.Items.Add("False")
'                    dlRun.Enabled = False
'                End If
'            End If

'            Dim conf3 As New Configuracion(Usuario, Clave, Bd, Servidor)
'            Dim SQL3 As String
'            SQL3 = " if exists (SELECT LTRIM(RTRIM(A.cod_vendedo)) Lider FROM FUNAMOR..VENDEDOR A WHERE  cod_vendedo <> vend_suplid and A.Cod_vendedo = '" + Usuario_Aut + "') select 1 ExisteLider else select 0 ExisteLider "
'            Datos3 = conf3.EjecutaSql(SQL3)

'            'If Datos3.Tables(0).Rows.Count > 0 Then
'            '    If Datos3.Tables(0).Rows(0).Item("ExisteLider").ToString.Length > 0 Then
'            Session.Add("UsuarioLider", Datos3.Tables(0).Rows(0).Item("ExisteLider"))
'            'End If



'        End If
'    End Sub




'    Protected Sub btnSalir_Click(sender As Object, e As ImageClickEventArgs) Handles btnSalir.Click
'        Response.Redirect("principal.aspx")
'    End Sub

'    Protected Sub btnArreglarVenta_click(sender As Object, e As EventArgs) Handles btnArreglarVenta.Click
'        PanelEditarVenta.Visible = True
'    End Sub


'    Protected Sub btnCancelarC_click(sender As Object, e As EventArgs) Handles btnCancelarC.Click
'        PanelEditarVenta.Visible = False
'    End Sub

'    Protected Sub btnCanModalCl_click(sender As Object, e As EventArgs) Handles btnCanModalCl.Click
'        PanelEditarVenta.Visible = False
'    End Sub






'    Protected Sub btnGuardarCamb_click(sender As Object, e As EventArgs) Handles btnGuardarCamb.Click
'        PanelConfirmacion.Visible = True
'    End Sub

'    Protected Sub BtnGuardarNo_click(sender As Object, e As EventArgs) Handles BtnGuardarNo.Click
'        PanelConfirmacion.Visible = False
'    End Sub


'    'Aqui va toda la Magia de Arreglar una venta
'    'Protected Sub BtnGuardarSi_click(sender As Object, e As EventArgs) Handles BtnGuardarSi.Click
'    '    PanelConfirmacion.Visible = False


'    'End Sub

'    Protected Sub btnBusVendEdt_click(sender As Object, e As EventArgs) Handles btnBusVendEdt.Click
'        PanelVendedoresEditar.Visible = True

'    End Sub
'    Protected Sub btnCerarPVend_click(sender As Object, e As EventArgs) Handles btnCerarPVend.Click
'        PanelVendedoresEditar.Visible = False

'    End Sub

'    Protected Sub btnRegresar_Click(sender As Object, e As ImageClickEventArgs) Handles btnRegresar.Click
'        Panel1.Visible = True
'        PanelImpresion.Visible = False
'        ifRepote.Dispose()
'        ifRepote.Src = ""
'    End Sub

'    'Protected Sub btnAddClient_Click() Handles btnAddClient.Click

'    '    PanelAddCliente.Visible = True

'    'End Sub

'    'Protected Sub btnCanAdd_Click(sender As Object, e As EventArgs) Handles btnCanAdd.Click

'    '    PanelAddCliente.Visible = False

'    'End Sub


'    Private Sub btnProcesar_Click(sender As Object, e As EventArgs) Handles btnProcesar.Click
'        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
'        Dim Sql As String

'        'Sql = "EXEC ExportImag '" + Session("Cobrador").ToString.TrimEnd + "' "
'        'conf.EjecutaSql(Sql)

'        GuardarImagen()

'        Sql = "EXEC ENVIARECIBOS '" + Session("Cobrador").ToString.TrimEnd + "','" + Usuario_Aut + "'"
'        conf.EjecutaSql(Sql)

'        btnBuscar_Click(sender, e)
'        PanelImpresion.Visible = False
'        Panel1.Visible = True

'    End Sub

'    Protected Sub txtBuscarVendedorV_TextChanged(sender As Object, e As EventArgs)
'        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
'        Dim Sql As String

'        Sql = "Select cod_vendedo Vendedor, Nombre_vend Nombre from FUNAMOR..VENDEDOR WHERE COD_VENDEDO + nombre_vend like '%" + txtBuscarVended.Text.TrimEnd + "%'"
'        Datos = conf.EjecutaSql(Sql)

'        gvvendEditVent.DataSource = Datos.Tables(0)
'        gvvendEditVent.DataBind()

'    End Sub



'    Sub Validar()
'        If Session("Usuario") = "AMPARO" Then
'            btnArreglarVenta.Enabled = True
'        End If

'    End Sub



'    Protected Sub txtVendEV_TextChanged(sender As Object, e As EventArgs)
'        Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
'        Dim Sql As String

'        Sql = "Select cod_vendedo Vendedor, Nombre_vend Nombre from FUNAMOR..VENDEDOR WHERE COD_VENDEDO + nombre_vend like '%" + txtCodVendEV.Text.TrimEnd + "%'"
'        Datos = conf.EjecutaSql(Sql)

'        If txtCodVendEV.Text.Length <> 4 Then
'            PanelVendedoresEditar.Visible = True
'            gvvendEditVent.DataSource = Datos.Tables(0)
'            gvvendEditVent.DataBind()
'        Else
'            If Datos.Tables(0).Rows.Count > 1 Then
'                txtCodVendEV.Text = Datos.Tables(0).Rows(0).Item("Vendedor")
'                txtnombreVendArr.InnerText = Datos.Tables(0).Rows(0).Item("Nombre")
'                txtBuscarVended.Text = ""
'            Else
'                PanelVendedoresEditar.Visible = True
'            End If
'        End If

'    End Sub


'    Private Sub GuardarImagen()
'        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
'        Dim Sql, FilePath As String

'        Sql = " Select A.FIRMA, B.identidad "
'        Sql += " From CONTRATON A "
'        Sql += " INNER Join CLIENTESN B ON A.Codigo_Clie = B.Codigo_Clie And B.Cl_VENDEDOR = A.cont_vended "
'        Sql += " INNER Join RECIBOS C ON A.CODIGO_CLIE = C.CODIGO_CLIE And A.cont_vended = C.RCODVEND "
'        Sql += " WHERE A.cont_vended = '" + Session("Cobrador") + "' AND C.LIQUIDA <> 'N' AND LIQUIDA2 = 'N' AND A.FIRMA IS NOT NULL "
'        Datos2 = conf.EjecutaSql(Sql)

'        For I As Integer = 0 To Datos2.Tables(0).Rows.Count - 1

'            Dim imageData As Byte() = DirectCast(Datos2.Tables(0).Rows(I).Item("FIRMA"), Byte())
'            If Not imageData Is Nothing Then
'                Using ms As New MemoryStream(imageData, 0, imageData.Length)
'                    ms.Write(imageData, 0, imageData.Length)
'                    FilePath = "C:\inetpub\wwwroot\firmas\" + Datos2.Tables(0).Rows(I).Item("identidad").ToString.TrimEnd + ".jpg"
'                    Image.FromStream(ms, True).Save(FilePath, Imaging.ImageFormat.Jpeg)
'                End Using
'            End If

'        Next
'    End Sub

'    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
'        Dim conf, Conf1 As New Configuracion(Usuario, Clave, "AEVENTAS", Servidor)
'        Dim Sql, Sql1 As String

'        If txtFecha.Text.Length > 0 Then
'            Session.Add("F1", txtFecha.Text)
'        Else
'            Session.Add("F1", DateTime.Now.ToString("yyyy-MM-dd"))
'        End If

'        If dlMostrar.SelectedIndex = 0 And dllider.SelectedValue = "TODOS" Then
'            gvMonitor.Columns(1).Visible = False
'            gvMonitor.Columns(2).Visible = False
'            lblVerdes.Visible = True
'            Sql = " DECLARE @FECHA DATE "
'            Sql += " SET @FECHA = '" + Session("F1") + "'"
'            Sql += " Select  B.VEND_LIDER [Codigo], c.Nombre_vend [Lider], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUM(A.Ventas) Ventas, SUM(A.Verdes) Verdes, D.vzon_nombre [Zona], C.Tel_vendedo Telefono "
'            Sql += " FROM ( "
'            Sql += " Select DISTINCT A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Ventas, 0 Verdes FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.RCODVEND = B.CONT_VENDED WHERE CONVERT(DATE,A.Fecha_recib) = @FECHA AND A.MARCA = 'N' "
'            Sql += " UNION ALL "
'            Sql += " Select A.VENDEDOR, 0, SUBSTRING(A.HORA,1,2) +':'+ SUBSTRING(A.HORA,3,2) +':00', 0 Recibos, 1 Visitados, 0 Ventas, 0 Verdes FROM NOVENTA A WHERE CONVERT(DATE,A.FECHA) = @FECHA "
'            Sql += " UNION ALL "
'            Sql += " Select A.CL_VENDEDOR, 0, CASE WHEN LEN(CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha))) = 1 THEN '0' + CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) ELSE CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) END + ':' + CASE WHEN LEN(CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha))) = 1 THEN '0' + CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) ELSE CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) END +':'+ CASE WHEN LEN(CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha))) = 1 THEN '0' + CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)) ELSE CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)) END, 0 Recibos, 1 Visitados, 0 Ventas, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Verdes FROM CLIENTESN A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.CL_VENDEDOR = B.CONT_VENDED WHERE CONVERT(DATE,A.cl_fecha) = @FECHA AND A.tempo = 'N'"
'            Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
'            Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
'            Sql += " LEFT JOIN FUNAMOR..VZONA D ON C.vzon_codigo = D.vzon_codigo "
'            Sql += " Group BY B.VEND_LIDER, C.Nombre_vend, D.vzon_nombre, C.Tel_vendedo "


'            '            Sql1 = " DECLARE @FECHA DATE "
'            '            Sql1 += " SET @FECHA = '" + Session("F1") + "' "
'            '            Sql1 += " SELECT DISTINCT A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + '
'            'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono, B.VEND_LIDER Lider, A.LONGITUD, A.LATITUD "
'            '            Sql1 += " FROM( "
'            '            Sql1 += " SELECT A.Num_doc, A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4, A.LONGITUD, A.LATITUD FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.RCODVEND = B.cont_vended WHERE CONVERT(DATE,A.Fecha_recib) = @FECHA "
'            '            Sql1 += " UNION ALL "
'            '            Sql1 += " SELECT 'NO VENTA', 0, SUBSTRING(A.HORA,1,2) +':'+ SUBSTRING(A.HORA,3,2) +':00', '', A.VENDEDOR, '', CONVERT(DATE,A.FECHA), A.liquida, 'N', 0, 0, 0, NULL, 0, 0, NULL, 0, 0, NULL, 0, 0, NULL, 0, 0, A.LONGITUD, A.LATITUD FROM NOVENTA A WHERE CONVERT(DATE,A.FECHA) = @FECHA "
'            '            Sql1 += " UNION ALL "
'            '            Sql1 += " SELECT 'PROSPECTO', 0, CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)), '', A.CL_VENDEDOR, A.Nombre_clie, CONVERT(DATE,A.cl_fecha), A.liquida, 'N', B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4, A.LATITUD, A.LONGITUD FROM CLIENTESN A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.CL_VENDEDOR = B.cont_vended WHERE CONVERT(DATE,A.cl_fecha) = @FECHA AND A.tempo = 'N' "
'            '            Sql1 += " ) A "
'            '            Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
'            '            Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "
'            If dlRun.SelectedIndex = 0 Then

'                Sql1 = "  DECLARE @FECHA DATE "
'                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
'                Sql1 += " exec [SP_MONITOR_VENTAS_l] @Fecha "
'            ElseIf dlRun.SelectedIndex = 1 Then

'                Sql1 = "  DECLARE @FECHA DATE "
'                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
'                Sql1 += " exec SP_MONITOR_VENTAS @Fecha "
'            End If
'            'Sql1 = "  DECLARE @FECHA DATE "
'            'Sql1 += " SET @FECHA = '" + Session("F1") + "' "
'            'Sql1 += " SELECT DISTINCT A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + ' 
'            'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono, B.VEND_LIDER Lider, A.LONGITUD, A.LATITUD  "
'            'Sql1 += " FROM( "
'            'Sql1 += " SELECT A.Num_doc, A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4, A.LONGITUD, A.LATITUD FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.RCODVEND = B.cont_vended WHERE CONVERT(DATE,A.Fecha_recib) = @FECHA "
'            'Sql1 += " UNION ALL "
'            'Sql1 += " SELECT 'NO VENTA', 0, SUBSTRING(A.HORA,1,2) +':'+ SUBSTRING(A.HORA,3,2) +':00', '', A.VENDEDOR, '', CONVERT(DATE,A.FECHA), A.liquida, 'N', 0, 0, 0, NULL, 0, 0, NULL, 0, 0, NULL, 0, 0, NULL, 0, 0, A.LONGITUD, A.LATITUD FROM NOVENTA A WHERE CONVERT(DATE,A.FECHA) = @FECHA "
'            'Sql1 += " UNION ALL "
'            'Sql1 += " SELECT 'PROSPECTO', 0, CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)), '', A.CL_VENDEDOR, A.Nombre_clie, CONVERT(DATE,A.cl_fecha), A.liquida, 'N', B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4, A.LATITUD, A.LONGITUD FROM CLIENTESN A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.CL_VENDEDOR = B.cont_vended WHERE CONVERT(DATE,A.cl_fecha) = @FECHA AND A.tempo = 'N' "
'            'Sql1 += " ) A "
'            'Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
'            'Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "

'        End If

'        'lblMsg.Text = Sql1
'        'Exit Sub


'        If dlMostrar.SelectedIndex = 0 And dllider.SelectedValue <> "TODOS" Then
'            gvMonitor.Columns(1).Visible = False
'            gvMonitor.Columns(2).Visible = False
'            lblVerdes.Visible = True

'            'If Usuario_Aut = Session("UsuarioLider") Then
'            '    gvMonitor.Columns(8).Visible = False
'            'End If
'            Sql = " DECLARE @FECHA DATE "
'            Sql += " SET @FECHA = '" + Session("F1") + "'"
'            Sql += " Select  A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUM(A.Ventas) Ventas, SUM(A.Verdes) Verdes, SUBSTRING(C.Nombre_vend,1,16) [Lider], D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
'            Sql += " FROM ( "
'            Sql += " Select DISTINCT A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Ventas, 0 Verdes FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.RCODVEND = B.CONT_VENDED WHERE CONVERT(DATE,A.Fecha_recib) = @FECHA AND A.MARCA = 'N' "
'            Sql += " UNION ALL "
'            Sql += " Select A.VENDEDOR, 0, SUBSTRING(A.HORA,1,2) +':'+ SUBSTRING(A.HORA,3,2) +':00', 0 Recibos, 1 Visitados, 0 Ventas, 0 Verdes FROM NOVENTA A WHERE CONVERT(DATE,A.FECHA) = @FECHA "
'            Sql += " UNION ALL "
'            Sql += " Select A.CL_VENDEDOR, 0, CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)), 0 Recibos, 1 Visitados, 0 Ventas, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Verdes FROM CLIENTESN A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.CL_VENDEDOR = B.CONT_VENDED WHERE CONVERT(DATE,A.cl_fecha) = @FECHA AND (A.tempo = 'N' or a.tempo='A')"
'            Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
'            Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
'            Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
'            Sql += " WHERE C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
'            Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
'            Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
'            Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, D.vzon_nombre, B.Tel_vendedo "




'            If dlRun.SelectedIndex = 0 Then

'                Sql1 = "  DECLARE @FECHA DATE "
'                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
'                Sql1 += " exec [SP_MONITOR_VENTAS_l] @Fecha "
'            ElseIf dlRun.SelectedIndex = 1 Then

'                'Sql1 = "  DECLARE @FECHA DATE "
'                'Sql1 += " SET @FECHA = '" + Session("F1") + "' "
'                'Sql1 += " exec SP_MONITOR_VENTAS @Fecha "


'                Sql1 = " DECLARE @FECHA DATE "
'                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
'                Sql1 += " SELECT DISTINCT A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + '
'                MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono, A.LONGITUD, A.LATITUD,cf.ClienteSistema   "
'                Sql1 += " FROM( "
'                Sql1 += " SELECT A.Num_doc, A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4, A.LONGITUD, A.LATITUD FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.RCODVEND = B.cont_vended WHERE CONVERT(DATE,A.Fecha_recib) = @FECHA "
'                Sql1 += " UNION ALL "
'                Sql1 += " SELECT 'NO VENTA', 0, SUBSTRING(A.HORA,1,2) +':'+ SUBSTRING(A.HORA,3,2) +':00', '', A.VENDEDOR, '', CONVERT(DATE,A.FECHA), A.liquida, 'N', 0, 0, 0, NULL, 0, 0, NULL, 0, 0, NULL, 0, 0, NULL, 0, 0, A.LONGITUD, A.LATITUD FROM NOVENTA A WHERE CONVERT(DATE,A.FECHA) = @FECHA "
'                Sql1 += " UNION ALL "
'                Sql1 += " SELECT 'PROSPECTO', 0, CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)), '', A.CL_VENDEDOR, A.Nombre_clie, CONVERT(DATE,A.cl_fecha), A.liquida, 'N', B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4, A.LATITUD, A.LONGITUD FROM CLIENTESN A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.CL_VENDEDOR = B.cont_vended WHERE CONVERT(DATE,A.cl_fecha) = @FECHA AND A.tempo = 'N' "
'                Sql1 += " ) A "
'                Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
'                Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "
'                Sql1 += " left join (select  cli.identidad,cli.Nombre_clie, "
'                Sql1 += " 	 stuff((Select top 6 ', ' + Codigo_clie "
'                Sql1 += "  from FUNAMOR..CLIENTES cl "
'                Sql1 += "  Where  CL.identidad  =  CLI.IDENTIDAD and cl.Nombre_clie = cli.Nombre_clie "
'                Sql1 += " For xml PATH('')),1, 2,'' "
'                Sql1 += " 	  )	as ClientesSistema "
'                Sql1 += "  From FUNAMOR..CLIENTES cli "
'                Sql1 += " Group By identidad, nombre_clie "
'                Sql1 += " )Cf on replace(cf.identidad,'-','') = c.identidad COLLATE Modern_Spanish_CI_AS or cf.Nombre_clie = replace(c.Nombre_clie,'.','') COLLATE Modern_Spanish_CI_AS "


'            End If
'        End If
'        If dlMostrar.SelectedIndex = 1 Then
'            gvMonitor.Columns(1).Visible = False
'            gvMonitor.Columns(2).Visible = False

'            lblVerdes.Visible = False
'            'If Usuario_Aut = Session("UsuarioLider") Then
'            '    gvMonitor.Columns(8).Visible = False
'            ''End If

'            Sql = " DECLARE @FECHA DATE "
'            Sql += " SET @FECHA = '" + Session("F1") + "'"
'            Sql += " SELECT A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUM(A.Ventas) Ventas, SUBSTRING(C.Nombre_vend,1,16) [Lider], SUBSTRING(A.liquida,1,4) +'/'+ SUBSTRING(A.liquida,5,2) +'/'+ SUBSTRING(A.liquida,7,2) +' '+ SUBSTRING(A.liquida,9,2) +':'+ SUBSTRING(A.liquida,11,2) Liquida, CASE WHEN A.liquida2 != 'N' THEN SUBSTRING(A.liquida2,1,4) +'/'+ SUBSTRING(A.liquida2,5,2) +'/'+ SUBSTRING(A.liquida2,7,2) +' '+ SUBSTRING(A.liquida2,9,2) +':'+ SUBSTRING(A.liquida2,11,2) ELSE 'N' END Procesado, D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
'            Sql += " FROM ( "
'            Sql += " Select DISTINCT A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Ventas, A.liquida, A.liquida2 FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.RCODVEND = B.CONT_VENDED WHERE CONVERT(DATE,SUBSTRING(A.liquida,1,8)) = @FECHA AND A.MARCA = 'N' "
'            Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
'            Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
'            Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
'            Sql += " WHERE C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
'            Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
'            Sql += " AND A.liquida != 'N' "
'            Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
'            Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, A.Liquida, A.liquida2, D.vzon_nombre, B.Tel_vendedo "



'            If dlRun.SelectedIndex = 0 Then ''Sin Consulta Cliente Sistema


'                Sql1 = "  DECLARE @FECHA DATE "
'                Sql1 += " SET @FECHA = '" + Session("F1") + "'"
'                Sql1 += " EXEC [SP_MONITOR_VENTAS_X_LIQUID] @Fecha "

'            ElseIf dlRun.SelectedIndex = 1 Then


'                Sql1 = "  DECLARE @FECHA DATE "
'                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
'                Sql1 += " EXEC SP_MONITOR_VENTAS_X_LIQUID_Sistema  @Fecha "




'                'Sql1 = "  DECLARE @FECHA DATE "
'                'Sql1 += " SET @FECHA = '" + Session("F1") + "'"
'                'Sql1 += " EXEC SP_MONITOR_VENTAS_X_LIQUID_Sistema  @Fecha "

'                '    Sql1 = " DECLARE @FECHA DATE "
'                '    Sql1 += " SET @FECHA = '" + Session("F1") + "' "
'                'Sql1 += " SELECT DISTINCT A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AECobros..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono "
'                'Sql1 += " ,cf.ClientesSistema FROM( "
'                'Sql1 += " SELECT A.Num_doc, A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4 FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.RCODVEND = B.cont_vended WHERE CONVERT(DATE,SUBSTRING(A.liquida,1,8)) = @FECHA "
'                'Sql1 += " ) A "
'                'Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
'                'Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "
'                '    Sql1 += " left join (select distinct cli.identidad,cli.Nombre_clie, "
'                '    Sql1 += " 	 stuff((Select top 6 ', ' + Codigo_clie "
'                '    Sql1 += "  from FUNAMOR..CLIENTES cl "
'                'Sql1 += "  Where  CL.identidad  =  CLI.IDENTIDAD and cl.Nombre_clie = cli.Nombre_clie "
'                'Sql1 += " For xml PATH('')),1, 2,'' "
'                'Sql1 += " 	  )	as ClientesSistema "
'                'Sql1 += "  From FUNAMOR..CLIENTES cli "
'                'Sql1 += " Group By identidad, nombre_clie "
'                'Sql1 += " )Cf on replace(cf.identidad,'-','') = c.identidad COLLATE Modern_Spanish_CI_AS or cf.Nombre_clie = c.Nombre_clie COLLATE Modern_Spanish_CI_AS "
'                '    Sql1 += " WHERE A.liquida != 'N' "



'            End If
'            'lblMsg.Text = Sql1
'            'Exit Sub
'        End If



'        If dlMostrar.SelectedIndex = 2 Then
'            gvMonitor.Columns(1).Visible = False
'            gvMonitor.Columns(2).Visible = False
'            lblVerdes.Visible = False
'            If Usuario_Aut.ToUpper = "MANAGER" Then
'                gvMonitor.Columns(1).Visible = True
'            End If


'            Sql = " DECLARE @FECHA DATE "
'            Sql += " SET @FECHA = '" + Session("F1") + "'"
'            Sql += " SELECT A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUM(A.Ventas) Ventas, SUBSTRING(C.Nombre_vend,1,16) [Lider], A.Liquida, A.Liquida2 Procesado, D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
'            Sql += " FROM ( "
'            Sql += " Select DISTINCT A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Ventas, A.liquida, A.liquida2 FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.RCODVEND = B.CONT_VENDED WHERE A.MARCA = 'N' AND A.liquida = 'N' AND CONVERT(DATE,A.Fecha_recib) <= @FECHA "
'            Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
'            Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
'            Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
'            Sql += " WHERE C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
'            Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
'            Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
'            Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, A.Liquida, A.liquida2, D.vzon_nombre, B.Tel_vendedo "


'            If dlRun.SelectedIndex = 0 Then

'                Sql1 = "  DECLARE @FECHA DATE "
'                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
'                Sql1 += " exec SP_MONITOR_VENTAS_SIN_LIQUID @Fecha "

'            ElseIf dlRun.SelectedIndex = 1 Then


'                Sql1 = "  DECLARE @FECHA DATE "
'                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
'                Sql1 += " exec [SP_MONITOR_VENTAS_SIN_LIQUID_SISTEMA] @Fecha "



'                'Sql1 = " DECLARE @FECHA DATE "
'                'Sql1 += " SET @FECHA = '" + Session("F1") + "' "
'                'Sql1 += " SELECT DISTINCT A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,40) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono  "
'                'Sql1 += " ,cf.ClientesSistema FROM( "
'                'Sql1 += " SELECT A.Num_doc, A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4 FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.RCODVEND = B.cont_vended WHERE A.Liquida = 'N' AND CONVERT(DATE,A.Fecha_recib) <= @FECHA "
'                'Sql1 += " ) A "
'                'Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
'                'Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "
'                'Sql1 += " left join (select cli.identidad,cli.Nombre_clie, "
'                'Sql1 += " 	 stuff((Select top 6 ', ' + Codigo_clie "
'                'Sql1 += "  from FUNAMOR..CLIENTES cl "
'                'Sql1 += "  Where  CL.identidad  =  CLI.IDENTIDAD and cl.Nombre_clie = cli.Nombre_clie "
'                'Sql1 += " For xml PATH('')),1, 2,'' "
'                'Sql1 += " 	  )	as ClientesSistema "
'                'Sql1 += "  From FUNAMOR..CLIENTES cli "
'                'Sql1 += " Group By identidad, nombre_clie "
'                'Sql1 += " )Cf on replace(cf.identidad,'-','') = c.identidad COLLATE Modern_Spanish_CI_AS or cf.Nombre_clie = c.Nombre_clie COLLATE Modern_Spanish_CI_AS "
'            End If


'        End If

'        If dlMostrar.SelectedIndex = 3 Then
'            gvMonitor.Columns(1).Visible = False
'            gvMonitor.Columns(2).Visible = False
'            lblVerdes.Visible = False

'            Sql = " DECLARE @FECHA DATE "
'            Sql += " SET @FECHA = '" + Session("F1") + "'"
'            Sql += " SELECT A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUBSTRING(C.Nombre_vend,1,16) [Lider], A.Liquida, A.Liquida2 Procesado, D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
'            Sql += " FROM ( "
'            Sql += " SELECT A.cveage RCODVEND, 0 Por_lempira, '00:00:00' rhora, 0 Recibos, 0 Visitados, 'N' liquida, 'N' liquida2 FROM MVSAGEN A WHERE A.cveage COLLATE Modern_Spanish_CI_AS NOT IN (SELECT A.RCODVEND FROM RECIBOS A WHERE CONVERT(DATE,A.Fecha_recib) = @FECHA GROUP BY A.RCODVEND UNION ALL SELECT A.VENDEDOR FROM NOVENTA A WHERE CONVERT(DATE,A.FECHA) = @FECHA GROUP BY A.VENDEDOR UNION ALL SELECT A.CL_VENDEDOR FROM CLIENTESN A WHERE CONVERT(DATE,A.cl_fecha) = @FECHA AND A.tempo = 'N' GROUP BY A.CL_VENDEDOR ) "
'            Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS AND B.VEND_STATUS = 'A' "
'            Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
'            Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
'            Sql += " WHERE B.VEND_STATUS = 'A'"
'            Sql += " AND C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
'            Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
'            Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
'            Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, A.Liquida, A.liquida2, D.vzon_nombre, B.Tel_vendedo "


'            If dlRun.SelectedIndex = 1 Then

'                Sql1 = "  DECLARE @FECHA DATE "
'                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
'                Sql1 += " SELECT DISTINCT TOP 1 A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono  "
'                Sql1 += " ,cf.ClientesSistema FROM( "
'                Sql1 += " SELECT A.Num_doc, A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4 FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.RCODVEND = B.cont_vended WHERE A.Liquida = 'N' AND CONVERT(DATE,A.Fecha_recib) = @FECHA "
'                Sql1 += " ) A "
'                Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
'                Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "
'                Sql1 += " left join (select  cli.identidad,cli.Nombre_clie, "
'                Sql1 += " 	 stuff((Select top 6 ', ' + Codigo_clie "
'                Sql1 += "  from FUNAMOR..CLIENTES cl "
'                Sql1 += "  Where  CL.identidad  =  CLI.IDENTIDAD and cl.Nombre_clie = cli.Nombre_clie "
'                Sql1 += " For xml PATH('')),1, 2,'' "
'                Sql1 += " 	  )	as ClientesSistema "
'                Sql1 += "  From FUNAMOR..CLIENTES cli "
'                Sql1 += " Group By identidad, nombre_clie "
'                Sql1 += " )Cf on replace(cf.identidad,'-','') = c.identidad COLLATE Modern_Spanish_CI_AS or replace(cf.Nombre_clie,'.','') = replace(c.Nombre_clie,'.','') COLLATE Modern_Spanish_CI_AS "

'            ElseIf dlRun.SelectedIndex = 0 Then
'                Sql1 = "  DECLARE @FECHA DATE "
'                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
'                Sql1 += " SELECT DISTINCT TOP 1 A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono  "
'                Sql1 += " ,cf.ClientesSistema FROM( "
'                Sql1 += " SELECT A.Num_doc, A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4 FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.RCODVEND = B.cont_vended WHERE A.Liquida = 'N' AND CONVERT(DATE,A.Fecha_recib) = @FECHA "
'                Sql1 += " ) A "
'                Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
'                Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "
'                Sql1 += " left join (select  cli.identidad,cli.Nombre_clie, "
'                Sql1 += " 	 stuff((Select top 6 ', ' + Codigo_clie "
'                Sql1 += "  from FUNAMOR..CLIENTES cl "
'                Sql1 += "  Where  CL.identidad  =  CLI.IDENTIDAD and cl.Nombre_clie = cli.Nombre_clie "
'                Sql1 += " For xml PATH('')),1, 2,'' "
'                Sql1 += " 	  )	as ClientesSistema "
'                Sql1 += "  From FUNAMOR..CLIENTES cli "
'                Sql1 += " Group By identidad, nombre_clie "
'                Sql1 += " )Cf on replace(cf.identidad,'-','') = c.identidad COLLATE Modern_Spanish_CI_AS or cf.Nombre_clie = replace(c.Nombre_clie,'.','') COLLATE Modern_Spanish_CI_AS "
'            End If
'        End If

'        If dlMostrar.SelectedIndex = 4 Then
'            gvMonitor.Columns(1).Visible = True
'            gvMonitor.Columns(2).Visible = False
'            lblVerdes.Visible = False

'            Sql = " DECLARE @FECHA DATE "
'            Sql += " SET @FECHA = '" + Session("F1") + "'"
'            Sql += " SELECT A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUBSTRING(C.Nombre_vend,1,16) [Lider], SUBSTRING(A.liquida,1,4) +'/'+ SUBSTRING(A.liquida,5,2) +'/'+ SUBSTRING(A.liquida,7,2) +' '+ SUBSTRING(A.liquida,9,2) +':'+ SUBSTRING(A.liquida,11,2) Liquida, CASE WHEN A.liquida2 != 'N' THEN SUBSTRING(A.liquida2,1,4) +'/'+ SUBSTRING(A.liquida2,5,2) +'/'+ SUBSTRING(A.liquida2,7,2) +' '+ SUBSTRING(A.liquida2,9,2) +':'+ SUBSTRING(A.liquida2,11,2) ELSE 'N' END Procesado, D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
'            Sql += " FROM ( "
'            Sql += " Select A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, A.liquida, A.liquida2 FROM RECIBOS A WHERE A.MARCA = 'N' AND A.liquida <> 'N' AND A.liquida2 = 'N' AND CONVERT(DATE,A.Fecha_recib) <= @FECHA "
'            Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
'            Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
'            Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
'            Sql += " WHERE C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
'            Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
'            Sql += " AND A.liquida != 'N' "
'            Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
'            Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, A.Liquida, A.liquida2, D.vzon_nombre, B.Tel_vendedo "


'            If dlRun.SelectedIndex = 1 Then



'                Sql1 = "  DECLARE @FECHA DATE "
'                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
'                Sql1 += " exec  [SP_MONITOR_VENTAS_SinProcesar]  @Fecha "


'                'lblMsg.Text = Sql1

'                'Exit Sub

'                'Sql1 = " DECLARE @FECHA DATE "
'                'Sql1 += " SET @FECHA = '" + Session("F1") + "' "
'                'Sql1 += " SELECT DISTINCT A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono "
'                'Sql1 += " ,isnull(cf.clientesSistema,'-'clientesSistema FROM( "
'                'Sql1 += " SELECT  distinct  case when B.SERVI1DES like '%lote%' or B.SERVI1DES like '%crem%' "
'                'Sql1 += " then 'P01'  +''+SUBSTRING(Num_doc,4,14) "
'                'Sql1 += " when (B.SERVI1DES not like '%lote%' or B.SERVI1DES not like '%crem%') AND SUBSTRING(Num_doc,1,3) = 'P01' 			 "
'                'Sql1 += " then rtrim(ltrim(codzona))+''+SUBSTRING(Num_doc,4,14) "
'                'Sql1 += " when (B.SERVI1DES not like '%lote%' or B.SERVI1DES not like '%crem%') AND SUBSTRING(Num_doc,1,3) <> 'P01' "
'                'Sql1 += " THEN rtrim(ltrim(codzona))+''+SUBSTRING(Num_doc,4,14) ELSE A.Num_doc end Num_Doc, "
'                'Sql1 += " A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4 "
'                'Sql1 += " FROM RECIBOS A "
'                'Sql1 += " LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.RCODVEND = B.cont_vended "
'                'Sql1 += " LEFT join CLIENTESN Cc ON CC.Codigo_clie = A.Codigo_clie AND a.RCODVEND = cc.CL_VENDEDOR "
'                'Sql1 += " INNER JOIN DEPTOZONA DP ON DP.desdepto = CC.departa AND dp.desmuni = Cc.municipio "
'                'Sql1 += " WHERE A.Liquida <> 'N' AND A.Liquida2 = 'N' AND CONVERT(DATE,A.Fecha_recib) <= @FECHA "
'                'Sql1 += " ) A "
'                'Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
'                'Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "
'                'Sql1 += " left join (select cli.identidad,cli.Nombre_clie, "
'                'Sql1 += " 	 stuff((Select top 6 ', ' + Codigo_clie "
'                'Sql1 += "  from FUNAMOR..CLIENTES cl "
'                'Sql1 += "  Where  CL.identidad  =  CLI.IDENTIDAD and cl.Nombre_clie = cli.Nombre_clie "
'                'Sql1 += " For xml PATH('')),1, 2,'' "
'                'Sql1 += " 	  )	as ClientesSistema "
'                'Sql1 += "  From FUNAMOR..CLIENTES cli "
'                'Sql1 += " Group By identidad, nombre_clie "
'                'Sql1 += " )Cf on replace(replace(cf.identidad,'-',''),'.','') = c.identidad COLLATE Modern_Spanish_CI_AS 	or replace(lTRIM(RTRIM(cf.Nombre_clie)),'.','') = REPLACE(c.Nombre_clie,'.','') COLLATE Modern_Spanish_CI_AS  "

'            ElseIf dlRun.SelectedIndex = 0 Then

'                Sql1 = " DECLARE @FECHA DATE "
'                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
'                Sql1 += " SELECT DISTINCT A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono "
'                Sql1 += " ,''clientesSistema FROM( "
'                Sql1 += " SELECT  distinct  case when B.SERVI1DES like '%lote%' or B.SERVI1DES like '%crem%' or B.SERVI1DES like '%placa%' "
'                Sql1 += " then 'P01'  +''+SUBSTRING(Num_doc,4,14) "
'                Sql1 += " when (B.SERVI1DES not like '%lote%' or B.SERVI1DES not like '%crem%' or B.SERVI1DES  not like  '%placa%'  ) AND SUBSTRING(Num_doc,1,3) = 'P01' 			 "
'                Sql1 += " then rtrim(ltrim(codzona))+''+SUBSTRING(Num_doc,4,14) "
'                Sql1 += " when (B.SERVI1DES not like '%lote%' or B.SERVI1DES not like '%crem%' or  B.SERVI1DES not like  '%placa%' ) AND SUBSTRING(Num_doc,1,3) <> 'P01' "
'                Sql1 += " THEN rtrim(ltrim(codzona))+''+SUBSTRING(Num_doc,4,14) ELSE A.Num_doc end Num_Doc, "
'                Sql1 += " A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4 "
'                Sql1 += " FROM RECIBOS A "
'                Sql1 += " LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie And A.RCODVEND = B.cont_vended "
'                Sql1 += " LEFT join CLIENTESN Cc ON CC.Codigo_clie = A.Codigo_clie And a.RCODVEND = cc.CL_VENDEDOR "
'                Sql1 += " INNER JOIN DEPTOZONA DP ON DP.desdepto = CC.departa And dp.desmuni = Cc.municipio "
'                Sql1 += " WHERE A.Liquida <> 'N' AND A.Liquida2 = 'N' AND CONVERT(DATE,A.Fecha_recib) <= @FECHA "
'                Sql1 += " ) A "
'                Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
'                Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "
'                'Sql1 += " left join (select cli.identidad,cli.Nombre_clie, "
'                'Sql1 += " 	 stuff((Select top 6 ', ' + Codigo_clie "
'                'Sql1 += "  from FUNAMOR..CLIENTES cl "
'                'Sql1 += "  Where  CL.identidad  =  CLI.IDENTIDAD and cl.Nombre_clie = cli.Nombre_clie "
'                'Sql1 += " For xml PATH('')),1, 2,'' "
'                'Sql1 += " 	  )	as ClientesSistema "
'                'Sql1 += "  From FUNAMOR..CLIENTES cli "
'                'Sql1 += " Group By identidad, nombre_clie "
'                'Sql1 += " )Cf on replace(replace(cf.identidad,'-',''),'.','') = c.identidad COLLATE Modern_Spanish_CI_AS 	or replace(replace(replace(lTRIM(RTRIM(cf.Nombre_clie)),'.',''),'á','a'),'É','E') = REPLACE(c.Nombre_clie,'.','') COLLATE Modern_Spanish_CI_AS  "


'                'lblMsg.Text = Sql1
'                'Exit Sub
'            End If

'        End If

'        If dlMostrar.SelectedIndex = 5 Then
'            gvMonitor.Columns(2).Visible = True
'            gvMonitor.Columns(1).Visible = False
'            lblVerdes.Visible = False

'            Sql = " DECLARE @FECHA DATE "
'            Sql += " SET @FECHA = '" + Session("F1") + "'"
'            Sql += " SELECT A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUBSTRING(C.Nombre_vend,1,16) [Lider], SUBSTRING(A.liquida,1,4) +'/'+ SUBSTRING(A.liquida,5,2) +'/'+ SUBSTRING(A.liquida,7,2) +' '+ SUBSTRING(A.liquida,9,2) +':'+ SUBSTRING(A.liquida,11,2) Liquida, CASE WHEN A.liquida2 != 'N' THEN SUBSTRING(A.liquida2,1,4) +'/'+ SUBSTRING(A.liquida2,5,2) +'/'+ SUBSTRING(A.liquida2,7,2) +' '+ SUBSTRING(A.liquida2,9,2) +':'+ SUBSTRING(A.liquida2,11,2) ELSE 'N' END Procesado, D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
'            Sql += " FROM ( "
'            Sql += " Select A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, A.liquida, A.liquida2 FROM RECIBOS A WHERE CONVERT(DATE,SUBSTRING(A.liquida2,1,8)) = @FECHA AND A.MARCA = 'N' "
'            Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
'            Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
'            Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
'            Sql += " WHERE C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
'            Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
'            Sql += " AND A.liquida2 != 'N' "
'            Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
'            Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, A.Liquida, A.liquida2, D.vzon_nombre, B.Tel_vendedo "

'            Sql1 = " DECLARE @FECHA DATE "
'            Sql1 += " SET @FECHA = '" + Session("F1") + "' "
'            Sql1 += " SELECT DISTINCT A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono  "
'            Sql1 += " ,'' ClientesSistema FROM( "
'            Sql1 += " SELECT  distinct  case when B.SERVI1DES like '%lote%' or B.SERVI1DES like '%crem%' or B.SERVI1DES like '%placa%' "
'            Sql1 += " then 'P01'  +''+SUBSTRING(Num_doc,4,14) "
'            Sql1 += " when (B.SERVI1DES not like '%lote%' or B.SERVI1DES not like '%crem%' or B.SERVI1DES not like '%placa%') AND SUBSTRING(Num_doc,1,3) = 'P01' 			 "
'            Sql1 += " then rtrim(ltrim(codzona))+''+SUBSTRING(Num_doc,4,14) "
'            Sql1 += " when (B.SERVI1DES not like '%lote%' or B.SERVI1DES not like '%crem%' not like B.SERVI1DES like '%placa%') AND SUBSTRING(Num_doc,1,3) <> 'P01' "
'            Sql1 += " THEN rtrim(ltrim(codzona))+''+SUBSTRING(Num_doc,4,14) ELSE A.Num_doc end Num_Doc, "
'            Sql1 += " A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4 "
'            Sql1 += " FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.RCODVEND = B.cont_vended "
'            Sql1 += " LEFT join CLIENTESN Cc ON CC.Codigo_clie = A.Codigo_clie And a.RCODVEND = cc.CL_VENDEDOR "
'            Sql1 += " INNER JOIN DEPTOZONA DP ON DP.desdepto = CC.departa And dp.desmuni = Cc.municipio "
'            Sql1 += " WHERE CONVERT(DATE,SUBSTRING(A.liquida2,1,8)) = @FECHA "
'            Sql1 += " ) A "
'            Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
'            Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "
'            'Sql1 += " left join (select cli.identidad,cli.Nombre_clie, "
'            'Sql1 += " 	 stuff((Select top 6 ', ' + Codigo_clie "
'            'Sql1 += "  from FUNAMOR..CLIENTES cl "
'            'Sql1 += "  Where  CL.identidad  =  CLI.IDENTIDAD and cl.Nombre_clie = cli.Nombre_clie "
'            'Sql1 += " For xml PATH('')),1, 2,'' "
'            'Sql1 += " 	  )	as ClientesSistema "
'            'Sql1 += "  From FUNAMOR..CLIENTES cli "
'            'Sql1 += " Group By identidad, nombre_clie "
'            'Sql1 += " )Cf on replace(replace(cf.identidad,'-',''),'.','') = c.identidad COLLATE Modern_Spanish_CI_AS or REPLACE(REPLACE(replace(replace(replace(replace(lTRIM(RTRIM(cf.Nombre_clie)),'.',''),'á','a'),'É','E'),'Í','I'),'Ó','O'),'Ú','U')  = REPLACE(REPLACE(replace(replace(replace(replace(lTRIM(RTRIM(c.Nombre_clie)),'.',''),'á','a'),'É','E'),'Í','I'),'Ó','O'),'Ú','U') COLLATE Modern_Spanish_CI_AS "
'            Sql1 += " WHERE A.liquida2 != 'N' "
'        End If

'        'lblMsg.Text = Sql1
'        'Exit Sub

'        If dlMostrar.SelectedValue = "VERDES" Then
'            gvMonitor.Columns(1).Visible = False
'            gvMonitor.Columns(2).Visible = False

'            Sql = " DECLARE @FECHA DATE "
'            'Sql += " SET @FECHA = '" + Session("F1") + "'"
'            Sql += " SELECT A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Verdes, SUM(A.Ventas) Ventas, SUBSTRING(C.Nombre_vend,1,16) [Lider], D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
'            Sql += " FROM ( "
'            'Sql += " Select DISTINCT A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Ventas, A.liquida, A.liquida2 FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.RCODVEND = B.CONT_VENDED WHERE A.MARCA = 'N' AND A.liquida = 'N' AND CONVERT(DATE,A.Fecha_recib) <= @FECHA "
'            Sql += " Select A.CL_VENDEDOR RCODVEND, 0 Por_lempira, CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)) rhora, 0 Recibos, 1 Visitados, 0 Ventas FROM CLIENTESN A WHERE A.tempo = 'N'"
'            Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
'            Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
'            Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
'            Sql += " WHERE C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
'            Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
'            Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
'            Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, D.vzon_nombre, B.Tel_vendedo "

'            Sql1 = " DECLARE @FECHA DATE "
'            Sql1 += " SET @FECHA = '" + Session("F1") + "' "
'            Sql1 += " SELECT DISTINCT A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono "
'            Sql1 += " ,'-' ClientesSistema FROM( "
'            Sql1 += " SELECT 'PROSPECTO' Num_doc, 0 Por_lempira, CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)) rhora, A.Codigo_clie, A.CL_VENDEDOR RCODVEND, '' Concepto, A.Nombre_clie, CONVERT(DATE,A.cl_fecha) Fecha, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4, A.LONGITUD, A.LATITUD FROM CLIENTESN A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.CL_VENDEDOR = B.cont_vended WHERE A.tempo = 'N'  "
'            Sql1 += " ) A "
'            Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
'            Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "

'        End If

'        'lblMsg.Text = Sql1
'        'Exit Sub

'        Datos = conf.EjecutaSql(Sql)

'        If Usuario_Aut = "JULIOCAJA" Or Usuario_Aut = "MANAGER" Or Usuario_Aut = "MDERAS" Or Usuario_Aut = "ABLANDON" Or Usuario_Aut = "JULIO" And (Datos.Tables(0).Rows.Count = 1 And dlMostrar.SelectedIndex = 1) Then
'            Datos1 = Conf1.EjecutaSql(Sql1)
'            Session.Add("GVDetalle", Datos1.Tables(0))

'            Session("GVDetalle").DefaultView.RowFilter = "RCODVEND ='" + txtCobrador.Value + "'"
'            Session("GVDetalle").DefaultView.Sort = "Fecha Asc, Hora Asc"

'            gvDetalle2.DataSource = Session("GVDetalle")
'            gvDetalle2.DataBind()

'            Datos = conf.EjecutaSql(Sql)
'            Datos.Tables(0).DefaultView.Sort = "Cobrado Desc"
'            Session.Add("GV", Datos.Tables(0))
'            gvMonitor2.DataSource = Session("GV")
'            gvMonitor2.DataBind()

'            gvMonitor2.Visible = True
'            gvDetalle2.Visible = True
'            gvMonitor.Visible = False

'        ElseIf dlMostrar.SelectedIndex = 2 And Datos.Tables(0).Rows.Count = 1 Then
'            Datos1 = Conf1.EjecutaSql(Sql1)
'            Session.Add("GVDetalle", Datos1.Tables(0))

'            Session("GVDetalle").DefaultView.RowFilter = "RCODVEND ='" + txtCobrador.Value + "'"
'            Session("GVDetalle").DefaultView.Sort = "Fecha Asc, Hora Asc"

'            gvDetalle2.DataSource = Session("GVDetalle")
'            gvDetalle2.DataBind()

'            Datos = conf.EjecutaSql(Sql)
'            Datos.Tables(0).DefaultView.Sort = "Cobrado Desc"
'            Session.Add("GV", Datos.Tables(0))
'            gvMonitor2.DataSource = Session("GV")
'            gvMonitor2.DataBind()

'            gvMonitor2.Visible = True
'            gvDetalle2.Visible = True
'            gvMonitor.Visible = False
'            gvDetalle2.Columns(2).Visible = True
'            gvDetalle2.Columns(1).Visible = False

'        ElseIf dlMostrar.SelectedIndex = 0 And Datos.Tables(0).Rows.Count = 1 Then
'            Datos1 = Conf1.EjecutaSql(Sql1)
'            Session.Add("GVDetalle", Datos1.Tables(0))

'            Session("GVDetalle").DefaultView.RowFilter = "RCODVEND ='" + txtCobrador.Value + "'"
'            Session("GVDetalle").DefaultView.Sort = "Fecha Asc, Hora Asc"

'            gvDetalle2.DataSource = Session("GVDetalle")
'            gvDetalle2.DataBind()

'            Datos = conf.EjecutaSql(Sql)
'            Datos.Tables(0).DefaultView.Sort = "Cobrado Desc"
'            Session.Add("GV", Datos.Tables(0))
'            gvMonitor2.DataSource = Session("GV")
'            gvMonitor2.DataBind()

'            gvMonitor2.Visible = True
'            gvDetalle2.Visible = True
'            gvMonitor.Visible = False
'            gvDetalle2.Columns(0).Visible = False
'            gvDetalle2.Columns(1).Visible = True

'        Else
'            Datos1 = Conf1.EjecutaSql(Sql1)
'            Session.Add("GVDetalle", Datos1.Tables(0))

'            Datos = conf.EjecutaSql(Sql)
'            Datos.Tables(0).DefaultView.Sort = "Cobrado Desc"
'            Session.Add("GV", Datos.Tables(0))
'            gvMonitor.DataSource = Session("GV")
'            gvMonitor.DataBind()
'            gvMonitor2.Visible = False
'            gvDetalle2.Visible = False
'            gvMonitor.Visible = True
'        End If

'        lblHora.Text = "Actualizado: " + System.DateTime.Now.ToShortTimeString
'    End Sub


'    'Campo Codigo de cliente Modal de Editar Venta
'    Protected Sub txtCodClienteapp_TextChanged(sender As Object, e As EventArgs)
'        Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
'        Dim Sql As String


'        Sql = "Select distinct r.Codigo_clie Codigo,r.RNOMBRECLI Nombre, 
'		Case when r.liquida = 'N' THEN 'Sin Liquidacion'
'		when r.liquida <> 'N' then 'Liquidada' end Estatus,SUBSTRING(Num_doc,1,3)Empresa , ISNULL(c.identidad,'') Identidad, 
'		concat(ltrim(isnull(Dir_cliente,'')),ltrim(isnull(Dir2_client,'')),ltrim(isnull(Dir3_client,''))+ltrim(isnull(Dir4_client,'')))Direccion,
'        r.Num_doc Documento, r.Fecha_recib Fecha, R.RCODVEND, r.Por_lempira Prima,
'        isnull(co.CONT_NUMCUO, 0) NumCuotas,isnull(co.CONT_VALOR,0) Valor, 
'		isnull(CONT_CANTI, 0)Cantidad,isnull(co.CONT_SERVI,'0')IdServicio,isnull(SERVI1DES,'')Servicio,
'		isnull(CONT_VALCUO, 0) ValorCuota		
'		From RECIBOS R
'		Left Join CLIENTESN C ON C.Codigo_clie = R.Codigo_clie And R.RCODVEND = C.CL_VENDEDOR
'        Left Join CONTRATON CO ON CO.Codigo_clie = R.Codigo_clie And CONT_NUMERO = R.Codigo_clie And CO.cont_vended = R.RCODVEND
'        WHERE Liquida2 = 'N' and marca = 'N' AND r.codigo_clie = '" + txtCodClienteapp.Text + "'"


'    End Sub


'    'Campo Buscar del Modal de Clientes
'    Protected Sub txtBuscarCliente_TextChanged(sender As Object, e As EventArgs)
'        Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
'        Dim Sql As String

'        Sql = " Select distinct r.Codigo_clie Codigo,r.RNOMBRECLI Nombre,R.RCODVEND CodVendedor,ven.nombre_vend,  
'		Case when r.liquida = 'N' THEN 'Sin Liquidacion'
'		when r.liquida <> 'N' then 'Liquidada' end Estatus,SUBSTRING(Num_doc,1,3)Empresa , ISNULL(c.identidad,'') Identidad, 
'		concat(ltrim(isnull(Dir_cliente,'')),ltrim(isnull(Dir2_client,'')),ltrim(isnull(Dir3_client,''))+ltrim(isnull(Dir4_client,'')))Direccion,
'        r.Num_doc Documento, r.Fecha_recib Fecha, r.Por_lempira Prima,
'        isnull(co.CONT_NUMCUO, 0) NumCuotas,isnull(co.CONT_VALOR,0) Valor, 
'		isnull(CONT_CANTI, 0)Cantidad,isnull(co.CONT_SERVI,'0')IdServicio,isnull(SERVI1DES,'')Servicio,
'		isnull(CONT_VALCUO, 0) ValorCuota, ltrim(isnull(c.CL_CELULAR,''))Tel1, ltrim(isnull(c.Telef_clien,''))Tel2		
'		From RECIBOS R
'		Left Join CLIENTESN C ON C.Codigo_clie = R.Codigo_clie And R.RCODVEND = C.CL_VENDEDOR
'        Left Join CONTRATON CO ON CO.Codigo_clie = R.Codigo_clie And CONT_NUMERO = R.Codigo_clie And co.cont_vended = R.RCODVEND
'        inner join FUNAMOR..VENDEDOR ven on ven.Cod_vendedo = r.RCODVEND  collate SQL_Latin1_General_CP1_CI_AS
'        WHERE Liquida2 = 'N' and marca = 'N' AND r.codigo_clie like '%" + txtBuscarCliente.Text + "%'"

'        Datos = conf.EjecutaSql(Sql)

'        gvClientesVE.DataSource = Datos.Tables(0)
'        gvClientesVE.DataBind()
'        'txtCodClienteapp_TextChanged()
'    End Sub


'    Private Sub btnCerrarCliapp_Click(sender As Object, e As EventArgs) Handles btnCerrarCliapp.Click
'        PanelClientesVE.Visible = False

'    End Sub

'    Private Sub btnBuscClienVE_Click(sender As Object, e As EventArgs) Handles btnBuscClienVE.Click
'        PanelClientesVE.Visible = True
'        txtBuscarCliente_TextChanged(sender, e)

'    End Sub



'    Private Sub gvMonitor_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvMonitor.Sorting
'        Dim Orden As String

'        If Session("Orden") = "0" Then
'            Orden = "Asc"
'            Session.Add("Orden", "1")
'        Else
'            Orden = "Desc"
'            Session.Add("Orden", "0")
'        End If

'        Session("GV").DefaultView.Sort = e.SortExpression.ToString + " " + Orden
'        gvMonitor.DataSource = Session("GV")
'        gvMonitor.DataBind()
'    End Sub

'    Sub Msg(Mensaje As String)
'        Dim msg As String
'        msg = "<script language='javascript'>"
'        msg += "alert('" + Mensaje + "');"
'        msg += "<" & "/script>"
'        Response.Write(msg)
'    End Sub

'    Private Sub gvMonitor_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvMonitor.RowDataBound
'        Try
'            If ((e.Row.RowType = DataControlRowType.DataRow) And (e.Row.RowType <> DataControlRowType.EmptyDataRow)) Then
'                Dim Fila As System.Data.DataRowView = e.Row.DataItem
'                Total += Convert.ToDecimal(Fila.Item(2))
'                Visitados += Convert.ToDecimal(Fila.Item(6))
'                Recibos += Convert.ToDecimal(Fila.Item(5))
'                Cobradores += 1
'                If dlMostrar.SelectedIndex = 1 Or dlMostrar.SelectedIndex = 5 Then
'                    Liquida = Fila.Item(9)
'                    Liquida2 = Fila.Item(10)
'                End If

'                If dlMostrar.SelectedIndex = 0 Then
'                    Verdes += Convert.ToDecimal(Fila.Item(8))
'                    lblVerdes.Text = "Verdes: " + Format(Verdes, "#,##0")
'                End If

'                If dlMostrar.SelectedIndex = 0 Or dlMostrar.SelectedIndex = 1 Or dlMostrar.SelectedIndex = 2 Then
'                    Ventas += Convert.ToDecimal(Fila.Item(7))
'                    lblVentas.Text = "Ventas: " + Format(Ventas, "#,##0")
'                End If

'                If dlMostrar.SelectedIndex = 3 Then
'                    lblVentas.Text = "Ventas: 0"
'                End If

'                lblTotal.Text = "Cobrado: " + Format(Total, "#,##0.00")
'                lblRecibos.Text = "Recibos: " + Format(Recibos, "#,##0")
'                lblVisitados.Text = "Visitados: " + Format(Visitados, "#,##0")
'                lblCobradores.Text = "Vendedores: " + Cobradores.ToString
'                e.Row.Cells(6).Text = Format(Fila.Item(2), "#,##0.00")
'            End If
'        Catch ex As Exception
'            Msg(ex.Message.ToString() + " - " + ex.Source.ToString())
'        End Try

'        If e.Row.RowType = DataControlRowType.DataRow Then
'            Dim Codigo As String = gvMonitor.DataKeys(e.Row.RowIndex).Value.ToString()
'            Dim gvDetalle As GridView = TryCast(e.Row.FindControl("gvDetalle"), GridView)

'            If dlMostrar.SelectedIndex = 1 Or dlMostrar.SelectedIndex = 5 Then
'                Session("GVDetalle").DefaultView.RowFilter = "Liquida='" + Liquida.Replace("/", "").Replace(" ", "").Replace(":", "") + "' AND Liquida2='" + Liquida2.Replace("/", "").Replace(" ", "").Replace(":", "") + "' AND RCODVEND='" + Codigo + "'"
'            ElseIf dlMostrar.SelectedIndex = 0 And dllider.SelectedValue = "TODOS" Then
'                Session("GVDetalle").DefaultView.RowFilter = "Lider='" + Codigo + "'"
'            Else
'                Session("GVDetalle").DefaultView.RowFilter = "RCODVEND='" + Codigo + "'"
'            End If
'            Session("GVDetalle").DefaultView.Sort = "Fecha Asc, Hora Asc"

'            gvDetalle.DataSource = Session("GVDetalle")
'            gvDetalle.DataBind()

'            For i = 0 To gvDetalle.Rows.Count - 1
'                Select Case gvDetalle.Rows(i).Cells(0).Text
'                    Case "NO VENTA"
'                        gvDetalle.Rows(i).ControlStyle.ForeColor = System.Drawing.Color.Red
'                    Case "PROSPECTO"
'                        gvDetalle.Rows(i).ControlStyle.ForeColor = System.Drawing.Color.Green
'                End Select

'                If gvDetalle.Rows(i).Cells(10).Text.TrimEnd = "ANULADO" Then
'                    Session("GVDetalle").DefaultView.RowFilter = "Codigo ='" + gvDetalle.Rows(i).Cells(0).Text.TrimEnd + "'"
'                    If Session("GVDetalle").DefaultView.Count > 0 Then
'                        gvDetalle.Rows(i).Cells(10).ToolTip = Session("GVDetalle").DefaultView.Item(0).Item("Motivo").ToString
'                    End If
'                End If

'                'If gvDetalle.Rows(i).Cells(5).Text.Length > 0 Then
'                '    gvDetalle.Rows(i).Cells(5).ToolTip = Session("GVDetalle").DefaultView.Item(0).Item("identidad").ToString
'                'End If

'                Session("GVDetalle").DefaultView.RowFilter = "Codigo='" + gvDetalle.Rows(i).Cells(0).Text.TrimEnd + "'"
'                If Session("GVDetalle").DefaultView.Count > 0 And Session("GVDetalle").DefaultView.Item(0).Item("SERVI2DES").ToString.Length > 0 Then
'                    gvDetalle.Rows(i).Cells(9).ToolTip = Session("GVDetalle").DefaultView.Item(0).Item("SERVI2DES").ToString + "
'" + Session("GVDetalle").DefaultView.Item(0).Item("SERVI3DES").ToString + " 
'" + Session("GVDetalle").DefaultView.Item(0).Item("SERVI4DES").ToString
'                    gvDetalle.Rows(i).Cells(9).ControlStyle.ForeColor = System.Drawing.Color.DarkBlue
'                End If

'            Next
'        End If

'    End Sub

'    Private Sub gvDetalle2_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDetalle2.RowCommand
'        If e.CommandName = "Ubicacion" Then
'            Session.Add("Reporte", "Ubicacion")
'            Session.Add("Codigo", gvDetalle2.Rows(e.CommandArgument.ToString).Cells(2).Text.ToString.TrimEnd)
'            Response.Redirect("mapa.aspx")

'            ifRepote.Dispose()
'            ifRepote.Src = "mapa.aspx"
'            btnProcesar.Visible = False
'            Panel1.Visible = False
'            PanelImpresion.Visible = True
'            Exit Sub
'        End If

'        If e.CommandName = "Anular" And dlMostrar.SelectedIndex = 1 And (Usuario_Aut = "MANAGER" Or Usuario_Aut = "ABLANDON" Or Usuario_Aut = "MDERAS" Or Usuario_Aut = "JULIOCAJA" Or Usuario_Aut = "JULIO") Then
'            Dim conf1 As New Configuracion(Usuario, Clave, "AEVENTAS", Servidor)
'            Dim Sql1 As String
'            lblMensaje.Text = gvDetalle2.Rows(e.CommandArgument.ToString).Cells(2).Text.ToString.TrimEnd
'            Session.Add("Codigo_Clie", gvDetalle2.Rows(e.CommandArgument.ToString).Cells(6).Text.ToString.TrimEnd)
'            lblMensaje1.Text = Session("Codigo_Clie") + " - " + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(7).Text.ToString.TrimEnd
'            lblMensaje2.Text = gvDetalle2.Rows(e.CommandArgument.ToString).Cells(3).Text.ToString.TrimEnd
'            Session.Add("HoraVenta", gvDetalle2.Rows(e.CommandArgument.ToString).Cells(5).Text.ToString.TrimEnd)
'            'Sql1 = "EXEC ANULAR_RECIBOS_LIQUIDADOS '" + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(2).Text.ToString.TrimEnd + "', '" + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(6).Text.ToString.TrimEnd + "', '" + Usuario_Aut.TrimEnd + "', '" + txtComentario.Text + "'"
'            'conf1.EjecutaSql(Sql1)
'            Panel1.Visible = False
'            Panel2.Visible = True
'            Exit Sub

'        End If

'        If e.CommandName = "Anular" And dlMostrar.SelectedIndex = 2 Then
'            lblMensaje.Text = gvDetalle2.Rows(e.CommandArgument.ToString).Cells(2).Text.ToString.TrimEnd
'            Session.Add("Codigo_Clie", gvDetalle2.Rows(e.CommandArgument.ToString).Cells(6).Text.ToString.TrimEnd)
'            lblMensaje1.Text = Session("Codigo_Clie") + " - " + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(7).Text.ToString.TrimEnd
'            lblMensaje2.Text = gvDetalle2.Rows(e.CommandArgument.ToString).Cells(3).Text.ToString.TrimEnd
'            Session.Add("HoraVenta", gvDetalle2.Rows(e.CommandArgument.ToString).Cells(5).Text.ToString.TrimEnd)
'            Panel1.Visible = False
'            Panel2.Visible = True


'        End If
'    End Sub

'    Private Sub gvMonitor_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvMonitor.RowCommand
'        If e.CommandName = "Mapa" Then
'            Session.Add("Reporte", "MapaVenta")
'            Session.Add("Codigo", gvMonitor.Rows(e.CommandArgument.ToString).Cells(4).Text.ToString.TrimEnd)
'            'Response.Redirect("mapa.aspx")

'            ifRepote.Dispose()
'            ifRepote.Src = "mapa.aspx"
'            btnProcesar.Visible = False
'            Panel1.Visible = False
'            PanelImpresion.Visible = True
'            Exit Sub
'        End If

'        If e.CommandName = "Imprimir" And dlMostrar.SelectedIndex = 5 Then
'            Dim Liquida2 As String = gvMonitor.Rows(e.CommandArgument.ToString).Cells(13).Text.ToString.Substring(0, 4) + gvMonitor.Rows(e.CommandArgument.ToString).Cells(13).Text.ToString.Substring(5, 2) + gvMonitor.Rows(e.CommandArgument.ToString).Cells(13).Text.ToString.Substring(8, 2) + gvMonitor.Rows(e.CommandArgument.ToString).Cells(13).Text.ToString.Substring(11, 2) + gvMonitor.Rows(e.CommandArgument.ToString).Cells(13).Text.ToString.Substring(14, 2)
'            Session.Add("Cobrador", gvMonitor.Rows(e.CommandArgument.ToString).Cells(4).Text.ToString.TrimEnd)
'            Session.Add("Liquida2", Liquida2)
'            Session.Add("Reporte", "VentasLiquida2")
'            ifRepote.Dispose()
'            ifRepote.Src = "liquidacion.aspx"
'            btnProcesar.Visible = False
'            Panel1.Visible = False
'            PanelImpresion.Visible = True
'            Exit Sub
'        End If

'        If e.CommandName = "Liquidar" And dlMostrar.SelectedIndex = 4 Then
'            Session.Add("Reporte", "VentasLiquida")
'            Session.Add("Cobrador", gvMonitor.Rows(e.CommandArgument.ToString).Cells(4).Text.ToString.TrimEnd)
'            Session.Add("Liquida2", Liquida2)
'            ifRepote.Dispose()
'            ifRepote.Src = "liquidacion.aspx"
'            btnProcesar.Visible = True
'            Panel1.Visible = False
'            PanelImpresion.Visible = True
'            Exit Sub
'        End If

'        If e.CommandName = "Liquidar" And dlMostrar.SelectedIndex <> 4 Then
'            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
'            Dim Sql As String

'            Sql = "EXEC LIQUIDA_RECIBOS '" + gvMonitor.Rows(e.CommandArgument.ToString).Cells(4).Text.ToString.TrimEnd + "', '" + Session("F1") + "'"
'            conf.EjecutaSql(Sql)

'            btnBuscar_Click(sender, e)
'        End If
'    End Sub


'    Private Sub gvvendEditVent_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvvendEditVent.RowDataBound
'        If e.Row.RowType = DataControlRowType.DataRow Then
'            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(gvvendEditVent, "Select$" & e.Row.RowIndex)
'            e.Row.Attributes("style") = "cursor:pointer"
'        End If
'    End Sub

'    Private Sub gvClientesVe_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvClientesVE.RowCommand
'        Dim Fila As Integer = Convert.ToInt32(e.CommandArgument)
'        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
'        Dim Sql As String


'        If txtCodClienteapp.Text.Length = 0 Or lblNameClientapp.InnerText = "" Then
'            txtCodClienteapp.Text = gvClientesVE.Rows(Fila).Cells(1).Text
'            lblNameClientapp.InnerText = gvClientesVE.Rows(Fila).Cells(2).Text
'        End If

'        txtCodVendEV.Text = gvClientesVE.Rows(Fila).Cells(3).Text.TrimEnd
'        txtnombreVendArr.InnerText = gvClientesVE.Rows(Fila).Cells(4).Text.TrimEnd
'        dlstatusvend.Items.Add("" + gvClientesVE.Rows(Fila).Cells(5).Text + "")
'        dlempresaArr.Items.Add("" + gvClientesVE.Rows(Fila).Cells(6).Text + "")

'        txtidentiCliapp.Text = gvClientesVE.Rows(Fila).Cells(7).Text
'        txtdir1Cliapp.Text = gvClientesVE.Rows(Fila).Cells(8).Text
'        txtvalorcontApp.Text = gvClientesVE.Rows(Fila).Cells(13).Text
'        txtcanti1app.Text = gvClientesVE.Rows(Fila).Cells(14).Text
'        txtcodigoprod1.Text = gvClientesVE.Rows(Fila).Cells(15).Text
'        txtprod1.Text = gvClientesVE.Rows(Fila).Cells(16).Text
'        txtvalorcontApp.Text = gvClientesVE.Rows(Fila).Cells(17).Text
'        txttel1app.Text = gvClientesVE.Rows(Fila).Cells(17).Text
'        txttel2app.Text = gvClientesVE.Rows(Fila).Cells(18).Text

'        Session.Add("EmpresaV", gvClientesVE.Rows(Fila).Cells(6).Text)


'        Sql = "Select  cod_Zona Empresa from AEVENTAS..CZONA WHERE COD_ZONA NOT IN ('M','" + gvClientesVE.Rows(Fila).Cells(6).Text + "')"

'        Datos = conf.EjecutaSql(Sql)
'        dlempresaArr.Items.Add("" + gvClientesVE.Rows(Fila).Cells(6).Text + "")
'        dlempresaArr.DataSource = Datos.Tables(0)
'        dlempresaArr.DataTextField = "Empresa"
'        dlempresaArr.DataValueField = "Empresa"
'        dlempresaArr.DataBind()





'        PanelClientesVE.Visible = False



'    End Sub

'    Private Sub BtnGuardarSi_Click(sender As Object, e As EventArgs) Handles BtnGuardarSi.Click




'        PanelClientesVE.Visible = False

'    End Sub


'    Private Sub gvvendEditVent_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvvendEditVent.RowCommand
'        Dim Fila As Integer = Convert.ToInt32(e.CommandArgument)
'        txtBuscarVended.Text = ""
'        txtCodVendEV.Text = gvvendEditVent.Rows(Fila).Cells(1).Text.TrimEnd
'        txtnombreVendArr.InnerText = gvvendEditVent.Rows(Fila).Cells(2).Text.TrimEnd
'        PanelVendedoresEditar.Visible = False
'        'txtVendEV_TextChanged(sender, e)
'    End Sub


'    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
'        Panel2.Visible = False
'        Panel1.Visible = True
'    End Sub

'    Private Sub BtnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
'        If txtClave.Text.ToUpper <> Clave_Aut.ToUpper Then
'            Msg("Clave Incorrecta")
'            Exit Sub
'        End If

'        If dlMostrar.SelectedItem.Text = "POR LIQUIDACION" And (Usuario_Aut.ToUpper = "MANAGER" Or Usuario_Aut = "MDERAS" Or Usuario_Aut.ToUpper = "ABLANDON" Or Usuario_Aut.ToUpper = "JULIOCAJA" Or Usuario_Aut.ToUpper = "JULIO") Then
'            Dim conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
'            Dim Sql1 As String

'            Sql1 = "EXEC ANULAR_RECIBOS_LIQUIDADOS '" + lblMensaje.Text.TrimEnd + "', '" + Session("Codigo_Clie") + "', '" + Usuario_Aut.TrimEnd + "', '" + txtComentario.Text + "','" + Session("HoraVenta") + "'"
'            conf1.EjecutaSql(Sql1)

'            Panel2.Visible = False
'            Panel1.Visible = True
'            btnBuscar_Click(sender, e)
'            Exit Sub
'        End If

'        If dlMostrar.SelectedIndex = 2 Then
'            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
'            Dim Sql As String

'            Sql = "EXEC ANULAR_RECIBOS '" + lblMensaje.Text.TrimEnd + "', '" + Session("Codigo_Clie") + "', '" + Usuario_Aut + "', '" + txtComentario.Text + "','" + Session("HoraVenta") + "'"
'            conf.EjecutaSql(Sql)

'            Panel2.Visible = False
'            Panel1.Visible = True
'            btnBuscar_Click(sender, e)
'        End If
'    End Sub

'End Class