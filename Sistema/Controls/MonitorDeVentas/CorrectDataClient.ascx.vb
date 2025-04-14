Imports System.Data.SqlClient

'<Serializable>
Public Class DataClient
    Inherits System.Web.UI.UserControl
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut As String
    Public Event AlertGenerated As EventHandler(Of AlertEventArgs)
    Public Event PanelEditarVentaVisible As EventHandler
    Public Event PanelConfirmacionVisible As EventHandler

    Public Event ProductTextChanged As EventHandler(Of EventArgs)
    Public Event enableButton As EventHandler(Of EventArgs)
    Public Event ProductButtonClick As EventHandler(Of EventArgs)
    Public Property DataOfClient As DatosDeCliente
        Get
            If Session("DataOfClient") IsNot Nothing Then
                Return Session("DataOfClient")
            Else
                Return Nothing
            End If
        End Get
        Set(value As DatosDeCliente)
            Session("DataOfClient") = value
        End Set
    End Property
    Public Property DataDepartments As IEnumerable(Of Object)
        Get
            If Session("DataDepartments") IsNot Nothing Then
                Return Session("DataDepartments")
            Else
                Return Nothing
            End If
        End Get
        Set(value As IEnumerable(Of Object))
            Session("DataDepartments") = value
        End Set
    End Property
    Public Property DataCities As IEnumerable(Of Object)
        Get
            If Session("DataCities") IsNot Nothing Then
                Return Session("DataCities")
            Else
                Return Nothing
            End If
        End Get
        Set(value As IEnumerable(Of Object))
            Session("DataCities") = value
        End Set
    End Property
    Public Property ProductNombre1Text As String
        Get
            Return textBoxProductNombre1.Text
        End Get
        Set(value As String)
            textBoxProductNombre1.Text = value.Trim()
        End Set
    End Property

    Public Property CuotaContratoAppText As String
        Get
            Return textBoxCuotaContratoApp.Text
        End Get
        Set(value As String)
            textBoxCuotaContratoApp.Text = value
        End Set
    End Property

    Public Property LetraContratoAppText As String
        Get
            Return textBoxLetraContratoApp.Text
        End Get
        Set(value As String)
            textBoxLetraContratoApp.Text = value
        End Set
    End Property

    Public Property ValorContratoAppText As String
        Get
            Return textBoxValorContratoApp.Text
        End Get
        Set(value As String)
            textBoxValorContratoApp.Text = value
        End Set
    End Property

    Public Property CantidadProducto1appText As String
        Get
            Return textBoxCantidadProducto1app.Text
        End Get
        Set(value As String)
            textBoxCantidadProducto1app.Text = value
        End Set
    End Property

    Dim msg = "Error inesperado: "
    Public Property IdentificationText As String
        Get
            Return txtidentiCliapp.Text
        End Get
        Set(value As String)
            txtidentiCliapp.Text = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        Usuario = Session("Usuario")
        Clave = Session("Clave")
        Servidor = Session("Servidor")
        Bd = "AEVentas"
        Usuario_Aut = Session("Usuario_Aut")
        Clave_Aut = Session("Clave_Aut")
        Session.Timeout = 90

        If Not IsPostBack Then
            Try
                PopulateDdlDeptoCliente()


            Catch ex As Exception
                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg & ex.Message, "danger"))

            End Try
        End If


    End Sub
    Public Sub OnDataReceived(ByVal sender As Object, ByVal e As ClientDataReceivedEventArgs)
        Try

            Using dbcontext As New AeVentasDbContext


                DataOfClient = dbcontext.DatosDeClientes.Where(Function(c) c.CodigoVendedor.Contains(e.SalesPersonId) And c.Identidad.Contains(e.IdentificationDocument)).OrderByDescending(Function(c) c.cl_fecha).FirstOrDefault()
                Dim deptoCiudadQuery = dbcontext.MunicipiosZonasDepartamentos _
                                      .Select(Function(d) New With {d.MunicipioId, d.NombreMunicipio}).Where(Function(c) c.NombreMunicipio.Contains(DataOfClient.Municipio)).FirstOrDefault()
                Dim department = dbcontext.MunicipiosZonasDepartamentos _
                .Select(Function(d) New With {d.NombreDepartamento, d.DepartamentoId}).Where(Function(d) d.NombreDepartamento.Contains(DataOfClient.Departamento)) _
                .FirstOrDefault()
                DataDepartments = dbcontext.MunicipiosZonasDepartamentos _
                .Select(Function(d) New With {d.NombreDepartamento, d.DepartamentoId}).ToList()
                DataCities = dbcontext.MunicipiosZonasDepartamentos _
                                      .Select(Function(d) New With {d.MunicipioId, d.NombreMunicipio}).ToList()
                txtidentiCliapp.Text = DataOfClient.Identidad.Trim
                TextBoxCelular.Text = DataOfClient.Celular.Trim
                TextBoxPhone.Text = DataOfClient.Telefono.Trim
                TxtPrimaApp.Text = e.InitialPayment.Trim
                txtdir1Cliapp.Text = DataOfClient.Direccion.Trim
                TextBoxAddress2.Text = DataOfClient.Dir2_client.Trim
                TextBoxAddress3.Text = DataOfClient.Dir3_client.Trim
                'dlDeptoCliente.DataTextField = department.NombreDepartamento
                dlDeptoCliente.SelectedValue = department.DepartamentoId.Trim
                PopulateDdlCiudadCliente()
                'dlCiudadCliente.DataTextField = deptoCiudadQuery.NombreMunicipio
                dlCiudadCliente.SelectedValue = deptoCiudadQuery.MunicipioId.Trim
                txtdir1Cliapp.Text = DataOfClient.Direccion.Trim
                dlCiudadCliente.Enabled = True
                dlDeptoCliente.Enabled = True
            End Using
        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error" & ex.Message, "danger"))

        End Try


    End Sub
    Public Sub OnContractDataReceived(ByVal sender As Object, ByVal e As ContractDataReceivedEventArgs)
        Try

            Using dbcontext As New AeVentasDbContext
                textBoxProductNombre1.Text = e.ServiceName.Trim
                textBoxCuotaContratoApp.Text = e.Payment
                textBoxLetraContratoApp.Text = e.BillNumber
                textBoxValorContratoApp.Text = e.TotalAmount
                textBoxCantidadProducto1app.Text = e.Quantity

            End Using
        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error" & ex.Message, "danger"))

        End Try


    End Sub
    Public Sub OnProductDataReceived(ByVal sender As Object, ByVal e As ProductDataReceivedEventArgs)
        Try

            Using dbcontext As New AeVentasDbContext
                textBoxProductNombre1.Text = e.ServiceName.Trim
                textBoxCuotaContratoApp.Text = e.Payment
                textBoxValorContratoApp.Text = e.TotalAmount

            End Using
        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error" & ex.Message, "danger"))

        End Try


    End Sub
    Protected Sub dlCiudadCliente_TextChanged(sender As Object, e As EventArgs)
        Try
            'Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
            'Dim Sql As String

            'Sql = "Select desdepto Depto, ltrim(rtrim(desmuni))  + '-' +codzona CiudadEmpresa, codmuni, coddepto from AEVentas..DEPTOZONA"


        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg & ex.Message, "danger"))

        End Try


    End Sub
    Protected Sub dlDeptoCliente_TextChanged(sender As Object, e As EventArgs)
        Try
            PopulateDdlCiudadCliente()

        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg & ex.Message, "danger"))

        End Try

    End Sub
    Sub PopulateDdlCiudadCliente()
        Dim SelectedDeptoCode As String = dlDeptoCliente.SelectedValue.Trim
        Using dbcontext As New AeVentasDbContext
            Dim deptoCiudadByDepartment = dbcontext.MunicipiosZonasDepartamentos _
                                  .Where(Function(c) c.DepartamentoId.Contains(SelectedDeptoCode)) _
            .Select(Function(d) New With {.Id = d.MunicipioId.Trim(), .Nombre = d.NombreMunicipio.Trim() & "-" & d.ZonaId.Trim()}) _
            .ToList()
            dlCiudadCliente.Items.Clear()
            dlCiudadCliente.DataSource = deptoCiudadByDepartment
            dlCiudadCliente.DataTextField = "Nombre"
            dlCiudadCliente.DataValueField = "Id"
            dlCiudadCliente.DataBind()
        End Using
    End Sub
    Sub PopulateDdlDeptoCliente()
        Try

            Using dbcontext As New AeVentasDbContext
                Dim departments = dbcontext.MunicipiosZonasDepartamentos _
                .Select(Function(d) New With {.Id = d.DepartamentoId.Trim(), .Nombre = d.NombreDepartamento.Trim()}) _
                                      .Distinct() _
                                      .ToList()

                dlDeptoCliente.DataSource = departments
                dlDeptoCliente.DataTextField = "Nombre"
                dlDeptoCliente.DataValueField = "Id"
                dlDeptoCliente.DataBind()

                dlCiudadCliente.Enabled = False
                dlDeptoCliente.Enabled = False

            End Using
        Catch ex As SqlException

            RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error de base de datos: " & ex.Message, "danger"))

        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg & ex.Message, "danger"))

        End Try


    End Sub
    Public Sub SaveChanges()
        Try
            Dim city = dlCiudadCliente.SelectedItem.Text.Trim
            Dim index As Integer = city.IndexOf("-")

            If index >= 0 Then
                ' If the character is found, remove everything after it
                city = city.Substring(0, index)
            End If

            Dim newDataClient As DatosDeCliente = DataOfClient.DeepCopy()
            newDataClient.Identidad = txtidentiCliapp.Text.Replace("-", String.Empty).Trim
            newDataClient.Celular = TextBoxCelular.Text.Replace("-", String.Empty).Trim
            newDataClient.Telefono = TextBoxPhone.Text.Replace("-", String.Empty).Trim
            newDataClient.Departamento = dlDeptoCliente.SelectedItem.Text.Trim
            newDataClient.Municipio = city
            newDataClient.Direccion = txtdir1Cliapp.Text.Trim
            newDataClient.Dir2_client = TextBoxAddress2.Text.Trim
            newDataClient.Dir3_client = TextBoxAddress3.Text.Trim

            Using Context As New AeVentasDbContext()
                Context.Database.Log = Sub(s) System.Diagnostics.Debug.WriteLine(s)

                Dim parameters As Object() = {
    New SqlParameter("@Identidad", SqlDbType.NVarChar) With {.Value = newDataClient.Identidad},
    New SqlParameter("@Vendedor", SqlDbType.NVarChar) With {.Value = newDataClient.CodigoVendedor},
    New SqlParameter("@Cliente", SqlDbType.NVarChar) With {.Value = newDataClient.Codigo}
}
                Session("OldId") = DataOfClient.Identidad.Replace("-", String.Empty).Trim
                If newDataClient.Identidad <> DataOfClient.Identidad.Replace("-", String.Empty).Trim Then
                    Context.Database.ExecuteSqlCommand("update CLIENTESN set identidad=@Identidad
  where CL_VENDEDOR=@Vendedor and Codigo_clie=@Cliente
  and convert(date,cl_fecha)=convert(date,getdate()) ", parameters)
                End If
                Context.ExecuteVIENECLIENTESN_A3(
        Codigo_clie:=newDataClient.Codigo,
        Nombre_clie:=newDataClient.Nombre,
        identidad:=newDataClient.Identidad,
        tributario:=newDataClient.tributario,
        CL_STATUS:="",
        CL_COMPANIA:="",
        Dir_cliente:=newDataClient.Direccion,
        Dir2_client:=newDataClient.Dir2_client,
        Dir3_client:=newDataClient.Dir3_client,
        Dir4_client:=newDataClient.Dir4_client,
        Dir5_client:=newDataClient.Dir5_client,
        Telef_clien:=newDataClient.Telefono,
        CL_CELULAR:=newDataClient.Celular,
        CL_EMAIL:=newDataClient.Email,
        cl_conyunom:=newDataClient.NombreDelConyuge,
        cl_conyutel:=newDataClient.TelefonoDelConyuge,
        cl_conyudir:=newDataClient.DireccionDelConyuge,
        CL_PARENt:=newDataClient.CL_PARENt,
        Cod_zona:="",
        CL_VENDEDOR:=newDataClient.CodigoVendedor,
        cl_usuario:=newDataClient.Cl_usuario,
        cl_terminal:=newDataClient.Cl_terminal,
        longitud:=newDataClient.Longitud,
        latitud:=newDataClient.Latitud,
        secuencia:=newDataClient.Secuencia,
        diavisita:=newDataClient.Diavisita,
        semana:=newDataClient.Semana,
        cierre:=newDataClient.Cierre,
        tempo:=newDataClient.Tempo,
        departa:=newDataClient.Departamento,
        municipio:=newDataClient.Municipio,
        circuito:=newDataClient.circuito,
        liquida:=newDataClient.Liquida)

                Dim newLogClientesNEdition As New LogEdicionCLIENTESN() With {
    .TiempoDeEdicion = DateTime.Now,
    .EditadoPor = Session("Usuario_Aut"),
    .Codigo_clie = DataOfClient.Codigo,
    .CL_VENDEDOR = DataOfClient.CodigoVendedor,
    .Anterior_identidad = DataOfClient.Identidad,
    .Nuevo_identidad = newDataClient.Identidad,
    .Anterior_Dir_cliente = DataOfClient.Direccion,
    .Nuevo_Dir_cliente = newDataClient.Direccion,
    .Anterior_Dir2_client = DataOfClient.Dir2_client,
    .Nuevo_Dir2_client = newDataClient.Dir2_client,
    .Anterior_Dir3_client = DataOfClient.Dir3_client,
    .Nuevo_Dir3_client = newDataClient.Dir3_client,
    .Anterior_Dir4_client = DataOfClient.Dir4_client,
    .Nuevo_Dir4_client = newDataClient.Dir4_client,
    .Anterior_Dir5_client = DataOfClient.Dir5_client,
    .Nuevo_Dir5_client = newDataClient.Dir5_client,
    .Anterior_Telef_clien = DataOfClient.Telefono,
    .Nuevo_Telef_clien = newDataClient.Telefono,
    .Anterior_CL_CELULAR = DataOfClient.Celular,
    .Nuevo_CL_CELULAR = newDataClient.Celular,
    .Anterior_CL_EMAIL = DataOfClient.Email,
    .Nuevo_CL_EMAIL = newDataClient.Email,
    .Anterior_cl_conyunom = DataOfClient.NombreDelConyuge,
    .Nuevo_cl_conyunom = newDataClient.NombreDelConyuge,
    .Anterior_cl_conyutel = DataOfClient.TelefonoDelConyuge,
    .Nuevo_cl_conyutel = newDataClient.TelefonoDelConyuge,
    .Anterior_cl_conyudir = DataOfClient.DireccionDelConyuge,
    .Nuevo_cl_conyudir = newDataClient.DireccionDelConyuge,
    .Anterior_departa = DataOfClient.Departamento,
    .Nuevo_departa = newDataClient.Departamento,
    .Anterior_municipio = DataOfClient.Municipio,
    .Nuevo_municipio = newDataClient.Municipio
}

                Context.LogsEdicionesCLIENTESN.Add(newLogClientesNEdition)
                Context.SaveChanges()
            End Using

        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg & ex.Message, "danger"))

        End Try
    End Sub





    'Editar contrato

    Protected Sub txtprod1_TextChanged(sender As Object, e As EventArgs)
        Try
            RaiseEvent ProductTextChanged(sender, e)


        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg & ex.Message, "danger"))

        End Try

    End Sub

    Protected Sub ButtonManySalesSameClient_Click(sender As Object, e As EventArgs)
        Try

            Dim result As String
        Dim helper As New FixSalesHelper()
        Dim clientCode As String = Session("CodigoClienteAPP")
        Dim salesPersonCode As String = Session("CodigoVendedorApp")
        If clientCode IsNot Nothing AndAlso salesPersonCode IsNot Nothing Then
            result = helper.CorrectTheSameCLientManyTimes(clientCode:=clientCode.Trim(), salesPerson:=salesPersonCode.Trim())
        Else
            result = "Seleccione una venta primero"
        End If
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(result, "warning"))

        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg & ex.Message, "danger"))
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))


        End Try

    End Sub

    Protected Sub txtvalorcontApp_TextChanged(sender As Object, e As EventArgs)
        Dim Letra As Integer
        Dim Cuota As Decimal
        Try
            If textBoxCuotaContratoApp.Text.Length > 0 And textBoxValorContratoApp.Text.Length > 0 And textBoxLetraContratoApp.Text.Length > 0 And TxtPrimaApp.Text.Length > 0 Then

                If TxtPrimaApp.Text = textBoxValorContratoApp.Text Then
                    Cuota = 0
                    Letra = 0
                Else
                    If textBoxValorContratoApp.Text > 0 And TxtPrimaApp.Text > 0 And textBoxCuotaContratoApp.Text > 0 Then
                        Letra = (textBoxValorContratoApp.Text - TxtPrimaApp.Text) / textBoxCuotaContratoApp.Text

                        Cuota = textBoxCuotaContratoApp.Text
                    End If

                End If
                textBoxLetraContratoApp.Text = Letra
                textBoxCuotaContratoApp.Text = Cuota

                RaiseEvent enableButton(sender, e)
                btnGuardarCamb.Enabled = True

            End If

        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg & ex.Message, "danger"))


        End Try


    End Sub
    Private Sub btnBuscarProducto_Click(sender As Object, e As EventArgs) Handles btnBuscarProducto.Click
        Try
            RaiseEvent ProductButtonClick(sender, e)
            ''PanelProductosApp.Visible = True
            RaiseEvent ProductTextChanged(textBoxProductNombre1, e)

        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg & ex.Message, "danger"))

        End Try

    End Sub
    Private Sub btnGuardarCamb_click(seder As Object, e As EventArgs) Handles btnGuardarCamb.Click
        Try
            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Prima, ValorCont As Integer
            Dim Sql, Producto, IdProduct As String
            Dim isValid = True
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

            Dim Datos = conf.EjecutaSql(Sql)

            If Session("PrimaM") = "" Then
                Prima = Session("Prima")
                ValorCont = Session("ValorContratoApp")
            Else
                Prima = Session("PrimaM")
                ValorCont = Session("ValorContApp")
            End If


            Prima = Session("initialPayment")
            ValorCont = Session("totalAmount")


            If textBoxProductNombre1.Text.Trim.Length = 0 Then
                msg = "Error: Debe agregar un producto"
                isValid = False
            End If

            If textBoxCuotaContratoApp.Text.TrimEnd = 0 And Prima < textBoxValorContratoApp.Text Then
                msg = "Error: Cuotas no debe ser cero"
                isValid = False
            End If

            If textBoxCuotaContratoApp.Text.TrimEnd > 0 And Prima = textBoxValorContratoApp.Text Then
                msg = "Error: Está pagando de más"
                isValid = False
            End If

            If textBoxLetraContratoApp.Text = 0 And Prima < textBoxValorContratoApp.Text Then
                msg = "Error: Debe Ingresar número de letras"
                isValid = False
            End If

            If (textBoxLetraContratoApp.Text * textBoxCuotaContratoApp.Text) > textBoxValorContratoApp.Text Then
                msg = "Error: Corregir el valor o numero de cuota"
                isValid = False
            End If

            If textBoxCantidadProducto1app.Text = 0 Then
                msg = "Error: Cantidad debe ser mayor a cero(0)"
                isValid = False
            End If

            'If txtvalorcontApp.Text.TrimEnd > Datos.Tables(0).Rows(0).Item("PrecioMaximo") Then
            '    lblMsjError.Text = "Error: Valor debe ser menor Que '" + Format(Datos.Tables(0).Rows(0).Item("PrecioMaximo"), "#,##0.00") + "'"
            '    lblMsjError.ControlStyle.CssClass = "alert alert-danger"
            '    Exit Sub
            'End If

            'If txtvalorcontApp.Text.TrimEnd < Datos.Tables(0).Rows(0).Item("PrecioMinimo") Then
            '    lblMsjError.Text = "Error: Valor debe ser Mayor Que '" + Format(Datos.Tables(0).Rows(0).Item("PrecioMinimo"), "#,##0.00") + "'"
            '    lblMsjError.ControlStyle.CssClass = "alert alert-danger"
            '    Exit Sub
            'End If

            'If txtCodClienteapp.Text.Length = 0 Then
            '    lblMsjError.Text = "Error: Debe Seleccionar un Cliente"
            '    lblMsjError.ControlStyle.CssClass = "alert alert-danger"
            '    Exit Sub
            'End If



            If ((textBoxCuotaContratoApp.Text * textBoxLetraContratoApp.Text) - textBoxCuotaContratoApp.Text) > ((textBoxValorContratoApp.Text * textBoxCantidadProducto1app.Text) - Prima) Then
                msg = "Error: Verifique el N.Cuotas y Letras"
                isValid = False
            End If
            If txtidentiCliapp.Text.Trim.Length > 15 Then
                msg = "La identidad no puede ser mayor a 15 caracteres"
                isValid = False

            End If
            If TextBoxCelular.Text.Trim.Length > 20 Then
                msg = "El celular no puede ser mayor a 20 caracteres"
                isValid = False

            End If
            If TextBoxPhone.Text.Trim.Length > 20 Then
                msg = "El teléfono no puede ser mayor a 20 caracteres"
                isValid = False
            End If
            If txtdir1Cliapp.Text.Trim.Length > 80 Or TextBoxAddress2.Text.Trim.Length > 80 Or TextBoxAddress3.Text.Trim.Length > 80 Then
                msg = "Cada línea de dirección no puede ser mayor a 80 caracteres"
                isValid = False
            End If
            If isValid = False Then
                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
                Exit Sub
            End If


            RaiseEvent PanelConfirmacionVisible(Me, EventArgs.Empty)

        Catch ex As Exception


        End Try

    End Sub
    Protected Sub btnCanModalCl_click(sender As Object, e As EventArgs) Handles btnCanModalCl.Click
        RaiseEvent PanelEditarVentaVisible(Me, EventArgs.Empty)
    End Sub
    Public Sub valorcontAppTextChanged(sender As Object, e As EventArgs)
        btnGuardarCamb.Enabled = True
    End Sub


End Class
Public Class ClientDataReceivedEventArgs
    Inherits EventArgs

    Public Property ClientId As String
    Public Property SalesPersonId As String
    Public Property IdentificationDocument As String
    Public Property PhoneNumberOne As String
    Public Property PhoneNumberTwo As String
    Public Property Department As String
    Public Property City As String
    Public Property Address As String
    Public Property InitialPayment As String

    Public Sub New(ClientId As String, salesPersonId As String, identification As String, phone1 As String, phone2 As String, initialPayment As String)
        Me.IdentificationDocument = identification
        Me.PhoneNumberOne = phone1
        Me.PhoneNumberTwo = phone2
        Me.InitialPayment = initialPayment
        Me.ClientId = ClientId
        Me.SalesPersonId = salesPersonId
    End Sub

End Class

Public Class ContractDataReceivedEventArgs
    Inherits EventArgs

    Public Property ServiceId As String
    Public Property ServiceName As String
    Public Property Quantity As String
    Public Property Payment As String
    Public Property TotalAmount As String
    Public Property BillNumber As String
    Public Property serviempre As String

    Public Sub New(serviceId As String, serviceName As String, quantity As String, payment As String, totalAmount As String, billNumber As String, serviempre As String)
        Me.ServiceId = serviceId
        Me.ServiceName = serviceName
        Me.Quantity = quantity
        Me.Payment = payment
        Me.TotalAmount = totalAmount
        Me.BillNumber = billNumber
        Me.serviempre = serviempre
    End Sub

End Class
Public Class ProductDataReceivedEventArgs
    Inherits EventArgs

    Public Property ServiceId As String
    Public Property ServiceName As String
    Public Property Payment As String
    Public Property TotalAmount As String
    Public Property Serviempre As String
    Public Sub New(serviceName, serviceId, payment, amount, serviempre)
        Me.ServiceId = serviceId
        Me.ServiceName = serviceName
        Me.Payment = payment
        Me.TotalAmount = amount
        Me.Serviempre = serviempre
    End Sub

End Class