Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("CONTRATO")>
Public Class Contrato
    <Key>
    <Column("CONT_NUMERO", TypeName:="char")>
    <StringLength(15)>
    Public Property Numero As String

    <Column("CONT_FECHA", TypeName:="datetime")>
    Public Property Fecha As DateTime?

    <ForeignKey("Cliente")>
    <Column("Codigo_clie", TypeName:="char")>
    <StringLength(10)>
    Public Property CodigoCliente As String

    <Column("CONT_DESCRI", TypeName:="char")>
    <StringLength(80)>
    Public Property Descripcion As String

    <Column("CONT_DESC2", TypeName:="char")>
    <StringLength(80)>
    Public Property Descripcion2 As String

    <Column("CONT_DESC3", TypeName:="char")>
    <StringLength(80)>
    Public Property Descripcion3 As String

    <Column("CONT_VALOR", TypeName:="money")>
    Public Property Valor As Decimal?

    <Column("CONT_CREDIT", TypeName:="char")>
    <StringLength(1)>
    Public Property Credito As String

    <Column("CONT_PRIMA", TypeName:="money")>
    Public Property Prima As Decimal?

    <Column("CONT_NUMCUO", TypeName:="smallint")>
    Public Property NumeroCuotas As Short?

    <Column("CONT_VALCUO", TypeName:="money")>
    Public Property ValorCuota As Decimal?

    <Column("CONT_BENEFI", TypeName:="char")>
    <StringLength(40)>
    Public Property Beneficiario As String

    <Column("CONT_BENFEC", TypeName:="datetime")>
    Public Property FechaBeneficio As DateTime?

    <Column("CONT_BENPAR", TypeName:="char")>
    <StringLength(20)>
    Public Property ParticipanteBeneficio As String

    <Column("CONT_CLIENT", TypeName:="char")>
    <StringLength(10)>
    Public Property NombreCliente As String

    <Column("CONT_SUPERV", TypeName:="char")>
    <StringLength(1)>
    Public Property Supervisor As String

    <Column("CONT_FSUPER", TypeName:="datetime")>
    Public Property FechaSupervision As DateTime?

    <Column("cont_vended", TypeName:="char")>
    <StringLength(8)>
    Public Property Vendedor As String

    <Column("cont_cobrad", TypeName:="char")>
    <StringLength(5)>
    Public Property Cobrador As String

    <Column("CONT_SALDO", TypeName:="money")>
    Public Property Saldo As Decimal?

    <Column("CONT_FECAM", TypeName:="datetime")>
    Public Property FechaAmortizacion As DateTime?

    <Column("CONT_CJARD", TypeName:="char")>
    <StringLength(15)>
    Public Property JardinCobro As String

    <Column("CONT_CLETRA", TypeName:="char")>
    <StringLength(2)>
    Public Property LetraCobro As String

    <Column("CONT_CNUM", TypeName:="char")>
    <StringLength(4)>
    Public Property NumeroCobro As String

    <Column("CONT_SERVI", TypeName:="char")>
    <StringLength(4)>
    Public Property Servicio As String

    <Column("CONT_CANTI", TypeName:="smallint")>
    Public Property Cantidad As Short?

    <Column("CONT_FOTO", TypeName:="varbinary(max)")>
    Public Property Foto As Byte()

    <Column("CONT_COMPA", TypeName:="char")>
    <StringLength(3)>
    Public Property Compania As String

    <Column("CONT_CANT2", TypeName:="smallint")>
    Public Property Cantidad2 As Short?

    <Column("CONT_SERV2", TypeName:="char")>
    <StringLength(4)>
    Public Property Servicio2 As String

    <Column("CONT_CANT3", TypeName:="smallint")>
    Public Property Cantidad3 As Short?

    <Column("CONT_SERV3", TypeName:="char")>
    <StringLength(4)>
    Public Property Servicio3 As String

    <Column("CONT_FOT2", TypeName:="varbinary(max)")>
    Public Property Foto2 As Byte()

    <Column("CONT_FOT3", TypeName:="varbinary(max)")>
    Public Property Foto3 As Byte()

    <Column("CONT_FOT4", TypeName:="varbinary(max)")>
    Public Property Foto4 As Byte()

    <Column("CONT_FOT5", TypeName:="varbinary(max)")>
    Public Property Foto5 As Byte()

    <Column("CONT_FOT6", TypeName:="varbinary(max)")>
    Public Property Foto6 As Byte()

    <Column("CONT_CJAR2", TypeName:="char")>
    <StringLength(15)>
    Public Property JardinCobro2 As String

    <Column("CONT_CJAR3", TypeName:="char")>
    <StringLength(15)>
    Public Property JardinCobro3 As String

    <Column("CONT_CLET2", TypeName:="char")>
    <StringLength(2)>
    Public Property LetraCobro2 As String

    <Column("CONT_CLET3", TypeName:="char")>
    <StringLength(2)>
    Public Property LetraCobro3 As String

    <Column("CONT_CNU2", TypeName:="char")>
    <StringLength(4)>
    Public Property NumeroCobro2 As String

    <Column("CONT_CNU3", TypeName:="char")>
    <StringLength(4)>
    Public Property NumeroCobro3 As String

    <Column("CONT_CSUPER", TypeName:="char")>
    <StringLength(2)>
    Public Property ClaseSupervision As String

    <Column("CONT_FECPRO", TypeName:="datetime")>
    Public Property FechaProceso As DateTime?

    <Column("CONT_SVAL1", TypeName:="money")>
    Public Property Valor1 As Decimal?

    <Column("CONT_SVAL2", TypeName:="money")>
    Public Property Valor2 As Decimal?

    <Column("CONT_SVAL3", TypeName:="money")>
    Public Property Valor3 As Decimal?

    <Column("CONT_RFECHA", TypeName:="datetime")>
    Public Property FechaRegistro As DateTime?

    <Column("CONT_RHORA", TypeName:="char")>
    <StringLength(8)>
    Public Property HoraRegistro As String

    <Column("CONT_FECTER", TypeName:="datetime")>
    Public Property FechaTerminacion As DateTime?

    <Column("CONT_HORTER", TypeName:="char")>
    <StringLength(8)>
    Public Property HoraTerminacion As String

    <Column("CONT_NOTAS", TypeName:="char")>
    <StringLength(180)>
    Public Property Notas As String

    <Column("CONT_CUOACT", TypeName:="smallint")>
    Public Property CuotaActual As Short?

    <Column("CONT_FECACT", TypeName:="datetime")>
    Public Property FechaActualizacion As DateTime?

    <Column("CONT_ORDEN", TypeName:="char")>
    <StringLength(10)>
    Public Property Orden As String

    <Column("CONT_LIDER", TypeName:="char")>
    <StringLength(8)>
    Public Property Lider As String

    <Column("CONT_VENDAN", TypeName:="char")>
    <StringLength(8)>
    Public Property VendedorAnexo As String

    <Column("CONT_USUPER", TypeName:="char")>
    <StringLength(10)>
    Public Property UsuarioSupervisor As String

    <Column("CONT_DESC4", TypeName:="char")>
    <StringLength(80)>
    Public Property Descripcion4 As String

    <Column("CONT_CANT4", TypeName:="smallint")>
    Public Property Cantidad4 As Short?

    <Column("CONT_SVAL4", TypeName:="money")>
    Public Property Valor4 As Decimal?

    <Column("CONT_SERV4", TypeName:="char")>
    <StringLength(4)>
    Public Property Servicio4 As String

    <Column("cont_numven", TypeName:="smallint")>
    Public Property NumeroVendedor As Short?

    <Column("CONT_FIRMA", TypeName:="varbinary(max)")>
    Public Property Firma As Byte()

    <Column("CONT_SVAL02", TypeName:="money")>
    Public Property Valor02 As Decimal?

    <Column("CONT_SVAL01", TypeName:="money")>
    Public Property Valor01 As Decimal?

    <Column("CONT_USUIM", TypeName:="char")>
    <StringLength(10)>
    Public Property UsuarioModificacion As String

    <Column("CONT_FECHIM", TypeName:="datetime")>
    Public Property FechaModificacion As DateTime?

    <Column("CONT_FREACT", TypeName:="datetime")>
    Public Property FechaReactivacion As DateTime?

    <Column("CONT_SLIDER", TypeName:="char")>
    <StringLength(5)>
    Public Property Slider As String

    <Column("CONT_SUPNOM", TypeName:="char")>
    <StringLength(40)>
    Public Property NombreSupervisor As String

    <Column("CONT_SUPCOD", TypeName:="char")>
    <StringLength(5)>
    Public Property CodigoSupervisor As String

    <Column("CONT_L3P4", TypeName:="char")>
    <StringLength(1)>
    Public Property L3P4 As String

    <Column("CONT_L3P3", TypeName:="char")>
    <StringLength(1)>
    Public Property L3P3 As String

    <Column("CONT_L3P2", TypeName:="char")>
    <StringLength(1)>
    Public Property L3P2 As String

    <Column("CONT_L3P1", TypeName:="char")>
    <StringLength(1)>
    Public Property L3P1 As String

    <Column("CONT_L2P4", TypeName:="char")>
    <StringLength(1)>
    Public Property L2P4 As String

    <Column("CONT_L2P3", TypeName:="char")>
    <StringLength(1)>
    Public Property L2P3 As String

    <Column("CONT_L2P2", TypeName:="char")>
    <StringLength(1)>
    Public Property L2P2 As String

    <Column("CONT_L2P1", TypeName:="char")>
    <StringLength(1)>
    Public Property L2P1 As String

    <Column("CONT_L1P4", TypeName:="char")>
    <StringLength(1)>
    Public Property L1P4 As String

    <Column("CONT_L1P3", TypeName:="char")>
    <StringLength(1)>
    Public Property L1P3 As String

    <Column("CONT_L1P2", TypeName:="char")>
    <StringLength(1)>
    Public Property L1P2 As String

    <Column("CONT_L1P1", TypeName:="char")>
    <StringLength(1)>
    Public Property L1P1 As String

    <Column("CONT_EMPFEC", TypeName:="datetime")>
    Public Property FechaEmision As DateTime?

    <Column("CONT_EMNUMC", TypeName:="smallint")>
    Public Property NumeroCuotasEmision As Short?

    <Column("CONT_EMCUOT", TypeName:="money")>
    Public Property CuotaEmision As Decimal?

    <Column("CONT_EMSFEC", TypeName:="datetime")>
    Public Property FechaSaldoEmision As DateTime?

    <Column("CONT_EMSALD", TypeName:="money")>
    Public Property SaldoEmision As Decimal?

    <Column("CONT_DESTE", TypeName:="char")>
    <StringLength(80)>
    Public Property DescripcionEstado As String

    <Column("CONT_MESTE", TypeName:="smallint")>
    Public Property MesEstado As Short?

    <Column("CONT_DOESTE", TypeName:="char")>
    <StringLength(80)>
    Public Property DescripcionOtraEstado As String

    <Column("CONT_MOESTE", TypeName:="smallint")>
    Public Property MesOtroEstado As Short?
    Public Property Cliente As Cliente

End Class
