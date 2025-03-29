Imports System.Web.UI
Imports System
Imports System.Web.UI.WebControls
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports Subgurim.Controls
Imports Subgurim.Controls.GoogleMaps
Imports Subgurim.Controles
Imports Subgurim.Controles.GoogleChartIconMaker

Public Class mapa
    Inherits System.Web.UI.Page
    Private Datos As DataSet
    Dim Pin As XPinLetter
    Dim Marca As GMarker
    Dim Info As GInfoWindow
    Dim Linea As GPolyline
    Dim Coordenadas As GLatLng
    Dim Puntos As New List(Of GLatLng)
    Dim textColor As String = "black"
    Dim pathColor As String = "FF0000"
    Dim textContent As String = "9"


    Dim defaultIcon As String = "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0MCIgaGVpZ2h0PSI0MCIgdmlld0JveD0iMCAwIDIwIDIwIj48cGF0aCBmaWxsPSJjdXJyZW50Q29sb3IiIGZpbGwtcnVsZT0iZXZlbm9kZCIgZD0ibTkuNjkgMTguOTMzbC4wMDMuMDAxQzkuODkgMTkuMDIgMTAgMTkgMTAgMTlzLjExLjAyLjMwOC0uMDY2bC4wMDItLjAwMWwuMDA2LS4wMDNsLjAxOC0uMDA4YTYgNiAwIDAgMCAuMjgxLS4xNGMuMTg2LS4wOTYuNDQ2LS4yNC43NTctLjQzM2MuNjItLjM4NCAxLjQ0NS0uOTY2IDIuMjc0LTEuNzY1QzE1LjMwMiAxNC45ODggMTcgMTIuNDkzIDE3IDlBNyA3IDAgMSAwIDMgOWMwIDMuNDkyIDEuNjk4IDUuOTg4IDMuMzU1IDcuNTg0YTEzLjcgMTMuNyAwIDAgMCAyLjI3MyAxLjc2NWExMiAxMiAwIDAgMCAuOTc2LjU0NGwuMDYyLjAyOWwuMDE4LjAwOHpNMTAgMTEuMjVhMi4yNSAyLjI1IDAgMSAwIDAtNC41YTIuMjUgMi4yNSAwIDAgMCAwIDQuNSIgY2xpcC1ydWxlPSJldmVub2RkIi8+PC9zdmc+"

    Dim redIcon As New IconGenerator("#FF0000")
    Dim blueIcon As New IconGenerator("#0000FF")
    Dim yellowIcon As New IconGenerator("#FFFF00")
    Dim purpleIcon As New IconGenerator("#800080")
    Dim greenIcon As New IconGenerator("#008000")


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            If (Not IsPostBack) Then
                If Session("Usuario") = "" Then
                    Response.Redirect("inicio.aspx")
                End If

                If Session("Reporte") = "Ubicacion" Then
                    Ubicacion()
                End If

                If Session("Reporte") = "Mapa" Then
                    Mapa()
                End If

                If Session("Reporte") = "MapaVenta" Then
                    MapaVenta()
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Sub Ubicacion()
        Dim mainLocation As GLatLng = New GLatLng(21.622517, 87.523417)
        GMap1.setCenter(mainLocation, 15)
        Dim XPinLetter As XPinLetter = New XPinLetter(PinShapes.pin_star, "A", Color.Blue, Color.White, Color.Chocolate)
        GMap1.Add(New GMarker(mainLocation, New GMarkerOptions(New GIcon(XPinLetter.ToString, XPinLetter.Shadow()))))


        GMap1.setCenter(New GLatLng(Session("GVDetalle").DefaultView.Item(0).Item("LATITUD"), Session("GVDetalle").DefaultView.Item(0).Item("LONGITUD")), 15)
        Pin = New XPinLetter(PinShapes.pin, "A", Color.Green, Color.White, Color.Chocolate)
        Marca = New GMarker(New GLatLng(Session("GVDetalle").DefaultView.Item(0).Item("LATITUD"), Session("GVDetalle").DefaultView.Item(0).Item("LONGITUD")), New GMarkerOptions(New GIcon(Pin.ToString(), Pin.Shadow())))
        Info = New GInfoWindow(Marca, "<b>" + Session("GVDetalle").DefaultView.Item(0).Item("Codigo Cliente") + " - " + Session("GVDetalle").DefaultView.Item(0).Item("Nombre_clie") + "</b><br>Fecha: " + Session("GVDetalle").DefaultView.Item(0).Item("Fecha") + "<br>Hora: " + Session("GVDetalle").DefaultView.Item(0).Item("Hora") + "<br>Cobrado: " + Session("GVDetalle").DefaultView.Item(0).Item("Cobrado").ToString, False, GListener.Event.mouseover)
        GMap1.Add(Info)
    End Sub

    Sub Mapa()
        Session("GVDetalle").DefaultView.RowFilter = "codigo_cobr='" + Session("Codigo") + "'"
        Session("GVDetalle").DefaultView.Sort = "Fecha Asc, Hora Asc"

        If Session("GVDetalle").DefaultView.Count > 0 Then
            GMap1.ShowMapTypeControl = True
            GMap1.setCenter(New GLatLng(Session("GVDetalle").DefaultView.Item(0).Item("LATITUD"), Session("GVDetalle").DefaultView.Item(0).Item("LONGITUD")), 15)
        Else
            Exit Sub
        End If

        For i As Integer = 0 To Session("GVDetalle").DefaultView.Count - 1
            Dim icon As String = defaultIcon
            Dim shadow = "/imagenes/circleShadow.png"
            Dim count = (i + 1)
            Select Case Session("GVDetalle").DefaultView.Item(i).Item("Codigo")
                Case "NO COBRO"
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Red, Color.White, Color.Chocolate)
                    redIcon.TextContent = count.ToString
                    icon = redIcon.GetBase64UriIcon()
                Case "SUPERVISION"
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Blue, Color.White, Color.Chocolate)
                    blueIcon.TextContent = count.ToString
                    icon = blueIcon.GetBase64UriIcon()
                Case "NO SUPERVISION"
                    blueIcon.TextContent = count.ToString
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Red, Color.White, Color.Chocolate)
                    icon = yellowIcon.GetBase64UriIcon()
                Case Else
                    greenIcon.TextContent = count.ToString
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Green, Color.White, Color.Chocolate)
                    icon = greenIcon.GetBase64UriIcon()
            End Select

            Coordenadas = New GLatLng(Session("GVDetalle").DefaultView.Item(i).Item("LATITUD"), Session("GVDetalle").DefaultView.Item(i).Item("LONGITUD"))
            Puntos.Add(Coordenadas)
            Dim m = Pin.ToString()

            Dim n = Pin.Shadow()
            Marca = New GMarker(Coordenadas, New GMarkerOptions(New GIcon(icon, shadow)))
            Info = New GInfoWindow(Marca, "<b>" + Session("GVDetalle").DefaultView.Item(i).Item("Codigo Cliente") + " - " + Session("GVDetalle").DefaultView.Item(i).Item("Nombre_clie") + "</b><br>Fecha: " + Session("GVDetalle").DefaultView.Item(i).Item("Fecha") + "<br>Hora: " + Session("GVDetalle").DefaultView.Item(i).Item("Hora") + "<br>Cobrado: " + Session("GVDetalle").DefaultView.Item(i).Item("Cobrado").ToString, False, GListener.Event.mouseover)
            Linea = New GPolyline(Puntos)

            GMap1.Add(Linea)
            GMap1.Add(Info)
        Next

    End Sub

    Sub MapaVenta()
        Session("GVDetalle").DefaultView.RowFilter = "RCODVEND='" + Session("Codigo") + "'"
        Session("GVDetalle").DefaultView.Sort = "Fecha Asc, Hora Asc"

        If Session("GVDetalle").DefaultView.Count > 0 Then
            GMap1.ShowMapTypeControl = True
            GMap1.setCenter(New GLatLng(Session("GVDetalle").DefaultView.Item(0).Item("LATITUD"), Session("GVDetalle").DefaultView.Item(0).Item("LONGITUD")), 15)
        Else
            Exit Sub
        End If

        For i As Integer = 0 To Session("GVDetalle").DefaultView.Count - 1
            Dim icon As String = defaultIcon
            Dim count = i + 1
            Select Case Session("GVDetalle").DefaultView.Item(i).Item("Codigo")
                Case "PROSPECTO"
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Green, Color.White, Color.Chocolate)
                    greenIcon.TextContent = count.ToString
                    icon = greenIcon.GetBase64UriIcon()
                Case "NO VENTA"
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Purple, Color.White, Color.Chocolate)
                    purpleIcon.TextContent = count.ToString
                    icon = purpleIcon.GetBase64UriIcon()

                Case Else
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Red, Color.White, Color.Chocolate)
                    redIcon.TextContent = count.ToString
                    icon = redIcon.GetBase64UriIcon()
            End Select

            Coordenadas = New GLatLng(Session("GVDetalle").DefaultView.Item(i).Item("LATITUD"), Session("GVDetalle").DefaultView.Item(i).Item("LONGITUD"))
            Puntos.Add(Coordenadas)

            Marca = New GMarker(Coordenadas, New GMarkerOptions(New GIcon(icon, Pin.Shadow())))
            Info = New GInfoWindow(Marca, "<b>" + Session("GVDetalle").DefaultView.Item(i).Item("Codigo Cliente") + " - " + Session("GVDetalle").DefaultView.Item(i).Item("Nombre_clie") + "</b><br>Fecha: " + Session("GVDetalle").DefaultView.Item(i).Item("Fecha") + "<br>Hora: " + Session("GVDetalle").DefaultView.Item(i).Item("Hora") + "<br>Cobrado: " + Session("GVDetalle").DefaultView.Item(i).Item("Cobrado").ToString, False, GListener.Event.mouseover)
            Linea = New GPolyline(Puntos)


            GMap1.Add(Linea)
            GMap1.Add(Info)
        Next

    End Sub

    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub

    Private Sub Vista_Click(sender As Object, e As EventArgs) Handles Vista.Click
        If Vista.Text = "Satelite" Then
            GMap1.mapType = GMapType.GTypes.Satellite
            Vista.Text = "Mapa"
        Else
            GMap1.mapType = GMapType.GTypes.Normal
            Vista.Text = "Satelite"
        End If

    End Sub
End Class
Public Class IconGenerator
    Private _color As String
    Private _textColor As String
    Private _textContent As String
    Public Property Color As String
        Get
            Return _color
        End Get
        Set(value As String)
            _color = value
        End Set
    End Property

    Public Property TextColor As String
        Get
            Return _textColor
        End Get
        Set(value As String)
            _textColor = value
        End Set
    End Property

    Public Property TextContent As String
        Get
            Return _textContent
        End Get
        Set(value As String)
            _textContent = value
        End Set
    End Property
    Public Sub New(ByVal pathColor As String)
        Me.Color = pathColor
        Me.TextColor = "white"
        Me.TextContent = ""
    End Sub

    Public Function GetBase64UriIcon()
        Dim svgCode As String = "<svg xmlns=""http://www.w3.org/2000/svg"" width=""40"" height=""40"" viewBox=""0 0 20 20"">" &
                    "<path fill=""" & Color & """ fill-rule=""evenodd"" d=""m9.69 18.933 0.003 0.001c0.197 0.086 0.308 0.066 0.308 0.066s0.11 0.02 0.308 -0.066l0.002 -0.001 0.006 -0.003 0.018 -0.008a6.222 6.222 0 0 0 0.281 -0.14c0.186 -0.096 0.446 -0.24 0.757 -0.433a13.689 13.689 0 0 0 2.274 -1.765c1.657 -1.596 3.355 -4.091 3.355 -7.584a7 7 0 1 0 -14 0c0 3.492 1.698 5.988 3.355 7.584a13.689 13.689 0 0 0 2.273 1.765 12.444 12.444 0 0 0 0.976 0.544l0.062 0.029 0.018 0.008zm0.31 -7.684a2.25 2.25 0 1 0 0 -4.5 2.25 2.25 0 0 0 0 4.5"" clip-rule=""evenodd""/>" &
                    "<text x=""20"" y=""19"" font-size=""8"" fill=""" & TextColor & """ text-anchor=""end"" font-weight=""bold"">" & TextContent & "</text></svg>"
        Dim bytes As Byte() = Encoding.UTF8.GetBytes(svgCode)

        ' Convert the byte array to a base64 string
        Dim base64String As String = Convert.ToBase64String(bytes)
        base64String = "data:image/svg+xml;base64," & base64String
        ' Return the base64 string

        Return base64String
    End Function
End Class