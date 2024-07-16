


Imports System.Threading.Tasks

Public Class MapClusteredPage
    Inherits System.Web.UI.Page

    Protected Async Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            Dim startTime As DateTime = DateTime.Now
            Debug.WriteLine("Start time: " & startTime.ToString())

            Dim mapTitle As String = ""
            Dim ProductionBasePath As String = ConfigurationManager.AppSettings("ProductionBasePath")

            Dim redIcon As New IconGenerator2("#FF0000", "black")
            Dim blueIcon As New IconGenerator2("#0000FF", "black")
            If Session("MarkersData") IsNot Nothing Then

                Dim sessionStartTime As DateTime = DateTime.Now
                Dim dataForMaps = TryCast(Session("MarkersData"), DataForMapGenerator)
                Dim sessionEndTime As DateTime = DateTime.Now
                Debug.WriteLine("Session handling time: " & (sessionEndTime - sessionStartTime).TotalMilliseconds & " ms")

                Dim items As List(Of MarkerForMap) = dataForMaps.ValidMarkers
                mapTitle = dataForMaps.Title
                Dim title As New LiteralControl($"<h2 class""display-6 fw-bold text-center text-lg-start text-sm text-md"">{dataForMaps.Title}</h2>")
                'MapTitleHolder.Controls.Add(title)
                Dim clustered As Boolean = False
                If items.Count > 1 Then
                    clustered = True
                End If

                Dim script As String = "<script>" & vbCrLf &
                      "var tiles = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {" & vbCrLf &
                      "  attribution: 'Map data © <a href=""https://openstreetmap.org"">OpenStreetMap</a> contributors'," & vbCrLf &
                      "  maxZoom: 18," & vbCrLf &
                      "}), 			latlng = L.latLng(" & dataForMaps.SetViewlLat & ", " & dataForMaps.SetViewlLong & " );" & vbCrLf &
                      "var map = L.map('mapContainer', { center: latlng, zoom: 13, layers: [tiles] });" & vbCrLf
                script &= "		var progress = document.getElementById('progress');" & vbCrLf
                script &= "var progressBar = document.getElementById('progress-bar');" & vbCrLf
                script &= "		function updateProgressBar(processed, total, elapsed, layersArray) {
                    if (elapsed > 1000) {
                        // if it takes more than a second to load, display the progress bar:
                        progress.style.display = 'block';
                        progressBar.style.width = Math.round(processed/total*100) + '%';
                    }

                    if (processed === total) {
                        // all markers processed - hide the progress bar:
                        progress.style.display = 'none';
                    }
                }" & vbCrLf
                script &= "var markers = L.markerClusterGroup({ chunkedLoading: true, chunkProgress: updateProgressBar });" & vbCrLf
                script &= "var markerList = [];" & vbCrLf
                script &= "console.log('start creating markers: ' + window.performance.now());" & vbCrLf

                Dim dataProcessingStartTime As DateTime = DateTime.Now

                If items.Count < 1 Then
                    AlertHelper.GenerateAlert("warning", "No se encontraron registros para mostrar en el mapa.", alertPlaceholder)

                Else
                    ' Batch processing markers asynchronously
                    Dim batchSize As Integer = 1000 ' Adjust batch size as needed
                    Await Task.Run(Sub()
                                       For i As Integer = 0 To items.Count - 1 Step batchSize
                                           Dim batch = items.Skip(i).Take(batchSize)
                                           'script &= "markerList = [];" & vbCrLf
                                           Dim batchScript = ""
                                           For Each item In batch
                                               batchScript &= "var marker = L.marker([" & item.Latitud & ", " & item.Longitud & "]);" & vbCrLf
                                               batchScript &= "markerList.push(marker);" & vbCrLf
                                           Next
                                           'script &= "markers.addLayers(markerList);" & vbCrLf
                                           script &= "markerList = [];" & vbCrLf
                                           script &= batchScript
                                           script &= "markers.addLayers(markerList);" & vbCrLf
                                           ClientScript.RegisterStartupScript(Me.GetType(), "updateMarkers_" & i, batchScript, True)

                                       Next
                                       'script &= "map.addLayer(markers);" & vbCrLf
                                       'script &= "console.log('end clustering: ' + window.performance.now());" & vbCrLf

                                       'script &= "</script>"
                                   End Sub)
                    script &= "map.addLayer(markers);" & vbCrLf

                    script &= "</script>"

                    ClientScript.RegisterStartupScript(Me.GetType(), "initializeMap", script)
                    AlertHelper.GenerateAlert("warning", dataForMaps.CountOfCorruptItems.ToString() & " coordenadas corruptas.", alertPlaceholder)
                End If

                Dim dataProcessingEndTime As DateTime = DateTime.Now
                Debug.WriteLine("Data processing time: " & (dataProcessingEndTime - dataProcessingStartTime).TotalMilliseconds & " ms")

            Else
                'Context.ApplicationInstance.CompleteRequest()
                'Response.Redirect("~/Principal.aspx")
            End If

            Dim endTime As DateTime = DateTime.Now
            Debug.WriteLine("End time: " & endTime.ToString())
            Debug.WriteLine("Total Page Load Time: " & (endTime - startTime).TotalMilliseconds & " ms")

        Catch ex As Exception
            AlertHelper.GenerateAlert("danger", "Algo inesperado sucedió: " & ex.Message, alertPlaceholder)
        End Try
    End Sub

End Class
