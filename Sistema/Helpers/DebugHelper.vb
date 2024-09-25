Imports System.Globalization
Imports System.IO
Imports System.Threading

Public Class DebugHelper

    Public Shared Function SendDebugInfo(alertType As String, exception As Exception, user As String, Optional detail As String = "") As String
        If TypeOf exception IsNot ThreadAbortException Then
            Try
                ' Basic information
                Dim timestamp As String = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")
                Dim serverName As String = Environment.MachineName
                Dim appVersion As String = "" ' Add actual app version if needed
                Dim userInfo As String = $"User: {HttpContext.Current.User.Identity.Name}"
                Dim environmentInfo As String = $"Server: {serverName}, Application Version: {appVersion}"
                Dim innerExceptionMsg As String = If(exception.InnerException IsNot Nothing, exception.InnerException.Message, "None")

                ' Build the debug message
                Dim logBuilder As New StringBuilder()
                logBuilder.AppendLine($"[ERROR] {timestamp} HResult: {exception.HResult}")
                logBuilder.AppendLine()
                logBuilder.AppendLine($"User: {user}")
                logBuilder.AppendLine($"Environment: {environmentInfo}")
                logBuilder.AppendLine($"Alert Type: {alertType}")
                logBuilder.AppendLine($"exception Type: {exception.GetType().Name}")
                logBuilder.AppendLine()
                logBuilder.AppendLine($"Message: {exception.Message}")
                logBuilder.AppendLine($"Detail: {detail}")
                logBuilder.AppendLine($"Target Site: {exception.TargetSite}")
                logBuilder.AppendLine($"Inner Exception: {innerExceptionMsg}")
                logBuilder.AppendLine("Stack Trace:")
                logBuilder.AppendLine(exception.StackTrace)
                logBuilder.AppendLine()
                logBuilder.AppendLine(New String("-"c, 80)) ' Separator
                logBuilder.AppendLine()
                logBuilder.AppendLine()

                Dim serverPath As String = ConfigurationManager.AppSettings("ServerPath")
                Dim logFilePath As String = Path.Combine(serverPath, "Musica\Logs", $"{serverName}_{DateTime.Now:dddd_dd_MMMM_yyyy}.log")

                Dim logDir As String = Path.GetDirectoryName(logFilePath)
                If Not Directory.Exists(logDir) Then Directory.CreateDirectory(logDir)

                Using writer As New StreamWriter(logFilePath, True)
                    writer.WriteLine(logBuilder.ToString())
                End Using

                Return logBuilder.ToString()
            Catch logEx As Exception
                Return $"Failed to write log. Error: {logEx.Message}"
            End Try
        End If

        Return False
    End Function

End Class
