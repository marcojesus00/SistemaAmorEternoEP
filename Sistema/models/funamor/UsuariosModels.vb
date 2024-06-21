Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("DETSEG")>
Public Class Detseg

    <Key>
    <MaxLength(10)>
    <Required>
    <Column("SEG_USUARIO", TypeName:="char")>
    Public Property SegUsuario As String

    <MaxLength(8)>
    <Required>
    <Column("SEG_ARCHIVO", TypeName:="char")>
    Public Property SegArchivo As String

    <MaxLength(1)>
    <Column("SEG_INS", TypeName:="char")>
    Public Property SegIns As String

    <MaxLength(1)>
    <Column("SEG_UPD", TypeName:="char")>
    Public Property SegUpd As String

    <MaxLength(1)>
    <Column("SEG_DEL", TypeName:="char")>
    Public Property SegDel As String

End Class

