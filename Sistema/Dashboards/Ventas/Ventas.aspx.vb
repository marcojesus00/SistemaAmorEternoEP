Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI.WebControls
Imports System.Data.Entity
Imports System.Linq

Public Class VentasDashboard
    Inherits System.Web.UI.Page
    'Public Event AlertGenerated As EventHandler(Of AlertEventArgs)
    Private _receipts As List(Of ReciboDeCobro)
    Dim thisPage = "~/Dashboards/Ventas/Ventas.aspx"
    Public PageNumber As Integer = 1
    Public PageSize As Integer = 10
    Public TotalPages As Integer '
    Public TotalItems As Integer = 0
    Private ReadOnly _controlStateManager As New ControlStateManager()

    Dim filterData As New ReportData()
    Public Property SalesReceiptsCachedList As List(Of VentasDto)
        Get
            Return CachingHelper.GetOrFetch(filterData.GenerateCacheKey, AddressOf getReceiptsFromDB, 100)
        End Get
        Set(value As List(Of VentasDto))
            CachingHelper.CacheSet(filterData.GenerateCacheKey(), value, 100)
        End Set
    End Property

    'Public Property ClientsContainsCollectorCachedList As List(Of Cliente)
    '    Get
    '        Return CachingHelper.GetOrFetch("ClientsWithRemainingBalance", AddressOf GetClientsByCollectorIdFromDb, 150)
    '    End Get
    '    Set(value As List(Of Cliente))
    '        CachingHelper.CacheSet("ClientsWithRemainingBalance", value, 150)
    '    End Set
    'End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim Usuario_Aut = Session("Usuario_Aut")
            Session("BackPageUrl") = "~/monitorventas.aspx"
            Dim thisPage = "~/Dashboards/Ventas/Ventas.aspx"
            If Usuario_Aut IsNot Nothing Then
                Usuario_Aut = Usuario_Aut.ToString().Trim().ToUpper()
                If Session("Usuario") = "" OrElse Not AuthHelper.isAuthorized(Usuario_Aut, "VENTAS_A") Then
                    'Response.Redirect("~/Principal.aspx")
                End If

                If Not IsPostBack Then
                    FillDll()
                    UpdatedData(filterData)
                    ReBind()
                    'storeOldValues()
                End If
                pnlMap.Visible = False

                AddHandler DashboardGridview.PageIndexChanging, AddressOf DashboardGridview_PageIndexChanging
            Else
                Response.Redirect("~/Principal.aspx")

            End If

        Catch ex As Exception
            Dim msg = "Problema al cargar página, por favor vuelva a intentarlo : " & ex.Message
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try
    End Sub
    Public Sub UpdatedData(ByVal Data As ReportData)

        Data.EndDate = endDate.Text
        Data.StartDate = startDate.Text
        Data.LeaderCode = ddlLeader.SelectedValue.Trim
        Data.SalesPersonCode = textBoxCode.Text.Trim
        Data.ClientCode = textBoxClientCode.Text
        Data.ValidReceiptsMark = ddlValidReceipts.SelectedValue
        Data.CompanyCode = ddlCompany.SelectedValue.Trim
        Data.ZoneCode = ddlCity.SelectedValue.Trim
        Data.DocumentNumber = textBoxNumDoc.Text.Trim
        Data.ServiceId = ddlService.SelectedValue.Trim
        Data.PageNumber = PageNumber ' Assuming PageNumber and PageSize are set elsewhere
        Data.PageSize = PageSize
    End Sub

    Protected Sub DashboardType_change(sender As Object, e As EventArgs) Handles DashboardType.SelectedIndexChanged
        ReBind()
    End Sub

    'Fill the gridviews 
    Protected Sub ReBind(Optional selectedPage As Integer = 1)
        Try

            Dim almostEmpyFilters = ddlCompany.SelectedValue.Trim = "" AndAlso ddlLeader.SelectedValue.Trim = "" AndAlso ddlCity.SelectedValue.Trim = ""
            Dim salePersonCodeConstraint = textBoxCode.Text.Trim.Length <= 2 AndAlso almostEmpyFilters
            Dim clientCodeConstraint = textBoxClientCode.Text.Trim.Length <= 2 AndAlso almostEmpyFilters
            Dim startDateParam As DateTime
            Dim endDateParam As DateTime
            Dim endD = endDate.Text
            Dim initD = startDate.Text
            Dim datesTooSpread = False

            If DateTime.TryParse(initD, startDateParam) AndAlso DateTime.TryParse(endD, endDateParam) Then
                Dim difference As TimeSpan = endDateParam - startDateParam
                If difference.TotalDays > 7 Then
                    datesTooSpread = True
                End If
            End If
            If DashboardType.SelectedValue = "0" Then

                'If (salePersonCodeConstraint AndAlso clientCodeConstraint) And datesTooSpread Then
                '    Dim msg = "Por favor refine su búsqueda"
                '    AlertHelper.GenerateAlert("warning", msg, alertPlaceholder)
                'Else
                'Dim DataList = GetReceiptDataForGridview()
                Dim result As PaginatedResult(Of SalesGroupedDto) = GetGroupedSalesBySalesmanFromDB(selectedPage)

                endDate.Enabled = True
                startDate.Enabled = True
                ddlValidReceipts.Enabled = True
                lblNumDoc.Text = "Número de documento del recibo"
                BtnRouteOfReceiptsMapByLeader.Enabled = True
                ddlService.Enabled = False
                BindGridView(result.Data, result.TotalCount, selectedPage)


                'End If
            ElseIf DashboardType.SelectedValue = "1" Then
                startDate.Enabled = True
                endDate.Enabled = True
                ddlValidReceipts.Enabled = True
                ddlService.Enabled = True

                BtnRouteOfReceiptsMapByLeader.Enabled = False
                lblNumDoc.Text = "Número de identidad del cliente"
                DashboardGridview.DataSource = Nothing
                DashboardGridview.DataBind()

                'If salePersonCodeConstraint AndAlso clientCodeConstraint Then
                '    Dim msg = "Por favor refine su búsqueda"
                '    AlertHelper.GenerateAlert("warning", msg, alertPlaceholder)

                'Else

                'End If
                Dim dataList = GetReceiptByServiceDataForGridview() ' GetPortfolioDataForGridview()

                'BindGridView(dataList)
            End If
        Catch ex As SqlException
            Dim msg = "Problema con la base de datos, por favor vuelva a intentarlo : " & ex.Message
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)


        Catch ex As Exception
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try
    End Sub

    Private Sub FillDll()
        endDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
        startDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
        Using context As New FunamorContext, ventasContext As New AeVentasDbContext
            Dim queryServices = "SELECT serv_codigo as Codigo, serv_descri as Nombre  FROM [FUNAMOR].[dbo].[SERVICIO] where serv_codigo <>''"
            Dim services = context.Database.SqlQuery(Of ServicesDto)(queryServices).ToList()
            ddlService.DataSource = services
            ddlService.DataTextField = "Nombre"
            ddlService.DataValueField = "Codigo"
            ddlService.DataBind()
            ddlService.Items.Insert(0, New ListItem("Todos los servicios", ""))

            Dim companies = context.Empresas.AsNoTracking().Where(Function(e) e.Codigo IsNot Nothing AndAlso e.Codigo.Trim().Length > 0).Select(Function(c) New With {c.Codigo, c.Nombre}).ToList()
            ddlCompany.DataSource = companies
            ddlCompany.DataTextField = "Nombre"
            ddlCompany.DataValueField = "Codigo"
            ddlCompany.DataBind()
            ddlCompany.Items.Insert(0, New ListItem("Todas las empresas", ""))
            Dim zones = ventasContext.MunicipiosZonasDepartamentos.AsNoTracking().
                Select(Function(c) New With {.Nombre = c.NombreMunicipio & " " & c.ZonaId, .Codigo = c.MunicipioId}) _
                .OrderBy(Function(z) z.Nombre).ToList()
            ddlCity.DataSource = zones
            ddlCity.DataTextField = "Nombre"
            ddlCity.DataValueField = "Codigo"
            ddlCity.DataBind()
            ddlCity.Items.Insert(0, New ListItem("Todas las zonas", ""))
            Dim leaders = context.Vendedores.AsNoTracking().Where(Function(c) c.Codigo = c.Lider) _
                .Select(Function(l) New With {l.Codigo, .Nombre = l.Nombre & " " & l.Codigo}) _
                .OrderBy(Function(l) l.Nombre).ToList()
            ddlLeader.DataSource = leaders
            ddlLeader.DataTextField = "Nombre"
            ddlLeader.DataValueField = "Codigo"
            ddlLeader.DataBind()
            ddlLeader.Items.Insert(0, New ListItem("Todos los líderes", ""))
        End Using
        ddlValidReceipts.Items.Add(New ListItem("Válidos", "N"))
        ddlValidReceipts.Items.Add(New ListItem("Nulos", "X"))
        ddlValidReceipts.Items.Add(New ListItem("Todos los recibos", ""))

    End Sub

    Private Sub BindGridView(dataList As Object, totalCount As Integer, page As Integer)


        Try

            Dim msg = ""

            TotalItems = totalCount
            PageNumber = page
            TotalPages = Math.Ceiling(totalCount / PageSize)
            DashboardGridview.DataSource = dataList
            DashboardGridview.DataBind()
            ' Update the Previous and Next buttons' enabled state
            Dim pages As New List(Of Integer)()
            For i As Integer = 1 To TotalPages
                pages.Add(i)
            Next

            rptPager.DataSource = pages
            rptPager.DataBind()
            lnkbtnPrevious.Enabled = PageNumber > 1
            lnkbtnNext.Enabled = PageNumber < TotalPages
            lblTotalCount.DataBind()

            If DashboardGridview.Rows.Count = 0 Then
                msg = "No se encontraron resultados"
            Else
                msg = "Mostrando primeros " & $"{dataList.Count} resultados."

            End If
        Catch ex As SqlException
            Dim msg = "Problema con la base de datos, por favor vuelva a intentarlo : " & ex.Message
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)


        Catch ex As Exception
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try
    End Sub




    Protected Sub DashboardGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        Try

            Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
            Dim keyValue As String = DashboardGridview.DataKeys(rowIndex).Value.ToString()

            If e.CommandName = "ClientsByCollectorMap" Then
                'ClientsByCollectorMap(keyValue)
            ElseIf e.CommandName = "RouteOfReceiptsMap" Then
                If textBoxClientCode.Text.Length > 0 Then
                    textBoxClientCode.Text = ""

                End If

                RouteOfReceiptsMap(keyValue)
            End If


        Catch ex As FormatException
            AlertHelper.GenerateAlert("danger", "Error al convertir el índice de fila.", alertPlaceholder)
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

        Catch ex As IndexOutOfRangeException
            AlertHelper.GenerateAlert("danger", "Índice de fila fuera de rango.", alertPlaceholder)
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

        Catch ex As IOException
            AlertHelper.GenerateAlert("danger", "Error de entrada/salida al procesar el archivo.", alertPlaceholder)
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

        Catch ex As Exception
            AlertHelper.GenerateAlert("danger", "Se produjo un error inesperado: " & ex.Message, alertPlaceholder)
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

        End Try
    End Sub
    Protected Sub DashboardGridView_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim btnClientsByCollectorMap As LinkButton = CType(e.Row.FindControl("btnClientsByCollectorMap"), LinkButton)
            Dim btnRouteOfReceiptsMap As LinkButton = CType(e.Row.FindControl("btnRouteOfReceiptsMap"), LinkButton)
            Dim detailsControl As GridView = TryCast(e.Row.FindControl("DetailsControl"), GridView)
            Dim pnlPluMinus As Panel = TryCast(e.Row.FindControl("pnlPluMinus"), Panel)

            Dim rowIndex As Integer = e.Row.RowIndex
            Dim dataKeyValue As Object = DashboardGridview.DataKeys(rowIndex).Value
            BindNestedGridview(detailsControl, dataKeyValue)

            If btnClientsByCollectorMap IsNot Nothing Then
                ' Set the button to be hidden
                If DashboardType.SelectedValue = "0" Then
                    pnlPluMinus.Visible = True
                    btnClientsByCollectorMap.Visible = False
                    btnRouteOfReceiptsMap.Visible = True
                ElseIf DashboardType.SelectedValue = "1" Then
                    pnlPluMinus.Visible = False
                    btnClientsByCollectorMap.Visible = False
                    btnRouteOfReceiptsMap.Visible = False
                End If
            End If
        End If
    End Sub
    Protected Sub DashboardGridview_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub


    Protected Sub DashboardGridview_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        ' Handle the PageIndexChanging event here
        DashboardGridview.PageIndex = e.NewPageIndex
        ReBind()
    End Sub
    Private Sub SubmitButton_Click(sender As Object, e As EventArgs) Handles submitButton.Click
        ReBind()

    End Sub




    Protected Sub DetailsControl_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try

            Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
            'Dim nestedGrid As GridView = DirectCast(DashboardGridview.Rows(rowIndex).FindControl("DetailsControl"), GridView)
            Dim nestedGrid As GridView = CType(sender, GridView)
            Dim row As GridViewRow = nestedGrid.Rows(rowIndex)
            'Dim keyValue As String = nestedGrid.DataKeys(rowIndex).Value.ToString()
            'Dim keyValue As String = DataBinder.Eval(e.Row.DataItem, "Codigo").ToString()
            Dim keyValue As String = nestedGrid.DataKeys(rowIndex).Value.ToString()


            If e.CommandName = "ReceiptLocationMap" Then
                'Dim result As PaginatedResult(Of VentasDto) = 
                Dim sales As List(Of VentasDto) = getReceiptsFromDB()
                Dim d = sales.Where(Function(r) r.Recibo.Contains(keyValue)).Select(Function(r) New With {r.LATITUD, r.LONGITUD}).FirstOrDefault()
                If d IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(d.LATITUD) AndAlso Not String.IsNullOrWhiteSpace(d.LONGITUD) Then
                    Dim lat = d.LATITUD
                    Dim lon = d.LONGITUD
                    Dim linkToMaps = $"https://www.google.com/maps?q={lat},{lon}"
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenNewTabScript", "openLinkInNewTab('" & linkToMaps & "');", True)

                    'Response.Redirect(linkToMaps)
                    'LinkHelper.OpenLinkInNewTab(Me.Page, linkToMaps)
                Else
                    AlertHelper.GenerateAlert("danger", "Coordenadas corruptas", alertPlaceholder)

                End If


            End If


        Catch ex As FormatException
            AlertHelper.GenerateAlert("danger", "Error al convertir el índice de fila.", alertPlaceholder)
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

        Catch ex As IndexOutOfRangeException
            AlertHelper.GenerateAlert("danger", "Índice de fila fuera de rango.", alertPlaceholder)
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

        Catch ex As IOException
            AlertHelper.GenerateAlert("danger", "Error de entrada/salida al procesar el archivo.", alertPlaceholder)
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

        Catch ex As Exception
            AlertHelper.GenerateAlert("danger", "Se produjo un error inesperado: " & ex.Message, alertPlaceholder)
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

        End Try
    End Sub

    Protected Sub DetailsControl_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then

            If DashboardType.SelectedValue = "0" Then
                Dim estadoCell As TableCell = e.Row.Cells(9)
                Dim linkUrl As String = FindMapLink(e.Row.Cells(1).Text)
                ' Create a button to open the link in a new tab
                Dim linkButotnOpenLink As New LinkButton()
                linkButotnOpenLink.CssClass = "btn btn-sm btn-outline-dark"
                linkButotnOpenLink.ToolTip = "Ubicación del recibo"
                linkButotnOpenLink.Attributes("onclick") = "window.open('" & linkUrl & "', '_blank'); return false;"
                Dim icon As New HtmlGenericControl("i")
                icon.Attributes("class") = "bi bi-geo-alt-fill" ' Bootstrap icon class
                linkButotnOpenLink.Controls.Add(icon)

                ' Add the button to the first cell of the row
                e.Row.Cells(0).Controls.Add(linkButotnOpenLink)

                ' Check the value of the "Estado" column
                If estadoCell.Text.ToLower() = "nulo" Then
                    'e.Row.ForeColor = System.Drawing.Color.Red
                    e.Row.CssClass = "table-danger"
                    'e.Row.Font.Strikeout = True
                End If
            ElseIf DashboardType.SelectedValue = "1" Then
            End If

        End If
    End Sub
    Public Sub BindNestedGridview(currentNestedGrid As GridView, keyValue As Integer)
        If DashboardType.SelectedValue = "0" Then
            BindReceiptsDetails(currentNestedGrid, keyValue)
        ElseIf DashboardType.SelectedValue = "1" Then
            'BindClientDetails(currentNestedGrid, keyValue)

            'BindReceiptsByProductDetails(currentNestedGrid, keyValue)

        End If
    End Sub
    Public Function FindMapLink(Id)
        Dim sales As List(Of VentasDto) = getReceiptsFromDB(receiptNumber:=Id)

        Return sales.Where(Function(r) r.Recibo.Contains(Id)).Select(Function(r) New With {.Link = If(r.LATITUD.ToString() IsNot Nothing AndAlso r.LONGITUD.ToString() IsNot Nothing,
               $"https://www.google.com/maps?q={r.LATITUD},{r.LONGITUD}",
               "https://www.google.com/maps")}).FirstOrDefault().Link
    End Function



    Protected Sub Close_Click(sender As Object, e As EventArgs)
        pnlMap.Visible = False
    End Sub
    Public Sub StartDate_OnTextChanged(sender As Object, e As EventArgs) Handles startDate.TextChanged
        CachingHelper.CacheRemove("SalesReceipts")
    End Sub
    Public Sub EndDate_OnTextChanged(sender As Object, e As EventArgs) Handles endDate.TextChanged
        CachingHelper.CacheRemove("SalesReceipts")

    End Sub
    Public Sub CLientCode_OnTextChanged(sender As Object, e As EventArgs) Handles textBoxClientCode.TextChanged
        Dim txt As TextBox = CType(sender, TextBox)
        CachingHelper.CacheRemove("SalesReceipts")
        ' CachingHelper.CacheRemove("ClientsForGridviewsCachedList")


    End Sub


    Public Sub DdlLeader_OnTextChanged(sender As Object, e As EventArgs) Handles ddlLeader.SelectedIndexChanged

        CachingHelper.CacheRemove("SalesReceipts")
        ' CachingHelper.CacheRemove("ClientsForGridviewsCachedList")
        filterData.LeaderCode = ddlLeader.SelectedValue.Trim


    End Sub
    Public Sub CollectorCode_OnTextChanged(sender As Object, e As EventArgs) Handles textBoxCode.TextChanged
        CachingHelper.CacheRemove("SalesReceipts")
        ' CachingHelper.CacheRemove("ClientsForGridviewsCachedList")
        filterData.SalesPersonCode = textBoxCode.Text.Trim


    End Sub
    Public Sub TextBoxNumDoc_OnTextChanged(sender As Object, e As EventArgs) Handles textBoxNumDoc.TextChanged
        CachingHelper.CacheRemove("SalesReceipts")
        filterData.DocumentNumber = textBoxNumDoc.Text.Trim

    End Sub
    Public Sub DdlCompanyalisReceips_OnTextChanged(sender As Object, e As EventArgs) Handles ddlCompany.SelectedIndexChanged
        CachingHelper.CacheRemove("SalesReceipts")
        ' CachingHelper.CacheRemove("ClientsForGridviewsCachedList")
        filterData.CompanyCode = ddlCompany.SelectedValue.Trim


    End Sub
    Public Sub DdlZone_OnTextChanged(sender As Object, e As EventArgs) Handles ddlCity.SelectedIndexChanged
        CachingHelper.CacheRemove("SalesReceipts")
        filterData.ZoneCode = ddlCity.SelectedValue.Trim

        ' CachingHelper.CacheRemove("ClientsForGridviewsCachedList")

    End Sub

    Public Sub DdlValidReceips_OnTextChanged(sender As Object, e As EventArgs) Handles ddlValidReceipts.SelectedIndexChanged
        CachingHelper.CacheRemove("SalesReceipts")
        filterData.ValidReceiptsMark = ddlValidReceipts.SelectedValue.Trim


    End Sub

    Protected Sub lnkbtnPage_Click(sender As Object, e As EventArgs)
        Dim lnkButton As LinkButton = CType(sender, LinkButton)
        Dim selectedPage As Integer = Integer.Parse(lnkButton.Text)
        filterData.PageNumber = selectedPage
        ReBind(selectedPage)
    End Sub

    Protected Sub lnkbtnPrevious_Click(sender As Object, e As EventArgs)
        filterData.PageNumber = filterData.PageNumber - 1
        ReBind(PageNumber - 1)
        'filterData.PageNumber = selectedPage

    End Sub

    Protected Sub lnkbtnNext_Click(sender As Object, e As EventArgs)
        filterData.PageNumber = filterData.PageNumber + 1

        ReBind(PageNumber + 1)
    End Sub


End Class