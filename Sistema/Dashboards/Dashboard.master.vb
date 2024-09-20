Public Class Dashboard
    Inherits System.Web.UI.MasterPage
    'Public PageNumber As Integer = 1
    'Public PageSize As Integer = 10
    'Public TotalPages As Integer '
    'Public TotalItems As Integer = 0
    'Public itemText As String
    'Public pagination As PaginationHelper = New PaginationHelper
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    'Protected Sub lnkbtnPage_Click(sender As Object, e As EventArgs)
    '    Dim lnkButton As LinkButton = CType(sender, LinkButton)
    '    Dim resultInteger As Integer
    '    If Integer.TryParse(lnkButton.Text, resultInteger) Then
    '        Dim SelectedPageDashboardVentas As Integer = Integer.Parse(lnkButton.Text)
    '        Session("SelectedPageDashboardVentas") = SelectedPageDashboardVentas
    '        ReBind(selectedPage:=SelectedPageDashboardVentas)
    '    End If

    'End Sub

    'Protected Sub lnkbtnPrevious_Click(sender As Object, e As EventArgs)

    '    If Session("SelectedPageDashboardVentas") IsNot Nothing AndAlso Session("SelectedPageDashboardVentas") > 1 Then
    '        Session("SelectedPageDashboardVentas") = Session("SelectedPageDashboardVentas") - 1

    '        ReBind(selectedPage:=Session("SelectedPageDashboardVentas"))

    '    End If

    'End Sub

    'Protected Sub lnkbtnNext_Click(sender As Object, e As EventArgs)
    '    'filterData.PageNumber = filterData.PageNumber + 1
    '    If Session("SelectedPageDashboardVentas") IsNot Nothing Then
    '        Session("SelectedPageDashboardVentas") = Session("SelectedPageDashboardVentas") + 1
    '        ReBind(selectedPage:=Session("SelectedPageDashboardVentas"))
    '    End If

    'End Sub

    'Protected Sub btnExitWhatsapToAll_Click(sender As Object, e As EventArgs)
    '    PnlGoodAndBadPhones.Visible = False
    '    PnlPrimary.Visible = True
    '    PnlBasPhones.Visible = False
    'End Sub

    'Protected Sub btnSendMassiveWhatsApp_Click(sender As Object, e As EventArgs)
    '    If Session("CobradorSeleccionado") Then
    '        Dim clients As List(Of DocsDto) = clientsValidToSend(Session("CobradorSeleccionado"))
    '        EnviarEstadoDeCuenta(clients)

    '    End If
    'End Sub

    'Protected Sub SendGridview_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
    '    SendGridview.PageIndex = e.NewPageIndex
    '    rebindGoodPhones()
    'End Sub


    'Protected Sub GridViewBadPhones_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
    '    GridViewBadPhones.PageIndex = e.NewPageIndex
    '    rebindBadPhones()
    'End Sub
End Class