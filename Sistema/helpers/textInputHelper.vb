Public Class textInputHelper
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

End Class

