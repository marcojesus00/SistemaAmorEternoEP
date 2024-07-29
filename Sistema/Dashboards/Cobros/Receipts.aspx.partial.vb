Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI.WebControls
Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.ComponentModel.DataAnnotations

Partial Public Class CobrosDashboard
    Inherits System.Web.UI.Page

    Public Sub RouteOfReceiptsMap(keyValue As String)
        Dim cobros As New CobrosService()
        Dim controlsData As CobrosParams = UpdatedData()
        controlsData.SalesPersonCode = keyValue
        Dim receipts As List(Of ReciboDTO) = cobros.GetRecepits(GetParams(controlsData))


        Dim markers As New List(Of MarkerForMap)
        For Each receipt As ReciboDTO In receipts
            Dim tooltipMsg = $"<b>Documento:{receipt.Codigo.Trim}</b> <br>Cliente: {receipt.Cliente} <br>Cobrado: {receipt.Cobrado} <br>Hora: {receipt.Hora} <br>Fecha: {receipt.Fecha}"
            If receipt.LATITUD.ToString().Trim.Length > 0 AndAlso receipt.LONGITUD.ToString().Trim.Length > 0 Then
                Dim marker As New MarkerForMap With {.TooltipMessage = tooltipMsg, .Latitud = receipt.LATITUD, .Longitud = receipt.LONGITUD, .MarkerType = MarkerTypes.Cliente}
                markers.Add(marker)
            End If

        Next

        Dim dataForMaps As New DataForMapGenerator($"Recibos del cobrador {keyValue} del {startDate.Text} al {endDate.Text}", markers, True)
        Session("MarkersData") = dataForMaps
        iMap.Dispose()
        iMap.Src = "../../Shared/Map/Map.aspx"
        pnlMap.Visible = True

    End Sub
    Private Sub BindReceiptsDetails(DetailsControl As GridView, keyValue As String)

        Dim cobros As New CobrosService()
        Dim controlsData As CobrosParams = UpdatedData()
        controlsData.SalesPersonCode = keyValue
        Dim receipts As List(Of ReciboDTO) = cobros.GetRecepits(GetParams(controlsData))
        Dim re = receipts.Select(Function(r) New With {r.Codigo, r.Cliente, r.Codigo_cliente, r.Cobrado, r.Fecha, r.Hora, r.Saldo_actual, r.Saldo_anterior, r.Empresa, .Estado = FormattingHelper.MarcaToNulo(r.MARCA, r.liquida, r.liquida2)}).ToList()
        DetailsControl.DataSource = re
        DetailsControl.DataBind()
        DetailsControl.Visible = True
    End Sub
    Public Sub RouteOfReceiptsByLeaderMap(sender As Object, e As EventArgs) Handles BtnRouteOfReceiptsMapByLeader.Click
        Dim keyValue = ddlLeader.SelectedValue
        iMap.Dispose()

        If keyValue = "" Then
            Dim msg = "Seleccione un lider"
            AlertHelper.GenerateAlert("warning", msg, alertPlaceholder)
            Exit Sub
        End If
        If DashboardType.SelectedValue = "0" Then



            Dim cobros As New CobrosService()
            Dim controlsData = UpdatedData()
            controlsData.LeaderCode = keyValue
            Dim receipts As List(Of ReciboDTO) = cobros.GetRecepits(GetParams(controlsData))
            Dim markers As New List(Of MarkerForMap)
            For Each receipt As ReciboDTO In receipts
                Dim tooltipMsg = $"<b>Cobrador: {receipt.Codigo} </b> <br>Cliente: {receipt.Cliente} <br>Cobrado: {receipt.Cobrado} <br>Fecha:{receipt.Fecha}"
                If receipt.LATITUD.ToString().Trim.Length > 0 AndAlso receipt.LONGITUD.ToString().Trim.Length > 0 Then
                    Dim marker As New MarkerForMap With {.TooltipMessage = tooltipMsg, .Latitud = receipt.LATITUD, .Longitud = receipt.LONGITUD, .MarkerType = MarkerTypes.Cliente}
                    markers.Add(marker)
                End If

            Next

            Dim dataForMaps As New DataForMapGenerator($"Recibos del lider {keyValue} del {startDate.Text} al {endDate.Text}", markers, False)
            Session("MarkersData") = dataForMaps
        ElseIf DashboardType.SelectedValue = "1" Then
            Dim clients As List(Of PortfolioDetailsDto)
            clients = GetClientsByCollectorIdFromDb(top:="")
            clients = clients.Where(Function(c) c.cob_lider.Contains(keyValue)).OrderByDescending(Function(r) r.Saldo).ToList()
            Dim markers As New List(Of MarkerForMap)
            For Each client As PortfolioDetailsDto In clients
                Dim tooltipMsg = $"<b>Cliente: {client.Nombre}  </b> <br>Saldo: {FormattingHelper.ToLempiras(client.Saldo)} <br>Cobrador:{client.codigo_cobr}"
                If client.Latitud.ToString().Trim.Length > 0 AndAlso client.Longitud.ToString().Trim.Length > 0 Then
                    Dim marker As New MarkerForMap With {.TooltipMessage = tooltipMsg, .Latitud = client.Latitud, .Longitud = client.Longitud, .MarkerType = MarkerTypes.Cliente}
                    markers.Add(marker)
                End If

            Next

            Dim dataForMaps As New DataForMapGenerator($"Recibos del lider {keyValue} del {startDate.Text} al {endDate.Text}", markers, False)
            Session("MarkersData") = dataForMaps
        End If
        iMap.Src = "../../Shared/Map/Map.aspx"
        pnlMap.Visible = True

    End Sub

End Class
