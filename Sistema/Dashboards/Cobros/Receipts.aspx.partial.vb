Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI.WebControls
Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations.Schema

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
        <Column("RFECHA")>
        Public Property Fecha As String

        <Column("codigo_cobr")>
        Public Property CodigoCobrador As String

        <Column("nombre_cobr")>
        Public Property NombreCobrador As String

        <Column("cob_lider")>
        Public Property LiderCobrador As String

        <Column("Codigo_clie")>
        Public Property CodigoCLiente As String

        <Column("Nombre_clie")>
        Public Property NombreCliente As String

        <Column("Por_lempira")>
        Public Property PorLempira As String

        <Column("Saldo_actua")>
        Public Property SaldoActual As String

        <Column("Cod_zona")>
        Public Property Empresa As String

        <Column("VZCODIGO")>
        Public Property Municipio As String

        <Column("LATITUD")>
        Public Property Latitud As String

        <Column("LONGITUD")>
        Public Property Longitud As String
    End Class

    Public Function getReceiptsFromDB() As Object
        Dim endD = endDate.Text
        Dim initD = startDate.Text
        Using cobrosContext As New AeCobrosContext
            Dim a = cobrosContext.RecibosDeCobro.Where(Function(r) r.Rfecha >= initD AndAlso r.Rfecha <= endD).ToList()
        End Using
        Using funamorContext As New FunamorContext
            Dim query As String = "
        SELECT r.RFECHA, r.codigo_cobr, cb.nombre_cobr, cb.cob_lider, c.Codigo_clie, c.Nombre_clie, r.Por_lempira, c.Saldo_actua, c.Cod_zona, c.VZCODIGO, r.LATITUD, r.LONGITUD
        FROM aecobros.dbo.recibos r
        INNER JOIN aecobros.dbo.clientes c ON r.codigo_clie = c.codigo_clie
        LEFT JOIN cobrador cb ON r.codigo_cobr = cb.codigo_cobr
        WHERE r.RFECHA >= @start
    "
            Dim result As List(Of RecibosDTO) = funamorContext.Database.SqlQuery(Of RecibosDTO)(
        query,
        New SqlParameter("@start", initD),
        New SqlParameter("@end", endD)).ToList()
            Return result
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
        Dim data = ReceiptsByDateCachedList _
            .Where(
  Function(r) (If(r.CodigoCobrador, "").Contains(collectorCode)) AndAlso
                    (If(r.LiderCobrador, "").Contains(leaderCode)) AndAlso
                    (If(r.Municipio, "").Contains(zoneCode)) AndAlso
                    (If(r.Empresa, "").Contains(CompanyCode))).GroupBy(Function(r) r.CodigoCobrador) _
            .Select(Function(r) New With {
                                                .Codigo = r.Key,
                                                .Nombre = r.Count(Function(w) w.NombreCobrador),
                                                .Recibos = r.Count(Function(w) w.NombreCliente),
                                                .Cobrado = r.Sum(Function(c) c.PorLempira)}).ToList()

        'Using funamorContext As New FunamorContext
        '    Dim a = CollectorsCachedList
        '    collectors = CollectorsCachedList.Where(Function(c)
        '                                                Dim codigoDeLider = If(c.CodigoDeLider, "") ' Handle null cases
        '                                                Dim codigo = If(c.Codigo, "")

        '                                                Return codigoDeLider.Contains(leaderCode) AndAlso
        '                                               codigo.Contains(collectorCode)
        '                                            End Function).ToList()

        '    Dim receiptsByDate = ReceiptsByDateCachedList.Select(Function(r) New With {
        '                                                    r.CodigoCliente, r.CodigoCobr, r.PorLempira}).ToList()

        '    If CompanyCode.Length > 0 Or zoneCode.Length > 0 Then
        '        clients = funamorContext.Clientes.Where(Function(c) c.CodigoZona.Contains(CompanyCode) AndAlso c.CodigoVZ.Contains(zoneCode) AndAlso c.CodigoCobrador.Contains(collectorCode) AndAlso c.SaldoActual > 0).Select(Function(c) New SimpleClientDto With {.Codigo = c.Codigo, .Nombre = c.Nombre}).ToList()
        '        receiptsByDate = receiptsByDate.Join(clients, Function(e1) e1.CodigoCliente,
        '                                        Function(e2) e2.Codigo,
        '                                        Function(e1, e2) New With {
        '                                        e1.CodigoCliente,
        '                                        e1.CodigoCobr,
        '                                        e1.PorLempira}).ToList()
        '    Else

        '    End If
        '    Dim greceipts = receiptsByDate.Where(Function(c) c.CodigoCobr.Contains(collectorCode)).GroupBy(Function(r) r.CodigoCobr).ToList()
        '    Dim greceiptsSelect = greceipts.Select(Function(r) New With {
        '                                        .Codigo = r.Key,
        '                                        .Recibos = r.Count(Function(w) w.PorLempira),
        '                                        .Cobrado = r.Sum(Function(c) c.PorLempira)}).ToList()
        '    Dim data = greceiptsSelect.Join(collectors, Function(e1) e1.Codigo,
        '                                        Function(e2) e2.Codigo,
        '                                        Function(e1, e2) New With {
        '                                            e1.Codigo,
        '                                           e2.Nombre,
        '                                           e1.Recibos,
        '                                           e1.Cobrado
        '                                        }).OrderByDescending(Function(r) r.Cobrado).ToList()
        Return data

        'End Using

    End Function
End Class
