Imports System.Web

Public Class CachingHelper
    Public Shared Function GetCachedItem(Of T)(key As String) As T
        Dim cachedItem As Object = HttpContext.Current.Cache(key)
        If cachedItem IsNot Nothing Then
            Return DirectCast(cachedItem, T)
        Else
            Return Nothing
        End If
    End Function

    Public Shared Sub SetCachedItem(Of T)(key As String, value As T, expirationSeconds As Integer)
        HttpContext.Current.Cache.Insert(key, value, Nothing, DateTime.Now.AddSeconds(expirationSeconds), System.Web.Caching.Cache.NoSlidingExpiration)
    End Sub



    Public Shared Function GetOrFetch(Of T)(theKey As String, getFromDb As Func(Of T), Optional expirationSeconds As Integer = 60) As T
        Dim cachedItem As T = CachingHelper.GetCachedItem(Of T)(theKey)

        If cachedItem IsNot Nothing Then
            Return cachedItem
        Else
            Dim newObject As T = getFromDb()
            CacheSet(theKey, newObject, expirationSeconds)
            Return newObject
        End If
    End Function
    Public Shared Sub CacheSet(Of T)(myKey As String, myObject As T, Optional expirationSeconds As Integer = 60)
        CachingHelper.SetCachedItem(myKey, myObject, expirationSeconds)
    End Sub
    Public Shared Sub CacheRemove(key As String)
        HttpContext.Current.Cache.Remove(key)
    End Sub
End Class

Public Class ControlStateManager

    Private ReadOnly _trackedValues As New Dictionary(Of String, String)

    Public Sub TrackValue(controlId As String, value As String)
        If Not _trackedValues.ContainsKey(controlId) Then
            _trackedValues.Add(controlId, value)
        End If
    End Sub
    Public Function GetValue(key As String) As Object
        If _trackedValues.ContainsKey(key) Then
            Return _trackedValues(key)
        End If
        Return Nothing
    End Function

    Public Function HasValueChanged(controlId As String, newValue As String) As Boolean
        If _trackedValues.ContainsKey(controlId) Then
            Dim oldValue As String = _trackedValues(controlId)
            Return Not String.Equals(oldValue, newValue, StringComparison.Ordinal)
        End If
        Return False
    End Function

    Public Function NewValueContainsOldValue(controlId As String, newValue As String) As Boolean
        If _trackedValues.ContainsKey(controlId) Then
            Dim oldValue As String = _trackedValues(controlId)
            Return newValue.Contains(oldValue)
        End If
        Return False
    End Function

End Class


'Public Class ControlStateManager

'    Private ReadOnly _trackedValues As New Dictionary(Of String, Object)

'    Public Sub TrackValue(controlId As String, value As Object)
'        If Not _trackedValues.ContainsKey(controlId) Then
'            _trackedValues.Add(controlId, value)
'        End If
'    End Sub

'    Public Function HasValueChanged(controlId As String, newValue As Object) As Boolean
'        If _trackedValues.ContainsKey(controlId) Then
'            Dim oldValue As Object = _trackedValues(controlId)
'            Return Not Object.Equals(oldValue, newValue)
'        End If
'        Return False
'    End Function
'    Public Function HasNewContainsOldValue(controlId As String, newValue As Object) As Boolean
'        If _trackedValues.ContainsKey(controlId) Then
'            Dim oldValue As Object = _trackedValues(controlId)
'            Return Not Object.Equals(oldValue, newValue)
'        End If
'        Return False
'    End Function
'End Class






'Imports System.Web

'Public Class cachingHelper
'    ' Set the session variable and its expiration time in seconds
'    Public Sub SetV(key As String, value As Object, expirationSeconds As Integer)
'        ' Store the value
'        HttpContext.Current.Session(key) = value
'        ' Store the expiration time
'        HttpContext.Current.Session(key & "_Expiration") = DateTime.Now.AddSeconds(expirationSeconds)
'    End Sub

'    ' Get the session variable if it hasn't expired
'    Public Function GetV(key As String) As Object
'        Dim expirationKey As String = key & "_Expiration"

'        If HttpContext.Current.Session(expirationKey) IsNot Nothing Then
'            Dim expirationTime As DateTime = CType(HttpContext.Current.Session(expirationKey), DateTime)

'            If DateTime.Now < expirationTime Then
'                ' The session variable is still valid
'                Return HttpContext.Current.Session(key)
'            Else
'                ' The session variable has expired, remove it
'                HttpContext.Current.Session.Remove(key)
'                HttpContext.Current.Session.Remove(expirationKey)
'            End If
'        End If

'        ' Return Nothing if the session variable has expired or doesn't exist
'        Return Nothing
'    End Function

'End Class
