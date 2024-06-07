Public Class MarkerForMap
    Public Property MarkerType As MarkerTypes = MarkerTypes.otro

    Public Property TooltipMessage As String = ""

    Public Property Latitud As String
    Public Property Longitud As String


End Class
Public Enum MarkerTypes
    Cobro
    Venta
    Visita
    Verde
    Cliente
    otro
End Enum

Public Class DataForMapGenerator
    Private _title As String = " "
    Private _validMarkers As List(Of MarkerForMap)
    Private _countOfCorruptItems As Integer = 0
    Private _traceLine As Boolean = False
    Private _setViewZoom As String = "15"
    Private _setViewLong As Double = 15.39304
    Private _setViewlLat As Double = -87.81448390




    Public ReadOnly Property Title As String
        Get
            Return _title
        End Get
    End Property

    Public ReadOnly Property ValidMarkers As List(Of MarkerForMap)
        Get
            Return _validMarkers
        End Get
    End Property

    Public ReadOnly Property CountOfCorruptItems As Integer
        Get
            Return _countOfCorruptItems
        End Get
    End Property

    Public ReadOnly Property TraceLine As Boolean
        Get
            Return _traceLine
        End Get
    End Property
    Public ReadOnly Property SetViewlLat As String
        Get
            Return _setViewlLat
        End Get
    End Property
    Public ReadOnly Property SetViewlLong As String
        Get
            Return _setViewLong
        End Get
    End Property
    Public ReadOnly Property SetViewZoom As String
        Get
            Return _setViewZoom
        End Get
    End Property

    Public Sub New(title As String, markers As List(Of MarkerForMap), Optional traceLine As Boolean = False)
        _title = title
        _traceLine = traceLine
        SetValidMakerAndCentroid(markers)
    End Sub

    Private Function SetValidMakerAndCentroid(markers As List(Of MarkerForMap)) As List(Of MarkerForMap)
        Dim latitude As Double
        Dim longitude As Double
        Dim validMarkers As New List(Of MarkerForMap)
        Dim locations As New List(Of Point)


        For Each marker In markers
            Dim LatToDouble As Boolean = Double.TryParse(marker.Latitud, latitude)
            Dim LongToDouble As Boolean = Double.TryParse(marker.Longitud, longitude)

            If (LatToDouble AndAlso LongToDouble) Then
                If latitude <> 0.0 And longitude <> 0.0 Then
                    validMarkers.Add(marker)
                    Dim location As New Point(latitude, longitude)
                    locations.Add(location)
                Else
                    _countOfCorruptItems += 1

                End If

            Else
                _countOfCorruptItems += 1
            End If
        Next
        Dim centorid As Point = CalculateCentroid(locations)
        _validMarkers = validMarkers
        _setViewlLat = centorid.Latitude
        _setViewLong = centorid.Longitude
        Return validMarkers
    End Function
    Public Function CalculateCentroid(points As List(Of Point)) As Point
        Dim totalLat As Double = 0
        Dim totalLon As Double = 0

        For Each point In points
            totalLat += point.Latitude
            totalLon += point.Longitude
        Next

        Dim centroidLat As Double = totalLat / points.Count
        Dim centroidLon As Double = totalLon / points.Count
        Dim centroid As New Point(centroidLat, centroidLon)
        Return centroid
    End Function

End Class
Public Class Point
    Private _lat As Double
    Private _long As String

    Public Sub New(latitude As Double, longitude As Double)
        Me._lat = latitude
        Me._long = longitude
    End Sub
    Public ReadOnly Property Latitude As Double
        Get
            Return _lat
        End Get
    End Property
    Public ReadOnly Property Longitude As Double
        Get
            Return _long
        End Get
    End Property
End Class
