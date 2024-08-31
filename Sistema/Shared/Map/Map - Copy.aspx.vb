'Public Class MapPage
'    Inherits System.Web.UI.Page

'    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
'        Try
'            Dim mapTitle As String = ""
'            Dim ProductionBasePath As String = ConfigurationManager.AppSettings("ProductionBasePath")

'            Dim redIcon As New IconGenerator2("#FF0000", "black")
'            Dim blueIcon As New IconGenerator2("#0000FF", "black")
'            If Session("MarkersData") IsNot Nothing Then

'                Dim dataForMaps = TryCast(Session("MarkersData"), DataForMapGenerator)
'                Dim items As List(Of MarkerForMap) = dataForMaps.ValidMarkers
'                mapTitle = dataForMaps.Title
'                Dim title As New LiteralControl($"<h2 class""display-6 fw-bold text-center text-lg-start text-sm text-md"">{dataForMaps.Title}</h2>")
'                MapTitleHolder.Controls.Add(title)
'                Dim clustered As Boolean = False
'                If items.Count > 1 Then
'                    clustered = True
'                End If

'                Dim script As String = "<script>" & vbCrLf &
'                      "var tiles = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {" & vbCrLf &
'                      "  attribution: 'Map data © <a href=""https://openstreetmap.org"">OpenStreetMap</a> contributors'," & vbCrLf &
'                      "  maxZoom: 18," & vbCrLf &
'                      "}), latlng(-37.79,175.27);" & vbCrLf &
'                      "var map = L.map('map', { center: latlng, zoom: 13, layers: [tiles] });" & vbCrLf

'                If clustered Then
'                    script &= "var markers = L.markerClusterGroup({ chunkedLoading: true, chunkProgress: updateProgressBar });" & vbCrLf
'                    script &= "		var markerList = [];" & vbCrLf
'                    script &= "console.log('start creating markers: ' + window.performance.now());" & vbCrLf
'                Else
'                    script &= "var markers = [];"
'                End If
'                script &= "var linePoints = [];"
'                If items.Count < 1 Then
'                    AlertHelper.GenerateAlert("warning", "No se encontraron registros para mostrar en el mapa.", alertPlaceholder)

'                Else

'                    For i As Integer = 0 To items.Count - 1
'                        Dim count = (i + 1)
'                        redIcon.TextContent = count.ToString
'                        Dim icon = redIcon.GetBase64UriIcon()
'                        Dim tooltipMsg As String = items(i).TooltipMessage
'                        If items(i).MarkerType = MarkerTypes.Cobro Then
'                            blueIcon.TextContent = count.ToString
'                            icon = blueIcon.GetBase64UriIcon()
'                        End If
'                        script &= "var myIcon = L.icon({
'                        iconUrl: '" & icon & "',
'                        iconSize: [38, 38],
'                        iconAnchor: [19, 38],
'                        popupAnchor: [-3, -76],
'                        shadowUrl: '/imagenes/circleShadow.png',
'                        shadowSize: [44, 44],
'                        shadowAnchor: [22, 44]
'                    });" & vbCrLf
'                        script &= "linePoints.push([" & items(i).Latitud & ", " & items(i).Longitud & "]);"
'                        If clustered Then
'                            script &= "var marker = L.marker([" & items(i).Latitud & ", " & items(i).Longitud & "]);" & vbCrLf

'                        Else
'                            script &= "var marker = L.marker([" & items(i).Latitud & ", " & items(i).Longitud & "], {icon: myIcon}).addTo(map);" & vbCrLf

'                        End If
'                        script &= "marker.bindTooltip('" & tooltipMsg & "');" & vbCrLf
'                        If clustered Then
'                            script &= "markerList.push(marker);" & vbCrLf

'                            'script &= "markers.addLayer(marker);" & vbCrLf
'                        Else
'                            script &= "markers.push(marker);" & vbCrLf

'                        End If

'                        If clustered Then
'                            script &= "console.log('start clustering: ' + window.performance.now());" & vbCrLf

'                            script &= "markers.addLayers(markerList);" & vbCrLf
'                            script &= "map.addLayer(markers);" & vbCrLf
'                            script &= "console.log('end clustering: ' + window.performance.now());" & vbCrLf

'                        End If
'                    Next
'                    If dataForMaps.TraceLine = True Then
'                        script &= "var line = L.polyline(linePoints, {
'                      color: 'red', 
'                      weight: 2
'                    });" & vbCrLf
'                        script &= "line.addTo(map);" & vbCrLf
'                    End If

'                    script &= "</script>"
'                    ClientScript.RegisterStartupScript(Me.GetType(), "initializeMap", script)
'                    AlertHelper.GenerateAlert("warning", dataForMaps.CountOfCorruptItems.ToString() & " coordenadas corruptas.", alertPlaceholder)
'                End If
'            Else
'                Response.Redirect("~/Principal.aspx")
'            End If

'        Catch ex As Exception
'            AlertHelper.GenerateAlert("danger", "Algo inesperado sucedió: " & ex.Message, alertPlaceholder)
'        End Try

'    End Sub


'End Class