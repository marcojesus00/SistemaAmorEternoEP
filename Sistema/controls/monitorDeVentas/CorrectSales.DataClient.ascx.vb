Public Class DataClient
    Inherits System.Web.UI.UserControl
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut As String

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
                DeptoCliente()


            Catch ex As Exception
                Dim msg = "Error al leer de la base de datos, por favor recargue la página : " & ex.Message
            End Try
        End If


    End Sub
    'Public Event DataClientReceived As EventHandler(Of ClientDataReceivedEventArgs)
    Public Sub OnDataReceived(ByVal sender As Object, ByVal e As ClientDataReceivedEventArgs)

        txtidentiCliapp.Text = e.IdentificationDocument
        txttel1app.Text = e.PhoneNumberOne
        txttel2app.Text = e.PhoneNumberTwo
        TxtPrimaApp.Text = e.InitialPayment
        'dlDeptoCliente
        'dlCiudadCliente
        'txtdir1Cliapp
    End Sub
    Protected Sub dlCiudadCliente_TextChanged(sender As Object, e As EventArgs)

        Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "Select desdepto Depto, ltrim(rtrim(desmuni))  + '-' +codzona CiudadEmpresa, codmuni, coddepto from AEVentas..DEPTOZONA"



    End Sub
    Protected Sub dlDeptoCliente_TextChanged(sender As Object, e As EventArgs)
        Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = $"Select desdepto Depto, ltrim(rtrim(desmuni))  + '-' +codzona CiudadEmpresa, codmuni, coddepto from AEVentas..DEPTOZONA where coddepto = ltrim(rtrim('{dlDeptoCliente.SelectedValue} ')) "
        Dim Datos = conf.EjecutaSql(Sql)

        dlCiudadCliente.SelectedValue = Datos.Tables(0).Rows(0).Item("CodMuni")
        dlCiudadCliente.DataSource = Datos.Tables(0)
        dlCiudadCliente.DataTextField = "CiudadEmpresa"
        dlCiudadCliente.DataValueField = "Codmuni"
        dlCiudadCliente.DataBind()

    End Sub
    Sub DeptoCliente()
        Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "Select distinct desdepto Depto, coddepto from AEVentas..DEPTOZONA Select desdepto Depto, ltrim(rtrim(desmuni))  + '-' +codzona CiudadEmpresa, codmuni, coddepto from AEVentas..DEPTOZONA"

        Dim Datos = conf.EjecutaSql(Sql)
        dlDeptoCliente.DataSource = Datos.Tables(0)
        dlDeptoCliente.DataTextField = "Depto"
        dlDeptoCliente.DataValueField = "CodDepto"
        dlDeptoCliente.DataBind()


        dlCiudadCliente.DataSource = Datos.Tables(1)
        dlCiudadCliente.DataTextField = "CiudadEmpresa"
        dlCiudadCliente.DataValueField = "Codmuni"
        dlCiudadCliente.DataBind()

    End Sub

    Sub DeptoCiudadCliente()
        Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "Select desdepto Depto, ltrim(rtrim(desmuni))  + '-' +codzona CiudadEmpresa, codmuni, coddepto from AEVentas..DEPTOZONA"

        Dim Datos = conf.EjecutaSql(Sql)
        dlCiudadCliente.DataSource = Datos.Tables(0)
        dlCiudadCliente.DataTextField = "CiudadEmpresa"
        dlCiudadCliente.DataValueField = "Codmuni"
        dlCiudadCliente.DataBind()

    End Sub

End Class
Public Class ClientDataReceivedEventArgs
    Inherits EventArgs

    Public Property IdentificationDocument As String
    Public Property PhoneNumberOne As String
    Public Property PhoneNumberTwo As String
    Public Property Department As String
    Public Property City As String
    Public Property Address As String
    Public Property InitialPayment As String
    Public Sub New(identification As String, phone1 As String, phone2 As String, initialPayment As String)
        Me.IdentificationDocument = identification
        Me.PhoneNumberOne = phone1
        Me.PhoneNumberTwo = phone2
        Me.InitialPayment = initialPayment
        'Me.Department = depto
        'Me.City = city
        'Me.Address = address
    End Sub
End Class