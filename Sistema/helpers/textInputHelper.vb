Public Class TextInputHelper
    Public Shared Function ValidateTextLength(text As String, lengthAllowed As Integer) As Boolean
        If text.Length > lengthAllowed Then
            Return False
        End If
        Return True
    End Function
    Public Function PreventDefaultAction(ByVal e As EventArgs) As Boolean
        ' Set e.Handled to True only if it's a valid EventArgs object
        If e IsNot Nothing Then
            'e.Cancel = True
            Return True
        Else
            Return False ' Indicate that e.Handled wasn't set
        End If
    End Function

    Public Shared Function FormatWithHyphens(inputString As String) As String
        Dim result As String = ""
        Dim count As Integer = 0

        For Each c As Char In inputString
            result &= c
            count += 1
            If inputString.Length > 8 Then
                If count Mod 4 = 0 AndAlso count < 9 Then
                    result &= "-"
                End If
            Else
                If count Mod 4 = 0 AndAlso count < 8 Then
                    result &= "-"
                End If
            End If

        Next

        Return result
    End Function

End Class

