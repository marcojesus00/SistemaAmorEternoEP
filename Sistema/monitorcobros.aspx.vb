Public Class monitorcobros
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut, Supervisor As String
    Private Datos, Datos1, Datos2, Datos3 As DataSet
    Private Total As Decimal = 0
    Private Visitados As Decimal = 0
    Private Recibos As Decimal = 0
    Private Cobradores As Decimal = 0
    Private Liquida, Liquida2 As String
    Private Tabla As DataTable

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
        Session.Timeout = 90
        If AuthHelper.isAuthorized(Usuario_Aut, "COBROS_A") Then
            btnCobrosAdvanced.Visible = True
        End If
        Dim conf3 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim SQL3 As String
        SQL3 = " SELECT COUNT(codigo_cobr) codigo_cobr FROM FUNAMOR..COBRADOR A WHERE A.COB_SUPERVI = '" + Usuario_Aut + "' "
        Datos3 = conf3.EjecutaSql(SQL3)

        If Datos3.Tables(0).Rows(0).Item("codigo_cobr").ToString <> "0" Then
            Supervisor = Usuario_Aut
        Else
            Supervisor = ""
        End If

        If Not IsPostBack Then
            Session.Add("Orden", "0")
            dlMostrar.Items.Add("POR DIA DE TRABAJO")
            dlMostrar.Items.Add("POR LIQUIDACION")
            dlMostrar.Items.Add("SIN LIQUIDACION")
            dlMostrar.Items.Add("SIN ACTIVIDAD")
            dlMostrar.SelectedIndex = 0

            If Session("Reporte") = "Caja Cobros" Then
                dlMostrar.Items.Add("SIN PROCESAR")
                dlMostrar.Items.Add("POR PROCESADOS")
                dlMostrar.SelectedIndex = 4
            End If

            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim SQL As String
            SQL = "SELECT B.nombre_cobr FROM AE_COBRADOR A INNER JOIN AE_COBRADOR B ON A.cob_lider = B.codigo_cobr WHERE A.COBR_STATUS = 'A' GROUP BY B.nombre_cobr"
            Datos2 = conf.EjecutaSql(SQL)

            dlLider.Items.Add("")
            For I As Integer = 0 To Datos2.Tables(0).Rows.Count - 1
                dlLider.Items.Add(Datos2.Tables(0).Rows(I).Item("nombre_cobr"))
            Next

            Dim conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim SQL1 As String
            SQL1 = " Select C.NOMBRE_SUCU From FUNAMOR..COBRADOR A INNER JOIN MVSAGEN B ON A.Codigo_Cobr = B.cveage INNER JOIN FUNAMOR..SUCURSAL C ON B.NUMSER = C.Codigo_Sucu WHERE A.COBR_STATUS = 'A' Group BY C.NOMBRE_SUCU "
            'SQL1 = " Select B.vzon_nombre From AE_COBRADOR A Left Join AE_VZONA B ON A.cob_zona = B.vzon_codigo WHERE A.COBR_STATUS = 'A' Group BY B.vzon_nombre "
            Datos = conf1.EjecutaSql(SQL1)

            dlZona.Items.Add("")
            For I As Integer = 0 To Datos.Tables(0).Rows.Count - 1
                dlZona.Items.Add(Datos.Tables(0).Rows(I).Item("NOMBRE_SUCU"))
            Next

            Dim conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim SQL2 As String
            SQL2 = " SELECT LTRIM(RTRIM(B.Nombre_sucu)) Nombre_sucu FROM FUNAMOR..CABSEG A INNER JOIN FUNAMOR..SUCURSAL B ON A.Codigo_sucu = B.Codigo_sucu WHERE A.SEG_USUARIO = '" + Usuario_Aut + "' "
            Datos1 = conf2.EjecutaSql(SQL2)



            If Datos1.Tables(0).Rows.Count > 0 Then
                If Datos1.Tables(0).Rows(0).Item("Nombre_sucu").ToString.Length > 0 And Datos1.Tables(0).Rows(0).Item("Nombre_sucu").ToString <> "PROGRESO" Then
                    dlZona.SelectedItem.Text = Datos1.Tables(0).Rows(0).Item("Nombre_sucu")
                    dlZona.Enabled = False
                End If
            End If

        End If
        'If Usuario_Aut = "CARLOSR" Then
        '    dlZona.SelectedItem.Text = "CHOLUTECA"
        '    dlZona.Enabled = False
        'End If
    End Sub

    Protected Sub btnSalir_Click(sender As Object, e As ImageClickEventArgs) Handles btnSalir.Click
        Response.Redirect("principal.aspx")
    End Sub

    Protected Sub btnRegresar_Click(sender As Object, e As ImageClickEventArgs) Handles btnRegresar.Click
        Panel1.Visible = True
        PanelImpresion.Visible = False
        ifRepote.Dispose()
        ifRepote.Src = ""
    End Sub

    'Sub ActualizaUbicacion()
    '    Dim conf As New Configuracion(Usuario, Clave, "AECOBROS", Servidor)
    '    Dim Sql As String
    '    Sql = "EXEC CorrigeUbicacionCero "
    '    conf.EjecutaSql(Sql)
    'End Sub

    Private Sub btnProcesar_Click(sender As Object, e As EventArgs) Handles btnProcesar.Click
        Dim conf As New Configuracion(Usuario, Clave, "AECOBROS", Servidor)
        Dim Sql As String
        Sql = "EXEC ENVIARECIBOS '" + Session("Cobrador") + "','" + Usuario_Aut + "'"
        conf.EjecutaSql(Sql)

        btnBuscar_Click(sender, e)
        PanelImpresion.Visible = False
        Panel1.Visible = True

    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Dim conf, Conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql, Sql1 As String

        'ActualizaUbicacion()
        If txtFecha.Text.Length > 0 Then
            Session.Add("F1", txtFecha.Text)
        Else
            Session.Add("F1", DateTime.Now.ToString("yyyy-MM-dd"))
        End If

        If dlMostrar.SelectedIndex = 0 Then
            ' ActualizaUbicacion()
            gvMonitor.Columns(1).Visible = False
            gvMonitor.Columns(2).Visible = False
            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("F1") + "'"
            Sql += " SELECT A.codigo_cobr [Codigo], B.nombre_cobr [Cobrador], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUBSTRING(C.nombre_cobr,1,16) [Lider], E.NOMBRE_SUCU [Zona], B.COB_TELEFO Telefono"
            Sql += " FROM ("
            Sql += " SELECT distinct A.codigo_cobr, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados FROM RECIBOS A WHERE CONVERT(DATE,A.Fecha_recib) = @FECHA AND A.MARCA = 'N' "
            Sql += " UNION ALL "
            Sql += " SELECT A.COBRADOR, 0, SUBSTRING(A.HORA,1,2) +':'+ SUBSTRING(A.HORA,3,2) +':'+ SUBSTRING(A.HORA,5,2), 0, 1 FROM NOCOBRO A WHERE CONVERT(DATE,A.FECHA) = @FECHA "
            Sql += " UNION ALL "
            Sql += " SELECT A.COBRADOR, 0, A.HORA, 0 Recibos, 1 Visitados FROM SUPERV A WHERE CONVERT(DATE,A.FECHA) = @FECHA "
            Sql += " UNION ALL "
            Sql += " SELECT A.COBRADOR, 0, A.HORA, 0 Recibos, 1 Visitados FROM NOSUPER A WHERE CONVERT(DATE,A.FECHA) = @FECHA "
            Sql += " ) A LEFT JOIN AE_COBRADOR B ON A.codigo_cobr = B.codigo_cobr "
            Sql += " LEFT JOIN AE_COBRADOR C ON B.Cob_Lider = C.Codigo_cobr "
            'Sql += " LEFT JOIN AE_VZONA D ON B.cob_zona = D.vzon_codigo "
            Sql += " Left Join MVSAGEN D ON B.Codigo_Cobr = D.cveage "
            Sql += " Left Join FUNAMOR..SUCURSAL E ON D.NUMSER = E.Codigo_Sucu "
            Sql += " WHERE C.nombre_cobr LIKE '%" + dllider.SelectedValue + "%'"
            Sql += " AND A.codigo_cobr + B.nombre_cobr LIKE '%" + txtCobrador.Value + "%'"
            Sql += " AND E.NOMBRE_SUCU LIKE '%" + dlzona.SelectedValue + "%'"
            Sql += " AND ISNULL(B.COB_SUPERVI,'') LIKE '%" + Supervisor + "%' "
            Sql += " GROUP BY A.codigo_cobr, B.nombre_cobr, C.nombre_cobr, E.NOMBRE_SUCU, B.COB_TELEFO "

            Sql1 = " DECLARE @FECHA DATE "
            Sql1 += " SET @FECHA = '" + Session("F1") + "' "
            Sql1 += " SELECT A.Num_doc [Codigo], A.Por_lempira [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], A.CONCEPTO, A.codigo_cobr, SUBSTRING(C.Nombre_clie,1,20) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, SUBSTRING(C.Dir_cliente,1,40) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + '
MOTIVO: ' + Z.MOTIVO FROM LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.LONGITUD, A.LATITUD  "
            Sql1 += " FROM ( "
            Sql1 += " SELECT distinct A.Num_doc, A.Por_lempira, A.rhora, A.Codigo_clie, A.codigo_cobr, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, A.LONGITUD, A.LATITUD FROM RECIBOS A WHERE CONVERT(DATE,A.Fecha_recib) = @FECHA "
            Sql1 += " UNION ALL "
            Sql1 += " SELECT 'NO COBRO', 0, SUBSTRING(A.HORA,1,2) +':'+ SUBSTRING(A.HORA,3,2) +':'+ SUBSTRING(A.HORA,5,2), A.Codigo_clie, A.COBRADOR, SUBSTRING(A.MOTIVO,6,30) MOTIVO, CONVERT(DATE,A.FECHA), A.liquida, A.liquida2, A.LONGITUD, A.LATITUD FROM NOCOBRO A WHERE CONVERT(DATE,A.FECHA) = @FECHA "
            Sql1 += " UNION ALL "
            Sql1 += " SELECT 'SUPERVISION', 0, A.HORA, A.Codigo_clie, A.COBRADOR, '', CONVERT(DATE,A.FECHA), A.liquida, A.liquida2, A.LONGITUD,A.LATITUD FROM SUPERV A WHERE CONVERT(DATE,A.FECHA) = @FECHA "
            Sql1 += " UNION ALL "
            Sql1 += " SELECT 'NO SUPERVISION', 0, A.HORA, A.Codigo_clie, A.COBRADOR, SUBSTRING(A.MOTIVO,4,30) MOTIVO, CONVERT(DATE,A.FECHA), A.liquida, A.liquida2, A.LONGITUD, A.LATITUD FROM NOSUPER A WHERE CONVERT(DATE,A.FECHA) = @FECHA "
            Sql1 += " ) A "
            Sql1 += " LEFT JOIN AE_COBRADOR B ON A.codigo_cobr = B.codigo_cobr "
            Sql1 += " LEFT JOIN AE_CLIENTES C ON A.Codigo_clie = C.Codigo_clie "
        End If

        'lblMsg.Text = Sql1
        'Exit Sub

        'Por liquidacion
        If dlMostrar.SelectedIndex = 1 Then
            gvMonitor.Columns(1).Visible = False
            gvMonitor.Columns(2).Visible = False
            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("F1") + "'"
            Sql += " SELECT distinct A.codigo_cobr [Codigo], B.nombre_cobr [Cobrador], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUBSTRING(C.nombre_cobr,1,16) [Lider], SUBSTRING(A.liquida,1,4) +'/'+ SUBSTRING(A.liquida,5,2) +'/'+ SUBSTRING(A.liquida,7,2) +' '+ SUBSTRING(A.liquida,9,2) +':'+ SUBSTRING(A.liquida,11,2) Liquida, CASE WHEN A.liquida2 != 'N' THEN SUBSTRING(A.liquida2,1,4) +'/'+ SUBSTRING(A.liquida2,5,2) +'/'+ SUBSTRING(A.liquida2,7,2) +' '+ SUBSTRING(A.liquida2,9,2) +':'+ SUBSTRING(A.liquida2,11,2) ELSE 'N' END Procesado, E.NOMBRE_SUCU [Zona], B.COB_TELEFO Telefono "
            Sql += " FROM ("
            Sql += " SELECT distinct A.Num_doc, A.codigo_cobr, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, A.liquida, A.liquida2 FROM RECIBOS A WHERE CONVERT(DATE,SUBSTRING(A.liquida,1,8)) = @FECHA AND A.MARCA = 'N' "
            Sql += " ) A LEFT JOIN FUNAMOR..COBRADOR B ON A.codigo_cobr = B.codigo_cobr "
            Sql += " LEFT JOIN FUNAMOR..COBRADOR C ON B.Cob_Lider = C.Codigo_cobr "
            'Sql += " LEFT JOIN AE_VZONA D ON B.cob_zona = D.vzon_codigo "
            Sql += " Left Join MVSAGEN D ON B.Codigo_Cobr = D.cveage "
            Sql += " Left Join FUNAMOR..SUCURSAL E ON D.NUMSER = E.Codigo_Sucu "
            Sql += " WHERE C.nombre_cobr LIKE '%" + dllider.SelectedValue + "%'"
            Sql += " AND A.codigo_cobr + B.nombre_cobr LIKE '%" + txtCobrador.Value + "%'"
            Sql += " AND A.liquida != 'N' "
            Sql += " AND E.NOMBRE_SUCU LIKE '%" + dlzona.SelectedValue + "%'"
            Sql += " AND ISNULL(B.COB_SUPERVI,'') LIKE '%" + Supervisor + "%' "
            Sql += " GROUP BY A.codigo_cobr, B.nombre_cobr, C.nombre_cobr, A.Liquida, A.liquida2, E.NOMBRE_SUCU, B.COB_TELEFO "

            Sql1 = " DECLARE @FECHA DATE "
            Sql1 += " SET @FECHA = '" + Session("F1") + "' "
            Sql1 += " SELECT distinct A.Num_doc [Codigo], A.Por_lempira [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], A.CONCEPTO, A.codigo_cobr, SUBSTRING(C.Nombre_clie,1,20) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, SUBSTRING(C.Dir_cliente,1,40) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + '
MOTIVO: ' + Z.MOTIVO FROM LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.LONGITUD, A.LATITUD  "
            Sql1 += " FROM ( "
            Sql1 += " SELECT A.Num_doc, A.Por_lempira, A.rhora, A.Codigo_clie, A.codigo_cobr, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, A.LONGITUD, A.LATITUD FROM RECIBOS A WHERE CONVERT(DATE,SUBSTRING(A.liquida,1,8)) = @FECHA "
            Sql1 += " ) A "
            Sql1 += " LEFT JOIN AE_COBRADOR B ON A.codigo_cobr = B.codigo_cobr "
            Sql1 += " LEFT JOIN AE_CLIENTES C ON A.Codigo_clie = C.Codigo_clie "
            Sql1 += " WHERE A.liquida != 'N' "
        End If


        'lblMsg.Text = Sql1
        'Exit Sub

        If dlMostrar.SelectedIndex = 2 Then
            gvMonitor.Columns(1).Visible = False
            gvMonitor.Columns(2).Visible = False
            If Usuario_Aut = "MANAGER" Then
                gvMonitor.Columns(1).Visible = True
            End If
            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("F1") + "'"
            Sql += " SELECT distinct A.codigo_cobr [Codigo], B.nombre_cobr [Cobrador], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUBSTRING(C.nombre_cobr,1,16) [Lider], A.Liquida, A.Liquida2 Procesado, E.NOMBRE_SUCU [Zona], B.COB_TELEFO Telefono "
            Sql += " FROM ("
            Sql += " SELECT distinct A.codigo_cobr, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, A.liquida, A.liquida2 FROM RECIBOS A WHERE A.MARCA = 'N' AND A.liquida = 'N' AND CONVERT(DATE,A.Fecha_recib) <= @FECHA "
            Sql += " ) A LEFT JOIN AE_COBRADOR B ON A.codigo_cobr = B.codigo_cobr "
            Sql += " LEFT JOIN AE_COBRADOR C ON B.Cob_Lider = C.Codigo_cobr "
            'Sql += " LEFT JOIN AE_VZONA D ON B.cob_zona = D.vzon_codigo "
            Sql += " Left Join MVSAGEN D ON B.Codigo_Cobr = D.cveage "
            Sql += " Left Join FUNAMOR..SUCURSAL E ON D.NUMSER = E.Codigo_Sucu "
            Sql += " WHERE C.nombre_cobr LIKE '%" + dllider.SelectedValue + "%'"
            Sql += " AND A.codigo_cobr + B.nombre_cobr LIKE '%" + txtCobrador.Value + "%'"
            Sql += " AND E.NOMBRE_SUCU LIKE '%" + dlzona.SelectedValue + "%'"
            Sql += " AND ISNULL(B.COB_SUPERVI,'') LIKE '%" + Supervisor + "%' "
            Sql += " GROUP BY A.codigo_cobr, B.nombre_cobr, C.nombre_cobr, A.Liquida, A.liquida2, E.NOMBRE_SUCU, B.COB_TELEFO "

            Sql1 = " DECLARE @FECHA DATE "
            Sql1 += " SET @FECHA = '" + Session("F1") + "' "
            Sql1 += " SELECT distinct A.Num_doc [Codigo], A.Por_lempira [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], A.CONCEPTO, A.codigo_cobr, SUBSTRING(C.Nombre_clie,1,20) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, SUBSTRING(C.Dir_cliente,1,40) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + '
MOTIVO: ' + Z.MOTIVO FROM LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc ORDER BY LEN(Z.MOTIVO) DESC) Motivo  "
            Sql1 += " FROM ( "
            Sql1 += " SELECT distinct A.Num_doc, A.Por_lempira, A.rhora, A.Codigo_clie, A.codigo_cobr, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha FROM RECIBOS A WHERE A.Liquida = 'N' AND CONVERT(DATE,A.Fecha_recib) <= @FECHA "
            Sql1 += " ) A "
            Sql1 += " LEFT JOIN AE_COBRADOR B ON A.codigo_cobr = B.codigo_cobr "
            Sql1 += " LEFT JOIN AE_CLIENTES C ON A.Codigo_clie = C.Codigo_clie "
        End If

        If dlMostrar.SelectedIndex = 3 Then
            gvMonitor.Columns(1).Visible = False
            gvMonitor.Columns(2).Visible = False
            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("F1") + "'"
            Sql += " SELECT A.codigo_cobr [Codigo], B.nombre_cobr [Cobrador], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUBSTRING(C.nombre_cobr,1,16) [Lider], E.NOMBRE_SUCU [Zona], B.COB_TELEFO Telefono "
            Sql += " FROM ("
            Sql += " SELECT A.cveage codigo_cobr, 0 Por_lempira, '00:00:00' rhora, 0 Recibos, 0 Visitados, 'N' liquida, 'N' liquida2 FROM MVSAGEN A WHERE A.cveage NOT IN (SELECT A.codigo_cobr FROM RECIBOS A WHERE CONVERT(DATE,A.Fecha_recib) = @FECHA GROUP BY A.codigo_cobr UNION ALL SELECT A.COBRADOR FROM NOCOBRO A WHERE CONVERT(DATE,A.FECHA) = @FECHA GROUP BY A.COBRADOR UNION ALL SELECT A.COBRADOR FROM SUPERV A WHERE CONVERT(DATE,A.FECHA) = @FECHA GROUP BY A.COBRADOR UNION ALL SELECT A.COBRADOR FROM NOSUPER A WHERE CONVERT(DATE,A.FECHA) = @FECHA GROUP BY A.COBRADOR) "
            Sql += " ) A INNER JOIN AE_COBRADOR B ON A.codigo_cobr = B.codigo_cobr AND B.COBR_STATUS = 'A' "
            Sql += " LEFT JOIN AE_COBRADOR C ON B.Cob_Lider = C.Codigo_cobr "
            'Sql += " LEFT JOIN AE_VZONA D ON B.cob_zona = D.vzon_codigo "
            Sql += " Left Join MVSAGEN D ON B.Codigo_Cobr = D.cveage "
            Sql += " Left Join FUNAMOR..SUCURSAL E ON D.NUMSER = E.Codigo_Sucu "
            Sql += " WHERE C.nombre_cobr LIKE '%" + dllider.SelectedValue + "%'"
            Sql += " AND A.codigo_cobr + B.nombre_cobr LIKE '%" + txtCobrador.Value + "%'"
            Sql += " AND E.NOMBRE_SUCU LIKE '%" + dlzona.SelectedValue + "%'"
            Sql += " AND ISNULL(B.COB_SUPERVI,'') LIKE '%" + Supervisor + "%' "
            Sql += " GROUP BY A.codigo_cobr, B.nombre_cobr, C.nombre_cobr, A.Liquida, A.liquida2, E.NOMBRE_SUCU, B.COB_TELEFO "


            Sql1 = " SELECT TOP 1 A.Num_doc [Codigo], A.Por_lempira [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], A.CONCEPTO, A.codigo_cobr, SUBSTRING(C.Nombre_clie,1,20) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, A.LATITUD, A.LONGITUD, SUBSTRING(C.Dir_cliente,1,40) Dir_cliente, '' Motivo  "
            Sql1 += " FROM ( "
            Sql1 += " SELECT A.Num_doc, A.Por_lempira, A.rhora, A.Codigo_clie, A.codigo_cobr, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, A.LATITUD, A.LONGITUD FROM RECIBOS A WHERE A.Liquida = 'A' "
            Sql1 += " ) A "
            Sql1 += " LEFT JOIN AE_COBRADOR B ON A.codigo_cobr = B.codigo_cobr "
            Sql1 += " LEFT JOIN AE_CLIENTES C ON A.Codigo_clie = C.Codigo_clie "
        End If

        If dlMostrar.SelectedIndex = 4 Then
            gvMonitor.Columns(1).Visible = True
            gvMonitor.Columns(2).Visible = False
            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("F1") + "'"
            Sql += " SELECT distinct A.codigo_cobr [Codigo], B.nombre_cobr [Cobrador], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUBSTRING(C.nombre_cobr,1,16) [Lider], A.Liquida, A.Liquida2 Procesado, E.NOMBRE_SUCU [Zona], B.COB_TELEFO Telefono "
            Sql += " FROM ("
            Sql += " SELECT distinct A.codigo_cobr, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, A.liquida, A.liquida2 FROM RECIBOS A WHERE A.MARCA = 'N' AND A.liquida <> 'N' AND A.liquida2 = 'N' AND CONVERT(DATE,A.Fecha_recib) <= @FECHA "
            Sql += " ) A LEFT JOIN AE_COBRADOR B ON A.codigo_cobr = B.codigo_cobr "
            Sql += " LEFT JOIN AE_COBRADOR C ON B.Cob_Lider = C.Codigo_cobr "
            'Sql += " LEFT JOIN AE_VZONA D ON B.cob_zona = D.vzon_codigo "
            Sql += " Left Join MVSAGEN D ON B.Codigo_Cobr = D.cveage "
            Sql += " Left Join FUNAMOR..SUCURSAL E ON D.NUMSER = E.Codigo_Sucu "
            Sql += " WHERE C.nombre_cobr LIKE '%" + dllider.SelectedValue + "%'"
            Sql += " AND A.codigo_cobr + B.nombre_cobr LIKE '%" + txtCobrador.Value + "%'"
            Sql += " AND E.NOMBRE_SUCU LIKE '%" + dlzona.SelectedValue + "%'"
            Sql += " AND ISNULL(B.COB_SUPERVI,'') LIKE '%" + Supervisor + "%' "
            Sql += " GROUP BY A.codigo_cobr, B.nombre_cobr, C.nombre_cobr, A.Liquida, A.liquida2, E.NOMBRE_SUCU, B.COB_TELEFO "

            Sql1 = " DECLARE @FECHA DATE "
            Sql1 += " SET @FECHA = '" + Session("F1") + "' "
            Sql1 += " SELECT distinct A.Num_doc [Codigo], A.Por_lempira [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], A.CONCEPTO, A.codigo_cobr, SUBSTRING(C.Nombre_clie,1,20) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, SUBSTRING(C.Dir_cliente,1,40) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + '
MOTIVO: ' + Z.MOTIVO FROM LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc ORDER BY LEN(Z.MOTIVO) DESC) Motivo  "
            Sql1 += " FROM ( "
            Sql1 += " SELECT distinct A.Num_doc, A.Por_lempira, A.rhora, A.Codigo_clie, A.codigo_cobr, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha FROM RECIBOS A WHERE A.Liquida <> 'N' AND A.Liquida2 = 'N' AND CONVERT(DATE,A.Fecha_recib) <= @FECHA "
            Sql1 += " ) A "
            Sql1 += " LEFT JOIN AE_COBRADOR B ON A.codigo_cobr = B.codigo_cobr "
            Sql1 += " LEFT JOIN AE_CLIENTES C ON A.Codigo_clie = C.Codigo_clie "
        End If

        If dlMostrar.SelectedIndex = 5 Then
            gvMonitor.Columns(2).Visible = True
            gvMonitor.Columns(1).Visible = False
            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("F1") + "'"
            Sql += " SELECT distinct A.codigo_cobr [Codigo], B.nombre_cobr [Cobrador], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUBSTRING(C.nombre_cobr,1,16) [Lider], SUBSTRING(A.liquida,1,4) +'/'+ SUBSTRING(A.liquida,5,2) +'/'+ SUBSTRING(A.liquida,7,2) +' '+ SUBSTRING(A.liquida,9,2) +':'+ SUBSTRING(A.liquida,11,2) Liquida, CASE WHEN A.liquida2 != 'N' THEN SUBSTRING(A.liquida2,1,4) +'/'+ SUBSTRING(A.liquida2,5,2) +'/'+ SUBSTRING(A.liquida2,7,2) +' '+ SUBSTRING(A.liquida2,9,2) +':'+ SUBSTRING(A.liquida2,11,2) ELSE 'N' END Procesado, E.NOMBRE_SUCU [Zona], B.COB_TELEFO Telefono "
            Sql += " FROM ("
            Sql += " SELECT distinct A.codigo_cobr, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, A.liquida, A.liquida2 FROM RECIBOS A WHERE CONVERT(DATE,SUBSTRING(A.liquida2,1,8)) = @FECHA AND A.MARCA = 'N' "
            Sql += " ) A LEFT JOIN AE_COBRADOR B ON A.codigo_cobr = B.codigo_cobr "
            Sql += " LEFT JOIN AE_COBRADOR C ON B.Cob_Lider = C.Codigo_cobr "
            'Sql += " LEFT JOIN AE_VZONA D ON B.cob_zona = D.vzon_codigo "
            Sql += " Left Join MVSAGEN D ON B.Codigo_Cobr = D.cveage "
            Sql += " Left Join FUNAMOR..SUCURSAL E ON D.NUMSER = E.Codigo_Sucu "
            Sql += " WHERE C.nombre_cobr LIKE '%" + dllider.SelectedValue + "%'"
            Sql += " AND A.codigo_cobr + B.nombre_cobr LIKE '%" + txtCobrador.Value + "%'"
            Sql += " AND A.liquida2 != 'N' "
            Sql += " AND E.NOMBRE_SUCU LIKE '%" + dlzona.SelectedValue + "%'"
            Sql += " AND ISNULL(B.COB_SUPERVI,'') LIKE '%" + Supervisor + "%' "
            Sql += " GROUP BY A.codigo_cobr, B.nombre_cobr, C.nombre_cobr, A.Liquida, A.liquida2, E.NOMBRE_SUCU, B.COB_TELEFO "

            Sql1 = " DECLARE @FECHA DATE "
            Sql1 += " SET @FECHA = '" + Session("F1") + "' "
            Sql1 += " SELECT distinct A.Num_doc [Codigo], A.Por_lempira [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], A.CONCEPTO, A.codigo_cobr, SUBSTRING(C.Nombre_clie,1,20) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, SUBSTRING(C.Dir_cliente,1,40) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + '
MOTIVO: ' + Z.MOTIVO FROM LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc ORDER BY LEN(Z.MOTIVO) DESC) Motivo  "
            Sql1 += " FROM ( "
            Sql1 += " SELECT distinct A.Num_doc, A.Por_lempira, A.rhora, A.Codigo_clie, A.codigo_cobr, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2 FROM RECIBOS A WHERE CONVERT(DATE,SUBSTRING(A.liquida2,1,8)) = @FECHA "
            Sql1 += " ) A "
            Sql1 += " LEFT JOIN AE_COBRADOR B ON A.codigo_cobr = B.codigo_cobr "
            Sql1 += " LEFT JOIN AE_CLIENTES C ON A.Codigo_clie = C.Codigo_clie "
            Sql1 += " WHERE A.liquida2 != 'N' "
        End If

        'lblMsg.Text = Sql
        'Exit Sub

        Datos = conf.EjecutaSql(Sql)

        If Usuario_Aut = "MANAGER" Or (Usuario_Aut = "JULIO" Or Usuario_Aut = "julio") And Datos.Tables(0).Rows.Count = 1 And dlMostrar.SelectedIndex = 1 Then
            Datos1 = Conf1.EjecutaSql(Sql1)
            Session.Add("GVDetalle", Datos1.Tables(0))

            Session("GVDetalle").DefaultView.RowFilter = "codigo_cobr='" + txtCobrador.Value + "'"
            Session("GVDetalle").DefaultView.Sort = "Fecha Asc, Hora Asc"

            gvDetalle2.DataSource = Session("GVDetalle")
            gvDetalle2.DataBind()

            Datos = conf.EjecutaSql(Sql)
            Datos.Tables(0).DefaultView.Sort = "Cobrado Desc"
            Session.Add("GV", Datos.Tables(0))
            gvMonitor2.DataSource = Session("GV")
            gvMonitor2.DataBind()

            gvMonitor2.Visible = True
            gvDetalle2.Visible = True
            gvMonitor.Visible = False

        ElseIf dlMostrar.SelectedIndex = 2 And Datos.Tables(0).Rows.Count = 1 Then
            Datos1 = Conf1.EjecutaSql(Sql1)
            Session.Add("GVDetalle", Datos1.Tables(0))

            Session("GVDetalle").DefaultView.RowFilter = "codigo_cobr='" + txtCobrador.Value + "'"
            Session("GVDetalle").DefaultView.Sort = "Fecha Asc, Hora Asc"

            gvDetalle2.DataSource = Session("GVDetalle")
            gvDetalle2.DataBind()

            Datos = conf.EjecutaSql(Sql)
            Datos.Tables(0).DefaultView.Sort = "Cobrado Desc"
            Session.Add("GV", Datos.Tables(0))
            gvMonitor2.DataSource = Session("GV")
            gvMonitor2.DataBind()

            gvMonitor2.Visible = True
            gvDetalle2.Visible = True
            gvMonitor.Visible = False
        Else
            Datos1 = Conf1.EjecutaSql(Sql1)
            Session.Add("GVDetalle", Datos1.Tables(0))

            Datos = conf.EjecutaSql(Sql)
            Datos.Tables(0).DefaultView.Sort = "Cobrado Desc"
            Session.Add("GV", Datos.Tables(0))
            gvMonitor.DataSource = Session("GV")
            gvMonitor.DataBind()
            gvMonitor2.Visible = False
            gvDetalle2.Visible = False
            gvMonitor.Visible = True
        End If

        lblHora.Text = "Actualizado: " + System.DateTime.Now.ToShortTimeString
    End Sub

    Private Sub gvMonitor_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvMonitor.Sorting
        Dim Orden As String

        If Session("Orden") = "0" Then
            Orden = "Asc"
            Session.Add("Orden", "1")
        Else
            Orden = "Desc"
            Session.Add("Orden", "0")
        End If

        Session("GV").DefaultView.Sort = e.SortExpression.ToString + " " + Orden
        gvMonitor.DataSource = Session("GV")
        gvMonitor.DataBind()
    End Sub

    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub

    Private Sub gvMonitor_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvMonitor.RowDataBound
        Try
            If ((e.Row.RowType = DataControlRowType.DataRow) And (e.Row.RowType <> DataControlRowType.EmptyDataRow)) Then
                Dim Fila As System.Data.DataRowView = e.Row.DataItem
                Total += Convert.ToDecimal(Fila.Item("Cobrado"))
                Visitados += Convert.ToDecimal(Fila.Item("Visitados"))
                Recibos += Convert.ToDecimal(Fila.Item("Recibos"))
                Cobradores += 1
                If dlMostrar.SelectedIndex = 1 Or dlMostrar.SelectedIndex = 5 Then
                    Liquida = Fila.Item("Liquida")
                    Liquida2 = Fila.Item("Procesado")
                End If

                lblTotal.Text = "Cobrado: " + Format(Total, "#,##0.00")
                lblRecibos.Text = "Recibos: " + Format(Recibos, "#,##0")
                lblVisitados.Text = "Visitados: " + Format(Visitados, "#,##0")
                lblCobradores.Text = "Cobradores: " + Cobradores.ToString
                e.Row.Cells(6).Text = Format(Fila.Item("Cobrado"), "#,##0.00")
            End If
        Catch ex As Exception
            Msg(ex.Message.ToString() + " - " + ex.Source.ToString())
        End Try

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Codigo As String = gvMonitor.DataKeys(e.Row.RowIndex).Value.ToString()
            Dim gvDetalle As GridView = TryCast(e.Row.FindControl("gvDetalle"), GridView)

            If dlMostrar.SelectedIndex = 1 Or dlMostrar.SelectedIndex = 5 Then
                Session("GVDetalle").DefaultView.RowFilter = "Liquida='" + Liquida.Replace("/", "").Replace(" ", "").Replace(":", "") + "' AND Liquida2='" + Liquida2.Replace("/", "").Replace(" ", "").Replace(":", "") + "' AND codigo_cobr='" + Codigo + "'"
            Else
                Session("GVDetalle").DefaultView.RowFilter = "codigo_cobr='" + Codigo + "'"
            End If
            Session("GVDetalle").DefaultView.Sort = "Fecha Asc, Hora Asc"

            gvDetalle.DataSource = Session("GVDetalle")
            gvDetalle.DataBind()

            For i = 0 To gvDetalle.Rows.Count - 1
                Select Case gvDetalle.Rows(i).Cells(0).Text
                    Case "NO COBRO"
                        gvDetalle.Rows(i).ControlStyle.ForeColor = System.Drawing.Color.Red
                    Case "SUPERVISION"
                        gvDetalle.Rows(i).ControlStyle.ForeColor = System.Drawing.Color.Green
                    Case "NO SUPERVISION"
                        gvDetalle.Rows(i).ControlStyle.ForeColor = System.Drawing.Color.Blue
                End Select
                If gvDetalle.Rows(i).Cells(6).Text.TrimEnd = "ANULADO" Then

                    Session("GVDetalle").DefaultView.RowFilter = "Codigo='" + gvDetalle.Rows(i).Cells(0).Text.TrimEnd + "'"
                    If Session("GVDetalle").DefaultView.Count > 0 Then
                        gvDetalle.Rows(i).Cells(6).ToolTip = Session("GVDetalle").DefaultView.Item(0).Item("Motivo").ToString
                    End If

                End If
            Next
        End If

    End Sub

    Private Sub gvDetalle2_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDetalle2.RowCommand

        If e.CommandName = "Anular" Then
            lblMensaje.Text = gvDetalle2.Rows(e.CommandArgument.ToString).Cells(1).Text.ToString.TrimEnd
            Session.Add("Codigo_Clie", gvDetalle2.Rows(e.CommandArgument.ToString).Cells(5).Text.ToString.TrimEnd)
            lblMensaje1.Text = Session("Codigo_Clie") + " - " + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(6).Text.ToString.TrimEnd
            lblMensaje2.Text = gvDetalle2.Rows(e.CommandArgument.ToString).Cells(2).Text.ToString.TrimEnd
            Panel1.Visible = False
            Panel2.Visible = True
        End If

        'If e.CommandName = "Anular" And dlMostrar.SelectedIndex = 1 And Usuario_Aut = "MANAGER" Then
        '    Dim conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
        '    Dim Sql1 As String

        '    Sql1 = "EXEC ANULAR_RECIBOS_LIQUIDADOS '" + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(1).Text.ToString.TrimEnd + "', '" + Usuario_Aut.TrimEnd + "'"
        '    conf1.EjecutaSql(Sql1)
        '    btnBuscar_Click(sender, e)
        '    Exit Sub
        'End If

        'If e.CommandName = "Anular" Then
        '    Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        '    Dim Sql As String

        '    Sql = "EXEC ANULAR_RECIBOS '" + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(1).Text.ToString.TrimEnd + "', '" + Usuario_Aut + "'"
        '    conf.EjecutaSql(Sql)

        '    btnBuscar_Click(sender, e)
        'End If

    End Sub

    Private Sub gvMonitor_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvMonitor.RowCommand
        If e.CommandName = "Mapa" Then
            Session.Add("Reporte", "Mapa")
            Session.Add("Codigo", gvMonitor.Rows(e.CommandArgument.ToString).Cells(4).Text.ToString.TrimEnd)
            'Response.Redirect("mapa.aspx")

            ifRepote.Dispose()
            ifRepote.Src = "mapa.aspx"
            btnProcesar.Visible = False
            Panel1.Visible = False
            PanelImpresion.Visible = True
            Exit Sub
        End If

        If e.CommandName = "Imprimir" And dlMostrar.SelectedIndex = 5 Then
            Dim Liquida2 As String = gvMonitor.Rows(e.CommandArgument.ToString).Cells(13).Text.ToString.Substring(0, 4) + gvMonitor.Rows(e.CommandArgument.ToString).Cells(13).Text.ToString.Substring(5, 2) + gvMonitor.Rows(e.CommandArgument.ToString).Cells(13).Text.ToString.Substring(8, 2) + gvMonitor.Rows(e.CommandArgument.ToString).Cells(13).Text.ToString.Substring(11, 2) + gvMonitor.Rows(e.CommandArgument.ToString).Cells(13).Text.ToString.Substring(14, 2)
            Session.Add("Cobrador", gvMonitor.Rows(e.CommandArgument.ToString).Cells(4).Text.ToString.TrimEnd)
            Session.Add("Liquida2", Liquida2)
            Session.Add("Reporte", "Liquida2")
            ifRepote.Dispose()
            ifRepote.Src = "liquidacion.aspx"
            btnProcesar.Visible = False
            Panel1.Visible = False
            PanelImpresion.Visible = True
            Exit Sub
        End If


        If e.CommandName = "Liquidar" And dlMostrar.SelectedIndex = 4 Then
            Session.Add("Reporte", "Liquida")
            Session.Add("Cobrador", gvMonitor.Rows(e.CommandArgument.ToString).Cells(4).Text.ToString.TrimEnd)
            Session.Add("Liquida2", Liquida2)
            ifRepote.Dispose()
            ifRepote.Src = "liquidacion.aspx"
            btnProcesar.Visible = True
            Panel1.Visible = False
            PanelImpresion.Visible = True
            Exit Sub
        End If

        If e.CommandName = "Liquidar" And dlMostrar.SelectedIndex <> 4 Then
            Dim conf As New Configuracion(Usuario, Clave, "AECOBROS", Servidor)
            Dim Sql As String

            Sql = "EXEC LIQUIDA_RECIBOS '" + gvMonitor.Rows(e.CommandArgument.ToString).Cells(4).Text.ToString.TrimEnd + "', '" + Session("F1") + "'"
            conf.EjecutaSql(Sql)

            btnBuscar_Click(sender, e)
        End If
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Panel2.Visible = False
        Panel1.Visible = True
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        If txtClave.Text.ToUpper <> Clave_Aut.ToUpper Then
            Msg("Clave Incorrecta")
            Exit Sub
        End If

        If dlMostrar.SelectedIndex = 1 And Usuario_Aut.ToUpper = "MANAGER" Then
            Dim conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql1 As String

            Sql1 = "EXEC ANULAR_RECIBOS_LIQUIDADOS '" + lblMensaje.Text.TrimEnd + "', '" + Session("Codigo_Clie") + "', '" + Usuario_Aut.TrimEnd + "', '" + txtComentario.Text + "'"
            conf1.EjecutaSql(Sql1)

            Panel2.Visible = False
            Panel1.Visible = True
            btnBuscar_Click(sender, e)
            Exit Sub
        End If

        If dlMostrar.SelectedIndex = 2 Then
            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String

            Sql = "EXEC ANULAR_RECIBOS '" + lblMensaje.Text.TrimEnd + "', '" + Session("Codigo_Clie") + "', '" + Usuario_Aut + "', '" + txtComentario.Text + "'"
            conf.EjecutaSql(Sql)

            Panel2.Visible = False
            Panel1.Visible = True
            btnBuscar_Click(sender, e)
        End If

    End Sub
    Public Sub RedirectToCobrosAdvanced(sender As Object, e As EventArgs) Handles btnCobrosAdvanced.Click
        Response.Redirect("~/Dashboards/Cobros/Cobros.aspx")
    End Sub

End Class