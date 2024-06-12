Imports System.Runtime.CompilerServices

Public Module EntityExtensions
    <Extension()>
    Public Function DeepCopy(ByVal entity As DatosDeCliente) As DatosDeCliente
        Dim newEntity As New DatosDeCliente()

        newEntity.Codigo = entity.Codigo
        newEntity.Nombre = entity.Nombre
        newEntity.Identidad = entity.Identidad
        newEntity.tributario = entity.tributario
        newEntity.cl_fecha = entity.cl_fecha
        newEntity.CL_STATUS = entity.CL_STATUS
        newEntity.CL_COMPANIA = entity.CL_COMPANIA
        newEntity.Direccion = entity.Direccion
        newEntity.Dir2_client = entity.Dir2_client
        newEntity.Dir3_client = entity.Dir3_client
        newEntity.Dir4_client = entity.Dir4_client
        newEntity.Dir5_client = entity.Dir5_client
        newEntity.Telefono = entity.Telefono
        newEntity.Celular = entity.Celular
        newEntity.Email = entity.Email
        newEntity.NombreDelConyuge = entity.NombreDelConyuge
        newEntity.TelefonoDelConyuge = entity.TelefonoDelConyuge
        newEntity.DireccionDelConyuge = entity.DireccionDelConyuge
        newEntity.CL_PARENt = entity.CL_PARENt
        newEntity.Cod_zona = entity.Cod_zona
        newEntity.CodigoVendedor = entity.CodigoVendedor
        newEntity.Cl_usuario = entity.Cl_usuario
        newEntity.Cl_terminal = entity.Cl_terminal
        newEntity.Longitud = entity.Longitud
        newEntity.Latitud = entity.Latitud
        newEntity.Secuencia = entity.Secuencia
        newEntity.Diavisita = entity.Diavisita
        newEntity.Semana = entity.Semana
        newEntity.Cierre = entity.Cierre
        newEntity.Tempo = entity.Tempo
        newEntity.Departamento = entity.Departamento
        newEntity.Municipio = entity.Municipio
        newEntity.circuito = entity.circuito
        newEntity.Liquida = entity.Liquida
        newEntity.Hora = entity.Hora
        newEntity.FechaDoc = entity.FechaDoc

        Return newEntity
    End Function
End Module
