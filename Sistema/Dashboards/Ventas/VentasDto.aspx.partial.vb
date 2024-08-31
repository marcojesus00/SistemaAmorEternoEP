
Imports System.ComponentModel.DataAnnotations

Partial Public Class VentasDashboard

    Public Class VentasDto
        <Key>
        Public Property Recibo As String
        Public Property Fecha As DateTime
        Public Property VendedorId As String
        Public Property Vendedor As String
        Public Property LiderId As String
        Public Property ClienteId As String
        Public Property Cliente As String
        Public Property Por_lempira As Decimal?
        Public Property ServicioId As String
        Public Property TipoDeServicio As String
        Public Property Servicio As String
        Public Property Valor As Decimal
        Public Property Prima As Decimal
        Public Property NumeroCuotas As Int16
        Public Property Cuota As Decimal
        Public Property Cantidad As Int16
        Public Property MARCA As String
        Public Property Hora As String
        Public Property liquida As String
        Public Property liquida2 As String
        Public Property LATITUD As Decimal
        Public Property LONGITUD As Decimal
    End Class
    Public Class SalesByProductDto
        Public Property ServicioId As String
        Public Property TipoDeServicio As String

        Public Property Servicio As String
        Public Property Prima As Decimal
        Public Property Valor As Decimal
        Public Property Cuota As Decimal

        Public Property Cantidad As Integer
        Public Property Contratos As Integer


    End Class
    Public Class ServicesDto
        Public Property Codigo As String
        Public Property TipoDeServicio As String
        Public Property Nombre As String
    End Class

    Public Class SalesGroupedDto
        Public Property Codigo As String
        Public Property Nombre As String
        Public Property Ventas As Integer
        Public Property Cobrado As Decimal
        Public Property Lider As String
    End Class
End Class