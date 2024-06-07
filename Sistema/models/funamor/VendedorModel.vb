Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("VENDEDOR")>
Public Class Vendedor
    <Key>
    <Column("Cod_vendedo", TypeName:="char")>
    <StringLength(8)>
    Public Property Codigo As String

    <Column("Nombre_vend", TypeName:="char")>
    <StringLength(40)>
    Public Property Nombre As String

    <Column("Dir_vendedo", TypeName:="char")>
    <StringLength(50)>
    Public Property Direccion As String

    <Column("Tel_vendedo", TypeName:="char")>
    <StringLength(11)>
    Public Property Telefono As String

    <Column("Comision_ve", TypeName:="smallmoney")>
    Public Property Comision As Decimal?

    <Column("COMICOB", TypeName:="smallmoney")>
    Public Property ComisionCobro As Decimal?

    <Column("passvend", TypeName:="char")>
    <StringLength(8)>
    Public Property Password As String

    <Column("meta_vended", TypeName:="money")>
    Public Property Meta As Decimal?

    <Column("VEND_STATUS", TypeName:="char")>
    <StringLength(1)>
    Public Property Status As String

    <Column("vend_numbee", TypeName:="char")>
    <StringLength(6)>
    Public Property NumeroBee As String

    <Column("vend_telbee", TypeName:="char")>
    <StringLength(25)>
    Public Property TelefonoBee As String

    <Column("VEND_TIPO", TypeName:="char")>
    <StringLength(1)>
    Public Property Tipo As String

    <Column("vend_sucu", TypeName:="char")>
    <StringLength(2)>
    Public Property Sucursal As String

    <Column("VEND_LIDER", TypeName:="char")>
    <StringLength(8)>
    Public Property Lider As String

    '<ForeignKey("VZONA")>
    <Column("vzon_codigo", TypeName:="char")>
    <StringLength(4)>
    Public Property ZonaCodigo As String

    <Column("VEND_INGRES", TypeName:="datetime")>
    Public Property Ingreso As DateTime?

    <Column("vend_fechin", TypeName:="datetime")>
    Public Property FechaIngreso As DateTime?

    <Column("VEND_VIATIC", TypeName:="money")>
    Public Property Viatico As Decimal?

    <Column("vend_teltra", TypeName:="char")>
    <StringLength(11)>
    Public Property TelefonoTrabajo As String

    <Column("VEND_IDENTI", TypeName:="char")>
    <StringLength(15)>
    Public Property Identificacion As String

    <Column("VEND_FECNAC", TypeName:="datetime")>
    Public Property FechaNacimiento As DateTime?

    <Column("VEND_FOTO", TypeName:="varbinary(max)")>
    Public Property Foto As Byte()

    <Column("vend_suplid", TypeName:="char")>
    <StringLength(5)>
    Public Property Suplidor As String
End Class
