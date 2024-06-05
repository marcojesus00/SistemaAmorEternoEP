Imports System.IO
Imports Newtonsoft.Json

Public Class CobrosDashboard
    Inherits System.Web.UI.Page
    Public Event AlertGenerated As EventHandler(Of AlertEventArgs)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BindGridView()
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
        Dim dataCollector
        Using context As New MyDbContext

            dataCollector = context.Cobrador.Where(Function(c) c.CodigoCobr)
            'cobrosContext.RecibosDeCobro.Where(Function(r) r.Rfecha > "2024-06-03").Select(Function(r) New With {r.NumeroDeRecibo, r.PorLempira, r.Cliente.NombreCliente, r.Cobrador.NombreCobr, r.Rfecha}).OrderByDescending(Function(r) r.Rfecha).Take(10)
        End Using
        Using cobrosContext As New AeCobrosContext

            Dim bills = cobrosContext.RecibosDeCobro.Where(Function(r) r.Rfecha >= initD And r.Rfecha <= endD And r.CodigoCobr.Contains(collectorCode) And r.Cobrador.NombreCobr.Trim.Length > 0)
            Dim gbills = bills.GroupBy(Function(r) r.CodigoCobr).Select(Function(r) New With {.Codigo = r.Key, .Recibos = r.Count(Function(d) d.PorLempira), .Cobrado = r.Sum(Function(c) c.PorLempira)})
            Dim data = gbills.ToList()
            Return data

        End Using
    End Function
    Protected Sub DashboardGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        If e.CommandName = "ShowMap" Then

            Try

                'Dim args As String() = e.CommandArgument.ToString().Split("|"c)
                'Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
                'Dim row As GridViewRow = DashboardGridview.Rows(rowIndex)
                'Dim documentName As String = row.Cells(0).Text
                'Using dbContext As New MyDbContext
                '    Dim DocId As Integer = DashboardGridview.DataKeys(rowIndex).Value.ToString()
                '    Dim record As UrlCliente = dbContext.urlClientes.Find(DocId)
                '    Dim documentPath = record.RutaDelArchivo
                'End Using
                Dim items As New List(Of TransactionMapDto)()
                Dim a As New TransactionMapDto With {
                    .CodigoDeCobrador = "4150", .NombreDeCobrador = "Camilo", .CodigoDelCLiente = "M5C000002121", .NombreDelCliente = "Fernanda Antonnia Guzman Perez", .TipoDeTransaccion = "cobro", .Cantidad = "300.00", .Fecha = DateTime.Now.ToString("yyyy/MM/dd"), .Hora = DateTime.Now.Hour.ToString(), .Latitud = "15.403546", .Longitud = "-87.810689"}
                Dim b As New TransactionMapDto With {.CodigoDeCobrador = "4150", .NombreDeCobrador = "Camilo", .CodigoDelCLiente = "M5C000002125", .NombreDelCliente = "Juan Cupertino Rivas Sanchez", .TipoDeTransaccion = "visita", .Cantidad = "302.00", .Fecha = DateTime.Now.ToString("yyyy/MM/dd"), .Hora = DateTime.Now.Hour.ToString(), .Latitud = "15.401666", .Longitud = "-87.803231"}
                Dim c As New TransactionMapDto With {.CodigoDeCobrador = "4150", .NombreDeCobrador = "Camilo", .CodigoDelCLiente = "M5C0000425", .NombreDelCliente = "Marcelo Jose Alvarado Torres", .TipoDeTransaccion = "visita", .Cantidad = "2000.00", .Fecha = DateTime.Now.ToString("yyyy/MM/dd"), .Hora = DateTime.Now.Hour.ToString(), .Latitud = "15.398078", .Longitud = "-87.809251"}

                items.Add(a)
                items.Add(b)
                items.Add(c)

                Dim queryString As String = "?items=" & Server.UrlEncode(JsonConvert.SerializeObject(items))
                Response.Redirect("CobrosMap.aspx" & queryString)


            Catch ex As IOException
                Dim msg = "Error al procesar  archivo: " & ex.Message
                AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)
            Catch ex As Exception
                Dim msg = "Error de descarga: " & ex.Message
                AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)
            End Try

        End If
    End Sub

End Class