Imports System.Globalization
Imports System.IO
Imports System.Threading

Public Class DebugHelper

    Public Shared Function SendDebugInfo(alertType As String, exception As Exception, user As String, Optional detail As String = "") As String
        If TypeOf exception IsNot ThreadAbortException Then
            Dim ServerPath As String = ConfigurationManager.AppSettings("ServerPath")
            Dim app As String = "" ' Replace with actual application version if needed
            Dim timestamp As String = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")
            Dim stackTrace As String = Environment.StackTrace
            Dim userInfo As String = "User: " & HttpContext.Current.User.Identity.Name ' Example for web applications
            Dim authUser As String = user
            Dim environmentInfo As String = $"Server: {Environment.MachineName}, Application Version: {app}"
            Dim debugInfo As String = vbCrLf & $"Alert Type: {alertType}" & vbCrLf &
                                                  $"Alert Message: {exception.Message}" & vbCrLf &
                                                  $"Detail: {detail}" & vbCrLf &
                                                 $"Timestamp: {timestamp}" & vbCrLf &
                                              $"HResult error code: {exception.HResult}" & vbCrLf & vbCrLf &
                                              $"Exception type name: {exception.GetType().Name}" & vbCrLf &
                                  $"TargetSite: {exception.TargetSite}" & vbCrLf &
                                  $"Inner exception: {exception.InnerException}" & vbCrLf &
                                  $"User: {authUser}" & vbCrLf &
                                  $"Server: {Environment.MachineName}" & vbCrLf & vbCrLf &
                                  $"Stack Trace:" & vbCrLf & exception.StackTrace

            ' Path to the log file
            Dim monthName As String = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture)
            Dim year As String = DateTime.Now.ToString("yyyy")
            Dim dayName As String = DateTime.Now.ToString("dddd")
            Dim day As String = DateTime.Now.ToString("dd")

            Dim logFilePath As String = Path.Combine(ServerPath, $"Musica\Logs\{Environment.MachineName}_{dayName}_{day}_{monthName}_{year}.log")


            ' Ensure the directory exists
            Dim logDir As String = Path.GetDirectoryName(logFilePath)
            If Not Directory.Exists(logDir) Then
                Directory.CreateDirectory(logDir)
            End If

            ' Write the debug information to the log file
            Try
                Using writer As StreamWriter = New StreamWriter(logFilePath, True)
                    writer.WriteLine(debugInfo)
                    writer.WriteLine(New String("-"c, 80)) ' Separator line
                End Using
            Catch ex As Exception
                ' Handle any errors that might occur while writing to the log file
                ' For example, you might log this error to a different logging mechanism
            End Try

            Return debugInfo
        End If
        Return False
    End Function

End Class
