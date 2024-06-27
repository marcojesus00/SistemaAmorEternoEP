Public Class MapIframe
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub


    'Private Sub InitializeMap()
    '    Dim mapScript As New StringBuilder

    '    ' Define center coordinates (replace with your desired location)
    '    Dim centerLat As Double = 40.7128
    '    Dim centerLong As Double = -74.0059

    '    ' Set initial zoom level
    '    Dim zoomLevel As Integer = 13
    '    Dim url = "https: //www.openstreetmap.org/copyright\"
    '    mapScript.AppendLine("var map = L.map('mapContainer').setView([" & centerLat & "," & centerLong & "]," & zoomLevel & ");")
    '    ' Add your map tiles here (e.g., OpenStreetMap)
    '    mapScript.AppendLine("L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { attribution: '&copy; <a href=\" & url & ">OpenStreetMap</a> contributors' }).addTo(map);")

    '    ' Call the script from your code
    '    Page.ClientScript.RegisterStartupScript(GetType(Me), "mapScript", mapScript.ToString(), True, False)
    'End Sub


End Class