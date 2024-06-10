Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("CZONA")>
Public Class Empresa
    <Key>
    <Column("Cod_zona", TypeName:="char")>
    <StringLength(3)>
    Public Property Codigo As String

    <Column("Nombre_zona", TypeName:="char")>
    <StringLength(40)>
    Public Property Nombre As String

    <Column("E_ZONAS", TypeName:="char")>
    <StringLength(10)>
    Public Property EZonas As String

    <Column("E_VENTAS", TypeName:="char")>
    <StringLength(10)>
    Public Property EVentas As String

    <Column("E_DEVOLUC", TypeName:="char")>
    <StringLength(10)>
    Public Property EDevoluc As String

    <Column("E_VENTAS2", TypeName:="char")>
    <StringLength(10)>
    Public Property EVentas2 As String

    <Column("ZON_NC")>
    Public Property ZonNC As Integer?

    <Column("ZON_ND")>
    Public Property ZonND As Integer?

    <Column("descri_zona", TypeName:="char")>
    <StringLength(40)>
    Public Property DescriZona As String

    <Column("E_CAJA", TypeName:="char")>
    <StringLength(10)>
    Public Property ECaja As String

    <Column("E_APARTA", TypeName:="char")>
    <StringLength(10)>
    Public Property EAparta As String

    <Column("E_LETRAS", TypeName:="char")>
    <StringLength(10)>
    Public Property ELetras As String

    <Column("E_PRIMA", TypeName:="char")>
    <StringLength(10)>
    Public Property EPrima As String

    <Column("E_PAQUETE", TypeName:="char")>
    <StringLength(10)>
    Public Property EPaquete As String

    <Column("E_CXC", TypeName:="char")>
    <StringLength(10)>
    Public Property ECxc As String

    <Column("E_CONTADO", TypeName:="char")>
    <StringLength(10)>
    Public Property EContado As String

    <Column("CZ_NUME")>
    Public Property CzNume As Short?

    <Column("E_RTN", TypeName:="char")>
    <StringLength(16)>
    Public Property ERtn As String
End Class

<Table("VZONA")>
Public Class Municipio
    <Key>
    <Column("vzon_codigo", TypeName:="char")>
    <StringLength(4)>
    Public Property Codigo As String

    <Column("vzon_nombre", TypeName:="char")>
    <StringLength(50)>
    Public Property Nombre As String

    <Column("vzon_ciudad", TypeName:="char")>
    <StringLength(3)>
    Public Property VzonCiudad As String

    <Column("vzon_empres", TypeName:="char")>
    <StringLength(3)>
    Public Property VzonEmpres As String

    <Column("vzon_app", TypeName:="char")>
    <StringLength(4)>
    Public Property VzonApp As String
End Class