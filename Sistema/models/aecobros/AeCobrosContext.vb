Imports System.Data.Entity

Public Class AeCobrosContext
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=AECobrosConnectionString")
        Database.SetInitializer(Of AeCobrosContext)(Nothing)

    End Sub

    Public Property LogsNulos As DbSet(Of LogNulo)
    Public Property Agentes As DbSet(Of Mvsagen)
    Public Property NocobroModel As DbSet(Of NocobroModel)
    Public Property RecibosDeCobro As DbSet(Of ReciboDeCobro)
    Public Property Supervisados As DbSet(Of Superv)
    Public Property NoSupervisados As DbSet(Of Nosuper)




End Class
