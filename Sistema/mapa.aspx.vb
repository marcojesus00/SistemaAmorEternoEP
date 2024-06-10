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
    Dim defaultIcon As String = "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0MCIgaGVpZ2h0PSI0MCIgdmlld0JveD0iMCAwIDIwIDIwIj48cGF0aCBmaWxsPSJjdXJyZW50Q29sb3IiIGZpbGwtcnVsZT0iZXZlbm9kZCIgZD0ibTkuNjkgMTguOTMzbC4wMDMuMDAxQzkuODkgMTkuMDIgMTAgMTkgMTAgMTlzLjExLjAyLjMwOC0uMDY2bC4wMDItLjAwMWwuMDA2LS4wMDNsLjAxOC0uMDA4YTYgNiAwIDAgMCAuMjgxLS4xNGMuMTg2LS4wOTYuNDQ2LS4yNC43NTctLjQzM2MuNjItLjM4NCAxLjQ0NS0uOTY2IDIuMjc0LTEuNzY1QzE1LjMwMiAxNC45ODggMTcgMTIuNDkzIDE3IDlBNyA3IDAgMSAwIDMgOWMwIDMuNDkyIDEuNjk4IDUuOTg4IDMuMzU1IDcuNTg0YTEzLjcgMTMuNyAwIDAgMCAyLjI3MyAxLjc2NWExMiAxMiAwIDAgMCAuOTc2LjU0NGwuMDYyLjAyOWwuMDE4LjAwOHpNMTAgMTEuMjVhMi4yNSAyLjI1IDAgMSAwIDAtNC41YTIuMjUgMi4yNSAwIDAgMCAwIDQuNSIgY2xpcC1ydWxlPSJldmVub2RkIi8+PC9zdmc+"

    Dim redIcon = "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0MCIgaGVpZ2h0PSI0MCIgdmlld0JveD0iMCAwIDIwIDIwIj48cGF0aCBmaWxsPSIjZmYzODM4IiBmaWxsLXJ1bGU9ImV2ZW5vZGQiIGQ9Im05LjY5IDE4LjkzM2wuMDAzLjAwMUM5Ljg5IDE5LjAyIDEwIDE5IDEwIDE5cy4xMS4wMi4zMDgtLjA2NmwuMDAyLS4wMDFsLjAwNi0uMDAzbC4wMTgtLjAwOGE2IDYgMCAwIDAgLjI4MS0uMTRjLjE4Ni0uMDk2LjQ0Ni0uMjQuNzU3LS40MzNjLjYyLS4zODQgMS40NDUtLjk2NiAyLjI3NC0xLjc2NUMxNS4zMDIgMTQuOTg4IDE3IDEyLjQ5MyAxNyA5QTcgNyAwIDEgMCAzIDljMCAzLjQ5MiAxLjY5OCA1Ljk4OCAzLjM1NSA3LjU4NGExMy43IDEzLjcgMCAwIDAgMi4yNzMgMS43NjVhMTIgMTIgMCAwIDAgLjk3Ni41NDRsLjA2Mi4wMjlsLjAxOC4wMDh6TTEwIDExLjI1YTIuMjUgMi4yNSAwIDEgMCAwLTQuNWEyLjI1IDIuMjUgMCAwIDAgMCA0LjUiIGNsaXAtcnVsZT0iZXZlbm9kZCIvPjwvc3ZnPg=="
    Dim blueIcon = "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0MCIgaGVpZ2h0PSI0MCIgdmlld0JveD0iMCAwIDIwIDIwIj48cGF0aCBmaWxsPSIjMjA3OWVlIiBmaWxsLXJ1bGU9ImV2ZW5vZGQiIGQ9Im05LjY5IDE4LjkzM2wuMDAzLjAwMUM5Ljg5IDE5LjAyIDEwIDE5IDEwIDE5cy4xMS4wMi4zMDgtLjA2NmwuMDAyLS4wMDFsLjAwNi0uMDAzbC4wMTgtLjAwOGE2IDYgMCAwIDAgLjI4MS0uMTRjLjE4Ni0uMDk2LjQ0Ni0uMjQuNzU3LS40MzNjLjYyLS4zODQgMS40NDUtLjk2NiAyLjI3NC0xLjc2NUMxNS4zMDIgMTQuOTg4IDE3IDEyLjQ5MyAxNyA5QTcgNyAwIDEgMCAzIDljMCAzLjQ5MiAxLjY5OCA1Ljk4OCAzLjM1NSA3LjU4NGExMy43IDEzLjcgMCAwIDAgMi4yNzMgMS43NjVhMTIgMTIgMCAwIDAgLjk3Ni41NDRsLjA2Mi4wMjlsLjAxOC4wMDh6TTEwIDExLjI1YTIuMjUgMi4yNSAwIDEgMCAwLTQuNWEyLjI1IDIuMjUgMCAwIDAgMCA0LjUiIGNsaXAtcnVsZT0iZXZlbm9kZCIvPjwvc3ZnPg=="
    Dim yellowIcon = "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0MCIgaGVpZ2h0PSI0MCIgdmlld0JveD0iMCAwIDIwIDIwIj48cGF0aCBmaWxsPSIjZWJjNDAwIiBmaWxsLXJ1bGU9ImV2ZW5vZGQiIGQ9Im05LjY5IDE4LjkzM2wuMDAzLjAwMUM5Ljg5IDE5LjAyIDEwIDE5IDEwIDE5cy4xMS4wMi4zMDgtLjA2NmwuMDAyLS4wMDFsLjAwNi0uMDAzbC4wMTgtLjAwOGE2IDYgMCAwIDAgLjI4MS0uMTRjLjE4Ni0uMDk2LjQ0Ni0uMjQuNzU3LS40MzNjLjYyLS4zODQgMS40NDUtLjk2NiAyLjI3NC0xLjc2NUMxNS4zMDIgMTQuOTg4IDE3IDEyLjQ5MyAxNyA5QTcgNyAwIDEgMCAzIDljMCAzLjQ5MiAxLjY5OCA1Ljk4OCAzLjM1NSA3LjU4NGExMy43IDEzLjcgMCAwIDAgMi4yNzMgMS43NjVhMTIgMTIgMCAwIDAgLjk3Ni41NDRsLjA2Mi4wMjlsLjAxOC4wMDh6TTEwIDExLjI1YTIuMjUgMi4yNSAwIDEgMCAwLTQuNWEyLjI1IDIuMjUgMCAwIDAgMCA0LjUiIGNsaXAtcnVsZT0iZXZlbm9kZCIvPjwvc3ZnPg=="
    Dim greenIcon = "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0MCIgaGVpZ2h0PSI0MCIgdmlld0JveD0iMCAwIDIwIDIwIj48cGF0aCBmaWxsPSIjMDVjMjM0IiBmaWxsLXJ1bGU9ImV2ZW5vZGQiIGQ9Im05LjY5IDE4LjkzM2wuMDAzLjAwMUM5Ljg5IDE5LjAyIDEwIDE5IDEwIDE5cy4xMS4wMi4zMDgtLjA2NmwuMDAyLS4wMDFsLjAwNi0uMDAzbC4wMTgtLjAwOGE2IDYgMCAwIDAgLjI4MS0uMTRjLjE4Ni0uMDk2LjQ0Ni0uMjQuNzU3LS40MzNjLjYyLS4zODQgMS40NDUtLjk2NiAyLjI3NC0xLjc2NUMxNS4zMDIgMTQuOTg4IDE3IDEyLjQ5MyAxNyA5QTcgNyAwIDEgMCAzIDljMCAzLjQ5MiAxLjY5OCA1Ljk4OCAzLjM1NSA3LjU4NGExMy43IDEzLjcgMCAwIDAgMi4yNzMgMS43NjVhMTIgMTIgMCAwIDAgLjk3Ni41NDRsLjA2Mi4wMjlsLjAxOC4wMDh6TTEwIDExLjI1YTIuMjUgMi4yNSAwIDEgMCAwLTQuNWEyLjI1IDIuMjUgMCAwIDAgMCA0LjUiIGNsaXAtcnVsZT0iZXZlbm9kZCIvPjwvc3ZnPg=="
    Dim purpleIcon = "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0MCIgaGVpZ2h0PSI0MCIgdmlld0JveD0iMCAwIDIwIDIwIj48cGF0aCBmaWxsPSIjN2QwOWRjIiBmaWxsLXJ1bGU9ImV2ZW5vZGQiIGQ9Im05LjY5IDE4LjkzM2wuMDAzLjAwMUM5Ljg5IDE5LjAyIDEwIDE5IDEwIDE5cy4xMS4wMi4zMDgtLjA2NmwuMDAyLS4wMDFsLjAwNi0uMDAzbC4wMTgtLjAwOGE2IDYgMCAwIDAgLjI4MS0uMTRjLjE4Ni0uMDk2LjQ0Ni0uMjQuNzU3LS40MzNjLjYyLS4zODQgMS40NDUtLjk2NiAyLjI3NC0xLjc2NUMxNS4zMDIgMTQuOTg4IDE3IDEyLjQ5MyAxNyA5QTcgNyAwIDEgMCAzIDljMCAzLjQ5MiAxLjY5OCA1Ljk4OCAzLjM1NSA3LjU4NGExMy43IDEzLjcgMCAwIDAgMi4yNzMgMS43NjVhMTIgMTIgMCAwIDAgLjk3Ni41NDRsLjA2Mi4wMjlsLjAxOC4wMDh6TTEwIDExLjI1YTIuMjUgMi4yNSAwIDEgMCAwLTQuNWEyLjI1IDIuMjUgMCAwIDAgMCA0LjUiIGNsaXAtcnVsZT0iZXZlbm9kZCIvPjwvc3ZnPg=="

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
            Select Case Session("GVDetalle").DefaultView.Item(i).Item("Codigo")
                Case "NO COBRO"
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Red, Color.White, Color.Chocolate)
                    Icon = redIcon
                Case "SUPERVISION"
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Blue, Color.White, Color.Chocolate)
                    Icon = blueIcon
                Case "NO SUPERVISION"
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Red, Color.White, Color.Chocolate)
                    Icon = yellowIcon
                Case Else
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Green, Color.White, Color.Chocolate)
                    Icon = greenIcon
            End Select

            Coordenadas = New GLatLng(Session("GVDetalle").DefaultView.Item(i).Item("LATITUD"), Session("GVDetalle").DefaultView.Item(i).Item("LONGITUD"))
            Puntos.Add(Coordenadas)
            Dim m = Pin.ToString()

            Dim n = Pin.Shadow()
            Marca = New GMarker(Coordenadas, New GMarkerOptions(New GIcon(Icon, shadow)))
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
            Select Case Session("GVDetalle").DefaultView.Item(i).Item("Codigo")
                Case "PROSPECTO"
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Green, Color.White, Color.Chocolate)
                    icon = greenIcon
                Case "NO VENTA"
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Purple, Color.White, Color.Chocolate)
                    icon = purpleIcon

                Case Else
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Red, Color.White, Color.Chocolate)
                    icon = redIcon
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