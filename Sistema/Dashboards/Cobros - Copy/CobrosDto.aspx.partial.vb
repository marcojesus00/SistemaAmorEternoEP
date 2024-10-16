﻿
Imports System.ComponentModel.DataAnnotations

Partial Public Class CobrosDashboard
    Public Class ReciboDTO
        <Key>
        Public Property Codigo As String
        Public Property Cliente As String
        Public Property Codigo_cliente As String
        Public Property Cobrado As String
        Public Property Saldo_actual As String
        Public Property Saldo_anterior As String
        Public Property Empresa As String
        Public Property Zona As String
        Public Property Hora As String
        Public Property Fecha As String
        Public Property LATITUD As String
        Public Property LONGITUD As String
        Public Property MARCA As String
        Public Property liquida As String
        Public Property liquida2 As String
    End Class
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
        Public Property Nombre As String
        Public Property Clientes As Integer
        Public Property Cuota_mensual As String


    End Class
    Public Class PortfolioFinalDto
        Public Property Codigo As String
        Public Property Nombre As String
        Public Property Clientes As Integer
        Public Property Cuota_mensual As String

    End Class
    Public Class DocsDto
        Public Property Codigo As String
        Public Property Nombre As String
        Public Property Telefono As String

    End Class
    Public Class DocsDtoCL
        Public Property Codigo As String
        Public Property Nombre As String
        Public Property Telefono As String
        Public Property Saldo As String
        Public Property Ultimo_envio As String

    End Class
    Public Class PortfolioDetailsDto
        Public Property Codigo As String
        Public Property Nombre As String
        Public Property Identidad As String
        Public Property Telefonos As String
        Public Property Direccion As String
        Public Property Empresa As String
        Public Property Saldo As String
        Public Property Latitud As Decimal
        Public Property Longitud As Decimal
        Public Property codigo_cobr As String
        Public Property cob_lider As String

    End Class

    Public Class CobradorDto
        Public Property Codigo As String
        Public Property Lider As String
        Public Property Telefono_lider As String
        Public Property Zona As String

    End Class

End Class
