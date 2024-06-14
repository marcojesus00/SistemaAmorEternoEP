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


        Using funamorContext As New FunamorContext
            funamorContext.Database.Log = Sub(s) System.Diagnostics.Debug.WriteLine(s)

            Dim query As String = "
            SELECT r.Num_doc, r.RFECHA, r.codigo_cobr, cb.nombre_cobr, cb.cob_lider, c.Codigo_clie, c.Nombre_clie, r.Por_lempira, c.Saldo_actua, c.Cod_zona, c.VZCODIGO, r.LATITUD, r.LONGITUD
            FROM aecobros.dbo.recibos r
            LEFT JOIN clientes c ON r.Codigo_clie = c.Codigo_clie
            LEFT JOIN cobrador cb ON r.codigo_cobr = cb.codigo_cobr
            WHERE r.RFECHA >= @start AND r.RFECHA <= @end
        "
            Try
                Dim startDateParam As DateTime
                Dim endDateParam As DateTime

                If DateTime.TryParse(initD, startDateParam) AndAlso DateTime.TryParse(endD, endDateParam) Then
                    startDateParam = startDateParam.Date
                    endDateParam = endDateParam.Date.AddDays(1).AddSeconds(-1)

                    Dim result As List(Of RecibosDTO) = funamorContext.Database.SqlQuery(Of RecibosDTO)(
                        query,
                        New SqlParameter("@start", startDateParam),
                        New SqlParameter("@end", endDateParam)).ToList()
                    'For Each row In result
                    '    Dim numDoc = row.Num_doc
                    '    Dim fecha = row.RFECHA
                    '    Dim codigoCobrador = row.codigo_cobr
                    'Next

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
            Return funamorContext.Cobradores.Select(Function(c) New SimpleCollectorDto With {.Codigo = c.Codigo, .Nombre = c.Nombre, .CodigoDeLider = c.CobLider}).ToList()
        End Using
    End Function

    Public Function GetReceiptDataForGridview()
        Dim collectorCode = textBoxCode.Text.Trim
        Dim CompanyCode = ddlCompany.SelectedValue.Trim
        Dim leaderCode As String = ddlLeader.SelectedValue.Trim
        Dim zoneCode As String = ddlCity.SelectedValue.Trim
        'Dim endD = endDate.Text
        'Dim initD = startDate.Text
        'Dim collectors As List(Of SimpleCollectorDto)
        'Dim clients As List(Of SimpleClientDto)
        Try
            Dim data1 = ReceiptsByDateCachedList _
                   .Where(
            Function(r)
                ' Check for null and then perform operations
                Dim collectorCodeValid = If(r.codigo_cobr IsNot Nothing AndAlso r.codigo_cobr.Contains(collectorCode), True, False)
                Dim leaderCodeValid = If(r.cob_lider IsNot Nothing AndAlso r.cob_lider.Contains(leaderCode), True, False)
                'Dim zoneCodeValid = If(r.VZCODIGO IsNot Nothing AndAlso r.VZCODIGO.Contains(zoneCode), True, False)
                'Dim companyCodeValid = If(r.Cod_zona IsNot Nothing AndAlso r.Cod_zona.Contains(CompanyCode), True, False)

                Return collectorCodeValid AndAlso leaderCodeValid 'AndAlso zoneCodeValid AndAlso companyCodeValid
            End Function).OrderBy(Function(c) c.codigo_cobr).ToList()
            If zoneCode.Length > 0 Then
                data1 = data1.Where((Function(r) r.VZCODIGO IsNot Nothing AndAlso r.VZCODIGO.Contains(zoneCode))).ToList()

            End If
            If CompanyCode.Length > 0 Then
                data1 = data1.Where((Function(r) r.Cod_zona IsNot Nothing AndAlso r.Cod_zona.Contains(CompanyCode))).ToList()

            End If
            'Dim groupedData = data1.Where(Function(r) r.Por_lempira.ToString().Trim() <> "" AndAlso r.codigo_cobr IsNot Nothing) _
            '    .GroupBy(Function(r) New With {r.codigo_cobr, r.nombre_cobr}).ToList()
            'Dim data = groupedData.Select(Function(r) New With {
            '.Codigo = r.Key.codigo_cobr,
            '.Nombre = r.Key.nombre_cobr,
            '.Recibos = r.Count(),
            '    .Cobrado = r.Sum(Function(c) c.Por_lempira)}).OrderByDescending(Function(r) r.Cobrado).ToList()
            Dim groupedData = data1.Where(Function(r) r.Por_lempira.ToString().Trim() <> "" AndAlso r.codigo_cobr IsNot Nothing) _
                .GroupBy(Function(r) r.codigo_cobr).
                 Select(Function(group) New With {
        .Codigo = group.Key,
        .Nombre = group.FirstOrDefault().nombre_cobr,
        .Cobrado = group.Sum(Function(r) r.Por_lempira)
    }).
    ToList()
            'Dim data2 = groupedData.Select(Function(r) New With {
            '.Codigo = r.Key,
            '.Recibos = r.Count(),
            '    .Cobrado = r.Sum(Function(c) c.Por_lempira)}).OrderByDescending(Function(r) r.Cobrado).ToList()
            'Dim data = data2.Join(data1, Function(greceipt) greceipt.Codigo, Function(receipt) receipt.codigo_cobr,
            '                      Function(greceipt, receipt) New With {
            '                      .Codigo = greceipt,
            '                      .Nombre = receipt.nombre_cobr,
            '                      .Cobrado = greceipt.Cobrado,
            '                          .recibos = greceipt.Codigo.Count()}) _
            '                          .ToList()
            'Dim data3 = data2.GroupJoin(data1, Function(greceipt) greceipt.Codigo, Function(receipt) receipt.codigo_cobr,
            '                      Function(greceipt, receipt) New With {
            '                      .Codigo = greceipt,
            '                      .Nombre = receipt,
            '                      .Cobrado = greceipt.Cobrado,
            '                          .recibos = greceipt.Codigo.Count()}) _
            '                          .SelectMany(Function(c) c.).ToList()

            Dim dataCount = groupedData.Count()
        Return groupedData

        Catch ex As Exception
        Throw New Exception(ex.Message & ex.InnerException.Message, ex.InnerException)
        Throw
        End Try
    End Function
End Class
