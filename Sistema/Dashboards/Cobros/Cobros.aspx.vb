Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI.WebControls
Imports System.Data.Entity

Public Class CobrosDashboard
    Inherits System.Web.UI.Page
    Public Event AlertGenerated As EventHandler(Of AlertEventArgs)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                FillDll()
                ReBind()
            End If

        Catch ex As Exception
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try
    End Sub
    Protected Sub DashboardType_change(sender As Object, e As EventArgs) Handles DashboardType.SelectedIndexChanged
        ReBind()
    End Sub
    Protected Sub ReBind()
        Try

            If DashboardType.SelectedValue = "0" Then
                Dim dataList = GetReceiptData()
                endDate.Enabled = True
                startDate.Enabled = True
                BindGridView(dataList)
            ElseIf DashboardType.SelectedValue = "1" Then
                Dim dataList = GetPortfolioData()
                startDate.Enabled = False
                endDate.Enabled = False
                BindGridView(dataList)
            End If
        Catch ex As SqlException
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)


        Catch ex As Exception
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try
    End Sub

    Private Sub FillDll()
        endDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
        startDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
        Using context As New FunamorContext, ventasContext As New AeVentasDbContext
            Dim companies = context.Empresas.Select(Function(c) New With {c.Codigo, c.Nombre}).ToList()
            ddlCompany.DataSource = companies
            ddlCompany.DataTextField = "Nombre"
            ddlCompany.DataValueField = "Codigo"
            ddlCompany.DataBind()
            ddlCompany.Items.Insert(0, New ListItem("Seleccione una empresa", ""))
            Dim zones = ventasContext.MunicipiosZonasDepartamentos.
                Select(Function(c) New With {.Nombre = c.NombreMunicipio, .Codigo = c.MunicipioId}) _
                .OrderBy(Function(z) z.Nombre).ToList()
            ddlCity.DataSource = zones
            ddlCity.DataTextField = "Nombre"
            ddlCity.DataValueField = "Codigo"
            ddlCity.DataBind()
            ddlCity.Items.Insert(0, New ListItem("Seleccione una zona", ""))
            Dim leaders = context.Cobradores.Where(Function(c) c.Codigo = c.CobLider Or c.Codigo.Contains("4894")) _
                .Select(Function(l) New With {l.Codigo, l.Nombre}) _
                .OrderBy(Function(l) l.Nombre).ToList()
            ddlLeader.DataSource = leaders
            ddlLeader.DataTextField = "Nombre"
            ddlLeader.DataValueField = "Codigo"
            ddlLeader.DataBind()
            ddlLeader.Items.Insert(0, New ListItem("Seleccione un líder", ""))
        End Using
    End Sub

    Private Sub BindGridView(dataList As Object)


        Try

            Dim msg = ""
            DashboardGridview.DataSource = dataList
            DashboardGridview.DataBind()
            If DashboardGridview.Rows.Count = 0 Then
                msg = "No se encontraron resultados"
            Else
                msg = "Mostrando primeros " & $"{dataList.Count} resultados."

            End If
        Catch ex As SqlException
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)


        Catch ex As Exception
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try
    End Sub

    Public Function GetClientsByCollector(collectorCode As String)
        Using context As New FunamorContext, cobrosContext As New AeCobrosContext


            Return context.Clientes.Where(Function(c) c.CodigoCobrador.Contains(collectorCode)).ToList()
        End Using
    End Function
    Public Sub ClientsByCollectorMap(keyValue As String)

        Dim clients As List(Of Cliente) = GetClientsByCollector(keyValue)
        Dim markers As New List(Of MarkerForMap)
        For Each cliente As Cliente In clients
            Dim tooltipMsg = $"cliete: {cliente.Nombre}   {cliente.DireccionCliente}"
            If cliente.Latitud.ToString().Trim.Length > 0 And cliente.Longitud.ToString().Trim.Length > 0 Then
                Dim marker As New MarkerForMap With {.TooltipMessage = tooltipMsg, .Latitud = cliente.Latitud, .Longitud = cliente.Longitud, .MarkerType = MarkerTypes.Cliente}
                markers.Add(marker)
            End If

        Next

        Dim dataForMaps As New DataForMapGenerator($"Clientes del cobrador {keyValue}", markers, False)
        Session("MarkersData") = dataForMaps
        Response.Redirect("Map.aspx")

    End Sub
    Public Sub RouteOfReceiptsMap(keyValue As String)

    End Sub
    Protected Sub DashboardGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        Try

            Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
            Dim keyValue As String = DashboardGridview.DataKeys(rowIndex).Value.ToString()

            If e.CommandName = "ClientsByCollectorMap" Then
                ClientsByCollectorMap(keyValue)
            ElseIf e.CommandName = "RouteOfReceiptsMap" Then
                RouteOfReceiptsMap(keyValue)
            End If


        Catch ex As FormatException
            AlertHelper.GenerateAlert("danger", "Error al convertir el índice de fila.", alertPlaceholder)
        Catch ex As IndexOutOfRangeException
            AlertHelper.GenerateAlert("danger", "Índice de fila fuera de rango.", alertPlaceholder)
        Catch ex As IOException
            AlertHelper.GenerateAlert("danger", "Error de entrada/salida al procesar el archivo.", alertPlaceholder)
        Catch ex As Exception
            AlertHelper.GenerateAlert("danger", "Se produjo un error inesperado: " & ex.Message, alertPlaceholder)
        End Try
    End Sub
    Private Sub submitButton_Click(sender As Object, e As EventArgs) Handles submitButton.Click
        ReBind()

    End Sub
End Class