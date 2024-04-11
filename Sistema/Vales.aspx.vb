Imports System.Data.SqlClient

Public Class Vales
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut, Status, SuperUser, Funcion As String
    Private TotalDias As Decimal = 0
    Private Datos As DataSet
    Private Tabla As DataTable = New DataTable
    Private TablaPago As DataTable = New DataTable
    Private Conector As SqlConnection
    Private Adaptador As SqlDataAdapter

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario_Aut") = "" Then
            Response.Redirect("inicio.aspx")
        End If
        'Or Session("Destino") <>
        Usuario = Session("Usuario")
        Clave = Session("Clave")
        Servidor = Session("Servidor")
        Bd = "FUNAMOR"
        Usuario_Aut = Session("Usuario_Aut")
        Clave_Aut = Session("Clave_Aut")
        Session.Timeout = 90
        'Usuario = Session("Usuario")
        'Clave = Session("Clave")
        'Bd = Session("Bd")
        'Servidor = Session("Sevidor")
        'Funcion = Session("Funcion")
        'Session.Timeout = Session("Tiempo")

        Tabla.Columns.Add("Codigo")
        Tabla.Columns.Add("Producto")
        Tabla.Columns.Add("Cantidad")
        Tabla.Columns.Add("Precio")
        Tabla.Columns.Add("PrecioConDes")
        Tabla.Columns.Add("Costo")
        Tabla.Columns.Add("Descuento")
        Tabla.Columns.Add("Impuesto")
        Tabla.Columns.Add("ImpuestoValor")
        Tabla.Columns.Add("Total")

        TablaPago.Columns.Add("Monto")
        TablaPago.Columns.Add("Medio")
        TablaPago.Columns.Add("Referencia")
        TablaPago.Columns.Add("Banco")

        If Funcion = "Administrador" Then
            'gvProductos.Columns(5).ItemStyle.ForeColor = System.Drawing.Color.Black
        End If

        If Not IsPostBack Then
            txtFecha.Value = DateTime.Now.ToString("yyyy-MM-dd")
            'txtVencimiento.Value = DateTime.Now.ToString("yyyy-MM-dd")
            ' LlenarVendedor()
            'LlenarAlmacen()
            LLenarEmpresa()
            LlenarTipoCobro()
            'LlenarImpuestos()
            LlenarCorrelativo()
            Session.Add("Tabla", Tabla)
            Session.Add("tbPago", TablaPago)
        End If

        'If dlNumeracion.SelectedValue = "Manual" Then
        '    txtNumeracion.Enabled = True
        'Else
        '    txtNumeracion.Enabled = False
        'txtNumeracion.Text = Session("Numeracion").Rows(dlNumeracion.SelectedIndex).Item("NumSig")
        'End If
    End Sub

    'Sub LlenarVendedor()
    '    Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
    '    Dim Sql As String = ""

    '    Sql = "SELECT CONVERT(VARCHAR,A.EncarCod) Codigo, EncarNom Vendedor
    '         FROM ENCARGADO A	            
    '         WHERE Activo = 'Y'
    '            ORDER BY A.EncarCod"
    '    Datos = Conf.EjecutaSql(Sql)

    '    dlVendedor.DataSource = Datos.Tables(0)
    '    dlVendedor.DataTextField = "Vendedor"
    '    dlVendedor.DataValueField = "Codigo"
    '    dlVendedor.DataBind()
    'End Sub

    Sub LlenarCorrelativo()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String = ""

        Sql = "SELECT max(val_numero)Numero
                 FROM Vales where val_empresa = '" + dlempresa.SelectedValue + "'"
        Datos = Conf.EjecutaSql(Sql)

        txtNumeracion.Text = Datos.Tables(0).Rows(0).Item("Numero").ToString.TrimEnd()
        'dlAlmacen.DataTextField = "Descripcion"
        'dlAlmacen.DataValueField = "AlmacenCod"
        'dlAlmacen.DataBind()
    End Sub

    'Protected Sub DropTipoCobVend_TextChanged(sender As Object, e As EventArgs)

    'End Sub

    'Sub LlenarImpuestos()
    '    Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
    '    Dim Sql As String = ""

    '    Sql = "SELECT Descripcion, Porcentaje
    '         FROM Impuestos"
    '    Datos = Conf.EjecutaSql(Sql)
    '    Session.Add("dtImpuesto", Datos.Tables(0))

    '    dlImpuesto.DataSource = Session("dtImpuesto")
    '    dlImpuesto.DataTextField = "Descripcion"
    '    dlImpuesto.DataValueField = "Porcentaje"
    '    dlImpuesto.DataBind()
    'End Sub

    Sub LlenarTipoCobro()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String = ""


        Sql = " Select 'Cobrador' Codigo 
                union all
                Select 'Vendedor' "

        Datos = Conf.EjecutaSql(Sql)
        Session.Add("TipoCobro", Datos.Tables(0))

        DropTipoCobVend.DataSource = Session("TipoCobro")
        DropTipoCobVend.DataTextField = "Codigo"
        DropTipoCobVend.DataValueField = "Codigo"
        DropTipoCobVend.DataBind()

        DropTipoCobVend.SelectedIndex = 0
    End Sub

    Sub LLenarEmpresa()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String = ""

        Sql = "SELECT cod_zona Codigo , Nombre_zona Descripcion
                from czona  
            where cod_zona NOT IN ( ' ' ,'m') order by cod_zona "
        Datos = Conf.EjecutaSql(Sql)
        Session.Add("Empresa", Datos.Tables(0))

        dlempresa.DataSource = Session("Empresa")
        dlempresa.DataTextField = "Descripcion"
        dlempresa.DataValueField = "Codigo"
        dlempresa.DataBind()

        'dlempresa.Items.Add("Manual")
        dlempresa.SelectedIndex = 0
        ' txtNumeracion.Text = Session("Numeracion").Rows(dlNumeracion.SelectedIndex).Item("NumSig")
    End Sub

    'Sub BuscarProductos()
    '    Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
    '    Dim Sql As String = ""

    '    Sql = "SELECT TOP 100 A.CodArticulo Codigo, A.NomArticulo Descripcion, C.Precio, B.Stock, A.Costo
    '         FROM Articulo A
    '         INNER JOIN ArticuloExis B ON A.CodArticulo = B.CodArticulo
    'INNER JOIN ListDet C ON A.CodArticulo = C.CodArticulo
    'INNER JOIN ListEnc D ON C.ListCod = D.ListCod
    '         WHERE A.Activo = 'Y'
    '            AND B.AlmacenCod = '" + dlAlmacen.SelectedValue + "'
    'AND D.ListNom = '" + lblLista.InnerText + "'
    '            AND A.CodArticulo + A.NomArticulo+ + A.CodigoBarra LIKE '%" + txtProductos.Text + "%'"
    '    Datos = Conf.EjecutaSql(Sql)

    '    gvProductos.DataSource = Datos.Tables(0)
    '    gvProductos.DataBind()
    'End Sub

    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub

    'Sub Totales()
    '    Dim Total As Decimal = 0
    '    Dim TotalConDes As Decimal = 0
    '    Dim TotalImp As Decimal = 0

    '    For i As Integer = 0 To Session("Tabla").Rows.Count - 1
    '        Total += Decimal.Parse(Session("Tabla").Rows(i).Item("Precio")) * Decimal.Parse(Session("Tabla").Rows(i).Item("Cantidad"))
    '        TotalConDes += Decimal.Parse(Session("Tabla").Rows(i).Item("Total"))
    '        Session("dtImpuesto").DefaultView.RowFilter = "Descripcion ='" + Session("Tabla").Rows(i).Item("Impuesto") + "'"
    '        TotalImp += Decimal.Parse(Session("Tabla").Rows(i).Item("Total")) * (Decimal.Parse(Session("dtImpuesto").DefaultView.Item(0).Item("Porcentaje")) / 100)
    '    Next
    '    lblSubTotal.InnerText = FormatNumber(Decimal.Round(Total, 2))
    '    lblDescuento.InnerText = FormatNumber(Decimal.Round(Total - TotalConDes, 2))
    '    lblImpuesto.InnerText = FormatNumber(TotalImp)
    '    lblTotal.InnerText = FormatNumber(TotalConDes + TotalImp)

    '    If Decimal.Parse(lblTotal.InnerText.ToString) = 0 Then
    '        dlAlmacen.Enabled = True
    '    Else
    '        dlAlmacen.Enabled = False
    '    End If
    'End Sub

    'Sub TotalPagos()
    '    Dim Total As Decimal = 0
    '    For i As Integer = 0 To Session("tbPago").Rows.Count - 1
    '        Total += Decimal.Parse(Session("tbPago").Rows(i).Item("Monto"))
    '    Next

    '    lblSaldo.InnerText = FormatNumber(Decimal.Parse(lblTotal.InnerText) - Total)
    '    lblPagado.InnerText = FormatNumber(Total)
    'End Sub

    Protected Sub txtBuscarCliente_TextChanged(sender As Object, e As EventArgs)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String = ""

        Sql = " SELECT 
					Codigo,
					Nombre, Tipo
					FROM
					(SELECT  c.codigo_cobr Codigo, c.nombre_cobr Nombre , 'Cobrador'Tipo
					from Cobrador C where COBR_STATUS = 'A'
					union all
					Select cod_vendedo Codigo, v.nombre_vend , 'Vendedor' 
					From Vendedor V
					WHERE VEND_STATUS = 'A'
					)A
					 where Codigo + Nombre LIKE '%" + txtCodigo.Text + "%' and tipo = '" + DropTipoCobVend.SelectedValue.TrimEnd + "'"
        Datos = Conf.EjecutaSql(Sql)

        gvClientes.DataSource = Datos.Tables(0)
        gvClientes.DataBind()
    End Sub

    Protected Sub DropTipoCobVend_TextChanged(sender As Object, e As EventArgs)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String = ""

        If txtCodigo.Text.Length > 0 Then

            Sql = " SELECT 
					Codigo,
					Nombre, Tipo
					FROM
					(SELECT  c.codigo_cobr Codigo, c.nombre_cobr Nombre , 'Cobrador'Tipo
					from Cobrador C where COBR_STATUS = 'A'
					union all
					Select cod_vendedo Codigo, v.nombre_vend , 'Vendedor' 
					From Vendedor V
					WHERE VEND_STATUS = 'A'
					)A
					 where Codigo + Nombre LIKE '%" + txtCodigo.Text + "%' and tipo = '" + DropTipoCobVend.SelectedValue.TrimEnd + "'"
            Datos = Conf.EjecutaSql(Sql)

            txtNombre.Value = Datos.Tables(0).Rows(0).Item("Nombre").ToString.TrimEnd()

        End If

    End Sub

    Protected Sub dlempresa_SelectedIndexChanged(sender As Object, e As EventArgs)

        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String = ""

        Sql = "SELECT max(val_numero)+1 Numero
                 FROM Vales where val_empresa = '" + dlempresa.SelectedValue.TrimEnd + "'"
        Datos = Conf.EjecutaSql(Sql)

        txtNumeracion.Text = Datos.Tables(0).Rows(0).Item("Numero").ToString.TrimEnd()


    End Sub

    Protected Sub txtCodigoCliente_TextChanged(sender As Object, e As EventArgs)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String = ""



        Sql = "declare @Gestor varchar(10),  @Tipo varchar(15)
                Set @Gestor = '" + txtCodigo.Text.TrimEnd.TrimStart + "'
                Set @Tipo = '" + DropTipoCobVend.SelectedValue.TrimEnd + "'
                if exists(Select * from 
			        (Select 'Cobrador' Tipo,codigo_cobr Gestor,nombre_cobr Nombre,COBR_STATUS Estatus from COBRADOR 
			        union all
			        select 'Vendedor' Tipo,Cod_vendedo Gestor,Nombre_vend Nombre,VEND_STATUS from VENDEDOR
		           )A where A.Gestor = @Gestor and tipo = @Tipo
		           )
		           SELECT * from 
			        (Select 'Cobrador' Tipo,codigo_cobr Gestor,nombre_cobr Nombre ,COBR_STATUS Estatus from COBRADOR 
			        union all
			        select 'Vendedor' Tipo,Cod_vendedo Gestor,Nombre_vend Nombre,VEND_STATUS from VENDEDOR
		           )A where A.Gestor = @Gestor and tipo = @Tipo	   
		           ELSE SELECT '' Gestor, 'N' Estatus            
                "
        Datos = Conf.EjecutaSql(Sql)

        If Datos.Tables(0).Rows(0).Item("Estatus") = "N" And txtCodigo.Text.Length >= 4 Then
            lblMsg.Text = "Error:  Codigo de Gestor No existe"
            lblMsg.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If


        If Datos.Tables(0).Rows(0).Item("Estatus") <> "N" And txtCodigo.Text.Length >= 4 Then


            Sql = " SELECT 
					Codigo,
					Nombre, Tipo
					FROM
					(SELECT  c.codigo_cobr Codigo, c.nombre_cobr Nombre , 'Cobrador'Tipo
					from Cobrador C
					union all
					Select cod_vendedo Codigo, v.nombre_vend , 'Vendedor' 
					From Vendedor V
					
					)A
					 where Codigo + Nombre LIKE '%" + txtCodigo.Text + "%' and tipo = '" + DropTipoCobVend.SelectedValue.TrimEnd + "'"
            Datos = Conf.EjecutaSql(Sql)



            txtNombre.Value = Datos.Tables(0).Rows(0).Item("Nombre").ToString.TrimEnd()


        ElseIf txtCodigo.Text.Length < 4 Then

            txtBuscarCliente_TextChanged(sender, e)
            PanelClientes.Visible = True

        End If

        'If Datos.Tables(0).Rows.Count <> 0 Then
        '    Session.Add("Impuesto", Datos.Tables(0).Rows(0).Item("Codimpuesto"))
        '    Session.Add("CodPago", Datos.Tables(0).Rows(0).Item("CodPago"))
        '    txtNombre.Value = Datos.Tables(0).Rows(0).Item("NombreM")
        '    txtRTN.Value = Datos.Tables(0).Rows(0).Item("RTN")
        '    lblLista.InnerText = Datos.Tables(0).Rows(0).Item("ListNom")
        '    lblFormaPago.InnerText = Datos.Tables(0).Rows(0).Item("Descripcion")
        '    txtVencimiento.Value = Datos.Tables(0).Rows(0).Item("Fecha")
        '    dlVendedor.SelectedIndex = Datos.Tables(0).Rows(0).Item("EncarCod")
        '    dlImpuesto.SelectedIndex = Session("Impuesto")
        '    Session("Tabla").Clear
        '    gvDetalle.DataSource = Session("Tabla")
        '    gvDetalle.DataBind()
        '    Totales()
        'Else
        '    txtNombre.Value = ""
        '    txtRTN.Value = ""
        '    lblLista.InnerText = ""
        '    lblFormaPago.InnerText = ""
        '    txtVencimiento.Value = DateTime.Now.ToString("yyyy-MM-dd")
        '    dlImpuesto.SelectedIndex = 0
        '    dlVendedor.SelectedIndex = 0
        'End If
    End Sub

    'Protected Sub txtBuscar_TextChanged(sender As Object, e As EventArgs)
    '    Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
    '    Dim Sql As String = ""

    '    Sql = "SELECT TOP 100 A.CodArticulo Codigo, A.NomArticulo Descripcion, C.Precio, B.Stock, A.Costo
    '         FROM Articulo A
    '         INNER JOIN ArticuloExis B ON A.CodArticulo = B.CodArticulo
    'INNER JOIN ListDet C ON A.CodArticulo = C.CodArticulo
    'INNER JOIN ListEnc D ON C.ListCod = D.ListCod
    '         WHERE A.Activo = 'Y'
    '            AND B.AlmacenCod = '" + dlAlmacen.SelectedValue + "'
    'AND D.ListNom = '" + lblLista.InnerText + "'
    '            AND A.CodArticulo + A.NomArticulo + A.CodigoBarra LIKE '%" + txtBuscar.Text + "%'"
    '    Datos = Conf.EjecutaSql(Sql)

    '    If Datos.Tables(0).Rows.Count = 1 Then
    '        Dim Fila As DataRow = Session("Tabla").NewRow

    '        Fila("Codigo") = Datos.Tables(0).Rows(0).Item("Codigo")
    '        Fila("Producto") = Datos.Tables(0).Rows(0).Item("Descripcion")
    '        Fila("Cantidad") = FormatNumber(1)
    '        Fila("Precio") = FormatNumber(Datos.Tables(0).Rows(0).Item("Precio"))
    '        Fila("PrecioConDes") = FormatNumber(Datos.Tables(0).Rows(0).Item("Precio"))
    '        Fila("Costo") = Datos.Tables(0).Rows(0).Item("Costo")
    '        Fila("Descuento") = "0.00"
    '        Fila("Impuesto") = dlImpuesto.SelectedItem.Text
    '        Fila("Total") = FormatNumber(Datos.Tables(0).Rows(0).Item("Precio"))
    '        Session("Tabla").Rows.Add(Fila)

    '        'gvDetalle.DataSource = Session("Tabla")
    '        'gvDetalle.DataBind()
    '        'Totales()
    '        'txtBuscar.Text = ""
    '        'txtBuscar.Focus()
    '    Else
    '        ' txtProductos.Text = txtBuscar.Text
    '        btnBuscarProductos2_Click(sender, e)
    '    End If
    'End Sub

    Protected Sub txtProductos_TextChanged(sender As Object, e As EventArgs)
        PanelProductos.Visible = True
        'PanelTotales.Visible = False
        txtCodProducto.InnerText = ""
        txtNomProducto.Text = ""
        txtCantidad.Text = ""
        txtPrecio.Text = ""
        'BuscarProductos()
    End Sub



    Private Sub gvClientes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvClientes.RowCommand
        Dim Fila As Integer = Convert.ToInt32(e.CommandArgument)
        txtCodigo.Text = gvClientes.Rows(Fila).Cells(1).Text.TrimEnd
        PanelClientes.Visible = False
        ' PanelTotales.Visible = True
        txtCodigoCliente_TextChanged(sender, e)
    End Sub

    Private Sub btnBuscarCliente_Click(sender As Object, e As EventArgs) Handles btnBuscarCliente.Click
        txtBuscarCliente_TextChanged(sender, e)
        PanelClientes.Visible = True
        ' PanelTotales.Visible = False
    End Sub

    Private Sub btnCancelarC_Click(sender As Object, e As EventArgs) Handles btnCancelarC.Click
        PanelClientes.Visible = False
        ' PanelTotales.Visible = True
    End Sub

    'Private Sub btnCerrarPago_Click(sender As Object, e As EventArgs) Handles btnCerrarPago.Click
    '    PanelPagos.Visible = False
    '    'PanelTotales.Visible = True
    'End Sub

    'Private Sub btnAgregarPago_Click(sender As Object, e As EventArgs) Handles btnAgregarPago.Click
    '    If Decimal.Parse(txtMontoP.Text) <= Decimal.Parse(lblSaldo.InnerText) And Decimal.Parse(txtMontoP.Text) <> 0 Then
    '        Dim Fila As DataRow = Session("tbPago").NewRow

    '        Fila("Monto") = FormatNumber(txtMontoP.Text)
    '        Fila("Referencia") = txtRef.Text
    '        ' Fila("Medio") = dlMedio.SelectedValue
    '        Fila("Banco") = dlBanco.SelectedValue
    '        Session("tbPago").Rows.Add(Fila)

    '        'TotalPagos()

    '        gvPagos.DataSource = Session("tbPago")
    '        gvPagos.DataBind()
    '        txtMontoP.Text = Decimal.Parse(lblSaldo.InnerText)
    '    End If
    'End Sub

    Private Sub btnCrear_Click(sender As Object, e As EventArgs) Handles btnCrear.Click
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Cobr As String = ""
        Dim Sql As String = ""
        Dim Pago As String = ""
        Dim Sql1 As String = ""
        Dim Estado As String = ""
        Dim Numeracion As Integer = 0
        ' Dim ImpuestoArticulo, TotalsinDesc As Decimal


        Sql = "declare @Gestor varchar(5),  @Tipo varchar(15)
                Set @Gestor = '" + txtCodigo.Text.TrimEnd.TrimStart + "'
                Set @Tipo = '" + DropTipoCobVend.SelectedValue.TrimEnd + "'
                if exists(Select * from 
			        (Select 'Cobrador' Tipo,codigo_cobr Gestor,nombre_cobr Nombre,COBR_STATUS Estatus from COBRADOR 
			        union all
			        select 'Vendedor' Tipo,Cod_vendedo Gestor,Nombre_vend Nombre,VEND_STATUS from VENDEDOR
		           )A where A.Gestor = @Gestor and tipo = @Tipo
		           )
		           SELECT * from 
			        (Select 'Cobrador' Tipo,codigo_cobr Gestor,nombre_cobr Nombre ,COBR_STATUS Estatus from COBRADOR 
			        union all
			        select 'Vendedor' Tipo,Cod_vendedo Gestor,Nombre_vend Nombre,VEND_STATUS from VENDEDOR
		           )A where A.Gestor = @Gestor and tipo = @Tipo	   
		           ELSE SELECT '' Gestor, 'N' Estatus            
                "
        Datos = Conf.EjecutaSql(Sql)

        If Datos.Tables(0).Rows(0).Item("Estatus") = "N" Then
            lblMsg.Text = "Error:  Codigo de Gestor No existe"
            lblMsg.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If

        If txtvalor.Value.TrimEnd.TrimStart = "" Then
            txtvalor.Value = 0
        End If



        If txtvalor.Value.TrimEnd.TrimStart < 1 Then
            lblMsg.Text = "Error:  El Documento no Puede ser Cero"
            lblMsg.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If


        If Datos.Tables(0).Rows(0).Item("Estatus") = "I" Then
            lblMsg.Text = "Error:  Codigo de Gestor Inactivo"
            lblMsg.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If

    End Sub


End Class