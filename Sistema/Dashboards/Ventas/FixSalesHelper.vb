Imports System.Data.SqlClient

Public Class FixSalesHelper


    Public Function CorrectTheSameCLientManyTimes(salesPerson As String, clientCode As String)
        Dim message = "Error"
        Using context As New AeVentasDbContext()
            Dim sqlParameters As SqlParameter() = {
    New SqlParameter("@Client", clientCode),
    New SqlParameter("@SalesPerson", salesPerson)
}

            message = context.Database.SqlQuery(Of String)(
    "EXEC aeventas.[dbo].[SP_VS_ESTABLECER_NUEVO_CODIGO_DE_CLIENTE] @CodigoDeCliente = @Client, @CodigoDeVendedor = @SalesPerson",
    sqlParameters).FirstOrDefault()


        End Using
        Return message
    End Function

    Public Function ChangeSalesPerson(salesPerson As String, clientCode As String, newSalesPerson As String)
        Dim message = "Error"
        Using context As New FunamorContext()
            Dim sqlParameters As SqlParameter() = {
            New SqlParameter("@Client", clientCode),
            New SqlParameter("@SalesPerson", salesPerson),
            New SqlParameter("@NewSalesPerson", newSalesPerson)
            }
            message = context.Database.SqlQuery(Of String)("aeventas.[dbo].[SP_VS_ESTABLECER_NUEVO_VENDEDOR]
		@Client = @Client,
		@OldSalesPerson = @SalesPerson,
@NewSalesPerson=@NewSalesPerson", sqlParameters).FirstOrDefault()
        End Using
        Return message
    End Function

End Class
