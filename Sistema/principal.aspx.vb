Imports System.Data.SqlClient
Imports System.Data

Public Class principal
    Inherits System.Web.UI.Page
    Private Datos, Datos1, Datos4, Datos5 As New Data.DataSet
    Private sql As String
    Public P2, P3 As Boolean
    Public Usuario, clave, Servidor, Bd, Usuario_Aut, Clave_Aut As String
    Public Conectado As Boolean = False
    Public Tabla As DataTable = New DataTable

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
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
            Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
            Dim SQLlider As String

            If Session("Usuario_Aut") <> "" Then
                Dim conf1 As New Configuracion(Usuario, clave, Bd, Servidor)
                Dim Sql1 As String = "SELECT SEG_USUARIO, SEG_NOMUSU NOMBRE FROM FUNAMOR..CABSEG A WHERE A.SEG_USUARIO = '" + Session("Usuario_Aut") + "'"
                Datos = conf1.EjecutaSql(Sql1)

                lblBienvenido.InnerText = "Bienvenid@ '" + Datos.Tables(0).Rows(0).Item("Nombre") + "'"
            End If

            SQLlider = " Select 0 Codigo, 'Todos' Nombre_vend 
                            union all 
                        SELECT B.COD_VENDEDO Codigo, B.Nombre_vend FROM FUNAMOR..VENDEDOR A INNER JOIN FUNAMOR..VENDEDOR B ON A.VEND_LIDER = B.Cod_vendedo WHERE B.cod_vendedo not in ('0000') AND A.VEND_STATUS = 'A' GROUP BY B.Nombre_vend ,B.Cod_vendedo "
            Datos4 = conf.EjecutaSql(SQLlider)



            ''Llena los DropDownList con la lista de Lideres

            'DropLider1.Items.Add("")
            ''DropLiderRend.Items.Add("Todos")

            'For I As Integer = 0 To Datos4.Tables(0).Rows.Count - 1
            '    DropLider1.Items.Add(Datos4.Tables(0).Rows(I).Item("Nombre_vend"))
            '    'DropLiderRend.Items.Add(Datos4.Tables(0).Rows(I).Item("Nombre_vend"))
            DropLider1.DataSource = Datos4.Tables(0)
            DropLider1.DataTextField = "Nombre_vend"
            DropLider1.DataValueField = "Codigo"
            DropLider1.DataBind()

            dlVentasporFecha.DataSource = Datos4.Tables(0)
            dlVentasporFecha.DataTextField = "Nombre_vend"
            dlVentasporFecha.DataValueField = "Codigo"
            dlVentasporFecha.DataBind()
            '    Next

            sql = "SELECT SEG_ARCHIVO FROM FUNAMOR..DETSEG WHERE SEG_USUARIO = '" + Usuario_Aut + "'"
                Datos = conf.EjecutaSql(sql)


                Tabla = Datos.Tables(0)

                HABILITAR()


            Dim conf2 As New Configuracion(Usuario, clave, Bd, Servidor)
            Dim SQL2 As String
            SQL2 = " SELECT ltrim(cod_vendedo)Codigo, LTRIM(RTRIM(A.Nombre_vend)) Nombre_vend FROM FUNAMOR..VENDEDOR A WHERE A.VEND_STATUS = 'A' AND A.Cod_vendedo = '" + Usuario_Aut + "' "
            Datos5 = conf2.EjecutaSql(SQL2)



            If Datos5.Tables(0).Rows.Count > 0 Then
                If Datos5.Tables(0).Rows(0).Item("Nombre_vend").ToString.Length > 0 Then
                    DropLider1.DataSource = Datos5.Tables(0)
                    DropLider1.DataTextField = "Nombre_vend"
                    DropLider1.DataValueField = "Codigo"
                    DropLider1.DataBind()

                    dlVentasporFecha.DataSource = Datos5.Tables(0)
                    dlVentasporFecha.DataTextField = "Nombre_vend"
                    dlVentasporFecha.DataValueField = "Codigo"
                    dlVentasporFecha.DataBind()

                    dlVentasporFecha.SelectedValue = Datos5.Tables(0).Rows(0).Item("Codigo")
                    dlVentasporFecha.Enabled = False
                    CheckSuper.Enabled = False

                    DropLider1.SelectedValue = Datos5.Tables(0).Rows(0).Item("Codigo")
                    DropLider1.Enabled = False

                End If

                If Datos5.Tables(0).Rows(0).Item("Nombre_vend").ToString.Length = 0 Then
                    '' DropLiderRend.SelectedItem.Text = Datos5.Tables(0).Rows(0).Item("Nombre_vend")

                    dlVentasporFecha.DataSource = Datos5.Tables(0)
                    dlVentasporFecha.DataTextField = "Nombre_vend"
                    dlVentasporFecha.DataValueField = "Codigo"
                    dlVentasporFecha.DataBind()
                    dlVentasporFecha.Enabled = False

                    DropLider1.DataSource = Datos5.Tables(0)
                    DropLider1.DataTextField = "Nombre_vend"
                    DropLider1.DataValueField = "Codigo"
                    DropLider1.DataBind()

                    CheckSuper.Enabled = False

                    DropLider1.Enabled = False

                End If

            End If
        End If


        ''lblUsuario.Text = Usuario_Aut.Substring(0, 1).ToUpper + Usuario_Aut.Substring(1, (Usuario_Aut.Length - 1)).ToLower
    End Sub

    Sub HABILITAR()
        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'CAJ_1'"
        If Tabla.DefaultView.Count > 0 Then
            Caja_Cobros.Enabled = True
        Else
            Caja_Cobros.ToolTip = "No Tiene Acceso"
            Caja_Cobros.Style.Value = "color:darkgrey;"
        End If

        'Recibos de Caja Vendedores
        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'CAJ_2'"
        If Tabla.DefaultView.Count > 0 Then
            Caja_Ventas.Enabled = True
        Else
            Caja_Ventas.ToolTip = "No Tiene Acceso"
            Caja_Ventas.Style.Value = "color:darkgrey;"
        End If


        'Cartera De cobrador Para ver datos del cliente

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'CarCobCl'"
        If Tabla.DefaultView.Count > 0 Then
            btnCarteraCobrCliente.Enabled = True
        Else
            btnCarteraCobrCliente.ToolTip = "No Tiene Acceso"
            btnCarteraCobrCliente.Style.Value = "color:darkgrey;"
        End If

        'Recibos de Caja Cliente
        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'CAJ_3'"
        If Tabla.DefaultView.Count > 0 Then
            btncajaclie.Enabled = True
        Else
            btncajaclie.ToolTip = "No Tiene Acceso"
            btncajaclie.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'MON_1'"
        If Tabla.DefaultView.Count > 0 Then
            Monitor_Cobros.Enabled = True
        Else
            Monitor_Cobros.ToolTip = "No Tiene Acceso"
            Monitor_Cobros.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'MON_2'"
        If Tabla.DefaultView.Count > 0 Then
            Monitor_Ventas.Enabled = True
        Else
            Monitor_Ventas.ToolTip = "No Tiene Acceso"
            Monitor_Ventas.Style.Value = "color:darkgrey;"
        End If

        'Reporte Monitor Visitas
        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'Rpt-Mon3'"
        If Tabla.DefaultView.Count > 0 Then
            btnMonVisitasCobr.Enabled = True
        Else
            btnMonVisitasCobr.ToolTip = "No Tiene Acceso"
            btnMonVisitasCobr.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'REP_1'"
        If Tabla.DefaultView.Count > 0 Then
            Estadisticas_Visitas.Enabled = True
        Else
            Estadisticas_Visitas.ToolTip = "No Tiene Acceso"
            Estadisticas_Visitas.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'REP_2'"
        If Tabla.DefaultView.Count > 0 Then
            Clientes_sin_Visitar.Enabled = True
        Else
            Clientes_sin_Visitar.ToolTip = "No Tiene Acceso"
            Clientes_sin_Visitar.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'REP_3'"
        If Tabla.DefaultView.Count > 0 Then
            Clientes_Sin_Cobro.Enabled = True
        Else
            Clientes_Sin_Cobro.ToolTip = "No Tiene Acceso"
            Clientes_Sin_Cobro.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'REP_4'"
        If Tabla.DefaultView.Count > 0 Then
            Recibos_Nulos.Enabled = True
        Else
            Recibos_Nulos.ToolTip = "No Tiene Acceso"
            Recibos_Nulos.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'REP_5'"
        If Tabla.DefaultView.Count > 0 Then
            No_Estaba.Enabled = True
        Else
            No_Estaba.ToolTip = "No Tiene Acceso"
            No_Estaba.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'REP_6'"
        If Tabla.DefaultView.Count > 0 Then
            Estadistica_Cobradores.Enabled = True
        Else
            Estadistica_Cobradores.ToolTip = "No Tiene Acceso"
            Estadistica_Cobradores.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'REP_7'"
        If Tabla.DefaultView.Count > 0 Then
            No_Quiere_Continuar.Enabled = True
        Else
            No_Quiere_Continuar.ToolTip = "No Tiene Acceso"
            No_Quiere_Continuar.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'REP_8'"
        If Tabla.DefaultView.Count > 0 Then
            No_cobro_Visitas.Enabled = True
        Else
            No_cobro_Visitas.ToolTip = "No Tiene Acceso"
            No_cobro_Visitas.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'REP_9'"
        If Tabla.DefaultView.Count > 0 Then
            Ventas_Rendimiento.Enabled = True
        Else
            Ventas_Rendimiento.ToolTip = "No Tiene Acceso"
            Ventas_Rendimiento.Style.Value = "color:darkgrey;"
        End If


        'Reporte de Ventas detallada por Fecha
        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'RptVen10'"
        If Tabla.DefaultView.Count > 0 Then
            rtpVentDetXF.Enabled = True
        Else
            rtpVentDetXF.ToolTip = "No Tiene Acceso"
            rtpVentDetXF.Style.Value = "color:darkgrey;"
        End If


        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'REP_10'"
        If Tabla.DefaultView.Count > 0 Then
            Ventas_Resindidas.Enabled = True
        Else
            Ventas_Resindidas.ToolTip = "No Tiene Acceso"
            Ventas_Resindidas.Style.Value = "color:darkgrey;"
        End If

        'Movimiento de Clientes . Estados de Cuenta
        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'CLI_1'"
        If Tabla.DefaultView.Count > 0 Then
            Movimiento_Cliente.Enabled = True
        Else
            Movimiento_Cliente.ToolTip = "No Tiene Acceso"
            Movimiento_Cliente.Style.Value = "color:darkgrey;"
        End If
        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'CLI_1'"
        If Tabla.DefaultView.Count > 0 Then
            btnInformacionDeClientes.Enabled = True
        Else
            btnInformacionDeClientes.ToolTip = "No Tiene Acceso"
            btnInformacionDeClientes.Style.Value = "color:darkgrey;"
        End If


        'Ver Ihumados
        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'CLI_3'"
        If Tabla.DefaultView.Count > 0 Then
            btninhumados.Enabled = True
        Else
            btninhumados.ToolTip = "No Tiene Acceso"
            btninhumados.Style.Value = "color:darkgrey;"
        End If


        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'MAN_1'"
        If Tabla.DefaultView.Count > 0 Then
            Cobrador.Enabled = True
        Else
            Cobrador.ToolTip = "No Tiene Acceso"
            Cobrador.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'MAN_2'"
        If Tabla.DefaultView.Count > 0 Then
            Vendedor.Enabled = True
        Else
            Vendedor.ToolTip = "No Tiene Acceso"
            Vendedor.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'MAN_3'"
        If Tabla.DefaultView.Count > 0 Then
            Empleado.Enabled = True
        Else
            Empleado.ToolTip = "No Tiene Acceso"
            Empleado.Style.Value = "color:darkgrey;"
        End If
        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'MAN_4'"
        If Tabla.DefaultView.Count > 0 Then
            LinkButtonInventory.Enabled = True
        Else
            LinkButtonInventory.ToolTip = "No Tiene Acceso"
            LinkButtonInventory.Style.Value = "color:darkgrey;"
        End If
        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'LINEAS.R'"
        If Tabla.DefaultView.Count > 0 Then
            LinkButtonLines.Enabled = True
        Else
            LinkButtonLines.ToolTip = "No Tiene Acceso"
            LinkButtonLines.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'Rep_Dias'"
        If Tabla.DefaultView.Count > 0 Then
            btnNoCobro.Enabled = True
        Else
            btnNoCobro.Enabled = False
            btnNoCobro.ToolTip = "No Tiene Acceso"
            btnNoCobro.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'Gest_1'"
        If Tabla.DefaultView.Count > 0 Then
            btnGestionCobro.Enabled = True
        Else
            btnGestionCobro.Enabled = False
            btnGestionCobro.ToolTip = "No Tiene Acceso"
            btnGestionCobro.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'Ubi_1'"
        If Tabla.DefaultView.Count > 0 Then
            UbicacionLotes.Enabled = True
        Else
            UbicacionLotes.Enabled = False
            UbicacionLotes.ToolTip = "No Tiene Acceso"
            UbicacionLotes.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'Entre1'"
        If Tabla.DefaultView.Count > 0 Then
            btnEntreXF.Enabled = True
        Else
            btnEntreXF.Enabled = False
            btnEntreXF.ToolTip = "No Tiene Acceso"
            btnEntreXF.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'h_cli'"
        If Tabla.DefaultView.Count > 0 Then
            btnmovcliehist.Enabled = True
        Else
            btnmovcliehist.ToolTip = "No Tiene Acceso"
            btnmovcliehist.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'caj_val'"
        If Tabla.DefaultView.Count > 0 Then
            caja_vales.Enabled = True
        Else
            caja_vales.ToolTip = "No Tiene Acceso"
            caja_vales.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "SEG_ARCHIVO = 'RepExi1'"
        If Tabla.DefaultView.Count > 0 Then
            btnConsultaInv.Enabled = True
        Else
            btnConsultaInv.ToolTip = "No Tiene Acceso"
            btnConsultaInv.Style.Value = "color:darkgrey;"
        End If

    End Sub

    Private Sub btncajaclie_Click(sender As Object, e As EventArgs) Handles btncajaclie.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String
        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Recibos de Caja" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "Caja Cliente")
        Response.Redirect("CajaRecibos.aspx")
    End Sub

    Private Sub btnmovcliehist_Click(sender As Object, e As EventArgs) Handles btnmovcliehist.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String
        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Historial Visitas" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("DATOSCR", Datos)
        Session.Add("ReporteCR", "HisCliMovVisitas")
        Dim javaScript As String = "window.open('reportesCR.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)

    End Sub




    Private Sub btnPantallaServicios_Click(sender As Object, e As EventArgs) Handles btnPantallaServicios.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String

        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "PantallaServicios" + "')"
        Datos = conf.EjecutaSql(SQL)

        'Session.Add("Reporte", "Pantalla Servicios")
        Response.Redirect("PantallaServicios.aspx")
    End Sub
    Private Sub Movimiento_Cliente_Click(sender As Object, e As EventArgs) Handles Movimiento_Cliente.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String

        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Movimiento Clientes" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "Movimiento Cliente")
        Response.Redirect("monitorclientes.aspx")
    End Sub
    Private Sub btnInformacionDeClientes_Click(sender As Object, e As EventArgs) Handles btnInformacionDeClientes.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String

        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Informacion Clientes" + "')"
        Datos = conf.EjecutaSql(SQL)

        'Session.Add("Reporte", "Movimiento Cliente")
        Response.Redirect("Clientes/Informacion.aspx")
    End Sub


    Private Sub btninhumados_Click(sender As Object, e As EventArgs) Handles btninhumados.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String

        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Ihumados " + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "Inhumados")
        Response.Redirect("Inhumados.aspx")
    End Sub

    Private Sub Cambio_Contraseña_Click(sender As Object, e As EventArgs) Handles Cambio_Contraseña.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String

        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Cambio Contraseña" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Conectado", Conectado)
        Response.Redirect("cambioclave.aspx")
    End Sub

    'Private Sub caja_vales_Click(sender As Object, e As EventArgs) Handles caja_vales.Click
    '    Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
    '    Dim SQL As String

    '    'SQL = " Insert into FUNAMOR..LogAccesoApp 
    '    '        (Usuario, Fecha,Hora,NombreReporte) 
    '    '         values
    '    '        ('" + Session("Usuario_Aut") + "',
    '    '        '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
    '    '        '" + Format(DateTime.Now, "HH:mm:ss") + "',
    '    '        '" + "Vales" + "')"
    '    'Datos = conf.EjecutaSql(SQL)


    '    Response.Redirect("Vales.aspx")
    'End Sub


    Private Sub Caja_Cobros_Click(sender As Object, e As EventArgs) Handles Caja_Cobros.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String
        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Monitor de Cobros Caja" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "Caja Cobros")
        Response.Redirect("monitorcobros.aspx")
    End Sub

    Private Sub Caja_Ventas_Click(sender As Object, e As EventArgs) Handles Caja_Ventas.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String
        'SQL = " Insert into FUNAMOR..LogAccesoApp 
        '        (Usuario, Fecha,Hora,NombreReporte) 
        '         values
        '        ('" + Session("Usuario_Aut") + "',
        '        '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
        '        '" + Format(DateTime.Now, "HH:mm:ss") + "',
        '        '" + "Monitor Caja Ventas" + "')"
        'Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "Caja Ventas")
        Response.Redirect("monitorventas.aspx")
    End Sub

    Private Sub Cobrador_Click(sender As Object, e As EventArgs) Handles Cobrador.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String

        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Mantenimiento Cobrador" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "Mantemiento Cobrador")
        Response.Redirect("cobradores.aspx")
    End Sub

    Private Sub Vendedor_Click(sender As Object, e As EventArgs) Handles Vendedor.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String
        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Mantenimiento Vendedor" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "Mantemiento Vendedor")
        Response.Redirect("vendedores.aspx")
    End Sub

    Private Sub Empleado_Click(sender As Object, e As EventArgs) Handles Empleado.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String
        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Empleados Contratos" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "Contrato")
        Response.Redirect("Contratos.aspx")
    End Sub
    Private Sub LinkButtonInventory_Click(sender As Object, e As EventArgs) Handles LinkButtonInventory.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String
        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Inventario de equipo" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "Inventario de equipo")
        Response.Redirect("~/Mantenimiento/InventarioDeEquipo.aspx")
    End Sub
    Private Sub LinkButtonLines_Click(sender As Object, e As EventArgs) Handles LinkButtonLines.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String
        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Lineas celulares" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "Lineas celulares")
        Response.Redirect("~/Mantenimiento/Lineas.aspx")
    End Sub

    Private Sub Monitor_Cobros_Click(sender As Object, e As EventArgs) Handles Monitor_Cobros.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String
        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Monitor de Cobros" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "Cobros por agente")
        Response.Redirect("monitorcobros.aspx")
    End Sub

    Private Sub Monitor_Ventas_Click(sender As Object, e As EventArgs) Handles Monitor_Ventas.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String
        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Monitor de Ventas" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "Ventas por agente")
        Response.Redirect("monitorventas.aspx")
    End Sub

    Private Sub btnMonVisitasCobr_Click(sender As Object, e As EventArgs) Handles btnMonVisitasCobr.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String
        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Monitor de Ventas" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "MonitorVisitas")
        Response.Redirect("RptVisitasClientes.aspx")
    End Sub

    Private Sub Estadisticas_Visitas_Click(sender As Object, e As EventArgs) Handles Estadisticas_Visitas.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String
        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Estadistica de Visitas" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "Estadistica de Visitas")
        Response.Redirect("reportes.aspx")
    End Sub

    Private Sub Estadistica_Cobradores_Click(sender As Object, e As EventArgs) Handles Estadistica_Cobradores.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String
        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Estadistica Cobradores" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "Estadistica Cobradores")
        Response.Redirect("reportes.aspx")
    End Sub

    Private Sub No_cobro_Visitas_Click(sender As Object, e As EventArgs) Handles No_cobro_Visitas.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String
        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "No Cobro Visitas" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "No cobro Visitas")
        Response.Redirect("reportes.aspx")
    End Sub

    Private Sub Clientes_sin_Visitar_Click(sender As Object, e As EventArgs) Handles Clientes_sin_Visitar.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String
        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Clientes Sin Visitas" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "Clientes Sin Visitas")
        Response.Redirect("reportes.aspx")
    End Sub

    Private Sub Recibos_Nulos_Click(sender As Object, e As EventArgs) Handles Recibos_Nulos.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String
        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Recibos Nulos" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "Recibos Nulos")
        Response.Redirect("reportes.aspx")
    End Sub

    Private Sub Clientes_Sin_Cobror_Click(sender As Object, e As EventArgs) Handles Clientes_Sin_Cobro.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String
        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Clientes Sin Cobro" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "Clientes Sin Cobro")
        Response.Redirect("reportes.aspx")
    End Sub

    Private Sub No_Estaba_Click(sender As Object, e As EventArgs) Handles No_Estaba.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String

        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "No Estaba" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "No Estaba")
        Response.Redirect("reportes.aspx")
    End Sub


    ''Reporte Entregas Por Fecha

    Private Sub btnEntreXF_Click(sender As Object, e As EventArgs) Handles btnEntreXF.Click
        Dim conf As New Configuracion(Usuario, clave, "FUNAMOR", Servidor)
        Dim sql As String

        Dim javaScript As String = "$('#idEntregasXfecha').modal('show');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)

        sql = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Reporte De Entregas Por Fecha" + "')"
        Datos = conf.EjecutaSql(sql)

    End Sub

    'Reporte de Ventas por Fecha
    Private Sub rtpVentDetXF_Click(sender As Object, e As EventArgs) Handles rtpVentDetXF.Click
        Dim conf As New Configuracion(Usuario, clave, "FUNAMOR", Servidor)
        Dim sql As String

        Dim javaScript As String = "$('#idmodalVenXfecha').modal('show');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)

        sql = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Reporte De Ventas Por Fecha" + "')"
        Datos = conf.EjecutaSql(sql)

    End Sub

    'Reporte de Ventas por Fecha
    Private Sub btnBusvenxfecha_Click(sender As Object, e As EventArgs) Handles btnBusvenxfecha.Click
        Dim conf As New Configuracion(Usuario, clave, "FUNAMOR", Servidor)
        Dim sql As String

        If txtVenxfec1.Text.Length > 0 Then
            Session.Add("FechaD1", txtVenxfec1.Text)
        Else
            Session.Add("FechaD1", DateTime.Now.ToString("yyyy-MM-dd"))
        End If

        If txtVenxfec2.Text.Length > 0 Then
            Session.Add("FechaD2", txtVenxfec2.Text)
        Else
            Session.Add("FechaD2", DateTime.Now.ToString("yyyy-MM-dd"))
        End If


        If dlVentasporFecha.SelectedValue.Length = 1 Then
            sql = " EXEC FUNAMOR..SP_CR_VentasPorFechaDetallado '" + Session("FechaD1") + "', '" + Session("FechaD2") + "'"


        ElseIf dlVentasporFecha.SelectedValue.Length > 1 And txtvendrxfec.Text.Length = 0 Then
            sql = " EXEC FUNAMOR..SP_CR_VentasPorFechaDetallado_Lider '" + Session("FechaD1") + "', '" + Session("FechaD2") + "', '" + dlVentasporFecha.SelectedValue.TrimEnd + "'"

        ElseIf txtvendrxfec.Text.Length <> 0 Then
            sql = " EXEC FUNAMOR..SP_CR_VentasPorFechaDetallado_Vendedor '" + Session("FechaD1") + "', '" + Session("FechaD2") + "', '" + txtvendrxfec.Text.TrimEnd.TrimEnd + "'"
        End If
        '  End If
        Datos = conf.EjecutaSql(sql)
        Session.Add("DATOSCR", Datos)
        Session.Add("ReporteCR", "RptVentasXFecha")

        Dim javaScript As String = "window.open('reportesCR.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)

    End Sub

    Private Sub btnbuscarEntr1_Click(sender As Object, e As EventArgs) Handles btnbuscarEntr1.Click
        Dim conf As New Configuracion(Usuario, clave, "FUNAMOR", Servidor)
        Dim sql As String

        If fecha1Ent1.Value.Length > 0 Then
            Session.Add("FechaD1", fecha1Ent1.Value)
        Else
            Session.Add("FechaD1", DateTime.Now.ToString("yyyy-MM-dd"))
        End If

        If fecha1Ent2.Value.Length > 0 Then
            Session.Add("FechaD2", fecha1Ent2.Value)
        Else
            Session.Add("FechaD2", DateTime.Now.ToString("yyyy-MM-dd"))
        End If


        sql = " EXEC FUNAMOR..CR_SP_EntregasXFecha '" + Session("FechaD1") + "', '" + Session("FechaD2") + "'"
        Datos = conf.EjecutaSql(sql)

        Session.Add("DatosCR", Datos)
        Session.Add("ReporteCR", "EntregasPrg")

        Dim javaScript As String = "window.open('reportesCR.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)

    End Sub





    ''Modal para Reporte
    Private Sub BuscarFirma_Click(sender As Object, e As EventArgs) Handles IdBuscarFirma.Click
        Dim conf As New Configuracion(Usuario, clave, "AEVENTAS", Servidor)
        Dim sql As String

        sql = " EXEC [SP_CR_FIRMACLIENTES] '" + CodClienteapp.Value + "', '" + CodVendedor.Value + "'"
        Datos = conf.EjecutaSql(sql)

        Session.Add("DATOSCR", Datos)
        Session.Add("ReporteCR", "FirmaClientes")
        Dim javaScript As String = "window.open('reportesCR.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)

    End Sub

    Private Sub btnBuscarCarCobrCl_Click(sender As Object, e As EventArgs) Handles btnBuscarCarCobrCl.Click
        Dim conf As New Configuracion(Usuario, clave, "FUNAMOR", Servidor)
        Dim sql As String

        sql = " EXEC [SP_PBI_CarteraCobrador]'" + txtCodCobrCarteCl.Text.TrimEnd + "'"
        Datos = conf.EjecutaSql(sql)

        Session.Add("DATOSCR", Datos)
        Session.Add("ReporteCR", "CarteraCobradorCL")
        Dim javaScript As String = "window.open('reportesCR.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)

    End Sub

    ''Fin Modal

    ''Modal para Reporte
    Private Sub btnBuscarDias_Click(sender As Object, e As EventArgs) Handles btnBuscarDias.Click
        Dim conf As New Configuracion(Usuario, clave, "AEVENTAS", Servidor)
        Dim sql As String

        If DiasFecha1.Value.Length > 0 Then
            Session.Add("FechaD1", DiasFecha1.Value)
        Else
            Session.Add("FechaD1", DateTime.Now.ToString("yyyy-MM-dd"))
        End If

        If DiasFecha2.Value.Length > 0 Then
            Session.Add("FechaD2", DiasFecha2.Value)
        Else
            Session.Add("FechaD2", DateTime.Now.ToString("yyyy-MM-dd"))
        End If

        If idDiasCobrador.Value <> "" Then
            sql = " EXEC AECOBROS..SP_CR_CobDiasLaborados '" + Session("FechaD1") + "', '" + Session("FechaD2") + "','" + idDiasCobrador.Value + "'"
            Datos = conf.EjecutaSql(sql)
            Session.Add("Valor", "")
            'Session.Add("DATOSCR", Datos)
            'Session.Add("ReporteCR", "DiasLaborados")

            'DiasFecha1.Value = ""
            'DiasFecha2.Value = ""
            'idDiasCobrador.Value = ""
            'Session.Add("DATOSCR", Datos)
            'Session.Add("ReporteCR", "DiasLaborados")

        ElseIf DropdDiasLiderCob.SelectedValue <> "0" And CkeckDetalle.Checked = True Then

            sql = " EXEC AECOBROS..[SP_CR_CobDiasLaboradosXLider] '" + Session("FechaD1") + "', '" + Session("FechaD2") + "','" + DropdDiasLiderCob.SelectedValue + "'"
            Datos = conf.EjecutaSql(sql)
            Session.Add("Valor", "")
            'DiasFecha1.Value = ""
            'DiasFecha2.Value = ""
            'idDiasCobrador.Value = ""
            'Session.Add("DATOSCR", Datos)
            'Session.Add("ReporteCR", "DiasLaborados")

        ElseIf DropdDiasLiderCob.SelectedValue = "0" And CkeckDetalle.Checked = True Then
            sql = " EXEC AECOBROS..[SP_CR_CobDiasLaboradosXLider] '" + Session("FechaD1") + "', '" + Session("FechaD2") + "','" + DropdDiasLiderCob.SelectedValue + "'"
            Datos = conf.EjecutaSql(sql)
            Session.Add("Valor", "")
        ElseIf DropdDiasLiderCob.SelectedValue = "0" And CkeckDetalle.Checked = False Then
            sql = " EXEC AECOBROS..[SP_CR_CobDiasLaboradosXLiderResumido] '" + Session("FechaD1") + "', '" + Session("FechaD2") + "','" + DropdDiasLiderCob.SelectedValue + "'"
            Datos = conf.EjecutaSql(sql)
            Session.Add("Valor", "Resumido")

        ElseIf DropdDiasLiderCob.SelectedValue <> "0" And CkeckDetalle.Checked = False Then
            sql = " EXEC AECOBROS..[SP_CR_CobDiasLaboradosXLiderResumido] '" + Session("FechaD1") + "', '" + Session("FechaD2") + "','" + DropdDiasLiderCob.SelectedValue + "'"
            Datos = conf.EjecutaSql(sql)
            Session.Add("Valor", "Resumido")

        End If


        Session.Add("DATOSCR", Datos)
        Session.Add("ReporteCR", "DiasLaborados")


        DiasFecha1.Value = ""
        DiasFecha2.Value = ""
        idDiasCobrador.Value = ""
        DropdDiasLiderCob.SelectedValue = "0"

        Dim javaScript As String = "window.open('reportesCR.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)


    End Sub
    ''Fin Modal Dias Laborados

    Private Sub No_Quiere_Continuar_Click(sender As Object, e As EventArgs) Handles No_Quiere_Continuar.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String

        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "No Quiere Continuar" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "No Quiere Continuar")
        Response.Redirect("reportes.aspx")

    End Sub

    'Private Sub Ventas_Rendimiento_Click(sender As Object, e As EventArgs) Handles Ventas_Rendimiento.Click
    '    Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
    '    Dim SQL As String

    '    SQL = " Insert into FUNAMOR..LogAccesoApp 
    '            (Usuario, Fecha,Hora,NombreReporte) 
    '             values
    '            ('" + Session("Usuario_Aut") + "',
    '            '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
    '            '" + Format(DateTime.Now, "HH:mm:ss") + "',
    '            '" + "Ventas Rendimiento" + "')"
    '    Datos = conf.EjecutaSql(SQL)

    '    Session.Add("Reporte", "Ventas_Rendimiento")
    '    Response.Redirect("reportes.aspx")
    'End Sub

    'Private Sub Ventas_Rendimiento_Click(sender As Object, e As EventArgs) Handles Ventas_Rendimiento.Click
    '    Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
    '    Dim SQL As String

    '    SQL = " Insert into FUNAMOR..LogAccesoApp 
    '            (Usuario, Fecha,Hora,NombreReporte) 
    '             values
    '            ('" + Session("Usuario_Aut") + "',
    '            '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
    '            '" + Format(DateTime.Now, "HH:mm:ss") + "',
    '            '" + "Reporte Rend-Lider" + "')"
    '    Datos = conf.EjecutaSql(sql)

    '    Dim javaScript As String = "$('#idmodalRendLider').modal('show');"
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)

    'End Sub



    'Reporte por cobrador por fecha por empresa

    Private Sub Ventas_Rendimiento_Click(sender As Object, e As EventArgs) Handles Ventas_Rendimiento.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String

        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Reporte Rend-Lider" + "')"
        Datos = conf.EjecutaSql(sql)

        Dim javaScript As String = "$('#idmodalCobroResumidoXfechxCobr').modal('show');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)

    End Sub

    Private Sub btnCobrosResXFecha_Click(sender As Object, e As EventArgs) Handles btnCobrosResXFecha.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String

        If RendFechaDesde1.ToString.Length > 0 Then
            Session.Add("Fecha1I", RendFechaDesde1.Text)
        Else
            Session.Add("Fecha1I", DateTime.Now.ToString("yyyy-MM-dd"))
        End If

        If RendFechaHasta2.Text.Length > 0 Then
            Session.Add("Fecha2I", RendFechaHasta2.Text)
        Else
            Session.Add("Fecha2I", DateTime.Now.ToString("yyyy-MM-dd"))
        End If



        If CheckSuper.Checked = False And DropLider1.SelectedValue <> "0" Then
            SQL = " EXEC AEVENTAS..CR_COB_REND '" + Session("Fecha1I") + "', '" + Session("Fecha2I") + "', '" + DropLider1.SelectedValue + "' "
            Datos = conf.EjecutaSql(SQL)

        ElseIf CheckSuper.Checked = False And DropLider1.SelectedValue = "0" Then
            SQL = "EXEC AEVENTAS..[CR_LIDER_REND] '" + Session("Fecha1I") + "', '" + Session("Fecha2I") + "'"
            Datos = conf.EjecutaSql(SQL)

        ElseIf CheckSuper.Checked = True Then
            SQL = "EXEC AEVENTAS..[CR_LIDER2_REND] '" + Session("Fecha1I") + "', '" + Session("Fecha2I") + "'"
            Datos = conf.EjecutaSql(SQL)

        Else

        End If

        If Datos.Tables(0).Rows.Count < 0 Then
            MsgBox("NO hay registros")
            Exit Sub
        End If

        Session.Add("DATOSCR", Datos)
        Session.Add("ReporteCR", "Rendimiento")
        Dim javaScript As String = "window.open('reportesCR.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)
    End Sub

    'Reporte cartera de cobrador con datos generales del cliente 


    Private Sub btnCarteraCobrCliente_Click(sender As Object, e As EventArgs) Handles btnCarteraCobrCliente.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String

        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Cartera Cobr Cliente" + "')"
        Datos = conf.EjecutaSql(SQL)

        Dim javaScript As String = "$('#idmodalCobrCarteraCliente').modal('show');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)

    End Sub

    Private Sub BuscarRendLider1_Click(sender As Object, e As EventArgs) Handles BuscarRendLider1.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String

        If RendFechaDesde1.ToString.Length > 0 Then
            Session.Add("Fecha1I", RendFechaDesde1.Text)
        Else
            Session.Add("Fecha1I", DateTime.Now.ToString("yyyy-MM-dd"))
        End If

        If RendFechaHasta2.Text.Length > 0 Then
            Session.Add("Fecha2I", RendFechaHasta2.Text)
        Else
            Session.Add("Fecha2I", DateTime.Now.ToString("yyyy-MM-dd"))
        End If



        If CheckSuper.Checked = False And DropLider1.SelectedValue <> "0" Then
            SQL = " EXEC AEVENTAS..CR_COB_REND '" + Session("Fecha1I") + "', '" + Session("Fecha2I") + "', '" + DropLider1.SelectedValue + "' "
            Datos = conf.EjecutaSql(SQL)

        ElseIf CheckSuper.Checked = False And DropLider1.SelectedValue = "0" Then
            SQL = "EXEC AEVENTAS..[CR_LIDER_REND] '" + Session("Fecha1I") + "', '" + Session("Fecha2I") + "'"
            Datos = conf.EjecutaSql(SQL)

        ElseIf CheckSuper.Checked = True Then
            SQL = "EXEC AEVENTAS..[CR_LIDER2_REND] '" + Session("Fecha1I") + "', '" + Session("Fecha2I") + "'"
            Datos = conf.EjecutaSql(SQL)

        Else

        End If

        If Datos.Tables(0).Rows.Count <0 Then
            MsgBox("NO hay registros")
            Exit Sub
        End If

        Session.Add("DATOSCR", Datos)
        Session.Add("ReporteCR", "Rendimiento")
        Dim javaScript As String = "window.open('reportesCR.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)
    End Sub


    Private Sub Ventas_Resindidas_Click(sender As Object, e As EventArgs) Handles Ventas_Resindidas.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String

        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Ventas Resindidas" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "Ventas_Resindida")
        Response.Redirect("reportes.aspx")
    End Sub

    Private Sub btnGestionCobro_Click(sender As Object, e As EventArgs) Handles btnGestionCobro.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String

        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Monitor Gestion Cobro" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "MonitorGestion")
        Response.Redirect("MonitorGestion.aspx")

    End Sub

    Private Sub UbicacionLotes_Click(sender As Object, e As EventArgs) Handles UbicacionLotes.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String

        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Cambiar Ubicacion" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Reporte", "UbicacionLotes")
        Response.Redirect("UbicacionLotes.aspx")
    End Sub

    Private Sub Salir_Click(sender As Object, e As EventArgs) Handles Salir.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String

        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Salir del Sistema" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Clear()
        Response.Redirect("inicio.aspx")
    End Sub

    Private Sub btnConsultaInv_Click(sender As Object, e As EventArgs) Handles btnConsultaInv.Click
        Dim conf As New Configuracion(Usuario, clave, Bd, Servidor)
        Dim SQL As String

        SQL = " Insert into FUNAMOR..LogAccesoApp 
                (Usuario, Fecha,Hora,NombreReporte) 
                 values
                ('" + Session("Usuario_Aut") + "',
                '" + Format(Date.Now, "yyyy/MM/dd").ToString + "',
                '" + Format(DateTime.Now, "HH:mm:ss") + "',
                '" + "Consulta de Inventarios" + "')"
        Datos = conf.EjecutaSql(SQL)

        Session.Add("Destino", "RepExi.aspx")
        Response.Redirect("RepExi.aspx")
    End Sub



    'Private Sub btnSalir_Click(sender As Object, e As ImageClickEventArgs) Handles btnSalir.Click
    '    Session.Clear()
    '    Response.Redirect("inicio.aspx")
    'End Sub

    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub


End Class