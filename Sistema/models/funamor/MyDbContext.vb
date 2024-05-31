Imports System.Data.Entity

Public Class MyDbContext
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=MyDbContext")
        Database.SetInitializer(Of MyDbContext)(Nothing)

    End Sub

    Public Property FotosDeEmpleados As DbSet(Of FotoDeEmpleado)
    Public Property DocumentosDeEmpleados As DbSet(Of DocumentoDeEmpleado)
    Public Property Contratos As DbSet(Of Contrato)
    Public Property Clientes As DbSet(Of Cliente)

End Class
