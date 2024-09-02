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
            'Dim Usuario_Aut = Session("Usuario_Aut")
            Session("BackPageUrl") = "~/Dashboards/Cobros/Cobros.aspx"
            Dim thisPage = "~/Dashboards/Cobros/DetalleCarteraDeCobrador.aspx"
            'If Usuario_Aut IsNot Nothing Then
            '    Usuario_Aut = Usuario_Aut.ToString().Trim().ToUpper()
            '    If Session("Usuario") = "" OrElse Not AuthHelper.isAuthorized(Usuario_Aut, "COBROS_A") Then
            '        Response.Redirect("~/Principal.aspx")
            '    End If

            If Not IsPostBack Then
                'FillDll()
                Dim cobrador = Session("CobradorSeleccionado")
                If cobrador Then
                    PnlGoodAndBadPhones.Visible = False
                    ReBind(selectedPage:=1)

                End If
            End If
            PnlGoodAndBadPhones.Visible = False

            'Else
            '    Response.Redirect("~/Principal.aspx")

            'End If

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
        If Session("CobradorSeleccionado") Then
                Dim clients = clientsValidToSend(Session("CobradorSeleccionado"))
                SendGridview.DataSource = clients
                SendGridview.DataBind()
            End If




    End Sub
    Protected Sub ReBind(Optional selectedPage As Integer = 1)
        Try


            Dim cobrador = Session("CobradorSeleccionado")

            Dim selectClause = "Select A.Codigo_clie As Codigo, A.Nombre_clie Nombre,  Case 
        WHEN NULLIF(A.Telef_clien, '') IS NOT NULL THEN A.Telef_clien 
        Else A.CL_CELULAR
    End As Telefono, FORMAT(A.Saldo_actua, 'C', 'es-HN')  as Saldo"
            Dim fromClause = " From CLIENTES "
            Dim whereClause = "A where A.cl_cobrador = @Cobrador And A.Saldo_actua>0"
            Dim orderByClause = " Order BY A.Saldo_actua desc"
            Dim paginationClause = "OFFSET @Offset ROWS FETCH Next @PageSize ROWS ONLY"
            Dim dataQuery = $"
                            {selectClause} {fromClause}  {whereClause} 
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
                Dim toltalC = FunamorContext.Database.SqlQuery(Of Integer)(queryCount, totalCountparams.ToArray()).FirstOrDefault()

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

    Protected Sub btnCorrupPhones_Click(sender As Object, e As EventArgs)

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
    Public Function EnviarEstadoDeCuenta(result As List(Of DocsDto))
        Try
            Dim Usuario_Aut = "manager"
            If Usuario_Aut = Session("Usuario_Aut") Then
                Dim leaderPhone = ""

                Dim Usuario = Session("Usuario")
                Dim Clave = Session("Clave")
                For Each cliente In result
                    Dim Informe As New Movimiento_Clientes

                    Informe.SetDatabaseLogon(Usuario, Clave)
                    Informe.SetParameterValue("Cliente", cliente.Codigo)

                    Dim nombreArchivo As String = cliente.Codigo + "-" + DateTime.Now.ToString("yyyy-MM-dd") + "" + ".pdf" ' Cambia el nombre del archivo si lo deseas


                    Informe.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat)
                    Dim cap = "Estimado(a) " + cliente.Nombre + " Amor Eterno manda su estado de cuenta. Para mayor informacion o si desea comunicarse con servicio al cliente puede llamar al numero Pbx: O escribir al siguiente numero" + leaderPhone
                    Dim r4esult As ResultW = whatsapi.sendWhatsAppDocs(doc:=Informe, name:=nombreArchivo, localNumber:=cliente.Telefono, caption:=cap, couentryCode:="504")
                    Debug.WriteLine(r4esult.Msg)
                    If r4esult.Success = False Then
                        DebugHelper.SendDebugInfo("danger", New Exception(r4esult.Msg), Session("Usuario_Aut"))
                    Else
                        AlertHelper.GenerateAlert("success", r4esult.Msg, alertPlaceholder)

                    End If

                Next
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

    End Sub

    Protected Sub btnSendMassiveWhatsApp_Click(sender As Object, e As EventArgs)
        If Session("CobradorSeleccionado") Then
            Dim clients As List(Of DocsDto) = clientsValidToSend(Session("CobradorSeleccionado"))
            EnviarEstadoDeCuenta(clients)

        End If
    End Sub
End Class