Imports System.Data.Entity

Public Class MemorialContext
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=MemorialConnectionString")
        Database.SetInitializer(Of FunamorContext)(Nothing)

    End Sub



End Class
