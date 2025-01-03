Imports System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI.WebControls
Imports Sistema.InventarioDeEquipo

Public Class ClientsTable
    Inherits System.Web.UI.UserControl
    Public Usuario, clave, Servidor, Bd, Usuario_Aut, Clave_Aut As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.Timeout = 90
        Usuario = Session("Usuario")
        clave = Session("Clave")
        Servidor = Session("Servidor")
        Bd = Session("Bd")
        Usuario_Aut = Session("Usuario_Aut")
        Clave_Aut = Session("Clave_Aut")




        If Not IsPostBack Then
            Try

                If Session("Usuario_Aut") Is Nothing OrElse Session("Usuario_Aut") = "" OrElse Not AuthHelper.isAuthorized(Session("Usuario_Aut"), "TOCASH") Then
                    Session("isautorized") = False
                Else
                    Session("isautorized") = True
                    PnlToCash.Visible = False
                    SaveToCash.Enabled = False
                    SaveToCash.CssClass = "btn btn-secondary"

                End If
                'BindGridView()
                FIllDll()

            Catch ex As Exception
                Dim msg = "Error al leer de la base de datos, por favor recargue la página : " & ex.Message
                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            End Try
        End If

    End Sub
    Protected Sub LinkbuttonClose_Click(sender As Object, e As EventArgs)
        PnlToCash.Visible = False

    End Sub

    Protected Sub MyGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        Session("tabSelected") = "DocsTab"

        Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
        Dim dataKey As String = MyGridView.DataKeys(rowIndex).Value.ToString()
        Dim ctrolDocuments As GridView = GridViewDocs
        Try

            If e.CommandName = "ShowDocs" Then






                Dim documents As List(Of UrlCliente) = GetDocsByClientId(dataKey)

                If documents IsNot Nothing AndAlso documents.Count > 0 Then
                    ctrolDocuments.DataSource = documents
                    ctrolDocuments.DataBind()
                End If


            ElseIf e.CommandName = "FromCreditToCash" Then
            If Session("isautorized") Is Nothing Or Session("isautorized") = False Then
                AlertHelper.GenerateAlert("warning", "No tiene permiso", alertPlaceholder)
                RaiseEvent AlertGenerated(Me, New AlertEventArgs("No tiene permiso", "danger"))

            Else
                Session("ClientCodeToConvertToCash") = dataKey
                PnlToCash.Visible = True
                CardTitleLabel.Text = "Pasando a contado: " + dataKey
                If Session("isautorized") Is Nothing Or Session("isautorized") = False Then
                    AlertHelper.GenerateAlert("warning", "No tiene permiso", alertPlaceholder)
                Else
                    Dim clientCode = dataKey
                    'Dim rtipodebi = DdlRtipoDebi.SelectedValue
                    Dim parameters As SqlParameter() = {
                        New SqlParameter("@CodigoDeCliente", clientCode)}
                    Using context As New FunamorContext()
                            Dim sql = "EXEC SP_VS_SHOW_RECEIPTS_TO_SWITCH_FROM_CREDIT_TO_CASH @CodigoDeCliente"
                            Dim result As List(Of ReceiptToCash) = context.Database.SqlQuery(Of ReceiptToCash)(sql, parameters).ToList()
                            Repeater1.DataSource = result
                        Repeater1.DataBind()

                    End Using
                End If

                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "ShowModal", "var myModal = new bootstrap.Modal(document.getElementById('myModal')); myModal.show();", True)
            End If
        End If

        Catch ex As IOException
            Dim msg = "Error : " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

        Catch ex As Exception
            Dim msg = "Error : " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

        End Try
    End Sub
    Public Class ReceiptToCash
        Public Property Numero As String
        Public Property Concepto As String
        Public Property Monto As Decimal
        Public Property Fecha As DateTime


    End Class
    Private Sub FIllDll()
        '      Dim queryZone = "select [vzon_codigo] as Id
        '    ,vzon_codigo + ' ' +[vzon_nombre] as Name
        'FROM [FUNAMOR].[dbo].[VZONA]
        'where len(vzon_nombre)>3"
        '      Using context As New FunamorContext()

        '          Dim resultZOnes As List(Of DDL2) = context.Database.SqlQuery(Of DDL2)(
        '                   queryZone).ToList()
        '          DdlRtipoDebi.DataSource = resultZOnes
        '          DdlRtipoDebi.DataTextField = "Name"
        '          DdlRtipoDebi.DataValueField = "Id"
        '          DdlRtipoDebi.DataBind()
        DdlRtipoDebi.Items.Insert(0, New ListItem("Escoja tipo de servicio", ""))

        DdlRtipoDebi.Items.Insert(1, New ListItem("Ataudes", "03"))
        DdlRtipoDebi.Items.Insert(2, New ListItem("Sala Velacion       ", "04"))

        DdlRtipoDebi.Items.Insert(3, New ListItem("Lote de contado     ", "12"))



    End Sub


    Protected Sub SaveToCash_Click(sender As Object, e As EventArgs)
        Try
            If Session("isautorized") Is Nothing Or Session("isautorized") = False Then
                AlertHelper.GenerateAlert("warning", "No tiene permiso", alertPlaceholder)
            Else
                Dim clientCode = Session("ClientCodeToConvertToCash")
                Dim rtipodebi = DdlRtipoDebi.SelectedValue
                Dim parameters As SqlParameter() = {
                    New SqlParameter("@RTIPODEBI", rtipodebi),
                    New SqlParameter("@CodigoDeCliente", clientCode)}
                Using context As New FunamorContext()
                    Dim sql = "EXEC SP_VS_SWITCH_FROM_CREDIT_TO_CASH @RTIPODEBI=@RTIPODEBI, @CodigoDeCliente=@CodigoDeCliente"
                    Dim result As String = context.Database.SqlQuery(Of String)(sql, parameters).FirstOrDefault()
                    AlertHelper.GenerateAlert("info", result, alertPlaceholder)
                    RaiseEvent AlertGenerated(Me, New AlertEventArgs(result, "info"))
                    PnlToCash.Visible = False

                End Using
            End If
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            AlertHelper.GenerateAlert("danger", ex.Message, alertPlaceholder)
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(ex.Message, "danger"))

        End Try

    End Sub
    Private Function GetDocsByClientId(Id As String)
        Using dbcontext As New FunamorContext
            Dim docs = dbcontext.UrlClientes.Where(Function(u) u.CodigoCliente.Contains(Id.Trim))
            Return docs.ToList()
        End Using
    End Function




    Private Sub BindGridView(Optional str As String = "")


        Try

            If True Then

                Dim dataList As List(Of ClientContractQueryResult) = GetClientsAndContracts(str)

                MyGridView.DataSource = dataList
                MyGridView.DataBind()
                GridViewDocs.DataBind()
                If MyGridView.Rows.Count = 0 Then
                    lblMessage.Text = "No se encontraron documentos"
                Else
                    lblMessage.Text = "Mostrando primeros " & $"{dataList.Count} resultados."
                End If

            Else

            End If

        Catch ex As Exception
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

        End Try



    End Sub
    Public Sub OnDataReceived(ByVal sender As Object, ByVal e As TextboxEventArgs)
        Try
            BindGridView(e.str)
        Catch ex As Exception
            RaiseEvent AlertGenerated(Me, New AlertEventArgs("Error" & ex.Message, "danger"))

        End Try


    End Sub

    Protected Function GetClientsAndContracts(Optional id As String = "") As List(Of ClientContractQueryResult)
        Using dbContext As New FunamorContext()
            Dim data = dbContext.Contratos.AsNoTracking.
                    Where(Function(c) (c.CodigoCliente.Contains(id) Or c.Cliente.Nombre.Contains(id) Or c.Cliente.Identidad.Contains(id)) And c.Cliente.Nombre.Trim.Length > 0 And Not c.Cliente.Nombre.Contains("*")).
                    Select(Function(c) New ClientContractQueryResult With {
                        .NombreCliente = c.Cliente.Nombre.Trim,
                        .CodigoCliente = c.CodigoCliente.Trim,
                        .Telefono = c.Cliente.TelefonoCliente.Trim,
                        .Celular = c.Cliente.Celular.Trim,
                        .Vendedor = c.Vendedor.Trim,
                        .Cobrador = c.Cobrador.Trim}).
                    OrderBy(Function(c) c.NombreCliente).
                    Take(10).
                    ToList()
            Return data
        End Using
    End Function

    Protected Sub MyGridView_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim linkButton As LinkButton = CType(e.Row.FindControl("LinkButtonFromCreditToCash"), LinkButton)
            'Dim rowData As Sistema.ClientContractQueryResult = CType(e.Row.DataItem, Sistema.ClientContractQueryResult)


            'Dim rowData As DataRowView = CType(e.Row.DataItem, DataRowView)
            If Session("isautorized") Is Nothing Or Session("isautorized") = False Then
                linkButton.Visible = False
            Else

                linkButton.Visible = True
            End If
        End If
    End Sub

    Protected Sub DdlRtipoDebi_SelectedIndexChanged(sender As Object, e As EventArgs)
        If DdlRtipoDebi.SelectedIndex = 0 Then
            SaveToCash.CssClass = "btn btn-secondary"
            SaveToCash.Enabled = False

        Else
            SaveToCash.CssClass = "btn btn-primary"
            SaveToCash.Enabled = True

        End If
    End Sub

    Protected Function GetClientsAndContractsCount() As Integer
        Using dbContext As New FunamorContext()
            Dim data = dbContext.Contratos.Select(Function(c) c.CodigoCliente.Count).FirstOrDefault()
            Return data
        End Using
    End Function






    Protected Sub MyGridView_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles MyGridView.PageIndexChanging
        MyGridView.PageIndex = e.NewPageIndex
        Session("tabSelected") = "DocsTab"

    End Sub
    Public Event AlertGenerated As EventHandler(Of AlertEventArgs)
    Protected Sub GridViewDocs_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        If e.CommandName = "DownloadFile" Then

            Try

                Dim args As String() = e.CommandArgument.ToString().Split("|"c)
                Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = GridViewDocs.Rows(rowIndex)
                Dim documentName As String = row.Cells(0).Text
                Using dbContext As New FunamorContext
                    Dim DocId As Integer = GridViewDocs.DataKeys(rowIndex).Value.ToString()
                    Dim record As UrlCliente = dbContext.UrlClientes.Find(DocId)
                    Dim documentPath = record.RutaDelArchivo
                    Dim handlerUrl As String = $"~/Handlers/DownloadHandler.ashx?path={HttpUtility.UrlEncode(documentPath)}&name={HttpUtility.UrlEncode(documentName)}"
                    Response.Redirect(handlerUrl)
                End Using

            Catch ex As IOException
                Dim msg = "Error al procesar  archivo: " & ex.Message
                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            Catch ex As Exception
                Dim msg = "Error de descarga: " & ex.Message
                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            End Try

        End If
    End Sub
    Protected Sub GridViewDocs_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles GridViewDocs.RowDataBound

        Try

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim sizeColumnIndex As Integer = 0
                Dim fileName As String = e.Row.Cells(sizeColumnIndex).Text
                Dim fileExtension As String = Path.GetExtension(fileName)
                Dim iconCell As TableCell = e.Row.Cells(1)
                Dim nameCell As TableCell = e.Row.Cells(0)
                Dim imageClass = "bi bi-image"
                Select Case fileExtension
                    Case ".docx"
                        iconCell.Controls.Add(New LiteralControl("<i class=""bi bi-file-word-fill text-primary""></i>"))
                    Case ".xls"
                        iconCell.Controls.Add(New LiteralControl("<i class=""bi bi-file-earmark-excel-fill text-success""></i>"))
                    Case ".xlsx"
                        iconCell.Controls.Add(New LiteralControl("<i class=""bi bi-file-earmark-excel-fill text-success""></i>"))
                    Case ".pdf"
                        iconCell.Controls.Add(New LiteralControl("<i class=""bi bi-file-earmark-pdf-fill text-danger""></i>"))
                    Case ".jpg"
                        iconCell.Controls.Add(New LiteralControl("<i class=""bi bi-image""></i>"))
                    Case ".jpg"
                        iconCell.Controls.Add(New LiteralControl("<i class=""bi bi-image""></i>"))
                    Case ".jpeg"
                        iconCell.Controls.Add(New LiteralControl("<i class=""bi bi-image""></i>"))
                    Case ".png"
                        iconCell.Controls.Add(New LiteralControl($"<i class=""bi bi-image""></i>"))
                    Case Else
                        iconCell.Controls.Add(New LiteralControl("<i class=""bi bi-file-earmark-text-fill""></i>"))
                End Select

                If fileName.Length > 32 Then
                    fileName = fileName.Substring(Math.Max(fileName.Length - 32, 0))
                End If

            End If

        Catch ex As Exception
            Dim msg = "Error inesperado : " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
        End Try

    End Sub

End Class
