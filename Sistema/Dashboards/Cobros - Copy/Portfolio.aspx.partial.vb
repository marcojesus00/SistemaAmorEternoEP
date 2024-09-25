Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI.WebControls
Imports System.Data.Entity
Imports System.Linq

Partial Public Class CobrosDashboard
    Inherits System.Web.UI.Page



    Public Sub BindClientDetails(DetailsControl As GridView, id As String)
        Dim q As List(Of PortfolioDetailsDto) = GetClientsByCollectorIdFromDb(id)

        DetailsControl.DataSource = q
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
        If textBoxClientCode.Text.Trim.Length > 3 OrElse textBoxCode.Text.Trim.Length > 3 Then
            top = ""
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
    cl.Cod_zona AS Empresa, FORMAT( ISNULL(cl.Saldo_actua, 0), 'C', 'es-HN') as Saldo,ISNULL(cl.latitud, 0) as latitud, ISNULL(cl.longitud, 0) as longitud, cr.codigo_cobr, cr.cob_lider "
        Dim fromClause As String = "from COBRADOR cr join CLIENTES cl on cl.cl_cobrador = cr.codigo_cobr"
        Dim whereClauseList As New List(Of String)()
        Dim orderByClause As String = "order by cl.Saldo_actua desc"

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
        Dim clients As List(Of PortfolioDetailsDto) = GetClientsByCollectorIdFromDb(keyValue, "")
        Dim markers As New List(Of MarkerForMap)
        Dim count = 0

        For Each cliente As PortfolioDetailsDto In clients
            Dim tooltipMsg = $"cliente: {cliente.Nombre}   {cliente.Direccion.Replace("'", "")}  deuda: {cliente.Saldo}"
            If cliente.Latitud.ToString().Trim.Length > 0 And cliente.Longitud.ToString().Trim.Length > 0 Then
                Dim marker As New MarkerForMap With {.TooltipMessage = tooltipMsg, .Latitud = cliente.Latitud, .Longitud = cliente.Longitud, .MarkerType = MarkerTypes.Cliente}
                markers.Add(marker)
            End If

        Next
        Dim dataForMaps As New DataForMapGenerator($"Clientes del cobrador {keyValue}", markers, False)
        Session("MarkersData") = dataForMaps
        iMap.Dispose()
        iMap.Src = "../../Shared/Map/Map.aspx"
        pnlMap.Visible = True

    End Sub

End Class
