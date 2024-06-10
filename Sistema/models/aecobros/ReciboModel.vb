Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("RECIBOS")>
Public Class ReciboDeCobro
    <Key>
    <Column("Num_doc", TypeName:="char")>
    <StringLength(16)>
    Public Property NumeroDeRecibo As String

    <Column("Tdoc", TypeName:="char")>
    <StringLength(1)>
    Public Property Tdoc As String

    <Column("rtipodoc", TypeName:="char")>
    <StringLength(1)>
    Public Property Rtipodoc As String

    <Column("Fecha_recib", TypeName:="datetime")>
    Public Property FechaRecib As DateTime

    <Column("RFECHA", TypeName:="datetime")>
    Public Property Rfecha As DateTime

    <Column("RCONCEPTO", TypeName:="char")>
    <StringLength(85)>
    Public Property Rconcepto As String

    <Column("RCONCEP2", TypeName:="char")>
    <StringLength(85)>
    Public Property Rconcepto2 As String

    <Column("RCAMBIO", TypeName:="decimal")>
    Public Property Rcambio As Decimal

    <Column("Por_lempira", TypeName:="numeric")>
    Public Property PorLempira As Decimal

    <Column("rlugar", TypeName:="char")>
    <StringLength(1)>
    Public Property Rlugar As String

    <Column("rhora", TypeName:="char")>
    <StringLength(8)>
    Public Property Rhora As String

    <Column("COBRADO_EN", TypeName:="char")>
    <StringLength(2)>
    Public Property CobradoEn As String

    <Column("rtermino", TypeName:="char")>
    <StringLength(1)>
    Public Property Rtermino As String
    <ForeignKey("Cliente")>
    <Column("Codigo_clie", TypeName:="char")>
    <StringLength(10)>
    Public Property CodigoCliente As String

    <Column("rcodigocli", TypeName:="char")>
    <StringLength(10)>
    Public Property Rcodigocli As String

    <Column("CXC_CTIPO", TypeName:="char")>
    <StringLength(1)>
    Public Property CxcCtipo As String

    <Column("CXC_PARTIDA", TypeName:="char")>
    <StringLength(9)>
    Public Property CxcPartida As String

    <Column("rusuario", TypeName:="char")>
    <StringLength(10)>
    Public Property Rusuario As String

    <Column("rterminal", TypeName:="char")>
    <StringLength(8)>
    Public Property Rterminal As String

    <Column("RECIBMODIFI", TypeName:="char")>
    <StringLength(10)>
    Public Property Recibmodifi As String

    <Column("RECIBFMODIF", TypeName:="datetime")>
    Public Property Recibfmodif As DateTime?

    <Column("rduradeb", TypeName:="smallint")>
    Public Property Rduradeb As Int16?

    <Column("RTIPODEBI", TypeName:="char")>
    <StringLength(2)>
    Public Property Rtipodebi As String

    <Column("rsucursal", TypeName:="char")>
    <StringLength(2)>
    Public Property Rsucursal As String

    <ForeignKey("Cobrador")>
    <Column("codigo_cobr", TypeName:="char")>
    <StringLength(5)>
    Public Property CodigoCobr As String

    <Column("RNOMBRECLI", TypeName:="char")>
    <StringLength(40)>
    Public Property Rnombrecli As String

    <Column("rprima", TypeName:="money")>
    Public Property Rprima As Decimal?

    <Column("rno_letras", TypeName:="smallint")>
    Public Property RnoLetras As Int16?

    <Column("VEFECTI", TypeName:="money")>
    Public Property Vefecto As Decimal?

    <Column("VCHEQUE", TypeName:="money")>
    Public Property Vcheque As Decimal?

    <Column("PAGOCK", TypeName:="char")>
    <StringLength(10)>
    Public Property Pagock As String

    <Column("PAGOBANCO", TypeName:="char")>
    <StringLength(25)>
    Public Property Pagobanco As String

    <Column("RECIBCUENTA", TypeName:="char")>
    <StringLength(10)>
    Public Property Recibcuenta As String

    <Column("rconciliado", TypeName:="char")>
    <StringLength(1)>
    Public Property Rconciliado As String

    <Column("rnumefa", TypeName:="char")>
    <StringLength(10)>
    Public Property Rnumefa As String

    <Column("RCUOTA", TypeName:="money")>
    Public Property Rcuota As Decimal?

    <Column("SUBIVIEJO", TypeName:="char")>
    <StringLength(1)>
    Public Property Subiviejo As String

    <Column("RCODVEND", TypeName:="char")>
    <StringLength(8)>
    Public Property Rcodvend As String

    <Column("rnumemp", TypeName:="int")>
    Public Property Rnumemp As Integer?

    <Column("LONGITUD", TypeName:="nchar")>
    <StringLength(60)>
    Public Property Longitud As String

    <Column("LATITUD", TypeName:="nchar")>
    <StringLength(60)>
    Public Property Latitud As String

    <Column("CIERRE", TypeName:="nchar")>
    <StringLength(20)>
    Public Property Cierre As String

    <Column("IMPRESO", TypeName:="nchar")>
    <StringLength(10)>
    Public Property Impreso As String

    <Column("MARCA", TypeName:="nchar")>
    <StringLength(10)>
    Public Property Marca As String

    <Column("SALDOANT", TypeName:="numeric")>
    Public Property Saldoant As Decimal?

    <Column("liquida", TypeName:="nchar")>
    <StringLength(20)>
    Public Property Liquida As String

    <Column("fecha", TypeName:="nchar")>
    <StringLength(10)>
    Public Property Fecha As String

    <Column("liquida2", TypeName:="nchar")>
    <StringLength(20)>
    Public Property Liquida2 As String
    Public Overridable Property Cliente As Cliente
    Public Overridable Property Cobrador As Cobrador
    'Public Overridable Property Vendedor As Vendedor

End Class

