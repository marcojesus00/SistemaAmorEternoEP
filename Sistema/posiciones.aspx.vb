Public Class posiciones
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Table1.Visible = False

            Session.Add("Servidor", "192.168.20.225")
            Session.Add("BD", "FUNAMOR")
            Session.Add("Usuario", "sistema.web")
            Session.Add("Clave", "$$Eterno4321.")

            Dim Datos As New Data.DataSet
            Dim Sql As String
            Dim conf As New Configuracion(Session("Usuario"), Session("Clave"), Session("BD"), Session("Servidor"))
            Sql = " SELECT FV1, FV2, FC1, FC2 FROM PARPOSI "
            Datos = conf.EjecutaSql(Sql)

            Session.Add("F1", Datos.Tables(0).Rows(0).Item("FV1"))
            Session.Add("F2", Datos.Tables(0).Rows(0).Item("FV2"))
            Session.Add("FC1", Datos.Tables(0).Rows(0).Item("FC1"))
            Session.Add("FC2", Datos.Tables(0).Rows(0).Item("FC2"))

            Panel1.Visible = False
            Panel2.Visible = False
            Panel3.Visible = False
            Panel4.Visible = False
            Panel5.Visible = False
            Panel6.Visible = False
            Panel7.Visible = False
            Panel8.Visible = False
            Panel9.Visible = True
            Panel10.Visible = False
            Panel11.Visible = False
            Panel12.Visible = False
            Panel13.Visible = False

            llenarGraficaLider()
            Timer1.Interval = 25000
            Timer1.Enabled = True
        End If
    End Sub

    Sub llenarGraficaVendedores()
        Dim F1, F2 As Date
        Dim Datos As New Data.DataSet
        Dim Sql As String
        F1 = Session("F1")
        F2 = Session("F2")

        Dim conf As New Configuracion(Session("Usuario"), Session("Clave"), Session("BD"), Session("Servidor"))
        Sql = " EXEC POSICIONES_VENTAS_1 '" + Format(F1, "yyyy/MM/dd") + "', '" + Format(F2, "yyyy/MM/dd") + "' "
        Datos = conf.EjecutaSql(Sql)

        If Datos.Tables.Count.ToString = "0" Then
            Msg("No hay registros en estas fechas" + F1.ToString + " " + F2.ToString)
            Exit Sub
        End If

        Dim Filas As Integer
        Filas = Datos.Tables(0).Rows.Count

        If Filas < 14 Then
            DesHabilitar_Imagen_Vendedor()
        End If

        If Filas >= 0 Then
            Dim A As Integer = 0
            If Filas > 14 Then
                A = 14
            Else
                A = Filas
            End If

            For i As Integer = 0 To A
                GraficaVendedor.Series(0).Points.AddXY(Datos.Tables(0).Rows(i).Item("Vendedor"), Datos.Tables(0).Rows(i).Item("Ventas"))
                GraficaVendedor.Series(0).Points(i).Label = (i + 1).ToString + ".    " + Datos.Tables(0).Rows(i).Item("Ventas").ToString + " Ventas"
            Next

            GraficaVendedor.Series(0).Font = New Drawing.Font(GraficaVendedor.Series(0).Font.Name, 12)
            GraficaCobrador.Series(0).LabelForeColor = Drawing.Color.DarkBlue
            GraficaVendedor.ChartAreas(0).AxisX.LabelStyle.Font = New Drawing.Font(GraficaVendedor.Series(0).Font.Name, 10)
            GraficaVendedor.Series(0).Palette = DataVisualization.Charting.ChartColorPalette.Pastel
            GraficaVendedor.Series(0).SmartLabelStyle.Enabled = False
            GraficaVendedor.Series(0).LabelAngle = -45
            GraficaVendedor.ChartAreas(0).AxisX.Interval = 1
            GraficaVendedor.Width = "1500"
            GraficaVendedor.Height = "800"
            GraficaVendedor.Titles(0).Text = "Top 15 Vendedores"
            GraficaVendedor.Titles(0).Font = New Drawing.Font(GraficaVendedor.Series(0).Font.Name, 25)
            GraficaVendedor.Titles(0).ForeColor = Drawing.Color.DarkBlue
            GraficaVendedor.Titles.Add("Desde: " + Format(F1, "dd/MM/yy") + " Hasta: " + Format(Today, "dd/MM/yy"))
            GraficaVendedor.Titles(1).Font = New Drawing.Font(GraficaVendedor.Series(0).Font.Name, 15)
            'GraficaVendedor.Titles(1).ForeColor = Drawing.Color.DarkBlue
        Else
            Panel2.Visible = True
        End If
    End Sub

    Sub llenarGraficaVentasDiamante()
        Dim F1, F2 As Date
        Dim Datos As New Data.DataSet
        Dim Sql As String
        F1 = Session("F1")
        F2 = Session("F2")

        Dim conf As New Configuracion(Session("Usuario"), Session("Clave"), Session("BD"), Session("Servidor"))
        Sql = " EXEC POSICIONES_VENTAS_CATEGORIA'" + Format(F1, "yyyy/MM/dd") + "', '" + Format(F2, "yyyy/MM/dd") + "', 'Diamante' "
        Datos = conf.EjecutaSql(Sql)

        If Datos.Tables.Count.ToString = "0" Then
            Msg("No hay registros en estas fechas" + F1.ToString + " " + F2.ToString)
            Exit Sub
        End If

        Dim Filas As Integer
        Filas = Datos.Tables(0).Rows.Count

        'If Filas < 14 Then
        '    DesHabilitar_Imagen_Vendedor()
        'End If

        If Filas >= 0 Then
            Dim A As Integer = 0
            If Filas > 14 Then
                A = 14
            Else
                A = Filas - 1
            End If

            For i As Integer = 0 To A
                GraficaVentasDiamante.Series(0).Points.AddXY(Datos.Tables(0).Rows(i).Item("Vendedor"), Datos.Tables(0).Rows(i).Item("Ventas"))
                GraficaVentasDiamante.Series(0).Points(i).Label = (i + 1).ToString + ".    " + Datos.Tables(0).Rows(i).Item("Ventas").ToString + " Ventas"
            Next

            GraficaVentasDiamante.Series(0).Font = New Drawing.Font(GraficaVentasDiamante.Series(0).Font.Name, 12)
            GraficaVentasDiamante.Series(0).LabelForeColor = Drawing.Color.DarkBlue
            GraficaVentasDiamante.ChartAreas(0).AxisX.LabelStyle.Font = New Drawing.Font(GraficaVentasDiamante.Series(0).Font.Name, 10)
            GraficaVentasDiamante.Series(0).Palette = DataVisualization.Charting.ChartColorPalette.Pastel
            GraficaVentasDiamante.Series(0).SmartLabelStyle.Enabled = False
            GraficaVentasDiamante.Series(0).LabelAngle = -45
            GraficaVentasDiamante.ChartAreas(0).AxisX.Interval = 1
            GraficaVentasDiamante.Width = "1500"
            GraficaVentasDiamante.Height = "800"
            GraficaVentasDiamante.Titles(0).Text = "Vendedores Diamante"
            GraficaVentasDiamante.Titles(0).Font = New Drawing.Font(GraficaVentasDiamante.Series(0).Font.Name, 25)
            GraficaVentasDiamante.Titles(0).ForeColor = Drawing.Color.DarkBlue
            GraficaVentasDiamante.Titles.Add("Desde: " + Format(F1, "dd/MM/yy") + " Hasta: " + Format(Today, "dd/MM/yy"))
            GraficaVentasDiamante.Titles(1).Font = New Drawing.Font(GraficaVentasDiamante.Series(0).Font.Name, 15)
            'GraficaVentasDiamante.Titles(1).ForeColor = Drawing.Color.DarkBlue
        Else
            Panel11.Visible = True
        End If
    End Sub

    Sub llenarGraficaVentasOro()
        Dim F1, F2 As Date
        Dim Datos As New Data.DataSet
        Dim Sql As String
        F1 = Session("F1")
        F2 = Session("F2")

        Dim conf As New Configuracion(Session("Usuario"), Session("Clave"), Session("BD"), Session("Servidor"))
        Sql = " EXEC POSICIONES_VENTAS_CATEGORIA'" + Format(F1, "yyyy/MM/dd") + "', '" + Format(F2, "yyyy/MM/dd") + "', 'Oro' "
        Datos = conf.EjecutaSql(Sql)

        If Datos.Tables.Count.ToString = "0" Then
            Msg("No hay registros en estas fechas" + F1.ToString + " " + F2.ToString)
            Exit Sub
        End If

        Dim Filas As Integer
        Filas = Datos.Tables(0).Rows.Count

        If Filas >= 0 Then
            Dim A As Integer = 0
            If Filas > 14 Then
                A = 14
            Else
                A = Filas - 1
            End If

            For i As Integer = 0 To A
                GraficaVentasOro.Series(0).Points.AddXY(Datos.Tables(0).Rows(i).Item("Vendedor"), Datos.Tables(0).Rows(i).Item("Ventas"))
                GraficaVentasOro.Series(0).Points(i).Label = (i + 1).ToString + ".    " + Datos.Tables(0).Rows(i).Item("Ventas").ToString + " Ventas"
            Next

            GraficaVentasOro.Series(0).Font = New Drawing.Font(GraficaVentasOro.Series(0).Font.Name, 12)
            GraficaVentasOro.Series(0).LabelForeColor = Drawing.Color.DarkBlue
            GraficaVentasOro.ChartAreas(0).AxisX.LabelStyle.Font = New Drawing.Font(GraficaVentasOro.Series(0).Font.Name, 10)
            GraficaVentasOro.Series(0).Palette = DataVisualization.Charting.ChartColorPalette.Pastel
            GraficaVentasOro.Series(0).SmartLabelStyle.Enabled = False
            GraficaVentasOro.Series(0).LabelAngle = -45
            GraficaVentasOro.ChartAreas(0).AxisX.Interval = 1
            GraficaVentasOro.Width = "1500"
            GraficaVentasOro.Height = "800"
            GraficaVentasOro.Titles(0).Text = "Vendedores Oro"
            GraficaVentasOro.Titles(0).Font = New Drawing.Font(GraficaVentasOro.Series(0).Font.Name, 25)
            GraficaVentasOro.Titles(0).ForeColor = Drawing.Color.DarkBlue
            GraficaVentasOro.Titles.Add("Desde: " + Format(F1, "dd/MM/yy") + " Hasta: " + Format(Today, "dd/MM/yy"))
            GraficaVentasOro.Titles(1).Font = New Drawing.Font(GraficaVentasOro.Series(0).Font.Name, 15)
            'GraficaVentasOro.Titles(1).ForeColor = Drawing.Color.DarkBlue
        Else
            Panel12.Visible = True
        End If
    End Sub

    Sub llenarGraficaVentasPlata()
        Dim F1, F2 As Date
        Dim Datos As New Data.DataSet
        Dim Sql As String
        F1 = Session("F1")
        F2 = Session("F2")

        Dim conf As New Configuracion(Session("Usuario"), Session("Clave"), Session("BD"), Session("Servidor"))
        Sql = " EXEC POSICIONES_VENTAS_CATEGORIA'" + Format(F1, "yyyy/MM/dd") + "', '" + Format(F2, "yyyy/MM/dd") + "', 'Plata' "
        Datos = conf.EjecutaSql(Sql)

        If Datos.Tables.Count.ToString = "0" Then
            Msg("No hay registros en estas fechas" + F1.ToString + " " + F2.ToString)
            Exit Sub
        End If

        Dim Filas As Integer
        Filas = Datos.Tables(0).Rows.Count

        If Filas >= 0 Then
            Dim A As Integer = 0
            If Filas > 14 Then
                A = 14
            Else
                A = Filas - 1
            End If

            For i As Integer = 0 To A
                GraficaVentasPlata.Series(0).Points.AddXY(Datos.Tables(0).Rows(i).Item("Vendedor"), Datos.Tables(0).Rows(i).Item("Ventas"))
                GraficaVentasPlata.Series(0).Points(i).Label = (i + 1).ToString + ".    " + Datos.Tables(0).Rows(i).Item("Ventas").ToString + " Ventas"
            Next

            GraficaVentasPlata.Series(0).Font = New Drawing.Font(GraficaVentasPlata.Series(0).Font.Name, 12)
            GraficaVentasPlata.Series(0).LabelForeColor = Drawing.Color.DarkBlue
            GraficaVentasPlata.ChartAreas(0).AxisX.LabelStyle.Font = New Drawing.Font(GraficaVentasPlata.Series(0).Font.Name, 10)
            GraficaVentasPlata.Series(0).Palette = DataVisualization.Charting.ChartColorPalette.Pastel
            GraficaVentasPlata.Series(0).SmartLabelStyle.Enabled = False
            GraficaVentasPlata.Series(0).LabelAngle = -45
            GraficaVentasPlata.ChartAreas(0).AxisX.Interval = 1
            GraficaVentasPlata.Width = "1500"
            GraficaVentasPlata.Height = "800"
            GraficaVentasPlata.Titles(0).Text = "Vendedores Plata"
            GraficaVentasPlata.Titles(0).Font = New Drawing.Font(GraficaVentasPlata.Series(0).Font.Name, 25)
            GraficaVentasPlata.Titles(0).ForeColor = Drawing.Color.DarkBlue
            GraficaVentasPlata.Titles.Add("Desde: " + Format(F1, "dd/MM/yy") + " Hasta: " + Format(Today, "dd/MM/yy"))
            GraficaVentasPlata.Titles(1).Font = New Drawing.Font(GraficaVentasPlata.Series(0).Font.Name, 15)
            'GraficaVentasPlata.Titles(1).ForeColor = Drawing.Color.DarkBlue
        Else
            Panel13.Visible = True
        End If
    End Sub

    Sub llenarGraficaVentasBronce()
        Dim F1, F2 As Date
        Dim Datos As New Data.DataSet
        Dim Sql As String
        F1 = Session("F1")
        F2 = Session("F2")

        Dim conf As New Configuracion(Session("Usuario"), Session("Clave"), Session("BD"), Session("Servidor"))
        Sql = " EXEC POSICIONES_VENTAS_CATEGORIA'" + Format(F1, "yyyy/MM/dd") + "', '" + Format(F2, "yyyy/MM/dd") + "', 'Bronce' "
        Datos = conf.EjecutaSql(Sql)

        If Datos.Tables.Count.ToString = "0" Then
            Msg("No hay registros en estas fechas" + F1.ToString + " " + F2.ToString)
            Exit Sub
        End If

        Dim Filas As Integer
        Filas = Datos.Tables(0).Rows.Count

        If Filas >= 0 Then
            Dim A As Integer = 0
            If Filas > 14 Then
                A = 14
            Else
                A = Filas - 1
            End If

            For i As Integer = 0 To A
                GraficaVentasBronce.Series(0).Points.AddXY(Datos.Tables(0).Rows(i).Item("Vendedor"), Datos.Tables(0).Rows(i).Item("Ventas"))
                GraficaVentasBronce.Series(0).Points(i).Label = (i + 1).ToString + ".    " + Datos.Tables(0).Rows(i).Item("Ventas").ToString + " Ventas"
            Next

            GraficaVentasBronce.Series(0).Font = New Drawing.Font(GraficaVentasBronce.Series(0).Font.Name, 12)
            GraficaVentasBronce.Series(0).LabelForeColor = Drawing.Color.DarkBlue
            GraficaVentasBronce.ChartAreas(0).AxisX.LabelStyle.Font = New Drawing.Font(GraficaVentasBronce.Series(0).Font.Name, 10)
            GraficaVentasBronce.Series(0).Palette = DataVisualization.Charting.ChartColorPalette.Pastel
            GraficaVentasBronce.Series(0).SmartLabelStyle.Enabled = False
            GraficaVentasBronce.Series(0).LabelAngle = -45
            GraficaVentasBronce.ChartAreas(0).AxisX.Interval = 1
            GraficaVentasBronce.Width = "1500"
            GraficaVentasBronce.Height = "800"
            GraficaVentasBronce.Titles(0).Text = "Vendedores Bronce"
            GraficaVentasBronce.Titles(0).Font = New Drawing.Font(GraficaVentasBronce.Series(0).Font.Name, 25)
            GraficaVentasBronce.Titles(0).ForeColor = Drawing.Color.DarkBlue
            GraficaVentasBronce.Titles.Add("Desde: " + Format(F1, "dd/MM/yy") + " Hasta: " + Format(Today, "dd/MM/yy"))
            GraficaVentasBronce.Titles(1).Font = New Drawing.Font(GraficaVentasBronce.Series(0).Font.Name, 15)
            'GraficaVentasPlata.Titles(1).ForeColor = Drawing.Color.DarkBlue
        Else
            Panel1.Visible = True
        End If
    End Sub

    Sub llenarGridViewVendedores()
        Dim F1, F2 As Date
        Dim Datos As New Data.DataSet
        Dim Sql As String
        F1 = Session("F1")
        F2 = Session("F2")

        Dim conf As New Configuracion(Session("Usuario"), Session("Clave"), Session("BD"), Session("Servidor"))
        Sql = " EXEC POSICIONES_VENTAS_1 '" + Format(F1, "yyyy/MM/dd") + "', '" + Format(F2, "yyyy/MM/dd") + "' "
        Datos = conf.EjecutaSql(Sql)

        If Datos.Tables.Count.ToString = "0" Then
            Msg("No hay registros en estas fechas")
            Exit Sub
        End If

        Dim Filas As Integer
        Filas = Datos.Tables(0).Rows.Count

        If Filas >= 1 Then
            gvPosiciones.DataSource = Datos.Tables(0)
            gvPosiciones.DataBind()

            If Filas > 30 Then
                gvPosiciones0.DataSource = Datos.Tables(0)
                gvPosiciones0.DataBind()
            End If

            If Filas > 60 Then
                gvPosiciones1.DataSource = Datos.Tables(0)
                gvPosiciones1.DataBind()
            End If

            If Filas > 90 Then
                gvPosiciones2.DataSource = Datos.Tables(0)
                gvPosiciones2.DataBind()
            End If
        Else
            Panel2.Visible = True
        End If
    End Sub

    Sub llenarGraficaCobradores()
        Dim F1, F2 As Date
        Dim Datos As New Data.DataSet
        Dim Sql As String
        F1 = Session("FC1")
        F2 = Session("FC2")

        Dim conf1 As New Configuracion(Session("Usuario"), Session("Clave"), Session("BD"), Session("Servidor"))
        Sql = " EXEC POSICIONES_COBRADORES '" + Format(F1, "yyyy/MM/dd") + "', '" + Format(F2, "yyyy/MM/dd") + "'"
        Datos = conf1.EjecutaSql(Sql)

        If Datos.Tables.Count.ToString = "0" Then
            Msg("No hay registros en estas fechas")
            Exit Sub
        End If

        Dim Filas As Integer
        Filas = Datos.Tables(0).Rows.Count

        If Filas >= 1 Then
            For i As Integer = 0 To 14
                GraficaCobrador.Series(0).Points.AddXY(Datos.Tables(0).Rows(i).Item("Cobrador"), Datos.Tables(0).Rows(i).Item("Valor"))
                GraficaCobrador.Series(0).Points(i).Label = (i + 1).ToString  ''+ ".    " + Datos1.Tables(0).Rows(i).Item("Cobrador").ToString + ""
            Next

            GraficaCobrador.Series(0).Font = New Drawing.Font(GraficaCobrador.Series(0).Font.Name, 15)
            GraficaCobrador.Series(0).LabelForeColor = Drawing.Color.DarkBlue
            GraficaCobrador.ChartAreas(0).AxisX.LabelStyle.Font = New Drawing.Font(GraficaCobrador.Series(0).Font.Name, 10)
            GraficaCobrador.Series(0).Palette = DataVisualization.Charting.ChartColorPalette.Pastel
            GraficaCobrador.Series(0).SmartLabelStyle.Enabled = False
            GraficaCobrador.ChartAreas(0).AxisX.Interval = 1
            GraficaCobrador.Width = "1500"
            GraficaCobrador.Height = "800"
            GraficaCobrador.Titles(0).Text = "TOP 15 Cobradores"
            GraficaCobrador.Titles(0).Font = New Drawing.Font(GraficaCobrador.Series(0).Font.Name, 25)
            GraficaCobrador.Titles(0).ForeColor = Drawing.Color.DarkBlue
            GraficaCobrador.Titles.Add("Desde: " + Format(F1, "dd/MM/yy") + " Hasta: " + Format(Today, "dd/MM/yy"))
            GraficaCobrador.Titles(1).Font = New Drawing.Font(GraficaCobrador.Series(0).Font.Name, 15)
            'GraficaCobrador.Titles(1).ForeColor = Drawing.Color.DarkBlue
        Else
            Panel4.Visible = True
        End If
    End Sub

    Sub llenarGridViewCobradores()
        Dim F1, F2 As Date
        Dim Datos1, Datos2, Datos3 As New Data.DataSet
        Dim Sql1 As String
        F1 = Session("FC1")
        F2 = Session("FC2")

        Dim conf1 As New Configuracion(Session("Usuario"), Session("Clave"), Session("BD"), Session("Servidor"))
        Sql1 = " EXEC POSICIONES_COBRADORES '" + Format(F1, "yyyy/MM/dd") + "', '" + Format(F2, "yyyy/MM/dd") + "'"
        Datos1 = conf1.EjecutaSql(Sql1)

        If Datos1.Tables.Count.ToString = "0" Then
            Panel1.Visible = True
        End If

        Dim Filas1 As Integer
        Filas1 = Datos1.Tables(0).Rows.Count

        If Filas1 >= 1 Then
            GridView1.DataSource = Datos1.Tables(0)
            GridView1.DataBind()
        End If
        If Filas1 > 30 Then
            GridView2.DataSource = Datos1.Tables(0)
            GridView2.DataBind()
        End If
        If Filas1 > 60 Then
            GridView3.DataSource = Datos1.Tables(0)
            GridView3.DataBind()
        End If

    End Sub

    Sub llenarGraficaLider()
        Dim F1, F2 As Date
        Dim Datos, Datos1 As New Data.DataSet
        Dim Total As Integer
        Dim Sql As String
        F1 = Session("F1")
        F2 = Session("F2")

        Dim conf1 As New Configuracion(Session("Usuario"), Session("Clave"), Session("BD"), Session("Servidor"))
        Sql = " EXEC POSICIONES_VENTAS_LIDER_1 '" + Format(F1, "yyyy/MM/dd") + "', '" + Format(F2, "yyyy/MM/dd") + "'"
        Datos = conf1.EjecutaSql(Sql)

        If Datos.Tables.Count.ToString = "0" Then
            Msg("No hay registros en estas fechas")
            Exit Sub
        End If

        Dim Filas2 As Integer
        Filas2 = Datos.Tables(0).Rows.Count

        If Filas2 <= 12 Then
            DesHabilitar_Imagen_Lider()
        End If

        If Filas2 >= 0 Then
            Dim A As Integer = 0
            If Filas2 > 12 Then
                A = 12
            Else
                A = Filas2
            End If

            For i As Integer = 0 To A - 1
                GraficaLider.Series(0).Points.AddXY(Datos.Tables(0).Rows(i).Item("Lider"), Datos.Tables(0).Rows(i).Item("Ventas"))
                GraficaLider.Series(0).Points(i).Label = (i + 1).ToString + ".    " + Datos.Tables(0).Rows(i).Item("Ventas").ToString + " Ventas"
                Total += Integer.Parse(Datos.Tables(0).Rows(i).Item("Ventas"))
            Next

            GraficaLider.Series(0).Font = New Drawing.Font(GraficaLider.Series(0).Font.Name, 15)
            GraficaLider.Series(0).LabelForeColor = Drawing.Color.DarkBlue
            GraficaLider.ChartAreas(0).AxisX.LabelStyle.Font = New Drawing.Font(GraficaLider.Series(0).Font.Name, 10)
            GraficaLider.Series(0).Palette = DataVisualization.Charting.ChartColorPalette.Pastel
            GraficaLider.Series(0).SmartLabelStyle.Enabled = False
            GraficaLider.Series(0).LabelAngle = -45
            GraficaLider.ChartAreas(0).AxisX.Interval = 1
            GraficaLider.Width = "1500"
            GraficaLider.Height = "800"
            GraficaLider.Titles(0).Text = "Lideres de Ventas 
Total Ventas " + Total.ToString
            GraficaLider.Titles(0).Font = New Drawing.Font(GraficaLider.Series(0).Font.Name, 25)
            GraficaLider.Titles(0).ForeColor = Drawing.Color.DarkBlue
            GraficaLider.Titles.Add("Desde: " + Format(F1, "dd/MM/yy") + " Hasta: " + Format(Today, "dd/MM/yy"))
            GraficaLider.Titles(1).Font = New Drawing.Font(GraficaLider.Series(0).Font.Name, 15)
            'GraficaCobrador2.Titles(1).ForeColor = Drawing.Color.DarkBlue
        Else
            Panel1.Visible = True
        End If
    End Sub

    Sub llenarGraficaVisitas()
        Dim F1, F2 As Date
        Dim Datos As New Data.DataSet
        Dim Sql As String
        F1 = Session("FC1")
        F2 = Session("FC2")

        Dim conf1 As New Configuracion(Session("Usuario"), Session("Clave"), Session("BD"), Session("Servidor"))
        Sql = " EXEC Estadistica_Visitas '" + Format(F1, "yyyy/MM/dd") + "', '" + Format(F2, "yyyy/MM/dd") + "', 'FRANCISCO ALBERTO'"
        Datos = conf1.EjecutaSql(Sql)

        If Datos.Tables.Count.ToString = "0" Then
            Msg("No hay registros en estas fechas")
            Exit Sub
        End If

        Dim Filas As Integer
        Filas = Datos.Tables(0).Rows.Count
        GraficaVisitas.Series.RemoveAt(0)
        GraficaVisitas.Series.Add("Con Cobro")
        GraficaVisitas.Series.Add("Visitados sin Cobro")
        GraficaVisitas.Series.Add("Sin Visitar")
        GraficaVisitas.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.StackedBar
        GraficaVisitas.Series(1).ChartType = DataVisualization.Charting.SeriesChartType.StackedBar
        GraficaVisitas.Series(2).ChartType = DataVisualization.Charting.SeriesChartType.StackedBar
        GraficaVisitas.Series(0).Color = Drawing.Color.DarkBlue
        GraficaVisitas.Series(1).Color = Drawing.Color.Orange
        GraficaVisitas.Series(2).Color = Drawing.Color.Green
        GraficaVisitas.Series(0).LabelForeColor = Drawing.Color.White
        GraficaVisitas.Series(1).LabelForeColor = Drawing.Color.White
        GraficaVisitas.Series(2).LabelForeColor = Drawing.Color.White
        GraficaVisitas.Legends.Add("Leyenda")
        GraficaVisitas.Legends("Leyenda").Font = New Drawing.Font(GraficaVisitas.Series(0).Font.Name, 15)

        If Filas >= 1 Then
            For i As Integer = 0 To Filas - 1
                GraficaVisitas.Series(0).Points.AddXY(Datos.Tables(0).Rows(i).Item("Cobrador"), Datos.Tables(0).Rows(i).Item("Rend_Cobrado"))
                GraficaVisitas.Series(1).Points.AddXY(Datos.Tables(0).Rows(i).Item("Cobrador"), Datos.Tables(0).Rows(i).Item("Rend_Visitados"))
                GraficaVisitas.Series(2).Points.AddXY(Datos.Tables(0).Rows(i).Item("Cobrador"), Datos.Tables(0).Rows(i).Item("Rend_SinVisitados"))
                GraficaVisitas.Series(0).Points(i).Label = Datos.Tables(0).Rows(i).Item("Rend_Cobrado").ToString
                GraficaVisitas.Series(1).Points(i).Label = Datos.Tables(0).Rows(i).Item("Rend_Visitados").ToString
                GraficaVisitas.Series(2).Points(i).Label = Datos.Tables(0).Rows(i).Item("Rend_SinVisitados").ToString
            Next

            GraficaVisitas.ChartAreas(0).AxisX.LabelStyle.Font = New Drawing.Font(GraficaVisitas.Series(0).Font.Name, 10)
            GraficaVisitas.ChartAreas(0).AxisX.Interval = 1
            GraficaVisitas.Width = "1600"
            GraficaVisitas.Height = "850"
            GraficaVisitas.Titles(0).Text = "FRANCISCO ALBERTO"
            GraficaVisitas.Titles(0).Font = New Drawing.Font(GraficaVisitas.Series(0).Font.Name, 25)
            GraficaVisitas.Titles(0).ForeColor = Drawing.Color.DarkBlue
            GraficaVisitas.Titles.Add("Desde: " + Format(F1, "dd/MM/yy") + " Hasta: " + Format(Today, "dd/MM/yy"))
            GraficaVisitas.Titles(1).Font = New Drawing.Font(GraficaVisitas.Series(0).Font.Name, 15)
        Else
            Panel4.Visible = True
        End If
    End Sub

    Sub llenarGraficaVisitas1()
        Dim F1, F2 As Date
        Dim Datos As New Data.DataSet
        Dim Sql As String
        F1 = Session("FC1")
        F2 = Session("FC2")

        Dim conf1 As New Configuracion(Session("Usuario"), Session("Clave"), Session("BD"), Session("Servidor"))
        Sql = " EXEC Estadistica_Visitas '" + Format(F1, "yyyy/MM/dd") + "', '" + Format(F2, "yyyy/MM/dd") + "', 'NORA AGURCIA'"
        Datos = conf1.EjecutaSql(Sql)

        If Datos.Tables.Count.ToString = "0" Then
            Msg("No hay registros en estas fechas")
            Exit Sub
        End If

        Dim Filas As Integer
        Filas = Datos.Tables(0).Rows.Count
        GraficaVisitas1.Series.RemoveAt(0)
        GraficaVisitas1.Series.Add("Con Cobro")
        GraficaVisitas1.Series.Add("Visitados sin Cobro")
        GraficaVisitas1.Series.Add("Sin Visitar")
        GraficaVisitas1.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.StackedBar
        GraficaVisitas1.Series(1).ChartType = DataVisualization.Charting.SeriesChartType.StackedBar
        GraficaVisitas1.Series(2).ChartType = DataVisualization.Charting.SeriesChartType.StackedBar
        GraficaVisitas1.Series(0).Color = Drawing.Color.DarkBlue
        GraficaVisitas1.Series(1).Color = Drawing.Color.Orange
        GraficaVisitas1.Series(2).Color = Drawing.Color.Green
        GraficaVisitas1.Series(0).LabelForeColor = Drawing.Color.White
        GraficaVisitas1.Series(1).LabelForeColor = Drawing.Color.White
        GraficaVisitas1.Series(2).LabelForeColor = Drawing.Color.White
        GraficaVisitas1.Legends.Add("Leyenda")
        GraficaVisitas1.Legends("Leyenda").Font = New Drawing.Font(GraficaVisitas1.Series(0).Font.Name, 15)

        If Filas >= 1 Then
            For i As Integer = 0 To Filas - 1
                GraficaVisitas1.Series(0).Points.AddXY(Datos.Tables(0).Rows(i).Item("Cobrador"), Datos.Tables(0).Rows(i).Item("Rend_Cobrado"))
                GraficaVisitas1.Series(1).Points.AddXY(Datos.Tables(0).Rows(i).Item("Cobrador"), Datos.Tables(0).Rows(i).Item("Rend_Visitados"))
                GraficaVisitas1.Series(2).Points.AddXY(Datos.Tables(0).Rows(i).Item("Cobrador"), Datos.Tables(0).Rows(i).Item("Rend_SinVisitados"))
                GraficaVisitas1.Series(0).Points(i).Label = Datos.Tables(0).Rows(i).Item("Rend_Cobrado").ToString
                GraficaVisitas1.Series(1).Points(i).Label = Datos.Tables(0).Rows(i).Item("Rend_Visitados").ToString
                GraficaVisitas1.Series(2).Points(i).Label = Datos.Tables(0).Rows(i).Item("Rend_SinVisitados").ToString
                'GraficaVisitas.Series(0)("PointWidth") = "0.5"
            Next

            GraficaVisitas1.ChartAreas(0).AxisX.LabelStyle.Font = New Drawing.Font(GraficaVisitas1.Series(0).Font.Name, 10)
            GraficaVisitas1.ChartAreas(0).AxisX.Interval = 1
            GraficaVisitas1.Width = "1600"
            GraficaVisitas1.Height = "850"
            GraficaVisitas1.Titles(0).Text = "NORA AGURCIA"
            GraficaVisitas1.Titles(0).Font = New Drawing.Font(GraficaVisitas1.Series(0).Font.Name, 25)
            GraficaVisitas1.Titles(0).ForeColor = Drawing.Color.DarkBlue
            GraficaVisitas1.Titles.Add("Desde: " + Format(F1, "dd/MM/yy") + " Hasta: " + Format(Today, "dd/MM/yy"))
            GraficaVisitas1.Titles(1).Font = New Drawing.Font(GraficaVisitas1.Series(0).Font.Name, 15)
            'GraficaCobrador.Titles(1).ForeColor = Drawing.Color.DarkBlue
        Else
            Panel4.Visible = True
        End If
    End Sub

    Sub llenarGraficaVisitas2()
        Dim F1, F2 As Date
        Dim Datos As New Data.DataSet
        Dim Sql As String
        F1 = Session("FC1")
        F2 = Session("FC2")

        Dim conf1 As New Configuracion(Session("Usuario"), Session("Clave"), Session("BD"), Session("Servidor"))
        Sql = " EXEC Estadistica_Visitas '" + Format(F1, "yyyy/MM/dd") + "', '" + Format(F2, "yyyy/MM/dd") + "', 'ABDUL AMED BLANDON PINEDA'"
        Datos = conf1.EjecutaSql(Sql)

        If Datos.Tables.Count.ToString = "0" Then
            Msg("No hay registros en estas fechas")
            Exit Sub
        End If

        Dim Filas As Integer
        Filas = Datos.Tables(0).Rows.Count
        GraficaVisitas2.Series.RemoveAt(0)
        GraficaVisitas2.Series.Add("Con Cobro")
        GraficaVisitas2.Series.Add("Visitados sin Cobro")
        GraficaVisitas2.Series.Add("Sin Visitar")
        GraficaVisitas2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.StackedBar
        GraficaVisitas2.Series(1).ChartType = DataVisualization.Charting.SeriesChartType.StackedBar
        GraficaVisitas2.Series(2).ChartType = DataVisualization.Charting.SeriesChartType.StackedBar
        GraficaVisitas2.Series(0).Color = Drawing.Color.DarkBlue
        GraficaVisitas2.Series(1).Color = Drawing.Color.Orange
        GraficaVisitas2.Series(2).Color = Drawing.Color.Green
        GraficaVisitas2.Series(0).LabelForeColor = Drawing.Color.White
        GraficaVisitas2.Series(1).LabelForeColor = Drawing.Color.White
        GraficaVisitas2.Series(2).LabelForeColor = Drawing.Color.White
        GraficaVisitas2.Legends.Add("Leyenda")
        GraficaVisitas2.Legends("Leyenda").Font = New Drawing.Font(GraficaVisitas2.Series(0).Font.Name, 15)

        If Filas >= 1 Then
            For i As Integer = 0 To Filas - 1
                GraficaVisitas2.Series(0).Points.AddXY(Datos.Tables(0).Rows(i).Item("Cobrador"), Datos.Tables(0).Rows(i).Item("Rend_Cobrado"))
                GraficaVisitas2.Series(1).Points.AddXY(Datos.Tables(0).Rows(i).Item("Cobrador"), Datos.Tables(0).Rows(i).Item("Rend_Visitados"))
                GraficaVisitas2.Series(2).Points.AddXY(Datos.Tables(0).Rows(i).Item("Cobrador"), Datos.Tables(0).Rows(i).Item("Rend_SinVisitados"))
                GraficaVisitas2.Series(0).Points(i).Label = Datos.Tables(0).Rows(i).Item("Rend_Cobrado").ToString
                GraficaVisitas2.Series(1).Points(i).Label = Datos.Tables(0).Rows(i).Item("Rend_Visitados").ToString
                GraficaVisitas2.Series(2).Points(i).Label = Datos.Tables(0).Rows(i).Item("Rend_SinVisitados").ToString
                'GraficaVisitas.Series(0)("PointWidth") = "0.5"
            Next

            GraficaVisitas2.ChartAreas(0).AxisX.LabelStyle.Font = New Drawing.Font(GraficaVisitas2.Series(0).Font.Name, 10)
            GraficaVisitas2.ChartAreas(0).AxisX.Interval = 1
            GraficaVisitas2.Width = "1600"
            GraficaVisitas2.Height = "850"
            GraficaVisitas2.Titles(0).Text = "ABDUL AMED BLANDON PINEDA"
            GraficaVisitas2.Titles(0).Font = New Drawing.Font(GraficaVisitas2.Series(0).Font.Name, 25)
            GraficaVisitas2.Titles(0).ForeColor = Drawing.Color.DarkBlue
            GraficaVisitas2.Titles.Add("Desde: " + Format(F1, "dd/MM/yy") + " Hasta: " + Format(Today, "dd/MM/yy"))
            GraficaVisitas2.Titles(1).Font = New Drawing.Font(GraficaVisitas2.Series(0).Font.Name, 15)
            'GraficaCobrador.Titles(1).ForeColor = Drawing.Color.DarkBlue
        Else
            Panel4.Visible = True
        End If
    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        If Panel13.Visible = True Then
            llenarGraficaLider()
            Panel1.Visible = True
            Panel2.Visible = False
            Panel3.Visible = False
            Panel4.Visible = False
            Panel5.Visible = False
            Panel6.Visible = False
            Panel7.Visible = False
            Panel8.Visible = False
            Panel9.Visible = False
            Panel10.Visible = False
            Panel11.Visible = False
            Panel12.Visible = False
            Panel13.Visible = False
            Exit Sub
        End If

        If Panel1.Visible = True Then
            llenarGraficaVendedores()
            Panel1.Visible = False
            Panel2.Visible = True
            Panel3.Visible = False
            Panel4.Visible = False
            Panel5.Visible = False
            Panel6.Visible = False
            Panel7.Visible = False
            Panel8.Visible = False
            Panel9.Visible = False
            Panel10.Visible = False
            Panel11.Visible = False
            Panel12.Visible = False
            Panel13.Visible = False
            Exit Sub
        End If

        If Panel2.Visible = True Then
            llenarGridViewVendedores()
            Panel1.Visible = False
            Panel2.Visible = False
            Panel3.Visible = True
            Panel4.Visible = False
            Panel5.Visible = False
            Panel6.Visible = False
            Panel7.Visible = False
            Panel8.Visible = False
            Panel9.Visible = False
            Panel10.Visible = False
            Panel11.Visible = False
            Panel12.Visible = False
            Panel13.Visible = False
            Exit Sub
        End If

        If Panel3.Visible = True Then
            Panel1.Visible = False
            Panel2.Visible = False
            Panel3.Visible = False
            Panel4.Visible = True
            Panel5.Visible = False
            Panel6.Visible = False
            Panel7.Visible = False
            Panel8.Visible = False
            Panel9.Visible = False
            If An1.ImageUrl = "~/imagenes/Im1.png" Then
                An1.ImageUrl = "~/imagenes/Im3.png"
                An2.ImageUrl = "~/imagenes/Im4.png"
                Exit Sub
            End If
            'If An1.ImageUrl = "~/imagenes/Im3.jpg" Then
            '    An1.ImageUrl = "~/imagenes/Im5.jpg"
            '    An2.ImageUrl = "~/imagenes/Im6.jpg"
            '    Exit Sub
            'End If
            'If An1.ImageUrl = "~/imagenes/Im5.jpg" Then
            '    An1.ImageUrl = "~/imagenes/Im7.jpg"
            '    An2.ImageUrl = "~/imagenes/Im8.jpg"
            '    Exit Sub
            'End If
            'If An1.ImageUrl = "~/imagenes/Im7.jpg" Then
            '    An1.ImageUrl = "~/imagenes/Im9.jpg"
            '    An2.ImageUrl = "~/imagenes/Im10.jpg"
            '    Exit Sub
            'End If
            'If An1.ImageUrl = "~/imagenes/Im9.jpg" Then
            '    An1.ImageUrl = "~/imagenes/Im11.jpg"
            '    An2.ImageUrl = "~/imagenes/Im12.jpg"
            '    Exit Sub
            'End If
            If An1.ImageUrl = "~/imagenes/Im3.png" Then
                An1.ImageUrl = "~/imagenes/Im1.png"
                An2.ImageUrl = "~/imagenes/Im2.png"
                Exit Sub
            End If
            Panel10.Visible = False
            Panel11.Visible = False
            Panel12.Visible = False
            Panel13.Visible = False
        End If

        If Panel4.Visible = True Then
            llenarGraficaCobradores()
            Panel1.Visible = False
            Panel2.Visible = False
            Panel3.Visible = False
            Panel4.Visible = False
            Panel5.Visible = True
            Panel6.Visible = False
            Panel7.Visible = False
            Panel8.Visible = False
            Panel9.Visible = False
            Panel10.Visible = False
            Panel11.Visible = False
            Panel12.Visible = False
            Panel13.Visible = False
            Exit Sub
        End If

        If Panel5.Visible = True Then
            llenarGridViewCobradores()
            Panel1.Visible = False
            Panel2.Visible = False
            Panel3.Visible = False
            Panel4.Visible = False
            Panel5.Visible = False
            Panel6.Visible = True
            Panel7.Visible = False
            Panel8.Visible = False
            Panel9.Visible = False
            Panel10.Visible = False
            Panel11.Visible = False
            Panel12.Visible = False
            Panel13.Visible = False
            Exit Sub
        End If

        If Panel6.Visible = True Then
            llenarGraficaVisitas()
            Panel1.Visible = False
            Panel2.Visible = False
            Panel3.Visible = False
            Panel4.Visible = False
            Panel5.Visible = False
            Panel6.Visible = False
            Panel7.Visible = True
            Panel8.Visible = False
            Panel9.Visible = False
            Panel10.Visible = False
            Panel11.Visible = False
            Panel12.Visible = False
            Panel13.Visible = False
            Exit Sub
        End If

        If Panel7.Visible = True Then
            llenarGraficaVisitas1()
            Panel1.Visible = False
            Panel2.Visible = False
            Panel3.Visible = False
            Panel4.Visible = False
            Panel5.Visible = False
            Panel6.Visible = False
            Panel7.Visible = False
            Panel8.Visible = True
            Panel9.Visible = False
            Panel10.Visible = False
            Panel11.Visible = False
            Panel12.Visible = False
            Panel13.Visible = False
            Exit Sub
        End If

        If Panel8.Visible = True Then
            llenarGraficaVisitas2()
            Panel1.Visible = False
            Panel2.Visible = False
            Panel3.Visible = False
            Panel4.Visible = False
            Panel5.Visible = False
            Panel6.Visible = False
            Panel7.Visible = False
            Panel8.Visible = False
            Panel9.Visible = True
            Panel10.Visible = False
            Panel11.Visible = False
            Panel12.Visible = False
            Panel13.Visible = False
            Exit Sub
        End If

        If Panel9.Visible = True Then
            llenarGraficaVentasDiamante()
            Panel1.Visible = False
            Panel2.Visible = False
            Panel3.Visible = False
            Panel4.Visible = False
            Panel5.Visible = False
            Panel6.Visible = False
            Panel7.Visible = False
            Panel8.Visible = False
            Panel9.Visible = False
            Panel10.Visible = True
            Panel11.Visible = False
            Panel12.Visible = False
            Panel13.Visible = False
            Exit Sub
        End If

        If Panel10.Visible = True Then
            llenarGraficaVentasOro()
            Panel1.Visible = False
            Panel2.Visible = False
            Panel3.Visible = False
            Panel4.Visible = False
            Panel5.Visible = False
            Panel6.Visible = False
            Panel7.Visible = False
            Panel8.Visible = False
            Panel9.Visible = False
            Panel10.Visible = False
            Panel11.Visible = True
            Panel12.Visible = False
            Panel13.Visible = False
            Exit Sub
        End If

        If Panel11.Visible = True Then
            llenarGraficaVentasPlata()
            Panel1.Visible = False
            Panel2.Visible = False
            Panel3.Visible = False
            Panel4.Visible = False
            Panel5.Visible = False
            Panel6.Visible = False
            Panel7.Visible = False
            Panel8.Visible = False
            Panel9.Visible = False
            Panel10.Visible = False
            Panel11.Visible = False
            Panel12.Visible = True
            Panel13.Visible = False
            Exit Sub
        End If

        If Panel12.Visible = True Then
            llenarGraficaVentasBronce()
            Panel1.Visible = False
            Panel2.Visible = False
            Panel3.Visible = False
            Panel4.Visible = False
            Panel5.Visible = False
            Panel6.Visible = False
            Panel7.Visible = False
            Panel8.Visible = False
            Panel9.Visible = False
            Panel10.Visible = False
            Panel11.Visible = False
            Panel12.Visible = False
            Panel13.Visible = True
            Exit Sub
        End If

    End Sub

    Sub DesHabilitar_Imagen_Lider()
        Image38.Visible = False
        Image39.Visible = False
        Image40.Visible = False
        Image41.Visible = False
        Image42.Visible = False
        Image43.Visible = False
        Image44.Visible = False
        Image45.Visible = False
        Image46.Visible = False
        Image47.Visible = False
        Image48.Visible = False
        Image49.Visible = False
    End Sub

    Sub DesHabilitar_Imagen_Vendedor()
        Image1.Visible = False
        Image2.Visible = False
        Image3.Visible = False
        Image4.Visible = False
        Image5.Visible = False
        Image6.Visible = False
        Image7.Visible = False
        Image8.Visible = False
        Image9.Visible = False
        Image10.Visible = False
        Image11.Visible = False
        Image12.Visible = False
        Image13.Visible = False
        Image14.Visible = False
        Image15.Visible = False
    End Sub

    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub

End Class