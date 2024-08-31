Public Class IconGenerator2
    Private _color As String
    Private _textColor As String
    Private _textContent As String
    Public Property Color As String
        Get
            Return _color
        End Get
        Set(value As String)
            _color = value
        End Set
    End Property

    Public Property TextColor As String
        Get
            Return _textColor
        End Get
        Set(value As String)
            _textColor = value
        End Set
    End Property

    Public Property TextContent As String
        Get
            Return _textContent
        End Get
        Set(value As String)
            _textContent = value
        End Set
    End Property
    Public Sub New(ByVal pathColor As String, Optional ByVal textColorName As String = "white")
        Me.Color = pathColor
        Me.TextColor = textColorName
        Me.TextContent = ""
    End Sub

    Public Function GetBase64UriIcon()
        Dim svgCode As String = "<svg xmlns=""http://www.w3.org/2000/svg"" width=""40"" height=""40"" viewBox=""0 0 20 20"">" &
                    "<path fill=""" & Color & """ fill-rule=""evenodd"" d=""m9.69 18.933 0.003 0.001c0.197 0.086 0.308 0.066 0.308 0.066s0.11 0.02 0.308 -0.066l0.002 -0.001 0.006 -0.003 0.018 -0.008a6.222 6.222 0 0 0 0.281 -0.14c0.186 -0.096 0.446 -0.24 0.757 -0.433a13.689 13.689 0 0 0 2.274 -1.765c1.657 -1.596 3.355 -4.091 3.355 -7.584a7 7 0 1 0 -14 0c0 3.492 1.698 5.988 3.355 7.584a13.689 13.689 0 0 0 2.273 1.765 12.444 12.444 0 0 0 0.976 0.544l0.062 0.029 0.018 0.008zm0.31 -7.684a2.25 2.25 0 1 0 0 -4.5 2.25 2.25 0 0 0 0 4.5"" clip-rule=""evenodd""/>" &
                    "<text x=""20"" y=""19"" font-size=""8"" fill=""" & TextColor & """ text-anchor=""end"" font-weight=""bold"">" & TextContent & "</text></svg>"
        Dim bytes As Byte() = Encoding.UTF8.GetBytes(svgCode)

        ' Convert the byte array to a base64 string
        Dim base64String As String = Convert.ToBase64String(bytes)
        base64String = "data:image/svg+xml;base64," & base64String
        ' Return the base64 string

        Return base64String
    End Function
End Class
