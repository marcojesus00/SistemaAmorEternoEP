Imports System.Data.Entity

Public Class FunamorContext
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=FunamorConnectionString")
        Database.SetInitializer(Of FunamorContext)(Nothing)

    End Sub

    Public Property FotosDeEmpleados As DbSet(Of FotoDeEmpleado)
    Public Property DocumentosDeEmpleados As DbSet(Of DocumentoDeEmpleado)
    Public Property TiposDeDocumentoDeEmpleado As DbSet(Of TipoDeDocumentoDeEmpleado)

    Public Property Contratos As DbSet(Of Contrato)
    Public Property Clientes As DbSet(Of Cliente)
    Public Property UrlClientes As DbSet(Of UrlCliente)
    Public Property Cobradores As DbSet(Of Cobrador)
    Public Property Empresas As DbSet(Of Empresa)
    Public Property Municipios As DbSet(Of Municipio)
    Public Property Vendedores As DbSet(Of Vendedor)
    Public Property Permisos As DbSet(Of Detseg)

End Class
