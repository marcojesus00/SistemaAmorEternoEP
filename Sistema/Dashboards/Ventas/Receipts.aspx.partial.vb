Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI.WebControls
Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.ComponentModel.DataAnnotations

Partial Public Class VentasDashboard
    Inherits System.Web.UI.Page

    Public Class ReportData
        ' Control references
        Public Property EndDate As String
        Public Property StartDate As String
        Public Property LeaderCode As String
        Public Property SalesPersonCode As String
        Public Property ClientCode As String
        Public Property ValidReceiptsMark As String
        Public Property CompanyCode As String
        Public Property ZoneCode As String
        Public Property DocumentNumber As String
        Public Property ServiceId As String
        Public Property PageSize As Integer
        Public Property PageNumber As Integer
        'Private endDateControl As TextBox
        'Private startDateControl As TextBox
        'Private ddlLeaderControl As DropDownList
        'Private textBoxCodeControl As TextBox
        'Private textBoxClientCodeControl As TextBox
        'Private ddlValidReceiptsControl As DropDownList
        'Private ddlCompanyControl As DropDownList
        'Private ddlCityControl As DropDownList
        'Private textBoxNumDocControl As TextBox
        'Private ddlServiceControl As DropDownList

        ' Constructor to initialize control references
        Public Sub New()
            Me.PageNumber = 1
            Me.PageSize = 10
        End Sub

        ' Method to generate a concatenated key for caching
        Public Function GenerateCacheKey() As String
            'UpdatedData(Me)
            Return String.Join("|", EndDate, StartDate, LeaderCode, SalesPersonCode, ClientCode,
                                ValidReceiptsMark, CompanyCode, ZoneCode, DocumentNumber,
                                ServiceId, PageSize.ToString(), PageNumber.ToString())
        End Function
        Public Function GetUpdatedData()
            Return Me
        End Function
        ' Method to update data from UI elements


        ' Method to generate where clause and parameters
        Public Function GetWhereAndParams(Optional selectedPage As Integer = 1, Optional salesman As String = "", Optional receiptNumber As String = "") As whereAndParamsDto
            ' Get updated data from controls
            'UpdatedData(Me)
            Dim currentData As ReportData = Me

            Dim sqlParameters As New List(Of SqlParameter)
            Dim whereClauseList As New List(Of String)()

            Dim whereClause As String = ""

            Dim offset = (selectedPage - 1) * currentData.PageSize

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
            If Not String.IsNullOrEmpty(salesman) Then
                currentData.SalesPersonCode = salesman
            End If
            If Not String.IsNullOrEmpty(receiptNumber) Then
                currentData.DocumentNumber = receiptNumber
            End If

            ' Constant where clause list
            whereClauseList.Add("r.RFECHA <= @End")
            whereClauseList.Add("r.RFECHA >= @Start")
            whereClauseList.Add("r.Por_lempira > 0")

            ' Conditionally where and params
            If Not String.IsNullOrEmpty(currentData.ClientCode) Then
                whereClauseList.Add("r.Codigo_clie like @Client")
                sqlParameters.Add(New SqlParameter("@Client", "%" & currentData.ClientCode & "%"))
            End If

            If Not String.IsNullOrEmpty(currentData.SalesPersonCode) Then
                whereClauseList.Add("r.RCODVEND like @Collector")
                sqlParameters.Add(New SqlParameter("@Collector", "%" & currentData.SalesPersonCode & "%"))
            End If

            If Not String.IsNullOrEmpty(currentData.LeaderCode) Then
                whereClauseList.Add("v.VEND_LIDER like @Leader")
                sqlParameters.Add(New SqlParameter("@Leader", "%" & currentData.LeaderCode & "%"))
            End If

            If Not String.IsNullOrEmpty(currentData.ZoneCode) Then
                whereClauseList.Add("v.vzon_codigo like @City")
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

            If Not String.IsNullOrEmpty(currentData.ServiceId) Then
                whereClauseList.Add("r.ServicioId like @ServiceId")
                sqlParameters.Add(New SqlParameter("@ServiceId", "%" & currentData.ServiceId & "%"))
            End If

            sqlParameters.Add(New SqlParameter("@Start", startDateParam))
            sqlParameters.Add(New SqlParameter("@End", endDateParam))
            sqlParameters.Add(New SqlParameter("@Offset", offset))
            sqlParameters.Add(New SqlParameter("@PageSize", currentData.PageSize))

            If whereClauseList.Count > 0 Then
                whereClause = "WHERE " & String.Join(" AND ", whereClauseList)
            End If

            Return New whereAndParamsDto With {.whereClause = whereClause, .sqlParams = sqlParameters}
        End Function

        ' DTO class for where clause and parameters
        Public Class whereAndParamsDto
            Public Property whereClause As String
            Public Property sqlParams As List(Of SqlParameter)
        End Class
    End Class


    Public Function GetGroupedSalesBySalesmanFromDB(selectedPage)
        Dim fromClause As String = "from recibos r
        LEFT JOIN
            CLIENTESN c ON r.Codigo_clie = c.Codigo_clie COLLATE DATABASE_DEFAULT and r.RCODVEND=c.CL_VENDEDOR
        LEFT JOIN
	        CONTRATON con ON con.Codigo_clie = r.Codigo_clie and con.cont_vended= r.RCODVEND
        LEFT JOIN
            funamor.dbo.VENDEDOR v ON r.RCODVEND = v.Cod_vendedo COLLATE DATABASE_DEFAULT"


        Dim whereClause = filterData.GetWhereAndParams().whereClause

        Dim selectClause = " select r.RCODVEND as Codigo, v.Nombre_vend as Nombre, COUNT(r.Num_doc) as Ventas,sum(r.Por_lempira) as Cobrado"
        Dim groupByClause = "	group by r.RCODVEND, v.Nombre_vend"
        Dim orderByClause = "    order by Ventas desc,v.Nombre_vend  "
        Dim paginationClause = "OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY"
        Dim query = $"
                            {selectClause} {fromClause}  {whereClause} {groupByClause}
                            {orderByClause} 
                            {paginationClause} OPTION(RECOMPILE)
                        "
        Dim queryCount = $"
                    SELECT COUNT(*) AS TotalCount from
                         ( select r.RCODVEND  {fromClause}
                        {whereClause} {groupByClause}) as subquery
                             ;"


        Dim paginated As New PaginatedResult(Of SalesGroupedDto) ' GetFromDb1(Of PaginatedResult(Of SalesBySalesmanDto))(query, queryCOunt:=queryCount)

        Using aeVentasDbContext As New AeVentasDbContext
            aeVentasDbContext.Database.Log = Sub(s) System.Diagnostics.Debug.WriteLine(s)
            Dim parameters As List(Of SqlParameter) = filterData.GetWhereAndParams(selectedPage).sqlParams
            Dim totalCountParams As List(Of SqlParameter) = filterData.GetWhereAndParams(selectedPage).sqlParams ' Clonar los parámetros para la segunda consulta

            Try

                Dim result As List(Of SalesGroupedDto) = aeVentasDbContext.Database.SqlQuery(Of SalesGroupedDto)(
                        query, parameters.ToArray()).ToList()
                Dim toltalC = aeVentasDbContext.Database.SqlQuery(Of Integer)(queryCount, totalCountParams.ToArray()).FirstOrDefault()

                Dim p = New PaginatedResult(Of SalesGroupedDto) With {
                                                            .Data = result,
                                                            .TotalCount = toltalC
                                                        }
                Return p

            Catch ex As SqlException
                Dim msg = "Problema con la base de datos, por favor vuelva a intentarlo : " & ex.Message
                DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
                AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)
            Catch ex As Exception
                DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
                Throw New Exception("Problema al recibir información de la base de datos." & ex.Message, ex)
            End Try
        End Using
        Return paginated
    End Function
    Public Function getReceiptsFromDB(Optional salesMan As String = "", Optional receiptNumber As String = "") As Object

        Dim selectClause As String = ""

        Dim orderByClause As String = ""

        Dim groupByClause As String = ""
        Dim paginationClause = ""
        Dim whereClause As String = filterData.GetWhereAndParams(salesman:=salesMan, receiptNumber:=receiptNumber).whereClause
        Dim fromClause As String = "from recibos r
        LEFT JOIN
            CLIENTESN c ON r.Codigo_clie = c.Codigo_clie COLLATE DATABASE_DEFAULT and r.RCODVEND=c.CL_VENDEDOR
        LEFT JOIN
	        CONTRATON con ON con.Codigo_clie = r.Codigo_clie and con.cont_vended= r.RCODVEND
        LEFT JOIN
            funamor.dbo.VENDEDOR v ON r.RCODVEND = v.Cod_vendedo COLLATE DATABASE_DEFAULT"


        Try


            selectClause = "select top 20  r.Num_doc as Recibo, r.RFECHA as Fecha, r.RCODVEND as VendedorId,
                                    LTRIM(RTRIM(v.Nombre_vend)) AS Vendedor, v.VEND_LIDER as LiderId,
                                    c.Codigo_clie as ClienteId, LTRIM(RTRIM(c.Nombre_clie)) AS Cliente,
                                    r.Por_lempira, con.CONT_SERVI ServicioId,
	                                con.SERVI1DES as Servicio,
                                    ISNULL(r.SALDOANT, 0) as Valor,
	                                ISNULL(con.CONT_PRIMA, 0) as Prima,
	                                ISNULL(con.CONT_NUMCUO, 0) as NumeroCuotas,
	                                ISNULL(con.CONT_VALCUO, 0) as Cuota,
	                                ISNULL(con.CONT_CANTI , 0) as Cantidad, 
                                    r.MARCA, r.rhora as Hora,
                                    r.liquida, r.liquida2,
                                    ISNULL(c.LATITUD, 0) as LATITUD,
                                    ISNULL(c.LONGITUD , 0) AS LONGITUD"
            orderByClause = "order by RFECHA desc"
            Dim query = String.Format("{0} {1} {2} {3} {4}", selectClause, fromClause, whereClause, groupByClause, orderByClause)
            Dim params = filterData.GetWhereAndParams(salesman:=salesMan, receiptNumber:=receiptNumber).sqlParams



            Using aeVentasDbContext As New AeVentasDbContext()
                aeVentasDbContext.Database.Log = Sub(s) System.Diagnostics.Debug.WriteLine(s)

                aeVentasDbContext.Configuration.AutoDetectChangesEnabled = False
                aeVentasDbContext.Configuration.LazyLoadingEnabled = False

                Dim result As List(Of VentasDto) = aeVentasDbContext.Database.SqlQuery(Of VentasDto)(
                        query, params.ToArray()).ToList()
                Return result

            End Using


        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Throw New Exception("Problema al recibir información de la base de datos.", ex)
        End Try
    End Function
    Public Class PaginatedResult(Of T)
        Public Property Data As List(Of T)
        Public Property TotalCount As Integer
    End Class



    Public Sub RouteOfReceiptsMap(keyValue As String)
        Dim receipts As List(Of VentasDto)
        Dim sales As List(Of VentasDto) = getReceiptsFromDB(salesMan:=keyValue)

        receipts = sales.Where(Function(c) c.VendedorId.Contains(keyValue)).OrderByDescending(Function(r) r.Fecha).ThenBy _
            (Function(r)
                 Dim time As DateTime
                 If DateTime.TryParse(r.Hora, time) Then
                     Return time
                 Else
                     Return DateTime.MinValue ' Default value for invalid time strings
                 End If
             End Function).ToList()
        Dim markers As New List(Of MarkerForMap)
        For Each receipt As VentasDto In receipts
            Dim tooltipMsg = $"<b>Documento:{receipt.Recibo.Trim}</b> <br>Cliente: {receipt.Cliente} <br>Servicio: {receipt.Servicio} <br>Cobrado: {FormattingHelper.ToLempiras(receipt.Prima)} <br>Hora: {receipt.Hora} <br>Fecha: {receipt.Fecha:dd/MM/yyyy}"
            If receipt.LATITUD.ToString().Trim.Length > 0 AndAlso receipt.LONGITUD.ToString().Trim.Length > 0 Then
                Dim marker As New MarkerForMap With {.TooltipMessage = tooltipMsg, .Latitud = receipt.LATITUD, .Longitud = receipt.LONGITUD, .MarkerType = MarkerTypes.Cliente}
                markers.Add(marker)
            End If

        Next

        Dim dataForMaps As New DataForMapGenerator($"Recibos del vendedor {keyValue} del {startDate.Text} al {endDate.Text}", markers, True)
        Session("MarkersData") = dataForMaps
        iMap.Dispose()
        iMap.Src = "../../Shared/Map/Map.aspx"
        pnlMap.Visible = True

    End Sub
    Private Sub BindReceiptsDetails(DetailsControl As GridView, keyValue As String)
        Dim sales As List(Of VentasDto) = getReceiptsFromDB(salesMan:=keyValue)
        If sales IsNot Nothing Then
            Dim result = sales.OrderByDescending(Function(r) r.Fecha).ThenByDescending _
                (Function(e)
                     Dim time As DateTime
                     If DateTime.TryParse(e.Hora, time) Then
                         Return time
                     Else
                         Return DateTime.MinValue ' Default value for invalid time strings
                     End If
                 End Function) _
                .Select(Function(r) New With {.Codigo = r.Recibo, .Cliente = r.Cliente.Trim() + " " + r.ClienteId.Trim(), .Prima = FormattingHelper.ToLempiras(r.Prima), r.Servicio, r.Cantidad, .Valor = FormattingHelper.ToLempiras(r.Valor), .Fecha = r.Fecha.ToString("dd/M/yyyy"), r.Hora, .Estado = FormattingHelper.MarcaToNulo(r.MARCA, r.liquida, r.liquida2)}).ToList()
            DetailsControl.DataSource = result

        Else
            DetailsControl.DataSource = sales
        End If

        DetailsControl.DataBind()
        DetailsControl.Visible = True
    End Sub
    Public Sub RouteOfReceiptsByLeaderMap(sender As Object, e As EventArgs) Handles BtnRouteOfReceiptsMapByLeader.Click
        Dim keyValue = ddlLeader.SelectedValue
        If keyValue = "" Then
            Dim msg = "Seleccione un lider"
            AlertHelper.GenerateAlert("warning", msg, alertPlaceholder)
            Exit Sub
        End If
        Dim receipts As List(Of VentasDto)
        Dim sales As List(Of VentasDto) = getReceiptsFromDB()

        receipts = sales.Where(Function(c) c.LiderId.Contains(keyValue)).OrderByDescending(Function(r) r.Fecha).ThenBy _
            (Function(r)
                 Dim time As DateTime
                 If DateTime.TryParse(r.Hora, time) Then
                     Return time
                 Else
                     Return DateTime.MinValue ' Default value for invalid time strings
                 End If
             End Function).ToList()
        If receipts.Count() < 1 Then
            Dim msg = "No se encontraron recibos para ese lider, intente de nuevo cambiando los filtros y presionando <span class=""text-primary border border-primary px-2 py-1"">Aplicar filtros</span> "
            AlertHelper.GenerateAlert("warning", msg, alertPlaceholder)
            Exit Sub
        End If
        Dim markers As New List(Of MarkerForMap)
        For Each receipt As VentasDto In receipts
            Dim tooltipMsg = $"<b>Vendedor: {receipt.VendedorId} </b> <br>Cliente: {receipt.Cliente} <br>Cobrado: {FormattingHelper.ToLempiras(receipt.Por_lempira)} <br>Fecha:{receipt.Fecha.ToString("dd-MM-yyyy")}"
            If receipt.LATITUD.ToString().Trim.Length > 0 AndAlso receipt.LONGITUD.ToString().Trim.Length > 0 Then
                Dim marker As New MarkerForMap With {.TooltipMessage = tooltipMsg, .Latitud = receipt.LATITUD, .Longitud = receipt.LONGITUD, .MarkerType = MarkerTypes.Cliente}
                markers.Add(marker)
            End If

        Next

        Dim dataForMaps As New DataForMapGenerator($"Recibos del lider {keyValue} del {startDate.Text} al {endDate.Text}", markers, False)
        Session("MarkersData") = dataForMaps
        'Session("BackPageUrl") = thisPage
        iMap.Dispose()
        iMap.Src = "../../Shared/Map/Map.aspx"
        pnlMap.Visible = True

    End Sub

End Class
