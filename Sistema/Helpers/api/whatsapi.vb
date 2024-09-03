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
    Private Shared Function PostData(ByVal url As String, ByVal data As Dictionary(Of String, Object)) As HttpResponseMessage
        Using client As New HttpClient()
            client.DefaultRequestHeaders.Accept.Add(New System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"))
            ' Convert the dictionary to JSON
            Dim json As String = JsonConvert.SerializeObject(data)
            Dim content As New StringContent(json, Encoding.UTF8, "application/json")

            Dim response1 As HttpResponseMessage = client.PostAsync(url, content).Result


            Return response1



        End Using
    End Function
    Shared Function GetHttpResponse(url As String) As String
        Try
            ' Create a new instance of HttpWebRequest
            Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)

            ' Set the method to GET
            request.Method = "GET"

            ' Get the response from the server
            Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                ' Read the response content
                Using reader As New StreamReader(response.GetResponseStream())
                    Return reader.ReadToEnd()
                End Using
            End Using
        Catch ex As Exception
            ' Handle any errors that occurred during the request
            Return $"Error: {ex.Message}"
        End Try
    End Function
    Public Shared Function sendWhatsAppDocs(doc As Object, name As String, couentryCode As String, localNumber As String, caption As String, clientCode As String, user As String, Optional instancia As String = "", Optional docDescription As String = "Estado de cuenta") As ResultW

        Dim msg = ""
        Dim isSuccess As Boolean = False
        Dim IdDeLaPlataforma As Integer = 0
        ' Exportar el informe a PDF y guardar en la ruta especificada con el nombre del archivo
        'Informe.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaCompletaArchivo)
        Using memoryStream As New System.IO.MemoryStream()
            ' Export the report to the memory stream as a PDF
            Try
                doc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat).CopyTo(memoryStream)

                ' Convert the memory stream to a byte array
                Dim reportBytes As Byte() = memoryStream.ToArray()

                ' Convert the byte array to a base64 string
                Dim base64String As String = Convert.ToBase64String(reportBytes)




                'Dim url As String = "http://localhost:8002/v1/messages"
                'url = "https://whatsapi-vlvp.onrender.com/v1/messages"

                Dim url = "http://192.168.20.75:8000/v1/messages"
                Dim docsUrl = url + "/docs/"
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
                Dim response1 = whatsapi.PostData(docsUrl, data)
                'Response.Write(rapiRsponse)
                Dim contentStr As StreamContent = response1.Content
                Dim contentString As String = contentStr.ReadAsStringAsync().Result

                Dim serializer As New Newtonsoft.Json.JsonSerializer
                Dim jsonReader As New Newtonsoft.Json.JsonTextReader(New StringReader(contentString))
                Dim jsonResponse As ResponseObject = serializer.Deserialize(Of ResponseObject)(jsonReader)



                If response1.IsSuccessStatusCode Then


                    If Integer.TryParse(jsonResponse.Data, IdDeLaPlataforma) Then
                        msg = "WhatsApp enviado con éxito"
                        isSuccess = True
                    Else
                        Throw New Exception("Error con id de la plataforma")
                    End If
                Else
                    If jsonResponse.Errors(0) IsNot Nothing Then
                        msg = jsonResponse.Errors(0).Msg
                    Else
                        msg = $"{response1.StatusCode} {response1.ReasonPhrase}"
                        End If
                        msg = $"Error, intente de nuevo: " + msg

                End If
            Catch ex As Exception
                Dim errorMessage As String = ex.Message
                DebugHelper.SendDebugInfo("danger", ex, user)
                msg = "Error del sistema: " + ex.GetType().Name
            End Try
            Dim result As New ResultW(isSuccess, msg)
            logW(name, couentryCode, localNumber, caption, clientCode, user, instancia, docDescription, isSuccess, msg, IdDeLaPlataforma)

            Return result
        End Using



    End Function



    Public Shared Sub logW(name, couentryCode, localNumber, caption, clientCode, user, instancia, docDescription, isSuccess, msg, Optional IdDeLaPlataforma = Nothing)
        Using context As New FunamorContext()
            context.Database.Log = Sub(s) System.Diagnostics.Debug.WriteLine(s)

            Dim sql As String = "
            INSERT INTO [dbo].[LogDocumentosPorWhatsApp]
            ([CodigoDeCliente], [Telefono], [Usuario], [NombreDeDocumento], [Instancia], [CodigoDePais], [Mensaje], [DescripcionDeDocumento], [FueExitoso], [MensajeDelResultado], [IdDeLaPlataforma])
            VALUES
            (@CodigoDeCliente, @Telefono, @Usuario, @NombreDeDocumento, @Instancia, @CodigoDePais, @Mensaje, @DescripcionDeDocumento, @FueExitoso, @MensajeDelResultado, @IdDeLaPlataforma);
        "
            Dim succ
            If isSuccess = True Then
                succ = 1
            Else
                succ = 0
            End If

            Dim parameters As SqlParameter() = {
                New SqlParameter("@CodigoDeCliente", clientCode),
                New SqlParameter("@Telefono", localNumber),
                New SqlParameter("@Usuario", user),
                New SqlParameter("@NombreDeDocumento", name),
                New SqlParameter("@Instancia", instancia),
                New SqlParameter("@CodigoDePais", couentryCode),
                New SqlParameter("@Mensaje", caption),
                New SqlParameter("@DescripcionDeDocumento", docDescription),
                New SqlParameter("@FueExitoso", succ),
                New SqlParameter("@MensajeDelResultado", msg),
                New SqlParameter("IdDeLaPlataforma", IdDeLaPlataforma)
            }

            context.Database.ExecuteSqlCommand(sql, parameters)
        End Using
    End Sub

End Class
