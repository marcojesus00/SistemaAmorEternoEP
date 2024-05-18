Imports System.Data.Entity

Public Class MyDbContext
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=MyDbContext")
        Database.SetInitializer(Of MyDbContext)(Nothing)

    End Sub

    Public Property FotosDeEmpleados As DbSet(Of FotoDeEmpleado)

End Class
