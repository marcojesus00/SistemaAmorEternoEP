<%@ Page Title="Líneas" Language="vb" AutoEventWireup="false" MasterPageFile="~/Dashboards/Dashboard.master" CodeBehind="Lineas.aspx.vb" Inherits="Sistema.Lineas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Filters" runat="server">
        <h2 class=" text-center">Mantenimiento de líneas celulares</h2>

        <div class="container-fluid container-lg">
                <div class="col-12  mb-4">
                      <div class="card">
                          <div class=" card-header">

                                      <div class="card-title">Filtros</div>


                          </div>
        <div class="card-body">
                            <div class="row">

                            <div class="col-lg-2  col-md-6 col-12 pb-2 ">
                                    <asp:TextBox runat="server" Id="txtSearchPhone" Placeholder="Número de teléfono"  CssClass="form-control form-control-sm"></asp:TextBox>


                            </div>
              <div class="col-lg-2  col-md-6 col-12 pb-2 ">
                    <asp:DropDownList runat="server" Id="DdlAsignado" CssClass="form-control form-control-sm"></asp:DropDownList>


                            </div>                            </div>



                    <div class="row">
            <div class="col-md-6 offset-6">
      <asp:LinkButton ID="btnSearch" class="btn btn-sm btn-outline-dark" OnClick="btnSearch_Click" runat="server">
            <i class="bi bi-search"></i>
            Buscar
        </asp:LinkButton>
            </div>
        </div>
       </div>
  
    </div>
    </div>
              <div class="row">
    <!-- Each card will take up 4 columns on medium and larger screens (making 3 cards per row) -->
                  <asp:Panel ID="PnlOperation" runat="server">
                          <div class="col-12 col-md-6 col-lg-6 mb-4">
    <div class="card">
                                  <div class=" card-header">

        <div class="card-title">Asignar linea telefónica</div></div>
        <div class="card-body">
                                        <div class="col-lg-2  col-md-6 col-12 pb-2 ">

                                                            <asp:DropDownList runat="server" Id="DdlAvailableLines" CssClass="form-control form-control-sm"></asp:DropDownList>

                                        </div>
               <div class="col-lg-2  col-md-6 col-12 pb-2 ">
                           <asp:DropDownList runat="server" Id="DdlAgents" CssClass="form-control form-control-sm"></asp:DropDownList>


                                        </div>
               <div class="col-lg-2  col-md-6 col-12 pb-2 ">
        <asp:DropDownList runat="server" Id="DdlEmployees" CssClass="form-control form-control-sm"></asp:DropDownList>


                                        </div>






        <div class="row">
            <div class="">
      <asp:LinkButton ID="BtnAssign" class="btn btn-sm btn-outline-success" OnClick="BtnAssign_Click" runat="server">
            <i class="bi bi-caret-down"></i>
            Asignar
        </asp:LinkButton>
                   <asp:LinkButton ID="BtnUnassing" class="btn btn-sm btn-outline-warning" OnClick="BtnUnassing_Click" runat="server">
            <i class="bi bi-caret-down"></i>
            Desasignar
        </asp:LinkButton>
                   <asp:LinkButton ID="BtnOutOfService" class="btn btn-sm btn-outline-danger" OnClick="BtnOutOfService_Click" runat="server">
            <i class="bi bi-caret-down"></i>
            Fuera de serivicio
        </asp:LinkButton>
            </div>
        </div>
          </div>

    </div>
           </div>

                  </asp:Panel>
            </div>
                        </div>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Table" runat="server">

        <div class="container-fluid container-lg">
   

            <div class="table-responsive">
                    <asp:GridView ID="DashboardGridview" runat="server" DataKeyNames="Codigo" CssClass="table  table-sm table-hover" AutoGenerateColumns="True" EmptyDataText="No se encontraron resultados" OnRowCommand="DashboardGridview_RowCommand"  EnableViewState="true" AllowPaging="true" PageIndexChanging="DashboardGridview_PageIndexChanging" AllowSorting="false" OnRowDataBound="DashboardGridview_RowDataBound" OnSelectedIndexChanged="DashboardGridview_SelectedIndexChanged" >
                        <Columns>
                       
                            <asp:TemplateField HeaderText="">

                                <ItemTemplate>
                                    <asp:LinkButton ID="btnOperation" ToolTip="Modificar" CssClass="btn btn-sm btn-outline-info " runat="server" CommandName="Operation" CommandArgument='<%# Container.DataItemIndex %>'>
<i class="bi bi-geo-alt-fill"></i>                                     </asp:LinkButton>


                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
<%--                 <nav aria-label="Page navigation">
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-2">
                                    <asp:Label runat="server" ID="lblTotalCount" Text=' <%# "Total: " & TotalItems & itemText %>'></asp:Label>

                                </div>
                                <div class="col-10">
                                    <ul class="pagination justify-content-center">

                                        <li class="page-item" runat="server" id="PreviousPage">
                                            <asp:LinkButton ID="lnkbtnPrevious" runat="server" Text="&laquo; Previa" CssClass="page-link" OnClick="lnkbtnPrevious_Click" Enabled='<%# PageNumber > 1 %>'></asp:LinkButton>
                                        </li>

                                        <asp:Repeater ID="rptPager" runat="server">
                                            <ItemTemplate>
                                                <li class="page-item <%# pagination.IsActivePage(Container.DataItem, PageNumber) %>">
                                                    <asp:LinkButton ID="lnkbtnPage" runat="server" Text='<%# Container.DataItem %>' CssClass="page-link" OnClick="lnkbtnPage_Click"></asp:LinkButton>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                        <li class="page-item" runat="server" id="NextPage">
                                            <asp:LinkButton ID="lnkbtnNext" runat="server" Text="Siguiente &raquo;" CssClass="page-link" OnClick="lnkbtnNext_Click" Enabled='<%# PageNumber < TotalPages %>'></asp:LinkButton>
                                        </li>
                                    </ul>

                                </div>
                            </div>

                        </div>

                    </nav>--%>
                </div>
    
        </div>
        <div id="alertPlaceholder" runat="server"></div>


</asp:Content>
