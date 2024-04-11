Public Class cobradores1
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut As String
    Private Datos, Datos1, Datos2 As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario") = "" Then
            Response.Redirect("inicio.aspx")
        End If

        If Not IsPostBack Then
            Session.Add("Orden", "0")
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
        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String
        Sql = " SELECT * FROM ( "
        Sql += " SELECT RTRIM(A.cveage) [Codigo], RTRIM(A.nomage) [Nombre], RTRIM(A.usuari) [Usuario], RTRIM(A.usupwd) [Clave], RTRIM(A.sermov) [Mac], RTRIM(A.numser) [Sucursal] "
        Sql += " FROM MVSAGEN A "
        Sql += " UNION ALL "
        Sql += " SELECT A.Codigo_Cobr [Codigo], A.Nombre_Cobr [Nombre], '' Usuario, '' Clave, '' Mac, '' Sucursal "
        Sql += " FROM AE_COBRADOR A "
        Sql += " WHERE A.COBR_STATUS = 'A' "
        Sql += " AND A.CODIGO_COBR Not In (Select A.cveage From MVSAGEN A) "
        Sql += " AND A.cob_lider Is Not NULL "
        Sql += " AND A.Codigo_cobr <> A.Cob_lider )  A"
        Sql += " WHERE A.Codigo + A.Nombre Like '%" + txtCobradorN1.Text + "%' "
        Sql += " and A.Codigo + A.Nombre Like '%" + txtCobradorN2.Text + "%' "
        Sql += " and A.Mac Like '%" + txtMac.Text + "%' "

        Datos = conf.EjecutaSql(Sql)
        Datos.Tables(0).DefaultView.Sort = "Codigo Desc"
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

    Private Sub gvCobradores_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvCobradores.RowEditing
        gvCobradores.EditIndex = e.NewEditIndex
        gvCobradores.DataSource = Session("GV")
        gvCobradores.DataBind()
    End Sub

    Private Sub gvCobradores_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gvCobradores.RowUpdating
        Dim conf, conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql, Sql1 As String
        Dim Fila = gvCobradores.Rows(e.RowIndex)

        If CType(Fila.Cells(3).Controls(0), TextBox).Text.TrimEnd.Length = 0 Then
            Msg("Debe Agregar un usuario")
            Exit Sub
        End If

        Sql1 = "SELECT A.cveage FROM MVSAGEN A WHERE A.cveage = '" + CType(Fila.Cells(1).Controls(0), TextBox).Text.TrimEnd + "' "
        Datos1 = conf1.EjecutaSql(Sql1)

        If Datos1.Tables(0).Rows.Count.ToString = "0" Then
            Sql = " INSERT INTO [dbo].[MVSAGEN] (cveage, nomage, abrage, agecls, cvealm, usuari, usupwd, nvlpre, limcre, limcro, limcrv, limcrd, limcrn, stsmov, cvemov, tipmov, desmov, sermov, numser, numped, re1mov, numpedsap) "
            Sql += " VALUES ('" + CType(Fila.Cells(1).Controls(0), TextBox).Text.TrimEnd + "','" + CType(Fila.Cells(2).Controls(0), TextBox).Text.TrimEnd.ToUpper + "','2','EP',NULL,'" + CType(Fila.Cells(3).Controls(0), TextBox).Text.TrimEnd + "', '" + CType(Fila.Cells(4).Controls(0), TextBox).Text.TrimEnd + "', NULL, NULL, NULL, NULL, NULL, '21', 'A', NULL, '80','', '" + CType(Fila.Cells(5).Controls(0), TextBox).Text.TrimEnd.ToLower + "', '" + CType(Fila.Cells(6).Controls(0), TextBox).Text.TrimEnd + "', '0', NULL, NULL ) "
            Sql += " INSERT INTO [dbo].[CORRELA] (cveage, cod_zona, correla, CIERRE, liquida) "
            Sql += " SELECT '" + CType(Fila.Cells(1).Controls(0), TextBox).Text.TrimEnd + "', A.Cod_zona, '1', 'N', 'N' FROM AE_CZONA A WHERE A.E_RTN IS NOT NULL"
        Else
            Sql = " UPDATE MVSAGEN SET nomage = '" + CType(Fila.Cells(2).Controls(0), TextBox).Text.TrimEnd.ToUpper + "', usuari = '" + CType(Fila.Cells(3).Controls(0), TextBox).Text.TrimEnd + "', usupwd = '" + CType(Fila.Cells(4).Controls(0), TextBox).Text.TrimEnd + "', sermov = '" + CType(Fila.Cells(5).Controls(0), TextBox).Text.TrimEnd.ToLower + "', numser = '" + CType(Fila.Cells(6).Controls(0), TextBox).Text.TrimEnd + "' "
            Sql += " WHERE cveage = '" + CType(Fila.Cells(1).Controls(0), TextBox).Text.TrimEnd + "'"
        End If

        Datos = conf.EjecutaSql(Sql)

        gvCobradores.EditIndex = -1
        btnBuscar_Click(sender, e)
    End Sub

    Private Sub gvCobradores_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvCobradores.RowCancelingEdit
        gvCobradores.EditIndex = -1
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

End Class