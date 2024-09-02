﻿Imports System.Data.SqlClient
Imports Sistema.CobrosDashboard

Public Class CobrosService


    'Public Function getCobrosReceiptsFromDB(params As CobrosParams) As Object
    '    Dim endD = params.EndDate
    '    Dim initD = params.StartDate

    '    Dim query As String
    '    Using funamorContext As New FunamorContext
    '        funamorContext.Database.Log = Sub(s) System.Diagnostics.Debug.WriteLine(s)
    '        Dim leaderCode = params.LeaderCode
    '        Dim collectorCode = params.SalesPersonCode
    '        Dim ClientCode = params.ClientCode
    '        Dim mark = params.ValidReceiptsMark
    '        Dim companyCode = params.CompanyCode
    '        Dim ZoneCode = params.ZoneCode
    '        Dim documentNumber = params.DocumentNumber

    '        query = "
    '        SELECT r.Num_doc, r.RFECHA, r.codigo_cobr, LTRIM(RTRIM(cb.nombre_cobr)) as nombre_cobr, cb.cob_lider, c.Codigo_clie, LTRIM(RTRIM(c.Nombre_clie)) as Nombre_clie, r.Por_lempira, c.Saldo_actua, r.SALDOANT, r.MARCA, r.rhora, c.Cod_zona, c.VZCODIGO,r.liquida, r.liquida2, r.LATITUD, r.LONGITUD
    '        FROM aecobros.dbo.recibos r
    '        LEFT JOIN clientes c ON r.Codigo_clie = c.Codigo_clie
    '        LEFT JOIN cobrador cb ON r.codigo_cobr = cb.codigo_cobr
    '        WHERE r.RFECHA >= @start AND r.RFECHA <= @end and r.Codigo_clie like @client and r.MARCA LIKE @Mark AND r.codigo_cobr like @Collector AND c.Cod_zona like @Company AND c.VZCODIGO like @City and cb.cob_lider like @leader and r.Num_doc like @Document
    '    "
    '        Try
    '            Dim startDateParam As DateTime
    '            Dim endDateParam As DateTime

    '            If DateTime.TryParse(initD, startDateParam) AndAlso DateTime.TryParse(endD, endDateParam) Then
    '                startDateParam = startDateParam.Date
    '                endDateParam = endDateParam.Date.AddDays(1).AddSeconds(-1)
    '                Dim clientCodeParam As String = "%" & ClientCode & "%"
    '                Dim LeaderCodeParam As String = "%" & leaderCode & "%"
    '                Dim collectorCodeParam As String = "%" & collectorCode & "%"
    '                Dim markParam = "%" & mark & "%"
    '                Dim CompanyCodeParam As String = "%" & companyCode & "%"
    '                Dim CityCodeParam As String = "%" & ZoneCode & "%"
    '                Dim documentNumberParam As String = "%" & documentNumber & "%"

    '                Dim result As List(Of RecibosDTO) = funamorContext.Database.SqlQuery(Of RecibosDTO)(
    '                    query,
    '                                            New SqlParameter("@Leader", LeaderCodeParam),
    '                        New SqlParameter("@Document", documentNumberParam),
    '                    New SqlParameter("@Collector", collectorCodeParam),
    '                    New SqlParameter("@Mark", markParam),
    '                    New SqlParameter("@client", clientCodeParam),
    '                                           New SqlParameter("@Company", CompanyCodeParam),
    '                   New SqlParameter("@City", CityCodeParam),
    '                    New SqlParameter("@start", startDateParam),
    '                    New SqlParameter("@end", endDateParam)).ToList()


    '                Return result
    '            Else
    '                Throw New ArgumentException("Invalid date format for start or end date.")
    '            End If
    '        Catch ex As Exception
    '            'DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
    '            Throw New Exception("Problema al recibir información de la base de datos.", ex)
    '        End Try
    '    End Using
    'End Function
    Public Function getCobrosLocationReceipt()
        Dim result As New Point(15, 87)
        Return result
    End Function
    Public Function GetRecepits(r As whereAndParamsDto, filters As CobrosFiltersData, Optional top As String = "") As List(Of ReciboDTO)
        Dim params = r.sqlParams
        Dim whereClause = r.whereClause
        If top.Length > 0 AndAlso filters IsNot Nothing Then
            If (filters.ClientCode IsNot Nothing AndAlso filters.ClientCode.Length > 0) Then
                top = ""

            Else
            End If
        End If
        Dim leftj = ""
        If filters IsNot Nothing AndAlso filters.LeaderCode.Length > 0 Then
            leftj = "LEFT JOIN cobrador cb ON r.codigo_cobr = cb.codigo_cobr"
        End If
        Using funamorContext As New FunamorContext()

            Dim Query = $"
            SELECT {top} r.Num_doc as Codigo,  c.Codigo_clie as Codigo_cliente, LTRIM(RTRIM(c.Nombre_clie)) as Cliente, FORMAT(r.Por_lempira, 'C', 'es-HN')  as Cobrado, FORMAT(c.Saldo_actua , 'C', 'es-HN') as Saldo_actual, FORMAT(r.SALDOANT , 'C', 'es-HN') as Saldo_anterior, r.MARCA, r.rhora as Hora, FORMAT(r.RFECHA, 'dd-MM-yyyy') as Fecha, c.Cod_zona as Empresa, c.VZCODIGO as Zona,r.liquida, r.liquida2, r.LATITUD, r.LONGITUD
            FROM aecobros.dbo.recibos r
            LEFT JOIN clientes c ON r.Codigo_clie = c.Codigo_clie
            {leftj}
        "
            '"            WHERE r.RFECHA >= @start AND r.RFECHA <= @end and r.Codigo_clie like @client and r.MARCA LIKE @Mark AND r.codigo_cobr like @Collector AND c.Cod_zona like @Company AND c.VZCODIGO like @City and cb.cob_lider like @leader and r.Num_doc like @Document"

            Query += whereClause
            Dim orderByClause = "
ORDER BY r.RFECHA DESC, Hora desc
"
            Query += orderByClause
            funamorContext.Database.Log = Sub(s) System.Diagnostics.Debug.WriteLine(s)

            Dim result As List(Of ReciboDTO) = funamorContext.Database.SqlQuery(Of ReciboDTO)(
                    Query, params.ToArray()).ToList()


            Return result

        End Using

    End Function
    Public Function GetRecepitsGrouped(r As whereAndParamsDto, totalCountparams As List(Of SqlParameter)) As PaginatedResult(Of groupedreceiptDto)
        'Params
        Dim params = r.sqlParams
        Dim whereClause = r.whereClause
        Using funamorContext As New FunamorContext()


            Dim selectClause = "
            SELECT r.codigo_cobr as Codigo, LTRIM(RTRIM(cb.nombre_cobr)) as Nombre, COUNT(r.Num_doc) as Recibos, SUM(r.Por_lempira) as Cobrado, cb.cob_lider as Lider
        "
            '"            WHERE r.RFECHA >= @start AND r.RFECHA <= @end and r.Codigo_clie like @client and r.MARCA LIKE @Mark AND r.codigo_cobr like @Collector AND c.Cod_zona like @Company AND c.VZCODIGO like @City and cb.cob_lider like @leader and r.Num_doc like @Document"
            Dim fromClause = "            FROM aecobros.dbo.recibos r
            LEFT JOIN clientes c ON r.Codigo_clie = c.Codigo_clie
            LEFT JOIN cobrador cb ON r.codigo_cobr = cb.codigo_cobr
"
            Dim groupByClause = "
GROUP BY r.codigo_cobr, cb.nombre_cobr, cb.cob_lider
"
            Dim orderByClause = "
ORDER BY Cobrado desc"
            'Dim Query = ""

            'Query += selectClause
            'Query += fromClause
            'Query += whereClause
            'Query += groupByClause
            'Query += orderByClause
            Dim queryCount = $"
                    SELECT COUNT(*) AS TotalCount from
                         ( select r.codigo_cobr  {fromClause}
                        {whereClause} {groupByClause}) as subquery  OPTION(RECOMPILE)
                             ;"
            Dim paginationClause = "OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY"
            Dim dataQuery = $"
                            {selectClause} {fromClause}  {whereClause} {groupByClause}
                            {orderByClause} 
                            {paginationClause} OPTION(RECOMPILE)
                        "
            'funamorContext.Database.Log = Sub(s) System.Diagnostics.Debug.WriteLine(s)

            Dim result As List(Of groupedreceiptDto) = funamorContext.Database.SqlQuery(Of groupedreceiptDto)(
                    dataQuery, params.ToArray()).ToList()
            Dim toltalC = funamorContext.Database.SqlQuery(Of Integer)(queryCount, totalCountparams.ToArray()).FirstOrDefault()

            Dim p = New PaginatedResult(Of groupedreceiptDto) With {
                                                            .Data = result,
                                                            .TotalCount = toltalC
                                                        }
            Dim paginatedEmpty As New PaginatedResult(Of groupedreceiptDto)
            Return p

        End Using

    End Function


    Public Function GetClients(filters As CobrosFiltersData, params As List(Of SqlParameter), totalCountparams As List(Of SqlParameter)) As PaginatedResult(Of PortfolioIDto)

        Using funamorContext As New FunamorContext
            'funamorContext.Database.Log = Sub(s) System.Diagnostics.Debug.WriteLine(s)


            Dim selectClause As String = "select cr.codigo_cobr AS Codigo, cr.nombre_cobr as Nombre, count(cl.Codigo_clie) as Clientes,  sum(cn.CONT_VALCUO) as Cuota_mensual " ' "select cr.codigo_cobr AS Codigo, cr.nombre_cobr, count(cl.Codigo_clie) as Clientes, sum(cl.Saldo_actua) as Cartera "
            Dim fromClause As String = "from COBRADOR cr join CLIENTES cl 
                 on cl.cl_cobrador = cr.codigo_cobr
                 left join CONTRATO cn
				 on cl.Codigo_clie = cn.Codigo_clie"
            Dim whereClauseList As New List(Of String)()

            Dim orderByClause As String = "order by Cuota_mensual  desc"
            Dim groupByClause As String = "group by cr.codigo_cobr, nombre_cobr"
            whereClauseList.Add("cl.Saldo_actua > 0")
            If Not String.IsNullOrEmpty(filters.ClientCode) Then
                whereClauseList.Add("cl.Codigo_clie like @Client")
            End If

            If Not String.IsNullOrEmpty(filters.SalesPersonCode) Then
                whereClauseList.Add("cr.codigo_cobr like @Collector")
            End If

            If Not String.IsNullOrEmpty(filters.LeaderCode) Then
                whereClauseList.Add("cr.cob_lider like @Leader")
            End If

            If Not String.IsNullOrEmpty(filters.CompanyCode) Then
                whereClauseList.Add("cl.Cod_zona like @Company")
            End If

            If Not String.IsNullOrEmpty(filters.ZoneCode) Then
                whereClauseList.Add("cl.VZCODIGO like @City")
            End If

            If Not String.IsNullOrEmpty(filters.DocumentNumber) Then
                whereClauseList.Add("REPLACE(cl.identidad, '-', '') LIKE @Document")
            End If

            Dim whereClause As String = ""
            If whereClauseList.Count > 0 Then
                whereClause = "WHERE " & String.Join(" AND ", whereClauseList)
            End If
            Dim query As String = String.Format("{0} {1} {2} {3} {4}", selectClause, fromClause, whereClause, groupByClause, orderByClause)
            Dim queryCount = $"
                    SELECT COUNT(*) AS TotalCount from
                         ( select cr.codigo_cobr  {fromClause}
                        {whereClause} {groupByClause}) as subquery  OPTION(RECOMPILE)
                             ;"
            Dim paginationClause = "OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY"
            Dim dataQuery = $"
                            {selectClause} {fromClause}  {whereClause} {groupByClause}
                            {orderByClause} 
                            {paginationClause} OPTION(RECOMPILE)"

            Dim result As List(Of PortfolioIDto) = funamorContext.Database.SqlQuery(Of PortfolioIDto)(
                    dataQuery, params.ToArray()).ToList()
            Dim toltalC = funamorContext.Database.SqlQuery(Of Integer)(queryCount, totalCountparams.ToArray()).FirstOrDefault()

            Dim p = New PaginatedResult(Of PortfolioIDto) With {
                                                            .Data = result,
                                                            .TotalCount = toltalC
                                                        }
            Return p
        End Using
    End Function


End Class
Public Class PaginatedResult(Of T)
    Public Property Data As List(Of T)
    Public Property TotalCount As Integer
End Class

Public Class groupedreceiptDto
    Public Property Codigo As String
    Public Property Nombre As String
    Public Property Recibos As Integer
    Public Property Cobrado As Decimal?
    Public Property Lider As String
End Class
Public Class CobrosFiltersData
    Public Property EndDate As String
    Public Property StartDate As String
    Public Property LeaderCode As String
    Public Property SalesPersonCode As String
    Public Property ClientCode As String
    Public Property ValidReceiptsMark As String
    Public Property CompanyCode As String
    Public Property ZoneCode As String
    Public Property DocumentNumber As String
    Public Property ServiceId As String
    Public Property PageSize As Integer
    Public Property PageNumber As Integer
End Class