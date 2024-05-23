Imports System.Data.Entity

Public Class FunamorDbContext
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=MyDbContext")
        Database.SetInitializer(Of FunamorDbContext)(Nothing)

    End Sub

    Public Property FotosDeEmpleados As DbSet(Of FotoDeEmpleado)
    Public Property DocumentosDeEmpleados As DbSet(Of DocumentoDeEmpleado)
End Class
