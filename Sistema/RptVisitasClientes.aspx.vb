Imports CrystalDecisions.Shared
Imports System.IO
Imports System.Drawing
Public Class RptVisitasClientes


    Inherits System.Web.UI.Page
        Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut, Supervisor As String
        Private Datos, Datos1, Datos2, Datos3 As DataSet
        Private Total As Decimal = 0
    Private Visitados As Decimal = 0
    Private ClientesConRec As Decimal = 0
    Private Recibos As Decimal = 0
        Private Cobradores As Decimal = 0
        Private Liquida, Liquida2 As String
        Private Tabla As DataTable
    Private Clientes As Decimal
    Private SinVisitar As Decimal
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Session("Usuario") = "" Then
                Response.Redirect("inicio.aspx")
            End If

            Usuario = Session("Usuario")
            Clave = Session("Clave")
            Servidor = Session("Servidor")
        Bd = "Aecobros" 'Session("DB")
        Usuario_Aut = Session("Usuario_Aut")
            Clave_Aut = Session("Clave_Aut")
            Session.Timeout = 90

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
            'Session.Add("Orden", "0")
            'dlMostrar.Items.Add("POR DIA DE TRABAJO")
            'dlMostrar.Items.Add("POR LIQUIDACION")
            'dlMostrar.Items.Add("SIN LIQUIDACION")
            'dlMostrar.Items.Add("SIN ACTIVIDAD")
            'dlMostrar.SelectedIndex = 0

            'If Session("Reporte") = "Caja Cobros" Then
            '    dlMostrar.Items.Add("SIN PROCESAR")
            '    dlMostrar.Items.Add("POR PROCESADOS")
            '    dlMostrar.SelectedIndex = 4
            'End If

            'Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            'Dim SQL As String
            '  SQL = "SELECT B.nombre_cobr FROM AE_COBRADOR A INNER JOIN AE_COBRADOR B ON A.cob_lider = B.codigo_cobr WHERE A.COBR_STATUS = 'A' GROUP BY B.nombre_cobr"
            '    Datos2 = conf.EjecutaSql(SQL)

            '    dllider.Items.Add("")
            '    For I As Integer = 0 To Datos2.Tables(0).Rows.Count - 1
            '        dllider.Items.Add(Datos2.Tables(0).Rows(I).Item("nombre_cobr"))
            '    Next

            '    Dim conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
            '    Dim SQL1 As String
            '    SQL1 = " Select C.NOMBRE_SUCU From FUNAMOR..COBRADOR A INNER JOIN MVSAGEN B ON A.Codigo_Cobr = B.cveage INNER JOIN FUNAMOR..SUCURSAL C ON B.NUMSER = C.Codigo_Sucu WHERE A.COBR_STATUS = 'A' Group BY C.NOMBRE_SUCU "
            '    'SQL1 = " Select B.vzon_nombre From AE_COBRADOR A Left Join AE_VZONA B ON A.cob_zona = B.vzon_codigo WHERE A.COBR_STATUS = 'A' Group BY B.vzon_nombre "
            '    Datos = conf1.EjecutaSql(SQL1)

            '    dlzona.Items.Add("")
            '    For I As Integer = 0 To Datos.Tables(0).Rows.Count - 1
            '        dlzona.Items.Add(Datos.Tables(0).Rows(I).Item("NOMBRE_SUCU"))
            '    Next

            '    Dim conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
            '    Dim SQL2 As String
            '    SQL2 = " SELECT LTRIM(RTRIM(B.Nombre_sucu)) Nombre_sucu FROM FUNAMOR..CABSEG A INNER JOIN FUNAMOR..SUCURSAL B ON A.Codigo_sucu = B.Codigo_sucu WHERE A.SEG_USUARIO = '" + Usuario_Aut + "' "
            '    Datos1 = conf2.EjecutaSql(SQL2)

            'GVDetalle.Datasource = Datos1.Tables(1)

            '    If Datos1.Tables(0).Rows.Count > 0 Then
            '        If Datos1.Tables(0).Rows(0).Item("Nombre_sucu").ToString.Length > 0 And Datos1.Tables(0).Rows(0).Item("Nombre_sucu").ToString <> "PROGRESO" Then
            '            dlzona.SelectedItem.Text = Datos1.Tables(0).Rows(0).Item("Nombre_sucu")
            '            dlzona.Enabled = False
            '        End If
            '    End If

        End If
        'If Usuario_Aut = "CARLOSR" Then
        '    dlZona.SelectedItem.Text = "CHOLUTECA"
        '    dlZona.Enabled = False
        'End If
    End Sub

    Sub Llenar()

    End Sub


    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Dim conf, Conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql, Sql1 As String

        'ActualizaUbicacion()
        If txtfecha1.Value.Length > 0 Then
            Session.Add("F1", txtfecha1.Value)
        Else
            Session.Add("F1", DateTime.Now.ToString("yyyy-MM-dd"))
        End If

        If txtfecha2.Value.Length > 0 Then
            Session.Add("F2", txtfecha2.Value)
        Else
            Session.Add("F2", DateTime.Now.ToString("yyyy-MM-dd"))
        End If


        Sql = "EXEC SP_MON_VISITAS '" + Session("F1") + "','" + Session("F2") + "','" + txtCobrador.Value.Trim + "'"
        Datos = conf.EjecutaSql(Sql)

        Sql1 = "Exec SP_MON_Visitas_Detalle '" + Session("F1") + "','" + Session("F2") + "','" + txtCobrador.Value.Trim + "'"
        Datos1 = Conf1.EjecutaSql(Sql1)

        Session.Add("gvDetalle", Datos1.Tables(0))
        Session.Add("GVPrincipal", Datos.Tables(0))

        GvPrincipal.DataSource = Datos.Tables(0)
        GvPrincipal.DataBind()

        'If Usuario_Aut = "MANAGER" Or (Usuario_Aut = "JULIO" Or Usuario_Aut = "julio") And Datos.Tables(0).Rows.Count = 1 And dlMostrar.SelectedIndex = 1 Then
        '    Datos1 = Conf1.EjecutaSql(Sql1)
        '    Session.Add("GVDetalle", Datos1.Tables(0))

        '    Session("GVDetalle").DefaultView.RowFilter = "codigo_cobr='" + txtCobrador.Value + "'"
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

        'End If


        'lblHora.Text = "Actualizado: " + System.DateTime.Now.ToShortTimeString
    End Sub

    Private Sub GvPrincipal_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GvPrincipal.RowDataBound

        Try
            If ((e.Row.RowType = DataControlRowType.DataRow) And (e.Row.RowType <> DataControlRowType.EmptyDataRow)) Then
                Dim Fila As System.Data.DataRowView = e.Row.DataItem
                'Total += Convert.ToDecimal(Fila.Item(1))
                '' Visitados += Convert.ToDecimal(Fila.Item(2))
                'SinVisitar += Convert.ToDecimal(Fila.Item(2))
                Recibos += Convert.ToDecimal(Fila.Item(8))
                Clientes += 1



                For i = 0 To GvPrincipal.Rows.Count - 1
                    Select Case GvPrincipal.Rows(i).Cells(9).Text
                        Case > 0
                            ClientesConRec += GvPrincipal.Rows.Count
                    End Select
                Next

                'If dlMostrar.SelectedIndex = 1 Or dlMostrar.SelectedIndex = 5 Then
                '    Liquida = Fila.Item(9)
                '    Liquida2 = Fila.Item(10)
                'End If

                'If dlMostrar.SelectedIndex = 0 Then
                '    Verdes += Convert.ToDecimal(Fila.Item(8))
                '    lblVerdes.Text = "Verdes: " + Format(Verdes, "#,##0")
                'End If

                'If dlMostrar.SelectedIndex = 0 Or dlMostrar.SelectedIndex = 1 Or dlMostrar.SelectedIndex = 2 Then
                '    ventas += Convert.ToDecimal(Fila.Item(7))
                '    lblVentas.Text = "Ventas: " + Format(ventas, "#,##0")
                'End If

                'If dlMostrar.SelectedIndex = 3 Then
                '    lblVentas.Text = "Ventas: 0"
                'End If

                'lblTotal.Text = "Cobrado: " + Format(Total, "#,##")
                lblRecibos.Text = "Recibos: " + Format(Recibos, "#,##")
                'lblTotalGanancia.Text = "Clientes Con Recibos: " + Format(ClientesConRec, "#,##")
                LblTotalClientesAsignados.Text = "Total Clientes: " + Clientes.ToString

                'e.Row.Cells(1).Text = Format(Fila.Item(2), "#,##")
            End If
        Catch ex As Exception
            Msg(ex.Message.ToString() + " - " + ex.Source.ToString())
        End Try



        If ((e.Row.RowType = DataControlRowType.DataRow) And (e.Row.RowType <> DataControlRowType.EmptyDataRow)) Then
            Dim Fila As System.Data.DataRowView = e.Row.DataItem
            Dim Codigo As String = Fila.Item("Codigo").ToString
            Dim gvDetalle As GridView = TryCast(e.Row.FindControl("GVDetalle"), GridView)

            Session("gvDetalle").DefaultView.RowFilter = "Codigo='" + Codigo + "'"
            gvDetalle.DataSource = Session("gvDetalle")
            gvDetalle.DataBind()

            'TotalDocumento += Convert.ToDecimal(Fila.Item("Saldo"))
            'TotalVenta += Convert.ToDecimal(Replace(Fila.Item("Corriente"), "-", "0"))
            'TotalCosto += Convert.ToDecimal(Fila.Item("Saldo")) - Convert.ToDecimal(Replace(Fila.Item("Corriente"), "-", "0"))

            'lblTotalDocumento.Text = "Saldo: " + Format(TotalDocumento, "#,##0.00")
            'lblTotalVenta.Text = "Corriente: " + Format(TotalVenta, "#,##0.00")
            'lblTotalCosto.Text = "Vencido: " + Format(TotalCosto, "#,##0.00")
        End If


    End Sub



    Private Sub gvMonitor_Sorting(sender As Object, e As GridViewSortEventArgs) Handles GvPrincipal.Sorting
        Dim Orden As String

        If Session("Orden") = "0" Then
            Orden = "Asc"
            Session.Add("Orden", "1")
        Else
            Orden = "Desc"
            Session.Add("Orden", "0")
        End If

        Session("GVPrincipal").DefaultView.Sort = e.SortExpression.ToString + " " + Orden
        GvPrincipal.DataSource = Session("GVPrincipal")
        GvPrincipal.DataBind()
    End Sub

    Sub Msg(Mensaje As String)
            Dim msg As String
            msg = "<script language='javascript'>"
            msg += "alert('" + Mensaje + "');"
            msg += "<" & "/script>"
            Response.Write(msg)
        End Sub

    'Private Sub gvMonitor_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvMonitor.RowDataBound
    '    Try
    '        If ((e.Row.RowType = DataControlRowType.DataRow) And (e.Row.RowType <> DataControlRowType.EmptyDataRow)) Then
    '            Dim Fila As System.Data.DataRowView = e.Row.DataItem
    '            Total += Convert.ToDecimal(Fila.Item("Cobrado"))
    '            Visitados += Convert.ToDecimal(Fila.Item("Visitados"))
    '            Recibos += Convert.ToDecimal(Fila.Item("Recibos"))
    '            Cobradores += 1
    '        'If dlMostrar.SelectedIndex = 1 Or dlMostrar.SelectedIndex = 5 Then
    '        '    Liquida = Fila.Item("Liquida")
    '        '    Liquida2 = Fila.Item("Procesado")
    '        'End If

    '        lblTotal.Text = "Cobrado: " + Format(Total, "#,##0.00")
    '            lblRecibos.Text = "Recibos: " + Format(Recibos, "#,##0")
    '            lblVisitados.Text = "Visitados: " + Format(Visitados, "#,##0")
    '            lblCobradores.Text = "Cobradores: " + Cobradores.ToString
    '            e.Row.Cells(6).Text = Format(Fila.Item("Cobrado"), "#,##0.00")
    '        End If
    '    Catch ex As Exception
    '        Msg(ex.Message.ToString() + " - " + ex.Source.ToString())
    '    End Try

    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        Dim Codigo As String = gvMonitor.DataKeys(e.Row.RowIndex).Value.ToString()
    '        Dim gvDetalle As GridView = TryCast(e.Row.FindControl("gvDetalle"), GridView)

    '        If dlMostrar.SelectedIndex = 1 Or dlMostrar.SelectedIndex = 5 Then
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
    '            If gvDetalle.Rows(i).Cells(6).Text.TrimEnd = "ANULADO" Then

    '                Session("GVDetalle").DefaultView.RowFilter = "Codigo='" + gvDetalle.Rows(i).Cells(0).Text.TrimEnd + "'"
    '                If Session("GVDetalle").DefaultView.Count > 0 Then
    '                    gvDetalle.Rows(i).Cells(6).ToolTip = Session("GVDetalle").DefaultView.Item(0).Item("Motivo").ToString
    '                End If

    '            End If
    '        Next
    '    End If

    'End Sub

    'Private Sub gvDetalle2_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDetalle2.RowCommand

    '        If e.CommandName = "Anular" Then
    '            lblMensaje.Text = gvDetalle2.Rows(e.CommandArgument.ToString).Cells(1).Text.ToString.TrimEnd
    '            Session.Add("Codigo_Clie", gvDetalle2.Rows(e.CommandArgument.ToString).Cells(5).Text.ToString.TrimEnd)
    '            lblMensaje1.Text = Session("Codigo_Clie") + " - " + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(6).Text.ToString.TrimEnd
    '            lblMensaje2.Text = gvDetalle2.Rows(e.CommandArgument.ToString).Cells(2).Text.ToString.TrimEnd
    '            Panel1.Visible = False
    '            Panel2.Visible = True
    '        End If

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

    'End Sub

    'Private Sub gvMonitor_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvMonitor.RowCommand
    '        If e.CommandName = "Mapa" Then
    '            Session.Add("Reporte", "Mapa")
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
    '            Session.Add("Reporte", "Liquida2")
    '            ifRepote.Dispose()
    '            ifRepote.Src = "liquidacion.aspx"
    '            btnProcesar.Visible = False
    '            Panel1.Visible = False
    '            PanelImpresion.Visible = True
    '            Exit Sub
    '        End If


    '        If e.CommandName = "Liquidar" And dlMostrar.SelectedIndex = 4 Then
    '            Session.Add("Reporte", "Liquida")
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
    '            Dim conf As New Configuracion(Usuario, Clave, "AECOBROS", Servidor)
    '            Dim Sql As String

    '            Sql = "EXEC LIQUIDA_RECIBOS '" + gvMonitor.Rows(e.CommandArgument.ToString).Cells(4).Text.ToString.TrimEnd + "', '" + Session("F1") + "'"
    '            conf.EjecutaSql(Sql)

    '            btnBuscar_Click(sender, e)
    '        End If
    '    End Sub

    'Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
    '        Panel2.Visible = False
    '        Panel1.Visible = True
    '    End Sub

    'Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
    '        If txtClave.Text.ToUpper <> Clave_Aut.ToUpper Then
    '            Msg("Clave Incorrecta")
    '            Exit Sub
    '        End If

    '        If dlMostrar.SelectedIndex = 1 And Usuario_Aut.ToUpper = "MANAGER" Then
    '            Dim conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
    '            Dim Sql1 As String

    '            Sql1 = "EXEC ANULAR_RECIBOS_LIQUIDADOS '" + lblMensaje.Text.TrimEnd + "', '" + Session("Codigo_Clie") + "', '" + Usuario_Aut.TrimEnd + "', '" + txtComentario.Text + "'"
    '            conf1.EjecutaSql(Sql1)

    '            Panel2.Visible = False
    '            Panel1.Visible = True
    '            btnBuscar_Click(sender, e)
    '            Exit Sub
    '        End If

    '        If dlMostrar.SelectedIndex = 2 Then
    '            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
    '            Dim Sql As String

    '            Sql = "EXEC ANULAR_RECIBOS '" + lblMensaje.Text.TrimEnd + "', '" + Session("Codigo_Clie") + "', '" + Usuario_Aut + "', '" + txtComentario.Text + "'"
    '            conf.EjecutaSql(Sql)

    '            Panel2.Visible = False
    '            Panel1.Visible = True
    '            btnBuscar_Click(sender, e)
    '        End If
    '    End Sub

End Class