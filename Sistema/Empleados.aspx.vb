Imports CrystalDecisions.Shared
Imports System.IO
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Web.Script.Services

Public Class Empleados
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut As String
    Private Datos, Datos1, Datos2 As DataSet
    Private Conector As SqlConnection
    Private Adaptador As SqlDataAdapter
    Private SqlCMD As SqlCommand
    Public nombreDeEmpleado As String = ""
    Dim numeroDeEmpleado As String
    Public anEmployeeIsSelected As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario") = "" Then
            Response.Redirect("inicio.aspx")
        End If

        If Not IsPostBack Then
            Session.Add("Orden1", "0")
            If Session("Estatus") = "Pendiente" Then
                txtNombre.Text = Session("Nombre")
                txtIdentidad.Text = Session("identidad")
            End If
        End If

        Session.Timeout = 90
        Usuario = Session("Usuario")
        Clave = Session("Clave")
        Servidor = Session("Servidor")
        Bd = Session("Bd")
        Usuario_Aut = Session("Usuario_Aut")
        Clave_Aut = Session("Clave_Aut")

        txtCodigo.Text = Session("Codigo_Empleado")

        If txtCodigo.Text.Length > 0 And IsPostBack = False Then
            llenar_Campos()
        Else
            txtCodigo.Enabled = True
            'Limpiar campos
            txtNombre.Text = ""
            txtIdentidad.Text = ""
            txtDireccion.Text = ""
            txtFechaN.Text = ""
            txtSexo.Text = ""
            txtDepto.Text = ""
            txtCiudad.Text = ""
            txtCivil.Text = ""
            txtCargo.Text = ""
            txtDepto.Text = ""
            txtTmpPer.Text = ""
            txtSueldo.Text = ""
            txtTipoPlan.Text = ""
            txtActivo.Text = ""
            txtFechaI.Text = ""
            TxtMotivo.Text = ""
        End If
        If (Session("Codigo_Empleado") Is Nothing) Then
            EmployeeCard.Visible = False
            Documents.Visible = False
            anEmployeeIsSelected = False

        ElseIf (Session("Codigo_Empleado").ToString().Length < 1) Then
            EmployeeCard.Visible = False
            Documents.Visible = False
            anEmployeeIsSelected = False

        Else
            EmployeeCard.Visible = True
            Documents.Visible = True
            anEmployeeIsSelected = True


        End If
        If anEmployeeIsSelected Then
            btnActualizar.Text = "Actualizar"
        Else
            btnActualizar.Text = "Crear"
        End If

    End Sub

    Sub llenar_Campos()
        Dim conf As New Configuracion(Usuario, Clave, "FUNAMOR", Servidor)
        Dim Sql
        Sql = " SELECT *
                FROM PLAEMP A
                WHERE A.P_num_emple = '" + txtCodigo.Text + "' "
        Datos = conf.EjecutaSql(Sql)
        Session.Add("DATOSCR", Datos)
        Session.Add("Reporte", "Ficha_Empleado")

        If Datos.Tables(0).Rows.Count = 0 Then
            Exit Sub
        End If

        'If Not String.IsNullOrWhiteSpace(Datos.Tables(0).Rows(0).Item("P_carnet_fo").ToString) Then
        '    Dim FilePath As String = Server.MapPath("~\fotosCarnet\") + Datos.Tables(0).Rows(0).Item("P_num_emple").ToString.TrimEnd + ".jpg"
        '    Dim stringB64 As String
        '    Using fs As New FileStream(FilePath, FileMode.Open)
        '        Dim br As New BinaryReader(fs)
        '        Dim datosImg As Byte()
        '        datosImg = br.ReadBytes(br.BaseStream.Length)
        '        stringB64 = Convert.ToBase64String(datosImg, 0, datosImg.Length)
        '        Dim imgStr = "data:image/png;base64," + stringB64
        '        'imgFoto.Attributes.Add("src", imgStr)
        '        'imgFoto.Visible = True
        '    End Using

        'End If

        nombreDeEmpleado = Datos.Tables(0).Rows(0).Item("P_nomb_empl").ToString
        txtNombre.Text = nombreDeEmpleado
        txtIdentidad.Text = Datos.Tables(0).Rows(0).Item("P_identidad").ToString
        txtDireccion.Text = Datos.Tables(0).Rows(0).Item("P_dir_emple").ToString
        txtFechaN.Text = If(String.IsNullOrEmpty(Datos.Tables(0).Rows(0).Item("P_fecha_nac").ToString), "", Format(Datos.Tables(0).Rows(0).Item("P_fecha_nac"), "yyyy-MM-dd").ToString)
        txtSexo.Text = Datos.Tables(0).Rows(0).Item("P_sexo").ToString
        txtDepto.Text = Datos.Tables(0).Rows(0).Item("P_cod_sucur").ToString
        txtCiudad.Text = Datos.Tables(0).Rows(0).Item("P_lugar_nac").ToString
        txtCivil.Text = Datos.Tables(0).Rows(0).Item("P_estado_ci").ToString
        txtCargo.Text = Datos.Tables(0).Rows(0).Item("P_cod_puest").ToString
        txtDepto.Text = Datos.Tables(0).Rows(0).Item("P_cod_depar").ToString
        txtTmpPer.Text = Datos.Tables(0).Rows(0).Item("P_temporal").ToString
        txtSueldo.Text = Datos.Tables(0).Rows(0).Item("P_sueldo_ac").ToString
        txtTipoPlan.Text = Datos.Tables(0).Rows(0).Item("P_tipo_plan").ToString
        txtActivo.Text = Datos.Tables(0).Rows(0).Item("P_status").ToString
        txtFechaI.Text = If(String.IsNullOrEmpty(Datos.Tables(0).Rows(0).Item("P_fecha_ing").ToString), " ", Format(Datos.Tables(0).Rows(0).Item("P_fecha_ing"), "yyyy-MM-dd").ToString)
        TxtMotivo.Text = Datos.Tables(0).Rows(0).Item("P_causa_can").ToString

        If Datos.Tables(0).Rows(0).Item("P_fecha_can").Equals(DBNull.Value) = False Then
            txtFechaS.Text = Format(Datos.Tables(0).Rows(0).Item("P_fecha_can"), "yyyy-MM-dd").ToString
        End If

    End Sub
    Private Sub btnSalir_Click(sender As Object, e As ImageClickEventArgs) Handles btnSalir.Click
        Session.Add("Codigo_Empleado", "")
        Session.Add("Nombre_Completo", "")
        Session.Add("Identidad", "")
        Response.Redirect("Empleados.aspx")
    End Sub

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        Dim conf As New Configuracion(Usuario, Clave, "PRUEBA", Servidor)
        Dim Sql As String

        If String.IsNullOrEmpty(txtNombre.Text.ToString) Then
            Msg("Debe ingresar Nombre Empleado")
            txtNombre.Focus()
            Exit Sub
        End If

        If String.IsNullOrEmpty(txtIdentidad.Text.ToString) Then
            Msg("Debe ingresar Identidad")
            txtIdentidad.Focus()
            Exit Sub
        End If

        If String.IsNullOrEmpty(txtFechaN.Text.ToString) Then
            Msg("Debe ingresar Fecha Nacimiento")
            txtFechaN.Focus()
            Exit Sub
        End If

        If txtCodigo.Enabled Then

            'If archivoInput.HasFile = True Then
            '    Dim ruta As String = Server.MapPath("~\fotosCarnet\") + txtCodigo.Text + ".jpg"
            '    Dim stream As Stream = archivoInput.PostedFile.InputStream
            '    Dim BR As BinaryReader = New BinaryReader(stream)
            '    Dim Bytes As Byte() = BR.ReadBytes(stream.Length)

            '    Using fs As New FileStream(ruta, FileMode.Create)
            '        Using bw As New BinaryWriter(fs)
            '            Session.Add("rutaFoto", txtCodigo.Text + ".jpg")
            '            bw.Write(Bytes)
            '            bw.Close()
            '        End Using
            '    End Using
            'End If

            Sql = " INSERT INTO [dbo].[PLAEMP]
                    ([P_num_emple]
                    ,[P_nom_emp]
                    ,[P_dir_emple]
                    ,[P_identidad]
                    ,[P_fecha_nac]
                    ,[P_sexo]
                    ,[P_lugar_nac]
                    ,[P_estado_ci]
                    ,[P_cod_puest]
                    ,[P_cod_depar]
                    ,[P_temporal]
                    ,[P_fecha_ing]
                    ,[P_fecha_can]
                    ,[P_causa_can]
                    ,[P_sueldo_ac]
                    ,[P_tipo_plan]
                    ,[P_carnet_fo]
                    ,[P_status])
                VALUES
                    ('" + txtCodigo.Text + "',
                    '" + txtNombre.Text + "',
                    '" + txtDireccion.Text + "',
                    '" + txtIdentidad.Text + "',
                    '" + txtFechaN.Text + "',
                    '" + txtSexo.SelectedValue + "',
                    '" + txtCiudad.Text + "',
                    '" + txtCivil.SelectedValue + "',
                    '" + txtCargo.Text + "',
                    '" + txtDepto.Text + "',
                    '" + txtTmpPer.SelectedValue + "',
                    '" + txtFechaI.Text + "',
                    '" + txtFechaS.Text + "',
                    '" + TxtMotivo.Text + "',
                    '" + txtSueldo.Text + "',
                    '" + IIf(Session("rutaFoto").ToString Is Nothing, " ", Session("rutaFoto").ToString) + "',
                    '" + txtTipoPlan.SelectedValue + "',
                    '" + txtActivo.SelectedValue + "')"
            Datos = conf.EjecutaSql(Sql)
            Msg("Se agregaron los datos")
        Else
            'Dim rutaFoto As String
            'If archivoInput.HasFile = True Then
            '    Dim ruta As String = Server.MapPath("~\fotosCarnet\") + txtCodigo.Text + ".jpg"
            '    Dim stream As Stream = archivoInput.PostedFile.InputStream
            '    Dim BR As BinaryReader = New BinaryReader(stream)
            '    Dim Bytes As Byte() = BR.ReadBytes(stream.Length)

            '    Using fs As New FileStream(ruta, FileMode.Create)
            '        Using bw As New BinaryWriter(fs)
            '            Session.Add("rutaFoto", txtCodigo.Text + ".jpg")
            '            bw.Write(Bytes)
            '            bw.Close()
            '        End Using
            '    End Using
            'End If

            'If Not Session("rutaFoto") Is Nothing Then
            '    rutaFoto = Session("rutaFoto").ToString
            'Else
            '    rutaFoto = ""
            'End If
            Sql = "UPDATE PLAEMP SET                 
                P_nomb_empl = '" + txtNombre.Text + "',
                P_dir_emple = '" + txtDireccion.Text + "',
                P_identidad = '" + txtIdentidad.Text + "',
                P_fecha_nac = '" + txtFechaN.Text + "',
                P_sexo = '" + txtSexo.SelectedValue + "',
                P_lugar_nac = '" + txtCiudad.Text + "',
                P_estado_ci = '" + txtCivil.SelectedValue + "',
                P_cod_puest = '" + txtCargo.Text + "',
                P_cod_depar = '" + txtDepto.Text + "',
                P_temporal = '" + txtTmpPer.SelectedValue + "',
                P_status = '" + txtActivo.SelectedValue + "',
                P_fecha_ing = '" + txtFechaI.Text + "',
                P_fecha_can = '" + txtFechaS.Text + "',
                P_causa_can = '" + TxtMotivo.Text + "',
                P_sueldo_ac = '" + txtSueldo.Text + "',
                P_tipo_plan = '" + txtTipoPlan.SelectedValue + "' 
                WHERE P_num_emple = '" + txtCodigo.Text + "'"
            Datos = conf.EjecutaSql(Sql)
            Msg("Se Guardaron los Cambios")

        End If

        llenar_Campos()

    End Sub

    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub


End Class

