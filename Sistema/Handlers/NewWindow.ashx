<%@ WebHandler Language="VB" Class="LinkHandler" %>

'Imports System
'Imports System.Web
'Imports System.IO

'Public Class LinkHandler
'    Implements IHttpHandler

'    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
'        Dim dataKey As String = context.Request.QueryString("datakey")

'        ' Get the link associated with the datakey (from database, etc.)
'        Dim link As String = GetLinkForDataKey(dataKey)

'        ' Redirect the new window to the retrieved link
'        context.Response.Redirect(link)
'    End Sub
'    Public Function FindMapLink(Id)
'        Dim sales As List(Of VentasDto) = getReceiptsFromDB(receiptNumber:=Id)

'        Return sales.Where(Function(r) r.Recibo.Contains(Id)).Select(Function(r) New With {.Link = If(r.LATITUD.ToString() IsNot Nothing AndAlso r.LONGITUD.ToString() IsNot Nothing,
'               $"https://www.google.com/maps?q={r.LATITUD},{r.LONGITUD}",
'               "https://www.google.com/maps")}).FirstOrDefault().Link
'    End Function

'    Private Function GetLinkForDataKey(dataKey As String) As String
'        ' Your logic to fetch the link based on the datakey
'        ' Example using a database query:
'        ' ... (Connect to database) ...
'        ' ... (Execute query using dataKey) ...
'        ' ... (Return the link from the query result) ...
'    End Function

'    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
'        Get
'            Return False
'        End Get
'    End Property
'End Class
