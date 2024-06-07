Imports System.IO
Imports Newtonsoft.Json

Public Class CobrosDashboard
    Inherits System.Web.UI.Page
    Public Event AlertGenerated As EventHandler(Of AlertEventArgs)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
            FillDll()
            BindGridView()
        End If

        Catch ex As Exception
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try
    End Sub
    Private Sub FillDll()
        Using context As New MyDbContext
            Dim companies = context.Empresas.Select(Function(c) New With {c.Codigo, c.Nombre}).ToList()
            ddlCompany.DataSource = companies
            ddlCompany.DataTextField = "Nombre"
            ddlCompany.DataValueField = "Codigo"
            ddlCompany.DataBind()
            ddlCompany.Items.Insert(0, New ListItem("Seleccione una empresa", ""))
            Dim cities = context.Municipios.Select(Function(c) New With {c.Codigo, c.Nombre}).ToList()
            ddlCity.DataSource = cities
            ddlCity.DataTextField = "Nombre"
            ddlCity.DataValueField = "Codigo"
            ddlCity.DataBind()
            ddlCity.Items.Insert(0, New ListItem("Seleccione una zona", ""))

        End Using
    End Sub
    Private Sub BindGridView(Optional str As String = "")


        Try

            Dim msg = ""
            Dim dataList = GetData()

            DashboardGridview.DataSource = dataList
            DashboardGridview.DataBind()
            If DashboardGridview.Rows.Count = 0 Then
                msg = "No se encontraron resultados"
            Else
                msg = "Mostrando primeros " & $"{dataList.Count} resultados."

            End If


        Catch ex As Exception
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try
    End Sub
    Public Function GetData()
        Dim collectorCode = textBoxCode.Text.Trim
        Dim endD As Date
        Dim initD As Date
        If endDate.Text IsNot "" Then
            endD = CType(endDate.Text, Date).Date
        Else
            endD = Date.Now().Date
        End If
        If startDate.Text IsNot "" Then
            initD = CType(startDate.Text, Date).Date
        Else
            initD = Date.Now().Date

        End If
        Dim collectors As List(Of Cobrador)
        Using context As New MyDbContext, cobrosContext As New AeCobrosContext

            collectors = context.Cobradores.ToList()
            'cobrosContext.RecibosDeCobro.Where(Function(r) r.Rfecha > "2024-06-03").Select(Function(r) New With {r.NumeroDeRecibo, r.PorLempira, r.Cliente.NombreCliente, r.Cobrador.NombreCobr, r.Rfecha}).OrderByDescending(Function(r) r.Rfecha).Take(10)

            Dim leader As String = ""
            leader = ddlLeader.DataValueField.ToString()
            Dim bills = cobrosContext.RecibosDeCobro.Where(Function(r) r.Rfecha >= initD And r.Rfecha <= endD And r.CodigoCobr.Contains(collectorCode) And r.Cobrador.NombreCobr.Trim.Length > 0)
            Dim gbills = bills.GroupBy(Function(r) r.CodigoCobr).Select(Function(r) New With {.Codigo = r.Key, .Recibos = r.Count(Function(w) w.PorLempira), .Cobrado = r.Sum(Function(c) c.PorLempira)}).ToList()
            Dim data = gbills.Join(collectors, Function(e1) e1.Codigo,
                                            Function(e2) e2.CodigoCobr,
                                            Function(e1, e2) New With {
                                                e1.Codigo,
                                               e2.NombreCobr,
                                               e1.Recibos,
                                               e1.Cobrado
                                            }).ToList()
            Return data

        End Using
    End Function
    Public Function GetClientsByCollector(collectorCode As String)
        Using context As New MyDbContext, cobrosContext As New AeCobrosContext


            Return context.Clientes.Where(Function(c) c.CodigoCobrador.Contains(collectorCode)).ToList()
        End Using
    End Function
    Protected Sub DashboardGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        If e.CommandName = "ShowMap" Then

            Try

                Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
                Dim keyValue As String = DashboardGridview.DataKeys(rowIndex).Value.ToString()


                Dim items As New List(Of TransactionMapDto)()
                Dim a As New TransactionMapDto With {.CodigoDeCobrador = "4150", .NombreDeCobrador = "Camilo", .CodigoDelCLiente = "M05-02121", .NombreDelCliente = "Fernanda Antonnia Guzman Perez", .TipoDeTransaccion = "cobro", .Cantidad = "300.00", .Fecha = DateTime.Now.ToString("yyyy/MM/dd"), .Hora = DateTime.Now.ToString("HH:mm:ss"), .Latitud = "15.403546", .Longitud = "-87.810689"}
                Dim b As New TransactionMapDto With {.CodigoDeCobrador = "4150", .NombreDeCobrador = "Camilo", .CodigoDelCLiente = "M05-02125", .NombreDelCliente = "Juan Cupertino Rivas Sanchez", .TipoDeTransaccion = "visita", .Cantidad = "302.00", .Fecha = DateTime.Now.ToString("yyyy/MM/dd"), .Hora = DateTime.Now.ToString("HH:mm:ss"), .Latitud = "15.401666", .Longitud = "-87.803231"}
                Dim c As New TransactionMapDto With {.CodigoDeCobrador = "4150", .NombreDeCobrador = "Camilo", .CodigoDelCLiente = "M05-C0425", .NombreDelCliente = "Marcelo Jose Alvarado Torres", .TipoDeTransaccion = "visita", .Cantidad = "2000.00", .Fecha = DateTime.Now.ToString("yyyy/MM/dd"), .Hora = DateTime.Now.ToString("HH:mm:ss"), .Latitud = "15.398078", .Longitud = "-87.809251"}
                Dim clients As List(Of Cliente) = GetClientsByCollector(keyValue)
                Dim markers As New List(Of MarkerForMap)
                For Each cliente As Cliente In clients
                    Dim tooltipMsg = $"cliete: {cliente.NombreCliente}   {cliente.DireccionCliente}"
                    If cliente.Latitud.ToString().Trim.Length > 0 And cliente.Longitud.ToString().Trim.Length > 0 Then
                        Dim marker As New MarkerForMap With {.TooltipMessage = tooltipMsg, .Latitud = cliente.Latitud, .Longitud = cliente.Longitud, .MarkerType = MarkerTypes.Cliente}
                        markers.Add(marker)
                    End If

                Next

                If markers.Count > 0 Then

                    Dim dataForMaps As New DataForMapGenerator($"Clientes del cobrador {keyValue}", markers, False)
                    Session("MarkersData") = dataForMaps
                    Response.Redirect("Map.aspx")

                Else
                    AlertHelper.GenerateAlert("warning", "No se encontraron registros para mostrar en el mapa.", alertPlaceholder)

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
        End If
    End Sub
    Private Sub submitButton_Click(sender As Object, e As EventArgs) Handles submitButton.Click
        BindGridView()

    End Sub
End Class