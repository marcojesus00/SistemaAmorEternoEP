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


    Public Function getGroupedSalesByProductFromDB(Optional orderBy As String = "Valor") As Object
        Dim endD = endDate.Text
        Dim initD = startDate.Text

        Dim leaderCode = ddlLeader.SelectedValue.Trim
        Dim salesPersonCode = textBoxCode.Text.Trim
        Dim ClientCode = textBoxClientCode.Text
        Dim mark = ddlValidReceipts.SelectedValue
        Dim companyCode = ddlCompany.SelectedValue.Trim
        Dim ZoneCode = ddlCity.SelectedValue.Trim
        Dim documentNumber = textBoxNumDoc.Text.Trim
        Dim serviceId = ddlService.SelectedValue.Trim
        Dim selectClause As String = "Select	con.CONT_SERVI as ServicioId,
	con.SERVI1DES as Servicio,
	SUM(con.CONT_PRIMA) as Prima,
	SUM(con.CONT_VALOR) as Valor,
	Sum(con.CONT_VALCUO) as Cuota,
	SUM(con.CONT_CANTI) as Cantidad,
	count(r.Num_doc) as Contratos"
        Dim fromClause As String = "from recibos r LEFT JOIN
    CLIENTESN c ON r.Codigo_clie = c.Codigo_clie COLLATE DATABASE_DEFAULT and r.RCODVEND=c.CL_VENDEDOR
LEFT JOIN
	CONTRATON con ON con.Codigo_clie = r.Codigo_clie and con.cont_vended= r.RCODVEND
LEFT JOIN
    funamor.dbo.VENDEDOR v ON r.RCODVEND = v.Cod_vendedo COLLATE DATABASE_DEFAULT"
        Dim whereClauseList As New List(Of String)()

        Dim orderByClause As String = $"order by {orderBy} desc"
        Dim groupByClause As String = "GROUP BY con.CONT_SERVI, con.SERVI1DES"
        whereClauseList.Add("r.RFECHA <= @End")
        whereClauseList.Add("r.RFECHA >= @Start")

        whereClauseList.Add("con.SERVI1DES is not null")
        whereClauseList.Add("con.CONT_SERVI is not null")


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
        If Not String.IsNullOrEmpty(serviceId) Then
            whereClauseList.Add("con.CONT_SERVI like @ServiceId")
        End If


        If Not String.IsNullOrEmpty(documentNumber) Then
            whereClauseList.Add("REPLACE(r.Num_doc, '-', '') LIKE @Document")
        End If

        Dim whereClause As String = ""
        If whereClauseList.Count > 0 Then
            whereClause = "WHERE " & String.Join(" AND ", whereClauseList)
        End If
        Dim query As String = String.Format("{0} {1} {2} {3} {4}", selectClause, fromClause, whereClause, groupByClause, orderByClause)
        Try
            Return GetFromDb(Of SalesByProductDto)(query, salesPersonCode, ClientCode, documentNumber, companyCode, ZoneCode, leaderCode, mark, ServiceId:=serviceId)


        Catch ex As Exception
            ' Handle any other exceptions
            Throw New Exception("Problema al recibir información de la base de datos.", ex)
        End Try
    End Function

    Public Function GetReceiptByServiceDataForGridview()
        Dim collectorCode = textBoxCode.Text.Trim
        Dim CompanyCode = ddlCompany.SelectedValue.Trim
        Dim leaderCode As String = ddlLeader.SelectedValue.Trim
        Dim zoneCode As String = ddlCity.SelectedValue.Trim
        Dim ClientCode As String = textBoxClientCode.Text.Trim
        Try

            Dim data1 As List(Of SalesByProductDto) = getGroupedSalesByProductFromDB()


            Dim finalData = data1.Select(Function(s) New With {.Codigo = s.ServicioId, s.Servicio, s.Cantidad, s.Contratos, .Prima = FormattingHelper.ToLempiras(s.Prima), .Cuota = FormattingHelper.ToLempiras(s.Cuota), .Valor = FormattingHelper.ToLempiras(s.Valor)}).ToList()

            Return finalData

        Catch ex As Exception
            Throw New Exception(ex.Message & ex.InnerException.Message, ex.InnerException)
            Throw
        End Try
    End Function


    Private Sub BindReceiptsByProductDetails(DetailsControl As GridView, keyValue As String)
        Dim lista = ReceiptsByDateCachedList
        Dim d = lista.Where(Function(r) r.ServicioId IsNot Nothing AndAlso r.ServicioId.Contains(keyValue)).OrderByDescending(Function(r) r.Fecha).ThenByDescending _
            (Function(e)
                 Dim time As DateTime
                 If DateTime.TryParse(e.Hora, time) Then
                     Return time
                 Else
                     Return DateTime.MinValue ' Default value for invalid time strings
                 End If
             End Function) _
            .Select(Function(r) New With {.Codigo = r.Recibo, .Cliente = r.Cliente.Trim() + r.ClienteId.Trim(), .Prima = FormattingHelper.ToLempiras(r.Por_lempira), r.Servicio, r.Cantidad, .Valor = FormattingHelper.ToLempiras(r.Valor), .Fecha = r.Fecha.ToString("dd/M/yyyy"), .Hora = r.Hora, .Estado = FormattingHelper.MarcaToNulo(r.MARCA)
}).ToList()
        DetailsControl.DataSource = d
        DetailsControl.DataBind()
        DetailsControl.Visible = True
    End Sub
End Class
