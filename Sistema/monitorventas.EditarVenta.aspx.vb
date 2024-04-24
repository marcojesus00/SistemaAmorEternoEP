Partial Public Class monitorventas
    Private Sub BtnGuardarCamb_click(seder As Object, e As EventArgs) Handles btnGuardarCamb.Click
        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Prima, ValorCont As Integer
        Dim Sql, Producto, IdProduct As String

        If Session("IdServicio") = "" Then
            IdProduct = Session("IdServicioM")
            Producto = Session("ProductoM")

        Else
            IdProduct = Session("IdServicio")
            Producto = Session("Producto")
        End If



        Sql = "Select serv_codigo Codigo, serv_descri Descripcion
		,serv_cant Equivale
		,serv_precio Precio
		,serv_valoje Cuotas
		,SERV_PMAX  PrecioMaximo
		,SERV_PMINI PrecioMinimo
		from AEVentas..SERVICIO
        WHERE serv_codigo not in ('','08') and serv_precio > 0 and serv_codigo = '" + IdProduct + "'"

        Datos = conf.EjecutaSql(Sql)

        If Session("PrimaM") = "" Then
            Prima = Session("Prima")
            ValorCont = Session("ValorContratoApp")
        Else
            Prima = Session("PrimaM")
            ValorCont = Session("ValorContApp")
        End If


        If txtprod1.Text.Trim.Length = 0 Then
            lblMsjError.Text = "Error: Debe Agregar un Producto"
            lblMsjError.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If

        If txtcuotaApp.Text.TrimEnd = 0 And Prima < txtvalorcontApp.Text Then
            lblMsjError.Text = "Error: Cuotas No debe ser Cero"
            lblMsjError.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If

        If txtcuotaApp.Text.TrimEnd > 0 And Prima = txtvalorcontApp.Text Then
            lblMsjError.Text = "Error: Cuotas debe ser Cero"
            lblMsjError.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If

        If txtLetraApp.Text = 0 And Prima < txtvalorcontApp.Text Then
            lblMsjError.Text = "Error: Debe Ingresar Numero de Letras"
            lblMsjError.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If

        If (txtLetraApp.Text * txtcuotaApp.Text) > txtvalorcontApp.Text Then
            lblMsjError.Text = "Error: Corregir el Valor o numero de Cuota"
            lblMsjError.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If

        If txtcanti1app.Text = 0 Then
            lblMsjError.Text = "Error: Cantidad debe Ser mayor a Cero(0)"
            lblMsjError.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If

        If txtvalorcontApp.Text.TrimEnd > Datos.Tables(0).Rows(0).Item("PrecioMaximo") Then
            lblMsjError.Text = "Error: Valor debe ser menor Que '" + Format(Datos.Tables(0).Rows(0).Item("PrecioMaximo"), "#,##0.00") + "'"
            lblMsjError.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If

        If txtvalorcontApp.Text.TrimEnd < Datos.Tables(0).Rows(0).Item("PrecioMinimo") Then
            lblMsjError.Text = "Error: Valor debe ser Mayor Que '" + Format(Datos.Tables(0).Rows(0).Item("PrecioMinimo"), "#,##0.00") + "'"
            lblMsjError.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If

        'If txtCodClienteapp.Text.Length = 0 Then
        '    lblMsjError.Text = "Error: Debe Seleccionar un Cliente"
        '    lblMsjError.ControlStyle.CssClass = "alert alert-danger"
        '    Exit Sub
        'End If



        If txtcuotaApp.Text * txtLetraApp.Text > ((txtvalorcontApp.Text * txtcanti1app.Text) - Prima) Then
            lblMsjError.Text = "Error: Verifique el N.Cuotas y Letras"
            lblMsjError.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If


        PanelConfirmacion.Visible = True
    End Sub


    Private Sub gvvendEditVent_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvvendEditVent.RowCommand
        Dim Fila As Integer = Convert.ToInt32(e.CommandArgument)
        txtBuscarVended.Text = ""
        txtCodVendEV.Text = gvvendEditVent.Rows(Fila).Cells(1).Text.TrimEnd
        txtnombreVendArr.InnerText = gvvendEditVent.Rows(Fila).Cells(2).Text.TrimEnd
        PanelVendedoresEditar.Visible = False
        'txtVendEV_TextChanged(sender, e)
    End Sub

    Private Sub gvvendEditVent_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvvendEditVent.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(gvvendEditVent, "Select$" & e.Row.RowIndex)
            e.Row.Attributes("style") = "cursor:pointer"
        End If
    End Sub

    Protected Sub txtBuscarVendedorV_TextChanged(sender As Object, e As EventArgs)
        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "Select cod_vendedo Vendedor, Nombre_vend Nombre from FUNAMOR..VENDEDOR WHERE COD_VENDEDO + nombre_vend like '%" + txtBuscarVended.Text.TrimEnd + "%'"
        Datos = conf.EjecutaSql(Sql)

        gvvendEditVent.DataSource = Datos.Tables(0)
        gvvendEditVent.DataBind()

    End Sub
    Protected Sub DlempresaArr_TextChanged(sender As Object, e As EventArgs)
        Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "Select * from (Select '' Codigo, '' Descripcion
                Union all
                Select 'N' Codigo, 'SinLiquidacion' Descripcion 
                union all
                SElect 'X' Codigo, 'Anular'
                union all
                Select CONCAT(DATEPART(year, GETDATE()) , CASE WHEN LEN(DATEPART(MONTH,GETDATE()))=1 THEN CONCAT('0', DATEPART(MONTH,GETDATE())) ELSE CONCAT('', DATEPART(MONTH,GETDATE()))  END ,
                CASE WHEN LEN(DATEPART(DAY,GETDATE()))=1 THEN CONCAT('0', DATEPART(DAY,GETDATE())) ELSE CONCAT('', DATEPART(DAY,GETDATE())) END  , CASE WHEN LEN(DATEPART(HOUR,GETDATE()))=1 THEN CONCAT('0', DATEPART(HOUR,GETDATE())) 
                ELSE CONCAT('', DATEPART(HOUR,GETDATE()))  END , CASE WHEN LEN(DATEPART(MINUTE,GETDATE()))=1 THEN CONCAT('0', DATEPART(MINUTE,GETDATE())) ELSE CONCAT('', DATEPART(MINUTE,GETDATE())) END )  Codigo, 'Liquidar' Descripcion -- SinLiquidacion
                union all
                Select 'N' Codigo, 'DesAnular')A
                where Codigo = '" + dlempresaArr.SelectedValue + "'"
        '        parametersForQueries.Add("@codigoEmpresaASearchString", "%" & dlempresaArr.SelectedValue & "%")


        PanelConfirmacion2.Visible = True

    End Sub
    Protected Sub txtVendEV_TextChanged(sender As Object, e As EventArgs)
        Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        'Parametrizando queries para evitar ataques de sql injection
        Sql = "Select cod_vendedo Vendedor, Nombre_vend Nombre from FUNAMOR..VENDEDOR WHERE COD_VENDEDO + nombre_vend like @searchString"
        Dim parameters = New Dictionary(Of String, Object) From {
            {"@searchString", "%" & txtCodVendEV.Text.TrimEnd & "%"}
        }
        Datos = conf.RunQuery(Sql, parameters)
        'conf.EjecutaSql(Sql)
        If txtCodVendEV.Text.Length <> 4 Then
            PanelVendedoresEditar.Visible = True
            gvvendEditVent.DataSource = Datos.Tables(0)
            gvvendEditVent.DataBind()
        Else
            If Datos.Tables(0).Rows.Count > 1 Then
                txtCodVendEV.Text = Datos.Tables(0).Rows(0).Item("Vendedor")
                txtnombreVendArr.InnerText = Datos.Tables(0).Rows(0).Item("Nombre")
                txtBuscarVended.Text = ""
            Else
                PanelVendedoresEditar.Visible = True
            End If
        End If

    End Sub
    Private Sub gvClientesVe_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvClientesVE.RowCommand
        Dim Fila As Integer = Convert.ToInt32(e.CommandArgument)
        Dim conf, confZona As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String, SqlZona As String

        Session.Add("CodigoClienteAPP", gvClientesVE.Rows(Fila).Cells(1).Text)
        Session.Add("CodigoVendedorApp", gvClientesVE.Rows(Fila).Cells(3).Text)

        SqlZona = $"Select  "


        If txtCodClienteapp.Text.Length = 0 Or lblNameClientapp.InnerText = "" Then
            txtCodClienteapp.Text = gvClientesVE.Rows(Fila).Cells(1).Text
            lblNameClientapp.InnerText = gvClientesVE.Rows(Fila).Cells(2).Text
        End If

        txtCodVendEV.Text = gvClientesVE.Rows(Fila).Cells(3).Text.TrimEnd
        txtnombreVendArr.InnerText = gvClientesVE.Rows(Fila).Cells(4).Text.TrimEnd
        dlstatusvend.Items.Add("" + gvClientesVE.Rows(Fila).Cells(5).Text + "")
        dlempresaArr.Items.Add("" + gvClientesVE.Rows(Fila).Cells(6).Text + "")
        TxtPrimaApp.Text = gvClientesVE.Rows(Fila).Cells(11).Text
        txtidentiCliapp.Text = gvClientesVE.Rows(Fila).Cells(7).Text
        txtdir1Cliapp.Text = gvClientesVE.Rows(Fila).Cells(8).Text
        txtvalorcontApp.Text = gvClientesVE.Rows(Fila).Cells(13).Text
        txtcanti1app.Text = gvClientesVE.Rows(Fila).Cells(14).Text
        'txtcodigoprod1.Text = gvClientesVE.Rows(Fila).Cells(15).Text
        txtprod1.Text = gvClientesVE.Rows(Fila).Cells(16).Text
        txtcuotaApp.Text = gvClientesVE.Rows(Fila).Cells(17).Text
        txtvalorcontApp.Text = gvClientesVE.Rows(Fila).Cells(13).Text
        txtLetraApp.Text = gvClientesVE.Rows(Fila).Cells(12).Text
        txttel1app.Text = gvClientesVE.Rows(Fila).Cells(18).Text
        txttel2app.Text = gvClientesVE.Rows(Fila).Cells(19).Text
        'txtcuotaApp.Text =
        Session.Add("EmpresaV", gvClientesVE.Rows(Fila).Cells(6).Text)


        Sql = "Select  cod_Zona Empresa from AEVENTAS..CZONA WHERE COD_ZONA NOT IN ('M','" + gvClientesVE.Rows(Fila).Cells(6).Text + "')"

        Datos = conf.EjecutaSql(Sql)
        dlempresaArr.Items.Add("" + gvClientesVE.Rows(Fila).Cells(6).Text + "")
        dlempresaArr.DataSource = Datos.Tables(0)
        dlempresaArr.DataTextField = "Empresa"
        dlempresaArr.DataValueField = "Empresa"
        dlempresaArr.DataBind()

        PanelClientesVE.Visible = False



    End Sub
    Sub VentasApp()
        Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "Select * from (
                        Select '0' Codigo, '' Descripcion 
                        union all
                        Select 'N' Codigo, 'SinLiquidacion' Descripcion 
                        union all
                        Select 'X' Codigo, 'Anular'
                        union all
                        Select CONCAT(
                            DATEPART(year, GETDATE()) ,
                            CASE WHEN LEN(DATEPART(MONTH,GETDATE()))=1 
                                THEN CONCAT('0', DATEPART(MONTH,GETDATE())) 
                            ELSE CONCAT('', DATEPART(MONTH,GETDATE()))
                            END, 
                            CASE WHEN LEN(DATEPART(DAY,GETDATE()))=1 
                                THEN CONCAT('0', DATEPART(DAY,GETDATE())) 
                            ELSE CONCAT('', DATEPART(DAY,GETDATE())) 
                            END, 
                            CASE WHEN LEN(DATEPART(HOUR,GETDATE()))=1 
                                THEN CONCAT('0', DATEPART(HOUR,GETDATE())) 
                            ELSE CONCAT('', DATEPART(HOUR,GETDATE()))  
                            END, 
                            CASE WHEN LEN(DATEPART(MINUTE,GETDATE()))=1 
                                THEN CONCAT('0', DATEPART(MINUTE,GETDATE())) 
                            ELSE CONCAT('', DATEPART(MINUTE,GETDATE())) 
                            END 
                            )  
                        Codigo, 'Liquidar' Descripcion -- SinLiquidacion
                        union all
                        Select 'N' Codigo, 'DesAnular'
                            ) A"

        dlempresaArr.DataSource = Datos.Tables(0)
        dlempresaArr.DataValueField = "Codigo"
        dlempresaArr.DataTextField = "Descripcion"
        dlempresaArr.DataBind()


    End Sub
    Private Sub BtnSiSalvarCamb_Clik(sender As Object, e As EventArgs) Handles BtnSiSalvarCamb.Click
        Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim spContratoLogQuery, Producto, IdProduct As String

        If Session("IdServicio") = "" Then
            IdProduct = Session("IdServicioM")
            Producto = Session("ProductoM")

        Else
            IdProduct = Session("IdServicio")
            Producto = Session("Producto")
        End If

        'If txtcuotaApp.Text = 0 Then
        '    lblMsg.Text = "Error: Cuotas debe ser Cero"
        '    lblMsg.ControlStyle.CssClass = "alert alert-danger"
        '    Exit Sub
        'End If

        'If TxtPrimaApp.Text < txtvalorcontApp.Text And (txtcuotaApp.Text = 0 Or txtLetraApp.Text = 0) Then
        '    lblMsg.Text = "Error: Debe Ingresar el Valor de la Cuota y N.Letras"
        '    lblMsg.ControlStyle.CssClass = "alert alert-danger"
        '    Exit Sub
        'End If

        'Sql = "Exec SP_CONTRATO_LOG '" + txtCodClienteapp.Text.TrimEnd + "'
        '    ,'" + txtCodClienteapp.Text.TrimEnd.TrimStart + "','" + txtvalorcontApp.Text.TrimEnd.TrimStart + "'
        '    ,'" + Session("Cred") + "'," + Replace(TxtPrimaApp.Text, ",", "") + "
        '    ,'" + txtLetraApp.Text + "','" + txtcuotaApp.Text + "','" + txtCodVendEV.Text + "'
        '    ,'" + IdProduct + "'
        '    ,'" + txtcanti1app.Text + "','','0','0','0','0','0','0'
        '    ,'" + txtvalorcontApp.Text + "','0','0','0','S','F','" + txtidentiCliapp.Text.TrimStart.TrimEnd + "'
        '    ,'" + Producto + "','','','','W'
        '    ,'" + lblNameClientapp.InnerText.TrimEnd.TrimStart + "','202208201200','N',NULL,'" + Usuario_Aut + "'"
        spContratoLogQuery = "Exec SP_CONTRATO_LOG @CONT_NUMERO,
            @Codigo_clie, @CONT_VALOR, @CONT_CREDIT, @CONT_PRIMA,
            @CONT_NUMCUO, @CONT_VALCUO, @cont_vended, @CONT_SERVI,
            @CONT_CANTI, @CONT_COMPA, @CONT_CANT2, @CONT_SERV2, 
            @CONT_CANT3, @CONT_CANT4, @CONT_SERV4, @CONT_SERV4,
            @CONT_SVAL1, @CONT_SVAL2, @CONT_SVAL3, @CONT_SVAL4, 
            @VENTA, @TEMPO, @CEDULA, @SERVI1DES, @SERVI2DES, 
            @SERVI3DES, @SERVI4DES, @SERVIEMPRE, @NOMCLIE, 
            @CIERRE, @LIQUIDA, NULL, @Usuario"

        Dim spContratoLogParameters = New Dictionary(Of String, Object) From {
            {"@CONT_NUMERO", txtCodClienteapp.Text.TrimEnd},
            {"@Codigo_clie", txtCodClienteapp.Text.TrimEnd.TrimStart},
            {"@CONT_VALOR", txtvalorcontApp.Text.TrimEnd.TrimStart},
            {"@CONT_CREDIT", Session("Cred")},
            {"@CONT_PRIMA", Replace(TxtPrimaApp.Text, ",", "")},
            {"@CONT_NUMCUO", txtLetraApp.Text},
            {"@CONT_VALCUO", txtcuotaApp.Text},
            {"@cont_vended", txtCodVendEV.Text},
            {"@CONT_SERVI", IdProduct},
            {"@CONT_CANTI", txtcanti1app.Text},
            {"@CONT_COMPA", ""},
            {"@CONT_CANT2", "0"},
            {"@CONT_SERV2", "0"},
            {"@CONT_CANT3", "0"},
            {"@CONT_SERV3", "0"},
            {"@CONT_CANT4", "0"},
            {"@CONT_SERV4", "0"},
            {"@CONT_SVAL1", txtvalorcontApp.Text},
            {"@CONT_SVAL2", "0"},
            {"@CONT_SVAL3", "0"},
            {"@CONT_SVAL4", "0"},
            {"@VENTA", "S"},
            {"@TEMPO", "F"},
            {"@CEDULA", txtidentiCliapp.Text.TrimStart.TrimEnd},
            {"@SERVI1DES", Producto},
            {"@SERVI2DES", ""},
            {"@SERVI3DES", ""},
            {"@SERVI4DES", ""},
            {"@SERVIEMPRE", "W"},
            {"@NOMCLIE", lblNameClientapp.InnerText.TrimEnd.TrimStart},
            {"@CIERRE", "202208201200"},
            {"@LIQUIDA", "N"},
            {"@Usuario", Usuario_Aut}
        }
        Datos = conf.RunQuery(spContratoLogQuery, spContratoLogParameters)
        'Datos = conf.EjecutaSql(Sql)

        lblMsg.Text = "Datos Guardados Correctamente"
        lblMsg.ControlStyle.CssClass = "alert alert-success"

        txtCodClienteapp.Text = ""
        txtCodVendEV.Text = ""
        txtnombreVendArr.InnerText = ""
        txtidentiCliapp.Text = ""
        Session("ValorContrato") = 0
        Session("PrimaM") = 0
        Session("Prima") = 0
        Session("ProductoM") = ""
        Session("IdServicioM") = ""
        Session("IdServicio") = ""
        lblNameClientapp.InnerText = ""
        'txtCobrador.Text = ""
        TxtPrimaApp.Text = 0
        txttel1app.Text = ""
        txttel2app.Text = ""
        txtLetraApp.Text = ""
        txtvalorcontApp.Text = ""
        txtcuotaApp.Text = ""
        txtcanti1app.Text = ""
        txtdir1Cliapp.Text = ""
        txtFechaInicio.Text = ""
        txtCodVendEV.Text = ""
        txtprod1.Text = ""
        btnGuardarCamb.Enabled = False
        Producto = ""
        IdProduct = ""
        PanelEditarVenta.Visible = False
        PanelConfirmacion.Visible = False
        'txtCobrador.Text = txtCodVendEV.Text
        btnBuscar_Click(sender, e)


    End Sub

    Protected Sub btnBusVendEdt_click(sender As Object, e As EventArgs) Handles btnBusVendEdt.Click
        PanelVendedoresEditar.Visible = True
    End Sub

    Private Sub btnBuscClienVE_Click(sender As Object, e As EventArgs) Handles btnBuscClienVE.Click

        PanelClientesVE.Visible = True
        txtBuscarCliente_TextChanged(sender, e)

    End Sub
    Private Sub btnCambStatus_click(Sender As Object, e As EventArgs) Handles btnCambStatus.Click
        Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        If dlstatusvend.SelectedItem.Value = "Liquidada" Or dlstatusvend.SelectedItem.Value = "SinLiquidacion" Then
            Sql = "EXEC ANULAR_RECIBOS_LIQUIDADOS '" + Session("Documento") + "','" + txtCodClienteapp.Text + "','" + Session("Usuario") + "','" + txtmotivoCambSta.Text.TrimEnd + "'"
        End If

        Datos = conf.EjecutaSql(Sql)


        PanelConfirmacion2.Visible = False
        PanelEditarVenta.Visible = False

    End Sub
    Private Sub btnBuscarProducto_Click(sender As Object, e As EventArgs) Handles btnBuscarProducto.Click
        PanelProductosApp.Visible = True
        txtprod1_TextChanged(sender, e)

    End Sub
    Protected Sub txtprod1_TextChanged(sender As Object, e As EventArgs)
        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String


        Sql = "Select serv_codigo Codigo, serv_descri Descripcion
		,serv_cant Equivale
		,serv_precio Precio
		,serv_valoje Cuotas
		,SERV_PMAX  PrecioMaximo
		,SERV_PMINI PrecioMinimo

		from AEVentas..SERVICIO

        WHERE serv_codigo not in ('','08') and serv_precio > 0"
        Datos = conf.EjecutaSql(Sql)


        gvDetalleProductosContrato.DataSource = Datos.Tables(0)
        gvDetalleProductosContrato.DataBind()

        btnGuardarCamb.Enabled = True

    End Sub
    Protected Sub txtvalorcontApp_TextChanged(sender As Object, e As EventArgs)
        Dim Letra, Cuota As Integer

        If txtcuotaApp.Text.Length > 0 And txtvalorcontApp.Text.Length > 0 And txtLetraApp.Text.Length > 0 Then

            If TxtPrimaApp.Text = txtvalorcontApp.Text Then
                Cuota = 0
                Letra = 0
            Else
                If txtvalorcontApp.Text > 0 And TxtPrimaApp.Text > 0 And txtcuotaApp.Text > 0 Then
                    Letra = (txtvalorcontApp.Text - TxtPrimaApp.Text) / txtcuotaApp.Text
                    Cuota = txtcuotaApp.Text
                End If

            End If

        End If
        txtLetraApp.Text = Letra
        txtcuotaApp.Text = Cuota


        btnGuardarCamb.Enabled = True

    End Sub
    Sub DeptoCliente()
        Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "Select distinct desdepto Depto, coddepto from AEVentas..DEPTOZONA Select desdepto Depto, ltrim(rtrim(desmuni))  + '-' +codzona CiudadEmpresa, codmuni, coddepto from AEVentas..DEPTOZONA"
        'Dim Deptos = aeVentasContext.DEPTOZONAs.GroupBy(Function(d) d.desdepto).Select(Function(d) New With {d.desdepto, d.coddepto})

        Datos = conf.EjecutaSql(Sql)
        'dlDeptoCliente.DataSource = Deptos.ToList()
        dlDeptoCliente.DataTextField = "Depto"
        dlDeptoCliente.DataValueField = "CodDepto"
        dlDeptoCliente.DataBind()


        dlCiudadCliente.DataSource = Datos.Tables(1)
        dlCiudadCliente.DataTextField = "CiudadEmpresa"
        dlCiudadCliente.DataValueField = "Codmuni"
        dlCiudadCliente.DataBind()

    End Sub
    Protected Sub dlDeptoCliente_TextChanged(sender As Object, e As EventArgs)
        Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = $"Select desdepto Depto, ltrim(rtrim(desmuni))  + '-' +codzona CiudadEmpresa, codmuni, coddepto from AEVentas..DEPTOZONA where coddepto = ltrim(rtrim('{dlDeptoCliente.SelectedValue} ')) "
        Datos = conf.EjecutaSql(Sql)

        'dlCiudadCliente.SelectedValue = Datos.Tables(0).Rows(0).Item("CodMuni")
        dlCiudadCliente.DataSource = Datos.Tables(0)
        dlCiudadCliente.DataTextField = "CiudadEmpresa"
        dlCiudadCliente.DataValueField = "Codmuni"
        dlCiudadCliente.DataBind()

    End Sub
    Protected Sub dlCiudadCliente_TextChanged(sender As Object, e As EventArgs)

        Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "Select desdepto Depto, ltrim(rtrim(desmuni))  + '-' +codzona CiudadEmpresa, codmuni, coddepto from AEVentas..DEPTOZONA"



    End Sub
    Protected Sub btnCanModalCl_click(sender As Object, e As EventArgs) Handles btnCanModalCl.Click
        PanelEditarVenta.Visible = False
    End Sub

    'Protected Sub btnGuardarCamb_click(sender As Object, e As EventArgs) Handles btnGuardarCamb.Click
    '    PanelConfirmacion.Visible = True
    'End Sub

    '  Protected Sub BtnGuardarNo_click(sender As Object, e As EventArgs) Handles BtnGuardarNo.Click
    '    PanelConfirmacion.Visible = False
    'End Sub


    'Aqui va toda la Magia de Arreglar una venta
    'Protected Sub BtnGuardarSi_click(sender As Object, e As EventArgs) Handles BtnGuardarSi.Click
    '    PanelConfirmacion.Visible = False


    'End Sub
    Protected Sub btnCerarPVend_click(sender As Object, e As EventArgs) Handles btnCerarPVend.Click
        PanelVendedoresEditar.Visible = False

    End Sub
    'Campo Buscar del Modal de Clientes
    Protected Sub txtBuscarCliente_TextChanged(sender As Object, e As EventArgs)
        Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = " Select distinct r.Codigo_clie Codigo,r.RNOMBRECLI Nombre,R.RCODVEND CodVendedor,ven.nombre_vend,  
		Case when r.liquida = 'N' THEN 'Sin Liquidacion'
		when r.liquida <> 'N' then 'Liquidada' end Estatus,SUBSTRING(Num_doc,1,3)Empresa , ISNULL(c.identidad,'') Identidad, 
		Replace(Replace(Replace(Replace(Replace(Replace(replace(concat(rtrim(ltrim(isnull(Dir_cliente,''))),rtrim(ltrim(isnull(Dir2_client,''))),RTRIM(ltrim(isnull(Dir3_client,'')))+RTRIM(ltrim(isnull(Dir4_client,'')))),'Ñ','N'),'Á','A'),'É','E'),'Í','I'),'Ó','O'),'Ú','U'),'°','')Direccion,
        r.Num_doc Documento, r.Fecha_recib Fecha, r.Por_lempira Prima,
        isnull(co.CONT_NUMCUO, 0) NumCuotas,isnull(co.CONT_VALOR,0) Valor, 
		isnull(CONT_CANTI, 0)Cantidad,isnull(co.CONT_SERVI,'0')IdServicio,isnull(SERVI1DES,'')Servicio,
		isnull(CONT_VALCUO, 0) ValorCuota, ltrim(isnull(c.CL_CELULAR,''))Tel1, ltrim(isnull(c.Telef_clien,''))Tel2		
		From RECIBOS R
		Left Join CLIENTESN C ON C.Codigo_clie = R.Codigo_clie And R.RCODVEND = C.CL_VENDEDOR
        Left Join CONTRATON CO ON CO.Codigo_clie = R.Codigo_clie And CONT_NUMERO = R.Codigo_clie And co.cont_vended = R.RCODVEND
        inner join FUNAMOR..VENDEDOR ven on ven.Cod_vendedo = r.RCODVEND  collate SQL_Latin1_General_CP1_CI_AS
        WHERE Liquida2 = 'N' and marca = 'N' AND r.codigo_clie like '%" + txtBuscarCliente.Text + "%'"

        Datos = conf.EjecutaSql(Sql)

        gvClientesVE.DataSource = Datos.Tables(0)
        gvClientesVE.DataBind()
        'txtCodClienteapp_TextChanged()

    End Sub

    Private Sub btnCerrarPanelProductosApp_Click(sender As Object, e As EventArgs) Handles btnCerrarPanelProductosApp.Click
        PanelProductosApp.Visible = False
    End Sub
    Private Sub BtnCerrarStatus_click(Sender As Object, e As EventArgs) Handles BtnCerrarStatus.Click
        PanelConfirmacion2.Visible = False
        PanelEditarVenta.Visible = False
    End Sub

End Class
