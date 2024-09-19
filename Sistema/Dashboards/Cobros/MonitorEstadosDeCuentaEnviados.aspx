<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Dashboards/Dashboard.master" CodeBehind="MonitorEstadosDeCuentaEnviados.aspx.vb" Inherits="Sistema.MonitorEstadosDeCuentaEnviados" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Filters" runat="server">
        <h2 class=" text-center">Envíos de WhatsApp</h2>
    <div class="container-fluid container-lg mt-1 mb-1">
        <div class="border border-dark-subtle p-2">
            <div class="row">
<%--                <div class="col-lg-3 col-md-12 col-12 pb-2 ">
                                <label for="DashboardType" class="form-control-label">Tipo de dashboard</label>--%>

<%--                    <asp:DropDownList ID="DashboardType" CssClass="form-control form-control-sm border-info-subtle border-2" runat="server" AutoPostBack="true">
                        <asp:ListItem Text="Dashboard de recibos" Value="0">
                        </asp:ListItem>
                        <asp:ListItem Text="Dashboard de cartera" Value="1"></asp:ListItem>
                    </asp:DropDownList>


                </div>--%>
                <div class="col-lg-2  col-md-6 col-12 pb-2 ">
                    <div class="row">
                        <div class="col-3">
                            <label for="textBoxInitialDate" class="form-control-label">Desde </label>

                        </div>
                        <div class="col-9">
                            <asp:TextBox ID="textBoxInitialDate" runat="server" TextMode="Date" CssClass="form-control form-control-sm"></asp:TextBox>

                        </div>

                    </div>

                </div>
                <div class="col-lg-2  col-md-6 col-12 pb-2 ">
                    <div class="row">
                        <div class="col-3">
                            <%--                                                <asp:TextBox ID="TextBox1" runat="server" Text="Desde" CssClass="form-control form-control-sm">desde</asp:TextBox>--%>
                            <label for="TextBoxFinalDate" class="form-control-label">Hasta </label>


                        </div>
                        <div class="col-9">
                            <asp:TextBox ID="TextBoxFinalDate" runat="server" TextMode="Date" CssClass="form-control form-control-sm">desde</asp:TextBox>

                        </div>

                    </div>

                </div>





                <div class="col-lg-3  col-md-6 col-12 pb-2">
                    <div class="row">
                        <%--                        <label for="ddlLeader" class="form-control-label">Lider</label>--%>

                        <div class="col-lg-10 col-10">

                            <asp:DropDownList ID="ddlLeader" runat="server" CssClass="form-control form-control-sm">
                            </asp:DropDownList>
                        </div>
 

                    </div>
                </div>
                <div class="col-lg-2  col-md-6 col-12 pb-2 ">
                    <%--                    <label for="ddlValidReceipts" class="form-control-label">Estado de los recibos</label>--%>

                    <asp:DropDownList ID="ddlStatus" CssClass="form-control form-control-sm" runat="server"></asp:DropDownList>


                </div>
         
                <div class="col-lg-2  col-md-6 col-12 pb-2 ">
                    <%--<asp:Label ID="lblNumDoc" runat="server" CssClass="form-control-label" AssociatedControlID="textBoxNumDoc"></asp:Label>--%>

                    <asp:TextBox ID="textBoxPhone" runat="server" Placeholder="Teléfono" CssClass="form-control form-control-sm"></asp:TextBox>

                </div>
                <div class="col-lg-2  col-md-6 col-12 pb-2 ">
                    <%--                    <label for="code" class="form-control-label">Código de cobrador</label>--%>

                    <asp:TextBox ID="textBoxCode" Placeholder="Código de cobrador" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>

                </div>
                <div class="col-lg-2  col-md-6 col-12 pb-2 ">
                    <%--                    <label for="textBoxClientCode" class="form-control-label">Código de cliente</label>--%>

                    <asp:TextBox ID="textBoxClientCode" runat="server" Placeholder="Código de cliente" CssClass="form-control form-control-sm"></asp:TextBox>

                </div>



                <div class="col-lg-2 align-self-center">
                    <asp:Button ID="submitButton" runat="server" Text="Aplicar Filtros" CssClass="btn btn-sm btn-outline-primary" />
   <%--                 <asp:LinkButton ID="LinkButtonClear" ToolTip="Limpiar filtros" CssClass="btn btn-sm btn-outline-dark" runat="server">
                        <i class="bi bi-stars"></i>
                    </asp:LinkButton>--%>
                </div>
                <div class="col-lg-1 col-1">
                </div>

            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Table" runat="server">
                     <div class="container-fluid container-lg">


    <asp:panel runat="server" id="PnlPrimary">
   

            <div class="table-responsive">
                    <asp:GridView ID="DashboardGridview" OnRowDataBound="DashboardGridview_RowDataBound" runat="server" CssClass="table  table-sm table-hover" AutoGenerateColumns="True" EmptyDataText="No se encontraron resultados"  >
                        <Columns>

                        </Columns>
                    </asp:GridView>
                 <nav aria-label="Page navigation">
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

                    </nav>
                </div>
    

    </asp:panel>

        </div>

        <div id="alertPlaceholder" runat="server"></div>

</asp:Content>
