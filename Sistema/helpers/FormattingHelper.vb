Public Class FormattingHelper
    Public Shared Function ToLempiras(amount As Double)
        Dim lempiraString As String = String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-HN"), "{0:C2}", amount)

        Return lempiraString
    End Function
    Public Shared Function ToLempiras(amountString As String) As String

        Dim amount As Double
        If Not Double.TryParse(amountString, amount) Then
            If amountString IsNot Nothing Then
                Throw New ArgumentException("Input string is not in a correct format.")

            End If
        End If

        Return ToLempiras(amount)
    End Function
End Class
