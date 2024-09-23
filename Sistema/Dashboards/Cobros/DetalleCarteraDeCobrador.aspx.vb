Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Http
Imports Newtonsoft.Json
Imports OfficeOpenXml
Imports OfficeOpenXml.Style
Imports Sistema.CobrosDashboard

Public Class DetalleCarteraDeCobrador
    Inherits System.Web.UI.Page
    Public PageNumber As Integer = 1
    Public PageSize As Integer = 10
    Public TotalPages As Integer '
    Public TotalItems As Integer = 0
    Public itemText As String
    Public pagination As PaginationHelper = New PaginationHelper
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim Usuario_Aut = Session("Usuario_Aut")
            Session("BackPageUrl") = "~/Dashboards/Cobros/Cobros.aspx"
            Dim thisPage = "~/Dashboards/Cobros/DetalleCarteraDeCobrador.aspx"
            If Usuario_Aut IsNot Nothing Then
                Usuario_Aut = Usuario_Aut.ToString().Trim().ToUpper()
                If Session("Usuario") = "" OrElse Not AuthHelper.isAuthorized(Usuario_Aut, "COBROS_A") Then
                    Response.Redirect("~/Principal.aspx")
                End If
                If Not AuthHelper.isAuthorized(Usuario_Aut, "MASSW") Then
                    btnSendMassiveWhatsApp.Enabled = False
                    btnSendMassiveWhatsApp.CssClass = "btn btn-sm btn-secondary"
                End If
                If Not IsPostBack Then
                    'FillDll()
                    Dim cobrador = Session("CobradorSeleccionado")
                    If cobrador Then
                        PnlGoodAndBadPhones.Visible = False
                        PnlBasPhones.Visible = False
                        ReBind(selectedPage:=1)

                    End If
                End If

            Else
                Response.Redirect("~/Principal.aspx")

            End If

        Catch ex As Exception
            Dim msg = "Problema al la cargar página, por favor vuelva a intentarlo : " & ex.Message
            'RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try
    End Sub

    Protected Sub WhatsAppToAll_Click(sender As Object, e As EventArgs)

        PnlGoodAndBadPhones.Visible = True
        Dim cobrador As CobradorDto = CType(Session("cobradorObject"), CobradorDto)
        If cobrador IsNot Nothing Then
            If cobrador.Lider.ToLower.Contains("nora") Then
                DdlIntance.SelectedValue = 1
            End If

        End If
        PnlPrimary.Visible = False
        PnlBasPhones.Visible = False
        If Session("CobradorSeleccionado") Then

            CardTitleLiteral.Text = "Clientes a los que se enviará"

            rebindGoodPhones()
        End If




    End Sub
    Protected Sub btnCorrupPhones_Click(sender As Object, e As EventArgs)

        PnlGoodAndBadPhones.Visible = False
        PnlPrimary.Visible = False
        PnlBasPhones.Visible = True
        If Session("CobradorSeleccionado") Then
            CardTitleLiteral.Text = "Clientes con teléfonos corruptos"
            rebindBadPhones()

        End If
    End Sub
    'Protected Sub rebindDetails()
    '    Dim clients As List(Of DocsDto)
    '    If Session("SelectedDetail") = "Corrupt" Then

    '        clients = clientsWithCorrupPhones(Session("CobradorSeleccionado"))
    '    ElseIf Session("SelectedDetail") = "Valid" Then
    '        clients = clientsValidToSend(Session("CobradorSeleccionado"))
    '    Else
    '        clients = New List(Of DocsDto)
    '    End If
    '    SendGridview.DataSource = clients
    '    SendGridview.DataBind()
    'End Sub
    Protected Sub rebindBadPhones()
        Dim clients As List(Of DocsDto)

        clients = clientsWithCorrupPhones(Session("CobradorSeleccionado"))
        GridViewBadPhones.DataSource = clients
        GridViewBadPhones.DataBind()
    End Sub
    Protected Sub rebindGoodPhones()
        Dim clients As List(Of DocsDto)

        clients = clientsValidToSend(Session("CobradorSeleccionado"))
        SendGridview.DataSource = clients
        SendGridview.DataBind()
    End Sub

    Protected Sub ReBind(Optional selectedPage As Integer = 1)
        Try


            Dim cobrador = Session("CobradorSeleccionado")

            Dim selectClause = "Select A.Codigo_clie As Codigo, A.Nombre_clie Nombre,  Case 
        WHEN NULLIF(A.Telef_clien, '') IS NOT NULL THEN A.Telef_clien 
        Else A.CL_CELULAR
    End As Telefono, FORMAT(A.Saldo_actua, 'C', 'es-HN')  as Saldo, ISNULL(CONVERT(VARCHAR, CONVERT(DATE, w.FechaDeEnvio)),'Nunca') AS Ultimo_envio"
            Dim fromClause = " From CLIENTES a "
            Dim joinCLause = "			 left join LogDocumentosPorWhatsApp w ON A.Codigo_clie=w.CodigoDeCliente and w.fueExitoso=1
"
            Dim whereClause = " where A.cl_cobrador = @Cobrador And A.Saldo_actua>0"
            Dim orderByClause = " Order BY A.Saldo_actua desc"
            Dim paginationClause = "OFFSET @Offset ROWS FETCH Next @PageSize ROWS ONLY"
            Dim dataQuery = $"
                            {selectClause} {fromClause} {joinCLause} {whereClause} 
                            {orderByClause} 
                            {paginationClause} OPTION(RECOMPILE)
                        "
            Dim queryCount = $"
                            select count(*) {fromClause}  {whereClause} 
                           OPTION(RECOMPILE) "
            Using FunamorContext As New FunamorContext()
                Dim offset = (selectedPage - 1) * 10

                Dim sqlParameters As New List(Of SqlParameter)
                sqlParameters.Add(New SqlParameter("@Cobrador", $"{cobrador}"))
                sqlParameters.Add(New SqlParameter("@Offset", offset))
                sqlParameters.Add(New SqlParameter("@PageSize", 10))
                Dim result As List(Of DocsDtoCL) = FunamorContext.Database.SqlQuery(Of DocsDtoCL)(
                dataQuery, sqlParameters.ToArray()).ToList()

                Dim totalCountparams As New List(Of SqlParameter)
                totalCountparams.Add(New SqlParameter("@Cobrador", $"{cobrador}"))
                totalCountparams.Add(New SqlParameter("@Offset", offset))
                totalCountparams.Add(New SqlParameter("@PageSize", 10))
                Dim toltalC As Integer = FunamorContext.Database.SqlQuery(Of Integer)(queryCount, totalCountparams.ToArray()).FirstOrDefault()

                Dim cobradorQuery = "select C.codigo_cobr Codigo, isnull(D.nombre_cobr,'') Lider, REPLACE(ISNULL(d.cob_telefo, ''), '-', '')  Telefono_lider, isnull(c.cob_zona,'') Zona from COBRADOR c
join COBRADOR d ON c.cob_lider=d.codigo_cobr
where c.codigo_cobr like @Cobrador"
                Dim cobParams As New List(Of SqlParameter)
                cobParams.Add(New SqlParameter("@Cobrador", $"{cobrador}"))
                Dim cobradorInfo As CobradorDto = FunamorContext.Database.SqlQuery(Of CobradorDto)(cobradorQuery, cobParams.ToArray()).FirstOrDefault()
                Session("TelefonoLider") = cobradorInfo.Telefono_lider.Trim
                Session("cobradorObject") = cobradorInfo

                DashboardGridview.DataSource = result
                DashboardGridview.DataBind()

                TotalItems = toltalC
                PageNumber = selectedPage
                TotalPages = Math.Ceiling(toltalC / PageSize)

                ' Update the Previous and Next buttons' enabled state
                Dim pages As New List(Of Integer)()
                For i As Integer = 1 To TotalPages
                    pages.Add(i)
                Next

                rptPager.DataSource = pagination.GetLimitedPageNumbers(TotalItems, PageSize, PageNumber, 3)
                rptPager.DataBind()
                lnkbtnPrevious.Enabled = PageNumber > 1
                lnkbtnNext.Enabled = PageNumber < TotalPages
                lblTotalCount.DataBind()
            End Using
        Catch ex As Exception
            Dim msg = "No se cargó la tabla."
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)
        End Try
    End Sub



    Private Function clientsValidToSend(cobrador As String)
        Try
            Using FunamorContext As New FunamorContext()
                Dim sqlParameters As New List(Of SqlParameter)
                sqlParameters.Add(New SqlParameter("@cobrador", cobrador.Trim()))

                Dim result As List(Of DocsDto) = FunamorContext.Database.SqlQuery(Of DocsDto)(
            "EXEC SP_VS_CarteraDeClienteParaWhatsapp @cobrador", sqlParameters.ToArray()).ToList()
                Return result
            End Using
        Catch ex As Exception
            Dim msg = "No se cargó la tabla."
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)
        End Try
    End Function
    Private Function clientsWithCorrupPhones(cobrador As String)
        Try
            Using FunamorContext As New FunamorContext()
                Dim sqlParameters As New List(Of SqlParameter)
                sqlParameters.Add(New SqlParameter("@cobrador", cobrador.Trim()))

                Dim result As List(Of DocsDto) = FunamorContext.Database.SqlQuery(Of DocsDto)(
            "EXEC SP_VS_CarteraMalaDeClienteParaWhatsapp @cobrador", sqlParameters.ToArray()).ToList()
                Return result
            End Using
        Catch ex As Exception
            Dim msg = "No se cargó la tabla."
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)
        End Try
    End Function
    Public Function EnviarEstadoDeCuenta(result As List(Of DocsDto))
        Try
            Dim falla = ""
            Dim queue1 As Integer = 0
            Dim fallas As New List(Of String)
            Dim Usuario_Aut = "yo" 'Session("Usuario_Aut")
            Dim successCount = 0
            If AuthHelper.isAuthorized(Usuario_Aut, "MASSW") Then
                Dim leaderPhone = Session("TelefonoLider")
                Dim cobrador As CobradorDto = CType(Session("cobradorObject"), CobradorDto)
                Dim batchId = cobrador.Codigo + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + result.Count.ToString()

                Dim Leader = ""
                Dim numeroPBX = "2647-3390"
                If cobrador IsNot Nothing Then
                    leaderPhone = cobrador.Telefono_lider
                    Leader = cobrador.Lider
                End If
                Dim Usuario = Session("Usuario")
                Dim Clave = Session("Clave")

                For Each cliente In result
                    Try
                        Dim Informe As New Movimiento_Clientes
                        Informe.SetDatabaseLogon(Usuario, Clave)
                        Informe.SetParameterValue("Cliente", cliente.Codigo)
                        Dim ins = ""

                        If DdlIntance.SelectedValue = 0 Then
                            ins = "other"
                        Else
                            ins = "default"
                        End If
                        Dim nombreArchivo As String = cliente.Codigo + "-" + DateTime.Now.ToString("yyyy-MM-dd") + "" + ".pdf" ' Cambia el nombre del archivo si lo deseas

                        Dim pdf As System.IO.Stream = Informe.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat)
                        Dim cap = "Estimado(a) " + cliente.Nombre + $", Amor Eterno manda su estado de cuenta. Para mayor informacion o si desea comunicarse con servicio al cliente puede llamar al Pbx:(+504) 2647-3390 / (+504) 2647-4529 /(+504) 2647-4986 Tel: (+504) 3290-7278"
                        Dim user = Session("Usuario_Aut")

                        Dim r4esult As ResultW = whatsapi.sendWhatsAppDocs(doc:=pdf, name:=nombreArchivo, localNumber:=cliente.Telefono, caption:=cap, couentryCode:="504", user:=user, clientCode:=cliente.Codigo, instancia:=ins, CodigoDeCobrador:=cobrador.Codigo, BatchId:=batchId)
                        'Debug.WriteLine(r4esult.Msg)

                        If r4esult.Success = False Then
                            Dim m = "Codigo de cliente: " + cliente.Codigo + r4esult.Msg
                            DebugHelper.SendDebugInfo("danger", New Exception(m), Session("Usuario_Aut"))
                            If r4esult.Msg.ToLower.Contains("queue") Then
                                queue1 += 1
                            Else

                            End If
                            fallas.Add("el servicio de envio")

                        Else
                            successCount = successCount + 1
                        End If
                        Informe.Close()
                        Informe.Dispose()
                    Catch ex As Exception
                        fallas.Add("la generación del reporte")

                        Dim m = "Codigo de cliente: " + cliente.Codigo
                        DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"), m)

                        whatsapi.logW(name:="", couentryCode:="504", localNumber:=cliente.Telefono, caption:="", clientCode:=cliente.Codigo, user:=User, instancia:="", docDescription:="Estado de cuenta", isSuccess:=False, msg:=m + ex.Message, CodigoDeCobrador:=cobrador.Codigo, Estado:="Error servidor", IdDeLaPlataforma:=0, BatchId:=batchId, ReferenceId:=Guid.NewGuid())

                    End Try


                Next
            End If
            Dim countErrors = result.Count - successCount - queue1
            Dim mensaje = $"Enviados con éxito: {successCount}. Fallidos: {countErrors}. En Cola: {queue1}"

            If result.Count = 0 Then
                Dim uniqueSet As New HashSet(Of String)(fallas)

                ' Convert the HashSet back to a List
                Dim uniqueList As List(Of String) = uniqueSet.ToList()
                Dim s = String.Join(", ", uniqueList)

                AlertHelper.GenerateAlert("danger", mensaje + $" debido a errores en {uniqueList}", alertPlaceholder)

            Else
                AlertHelper.GenerateAlert("info", mensaje, alertPlaceholder)
            End If


            Return True

        Catch ex As Exception
            Dim msg = "Al mandar mensajes"
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)
        End Try



    End Function
    Protected Sub lnkbtnPage_Click(sender As Object, e As EventArgs)
        Dim lnkButton As LinkButton = CType(sender, LinkButton)
        Dim resultInteger As Integer
        If Integer.TryParse(lnkButton.Text, resultInteger) Then
            Dim SelectedPageDashboardVentas As Integer = Integer.Parse(lnkButton.Text)
            Session("SelectedPageDashboardVentas") = SelectedPageDashboardVentas
            ReBind(selectedPage:=SelectedPageDashboardVentas)
        End If

    End Sub

    Protected Sub lnkbtnPrevious_Click(sender As Object, e As EventArgs)

        If Session("SelectedPageDashboardVentas") IsNot Nothing AndAlso Session("SelectedPageDashboardVentas") > 1 Then
            Session("SelectedPageDashboardVentas") = Session("SelectedPageDashboardVentas") - 1

            ReBind(selectedPage:=Session("SelectedPageDashboardVentas"))

        End If

    End Sub

    Protected Sub lnkbtnNext_Click(sender As Object, e As EventArgs)
        'filterData.PageNumber = filterData.PageNumber + 1
        If Session("SelectedPageDashboardVentas") IsNot Nothing Then
            Session("SelectedPageDashboardVentas") = Session("SelectedPageDashboardVentas") + 1
            ReBind(selectedPage:=Session("SelectedPageDashboardVentas"))
        End If

    End Sub

    Protected Sub btnExitWhatsapToAll_Click(sender As Object, e As EventArgs)
        PnlGoodAndBadPhones.Visible = False
        PnlPrimary.Visible = True
        PnlBasPhones.Visible = False
    End Sub

    Protected Sub btnSendMassiveWhatsApp_Click(sender As Object, e As EventArgs)
        If Session("CobradorSeleccionado") Then
            Dim clients As List(Of DocsDto) = clientsValidToSend(Session("CobradorSeleccionado"))
            EnviarEstadoDeCuenta(clients)

        End If
    End Sub

    Protected Sub SendGridview_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        SendGridview.PageIndex = e.NewPageIndex
        rebindGoodPhones()
    End Sub


    Protected Sub GridViewBadPhones_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        GridViewBadPhones.PageIndex = e.NewPageIndex
        rebindBadPhones()
    End Sub
    Public Sub DownloadExcelFromList(docs As List(Of DocsDto), invalidPhones As List(Of DocsDto), title As String)
        ' Create a new ExcelPackage
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial

        Using package As New ExcelPackage()
            ' Add a worksheet to the workbook
            If docs.Count > 0 Then
                Dim tit = "Telefonos mal escritos"
                Dim worksheet = package.Workbook.Worksheets.Add(tit)

                ' Add the title to the first row, merging cells A1 to C1
                worksheet.Cells("A1:C1").Merge = True
                worksheet.Cells("A1").Value = tit
                worksheet.Cells("A1").Style.Font.Size = 14
                worksheet.Cells("A1").Style.Font.Bold = True
                worksheet.Cells("A1").Style.HorizontalAlignment = ExcelHorizontalAlignment.Center

                ' Add headers to the second row
                worksheet.Cells(2, 1).Value = "Codigo"
                worksheet.Cells(2, 2).Value = "Nombre"
                worksheet.Cells(2, 3).Value = "Telefono"

                ' Populate the worksheet with data starting from row 3
                Dim row As Integer = 3
                For Each doc As DocsDto In docs
                    worksheet.Cells(row, 1).Value = doc.Codigo
                    worksheet.Cells(row, 2).Value = doc.Nombre
                    worksheet.Cells(row, 3).Value = doc.Telefono
                    row += 1
                Next

                ' Auto-fit columns for better readability
                worksheet.Cells.AutoFitColumns()
            End If
            If invalidPhones.Count > 0 Then
                Dim tit = "Telefonos invalidos en whatsapp"
                Dim worksheet2 = package.Workbook.Worksheets.Add(tit)

                ' Add the title to the first row, merging cells A1 to C1
                worksheet2.Cells("A1:C1").Merge = True
                worksheet2.Cells("A1").Value = tit
                worksheet2.Cells("A1").Style.Font.Size = 14
                worksheet2.Cells("A1").Style.Font.Bold = True
                worksheet2.Cells("A1").Style.HorizontalAlignment = ExcelHorizontalAlignment.Center

                ' Add headers to the second row
                worksheet2.Cells(2, 1).Value = "Codigo"
                worksheet2.Cells(2, 2).Value = "Nombre"
                worksheet2.Cells(2, 3).Value = "Telefono"

                ' Populate the worksheet with data starting from row 3
                Dim row2 As Integer = 3
                For Each doc As DocsDto In invalidPhones
                    worksheet2.Cells(row2, 1).Value = doc.Codigo
                    worksheet2.Cells(row2, 2).Value = doc.Nombre
                    worksheet2.Cells(row2, 3).Value = doc.Telefono
                    row2 += 1
                    worksheet2.Cells.AutoFitColumns()

                Next

            End If


            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            HttpContext.Current.Response.AddHeader("content-disposition", $"attachment; filename={title.Replace(" ", "_")}.xlsx")

            ' Stream  to the client
            Using memoryStream As New MemoryStream()
                package.SaveAs(memoryStream)
                memoryStream.WriteTo(HttpContext.Current.Response.OutputStream)
            End Using

            ' End the response to prevent any additional content from being sent
            HttpContext.Current.Response.Flush()
            HttpContext.Current.Response.End()
        End Using

    End Sub
    Protected Sub btnDownloadExcel_Click(sender As Object, e As EventArgs)
        Dim cobrador = Session("CobradorSeleccionado")
        If cobrador Then


            Dim docTitle As String = $"Telefonos malos del cobrador {cobrador}"
            Dim clients As List(Of DocsDto)

            clients = clientsWithCorrupPhones(Session("CobradorSeleccionado"))
            Dim q = "EXEC SP_VS_CarteraInvalidaDeClienteParaWhatsapp @cobrador"
            ' Call the method to generate and download the Excel file
            Using FunamorContext As New FunamorContext()
                Dim sqlParameters As New List(Of SqlParameter)
                sqlParameters.Add(New SqlParameter("@cobrador", cobrador.Trim()))

                Dim result As List(Of DocsDto) = FunamorContext.Database.SqlQuery(Of DocsDto)(
            q, sqlParameters.ToArray()).ToList()
                DownloadExcelFromList(clients, result, docTitle)

            End Using
        End If


    End Sub
    Public Function SendJsonToFastApi(data As List(Of DocsDto), title As String, url As String)
        ' Create an object that includes the title and data
        Dim jsonData As New With {
        .title = title,
        .docs = data
    }

        ' Serialize the object to JSON
        Dim jsonString As String = JsonConvert.SerializeObject(jsonData)
        Dim httpClient As New HttpClient()

        ' Send the JSON data
        Dim content As New StringContent(jsonString, Encoding.UTF8, "application/json")
        Dim response As HttpResponseMessage = httpClient.PostAsync(url, content).Result

        If response.IsSuccessStatusCode Then
            Console.WriteLine("Data successfully sent!")
        Else
            Console.WriteLine("Failed to send data. Status: " & response.StatusCode.ToString())
        End If
    End Function
    Public Function GenerateHtmlReport(docsList As List(Of DocsDto)) As String
        Dim html As New System.Text.StringBuilder()

        ' Start HTML document
        html.AppendLine("<html>")
        html.AppendLine("<head>")
        html.AppendLine("<style>")
        html.AppendLine("table { font-family: Arial, sans-serif; border-collapse: collapse; width: 100%; }")
        html.AppendLine("th, td { border: 1px solid #dddddd; text-align: left; padding: 8px; }")
        html.AppendLine("tr:nth-child(even) { background-color: #f2f2f2; }")
        html.AppendLine("</style>")
        html.AppendLine("</head>")
        html.AppendLine("<body>")

        ' Table header
        html.AppendLine("<h2>Docs Report</h2>")
        html.AppendLine("<table>")
        html.AppendLine("<tr><th>Codigo</th><th>Nombre</th><th>Telefono</th></tr>")

        ' Table rows
        For Each doc As DocsDto In docsList
            html.AppendLine($"<tr><td>{doc.Codigo}</td><td>{doc.Nombre}</td><td>{doc.Telefono}</td></tr>")
        Next

        ' End HTML document
        html.AppendLine("</table>")
        html.AppendLine("</body>")
        html.AppendLine("</html>")

        Return html.ToString()
    End Function
    Public Sub Monitor_click()
        Response.Redirect("~/Dashboards/Cobros/MonitorEstadosDeCuentaEnviados.aspx")

    End Sub
End Class