Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
<Table("MVSAGEN")>
Public Class Mvsagen
    <Key>
    <Column("cveage", TypeName:="char")>
    <StringLength(10)>
    Public Property Cveage As String

    <Column("nomage", TypeName:="nvarchar")>
    <StringLength(155)>
    Public Property Nomage As String

    <Column("abrage", TypeName:="char")>
    <StringLength(15)>
    Public Property Abrage As String

    <Column("agecls", TypeName:="char")>
    <StringLength(10)>
    Public Property Agecls As String

    <Column("cvealm", TypeName:="char")>
    <StringLength(10)>
    Public Property Cvealm As String

    <Column("usuari", TypeName:="char")>
    <StringLength(15)>
    Public Property Usuari As String

    <Column("usupwd", TypeName:="char")>
    <StringLength(15)>
    Public Property Usupwd As String

    <Column("nvlpre", TypeName:="char")>
    <StringLength(15)>
    Public Property Nvlpre As String

    <Column("limcre", TypeName:="char")>
    <StringLength(15)>
    Public Property Limcre As String

    <Column("limcro", TypeName:="char")>
    <StringLength(15)>
    Public Property Limcro As String

    <Column("limcrv", TypeName:="char")>
    <StringLength(15)>
    Public Property Limcrv As String

    <Column("limcrd", TypeName:="char")>
    <StringLength(15)>
    Public Property Limcrd As String

    <Column("limcrn", TypeName:="char")>
    <StringLength(15)>
    Public Property Limcrn As String

    <Column("stsmov", TypeName:="char")>
    <StringLength(15)>
    Public Property Stsmov As String

    <Column("cvemov", TypeName:="char")>
    <StringLength(15)>
    Public Property Cvemov As String

    <Column("tipmov", TypeName:="char")>
    <StringLength(15)>
    Public Property Tipmov As String

    <Column("desmov", TypeName:="char")>
    <StringLength(20)>
    Public Property Desmov As String

    <Column("sermov", TypeName:="char")>
    <StringLength(30)>
    Public Property Sermov As String

    <Column("numser", TypeName:="char")>
    <StringLength(30)>
    Public Property Numser As String

    <Column("numped", TypeName:="numeric")>
    Public Property Numped As Decimal

    <Column("re1mov", TypeName:="char")>
    <StringLength(15)>
    Public Property Re1mov As String

    <Column("numpedsap", TypeName:="char")>
    <StringLength(15)>
    Public Property Numpedsap As String
End Class
