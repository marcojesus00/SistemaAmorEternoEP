Imports System.ComponentModel.DataAnnotations

Imports System.ComponentModel.DataAnnotations.Schema


<Table("CLIENTESN")>
Public Class DatosDeCliente

    <Key>
    <Column("Codigo_clie", TypeName:="char")>
    <StringLength(10)>
    Public Property Codigo As String

    <Column("Nombre_clie", TypeName:="char")>
    <StringLength(55)>
    Public Property Nombre As String

    <Column("identidad", TypeName:="char")>
    <StringLength(15)>
    Public Property Identidad As String

    <Column("tributario", TypeName:="char")>
    <StringLength(15)>
    Public Property tributario As String

    <Column("cl_fecha")>
    Public Property cl_fecha As Nullable(Of DateTime)

    <Column("CL_STATUS", TypeName:="char")>
    <StringLength(1)>
    Public Property CL_STATUS As String

    <Column("CL_COMPANIA", TypeName:="char")>
    <StringLength(1)>
    Public Property CL_COMPANIA As String

    <Column("Dir_cliente", TypeName:="char")>
    <StringLength(80)>
    Public Property Direccion As String

    <Column("Dir2_client", TypeName:="char")>
    <StringLength(80)>
    Public Property Dir2_client As String

    <Column("Dir3_client", TypeName:="char")>
    <StringLength(80)>
    Public Property Dir3_client As String

    <Column("Dir4_client", TypeName:="char")>
    <StringLength(80)>
    Public Property Dir4_client As String

    <Column("Dir5_client", TypeName:="char")>
    <StringLength(80)>
    Public Property Dir5_client As String

    <Column("Telef_clien", TypeName:="char")>
    <StringLength(20)>
    Public Property Telefono As String

    <Column("CL_CELULAR", TypeName:="char")>
    <StringLength(20)>
    Public Property Celular As String

    <Column("CL_EMAIL", TypeName:="char")>
    <StringLength(80)>
    Public Property Email As String

    <Column("cl_conyunom", TypeName:="char")>
    <StringLength(40)>
    Public Property NombreDelConyuge As String

    <Column("cl_conyutel", TypeName:="char")>
    <StringLength(20)>
    Public Property TelefonoDelConyuge As String

    <Column("cl_conyudir", TypeName:="char")>
    <StringLength(80)>
    Public Property DireccionDelConyuge As String

    <Column("CL_PARENt", TypeName:="char")>
    <StringLength(20)>
    Public Property CL_PARENt As String

    <Column("Cod_zona", TypeName:="char")>
    <StringLength(3)>
    Public Property Cod_zona As String

    <Column("CL_VENDEDOR", TypeName:="char")>
    <StringLength(8)>
    Public Property CodigoVendedor As String

    <Column("cl_usuario", TypeName:="char")>
    <StringLength(10)>
    Public Property Cl_usuario As String

    <Column("cl_terminal", TypeName:="char")>
    <StringLength(8)>
    Public Property Cl_terminal As String

    <Column("longitud")>
    Public Property Longitud As Nullable(Of Decimal)

    <Column("latitud")>
    Public Property Latitud As Nullable(Of Decimal)

    <Column("secuencia")>
    Public Property Secuencia As Nullable(Of Integer)

    <Column("diavisita")>
    Public Property Diavisita As Nullable(Of Short)

    <Column("semana")>
    Public Property Semana As Nullable(Of Short)

    <Column("cierre", TypeName:="char")>
    <StringLength(15)>
    Public Property Cierre As String

    <Column("tempo", TypeName:="char")>
    <StringLength(1)>
    Public Property Tempo As String

    <Column("departa", TypeName:="char")>
    <StringLength(30)>
    Public Property Departamento As String

    <Column("municipio", TypeName:="char")>
    <StringLength(30)>
    Public Property Municipio As String

    <Column("circuito", TypeName:="char")>
    <StringLength(10)>
    Public Property circuito As String

    <Column("liquida", TypeName:="nchar")>
    <StringLength(15)>
    Public Property Liquida As String

    <Column("hora", TypeName:="nchar")>
    <StringLength(8)>
    Public Property Hora As String

    <Column("FechaDoc", TypeName:="varchar")>
    <StringLength(12)>
    Public Property FechaDoc As String

End Class
