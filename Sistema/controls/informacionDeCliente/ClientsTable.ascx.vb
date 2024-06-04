Imports System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder
Imports System.Data.SqlClient
Imports System.IO

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

        If Session("Usuario") = "" Then
            Response.Redirect("inicio.aspx")
        End If



        If Not IsPostBack Then
                Try


                    BindGridView()

                Catch ex As Exception
                    Dim msg = "Error al leer de la base de datos, por favor recargue la página : " & ex.Message
                    RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
                End Try
            End If

    End Sub

    Protected Sub MyGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        Session("tabSelected") = "DocsTab"

        If e.CommandName = "ShowDocs" Then

            Try


                Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
                Dim dataKey As String = MyGridView.DataKeys(rowIndex).Value.ToString()
                Dim ctrolDocuments As GridView = GridViewDocs



                Dim documents As List(Of UrlCliente) = GetDocsByClientId(dataKey)

                    If documents IsNot Nothing AndAlso documents.Count > 0 Then
                        ctrolDocuments.DataSource = documents
                        ctrolDocuments.DataBind()
                    End If


            Catch ex As IOException
                Dim msg = "Error : " & ex.Message
                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            Catch ex As Exception
                Dim msg = "Error : " & ex.Message
                RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            End Try

        End If

    End Sub
    Private Function GetDocsByClientId(Id As String)
        Using dbcontext As New MyDbContext
            Dim docs = dbcontext.urlClientes.Where(Function(u) u.CodigoCliente.Contains(Id.Trim))
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
        Using dbContext As New MyDbContext()
            Dim data = dbContext.Contratos.
                    Where(Function(c) (c.CodigoCliente.Contains(id) Or c.Cliente.NombreCliente.Contains(id) Or c.Cliente.Identidad.Contains(id)) And c.Cliente.NombreCliente.Trim.Length > 0 And Not c.Cliente.NombreCliente.Contains("*")).
                    Select(Function(c) New ClientContractQueryResult With {
                        .NombreCliente = c.Cliente.NombreCliente.Trim,
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
    Protected Function GetClientsAndContractsCount() As Integer
        Using dbContext As New MyDbContext()
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
                Using dbContext As New MyDbContext
                    Dim DocId As Integer = GridViewDocs.DataKeys(rowIndex).Value.ToString()
                    Dim record As UrlCliente = dbContext.urlClientes.Find(DocId)
                    Dim documentPath = record.RutaDelArchivo
                    Dim handlerUrl As String = $"/Handlers/DownloadHandler.ashx?path={HttpUtility.UrlEncode(documentPath)}&name={HttpUtility.UrlEncode(documentName)}"
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
