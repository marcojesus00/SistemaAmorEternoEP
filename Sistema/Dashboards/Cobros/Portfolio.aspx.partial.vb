Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI.WebControls
Imports System.Data.Entity
Imports System.Linq

Partial Public Class CobrosDashboard
    Inherits System.Web.UI.Page
    Public Function GetPortfolioDataForGridview()

        Dim portfolioList As List(Of PortfolioIDto) = GetGroupedClientsByCollectorFromDb()
        Dim finalList As List(Of PortfolioFinalDto) = portfolioList.Select(Function(c) New PortfolioFinalDto With {.Codigo = c.Codigo, .Nombre = c.nombre_cobr, .Clientes = c.Clientes, .Cartera = FormattingHelper.ToLempiras(CType(c.Cartera, Decimal?))}).ToList()

        Return finalList

    End Function
    Public Function GetGroupedClientsByCollectorFromDb(Optional specificQuery As Boolean = True) As List(Of PortfolioIDto)
        Dim endD = endDate.Text
        Dim initD = startDate.Text

        Using funamorContext As New FunamorContext
            funamorContext.Database.Log = Sub(s) System.Diagnostics.Debug.WriteLine(s)
            Dim collectorCode = textBoxCode.Text.Trim
            Dim ClientCode = textBoxClientCode.Text
            Dim LeaderCode = ddlLeader.SelectedValue.Trim
            Dim companyCode = ddlCompany.SelectedValue.Trim
            Dim ZoneCode = ddlCity.SelectedValue.Trim
            Dim clientIdentification = textBoxNumDoc.Text.Trim
            '    query = "
            '    select cr.codigo_cobr AS Codigo, cr.nombre_cobr, count(cl.Codigo_clie) as Clientes, sum(cl.Saldo_actua) as Cartera from COBRADOR cr join CLIENTES cl 
            '     on cl.cl_cobrador = cr.codigo_cobr
            '    WHERE  cl.Codigo_clie like @client  AND cr.codigo_cobr like @Collector AND  cr.cob_lider like @Leader AND cl.Cod_zona like @Company AND cl.VZCODIGO like @City
            '    AND cl.Saldo_actua>0 AND cl.identidad like @Document
            '    group by cr.codigo_cobr, nombre_cobr
            '    order by Cartera desc
            '"
            Dim selectClause As String = "select cr.codigo_cobr AS Codigo, cr.nombre_cobr, count(cl.Codigo_clie) as Clientes, sum(cl.Saldo_actua) as Cartera "
            Dim fromClause As String = "from COBRADOR cr join CLIENTES cl 
                 on cl.cl_cobrador = cr.codigo_cobr"
            Dim whereClauseList As New List(Of String)()

            Dim orderByClause As String = "order by Cartera desc"
            Dim groupByClause As String = "group by cr.codigo_cobr, nombre_cobr"
            whereClauseList.Add("cl.Saldo_actua > 0")
            If Not String.IsNullOrEmpty(ClientCode) Then
                whereClauseList.Add("cl.Codigo_clie like @Client")
            End If

            If Not String.IsNullOrEmpty(collectorCode) Then
                whereClauseList.Add("cr.codigo_cobr like @Collector")
            End If

            If Not String.IsNullOrEmpty(LeaderCode) Then
                whereClauseList.Add("cr.cob_lider like @Leader")
            End If

            If Not String.IsNullOrEmpty(companyCode) Then
                whereClauseList.Add("cl.Cod_zona like @Company")
            End If

            If Not String.IsNullOrEmpty(ZoneCode) Then
                whereClauseList.Add("cl.VZCODIGO like @City")
            End If

            If Not String.IsNullOrEmpty(clientIdentification) Then
                whereClauseList.Add("REPLACE(cl.identidad, '-', '') LIKE @Document")
            End If

            Dim whereClause As String = ""
            If whereClauseList.Count > 0 Then
                whereClause = "WHERE " & String.Join(" AND ", whereClauseList)
            End If
            Dim query As String = String.Format("{0} {1} {2} {3} {4}", selectClause, fromClause, whereClause, groupByClause, orderByClause)
            Try

                Return GetFromDb(Of PortfolioIDto)(query, collectorCode, ClientCode, clientIdentification, companyCode, ZoneCode, LeaderCode)
            Catch ex As Exception
                ' Handle any other exceptions
                Throw New Exception("Problema al recibir información de la base de datos.", ex)
            End Try
        End Using
    End Function


    Public Sub BindClientDetails(DetailsControl As GridView, id As String)
        Dim q As List(Of PortfolioDetailsDto) = GetClientsByCollectorIdFromDb(id)
        Dim d = q.Select(Function(c) New With {
                                                                                                      c.Codigo,
                                                                                                      c.Nombre,
                                                                                                      c.Identidad,
                                                                                                      c.Telefonos,
                                                                                                      c.Direccion,
                                                                                                      c.Empresa,
                                                                                                      .Saldo = FormattingHelper.ToLempiras(c.Saldo)}).ToList()
        DetailsControl.DataSource = d
        DetailsControl.DataBind()
        DetailsControl.Visible = True
    End Sub


    Public Function GetClientsByCollectorIdFromDb(Optional ByVal rowValue = "")
        Dim collectorCode
        Dim clientCode = textBoxClientCode.Text.Trim
        If rowValue = "" Then
            collectorCode = textBoxCode.Text.Trim
        Else
            collectorCode = rowValue
        End If

        Dim companyCode = ddlCompany.SelectedValue.Trim
        Dim ZoneCode = ddlCity.SelectedValue.Trim
        Dim leaderCode = ddlLeader.SelectedValue.Trim
        Dim clientIdentification = textBoxNumDoc.Text.Trim
        'Using context As New FunamorContext
        '    Return context.Clientes.Where(Function(c) c.Codigo.Contains(clientCode) AndAlso c.CodigoCobrador IsNot Nothing AndAlso c.CodigoCobrador.Contains(collectorCode) AndAlso c.SaldoActual IsNot Nothing AndAlso c.SaldoActual > 0).OrderByDescending(Function(c) c.SaldoActual).ToList()
        'End Using
        Dim selectClause As String = "select top 1 cl.Codigo_clie as Codigo, cl.Nombre_clie as Nombre,
	REPLACE(cl.identidad, '-', '') as Identidad,
    CASE 
        WHEN LTRIM(RTRIM(cl.Telef_clien)) <> '' AND LTRIM(RTRIM(cl.CL_CELULAR)) <> '' THEN LTRIM(RTRIM(cl.Telef_clien)) + ', ' + LTRIM(RTRIM(cl.CL_CELULAR))
        WHEN LTRIM(RTRIM(cl.Telef_clien)) <> '' THEN LTRIM(RTRIM(cl.Telef_clien))
        WHEN LTRIM(RTRIM(cl.CL_CELULAR)) <> '' THEN LTRIM(RTRIM(cl.CL_CELULAR))
        ELSE ''
    END AS Telefonos, 
     LTRIM(RTRIM(cl.Dir_cliente))  + ' ' +  LTRIM(RTRIM(cl.Dir2_client)) AS Direccion, 
    cl.Cod_zona AS Empresa,  ISNULL(cl.latitud, 0) as Saldo,ISNULL(cl.latitud, 0), ISNULL(cl.longitud, 0) "
        Dim fromClause As String = "from COBRADOR cr join CLIENTES cl on cl.cl_cobrador = cr.codigo_cobr"
        Dim whereClauseList As New List(Of String)()

        'Dim whereClause As String = "WHERE cl.Saldo_actua > 0 AND cl.Codigo_clie like @Client AND cr.codigo_cobr like @Collector AND cr.cob_lider like @Leader AND cl.Cod_zona like @Company AND cl.VZCODIGO like @City AND REPLACE(cl.identidad, '-', '') LIKE @ClientIdentification"
        Dim orderByClause As String = "order by Saldo desc"

        whereClauseList.Add("cl.Saldo_actua > 0")
        If Not String.IsNullOrEmpty(clientCode) Then
            whereClauseList.Add("cl.Codigo_clie like @Client")
        End If

        If Not String.IsNullOrEmpty(collectorCode) Then
            whereClauseList.Add("cr.codigo_cobr like @Collector")
        End If

        If Not String.IsNullOrEmpty(leaderCode) Then
            whereClauseList.Add("cr.cob_lider like @Leader")
        End If

        If Not String.IsNullOrEmpty(companyCode) Then
            whereClauseList.Add("cl.Cod_zona like @Company")
        End If

        If Not String.IsNullOrEmpty(ZoneCode) Then
            whereClauseList.Add("cl.VZCODIGO like @City")
        End If

        If Not String.IsNullOrEmpty(clientIdentification) Then
            whereClauseList.Add("REPLACE(cl.identidad, '-', '') LIKE @Document")
        End If

        Dim whereClause As String = ""
        If whereClauseList.Count > 0 Then
            whereClause = "WHERE " & String.Join(" AND ", whereClauseList)
        End If
        Dim query As String = String.Format("{0} {1} {2} {3}", selectClause, fromClause, whereClause, orderByClause)

        Return GetFromDb(Of PortfolioDetailsDto)(query, collectorCode, clientCode, clientIdentification, companyCode, ZoneCode, leaderCode)
    End Function
    Public Sub ClientsByCollectorMap(keyValue As String)
        Dim clients As List(Of Cliente) = ClientsContainsCollectorCachedList.Where(Function(c) c.CodigoCobrador.Contains(keyValue)).ToList()
        Dim markers As New List(Of MarkerForMap)
        Dim count = 0
        'clients = clients.Skip(300).ToList()
        For Each cliente As Cliente In clients
            Dim tooltipMsg = $"cliente: {cliente.Nombre}   {cliente.DireccionCliente}  deuda: {cliente.SaldoActual}"
            If cliente.Latitud.ToString().Trim.Length > 0 And cliente.Longitud.ToString().Trim.Length > 0 Then
                Dim marker As New MarkerForMap With {.TooltipMessage = tooltipMsg, .Latitud = cliente.Latitud, .Longitud = cliente.Longitud, .MarkerType = MarkerTypes.Cliente}
                markers.Add(marker)
            End If

        Next

        Dim dataForMaps As New DataForMapGenerator($"Clientes del cobrador {keyValue}", markers, False)
        Session("MarkersData") = dataForMaps
        iMap.Dispose()
        iMap.Src = "/shared/Map/Map.aspx"
        iMap.Dispose()
        'Response.Redirect("~/shared/Map/Map.aspx")

    End Sub

End Class
