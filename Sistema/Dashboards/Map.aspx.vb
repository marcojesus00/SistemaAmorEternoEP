Public Class MapPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            Dim mapTitle As String = ""


            If Session("MarkersData") IsNot Nothing Then
                'Dim serializedList As String = Request.QueryString("items")
                'Dim items As List(Of MarkerMapDto) = JsonConvert.DeserializeObject(Of List(Of MarkerMapDto))(Server.UrlDecode(serializedList))
                ' Use the items list here
                Dim dataForMaps = TryCast(Session("MarkersData"), DataForMapGenerator)
                Dim items As List(Of MarkerForMap) = dataForMaps.ValidMarkers
                mapTitle = dataForMaps.Title
                Dim title As New LiteralControl($"<h2 class=""mb-4"">{dataForMaps.Title}</h2>")
                MapTitleHolder.Controls.Add(title)
                'If Not IsPostBack Then
                '    Dim scriptInit As String = "
                '<script>
                '        var map = L.map('mapContainer').setView([15.400, -87.80], 20);
                '        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                '            attribution: 'Map data © <a href=""https://openstreetmap.org"">OpenStreetMap</a> contributors',
                '            maxZoom: 25,
                '        }).addTo(map);
                '      var marker = L.marker([15.400, -87.80]).addTo(map);
                '      marker.bindPopup(""<b>Hello world!</b><br />This is a popup."").openPopup();
                '</script>"
                '    ClientScript.RegisterStartupScript(Me.GetType(), "initializeMap", scriptInit)
                'End If

                Dim script As String = "<script>" & vbCrLf &
                      "var map = L.map('mapContainer').setView([" & dataForMaps.SetViewlLat & ", " & dataForMaps.SetViewlLong & "], 13);" & vbCrLf &
                      "L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {" & vbCrLf &
                      "  attribution: 'Map data © <a href=""https://openstreetmap.org"">OpenStreetMap</a> contributors'," & vbCrLf &
                      "  maxZoom: 25," & vbCrLf &
                      "}).addTo(map);" & vbCrLf &
                      "var markers = [];" & vbCrLf
                script &= "var linePoints = [];"
                For i As Integer = 0 To items.Count - 1

                    Dim icon = "map.png"
                    Dim tooltipMsg As String = items(i).TooltipMessage
                    If items(i).MarkerType = MarkerTypes.Cobro Then
                        icon = "moneyIcon.png"
                    End If
                    script &= "var myIcon = L.icon({
                        iconUrl: '/imagenes/" & icon & "',
                        iconSize: [38, 38],
                        iconAnchor: [19, 38],
                        popupAnchor: [-3, -76],
                        shadowUrl: '/imagenes/circleShadow.png',
                        shadowSize: [44, 44],
                        shadowAnchor: [22, 44]
                    });" & vbCrLf
                    script &= "linePoints.push([" & items(i).Latitud & ", " & items(i).Longitud & "]);"
                    script &= "var marker = L.marker([" & items(i).Latitud & ", " & items(i).Longitud & "], {icon: myIcon}).addTo(map);" & vbCrLf
                    script &= "marker.bindTooltip('" & tooltipMsg & "');" & vbCrLf
                    script &= "markers.push(marker);" & vbCrLf


                Next
                If dataForMaps.TraceLine = True Then
                    script &= "var line = L.polyline(linePoints, {
                      color: 'red', 
                      weight: 2
                    });" & vbCrLf
                    script &= "line.addTo(map);" & vbCrLf
                End If

                script &= "</script>"
                ClientScript.RegisterStartupScript(Me.GetType(), "initializeMap", script)
                AlertHelper.GenerateAlert("warning", dataForMaps.CountOfCorruptItems.ToString() & " coordenadas corruptas.", alertPlaceholder)

            Else
                Response.Redirect("Cobros.aspx")
            End If

        Catch ex As Exception
            AlertHelper.GenerateAlert("danger", "Algo inesperado sucedió: " & ex.Message, alertPlaceholder)
        End Try

    End Sub


End Class