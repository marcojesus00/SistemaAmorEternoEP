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



        Using context As New MemorialContext()

            Dim resultLines As List(Of DDL) = context.Database.SqlQuery(Of DDL)(
                     queryLines).ToList()

            Dim resultAgents As List(Of DDL) = context.Database.SqlQuery(Of DDL)(
                     queryAgent).ToList()

            Dim resultDepartments As List(Of DDL) = context.Database.SqlQuery(Of DDL)(
                     queryDepartments).ToList()
            DdlAvailableLines.DataSource = resultLines
            DdlAvailableLines.DataTextField = "Name"
            DdlAvailableLines.DataValueField = "Id"
            DdlAvailableLines.DataBind()
            DdlAvailableLines.Items.Insert(0, New ListItem("Linea", ""))


            If DdlAvailableLines.SelectedValue.Length > 1 Then
                'Dim q sele
            End If



            DdlAgents.DataSource = resultAgents
            DdlAgents.DataTextField = "Name"
            DdlAgents.DataValueField = "Id"
            DdlAgents.DataBind()
            DdlAgents.Items.Insert(0, New ListItem("Gestor", ""))

            DdlAsignado.Items.Insert(0, New ListItem("Disponibles", "0"))
            DdlAsignado.Items.Insert(0, New ListItem("Asignados", "1"))


            DdlDepartment.DataSource = resultDepartments
            DdlDepartment.DataTextField = "Name"
            DdlDepartment.DataValueField = "Id"
            DdlDepartment.DataBind()
            'DdlDepartment.Items.Insert(0, New ListItem("Gestor", ""))

            'DdlDepartment.Items.Insert(0, New ListItem("Disponibles", "0"))
            'DdlDepartment.Items.Insert(0, New ListItem("Asignados", "1"))
        End Using

    End Sub

    Private Sub BindPrincipalGridView(Optional selectedPage As Integer = 1)


        Try
            Dim LocalNumber = txtSearchPhone.Text
            Dim agent = txtSearchAgent.Text
            Dim areAssigned As Boolean = True
            Dim pageSize As Integer = 10
            Dim offset = (selectedPage - 1) * pageSize
            Dim msg = ""

            If DdlAsignado.SelectedValue = "0" Then

                areAssigned = False
            End If

            Dim parameters As SqlParameter() = {
                    New SqlParameter("@LocalNumber", "%" + LocalNumber + "%"),
                                                            New SqlParameter("@Agent", "%" + agent + "%"),
                                        New SqlParameter("@areAssigned", areAssigned),
                                        New SqlParameter("@Offset", offset),
                                        New SqlParameter("@PageSize", pageSize)
                }
            Dim parametersCount As SqlParameter() = {
                    New SqlParameter("@LocalNumber", "%" + LocalNumber + "%"),
                                                                                New SqlParameter("@Agent", "%" + agent + "%"),
                                        New SqlParameter("@areAssigned", areAssigned),
                                        New SqlParameter("@Offset", offset),
                                        New SqlParameter("@PageSize", pageSize)
                }




            Using context As New MemorialContext()
                Dim queryTableLines = "
SELECT * FROM (SELECT BL.Id Codigo,BL.LocalNumber Numero, CASE 
        WHEN BL.IsAssigned = 1 THEN 'Si'
        ELSE 'No'
    END AS Asignado , CASE 
        WHEN BL.IsOperative = 1 THEN 'Si'
        ELSE 'No'
    END  Operativo ,
	CASE 
        WHEN BL.MissedCall = 1 THEN 'Si'
        ELSE 'No'
    END  Llamada_perdida ,
RTRIM(ISNULL(SL.Codigo+' '+SL.Nombre,'')) Gestor,
Dep.Name Departamento,
SL.Lider ,
SL.Zona,
IsNull(FORMAT(BLA.CreationDate , 'dd MMM yyyy', 'es-ES'),'Nunca') Modificacion,
RTRIM(ISNULL(BLA.AssignedBy,'')) Modificado_por
FROM Memorial..BusinessPhoneLines BL
LEFT JOIN (
SELECT 
 BLA.*,
 ROW_NUMBER() OVER (PARTITION BY LineId ORDER BY CreationDate DESC)
 AS RowNum
FROM Memorial..BusinessPhoneLinesAssignments
BLA ) BLA ON
BL.Id= BLA.LineId AND BLA.RowNum = 1

LEFT JOIN Memorial..vw_ActiveSellersUnionAllCollectors SL 
ON SL.Codigo COLLATE SQL_Latin1_General_CP1_CI_AS = BLA.AgentCode

LEFT JOIN Memorial..CompanyDepartments Dep ON
bla.DepartmentId=dep.Id

WHERE BL.LocalNumber LIKE @LocalNumber AND BL.IsAssigned =@areAssigned
) S WHERE Gestor LIKE @Agent
ORDER BY 
    Codigo
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
"
                Dim countQuery = "

SELECT COUNT(Codigo) FROM (SELECT BL.Id Codigo,
RTRIM(ISNULL(SL.Codigo+' '+SL.Nombre,'')) Gestor

FROM Memorial..BusinessPhoneLines BL
LEFT JOIN (
SELECT 
 BLA.*,
 ROW_NUMBER() OVER (PARTITION BY LineId ORDER BY CreationDate DESC)
 AS RowNum
FROM Memorial..BusinessPhoneLinesAssignments
BLA ) BLA ON
BL.Id= BLA.LineId AND BLA.RowNum = 1
LEFT JOIN Memorial..vw_ActiveSellersUnionAllCollectors SL 
ON SL.Codigo COLLATE SQL_Latin1_General_CP1_CI_AS = BLA.AgentCode

LEFT JOIN Memorial..CompanyDepartments Dep ON
bla.DepartmentId=dep.Id

WHERE BL.LocalNumber LIKE @LocalNumber AND BL.IsAssigned =@areAssigned
) S WHERE Gestor LIKE @Agent
"
                Dim resultAgents As List(Of TableLinesDto) = context.Database.SqlQuery(Of TableLinesDto)(
                     queryTableLines, parameters).ToList()
                Dim resultCount As Integer = context.Database.SqlQuery(Of Integer)(
                     countQuery, parametersCount).FirstOrDefault()

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

                rptPager.DataSource = pagination.GetLimitedPageNumbers(TotalItems, PageSize, PageNumber, 3)
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

    Protected Sub DashboardGridview_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try

            Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
            Dim digitalRoot = GetDigitalRoot(rowIndex)
            Dim row As GridViewRow = DashboardGridview.Rows(rowIndex)

            Dim keyValue As String = DashboardGridview.DataKeys(digitalRoot).Value.ToString()

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