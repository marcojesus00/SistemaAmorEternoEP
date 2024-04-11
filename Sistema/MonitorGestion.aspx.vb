Public Class MonitorGestion
    Inherits System.Web.UI.Page

    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut As String

    Private Datos, Datos1, Datos2, datos3, DatosCuadre, DatosGridCuadre As DataSet
    Private Total As Decimal = 0
    Private Visitados As Decimal = 0
    Private Recibos As Decimal = 0
    Private Cobradores As Decimal = 0
    Private Funerales As Decimal = 0
    Private Ambientes As Decimal = 0
    Private Inversiones As Decimal = 0
    Private Jardines As Decimal = 0
    Private Liquida, Liquida2 As String
    Private gvDetalle As GridView
    Private GridCua As GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario") = "" Then
            Response.Redirect("inicio.aspx")
        End If

        Usuario = Session("Usuario")
        Clave = Session("Clave")
        Servidor = Session("Servidor")
        Bd = "FUNAMOR" 'Session("Bd")            
        Usuario_Aut = Session("Usuario_Aut")
        Clave_Aut = Session("Clave_Aut")

        If Not IsPostBack Then
            Session.Timeout = 90
            Session.Add("Orden", "0")
            dlMostrar.Items.Add("POR DIA")
            dlMostrar.Items.Add("SUSPENDIDO")
            dlMostrar.Items.Add("SIN TRABAJO")
            dlMostrar.Items.Add("NO UBICADO")
            dlMostrar.Items.Add("Agendado")
            'dlMostrar.Items.Add("CUADRE DIA")

            dlMostrar.SelectedIndex = 0

            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim SQL As String
            SQL = "SELECT B.nombre_cobr FROM FUNAMOR..COBRADOR A INNER JOIN FUNAMOR..COBRADOR B ON A.cob_lider = B.codigo_cobr WHERE A.COBR_STATUS = 'A' GROUP BY B.nombre_cobr"
            Datos2 = conf.EjecutaSql(SQL)

            dlLider.Items.Add("")
            For I As Integer = 0 To Datos2.Tables(0).Rows.Count - 1
                dlLider.Items.Add(Datos2.Tables(0).Rows(I).Item("nombre_cobr"))
            Next

            'Dim conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
            'Dim SQL1 As String
            'SQL1 = " Select B.vzon_nombre From FUNAMOR..COBRADOR A INNER JOIN FUNAMOR..VZONA B ON A.cob_zona = B.vzon_codigo WHERE A.COBR_STATUS = 'A' AND A.Cob_Lider IS NOT NULL Group BY B.vzon_nombre "
            'Datos = conf1.EjecutaSql(SQL1)

            'dlZona.Items.Add("")
            'For I As Integer = 0 To Datos.Tables(0).Rows.Count - 1
            '    dlZona.Items.Add(Datos.Tables(0).Rows(I).Item("vzon_nombre"))
            'Next

        End If
    End Sub

    Protected Sub btnSalir_Click(sender As Object, e As ImageClickEventArgs) Handles btnSalir.Click
        Response.Redirect("principal.aspx")
    End Sub



    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Dim conf, Conf1, conf2, confgridcuadre, confcuadre As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        If txtFecha.Text.Length > 0 Then
            Session.Add("F1", txtFecha.Text)
        Else
            Session.Add("F1", DateTime.Now.ToString("yyyy-MM-dd"))
        End If

        'lblMsg.Text = Sql
        'Exit Sub


        If dlMostrar.SelectedIndex = 0 Then
            GridCuadre.Columns(7).Visible = False
            GridCuadre.Columns(8).Visible = False

            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("F1") + "'"
            Sql += " Select nc.Codigo_clie Codigo, c.Nombre_clie Nombre_Cliente,convert(date,Fecha)Fecha,nc.Hora, ltrim(rtrim(cl_cobrador))+' - '+ltrim(nombre_cobr) Cobrador,  "
            Sql += " Motivo, Lider, Comentario,'0' Visitado,'0' Bajado "
            Sql += " from GestionCobro nc "
            Sql += " LEFT join FUNAMOR..CLIENTES c on c.Codigo_clie = nc.Codigo_clie "
            Sql += " LEFT JOIN COBRADOR COB ON COB.codigo_cobr = NC.Cobrador WHERE nc.Lider Like '%" + dlLider.SelectedValue + "%' "
            Sql += " AND Fecha >= @Fecha  UNION ALL"
            Sql += " Select Codigo_clie Codigo, Nombre_clie Nombre_cliente, convert(date,Fecha)Fecha, SUBSTRING(Hora,1,2)+':'+sUBSTRING(HORA,2,2)Hora, ltrim(rtrim(CVEAGE))+' - '+ltrim(Cob.nombre_cobr) Cobrador, 'VISITA' Motivo, lid.Nombre_cobr Lider, '0' Comentario, Visitado,Cierre Bajado "
            Sql += " From AECOBROS..AGENDA A "
            Sql += " inner join FUNAMOR..COBRADOR cob on cob.codigo_cobr = a.CVEAGE "
            Sql += " inner join FUNAMOR..COBRADOR Lid ON LID.CODIGO_COBR = cob.cob_lider WHERE Lid.Nombre_cobr Like '%" + dlLider.SelectedValue + "%' "
            Sql += " and a.Fecha >= @Fecha AND Gestion = 'WEB' "

        End If
        If dlMostrar.SelectedIndex <> 4 And dlMostrar.SelectedIndex <> 0 Then
            GridCuadre.Columns(7).Visible = False
            GridCuadre.Columns(8).Visible = False

            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("F1") + "'"
            Sql += " Select nc.Codigo_clie Codigo, c.Nombre_clie Nombre_Cliente,convert(date,Fecha)Fecha,nc.Hora, ltrim(rtrim(cl_cobrador))+''+ltrim(nombre_cobr) Cobrador,  "
            Sql += " Motivo, Lider, Comentario, 0 Visitado, 0 Bajado "
            Sql += " from GestionCobro nc "
            Sql += " inner join FUNAMOR..CLIENTES c on c.Codigo_clie = nc.Codigo_clie "
            Sql += " INNER JOIN COBRADOR COB ON COB.codigo_cobr = NC.Cobrador WHERE nc.Lider Like '%" + dlLider.SelectedValue + "%' "
            Sql += " AND motivo LIKE '%" + dlMostrar.SelectedValue + "%' and Fecha >= @Fecha "
        End If

        If dlMostrar.SelectedIndex = 4 Then
            GridCuadre.Columns(6).Visible = False

            Sql = " DECLARE @FECHA DATE "
            Sql += " SET @FECHA = '" + Session("F1") + "'"
            Sql += " Select Codigo_clie Codigo, Nombre_clie Nombre_cliente, convert(date,Fecha)Fecha, SUBSTRING(Hora,1,2)+':'+sUBSTRING(HORA,2,2)Hora, ltrim(rtrim(CVEAGE))+' - '+ltrim(Cob.nombre_cobr) Cobrador, 'Visita' Motivo, lid.Nombre_cobr Lider, 0 Comentario, Visitado,Cierre Bajado "
            Sql += " From AECOBROS..AGENDA A "
            Sql += " inner join FUNAMOR..COBRADOR cob on cob.codigo_cobr = a.CVEAGE "
            Sql += " inner join FUNAMOR..COBRADOR Lid ON LID.CODIGO_COBR = cob.cob_lider WHERE Lid.Nombre_cobr Like '%" + dlLider.SelectedValue + "%' "
            Sql += " and a.Fecha >= @Fecha AND Gestion = 'WEB' "

        End If

        Datos = conf.EjecutaSql(Sql)
        Session.Add("GridCua", Datos.Tables(0))
        GridCuadre.DataSource = Session("GridCua")
        GridCuadre.DataBind()


        lblHora.Text = "Actualizado: " + System.DateTime.Now.ToShortTimeString
    End Sub
    Private Sub btnsavarNC_Click(sender As Object, e As EventArgs) Handles btnsavarNC.Click
        Dim Conf, Conf2 As New Configuracion(Usuario, Clave, "FUNAMOR", Servidor)
        Dim SqlInsert, cliente, SqlCobrador, SqlCliente, valor As String


        If NCFechallamada.Value.Length > 0 Then
            Session.Add("FechaNC", NCFechallamada.Value)
        Else
            Session.Add("FechaNC", DateTime.Now.ToString("yyyy-MM-dd"))
        End If

        cliente = LTrim(RTrim(dropempcli.SelectedValue.Trim)) + "-" + RTrim(LTrim(txtNccodigo.Value.Trim))
        SqlCobrador = " Select case when isnull(( Select cl_cobrador  from FUNAMOR..clientes where codigo_clie = '" + cliente.TrimEnd + "'),0) = ' ' then 0  else isnull((Select cl_cobrador from FUNAMOR..clientes where codigo_clie = '" + cliente.TrimEnd + "'),0) end Cob"

        SqlCliente = " Select case when (Select codigo_clie from FUNAMOR..clientes where Codigo_clie = '" + cliente.ToString + "') = ' ' then 'No' else isnull((Select codigo_clie from FUNAMOR..clientes where Codigo_clie = '" + cliente.ToString + "' ),'No') end Cliente "

        Datos = Conf2.EjecutaSql(SqlCliente)
        Datos1 = Conf.EjecutaSql(SqlCobrador)

        valor = Datos.Tables(0).Rows(0).Item("Cliente").ToString.TrimEnd

        If valor.ToString = "No" Then
            Response.Write("<script language=javascript>alert('Cliente No Existe - INTENTELO NUEVAMENTE');</script>")
        ElseIf DropidMotivo.SelectedValue = "Seleccione" Then
            Response.Write("<script language=javascript>alert('INDIQUE EL MOTIVO - INTENTELO NUEVAMENTE');</script>")

            ''Agendar Visita
        ElseIf DropidMotivo.SelectedValue = "AgendarVisita" Then

            If FechaVisita.Value.Length = 0 Then
                Response.Write("<script language=javascript>alert('INDIQUE LA FECHA DE VISITA');</script>")
            ElseIf idcobrGestion.Value.Length = 0 Then
                Response.Write("<script language=javascript>alert('Indique el Gestor de Cobro');</script>")
            Else
                SqlInsert = "Exec AECOBROS..SP_VS_AgendaVisita ' " + cliente.Trim + "',
            '" + idcobrGestion.Value.TrimEnd + "', 
            '" + FechaVisita.Value + "',
            '" + Session("Usuario_Aut") + "',
            '" + "WEB" + "'"

                If txtcomentarioNC.Value.TrimEnd = "" Then
                    txtcomentarioNC.Value = "-"
                End If
                Conf.EjecutaSql(SqlInsert)
                Response.Write("<script language=javascript>alert('Visita Agendada Exitosamente.!');</script>")

                NCFechallamada.Value = ""
                DropidMotivo.SelectedValue = "Seleccione"
                NCnombretxt.Value = ""
                dropempcli.SelectedValue = "Empresa"
                txtNccodigo.Value = ""
                FechaVisita.Value = ""
                idcobrGestion.Value = ""
            End If
        ElseIf dropempcli.SelectedValue = "Empresa" Then
            Response.Write("<script language=javascript>alert('COLOQUE CODIGO DE EMPRESA - INTENTELO NUEVAMENTE');</script>")

        ElseIf txtNccodigo.Value = " " Then
            Response.Write("<script language=javascript>alert('CODIGO DE CLIENTE OBLIGATORIO');</script>")
        ElseIf DropidMotivo.SelectedValue <> "AgendarVisita" And DropidMotivo.SelectedValue <> "Seleccione" Then

            SqlInsert = "Exec FUNAMOR..SP_VS_GestionCobro ' " + cliente.TrimEnd.TrimStart + "',
            '" + NCnombretxt.Value.TrimEnd + "', 
            '" + "0" + "',
            '" + Datos1.Tables(0).Rows(0).Item("cob").ToString.TrimEnd + "',
            '" + Session("FechaNC") + "',
            '" + Format(DateTime.Now, "HH:mm:ss") + "',
            '" + "0" + "',
            '" + DropidMotivo.SelectedValue + "',
            '" + txtcomentarioNC.Value.TrimEnd + "',
            '" + Session("Usuario_Aut") + "'"

            If txtcomentarioNC.Value.TrimEnd = "" Then
                txtcomentarioNC.Value = "-"
            End If

            Conf.EjecutaSql(SqlInsert)
            Response.Write("<script language=javascript>alert('Datos Guardados Exitosamente.!');</script>")

            ' Response.Redirect("MonitorGestion.aspx")
            NCFechallamada.Value = ""
            DropidMotivo.SelectedValue = "Seleccione"
            NCnombretxt.Value = ""
            dropempcli.SelectedValue = "Empresa"
            txtNccodigo.Value = ""
            FechaVisita.Value = ""
            idcobrGestion.Value = ""

        End If



    End Sub


    Private Sub GridCuadre_Sorting(sender As Object, e As GridViewSortEventArgs) Handles GridCuadre.Sorting
        Dim Orden As String

        If Session("Orden") = "0" Then
            Orden = "Asc"
            Session.Add("Orden", "1")
        Else
            Orden = "Desc"
            Session.Add("Orden", "0")
        End If

        Session("GV").DefaultView.Sort = e.SortExpression.ToString + " " + Orden
        GridCuadre.DataSource = Session("GV")
        GridCuadre.DataBind()
    End Sub

    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub


    Protected Sub btnRegresar_Click(sender As Object, e As ImageClickEventArgs) Handles btnRegresar.Click
        Panel1.Visible = True
        PanelImpresion.Visible = False
        ifRepote.Dispose()
        ifRepote.Src = ""
    End Sub

    Private Sub DropidMotivo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropidMotivo.SelectedIndexChanged

        If DropidMotivo.SelectedValue = "Agendar Visita" Then
            FechaVisita.Visible = True
        End If

    End Sub

    Private Sub btnAtras_Click(sender As Object, e As ImageClickEventArgs) Handles btnAtras.Click

        Response.Redirect("Principal.aspx")

    End Sub



    'Private Sub gvMonitor_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvMonitor.RowDataBound
    '    Try
    '        If ((e.Row.RowType = DataControlRowType.DataRow) And (e.Row.RowType <> DataControlRowType.EmptyDataRow)) Then
    '            Dim Fila As System.Data.DataRowView = e.Row.DataItem
    '            Total += Convert.ToDecimal(Fila.Item(2))
    '            Visitados += Convert.ToDecimal(Fila.Item(6))
    '            Recibos += Convert.ToDecimal(Fila.Item(5))
    '            Funerales += Convert.ToDecimal(Fila.Item(10))
    '            Inversiones += Convert.ToDecimal(Fila.Item(11))
    '            Jardines += Convert.ToDecimal(Fila.Item(12))
    '            Ambientes += Convert.ToDecimal(Fila.Item(13))
    '            Cobradores += 1


    '            If dlMostrar.SelectedIndex = 1 Then
    '                Liquida = Fila.Item(8)
    '                Liquida2 = Fila.Item(9)
    '            End If

    '            lblTotal.Text = "Cobrado: " + Format(Total, "#,##0.00")
    '            lblRecibos.Text = "Recibos: " + Format(Recibos, "#,##0")
    '            lblVisitados.Text = "Visitados: " + Format(Visitados, "#,##0")
    '            lblCobradores.Text = "Cobradores: " + Cobradores.ToString
    '            e.Row.Cells(7).Text = Format(Fila.Item("Cobrado"), "#,##0.00")
    '            lblfune.Text = "Funerales: " + Format(Funerales, "#,##0.00")
    '            lblinv.Text = "Inversiones: " + Format(Inversiones, "#,##0.00")
    '            lbltau.Text = "Tauros: " + Format(Jardines, "#,##0.00")
    '            lblamb.Text = "Ambientes: " + Format(Ambientes, "#,##0.00")
    '            lblto.Text = "Total Cobrado: " + Format(Total, "#,##0.00")
    '        End If

    '    Catch ex As Exception
    '        Msg(ex.Message.ToString() + " - " + ex.Source.ToString())
    '    End Try


    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        Dim Codigo As String = gvMonitor.DataKeys(e.Row.RowIndex).Value.ToString()
    '        gvDetalle = TryCast(e.Row.FindControl("gvDetalle"), GridView)

    '        If dlMostrar.SelectedIndex = 1 Then
    '            Session("GVDetalle").DefaultView.RowFilter = "Liquida='" + Liquida.Replace("/", "").Replace(" ", "").Replace(":", "") + "' AND Liquida2='" + Liquida2.Replace("/", "").Replace(" ", "").Replace(":", "") + "' AND codigo_cobr='" + Codigo + "'"
    '        Else
    '            Session("GVDetalle").DefaultView.RowFilter = "codigo_cobr='" + Codigo + "'"
    '        End If
    '        Session("GVDetalle").DefaultView.Sort = "Fecha Asc, Hora Asc"

    '        gvDetalle.DataSource = Session("GVDetalle")
    '        gvDetalle.DataBind()

    '        For i = 0 To gvDetalle.Rows.Count - 1
    '            Select Case gvDetalle.Rows(i).Cells(0).Text
    '                Case "NO COBRO"
    '                    gvDetalle.Rows(i).ControlStyle.ForeColor = System.Drawing.Color.Red
    '                Case "SUPERVISION"
    '                    gvDetalle.Rows(i).ControlStyle.ForeColor = System.Drawing.Color.Green
    '                Case "NO SUPERVISION"
    '                    gvDetalle.Rows(i).ControlStyle.ForeColor = System.Drawing.Color.Blue
    '            End Select

    '            If gvDetalle.Rows(i).Cells(7).Text.TrimEnd = "ANULADO" Then
    '                Session("GVDetalle").DefaultView.RowFilter = "Codigo='" + gvDetalle.Rows(i).Cells(0).Text.TrimEnd + "'"
    '                If Session("GVDetalle").DefaultView.Count > 0 Then
    '                    gvDetalle.Rows(i).Cells(7).ToolTip = Session("GVDetalle").DefaultView.Item(0).Item("Motivo").ToString
    '                End If
    '            End If

    '            'If Session("GVDetalle").DefaultView.Item(0).Item("Empresa") = "FUNERALES" Then
    '            'End If

    '        Next
    '    End If
    'End Sub

    'Private Sub gvMonitor_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvMonitor.RowCommand
    'If e.CommandName = "Mapa" Then
    '    Session.Add("Reporte", "Mapa")
    '    Session.Add("Codigo", gvMonitor.Rows(e.CommandArgument.ToString).Cells(5).Text.ToString.TrimEnd)
    '    'Response.Redirect("mapa.aspx")

    '    ifRepote.Dispose()
    '    ifRepote.Src = "mapa.aspx"
    '    btnProcesar.Visible = False
    '    Panel1.Visible = False
    '    PanelImpresion.Visible = True
    '    Exit Sub
    'End If

    'If e.CommandName = "Imprimir" And dlMostrar.SelectedIndex = 5 Then
    '    Dim Liquida2 As String = gvMonitor.Rows(e.CommandArgument.ToString).Cells(14).Text.ToString.Substring(0, 4) + gvMonitor.Rows(e.CommandArgument.ToString).Cells(14).Text.ToString.Substring(5, 2) + gvMonitor.Rows(e.CommandArgument.ToString).Cells(14).Text.ToString.Substring(8, 2) + gvMonitor.Rows(e.CommandArgument.ToString).Cells(14).Text.ToString.Substring(11, 2) + gvMonitor.Rows(e.CommandArgument.ToString).Cells(14).Text.ToString.Substring(14, 2)
    '    Session.Add("Cobrador", gvMonitor.Rows(e.CommandArgument.ToString).Cells(5).Text.ToString.TrimEnd)
    '    Session.Add("Liquida2", Liquida2)
    '    Session.Add("Reporte", "Liquida2")

    '    ifRepote.Dispose()
    '    ifRepote.Src = "liquidacion.aspx"
    '    btnProcesar.Visible = False
    '    Panel1.Visible = False
    '    PanelImpresion.Visible = True
    '    Exit Sub
    'End If

    '    If e.CommandName = "Liquidar" And dlMostrar.SelectedIndex = 4 Then
    '        Session.Add("Reporte", "Liquida")
    '        Session.Add("Cobrador", gvMonitor.Rows(e.CommandArgument.ToString).Cells(5).Text.ToString.TrimEnd)
    '        Session.Add("Liquida2", Liquida2)
    '        ifRepote.Dispose()
    '        ifRepote.Src = "liquidacion.aspx"
    '        btnProcesar.Visible = True
    '        Panel1.Visible = False
    '        PanelImpresion.Visible = True
    '        Exit Sub
    '    End If

    '    If e.CommandName = "Liquidar" And dlMostrar.SelectedIndex <> 4 Then
    '        Dim conf As New Configuracion(Usuario, Clave, "AECOBROS", Servidor)
    '        Dim Sql As String
    '        Sql = "EXEC LIQUIDA_RECIBOS '" + gvMonitor.Rows(e.CommandArgument.ToString).Cells(5).Text.ToString.TrimEnd + "', '" + Session("F1") + "'"
    '        conf.EjecutaSql(Sql)
    '        btnBuscar_Click(sender, e)
    '    End If


    'End Sub

    'Private Sub btnCerrar_clik(sender As Object, e As EventArgs) Handles btnCerrar.Click
    '    Session.Add("Reporte", "Caja Cobros")
    '    Response.Redirect("monitorcobros.aspx")
    'End Sub

    'Private Sub gvDetalle2_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDetalle2.RowCommand
    '    If e.CommandName = "Anular" Then
    '        lblMensaje.Text = gvDetalle2.Rows(e.CommandArgument.ToString).Cells(1).Text.ToString.TrimEnd
    '        Session.Add("Codigo_Clie", gvDetalle2.Rows(e.CommandArgument.ToString).Cells(5).Text.ToString.TrimEnd)
    '        lblMensaje1.Text = Session("Codigo_Clie") + " - " + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(6).Text.ToString.TrimEnd
    '        lblMensaje2.Text = gvDetalle2.Rows(e.CommandArgument.ToString).Cells(2).Text.ToString.TrimEnd
    '        Panel1.Visible = False
    '        Panel2.Visible = True
    '    End If

    '    'If e.CommandName = "Anular" And dlMostrar.SelectedIndex = 1 And (Usuario_Aut = "MANAGER" Or Usuario_Aut = "RAMON") Then
    '    '    Dim conf1 As New Configuracion(Usuario, Clave, "AECOBROS", Servidor)
    '    '    Dim Sql1 As String

    '    '    Sql1 = "EXEC ANULAR_RECIBOS_LIQUIDADOS '" + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(1).Text.ToString.TrimEnd + "', '" + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(5).Text.ToString.TrimEnd + "', '" + Usuario_Aut.TrimEnd + "'"
    '    '    conf1.EjecutaSql(Sql1)
    '    '    btnBuscar_Click(sender, e)
    '    '    Exit Sub
    '    'End If

    '    'If e.CommandName = "Anular" And dlMostrar.SelectedIndex = 2 Then
    '    '    Dim conf As New Configuracion(Usuario, Clave, "AECOBROS", Servidor)
    '    '    Dim Sql As String

    '    '    Sql = "EXEC ANULAR_RECIBOS '" + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(1).Text.ToString.TrimEnd + "', '" + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(5).Text.ToString.TrimEnd + "', '" + Usuario_Aut.TrimEnd + "'"
    '    '    conf.EjecutaSql(Sql)
    '    '    btnBuscar_Click(sender, e)
    '    'End If
    'End Sub


    'Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
    '    'If txtClave.Text.ToUpper <> Clave_Aut.ToUpper Then
    '    '    Msg("Clave Incorrecta")
    '    '    Exit Sub
    '    'End If

    '    If dlMostrar.SelectedIndex = 1 And (Usuario_Aut.ToUpper = "MANAGER" Or Usuario_Aut.ToUpper = "RAMON") Then
    '        Dim conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
    '        Dim Sql1 As String

    '        Sql1 = "EXEC ANULAR_RECIBOS_LIQUIDADOS '" + lblMensaje.Text.TrimEnd + "', '" + Session("Codigo_Clie") + "', '" + Usuario_Aut.TrimEnd + "', '" + txtComentario.Text + "'"
    '        conf1.EjecutaSql(Sql1)

    '        Panel2.Visible = False
    '        Panel1.Visible = True
    '        btnBuscar_Click(sender, e)
    '        Exit Sub
    '    End If

    '    If dlMostrar.SelectedIndex = 2 Then
    '        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
    '        Dim Sql As String

    '        Sql = "EXEC ANULAR_RECIBOS '" + lblMensaje.Text.TrimEnd + "', '" + Session("Codigo_Clie") + "', '" + Usuario_Aut + "', '" + txtComentario.Text + "'"
    '        conf.EjecutaSql(Sql)

    '        Panel2.Visible = False
    '        Panel1.Visible = True
    '        btnBuscar_Click(sender, e)
    '    End If
    'End Sub

End Class
''Por Dia de trabajo

'Sql = " DECLARE @FECHA DATE "
'        Sql += " SET @FECHA = '" + Session("F1") + "'"
'        Sql += " Select nc.Codigo_clie Codigo, c.Nombre_clie Cliente,Fecha,nc.hora, ltrim(rtrim(cl_cobrador))+''+ltrim(nombre_cobr) Cobrador,  "
'        Sql += " Motivo, Lider "
'        Sql += " from GestionNoCobro nc "
'        Sql += " inner join FUNAMOR..CLIENTES c on c.Codigo_clie = nc.Codigo_clie "
'        Sql += " INNER JOIN COBRADOR COB ON COB.codigo_cobr = NC.Cobrador WHERE nc.Lider Like '%" + dlLider.SelectedValue + "%' "
'        Sql += " AND motivo LIKE '%" + dlMostrar.SelectedValue + "%' and Fecha >= @Fecha "



