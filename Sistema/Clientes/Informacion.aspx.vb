Public Class Informacion
    Inherits System.Web.UI.Page
    Public Event SendTextboxSendEvent As EventHandler(Of TextboxEventArgs)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario") = "" Then
            Response.Redirect("~/inicio.aspx")
        End If
        AddHandler SendTextboxSendEvent, AddressOf ClientsTable1.OnDataReceived
        AddHandler ClientsTable1.AlertGenerated, AddressOf HandleAlertGenerated

    End Sub
    Protected Sub textBoxCode_TextChanged() Handles textBoxCode.TextChanged
        RaiseEvent SendTextboxSendEvent(Me, New TextboxEventArgs(textBoxCode.Text))
    End Sub
    Protected Sub HandleAlertGenerated(ByVal sender As Object, ByVal e As AlertEventArgs)
        Dim message As String = e.Message
        Dim alertType As String = e.AlertType
        AlertHelper.GenerateAlert(alertType, message, alertPlaceholder)
    End Sub
End Class

Public Class TextboxEventArgs
    Inherits EventArgs

    Public Property str
    Public Sub New(str)
        Me.str = str
    End Sub

End Class
