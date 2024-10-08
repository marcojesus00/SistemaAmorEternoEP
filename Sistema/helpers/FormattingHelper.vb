﻿Public Class FormattingHelper
    ' Function to format a Double amount as Lempiras currency
    Public Shared Function ToLempiras(amount As Double) As String
        Try
            ' Use the culture info for Honduras (es-HN)
            Dim cultureInfoHN As New System.Globalization.CultureInfo("es-HN")
            ' Format the amount as currency using the specified culture
            Dim lempiraString As String = String.Format(cultureInfoHN, "{0:C2}", amount)
            Return lempiraString
        Catch ex As Exception
            ' Fallback formatting if the culture is not available
            Return "L " & amount.ToString("N2")
        End Try
    End Function

    ' Overloaded function to format a Decimal amount as Lempiras currency
    Public Shared Function ToLempiras(amount As Decimal?) As String
        Try
            If amount IsNot Nothing Then
                Dim cultureInfoHN As New System.Globalization.CultureInfo("es-HN")
                ' Format the amount as currency using the specified culture
                Dim lempiraString As String = String.Format(cultureInfoHN, "{0:C2}", amount)
                Return lempiraString
            Else
                Return "L 0.00"
            End If
            ' Use the culture info for Honduras (es-HN)

        Catch ex As Exception
            ' Fallback formatting if the culture is not available
            Return "L " & amount.ToString("N2")
        End Try
    End Function

    Public Shared Function ToLempiras(amount As Decimal) As String
        Try
            Dim cultureInfoHN As New System.Globalization.CultureInfo("es-HN")
            ' Format the amount as currency using the specified culture
            Dim lempiraString As String = String.Format(cultureInfoHN, "{0:C2}", amount)
                Return lempiraString

            ' Use the culture info for Honduras (es-HN)

        Catch ex As Exception
            ' Fallback formatting if the culture is not available
            Return "L " & amount.ToString("N2")
        End Try
    End Function
    ' Function to format a string amount as Lempiras currency
    Public Shared Function ToLempiras(amountString As String) As String
        Dim amount As Double
        If Not Double.TryParse(amountString, amount) Then
            If amountString IsNot Nothing Then
                Throw New ArgumentException("Input string is not in a correct format.")
            End If
        End If
        ' Call the function to format the parsed Double amount
        Return ToLempiras(amount)
    End Function


    Public Shared Function MarcaToNulo(mark As String, liquida As String, liquida2 As String) As String
        mark = mark.Trim()
        liquida = liquida.Trim()
        liquida2 = liquida2.Trim()
        If mark IsNot Nothing AndAlso mark.Contains("X") Then
            Return "Nulo"
        ElseIf mark.Contains("N") Then
            If liquida2 IsNot Nothing AndAlso liquida2.Length > 5 Then
                Return "Procesado en la web"
            ElseIf liquida IsNot Nothing AndAlso liquida.Length > 5 Then
                Return "Liquidado en dispositivo"
            End If

            Return "No liquidado en dispositivo"

        End If
        Return "Recibo corrupto"

    End Function
End Class
