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
    Public Class SimpleCollectorDto
        Public Property Codigo As String
        Public Property Nombre As String
        Public Property CodigoDeLider As String
    End Class
    Public Class SimpleClientDto
        Public Property Codigo As String
        Public Property Nombre As String

    End Class

    Public Class RecibosDTO
        <Key>
        Public Property Num_doc As String

        Public Property RFECHA As DateTime

        Public Property codigo_cobr As String

        Public Property nombre_cobr As String

        Public Property cob_lider As String

        Public Property Codigo_clie As String

        Public Property Nombre_clie As String

        Public Property Por_lempira As Decimal

        Public Property Saldo_actua As Decimal

        Public Property Cod_zona As String

        Public Property VZCODIGO As String

        Public Property LATITUD As String

        Public Property LONGITUD As String
    End Class

    Public Function getReceiptsFromDB() As Object
        Dim endD = endDate.Text
        Dim initD = startDate.Text
        Dim ClientCode = ""

        Using funamorContext As New FunamorContext
            funamorContext.Database.Log = Sub(s) System.Diagnostics.Debug.WriteLine(s)

            Dim query As String = "
            SELECT r.Num_doc, r.RFECHA, r.codigo_cobr, LTRIM(RTRIM(cb.nombre_cobr)) as nombre_cobr, cb.cob_lider, c.Codigo_clie, LTRIM(RTRIM(c.Nombre_clie)) as Nombre_clie, r.Por_lempira, c.Saldo_actua, c.Cod_zona, c.VZCODIGO, r.LATITUD, r.LONGITUD
            FROM aecobros.dbo.recibos r
            LEFT JOIN clientes c ON r.Codigo_clie = c.Codigo_clie
            LEFT JOIN cobrador cb ON r.codigo_cobr = cb.codigo_cobr
            WHERE r.RFECHA >= @start AND r.RFECHA <= @end and r.Codigo_clie like @client
        "
            Try
                Dim startDateParam As DateTime
                Dim endDateParam As DateTime

                If DateTime.TryParse(initD, startDateParam) AndAlso DateTime.TryParse(endD, endDateParam) Then
                    startDateParam = startDateParam.Date
                    endDateParam = endDateParam.Date.AddDays(1).AddSeconds(-1)
                    Dim clientCodeParam As String = "%" & ClientCode & "%"

                    Dim result As List(Of RecibosDTO) = funamorContext.Database.SqlQuery(Of RecibosDTO)(
                        query,
                        New SqlParameter("@client", clientCodeParam),
                        New SqlParameter("@start", startDateParam),
                        New SqlParameter("@end", endDateParam)).ToList()


                    Return result
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
    Public Function GetCollectorsFromDb() As Object

        Using funamorContext As New FunamorContext
            Return funamorContext.Cobradores.Where(Function(r) r.CobLider IsNot Nothing).Select(Function(c) New SimpleCollectorDto With {.Codigo = c.Codigo, .Nombre = c.Nombre, .CodigoDeLider = c.CobLider}).ToList()
        End Using
    End Function

    Public Function GetReceiptDataForGridview()
        Dim collectorCode = textBoxCode.Text.Trim
        Dim CompanyCode = ddlCompany.SelectedValue.Trim
        Dim leaderCode As String = ddlLeader.SelectedValue.Trim
        Dim zoneCode As String = ddlCity.SelectedValue.Trim
        Dim ClientCode As String = textBoxClientCode.Text.Trim
        Try
            Dim data1 = ReceiptsByDateCachedList _
                   .Where(
            Function(r)
                ' Check for null and then perform operations
                Dim collectorCodeValid = If(r.codigo_cobr IsNot Nothing AndAlso r.codigo_cobr.Contains(collectorCode), True, False)
                Dim leaderCodeValid = If(r.cob_lider IsNot Nothing AndAlso r.cob_lider.Contains(leaderCode), True, False)

                Return collectorCodeValid AndAlso leaderCodeValid
            End Function).OrderBy(Function(c) c.codigo_cobr).ToList()
            If zoneCode.Length > 0 Then
                data1 = data1.Where((Function(r) r.VZCODIGO IsNot Nothing AndAlso r.VZCODIGO.Contains(zoneCode))).ToList()

            End If
            If ClientCode.Length > 0 Then
                data1 = data1.Where((Function(r) r.Codigo_clie IsNot Nothing AndAlso r.Codigo_clie.Contains(ClientCode))).ToList()

            End If

            If CompanyCode.Length > 0 Then
                data1 = data1.Where((Function(r) r.Cod_zona IsNot Nothing AndAlso r.Cod_zona.Contains(CompanyCode))).ToList()

            End If

            Dim groupedData = data1.Where(Function(r) r.Por_lempira.ToString().Trim() <> "" AndAlso r.codigo_cobr IsNot Nothing) _
                .GroupBy(Function(r) r.codigo_cobr).
                 Select(Function(group) New With {
        .Codigo = group.Key,
        .Nombre = group.FirstOrDefault().nombre_cobr,
        .Recibos = group.Count(),
        .Cobrado = FormattingHelper.ToLempiras(group.Sum(Function(r) r.Por_lempira))
    }).
    ToList()

            Dim dataCount = groupedData.Count()
            Return groupedData

        Catch ex As Exception
            Throw New Exception(ex.Message & ex.InnerException.Message, ex.InnerException)
            Throw
        End Try
    End Function

    Public Sub RouteOfReceiptsMap(keyValue As String)
        Dim receipts As List(Of RecibosDTO)
        Dim cachedReceipts = ReceiptsByDateCachedList
        receipts = cachedReceipts.Where(Function(c) c.codigo_cobr.Contains(keyValue)).ToList()
        Dim markers As New List(Of MarkerForMap)
        For Each receipt As RecibosDTO In receipts
            Dim tooltipMsg = $"Fecha: {receipt.RFECHA}, cliente: {receipt.Nombre_clie} cobrado: {receipt.Por_lempira}"
            If receipt.LATITUD.ToString().Trim.Length > 0 AndAlso receipt.LONGITUD.ToString().Trim.Length > 0 Then
                Dim marker As New MarkerForMap With {.TooltipMessage = tooltipMsg, .Latitud = receipt.LATITUD, .Longitud = receipt.LONGITUD, .MarkerType = MarkerTypes.Cliente}
                markers.Add(marker)
            End If

        Next

        Dim dataForMaps As New DataForMapGenerator($"Recibos del cobrador {keyValue} del {startDate.Text} al {endDate.Text}", markers, True)
        Session("MarkersData") = dataForMaps
        Response.Redirect("~/shared/Map/Map.aspx")

    End Sub
    Public Sub RouteOfReceiptsByLeaderMap(sender As Object, e As EventArgs) Handles BtnRouteOfReceiptsMapByLeader.Click
        Dim keyValue = ddlLeader.SelectedValue
        If keyValue = "" Then
            Dim msg = "Seleccione unlider"
            AlertHelper.GenerateAlert("warning", msg, alertPlaceholder)
            Exit Sub
        End If
        Dim receipts As List(Of RecibosDTO)
        Dim cachedReceipts = ReceiptsByDateCachedList
        receipts = cachedReceipts.Where(Function(c) c.cob_lider.Contains(keyValue)).ToList()
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
        Response.Redirect("~/shared/Map/Map.aspx")

    End Sub

End Class
