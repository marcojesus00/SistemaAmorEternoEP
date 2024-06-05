Public Class CobrosMap
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim script As String = "
            <script>
                var map = L.map('mapContainer').setView([15.400, -87.80], 13);
                L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                    attribution: 'Map data © <a href=""https://openstreetmap.org"">OpenStreetMap</a> contributors',
                    maxZoom: 18,
                }).addTo(map);
  var marker = L.marker([15.400, -87.80]).addTo(map);
  marker.bindPopup(""<b>Hello world!</b><br />This is a popup."").openPopup();
            </script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "initializeMap", script)
        End If
    End Sub


End Class