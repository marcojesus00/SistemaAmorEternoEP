<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Shared/AE.Master" CodeBehind="InventarioDeEquipo.aspx.vb" Inherits="Sistema.InventarioDeEquipo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="PnlAssign" runat="server">
        <div class="card">

                        <div class="card-title"></div>


            <div class="card-body"></div>
        </div>
        <div>
                    <span>Empleado</span>
                   <asp:DropDownList ID="DDLmployee" runat="server">
                   </asp:DropDownList>
        </div>
            <asp:TextBox runat="server" ID="TextBoxItemId" Placeholder="Id" Visible="false">
        </asp:TextBox>



                <span>Jefe</span>

                           <asp:DropDownList ID="DdlBoss" runat="server">

                           </asp:DropDownList>
                 <span>Ciudad</span>
            <asp:DropDownList ID="DdlCity" runat="server">
            </asp:DropDownList>

                  <span>Area</span>
            <asp:DropDownList ID="DdlBusinessDepartments" runat="server">
            </asp:DropDownList>
                     <span>Departamento</span>
            <asp:DropDownList ID="DdlDepartments" runat="server">
            </asp:DropDownList>
                         <span>Descripción Adicional</span>

            <asp:TextBox runat="server" Id="TextBoxDescription" TextMode="MultiLine" Placheholder="Descripcion">
            </asp:TextBox>

            <asp:LinkButton Id="lnkButtonAssingn" runat="server" OnClick="CreateAssingment">
                Asignar
            </asp:LinkButton>
    </asp:Panel>
<%--    <asp:Panel ID="PnlCreateItem" runat="server">
        <asp:DropDownList ID="DDLCategory" runat="server" Placeholder="Categoria">
        </asp:DropDownList>
        </asp:DropDownList>

        <asp:DropDownList ID="DdlBrand" runat="server" Placeholder="Marca">
        </asp:DropDownList>
        <asp:DropDownList ID="DdlStatus" runat="server" Placeholder="Marca">
        </asp:DropDownList>
        <asp:TextBox runat="server" ID="TextBoxNewSerialNumber" Placeholder="Número de serie">

        </asp:TextBox>


        <asp:LinkButton ID="LinkButtonNewItem" runat="server">
        Agregar
        </asp:LinkButton>
    </asp:Panel>--%>


    <div class="container-fluid container-lg">


        <asp:Panel runat="server" ID="PnlPrimary">


            <div class="table-responsive">
                <asp:GridView ID="DashboardGridview" runat="server" CssClass="table  table-sm table-hover" AutoGenerateColumns="True" EmptyDataText="No se encontraron resultados" OnRowCommand="DashboardGridview_RowCommand">
                    <Columns>
                              <asp:TemplateField HeaderText="Acciones">
            <ItemTemplate>
                <asp:LinkButton ID="btnAssingThisItem" runat="server" Text="Asignar" CommandName="AssingThisItem" CommandArgument='<%# Eval("Id") %>' />
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


        </asp:Panel>

    </div>

    <div id="alertPlaceholder" runat="server"></div>

</asp:Content>


<%--inventarip
    <asp:DropDownList ID="DropDownList1" runat="server" Placeholder="Categoria">

    </asp:DropDownList>
        <asp:DropDownList ID="DropDownList2" runat="server" Placeholder="Marca">

    </asp:DropDownList>

            <asp:DropDownList ID="DropDownList3" runat="server" Placeholder="Marca">

    </asp:DropDownList>
    <asp:TextBox runat="server" Id="TextBox1" Placeholder="Número de serie">

    </asp:TextBox>
        <asp:TextBox runat="server" Id="TextBox2" Placheholder="Empleado">

    </asp:TextBox>

    <asp:LinkButton Id="LinkButton1" runat="server">
        Asignar
    </asp:LinkButton>--%>