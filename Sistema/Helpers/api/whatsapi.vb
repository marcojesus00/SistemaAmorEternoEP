Imports CrystalDecisions.Shared
Imports System.IO
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Web.Script.Services
Imports System.Net.Mail
Imports System.Net
Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json


Public Class whatsapi
    Public Class ResponseObject
        Public Property Success As Boolean
        Public Property Data As Object
        Public Property Errors As List(Of ErrorObject)
    End Class

    Public Class ErrorObject
        Public Property Type As String
        Public Property Loc As Object
        Public Property Msg As String
        Public Property Code As String
        Public Property StatusCode As Integer
    End Class
    Private Function PostData(ByVal url As String, ByVal data As Dictionary(Of String, Object)) As HttpResponseMessage
        Using client As New HttpClient()
            client.DefaultRequestHeaders.Accept.Add(New System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"))
            Dim s
            ' Convert the dictionary to JSON
            Dim json As String = JsonConvert.SerializeObject(data)
            Dim content As New StringContent(json, Encoding.UTF8, "application/json")

            Dim response1 As HttpResponseMessage = client.PostAsync(url, content).Result


            Return response1



        End Using
    End Function

    Public Shared Function sendWhatsAppDocs(doc, name, couentryCode, localNumber, caption)


        ' Exportar el informe a PDF y guardar en la ruta especificada con el nombre del archivo
        'Informe.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaCompletaArchivo)
        Using memoryStream As New System.IO.MemoryStream()
            ' Export the report to the memory stream as a PDF
            doc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat).CopyTo(memoryStream)

            ' Convert the memory stream to a byte array
            Dim reportBytes As Byte() = memoryStream.ToArray()

            ' Convert the byte array to a base64 string
            Dim base64String As String = Convert.ToBase64String(reportBytes)




            'Dim url As String = "http://localhost:8002/v1/messages/docs/"
            'url = "https://whatsapi-vlvp.onrender.com/v1/messages/docs/"
            Dim url = "http://192.168.20.75:8000/v1/messages/docs/"
            Dim phoneNumber As New Dictionary(Of String, String) From {
                {"country_code", couentryCode},
                {"local_number", localNumber}
            }

            Dim data As New Dictionary(Of String, Object) From {
                {"phone_number", phoneNumber},
                {"url", base64String},
                {"file_name", name},
                {"caption", caption},
                {"priority", 10},
                {"referenceID", ""}
            }

            ' Send the POST request
            Dim response1 = PostData(url, data)
            'Response.Write(rapiRsponse)
            Dim contentStr As StreamContent = response1.Content
            Dim contentString As String = contentStr.ReadAsStringAsync().Result
            Dim serializer As New Newtonsoft.Json.JsonSerializer
            Dim jsonReader As New Newtonsoft.Json.JsonTextReader(New StringReader(contentString))
            Dim jsonResponse As ResponseObject = serializer.Deserialize(Of ResponseObject)(jsonReader)
            If response1.IsSuccessStatusCode Then
                Dim msg = "WhatsApp enviado con éxito"
                Dim result As New ResultW(True, msg)
                Return result
                'Return response1.Content.ReadAsStringAsync().Result

            Else
                Dim msg = ""
                If jsonResponse.Errors(0) IsNot Nothing Then
                    msg = jsonResponse.Errors(0).Msg
                Else
                    msg = $"{response1.StatusCode} {response1.ReasonPhrase}"
                End If
                msg = $"Error, intente de nuevo: " + msg

                Return $"Error: {response1.StatusCode} - {response1.ReasonPhrase}"
                Dim result As New ResultW(False, msg)
                Return result
            End If


        End Using



    End Function
    Private Class ResultW
        Private Success As Boolean
        Private Msg As String
        Public Sub New(success, msg)
            Me.Success = success
            Me.Msg = msg
        End Sub

    End Class
End Class
