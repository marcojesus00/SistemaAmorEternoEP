<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Dashboards/Dashboard.master" CodeBehind="Ventas.aspx.vb" Inherits="Sistema.VentasDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Filters" runat="server">
    <h2 class="  text-center">Dashboard de ventas</h2>

    <div class="container-fluid mt-1 mb-1">
        <div class="border border-dark-subtle p-2">
            <div class="row">
                <div class="col-lg-2 col-md-6 col-12 pb-2 ">
                    <label for="DashboardType" class="form-control-label">Tipo de dashboard</label>

                    <asp:DropDownList ID="DashboardType" CssClass="form-control form-control-sm" runat="server" AutoPostBack="true">
                        <asp:ListItem Text="Recibos por vendedor" Value="0">
                        </asp:ListItem>
                        <asp:ListItem Text="Ventas por producto" Value="1"></asp:ListItem>
                    </asp:DropDownList>


                </div>
                <div class="col-lg-2  col-md-6 col-12 pb-2 ">
                    <label for="startDate" class="form-control-label">Fecha inicial</label>

                    <asp:TextBox ID="startDate" runat="server" TextMode="Date" CssClass="form-control form-control-sm"></asp:TextBox>

                </div>
                <div class="col-lg-2  col-md-6 col-12 pb-2 ">
                    <label for="endDate" class="form-control-label">Fecha final</label>

                    <asp:TextBox ID="endDate" runat="server" TextMode="Date" CssClass="form-control form-control-sm"></asp:TextBox>

                </div>
                <div class="col-lg-2  col-md-6 col-12 pb-2 ">
                    <label for="ddlCity" class="form-control-label">Zona</label>
                    <asp:DropDownList ID="ddlCity" CssClass="form-control form-control-sm" runat="server"></asp:DropDownList>


                </div>

                <div class="col-lg-2  col-md-6 col-12 pb-2 ">
                    <label for="ddlCompany" class="form-control-label">Empresa</label>

                    <asp:DropDownList ID="ddlCompany" CssClass="form-control form-control-sm" runat="server"></asp:DropDownList>


                </div>


                <div class="col-md-3  col-md-6 col-12 pb-2 " hidden>
                    <label for="leader" class="form-control-label">Estado del lider</label>

                    <asp:DropDownList ID="leader" runat="server" CssClass="form-control form-control-sm">
                        <asp:ListItem Text="Líder actual" Value="current" />
                        <asp:ListItem Text="Líder anterior" Value="previous" />
                    </asp:DropDownList>

                </div>
                <div class="col-lg-2  col-md-6 col-12 pb-2">
                    <div class="row">
                        <label for="ddlLeader" class="form-control-label">Lider</label>

                        <div class="col-lg-8 col-8 ">

                            <asp:DropDownList ID="ddlLeader" runat="server" CssClass="form-control form-control-sm">
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-auto col-auto">

                            <asp:LinkButton ID="BtnRouteOfReceiptsMapByLeader" ToolTip="Mapa de recibos por lider" runat="server">
<i class="bi bi-geo-alt-fill"></i>

                            </asp:LinkButton>
                        </div>

                    </div>
                </div>
                <div class="col-lg-2  col-md-6 col-12 pb-2 ">
                    <label for="ddlState" class="form-control-label">Estado </label>

                    <asp:DropDownList ID="ddlValidReceipts" CssClass="form-control form-control-sm" runat="server"></asp:DropDownList>


                </div>
                <div class="col-lg-2  col-md-6 col-12 pb-2 ">
                    <label for="ddlService" class="form-control-label">Servicio </label>

                    <asp:DropDownList ID="ddlService" CssClass="form-control form-control-sm" runat="server"></asp:DropDownList>


                </div>
                <div class="col-sm-3  col-md-6 col-12 pb-2 " hidden>
                    <div class="form-check ">
                        <label for="supervised" class="form-check-label">Incluir supervisado</label>

                        <asp:CheckBox ID="supervised" runat="server" CssClass="form-check-input" />
                    </div>
                </div>
                <div class="col-lg-2  col-md-6 col-12 pb-2 ">
                    <asp:Label ID="lblNumDoc" runat="server" CssClass="form-control-label" AssociatedControlID="textBoxNumDoc"></asp:Label>

                    <asp:TextBox ID="textBoxNumDoc" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>

                </div>
                <div class="col-lg-2  col-md-6 col-12 pb-2 ">
                    <label for="code" class="form-control-label">Código de vendedor</label>

                    <asp:TextBox ID="textBoxCode" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>

                </div>
                <div class="col-lg-2  col-md-6 col-12 pb-2 ">
                    <label for="textBoxClientCode" class="form-control-label">Código de cliente</label>

                    <asp:TextBox ID="textBoxClientCode" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>

                </div>



                <div class="col-lg-2 align-self-end pb-2">
                    <asp:Button ID="submitButton" runat="server" Text="Aplicar Filtros" CssClass="btn btn-sm btn-outline-primary" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Table" runat="server">
    <div class="row">
        <div class="col-lg-12 mb-2">
            <%--                                        <asp:Label ID="GridviewTitle" runat="server" Text="GridView Title" CssClass="gridViewTitle"></asp:Label>--%>

            <div class="table-responsive">
                <div class="row">
                    <asp:GridView ID="DashboardGridview" runat="server" DataKeyNames="Codigo" CssClass="table  table-sm table-hover" AutoGenerateColumns="True" EmptyDataText="No se encontraron resultados" OnRowCommand="DashboardGridView_RowCommand" EnableViewState="true" AllowPaging="true" PageIndexChanging="DashboardGridview_PageIndexChanging" AllowSorting="false" OnRowDataBound="DashboardGridView_RowDataBound" OnSelectedIndexChanged="DashboardGridview_SelectedIndexChanged">
                        <Columns>
                            <%--<asp:CommandField ShowSelectButton="True" />--%>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Panel ID="pnlPluMinus" runat="server" Visible="true">
                                        <img alt="" style="cursor: pointer" src="images/plus.png" />
                                        <img alt="" style="cursor: pointer; display: none" src="images/minus.png" />
                                    </asp:Panel>

                                    <%--                                                   <asp:LinkButton ID="ExpandButton" runat="server" Text="Expand" CssClass="btn btn-primary" OnClick="ExpandButton_Click" CommandArgument='<%# Container.DataItemIndex %>' />--%>
                                    <asp:Panel ID="pnlDetails" runat="server" Style="display: none">
                                        <asp:GridView ID="DetailsControl" DataKeyNames="Codigo" BackColor="White" CssClass="ChildGrid table table-sm table-striped table-hover " runat="server" AutoGeneratedColumns="True" OnRowDataBound="DetailsControl_RowDataBound" OnRowCommand="DetailsControl_RowCommand">

                                            <Columns>

                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">

                                <ItemTemplate>
                                    <asp:LinkButton ID="btnClientsByCollectorMap" ToolTip="Mapa de clientes" CssClass="btn btn-sm btn-outline-info " runat="server" CommandName="ClientsByCollectorMap" CommandArgument='<%# Container.DataItemIndex %>'>
<i class="bi bi-geo-alt-fill"></i>

                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnRouteOfReceiptsMap" ToolTip="Mapa de ruta de recibos" CssClass="btn btn-sm btn-outline-warning" runat="server" CommandName="RouteOfReceiptsMap" CommandArgument='<%# Container.DataItemIndex %>'>
<i class="bi bi-geo-alt-fill"></i>

                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
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
                                                <li class="page-item <%# IsActivePage(Container.DataItem, PageNumber) %>">
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
                <div class="col-lg-4">
                </div>
            </div>
        </div>

    </div>
    <asp:Panel ID="pnlMap" runat="server" Visible="false">

        <div style="width: 100%; height: 100%;">
            <iframe id="iMap" runat="server" style="position: fixed; width: 100%; height: 90%; top: 42px; background-color: none"></iframe>
        </div>
    </asp:Panel>
    <div id="alertPlaceholder" runat="server"></div>

    <script type="module">

        // Function to handle click on elements with src containing 'plus'
        document.addEventListener('click', function (event) {
            if (event.target.matches('[src*=plus]')) {
                const closestTr = event.target.closest('tr');
                const ordersHtml = closestTr.querySelector('[id*=Details]').innerHTML;

                const newRow = document.createElement('tr');
                newRow.innerHTML = `<td></td><td class='details' colspan='999'>${ordersHtml}</td>`;

                closestTr.after(newRow);
                closestTr.querySelector('[src*=minus]').style.display = 'inline'; // Show minus button
                event.target.style.display = 'none'; // Hide plus button
            }
        });

        // Function to handle click on elements with src containing 'minus'
        document.addEventListener('click', function (event) {
            if (event.target.matches('[src*=minus]')) {
                const closestTr = event.target.closest('tr');
                closestTr.nextElementSibling.remove(); // Remove the next row
                closestTr.querySelector('[src*=plus]').style.display = 'inline'; // Show plus button
                event.target.style.display = 'none'; // Hide minus button
            }
        });
    </script>
    <script type="text/javascript">
        function openLinkInNewTab(url) {
            window.open(url, '_blank');
        }
    </script>
    <script>
        window.addEventListener('message', function (event) {
            if (event.data === 'closeMap') {

                document.getElementById('<%= pnlMap.ClientID %>').style.display = 'none';
                }
            });
    </script>

</asp:Content>
