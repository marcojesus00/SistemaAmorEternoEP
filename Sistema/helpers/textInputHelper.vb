Public Class textInputHelper
    Public Shared Function ValidateTextLength(text As String, lengthAllowed As Integer) As Boolean
        If text.Length > lengthAllowed Then
            Return False
        End If
        Return True
    End Function

End Class
