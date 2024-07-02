
Imports System.ComponentModel.DataAnnotations

Partial Public Class CobrosDashboard

    Public Class RecibosDTO
        <Key>
        Public Property Num_doc As String
        Public Property RFECHA As DateTime
        Public Property codigo_cobr As String
        Public Property nombre_cobr As String
        Public Property cob_lider As String
        Public Property Codigo_clie As String
        Public Property Nombre_clie As String
        Public Property Por_lempira As Decimal?
        Public Property Saldo_actua As Decimal?
        Public Property Cod_zona As String
        Public Property VZCODIGO As String
        Public Property LATITUD As String
        Public Property LONGITUD As String
        Public Property SALDOANT As Decimal
        Public Property MARCA As String
        Public Property rhora As String
        Public Property liquida As String
        Public Property liquida2 As String
    End Class
    Public Class PortfolioIDto
        Public Property Codigo As String
        Public Property nombre_cobr As String
        Public Property Clientes As Integer
        Public Property Cartera As Decimal


    End Class
    Public Class PortfolioFinalDto
        Public Property Codigo As String
        Public Property Nombre As String
        Public Property Clientes As Integer
        Public Property Cartera As String

    End Class
    Public Class PortfolioDetailsDto
        Public Property Codigo As String
        Public Property Nombre As String
        Public Property Identidad As String
        Public Property Telefonos As String
        Public Property Direccion As String
        Public Property Empresa As String
        Public Property Saldo As Decimal
        Public Property Latitud As Decimal
        Public Property Longitud As Decimal

    End Class

End Class
