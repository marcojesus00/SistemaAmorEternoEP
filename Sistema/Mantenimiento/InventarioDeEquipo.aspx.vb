Public Class InventarioDeEquipo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        FillDdl()

    End Sub
    Public Sub FillDdl()
        Using context As New MemorialContext()
            Dim queryCategories = "SELECT Id, Name FROM MEMORIAL.DBO.CATEGORIES"
            Dim queryInventory = "SELECT Id, Name FROM MEMORIAL.DBO.Models"
            Dim queryBrands = "SELECT Id, Name FROM MEMORIAL.DBO.Brands"


            Dim result As List(Of DDL) = context.Database.SqlQuery(Of DDL)(
                    queryCategories).ToList()
            DDLCategory.DataSource = result
            DDLCategory.DataTextField = "Name"
            DDLCategory.DataValueField = "Id"
            DDLCategory.DataBind()

            Dim resultInventory As List(Of DDL) = context.Database.SqlQuery(Of DDL)(
                 queryInventory).ToList()
            DdlModel.DataSource = resultInventory
            DdlModel.DataTextField = "Name"
            DdlModel.DataValueField = "Id"
            DdlModel.DataBind()


            Dim resultBrands As List(Of DDL) = context.Database.SqlQuery(Of DDL)(
                 queryBrands).ToList()
            DdlBrand.DataSource = resultBrands
            DdlBrand.DataTextField = "Name"
            DdlBrand.DataValueField = "Id"
            DdlBrand.DataBind()

        End Using
        Using funamor As New FunamorContext()
            Dim queryBrands = "SELECT [P_num_emple]
     as Id, Name FROM MEMORIAL.DBO.Brands"
            Dim resultBrands As List(Of DDL) = funamor.Database.SqlQuery(Of DDL)(
                 queryBrands).ToList()
            DdlBrand.DataSource = resultBrands
            DdlBrand.DataTextField = "Name"
            DdlBrand.DataValueField = "Id"
            DdlBrand.DataBind()

        End Using

    End Sub
    Public Class DDL
        Private _value As Object

        ' Getter and Setter for the property
        Public Property Id As Object
            Get
                Return _value
            End Get
            Set(ByVal value As Object)
                ' Optionally, add checks to ensure the value is either an Integer or String
                If TypeOf value Is Integer OrElse TypeOf value Is String Then
                    _value = value
                Else
                    Throw New ArgumentException("Value must be either an Integer or a String.")
                End If
            End Set
        End Property
        Public Property Name As String
    End Class

End Class