Public Class UbicacionLotes
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut As String
    Private Datos, Datos1, Datos2, Datos3, DatosCliente, DatosU1, DatosU2 As DataSet
    Private Liquida, Liquida2 As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Session("Usuario") = "" Then
            Response.Redirect("inicio.aspx")
        End If

        Usuario = Session("Usuario")
        Clave = Session("Clave")
        Servidor = Session("Servidor")
        Bd = Session("Bd")
        Usuario_Aut = Session("Usuario_Aut")
        Clave_Aut = Session("Clave_Aut")

        lblMsjError.Text = ""
            lblMsjError.ControlStyle.CssClass = ""
        Catch ex As Exception
            Dim msg = "Problema al la cargar página, por favor vuelva a intentarlo : " & ex.Message
            'RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try
    End Sub

    Protected Sub btnSalir_Click(sender As Object, e As ImageClickEventArgs) Handles btnSalir.Click
        Response.Redirect("principal.aspx")
    End Sub


    Protected Sub txtNccodigoBuscar_TextChanged(sender As Object, e As EventArgs)
        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String, cliente = ""
        If txtNccodigo.Text.Length > 6 Then
            cliente = txtNccodigo.Text.Trim
        ElseIf txtNccodigo.Text.Length < 7 Then
            cliente = "P01" + "-" + RTrim(LTrim(txtNccodigo.Text.Trim))
        End If

        Sql = "Select Nombre_clie Nombre from Funamor..Clientes where codigo_clie = '" + cliente.ToString.TrimEnd + "'"
        Datos = conf.EjecutaSql(Sql)

        If Datos.Tables(0).Rows.Count > 0 Then
            txtNombre.Text = Datos.Tables(0).Rows(0).Item("Nombre")
        End If

    End Sub
    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String



        Sql = " Select top 500 c.Codigo_clie,c.Nombre_clie,c.identidad, "
        Sql += " CONT_JARDIN Jardin, CONT_SERVI NivelLote, "
        Sql += " co.CONT_CLETRA+' - '+ co.CONT_CNUM UbicacionContrato, "
        Sql += " c1.CONT_LETRA+' - '+ c1.CONT_NNUM UbicacionLote,"
        Sql += " C1.CONT_NUMERO Contrato "
        Sql += " from FUNAMOR..CONTRATO co "
        Sql += " inner join FUNAMOR..CONTRAT1 c1  on c1.CONT_NUMERO = co.CONT_NUMERO "
        Sql += " inner join CLIENTES c on c.Codigo_clie = co.Codigo_clie "
        Sql += " WHERE SUBSTRING(c.Codigo_clie,1,3) = 'P01' and  "
        Sql += " c.CODIGO_CLIE like '%" + txtCliente.Text + "%' "
        Sql += " and c.identidad like '%" + txtidentidad.Text + "%' "
        Sql += " and c.NOMBRE_CLIE Like '%" + TxtCliente2.Text + "%' "
        Sql += " and c.NOMBRE_CLIE Like '%" + TxtCliente3.Text + "%' "
        Sql += " ORDER BY c.Codigo_clie "
        Datos = conf.EjecutaSql(Sql)

        gvClientes.DataSource = Datos.Tables(0)
        gvClientes.DataBind()
    End Sub

    'Protected Sub txtNccodig_TextChange_Click(sender As Object, e As EventArgs)
    '    Dim conf, conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
    '    Dim SQL, cliente, SqlCliente, valor As String

    '    cliente = "P01" + "-" + RTrim(LTrim(txtNccodig.Text.Trim))
    '    SqlCliente = " Select case when (Select codigo_clie from FUNAMOR..clientes where Codigo_clie = '" + cliente.ToString + "') = ' ' then 'No' else isnull((Select codigo_clie from FUNAMOR..clientes where Codigo_clie = '" + cliente.ToString + "' ),'No') end Cliente "
    '    Datos = conf.EjecutaSql(SqlCliente)

    '    valor = Datos.Tables(0).Rows(0).Item("Cliente").ToString.TrimEnd


    '    SQL = "Select nombre_clie from funamor..clientes where codigo_clie = '" + valor + "'"
    '    Datos1 = conf1.EjecutaSql(SQL)

    '    nombreclieChange.Value = Datos.Tables(0).Rows(0).Item("Nombre_clie").ToString.TrimEnd


    'End Sub

    'Protected Sub btnhacercambio_click(sender As Object, e As EventArgs) Handles btnhacercambio.Click

    '    PanelUbicacion.Visible = True


    'End Sub

    Protected Sub idcancelarub_Click(sender As Object, e As EventArgs) Handles idcancelarub.Click
        PanelUbicacion.Visible = False
        txtubicacion.Text = ""

        DropJardin.SelectedIndex = 0
    End Sub

    Protected Sub btnsalvarUb_Click(sender As Object, e As EventArgs) Handles btnsalvarUb.Click
        Try



            Dim Conf, Conf2, Conf3, confi, confClie, ConfU1, ConfU2 As New Configuracion(Usuario, Clave, "FUNAMOR", Servidor)
            Dim SqlInsert, cliente, SqlJardin, SqlCliente, valor, SqlExiste, Jardin, codigojardin, Letra, Nombre, SqlNombre, SqlUbicacion, ClienteOcupado1, ClienteOcupado2 As String

            If txtNccodigo.Text.Length > 6 Then
                cliente = txtNccodigo.Text.Trim
            ElseIf txtNccodigo.Text.Length < 7 Then
                cliente = "P01" + "-" + RTrim(LTrim(txtNccodigo.Text.Trim))
            End If


            SqlCliente = " IF EXISTS  (Select codigo_clie from FUNAMOR..clientes where Codigo_clie = '" + cliente.ToString + "') Select 'Si' Cliente else SElect 'No' Cliente "
            Datos = Conf.EjecutaSql(SqlCliente)

            valor = Datos.Tables(0).Rows(0).Item("Cliente").ToString.TrimEnd

            If valor = "No" Then
                '  Response.Write("<script language=javascript>alert('Cliente No Existe - INTENTELO NUEVAMENTE');</script>")
                lblMsjError.Text = "Error: Cliente No existe - Colocar el Correcto "
                lblMsjError.ControlStyle.CssClass = "alert alert-danger"
                Exit Sub
            Else

                ''Busca el Jardin del Cliente.
                If DropJardin.SelectedValue.TrimEnd.Length = 0 Then

                    lblMsjError.Text = "Error: Debe Seleccionar el Jardin- Intentelo Nuevamente "
                    lblMsjError.ControlStyle.CssClass = "alert alert-danger"
                    Exit Sub

                ElseIf DropJardin.SelectedValue.TrimEnd.Length > 0 Then
                    SqlNombre = "Select ltrim(rtrim(codigo_clie))+'-'+rtrim(ltrim(nombre_clie))Nombre from FUNAMOR..CLIENTES WHERE CODIGO_CLIE = '" + cliente.TrimEnd + "'"
                    DatosCliente = confClie.EjecutaSql(SqlNombre)
                    Nombre = DatosCliente.Tables(0).Rows(0).Item("Nombre").ToString.TrimEnd()

                    SqlExiste = " Select Case when (Select UBIJ_LETRA from FUNAMOR..UBIJARD where UBIJ_JARDIN = '" + DropJardin.SelectedValue.TrimEnd + "' and ubij_numero = '" + txtubicacion.Text + " ') is null then 'No' else "
                    SqlExiste += " (Select UBIJ_LETRA from FUNAMOR..UBIJARD where UBIJ_JARDIN = '" + DropJardin.SelectedValue.TrimEnd + "' and ubij_numero = '" + txtubicacion.Text + " ') end Existe"
                    Datos3 = Conf3.EjecutaSql(SqlExiste)
                    Letra = Datos3.Tables(0).Rows(0).Item("Existe").ToString


                    SqlUbicacion = " Select ubij_numero Numero, rtrim(ltrim(Ubij_client)) Cliente, ubij_letra,c.codigo_clie +'-'+ rtrim(ltrim(c.nombre_clie))Nombre, 
                                ubij_jardin Jardin from ubijard j 
                                inner join clientes c on c.codigo_clie = j.ubij_client 
                                where ubij_jardin = '" + DropJardin.SelectedValue.TrimEnd + "' and ubij_numero = '" + txtubicacion.Text + " '"
                    DatosU2 = ConfU2.EjecutaSql(SqlUbicacion)

                    ''Verifica si la ubicacion ingresada pertenece a un cliente
                    If DatosU2.Tables(0).Rows.Count > 0 Then
                        ClienteOcupado2 = DatosU2.Tables(0).Rows(0).Item("Nombre").ToString()


                        If DatosU2.Tables(0).Rows(0).Item("Numero").ToString.TrimEnd() = txtubicacion.Text And DatosU2.Tables(0).Rows(0).Item("Jardin").ToString.TrimEnd() = DropJardin.SelectedValue.TrimEnd And DatosU2.Tables(0).Rows(0).Item("Cliente").ToString.TrimEnd.Length > 1 Then
                            lblMsjError.Text = "Error: La ubicacion Pertenece a : '" + ClienteOcupado2.TrimEnd + "'"
                            lblMsjError.ControlStyle.CssClass = "alert alert-danger"
                            Exit Sub
                            'Response.Write("<script language=javascript>alert('La ubicacion que Ingresó Pertenece a: " + ClienteOcupado2.TrimEnd + "');</script>")
                        End If

                    Else
                        ''Si la ubicacion existe y está libre que guarde.
                        If Datos3.Tables(0).Rows(0).Item("Existe").ToString = "No" Or txtubicacion.Text.Length = 0 Then

                            lblMsjError.Text = "Error: Ubicacion No existe- Intentelo Nuevamente "
                            lblMsjError.ControlStyle.CssClass = "alert alert-danger"
                            Exit Sub
                            ''Response.Write("<script language=javascript>alert('Ubicacion No existe- Intentelo Nuevamente');</script>")

                        Else

                            SqlInsert = "Exec FUNAMOR..SP_VS_CambiaUBicacion ' " + cliente + "',               
                    '" + DropJardin.SelectedValue.TrimEnd + "',                 
                    '" + txtubicacion.Text.TrimEnd + "',                
                    '" + Datos3.Tables(0).Rows(0).Item("Existe").ToString + "',                 
                    '" + Session("Usuario_Aut") + "'"

                            confi.EjecutaSql(SqlInsert)


                            'Response.Write("<script language=javascript>alert('Ubicacion Guardada en Cliente: " + Nombre.TrimEnd + "');</script>")
                            lblMsjError.Text = "Ubicacion Guardada Exitosamente "
                            lblMsjError.ControlStyle.CssClass = "alert alert-success"



                            txtNccodigo.Text = ""
                            txtubicacion.Text = ""
                            DropJardin.SelectedIndex = 0
                            PanelUbicacion.Visible = False
                        End If
                    End If
                End If
            End If
            AlertHelper.GenerateAlert("success", "Guardado", alertPlaceholder)

        Catch ex As Exception
            Dim msg = "Problema al la los datos, por favor vuelva a intentarlo : " & ex.Message
            'RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try
    End Sub


    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub

    Private Sub gvClientes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvClientes.RowCommand
        If e.CommandName = "Detalle" Then

            Session.Add("CodigoCliente", gvClientes.Rows(e.CommandArgument.ToString).Cells(1).Text.ToString.TrimEnd)

            PanelUbicacion.Visible = True
            txtNccodigo.Text = Session("CodigoCliente")
            txtNccodigoBuscar_TextChanged(sender, e)
            'Dim javaScript As String = "$('#idmodalUbicacion').modal('show');"
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)
            'ClientesUbicacion(gvClientes.Rows(e.CommandArgument.ToString).Cells(1).Text.ToString.TrimEnd)
        End If
    End Sub
End Class