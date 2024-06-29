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

    Public Function getReceiptsFromDB(Optional specificQuery As Boolean = True) As Object
        Dim endD = endDate.Text
        Dim initD = startDate.Text

        Dim leaderCode = ddlLeader.SelectedValue.Trim
        Dim salesPersonCode = textBoxCode.Text.Trim
        Dim ClientCode = textBoxClientCode.Text
        Dim mark = ddlValidReceipts.SelectedValue
        Dim companyCode = ddlCompany.SelectedValue.Trim
        Dim ZoneCode = ddlCity.SelectedValue.Trim
        Dim documentNumber = textBoxNumDoc.Text.Trim
        Dim selectClause As String = "select     r.Num_doc as Recibo,
    r.RFECHA as Fecha,
    r.RCODVEND as VendedorId,
    LTRIM(RTRIM(v.Nombre_vend)) AS Vendedor,
    v.VEND_LIDER as LiderId,
    c.Codigo_clie as ClienteId,
    LTRIM(RTRIM(c.Nombre_clie)) AS Cliente,
    r.Por_lempira,
	con.CONT_SERVI ServicioId,
	con.SERVI1DES as Servicio,
       ISNULL(r.SALDOANT, 0) as Valor,
	   ISNULL(con.CONT_PRIMA, 0) as Prima,
	   ISNULL(con.CONT_NUMCUO, 0) as NumeroCuotas,
	   ISNULL(con.CONT_VALCUO, 0) as Cuota,
	  ISNULL(con.CONT_CANTI , 0) as Cantidad, 
    r.MARCA,
    r.rhora as Hora,
     ISNULL(c.LATITUD, 0) as LATITUD,
     ISNULL(c.LONGITUD , 0) AS LONGITUD"
        Dim fromClause As String = "from recibos r LEFT JOIN
    CLIENTESN c ON r.Codigo_clie = c.Codigo_clie COLLATE DATABASE_DEFAULT and r.RCODVEND=c.CL_VENDEDOR

LEFT JOIN
	CONTRATON con ON con.Codigo_clie = r.Codigo_clie and con.cont_vended= r.RCODVEND
LEFT JOIN
    funamor.dbo.VENDEDOR v ON r.RCODVEND = v.Cod_vendedo COLLATE DATABASE_DEFAULT"
        Dim whereClauseList As New List(Of String)()

        Dim orderByClause As String = "order by RFECHA desc"
        Dim groupByClause As String = ""
        whereClauseList.Add("r.RFECHA <= @End")

        whereClauseList.Add("r.RFECHA >= @Start")

        If Not String.IsNullOrEmpty(ClientCode) Then
            whereClauseList.Add("r.Codigo_clie like @Client")
        End If

        If Not String.IsNullOrEmpty(salesPersonCode) Then
            whereClauseList.Add("r.RCODVEND like @Collector")
        End If

        If Not String.IsNullOrEmpty(leaderCode) Then
            whereClauseList.Add("v.VEND_LIDER like @Leader")
        End If

        'If Not String.IsNullOrEmpty(companyCode) Then
        '    whereClauseList.Add("cl.Cod_zona like @Company")
        'End If

        If Not String.IsNullOrEmpty(ZoneCode) Then
            whereClauseList.Add("v.VZCODIGO like @City")
        End If
        If Not String.IsNullOrEmpty(mark) Then
            whereClauseList.Add("r.MARCA like @Mark")
        End If


        If Not String.IsNullOrEmpty(documentNumber) Then
            whereClauseList.Add("REPLACE(c.Num_doc, '-', '') LIKE @Document")
        End If

        Dim whereClause As String = ""
        If whereClauseList.Count > 0 Then
            whereClause = "WHERE " & String.Join(" AND ", whereClauseList)
        End If
        Dim query As String = String.Format("{0} {1} {2} {3} {4}", selectClause, fromClause, whereClause, groupByClause, orderByClause)
        Try
            Return GetFromDb(Of VentasDto)(query, salesPersonCode, ClientCode, documentNumber, companyCode, ZoneCode, leaderCode, mark)


        Catch ex As Exception
            ' Handle any other exceptions
            Throw New Exception("Problema al recibir información de la base de datos.", ex)
        End Try
    End Function

    Public Function GetReceiptDataForGridview()
        Dim collectorCode = textBoxCode.Text.Trim
        Dim CompanyCode = ddlCompany.SelectedValue.Trim
        Dim leaderCode As String = ddlLeader.SelectedValue.Trim
        Dim zoneCode As String = ddlCity.SelectedValue.Trim
        Dim ClientCode As String = textBoxClientCode.Text.Trim
        Try

            Dim data1 = ReceiptsByDateCachedList.OrderByDescending(Function(c) c.Por_lempira).ToList()


            Dim groupedData = data1.Where(Function(r) r.Por_lempira.ToString().Trim() <> "" AndAlso r.VendedorId IsNot Nothing) _
                .GroupBy(Function(r) r.VendedorId).
                 Select(Function(group) New With {
        .Codigo = group.Key,
        .Vendedor = group.FirstOrDefault().Vendedor,
        .Ventas = group.Count(),
        .Cobrado = FormattingHelper.ToLempiras(group.Sum(Function(r) r.Por_lempira)),
                .CobradoDecimal = group.Sum(Function(r) r.Por_lempira),
        .Lider = group.FirstOrDefault().LiderId
    }).OrderByDescending(Function(c) c.CobradoDecimal).Select(Function(r) New With {r.Codigo, r.Vendedor, r.Ventas, r.Cobrado, r.Lider}).ToList()

            Dim dataCount = groupedData.Count()
            Return groupedData

        Catch ex As Exception
            Throw New Exception(ex.Message & ex.InnerException.Message, ex.InnerException)
            Throw
        End Try
    End Function

    Public Sub RouteOfReceiptsMap(keyValue As String)
        Dim receipts As List(Of VentasDto)
        Dim cachedReceipts = ReceiptsByDateCachedList
        receipts = cachedReceipts.Where(Function(c) c.VendedorId.Contains(keyValue)).OrderByDescending(Function(r) r.Fecha).ThenBy _
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
        iMap.Src = "/shared/Map/Map.aspx"
        pnlMap.Visible = True

    End Sub
    Private Sub BindReceiptsDetails(DetailsControl As GridView, keyValue As String)
        Dim lista = ReceiptsByDateCachedList
        Dim d = lista.Where(Function(r) r.VendedorId.Contains(keyValue)).OrderByDescending(Function(r) r.Fecha).ThenByDescending _
            (Function(e)
                 Dim time As DateTime
                 If DateTime.TryParse(e.Hora, time) Then
                     Return time
                 Else
                     Return DateTime.MinValue ' Default value for invalid time strings
                 End If
             End Function) _
            .Select(Function(r) New With {.Codigo = r.Recibo, .Cliente = r.Cliente.Trim() + r.ClienteId.Trim(), .Prima = FormattingHelper.ToLempiras(r.Prima), r.Servicio, r.Cantidad, .Valor = FormattingHelper.ToLempiras(r.Valor), .Fecha = r.Fecha.ToString("dd/M/yyyy"), .Hora = r.Hora, .Estado = FormattingHelper.MarcaToNulo(r.MARCA)
}).ToList()
        DetailsControl.DataSource = d
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
        Dim cachedReceipts = ReceiptsByDateCachedList
        receipts = cachedReceipts.Where(Function(c) c.LiderId.Contains(keyValue)).OrderByDescending(Function(r) r.Fecha).ThenBy _
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
        iMap.Src = "/shared/Map/Map.aspx"
        pnlMap.Visible = True

    End Sub

End Class
