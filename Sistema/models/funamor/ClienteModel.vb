Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("CLIENTES")>
Public Class Cliente
    <Key>
    <Column("Codigo_clie", TypeName:="char")>
    <StringLength(10)>
    Public Property Codigo As String

    <Column("Nombre_clie", TypeName:="char")>
    <StringLength(55)>
    Public Property Nombre As String

    <Column("CTIPOCLI", TypeName:="char")>
    <StringLength(1)>
    Public Property TipoCliente As String

    <Column("clientenp", TypeName:="char")>
    <StringLength(10)>
    Public Property ClienteNP As String

    <Column("cod_listpre", TypeName:="char")>
    <StringLength(6)>
    Public Property CodigoListaPrecio As String

    <Column("cod_compani", TypeName:="char")>
    <StringLength(1)>
    Public Property CodigoCompania As String

    <Column("clisucursal", TypeName:="char")>
    <StringLength(2)>
    Public Property SucursalCliente As String

    <Column("mayorista", TypeName:="char")>
    <StringLength(1)>
    Public Property Mayorista As String

    <Column("CL_NIVEL", TypeName:="char")>
    <StringLength(1)>
    Public Property NivelCliente As String

    <Column("cl_descto", TypeName:="smallint")>
    Public Property DescuentoCliente As Short?

    <Column("cl_fecha", TypeName:="datetime")>
    Public Property FechaCliente As DateTime?

    <Column("CL_STATUS", TypeName:="char")>
    <StringLength(1)>
    Public Property EstadoCliente As String

    <Column("CL_COMPANIA", TypeName:="char")>
    <StringLength(1)>
    Public Property CompaniaCliente As String

    <Column("Dir_cliente", TypeName:="char")>
    <StringLength(80)>
    Public Property DireccionCliente As String

    <Column("Dir2_client", TypeName:="char")>
    <StringLength(80)>
    Public Property Direccion2Cliente As String

    <Column("Dir3_client", TypeName:="char")>
    <StringLength(80)>
    Public Property Direccion3Cliente As String

    <Column("Dir4_client", TypeName:="char")>
    <StringLength(80)>
    Public Property Direccion4Cliente As String

    <Column("Limite_cred", TypeName:="money")>
    Public Property LimiteCredito As Decimal?

    <Column("Saldo_actua", TypeName:="money")>
    Public Property SaldoActual As Decimal?

    <Column("ESALDO_ACTU", TypeName:="money")>
    Public Property SaldoActualE As Decimal?

    <Column("CSALDO_ACTU", TypeName:="money")>
    Public Property SaldoActualC As Decimal?

    <Column("ISALDO", TypeName:="money")>
    Public Property SaldoI As Decimal?

    <Column("catalogo", TypeName:="char")>
    <StringLength(2)>
    Public Property Catalogo As String

    <Column("Telef_clien", TypeName:="char")>
    <StringLength(20)>
    Public Property TelefonoCliente As String

    <Column("Cod_zona", TypeName:="char")>
    <StringLength(3)>
    Public Property CodigoZona As String

    <Column("representan", TypeName:="char")>
    <StringLength(40)>
    Public Property Representante As String

    <Column("identidad", TypeName:="char")>
    <StringLength(15)>
    Public Property Identidad As String

    <Column("tributario", TypeName:="char")>
    <StringLength(15)>
    Public Property Tributario As String

    <Column("cl_usuario", TypeName:="char")>
    <StringLength(10)>
    Public Property UsuarioCliente As String

    <Column("cl_terminal", TypeName:="char")>
    <StringLength(8)>
    Public Property TerminalCliente As String

    <Column("tipo_client", TypeName:="char")>
    <StringLength(1)>
    Public Property TipoCliente2 As String

    <Column("CUECLIEN", TypeName:="char")>
    <StringLength(10)>
    Public Property CueCliente As String

    <Column("CL_DESCFAC", TypeName:="char")>
    <StringLength(1)>
    Public Property DescFac As String

    <Column("CL_MORA", TypeName:="char")>
    <StringLength(1)>
    Public Property Mora As String

    <Column("CL_CAPITALI", TypeName:="char")>
    <StringLength(1)>
    Public Property Capital As String

    <Column("CL_FAX", TypeName:="char")>
    <StringLength(20)>
    Public Property Fax As String
    <ForeignKey("VendedorNav")>
    <Column("CL_VENDEDOR", TypeName:="char")>
    <StringLength(8)>
    Public Property CodigoVendedor As String

    <Column("CL_CELULAR", TypeName:="char")>
    <StringLength(20)>
    Public Property Celular As String

    <Column("CL_EXENTO", TypeName:="char")>
    <StringLength(1)>
    Public Property Exento As String

    <Column("cl_conyunom", TypeName:="char")>
    <StringLength(40)>
    Public Property ConyugueNombre As String

    <Column("cl_conyutel", TypeName:="char")>
    <StringLength(20)>
    Public Property ConyugueTelefono As String

    <Column("cl_conyudir", TypeName:="char")>
    <StringLength(80)>
    Public Property ConyugueDireccion As String

    <ForeignKey("CobradorNav")>
    <Column("cl_cobrador", TypeName:="char")>
    <StringLength(5)>
    Public Property CodigoCobrador As String

    <Column("CL_FECINAC", TypeName:="datetime")>
    Public Property FechaNacimientoCliente As DateTime?

    <Column("cl_fecobra", TypeName:="datetime")>
    Public Property FechaCobro As DateTime?

    <Column("cl_stat2", TypeName:="char")>
    <StringLength(1)>
    Public Property Estado2 As String

    <Column("CL_PARENt", TypeName:="char")>
    <StringLength(20)>
    Public Property Parent As String

    <Column("Dir5_client", TypeName:="char")>
    <StringLength(80)>
    Public Property Direccion5Cliente As String

    <Column("CL_2conynom", TypeName:="char")>
    <StringLength(40)>
    Public Property SegundoConyugueNombre As String

    <Column("cl_2conytel", TypeName:="char")>
    <StringLength(20)>
    Public Property SegundoConyugueTelefono As String

    <Column("cl_2condire", TypeName:="char")>
    <StringLength(80)>
    Public Property SegundoConyugueDireccion As String

    <Column("cl_2parent", TypeName:="char")>
    <StringLength(20)>
    Public Property SegundoParent As String

    <Column("SUBIVIEJO", TypeName:="char")>
    <StringLength(1)>
    Public Property SubIviejo As String

    <Column("CLIENTEREDU", TypeName:="char")>
    <StringLength(6)>
    Public Property ClienteReducido As String

    <Column("CL_EMAIL", TypeName:="char")>
    <StringLength(80)>
    Public Property Email As String

    <Column("VZCODIGO", TypeName:="char")>
    <StringLength(4)>
    Public Property CodigoVZ As String

    <Column("CL_COMENT", TypeName:="char")>
    <StringLength(40)>
    Public Property Comentario As String

    <Column("cl_usumodi", TypeName:="char")>
    <StringLength(10)>
    Public Property UsuarioModificacion As String

    <Column("longitud", TypeName:="decimal")>
    Public Property Longitud As Decimal?

    <Column("latitud", TypeName:="decimal")>
    Public Property Latitud As Decimal?

    <Column("secuencia", TypeName:="int")>
    Public Property Secuencia As Integer?

    <Column("diavisita", TypeName:="smallint")>
    Public Property DiaVisita As Short?

    <Column("semana", TypeName:="smallint")>
    Public Property Semana As Short?

    <Column("cl_fechanac", TypeName:="datetime")>
    Public Property FechaNacimiento As DateTime?

    <Column("VZONAPP", TypeName:="char")>
    <StringLength(4)>
    Public Property VZONAPP As String

    <Column("VZONVIEJ", TypeName:="char")>
    <StringLength(4)>
    Public Property VZONVIEJ As String

    <Column("CLNUMNUEVP", TypeName:="smallint")>
    Public Property NumeroNuevoVP As Short?

    <Column("CLVALNUEVP", TypeName:="money")>
    Public Property ValorNuevoVP As Decimal?

    <Column("CLFECNUEVP", TypeName:="datetime")>
    Public Property FechaNuevoVP As DateTime?

    <Column("CLICOMULTI", TypeName:="smallint")>
    Public Property ICN As Short?
    Public Overridable Property CobradorNav As Cobrador
    Public Overridable Property VendedorNav As Vendedor


End Class





<Table("UrlClientes")>
Public Class UrlCliente

    <Key>
    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    <Column("idline")>
    Public Property Id As Integer

    <ForeignKey("Cliente")>
    <MaxLength(12)>
    <Column("Codigo_clie", TypeName:="varchar")>
    Public Property CodigoCliente As String

    <Column("FechaSys")>
    Public Property FechaSys As DateTime?

    <MaxLength(10)>
    <Column("Hora", TypeName:="varchar")>
    Public Property Hora As String

    <Column("FechaMod")>
    Public Property FechaModificacion As Date?

    <MaxLength(200)>
    <Column("Comentario", TypeName:="varchar")>
    Public Property Comentario As String

    <Column("NombreArchivo", TypeName:="varchar(max)")>
    Public Property NombreDelDocumento As String

    <Column("NombreArchivoCorrelativo", TypeName:="varchar(max)")>
    Public Property NombreDelArchivo As String

    <Column("Ruta", TypeName:="varchar(max)")>
    Public Property RutaDelDirectorio As String

    <Column("ImagenUrl", TypeName:="varchar(max)")>
    Public Property RutaDelArchivo As String

    <MaxLength(10)>
    <Column("Terminal", TypeName:="varchar")>
    Public Property Terminal As String

    <MaxLength(12)>
    <Column("usuario", TypeName:="varchar")>
    Public Property Usuario As String

    <MaxLength(25)>
    <Column("Referencia", TypeName:="varchar")>
    Public Property Referencia As String

    <Column("Estado")>
    Public Property Estado As Boolean?

    <Column("ParaSala")>
    Public Property ParaSala As Boolean?
    Public Overridable Property Cliente As Cliente

End Class
