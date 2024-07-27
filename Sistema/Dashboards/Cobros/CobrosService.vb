Imports System.Data.SqlClient
Imports Sistema.CobrosDashboard

Public Class CobrosService

    Public Class CobrosParams
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
    Public Function getCobrosReceiptsFromDB(params As CobrosParams) As Object
        Dim endD = params.EndDate
        Dim initD = params.StartDate

        Dim query As String
        Using funamorContext As New FunamorContext
            funamorContext.Database.Log = Sub(s) System.Diagnostics.Debug.WriteLine(s)
            Dim leaderCode = params.LeaderCode
            Dim collectorCode = params.SalesPersonCode
            Dim ClientCode = params.ClientCode
            Dim mark = params.ValidReceiptsMark
            Dim companyCode = params.CompanyCode
            Dim ZoneCode = params.ZoneCode
            Dim documentNumber = params.DocumentNumber

            query = "
            SELECT r.Num_doc, r.RFECHA, r.codigo_cobr, LTRIM(RTRIM(cb.nombre_cobr)) as nombre_cobr, cb.cob_lider, c.Codigo_clie, LTRIM(RTRIM(c.Nombre_clie)) as Nombre_clie, r.Por_lempira, c.Saldo_actua, r.SALDOANT, r.MARCA, r.rhora, c.Cod_zona, c.VZCODIGO,r.liquida, r.liquida2, r.LATITUD, r.LONGITUD
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
                    Dim LeaderCodeParam As String = "%" & leaderCode & "%"
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
                    Throw New ArgumentException("Invalid date format for start or end date.")
                End If
            Catch ex As Exception
                'DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
                Throw New Exception("Problema al recibir información de la base de datos.", ex)
            End Try
        End Using
    End Function
    Public Function getCobrosLocationReceipt()
        Dim result As New Point(15, 87)
        Return result
    End Function
    Public Function GetRecepits(cobrador)
        Dim result As New List(Of RecibosDTO)
        Return result
    End Function
End Class
