Imports System.Net.Http
Imports System.Text
Imports System.Threading.Tasks
Imports Newtonsoft.Json
Imports System.IO

Public Class ApiService
    Public Shared Function PostHtmlAndReceivePdf(ByVal htmlContent As String) As Byte()
        Dim requestBody As New With {Key .html_content = htmlContent}
        Dim jsonBody As String = JsonConvert.SerializeObject(requestBody)

        Using client As New HttpClient()
            client.DefaultRequestHeaders.Accept.Clear()
            client.DefaultRequestHeaders.Accept.Add(New System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"))

            Dim content As New StringContent(jsonBody, Encoding.UTF8, "application/json")

            ' Replace the URL with your API endpoint that returns the PDF
            Dim response As HttpResponseMessage = client.PostAsync("http://192.168.20.75:8000/v1/helpers/html-to-pdf/", content).Result

            If response.IsSuccessStatusCode Then
                ' Read the response content as a byte array (assuming the response is the PDF file)
                Dim pdfBytes As Byte() = response.Content.ReadAsByteArrayAsync().Result

                ' Write the byte array to a file (PDF)
                Return pdfBytes

            Else

                Return New Byte() {}
                Console.WriteLine("Failed to receive PDF. Status code: " & response.StatusCode)
            End If
        End Using
    End Function
End Class
