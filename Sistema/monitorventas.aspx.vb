﻿Imports System.Data.Entity.Core
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Public Class monitorventas
    Inherits System.Web.UI.Page

    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut As String
    Private Datos, Datos1, Datos2, Datos3 As DataSet
    Private Total As Decimal = 0
    Private Visitados As Decimal = 0
    Private Ventas As Decimal = 0
    Private Verdes As Decimal = 0
    Private Recibos As Decimal = 0
    Private Cobradores As Decimal = 0
    Private Liquida, Liquida2 As String
    Private initialPayment As String
    Private dangerMsg = "Error inesperado: "
    ' Private Tabla As DataTable

    Public Event DataSendEvent As EventHandler(Of ClientDataReceivedEventArgs)
    Public Event DataContractSendEvent As EventHandler(Of ContractDataReceivedEventArgs)
    Public Event ProductSendEvent As EventHandler(Of ProductDataReceivedEventArgs)
    Public Event ConfirmacionSi As EventHandler(Of EventArgs)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("Usuario") = "" Then
                Response.Redirect("inicio.aspx")
            End If

            Usuario = Session("Usuario")
            Clave = Session("Clave")
            Servidor = Session("Servidor")
            Bd = "AEVentas"
            Usuario_Aut = Session("Usuario_Aut")
            Clave_Aut = Session("Clave_Aut")
            Session.Timeout = 90
            Dim isAuthToAdvanced As Boolean = AuthHelper.isAuthorized(Usuario_Aut, "VENTAS_A")
            If isAuthToAdvanced Then
                btnAdvanced.Visible = True
            End If

            'Tabla.Columns.Add("CodigoVendedor")
            'Tabla.Columns.Add("Producto")
            'Tabla.Columns.Add("Cantidad")
            'Tabla.Columns.Add("ValorProducto")
            'Tabla.Columns.Add("Costo")
            'Tabla.Columns.Add("Vendedor")
            'Tabla.Columns.Add("nombreVend")
            'Tabla.Columns.Add("NombreCliente")
            'Tabla.Columns.Add("CodigoCliente")
            'Tabla.Columns.Add("Letra")
            'Tabla.Columns.Add("Cuota")
            'Tabla.Columns.Add("Valor")
            'Tabla.Columns.Add("Total")

            If Not IsPostBack Then
                'Throw New ArgumentException("Ejemplo de error")
                Session.Add("Orden", "0")
                dlMostrar.Items.Add("POR DIA DE TRABAJO")
                dlMostrar.Items.Add("POR LIQUIDACION")
                dlMostrar.Items.Add("SIN LIQUIDACION")
                dlMostrar.Items.Add("SIN ACTIVIDAD")
                dlMostrar.SelectedIndex = 0

                dlRun.Items.Add("False")
                dlRun.Items.Add("True")
                dlRun.SelectedIndex = 0

                If Usuario_Aut = "MANAGER" Or Usuario_Aut = "MDERAS" Or Usuario_Aut = "REINALDO" Or Usuario_Aut = "yasmin" Or Usuario_Aut = "YASMIN" Or Usuario_Aut = "manager" Or Usuario_Aut = "mderas" Or Usuario_Aut = "reinaldo" Then
                    btnArreglarVenta.Visible = True
                End If

                If Session("Reporte") = "Caja Ventas" Then
                    dlMostrar.Items.Add("SIN PROCESAR")
                    dlMostrar.Items.Add("POR PROCESADOS")
                    dlMostrar.SelectedIndex = 4
                End If
                dlMostrar.Items.Add("VERDES")



                Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)

                Dim SQL As String
                SQL = " SELECT B.NOMBRE_VEND FROM FUNAMOR..VENDEDOR A INNER JOIN FUNAMOR..VENDEDOR B ON A.VEND_LIDER = B.Cod_vendedo WHERE A.VEND_STATUS = 'A' GROUP BY B.NOMBRE_VEND "
                Datos2 = conf.EjecutaSql(SQL)

                dllider.Items.Add("")
                dllider.Items.Add("TODOS")
                For I As Integer = 0 To Datos2.Tables(0).Rows.Count - 1
                    dllider.Items.Add(Datos2.Tables(0).Rows(I).Item("NOMBRE_VEND"))
                Next

                Dim conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
                Dim SQL1 As String
                SQL1 = " Select B.vzon_nombre From FUNAMOR..VENDEDOR A Left Join FUNAMOR..VZONA B ON A.vzon_codigo = B.vzon_codigo WHERE A.VEND_STATUS = 'A' Group BY B.vzon_nombre "
                Datos = conf1.EjecutaSql(SQL1)

                dlzona.Items.Add("")
                For I As Integer = 0 To Datos.Tables(0).Rows.Count - 1
                    'dlzona.Items.Add(Datos.Tables(0).Rows(I).Item("vzon_nombre"))
                    Dim vzonNombre As Object = Datos.Tables(0).Rows(I)("vzon_nombre")
                    If Not IsDBNull(vzonNombre) Then
                        dlzona.Items.Add(vzonNombre.ToString())
                    End If
                Next

                Dim conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
                Dim SQL2 As String
                SQL2 = " SELECT LTRIM(RTRIM(A.Nombre_vend)) Nombre_vend FROM FUNAMOR..VENDEDOR A WHERE A.Cod_vendedo = '" + Usuario_Aut + "' "
                Datos1 = conf2.EjecutaSql(SQL2)

                If Datos1.Tables(0).Rows.Count > 0 Then
                    If Datos1.Tables(0).Rows(0).Item("Nombre_vend").ToString.Length > 0 Then
                        dllider.SelectedItem.Text = Datos1.Tables(0).Rows(0).Item("Nombre_vend")
                        dllider.Enabled = False
                        dlRun.Items.Add("False")
                        dlRun.Enabled = False
                    End If
                End If

                Dim conf3 As New Configuracion(Usuario, Clave, Bd, Servidor)
                Dim SQL3 As String
                SQL3 = " if exists (SELECT LTRIM(RTRIM(A.cod_vendedo)) Lider FROM FUNAMOR..VENDEDOR A WHERE  cod_vendedo <> vend_suplid and A.Cod_vendedo = '" + Usuario_Aut + "') select 1 ExisteLider else select 0 ExisteLider "
                Datos3 = conf3.EjecutaSql(SQL3)

                'If Datos3.Tables(0).Rows.Count > 0 Then
                '    If Datos3.Tables(0).Rows(0).Item("ExisteLider").ToString.Length > 0 Then
                Session.Add("UsuarioLider", Datos3.Tables(0).Rows(0).Item("ExisteLider"))
                'End If



            End If
            Dim dataClientControl As DataClient = CType(FindControl("CorrectSalesDataClient1"), DataClient)

            AddHandler DataSendEvent, AddressOf CorrectSalesDataClient1.OnDataReceived
            AddHandler DataContractSendEvent, AddressOf CorrectSalesDataClient1.OnContractDataReceived
            AddHandler ProductSendEvent, AddressOf CorrectSalesDataClient1.OnProductDataReceived
            AddHandler CorrectSalesDataClient1.AlertGenerated, AddressOf HandleAlertGenerated
            AddHandler CorrectSalesDataClient1.ProductTextChanged, AddressOf CorrectContract_ProductTextChanged
            AddHandler CorrectSalesDataClient1.enableButton, AddressOf CorrectContract_ProductTextChanged
            AddHandler CorrectSalesDataClient1.ProductButtonClick, AddressOf ProductButtonClick
            AddHandler CorrectSalesDataClient1.PanelEditarVentaVisible, AddressOf HandlePanelEditarVentaVisible
            AddHandler CorrectSalesDataClient1.PanelConfirmacionVisible, AddressOf HandlePanelConfirmacionVisible

        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try

    End Sub

    Protected Sub HandlePanelEditarVentaVisible(ByVal sender As Object, ByVal e As EventArgs)

        PanelEditarVenta.Visible = False
    End Sub
    Protected Sub HandlePanelConfirmacionVisible(ByVal sender As Object, ByVal e As EventArgs)

        PanelConfirmacion.Visible = True
    End Sub
    Protected Sub btnSalir_Click(sender As Object, e As ImageClickEventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("principal.aspx")
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub

    Protected Sub btnArreglarVenta_click(sender As Object, e As EventArgs) Handles btnArreglarVenta.Click
        Try
            PanelEditarVenta.Visible = True
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub










    'Protected Sub btnGuardarCamb_click(sender As Object, e As EventArgs) Handles btnGuardarCamb.Click
    '    PanelConfirmacion.Visible = True
    'End Sub

    '  Protected Sub BtnGuardarNo_click(sender As Object, e As EventArgs) Handles BtnGuardarNo.Click
    '    PanelConfirmacion.Visible = False
    'End Sub


    'Aqui va toda la Magia de Arreglar una venta
    'Protected Sub BtnGuardarSi_click(sender As Object, e As EventArgs) Handles BtnGuardarSi.Click
    '    PanelConfirmacion.Visible = False


    'End Sub

    Protected Sub btnBusVendEdt_click(sender As Object, e As EventArgs) Handles btnBusVendEdt.Click
        Try
            PanelVendedoresEditar.Visible = True
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try

    End Sub
    Protected Sub btnCerarPVend_click(sender As Object, e As EventArgs) Handles btnCerarPVend.Click
        Try
            PanelVendedoresEditar.Visible = False
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try

    End Sub

    Protected Sub btnRegresar_Click(sender As Object, e As ImageClickEventArgs) Handles btnRegresar.Click
        Try
            Panel1.Visible = True
            PanelImpresion.Visible = False
            ifRepote.Dispose()
            ifRepote.Src = ""
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub

    'Protected Sub btnAddClient_Click() Handles btnAddClient.Click

    '    PanelAddCliente.Visible = True

    'End Sub

    'Protected Sub btnCanAdd_Click(sender As Object, e As EventArgs) Handles btnCanAdd.Click

    '    PanelAddCliente.Visible = False

    'End Sub


    Private Sub btnProcesar_Click(sender As Object, e As EventArgs) Handles btnProcesar.Click
        Try
            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String

            'Sql = "EXEC ExportImag '" + Session("Cobrador").ToString.TrimEnd + "' "
            'conf.EjecutaSql(Sql)
            Dim user, collector As String
            user = Usuario_Aut
            collector = Session("Cobrador").ToString.TrimEnd
            Dim parameters() = {
                New SqlParameter("@RCODVEND", collector),
                New SqlParameter("@Usuario", user)}
            GuardarImagen()
            Using context As New AeVentasDbContext()
                Sql = "EXEC ENVIARECIBOS2 @RCODVEND=@RCODVEND, @Usuario=@Usuario"
                Dim result As String = context.Database.SqlQuery(Of String)(Sql, parameters).FirstOrDefault()
                RabbitMQHelper.PublishToRabbitMQ(messageType:="01", collectorId:=collector, userId:=Usuario_Aut, queueName:="start_sending_payment_receipts")

                Alert(result, "info")
                'If result.ToLower().Contains("primary key") Then
                '    Dim pattern As String = "\(([^,]+),"

                '    ' Use Regex to extract the key value
                '    Dim match As Match = Regex.Match(result, pattern)

                '    If match.Success Then
                '        ' Extract the first capturing group, which contains the key value
                '        Dim keyValue As String = match.Groups(1).Value
                '        Session("DocumentNumberToFIx") = keyValue
                '        PnlFixCorrelative.Visible = True
                '    Else
                '        Alert("No match found.", "danger")
                '    End If

                'Else

                '    btnBuscar_Click(sender, e)
                '    PanelImpresion.Visible = False
                '    Panel1.Visible = True
                'End If

            End Using

        Catch ex As EntityCommandExecutionException
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))


        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try

    End Sub
    Protected Sub LinkButtonFixCorrelative_Click(sender As Object, e As EventArgs)
        Dim documentNumberToFIx = Session("DocumentNumberToFIx")
        Alert(FixHelper.FixDbCorrelative(documentNumberToFIx), "info")
        PnlFixCorrelative.Visible = False

        'btnBuscar_Click(sender, e)
        'PanelImpresion.Visible = False
        'Panel1.Visible = True

    End Sub

    Protected Sub LinkButtonCancelFixCorrelative_Click(sender As Object, e As EventArgs)
        PnlFixCorrelative.Visible = False
    End Sub

    Protected Sub txtBuscarVendedorV_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String

            Sql = "Select cod_vendedo Vendedor, Nombre_vend Nombre from FUNAMOR..VENDEDOR WHERE COD_VENDEDO + nombre_vend like '%" + txtBuscarVended.Text.TrimEnd + "%'"
            Datos = conf.EjecutaSql(Sql)

            gvvendEditVent.DataSource = Datos.Tables(0)
            gvvendEditVent.DataBind()
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try

    End Sub



    Protected Sub dlCiudadCliente_TextChanged(sender As Object, e As EventArgs)
        Try

            Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String

            Sql = "Select desdepto Depto, ltrim(rtrim(desmuni))  + '-' +codzona CiudadEmpresa, codmuni, coddepto from AEVentas..DEPTOZONA"


        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub


    'Sub Validar()
    '    If Session("Usuario") = "AMPARO" Then
    '        btnArreglarVenta.Enabled = True
    '    End If

    'End Sub



    Protected Sub txtVendEV_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String

            Sql = "Select cod_vendedo Vendedor, Nombre_vend Nombre from FUNAMOR..VENDEDOR WHERE COD_VENDEDO + nombre_vend like '%" + txtCodVendEV.Text.TrimEnd + "%'"
            Datos = conf.EjecutaSql(Sql)

            If txtCodVendEV.Text.Length <> 4 Then
                PanelVendedoresEditar.Visible = True
                gvvendEditVent.DataSource = Datos.Tables(0)
                gvvendEditVent.DataBind()
            Else
                If Datos.Tables(0).Rows.Count > 1 Then
                    txtCodVendEV.Text = Datos.Tables(0).Rows(0).Item("Vendedor")
                    txtnombreVendArr.InnerText = Datos.Tables(0).Rows(0).Item("Nombre")
                    txtBuscarVended.Text = ""
                Else
                    PanelVendedoresEditar.Visible = True
                End If
            End If
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try

    End Sub


    Private Sub GuardarImagen()
        Try
            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql, FilePath As String

            Sql = " Select A.FIRMA, B.identidad "
            Sql += " From CONTRATON A "
            Sql += " INNER Join CLIENTESN B ON A.Codigo_Clie = B.Codigo_Clie And B.Cl_VENDEDOR = A.cont_vended "
            Sql += " INNER Join RECIBOS C ON A.CODIGO_CLIE = C.CODIGO_CLIE And A.cont_vended = C.RCODVEND "
            Sql += " WHERE A.cont_vended = '" + Session("Cobrador") + "' AND C.LIQUIDA <> 'N' AND LIQUIDA2 = 'N' AND A.FIRMA IS NOT NULL "
            Datos2 = conf.EjecutaSql(Sql)

            For I As Integer = 0 To Datos2.Tables(0).Rows.Count - 1

                Dim imageData As Byte() = DirectCast(Datos2.Tables(0).Rows(I).Item("FIRMA"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)
                        FilePath = "C:\inetpub\wwwroot\firmas\" + Datos2.Tables(0).Rows(I).Item("identidad").ToString.TrimEnd + ".jpg"
                        Image.FromStream(ms, True).Save(FilePath, Imaging.ImageFormat.Jpeg)
                    End Using
                End If

            Next
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Try
            Dim conf, Conf1 As New Configuracion(Usuario, Clave, "AEVENTAS", Servidor)
            Dim Sql, Sql1 As String

            If txtFecha.Text.Length > 0 Then
                Session.Add("F1", txtFecha.Text)
            Else
                Session.Add("F1", DateTime.Now.ToString("yyyy-MM-dd"))
            End If

            If dlMostrar.SelectedIndex = 0 And dllider.SelectedValue = "TODOS" Then
                gvMonitor.Columns(1).Visible = False
                gvMonitor.Columns(2).Visible = False
                lblVerdes.Visible = True
                Sql = " DECLARE @FECHA DATE "
                Sql += " SET @FECHA = '" + Session("F1") + "'"
                Sql += " Select  B.VEND_LIDER [Codigo], c.Nombre_vend [Lider], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUM(A.Ventas) Ventas, SUM(A.Verdes) Verdes, D.vzon_nombre [Zona], C.Tel_vendedo Telefono "
                Sql += " FROM ( "
                Sql += " Select DISTINCT A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Ventas, 0 Verdes FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.RCODVEND = B.CONT_VENDED WHERE CONVERT(DATE,A.Fecha_recib) = @FECHA AND A.MARCA = 'N' "
                Sql += " UNION ALL "
                Sql += " Select A.VENDEDOR, 0, SUBSTRING(A.HORA,1,2) +':'+ SUBSTRING(A.HORA,3,2) +':00', 0 Recibos, 1 Visitados, 0 Ventas, 0 Verdes FROM NOVENTA A WHERE CONVERT(DATE,A.FECHA) = @FECHA "
                Sql += " UNION ALL "
                Sql += " Select A.CL_VENDEDOR, 0, CASE WHEN LEN(CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha))) = 1 THEN '0' + CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) ELSE CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) END + ':' + CASE WHEN LEN(CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha))) = 1 THEN '0' + CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) ELSE CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) END +':'+ CASE WHEN LEN(CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha))) = 1 THEN '0' + CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)) ELSE CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)) END, 0 Recibos, 1 Visitados, 0 Ventas, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Verdes FROM CLIENTESN A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.CL_VENDEDOR = B.CONT_VENDED WHERE CONVERT(DATE,A.cl_fecha) = @FECHA AND A.tempo = 'N'"
                Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
                Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
                Sql += " LEFT JOIN FUNAMOR..VZONA D ON C.vzon_codigo = D.vzon_codigo "
                Sql += " Group BY B.VEND_LIDER, C.Nombre_vend, D.vzon_nombre, C.Tel_vendedo "


                If dlRun.SelectedIndex = 0 Then

                    Sql1 = "  DECLARE @FECHA DATE "
                    Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                    Sql1 += " exec [SP_MONITOR_VENTAS_l] @Fecha "
                ElseIf dlRun.SelectedIndex = 1 Then

                    Sql1 = "  DECLARE @FECHA DATE "
                    Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                    Sql1 += " exec SP_MONITOR_VENTAS @Fecha "
                End If

            End If

            'lblMsg.Text = Sql1
            'Exit Sub


            If dlMostrar.SelectedIndex = 0 And dllider.SelectedValue <> "TODOS" Then
                gvMonitor.Columns(1).Visible = False
                gvMonitor.Columns(2).Visible = False
                lblVerdes.Visible = True

                'If Usuario_Aut = Session("UsuarioLider") Then
                '    gvMonitor.Columns(8).Visible = False
                'End If
                Sql = " DECLARE @FECHA DATE "
                Sql += " SET @FECHA = '" + Session("F1") + "'"
                Sql += " Select  A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUM(A.Ventas) Ventas, SUM(A.Verdes) Verdes, SUBSTRING(C.Nombre_vend,1,16) [Lider], D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
                Sql += " FROM ( "
                Sql += " Select DISTINCT A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Ventas, 0 Verdes FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.RCODVEND = B.CONT_VENDED WHERE CONVERT(DATE,A.Fecha_recib) = @FECHA AND A.MARCA = 'N' "
                Sql += " UNION ALL "
                Sql += " Select A.VENDEDOR, 0, SUBSTRING(A.HORA,1,2) +':'+ SUBSTRING(A.HORA,3,2) +':00', 0 Recibos, 1 Visitados, 0 Ventas, 0 Verdes FROM NOVENTA A WHERE CONVERT(DATE,A.FECHA) = @FECHA "
                Sql += " UNION ALL "
                Sql += " Select A.CL_VENDEDOR, 0, CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)), 0 Recibos, 1 Visitados, 0 Ventas, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Verdes FROM CLIENTESN A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.CL_VENDEDOR = B.CONT_VENDED WHERE CONVERT(DATE,A.cl_fecha) = @FECHA AND (A.tempo = 'N' or a.tempo='A')"
                Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
                Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
                Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
                Sql += " WHERE C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
                Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
                Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
                Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, D.vzon_nombre, B.Tel_vendedo "




                If dlRun.SelectedIndex = 0 Then

                    Sql1 = "  DECLARE @FECHA DATE "
                    Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                    Sql1 += " exec [SP_MONITOR_VENTAS_l] @Fecha "
                ElseIf dlRun.SelectedIndex = 1 Then

                    'Sql1 = "  DECLARE @FECHA DATE "
                    'Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                    'Sql1 += " exec SP_MONITOR_VENTAS @Fecha "


                    Sql1 = " DECLARE @FECHA DATE "
                    Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                    Sql1 += " SELECT DISTINCT A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + '
                MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono, A.LONGITUD, A.LATITUD,cf.ClientesSistema   "
                    Sql1 += " FROM( "
                    Sql1 += " SELECT A.Num_doc, A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4, A.LONGITUD, A.LATITUD FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.RCODVEND = B.cont_vended WHERE CONVERT(DATE,A.Fecha_recib) = @FECHA "
                    Sql1 += " UNION ALL "
                    Sql1 += " SELECT 'NO VENTA', 0, SUBSTRING(A.HORA,1,2) +':'+ SUBSTRING(A.HORA,3,2) +':00', '', A.VENDEDOR, '', CONVERT(DATE,A.FECHA), A.liquida, 'N', 0, 0, 0, NULL, 0, 0, NULL, 0, 0, NULL, 0, 0, NULL, 0, 0, A.LONGITUD, A.LATITUD FROM NOVENTA A WHERE CONVERT(DATE,A.FECHA) = @FECHA "
                    Sql1 += " UNION ALL "
                    Sql1 += " SELECT 'PROSPECTO', 0, CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)), '', A.CL_VENDEDOR, A.Nombre_clie, CONVERT(DATE,A.cl_fecha), A.liquida, 'N', B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4, A.LATITUD, A.LONGITUD FROM CLIENTESN A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.CL_VENDEDOR = B.cont_vended WHERE CONVERT(DATE,A.cl_fecha) = @FECHA AND A.tempo = 'N' "
                    Sql1 += " ) A "
                    Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
                    Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "
                    Sql1 += " left join (select  cli.identidad,cli.Nombre_clie, "
                    Sql1 += " 	 stuff((Select top 6 ', ' + Codigo_clie "
                    Sql1 += "  from FUNAMOR..CLIENTES cl "
                    Sql1 += "  Where  CL.identidad  =  CLI.IDENTIDAD and cl.Nombre_clie = cli.Nombre_clie "
                    Sql1 += " For xml PATH('')),1, 2,'' "
                    Sql1 += " 	  )	as ClientesSistema "
                    Sql1 += "  From FUNAMOR..CLIENTES cli "
                    Sql1 += " Group By identidad, nombre_clie "
                    Sql1 += " )Cf on replace(cf.identidad,'-','') = c.identidad COLLATE Modern_Spanish_CI_AS or cf.Nombre_clie = replace(c.Nombre_clie,'.','') COLLATE Modern_Spanish_CI_AS "


                End If
            End If
            If dlMostrar.SelectedIndex = 1 Then
                gvMonitor.Columns(1).Visible = False
                gvMonitor.Columns(2).Visible = False

                lblVerdes.Visible = False
                'If Usuario_Aut = Session("UsuarioLider") Then
                '    gvMonitor.Columns(8).Visible = False
                ''End If

                Sql = " DECLARE @FECHA DATE "
                Sql += " SET @FECHA = '" + Session("F1") + "'"
                Sql += " SELECT A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUM(A.Ventas) Ventas, SUBSTRING(C.Nombre_vend,1,16) [Lider], SUBSTRING(A.liquida,1,4) +'/'+ SUBSTRING(A.liquida,5,2) +'/'+ SUBSTRING(A.liquida,7,2) +' '+ SUBSTRING(A.liquida,9,2) +':'+ SUBSTRING(A.liquida,11,2) Liquida, CASE WHEN A.liquida2 != 'N' THEN SUBSTRING(A.liquida2,1,4) +'/'+ SUBSTRING(A.liquida2,5,2) +'/'+ SUBSTRING(A.liquida2,7,2) +' '+ SUBSTRING(A.liquida2,9,2) +':'+ SUBSTRING(A.liquida2,11,2) ELSE 'N' END Procesado, D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
                Sql += " FROM ( "
                Sql += " Select DISTINCT A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Ventas, A.liquida, A.liquida2 FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.RCODVEND = B.CONT_VENDED WHERE CONVERT(DATE,SUBSTRING(A.liquida,1,8)) = @FECHA AND A.MARCA = 'N' "
                Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
                Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
                Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
                Sql += " WHERE C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
                Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
                Sql += " AND A.liquida != 'N' "
                Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
                Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, A.Liquida, A.liquida2, D.vzon_nombre, B.Tel_vendedo "



                If dlRun.SelectedIndex = 0 Then ''Sin Consulta Cliente Sistema


                    Sql1 = "  DECLARE @FECHA DATE "
                    Sql1 += " SET @FECHA = '" + Session("F1") + "'"
                    Sql1 += " EXEC [SP_MONITOR_VENTAS_X_LIQUID] @Fecha "

                ElseIf dlRun.SelectedIndex = 1 Then


                    Sql1 = "  DECLARE @FECHA DATE "
                    Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                    Sql1 += " EXEC SP_MONITOR_VENTAS_X_LIQUID_Sistema  @Fecha "

                End If
                'lblMsg.Text = Sql1
                'Exit Sub
            End If

            If dlMostrar.SelectedIndex = 2 Then
                gvMonitor.Columns(1).Visible = False
                gvMonitor.Columns(2).Visible = False
                lblVerdes.Visible = False
                If Usuario_Aut.ToUpper = "MANAGER" Then
                    gvMonitor.Columns(1).Visible = True
                End If


                Sql = " DECLARE @FECHA DATE "
                Sql += " SET @FECHA = '" + Session("F1") + "'"
                Sql += " SELECT A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUM(A.Ventas) Ventas, SUBSTRING(C.Nombre_vend,1,16) [Lider], A.Liquida, A.Liquida2 Procesado, D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
                Sql += " FROM ( "
                Sql += " Select DISTINCT A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Ventas, A.liquida, A.liquida2 FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.RCODVEND = B.CONT_VENDED WHERE A.MARCA = 'N' AND A.liquida = 'N' AND CONVERT(DATE,A.Fecha_recib) <= @FECHA "
                Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
                Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
                Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
                Sql += " WHERE C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
                Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
                Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
                Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, A.Liquida, A.liquida2, D.vzon_nombre, B.Tel_vendedo "


                If dlRun.SelectedIndex = 0 Then

                    Sql1 = "  DECLARE @FECHA DATE "
                    Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                    Sql1 += " exec SP_MONITOR_VENTAS_SIN_LIQUID @Fecha "

                ElseIf dlRun.SelectedIndex = 1 Then


                    Sql1 = "  DECLARE @FECHA DATE "
                    Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                    Sql1 += " exec [SP_MONITOR_VENTAS_SIN_LIQUID_SISTEMA] @Fecha "

                End If


            End If

            If dlMostrar.SelectedIndex = 3 Then
                gvMonitor.Columns(1).Visible = False
                gvMonitor.Columns(2).Visible = False
                lblVerdes.Visible = False

                Sql = " DECLARE @FECHA DATE "
                Sql += " SET @FECHA = '" + Session("F1") + "'"
                Sql += " SELECT A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUBSTRING(C.Nombre_vend,1,16) [Lider], A.Liquida, A.Liquida2 Procesado, D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
                Sql += " FROM ( "
                Sql += " SELECT A.cveage RCODVEND, 0 Por_lempira, '00:00:00' rhora, 0 Recibos, 0 Visitados, 'N' liquida, 'N' liquida2 FROM MVSAGEN A WHERE A.cveage COLLATE Modern_Spanish_CI_AS NOT IN (SELECT A.RCODVEND FROM RECIBOS A WHERE CONVERT(DATE,A.Fecha_recib) = @FECHA GROUP BY A.RCODVEND UNION ALL SELECT A.VENDEDOR FROM NOVENTA A WHERE CONVERT(DATE,A.FECHA) = @FECHA GROUP BY A.VENDEDOR UNION ALL SELECT A.CL_VENDEDOR FROM CLIENTESN A WHERE CONVERT(DATE,A.cl_fecha) = @FECHA AND A.tempo = 'N' GROUP BY A.CL_VENDEDOR ) "
                Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS AND B.VEND_STATUS = 'A' "
                Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
                Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
                Sql += " WHERE B.VEND_STATUS = 'A'"
                Sql += " AND C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
                Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
                Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
                Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, A.Liquida, A.liquida2, D.vzon_nombre, B.Tel_vendedo "


                If dlRun.SelectedIndex = 1 Then

                    Sql1 = "  DECLARE @FECHA DATE "
                    Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                    Sql1 += " SELECT DISTINCT TOP 1 A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono  "
                    Sql1 += " ,cf.ClientesSistema FROM( "
                    Sql1 += " SELECT A.Num_doc, A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4 FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.RCODVEND = B.cont_vended WHERE A.Liquida = 'N' AND CONVERT(DATE,A.Fecha_recib) = @FECHA "
                    Sql1 += " ) A "
                    Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
                    Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "
                    Sql1 += " left join (select  cli.identidad,cli.Nombre_clie, "
                    Sql1 += " 	 stuff((Select top 6 ', ' + Codigo_clie "
                    Sql1 += "  from FUNAMOR..CLIENTES cl "
                    Sql1 += "  Where  CL.identidad  =  CLI.IDENTIDAD and cl.Nombre_clie = cli.Nombre_clie "
                    Sql1 += " For xml PATH('')),1, 2,'' "
                    Sql1 += " 	  )	as ClientesSistema "
                    Sql1 += "  From FUNAMOR..CLIENTES cli "
                    Sql1 += " Group By identidad, nombre_clie "
                    Sql1 += " )Cf on replace(cf.identidad,'-','') = c.identidad COLLATE Modern_Spanish_CI_AS or replace(cf.Nombre_clie,'.','') = replace(c.Nombre_clie,'.','') COLLATE Modern_Spanish_CI_AS "

                ElseIf dlRun.SelectedIndex = 0 Then
                    Sql1 = "  DECLARE @FECHA DATE "
                    Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                    Sql1 += " SELECT DISTINCT TOP 1 A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,60)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono  "
                    Sql1 += " ,cf.ClientesSistema FROM( "
                    Sql1 += " SELECT A.Num_doc, A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4 FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.RCODVEND = B.cont_vended WHERE A.Liquida = 'N' AND CONVERT(DATE,A.Fecha_recib) = @FECHA "
                    Sql1 += " ) A "
                    Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
                    Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "
                    Sql1 += " left join (select  cli.identidad,cli.Nombre_clie, "
                    Sql1 += " 	 stuff((Select top 6 ', ' + Codigo_clie "
                    Sql1 += "  from FUNAMOR..CLIENTES cl "
                    Sql1 += "  Where  CL.identidad  =  CLI.IDENTIDAD and cl.Nombre_clie = cli.Nombre_clie "
                    Sql1 += " For xml PATH('')),1, 2,'' "
                    Sql1 += " 	  )	as ClientesSistema "
                    Sql1 += "  From FUNAMOR..CLIENTES cli "
                    Sql1 += " Group By identidad, nombre_clie "
                    Sql1 += " )Cf on replace(cf.identidad,'-','') = c.identidad COLLATE Modern_Spanish_CI_AS or cf.Nombre_clie = replace(c.Nombre_clie,'.','') COLLATE Modern_Spanish_CI_AS "
                End If
            End If

            If dlMostrar.SelectedIndex = 4 Then
                gvMonitor.Columns(1).Visible = True
                gvMonitor.Columns(2).Visible = False
                lblVerdes.Visible = False

                Sql = " DECLARE @FECHA DATE "
                Sql += " SET @FECHA = '" + Session("F1") + "'"
                Sql += " SELECT A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUBSTRING(C.Nombre_vend,1,16) [Lider], SUBSTRING(A.liquida,1,4) +'/'+ SUBSTRING(A.liquida,5,2) +'/'+ SUBSTRING(A.liquida,7,2) +' '+ SUBSTRING(A.liquida,9,2) +':'+ SUBSTRING(A.liquida,11,2) Liquida, CASE WHEN A.liquida2 != 'N' THEN SUBSTRING(A.liquida2,1,4) +'/'+ SUBSTRING(A.liquida2,5,2) +'/'+ SUBSTRING(A.liquida2,7,2) +' '+ SUBSTRING(A.liquida2,9,2) +':'+ SUBSTRING(A.liquida2,11,2) ELSE 'N' END Procesado, D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
                Sql += " FROM ( "
                Sql += " Select A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, A.liquida, A.liquida2 FROM RECIBOS A WHERE A.MARCA = 'N' AND A.liquida <> 'N' AND A.liquida2 = 'N' AND CONVERT(DATE,A.Fecha_recib) <= @FECHA "
                Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
                Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
                Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
                Sql += " WHERE C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
                Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
                Sql += " AND A.liquida != 'N' "
                Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
                Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, A.Liquida, A.liquida2, D.vzon_nombre, B.Tel_vendedo "


                If dlRun.SelectedIndex = 1 Then


                    Sql1 = "  DECLARE @FECHA DATE "
                    Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                    Sql1 += " exec  [SP_MONITOR_VENTAS_SinProcesar]  @Fecha "


                ElseIf dlRun.SelectedIndex = 0 Then

                    Sql1 = " DECLARE @FECHA DATE "
                    Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                    Sql1 += " SELECT DISTINCT A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,60)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono "
                    Sql1 += " ,''clientesSistema FROM( "
                    Sql1 += " SELECT  distinct  case when B.SERVI1DES like '%lote%' or B.SERVI1DES like '%crem%' or B.SERVI1DES like '%placa%' "
                    Sql1 += " then 'P01'  +''+SUBSTRING(Num_doc,4,14) "
                    Sql1 += " when (B.SERVI1DES not like '%lote%' or B.SERVI1DES not like '%crem%' or B.SERVI1DES  not like  '%placa%'  ) AND SUBSTRING(Num_doc,1,3) = 'P01' 			 "
                    Sql1 += " then rtrim(ltrim(codzona))+''+SUBSTRING(Num_doc,4,14) "
                    Sql1 += " when (B.SERVI1DES not like '%lote%' or B.SERVI1DES not like '%crem%' or  B.SERVI1DES not like  '%placa%' ) AND SUBSTRING(Num_doc,1,3) <> 'P01' "
                    Sql1 += " THEN rtrim(ltrim(codzona))+''+SUBSTRING(Num_doc,4,14) ELSE A.Num_doc end Num_Doc, "
                    Sql1 += " A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4 "
                    Sql1 += " FROM RECIBOS A "
                    Sql1 += " LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie And A.RCODVEND = B.cont_vended "
                    Sql1 += " LEFT join CLIENTESN Cc ON CC.Codigo_clie = A.Codigo_clie And a.RCODVEND = cc.CL_VENDEDOR "
                    Sql1 += " inner JOIN DEPTOZONA DP ON DP.desdepto = CC.departa And rtrim(ltrim(dp.desmuni)) = rtrim(ltrim(Cc.municipio)) "
                    Sql1 += " WHERE A.Liquida <> 'N' AND A.Liquida2 = 'N' AND CONVERT(DATE,A.Fecha_recib) <= @FECHA "
                    Sql1 += " ) A "
                    Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
                    Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "
                    'Sql1 += " left join (select cli.identidad,cli.Nombre_clie, "
                    'Sql1 += " 	 stuff((Select top 6 ', ' + Codigo_clie "
                    'Sql1 += "  from FUNAMOR..CLIENTES cl "
                    'Sql1 += "  Where  CL.identidad  =  CLI.IDENTIDAD and cl.Nombre_clie = cli.Nombre_clie "
                    'Sql1 += " For xml PATH('')),1, 2,'' "
                    'Sql1 += " 	  )	as ClientesSistema "
                    'Sql1 += "  From FUNAMOR..CLIENTES cli "
                    'Sql1 += " Group By identidad, nombre_clie "
                    'Sql1 += " )Cf on replace(replace(cf.identidad,'-',''),'.','') = c.identidad COLLATE Modern_Spanish_CI_AS 	or replace(replace(replace(lTRIM(RTRIM(cf.Nombre_clie)),'.',''),'á','a'),'É','E') = REPLACE(c.Nombre_clie,'.','') COLLATE Modern_Spanish_CI_AS  "
                    'lblMsg.Text = Sql1
                    'Exit Sub
                End If

            End If

            If dlMostrar.SelectedIndex = 5 Then
                gvMonitor.Columns(2).Visible = True
                gvMonitor.Columns(1).Visible = False
                lblVerdes.Visible = False

                Sql = " DECLARE @FECHA DATE "
                Sql += " SET @FECHA = '" + Session("F1") + "'"
                Sql += " SELECT A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Visitados, SUBSTRING(C.Nombre_vend,1,16) [Lider], SUBSTRING(A.liquida,1,4) +'/'+ SUBSTRING(A.liquida,5,2) +'/'+ SUBSTRING(A.liquida,7,2) +' '+ SUBSTRING(A.liquida,9,2) +':'+ SUBSTRING(A.liquida,11,2) Liquida, CASE WHEN A.liquida2 != 'N' THEN SUBSTRING(A.liquida2,1,4) +'/'+ SUBSTRING(A.liquida2,5,2) +'/'+ SUBSTRING(A.liquida2,7,2) +' '+ SUBSTRING(A.liquida2,9,2) +':'+ SUBSTRING(A.liquida2,11,2) ELSE 'N' END Procesado, D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
                Sql += " FROM ( "
                Sql += " Select A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, A.liquida, A.liquida2 FROM RECIBOS A WHERE CONVERT(DATE,SUBSTRING(A.liquida2,1,8)) = @FECHA AND A.MARCA = 'N' "
                Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
                Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
                Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
                Sql += " WHERE C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
                Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
                Sql += " AND A.liquida2 != 'N' "
                Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
                Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, A.Liquida, A.liquida2, D.vzon_nombre, B.Tel_vendedo "

                Sql1 = " DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                Sql1 += " SELECT DISTINCT A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, A.liquida, A.liquida2, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,60)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono  "
                Sql1 += " ,'' ClientesSistema FROM( "
                Sql1 += " SELECT  distinct  case when B.SERVI1DES like '%lote%' or B.SERVI1DES like '%crem%' or B.SERVI1DES like '%placa%' "
                Sql1 += " then 'P01'  +''+SUBSTRING(Num_doc,4,14) "
                Sql1 += " when (B.SERVI1DES not like '%lote%' or B.SERVI1DES not like '%crem%' or B.SERVI1DES not like '%placa%') AND SUBSTRING(Num_doc,1,3) = 'P01' 			 "
                Sql1 += " then rtrim(ltrim(codzona))+''+SUBSTRING(Num_doc,4,14) "
                Sql1 += " when (B.SERVI1DES not like '%lote%' or B.SERVI1DES not like '%crem%' and B.SERVI1DES not like '%placa%') AND SUBSTRING(Num_doc,1,3) <> 'P01' "
                Sql1 += " THEN rtrim(ltrim(codzona))+''+SUBSTRING(Num_doc,4,14) ELSE A.Num_doc end Num_Doc, "
                Sql1 += " A.Por_lempira, A.rhora, A.Codigo_clie, A.RCODVEND, CASE WHEN A.MARCA = 'X' THEN 'ANULADO' ELSE '' END CONCEPTO, CONVERT(DATE,A.Fecha_recib) Fecha, A.liquida, A.liquida2, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4 "
                Sql1 += " FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.RCODVEND = B.cont_vended "
                Sql1 += " LEFT join CLIENTESN Cc ON CC.Codigo_clie = A.Codigo_clie And a.RCODVEND = cc.CL_VENDEDOR "
                Sql1 += " inner JOIN DEPTOZONA DP ON DP.desdepto = CC.departa And  rtrim(ltrim(dp.desmuni)) = rtrim(ltrim(Cc.municipio)) "
                Sql1 += " WHERE CONVERT(DATE,SUBSTRING(A.liquida2,1,8)) = @FECHA "
                Sql1 += " ) A "
                Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
                Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "
                'Sql1 += " left join (select cli.identidad,cli.Nombre_clie, "
                'Sql1 += " 	 stuff((Select top 6 ', ' + Codigo_clie "
                'Sql1 += "  from FUNAMOR..CLIENTES cl "
                'Sql1 += "  Where  CL.identidad  =  CLI.IDENTIDAD and cl.Nombre_clie = cli.Nombre_clie "
                'Sql1 += " For xml PATH('')),1, 2,'' "
                'Sql1 += " 	  )	as ClientesSistema "
                'Sql1 += "  From FUNAMOR..CLIENTES cli "
                'Sql1 += " Group By identidad, nombre_clie "
                'Sql1 += " )Cf on replace(replace(cf.identidad,'-',''),'.','') = c.identidad COLLATE Modern_Spanish_CI_AS or REPLACE(REPLACE(replace(replace(replace(replace(lTRIM(RTRIM(cf.Nombre_clie)),'.',''),'á','a'),'É','E'),'Í','I'),'Ó','O'),'Ú','U')  = REPLACE(REPLACE(replace(replace(replace(replace(lTRIM(RTRIM(c.Nombre_clie)),'.',''),'á','a'),'É','E'),'Í','I'),'Ó','O'),'Ú','U') COLLATE Modern_Spanish_CI_AS "
                Sql1 += " WHERE A.liquida2 != 'N' "
            End If

            'lblMsg.Text = Sql1
            'Exit Sub

            If dlMostrar.SelectedValue = "VERDES" Then
                gvMonitor.Columns(1).Visible = False
                gvMonitor.Columns(2).Visible = False

                Sql = " DECLARE @FECHA DATE "
                'Sql += " SET @FECHA = '" + Session("F1") + "'"
                Sql += " SELECT A.RCODVEND [Codigo], B.Nombre_vend [Vendedor], SUM(A.Por_lempira) [Cobrado], MIN(A.rhora) [Primer Visita], MAX(A.rhora) [Ultima Visita], SUM(A.Recibos) Recibos, SUM(A.Visitados) Verdes, SUM(A.Ventas) Ventas, SUBSTRING(C.Nombre_vend,1,16) [Lider], D.vzon_nombre [Zona], B.Tel_vendedo Telefono "
                Sql += " FROM ( "
                'Sql += " Select DISTINCT A.RCODVEND, A.Por_lempira, A.rhora, 1 Recibos, 1 Visitados, (B.CONT_CANTI + B.CONT_CANT2 + B.CONT_CANT3 + B.CONT_CANT4) Ventas, A.liquida, A.liquida2 FROM RECIBOS A LEFT JOIN CONTRATON B ON A.Codigo_Clie = B.Codigo_Clie AND A.RCODVEND = B.CONT_VENDED WHERE A.MARCA = 'N' AND A.liquida = 'N' AND CONVERT(DATE,A.Fecha_recib) <= @FECHA "
                Sql += " Select A.CL_VENDEDOR RCODVEND, 0 Por_lempira, CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)) rhora, 0 Recibos, 1 Visitados, 0 Ventas FROM CLIENTESN A WHERE A.tempo = 'N'"
                Sql += " ) A LEFT JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
                Sql += " LEFT JOIN FUNAMOR..VENDEDOR C ON B.VEND_LIDER = C.Cod_vendedo "
                Sql += " LEFT JOIN FUNAMOR..VZONA D ON B.vzon_codigo = D.vzon_codigo "
                Sql += " WHERE C.Nombre_vend  LIKE '%" + dllider.SelectedValue + "%' "
                Sql += " AND A.RCODVEND + B.Nombre_vend COLLATE Modern_Spanish_CI_AS LIKE '%" + txtCobrador.Value + "%' "
                Sql += " AND D.vzon_nombre LIKE '%" + dlzona.SelectedValue + "%' "
                Sql += " Group BY A.RCODVEND, B.Nombre_vend, C.Nombre_vend, D.vzon_nombre, B.Tel_vendedo "

                Sql1 = " DECLARE @FECHA DATE "
                Sql1 += " SET @FECHA = '" + Session("F1") + "' "
                Sql1 += " SELECT DISTINCT A.Num_doc [Codigo], CONVERT(VARCHAR(50), CAST(A.Por_lempira AS MONEY ),1) [Cobrado], A.rhora [Hora], A.Codigo_clie [Codigo Cliente], CASE WHEN A.Num_doc = 'PROSPECTO' THEN '' ELSE A.CONCEPTO END CONCEPTO, A.RCODVEND, SUBSTRING(CASE WHEN C.Nombre_clie IS NULL THEN A.CONCEPTO ELSE C.Nombre_clie END ,1,30) Nombre_clie, CONVERT(VARCHAR,A.Fecha) Fecha, UPPER(SUBSTRING(C.Dir_cliente + ' '+ ISNULL(C.Dir2_client,'') COLLATE Modern_Spanish_CI_AS,1,40)) Dir_cliente, (SELECT TOP 1 'ANULADO POR: ' + Z.USUARIO + 'MOTIVO: ' + Z.MOTIVO FROM AEVentas..LOG_NULOS Z WHERE Z.NUM_DOC = A.Num_doc COLLATE Modern_Spanish_CI_AS ORDER BY LEN(Z.MOTIVO) DESC) Motivo, A.CONT_NUMCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALCUO AS MONEY ),1) CONT_VALCUO, CONVERT(VARCHAR(50), CAST(A.CONT_VALOR AS MONEY ),1) CONT_VALOR, CONVERT(VARCHAR,A.CONT_CANTI) + ' ' + RTRIM(A.SERVI1DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL1 AS MONEY ),1) SERVI1DES, CONVERT(VARCHAR,A.CONT_CANT2) + ' ' + RTRIM(A.SERVI2DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL2 AS MONEY ),1) SERVI2DES, CONVERT(VARCHAR,A.CONT_CANT3) + ' ' + RTRIM(A.SERVI3DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL3 AS MONEY ),1) SERVI3DES, CONVERT(VARCHAR,A.CONT_CANT4) + ' ' + RTRIM(A.SERVI4DES) + ' - ' + CONVERT(VARCHAR(50), CAST(A.CONT_SVAL4 AS MONEY ),1) SERVI4DES, C.identidad, LTRIM(RTRIM(C.CL_CELULAR)) + ISNULL(RTRIM(' ' + C.TELEF_CLIEN),'') Telefono "
                Sql1 += " ,'-' ClientesSistema FROM( "
                Sql1 += " SELECT 'PROSPECTO' Num_doc, 0 Por_lempira, CONVERT(VARCHAR,DATEPART(HOUR,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(MINUTE,A.cl_fecha)) +':'+ CONVERT(VARCHAR,DATEPART(SECOND,A.cl_fecha)) rhora, A.Codigo_clie, A.CL_VENDEDOR RCODVEND, '' Concepto, A.Nombre_clie, CONVERT(DATE,A.cl_fecha) Fecha, B.CONT_NUMCUO, B.CONT_VALCUO, B.CONT_VALOR, B.SERVI1DES, B.CONT_CANTI, B.CONT_SVAL1, B.SERVI2DES, B.CONT_CANT2, B.CONT_SVAL2, B.SERVI3DES, B.CONT_CANT3, B.CONT_SVAL3, B.SERVI4DES, B.CONT_CANT4, B.CONT_SVAL4, A.LONGITUD, A.LATITUD FROM CLIENTESN A LEFT JOIN CONTRATON B ON A.Codigo_clie = B.Codigo_clie AND A.CL_VENDEDOR = B.cont_vended WHERE A.tempo = 'N'  "
                Sql1 += " ) A "
                Sql1 += " Left JOIN FUNAMOR..VENDEDOR B ON A.RCODVEND = B.Cod_vendedo COLLATE Modern_Spanish_CI_AS "
                Sql1 += " Left JOIN CLIENTESN C ON A.Codigo_clie = C.Codigo_clie AND A.RCODVEND = C.CL_VENDEDOR "

            End If

            'lblMsg.Text = Sql1
            'Exit Sub

            Datos = conf.EjecutaSql(Sql)

            If Usuario_Aut = "JULIOCAJA" Or Usuario_Aut = "MANAGER" Or Usuario_Aut = "MDERAS" Or Usuario_Aut = "YASMIN" Or Usuario_Aut = "CBONILLA" Or Usuario_Aut = "ABLANDON" Or Usuario_Aut = "IBLANDON" Or Usuario_Aut = "JULIO" And (Datos.Tables(0).Rows.Count = 1 And dlMostrar.SelectedIndex = 1) Then
                Datos1 = Conf1.EjecutaSql(Sql1)
                If Datos1 IsNot Nothing AndAlso Datos1.Tables.Count > 0 Then

                    Session.Add("GVDetalle", Datos1.Tables(0))

                    Session("GVDetalle").DefaultView.RowFilter = "RCODVEND ='" + txtCobrador.Value + "'"
                    Session("GVDetalle").DefaultView.Sort = "Fecha Asc, Hora Asc"

                    gvDetalle2.DataSource = Session("GVDetalle")
                    gvDetalle2.DataBind()

                    Datos = conf.EjecutaSql(Sql)
                    Datos.Tables(0).DefaultView.Sort = "Cobrado Desc"
                    Session.Add("GV", Datos.Tables(0))
                    gvMonitor2.DataSource = Session("GV")
                    gvMonitor2.DataBind()

                    gvMonitor2.Visible = True
                    gvDetalle2.Visible = True
                    gvMonitor.Visible = False
                Else
                    Throw New Exception("No se encontraron datos para la tabla")

                End If

            ElseIf dlMostrar.SelectedIndex = 2 And Datos.Tables(0).Rows.Count = 1 Then
                Datos1 = Conf1.EjecutaSql(Sql1)
                Session.Add("GVDetalle", Datos1.Tables(0))

                Session("GVDetalle").DefaultView.RowFilter = "RCODVEND ='" + txtCobrador.Value + "'"
                Session("GVDetalle").DefaultView.Sort = "Fecha Asc, Hora Asc"

                gvDetalle2.DataSource = Session("GVDetalle")
                gvDetalle2.DataBind()

                Datos = conf.EjecutaSql(Sql)
                Datos.Tables(0).DefaultView.Sort = "Cobrado Desc"
                Session.Add("GV", Datos.Tables(0))
                gvMonitor2.DataSource = Session("GV")
                gvMonitor2.DataBind()

                gvMonitor2.Visible = True
                gvDetalle2.Visible = True
                gvMonitor.Visible = False
                gvDetalle2.Columns(2).Visible = True
                gvDetalle2.Columns(1).Visible = False

            ElseIf dlMostrar.SelectedIndex = 0 And Datos.Tables(0).Rows.Count = 1 Then
                Datos1 = Conf1.EjecutaSql(Sql1)
                Session.Add("GVDetalle", Datos1.Tables(0))

                Session("GVDetalle").DefaultView.RowFilter = "RCODVEND ='" + txtCobrador.Value + "'"
                Session("GVDetalle").DefaultView.Sort = "Fecha Asc, Hora Asc"

                gvDetalle2.DataSource = Session("GVDetalle")
                gvDetalle2.DataBind()

                Datos = conf.EjecutaSql(Sql)
                Datos.Tables(0).DefaultView.Sort = "Cobrado Desc"
                Session.Add("GV", Datos.Tables(0))
                gvMonitor2.DataSource = Session("GV")
                gvMonitor2.DataBind()

                gvMonitor2.Visible = True
                gvDetalle2.Visible = True
                gvMonitor.Visible = False
                gvDetalle2.Columns(0).Visible = False
                gvDetalle2.Columns(1).Visible = True

            Else
                Try

                    Datos1 = Conf1.EjecutaSql(Sql1)

                    If Datos1 IsNot Nothing AndAlso Datos1.Tables.Count > 0 Then
                        Session.Add("GVDetalle", Datos1.Tables(0))
                    Else
                        ' Handle the case where no tables are returned
                        ' Display an error message or perform any other action
                        Throw New Exception("No se recibieron datos para los detalles")
                        Return
                    End If
                    Datos = conf.EjecutaSql(Sql)

                    If Datos IsNot Nothing AndAlso Datos.Tables.Count > 0 Then
                        Datos.Tables(0).DefaultView.Sort = "Cobrado Desc"
                        Session.Add("GV", Datos.Tables(0))
                        gvMonitor.DataSource = Session("GV")
                        gvMonitor.DataBind()
                    Else
                        Throw New Exception("No se recibieron datos")
                    End If
                    gvMonitor2.Visible = False
                    gvDetalle2.Visible = False
                    gvMonitor.Visible = True

                Catch ex As Exception
                    DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"), $"sql var: {Sql} {vbCrLf} sql1 var:{Sql1}")
                    Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")

                End Try
            End If

            lblHora.Text = "Actualizado: " + System.DateTime.Now.ToShortTimeString

        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub


    'Campo Codigo de cliente Modal de Editar Venta
    Protected Sub txtCodClienteapp_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String


            Sql = "Select distinct r.Codigo_clie Codigo,
         Replace(REPLACE(Replace(Replace(replace(RNOMBRECLI,'Á','A'),'É','E'),'Í','I'),'Ó','O'),'Ú','U') Nombre, 
		Case when r.liquida = 'N' THEN 'Sin Liquidacion'
		when r.liquida <> 'N' then 'Liquidada' end Estatus,SUBSTRING(Num_doc,1,3)Empresa , ISNULL(c.identidad,'') Identidad, 
		Replace(Replace(Replace(Replace(Replace(Replace(replace(concat(rtrim(ltrim(isnull(Dir_cliente,''))),rtrim(ltrim(isnull(Dir2_client,''))),RTRIM(ltrim(isnull(Dir3_client,'')))+RTRIM(ltrim(isnull(Dir4_client,'')))),'Ñ','N'),'Á','A'),'É','E'),'Í','I'),'Ó','O'),'Ú','U'),'°','')Direccion,
        r.Num_doc Documento, r.Fecha_recib Fecha, R.RCODVEND, r.Por_lempira Prima,
        isnull(co.CONT_NUMCUO, 0) NumCuotas,isnull(co.CONT_VALOR,0) Valor, 
		isnull(CONT_CANTI, 0)Cantidad,isnull(co.CONT_SERVI,'0')IdServicio,isnull(SERVI1DES,'')Servicio,
		isnull(CONT_VALCUO, 0) ValorCuota		
		From RECIBOS R
		Left Join CLIENTESN C ON C.Codigo_clie = R.Codigo_clie And R.RCODVEND = C.CL_VENDEDOR
        Left Join CONTRATON CO ON CO.Codigo_clie = R.Codigo_clie And CONT_NUMERO = R.Codigo_clie And CO.cont_vended = R.RCODVEND
        WHERE Liquida2 = 'N' and marca = 'N' AND r.codigo_clie = '" + txtCodClienteapp.Text + "'"
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try

    End Sub


    'Campo Buscar del Modal de Clientes
    Protected Sub txtBuscarCliente_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String

            Sql = " Select distinct r.Codigo_clie Codigo,r.RNOMBRECLI Nombre,R.RCODVEND CodVendedor,ven.nombre_vend,  
		Case when r.liquida = 'N' THEN 'Sin Liquidacion'
		when r.liquida <> 'N' then 'Liquidada' end Estatus,SUBSTRING(Num_doc,1,3)Empresa , ISNULL(c.identidad,'') Identidad, 
		Replace(Replace(Replace(Replace(Replace(Replace(replace(concat(rtrim(ltrim(isnull(Dir_cliente,''))),rtrim(ltrim(isnull(Dir2_client,''))),RTRIM(ltrim(isnull(Dir3_client,'')))+RTRIM(ltrim(isnull(Dir4_client,'')))),'Ñ','N'),'Á','A'),'É','E'),'Í','I'),'Ó','O'),'Ú','U'),'°','')Direccion,
        r.Num_doc Documento, r.Fecha_recib Fecha, r.Por_lempira Prima,
        isnull(co.CONT_NUMCUO, 0) NumCuotas,isnull(co.CONT_VALOR,0) Valor, 
		isnull(CONT_CANTI, 0)Cantidad,isnull(co.CONT_SERVI,'0')IdServicio,isnull(SERVI1DES,'')Servicio,
		isnull(CONT_VALCUO, 0) ValorCuota, ltrim(isnull(c.CL_CELULAR,''))Tel1, ltrim(isnull(c.Telef_clien,''))Tel2, co.SERVIEMPRE		
		From RECIBOS R
		Left Join CLIENTESN C ON C.Codigo_clie = R.Codigo_clie And R.RCODVEND = C.CL_VENDEDOR
        Left Join CONTRATON CO ON CO.Codigo_clie = R.Codigo_clie And CONT_NUMERO = R.Codigo_clie And co.cont_vended = R.RCODVEND
        inner join FUNAMOR..VENDEDOR ven on ven.Cod_vendedo = r.RCODVEND  collate SQL_Latin1_General_CP1_CI_AS
        WHERE Liquida2 = 'N' and marca = 'N' AND r.codigo_clie like '%" + txtBuscarCliente.Text + "%'"

            Datos = conf.EjecutaSql(Sql)

            gvClientesVE.DataSource = Datos.Tables(0)
            gvClientesVE.DataBind()
            'txtCodClienteapp_TextChanged()
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub


    Private Sub btnCerrarCliapp_Click(sender As Object, e As EventArgs) Handles btnCerrarCliapp.Click
        Try

            PanelClientesVE.Visible = False
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub

    Sub VentasApp()
        Try
            Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String

            Sql = "Select * from (
                Select '0' Codigo, '' Descripcion 
                union all
                Select 'N' Codigo, 'SinLiquidacion' Descripcion 
                union all
                SElect 'X' Codigo, 'Anular'
                union all
                Select CONCAT(DATEPART(year, GETDATE()) , CASE WHEN LEN(DATEPART(MONTH,GETDATE()))=1 THEN CONCAT('0', DATEPART(MONTH,GETDATE())) ELSE CONCAT('', DATEPART(MONTH,GETDATE()))  END , 
                CASE WHEN LEN(DATEPART(DAY,GETDATE()))=1 THEN CONCAT('0', DATEPART(DAY,GETDATE())) ELSE CONCAT('', DATEPART(DAY,GETDATE())) END  , CASE WHEN LEN(DATEPART(HOUR,GETDATE()))=1 THEN CONCAT('0', DATEPART(HOUR,GETDATE())) ELSE CONCAT('', DATEPART(HOUR,GETDATE()))  END , CASE WHEN LEN(DATEPART(MINUTE,GETDATE()))=1 THEN CONCAT('0', DATEPART(MINUTE,GETDATE())) ELSE CONCAT('', DATEPART(MINUTE,GETDATE())) END )  Codigo, 'Liquidar' Descripcion -- SinLiquidacion
                union all
                Select 'N' Codigo, 'DesAnular')A"

            Datos = conf.EjecutaSql(Sql)

            dlempresaArr.DataSource = Datos.Tables(0)
            dlempresaArr.DataValueField = "Codigo"
            dlempresaArr.DataTextField = "Descripcion"
            dlempresaArr.DataBind()
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try

    End Sub
    Private Sub btnBuscClienVE_Click(sender As Object, e As EventArgs) Handles btnBuscClienVE.Click
        Try

            PanelClientesVE.Visible = True
            txtBuscarCliente_TextChanged(sender, e)
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub



    Private Sub gvMonitor_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvMonitor.Sorting
        Try
            Dim Orden As String

            If Session("Orden") = "0" Then
                Orden = "Asc"
                Session.Add("Orden", "1")
            Else
                Orden = "Desc"
                Session.Add("Orden", "0")
            End If

            Session("GV").DefaultView.Sort = e.SortExpression.ToString + " " + Orden
            gvMonitor.DataSource = Session("GV")
            gvMonitor.DataBind()
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try

    End Sub

    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub

    Private Sub gvMonitor_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvMonitor.RowDataBound
        Try
            Try
                If ((e.Row.RowType = DataControlRowType.DataRow) And (e.Row.RowType <> DataControlRowType.EmptyDataRow)) Then
                    Dim Fila As System.Data.DataRowView = e.Row.DataItem
                    Try
                        Total += Convert.ToDecimal(Fila.Item(2))
                    Catch ex As Exception
                        Dim itemValue As Object = Fila.Item(2)
                        Dim itemDescription As String = If(IsDBNull(itemValue), "DBNull", If(itemValue Is Nothing, "Nothing", itemValue.ToString()))

                        Throw New Exception($"Error converting 'Fila.Item(2)' *{itemDescription}* to Decimal: " & ex.Message)
                    End Try

                    Try
                        Visitados += Convert.ToDecimal(Fila.Item(6))
                    Catch ex As Exception
                        Dim itemValue As Object = Fila.Item(6)
                        Dim itemDescription As String = If(IsDBNull(itemValue), "DBNull", If(itemValue Is Nothing, "Nothing", itemValue.ToString()))

                        Throw New Exception($"Error converting 'Fila.Item(6)' *{itemDescription}* to Decimal: " & ex.Message)
                    End Try

                    Try
                        Recibos += Convert.ToDecimal(Fila.Item(5))
                    Catch ex As Exception
                        Dim itemValue As Object = Fila.Item(5)
                        Dim itemDescription As String = If(IsDBNull(itemValue), "DBNull", If(itemValue Is Nothing, "Nothing", itemValue.ToString()))

                        Throw New Exception($"Error converting 'Fila.Item(5)' *{itemDescription}* to Decimal: " & ex.Message)
                    End Try

                    Cobradores += 1
                    If dlMostrar.SelectedIndex = 1 Or dlMostrar.SelectedIndex = 5 Then
                        Liquida = Fila.Item(9)
                        Liquida2 = Fila.Item(10)
                    End If

                    If dlMostrar.SelectedIndex = 0 Then

                        Try
                            Verdes += Convert.ToDecimal(Fila.Item(8))
                            lblVerdes.Text = "Verdes: " + Format(Verdes, "#,##0")
                        Catch ex As Exception
                            Dim itemValue As Object = Fila.Item(8)
                            Dim itemDescription As String = If(IsDBNull(itemValue), "DBNull", If(itemValue Is Nothing, "Nothing", itemValue.ToString()))

                            Throw New Exception($"Error converting 'Fila.Item(8)': **{itemDescription}**  to Decimal: " & ex.Message)
                        End Try
                    End If

                    If dlMostrar.SelectedIndex = 0 Or dlMostrar.SelectedIndex = 1 Or dlMostrar.SelectedIndex = 2 Then
                        Try
                            Ventas += Convert.ToDecimal(Fila.Item(7))
                            lblVentas.Text = "Ventas: " + Format(Ventas, "#,##0")
                        Catch ex As Exception
                            Dim itemValue As Object = Fila.Item(7)
                            Dim itemDescription As String = If(IsDBNull(itemValue), "DBNull", If(itemValue Is Nothing, "Nothing", itemValue.ToString()))
                            Dim details As String = $"Fecha: {txtFecha.Text}, Mostrar: {dlMostrar.SelectedValue}, Lider: {dllider.SelectedValue}, Zona: {dlzona.SelectedValue}, Run: {dlRun.SelectedValue}"
                            Throw New Exception($"Error converting 'Fila.Item(7)': **{itemDescription}**  to Decimal: {details}. " & ex.Message)
                        End Try

                    End If

                    If dlMostrar.SelectedIndex = 3 Then
                        lblVentas.Text = "Ventas: 0"
                    End If

                    lblTotal.Text = "Cobrado: " + Format(Total, "#,##0.00")
                    lblRecibos.Text = "Recibos: " + Format(Recibos, "#,##0")
                    lblVisitados.Text = "Visitados: " + Format(Visitados, "#,##0")
                    lblCobradores.Text = "Vendedores: " + Cobradores.ToString
                    e.Row.Cells(6).Text = Format(Fila.Item(2), "#,##0.00")
                End If
            Catch ex As Exception
                DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
                Msg(ex.Message.ToString() + " - " + ex.Source.ToString())
            End Try

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim Codigo As String = gvMonitor.DataKeys(e.Row.RowIndex).Value.ToString()
                Dim gvDetalle As GridView = TryCast(e.Row.FindControl("gvDetalle"), GridView)

                If dlMostrar.SelectedIndex = 1 Or dlMostrar.SelectedIndex = 5 Then
                    Session("GVDetalle").DefaultView.RowFilter = "Liquida='" + Liquida.Replace("/", "").Replace(" ", "").Replace(":", "") + "' AND Liquida2='" + Liquida2.Replace("/", "").Replace(" ", "").Replace(":", "") + "' AND RCODVEND='" + Codigo + "'"
                ElseIf dlMostrar.SelectedIndex = 0 And dllider.SelectedValue = "TODOS" Then
                    Session("GVDetalle").DefaultView.RowFilter = "Lider='" + Codigo + "'"
                Else
                    Session("GVDetalle").DefaultView.RowFilter = "RCODVEND='" + Codigo + "'"
                End If
                Session("GVDetalle").DefaultView.Sort = "Fecha Asc, Hora Asc"

                gvDetalle.DataSource = Session("GVDetalle")
                gvDetalle.DataBind()

                For i = 0 To gvDetalle.Rows.Count - 1
                    Select Case gvDetalle.Rows(i).Cells(0).Text
                        Case "NO VENTA"
                            gvDetalle.Rows(i).ControlStyle.ForeColor = System.Drawing.Color.Red
                        Case "PROSPECTO"
                            gvDetalle.Rows(i).ControlStyle.ForeColor = System.Drawing.Color.Green
                    End Select

                    If gvDetalle.Rows(i).Cells(10).Text.TrimEnd = "ANULADO" Then
                        Session("GVDetalle").DefaultView.RowFilter = "Codigo ='" + gvDetalle.Rows(i).Cells(0).Text.TrimEnd + "'"
                        If Session("GVDetalle").DefaultView.Count > 0 Then
                            gvDetalle.Rows(i).Cells(10).ToolTip = Session("GVDetalle").DefaultView.Item(0).Item("Motivo").ToString
                        End If
                    End If

                    gvDetalle.Rows(i).Cells(11).ToolTip = Session("GVDetalle").DefaultView.Item(0).Item("Dir_cliente").ToString
                    'If gvDetalle.Rows(i).Cells(5).Text.Length > 0 Then
                    '    gvDetalle.Rows(i).Cells(5).ToolTip = Session("GVDetalle").DefaultView.Item(0).Item("identidad").ToString
                    'End If

                    Session("GVDetalle").DefaultView.RowFilter = "Codigo='" + gvDetalle.Rows(i).Cells(0).Text.TrimEnd + "'"
                    If Session("GVDetalle").DefaultView.Count > 0 And Session("GVDetalle").DefaultView.Item(0).Item("SERVI2DES").ToString.Length > 0 Then
                        gvDetalle.Rows(i).Cells(9).ToolTip = Session("GVDetalle").DefaultView.Item(0).Item("SERVI2DES").ToString + "
" + Session("GVDetalle").DefaultView.Item(0).Item("SERVI3DES").ToString + " 
" + Session("GVDetalle").DefaultView.Item(0).Item("SERVI4DES").ToString
                        gvDetalle.Rows(i).Cells(9).ControlStyle.ForeColor = System.Drawing.Color.DarkBlue
                    End If

                Next
            End If
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try

    End Sub

    Private Sub gvDetalle2_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDetalle2.RowCommand
        Try
            If e.CommandName = "Ubicacion" Then
                Session.Add("Reporte", "Ubicacion")
                Session.Add("Codigo", gvDetalle2.Rows(e.CommandArgument.ToString).Cells(2).Text.ToString.TrimEnd)
                Response.Redirect("mapa.aspx")

                ifRepote.Dispose()
                ifRepote.Src = "mapa.aspx"
                btnProcesar.Visible = False
                Panel1.Visible = False
                PanelImpresion.Visible = True
                Exit Sub
            End If

            If e.CommandName = "Anular" And dlMostrar.SelectedIndex = 1 And (Usuario_Aut = "MANAGER" Or Usuario_Aut = "ABLANDON" Or Usuario_Aut = "MDERAS" Or Usuario_Aut = "YASMIN" Or Usuario_Aut = "IBLANDON" Or Usuario_Aut = "JULIOCAJA" Or Usuario_Aut = "JULIO") Then
                Dim conf1 As New Configuracion(Usuario, Clave, "AEVENTAS", Servidor)
                Dim Sql1 As String
                lblMensaje.Text = gvDetalle2.Rows(e.CommandArgument.ToString).Cells(2).Text.ToString.TrimEnd
                Session.Add("Codigo_Clie", gvDetalle2.Rows(e.CommandArgument.ToString).Cells(6).Text.ToString.TrimEnd)
                lblMensaje1.Text = Session("Codigo_Clie") + " - " + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(7).Text.ToString.TrimEnd
                lblMensaje2.Text = gvDetalle2.Rows(e.CommandArgument.ToString).Cells(3).Text.ToString.TrimEnd
                Session.Add("HoraVenta", gvDetalle2.Rows(e.CommandArgument.ToString).Cells(5).Text.ToString.TrimEnd)
                'Sql1 = "EXEC ANULAR_RECIBOS_LIQUIDADOS '" + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(2).Text.ToString.TrimEnd + "', '" + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(6).Text.ToString.TrimEnd + "', '" + Usuario_Aut.TrimEnd + "', '" + txtComentario.Text + "'"
                'conf1.EjecutaSql(Sql1)
                Panel1.Visible = False
                Panel2.Visible = True
                Exit Sub
            End If

            If e.CommandName = "Anular" And dlMostrar.SelectedIndex = 2 Then
                lblMensaje.Text = gvDetalle2.Rows(e.CommandArgument.ToString).Cells(2).Text.ToString.TrimEnd
                Session.Add("Codigo_Clie", gvDetalle2.Rows(e.CommandArgument.ToString).Cells(6).Text.ToString.TrimEnd)
                lblMensaje1.Text = Session("Codigo_Clie") + " - " + gvDetalle2.Rows(e.CommandArgument.ToString).Cells(7).Text.ToString.TrimEnd
                lblMensaje2.Text = gvDetalle2.Rows(e.CommandArgument.ToString).Cells(3).Text.ToString.TrimEnd
                Session.Add("HoraVenta", gvDetalle2.Rows(e.CommandArgument.ToString).Cells(5).Text.ToString.TrimEnd)
                Panel1.Visible = False
                Panel2.Visible = True
            End If
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub

    Private Sub gvMonitor_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvMonitor.RowCommand
        Try
            If e.CommandName = "Mapa" Then
                Session.Add("Reporte", "MapaVenta")
                Session.Add("Codigo", gvMonitor.Rows(e.CommandArgument.ToString).Cells(4).Text.ToString.TrimEnd)
                'Response.Redirect("mapa.aspx")

                ifRepote.Dispose()
                ifRepote.Src = "mapa.aspx"
                btnProcesar.Visible = False
                Panel1.Visible = False
                PanelImpresion.Visible = True
                Exit Sub
            End If

            If e.CommandName = "Imprimir" And dlMostrar.SelectedIndex = 5 Then
                Dim Liquida2 As String = gvMonitor.Rows(e.CommandArgument.ToString).Cells(13).Text.ToString.Substring(0, 4) + gvMonitor.Rows(e.CommandArgument.ToString).Cells(13).Text.ToString.Substring(5, 2) + gvMonitor.Rows(e.CommandArgument.ToString).Cells(13).Text.ToString.Substring(8, 2) + gvMonitor.Rows(e.CommandArgument.ToString).Cells(13).Text.ToString.Substring(11, 2) + gvMonitor.Rows(e.CommandArgument.ToString).Cells(13).Text.ToString.Substring(14, 2)
                Session.Add("Cobrador", gvMonitor.Rows(e.CommandArgument.ToString).Cells(4).Text.ToString.TrimEnd)
                Session.Add("Liquida2", Liquida2)
                Session.Add("Reporte", "VentasLiquida2")
                ifRepote.Dispose()
                ifRepote.Src = "liquidacion.aspx"
                btnProcesar.Visible = False
                Panel1.Visible = False
                PanelImpresion.Visible = True
                Exit Sub
            End If

            If e.CommandName = "Liquidar" And dlMostrar.SelectedIndex = 4 Then
                Session.Add("Reporte", "VentasLiquida")
                Session.Add("Cobrador", gvMonitor.Rows(e.CommandArgument.ToString).Cells(4).Text.ToString.TrimEnd)
                Session.Add("Liquida2", Liquida2)
                ifRepote.Dispose()
                ifRepote.Src = "liquidacion.aspx"
                btnProcesar.Visible = True
                Panel1.Visible = False
                PanelImpresion.Visible = True
                Exit Sub
            End If

            If e.CommandName = "Liquidar" And dlMostrar.SelectedIndex <> 4 Then
                Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
                Dim Sql As String

                Sql = "EXEC LIQUIDA_RECIBOS '" + gvMonitor.Rows(e.CommandArgument.ToString).Cells(4).Text.ToString.TrimEnd + "', '" + Session("F1") + "'"
                conf.EjecutaSql(Sql)
                btnBuscar_Click(sender, e)

            End If
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub


    Private Sub gvvendEditVent_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvvendEditVent.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(gvvendEditVent, "Select$" & e.Row.RowIndex)
                e.Row.Attributes("style") = "cursor:pointer"
            End If
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub

    Private Sub gvClientesVe_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvClientesVE.RowCommand
        Try
            Dim Fila As Integer = Convert.ToInt32(e.CommandArgument)
            Dim conf, confZona As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String, SqlZona As String

            Session.Add("CodigoClienteAPP", gvClientesVE.Rows(Fila).Cells(1).Text)
            Session.Add("CodigoVendedorApp", gvClientesVE.Rows(Fila).Cells(3).Text)

            SqlZona = $"Select  "

            'Esto hace que el nombre y el codigo del cliente no se cambie si se escoge otro, para que sirve?
            'If txtCodClienteapp.Text.Length = 0 Or lblNameClientapp.InnerText = "" Then
            txtCodClienteapp.Text = gvClientesVE.Rows(Fila).Cells(1).Text
            lblNameClientapp.InnerText = gvClientesVE.Rows(Fila).Cells(2).Text
            'End If
            Dim clientId = gvClientesVE.Rows(Fila).Cells(1).Text.TrimEnd
            Dim salesPersonId = gvClientesVE.Rows(Fila).Cells(3).Text.TrimEnd
            txtCodVendEV.Text = gvClientesVE.Rows(Fila).Cells(3).Text.TrimEnd
            txtnombreVendArr.InnerText = gvClientesVE.Rows(Fila).Cells(4).Text.TrimEnd
            dlstatusvend.Items.Add("" + gvClientesVE.Rows(Fila).Cells(5).Text + "")
            dlempresaArr.Items.Add("" + gvClientesVE.Rows(Fila).Cells(6).Text + "")
            initialPayment = gvClientesVE.Rows(Fila).Cells(11).Text
            Session("initialPayment") = initialPayment
            Dim identificationDocument = gvClientesVE.Rows(Fila).Cells(7).Text.TrimEnd
            Dim phoneNumberOne = gvClientesVE.Rows(Fila).Cells(18).Text.TrimEnd
            Dim phoneNumberTwo = gvClientesVE.Rows(Fila).Cells(19).Text.TrimEnd
            Dim serviceId = gvClientesVE.Rows(Fila).Cells(15).Text
            Dim serviceName = gvClientesVE.Rows(Fila).Cells(16).Text
            Dim quantity = gvClientesVE.Rows(Fila).Cells(14).Text
            Dim payment = gvClientesVE.Rows(Fila).Cells(17).Text
            Dim totalAmount = gvClientesVE.Rows(Fila).Cells(13).Text
            Dim billNumber = gvClientesVE.Rows(Fila).Cells(12).Text
            Dim serviempre = gvClientesVE.Rows(Fila).Cells(20).Text.TrimEnd
            Session("serviempre") = serviempre
            Session("serviceId") = serviceId
            Session("Product") = serviceName
            Session("totalAmount") = totalAmount
            'txtcuotaApp.Text =
            Session.Add("EmpresaV", gvClientesVE.Rows(Fila).Cells(6).Text)


            Sql = "Select  cod_Zona Empresa from AEVENTAS..CZONA WHERE COD_ZONA NOT IN ('M','" + gvClientesVE.Rows(Fila).Cells(6).Text + "')"

            Datos = conf.EjecutaSql(Sql)
            dlempresaArr.Items.Add("" + gvClientesVE.Rows(Fila).Cells(6).Text + "")
            dlempresaArr.DataSource = Datos.Tables(0)
            dlempresaArr.DataTextField = "Empresa"
            dlempresaArr.DataValueField = "Empresa"
            dlempresaArr.DataBind()
            RaiseEvent DataSendEvent(Me, New ClientDataReceivedEventArgs(clientId, salesPersonId, identificationDocument, phoneNumberOne, phoneNumberTwo, initialPayment))
            RaiseEvent DataContractSendEvent(Me, New ContractDataReceivedEventArgs(serviceId, serviceName, quantity, payment, totalAmount, billNumber, serviempre))

            PanelClientesVE.Visible = False

        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try

    End Sub


    Private Sub gvDetalleProductosContrato_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDetalleProductosContrato.RowCommand
        Try
            Dim Fila As Integer = Convert.ToInt32(e.CommandArgument)
            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            'Dim Letras As Integer
            'Dim Sql As String

            Dim product = gvDetalleProductosContrato.Rows(Fila).Cells(2).Text
            Dim amount = gvDetalleProductosContrato.Rows(Fila).Cells(4).Text
            Dim payment = gvDetalleProductosContrato.Rows(Fila).Cells(5).Text
            Dim serviceId = gvDetalleProductosContrato.Rows(Fila).Cells(1).Text
            Dim serviempre = gvDetalleProductosContrato.Rows(Fila).Cells(8).Text
            Session("serviempre") = serviempre
            Session("serviceId") = serviceId
            Session("Product") = product
            Session.Add("Producto", gvDetalleProductosContrato.Rows(Fila).Cells(2).Text)
            Session.Add("IdServicio", gvDetalleProductosContrato.Rows(Fila).Cells(1).Text)
            Session.Add("ValorContratoApp", gvDetalleProductosContrato.Rows(Fila).Cells(3).Text)
            Session.Add("ValorMaximo", gvDetalleProductosContrato.Rows(Fila).Cells(6).Text)
            Session.Add("ValorMinimo", gvDetalleProductosContrato.Rows(Fila).Cells(7).Text)

            '  Letras = (gvDetalleProductosContrato.Rows(Fila).Cells(3).Text - Session("Prima")) '/ gvDetalleProductosContrato.Rows(Fila).Cells(4).Text

            'txtLetraApp.Text = Letras
            RaiseEvent ProductSendEvent(Me, New ProductDataReceivedEventArgs(product, serviceId, payment, amount, serviempre))

            PanelProductosApp.Visible = False
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try

    End Sub

    'Private Sub btnBuscarProducto_Click(sender As Object, e As EventArgs) Handles btnBuscarProducto.Click
    '    PanelProductosApp.Visible = True
    '    txtprod1_TextChanged(sender, e)

    'End Sub

    Protected Sub txtprod1_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String
            Dim textBox As TextBox = CType(sender, TextBox)

            Dim productNameOrCode As String = textBox.Text.Trim

            Sql = "Select serv_codigo Codigo, serv_descri Descripcion
		,serv_cant Equivale
		,serv_precio Precio
		,serv_valoje Cuotas
		,SERV_PMAX  PrecioMaximo
		,SERV_PMINI PrecioMinimo
        ,serv_empre 
		from AEVentas..SERVICIO

        WHERE serv_codigo not in ('','08') and serv_precio > 0 and serv_descri like '%" & productNameOrCode & "%'"
            Datos = conf.EjecutaSql(Sql)


            gvDetalleProductosContrato.DataSource = Datos.Tables(0)
            gvDetalleProductosContrato.DataBind()

            'btnGuardarCamb.Enabled = True
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub
    'Protected Sub txtvalorcontApp_TextChanged(sender As Object, e As EventArgs)
    '    Dim Letra, Cuota As Integer
    '    Try
    '        If txtcuotaApp.Text.Length > 0 And txtvalorcontApp.Text.Length > 0 And txtLetraApp.Text.Length > 0 And initialPayment.Length > 0 Then

    '            If initialPayment = txtvalorcontApp.Text Then
    '                Cuota = 0
    '                Letra = 0
    '            Else
    '                If txtvalorcontApp.Text > 0 And initialPayment > 0 And txtcuotaApp.Text > 0 Then
    '                    Letra = (txtvalorcontApp.Text - initialPayment) / txtcuotaApp.Text
    '                    Cuota = txtcuotaApp.Text
    '                End If

    '            End If
    '            txtLetraApp.Text = Letra
    '            txtcuotaApp.Text = Cuota


    '            btnGuardarCamb.Enabled = True

    '        End If

    '    Catch ex As Exception
    '        lblMsjError.Text = "Error: " & ex.Message
    '        lblMsjError.ControlStyle.CssClass = "alert alert-danger"

    '    End Try


    'End Sub

    'Private Sub BtnGuardarSi_Click(sender As Object, e As EventArgs) Handles BtnGuardarSi.Click
    '    PanelClientesVE.Visible = False

    'End Sub

    '  Private Sub btnGuardarCamb_click(seder As Object, e As EventArgs) Handles btnGuardarCamb.Click
    '      Try
    '          Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
    '          Dim Prima, ValorCont As Integer
    '          Dim Sql, Producto, IdProduct As String

    '          If Session("IdServicio") = "" Then
    '              IdProduct = Session("IdServicioM")
    '              Producto = Session("ProductoM")

    '          Else
    '              IdProduct = Session("IdServicio")
    '              Producto = Session("Producto")
    '          End If



    '          Sql = "Select serv_codigo Codigo, serv_descri Descripcion
    ',serv_cant Equivale
    ',serv_precio Precio
    ',serv_valoje Cuotas
    ',SERV_PMAX  PrecioMaximo
    ',SERV_PMINI PrecioMinimo
    'from AEVentas..SERVICIO
    '      WHERE serv_codigo not in ('','08') and serv_precio > 0 and serv_codigo = '" + IdProduct + "'"

    '          Datos = conf.EjecutaSql(Sql)

    '          If Session("PrimaM") = "" Then
    '              Prima = Session("Prima")
    '              ValorCont = Session("ValorContratoApp")
    '          Else
    '              Prima = Session("PrimaM")
    '              ValorCont = Session("ValorContApp")
    '          End If


    '          If txtprod1.Text.Trim.Length = 0 Then
    '              lblMsjError.Text = "Error: Debe agregar un producto"
    '              lblMsjError.ControlStyle.CssClass = "alert alert-danger"
    '              Exit Sub
    '          End If

    '          If txtcuotaApp.Text.TrimEnd = 0 And Prima < txtvalorcontApp.Text Then
    '              lblMsjError.Text = "Error: Cuotas no debe ser cero"
    '              lblMsjError.ControlStyle.CssClass = "alert alert-danger"
    '              Exit Sub
    '          End If

    '          If txtcuotaApp.Text.TrimEnd > 0 And Prima = txtvalorcontApp.Text Then
    '              lblMsjError.Text = "Error: Cuotas no debe ser Cero"
    '              lblMsjError.ControlStyle.CssClass = "alert alert-danger"
    '              Exit Sub
    '          End If

    '          If txtLetraApp.Text = 0 And Prima < txtvalorcontApp.Text Then
    '              lblMsjError.Text = "Error: Debe Ingresar numero de letras"
    '              lblMsjError.ControlStyle.CssClass = "alert alert-danger"
    '              Exit Sub
    '          End If

    '          If (txtLetraApp.Text * txtcuotaApp.Text) > txtvalorcontApp.Text Then
    '              lblMsjError.Text = "Error: Corregir el Valor o numero de cuota"
    '              lblMsjError.ControlStyle.CssClass = "alert alert-danger"
    '              Exit Sub
    '          End If

    '          If txtcanti1app.Text = 0 Then
    '              lblMsjError.Text = "Error: Cantidad debe ser mayor a cero(0)"
    '              lblMsjError.ControlStyle.CssClass = "alert alert-danger"
    '              Exit Sub
    '          End If

    '          'If txtvalorcontApp.Text.TrimEnd > Datos.Tables(0).Rows(0).Item("PrecioMaximo") Then
    '          '    lblMsjError.Text = "Error: Valor debe ser menor Que '" + Format(Datos.Tables(0).Rows(0).Item("PrecioMaximo"), "#,##0.00") + "'"
    '          '    lblMsjError.ControlStyle.CssClass = "alert alert-danger"
    '          '    Exit Sub
    '          'End If

    '          'If txtvalorcontApp.Text.TrimEnd < Datos.Tables(0).Rows(0).Item("PrecioMinimo") Then
    '          '    lblMsjError.Text = "Error: Valor debe ser Mayor Que '" + Format(Datos.Tables(0).Rows(0).Item("PrecioMinimo"), "#,##0.00") + "'"
    '          '    lblMsjError.ControlStyle.CssClass = "alert alert-danger"
    '          '    Exit Sub
    '          'End If

    '          'If txtCodClienteapp.Text.Length = 0 Then
    '          '    lblMsjError.Text = "Error: Debe Seleccionar un Cliente"
    '          '    lblMsjError.ControlStyle.CssClass = "alert alert-danger"
    '          '    Exit Sub
    '          'End If



    '          If txtcuotaApp.Text * txtLetraApp.Text > ((txtvalorcontApp.Text * txtcanti1app.Text) - Prima) Then
    '              lblMsjError.Text = "Error: Verifique el N.Cuotas y Letras"
    '              lblMsjError.ControlStyle.CssClass = "alert alert-danger"
    '              Exit Sub
    '          End If


    '          PanelConfirmacion.Visible = True
    '      Catch ex As Exception
    '          lblMsjError.Text = "Error: " & ex.Message
    '          lblMsjError.ControlStyle.CssClass = "alert alert-danger"

    '      End Try

    '  End Sub
    Protected Sub dlempresaArr_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String

            Sql = "Select * from (Select '' Codigo, '' Descripcion
                Union all
                Select 'N' Codigo, 'SinLiquidacion' Descripcion 
                union all
                SElect 'X' Codigo, 'Anular'
                union all
                Select CONCAT(DATEPART(year, GETDATE()) , CASE WHEN LEN(DATEPART(MONTH,GETDATE()))=1 THEN CONCAT('0', DATEPART(MONTH,GETDATE())) ELSE CONCAT('', DATEPART(MONTH,GETDATE()))  END ,
                CASE WHEN LEN(DATEPART(DAY,GETDATE()))=1 THEN CONCAT('0', DATEPART(DAY,GETDATE())) ELSE CONCAT('', DATEPART(DAY,GETDATE())) END  , CASE WHEN LEN(DATEPART(HOUR,GETDATE()))=1 THEN CONCAT('0', DATEPART(HOUR,GETDATE())) 
                ELSE CONCAT('', DATEPART(HOUR,GETDATE()))  END , CASE WHEN LEN(DATEPART(MINUTE,GETDATE()))=1 THEN CONCAT('0', DATEPART(MINUTE,GETDATE())) ELSE CONCAT('', DATEPART(MINUTE,GETDATE())) END )  Codigo, 'Liquidar' Descripcion -- SinLiquidacion
                union all
                Select 'N' Codigo, 'DesAnular')A
                where Codigo = '" + dlempresaArr.SelectedValue + "'"


            PanelConfirmacion2.Visible = True
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub



    Private Sub gvvendEditVent_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvvendEditVent.RowCommand
        Try
            Dim Fila As Integer = Convert.ToInt32(e.CommandArgument)
            txtBuscarVended.Text = ""
            txtCodVendEV.Text = gvvendEditVent.Rows(Fila).Cells(1).Text.TrimEnd
            txtnombreVendArr.InnerText = gvvendEditVent.Rows(Fila).Cells(2).Text.TrimEnd
            PanelVendedoresEditar.Visible = False
            'txtVendEV_TextChanged(sender, e)
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub


    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Try
            Panel2.Visible = False
            Panel1.Visible = True
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub

    Private Sub BtnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        Try
            If txtClave.Text.ToUpper <> Clave_Aut.ToUpper Then
                Msg("Clave Incorrecta")
                Exit Sub
            End If

            If dlMostrar.SelectedItem.Text = "POR LIQUIDACION" And (Usuario_Aut.ToUpper = "MANAGER" Or Usuario_Aut = "MDERAS" Or Usuario_Aut = "YASMIN" Or Usuario_Aut.ToUpper = "ABLANDON" Or Usuario_Aut = "IBLANDON" Or Usuario_Aut.ToUpper = "JULIOCAJA" Or Usuario_Aut.ToUpper = "JULIO") Then
                Dim conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
                Dim Sql1 As String

                Sql1 = "EXEC ANULAR_RECIBOS_LIQUIDADOS '" + lblMensaje.Text.TrimEnd + "', '" + Session("Codigo_Clie") + "', '" + Usuario_Aut.TrimEnd + "', '" + txtComentario.Text + "','" + Session("HoraVenta") + "'"
                conf1.EjecutaSql(Sql1)

                Panel2.Visible = False
                Panel1.Visible = True
                btnBuscar_Click(sender, e)
                Exit Sub
            End If

            If dlMostrar.SelectedIndex = 2 Then
                Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
                Dim Sql As String

                Sql = "EXEC ANULAR_RECIBOS '" + lblMensaje.Text.TrimEnd + "', '" + Session("Codigo_Clie") + "', '" + Usuario_Aut + "', '" + txtComentario.Text + "','" + Session("HoraVenta") + "'"
                conf.EjecutaSql(Sql)

                Panel2.Visible = False
                Panel1.Visible = True
                btnBuscar_Click(sender, e)
            End If
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub
    Private Sub btnCambStatus_click(Sender As Object, e As EventArgs) Handles btnCambStatus.Click
        Try
            Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String

            If dlstatusvend.SelectedItem.Value = "Liquidada" Or dlstatusvend.SelectedItem.Value = "SinLiquidacion" Then
                Sql = "EXEC ANULAR_RECIBOS_LIQUIDADOS '" + Session("Documento") + "','" + txtCodClienteapp.Text + "','" + Session("Usuario") + "','" + txtmotivoCambSta.Text.TrimEnd + "'"
            End If

            Datos = conf.EjecutaSql(Sql)


            PanelConfirmacion2.Visible = False
            PanelEditarVenta.Visible = False
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try

    End Sub



    Private Sub BtnCerrarStatus_click(Sender As Object, e As EventArgs) Handles BtnCerrarStatus.Click
        Try
            PanelConfirmacion2.Visible = False
            PanelEditarVenta.Visible = False
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub
    Private Sub BtnSiSalvarCamb_Clik(sender As Object, e As EventArgs) Handles BtnSiSalvarCamb.Click
        Try
            Dim conf, conf2 As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql, Producto, IdProduct, serviempre As String

            If Session("IdServicio") = "" Then
                IdProduct = Session("IdServicioM")
                Producto = Session("ProductoM")

            Else

                IdProduct = Session("IdServicio")
                Producto = Session("Producto")
            End If
            serviempre = Session("serviempre")
            Dim Product = Session("Product")

            Dim serviceId = Session("serviceId")
            'If txtcuotaApp.Text = 0 Then
            '    lblMsg.Text = "Error: Cuotas debe ser Cero"
            '    lblMsg.ControlStyle.CssClass = "alert alert-danger"
            '    Exit Sub
            'End If

            'If initialPayment < txtvalorcontApp.Text And (txtcuotaApp.Text = 0 Or txtLetraApp.Text = 0) Then
            '    lblMsg.Text = "Error: Debe Ingresar el Valor de la Cuota y N.Letras"
            '    lblMsg.ControlStyle.CssClass = "alert alert-danger"
            '    Exit Sub
            'End If
            Dim amount = CorrectSalesDataClient1.ValorContratoAppText
            Dim payment = CorrectSalesDataClient1.CuotaContratoAppText
            Dim billNumber = CorrectSalesDataClient1.LetraContratoAppText
            Dim quantity = CorrectSalesDataClient1.CantidadProducto1appText
            initialPayment = Session("initialPayment")

            If Usuario_Aut Is Nothing Then
                Usuario_Aut = Session("Usuario_Aut")
            End If
            'Datos = conf.EjecutaSql(Sql)
            Dim dateValue As DateTime = DateTime.Now ' Replace with your desired date
            Dim formattedDate As String = dateValue.ToString("yyyyMMddHHmm")
            Console.WriteLine(formattedDate)
            CorrectSalesDataClient1.SaveChanges()
            Dim oldId = Session("OldId")
            Using context As New AeVentasDbContext()
                context.Database.Log = Sub(s) System.Diagnostics.Debug.WriteLine(s)

                Dim parameters As SqlParameter() = {
                New SqlParameter("@CONT_NUMERO", txtCodClienteapp.Text.Trim),
                New SqlParameter("@Codigo_clie", txtCodClienteapp.Text.Trim),
                New SqlParameter("@CONT_VALOR", amount),
                New SqlParameter("@CONT_CREDIT", "S"),
                New SqlParameter("@CONT_PRIMA", Replace(initialPayment, ",", "")),
                New SqlParameter("@CONT_NUMCUO", billNumber),
                New SqlParameter("@CONT_VALCUO", payment),
                New SqlParameter("@cont_vended", txtCodVendEV.Text.Trim),
                New SqlParameter("@CONT_SERVI", serviceId),
                New SqlParameter("@CONT_CANTI", quantity),
                New SqlParameter("@CONT_COMPA", ""),
                New SqlParameter("@CONT_CANT2", 0),
                New SqlParameter("@CONT_SERV2", 0),
                New SqlParameter("@CONT_CANT3", 0),
                New SqlParameter("@CONT_SERV3", 0),
                New SqlParameter("@CONT_CANT4", 0),
                New SqlParameter("@CONT_SERV4", 0),
                New SqlParameter("@CONT_SVAL1", amount),
                New SqlParameter("@CONT_SVAL2", 0),
                New SqlParameter("@CONT_SVAL3", 0),
                New SqlParameter("@CONT_SVAL4", 0),
                New SqlParameter("@VENTA", "S"),
                New SqlParameter("@TEMPO", "F"),
                New SqlParameter("@CEDULA", CorrectSalesDataClient1.IdentificationText.Trim),
                New SqlParameter("@CEDULAVIEJA", oldId),
                New SqlParameter("@SERVI1DES", Product),
                New SqlParameter("@SERVI2DES", ""),
                New SqlParameter("@SERVI3DES", ""),
                New SqlParameter("@SERVI4DES", ""),
                New SqlParameter("@SERVIEMPRE", serviempre),
                New SqlParameter("@NOMCLIE", lblNameClientapp.InnerText.Trim),
                New SqlParameter("@CIERRE", formattedDate),
                New SqlParameter("@LIQUIDA", "N"),
                New SqlParameter("@Usuario", Usuario_Aut)
            }

                context.Database.ExecuteSqlCommand(
                "EXEC [dbo].[SP_CONTRATO_LOG] " &
                "@CONT_NUMERO = @CONT_NUMERO, @Codigo_clie = @Codigo_clie, @CONT_VALOR = @CONT_VALOR, @CONT_CREDIT = @CONT_CREDIT, @CONT_PRIMA = @CONT_PRIMA, " &
                "@CONT_NUMCUO = @CONT_NUMCUO, @CONT_VALCUO = @CONT_VALCUO, @cont_vended = @cont_vended, @CONT_SERVI = @CONT_SERVI, @CONT_CANTI = @CONT_CANTI, " &
                "@CONT_COMPA = @CONT_COMPA, @CONT_CANT2 = @CONT_CANT2, @CONT_SERV2 = @CONT_SERV2, @CONT_CANT3 = @CONT_CANT3, @CONT_SERV3 = @CONT_SERV3, " &
                "@CONT_CANT4 = @CONT_CANT4, @CONT_SERV4 = @CONT_SERV4, @CONT_SVAL1 = @CONT_SVAL1, @CONT_SVAL2 = @CONT_SVAL2, @CONT_SVAL3 = @CONT_SVAL3, " &
                "@CONT_SVAL4 = @CONT_SVAL4, @VENTA = @VENTA, @TEMPO = @TEMPO, @CEDULA = @CEDULA,@CEDULAVIEJA=@CEDULAVIEJA, @SERVI1DES = @SERVI1DES, @SERVI2DES = @SERVI2DES, " &
                "@SERVI3DES = @SERVI3DES, @SERVI4DES = @SERVI4DES, @SERVIEMPRE = @SERVIEMPRE, @NOMCLIE = @NOMCLIE, @CIERRE = @CIERRE, @LIQUIDA = @LIQUIDA, " &
                "@Usuario = @Usuario", parameters)

            End Using


            Session("OldId") = ""
            lblMsg.Text = "Operación realizada"
            lblMsg.ControlStyle.CssClass = "alert alert-success"

            txtCodClienteapp.Text = ""
            txtCodVendEV.Text = ""
            txtnombreVendArr.InnerText = ""

            Session("ValorContrato") = 0
            Session("PrimaM") = 0
            Session("Prima") = 0
            Session("ProductoM") = ""
            Session("IdServicioM") = ""
            Session("IdServicio") = ""
            Session("initialPayment") = 0
            Session("serviempre") = ""
            lblNameClientapp.InnerText = ""
            'txtCobrador.Text = ""
            txtFecha.Text = ""
            txtCodVendEV.Text = ""
            'btnGuardarCamb.Enabled = False
            Producto = ""
            IdProduct = ""
            PanelEditarVenta.Visible = False
            PanelConfirmacion.Visible = False
            'txtCobrador.Text = txtCodVendEV.Text
            btnBuscar_Click(sender, e)


        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try


    End Sub




    Private Sub BtnNoSalvar_Clik(sender As Object, e As EventArgs) Handles BtnNoSalvar.Click
        Try
            PanelConfirmacion.Visible = False
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub




    Private Sub btnCerrarPanelProductosApp_Click(sender As Object, e As EventArgs) Handles btnCerrarPanelProductosApp.Click
        Try
            PanelProductosApp.Visible = False
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub
    Protected Sub HandleAlertGenerated(ByVal sender As Object, ByVal e As AlertEventArgs)
        Dim message As String = e.Message
        Dim alertType As String = e.AlertType
        AlertHelper.GenerateAlert(alertType, message, alertPlaceholder)
    End Sub
    Protected Sub Alert(message, alertType)
        AlertHelper.GenerateAlert(alertType, message, alertPlaceholder)
    End Sub

    Public Sub CorrectContract_ProductTextChanged(sender As Object, e As EventArgs) Handles CorrectSalesDataClient1.ProductTextChanged
        Try
            ' Update the GridView
            'UpdateGVDetalleProductosContrato()
            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String

            Dim textBox As TextBox = CType(sender, TextBox)

            Dim productNameOrCode As String = textBox.Text.Trim

            Sql = "Select serv_codigo Codigo, serv_descri Descripcion
		,serv_cant Equivale
		,serv_precio Precio
		,serv_valoje Cuotas
		,SERV_PMAX  PrecioMaximo
		,SERV_PMINI PrecioMinimo
        ,serv_empre 
		from AEVentas..SERVICIO

        WHERE serv_codigo not in ('','08') and serv_precio > 0 and serv_descri like '%" & productNameOrCode & "%'"
            '          Sql = "Select serv_codigo Codigo, serv_descri Descripcion
            ',serv_cant Equivale
            ',serv_precio Precio
            ',serv_valoje Cuotas
            ',SERV_PMAX  PrecioMaximo
            ',SERV_PMINI PrecioMinimo
            '      ,serv_empre 

            'from AEVentas..SERVICIO

            '      WHERE serv_codigo not in ('','08') and serv_precio > 0"
            Datos = conf.EjecutaSql(Sql)


            gvDetalleProductosContrato.DataSource = Datos.Tables(0)
            gvDetalleProductosContrato.DataBind()

            'btnGuardarCamb.Enabled = True
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try
    End Sub

    Public Sub ProductButtonClick()
        Try
            PanelProductosApp.Visible = True
        Catch ex As Exception
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            Alert(dangerMsg & ex.Message & vbCrLf & "Código de error:" & ex.HResult, "danger")
        End Try

    End Sub
    Public Sub RedirectToCobrosAdvanced(sender As Object, e As EventArgs) Handles btnAdvanced.Click
        Response.Redirect("~/Dashboards/Ventas/Ventas.aspx")
    End Sub
End Class