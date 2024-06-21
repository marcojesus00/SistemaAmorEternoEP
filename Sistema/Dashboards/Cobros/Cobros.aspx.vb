Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI.WebControls
Imports System.Data.Entity
Imports System.Linq

Public Class CobrosDashboard
    Inherits System.Web.UI.Page
    Public Event AlertGenerated As EventHandler(Of AlertEventArgs)
    Private _receipts As List(Of ReciboDeCobro)
    'Protected WithEvents btnClientsByCollectorMap As Global.System.Web.UI.WebControls.LinkButton
    'Protected WithEvents BtnRouteOfReceiptsMap As Global.System.Web.UI.WebControls.LinkButton

    Public Property ReceiptsByDateCachedList As List(Of RecibosDTO)
        Get
            Return CachingHelper.GetOrFetch("ReceiptsByDate", AddressOf getReceiptsFromDB, 100)
        End Get
        Set(value As List(Of RecibosDTO))
            CachingHelper.CacheSet("ReceiptsByDate", value, 100)
        End Set
    End Property
    Public Property CollectorsCachedList As List(Of SimpleCollectorDto)
        Get
            Return CachingHelper.GetOrFetch("Collectors", AddressOf GetCollectorsFromDb, 150)
        End Get
        Set(value As List(Of SimpleCollectorDto))
            CachingHelper.CacheSet("Collectors", value, 150)
        End Set
    End Property
    Public Property ClientsContainsCollectorCachedList As List(Of Cliente)
        Get
            Return CachingHelper.GetOrFetch("ClientsWithRemainingBalance", AddressOf GetClientsByCollectorIdFromDb, 150)
        End Get
        Set(value As List(Of Cliente))
            CachingHelper.CacheSet("ClientsWithRemainingBalance", value, 150)
        End Set
    End Property
    Public Property ClientsForGridviewsCachedList As List(Of Cliente)
        Get
            Return CachingHelper.GetOrFetch("ClientsForGridviewsCachedList", AddressOf GetClientsForGridViewFromDb, 150)
        End Get
        Set(value As List(Of Cliente))
            CachingHelper.CacheSet("ClientsForGridviewsCachedList", value, 150)
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim Usuario_Aut = Session("Usuario_Aut")
            If Usuario_Aut IsNot Nothing Then
                Usuario_Aut = Usuario_Aut.ToString().Trim().ToUpper()



                If Session("Usuario") = "" OrElse Not authHelper.isAuthorized(Usuario_Aut, "COBROS_A") Then
                    Response.Redirect("~/Principal.aspx")
                End If

                If Not IsPostBack Then
                    FillDll()
                    ReBind()
                End If
                AddHandler DashboardGridview.PageIndexChanging, AddressOf DashboardGridview_PageIndexChanging
            Else
                Response.Redirect("~/Principal.aspx")

            End If

        Catch ex As Exception
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try
    End Sub
    Protected Sub DashboardType_change(sender As Object, e As EventArgs) Handles DashboardType.SelectedIndexChanged
        ReBind()
    End Sub

    'Fill the gridviews 
    Protected Sub ReBind()
        Try

            Dim almostEmpyFilters = ddlCompany.SelectedValue.Trim = "" AndAlso ddlLeader.SelectedValue.Trim = "" AndAlso ddlCity.SelectedValue.Trim = ""
            Dim collectorCodeConstraint = textBoxCode.Text.Trim.Length <= 2 AndAlso almostEmpyFilters
            Dim clientCodeConstraint = textBoxClientCode.Text.Trim.Length <= 2 AndAlso almostEmpyFilters
            Dim startDateParam As DateTime
            Dim endDateParam As DateTime
            Dim endD = endDate.Text
            Dim initD = startDate.Text
            Dim datesTooSpread = False
            DetailsControl.DataSource = Nothing
            DetailsControl.DataBind()

            If DateTime.TryParse(initD, startDateParam) AndAlso DateTime.TryParse(endD, endDateParam) Then
                If startDateParam.Year <> endDateParam.Year Then
                    datesTooSpread = True
                End If
            End If
            If DashboardType.SelectedValue = "0" Then
                DetailsTitle.Text = "Detalle de los recibos"

                If (collectorCodeConstraint AndAlso clientCodeConstraint) And datesTooSpread Then
                    Dim msg = "Por favor refine su búsqueda"
                    AlertHelper.GenerateAlert("warning", msg, alertPlaceholder)
                Else
                    Dim DataList = GetReceiptDataForGridview()
                    endDate.Enabled = True
                    startDate.Enabled = True
                    BindGridView(DataList)
                    Dim dt As New DataTable()
                    dt.Columns.Add("Recibo")
                    dt.Columns.Add("Cliente")
                    DetailsControl.DataSource = dt
                    DetailsControl.DataBind()

                End If
            ElseIf DashboardType.SelectedValue = "1" Then
                startDate.Enabled = False
                endDate.Enabled = False

                DashboardGridview.DataSource = Nothing
                DashboardGridview.DataBind()
                DetailsTitle.Text = "Clientes"

                If collectorCodeConstraint AndAlso clientCodeConstraint Then
                    Dim msg = "Por favor refine su búsqueda"
                    AlertHelper.GenerateAlert("warning", msg, alertPlaceholder)

                Else

                End If
                Dim dataList = GetPortfolioDataForGridview()

                BindGridView(dataList)
            End If
        Catch ex As SqlException
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)


        Catch ex As Exception
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try
    End Sub

    Private Sub FillDll()
        endDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
        startDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
        Using context As New FunamorContext, ventasContext As New AeVentasDbContext
            Dim companies = context.Empresas.Where(Function(e) e.Codigo IsNot Nothing AndAlso e.Codigo.Trim().Length > 0).Select(Function(c) New With {c.Codigo, c.Nombre}).ToList()
            ddlCompany.DataSource = companies
            ddlCompany.DataTextField = "Nombre"
            ddlCompany.DataValueField = "Codigo"
            ddlCompany.DataBind()
            ddlCompany.Items.Insert(0, New ListItem("Todas las empresas", ""))
            Dim zones = ventasContext.MunicipiosZonasDepartamentos.
                Select(Function(c) New With {.Nombre = c.NombreMunicipio & " " & c.ZonaId, .Codigo = c.MunicipioId}) _
                .OrderBy(Function(z) z.Nombre).ToList()
            ddlCity.DataSource = zones
            ddlCity.DataTextField = "Nombre"
            ddlCity.DataValueField = "Codigo"
            ddlCity.DataBind()
            ddlCity.Items.Insert(0, New ListItem("Todas las zonas", ""))
            Dim leaders = context.Cobradores.Where(Function(c) c.Codigo = c.CobLider Or c.Codigo.Contains("4894")) _
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

    Private Sub BindGridView(dataList As Object)


        Try

            Dim msg = ""
            DashboardGridview.DataSource = dataList
            DashboardGridview.DataBind()
            If DashboardGridview.Rows.Count = 0 Then
                msg = "No se encontraron resultados"
            Else
                msg = "Mostrando primeros " & $"{dataList.Count} resultados."

            End If
        Catch ex As SqlException
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)


        Catch ex As Exception
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try
    End Sub




    Protected Sub DashboardGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        Try

            Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
            Dim keyValue As String = DashboardGridview.DataKeys(rowIndex).Value.ToString()

            If e.CommandName = "ClientsByCollectorMap" Then
                ClientsByCollectorMap(keyValue)
            ElseIf e.CommandName = "RouteOfReceiptsMap" Then
                If textBoxClientCode.Text.Length > 0 Then
                    textBoxClientCode.Text = ""

                End If
                RouteOfReceiptsMap(keyValue)
            End If


        Catch ex As FormatException
            AlertHelper.GenerateAlert("danger", "Error al convertir el índice de fila.", alertPlaceholder)
        Catch ex As IndexOutOfRangeException
            AlertHelper.GenerateAlert("danger", "Índice de fila fuera de rango.", alertPlaceholder)
        Catch ex As IOException
            AlertHelper.GenerateAlert("danger", "Error de entrada/salida al procesar el archivo.", alertPlaceholder)
        Catch ex As Exception
            AlertHelper.GenerateAlert("danger", "Se produjo un error inesperado: " & ex.Message, alertPlaceholder)
        End Try
    End Sub
    Protected Sub DashboardGridView_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim btnClientsByCollectorMap As LinkButton = CType(e.Row.FindControl("btnClientsByCollectorMap"), LinkButton)
            Dim btnRouteOfReceiptsMap As LinkButton = CType(e.Row.FindControl("btnRouteOfReceiptsMap"), LinkButton)

            If btnClientsByCollectorMap IsNot Nothing Then
                ' Set the button to be hidden
                If DashboardType.SelectedValue = "0" Then
                    btnClientsByCollectorMap.Visible = False
                    btnRouteOfReceiptsMap.Visible = True
                ElseIf DashboardType.SelectedValue = "1" Then
                    btnClientsByCollectorMap.Visible = True
                    btnRouteOfReceiptsMap.Visible = False
                End If
            End If
        End If
    End Sub
    Protected Sub SellerGridView_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim selectedRowIndex As Integer = DashboardGridview.SelectedIndex
        Dim keyValue As String = DashboardGridview.DataKeys(selectedRowIndex).Value.ToString()
        If DashboardType.SelectedValue = "0" Then
            DetailsTitle.Text = $"Detalle de los recibos del cobrador {keyValue}"

            BindReceiptsDetails(keyValue)
        ElseIf DashboardType.SelectedValue = "1" Then
            DetailsTitle.Text = $"Detalle de los clientes del cobrador {keyValue}"
            BindClientDetails(keyValue)


        End If
    End Sub
    Protected Sub DetailsControl_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim btnReceiptLocation As LinkButton = CType(e.Row.FindControl("btnReceiptLocation"), LinkButton)

            If DashboardType.SelectedValue = "0" Then
                btnReceiptLocation.Visible = True
                Dim estadoCell As TableCell = e.Row.Cells(7)

                ' Check the value of the "Estado" column
                If estadoCell.Text.ToLower() = "nulo" Then
                    'e.Row.ForeColor = System.Drawing.Color.Red
                    e.Row.CssClass = "table-danger"
                    'e.Row.Font.Strikeout = True
                End If
            ElseIf DashboardType.SelectedValue = "1" Then
                btnReceiptLocation.Visible = False
            End If

        End If
    End Sub


    Protected Sub DashboardGridview_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        ' Handle the PageIndexChanging event here
        DashboardGridview.PageIndex = e.NewPageIndex
        ReBind()
    End Sub
    Private Sub submitButton_Click(sender As Object, e As EventArgs) Handles submitButton.Click
        ReBind()

    End Sub
    Public Sub StartDate_OnTextChanged(sender As Object, e As EventArgs) Handles startDate.TextChanged
        CachingHelper.CacheRemove("ReceiptsByDate")
    End Sub
    Public Sub EndDate_OnTextChanged(sender As Object, e As EventArgs) Handles endDate.TextChanged
        CachingHelper.CacheRemove("ReceiptsByDate")

    End Sub
    Public Sub CLientCode_OnTextChanged(sender As Object, e As EventArgs) Handles textBoxClientCode.TextChanged
        CachingHelper.CacheRemove("ReceiptsByDate")
        CachingHelper.CacheRemove("ClientsForGridviewsCachedList")


    End Sub
    Public Sub ddlLeader_OnTextChanged(sender As Object, e As EventArgs) Handles ddlLeader.SelectedIndexChanged
        CachingHelper.CacheRemove("ReceiptsByDate")
        CachingHelper.CacheRemove("ClientsForGridviewsCachedList")

    End Sub
    Public Sub CollectorCode_OnTextChanged(sender As Object, e As EventArgs) Handles textBoxCode.TextChanged
        CachingHelper.CacheRemove("ReceiptsByDate")
        CachingHelper.CacheRemove("ClientsForGridviewsCachedList")

    End Sub
    Public Sub textBoxNumDoc_OnTextChanged(sender As Object, e As EventArgs) Handles textBoxNumDoc.TextChanged
        CachingHelper.CacheRemove("ReceiptsByDate")
        CachingHelper.CacheRemove("ClientsForGridviewsCachedList")
    End Sub
    Public Sub ddlCompanyalisReceips_OnTextChanged(sender As Object, e As EventArgs) Handles ddlCompany.SelectedIndexChanged
        CachingHelper.CacheRemove("ReceiptsByDate")
        CachingHelper.CacheRemove("ClientsForGridviewsCachedList")

    End Sub
    Public Sub ddlZone_OnTextChanged(sender As Object, e As EventArgs) Handles ddlCity.SelectedIndexChanged
        CachingHelper.CacheRemove("ReceiptsByDate")
        CachingHelper.CacheRemove("ClientsForGridviewsCachedList")

    End Sub

    Public Sub ddlValisReceips_OnTextChanged(sender As Object, e As EventArgs) Handles ddlValidReceipts.SelectedIndexChanged
        CachingHelper.CacheRemove("ReceiptsByDate")
        CachingHelper.CacheRemove("ClientsForGridviewsCachedList")

    End Sub
    Public Function GetFromDb(Of T)(query As String, CollectorCode As String, Optional specificQuery As Boolean = True) As List(Of T)
        Dim endD = endDate.Text
        Dim initD = startDate.Text
        Dim ClientCode = ""
        Dim companyCode = ""
        Dim ZoneCode = ""
        Dim leaderCode = ""
        Using funamorContext As New FunamorContext
            funamorContext.Database.Log = Sub(s) System.Diagnostics.Debug.WriteLine(s)
            If specificQuery Then
                collectorCode = textBoxCode.Text.Trim
                ClientCode = textBoxClientCode.Text
                companyCode = ddlCompany.SelectedValue.Trim
                ZoneCode = ddlCity.SelectedValue.Trim
                leaderCode = ddlLeader.SelectedValue.Trim
            Else

            End If

            Try
                Dim startDateParam As DateTime
                Dim endDateParam As DateTime

                If DateTime.TryParse(initD, startDateParam) AndAlso DateTime.TryParse(endD, endDateParam) Then
                    startDateParam = startDateParam.Date
                    endDateParam = endDateParam.Date.AddDays(1).AddSeconds(-1)
                    Dim clientCodeParam As String = "%" & ClientCode & "%"
                    Dim collectorCodeParam As String = "%" & collectorCode & "%"
                    Dim LeaderCodeParam As String = "%" & LeaderCode & "%"
                    Dim CompanyCodeParam As String = "%" & companyCode & "%"
                    Dim CityCodeParam As String = "%" & ZoneCode & "%"

                    Return funamorContext.Database.SqlQuery(Of T)(
                        query,
                        New SqlParameter("@Leader", LeaderCodeParam),
                        New SqlParameter("@Collector", collectorCodeParam),
                        New SqlParameter("@Client", clientCodeParam),
                       New SqlParameter("@Company", CompanyCodeParam),
                       New SqlParameter("@City", CityCodeParam),
                       New SqlParameter("@start", startDateParam),
                        New SqlParameter("@end", endDateParam)
                    ).ToList()


                Else
                    ' Handle parsing error if needed
                    Throw New ArgumentException("Invalid date format for start or end date.")
                End If
            Catch ex As Exception
                ' Handle any other exceptions
                Throw New Exception("Error retrieving receipts from database.", ex)
            End Try
        End Using
    End Function

    Protected Sub DetailsControl_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try

            Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
            Dim s = DetailsControl
            Dim keyValue As String = DetailsControl.DataKeys(rowIndex).Value.ToString()

            If e.CommandName = "ReceiptLocationMap" Then
                Dim d = ReceiptsByDateCachedList.Where(Function(r) r.Num_doc.Contains(keyValue)).Select(Function(r) New With {r.LATITUD, r.LONGITUD}).FirstOrDefault()
                If d IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(d.LATITUD) AndAlso Not String.IsNullOrWhiteSpace(d.LONGITUD) Then
                    Dim lat = d.LATITUD.Trim().ToString()
                    Dim lon = d.LONGITUD.Trim().ToString()
                    Dim linkToMaps = $"https://www.google.com/maps?q={lat},{lon}"
                    Response.Redirect(linkToMaps)
                Else
                    AlertHelper.GenerateAlert("danger", "Coordenadas corruptas", alertPlaceholder)

                End If


            End If


        Catch ex As FormatException
            AlertHelper.GenerateAlert("danger", "Error al convertir el índice de fila.", alertPlaceholder)
        Catch ex As IndexOutOfRangeException
            AlertHelper.GenerateAlert("danger", "Índice de fila fuera de rango.", alertPlaceholder)
        Catch ex As IOException
            AlertHelper.GenerateAlert("danger", "Error de entrada/salida al procesar el archivo.", alertPlaceholder)
        Catch ex As Exception
            AlertHelper.GenerateAlert("danger", "Se produjo un error inesperado: " & ex.Message, alertPlaceholder)
        End Try
    End Sub
End Class