Public Class AlertHelper

    Public Shared Sub GenerateAlert(alertType As String, alertMessage As String, alertPlaceholder As HtmlGenericControl)
        Dim tipoDeAlerta As String
        Select Case alertType
            Case "success"
                tipoDeAlerta = "¡Exito"
            Case "danger"
                tipoDeAlerta = "¡Error"

            Case "warning"
                tipoDeAlerta = "¡Atención"

            Case Else
                tipoDeAlerta = "¡Info"

        End Select

        Dim alertHtml As String = String.Format(
            "<div class=""alert alert-{0}  fixed-bottom  alert-dismissible fade show"" role=""alert"">
                <strong>{1}!</strong> {2}
              <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
            </div>",
            alertType, tipoDeAlerta.ToUpperInvariant(), alertMessage)
        alertPlaceholder.Controls.Add(New LiteralControl(alertHtml))
    End Sub
End Class
