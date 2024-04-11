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

            Select Case Session("GVDetalle").DefaultView.Item(i).Item("Codigo")
                Case "NO COBRO"
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Red, Color.White, Color.Chocolate)
                Case "SUPERVISION"
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Blue, Color.White, Color.Chocolate)
                Case "NO SUPERVISION"
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Red, Color.White, Color.Chocolate)
                Case Else
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Green, Color.White, Color.Chocolate)
            End Select

            Coordenadas = New GLatLng(Session("GVDetalle").DefaultView.Item(i).Item("LATITUD"), Session("GVDetalle").DefaultView.Item(i).Item("LONGITUD"))
            Puntos.Add(Coordenadas)

            Marca = New GMarker(Coordenadas, New GMarkerOptions(New GIcon(Pin.ToString(), Pin.Shadow())))
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

            Select Case Session("GVDetalle").DefaultView.Item(i).Item("Codigo")
                Case "PROSPECTO"
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Green, Color.White, Color.Chocolate)
                Case "NO VENTA"
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Purple, Color.White, Color.Chocolate)
                Case Else
                    Pin = New XPinLetter(PinShapes.pin, (i + 1).ToString, Color.Red, Color.White, Color.Chocolate)
            End Select

            Coordenadas = New GLatLng(Session("GVDetalle").DefaultView.Item(i).Item("LATITUD"), Session("GVDetalle").DefaultView.Item(i).Item("LONGITUD"))
            Puntos.Add(Coordenadas)

            Marca = New GMarker(Coordenadas, New GMarkerOptions(New GIcon(Pin.ToString(), Pin.Shadow())))
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