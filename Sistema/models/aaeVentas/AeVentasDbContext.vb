Imports System.Data.Entity

Public Class AeVentasDbContext
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=AEVentasDbConnection")
        Database.SetInitializer(Of FunamorDbContext)(Nothing)

    End Sub
    Public Property DatosDeClientes As DbSet(Of DatosDeCliente)
End Class
