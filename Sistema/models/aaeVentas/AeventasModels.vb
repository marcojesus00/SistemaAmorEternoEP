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


<Table("LogEdicionCLIENTESN")>
Public Class LogEdicionCLIENTESN
    <Key>
    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property ID As Integer

    <Required>
    Public Property TiempoDeEdicion As DateTime

    <Required>
    <StringLength(128)>
    Public Property EditadoPor As String

    <Required>
    <StringLength(10)>
    <Column("Codigo_clie")>
    Public Property Codigo_clie As String

    <Required>
    <StringLength(8)>
    <Column("CL_VENDEDOR")>
    Public Property CL_VENDEDOR As String

    <StringLength(15)>
    <Column("Anterior_identidad")>
    Public Property Anterior_identidad As String

    <StringLength(15)>
    <Column("Nuevo_identidad")>
    Public Property Nuevo_identidad As String

    <StringLength(80)>
    <Column("Anterior_Dir_cliente")>
    Public Property Anterior_Dir_cliente As String

    <StringLength(80)>
    <Column("Nuevo_Dir_cliente")>
    Public Property Nuevo_Dir_cliente As String

    <StringLength(80)>
    <Column("Anterior_Dir2_client")>
    Public Property Anterior_Dir2_client As String

    <StringLength(80)>
    <Column("Nuevo_Dir2_client")>
    Public Property Nuevo_Dir2_client As String

    <StringLength(80)>
    <Column("Anterior_Dir3_client")>
    Public Property Anterior_Dir3_client As String

    <StringLength(80)>
    <Column("Nuevo_Dir3_client")>
    Public Property Nuevo_Dir3_client As String

    <StringLength(80)>
    <Column("Anterior_Dir4_client")>
    Public Property Anterior_Dir4_client As String

    <StringLength(80)>
    <Column("Nuevo_Dir4_client")>
    Public Property Nuevo_Dir4_client As String

    <StringLength(80)>
    <Column("Anterior_Dir5_client")>
    Public Property Anterior_Dir5_client As String

    <StringLength(80)>
    <Column("Nuevo_Dir5_client")>
    Public Property Nuevo_Dir5_client As String

    <StringLength(20)>
    <Column("Anterior_Telef_clien")>
    Public Property Anterior_Telef_clien As String

    <StringLength(20)>
    <Column("Nuevo_Telef_clien")>
    Public Property Nuevo_Telef_clien As String

    <StringLength(20)>
    <Column("Anterior_CL_CELULAR")>
    Public Property Anterior_CL_CELULAR As String

    <StringLength(20)>
    <Column("Nuevo_CL_CELULAR")>
    Public Property Nuevo_CL_CELULAR As String

    <StringLength(80)>
    <Column("Anterior_CL_EMAIL")>
    Public Property Anterior_CL_EMAIL As String

    <StringLength(80)>
    <Column("Nuevo_CL_EMAIL")>
    Public Property Nuevo_CL_EMAIL As String

    <StringLength(40)>
    <Column("Anterior_cl_conyunom")>
    Public Property Anterior_cl_conyunom As String

    <StringLength(40)>
    <Column("Nuevo_cl_conyunom")>
    Public Property Nuevo_cl_conyunom As String

    <StringLength(20)>
    <Column("Anterior_cl_conyutel")>
    Public Property Anterior_cl_conyutel As String

    <StringLength(20)>
    <Column("Nuevo_cl_conyutel")>
    Public Property Nuevo_cl_conyutel As String

    <StringLength(80)>
    <Column("Anterior_cl_conyudir")>
    Public Property Anterior_cl_conyudir As String

    <StringLength(80)>
    <Column("Nuevo_cl_conyudir")>
    Public Property Nuevo_cl_conyudir As String

    <StringLength(30)>
    <Column("Anterior_departa")>
    Public Property Anterior_departa As String

    <StringLength(30)>
    <Column("Nuevo_departa")>
    Public Property Nuevo_departa As String

    <StringLength(30)>
    <Column("Anterior_municipio")>
    Public Property Anterior_municipio As String

    <StringLength(30)>
    <Column("Nuevo_municipio")>
    Public Property Nuevo_municipio As String
End Class
