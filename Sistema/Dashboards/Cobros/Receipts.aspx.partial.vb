Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI.WebControls
Imports System.Data.Entity

Partial Public Class CobrosDashboard
    Inherits System.Web.UI.Page
    Public Class SimpleCollectorDto
        Public Property Codigo As String
        Public Property Nombre As String

    End Class
    Public Class SimpleClientDto
        Public Property Codigo As String
        Public Property Nombre As String

    End Class

    Public Function GetReceiptData()
        Dim collectorCode = textBoxCode.Text.Trim
        Dim CompanyCode = ddlCompany.SelectedValue.Trim
        Dim leaderCode As String = ddlLeader.SelectedValue.Trim
        Dim zoneCode As String = ddlCity.SelectedValue.Trim
        Dim endD = endDate.Text
        Dim initD = startDate.Text
        Dim collectors As List(Of SimpleCollectorDto)
        Dim clients As List(Of SimpleClientDto)
        Using funamorContext As New FunamorContext, cobrosContext As New AeCobrosContext

            collectors = funamorContext.Cobradores.Where(Function(c) c.CobLider.Contains(leaderCode) AndAlso c.Codigo.Contains(collectorCode)).Select(Function(c) New SimpleCollectorDto With {.Codigo = c.Codigo, .Nombre = c.Nombre}).ToList()
            Dim receiptsByDate = cobrosContext.RecibosDeCobro.Where(Function(r) r.Rfecha >= initD AndAlso r.Rfecha <= endD).Select(Function(r) New With {
                                                                                                                                   r.CodigoCliente, r.CodigoCobr, r.PorLempira}).ToList()

            If CompanyCode.Length > 0 Or zoneCode.Length > 0 Then
                clients = funamorContext.Clientes.Where(Function(c) c.CodigoZona.Contains(CompanyCode) AndAlso c.CodigoVZ.Contains(zoneCode) AndAlso c.CodigoCobrador.Contains(collectorCode)).Select(Function(c) New SimpleClientDto With {.Codigo = c.Codigo, .Nombre = c.Nombre}).ToList()
                receiptsByDate = receiptsByDate.Join(clients, Function(e1) e1.CodigoCliente,
                                                Function(e2) e2.Codigo,
                                                Function(e1, e2) New With {
                                                e1.CodigoCliente,
                                                e1.CodigoCobr,
                                                e1.PorLempira}).ToList()
            Else

            End If
            Dim greceipts = receiptsByDate.Where(Function(c) c.CodigoCobr.Contains(collectorCode)).GroupBy(Function(r) r.CodigoCobr).ToList()
            Dim greceiptsSelect = greceipts.Select(Function(r) New With {
                                                .Codigo = r.Key,
                                                .Recibos = r.Count(Function(w) w.PorLempira),
                                                .Cobrado = r.Sum(Function(c) c.PorLempira)}).ToList()
            Dim data = greceiptsSelect.Join(collectors, Function(e1) e1.Codigo,
                                                Function(e2) e2.Codigo,
                                                Function(e1, e2) New With {
                                                    e1.Codigo,
                                                   e2.Nombre,
                                                   e1.Recibos,
                                                   e1.Cobrado
                                                }).OrderByDescending(Function(r) r.Cobrado).ToList()
            Return data

        End Using

    End Function
End Class
