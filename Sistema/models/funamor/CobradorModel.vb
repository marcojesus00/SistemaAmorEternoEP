Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
<Table("COBRADOR")>
Public Class Cobrador
    <Key>
    <Column("codigo_cobr", TypeName:="char")>
    <StringLength(5)>
    Public Property Codigo As String

    <Column("nombre_cobr", TypeName:="char")>
    <StringLength(40)>
    Public Property Nombre As String

    <Column("COBR_STATUS", TypeName:="char")>
    <StringLength(1)>
    Public Property CobrStatus As String

    <Column("cob_telefo", TypeName:="char")>
    <StringLength(15)>
    Public Property CobTelefono As String

    <Column("cob_zona", TypeName:="char")>
    <StringLength(4)>
    Public Property CobZona As String

    <Column("cob_fingre", TypeName:="datetime")>
    Public Property CobFingre As DateTime?

    <Column("cob_lider", TypeName:="char")>
    <StringLength(5)>
    Public Property CobLider As String

    <Column("COB_DIRECCI", TypeName:="char")>
    <StringLength(80)>
    Public Property CobDireccion As String

    <Column("COB_FECHING", TypeName:="datetime")>
    Public Property CobFeching As DateTime?

    <Column("COB_SUPERVI", TypeName:="char")>
    <StringLength(5)>
    Public Property CobSupervi As String

    <Column("COB_EMAIL", TypeName:="char")>
    <StringLength(80)>
    Public Property CobEmail As String

    <Column("COB_TELTRAB", TypeName:="char")>
    <StringLength(15)>
    Public Property CobTeltrab As String

    <Column("COB_IDENTID", TypeName:="char")>
    <StringLength(15)>
    Public Property CobIdentid As String

    <Column("COB_FECNAC", TypeName:="datetime")>
    Public Property CobFecnac As DateTime?

    <Column("COB_FOTO", TypeName:="varbinary")>
    Public Property CobFoto As Byte()

    <Column("COB_SUCU", TypeName:="char")>
    <StringLength(2)>
    Public Property CobSucu As String

    Public WithEvents RecibosNavigation As ICollection(Of ReciboDeCobro)
    Public Overridable Property Clientes As ICollection(Of Cliente)


End Class
