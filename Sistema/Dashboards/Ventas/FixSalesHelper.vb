Imports System.Data.SqlClient

Public Class FixSalesHelper


    Public Function CorrectTheSameCLientManyTimes(salesPerson As String, clientCode As String)
        Dim message = "Error"
        Using context As New FunamorContext()
            Dim sqlParameters As New List(Of SqlParameter)
            sqlParameters.Add(New SqlParameter("@Client", clientCode))
            sqlParameters.Add(New SqlParameter("@SalesPerson", salesPerson))

            message = context.Database.SqlQuery(Of String)("[dbo].[SP_VS_ESTABLECER_NUEVO_CODIGO_DE_CLIENTE]
		@CodigoDeCliente = @Client,
		@CodigoDeVendedor = @SalesPerson", sqlParameters).FirstOrDefault()
        End Using
        Return message
    End Function

End Class
