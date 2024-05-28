<%@ WebHandler Language="VB" Class="DownloadHandler" %>

Imports System
Imports System.Web
Imports System.IO

Public Class DownloadHandler : Implements IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim filePath As String = context.Request.QueryString("path")
        Dim documentName As String = context.Request.QueryString("name")

        'Dim filePath As String = context.Server.MapPath($"~/{documentPath}")

        If File.Exists(filePath) Then
            context.Response.Clear()
            context.Response.ContentType = "application/pdf"
            context.Response.AppendHeader("Content-Disposition", $"attachment; filename={documentName}")
            context.Response.WriteFile(filePath)
            context.Response.End()
        Else
            context.Response.ContentType = "text/plain"
            context.Response.Write("Archivo no encontrado.")
        End If
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class
