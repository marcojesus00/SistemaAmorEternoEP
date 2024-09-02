Imports System.Threading.Tasks

Public Class MapPage
    Inherits System.Web.UI.Page
    Dim redIcon As New IconGenerator2("#FF0000", "black")
    Dim blueIcon As New IconGenerator2("#0000FF", "black")
    Protected Async Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            Dim startTime As DateTime = DateTime.Now
            Debug.WriteLine("Start time: " & startTime.ToString())
            Dim mapTitle As String = ""
            Dim ProductionBasePath As String = ConfigurationManager.AppSettings("ProductionBasePath")


            If Session("MarkersData") IsNot Nothing Then

                Dim sessionStartTime As DateTime = DateTime.Now
                Dim dataForMaps = TryCast(Session("MarkersData"), DataForMapGenerator)
                Dim sessionEndTime As DateTime = DateTime.Now
                Debug.WriteLine("Session handling time: " & (sessionEndTime - sessionStartTime).TotalMilliseconds & " ms")

                mapTitle = dataForMaps.Title
                Dim title As New LiteralControl($"<h2 class=""display-6 fw-bold text-center text-lg-start text-sm text-md"">{dataForMaps.Title}</h2>")
                MapTitleHolder.Controls.Add(title)
                Dim hasToBeBeClustered = dataForMaps.ValidMarkers.Count > 90 ' (dataForMaps.ValidMarkers.Count > 250 AndAlso dataForMaps.TraceLine = False) OrElse (dataForMaps.ValidMarkers.Count > 400)
                If hasToBeBeClustered Then
                    Await ProcessClusteredMap(dataForMaps)
                Else
                    Await ProcessMap(dataForMaps)
                End If

                AlertHelper.GenerateAlert("warning", dataForMaps.CountOfCorruptItems.ToString() & " coordenadas corruptas.", alertPlaceholder)
            Else
                'Response.Redirect("~/Principal.aspx")
            End If
            Dim endTime As DateTime = DateTime.Now

            Debug.WriteLine("End time: " & endTime.ToString())
            Debug.WriteLine("Total Page Load Time: " & (endTime - startTime).TotalMilliseconds & " ms")

        Catch ex As Exception
            AlertHelper.GenerateAlert("danger", "Algo inesperado sucedió: " & ex.Message, alertPlaceholder)
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

        End Try

    End Sub
    Private Async Function ProcessMap(dataForMaps) As Task
        Dim dataProcessingStartTime As DateTime = DateTime.Now

        Dim items As List(Of MarkerForMap) = dataForMaps.ValidMarkers

        Dim script As String = "<script>" & vbCrLf &
              "var map = L.map('mapContainer').setView([" & dataForMaps.SetViewlLat & ", " & dataForMaps.SetViewlLong & "], 13);" & vbCrLf &
              "L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {" & vbCrLf &
              "  attribution: 'Map data © <a href=""https://openstreetmap.org"">OpenStreetMap</a> contributors'," & vbCrLf &
              "  maxZoom: 25," & vbCrLf &
              "}).addTo(map);" & vbCrLf &
              "var markers = [];" & vbCrLf
        script &= "var linePoints = [];"
        If items.Count < 1 Then
            AlertHelper.GenerateAlert("warning", "No se encontraron registros para mostrar en el mapa.", alertPlaceholder)

        Else

            For i As Integer = 0 To items.Count - 1
                Dim count = (i + 1)
                redIcon.TextContent = count.ToString
                Dim icon = redIcon.GetBase64UriIcon()
                Dim tooltipMsg As String = items(i).TooltipMessage
                If items(i).MarkerType = MarkerTypes.Cobro Then
                    blueIcon.TextContent = count.ToString
                    icon = blueIcon.GetBase64UriIcon()
                End If
                script &= "var myIcon = L.icon({
                        iconUrl: '" & icon & "',
                        iconSize: [38, 38],
                        iconAnchor: [19, 38],
                        popupAnchor: [-3, -76],
                        shadowSize: [44, 44],
                        shadowAnchor: [22, 44]
                    });" & vbCrLf
                If dataForMaps.TraceLine = True Then
                    script &= "linePoints.push([" & items(i).Latitud & ", " & items(i).Longitud & "]);"
                End If
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
        End If
        Dim dataProcessingEndTime As DateTime = DateTime.Now
        Debug.WriteLine("Data processing time: " & (dataProcessingEndTime - dataProcessingStartTime).TotalMilliseconds & " ms")

    End Function
    Private Async Function ProcessClusteredMap(dataForMaps) As Task
        Dim dataProcessingStartTime As DateTime = DateTime.Now

        Dim items As List(Of MarkerForMap) = dataForMaps.ValidMarkers
        Dim initMarkerListScript = "<script> var markerList = []; </script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "initMarkerList", initMarkerListScript)

        Dim script As String = "<script>" & vbCrLf &
              "var tiles = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {" & vbCrLf &
              "  attribution: 'Map data © <a href=""https://openstreetmap.org"">OpenStreetMap</a> contributors'," & vbCrLf &
              "  maxZoom: 18" & vbCrLf &
              "}), 			latlng = L.latLng(" & dataForMaps.SetViewlLat & ", " & dataForMaps.SetViewlLong & " );" & vbCrLf &
              "var map = L.map('mapContainer', { center: latlng, zoom: 13, layers: [tiles] });" & vbCrLf

        script &= "var markers = L.markerClusterGroup({ chunkedLoading: true });" & vbCrLf
        script &= "" & vbCrLf


        If items.Count < 1 Then
            AlertHelper.GenerateAlert("warning", "No se encontraron registros para mostrar en el mapa.", alertPlaceholder)

        Else
            ' Batch processing markers asynchronously
            Dim batchSize As Integer = 1000 ' Adjust batch size as needed
            Await Task.Run(Sub()
                               For i As Integer = 0 To items.Count - 1 Step batchSize
                                   Dim batch = items.Skip(i).Take(batchSize)
                                   'script &= "markerList = [];" & vbCrLf
                                   Dim batchScript = "markerList = [];"
                                   For Each item In batch

                                       batchScript &= "var marker = L.marker([" & item.Latitud & ", " & item.Longitud & "]).bindTooltip('" & item.TooltipMessage & "');" & vbCrLf
                                       batchScript &= "markerList.push(marker);" & vbCrLf
                                   Next
                                   script &= batchScript
                                   script &= "markers.addLayers(markerList);" & vbCrLf
                                   ClientScript.RegisterStartupScript(Me.GetType(), "updateMarkers_" & i, batchScript, True)

                               Next

                           End Sub)
            script &= "map.addLayer(markers);" & vbCrLf &
          "</script>"


            ClientScript.RegisterStartupScript(Me.GetType(), "initializeMap", script)
        End If
        Dim dataProcessingEndTime As DateTime = DateTime.Now
        Debug.WriteLine("Data processing time: " & (dataProcessingEndTime - dataProcessingStartTime).TotalMilliseconds & " ms")

    End Function


    '    'First is necesary to improve the dashboard performance and robustness when dealing with 400,000 markers
    '    Private Async Sub ProcessClusteredMapImproved(dataForMaps)
    '        Dim dataProcessingStartTime As DateTime = DateTime.Now

    '        Dim items As List(Of MarkerForMap) = dataForMaps.ValidMarkers

    '        ' Initial map setup script (send once)
    '        Dim initScript As String = "<script>" & vbCrLf &
    '        "var tiles = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {" & vbCrLf &
    '        " attribution: 'Map data © <a href=""https://openstreetmap.org"">OpenStreetMap</a> contributors'," & vbCrLf &
    '        " maxZoom: 18," & vbCrLf &
    '        "}), 			latlng = L.latLng(" & dataForMaps.SetViewlLat & ", " & dataForMaps.SetViewlLong & " );" & vbCrLf &
    '        "var map = L.map('mapContainer', { center: latlng, zoom: 13, layers: [tiles] });" & vbCrLf &
    '        "var progress = document.getElementById('progress');" & vbCrLf &
    '        "var progressBar = document.getElementById('progress-bar');" & vbCrLf &
    '        "function updateProgressBar(processed, total, elapsed, layersArray) {" & vbCrLf &
    '        "    if (elapsed > 1000) { " & vbCrLf &
    '        "       progress.style.display = 'block';" & vbCrLf &
    '        "       progressBar.style.width = Math.round(processed/total*100) + '%';" & vbCrLf &
    '        "    }" & vbCrLf &
    '        "    if (processed === total) { " & vbCrLf &
    '        "       progress.style.display = 'none';" & vbCrLf &
    '        "    }" & vbCrLf &
    '        "}" & vbCrLf &
    '        "var markers = L.markerClusterGroup({ chunkedLoading: true, chunkProgress: updateProgressBar });" & vbCrLf &
    '        "</script>"

    '        ClientScript.RegisterStartupScript(Me.GetType(), "initializeMap", initScript, True) ' Send init script 

    '        If items.Count < 1 Then
    '            AlertHelper.GenerateAlert("warning", "No se encontraron registros para mostrar en el mapa.", alertPlaceholder)

    '        Else
    '            ' Batch processing markers asynchronously
    '            Dim batchSize As Integer = 1000 ' Adjust batch size as needed
    '            Await Task.Run(Sub()
    '                               For i As Integer = 0 To items.Count - 1 Step batchSize
    '                                   Dim batch = items.Skip(i).Take(batchSize)

    '                                   ' Generate JavaScript for the current batch
    '                                   Dim batchScript As String = "markerList = [];" & vbCrLf ' Initialize for each batch
    '                                   For Each item In batch
    '                                       batchScript &= "var marker = L.marker([" & item.Latitud & ", " & item.Longitud & "]).bindTooltip('" & item.TooltipMessage & "');" & vbCrLf
    '                                       batchScript &= "markerList.push(marker);" & vbCrLf
    '                                   Next
    '                                   batchScript &= "markers.addLayers(markerList);" & vbCrLf

    '                                   ' Send the batch script directly
    '                                   ClientScript.RegisterStartupScript(Me.GetType(), "updateMarkers_" & i, batchScript, True)
    '                               Next
    '                           End Sub)
    '        End If

    '        Dim dataProcessingEndTime As DateTime = DateTime.Now
    '        Debug.WriteLine("Data processing time: " & (dataProcessingEndTime - dataProcessingStartTime).TotalMilliseconds & " ms")

    '    End Sub


End Class