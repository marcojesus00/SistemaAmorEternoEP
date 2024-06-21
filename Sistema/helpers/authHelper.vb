Public Class authHelper
    Public Shared Function isAuthorized(user As String, permission As String) As Boolean
        If user IsNot Nothing AndAlso user.Length > 0 AndAlso permission IsNot Nothing AndAlso permission.Length > 0 Then
            Using dbContext As New FunamorContext()
                Dim permissionMatch = dbContext.Permisos.Where(Function(r) r.SegUsuario.Contains(user) AndAlso r.SegArchivo.Contains(permission)).Count()

                Return permissionMatch > 0
            End Using
        Else
            Return False
        End If

    End Function
End Class
