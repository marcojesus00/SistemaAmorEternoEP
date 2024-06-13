Imports System.Web

Public Class cachingHelper
    ' Set the session variable and its expiration time in seconds
    Public Sub SetV(key As String, value As Object, expirationSeconds As Integer)
        ' Store the value
        HttpContext.Current.Session(key) = value
        ' Store the expiration time
        HttpContext.Current.Session(key & "_Expiration") = DateTime.Now.AddSeconds(expirationSeconds)
    End Sub

    ' Get the session variable if it hasn't expired
    Public Function GetV(key As String) As Object
        Dim expirationKey As String = key & "_Expiration"

        If HttpContext.Current.Session(expirationKey) IsNot Nothing Then
            Dim expirationTime As DateTime = CType(HttpContext.Current.Session(expirationKey), DateTime)

            If DateTime.Now < expirationTime Then
                ' The session variable is still valid
                Return HttpContext.Current.Session(key)
            Else
                ' The session variable has expired, remove it
                HttpContext.Current.Session.Remove(key)
                HttpContext.Current.Session.Remove(expirationKey)
            End If
        End If

        ' Return Nothing if the session variable has expired or doesn't exist
        Return Nothing
    End Function

End Class
