Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("LOG_NULOS")>
Public Class LogNulo
    <Key>
    <Column("NUM_DOC", TypeName:="varchar")>
    <StringLength(16)>
    Public Property NumDoc As String

    <Column("USUARIO", TypeName:="varchar")>
    <StringLength(20)>
    Public Property Usuario As String

    <Column("FECHA", TypeName:="date")>
    Public Property Fecha As DateTime

    <Column("HORA", TypeName:="time")>
    Public Property Hora As TimeSpan

    <Column("ACCION", TypeName:="varchar")>
    <StringLength(20)>
    Public Property Accion As String

    <Column("MOTIVO", TypeName:="varchar")>
    <StringLength(500)>
    Public Property Motivo As String
End Class
