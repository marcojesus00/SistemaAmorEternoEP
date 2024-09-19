Imports System.Data.SqlClient
Imports Sistema.CobrosDashboard

Public Class MonitorEstadosDeCuentaEnviados
    Inherits System.Web.UI.Page
    Public PageNumber As Integer = 1
    Public PageSize As Integer = 10
    Public TotalPages As Integer '
    Public TotalItems As Integer = 0
    Public itemText As String
    Public pagination As PaginationHelper = New PaginationHelper

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("BackPageUrl") = "~/Dashboards/Cobros/DetalleCarteraDeCobrador.aspx"
        Dim Usuario_Aut = Session("Usuario_Aut")

        If Usuario_Aut IsNot Nothing Then
            Usuario_Aut = Usuario_Aut.ToString().Trim().ToUpper()
            If Session("Usuario") = "" OrElse Not AuthHelper.isAuthorized(Usuario_Aut, "COBROS_A") Then
                Response.Redirect("~/Principal.aspx")
            End If

            If Not IsPostBack Then
                FillDll()
                ReBind()
            End If
        Else

            Response.Redirect("~/Principal.aspx")

        End If

    End Sub
    Private Sub FillDll()
        textBoxInitialDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
        TextBoxFinalDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
        Using context As New FunamorContext, ventasContext As New AeVentasDbContext

            Dim leaders = context.Cobradores.AsNoTracking().Where(Function(c) c.Codigo = c.CobLider Or c.Codigo.Contains("4894") Or c.Codigo.Contains("3072")) _
                .Select(Function(l) New With {l.Codigo, .Nombre = l.Nombre & " " & l.Codigo}) _
                .OrderBy(Function(l) l.Nombre).ToList()
            ddlLeader.DataSource = leaders
            ddlLeader.DataTextField = "Nombre"
            ddlLeader.DataValueField = "Codigo"
            ddlLeader.DataBind()
            ddlLeader.Items.Insert(0, New ListItem("Todos los líderes", ""))
        End Using
        ddlStatus.Items.Add(New ListItem("Todos", 0))
        ddlStatus.Items.Add(New ListItem("Enviados", 1))
        ddlStatus.Items.Add(New ListItem("No Enviados", 2))



    End Sub

    Protected Sub lnkbtnPage_Click(sender As Object, e As EventArgs)
        Dim lnkButton As LinkButton = CType(sender, LinkButton)
        Dim resultInteger As Integer
        If Integer.TryParse(lnkButton.Text, resultInteger) Then
            Dim SelectedPageDashboardVentas As Integer = Integer.Parse(lnkButton.Text)
            Session("SelectedPageDashboardVentas") = SelectedPageDashboardVentas
            ReBind(selectedPage:=SelectedPageDashboardVentas)
        End If

    End Sub
    Protected Sub ReBind(Optional selectedPage As Integer = 1)
        Try
            Dim ClientCode = textBoxClientCode.Text
            Dim CollectorCode = textBoxCode.Text
            Dim LeaderCode = ddlLeader.SelectedValue
            Dim phone = textBoxPhone.Text
            Dim pagesize = 10
            Dim offset = (selectedPage - 1) * pagesize

            Dim paramsStringSPList As New List(Of String)()
            'Dim paramsCountStringSPList As New List(Of String)()
            Dim sqlParameters As New List(Of SqlParameter)
            Dim sqlCountParameters As New List(Of SqlParameter)
            Dim initialDate As DateTime
            Dim finalDate As DateTime


            If DateTime.TryParse(textBoxInitialDate.Text, initialDate) Then
                Dim InitialateParam As New SqlParameter("@InitialDate", SqlDbType.DateTime)
                InitialateParam.Value = initialDate
                sqlParameters.Add(InitialateParam)
                Dim InitialateParam1 As New SqlParameter("@InitialDate", SqlDbType.DateTime)
                InitialateParam1.Value = initialDate
                sqlCountParameters.Add(InitialateParam1)
                paramsStringSPList.Add("@InitialDate=@InitialDate")


            End If
            If DateTime.TryParse(TextBoxFinalDate.Text, finalDate) Then
                Dim IniatialateParam As New SqlParameter("@FinalDate", SqlDbType.DateTime)
                IniatialateParam.Value = finalDate
                sqlParameters.Add(IniatialateParam)
                Dim IniatialateParam1 As New SqlParameter("@FinalDate", SqlDbType.DateTime)
                IniatialateParam1.Value = finalDate
                sqlCountParameters.Add(IniatialateParam1)
                paramsStringSPList.Add("@FinalDate=@FinalDate")


            End If


            If ddlStatus.SelectedValue = 1 Then
                paramsStringSPList.Add("@Status=@Status")
                sqlCountParameters.Add(New SqlParameter("@Status", 1))
                sqlParameters.Add(New SqlParameter("@Status", 1))

            ElseIf ddlStatus.SelectedValue = 2 Then
                paramsStringSPList.Add("@Status=@Status")
                sqlCountParameters.Add(New SqlParameter("@Status", 0))
                sqlParameters.Add(New SqlParameter("@Status", 0))
            Else


            End If
            If Not String.IsNullOrEmpty(ClientCode) Then
                paramsStringSPList.Add("@Client=@Client")
                sqlParameters.Add(New SqlParameter("@Client", "%" & ClientCode & "%"))
                sqlCountParameters.Add(New SqlParameter("@Client", "%" & ClientCode & "%"))

            End If

            If Not String.IsNullOrEmpty(CollectorCode) Then
                paramsStringSPList.Add("@Collector=@Collector")
                sqlParameters.Add(New SqlParameter("@Collector", "%" & CollectorCode & "%"))
                sqlCountParameters.Add(New SqlParameter("@Collector", "%" & CollectorCode & "%"))

            End If

            If Not String.IsNullOrEmpty(LeaderCode) Then
                paramsStringSPList.Add("@Leader=@Leader")
                sqlParameters.Add(New SqlParameter("@Leader", "%" & LeaderCode & "%"))

                sqlCountParameters.Add(New SqlParameter("@Leader", "%" & LeaderCode & "%"))

            End If

            If Not String.IsNullOrEmpty(phone) Then
                paramsStringSPList.Add("@Phone=@Phone")

                sqlParameters.Add(New SqlParameter("@Phone", "%" & phone & "%"))
                sqlCountParameters.Add(New SqlParameter("@Leader", "%" & LeaderCode & "%"))

            End If
            Dim paramsSPString = ""
            Dim paramsCountSPString = ""
            If paramsStringSPList.Count > 0 Then
                paramsCountSPString = String.Join(", ", paramsStringSPList)
            End If



            Using context As New FunamorContext()
                Dim toltalC = context.Database.SqlQuery(Of Integer)($"EXEC SP_VS_DASH_GET_SENT_ACOUNT_STATEMENTS {paramsCountSPString}", sqlCountParameters.ToArray()).FirstOrDefault()

                sqlParameters.Add(New SqlParameter("@Offset", offset))

                paramsStringSPList.Add("@Offset=@Offset")
                If paramsStringSPList.Count > 0 Then
                    paramsSPString = String.Join(", ", paramsStringSPList)
                End If

                Dim result As List(Of WhatsAppMonitorDto) = context.Database.SqlQuery(Of WhatsAppMonitorDto)(
                    $"EXEC SP_VS_DASH_GET_SENT_ACOUNT_STATEMENTS {paramsSPString}", sqlParameters.ToArray()).ToList()





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


    Protected Sub lnkbtnPrevious_Click(sender As Object, e As EventArgs)

        If Session("SelectedPageDashboardVentas") IsNot Nothing AndAlso Session("SelectedPageDashboardVentas") > 1 Then
            Session("SelectedPageDashboardVentas") = Session("SelectedPageDashboardVentas") - 1

            ReBind(selectedPage:=Session("SelectedPageMonitorWhatsApp"))

        End If

    End Sub

    Protected Sub lnkbtnNext_Click(sender As Object, e As EventArgs)
        'filterData.PageNumber = filterData.PageNumber + 1
        If Session("SelectedPageDashboardVentas") IsNot Nothing Then
            Session("SelectedPageDashboardVentas") = Session("SelectedPageDashboardVentas") + 1
            ReBind(selectedPage:=Session("SelectedPageDashboardVentas"))
        End If

    End Sub





    Protected Sub SendGridview_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        DashboardGridview.PageIndex = e.NewPageIndex
        ReBind(0)
    End Sub


    Protected Sub GridViewBadPhones_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        'GridViewBadPhones.PageIndex = e.NewPageIndex
        'rebindBadPhones()
    End Sub

    Protected Sub DashboardGridview_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Get the value of the "sent" column (assuming the column index is 1)
            Dim sentValue As String = e.Row.Cells(7).Text.Trim().ToLower()

            ' If the value is not "yes", change the background color
            If sentValue <> "enviado" Then
                e.Row.BackColor = System.Drawing.Color.Red
                e.Row.ForeColor = System.Drawing.Color.Red
                e.Row.CssClass = "table-danger"
                e.Row.Font.Strikeout = True
            End If
        End If
    End Sub
End Class