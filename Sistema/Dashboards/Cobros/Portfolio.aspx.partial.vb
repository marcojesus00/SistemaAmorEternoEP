﻿Imports System
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
        Dim finalList As List(Of PortfolioFinalDto) = portfolioList.Select(Function(c) New PortfolioFinalDto With {.Codigo = c.Codigo, .Nombre = c.nombre_cobr, .Clientes = c.Clientes, .Cuota_mensual = FormattingHelper.ToLempiras(CType(c.Cuota_mensual, Decimal?))}).ToList()

        Return finalList

    End Function
    Public Function GetGroupedClientsByCollectorFromDb(Optional orderBy = "Valor") As List(Of PortfolioIDto)
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

            Dim selectClause As String = "select cr.codigo_cobr AS Codigo, cr.nombre_cobr, count(cl.Codigo_clie) as Clientes, sum(cl.Saldo_actua) as Cartera,  sum(cn.CONT_VALCUO) as Cuota_mensual " ' "select cr.codigo_cobr AS Codigo, cr.nombre_cobr, count(cl.Codigo_clie) as Clientes, sum(cl.Saldo_actua) as Cartera "
            Dim fromClause As String = "from COBRADOR cr join CLIENTES cl 
                 on cl.cl_cobrador = cr.codigo_cobr
                 left join CONTRATO cn
				 on cl.Codigo_clie = cn.Codigo_clie"
            Dim whereClauseList As New List(Of String)()

            Dim orderByClause As String = "order by Cuota_mensual  desc"
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
                DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
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


    Public Function GetClientsByCollectorIdFromDb(Optional ByVal rowValue = "", Optional ByVal top = "top 5")
        Dim collectorCode As String
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
        If textBoxClientCode.Text.Trim.Length > 3 Then
            top = "top 50"
        End If

        Dim selectClause As String = $"select {top} cl.Codigo_clie as Codigo, cl.Nombre_clie as Nombre,
	REPLACE(cl.identidad, '-', '') as Identidad,
    CASE 
        WHEN LTRIM(RTRIM(cl.Telef_clien)) <> '' AND LTRIM(RTRIM(cl.CL_CELULAR)) <> '' THEN LTRIM(RTRIM(cl.Telef_clien)) + ', ' + LTRIM(RTRIM(cl.CL_CELULAR))
        WHEN LTRIM(RTRIM(cl.Telef_clien)) <> '' THEN LTRIM(RTRIM(cl.Telef_clien))
        WHEN LTRIM(RTRIM(cl.CL_CELULAR)) <> '' THEN LTRIM(RTRIM(cl.CL_CELULAR))
        ELSE ''
    END AS Telefonos, 
     LTRIM(RTRIM(cl.Dir_cliente))  + ' ' +  LTRIM(RTRIM(cl.Dir2_client)) AS Direccion, 
    cl.Cod_zona AS Empresa,  ISNULL(cl.Saldo_actua, 0) as Saldo,ISNULL(cl.latitud, 0) as latitud, ISNULL(cl.longitud, 0) as longitud, cr.codigo_cobr, cr.cob_lider "
        Dim fromClause As String = "from COBRADOR cr join CLIENTES cl on cl.cl_cobrador = cr.codigo_cobr"
        Dim whereClauseList As New List(Of String)()
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
        Dim clients As List(Of PortfolioDetailsDto) = ClientsContainsCollectorCachedList
        Dim markers As New List(Of MarkerForMap)
        Dim count = 0

        For Each cliente As PortfolioDetailsDto In clients
            Dim tooltipMsg = $"cliente: {cliente.Nombre}   {cliente.Direccion}  deuda: {cliente.Saldo}"
            If cliente.Latitud.ToString().Trim.Length > 0 And cliente.Longitud.ToString().Trim.Length > 0 Then
                Dim marker As New MarkerForMap With {.TooltipMessage = tooltipMsg, .Latitud = cliente.Latitud, .Longitud = cliente.Longitud, .MarkerType = MarkerTypes.Cliente}
                markers.Add(marker)
            End If

        Next
        Dim dataForMaps As New DataForMapGenerator($"Clientes del cobrador {keyValue}", markers, False)
        Session("MarkersData") = dataForMaps
        iMap.Dispose()
        iMap.Src = "../../Shared/Map/Map.aspx"

    End Sub

End Class
