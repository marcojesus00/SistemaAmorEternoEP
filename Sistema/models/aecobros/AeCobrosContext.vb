Imports System.Data.Entity

Public Class AeCobrosContext
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=AECobrosConnectionString")
        Database.SetInitializer(Of AeCobrosContext)(Nothing)

    End Sub

    Public Property FotosDeEmpleados As DbSet(Of FotoDeEmpleado)



End Class
