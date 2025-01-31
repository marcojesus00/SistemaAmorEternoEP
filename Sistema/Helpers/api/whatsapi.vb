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
    Public Shared Function sendWhatsAppDocs(doc As System.IO.Stream, name As String, couentryCode As String, localNumber As String, caption As String, clientCode As String, user As String, instancia As String, CodigoDeCobrador As String, BatchId As String, Optional docDescription As String = "Estado de cuenta") As ResultW
        Dim estado As String = "no definido"

        Dim msg = ""
        Dim isSuccess As Boolean = False
        Dim IdDeLaPlataforma As Integer = 0
        Dim referenceID As Guid = Guid.NewGuid()

        ' Exportar el informe a PDF y guardar en la ruta especificada con el nombre del archivo
        'Informe.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaCompletaArchivo)
        Using memoryStream As New System.IO.MemoryStream()
            ' Export the report to the memory stream as a PDF
            Try
                doc.CopyTo(memoryStream)

                ' Convert the memory stream to a byte array
                Dim reportBytes As Byte() = memoryStream.ToArray()

                ' Convert the byte array to a base64 string
                Dim base64String As String = Convert.ToBase64String(reportBytes)



                'Dim url As String = "http://localhost:8002/v1/messages"
                'url = "https://whatsapi-vlvp.onrender.com/v1/messages"

                Dim url = "http://192.168.20.111:8000/v1/messages"
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
                    {"referenceID", referenceID.ToString()},
                    {"instance", instancia}
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
                        estado = "enviado"
                    Else
                        Throw New Exception("Error con id de la plataforma")
                    End If
                Else
                    If jsonResponse.Errors IsNot Nothing AndAlso jsonResponse.Errors IsNot Nothing Then
                        msg = $"{response1.StatusCode}" + jsonResponse.Errors(0).Msg
                    Else
                        msg = $"{response1.StatusCode} {response1.ReasonPhrase}"
                    End If
                    msg = $"Error " + msg

                End If
            Catch ex As Exception
                Dim errorMessage As String = ex.Message
                DebugHelper.SendDebugInfo("danger", ex, user)
                msg = "Error del sistema: " + ex.GetType().Name
            End Try
            If msg.ToLower.Contains("invalid") Then
                estado = "invalido"
            ElseIf msg.ToLower.Contains("queue") Then
                estado = "cola"
            End If
            Dim result As New ResultW(isSuccess, msg)
            logW(name:=name, couentryCode:=couentryCode, localNumber:=localNumber, caption:=caption, clientCode:=clientCode, user:=user, instancia:=instancia, docDescription:=docDescription, isSuccess:=isSuccess, msg:=msg, CodigoDeCobrador:=CodigoDeCobrador, Estado:=estado, IdDeLaPlataforma:=IdDeLaPlataforma, BatchId:=BatchId, ReferenceId:=referenceID)

            Return result
        End Using



    End Function




    Public Shared Function PostHtmlAndReceivePdf(htmlContent As String, title As String, download As Boolean, Optional filepath As String = "")
        Dim url = "http://192.168.20.111:8000/v1/helpers/html-to-pdf/"

        ' Create an instance of HttpClient
        Using client As New HttpClient()
            ' Define the JSON payload
            Dim requestBody As String = $"{{ ""html_content"": ""{htmlContent.Replace("""", "\""")}"" }}"

            ' Prepare the content with the correct content type (application/json)
            Dim content As New StringContent(requestBody, Encoding.UTF8, "application/json")

            ' Send the POST request to the API endpoint
            Dim response As HttpResponseMessage = client.PostAsync(url, content).Result

            ' Ensure the response is successful (status code 200 OK)

            If response.IsSuccessStatusCode Then
                Dim pdfBytes As Byte() = response.Content.ReadAsByteArrayAsync().Result
                If download Then
                    HttpContext.Current.Response.Clear()
                    HttpContext.Current.Response.ContentType = "application/pdf"
                    HttpContext.Current.Response.AddHeader("Content-Disposition", $"attachment; filename={title}.pdf")
                    HttpContext.Current.Response.BinaryWrite(pdfBytes)
                    HttpContext.Current.Response.End()
                Else

                End If

                ' Clear the response

            Else
                ' Handle errors if needed
                Throw New HttpException("ERROR code:" & response.StatusCode.ToString() & " reason:" & response.ReasonPhrase & " content:" & response.Content.ToString())
            End If
        End Using
    End Function

    Public Shared Sub logW(name, couentryCode, localNumber, caption, clientCode, user, instancia, docDescription, isSuccess, msg, CodigoDeCobrador, Estado, ReferenceId, Optional IdDeLaPlataforma = 0, Optional BatchId = "N/A")
        Using context As New FunamorContext()
            context.Database.Log = Sub(s) System.Diagnostics.Debug.WriteLine(s)
            If name.Length > 50 Then
                name = name.Substring(0, 50) ' Truncate to 50 characters
            End If

            If couentryCode.Length > 8 Then
                couentryCode = couentryCode.Substring(0, 8) ' Truncate to 8 characters
            End If

            If localNumber.Length > 15 Then
                localNumber = localNumber.Substring(0, 15) ' Truncate to 15 characters
            End If

            If user.Length > 15 Then
                user = user.Substring(0, 15) ' Truncate to 15 characters
            End If

            If instancia.Length > 10 Then
                instancia = instancia.Substring(0, 10) ' Truncate to 10 characters
            End If

            If caption.Length > 255 Then
                caption = caption.Substring(0, 255) ' Truncate to 255 characters
            End If

            If docDescription.Length > 25 Then
                docDescription = docDescription.Substring(0, 25) ' Truncate to 25 characters
            End If

            If Estado.Length > 15 Then
                Estado = Estado.Substring(0, 15) ' Truncate to 15 characters
            End If

            If BatchId.Length > 100 Then
                BatchId = BatchId.Substring(0, 100) ' Truncate to 100 characters
            End If

            If CodigoDeCobrador.Length > 10 Then
                CodigoDeCobrador = CodigoDeCobrador.Substring(0, 10) ' Truncate to 10 characters
            End If

            ' Convert boolean isSuccess to 1 or 0
            Dim succ As Integer = If(isSuccess, 1, 0)


            Dim sql As String = "
            INSERT INTO [dbo].[LogDocumentosPorWhatsApp]
            ([CodigoDeCliente], [Telefono], [Usuario], [NombreDeDocumento], [Instancia], [CodigoDePais], [Mensaje], [DescripcionDeDocumento], [FueExitoso], [MensajeDelResultado], [IdDeLaPlataforma], [CodigoDeCobrador], [Estado], [BatchId], [ReferenceId])
            VALUES
            (@CodigoDeCliente, @Telefono, @Usuario, @NombreDeDocumento, @Instancia, @CodigoDePais, @Mensaje, @DescripcionDeDocumento, @FueExitoso, @MensajeDelResultado, @IdDeLaPlataforma, @CodigoDeCobrador, @Estado, @BatchId, @ReferenceId);
        "


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
                New SqlParameter("@IdDeLaPlataforma", IdDeLaPlataforma),
                New SqlParameter("@CodigoDeCobrador", CodigoDeCobrador),
                                New SqlParameter("@Estado", Estado),
New SqlParameter("@BatchId", BatchId),
New SqlParameter("@ReferenceId", ReferenceId)
            }

            context.Database.ExecuteSqlCommand(sql, parameters)
        End Using
    End Sub

End Class
