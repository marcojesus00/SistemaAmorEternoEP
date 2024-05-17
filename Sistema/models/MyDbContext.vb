Imports System.Data.Entity

Public Class MyDbContext
    Inherits DbContext

    'Public Property Usuarios As DbSet(Of Usuario)
    'Public Property Roles As DbSet(Of Rol)
    Private _connectionString As String ' Private field to store connection string


    Public Sub New(connectionString As String)
        Me._connectionString = connectionString
    End Sub
End Class
