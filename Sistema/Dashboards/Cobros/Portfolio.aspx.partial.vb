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
        Dim collectorCode = textBoxCode.Text.Trim
        Dim CompanyCode = ddlCompany.SelectedValue.Trim
        Dim leaderCode As String = ddlLeader.SelectedValue.Trim
        Dim cityCode As String = ddlCity.SelectedValue.Trim
        Dim collectorList As List(Of SimpleCollectorDto) = CollectorsCachedList
        Dim ClientsList As List(Of Cliente)
        Dim ClientsQuery As IQueryable(Of Cliente)
        Dim groupedData As List(Of CollectorData)
        Dim finalData
        Dim cacheKey As String = collectorCode & CompanyCode & leaderCode & cityCode
        Using funamorContext As New FunamorContext
            If leaderCode.Length > 0 Then
                collectorList = collectorList.Where(Function(c) c.CodigoDeLider.Contains(leaderCode)).ToList()

            End If
            ClientsQuery = funamorContext.Clientes.Where(Function(c) c.CodigoCobrador.Contains(collectorCode) And c.SaldoActual > 0)


            If CompanyCode.Length > 0 Then
                ClientsQuery = ClientsQuery.Where(Function(c) c.CodigoZona IsNot Nothing AndAlso c.CodigoZona.Contains(CompanyCode))
            End If
            If cityCode.Length > 0 Then
                ClientsQuery = ClientsQuery.Where(Function(c) c.CodigoVZ IsNot Nothing AndAlso c.CodigoVZ.Contains(cityCode))
            End If
            If cityCode.Length > 0 Then
                ClientsQuery = ClientsQuery.Where(Function(c) c.CodigoVZ IsNot Nothing AndAlso c.CodigoVZ.Contains(cityCode))
            End If


            ClientsList = ClientsQuery.ToList()
            groupedData = collectorList _
        .GroupJoin(ClientsList,
                   Function(collector) collector.Codigo,
                   Function(client) client.CodigoCobrador,
                   Function(collector, clients) New CollectorData With {
                   .Codigo = collector.Codigo,
                   .Nombre = collector.Nombre,
                   .Saldo = clients.Sum(Function(s) s.SaldoActual),
                   .SaldoFormatted = FormattingHelper.ToLempiras(clients.Sum(Function(s) s.SaldoActual).ToString())}).OrderByDescending(Function(f) f.Saldo).ToList()


            finalData = groupedData.Select(Function(item) New With {
                   .Codigo = item.Codigo,
                   .Nombre = item.Nombre,
                   .Saldo = item.SaldoFormatted
               }).ToList()
            'For Each item In data
            '    item.Saldo = FormattingHelper.ToLempiras(item.Saldo)
            '    'item.Cobrado = FormattingHelper.ToLempiras(item.Cobrado)

            'Next
        End Using

        Return finalData



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
        Response.Redirect("~/shared/Map/Map.aspx")

    End Sub
    Public Class CollectorData
        Public Property Codigo As String
        Public Property Nombre As String
        Public Property Saldo As Decimal
        Public Property SaldoFormatted As String
    End Class
    Public Class PortfolioDto
        Public Property Codigo As String
        Public Property Nombre As String
        Public Property Clientes As Integer
        Public Property Saldo As String
    End Class
    Public Function GetClientsByCollectorIdFromDb(Optional ByVal rowValue = "")
        Dim collectorCode
        If rowValue = "" Then
            collectorCode = textBoxCode.Text.Trim
        Else
            collectorCode = rowValue
        End If
        Using context As New FunamorContext, cobrosContext As New AeCobrosContext
            Return context.Clientes.Where(Function(c) c.CodigoCobrador.Contains(collectorCode) And c.SaldoActual > 0).OrderByDescending(Function(c) c.SaldoActual).ToList()
        End Using
    End Function

End Class
