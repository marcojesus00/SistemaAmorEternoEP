Imports CrystalDecisions.Shared
Imports System.IO
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Web.Script.Services
Imports System.Net.Mail
Imports System.Net
Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json



Imports System.Threading.Tasks

'Imports Newtonsoft.Json.Linq
'Imports System.Net.Http
'Imports RestSharp
'Imports Sistema.ObjetoWhatsapp





Public Class monitorclientes
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut As String
    Private Datos, Datos1, Datos2, DatosCR, DatosCli As DataSet
    Private Liquida, Liquida2 As String
    Public Tabla As DataTable = New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String = ""

        'Session.Add("Bd", "FUNAMOR")

        If Session("Usuario") = "" Then
            Response.Redirect("inicio.aspx")
        End If


        'Sql = "SELECT SEG_ARCHIVO FROM FUNAMOR..DETSEG WHERE SEG_USUARIO = '" + Usuario_Aut + "'"
        'Datos = conf.EjecutaSql(Sql)

        'Tabla = Datos.Tables(0)

        Usuario = Session("Usuario")
        Clave = Session("Clave")
        Servidor = Session("Servidor")
        Bd = Session("Bd")
        Usuario_Aut = Session("Usuario_Aut")
        Clave_Aut = Session("Clave_Aut")
        If Not IsPostBack Then
            For Each country As KeyValuePair(Of String, String) In CountryPhoneCodes.countryCodes
                ddlCountryCode.Items.Add(New ListItem($"{country.Key} ({country.Value})", country.Value))
            Next
        End If


        'If Not IsPostBack Then
        '    Dim Conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
        '    Dim Sql1 As String = ""


        '    Sql1 = "Select max(FechaSys)Fecha, max(HoraSys)Hora, codigo_clie from Evento where Activo = 1
        '            group by codigo_clie
        '            "
        '    Datos = Conf.EjecutaSql(Sql1)
        '    If Datos.Tables.Count > 1 Then
        '        btnFinalizarEvento.Enabled = True
        '    End If
        'End If
        ' HABILITAR()


    End Sub



    Sub LLENARDATOS()
        Try
            Dim Conf As New Configuracion(Usuario, Clave, "FUNAMOR", Servidor)
            Dim Sql As String = ""
            ' LlenarSalas()
            LlenarSucursal()
            Sql = "SELECT top 1 [idline]
              ,[num_Doc]
              ,[Codigo_clie]
              ,[nombre_clie]
              ,[NombreDifunto]
              ,[idSala]
              ,[Sala]
              ,[DireccionVel]
              ,[DireccionVel2]
              ,[FechaVel]
              ,[HoraVel]
              ,[Fecha]
              ,[Hora]
              ,[FechaSys]
              ,[HoraSys]
              ,[Sepelio]
              ,[FechaSepelio]
              ,[DireccionSep]
              ,[DireccionSep2]
              ,[HoraSepelio]
              ,[Activo]
              ,[Estatus]
              ,Rtrim(Ltrim([Telefono]))Telefono
              ,[Sucursal]
          FROM [Evento] where codigo_clie = '" + Session("CodigoCliente") + "' order by fechasys desc, horasys desc "

            Datos = Conf.EjecutaSql(Sql)

            If Datos.Tables(0).Rows.Count > 0 Then

                If dlsalas.SelectedValue = 19 Then

                    txtSalaDetalle.Visible = True
                    txtSalaDetalle.Text = Datos.Tables(0).Rows(0).Item("Sala")
                Else
                    dlsalas.SelectedValue = Datos.Tables(0).Rows(0).Item("idSala")
                End If


                txtdifunto.Value = Datos.Tables(0).Rows(0).Item("NombreDifunto").ToString
                txtDirecVel.Text = Datos.Tables(0).Rows(0).Item("DireccionVel").ToString
                txtfechaVelacion.Text = Date.Parse(Datos.Tables(0).Rows(0).Item("FechaVel").ToString).ToString("yyyy-MM-dd")
                txtHoraVela.Value = Datos.Tables(0).Rows(0).Item("HoraVel").ToString
                dlsalas.SelectedValue = Datos.Tables(0).Rows(0).Item("idSala").ToString
                'dlsalas.SelectedItem.Value = Datos.Tables(0).Rows(0).Item("Sala").ToString
                txtdireccSep.Text = Datos.Tables(0).Rows(0).Item("DireccionSep").ToString
                txtfechaSep.Text = Date.Parse(Datos.Tables(0).Rows(0).Item("FechaSepelio").ToString).ToString("yyyy-MM-dd")
                txthorasep.Value = Datos.Tables(0).Rows(0).Item("HoraSepelio").ToString
                txtcontacto.Value = Datos.Tables(0).Rows(0).Item("Telefono").ToString
                dlsucursal.SelectedItem.Text = Datos.Tables(0).Rows(0).Item("Sucursal").ToString

                Session.Add("NumeroDocumento", Datos.Tables(0).Rows(0).Item("Num_Doc").ToString)
                btnReimprimir.Visible = True

                Session.Add("DATOSCR", Datos)
                Session.Add("ReporteCR", "RptNotaDuelo")

                ' BtnAgregarND.Text = "Actualizar"
                ' BtnAgregarND.Attributes.CssStyle.Value = "btn fa fa-plus text-success"

            End If
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

        End Try



    End Sub

    Protected Sub BtnReimprimir_click(sender As Object, e As EventArgs) Handles btnReimprimir.Click
        Try
            Dim Conf As New Configuracion(Usuario, Clave, "FUNAMOR", Servidor)
            Dim Sql As String = ""

            Sql = "SELECT top 1 [idline]
              ,[num_Doc]
              ,[Codigo_clie]
              ,[nombre_clie]
              ,[NombreDifunto]
              ,[idSala]
              ,[Sala]
              ,[DireccionVel]
              ,[DireccionVel2]
              ,[FechaVel]
              ,[HoraVel]
              ,[Fecha]
              ,[Hora]
              ,[FechaSys]
              ,[HoraSys]
              ,[Sepelio]
              ,[FechaSepelio]
              ,[DireccionSep]
              ,[DireccionSep2]
              ,[HoraSepelio]
              ,[Activo]
              ,[Estatus]
              ,Rtrim(Ltrim([Telefono]))Telefono
              ,[Sucursal]
          FROM [Evento] where codigo_clie = '" + Session("CodigoCliente") + "' order by fechasys desc, horasys desc "

            Datos = Conf.EjecutaSql(Sql)
            Session.Add("DATOSCR", Datos)
            Session.Add("ReporteCR", "RptNotaDuelo")

            Dim javaScript As String = "window.open('ReportesCR.aspx','_blank','scrollbars=yes,resizable=yes,top=5,left=5,width=700,height=700');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

        End Try


    End Sub

    'Sub HABILITAR()
    '    Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'NotaDue'"
    '    If Tabla.DefaultView.Count > 0 Then
    '        BtnCrearEvent.Enabled = True
    '    Else
    '        BtnCrearEvent.ToolTip = "No Tiene Acceso"
    '        BtnCrearEvent.Style.Value = "color:darkgrey;"
    '    End If
    'End Sub

    Sub LlenarSalas()
        Try
            Dim Conf As New Configuracion(Usuario, Clave, "FUNAMOR", Servidor)
            Dim Sql As String = ""

            Sql = "
				Select convert(int,Codigo_sucu) Codigo,'SALA DE VELACION'+' '+ Nombre_sucu Descripcion from SUCURSAL
				where Codigo_sucu not in (07,08,09,99,11,12,13,' ')
				union all
				 Select '20', 'CASA FAMILIAR' Descripcion
                union all
                Select '19','Otro'
                "
            Datos = Conf.EjecutaSql(Sql)

            dlsalas.DataSource = Datos.Tables(0)
            dlsalas.DataTextField = "Descripcion"
            dlsalas.DataValueField = "Codigo"
            dlsalas.DataBind()

            'dlsalas.SelectedItem.Selected = 1
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

        End Try


    End Sub
    Protected Sub btnSalir_Click(sender As Object, e As ImageClickEventArgs) Handles btnSalir.Click
        Response.Redirect("principal.aspx")
    End Sub


    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Try
            Dim conf As New Configuracion(Usuario, Clave, "FUNAMOR", Servidor)
            Dim Sql As String
            LlenarSalas()

            Sql = "SELECT top 1000 A.Codigo_clie Codigo, A.Nombre_clie Nombre, ISNULL(A.Saldo_actua,0) Saldo,  a.Identidad,A.Dir_cliente Direccion, "
            Sql += "   CASE 
        WHEN NULLIF(A.Telef_clien, '') IS NOT NULL THEN A.Telef_clien 
        ELSE A.CL_CELULAR
    END AS Telefono"
            Sql += " From CLIENTES A "
            Sql += " Where CODIGO_CLIE like '%" + txtcodigo.Text + "%' "
            Sql += " and isnull(identidad,'0') like '%" + txtidentidad.Text + "%' "
            Sql += " and A.NOMBRE_CLIE Like '%" + TxtCliente1.Text + "%' "
            Sql += " and A.NOMBRE_CLIE Like '%" + TxtCliente2.Text + "%' "
            Sql += " and (RTRIM(LTRIM(ISNULL(A.cl_conyunom,''))) Like '%" + txtBenef1.Text + "%' "
            Sql += " and RTRIM(LTRIM(ISNULL(A.cl_conyunom,''))) Like '%" + txtbenef2.Text + "%' "
            Sql += " or RTRIM(LTRIM(ISNULL(a.CL_2conynom,''))) Like '%" + txtBenef1.Text + "%' "
            Sql += " and RTRIM(LTRIM(ISNULL(a.CL_2conynom,''))) Like '%" + txtbenef2.Text + "%' )"
            Sql += " AND A.cl_cobrador LIKE '%" + TxtCobrador.Text + "%'"
            Sql += " ORDER BY A.Codigo_clie , a.cl_fecha desc"
            Datos = conf.EjecutaSql(Sql)

            ''If Datos.Tables(0).Rows.Count > 0 Then
            gvClientes.DataSource = Datos.Tables(0)
            gvClientes.DataBind()
            ''End If
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

        End Try


    End Sub


    Sub ReporteCRS(Informe As CrystalDecisions.CrystalReports.Engine.ReportDocument, Nombre_Archivo As String, DatosCR As DataSet)
        Informe.SetDataSource(DatosCR.Tables(1))
        Informe.Subreports(0).SetDataSource(DatosCR.Tables(0))
        'Informe.Subreports(1).SetDataSource(DatosCR.Tables(0))

        'Dim Location As String = "C:\\inetpub\\wwwroot\\EstadosDeCuenta\\dt140305.105459.pdf"
        'Dim myScript As String = "window.open('" + Location + "', 'PDFPOPUP', 'width=500,height=600');window.print();return false;"
        'Page.ClientScript.RegisterStartupScript(Me.GetType(), "Window", myScript, True)

        Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
        Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions

        exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        exportOpts.ExportFormatOptions = pdfOpts
        Informe.ExportToHttpResponse(exportOpts, Response, False, Nombre_Archivo)
        Informe.Close()
        Informe.Dispose()

    End Sub



    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub

    Private Sub gvClientes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvClientes.RowCommand
        Try
            Dim Conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql1 As String = ""


            If e.CommandName = "Detalle" Then




                lblWhatsAppValidation.Text = ""

                'Movimiento_Clientes(gvClientes.Rows(e.CommandArgument.ToString).Cells(1).Text.ToString.TrimEnd)
                Session.Add("CodigoCliente", gvClientes.Rows(e.CommandArgument.ToString).Cells(1).Text.ToString.TrimEnd)
                Session.Add("NombreCliente", gvClientes.Rows(e.CommandArgument.ToString).Cells(2).Text.ToString.TrimEnd)
                TxtTelefonoWhats.Text = gvClientes.Rows(e.CommandArgument.ToString).Cells(6).Text.ToString.TrimEnd.Replace("-", "")
                'Session.Add("TelefonoCliente", gvClientes.Rows(e.CommandArgument.ToString).Cells(6).Text.ToString.TrimEnd.Replace("-", ""))

                LLENARDATOS()
                LblClienteModal.InnerText = "Cliente: " + " " + gvClientes.Rows(e.CommandArgument.ToString).Cells(1).Text.ToString.TrimEnd + " - " + gvClientes.Rows(e.CommandArgument.ToString).Cells(2).Text.ToString.TrimEnd



                Sql1 = "Select max(FechaSys)Fecha, max(HoraSys)Hora, codigo_clie from FUNAMOR..Evento where Activo = 1 and codigo_clie = '" + Session("CodigoCliente") + "'
                    Group by codigo_clie  "
                DatosCli = Conf1.EjecutaSql(Sql1)
                If DatosCli.Tables(0).Rows.Count > 0 Then
                    btnFinalizarEvento.Enabled = True
                End If



                PanelClienteMovimiento.Visible = True


            End If
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

        End Try


    End Sub


    Protected Sub btnEstadoDeCuenta_clicl(sender As Object, e As EventArgs) Handles BtnEstadoDCuent.Click

        PanelClienteMovimiento.Visible = False
        PanelEnviarWhatsapp.Visible = True

    End Sub

    Protected Sub btnCerrarDivWhatsapp_clicl(sender As Object, e As EventArgs) Handles btnCerrarDivWhatsapp.Click

        PanelClienteMovimiento.Visible = True
        PanelEnviarWhatsapp.Visible = False

    End Sub

    Protected Sub btnCerrarConfCorreo_clicl(sender As Object, e As EventArgs) Handles btnCerrarConfCorreo.Click

        PanelConfirmaCorreoEnviado.Visible = False

    End Sub

    Private Sub btnEnviarPorCorreo_Click(sender As Object, e As EventArgs) Handles btnEnviarPorCorreo.Click
        Dim Informe As New Movimiento_Clientes

        Informe.SetDatabaseLogon(Usuario, Clave)
        Informe.SetParameterValue("Cliente", Session("CodigoCliente").TrimEnd)

        ' Ruta donde se guardará el archivo PDF
        Dim rutaPDF As String = "\\192.168.20.225\Ruta\CL"
        Dim nombreArchivo As String = Session("CodigoCliente").TrimEnd + "-" + DateTime.Now.ToString("yyyy-MM-dd") + "" + ".pdf" ' Cambia el nombre del archivo si lo deseas
        Dim rutaCompletaArchivo As String = Path.Combine(rutaPDF, nombreArchivo)

        Try
            ' Exportar el informe a PDF y guardar en la ruta especificada con el nombre del archivo
            Informe.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaCompletaArchivo)

            ' Mostrar un mensaje o redireccionar después de la descarga exitosa
            ' Response.Redirect("TuPaginaDestino.aspx")
        Catch ex As Exception
            ' Manejar el error, por ejemplo, mostrar un mensaje o registrar el error
            Dim errorMessage As String = ex.Message
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

            ' Puedes mostrar o registrar el mensaje de error aquí
        Finally
            ' Cerrar y liberar recursos
            Informe.Close()
            Informe.Dispose()
        End Try


        ' Dim rutaPDF As String = "C:\inetpub\wwwroot\EstadoDeCuenta\Movimiento_Cliente.pdf"

        ' Dirección de correo del remitente y destinatario
        Dim remitente As String = "notificacion@amoreternohn.com"
        Dim destinatario As String = TxtCorreoCliente.Text.Trim

        ' Configuración de credenciales de correo
        Dim smtpServer As String = "mail.amoreternohn.com"
        Dim smtpPort As Integer = 587
        Dim smtpUsername As String = "notificacion@amoreternohn.com"
        Dim smtpPassword As String = "Eterno.2020$$"

        Try
            ' Crear un nuevo mensaje de correo
            Dim correo As New MailMessage()
            correo.From = New MailAddress(remitente)
            correo.[To].Add(destinatario)

            correo.Subject = "Adjunto Estado de Cuenta"
            correo.Body = "Estimado(a) " + Session("NombreCliente") + "
            Deseando que tenga un excelente día, nos complace adjuntarle su estado de cuenta " + Session("CodigoCliente") + " a la fecha de este correo.
            Para cualquier consulta o sugerencia, contáctenos respondiendo a este mismo correo electrónico."

            ' Adjuntar el archivo PDF
            Dim adjunto As New Attachment(rutaCompletaArchivo)
            correo.Attachments.Add(adjunto)

            ' Configurar el cliente SMTP para enviar el correo
            Dim smtp As New SmtpClient(smtpServer, smtpPort)
            smtp.Credentials = New NetworkCredential(smtpUsername, smtpPassword)
            smtp.EnableSsl = True

            ' Enviar el correo
            smtp.Send(correo)

            ' Liberar recursos
            adjunto.Dispose()
            correo.Dispose()
            PanelEnviarWhatsapp.Visible = False
            TxtCorreoCliente.Text = ""


            lblAlert.CssClass = "alert-primary align-content-center"
            lblAlert.Text = "Correo enviado exitosamente"
            PanelConfirmaCorreoEnviado.Visible = True


        Catch ex As Exception
            ' Manejar el error, por ejemplo, mostrar un mensaje o registrar el error
            Dim errorMessage As String = ex.Message
            ' Puedes mostrar o registrar el mensaje de error aquí
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

        End Try

        ' PanelEnviarWhatsapp.Visible = False



    End Sub

    Public Class ResponseObject
        Public Property Success As Boolean
        Public Property Data As Object
        Public Property Errors As List(Of ErrorObject)
    End Class

    Public Class ErrorObject
        Public Property Type As String
        Public Property Loc As Object
        Public Property Msg As String
        Public Property Code As String
        Public Property StatusCode As Integer
    End Class
    Private Function PostData(ByVal url As String, ByVal data As Dictionary(Of String, Object)) As String
        Using client As New HttpClient()
            client.DefaultRequestHeaders.Accept.Add(New System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"))

            ' Convert the dictionary to JSON
            Dim json As String = JsonConvert.SerializeObject(data)
            Dim content As New StringContent(json, Encoding.UTF8, "application/json")

            Try
                Dim response1 As HttpResponseMessage = client.PostAsync(url, content).Result
                Dim contentStr As StreamContent = response1.Content
                Dim contentString As String = contentStr.ReadAsStringAsync().Result
                Dim serializer As New Newtonsoft.Json.JsonSerializer
                Dim jsonReader As New Newtonsoft.Json.JsonTextReader(New StringReader(contentString))
                Dim jsonResponse As ResponseObject = serializer.Deserialize(Of ResponseObject)(jsonReader)


                If response1.IsSuccessStatusCode Then
                    lblAlert.CssClass = "alert-primary align-content-center"
                    lblAlert.Text = "WhatsApp enviado con éxito"
                    Return response1.Content.ReadAsStringAsync().Result
                Else
                    lblAlert.CssClass = "alert-danger align-content-center"
                    Dim msg = ""
                    If jsonResponse.Errors(0) IsNot Nothing Then
                        msg = jsonResponse.Errors(0).Msg
                    Else
                        msg = $"{response1.StatusCode} {response1.ReasonPhrase}"
                    End If
                    lblAlert.Text = $"Error, intente de nuevo: {msg}"
                    Throw New Exception(msg)

                    Return $"Error: {response1.StatusCode} - {response1.ReasonPhrase}"

                End If

            Catch ex As Exception
                lblAlert.CssClass = "alert-danger align-content-center"
                lblAlert.Text = "Error, intente de nuevo"
                DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

                Return $"Exception: {ex.Message}"
            End Try
        End Using
    End Function

    Private Sub btnEnviarWhatsapp_click(sender As Object, e As EventArgs) Handles btnEnviarWhatsapp.Click
        If TxtTelefonoWhats.Text.Trim.Length < 7 Then
            lblWhatsAppValidation.Text = "Ingrese por lo menos 8 digitos"
            lblWhatsAppValidation.CssClass = "alert-danger align-content-center"

            Exit Sub
        End If
        lblWhatsAppValidation.Text = ""
        Dim Informe As New Movimiento_Clientes

        Informe.SetDatabaseLogon(Usuario, Clave)
        Informe.SetParameterValue("Cliente", Session("CodigoCliente").TrimEnd)

        Dim nombreArchivo As String = Session("CodigoCliente").TrimEnd + "-" + DateTime.Now.ToString("yyyy-MM-dd") + "" + ".pdf" ' Cambia el nombre del archivo si lo deseas

        Try
            ' Exportar el informe a PDF y guardar en la ruta especificada con el nombre del archivo
            'Informe.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaCompletaArchivo)
            Using memoryStream As New System.IO.MemoryStream()
                ' Export the report to the memory stream as a PDF
                Informe.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat).CopyTo(memoryStream)

                ' Convert the memory stream to a byte array
                Dim reportBytes As Byte() = memoryStream.ToArray()

                ' Convert the byte array to a base64 string
                Dim base64String As String = Convert.ToBase64String(reportBytes)

                Try


                    Dim caption = "Estimado(a) " + Session("NombreCliente") + " Amor Eterno envía su estado de cuenta."

                    'Dim url As String = "http://localhost:8002/v1/messages/docs/"
                    'url = "https://whatsapi-vlvp.onrender.com/v1/messages/docs/"
                    Dim url = "http://192.168.20.75:8000/v1/messages/docs/"
                    Dim phoneNumber As New Dictionary(Of String, String) From {
                    {"country_code", ddlCountryCode.SelectedValue.Replace("+", "")},
                    {"local_number", TxtTelefonoWhats.Text}
                }

                    Dim data As New Dictionary(Of String, Object) From {
                    {"phone_number", phoneNumber},
                    {"url", base64String},
                    {"file_name", nombreArchivo},
                    {"caption", caption},
                    {"priority", 10},
                    {"referenceID", ""}
                }

                    ' Send the POST request
                    Dim rapiRsponse = PostData(url, data)
                    Response.Write(rapiRsponse)


                    PanelEnviarWhatsapp.Visible = False
                    TxtTelefonoWhats.Text = ""


                    PanelConfirmaCorreoEnviado.Visible = True


                Catch ex As Exception
                    Dim errorMessage As String = ex.Message
                    DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

                End Try
            End Using
        Catch ex As Exception
            Dim errorMessage As String = ex.Message
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

        Finally
            Informe.Close()
            Informe.Dispose()
        End Try





    End Sub
    ' End Class

    'Protected Sub btnEnviarWhatsapp_Click(sender As Object, e As EventArgs) Handles btnEnviarWhatsapp.Click
    '    EnviarJsonAWhatsapp()
    'End Sub

    'Protected Sub BtnEstadoDCuent_Click(sender As Object, e As EventArgs) Handles BtnEstadoDCuent.Click
    '    Dim Informe As New Movimiento_Clientes
    '    Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
    '    Dim Sql As String

    '    Sql = " Exec FUNAMOR..SP_CR_VS_MovimientoClineteSubRPT '" + Session("CodigoCliente").TrimEnd + "' EXEC FUNAMOR..CR_ESTADO_DE_CUENTA  '" + Session("CodigoCliente").TrimEnd + "'"
    '    DatosCR = conf.EjecutaSql(Sql)

    '    Dim javaScript As String = "window.open('monitorclientes.aspx');"
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)

    '    Informe = New Movimiento_Clientes
    '    ReporteCRS(Informe, "Movimiento_Cliente", DatosCR)

    '    'Dim Location As String = "C:\\C:\\inetpub\\wwwroot\\EstadosDeCuenta\\dt140305.105459.pdf"
    '    'Dim myScript As String = "window.open('" + Location + "', 'PDFPOPUP', 'width=500,height=600');window.print();return false;"
    '    'Page.ClientScript.RegisterStartupScript(Me.GetType(), "Window", myScript, True)
    'End Sub


    Protected Sub BtnBitacora_Click(sender As Object, e As EventArgs) Handles BtnBitacora.Click

        PanelClienteMovimiento.Visible = False

    End Sub

    Protected Sub dlsalas_TextChanged(sender As Object, e As EventArgs)
        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        If dlsalas.SelectedValue = "19" Then
            txtSalaDetalle.Visible = True
        Else
            txtSalaDetalle.Visible = False
        End If

    End Sub


    Protected Sub btnCerrarVentana_Clikc(sender As Object, e As EventArgs) Handles btnCerrarVentana.Click
        PanelClienteMovimiento.Visible = False
        PanelCrearEvento.Visible = False

        Session("CodigoCliente") = ""
        Session("NombreCliente") = ""
        txtdifunto.Value = ""
        txtDirecVel.Text = ""
        txtfechaVelacion.Text = ""
        txtHoraVela.Value = ""
        txtdireccSep.Text = ""
        txtfechaSep.Text = ""
        txthorasep.Value = ""
        txtcontacto.Value = ""
        'dlsucursal.SelectedItem.Value = "1"
    End Sub


    Protected Sub btnCerrarEvento_Clikc(sender As Object, e As EventArgs) Handles btnCerrarEvento.Click
        PanelCrearEvento.Visible = False
        btnCerrarEvento.Visible = False
        PanelClienteMovimiento.Visible = True
        div3Sep.Visible = False


    End Sub

    Protected Sub BtnCrearEvent_Clikc(sender As Object, e As EventArgs) Handles BtnCrearEvent.Click

        PanelClienteMovimiento.Visible = False
        PanelCrearEvento.Visible = True
        btnCerrarEvento.Visible = True
        div3Sep.Visible = True

    End Sub


    Protected Sub BtnHistorial_click(sender As Object, e As EventArgs) Handles BtnHistorial.Click
        Dim conf As New Configuracion(Usuario, Clave, "AECOBROS", Servidor)
        Dim sql As String

        sql = " EXEC AECOBROS..SP_CR_VS_MOV_CLIENTESVISITAS '" + Session("CodigoCliente").TrimEnd + "'"
        Datos = conf.EjecutaSql(sql)

        Session.Add("DATOSCR", Datos)
        Session.Add("ReporteCR", "RptMovClienteM")

        Dim javaScript As String = "window.open('reportesCR.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)

    End Sub

    'Protected Sub btnCerrarVentana_Clikc(sender As Object, e As EventArgs) Handles btnCerrarVentana.Click
    '    PanelClienteMovimiento.Visible = False
    'End Sub

    'Protected Sub BtnCrearEvent_Clikc(sender As Object, e As EventArgs) Handles BtnCrearEvent.Click
    '    PanelCrearEvento.Visible = True
    'End Sub



    Protected Sub BtnBitacora_click() Handles BtnBitacora.Click

        Session.Add("Destino", "ClientesM1.aspx")
        Response.Redirect("ClientesM1.aspx")

    End Sub


    Sub LlenarSucursal()
        Dim Conf As New Configuracion(Usuario, Clave, "FUNAMOR", Servidor)
        Dim Sql As String = ""

        Sql = "Select Codigo_sucu Codigo,Nombre_sucu Sucursal from SUCURSAL
    where Codigo_sucu not in (07,08,09,99,11,12,13,' ')"
        Datos = Conf.EjecutaSql(Sql)

        dlsucursal.DataSource = Datos.Tables(0)
        dlsucursal.DataValueField = "Codigo"
        dlsucursal.DataTextField = "Sucursal"
        dlsucursal.DataBind()

    End Sub

    Protected Sub BtnGuardarNo_Clikc(sender As Object, e As EventArgs) Handles BtnGuardarNo.Click
        PanelClienteMovimiento.Visible = False
        PanelCrearEvento.Visible = True
        ConfirmarNotaDuelo.Visible = False
    End Sub

    Protected Sub BtnAgregarND_Clikc(sender As Object, e As EventArgs) Handles BtnAgregarND.Click
        If Decimal.Parse(txtdifunto.Value.Length) = 0 Then
            lblMsg.Text = "Error: Debe Ingresar el Nombre del Difunto"
            lblMsg.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If

        If Decimal.Parse(txtfechaSep.Text.Length) = 0 Then
            lblMsg.Text = "Error: Debe ingresar la Fecha del Sepelio"
            lblMsg.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If

        If Decimal.Parse(txtfechaVelacion.Text.Length) = 0 Then
            lblMsg.Text = "Error: Debe Ingresar la Fecha de Velacion"
            lblMsg.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If


        'If Decimal.Parse(txthorasep.Value.Length) = 0 Then
        '    lblMsg.Text = "Error: Debe Ingresar la hora de Sepelio"
        '    lblMsg.ControlStyle.CssClass = "alert alert-danger"
        '    Exit Sub
        'End If


        'If Decimal.Parse(txtHoraVela.Value.Length) = 0 Then
        '    lblMsg.Text = "Error: Debe Ingresar la Hora de Velacion"
        '    lblMsg.ControlStyle.CssClass = "alert alert-danger"
        '    Exit Sub
        'End If



        If Decimal.Parse(txtcontacto.Value.TrimEnd.TrimStart.Length) < 8 Then
            lblMsg.Text = "Error: Telefono Debe contener 8 minimo digitos"
            lblMsg.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If

        If dlsalas.SelectedValue = "19" And txtSalaDetalle.Text.Length < 2 Then
            lblMsg.Text = "Error: Debe Especificar la Sala"
            lblMsg.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If

        PanelClienteMovimiento.Visible = False
        PanelCrearEvento.Visible = False
        ConfirmarNotaDuelo.Visible = True
    End Sub

    Protected Sub BtnGuardarSi_Clikc(sender As Object, e As EventArgs) Handles BtnGuardarSi.Click
        Dim conf, Conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
        '  Dim informeND As New RptNotaDuelo
        Dim Sql, Sql2 As String
        Dim TelefonoContacto As String
        Dim SalaVelacion As String
        TelefonoContacto = ""
        'Sql = ""

        If txtcontacto.Value.TrimEnd.TrimStart.Length > 8 Then
            TelefonoContacto = txtcontacto.Value
        Else
            If txtcontacto.Value.TrimEnd.TrimStart.Length <= 8 Then
                TelefonoContacto = "+504 " + txtcontacto.Value.Trim
            End If
        End If
        'Datos = conf.EjecutaSql(Sql)

        If dlsalas.SelectedValue = "19" Then
            SalaVelacion = txtSalaDetalle.Text.TrimEnd.ToUpper
        Else
            SalaVelacion = dlsalas.SelectedItem.Text.TrimEnd()
        End If


        PanelClienteMovimiento.Visible = False
        PanelCrearEvento.Visible = False
        ConfirmarNotaDuelo.Visible = False



        Sql = " Exec FUNAMOR..SP_VS_CrearNotaDuelo_new                
                '" + Session("CodigoCliente").TrimEnd + "'
                ,'" + Session("NombreCliente").TrimEnd + "'
                ,'" + txtdifunto.Value.TrimStart.TrimEnd + "'
                ,'" + dlsalas.SelectedValue.TrimEnd + "'
                ,'" + SalaVelacion + "'
                ,'" + txtDirecVel.Text.TrimEnd.TrimStart + "'
                ,'" + txtfechaVelacion.Text + "'
                ,'" + txtHoraVela.Value.TrimStart + "'
                ,'" + txtdireccSep.Text.TrimStart.TrimEnd + "' 
                ,'" + txtfechaSep.Text + "'
                ,'" + txthorasep.Value + "'
                ,'" + TelefonoContacto.TrimEnd.TrimStart + "'         
                ,'" + dlsucursal.SelectedItem.Text + "'                  
                ,'1','1'
                ,'" + Usuario_Aut + "'"

        Datos = conf.EjecutaSql(Sql)

        lblMsg.ControlStyle.CssClass = "alert alert-success"

        If Integer.Parse(Datos.Tables(0).Rows(0).Item("Error")) = 0 Then
            Session.Add("NumDoc1", Datos.Tables(0).Rows(0).Item("NumDoc").ToString)
            lblMsg.Text = Datos.Tables(0).Rows(0).Item("MSG").ToString
            lblMsg.ControlStyle.CssClass = "alert alert-success"

            Session("CodigoCliente") = ""
            Session("NombreCliente") = ""
            txtdifunto.Value = ""
            txtDirecVel.Text = ""
            txtfechaVelacion.Text = ""
            txtHoraVela.Value = ""
            txtdireccSep.Text = ""
            txtfechaSep.Text = ""
            txthorasep.Value = ""
            txtcontacto.Value = ""
            'dlsucursal.SelectedItem.Value = "1"



            Sql2 = "SELECT [idline]
              ,[num_Doc]
              ,[Codigo_clie]
              ,[nombre_clie]
              ,[NombreDifunto]
              ,[idSala]
              ,[Sala]
              ,[DireccionVel]
              ,[DireccionVel2]
              ,[FechaVel]
              ,[HoraVel]
              ,[Fecha]
              ,[Hora]
              ,[FechaSys]
              ,[HoraSys]
              ,[Sepelio]
              ,[FechaSepelio]
              ,[DireccionSep]
              ,[DireccionSep2]
              ,[HoraSepelio]
              ,[Activo]
              ,[Estatus]
              ,[Telefono]
              ,[Sucursal]
          FROM [FUNAMOR].[dbo].[Evento] where num_doc = '" + Session("NumDoc1") + "' "
            Datos1 = Conf1.EjecutaSql(Sql2)
            Session.Add("DATOSCR", Datos1)
            Session.Add("ReporteCR", "RptNotaDuelo")


            Dim javaScript As String = "window.open('ReportesCR.aspx','_blank','scrollbars=yes,resizable=yes,top=5,left=5,width=700,height=700');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)

            'informeND = New RptNotaDuelo
            'ReporteCrystal(informeND, "RptNotaDuelo_", Datos1)
            ConfirmarNotaDuelo.Visible = False
            btnReimprimir.Visible = False
        Else
            lblMsg.Text = "Error: " + Datos.Tables(0).Rows(0).Item("Error").ToString + Datos.Tables(0).Rows(0).Item("MSG").ToString
            lblMsg.ControlStyle.CssClass = "alert alert-danger"

        End If



    End Sub


    ''Ventana para finalizar evento
    Private Sub btnFinalizarEvento_Click(sender As Object, e As EventArgs) Handles btnFinalizarEvento.Click

        DivFinalizarEvento.Visible = True

    End Sub

    Private Sub btnNoFinalizar_Click(sender As Object, e As EventArgs) Handles btnNoFinalizar.Click
        DivFinalizarEvento.Visible = False
    End Sub

    Private Sub btnsiFinalizarEvento_Click(sender As Object, e As EventArgs) Handles btnSiFinalizarEvento.Click
        DivFinalizarEvento.Visible = False

        Dim conf, Conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
        '  Dim informeND As New RptNotaDuelo
        Dim Sql As String

        Sql = "Exec SP_VS_AnularClientePantallaServicios '" + Session("CodigoCliente") + "','" + Usuario_Aut.TrimEnd + "'"
        Datos = conf.EjecutaSql(Sql)

        PanelClienteMovimiento.Visible = False

    End Sub


End Class