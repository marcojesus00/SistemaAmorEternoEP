﻿Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI.WebControls
Imports System.Data.Entity
Imports System.Linq

Public Class CobrosDashboard
    Inherits System.Web.UI.Page
    'Public Event AlertGenerated As EventHandler(Of AlertEventArgs)
    Private _receipts As List(Of ReciboDeCobro)
    Dim thisPage = "~/Dashboards/Cobros/Cobros.aspx"
    Public PageNumber As Integer = 1
    Public PageSize As Integer = 10
    Dim g As RecibosDTO
    'Public Property ReceiptsByDateCachedList As List(Of RecibosDTO)
    '    Get
    '        Return CachingHelper.GetOrFetch("ReceiptsByDate", AddressOf getCobrosReceiptsFromDB, 100)
    '    End Get
    '    Set(value As List(Of RecibosDTO))
    '        CachingHelper.CacheSet("ReceiptsByDate", value, 100)
    '    End Set
    'End Property
    Public Class whereAndParamsDto
        Public Property whereClause As String
        Public Property sqlParams As List(Of SqlParameter)
    End Class
    Public Property ClientsContainsCollectorCachedList As List(Of PortfolioDetailsDto)
        Get
            Return CachingHelper.GetOrFetch("ClientsWithRemainingBalance", AddressOf GetClientsByCollectorIdFromDb, 150)
        End Get
        Set(value As List(Of PortfolioDetailsDto))
            CachingHelper.CacheSet("ClientsWithRemainingBalance", value, 150)
        End Set
    End Property

    Public Function UpdatedData(Optional receiptNumber = "", Optional salesman = "", Optional pageNumer = 1) As CobrosParams
        Dim currentData As New CobrosParams
        currentData.EndDate = endDate.Text
        currentData.StartDate = startDate.Text
        currentData.LeaderCode = ddlLeader.SelectedValue.Trim
        currentData.SalesPersonCode = textBoxCode.Text.Trim
        currentData.ClientCode = textBoxClientCode.Text
        currentData.ValidReceiptsMark = ddlValidReceipts.SelectedValue
        currentData.CompanyCode = ddlCompany.SelectedValue.Trim
        currentData.ZoneCode = ddlCity.SelectedValue.Trim
        currentData.DocumentNumber = textBoxNumDoc.Text.Trim
        currentData.PageNumber = PageNumber ' Assuming PageNumber and PageSize are set elsewhere
        currentData.PageSize = PageSize
        Return currentData
    End Function
    Public Function GetParams(currentData As CobrosParams) As whereAndParamsDto

        Dim sqlParameters As New List(Of SqlParameter)
        Dim whereClauseList As New List(Of String)()

        Dim whereClause As String = ""

        Dim offset = 10 '(selectedPage - 1) * currentData.PageSize

        Dim startDateParam As DateTime
        Dim endDateParam As DateTime

        If DateTime.TryParse(currentData.StartDate, startDateParam) AndAlso DateTime.TryParse(currentData.EndDate, endDateParam) Then
            startDateParam = startDateParam.Date
            endDateParam = endDateParam.Date.AddDays(1).AddSeconds(-1)
        Else
            startDateParam = DateAndTime.Now().AddDays(-1)
            endDateParam = DateAndTime.Now()
        End If

        ' Parameters passed



        ' Constant where clause list
        whereClauseList.Add("r.RFECHA <= @End")
        whereClauseList.Add("r.RFECHA >= @Start")
        whereClauseList.Add("r.Por_lempira > 0")

        ' Conditionally where and params
        If Not String.IsNullOrEmpty(currentData.CompanyCode) Then
            sqlParameters.Add(New SqlParameter("@Company", currentData.CompanyCode & "%"))
            whereClauseList.Add("r.Num_doc like  @Company")


        End If
        If Not String.IsNullOrEmpty(currentData.ClientCode) Then
            whereClauseList.Add("r.Codigo_clie like @Client")
            sqlParameters.Add(New SqlParameter("@Client", "%" & currentData.ClientCode & "%"))
        End If


        If Not String.IsNullOrEmpty(currentData.SalesPersonCode) Then

            whereClauseList.Add("r.codigo_cobr like @Collector")
            sqlParameters.Add(New SqlParameter("@Collector", "%" & currentData.SalesPersonCode & "%"))


        End If


        If Not String.IsNullOrEmpty(currentData.LeaderCode) Then
            whereClauseList.Add("cb.cob_lider like @Leader")
            sqlParameters.Add(New SqlParameter("@Leader", "%" & currentData.LeaderCode & "%"))
        End If

        If Not String.IsNullOrEmpty(currentData.ZoneCode) Then
            whereClauseList.Add("c.Cod_zona like @City")
            sqlParameters.Add(New SqlParameter("@City", "%" & currentData.ZoneCode & "%"))
        End If

        If Not String.IsNullOrEmpty(currentData.ValidReceiptsMark) Then
            whereClauseList.Add("r.MARCA like @Mark")
            sqlParameters.Add(New SqlParameter("@Mark", "%" & currentData.ValidReceiptsMark & "%"))
        End If

        If Not String.IsNullOrEmpty(currentData.DocumentNumber) Then

            whereClauseList.Add("REPLACE(r.Num_doc, '-', '') LIKE @Document")
            sqlParameters.Add(New SqlParameter("@Document", "%" & currentData.DocumentNumber & "%"))
        End If

        'If Not String.IsNullOrEmpty(currentData.ServiceId) Then
        '    whereClauseList.Add("r.ServicioId like @ServiceId")
        '    sqlParameters.Add(New SqlParameter("@ServiceId", "%" & currentData.ServiceId & "%"))
        'End If

        sqlParameters.Add(New SqlParameter("@Start", startDateParam))
        sqlParameters.Add(New SqlParameter("@End", endDateParam))
        sqlParameters.Add(New SqlParameter("@Offset", offset))
        sqlParameters.Add(New SqlParameter("@PageSize", currentData.PageSize))

        If whereClauseList.Count > 0 Then
            whereClause = "WHERE " & String.Join(" AND ", whereClauseList)
        End If
        Dim result As New whereAndParamsDto
        result.sqlParams = sqlParameters
        result.whereClause = whereClause
        Return result
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim Usuario_Aut = Session("Usuario_Aut")
            Session("BackPageUrl") = "~/monitorcobros.aspx"
            Dim thisPage = "~/Dashboards/Cobros/Cobros.aspx"
            If Usuario_Aut IsNot Nothing Then
                Usuario_Aut = Usuario_Aut.ToString().Trim().ToUpper()
                If Session("Usuario") = "" OrElse Not AuthHelper.isAuthorized(Usuario_Aut, "COBROS_A") Then
                    Response.Redirect("~/Principal.aspx")
                End If

                If Not IsPostBack Then
                    FillDll()
                    ReBind()
                End If
                pnlMap.Visible = False

                AddHandler DashboardGridview.PageIndexChanging, AddressOf DashboardGridview_PageIndexChanging
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

            If DateTime.TryParse(initD, startDateParam) AndAlso DateTime.TryParse(endD, endDateParam) Then
                Dim difference As TimeSpan = endDateParam - startDateParam
                If difference.TotalDays > 7 Then
                    datesTooSpread = True
                End If
            End If
            If DashboardType.SelectedValue = "0" Then

                If (collectorCodeConstraint AndAlso clientCodeConstraint) And datesTooSpread Then
                    Dim msg = "Por favor refine su búsqueda"
                    AlertHelper.GenerateAlert("warning", msg, alertPlaceholder)
                Else
                    Dim cobrosService As New CobrosService()
                    Dim params = GetParams(UpdatedData())
                    Dim DataList As List(Of groupedreceiptDto) = cobrosService.GetRecepitsGrouped(params)
                    endDate.Enabled = True
                    startDate.Enabled = True
                    ddlValidReceipts.Enabled = True
                    'lblNumDoc.Text = "Número de documento"
                    textBoxNumDoc.Attributes("placeholder") = "Número de documento"
                    BtnRouteOfReceiptsMapByLeader.Enabled = True
                    BindGridView(DataList)


                End If
            ElseIf DashboardType.SelectedValue = "1" Then
                startDate.Enabled = False
                endDate.Enabled = False
                ddlValidReceipts.Enabled = False
                'lblNumDoc.Text = "Número de identidad"
                textBoxNumDoc.Attributes("placeholder") = "Número de identidad"

                BtnRouteOfReceiptsMapByLeader.Enabled = True
                DashboardGridview.DataSource = Nothing
                DashboardGridview.DataBind()

                If collectorCodeConstraint AndAlso clientCodeConstraint Then
                    Dim msg = "Por favor refine su búsqueda"
                    AlertHelper.GenerateAlert("warning", msg, alertPlaceholder)

                Else

                End If
                Dim dataList = GetPortfolioDataForGridview()

                BindGridView(dataList)
            End If
        Catch ex As SqlException
            Dim msg = "Problema con la base de datos, por favor vuelva a intentarlo : " & ex.Message
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)


        Catch ex As Exception
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            'RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try
    End Sub

    Private Sub FillDll()
        endDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
        startDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
        Using context As New FunamorContext, ventasContext As New AeVentasDbContext
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
            Dim leaders = context.Cobradores.AsNoTracking().Where(Function(c) c.Codigo = c.CobLider Or c.Codigo.Contains("4894") Or c.Codigo.Contains("3072")) _
                .Select(Function(l) New With {l.Codigo, .Nombre = l.Nombre & " " & l.Codigo}) _
                .OrderBy(Function(l) l.Nombre).ToList()
            ddlLeader.DataSource = leaders
            ddlLeader.DataTextField = "Nombre"
            ddlLeader.DataValueField = "Codigo"
            ddlLeader.DataBind()
            ddlLeader.Items.Insert(0, New ListItem("Todos los líderes", ""))
        End Using
        ddlValidReceipts.Items.Add(New ListItem("Recibos válidos", "N"))
        ddlValidReceipts.Items.Add(New ListItem("Rcibos nulos", "X"))
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
            Dim msg = "Problema con la base de datos, por favor vuelva a intentarlo : " & ex.Message
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)


        Catch ex As Exception
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try
    End Sub



    Function GetDigitalRoot(ByVal number As Integer) As Integer

        Return number Mod 10

    End Function

    Protected Sub DashboardGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        Try

            Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
            Dim digitalRoot = GetDigitalRoot(rowIndex)
            Dim keyValue As String = DashboardGridview.DataKeys(digitalRoot).Value.ToString()

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

            Dim rowIndex As Integer = e.Row.RowIndex
            Dim dataKeyValue As Object = DashboardGridview.DataKeys(rowIndex).Value
            BindNestedGridview(detailsControl, dataKeyValue)

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
    Protected Sub DashboardGridview_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub


    Protected Sub DashboardGridview_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        ' Handle the PageIndexChanging event here
        DashboardGridview.PageIndex = e.NewPageIndex
        ReBind()
    End Sub
    Private Sub SubmitButton_Click(sender As Object, e As EventArgs) Handles submitButton.Click
        CachingHelper.CacheRemove("ReceiptsByDate")

        ReBind()

    End Sub
    Private Sub LinkButtonClear_Click(sender As Object, e As EventArgs) Handles LinkButtonClear.Click
        'ReBind()
        CachingHelper.CacheRemove("ReceiptsByDate")
        clearFilters()
    End Sub
    Private Sub clearFilters()
        endDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
        startDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
        ddlCity.SelectedIndex = 0
        ddlCompany.SelectedIndex = 0
        ddlLeader.SelectedIndex = 0
        ddlValidReceipts.SelectedIndex = 0
        textBoxNumDoc.Text = ""
        textBoxCode.Text = ""
        textBoxClientCode.Text = ""
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
    Public Sub DdlLeader_OnTextChanged(sender As Object, e As EventArgs) Handles ddlLeader.SelectedIndexChanged
        CachingHelper.CacheRemove("ReceiptsByDate")
        CachingHelper.CacheRemove("ClientsForGridviewsCachedList")

    End Sub
    Public Sub CollectorCode_OnTextChanged(sender As Object, e As EventArgs) Handles textBoxCode.TextChanged
        CachingHelper.CacheRemove("ReceiptsByDate")
        CachingHelper.CacheRemove("ClientsForGridviewsCachedList")

    End Sub
    Public Sub TextBoxNumDoc_OnTextChanged(sender As Object, e As EventArgs) Handles textBoxNumDoc.TextChanged
        CachingHelper.CacheRemove("ReceiptsByDate")
    End Sub
    Public Sub DdlCompanyalisReceips_OnTextChanged(sender As Object, e As EventArgs) Handles ddlCompany.SelectedIndexChanged
        CachingHelper.CacheRemove("ReceiptsByDate")
        CachingHelper.CacheRemove("ClientsForGridviewsCachedList")

    End Sub
    Public Sub DdlZone_OnTextChanged(sender As Object, e As EventArgs) Handles ddlCity.SelectedIndexChanged
        CachingHelper.CacheRemove("ReceiptsByDate")
        CachingHelper.CacheRemove("ClientsForGridviewsCachedList")

    End Sub

    Public Sub DdlValidReceips_OnTextChanged(sender As Object, e As EventArgs) Handles ddlValidReceipts.SelectedIndexChanged
        CachingHelper.CacheRemove("ReceiptsByDate")

    End Sub
    Public Function GetFromDb(Of T)(query As String, Optional CollectorCode As String = "", Optional ClientCode As String = "", Optional numDoc As String = "", Optional companyCode As String = "", Optional ZoneCode As String = "", Optional leaderCode As String = "") As List(Of T)
        Dim endD = endDate.Text
        Dim initD = startDate.Text

        Using funamorContext As New FunamorContext
            funamorContext.Database.Log = Sub(s) System.Diagnostics.Debug.WriteLine(s)


            Try
                Dim startDateParam As DateTime
                Dim endDateParam As DateTime

                If DateTime.TryParse(initD, startDateParam) AndAlso DateTime.TryParse(endD, endDateParam) Then
                    startDateParam = startDateParam.Date
                    endDateParam = endDateParam.Date.AddDays(1).AddSeconds(-1)
                    Dim clientCodeParam As String = "%" & ClientCode & "%"
                    Dim collectorCodeParam As String = "%" & CollectorCode & "%"
                    Dim LeaderCodeParam As String = "%" & leaderCode & "%"
                    Dim CompanyCodeParam As String = "%" & companyCode & "%"
                    Dim CityCodeParam As String = "%" & ZoneCode & "%"
                    Dim numDocParam As String = "%" & numDoc & "%"
                    Return funamorContext.Database.SqlQuery(Of T)(
                        query,
                        New SqlParameter("@Leader", LeaderCodeParam),
                        New SqlParameter("@Collector", collectorCodeParam),
                        New SqlParameter("@Client", clientCodeParam),
                       New SqlParameter("@Company", CompanyCodeParam),
                       New SqlParameter("@City", CityCodeParam),
                       New SqlParameter("@start", startDateParam),
                        New SqlParameter("@end", endDateParam),
                        New SqlParameter("@Document", numDocParam)
                    ).ToList()


                Else
                    ' Handle parsing error if needed
                    Throw New ArgumentException("Invalid date format for start or end date.")
                End If
            Catch ex As SqlException
                Dim msg = "Problema con la base de datos, por favor vuelva a intentarlo : " & ex.Message
                DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
                AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)
            Catch ex As Exception
                DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
                Throw New Exception("Problema al recibir información de la base de datos." & ex.Message, ex)
            End Try
        End Using
    End Function

    Protected Sub DetailsControl_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try

            Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
            'Dim nestedGrid As GridView = DirectCast(DashboardGridview.Rows(rowIndex).FindControl("DetailsControl"), GridView)
            Dim nestedGrid As GridView = CType(sender, GridView)
            Dim row As GridViewRow = nestedGrid.Rows(rowIndex)
            'Dim keyValue As String = nestedGrid.DataKeys(rowIndex).Value.ToString()
            'Dim keyValue As String = DataBinder.Eval(e.Row.DataItem, "Codigo").ToString()
            Dim keyValue As String = nestedGrid.DataKeys(rowIndex).Value.ToString()
            Dim controlsData As CobrosParams = UpdatedData()
            controlsData.DocumentNumber = keyValue
            Dim cobros As New CobrosService()
            Dim params = GetParams(controlsData)

            If e.CommandName = "ReceiptLocationMap" Then
                Dim d = cobros.GetRecepits(params).FirstOrDefault()
                'cobroas.getCobrosReceiptsFromDB.Where(Function(r) r.Num_doc.Contains(keyValue)).Select(Function(r) New With {r.LATITUD, r.LONGITUD}).FirstOrDefault()
                If d IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(d.LATITUD) AndAlso Not String.IsNullOrWhiteSpace(d.LONGITUD) Then
                    Dim lat = d.LATITUD.Trim().ToString()
                    Dim lon = d.LONGITUD.Trim().ToString()
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
                Dim estadoCell As TableCell = e.Row.Cells(7)
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
            BindClientDetails(currentNestedGrid, keyValue)


        End If
    End Sub
    Public Function FindMapLink(Id)
        Dim cobrosService As New CobrosService()
        Dim controlsData As CobrosParams = UpdatedData()
        controlsData.DocumentNumber = Id
        Dim receipt As ReciboDTO = cobrosService.GetRecepits(GetParams(controlsData)).FirstOrDefault()
        Return If(receipt.LATITUD IsNot Nothing AndAlso receipt.LONGITUD IsNot Nothing,
               $"https://www.google.com/maps?q={receipt.LATITUD.Trim},{receipt.LONGITUD.Trim}",
               "https://www.google.com/maps")
    End Function



    Protected Sub Close_Click(sender As Object, e As EventArgs)
        pnlMap.Visible = False
    End Sub

End Class