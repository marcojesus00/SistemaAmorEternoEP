














Public Class CorrectContract
    Inherits System.Web.UI.UserControl
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut As String
    Public Event AlertGenerated As EventHandler(Of AlertEventArgs)
    Dim msg = "Error inesperado: "

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
                'DeptoCliente()


            Catch ex As Exception
                msg = "Error al leer de la base de datos, por favor recargue la página : "
                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg & ex.Message, "danger"))

            End Try
        End If


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
        'Datos = conf.EjecutaSql(Sql)


        'gvDetalleProductosContrato.DataSource = Datos.Tables(0)
        'gvDetalleProductosContrato.DataBind()

        'btnGuardarCamb.Enabled = True

    End Sub
    Protected Sub txtvalorcontApp_TextChanged(sender As Object, e As EventArgs)
        Dim Letra, Cuota As Integer
        Dim initialPayment
        Try
            If txtcuotaApp.Text.Length > 0 And txtvalorcontApp.Text.Length > 0 And txtLetraApp.Text.Length > 0 And initialPayment.Length > 0 Then

                If initialPayment = txtvalorcontApp.Text Then
                    Cuota = 0
                    Letra = 0
                Else
                    If txtvalorcontApp.Text > 0 And initialPayment > 0 And txtcuotaApp.Text > 0 Then
                        Letra = (txtvalorcontApp.Text - initialPayment) / txtcuotaApp.Text
                        Cuota = txtcuotaApp.Text
                    End If

                End If
                txtLetraApp.Text = Letra
                txtcuotaApp.Text = Cuota


                'btnGuardarCamb.Enabled = True

            End If

        Catch ex As Exception


        End Try


    End Sub
    'Public Event DataClientReceived As EventHandler(Of ClientDataReceivedEventArgs)
    '    Public Sub OnDataReceived(ByVal sender As Object, ByVal e As ClientDataReceivedEventArgs)
    '        Try

    '            Using dbcontext As New AeVentasDbContext


    '                _clientData = dbcontext.DatosDeClientes.Where(Function(c) c.CodigoVendedor.Contains(e.SalesPersonId) And c.Identidad.Contains(e.IdentificationDocument)).FirstOrDefault()
    '                Dim deptoCiudadQuery = dbcontext.MunicipiosZonasDepartamentos _
    '                                      .Select(Function(d) New With {d.MunicipioId, d.NombreMunicipio}).Where(Function(c) c.NombreMunicipio.Contains(_clientData.Municipio)).FirstOrDefault()
    '                Dim department = dbcontext.MunicipiosZonasDepartamentos _
    '                .Select(Function(d) New With {d.NombreDepartamento, d.DepartamentoId}).Where(Function(d) d.NombreDepartamento.Contains(_clientData.Departamento)) _
    '                .FirstOrDefault()

    '                txtidentiCliapp.Text = _clientData.Identidad
    '                TextBoxCelular.Text = _clientData.Celular
    '                TextBoxPhone.Text = _clientData.Telefono
    '                TxtPrimaApp.Text = e.InitialPayment
    '                txtdir1Cliapp.Text = _clientData.Direccion
    '                TextBoxAddress2.Text = _clientData.Dir2_client
    '                TextBoxAddress3.Text = _clientData.Dir3_client
    '                dlDeptoCliente.DataTextField = _clientData.Departamento
    '                dlDeptoCliente.SelectedValue = department.DepartamentoId

    '                dlCiudadCliente.DataTextField = _clientData.Municipio
    '                dlCiudadCliente.SelectedValue = deptoCiudadQuery.MunicipioId
    '                txtdir1Cliapp.Text = _clientData.Direccion

    '            End Using
    '        Catch ex As Exception
    '            RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error" & ex.Message, "danger"))

    '        End Try


    '    End Sub
    '    Protected Sub dlCiudadCliente_TextChanged(sender As Object, e As EventArgs)
    '        Try
    '            'Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
    '            'Dim Sql As String

    '            'Sql = "Select desdepto Depto, ltrim(rtrim(desmuni))  + '-' +codzona CiudadEmpresa, codmuni, coddepto from AEVentas..DEPTOZONA"


    '        Catch ex As Exception
    '            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg & ex.Message, "danger"))

    '        End Try


    '    End Sub
    '    Protected Sub dlDeptoCliente_TextChanged(sender As Object, e As EventArgs)
    '        Try

    '            Dim SelectedDeptoCode As String = dlDeptoCliente.SelectedValue
    '            Using dbcontext As New AeVentasDbContext
    '                Dim deptoCiudadByDepartment = dbcontext.MunicipiosZonasDepartamentos _
    '                  .Select(Function(d) New With {d.NombreDepartamento, .CiudadEmpresa = d.NombreMunicipio.Trim() & "-" & d.ZonaId, d.MunicipioId, d.DepartamentoId}) _
    '                  .Where(Function(c) c.DepartamentoId = SelectedDeptoCode) _
    '                .ToList()
    '                dlCiudadCliente.DataSource = deptoCiudadByDepartment
    '                dlCiudadCliente.DataTextField = "CiudadEmpresa"
    '                dlCiudadCliente.DataValueField = "MunicipioId"
    '                dlCiudadCliente.DataBind()
    '            End Using
    '        Catch ex As Exception
    '            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg & ex.Message, "danger"))

    '        End Try

    '    End Sub
    '    Sub DeptoCliente()
    '        Try

    '            Using dbcontext As New AeVentasDbContext
    '                Dim departments = dbcontext.MunicipiosZonasDepartamentos _
    '                .Select(Function(d) New With {d.NombreDepartamento, d.DepartamentoId}) _
    '                                      .Distinct() _
    '                                      .ToList()
    '                Dim deptoCiudadQuery = dbcontext.MunicipiosZonasDepartamentos _
    '                                  .Select(Function(d) New With {d.NombreDepartamento, .CiudadEmpresa = d.NombreMunicipio.Trim() & "-" & d.ZonaId, d.MunicipioId, d.DepartamentoId}) _
    '                                  .ToList()
    '                dlDeptoCliente.DataSource = departments
    '                dlDeptoCliente.DataTextField = "NombreDepartamento"
    '                dlDeptoCliente.DataValueField = "DepartamentoId"
    '                dlDeptoCliente.DataBind()


    '                dlCiudadCliente.DataSource = deptoCiudadQuery
    '                dlCiudadCliente.DataTextField = "CiudadEmpresa"
    '                dlCiudadCliente.DataValueField = "MunicipioId"
    '                dlCiudadCliente.DataBind()
    '            End Using
    '        Catch ex As SqlException

    '            RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error de base de datos: " & ex.Message, "danger"))

    '        Catch ex As Exception
    '            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg & ex.Message, "danger"))

    '        End Try


    '    End Sub
    '    Public Sub SaveChanges()
    '        Try

    '            Dim newDataClient As DatosDeCliente = _clientData
    '            newDataClient.Identidad = txtidentiCliapp.Text.Replace("-", String.Empty).Trim
    '            newDataClient.Celular = TextBoxCelular.Text.Replace("-", String.Empty).Trim
    '            newDataClient.Telefono = TextBoxPhone.Text.Replace("-", String.Empty).Trim
    '            newDataClient.Departamento = dlDeptoCliente.DataTextField.Trim
    '            newDataClient.Municipio = dlCiudadCliente.DataTextField.Trim
    '            newDataClient.Direccion = txtdir1Cliapp.Text.Trim
    '            newDataClient.Dir2_client = TextBoxAddress2.Text.Trim
    '            newDataClient.Dir3_client = TextBoxAddress3.Text.Trim

    '            Using Context As New AeVentasDbContext()
    '                Context.ExecuteVIENECLIENTESN_A3(
    '        Codigo_clie:=newDataClient.Codigo,
    '        Nombre_clie:=newDataClient.Nombre,
    '        identidad:=newDataClient.Identidad,
    '        tributario:=newDataClient.tributario,
    '        CL_STATUS:=newDataClient.CL_STATUS,
    '        CL_COMPANIA:=newDataClient.CL_COMPANIA,
    '        Dir_cliente:=newDataClient.Direccion,
    '        Dir2_client:=newDataClient.Dir2_client,
    '        Dir3_client:=newDataClient.Dir3_client,
    '        Dir4_client:=newDataClient.Dir4_client,
    '        Dir5_client:=newDataClient.Dir5_client,
    '        Telef_clien:=newDataClient.Telefono,
    '        CL_CELULAR:=newDataClient.Celular,
    '        CL_EMAIL:=newDataClient.Email,
    '        cl_conyunom:=newDataClient.NombreDelConyuge,
    '        cl_conyutel:=newDataClient.TelefonoDelConyuge,
    '        cl_conyudir:=newDataClient.DireccionDelConyuge,
    '        CL_PARENt:=newDataClient.CL_PARENt,
    '        Cod_zona:=newDataClient.Cod_zona,
    '        CL_VENDEDOR:=newDataClient.CodigoVendedor,
    '        cl_usuario:=newDataClient.Cl_usuario,
    '        cl_terminal:=newDataClient.Cl_terminal,
    '        longitud:=newDataClient.Longitud,
    '        latitud:=newDataClient.Latitud,
    '        secuencia:=newDataClient.Secuencia,
    '        diavisita:=newDataClient.Diavisita,
    '        semana:=newDataClient.Semana,
    '        cierre:=newDataClient.Cierre,
    '        tempo:=newDataClient.Tempo,
    '        departa:=newDataClient.Departamento,
    '        municipio:=newDataClient.Municipio,
    '        circuito:=newDataClient.circuito,
    '        liquida:=newDataClient.Liquida)

    '                Dim newLogClientesNEdition As New LogEdicionCLIENTESN() With {
    '    .TiempoDeEdicion = DateTime.Now,
    '    .EditadoPor = Session("Usuario"),
    '    .Codigo_clie = _clientData.Codigo,
    '    .CL_VENDEDOR = _clientData.CodigoVendedor,
    '    .Anterior_identidad = _clientData.Identidad,
    '    .Nuevo_identidad = newDataClient.Identidad,
    '    .Anterior_Dir_cliente = _clientData.Direccion,
    '    .Nuevo_Dir_cliente = newDataClient.Direccion,
    '    .Anterior_Dir2_client = _clientData.Dir2_client,
    '    .Nuevo_Dir2_client = newDataClient.Dir2_client,
    '    .Anterior_Dir3_client = _clientData.Dir3_client,
    '    .Nuevo_Dir3_client = newDataClient.Dir3_client,
    '    .Anterior_Dir4_client = _clientData.Dir4_client,
    '    .Nuevo_Dir4_client = newDataClient.Dir4_client,
    '    .Anterior_Dir5_client = _clientData.Dir5_client,
    '    .Nuevo_Dir5_client = newDataClient.Dir5_client,
    '    .Anterior_Telef_clien = _clientData.Telefono,
    '    .Nuevo_Telef_clien = newDataClient.Telefono,
    '    .Anterior_CL_CELULAR = _clientData.Celular,
    '    .Nuevo_CL_CELULAR = newDataClient.Celular,
    '    .Anterior_CL_EMAIL = _clientData.Email,
    '    .Nuevo_CL_EMAIL = newDataClient.Email,
    '    .Anterior_cl_conyunom = _clientData.NombreDelConyuge,
    '    .Nuevo_cl_conyunom = newDataClient.NombreDelConyuge,
    '    .Anterior_cl_conyutel = _clientData.TelefonoDelConyuge,
    '    .Nuevo_cl_conyutel = newDataClient.TelefonoDelConyuge,
    '    .Anterior_cl_conyudir = _clientData.DireccionDelConyuge,
    '    .Nuevo_cl_conyudir = newDataClient.DireccionDelConyuge,
    '    .Anterior_departa = _clientData.Departamento,
    '    .Nuevo_departa = newDataClient.Departamento,
    '    .Anterior_municipio = _clientData.Municipio,
    '    .Nuevo_municipio = newDataClient.Municipio
    '}

    '                Context.LogsEdicionesCLIENTESN.Add(newLogClientesNEdition)
    '                Context.SaveChanges()
    '            End Using

    '        Catch ex As Exception
    '            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg & ex.Message, "danger"))

    '        End Try
    '    End Sub

    '    'Sub DeptoCiudadCliente()
    '    '    Try
    '    '        'Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
    '    '        'Dim Sql As String

    '    '        'Sql = "Select desdepto Depto, ltrim(rtrim(desmuni))  + '-' +codzona CiudadEmpresa, codmuni, coddepto from AEVentas..DEPTOZONA"

    '    '        'Dim Datos = conf.EjecutaSql(Sql)
    '    '        'dlCiudadCliente.DataSource = Datos.Tables(0)
    '    '        'dlCiudadCliente.DataTextField = "CiudadEmpresa"
    '    '        'dlCiudadCliente.DataValueField = "Codmuni"
    '    '        'dlCiudadCliente.DataBind()

    '    '    Catch ex As Exception
    '    '        RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg & ex.Message, "danger"))

    '    '    End Try

    '    'End Sub

    'End Class
    'Public Class ClientDataReceivedEventArgs
    '    Inherits EventArgs

    '    Public Property ClientId As String
    '    Public Property SalesPersonId As String
    '    Public Property IdentificationDocument As String
    '    Public Property PhoneNumberOne As String
    '    Public Property PhoneNumberTwo As String
    '    Public Property Department As String
    '    Public Property City As String
    '    Public Property Address As String
    '    Public Property InitialPayment As String

    '    Public Sub New(ClientId As String, salesPersonId As String, identification As String, phone1 As String, phone2 As String, initialPayment As String)
    '        Me.IdentificationDocument = identification
    '        Me.PhoneNumberOne = phone1
    '        Me.PhoneNumberTwo = phone2
    '        Me.InitialPayment = initialPayment
    '        Me.ClientId = ClientId
    '        Me.SalesPersonId = salesPersonId
    '    End Sub

End Class