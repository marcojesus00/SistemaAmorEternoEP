<%@ WebHandler Language="VB" Class="Reports" %>
'Imports Sistema

''Imports System
''Imports System.Web
'Imports System.Web.Script.Serialization
''Imports System.Collections.Generic
'Public Class Reports : Implements IHttpHandler

'    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
'        Try
'            context.Response.ContentType = "application/json"

'            ' Example response data
'            Dim data As New Dictionary(Of String, String) From {
'                {"status", "success"},
'                {"message", "Hello from WebForms API"}
'            }
'            Estadistica_Visitas_WhatsApp()

'            Dim json As String = New JavaScriptSerializer().Serialize(data)
'            context.Response.Write(json)
'        Catch ex As Exception
'            context.Response.StatusCode = 500
'            context.Response.Write("{""status"":""error"",""message"":""" & ex.Message & """}")
'        End Try
'    End Sub

'    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
'        Get
'            Return False
'        End Get
'    End Property

'End Class
