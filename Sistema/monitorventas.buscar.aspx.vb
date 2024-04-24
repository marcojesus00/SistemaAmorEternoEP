Partial Public Class monitorventas
    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Dim conf, Conf1 As New Configuracion(Usuario, Clave, "AEVENTAS", Servidor)
        Dim Sql, Sql1 As String

        If txtFechaInicio.Text.Length > 0 Then
            Session.Add("InitialDate", txtFechaInicio.Text)
        Else
            Session.Add("InitialDate", DateTime.Now.ToString("yyyy-MM-dd"))
        End If

        If txtFechaInicio.Text.Length > 0 Then
            Session.Add("FinalDate", txtFechaFinal.Text)
        Else
            Session.Add("FinalDate", DateTime.Now.ToString("yyyy-MM-dd"))
        End If

        If dlMostrar.SelectedIndex = 0 And dllider.SelectedValue = "TODOS" Then
            gvMonitor.Columns(1).Visible = False
            gvMonitor.Columns(2).Visible = False
            lblVerdes.Visible = True
            Sql = " DECLARE @FECHA DATE "
            Sql = " DECLARE @FECHAFINAL DATE "

            Sql += " SET @FECHA = '" + Session("InitialDate") + "'"
            Sql += " SET @FECHAFINAL = '" + Session("FinalDate") + "'"

            Sql += " Select  B.VEND_LIDER [Codigo], c.Nombre_vend [Lider], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUM(A.Ventas) Ventas, SUM(A.Verdes) Verdes, D.vzon_nombre [Zona], C.Tel_vendedo Telefono "
            Sql += " FROM ( "
            Sql += " Select DISTINCT A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Ventas, 0 Verdes FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.RCODVEND = B.CONT_VENDED WHERE CONVERT(DATE,A.Fecha_recib) >= @FECHA AND CONVERT(DATE,A.Fecha_recib) <= @FECHAFINAL AND A.MARCA = 'N' "
            Sql += " UNION ALL "
            Sql += " Select A.VENDEDOR, 0, SUBSTRING(A.HORA,1,2) +':'+ SUBSTRING(A.HORA,3,2) +':00', 0 Recibos, 1 Visitados, 0 Ventas, 0 Verdes FROM NOVENTA A WHERE CONVERT(DATE,A.FECHA) >= @FECHA AND CONVERT(DATE,A.FECHA) <= @FECHAFINAL "
            Sql += " UNION ALL "
            Sql += " Select A.CL_VENDEDOR, 0, CASE WHEN LEN(CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha))) = 1 THEN '0' + CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) ELSE CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) END + ':' + CASE WHEN LEN(CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha))) = 1 THEN '0' + CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) ELSE CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) END +':'+ CASE WHEN LEN(CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha))) = 1 THEN '0' + CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)) ELSE CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)) END, 0 Recibos, 1 Visitados, 0 Ventas, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Verdes FROM CLIENTESN A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.CL_VENDEDOR = B.CONT_VENDED WHERE CONVERT(DATE,A.cl_fecha) >= @FECHA AND CONVERT(DATE,A.cl_fecha) <= @FECHAFINAL AND A.tempo = 'N'"
            Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
            Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
            Sql += " LEFT JOIN FUNAMOR..VZONA D ON C.vzon_codigo = D.vzon_codigo "
            Sql += " Group BY B.VEND_LIDER, C.Nombre_vend, D.vzon_nombre, C.Tel_vendedo "


            If dlRun.SelectedIndex = 0 Then

                Sql1 = "  DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("InitialDate") + "' "
                Sql1 += " exec [SP_MONITOR_VENTAS_l] @Fecha "
            ElseIf dlRun.SelectedIndex = 1 Then

                Sql1 = "  DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("InitialDate") + "' "
                Sql1 += " exec SP_MONITOR_VENTAS @Fecha "
            End If

        End If

        'lblMsg.Text = Sql1
        'Exit Sub


        If dlMostrar.SelectedIndex = 0 And dllider.SelectedValue <> "TODOS" Then
            gvMonitor.Columns(1).Visible = False
            gvMonitor.Columns(2).Visible = False
            lblVerdes.Visible = True

            'If Usuario_Aut = Session("UsuarioLider") Then
            '    gvMonitor.Columns(8).Visible = False
            'End If
            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("InitialDate") + "'"
            Sql += " Select  A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUM(A.Ventas) Ventas, SUM(A.Verdes) Verdes, SUBSTRING(C.Nombre_vend,1,16) [Lider], D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
            Sql += " FROM ( "
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
                Sql1 += " SET @FECHA = '" + Session("InitialDate") + "' "
                Sql1 += " exec [SP_MONITOR_VENTAS_l] @Fecha "
            ElseIf dlRun.SelectedIndex = 1 Then

                'Sql1 = "  DECLARE @FECHA DATE "
                'Sql1 += " SET @FECHA = '" + Session("InitialDate") + "' "
                'Sql1 += " exec SP_MONITOR_VENTAS @Fecha "


                Sql1 = " DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("InitialDate") + "' "
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
            gvMonitor.Columns(1).Visible = False
            gvMonitor.Columns(2).Visible = False

            lblVerdes.Visible = False
            'If Usuario_Aut = Session("UsuarioLider") Then
            '    gvMonitor.Columns(8).Visible = False
            ''End If

            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("InitialDate") + "'"
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
                Sql1 += " SET @FECHA = '" + Session("InitialDate") + "'"
                Sql1 += " EXEC [SP_MONITOR_VENTAS_X_LIQUID] @Fecha "

            ElseIf dlRun.SelectedIndex = 1 Then


                Sql1 = "  DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("InitialDate") + "' "
                Sql1 += " EXEC SP_MONITOR_VENTAS_X_LIQUID_Sistema  @Fecha "

            End If
            'lblMsg.Text = Sql1
            'Exit Sub
        End If

        If dlMostrar.SelectedIndex = 2 Then
            gvMonitor.Columns(1).Visible = False
            gvMonitor.Columns(2).Visible = False
            lblVerdes.Visible = False
            If Usuario_Aut.ToUpper = "MANAGER" Then
                gvMonitor.Columns(1).Visible = True
            End If


            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("InitialDate") + "'"
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
                Sql1 += " SET @FECHA = '" + Session("InitialDate") + "' "
                Sql1 += " exec SP_MONITOR_VENTAS_SIN_LIQUID @Fecha "

            ElseIf dlRun.SelectedIndex = 1 Then


                Sql1 = "  DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("InitialDate") + "' "
                Sql1 += " exec [SP_MONITOR_VENTAS_SIN_LIQUID_SISTEMA] @Fecha "

            End If


        End If

        If dlMostrar.SelectedIndex = 3 Then
            gvMonitor.Columns(1).Visible = False
            gvMonitor.Columns(2).Visible = False
            lblVerdes.Visible = False

            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("InitialDate") + "'"
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
                Sql1 += " SET @FECHA = '" + Session("InitialDate") + "' "
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
                Sql1 += " SET @FECHA = '" + Session("InitialDate") + "' "
                Sql1 += " SELECT DISTINCT TOP 1 A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,60)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono  "
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
            gvMonitor.Columns(1).Visible = True
            gvMonitor.Columns(2).Visible = False
            lblVerdes.Visible = False

            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("InitialDate") + "'"
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
                Sql1 += " SET @FECHA = '" + Session("InitialDate") + "' "
                Sql1 += " exec  [SP_MONITOR_VENTAS_SinProcesar]  @Fecha "


            ElseIf dlRun.SelectedIndex = 0 Then

                Sql1 = " DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("InitialDate") + "' "
                Sql1 += " SELECT DISTINCT A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,60)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono "
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
                Sql1 += " inner JOIN DEPTOZONA DP ON DP.desdepto = CC.departa And rtrim(ltrim(dp.desmuni)) = rtrim(ltrim(Cc.municipio)) "
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
            gvMonitor.Columns(2).Visible = True
            gvMonitor.Columns(1).Visible = False
            lblVerdes.Visible = False

            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("InitialDate") + "'"
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
            Sql1 += " SET @FECHA = '" + Session("InitialDate") + "' "
            Sql1 += " SELECT DISTINCT A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,60)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono  "
            Sql1 += " ,'' ClientesSistema FROM( "
            Sql1 += " SELECT  distinct  case when B.SERVI1DES like '%lote%' or B.SERVI1DES like '%crem%' or B.SERVI1DES like '%placa%' "
            Sql1 += " then 'P01'  +''+SUBSTRING(Num_doc,4,14) "
            Sql1 += " when (B.SERVI1DES not like '%lote%' or B.SERVI1DES not like '%crem%' or B.SERVI1DES not like '%placa%') AND SUBSTRING(Num_doc,1,3) = 'P01' 			 "
            Sql1 += " then rtrim(ltrim(codzona))+''+SUBSTRING(Num_doc,4,14) "
            Sql1 += " when (B.SERVI1DES not like '%lote%' or B.SERVI1DES not like '%crem%' and B.SERVI1DES not like '%placa%') AND SUBSTRING(Num_doc,1,3) <> 'P01' "
            Sql1 += " THEN rtrim(ltrim(codzona))+''+SUBSTRING(Num_doc,4,14) ELSE A.Num_doc end Num_Doc, "
            Sql1 += " A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4 "
            Sql1 += " FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.RCODVEND = B.cont_vended "
            Sql1 += " LEFT join CLIENTESN Cc ON CC.Codigo_clie = A.Codigo_clie And a.RCODVEND = cc.CL_VENDEDOR "
            Sql1 += " inner JOIN DEPTOZONA DP ON DP.desdepto = CC.departa And  rtrim(ltrim(dp.desmuni)) = rtrim(ltrim(Cc.municipio)) "
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
            gvMonitor.Columns(1).Visible = False
            gvMonitor.Columns(2).Visible = False

            Sql = " DECLARE @FECHA DATE "
            'Sql += " SET @FECHA = '" + Session("InitialDate") + "'"
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
            Sql1 += " SET @FECHA = '" + Session("InitialDate") + "' "
            Sql1 += " SELECT DISTINCT A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono "
            Sql1 += " ,'-' ClientesSistema FROM( "
            Sql1 += " SELECT 'PROSPECTO' Num_doc, 0 Por_lempira, CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)) rhora, A.Codigo_clie, A.CL_VENDEDOR RCODVEND, '' Concepto, A.Nombre_clie, CONVERT(DATE,A.cl_fecha) Fecha, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4, A.LONGITUD, A.LATITUD FROM CLIENTESN A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.CL_VENDEDOR = B.cont_vended WHERE A.tempo = 'N'  "
            Sql1 += " ) A "
            Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
            Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "

        End If

        'lblMsg.Text = Sql1
        'Exit Sub

        Datos = conf.EjecutaSql(Sql)

        If Usuario_Aut = "JULIOCAJA" Or Usuario_Aut = "MANAGER" Or Usuario_Aut = "MDERAS" Or Usuario_Aut = "CBONILLA" Or Usuario_Aut = "ABLANDON" Or Usuario_Aut = "IBLANDON" Or Usuario_Aut = "JULIO" And (Datos.Tables(0).Rows.Count = 1 And dlMostrar.SelectedIndex = 1) Then
            Datos1 = Conf1.EjecutaSql(Sql1)
            Console.WriteLine(Sql1)
            Session.Add("GVDetalle", Datos1.Tables(0))

            Session("GVDetalle").DefaultView.RowFilter = "RCODVEND ='" + txtCobrador.Value + "'"
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

            Session("GVDetalle").DefaultView.RowFilter = "RCODVEND ='" + txtCobrador.Value + "'"
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
            gvDetalle2.Columns(2).Visible = True
            gvDetalle2.Columns(1).Visible = False

        ElseIf dlMostrar.SelectedIndex = 0 And Datos.Tables(0).Rows.Count = 1 Then
            Datos1 = Conf1.EjecutaSql(Sql1)
            Session.Add("GVDetalle", Datos1.Tables(0))

            Session("GVDetalle").DefaultView.RowFilter = "RCODVEND ='" + txtCobrador.Value + "'"
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
            gvDetalle2.Columns(0).Visible = False
            gvDetalle2.Columns(1).Visible = True

        Else
            Datos1 = Conf1.EjecutaSql(Sql1)
            Session.Add("GVDetalle", Datos1.Tables(0))


            Datos = conf.EjecutaSql(Sql)
            Datos.Tables(0).DefaultView.Sort = "Cobrado Desc"
            'Dim data = aeCobrosContext.Almacens
            Session.Add("GV", Datos.Tables(0))
            'Session.Add("GV", data.ToList())
            gvMonitor.DataSource = Session("GV")
            gvMonitor.DataBind()
            gvMonitor2.Visible = False
            gvDetalle2.Visible = False
            gvMonitor.Visible = True
        End If

        lblHora.Text = "Actualizado: " + System.DateTime.Now.ToShortTimeString


    End Sub
End Class
