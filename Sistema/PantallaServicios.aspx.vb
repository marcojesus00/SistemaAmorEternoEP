Public Class PantallaServicios
    Inherits System.Web.UI.Page
    Private Datos As New Data.DataSet
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Table1.Visible = False

            Session.Add("Servidor", "192.168.20.225")
            Session.Add("BD", "FUNAMOR")
            Session.Add("Usuario", "mmejia")
            Session.Add("Clave", "Mm%%4567")

            'Dim Datos As New Data.DataSet
            'Dim Sql As String
            'Dim conf As New Configuracion(Session("Usuario"), Session("Clave"), Session("BD"), Session("Servidor"))
            'Sql = " SELECT F1, F2 FROM PARPOSI "
            'Datos = conf.EjecutaSql(Sql)

            'Session.Add("F1", Datos.Tables(0).Rows(0).Item("F1"))
            'Session.Add("F2", Datos.Tables(0).Rows(0).Item("F2"))

            ' Panel12.Visible = True
            llenarGrafica()
            Timer1.Interval = 25000
            Timer1.Enabled = True
        End If
    End Sub


    Sub llenarGrafica()

        Dim conf As New Configuracion(Session("Usuario"), Session("Clave"), Session("BD"), Session("Servidor"))
        Dim sql As String

        sql = " exec  FUNAMOR..SP_VS_PantallaServicios "

        Datos = conf.EjecutaSql(sql)
        GridView5.DataSource = Datos.Tables(0)
        GridView5.DataBind()

    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick


        'If Panel13.Visible = True Then
        '    llenarGridViewCatVendedor()
        '    Panel13.Visible = False
        '    Panel12.Visible = True
        '    Panel11.Visible = False
        '    Panel10.Visible = False
        '    Panel9.Visible = False
        '    'Panel1.Visible = False
        '    'Panel2.Visible = False

        '    Exit Sub
        'End If
        llenarGrafica()
    End Sub

    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub
End Class