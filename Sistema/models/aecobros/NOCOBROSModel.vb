Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("NOCOBRO")>
Public Class NocobroModel
    <Key>
    <Column("Codigo_clie", TypeName:="char")>
    <StringLength(10)>
    Public Property CodigoCliente As String

    <Column("Nombre_clie", TypeName:="char")>
    <StringLength(50)>
    Public Property NombreCliente As String

    <Column("CONT_NUMERO", TypeName:="char")>
    <StringLength(20)>
    Public Property ContNumero As String

    <Column("COBRADOR", TypeName:="char")>
    <StringLength(10)>
    Public Property Cobrador As String

    <Column("FECHA", TypeName:="char")>
    <StringLength(10)>
    Public Property Fecha As String

    <Column("HORA", TypeName:="char")>
    <StringLength(10)>
    Public Property Hora As String

    <Column("CODMOTIVO", TypeName:="char")>
    <StringLength(10)>
    Public Property CodMotivo As String

    <Column("MOTIVO", TypeName:="char")>
    <StringLength(50)>
    Public Property Motivo As String

    <Column("LATITUD", TypeName:="nchar")>
    <StringLength(30)>
    Public Property Latitud As String

    <Column("LONGITUD", TypeName:="nchar")>
    <StringLength(30)>
    Public Property Longitud As String

    <Column("CIERRE", TypeName:="nchar")>
    <StringLength(20)>
    Public Property Cierre As String

    <Column("liquida", TypeName:="nchar")>
    <StringLength(20)>
    Public Property Liquida As String

    <Column("liquida2", TypeName:="nchar")>
    <StringLength(20)>
    Public Property Liquida2 As String
End Class
