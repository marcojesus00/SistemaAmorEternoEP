Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI.WebControls
Imports System.Data.Entity
Imports System.Linq

Public Class CobrosDashboard
    Inherits System.Web.UI.Page
    Public Event AlertGenerated As EventHandler(Of AlertEventArgs)
    Private _receipts As List(Of ReciboDeCobro)
    Public Property ReceiptsByDateCachedList As List(Of RecibosDTO)
        Get
            Return CachingHelper.GetOrFetch("ReceiptsByDate", AddressOf getReceiptsFromDB, 100)
        End Get
        Set(value As List(Of RecibosDTO))
            CachingHelper.CacheSet("ReceiptsByDate", value, 100)
        End Set
    End Property
    Public Property CollectorsCachedList As List(Of SimpleCollectorDto)
        Get
            Return CachingHelper.GetOrFetch("Collectors", AddressOf GetCollectorsFromDb, 150)
        End Get
        Set(value As List(Of SimpleCollectorDto))
            CachingHelper.CacheSet("Collectors", value, 150)
        End Set
    End Property
    Public Property ClientsContainsCollectorCachedList As List(Of Cliente)
        Get
            Return CachingHelper.GetOrFetch("ClientsWithRemainingBalance", AddressOf GetClientsByCollectorIdFromDb, 150)
        End Get
        Set(value As List(Of Cliente))
            CachingHelper.CacheSet("ClientsWithRemainingBalance", value, 150)
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                FillDll()
                ReBind()
            End If
            AddHandler DashboardGridview.PageIndexChanging, AddressOf DashboardGridview_PageIndexChanging

        Catch ex As Exception
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try
    End Sub
    Protected Sub DashboardType_change(sender As Object, e As EventArgs) Handles DashboardType.SelectedIndexChanged
        ReBind()
    End Sub

    'Fill the gridviews 
    Protected Sub ReBind()
        Try

            If DashboardType.SelectedValue = "0" Then
                Dim dataList = GetReceiptDataForGridview()
                endDate.Enabled = True
                startDate.Enabled = True
                BindGridView(dataList)
            ElseIf DashboardType.SelectedValue = "1" Then
                startDate.Enabled = False
                endDate.Enabled = False
                DashboardGridview.DataSource = Nothing
                DashboardGridview.DataBind()
                If textBoxCode.Text.Trim.Length <= 2 AndAlso ddlCompany.SelectedValue.Trim = "" AndAlso ddlLeader.SelectedValue.Trim = "" And ddlCity.SelectedValue.Trim = "" Then
                    Dim msg = "Por favor refine su búsqueda"
                    AlertHelper.GenerateAlert("warning", msg, alertPlaceholder)
                Else
                    Dim dataList = GetPortfolioDataForGridview()

                    BindGridView(dataList)
                End If

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




    Protected Sub DashboardGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        Try

            Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
            Dim keyValue As String = DashboardGridview.DataKeys(rowIndex).Value.ToString()

            If e.CommandName = "ClientsByCollectorMap" Then
                ClientsByCollectorMap(keyValue)
            ElseIf e.CommandName = "RouteOfReceiptsMap" Then
                If textBoxClientCode.Text.Length > 0 Then
                    textBoxClientCode.Text = ""
                End If
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
    Protected Sub DashboardGridview_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        ' Handle the PageIndexChanging event here
        DashboardGridview.PageIndex = e.NewPageIndex
        ' Rebind your GridView data here
        ReBind() ' Example method to bind data
    End Sub
    Private Sub submitButton_Click(sender As Object, e As EventArgs) Handles submitButton.Click
        ReBind()

    End Sub
    Public Sub StartDate_OnTextChanged(sender As Object, e As EventArgs) Handles startDate.TextChanged
        CachingHelper.CacheRemove("ReceiptsByDate")
    End Sub
    Public Sub EndDate_OnTextChanged(sender As Object, e As EventArgs) Handles endDate.TextChanged
        CachingHelper.CacheRemove("ReceiptsByDate")

    End Sub
    Public Sub CLientCode_OnTextChanged(sender As Object, e As EventArgs) Handles textBoxClientCode.TextChanged
        CachingHelper.CacheRemove("ReceiptsByDate")
    End Sub
End Class