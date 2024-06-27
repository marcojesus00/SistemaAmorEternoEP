Public Class LinkHelper
    Public Shared Sub OpenLinkInNewTab(ByVal page As Page, ByVal url As String)
        Dim script As String = "window.open('" & url & "', '_blank');"
        page.ClientScript.RegisterStartupScript(page.GetType(), "OpenLink", script, True)
    End Sub
End Class
