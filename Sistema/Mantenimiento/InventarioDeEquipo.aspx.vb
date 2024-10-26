Imports System.Data.SqlClient
Imports System.IO

Public Class InventarioDeEquipo
    Inherits System.Web.UI.Page
    Public PageNumber As Integer = 1
    Public PageSize As Integer = 10
    Public TotalPages As Integer '
    Public TotalItems As Integer = 0
    Public itemText As String
    Public pagination As PaginationHelper = New PaginationHelper

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            PnlAssign.Visible = False
            FillDdl()
            rebind()
        End If
    End Sub
    Public Sub FillDdl()
        Using context As New MemorialContext()
            Dim queryCategories = "SELECT Id, Name FROM MEMORIAL.DBO.CATEGORIES"
            Dim queryModels = "SELECT Id, Name FROM MEMORIAL.DBO.Models"
            Dim queryBrands = "SELECT Id, Name FROM MEMORIAL.DBO.Brands"
            Dim queryItems = "SELECT Id, Name FROM MEMORIAL.DBO.Inventory"
            Dim queryItemStatus = "SELECT Id, Name FROM MEMORIAL.DBO.Statuses"
            Dim queryBusinessDepartments = "SELECT Id, Name FROM MEMORIAL.DBO.[CompanyDepartments]"
            '      Dim result As List(Of DDL) = context.Database.SqlQuery(Of DDL)(
            '              queryCategories).ToList()
            '      DDLCategory.DataSource = result
            '      DDLCategory.DataTextField = "Name"
            '      DDLCategory.DataValueField = "Id"
            '      DDLCategory.DataBind()

            'Dim resultModel As List(Of DDL) = context.Database.SqlQuery(Of DDL)(
            '     queryModels).ToList()
            'DdlModel.DataSource = resultModel
            'DdlModel.DataTextField = "Name"
            'DdlModel.DataValueField = "Id"
            'DdlModel.DataBind()


            '      Dim resultBrands As List(Of DDL) = context.Database.SqlQuery(Of DDL)(
            '           queryBrands).ToList()
            '      DdlBrand.DataSource = resultBrands
            '      DdlBrand.DataTextField = "Name"
            '      DdlBrand.DataValueField = "Id"
            '      DdlBrand.DataBind()
            '      Dim resultItemStatus As List(Of DDL) = context.Database.SqlQuery(Of DDL)(
            'queryItemStatus).ToList()
            '      DdlStatus.DataSource = resultItemStatus
            '      DdlStatus.DataTextField = "Name"
            '      DdlStatus.DataValueField = "Id"
            '      DdlStatus.DataBind()
            Dim resultBussinesDepartments As List(Of DDL) = context.Database.SqlQuery(Of DDL)(
      queryBusinessDepartments).ToList()
            DdlBusinessDepartments.DataSource = resultBussinesDepartments
            DdlBusinessDepartments.DataTextField = "Name"
            DdlBusinessDepartments.DataValueField = "Id"
            DdlBusinessDepartments.DataBind()
        End Using
        Using funamor As New FunamorContext()
            Dim queryBrands = "SELECT [P_num_emple]
     as Id, P_nomb_empl as Name FROM [FUNAMOR].[dbo].[PLAEMP] WHERE LEN(P_nomb_empl)>5"
            Dim resultBrands As List(Of DDL) = funamor.Database.SqlQuery(Of DDL)(
                 queryBrands).ToList()
            DDLmployee.DataSource = resultBrands
            DDLmployee.DataTextField = "Name"
            DDLmployee.DataValueField = "Id"
            DDLmployee.DataBind()

            DdlBoss.DataSource = resultBrands
            DdlBoss.DataTextField = "Name"
            DdlBoss.DataValueField = "Id"
            DdlBoss.DataBind()



            Dim queryCity = "SELECT [Codigo_sucu]
     as Id, [Nombre_sucu] as Name FROM [FUNAMOR].[dbo].[SUCURSAL] WHERE LEN([Nombre_sucu] )>2"
            Dim resultCity As List(Of DDL2) = funamor.Database.SqlQuery(Of DDL2)(
                 queryCity).ToList()
            DdlCity.DataSource = resultCity
            DdlCity.DataTextField = "Name"
            DdlCity.DataValueField = "Id"
            DdlCity.DataBind()
        End Using

    End Sub
    Public Sub rebind()
        Using context As New MemorialContext()
            Dim queryCity = "SELECT  Id, SerialNumber Name FROM INVENTORY"

            Dim resultCity As List(Of DDL) = context.Database.SqlQuery(Of DDL)(
                     queryCity).ToList()

            DashboardGridview.DataSource = resultCity
            DashboardGridview.DataBind()
        End Using

    End Sub
    Public Function CreateAssingment()
        Dim msg = ""
        Using context As New MemorialContext()
            Dim itemId As Integer, employeeId As Integer, sellerCollectorId As String, user As String

            Dim query = "select I.Id, I.SerialNumber,M.NAME Model,B.Name Brand, C.Name DeviceType from Inventory I 
LEFT JOIN MODELS M ON I.ModelId = M.Id
LEFT JOIN Categories C ON C.Id = M.CategoryId
LEFT JOIN BRANDS B ON B.ID =M.BrandId WHERE I.Id=@Id
"
            itemId = TextBoxItemId.Text

            Dim device As Device = context.Database.SqlQuery(Of Device)(
                 query, New SqlParameter("@Id", itemId)).FirstOrDefault()




            Dim queryEMp = "SELECT isnull(E.P_nomb_empl,'') as Name,isnull(E.P_identidad,'') as Dni,isnull(B.PhoneNumber,0) as PhoneNumber FROM Employees E
LEFT JOIN BusinessPhoneLines B ON E.P_num_emple=B.EmployeeId 
 where e.P_num_emple=@Id
"
            employeeId = DDLmployee.SelectedValue

            Dim employee As Employee = context.Database.SqlQuery(Of Employee)(
                 queryEMp, New SqlParameter("@Id", employeeId)).FirstOrDefault()
            If employee.Dni.Length < 5 Then
                msg = "Corrija la identidad del empleado"
                AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)
            ElseIf employee.PhoneNumber < 2999 Then
                msg = "El empleado no tiene asignada una línea de teléfono empresarial"
                AlertHelper.GenerateAlert("danger", msg, alertPlaceholder)

            Else
                sellerCollectorId = ""
                user = "yo" 'Session("Usuario_Aut")
                SaveAsignment(itemId:=itemId, employeeId:=employeeId, sellerCollectorId:=sellerCollectorId, user:=user)
                Dim documentNumber As String, city As String, depto As String, area As String, boss As String, cel As String, deviceType As String, brand As String, model As String, description As String, serialNumber As String, emloyeeName As String, employeeDni As String
                documentNumber = ""
                city = DdlCity.Text.Trim
                depto = ""
                area = DdlBusinessDepartments.Text
                boss = DdlBoss.Text
                cel = employee.PhoneNumber
                deviceType = device.DeviceType
                brand = device.Brand
                model = device.Model
                description = TextBoxDescription.Text
                serialNumber = device.SerialNumber
                emloyeeName = employee.Name
                employeeDni = employee.Dni
                GenerateDeviceAssigmentDocoument(documentNumber:=documentNumber, city:=city, depto:=depto, area:=area, boss:=boss, cel:=cel, deviceType:=deviceType, brand:=brand, model:=model, description:=description, serialNumber:=serialNumber, emloyeeName:=emloyeeName, employeeDni:=employeeDni)
            End If

            Return True
        End Using

    End Function
    Public Sub SaveAsignment(itemId As Integer, employeeId As Integer, sellerCollectorId As String, user As String)
        Dim paramateres As SqlParameter() = {
                        New SqlParameter("@ItemId", SqlDbType.Int) With {.Value = itemId},
            New SqlParameter("@AssignedToEmployeeId", SqlDbType.Int) With {.Value = employeeId},
            New SqlParameter("@SellerCollectorId", SqlDbType.VarChar) With {.Value = sellerCollectorId},
            New SqlParameter("@User", SqlDbType.VarChar) With {.Value = user}
            }
        Dim query = "INSERT INTO [dbo].[Assignments]
           ([ItemId]
           ,[AssignedToEmployeeId]
           ,[SellerCollectorId]
           ,[User])
     VALUES
           (
           @ItemId,
           @AssignedToEmployeeId,
           @SellerCollectorId, 
           @User
)"
        Using context As New MemorialContext()
            context.Database.ExecuteSqlCommand(query, paramateres)

        End Using
    End Sub
    Public Class Device
        Public Property Id As Integer
        Public Property DeviceType As String
        Public Property Brand As String

        Public Property Model As String
        Public Property SerialNumber As String

    End Class
    Public Class Employee
        Public Property Name As String
        Public Property Dni As String
        Public Property PhoneNumber As Integer
    End Class
    Public Function GenerateDeviceAssigmentDocoument(documentNumber As String, city As String, depto As String, area As String, boss As String, cel As String, deviceType As String, brand As String, model As String, description As String, serialNumber As String, emloyeeName As String, employeeDni As String)
        Using context As New MemorialContext()

            Dim parameters As SqlParameter() = {
                    New SqlParameter("@DocumentNumber", documentNumber),
                    New SqlParameter("@Department", depto),
                    New SqlParameter("@City", city),
                    New SqlParameter("@Area", area),
                    New SqlParameter("@Boss", boss),
                    New SqlParameter("@EmployeeWorkPhone", cel),
                    New SqlParameter("@DeviceType", deviceType),
                    New SqlParameter("@Brand", brand),
                    New SqlParameter("@Model", model),
                    New SqlParameter("@SerialNumber", serialNumber),
                    New SqlParameter("@Description", description),
                    New SqlParameter("@EmployeeName", emloyeeName),
                                    New SqlParameter("@EmployeeDNI", employeeDni)
                }
            Dim sqlQuery = "EXEC [dbo].[GenerateHtmlAssignmentDocument]
		@DocumentNumber = @DocumentNumber,
		@Department = @Department,
		@City = @City,
		@Area = @Area,
		@Boss = @Boss,
		@EmployeeWorkPhone = @EmployeeWorkPhone,
		@DeviceType = @DeviceType,
		@Brand = @Brand,
		@Model = @Model,
		@SerialNumber = @SerialNumber,
		@Description = @Description,
		@EmployeeName = @EmployeeName,
		@EmployeeDNI = @EmployeeDNI
"
            Dim html = context.Database.SqlQuery(Of String)(sqlQuery, parameters).FirstOrDefault()




            Dim pdf As Byte() = ApiService.PostHtmlAndReceivePdf(html)

            ' Stream  to the client
            Using memoryStream As New MemoryStream(pdf)
                Dim fileName = emloyeeName + "_" + documentNumber
                ' Set the content type and headers for the response
                HttpContext.Current.Response.ContentType = "application/pdf"
                HttpContext.Current.Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}.pdf")

                ' Write the memory stream to the output stream of the response
                memoryStream.WriteTo(HttpContext.Current.Response.OutputStream)

                ' Ensure the response is flushed and sent to the client
                HttpContext.Current.Response.Flush()
                HttpContext.Current.Response.End()

                Return True
            End Using

            ' End the response to prevent any additional content from being sent
            HttpContext.Current.Response.Flush()
            HttpContext.Current.Response.End()
        End Using

    End Function
    Public Class DDL

        ' Getter and Setter for the property
        Public Property Id As Integer
        Public Property Name As String
    End Class
    Public Class TableLinesDto

        ' Getter and Setter for the property
        Public Property Codigo As Integer
        Public Property Numero As String
        Public Property Asignado As String
        Public Property Operativo As String
        Public Property Nombre As String
        Public Property Gestor As String
        Public Property Modificacion As String
        Public Property Modificado_por As String


    End Class
    Public Class DDL2

        ' Getter and Setter for the property
        Public Property Id As String
        Public Property Name As String
    End Class

    Protected Sub DashboardGridview_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "AssingThisItem" Then
            ' Get the command argument (in this case, the ID of the row)
            PnlAssign.Visible = True
            Dim id As Integer = Convert.ToInt32(e.CommandArgument)
            TextBoxItemId.Text = id
            TextBoxDescription.Text = "•	Bateria Original
•	Incluye: Cargador original 
•	Color: 
•	Estado del equipo: Nuevo
"
            ' Add your action logic here, e.g., edit, delete, etc.

        End If
    End Sub
End Class