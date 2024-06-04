Imports System.IO

Public Class CobrosDashboard
    Inherits System.Web.UI.Page
    Public Event AlertGenerated As EventHandler(Of AlertEventArgs)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub BindGridView(Optional str As String = "")


        Try

            If True Then
                Dim msg = ""
                Dim dataList = GetData()

                DashboardGridview.DataSource = dataList
                DashboardGridview.DataBind()
                DashboardGridview.DataBind()
                If DashboardGridview.Rows.Count = 0 Then
                    msg = "No se encontraron resultados"
                Else
                    msg = "Mostrando primeros " & $"{dataList.Count} resultados."
                End If

            Else

            End If

        Catch ex As Exception
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
        End Try
    End Sub
    Public Function GetData()

        Return 0
    End Function
    Protected Sub DashboardGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        If e.CommandName = "ShowMap" Then

            Try

                Dim args As String() = e.CommandArgument.ToString().Split("|"c)
                Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = DashboardGridview.Rows(rowIndex)
                Dim documentName As String = row.Cells(0).Text
                Using dbContext As New MyDbContext
                    Dim DocId As Integer = DashboardGridview.DataKeys(rowIndex).Value.ToString()
                    Dim record As UrlCliente = dbContext.urlClientes.Find(DocId)
                    Dim documentPath = record.RutaDelArchivo
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

End Class