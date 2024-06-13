Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI.WebControls
Imports System.Data.Entity

Partial Public Class CobrosDashboard
    Inherits System.Web.UI.Page
    Public Function GetPortfolioData()
        Dim collectorCode = textBoxCode.Text.Trim
        Dim CompanyCode = ddlCompany.SelectedValue.Trim
        Dim leaderCode As String = ddlLeader.SelectedValue.Trim
        Dim zoneCode As String = ddlCity.SelectedValue.Trim

        Dim data As List(Of PortfolioDto)
        Using funamorContext As New FunamorContext

            'ClientsWithRemainingBalance = 
            data = funamorContext.Clientes.Include(Function(d) d.CobradorNav) _
                .Where(Function(c) c.CodigoZona.Contains(CompanyCode) AndAlso c.CodigoVZ.Contains(zoneCode) AndAlso c.CodigoCobrador.Contains(collectorCode) AndAlso c.SaldoActual > 0) _
                .GroupBy(Function(c) c.CobradorNav.Codigo) _
                .Select(Function(g) New PortfolioDto With
                {.Codigo = g.Key,
                .Nombre = g.FirstOrDefault().CobradorNav.Nombre,
                .Clientes = g.Count(),
                .Deudas = g.Sum(Function(e) e.SaldoActual)}) _
                .OrderByDescending(Function(e) e.Deudas).ToList()
            For Each item In data
                item.Deudas = FormattingHelper.ToLempiras(item.Deudas)
                item.Cobrado = FormattingHelper.ToLempiras(item.Cobrado)

            Next
            Return data

        End Using


    End Function
    Public Class PortfolioDto
        Public Property Codigo As String
        Public Property Nombre As String
        Public Property Clientes As Integer
        Public Property Deudas As String
        Public Property Cobrado As String

    End Class

End Class
