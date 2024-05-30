Imports System.ComponentModel.DataAnnotations

Imports System.ComponentModel.DataAnnotations.Schema

<Table("FotoDeEmpleado")>
Public Class FotoDeEmpleado
    <Key>
    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property Id As Integer

    <ForeignKey("Empleado")>
    Public Property NumeroDeEmpleado As Integer
    Public Property Ruta As String
    Public Overridable Property Empleado As DatosDeEmpleado

End Class

<Table("DocumentosDeEmpleado")>
Public Class DocumentoDeEmpleado
    <Key>
    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property Id As Integer

    <ForeignKey("Empleado")>
    Public Property NumeroDeEmpleado As Integer
    Public Property NombreDelArchivo As String
    Public Property FechaDeCreacion As DateTime
    Public Property Ruta As String
    Public Property Descripcion As String
    Public Property Archivado As Boolean
    Public Overridable Property Empleado As DatosDeEmpleado


End Class


<Table("PLAEMP")>
Public Class DatosDeEmpleado
    <Key>
    Public Property P_num_emple As Integer

    Public Property P_nomb_empl As String

    Public Property P_fecha_can As DateTime?

    Public Property P_causa_can As String

    Public Property P_cod_sucur As String

    Public Property P_prestacio As Decimal?

    Public Property P_status As String

    Public Property p_devpresta As Decimal?

    Public Property P_PLANBAJ As String

    Public Property viejcodigo As Integer?

    Public Property p_codvend As String

    Public Property p_fecha_ent As DateTime?

    Public Property p_nomb_entr As String

    Public Property p_nomb_cont As String

    Public Property P_t_emplead As String

    Public Property P_fecha_ing As DateTime?

    Public Property P_Isr_mensu As Decimal?

    Public Property P_dir_emple As String

    Public Property P_estado_ci As String

    Public Property P_dependien As Short?

    Public Property P_fecha_nac As DateTime?

    Public Property P_lugar_nac As String

    Public Property P_sexo As String

    Public Property P_contrato As String

    Public Property P_cod_depar As String

    Public Property P_cod_puest As String

    Public Property P_sueldo_ac As Decimal?

    Public Property p_sueldo_di As Decimal?

    Public Property P_carnet_ih As String

    Public Property P_carnet_fo As String

    Public Property P_identidad As String

    Public Property P_tipo_plan As String

    Public Property P_HORAS_TRA As Short?

    Public Property P_temporal As String

    Public Property p_e_ihss As Decimal?

    Public Property p_e_pihss As String

    Public Property p_e_fosovi As String

    Public Property p_extras As String

    Public Property p_cheque As String

    Public Property p_planmedic As Decimal?

    Public Property p_numcuenta As String

    Public Property p_deduc As Decimal?

    Public Property P_cliente As String

    Public Property p_ctasueldo As String

    Public Property p_ctaextras As String

    Public Property P_BONIFICA As Decimal?

    Public Property p_codpresta As String

    Public Property P_selplani As String

    Public Property p_provee As String

    Public Property P_TOPEDEDUC As Decimal?

    Public Property p_deveante As Decimal?

    Public Property P_VALCOMBUS As Decimal?

    Public Property P_PAGMEYPA As String

    Public Property P_LETMOTO As Decimal?

    Public Property P_NUMLMOTO As Short?

    Public Property p_feclmoto As DateTime?

    Public Property p_fecinihss As DateTime?

    Public Property p_tranck As String

    Public Property p_cliente2 As String

    Public Property p_cta13avo As String

    Public Property p_Cta14avo As String

    Public Property p_ctavaca As String

    Public Property P_EMPDEPRE As String

    Public Property P_IHSEMPRE As String

    Public Property P_PORCENT As Decimal?

End Class


