Imports System.Data.Entity

Public Class AeVentasContext
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=AEVentasConnectionString")
        Database.SetInitializer(Of AeVentasContext)(Nothing)

    End Sub

    Public Property DeptoZona As DbSet(Of DeptoZona)
    'Public Property Agentes As DbSet(Of Mvsagen)
    'Public Property NocobroModel As DbSet(Of NocobroModel)
    'Public Property RecibosDeCobro As DbSet(Of ReciboDeCobro)
    'Public Property Supervisados As DbSet(Of Superv)
    'Public Property NoSupervisados As DbSet(Of Nosuper)




End Class
