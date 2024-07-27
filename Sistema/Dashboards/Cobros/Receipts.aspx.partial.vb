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



    Public Function GetReceiptDataForGridview()
        Dim collectorCode = textBoxCode.Text.Trim
        Dim CompanyCode = ddlCompany.SelectedValue.Trim
        Dim leaderCode As String = ddlLeader.SelectedValue.Trim
        Dim zoneCode As String = ddlCity.SelectedValue.Trim
        Dim ClientCode As String = textBoxClientCode.Text.Trim
        Try

            Dim data1 = ReceiptsByDateCachedList.OrderByDescending(Function(c) c.Por_lempira).ToList()


            Dim groupedData = data1.Where(Function(r) r.Por_lempira.ToString().Trim() <> "" AndAlso r.codigo_cobr IsNot Nothing) _
                .GroupBy(Function(r) r.codigo_cobr).
                 Select(Function(group) New With {
        .Codigo = group.Key,
        .Cobrador = group.FirstOrDefault().nombre_cobr,
        .Recibos = group.Count(),
        .Cobrado = FormattingHelper.ToLempiras(group.Sum(Function(r) r.Por_lempira)),
                .CobradoDecimal = group.Sum(Function(r) r.Por_lempira),
        .Lider = group.FirstOrDefault().cob_lider
    }).OrderByDescending(Function(c) c.CobradoDecimal).Select(Function(r) New With {r.Codigo, r.Cobrador, r.Recibos, r.Cobrado, r.Lider}).ToList()

            Dim dataCount = groupedData.Count()
            Return groupedData

        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

            Throw New Exception(ex.Message & ex.InnerException.Message, ex.InnerException)
        End Try
    End Function

    Public Sub RouteOfReceiptsMap(keyValue As String)
        Dim cobros As New CobrosService()
        Dim receipts As List(Of RecibosDTO) = cobros.GetRecepits(keyValue)
        'ReceiptsByDateCachedList
        'receipts = cachedReceipts.Where(Function(c) c.codigo_cobr.Contains(keyValue)).OrderByDescending(Function(r) r.RFECHA).ThenBy _
        '    (Function(r)
        '         Dim time As DateTime
        '         If DateTime.TryParse(r.rhora, time) Then
        '             Return time
        '         Else
        '             Return DateTime.MinValue ' Default value for invalid time strings
        '         End If
        '     End Function).ToList()
        Dim markers As New List(Of MarkerForMap)
        For Each receipt As RecibosDTO In receipts
            Dim tooltipMsg = $"<b>Documento:{receipt.Num_doc.Trim}</b> <br>Cliente: {receipt.Nombre_clie} <br>Cobrado: {FormattingHelper.ToLempiras(receipt.Por_lempira)} <br>Hora: {receipt.rhora} <br>Fecha: {receipt.RFECHA:dd/MM/yyyy}"
            If receipt.LATITUD.ToString().Trim.Length > 0 AndAlso receipt.LONGITUD.ToString().Trim.Length > 0 Then
                Dim marker As New MarkerForMap With {.TooltipMessage = tooltipMsg, .Latitud = receipt.LATITUD, .Longitud = receipt.LONGITUD, .MarkerType = MarkerTypes.Cliente}
                markers.Add(marker)
            End If

        Next

        Dim dataForMaps As New DataForMapGenerator($"Recibos del cobrador {keyValue} del {startDate.Text} al {endDate.Text}", markers, True)
        Session("MarkersData") = dataForMaps
        'Session("BackPageUrl") = thisPage
        iMap.Dispose()
        iMap.Src = "../../Shared/Map/Map.aspx"
        pnlMap.Visible = True

    End Sub
    Private Sub BindReceiptsDetails(DetailsControl As GridView, keyValue As String)
        Dim lista = ReceiptsByDateCachedList
        Dim d = lista.Where(Function(r) r.codigo_cobr.Contains(keyValue)).OrderByDescending(Function(r) r.RFECHA).ThenByDescending _
            (Function(e)
                 Dim time As DateTime
                 If DateTime.TryParse(e.rhora, time) Then
                     Return time
                 Else
                     Return DateTime.MinValue ' Default value for invalid time strings
                 End If
             End Function) _
            .Select(Function(r) New With {.Codigo = r.Num_doc, .Cliente = r.Nombre_clie + " " + r.Codigo_clie, .Cobrado = FormattingHelper.ToLempiras(r.Por_lempira), .Saldo_anterior = FormattingHelper.ToLempiras(r.SALDOANT), .Fecha = r.RFECHA.ToString("dd/M/yyyy"), .Hora = r.rhora, .Estado = FormattingHelper.MarcaToNulo(r.MARCA, r.liquida, r.liquida2)
}).ToList()
        DetailsControl.DataSource = d
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



            Dim receipts As List(Of RecibosDTO)
            Dim cachedReceipts = ReceiptsByDateCachedList
            receipts = cachedReceipts.Where(Function(c) c.cob_lider.Contains(keyValue)).OrderByDescending(Function(r) r.RFECHA).ThenBy _
            (Function(r)
                 Dim time As DateTime
                 If DateTime.TryParse(r.rhora, time) Then
                     Return time
                 Else
                     Return DateTime.MinValue ' Default value for invalid time strings
                 End If
             End Function).ToList()
            Dim markers As New List(Of MarkerForMap)
            For Each receipt As RecibosDTO In receipts
                Dim tooltipMsg = $"<b>Cobrador: {receipt.codigo_cobr} </b> <br>Cliente: {receipt.Nombre_clie} <br>Cobrado: {FormattingHelper.ToLempiras(receipt.Por_lempira)} <br>Fecha:{receipt.RFECHA.ToString("dd-MM-yyyy")}"
                If receipt.LATITUD.ToString().Trim.Length > 0 AndAlso receipt.LONGITUD.ToString().Trim.Length > 0 Then
                    Dim marker As New MarkerForMap With {.TooltipMessage = tooltipMsg, .Latitud = receipt.LATITUD, .Longitud = receipt.LONGITUD, .MarkerType = MarkerTypes.Cliente}
                    markers.Add(marker)
                End If

            Next

            Dim dataForMaps As New DataForMapGenerator($"Recibos del lider {keyValue} del {startDate.Text} al {endDate.Text}", markers, False)
            Session("MarkersData") = dataForMaps
            'Session("BackPageUrl") = thisPage
        ElseIf DashboardType.SelectedValue = "1" Then
            Dim clients As List(Of PortfolioDetailsDto)
            clients = GetClientsByCollectorIdFromDb(top:="")
            clients = clients.Where(Function(c) c.cob_lider.Contains(keyValue)).OrderByDescending(Function(r) r.Saldo).ToList() '.ThenBy _
            '(Function(r)
            '     Dim time As DateTime
            '     If DateTime.TryParse(r.rhora, time) Then
            '         Return time
            '     Else
            '         Return DateTime.MinValue ' Default value for invalid time strings
            '     End If
            ' End Function).ToList()
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
