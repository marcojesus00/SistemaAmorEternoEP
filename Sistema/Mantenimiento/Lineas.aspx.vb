Imports System.Data.SqlClient
Imports Sistema.InventarioDeEquipo

Public Class Lineas
    Inherits System.Web.UI.Page
    Public PageNumber As Integer = 1
    Public PageSize As Integer = 10
    Public TotalPages As Integer '
    Public TotalItems As Integer = 0
    Public itemText As String
    Public pagination As PaginationHelper = New PaginationHelper
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim Usuario_Aut = Session("Usuario_Aut")
            Session("BackPageUrl") = "~/principal.aspx"
            Dim thisPage = "~/Mantenimiento/Lineas.aspx"
            If Usuario_Aut IsNot Nothing Then
                'If True Then

                Usuario_Aut = Usuario_Aut.ToString().Trim().ToUpper()
                    If Session("Usuario") = "" OrElse Not AuthHelper.isAuthorized(Usuario_Aut, "LINEAS.R") Then
                        Response.Redirect("~/Principal.aspx")
                    End If

                    If Not IsPostBack Then
                        'Session("SelectedPageLineas") = 1
                        PnlOperation.Visible = False
                        fillDdl()
                        BindPrincipalGridView()
                    End If
                    'pnlMap.Visible = False

                    'AddHandler DashboardGridview.PageIndexChanging, AddressOf DashboardGridview_PageIndexChanging
                Else
                    Response.Redirect("~/Principal.aspx")

            End If

        Catch ex As Exception
            Dim msg = "Problema al la cargar página, por favor vuelva a intentarlo : " & ex.Message
            'RaiseEvent AlertGenerated(Me, New AlertEventArgs(msg, "danger"))
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))

            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try

    End Sub
    Protected Sub fillDdl()
        Dim queryLines = "SELECT  Id, [LocalNumber] Name FROM BusinessPhoneLines ORDER BY LocalNumber ASC"
        Dim queryAgent = "SELECT CAST(Codigo AS INT) as Id, Codigo+ ' ' + max(Nombre) as Name FROM (select codigo_cobr Codigo, nombre_cobr Nombre
                            from FUNAMOR..COBRADOR where COBR_STATUS='A'
                            UNION
                            select Cod_vendedo Codigo,Nombre_vend Nombre from FUNAMOR..VENDEDOR where VEND_STATUS='A') S
                           GROUP BY Codigo"
        Dim queryEmployee = "Select P_num_emple Id , P_nomb_empl Name from FUNAMOR..PLAEMP
Where P_status='A' AND LEN(P_nomb_empl)>3   ORDER BY Name"

        Dim queryDepartments = "SELECT 
Id, Name
  FROM [Memorial].[dbo].[CompanyDepartments]"


        Dim queryLeader = "SELECT [Codigo] as Id
      , Codigo + ' ' + Nombre  as Name

  FROM [Memorial].[dbo].[vw_ActiveSellersUnionAllCollectors]
  where codigo=lider"
        Dim queryZone = "select [vzon_codigo] as Id
      ,vzon_codigo + ' ' +[vzon_nombre] as Name
  FROM [FUNAMOR].[dbo].[VZONA]
  where len(vzon_nombre)>3"
        Using context As New MemorialContext()

            Dim resultLines As List(Of DDL) = context.Database.SqlQuery(Of DDL)(
                     queryLines).ToList()

            Dim resultAgents As List(Of DDL) = context.Database.SqlQuery(Of DDL)(
                     queryAgent).ToList()

            Dim resultDepartments As List(Of DDL) = context.Database.SqlQuery(Of DDL)(
                     queryDepartments).ToList()

            Dim resultZOnes As List(Of DDL2) = context.Database.SqlQuery(Of DDL2)(
                     queryZone).ToList()
            Dim resultLeaders As List(Of DDL2) = context.Database.SqlQuery(Of DDL2)(
                     queryLeader).ToList()

            DdlAvailableLines.DataSource = resultLines
            DdlAvailableLines.DataTextField = "Name"
            DdlAvailableLines.DataValueField = "Id"
            DdlAvailableLines.DataBind()
            DdlAvailableLines.Items.Insert(0, New ListItem("Linea", ""))

            DdlAgents.DataSource = resultAgents
            DdlAgents.DataTextField = "Name"
            DdlAgents.DataValueField = "Id"
            DdlAgents.DataBind()
            DdlAgents.Items.Insert(0, New ListItem("Gestor", ""))



            DdlAsignado.Items.Insert(0, New ListItem("Disponibles", "0"))
            DdlAsignado.Items.Insert(0, New ListItem("Asignados", "1"))


            'DdlDepartment.DataSource = resultDepartments
            'DdlDepartment.DataTextField = "Name"
            'DdlDepartment.DataValueField = "Id"
            'DdlDepartment.DataBind()
            'DdlDepartment.Items.Insert(0, New ListItem("Área", ""))


            DdlLeader.DataSource = resultLeaders
            DdlLeader.DataTextField = "Name"
            DdlLeader.DataValueField = "Id"
            DdlLeader.DataBind()
            DdlLeader.Items.Insert(0, New ListItem("Lider", ""))


            DdlZone.DataSource = resultZOnes
            DdlZone.DataTextField = "Name"
            DdlZone.DataValueField = "Id"
            DdlZone.DataBind()
            DdlZone.Items.Insert(0, New ListItem("Zonas", ""))



            'DdlDepartment.Items.Insert(0, New ListItem("Gestor", ""))

            'DdlDepartment.Items.Insert(0, New ListItem("Disponibles", "0"))
            'DdlDepartment.Items.Insert(0, New ListItem("Asignados", "1"))
        End Using

    End Sub

    Private Sub BindPrincipalGridView(Optional selectedPage As Integer = 1)


        Try
            Dim LocalNumber = txtSearchPhone.Text
            Dim agent = txtSearchAgent.Text
            Dim leader = DdlLeader.SelectedValue
            Dim zone = DdlZone.SelectedValue
            'Dim department = DdlDepartment.SelectedValue
            'Dim departmentName = DdlDepartment.SelectedItem.Text.Substring(0, 3)

            Dim isAssigned As Boolean = True
            Dim pageSize As Integer = 10
            Dim offset = (selectedPage - 1) * pageSize
            Dim msg = ""

            If DdlAsignado.SelectedValue = "0" Then

                isAssigned = False
            End If

            If leader.Trim.Length < 1 Then
                leader = Nothing
            End If
            If zone.Trim.Length < 1 Then
                zone = Nothing
            End If
            'If department.Trim.Length < 1 Then
            '    department = Nothing
            'End If

            'If departmentName.Contains("COB") Then
            'ElseIf departmentName.Contains("VEN") Then

            'Else
            '    departmentName = Nothing

            'End If
            Dim parameters As SqlParameter() = {
                                        New SqlParameter("@LocalNumber", "%" + LocalNumber + "%"),
                                        New SqlParameter("@Agent", "%" + agent + "%"),
                                        New SqlParameter("@isAssigned", isAssigned),
                                        New SqlParameter("@Leader", If(leader Is Nothing, DBNull.Value, leader)),
                                        New SqlParameter("@Zone", If(zone Is Nothing, DBNull.Value, zone)),
                                        New SqlParameter("@Offset", offset),
                                        New SqlParameter("@PageSize", pageSize)
                }
            'Dim parametersCount As SqlParameter() = {
            '                            New SqlParameter("@LocalNumber", "%" + LocalNumber + "%"),
            '                            New SqlParameter("@Agent", "%" + agent + "%"),
            '                            New SqlParameter("@isAssigned", isAssigned),
            '                            New SqlParameter("@Leader", If(leader Is Nothing, DBNull.Value, leader)),
            '                            New SqlParameter("@Zone", If(zone Is Nothing, DBNull.Value, zone)),
            '                            New SqlParameter("@Department", If(department Is Nothing, DBNull.Value, department)),
            '                            New SqlParameter("@Offset", offset),
            '                            New SqlParameter("@PageSize", pageSize)
            '    }



            Using context As New MemorialContext()
                context.Database.Log = Sub(s) System.Diagnostics.Debug.WriteLine(s)
                context.Database.Log = AddressOf LogSql

                Dim queryTableLines = "
SELECT 
   distinct BL.Id AS Codigo,
    BL.LocalNumber AS Numero,
    CASE WHEN BL.IsAssigned = 1 THEN 'Si' ELSE 'No' END AS Asignado,
    CASE WHEN BL.IsOperative = 1 THEN 'Si' ELSE 'No' END AS Operativo,
    CASE WHEN BL.MissedCall = 1 THEN 'Si' ELSE 'No' END AS Llamada_perdida,
    RTRIM(ISNULL(SL.Codigo + ' ' + SL.Nombre, '')) AS Gestor,
    isnull(Dep.Name,'') AS Departamento,
    isnull(SL.[Lider_venta], '') as Lider_venta,
    isnull(SL.[Zona_venta],'') as Zona_venta,
  isnull(SL.[Lider_cobro], '') as Lider_cobro,
    isnull(SL.[Zona_cobro],'') as Zona_cobro,
    ISNULL(FORMAT(BLA.CreationDate, 'dd MMM yyyy', 'es-ES'), 'Nunca') AS Modificacion,
    RTRIM(ISNULL(BLA.AssignedBy, '')) AS Modificado_por
FROM 
    Memorial..BusinessPhoneLines AS BL
LEFT JOIN (
    SELECT 
        BLA.LineId,
        BLA.CreationDate,
        BLA.AssignedBy,
        BLA.DepartmentId,
		bla.AgentCode,

        ROW_NUMBER() OVER (PARTITION BY BLA.LineId ORDER BY BLA.CreationDate DESC) AS RowNum
    FROM 
        Memorial..BusinessPhoneLinesAssignments AS BLA
) AS BLA ON BL.Id = BLA.LineId AND BLA.RowNum = 1
LEFT JOIN 

    Memorial..vw_ActiveSellersUnionAllCollectors2 AS SL
ON SL.Codigo COLLATE SQL_Latin1_General_CP1_CI_AS = BLA.AgentCode
LEFT JOIN 
    Memorial..CompanyDepartments AS Dep 
    ON BLA.DepartmentId = Dep.Id
WHERE 
    BL.LocalNumber LIKE @LocalNumber
    AND RTRIM(ISNULL(SL.Codigo + ' ' + SL.Nombre, '')) LIKE @Agent
AND BL.IsAssigned=@IsAssigned

ORDER BY 
    Codigo


"
                '                Dim countQuery = "
                '                Select  count(*) As n from(Select 
                '    BL.Id AS Codigo,
                '    BL.LocalNumber AS Numero,
                '    CASE WHEN BL.IsAssigned = 1 THEN 'Si' ELSE 'No' END AS Asignado,
                '    CASE WHEN BL.IsOperative = 1 THEN 'Si' ELSE 'No' END AS Operativo,
                '    CASE WHEN BL.MissedCall = 1 THEN 'Si' ELSE 'No' END AS Llamada_perdida,
                '    RTRIM(ISNULL(SL.Codigo + ' ' + SL.Nombre, '')) AS Gestor,
                '    isnull(Dep.Name,'') AS Departamento,
                '    isnull(SL.Lider, '') as Lider,
                '    isnull(SL.Zona,'') as Zona,
                '    ISNULL(FORMAT(BLA.CreationDate, 'dd MMM yyyy', 'es-ES'), 'Nunca') AS Modificacion,
                '    RTRIM(ISNULL(BLA.AssignedBy, '')) AS Modificado_por
                'FROM 
                '    Memorial..BusinessPhoneLines AS BL
                'LEFT JOIN (
                '    SELECT 
                '        BLA.LineId,
                '        BLA.CreationDate,
                '        BLA.AssignedBy,
                '        BLA.DepartmentId,
                '		bla.AgentCode,

                '        ROW_NUMBER() OVER (PARTITION BY BLA.LineId ORDER BY BLA.CreationDate DESC) AS RowNum
                '    FROM 
                '        Memorial..BusinessPhoneLinesAssignments AS BLA
                ') AS BLA ON BL.Id = BLA.LineId AND BLA.RowNum = 1
                'LEFT JOIN 
                '    Memorial..vw_ActiveSellersUnionAllCollectors AS SL 
                '    ON SL.Codigo COLLATE SQL_Latin1_General_CP1_CI_AS = BLA.AgentCode

                'LEFT JOIN 
                '    Memorial..CompanyDepartments AS Dep 
                '    ON BLA.DepartmentId = Dep.Id
                'WHERE 
                '    BL.LocalNumber LIKE @LocalNumber
                '    AND RTRIM(ISNULL(SL.Codigo + ' ' + SL.Nombre, '')) LIKE @Agent
                'AND BL.IsAssigned=@IsAssigned
                ') as s

                '                "
                Dim resultAgents As List(Of TableLinesDto) = context.Database.SqlQuery(Of TableLinesDto)(
                     queryTableLines, parameters).ToList()
                'resultAgents.Where(Function(c) c.Lider.Contains)
                If leader IsNot Nothing AndAlso leader.Trim.Length > 1 Then
                    resultAgents = resultAgents.Where(Function(r) r.Lider_cobro.Contains(leader) OrElse r.Lider_venta.Contains(leader)).ToList()
                End If
                If zone IsNot Nothing AndAlso zone.Trim.Length > 1 Then
                    resultAgents = resultAgents.Where(Function(r) r.Zona_cobro.Contains(zone) OrElse r.Zona_venta.Contains(zone)).ToList()
                End If
                'If department IsNot Nothing AndAlso department.Trim.Length > 1 Then
                '    resultAgents = resultAgents.Where(Function(r) r.Departamento.Contains(department.Substring(0, 4))).ToList()
                'End If
                Dim resultCount As Integer = resultAgents.Count()

                resultAgents = resultAgents.Skip(offset).Take(pageSize).ToList()
                'resultAgents.Count()
                '
                'context.Database.SqlQuery(Of Integer)(
                '     countQuery, parametersCount).FirstOrDefault()

                DashboardGridview.DataSource = Nothing

                DashboardGridview.DataSource = resultAgents
                DashboardGridview.DataBind()


                TotalItems = resultCount
                PageNumber = selectedPage
                TotalPages = Math.Ceiling(resultCount / pageSize)

                ' Update the Previous and Next buttons' enabled state
                Dim pages As New List(Of Integer)()
                For i As Integer = 1 To TotalPages
                    pages.Add(i)
                Next

                rptPager.DataSource = pagination.GetLimitedPageNumbers(TotalItems, pageSize, PageNumber, 3)
                rptPager.DataBind()
                lnkbtnPrevious.Enabled = PageNumber > 1
                lnkbtnNext.Enabled = PageNumber < TotalPages
                lblTotalCount.DataBind()
                If DashboardGridview.Rows.Count = 0 Then
                    msg = "No se encontraron resultados"
                Else
                    msg = "Mostrando primeros " & $"{resultAgents.Count} resultados."

                End If
            End Using
        Catch ex As SqlException
            Dim msg = "Problema con la base de datos, por favor vuelva a intentarlo : " & ex.Message
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)


        Catch ex As Exception
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try
    End Sub

    Function GetDigitalRoot(ByVal number As Integer) As Integer

        Return number Mod 10

    End Function
    Public Sub LogSql(s As String)
        System.Diagnostics.Debug.WriteLine(s)
    End Sub
    Protected Sub DashboardGridview_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try

            Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
            Dim digitalRoot = GetDigitalRoot(rowIndex)
            Dim row As GridViewRow = DashboardGridview.Rows(rowIndex)

            Dim keyValue As String = DashboardGridview.DataKeys(digitalRoot).Value.ToString()
            Dim cellIndex As Integer = 1 ' Replace with the desired column index
            Dim agentCodeValue As String = DashboardGridview.Rows(rowIndex).Cells(cellIndex).ToString().Substring(0, 4)
            If e.CommandName = "Operation" Then
                If Session("Usuario_Aut") Is Nothing OrElse Session("Usuario_Aut") = "" OrElse Not AuthHelper.isAuthorized(Session("Usuario_Aut"), "LINEAS.W") Then
                    AlertHelper.GenerateAlert("warning", "No tiene permiso", alertPlaceholder)
                Else
                    PnlOperation.Visible = True
                    If DdlAvailableLines.Items.FindByValue(keyValue) IsNot Nothing Then

                        DdlAvailableLines.SelectedValue = keyValue
                    Else
                        AlertHelper.GenerateAlert("danger", keyValue, alertPlaceholder)

                    End If
                    If DdlAgents.Items.FindByValue(agentCodeValue) IsNot Nothing Then
                        DdlAgents.SelectedValue = agentCodeValue
                    Else
                        DdlAgents.SelectedValue = ""
                    End If



                End If

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub DashboardGridview_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub DashboardGridview_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)
        BindPrincipalGridView()
    End Sub



    Protected Sub UpdateLines(AssignedBy, AgentCode, ModificationDate, IsOperative, IsAssigned, LineId)
        If AssignedBy Is Nothing Then
            AssignedBy = "PRUEBA"
        End If
        Dim result As Integer

        'If Integer.TryParse(EmployeeId, result) Then
        'Else
        '    EmployeeId = ""
        'End If
        If Integer.TryParse(AgentCode, result) Then
        Else
            AgentCode = ""
        End If

        Dim parameters As SqlParameter() = {
                        New SqlParameter("@ModificationDate", ModificationDate),
                                            New SqlParameter("@IsOperative", IsOperative),
            New SqlParameter("@IsAssigned", IsAssigned),
            New SqlParameter("@LineId", LineId),
            New SqlParameter("@AgentCode", AgentCode),
            New SqlParameter("@AssignedBy", AssignedBy)}
        Try
            Using context As New MemorialContext()
                Dim queryUpdatePhoneLine = "UPDATE [dbo].[BusinessPhoneLines]
   SET 
      [ModificationDate] = @ModificationDate
      ,[IsOperative] = @IsOperative
      ,[IsAssigned] = @IsAssigned
 WHERE Id=@LineId"

                Dim queryAssign = "INSERT INTO [dbo].[BusinessPhoneLinesAssignments]
           ([LineId]
           ,[AgentCode]
           ,[AssignedBy])
     VALUES
           (@LineId
           ,@AgentCode
           ,@AssignedBy)
"
                Dim queryAssignComplete = $" BEGIN TRY
BEGIN TRANSACTION
{queryUpdatePhoneLine}
{queryAssign}
COMMIT
SELECT 'EXITO'
END TRY


BEGIN CATCH
ROLLBACK
SELECT ERROR_MESSAGE()
END CATCH


"
                Dim resultAgents As String = context.Database.SqlQuery(Of String)(
         queryAssignComplete, parameters).FirstOrDefault()
                Dim alert = "success"
                If resultAgents.Length > 10 Then
                    alert = "danger"
                End If
                AlertHelper.GenerateAlert(alert, resultAgents, alertPlaceholder)

            End Using


        Catch ex As SqlException
            Dim msg = "Problema con la base de datos, por favor vuelva a intentarlo : " & ex.Message
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)


        Catch ex As Exception
            Dim msg = "Error, por favor vuelva a intentarlo : " & ex.Message
            DebugHelper.SendDebugInfo("danger", ex, Session("Usuario_Aut"))
            AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

        End Try
    End Sub
    Public Sub updatePhoneLInes(LineId, AgentCode, Optional IsOperative = True, Optional IsAssigned = True, Optional MissedCall = False)

        Dim ModificationDate = DateTime.Now

        Dim parameters As SqlParameter() = {
                        New SqlParameter("@ModificationDate", ModificationDate),
                                            New SqlParameter("@IsOperative", IsOperative),
            New SqlParameter("@IsAssigned", IsAssigned),
            New SqlParameter("@LineId", LineId),
            New SqlParameter("@AgentCode", AgentCode),
            New SqlParameter("@MissedCall", MissedCall)}

        Using context As New MemorialContext()
            Dim queryUpdatePhoneLine = "

BEGIN TRY
BEGIN TRANSACTION


UPDATE [dbo].[BusinessPhoneLines]
   SET 
      [ModificationDate] = @ModificationDate
      ,[IsOperative] = @IsOperative
      ,[IsAssigned] = @IsAssigned
        ,[MissedCall]=@MissedCall
 WHERE Id=@LineId


COMMIT
SELECT 'EXITO'
END TRY


BEGIN CATCH
ROLLBACK
SELECT ERROR_MESSAGE()
END CATCH
"

            Dim resultAgents As String = context.Database.SqlQuery(Of String)(
     queryUpdatePhoneLine, parameters).FirstOrDefault()
        End Using
    End Sub
    Protected Sub BtnAssign_Click(sender As Object, e As EventArgs)
        Dim ModificationDate = DateTime.Now()
        Dim IsOperative = True
        Dim IsAssigned = True
        Dim LineId = DdlAvailableLines.SelectedValue
        'Dim EmployeeId = DdlEmployees.SelectedValue
        Dim AgentCode = DdlAgents.SelectedValue
        Dim AssignedBy = Session("Usuario_Aut")
        UpdateLines(AssignedBy, AgentCode, ModificationDate, IsOperative, IsAssigned, LineId)
    End Sub


    Protected Sub BtnOutOfService_Click(sender As Object, e As EventArgs)
        Dim ModificationDate = DateTime.Now()
        Dim IsOperative = False
        Dim IsAssigned = False
        Dim LineId = DdlAvailableLines.SelectedValue
        Dim AgentCode = ""
        Dim AssignedBy = Session("Usuario_Aut")
        UpdateLines(AssignedBy, AgentCode, ModificationDate, IsOperative, IsAssigned, LineId)

    End Sub

    Protected Sub BtnUnassing_Click(sender As Object, e As EventArgs)
        Dim ModificationDate = DateTime.Now()
        Dim IsOperative = True
        Dim IsAssigned = False
        Dim LineId = DdlAvailableLines.SelectedValue
        Dim AgentCode = ""
        Dim AssignedBy = Session("Usuario_Aut")
        UpdateLines(AssignedBy, AgentCode, ModificationDate, IsOperative, IsAssigned, LineId)

    End Sub
    Protected Sub BtnNotAnswer_Click(sender As Object, e As EventArgs)
        Dim IsOperative = True
        Dim IsAssigned = True
        Dim LineId = DdlAvailableLines.SelectedValue
        'Dim EmployeeId = DdlEmployees.SelectedValue
        Dim AgentCode = DdlAgents.SelectedValue
        updatePhoneLInes(LineId:=LineId, AgentCode:=AgentCode, MissedCall:=True)

    End Sub

    Protected Sub lnkbtnPage_Click(sender As Object, e As EventArgs)
        Dim lnkButton As LinkButton = CType(sender, LinkButton)
        Dim resultInteger As Integer
        If Integer.TryParse(lnkButton.Text, resultInteger) Then
            Dim SelectedPageLineas As Integer = Integer.Parse(lnkButton.Text)
            Session("SelectedPageLineas") = SelectedPageLineas
            BindPrincipalGridView(SelectedPageLineas)
        End If

    End Sub

    Protected Sub lnkbtnPrevious_Click(sender As Object, e As EventArgs)

        If Session("SelectedPageLineas") IsNot Nothing AndAlso Session("SelectedPageLineas") > 1 Then
            Session("SelectedPageLineas") = Session("SelectedPageLineas") - 1

            BindPrincipalGridView(Session("SelectedPageLineas"))

        End If

    End Sub

    Protected Sub lnkbtnNext_Click(sender As Object, e As EventArgs)
        If Session("SelectedPageLineas") IsNot Nothing Then
            Session("SelectedPageLineas") = Session("SelectedPageLineas") + 1
            BindPrincipalGridView(Session("SelectedPageLineas"))
        End If

    End Sub


End Class