Imports Newtonsoft.Json

Public Class CobrosMap
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim scriptIcos As String = "
            <script>


              </script>"

        If Request.QueryString("items") IsNot Nothing Then
            Dim serializedList As String = Request.QueryString("items")
            Dim items As List(Of TransactionMapDto) = JsonConvert.DeserializeObject(Of List(Of TransactionMapDto))(Server.UrlDecode(serializedList))
            ' Use the items list here

            Dim script As String = "<script>" & vbCrLf &
                      "var map = L.map('mapContainer').setView([15.400, -87.80], 13);" & vbCrLf &
                      "L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {" & vbCrLf &
                      "  attribution: 'Map data © <a href=""https://openstreetmap.org"">OpenStreetMap</a> contributors'," & vbCrLf &
                      "  maxZoom: 25," & vbCrLf &
                      "}).addTo(map);" & vbCrLf &
                      "var markers = [];" & vbCrLf
            script &= "var linePoints = [];"
            For i As Integer = 0 To items.Count - 1
                Dim icon = "map.png"
                Dim tooltipMsg As String = items(i).NombreDelCliente & " " & items(i).CodigoDelCLiente & " " & items(0).Fecha & " " & items(0).Hora
                If items(i).TipoDeTransaccion = "cobro" Then
                    icon = "moneyIcon.png"
                    tooltipMsg = "L" & items(i).Cantidad & " a " & tooltipMsg

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
        script &= "marker.bindTooltip('" & tooltipMsg & "').openTooltip();" & vbCrLf
                script &= "markers.push(marker);" & vbCrLf
            Next
            script &= "var line = L.polyline(linePoints, {
  color: 'red', 
  weight: 2
});" & vbCrLf
            script &= "line.addTo(map);" & vbCrLf
            script &= "</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "initializeMap", script)
        Else
            Response.Redirect("CobrosDashboard.aspx")
        End If
        If Not IsPostBack Then
            Dim script As String = "
            <script>
                var map = L.map('mapContainer').setView([15.400, -87.80], 20);
                L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                    attribution: 'Map data © <a href=""https://openstreetmap.org"">OpenStreetMap</a> contributors',
                    maxZoom: 25,
                }).addTo(map);
  var marker = L.marker([15.400, -87.80]).addTo(map);
  marker.bindPopup(""<b>Hello world!</b><br />This is a popup."").openPopup();
            </script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "initializeMap", script)
        End If
    End Sub


End Class