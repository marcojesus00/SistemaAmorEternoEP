Public Class DebugHelper
    Private Shared Function SendDebugInfo(alertType As String, alertMessage As String) As String
        Dim app = "" 'y.Application.Info.Version.ToString()
        Dim timestamp As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        Dim stackTrace As String = Environment.StackTrace
        Dim userInfo As String = "User: " & HttpContext.Current.User.Identity.Name ' Example for web applications
        Dim authUser = ""
        Dim environmentInfo As String = $"Server: {Environment.MachineName}, Application Version: {app}"
        Dim debugInfo As String = $"Timestamp: {timestamp}<br>" &
                              $"Alert Type: {alertType}<br>" &
                              $"Alert Message: {alertMessage}<br>" &
                              $"User Info: {userInfo}<br>" &
                              $"Environment Info: {environmentInfo}<br><br>" &
                              $"Stack Trace:<br>{stackTrace}"

        Return debugInfo
    End Function

End Class
