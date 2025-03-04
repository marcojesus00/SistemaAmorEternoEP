Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Estadistica_Visitas_WhatsApp()
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, "reports")
        End Try


    End Sub
    Public Sub Estadistica_Visitas_WhatsApp()
        Dim today = DateTime.Now.ToString("yyyy-MM-dd")
        Dim todayFormatted = DateTime.Now.ToString("dd-MM-yyyy")

        Dim OneMonthAgo = DateTime.Now.AddMonths(-1).AddDays(1).ToString("yyyy-MM-dd")
        Dim Informe = New Estadistica_Visitas

        Informe.SetDatabaseLogon("sistema.web", "$$Eterno4321.")
        Informe.SetParameterValue("F1", OneMonthAgo)
        Informe.SetParameterValue("F2", today)

        Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
        Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
        'exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
        'exportOpts.ExportFormatOptions = pdfOpts
        'Informe.ExportToHttpResponse(exportOpts, Response, False, "Estadistica_Visitas")
        Dim cap = $"Estadistica de visitas de cobradores de {todayFormatted}"
        Dim instance = "collections1"
        Using context As New MemorialContext()

            Dim sqlQuery = "select nombre_cobr Name,rtrim(cob_telefo) Phone from funamor..COBRADOR where codigo_cobr=cob_lider"
            Dim leaders = context.Database.SqlQuery(Of LeaderDto)(sqlQuery).ToList()
            For Each leader In leaders
                Dim pdf As System.IO.Stream = Informe.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat)
                Dim phone = leader.Phone
#If DEBUG Then
                phone = "95268888"
#End If
                Dim res As ResultW = whatsapi.sendWhatsAppDocs2(doc:=pdf, name:=$"Estadisitca_{todayFormatted}.pdf", countryCode:="504", localNumber:=phone, caption:=cap, user:="manager", instancia:=instance)
                'DebugHelper.SendDebugInfo("danger", New Exception, leader.Name)
            Next
            Informe.Close()
            Informe.Dispose()
        End Using

    End Sub
    Public Class LeaderDto
        Public Property Name As String
        Public Property Phone As String
    End Class

End Class