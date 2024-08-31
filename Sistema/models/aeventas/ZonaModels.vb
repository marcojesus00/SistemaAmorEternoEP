Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("DEPTOZONA", Schema:="dbo")>
Public Class DeptoZona
    <Column("coddepto", TypeName:="nchar")>
    <StringLength(5)>
    Public Property CodDepto As String

    <Column("desdepto", TypeName:="nchar")>
    <StringLength(200)>
    Public Property DesDepto As String

    <Key>
    <Column("codmuni", TypeName:="nchar")>
    <StringLength(10)>
    Public Property Codigo As String

    <Column("desmuni", TypeName:="nchar")>
    <StringLength(200)>
    Public Property Nombre As String

    <Column("codzona", TypeName:="nchar")>
    <StringLength(10)>
    <DefaultValue("0")>
    Public Property CodZona As String
End Class
