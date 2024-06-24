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

        Public Property Por_lempira As Decimal?

        Public Property Saldo_actua As Decimal?

        Public Property Cod_zona As String

        Public Property VZCODIGO As String

        Public Property LATITUD As String

        Public Property LONGITUD As String
        Public Property SALDOANT As Decimal
        Public Property MARCA As String
        Public Property rhora As String
    End Class

    Public Function getReceiptsFromDB(Optional specificQuery As Boolean = True) As Object
        Dim endD = endDate.Text
        Dim initD = startDate.Text
        Dim ClientCode = ""
        Dim leaderCode = ""
        Dim collectorCode = ""
        Dim mark = ""
        Dim companyCode = ""
        Dim ZoneCode = ""
        Dim documentNumber = ""
        Dim query As String
        Using funamorContext As New FunamorContext
            funamorContext.Database.Log = Sub(s) System.Diagnostics.Debug.WriteLine(s)
            If specificQuery Then
                leaderCode = ddlLeader.SelectedValue.Trim
                collectorCode = textBoxCode.Text.Trim
                ClientCode = textBoxClientCode.Text
                mark = ddlValidReceipts.SelectedValue
                companyCode = ddlCompany.SelectedValue.Trim
                ZoneCode = ddlCity.SelectedValue.Trim
                documentNumber = textBoxNumDoc.Text.Trim
            Else
                'query = ""

            End If
            query = "
            SELECT r.Num_doc, r.RFECHA, r.codigo_cobr, LTRIM(RTRIM(cb.nombre_cobr)) as nombre_cobr, cb.cob_lider, c.Codigo_clie, LTRIM(RTRIM(c.Nombre_clie)) as Nombre_clie, r.Por_lempira, c.Saldo_actua, r.SALDOANT, r.MARCA, r.rhora, c.Cod_zona, c.VZCODIGO, r.LATITUD, r.LONGITUD
            FROM aecobros.dbo.recibos r
            LEFT JOIN clientes c ON r.Codigo_clie = c.Codigo_clie
            LEFT JOIN cobrador cb ON r.codigo_cobr = cb.codigo_cobr
            WHERE r.RFECHA >= @start AND r.RFECHA <= @end and r.Codigo_clie like @client and r.MARCA LIKE @Mark AND r.codigo_cobr like @Collector AND c.Cod_zona like @Company AND c.VZCODIGO like @City and cb.cob_lider like @leader and r.Num_doc like @Document
        "
            Try
                Dim startDateParam As DateTime
                Dim endDateParam As DateTime

                If DateTime.TryParse(initD, startDateParam) AndAlso DateTime.TryParse(endD, endDateParam) Then
                    startDateParam = startDateParam.Date
                    endDateParam = endDateParam.Date.AddDays(1).AddSeconds(-1)
                    Dim clientCodeParam As String = "%" & ClientCode & "%"
                    Dim LeaderCodeParam As String = "%" & LeaderCode & "%"
                    Dim collectorCodeParam As String = "%" & collectorCode & "%"
                    Dim markParam = "%" & mark & "%"
                    Dim CompanyCodeParam As String = "%" & companyCode & "%"
                    Dim CityCodeParam As String = "%" & ZoneCode & "%"
                    Dim documentNumberParam As String = "%" & documentNumber & "%"

                    Dim result As List(Of RecibosDTO) = funamorContext.Database.SqlQuery(Of RecibosDTO)(
                        query,
                                                New SqlParameter("@Leader", LeaderCodeParam),
                            New SqlParameter("@Document", documentNumberParam),
                        New SqlParameter("@Collector", collectorCodeParam),
                        New SqlParameter("@Mark", markParam),
                        New SqlParameter("@client", clientCodeParam),
                                               New SqlParameter("@Company", CompanyCodeParam),
                       New SqlParameter("@City", CityCodeParam),
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
            'Dim data1 = ReceiptsByDateCachedList _
            '       .Where(
            'Function(r) r.codigo_cobr IsNot Nothing AndAlso r.codigo_cobr.Contains(collectorCode)).ToList()
            Dim data1 = ReceiptsByDateCachedList.OrderByDescending(Function(c) c.Por_lempira).ToList()
            'If zoneCode.Length > 0 Then
            '    data1 = data1.Where((Function(r) r.VZCODIGO IsNot Nothing AndAlso r.VZCODIGO.Contains(zoneCode))).ToList()

            'End If
            'If ClientCode.Length > 0 Then
            '    data1 = data1.Where((Function(r) r.Codigo_clie IsNot Nothing AndAlso r.Codigo_clie.Contains(ClientCode))).ToList()

            'End If

            'If CompanyCode.Length > 0 Then
            '    data1 = data1.Where((Function(r) r.Cod_zona IsNot Nothing AndAlso r.Cod_zona.Contains(CompanyCode))).ToList()

            'End If

            Dim groupedData = data1.Where(Function(r) r.Por_lempira.ToString().Trim() <> "" AndAlso r.codigo_cobr IsNot Nothing) _
                .GroupBy(Function(r) r.codigo_cobr).
                 Select(Function(group) New With {
        .Codigo = group.Key,
        .Cobrador = group.FirstOrDefault().nombre_cobr,
        .Recibos = group.Count(),
        .Cobrado = FormattingHelper.ToLempiras(group.Sum(Function(r) r.Por_lempira)),
                .CobradoDecimal = group.Sum(Function(r) r.Por_lempira),
        .Lider = group.FirstOrDefault().cob_lider
    }).OrderByDescending(Function(c) c.Cobrado).Select(Function(r) New With {r.Codigo, r.Cobrador, r.Recibos, r.Cobrado, r.Lider}).ToList()

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
            Dim msg = "Seleccione un lider"
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
    Private Sub BindReceiptsDetails(DetailsControl As GridView, keyValue As String)
        Dim lista = ReceiptsByDateCachedList
        Dim d = lista.Where(Function(r) r.codigo_cobr = keyValue).OrderByDescending(Function(r) r.RFECHA).ThenByDescending _
            (Function(e)
                 Dim time As DateTime
                 If DateTime.TryParse(e.rhora, time) Then
                     Return time
                 Else
                     Return DateTime.MinValue ' Default value for invalid time strings
                 End If
             End Function) _
            .Select(Function(r) New With {.Codigo = r.Num_doc, .Cliente = r.Nombre_clie, .Cobrado = FormattingHelper.ToLempiras(r.Por_lempira), .Saldo_anterior = r.SALDOANT, .Fecha = r.RFECHA.ToString("dd/M/yyyy"), .Hora = r.rhora, .Estado = FormattingHelper.MarcaToNulo(r.MARCA)}).ToList()
        DetailsControl.DataSource = d
        DetailsControl.DataBind()
        DetailsControl.Visible = True
    End Sub

End Class
