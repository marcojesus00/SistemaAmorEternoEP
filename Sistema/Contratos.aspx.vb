Public Class Contratos
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut As String
    Private Datos, Datos1, Datos2 As DataSet
    Private Total As Decimal = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario") = "" Then
            Response.Redirect("inicio.aspx")
        End If

        If Not IsPostBack Then
            Session.Add("Orden", "0")
            DlActivo.Items.Add("")
            DlActivo.Items.Add("A")
            DlActivo.Items.Add("R")
            DlActivo.SelectedIndex = 0

            DLContrato.Items.Add("")
            DlActivo.SelectedIndex = 0
        End If

        Session.Timeout = 90
        Usuario = Session("Usuario")
        Clave = Session("Clave")
        Servidor = Session("Servidor")
        Bd = Session("Bd")
        Usuario_Aut = Session("Usuario_Aut")
        Clave_Aut = Session("Clave_Aut")
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Dim conf As New Configuracion(Usuario, Clave, "FUNAMOR", Servidor)
        Dim Sql As String

        Sql = " SELECT * FROM PLAEMP WHERE CONVERT(VARCHAR,P_num_emple) + P_nomb_empl LIKE '%" + txtCobrador.Text + "%' AND P_status LIKE '%" + DlActivo.SelectedValue + "%'"

        Datos = conf.EjecutaSql(Sql)
        Datos.Tables(0).DefaultView.Sort = "P_nomb_empl Desc"
        Session.Add("GV", Datos.Tables(0))
        gvCobradores.DataSource = Session("GV")
        gvCobradores.DataBind()
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As ImageClickEventArgs) Handles btnSalir.Click
        Response.Redirect("principal.aspx")
    End Sub

    Private Sub gvCobradores_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvCobradores.Sorting
        Dim Orden As String

        If Session("Orden") = "0" Then
            Orden = "Asc"
            Session.Add("Orden", "1")
        Else
            Orden = "Desc"
            Session.Add("Orden", "0")
        End If

        Session("GV").DefaultView.Sort = e.SortExpression.ToString + " " + Orden
        gvCobradores.DataSource = Session("GV")
        gvCobradores.DataBind()
    End Sub

    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub

    Private Sub gvCobradores_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvCobradores.RowCommand
        If e.CommandName = "Codigo" Then
            Dim indice As Integer = Convert.ToInt32(e.CommandArgument)
            Dim id As String = gvCobradores.Rows(indice).Cells(0).Text
            Dim Nombre As String = gvCobradores.Rows(indice).Cells(1).Text
            Dim Identidad As String = gvCobradores.Rows(indice).Cells(2).Text
            Dim Estatus As String = gvCobradores.Rows(indice).Cells(4).Text

            Session.Add("Codigo_Empleado", id)
            Session.Add("Nombre", Nombre)
            Session.Add("Identidad", Identidad)
            Session.Add("Estatus", Estatus)

            ifEmpleado.Src = "Empleados.aspx"
            ifEmpleado.Visible = True
        End If

        If e.CommandName = "SAP" Then
            Dim indice As Integer = Convert.ToInt32(e.CommandArgument)
            Dim id As String = gvCobradores.Rows(indice).Cells(0).Text
            Session.Add("Codigo_Empleado", id)
            Session.Add("Reporte", "Empleados")
            ifEmpleado.Src = "Consultas_rrhh.aspx"
            ifEmpleado.Visible = True
        End If
    End Sub

    Private Sub gvCobradores_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvCobradores.RowDataBound
        If ((e.Row.RowType = DataControlRowType.DataRow) And (e.Row.RowType <> DataControlRowType.EmptyDataRow)) Then
            Dim Fila As System.Data.DataRowView = e.Row.DataItem
            Total += 1

            lblbarra.Text = "Total: " + Total.ToString
        End If
    End Sub

    'Private Sub btnCancelar_Click(sender As Object, e As ImageClickEventArgs) Handles btnCancelar.Click
    '    ifEmpleado.Visible = False
    '    Session.Add("Codigo_Empleado", "")
    '    ifEmpleado.Src = "Empleados.aspx"
    'End Sub
End Class