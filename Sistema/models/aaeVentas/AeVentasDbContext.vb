Imports System.Data.Entity
Imports System.Data.SqlClient

Public Class AeVentasDbContext
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=AEVentasDbConnection")
        Database.SetInitializer(Of AeVentasDbContext)(Nothing)

    End Sub
    Public Property DatosDeClientes As DbSet(Of DatosDeCliente)
    Public Property MunicipiosZonasDepartamentos As DbSet(Of MunicipioZonaDepartamento)
    Public Property LogsEdicionesCLIENTESN As DbSet(Of LogEdicionCLIENTESN)
    Public Sub ExecuteVIENECLIENTESN_A3(
        ByVal Codigo_clie As String,
        ByVal Nombre_clie As String,
        ByVal identidad As String,
        ByVal tributario As String,
        ByVal CL_STATUS As String,
        ByVal CL_COMPANIA As String,
        ByVal Dir_cliente As String,
        ByVal Dir2_client As String,
        ByVal Dir3_client As String,
        ByVal Dir4_client As String,
        ByVal Dir5_client As String,
        ByVal Telef_clien As String,
        ByVal CL_CELULAR As String,
        ByVal CL_EMAIL As String,
        ByVal cl_conyunom As String,
        ByVal cl_conyutel As String,
        ByVal cl_conyudir As String,
        ByVal CL_PARENt As String,
        ByVal Cod_zona As String,
        ByVal CL_VENDEDOR As String,
        ByVal cl_usuario As String,
        ByVal cl_terminal As String,
        ByVal longitud As String,
        ByVal latitud As String,
        ByVal secuencia As String,
        ByVal diavisita As String,
        ByVal semana As String,
        ByVal cierre As String,
        ByVal tempo As String,
        ByVal departa As String,
        ByVal municipio As String,
        ByVal circuito As String,
        ByVal liquida As String
    )
        Dim parameters As SqlParameter() = {
            New SqlParameter("@Codigo_clie", Codigo_clie),
            New SqlParameter("@Nombre_clie", Nombre_clie),
            New SqlParameter("@identidad", identidad),
            New SqlParameter("@tributario", tributario),
            New SqlParameter("@CL_STATUS", CL_STATUS),
            New SqlParameter("@CL_COMPANIA", CL_COMPANIA),
            New SqlParameter("@Dir_cliente", Dir_cliente),
            New SqlParameter("@Dir2_client", Dir2_client),
            New SqlParameter("@Dir3_client", Dir3_client),
            New SqlParameter("@Dir4_client", Dir4_client),
            New SqlParameter("@Dir5_client", Dir5_client),
            New SqlParameter("@Telef_clien", Telef_clien),
            New SqlParameter("@CL_CELULAR", CL_CELULAR),
            New SqlParameter("@CL_EMAIL", CL_EMAIL),
            New SqlParameter("@cl_conyunom", cl_conyunom),
            New SqlParameter("@cl_conyutel", cl_conyutel),
            New SqlParameter("@cl_conyudir", cl_conyudir),
            New SqlParameter("@CL_PARENt", CL_PARENt),
            New SqlParameter("@Cod_zona", Cod_zona),
            New SqlParameter("@CL_VENDEDOR", CL_VENDEDOR),
            New SqlParameter("@cl_usuario", cl_usuario),
            New SqlParameter("@cl_terminal", cl_terminal),
            New SqlParameter("@longitud", longitud),
            New SqlParameter("@latitud", latitud),
            New SqlParameter("@secuencia", secuencia),
            New SqlParameter("@diavisita", diavisita),
            New SqlParameter("@semana", semana),
            New SqlParameter("@cierre", cierre),
            New SqlParameter("@tempo", tempo),
            New SqlParameter("@departa", departa),
            New SqlParameter("@municipio", municipio),
            New SqlParameter("@circuito", circuito),
            New SqlParameter("@liquida", liquida)
        }

        Database.ExecuteSqlCommand("EXEC VIENECLIENTESN_A3 @Codigo_clie, @Nombre_clie, @identidad, @tributario, @CL_STATUS, @CL_COMPANIA, @Dir_cliente, @Dir2_client, @Dir3_client, @Dir4_client, @Dir5_client, @Telef_clien, @CL_CELULAR, @CL_EMAIL, @cl_conyunom, @cl_conyutel, @cl_conyudir, @CL_PARENt, @Cod_zona, @CL_VENDEDOR, @cl_usuario, @cl_terminal, @longitud, @latitud, @secuencia, @diavisita, @semana, @cierre, @tempo, @departa, @municipio, @circuito, @liquida", parameters)
    End Sub
End Class
