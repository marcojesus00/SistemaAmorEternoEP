Public Class AuthHelper
    Private Shared permissionsCache As New Dictionary(Of String, Boolean)

    Public Shared Function isAuthorized(user As String, permission As String) As Boolean
        If Not String.IsNullOrEmpty(user) AndAlso Not String.IsNullOrEmpty(permission) Then
            Dim cacheKey = $"{user}-{permission}"
            If permissionsCache.ContainsKey(cacheKey) Then
                Return permissionsCache(cacheKey)
            End If

            Using dbContext As New FunamorContext()
                Dim hasPermission = dbContext.Permisos.AsNoTracking().
                    Where(Function(r) r.SegUsuario = user AndAlso r.SegArchivo.Trim() = permission).
                    Any()

                ' Cache the result
                permissionsCache(cacheKey) = hasPermission
                Return hasPermission
            End Using
        Else
            Return False
        End If
    End Function
End Class
