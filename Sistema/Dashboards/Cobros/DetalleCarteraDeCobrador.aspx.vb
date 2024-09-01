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
            'Session("BackPageUrl") = "~/Dashboards/Cobros/Cobros.aspx"
            'Dim thisPage = "~/Dashboards/Cobros/DetalleCarteraDeCobrador.aspx"
            'If Usuario_Aut IsNot Nothing Then
            '    Usuario_Aut = Usuario_Aut.ToString().Trim().ToUpper()
            '    If Session("Usuario") = "" OrElse Not AuthHelper.isAuthorized(Usuario_Aut, "COBROS_A") Then
            '        Response.Redirect("~/Principal.aspx")
            '    End If

            If Not IsPostBack Then
                'FillDll()
                Dim cobrador = Session("CobradorSeleccionado")
                If cobrador Then
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
        Dim clients = clientsValidToSend("4872")
        SendGridview.DataSource = clients
        SendGridview.DataBind()
    End Sub
    Protected Sub ReBind(Optional selectedPage As Integer = 1)
        Dim cobrador = Session("CobradorSeleccionado")

        Dim selectClause = "Select A.Codigo_clie As Codigo, A.Nombre_clie Nombre,  Case 
        WHEN NULLIF(A.Telef_clien, '') IS NOT NULL THEN A.Telef_clien 
        Else A.CL_CELULAR
    End As Telefono"
        Dim fromClause = " From CLIENTES "
        Dim whereClause = "A where A.cl_cobrador = @Cobrador And A.Saldo_actua>0"
        Dim groupByClause = ""
        Dim orderByClause = "Order BY "
        Dim paginationClause = "OFFSET @Offset ROWS FETCH Next @PageSize ROWS ONLY"
        Dim dataQuery = $"
                            {selectClause} {fromClause}  {whereClause} {groupByClause}
                            {orderByClause} 
                            {paginationClause} OPTION(RECOMPILE)
                        "
        Using FunamorContext As New FunamorContext()
            Dim offset = (selectedPage - 1) * 10

            Dim sqlParameters As New List(Of SqlParameter)
            sqlParameters.Add(New SqlParameter("@Cobrador", $"{Cobrador}"))
            sqlParameters.Add(New SqlParameter("@Offset", offset))
            sqlParameters.Add(New SqlParameter("@PageSize", 10))
            Dim result As List(Of DocsDto) = FunamorContext.Database.SqlQuery(Of DocsDto)(
            dataQuery, sqlParameters.ToArray()).ToList()
            SendGridview.DataSource = result
            SendGridview.DataBind()
        End Using

    End Sub

    Protected Sub btnCorrupPhones_Click(sender As Object, e As EventArgs)

    End Sub
    Private Function clientsValidToSend(cobrador)
        Dim dataQuery = ""
        dataQuery = "SELECT A.Codigo_clie as Codigo, A.Nombre_clie Nombre, "
        dataQuery += "   CASE 
        WHEN NULLIF(A.Telef_clien, '') IS NOT NULL THEN A.Telef_clien 
        ELSE A.CL_CELULAR
    END AS Telefono"
        dataQuery += " From CLIENTES A where A.cl_cobrador = @Cobrador and A.Saldo_actua>0 and CL_CELULAR IS NOT NULL
 	AND LEN(REPLACE(REPLACE(Telef_clien,'504',''),' ',''))=8
	AND LEN(REPLACE(REPLACE(CL_CELULAR,'504',''),' ',''))=8
	AND LEN(REPLACE(Telef_clien,'-',''))=8
	AND LEN(REPLACE(CL_CELULAR,'-',''))=8
	AND SUBSTRING(REPLACE(Telef_clien,'-',''),1,1) IN ('3','9','8')
	AND SUBSTRING(REPLACE(CL_CELULAR,'-',''),1,1) IN ('3','9','8')"
        Using FunamorContext As New FunamorContext()
            Dim sqlParameters As New List(Of SqlParameter)
            sqlParameters.Add(New SqlParameter("@Cobrador", $"{cobrador}"))

            Dim result As List(Of DocsDto) = FunamorContext.Database.SqlQuery(Of DocsDto)(
            dataQuery, sqlParameters.ToArray()).ToList()
            Return result
        End Using
    End Function
    Public Function EnviarEstadoDeCuenta(result As List(Of DocsDto))
        Dim leaderPhone = ""
        Dim Usuario = Session("Usuario")
        Dim Clave = Session("Clave")
        For Each cliente In result
            Dim Informe As New Movimiento_Clientes

            Informe.SetDatabaseLogon(Usuario, Clave)
            Informe.SetParameterValue("Cliente", cliente.COdigo)

            Dim nombreArchivo As String = Session("CodigoCliente").TrimEnd + "-" + DateTime.Now.ToString("yyyy-MM-dd") + "" + ".pdf" ' Cambia el nombre del archivo si lo deseas


            Informe.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat)
            Dim cap = "Estimado(a) " + cliente.Nombre + " Amor Eterno envía su estado de cuenta. Para mayor informacion o si deseas comunicarte con servicio al cliente puedes llamar al numero Pbx:
                        O escribir al siguiente numero" + LeaderPhone
            whatsapi.sendWhatsAppDocs(doc:=Informe, name:=cliente.Nombre, localNumber:=cliente.Telefono, caption:=cap, couentryCode:="504")

        Next

        Return True
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


End Class