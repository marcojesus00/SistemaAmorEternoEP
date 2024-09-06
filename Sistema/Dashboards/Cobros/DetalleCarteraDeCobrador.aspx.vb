Imports System.Data.SqlClient
Imports Sistema.CobrosDashboard

Public Class DetalleCarteraDeCobrador
    Inherits System.Web.UI.Page
    Public PageNumber As Integer = 1
    Public PageSize As Integer = 10
    Public TotalPages As Integer '
    Public TotalItems As Integer = 0
    Public itemText As String
    Public pagination As PaginationHelper = New PaginationHelper
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim Usuario_Aut = Session("Usuario_Aut")
            Session("BackPageUrl") = "~/Dashboards/Cobros/Cobros.aspx"
            Dim thisPage = "~/Dashboards/Cobros/DetalleCarteraDeCobrador.aspx"
            If Usuario_Aut IsNot Nothing Then
                Usuario_Aut = Usuario_Aut.ToString().Trim().ToUpper()
                If Session("Usuario") = "" OrElse Not AuthHelper.isAuthorized(Usuario_Aut, "COBROS_A") Then
                    Response.Redirect("~/Principal.aspx")
                End If

                If Not IsPostBack Then
                    'FillDll()
                    Dim cobrador = Session("CobradorSeleccionado")
                    If cobrador Then
                        PnlGoodAndBadPhones.Visible = False
                        PnlBasPhones.Visible = False
                        ReBind(selectedPage:=1)

                    End If
                End If

            Else
                Response.Redirect("~/Principal.aspx")

            End If

        Catch ex As Exception
            Dim msg = "Problema al la cargar página, por favor vuelva a intentarlo : " & ex.Message
            'RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try
    End Sub

    Protected Sub WhatsAppToAll_Click(sender As Object, e As EventArgs)

        PnlGoodAndBadPhones.Visible = True
        PnlPrimary.Visible = False
        PnlBasPhones.Visible = False
        If Session("CobradorSeleccionado") Then

            CardTitleLiteral.Text = "Clientes a los que se enviará"

            rebindGoodPhones()
        End If




    End Sub
    Protected Sub btnCorrupPhones_Click(sender As Object, e As EventArgs)

        PnlGoodAndBadPhones.Visible = False
        PnlPrimary.Visible = False
        PnlBasPhones.Visible = True
        If Session("CobradorSeleccionado") Then
            CardTitleLiteral.Text = "Clientes con teléfonos corruptos"
            rebindBadPhones()

        End If
    End Sub
    'Protected Sub rebindDetails()
    '    Dim clients As List(Of DocsDto)
    '    If Session("SelectedDetail") = "Corrupt" Then

    '        clients = clientsWithCorrupPhones(Session("CobradorSeleccionado"))
    '    ElseIf Session("SelectedDetail") = "Valid" Then
    '        clients = clientsValidToSend(Session("CobradorSeleccionado"))
    '    Else
    '        clients = New List(Of DocsDto)
    '    End If
    '    SendGridview.DataSource = clients
    '    SendGridview.DataBind()
    'End Sub
    Protected Sub rebindBadPhones()
        Dim clients As List(Of DocsDto)

        clients = clientsWithCorrupPhones(Session("CobradorSeleccionado"))
        GridViewBadPhones.DataSource = clients
        GridViewBadPhones.DataBind()
    End Sub
    Protected Sub rebindGoodPhones()
        Dim clients As List(Of DocsDto)

        clients = clientsValidToSend(Session("CobradorSeleccionado"))
        SendGridview.DataSource = clients
        SendGridview.DataBind()
    End Sub

    Protected Sub ReBind(Optional selectedPage As Integer = 1)
        Try


            Dim cobrador = Session("CobradorSeleccionado")

            Dim selectClause = "Select A.Codigo_clie As Codigo, A.Nombre_clie Nombre,  Case 
        WHEN NULLIF(A.Telef_clien, '') IS NOT NULL THEN A.Telef_clien 
        Else A.CL_CELULAR
    End As Telefono, FORMAT(A.Saldo_actua, 'C', 'es-HN')  as Saldo, ISNULL(CONVERT(VARCHAR, CONVERT(DATE, w.FechaDeEnvio)),'Nunca') AS Ultimo_envio"
            Dim fromClause = " From CLIENTES a "
            Dim joinCLause = "			 left join LogDocumentosPorWhatsApp w ON A.Codigo_clie=w.CodigoDeCliente and w.fueExitoso=1
"
            Dim whereClause = " where A.cl_cobrador = @Cobrador And A.Saldo_actua>0"
            Dim orderByClause = " Order BY A.Saldo_actua desc"
            Dim paginationClause = "OFFSET @Offset ROWS FETCH Next @PageSize ROWS ONLY"
            Dim dataQuery = $"
                            {selectClause} {fromClause} {joinCLause} {whereClause} 
                            {orderByClause} 
                            {paginationClause} OPTION(RECOMPILE)
                        "
            Dim queryCount = $"
                            select count(*) {fromClause}  {whereClause} 
                           OPTION(RECOMPILE) "
            Using FunamorContext As New FunamorContext()
                Dim offset = (selectedPage - 1) * 10

                Dim sqlParameters As New List(Of SqlParameter)
                sqlParameters.Add(New SqlParameter("@Cobrador", $"{cobrador}"))
                sqlParameters.Add(New SqlParameter("@Offset", offset))
                sqlParameters.Add(New SqlParameter("@PageSize", 10))
                Dim result As List(Of DocsDtoCL) = FunamorContext.Database.SqlQuery(Of DocsDtoCL)(
                dataQuery, sqlParameters.ToArray()).ToList()

                Dim totalCountparams As New List(Of SqlParameter)
                totalCountparams.Add(New SqlParameter("@Cobrador", $"{cobrador}"))
                totalCountparams.Add(New SqlParameter("@Offset", offset))
                totalCountparams.Add(New SqlParameter("@PageSize", 10))
                Dim toltalC As Integer = FunamorContext.Database.SqlQuery(Of Integer)(queryCount, totalCountparams.ToArray()).FirstOrDefault()

                Dim cobradorQuery = "select C.codigo_cobr Codigo, isnull(D.nombre_cobr,'') Lider, REPLACE(ISNULL(d.cob_telefo, ''), '-', '')  Telefono_lider, isnull(c.cob_zona,'') Zona from COBRADOR c
join COBRADOR d ON c.cob_lider=d.codigo_cobr
where c.codigo_cobr like @Cobrador"
                Dim cobParams As New List(Of SqlParameter)
                cobParams.Add(New SqlParameter("@Cobrador", $"{cobrador}"))
                Dim cobradorInfo As CobradorDto = FunamorContext.Database.SqlQuery(Of CobradorDto)(cobradorQuery, cobParams.ToArray()).FirstOrDefault()
                Session("TelefonoLider") = cobradorInfo.Telefono_lider.Trim
                Session("cobradorObject") = cobradorInfo

                DashboardGridview.DataSource = result
                DashboardGridview.DataBind()

                TotalItems = toltalC
                PageNumber = selectedPage
                TotalPages = Math.Ceiling(toltalC / PageSize)

                ' Update the Previous and Next buttons' enabled state
                Dim pages As New List(Of Integer)()
                For i As Integer = 1 To TotalPages
                    pages.Add(i)
                Next

                rptPager.DataSource = pagination.GetLimitedPageNumbers(TotalItems, PageSize, PageNumber, 3)
                rptPager.DataBind()
                lnkbtnPrevious.Enabled = PageNumber > 1
                lnkbtnNext.Enabled = PageNumber < TotalPages
                lblTotalCount.DataBind()
            End Using
        Catch ex As Exception
            Dim msg = "No se cargó la tabla."
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)
        End Try
    End Sub



    Private Function clientsValidToSend(cobrador As String)
        Try
            Using FunamorContext As New FunamorContext()
                Dim sqlParameters As New List(Of SqlParameter)
                sqlParameters.Add(New SqlParameter("@cobrador", cobrador.Trim()))

                Dim result As List(Of DocsDto) = FunamorContext.Database.SqlQuery(Of DocsDto)(
            "EXEC SP_VS_CarteraDeClienteParaWhatsapp @cobrador", sqlParameters.ToArray()).ToList()
                Return result
            End Using
        Catch ex As Exception
            Dim msg = "No se cargó la tabla."
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)
        End Try
    End Function
    Private Function clientsWithCorrupPhones(cobrador As String)
        Try
            Using FunamorContext As New FunamorContext()
                Dim sqlParameters As New List(Of SqlParameter)
                sqlParameters.Add(New SqlParameter("@cobrador", cobrador.Trim()))

                Dim result As List(Of DocsDto) = FunamorContext.Database.SqlQuery(Of DocsDto)(
            "EXEC SP_VS_CarteraMalaDeClienteParaWhatsapp @cobrador", sqlParameters.ToArray()).ToList()
                Return result
            End Using
        Catch ex As Exception
            Dim msg = "No se cargó la tabla."
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)
        End Try
    End Function
    Public Function EnviarEstadoDeCuenta(result As List(Of DocsDto))
        Try
            Dim falla = ""
            Dim fallas As New List(Of String)
            Dim Usuario_Aut = "manager"
            Dim count = 0
            If Usuario_Aut = Session("Usuario_Aut") Then
                Dim leaderPhone = Session("TelefonoLider")
                Dim cobrador As CobradorDto = CType(Session("cobradorObject"), CobradorDto)
                Dim Leader = ""
                Dim numeroPBX = "2647-3390"
                If cobrador IsNot Nothing Then
                    leaderPhone = cobrador.Telefono_lider
                    Leader = cobrador.Lider
                End If
                Dim Usuario = Session("Usuario")
                Dim Clave = Session("Clave")
                For Each cliente In result
                    Try
                        Dim Informe As New Movimiento_Clientes

                        Informe.SetDatabaseLogon(Usuario, Clave)
                        Informe.SetParameterValue("Cliente", cliente.Codigo)

                        Dim nombreArchivo As String = cliente.Codigo + "-" + DateTime.Now.ToString("yyyy-MM-dd") + "" + ".pdf" ' Cambia el nombre del archivo si lo deseas


                        Informe.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat)
                        Dim cap = "Estimado(a) " + cliente.Nombre + $", Amor Eterno manda su estado de cuenta. Para mayor informacion o si desea comunicarse con servicio al cliente puede llamar al numero Pbx: {numeroPBX} O escribir al siguiente numero: " + leaderPhone
                        Dim user = Session("Usuario_Aut")

                        Dim r4esult As ResultW = whatsapi.sendWhatsAppDocs(doc:=Informe, name:=nombreArchivo, localNumber:=cliente.Telefono, caption:=cap, couentryCode:="504", user:=user, clientCode:=cliente.Codigo, instancia:="default")
                        'Debug.WriteLine(r4esult.Msg)

                        If r4esult.Success = False Then
                            Dim m = "Codigo de cliente: " + cliente.Codigo + r4esult.Msg
                            DebugHelper.SendDebugInfo("danger", New Exception(m), Session("Usuario_Aut"))
                            fallas.Add("el servicio de envio")

                        Else
                            count = count + 1
                        End If
                    Catch ex As Exception
                        fallas.Add("la generación del reporte")

                        Dim m = "Codigo de cliente: " + cliente.Codigo
                        DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"), m)
                        whatsapi.logW(name:="", couentryCode:="504", localNumber:=cliente.Telefono, caption:=ex.GetType().Name, clientCode:=cliente.Codigo, user:=Session("Usuario_Aut"), instancia:="", docDescription:="Estado de cuenta", isSuccess:=False, msg:=ex.Message)
                    End Try

                Next
            End If
            Dim countErrors = result.Count - count
            Dim mensaje = $"Enviados con éxito: {count}. Fallidos: {countErrors}"

            If result.Count = 0 Then
                Dim uniqueSet As New HashSet(Of String)(fallas)

                ' Convert the HashSet back to a List
                Dim uniqueList As List(Of String) = uniqueSet.ToList()
                Dim s = String.Join(", ", uniqueList)

                AlertHelper.GenerateAlert("danger", mensaje + $" debido a errores en {uniqueList}", alertPlaceholder)

            Else
                AlertHelper.GenerateAlert("info", mensaje, alertPlaceholder)
            End If


            Return True

        Catch ex As Exception
            Dim msg = "Al mandar mensajes"
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)
        End Try



    End Function
    Protected Sub lnkbtnPage_Click(sender As Object, e As EventArgs)
        Dim lnkButton As LinkButton = CType(sender, LinkButton)
        Dim resultInteger As Integer
        If Integer.TryParse(lnkButton.Text, resultInteger) Then
            Dim SelectedPageDashboardVentas As Integer = Integer.Parse(lnkButton.Text)
            Session("SelectedPageDashboardVentas") = SelectedPageDashboardVentas
            ReBind(selectedPage:=SelectedPageDashboardVentas)
        End If

    End Sub

    Protected Sub lnkbtnPrevious_Click(sender As Object, e As EventArgs)

        If Session("SelectedPageDashboardVentas") IsNot Nothing AndAlso Session("SelectedPageDashboardVentas") > 1 Then
            Session("SelectedPageDashboardVentas") = Session("SelectedPageDashboardVentas") - 1

            ReBind(selectedPage:=Session("SelectedPageDashboardVentas"))

        End If

    End Sub

    Protected Sub lnkbtnNext_Click(sender As Object, e As EventArgs)
        'filterData.PageNumber = filterData.PageNumber + 1
        If Session("SelectedPageDashboardVentas") IsNot Nothing Then
            Session("SelectedPageDashboardVentas") = Session("SelectedPageDashboardVentas") + 1
            ReBind(selectedPage:=Session("SelectedPageDashboardVentas"))
        End If

    End Sub

    Protected Sub btnExitWhatsapToAll_Click(sender As Object, e As EventArgs)
        PnlGoodAndBadPhones.Visible = False
        PnlPrimary.Visible = True
        PnlBasPhones.Visible = False
    End Sub

    Protected Sub btnSendMassiveWhatsApp_Click(sender As Object, e As EventArgs)
        If Session("CobradorSeleccionado") Then
            Dim clients As List(Of DocsDto) = clientsValidToSend(Session("CobradorSeleccionado"))
            EnviarEstadoDeCuenta(clients)

        End If
    End Sub

    Protected Sub SendGridview_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        SendGridview.PageIndex = e.NewPageIndex
        rebindGoodPhones()
    End Sub


    Protected Sub GridViewBadPhones_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        GridViewBadPhones.PageIndex = e.NewPageIndex
        rebindBadPhones()
    End Sub
End Class