Imports System.Data.SqlClient

Public Class FixHelper
    Public Shared Function ChangeCorrelative(documentNumber As String)
        Dim random As New Random()

        If documentNumber.Length >= 9 Then
            Dim randomNum1 As Integer = random.Next(0, 10) ' Random number between 0-9
            Dim randomNum2 As Integer = random.Next(0, 10) ' Random number between 0-9

            Dim modifiedString As String = documentNumber.Substring(0, 7) & randomNum1.ToString() & randomNum2.ToString() & documentNumber.Substring(9)


            Return modifiedString
        Else
            Throw New Exception("String is too short to modify.")
        End If
    End Function
    Public Shared Function FixDbCorrelative(documentNumber As String) As String
        Dim NewNumDoc = ChangeCorrelative(documentNumber)
        Using context As New AeVentasDbContext()
            Dim parameters() = {
                New SqlParameter("@DocumentNumber", documentNumber),
                New SqlParameter("@NewNumDoc", NewNumDoc)}
            Dim Sql = "EXEC SP_VS_CORREGIR_CORRELATIVO_DE_RECIBO @NewNumDoc=@NewNumDoc, @DocumentNumber = @DocumentNumber
"
            Dim result As String = context.Database.SqlQuery(Of String)(Sql, parameters).FirstOrDefault()
            Return result
        End Using
    End Function
End Class
