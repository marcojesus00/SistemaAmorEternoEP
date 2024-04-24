Imports System.Drawing
Imports System.IO
Public Class monitorventas
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut As String
    Private Datos, Datos1, Datos2, Datos3 As DataSet
    Private Total As Decimal = 0
    Private Visitados As Decimal = 0
    Private Ventas As Decimal = 0
    Private Verdes As Decimal = 0
    Private Recibos As Decimal = 0
    Private Cobradores As Decimal = 0
    Private Liquida, Liquida2 As String
    Dim aeCobrosContext As New AECobrosEntities2()
    Dim aeVentasContext As New AEVentasEntities4()
    Dim funAmorContext As New FUNAMOREntities2()


    ' Private Tabla As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario") = "" Then
            Response.Redirect("inicio.aspx")
        End If

        Usuario = Session("Usuario")
        Clave = Session("Clave")
        Servidor = Session("Servidor")
        Bd = "AEVentas"
        Usuario_Aut = Session("Usuario_Aut")
        Clave_Aut = Session("Clave_Aut")
        Session.Timeout = 90

        'Tabla.Columns.Add("CodigoVendedor")
        'Tabla.Columns.Add("Producto")
        'Tabla.Columns.Add("Cantidad")
        'Tabla.Columns.Add("ValorProducto")
        'Tabla.Columns.Add("Costo")
        'Tabla.Columns.Add("Vendedor")
        'Tabla.Columns.Add("nombreVend")
        'Tabla.Columns.Add("NombreCliente")
        'Tabla.Columns.Add("CodigoCliente")
        'Tabla.Columns.Add("Letra")
        'Tabla.Columns.Add("Cuota")
        'Tabla.Columns.Add("Valor")
        'Tabla.Columns.Add("Total")

        If Not IsPostBack Then
            Session.Add("Orden", "0")
            dlMostrar.Items.Add("POR DIA DE TRABAJO")
            dlMostrar.Items.Add("POR LIQUIDACION")
            dlMostrar.Items.Add("SIN LIQUIDACION")
            dlMostrar.Items.Add("SIN ACTIVIDAD")
            dlMostrar.SelectedIndex = 0

            dlRun.Items.Add("False")
            dlRun.Items.Add("True")
            dlRun.SelectedIndex = 0
            'Llena el campo de Departamento y ciudad del cliente en la ventana de Editar venta
            DeptoCliente()

            If Usuario_Aut = "MANAGER" Or Usuario_Aut = "MDERAS" Or Usuario_Aut = "REINALDO" Or Usuario_Aut = "yasmin" Or Usuario_Aut = "manager" Or Usuario_Aut = "mderas" Or Usuario_Aut = "reinaldo" Then
                btnArreglarVenta.Visible = True
            End If

            If Session("Reporte") = "Caja Ventas" Then
                dlMostrar.Items.Add("SIN PROCESAR")
                dlMostrar.Items.Add("POR PROCESADOS")
                dlMostrar.SelectedIndex = 4
            End If
            dlMostrar.Items.Add("VERDES")



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

            'cambiado a ef
            Dim conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
            'Dim SQL2 As String
            'SQL2 = " SELECT LTRIM(RTRIM(A.Nombre_vend)) Nombre_vend FROM FUNAMOR..VENDEDOR A WHERE A.Cod_vendedo = '" + Usuario_Aut + "' "
            'Datos1 = conf2.EjecutaSql(SQL2)
            Dim Vendedor = funAmorContext.VENDEDORs.Where(Function(v) v.Cod_vendedo Is Usuario_Aut)
            Dim NombreVendedor = From v In Vendedor Select New With {.Nombre = v.Nombre_vend}
            'If Datos1.Tables(0).Rows.Count > 0 Then
            If NombreVendedor.Any() Then

                'If Datos1.Tables(0).Rows(0).Item("Nombre_vend").ToString.Length > 0 Then
                If NombreVendedor.First().Nombre.Length > 0 Then

                    'dllider.SelectedItem.Text = Datos1.Tables(0).Rows(0).Item("Nombre_vend")
                    dllider.SelectedItem.Text = NombreVendedor.First().Nombre.ToString()
                    dllider.Enabled = False
                    dlRun.Items.Add("False")
                    dlRun.Enabled = False
                End If
            End If

            Dim conf3 As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim SQL3 As String
            SQL3 = " if exists (SELECT LTRIM(RTRIM(A.cod_vendedo)) Lider FROM FUNAMOR..VENDEDOR A WHERE  cod_vendedo <> vend_suplid and A.Cod_vendedo = '" + Usuario_Aut + "') select 1 ExisteLider else select 0 ExisteLider "
            Datos3 = conf3.EjecutaSql(SQL3)

            'If Datos3.Tables(0).Rows.Count > 0 Then
            '    If Datos3.Tables(0).Rows(0).Item("ExisteLider").ToString.Length > 0 Then
            Session.Add("UsuarioLider", Datos3.Tables(0).Rows(0).Item("ExisteLider"))
            'End If



        End If
    End Sub




    'Protected Sub btnSalir_Click(sender As Object, e As ImageClickEventArgs) Handles btnSalir.Click
    '    Response.Redirect("principal.aspx")
    'End Sub

    Protected Sub btnArreglarVenta_click(sender As Object, e As EventArgs) Handles btnArreglarVenta.Click
        PanelEditarVenta.Visible = True
    End Sub


    Protected Sub btnCancelarC_click(sender As Object, e As EventArgs) Handles btnCancelarC.Click
        PanelEditarVenta.Visible = False
    End Sub












    Protected Sub btnRegresar_Click(sender As Object, e As ImageClickEventArgs) Handles btnRegresar.Click
        Panel1.Visible = True
        PanelImpresion.Visible = False
        ifRepote.Dispose()
        ifRepote.Src = ""
    End Sub

    'Protected Sub btnAddClient_Click() Handles btnAddClient.Click

    '    PanelAddCliente.Visible = True

    'End Sub

    'Protected Sub btnCanAdd_Click(sender As Object, e As EventArgs) Handles btnCanAdd.Click

    '    PanelAddCliente.Visible = False

    'End Sub


    Private Sub btnProcesar_Click(sender As Object, e As EventArgs) Handles btnProcesar.Click
        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        'Sql = "EXEC ExportImag '" + Session("Cobrador").ToString.TrimEnd + "' "
        'conf.EjecutaSql(Sql)

        GuardarImagen()

        Sql = "EXEC ENVIARECIBOS '" + Session("Cobrador").ToString.TrimEnd + "','" + Usuario_Aut + "'"
        conf.EjecutaSql(Sql)

        btnBuscar_Click(sender, e)
        PanelImpresion.Visible = False
        Panel1.Visible = True

    End Sub











    Sub DeptoCiudadCliente()
        Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "Select desdepto Depto, ltrim(rtrim(desmuni))  + '-' +codzona CiudadEmpresa, codmuni, coddepto from AEVentas..DEPTOZONA"


        dlCiudadCliente.DataSource = Datos.Tables(0)
        dlCiudadCliente.DataTextField = "CiudadEmpresa"
        dlCiudadCliente.DataValueField = "Codmuni"
        dlCiudadCliente.DataBind()

    End Sub

    'Sub Validar()
    '    If Session("Usuario") = "AMPARO" Then
    '        btnArreglarVenta.Enabled = True
    '    End If

    'End Sub






    Private Sub GuardarImagen()
        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql, FilePath As String

        Sql = " Select A.FIRMA, B.identidad "
        Sql += " From CONTRATON A "
        Sql += " INNER Join CLIENTESN B ON A.Codigo_Clie = B.Codigo_Clie And B.Cl_VENDEDOR = A.cont_vended "
        Sql += " INNER Join RECIBOS C ON A.CODIGO_CLIE = C.CODIGO_CLIE And A.cont_vended = C.RCODVEND "
        Sql += " WHERE A.cont_vended = '" + Session("Cobrador") + "' AND C.LIQUIDA <> 'N' AND LIQUIDA2 = 'N' AND A.FIRMA IS NOT NULL "
        Datos2 = conf.EjecutaSql(Sql)

        For I As Integer = 0 To Datos2.Tables(0).Rows.Count - 1

            Dim imageData As Byte() = DirectCast(Datos2.Tables(0).Rows(I).Item("FIRMA"), Byte())
            If Not imageData Is Nothing Then
                Using ms As New MemoryStream(imageData, 0, imageData.Length)
                    ms.Write(imageData, 0, imageData.Length)
                    FilePath = "C:\inetpub\wwwroot\firmas\" + Datos2.Tables(0).Rows(I).Item("identidad").ToString.TrimEnd + ".jpg"
                    Image.FromStream(ms, True).Save(FilePath, Imaging.ImageFormat.Jpeg)
                End Using
            End If

        Next
    End Sub




    'Campo Codigo de cliente Modal de Editar Venta
    Protected Sub txtCodClienteapp_TextChanged(sender As Object, e As EventArgs)
        Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String


        Sql = "Select distinct r.Codigo_clie Codigo,
         Replace(REPLACE(Replace(Replace(replace(RNOMBRECLI,'Á','A'),'É','E'),'Í','I'),'Ó','O'),'Ú','U') Nombre, 
		Case when r.liquida = 'N' THEN 'Sin Liquidacion'
		when r.liquida <> 'N' then 'Liquidada' end Estatus,SUBSTRING(Num_doc,1,3)Empresa , ISNULL(c.identidad,'') Identidad, 
		Replace(Replace(Replace(Replace(Replace(Replace(replace(concat(rtrim(ltrim(isnull(Dir_cliente,''))),rtrim(ltrim(isnull(Dir2_client,''))),RTRIM(ltrim(isnull(Dir3_client,'')))+RTRIM(ltrim(isnull(Dir4_client,'')))),'Ñ','N'),'Á','A'),'É','E'),'Í','I'),'Ó','O'),'Ú','U'),'°','')Direccion,
        r.Num_doc Documento, r.Fecha_recib Fecha, R.RCODVEND, r.Por_lempira Prima,
        isnull(co.CONT_NUMCUO, 0) NumCuotas,isnull(co.CONT_VALOR,0) Valor, 
		isnull(CONT_CANTI, 0)Cantidad,isnull(co.CONT_SERVI,'0')IdServicio,isnull(SERVI1DES,'')Servicio,
		isnull(CONT_VALCUO, 0) ValorCuota		
		From RECIBOS R
		Left Join CLIENTESN C ON C.Codigo_clie = R.Codigo_clie And R.RCODVEND = C.CL_VENDEDOR
        Left Join CONTRATON CO ON CO.Codigo_clie = R.Codigo_clie And CONT_NUMERO = R.Codigo_clie And CO.cont_vended = R.RCODVEND
        WHERE Liquida2 = 'N' and marca = 'N' AND r.codigo_clie = '" + txtCodClienteapp.Text + "'"


    End Sub



    Private Sub btnCerrarCliapp_Click(sender As Object, e As EventArgs) Handles btnCerrarCliapp.Click

        PanelClientesVE.Visible = False

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

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Codigo As String = gvMonitor.DataKeys(e.Row.RowIndex).Value.ToString()
            Dim gvDetalle As GridView = TryCast(e.Row.FindControl("gvDetalle"), GridView)

            If dlMostrar.SelectedIndex = 1 Or dlMostrar.SelectedIndex = 5 Then
                Session("GVDetalle").DefaultView.RowFilter = "Liquida='" + Liquida.Replace("/", "").Replace(" ", "").Replace(":", "") + "' AND Liquida2='" + Liquida2.Replace("/", "").Replace(" ", "").Replace(":", "") + "' AND RCODVEND='" + Codigo + "'"
            ElseIf dlMostrar.SelectedIndex = 0 And dllider.SelectedValue = "TODOS" Then
                Session("GVDetalle").DefaultView.RowFilter = "Lider='" + Codigo + "'"
            Else
                Session("GVDetalle").DefaultView.RowFilter = "RCODVEND='" + Codigo + "'"
            End If
            Session("GVDetalle").DefaultView.Sort = "Fecha Asc, Hora Asc"

            gvDetalle.DataSource = Session("GVDetalle")
            gvDetalle.DataBind()

            For i = 0 To gvDetalle.Rows.Count - 1
                Select Case gvDetalle.Rows(i).Cells(0).Text
                    Case "NO VENTA"
                        gvDetalle.Rows(i).ControlStyle.ForeColor = System.Drawing.Color.Red
                    Case "PROSPECTO"
                        gvDetalle.Rows(i).ControlStyle.ForeColor = System.Drawing.Color.Green
                End Select

                If gvDetalle.Rows(i).Cells(10).Text.TrimEnd = "ANULADO" Then
                    Session("GVDetalle").DefaultView.RowFilter = "Codigo ='" + gvDetalle.Rows(i).Cells(0).Text.TrimEnd + "'"
                    If Session("GVDetalle").DefaultView.Count > 0 Then
                        gvDetalle.Rows(i).Cells(10).ToolTip = Session("GVDetalle").DefaultView.Item(0).Item("Motivo").ToString
                    End If
                End If

                gvDetalle.Rows(i).Cells(11).ToolTip = Session("GVDetalle").DefaultView.Item(0).Item("Dir_cliente").ToString
                'If gvDetalle.Rows(i).Cells(5).Text.Length > 0 Then
                '    gvDetalle.Rows(i).Cells(5).ToolTip = Session("GVDetalle").DefaultView.Item(0).Item("identidad").ToString
                'End If

                Session("GVDetalle").DefaultView.RowFilter = "Codigo='" + gvDetalle.Rows(i).Cells(0).Text.TrimEnd + "'"
                If Session("GVDetalle").DefaultView.Count > 0 And Session("GVDetalle").DefaultView.Item(0).Item("SERVI2DES").ToString.Length > 0 Then
                    gvDetalle.Rows(i).Cells(9).ToolTip = Session("GVDetalle").DefaultView.Item(0).Item("SERVI2DES").ToString + "
" + Session("GVDetalle").DefaultView.Item(0).Item("SERVI3DES").ToString + " 
" + Session("GVDetalle").DefaultView.Item(0).Item("SERVI4DES").ToString
                    gvDetalle.Rows(i).Cells(9).ControlStyle.ForeColor = System.Drawing.Color.DarkBlue
                End If

            Next
        End If

    End Sub

    Private Sub gvDetalle2_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDetalle2.RowCommand
        If e.CommandName = "Ubicacion" Then
            Session.Add("Reporte", "Ubicacion")
            Session.Add("Codigo", gvDetalle2.Rows(e.CommandArgument.ToString).Cells(2).Text.ToString.TrimEnd)
            Response.Redirect("mapa.aspx")

            ifRepote.Dispose()
            ifRepote.Src = "mapa.aspx"
            btnProcesar.Visible = False
            Panel1.Visible = False
            PanelImpresion.Visible = True
            Exit Sub
        End If

        If e.CommandName = "Anular" And dlMostrar.SelectedIndex = 1 And (Usuario_Aut = "MANAGER" Or Usuario_Aut = "ABLANDON" Or Usuario_Aut = "MDERAS" Or Usuario_Aut = "IBLANDON" Or Usuario_Aut = "JULIOCAJA" Or Usuario_Aut = "JULIO") Then
            Dim conf1 As New Configuracion(Usuario, Clave, "AEVENTAS", Servidor)
            Dim Sql1 As String
            lblMensaje.Text = gvDetalle2.Rows(e.CommandArgument.ToString).Cells(2).Text.ToString.TrimEnd
            Session.Add("Codigo_Clie", gvDetalle2.Rows(e.CommandArgument.ToString).Cells(6).Text.ToString.TrimEnd)
            lblMensaje1.Text = Session("Codigo_Clie") + " - " + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(7).Text.ToString.TrimEnd
            lblMensaje2.Text = gvDetalle2.Rows(e.CommandArgument.ToString).Cells(3).Text.ToString.TrimEnd
            Session.Add("HoraVenta", gvDetalle2.Rows(e.CommandArgument.ToString).Cells(5).Text.ToString.TrimEnd)
            'Sql1 = "EXEC ANULAR_RECIBOS_LIQUIDADOS '" + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(2).Text.ToString.TrimEnd + "', '" + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(6).Text.ToString.TrimEnd + "', '" + Usuario_Aut.TrimEnd + "', '" + txtComentario.Text + "'"
            'conf1.EjecutaSql(Sql1)
            Panel1.Visible = False
            Panel2.Visible = True
            Exit Sub
        End If

        If e.CommandName = "Anular" And dlMostrar.SelectedIndex = 2 Then
            lblMensaje.Text = gvDetalle2.Rows(e.CommandArgument.ToString).Cells(2).Text.ToString.TrimEnd
            Session.Add("Codigo_Clie", gvDetalle2.Rows(e.CommandArgument.ToString).Cells(6).Text.ToString.TrimEnd)
            lblMensaje1.Text = Session("Codigo_Clie") + " - " + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(7).Text.ToString.TrimEnd
            lblMensaje2.Text = gvDetalle2.Rows(e.CommandArgument.ToString).Cells(3).Text.ToString.TrimEnd
            Session.Add("HoraVenta", gvDetalle2.Rows(e.CommandArgument.ToString).Cells(5).Text.ToString.TrimEnd)
            Panel1.Visible = False
            Panel2.Visible = True
        End If
    End Sub

    Private Sub gvMonitor_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvMonitor.RowCommand
        If e.CommandName = "Mapa" Then
            Session.Add("Reporte", "MapaVenta")
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
            Session.Add("Reporte", "VentasLiquida2")
            ifRepote.Dispose()
            ifRepote.Src = "liquidacion.aspx"
            btnProcesar.Visible = False
            Panel1.Visible = False
            PanelImpresion.Visible = True
            Exit Sub
        End If

        If e.CommandName = "Liquidar" And dlMostrar.SelectedIndex = 4 Then
            Session.Add("Reporte", "VentasLiquida")
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
            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String

            Sql = "EXEC LIQUIDA_RECIBOS '" + gvMonitor.Rows(e.CommandArgument.ToString).Cells(4).Text.ToString.TrimEnd + "', '" + Session("InitialDate") + "'"
            conf.EjecutaSql(Sql)
            btnBuscar_Click(sender, e)

        End If
    End Sub











    'Private Sub BtnGuardarSi_Click(sender As Object, e As EventArgs) Handles BtnGuardarSi.Click
    '    PanelClientesVE.Visible = False

    'End Sub








    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Panel2.Visible = False
        Panel1.Visible = True
    End Sub

    Private Sub BtnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        If txtClave.Text.ToUpper <> Clave_Aut.ToUpper Then
            Msg("Clave Incorrecta")
            Exit Sub
        End If

        If dlMostrar.SelectedItem.Text = "POR LIQUIDACION" And (Usuario_Aut.ToUpper = "MANAGER" Or Usuario_Aut = "MDERAS" Or Usuario_Aut.ToUpper = "ABLANDON" Or Usuario_Aut = "IBLANDON" Or Usuario_Aut.ToUpper = "JULIOCAJA" Or Usuario_Aut.ToUpper = "JULIO") Then
            Dim conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql1 As String

            Sql1 = "EXEC ANULAR_RECIBOS_LIQUIDADOS '" + lblMensaje.Text.TrimEnd + "', '" + Session("Codigo_Clie") + "', '" + Usuario_Aut.TrimEnd + "', '" + txtComentario.Text + "','" + Session("HoraVenta") + "'"
            conf1.EjecutaSql(Sql1)

            Panel2.Visible = False
            Panel1.Visible = True
            btnBuscar_Click(sender, e)
            Exit Sub
        End If

        If dlMostrar.SelectedIndex = 2 Then
            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String

            Sql = "EXEC ANULAR_RECIBOS '" + lblMensaje.Text.TrimEnd + "', '" + Session("Codigo_Clie") + "', '" + Usuario_Aut + "', '" + txtComentario.Text + "','" + Session("HoraVenta") + "'"
            conf.EjecutaSql(Sql)

            Panel2.Visible = False
            Panel1.Visible = True
            btnBuscar_Click(sender, e)
        End If
    End Sub






    Private Sub BtnNoSalvar_Clik(sender As Object, e As EventArgs) Handles BtnNoSalvar.Click
        PanelConfirmacion.Visible = False
    End Sub



    Private Sub btnCanModalCl_Clik(sender As Object, e As EventArgs) Handles btnCanModalCl.Click
        PanelEditarVenta.Visible = False
    End Sub



End Class