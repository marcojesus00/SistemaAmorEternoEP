<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Dashboards/Dashboard.master" CodeBehind="Cobros.aspx.vb" Inherits="Sistema.CobrosDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Filters" runat="server">

    <div class="container-fluid mt-1 mb-1">
        <div class="border border-dark-subtle p-2">
            <div class="row">
                <div class="col-lg-3 col-md-6 col-12 ">
                    <asp:DropDownList ID="DashboardType" CssClass="form-control" runat="server" AutoPostBack="true">
                        <asp:ListItem Text="Recibos" Value="0">
                        </asp:ListItem>
                        <asp:ListItem Text="Cartera (Bloqueado)" Value="100"></asp:ListItem>
                    </asp:DropDownList>
                    <label for="DashboardType" class="text-left  text-secondary fs-6">Tipo de dashboard</label>


                </div>
                <div class="col-lg-2  col-md-6 col-12 ">
                    <asp:TextBox ID="startDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                    <label for="startDate" class="text-left  text-secondary fs-6">Fecha inicial</label>

                </div>
                <div class="col-lg-2  col-md-6 col-12 ">
                    <asp:TextBox ID="endDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                    <label for="endDate" class="text-left  text-secondary fs-6">Fecha final</label>

                </div>
                <div class="col-lg-2  col-md-6 col-12 ">
                    <asp:DropDownList ID="ddlCity" CssClass="form-control" runat="server"></asp:DropDownList>
                    <label for="ddlCity" class="text-left  text-secondary fs-6">Zona</label>


                </div>

                <div class="col-lg-2  col-md-6 col-12 ">
                    <asp:DropDownList ID="ddlCompany" CssClass="form-control" runat="server"></asp:DropDownList>
                    <label for="ddlCompany" class="text-left  text-secondary fs-6">Empresa</label>


                </div>


                <div class="col-md-3  col-md-6 col-12 " hidden>
                    <asp:DropDownList ID="leader" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Líder actual" Value="current" />
                        <asp:ListItem Text="Líder anterior" Value="previous" />
                    </asp:DropDownList>
                    <label for="leader" class="text-left text-secondary fs-6">Estado del lider</label>

                </div>
                <div class="col-lg-3  col-md-6 col-12">
                    <div class="row">
                        <div class="col-lg-10 col-10">
                            <asp:DropDownList ID="ddlLeader" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                            <label for="ddlLeader" class="text-left  text-secondary fs-6">Lider</label>
                        </div>
                        <div class="col-lg-2 col-2">

                            <asp:LinkButton ID="BtnRouteOfReceiptsMapByLeader" ToolTip="Mapa de recibos por lider" CssClass="btn btn-outline-danger" runat="server">
<i class="bi bi-geo-alt-fill"></i>

                            </asp:LinkButton>
                        </div>

                    </div>
                </div>
                <div class="col-lg-2  col-md-6 col-12 ">
                    <asp:DropDownList ID="ddlValidReceipts" CssClass="form-control" runat="server"></asp:DropDownList>
                    <label for="ddlValidReceipts" class="text-left  text-secondary fs-6">Estado de los recibos</label>


                </div>
                <div class="col-sm-3  col-md-6 col-12 " hidden>
                    <div class="form-check ">
                        <asp:CheckBox ID="supervised" runat="server" CssClass="form-check-input" />
                        <label for="supervised" class="form-check-label">Incluir supervisado</label>
                    </div>
                </div>
                <div class="col-lg-2  col-md-6 col-12 ">
                    <asp:TextBox ID="textBoxNumDoc" runat="server" CssClass="form-control"></asp:TextBox>
                    <label for="textBoxNumDoc" class="text-left  text-secondary fs-6">Número de documento</label>

                </div>
                <div class="col-lg-2  col-md-6 col-12 ">
                    <asp:TextBox ID="textBoxCode" runat="server" CssClass="form-control"></asp:TextBox>
                    <label for="code" class="text-left  text-secondary fs-6">Código de cobrador</label>

                </div>
                <div class="col-lg-2  col-md-6 col-12 ">
                    <asp:TextBox ID="textBoxClientCode" runat="server" CssClass="form-control"></asp:TextBox>
                    <label for="textBoxClientCode" class="text-left  text-secondary fs-6">Código de cliente</label>

                </div>



                <div class="col-lg-2 align-self-center">
                    <asp:Button ID="submitButton" runat="server" Text="Aplicar Filtros" CssClass="btn btn-outline-primary" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Table" runat="server">
    <div class="row">
        <div class="col-lg-6 mb-2">
            <%--                                        <asp:Label ID="GridviewTitle" runat="server" Text="GridView Title" CssClass="gridViewTitle"></asp:Label>--%>

            <div class="table-responsive">
                <div class="row">
                    <asp:GridView ID="DashboardGridview" runat="server" DataKeyNames="Codigo" CssClass="table  table-sm table-striped table-hover" AutoGenerateColumns="True" EmptyDataText="No se encontraron resultados" OnRowCommand="DashboardGridView_RowCommand" EnableViewState="true" AllowPaging="true" PageIndexChanging="DashboardGridview_PageIndexChanging" AllowSorting="false" OnRowDataBound="DashboardGridView_RowDataBound" OnSelectedIndexChanged="SellerGridView_SelectedIndexChanged">
                        <Columns>
                            <%--<asp:CommandField ShowSelectButton="True" />--%>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <img alt="" style="cursor: pointer" src="images/plus.png" />
                                    <img alt="" style="cursor: pointer; display: none" src="images/minus.png" />
                                    <%--                                                   <asp:LinkButton ID="ExpandButton" runat="server" Text="Expand" CssClass="btn btn-primary" OnClick="ExpandButton_Click" CommandArgument='<%# Container.DataItemIndex %>' />--%>
                                    <asp:Panel ID="pnlDetails" runat="server" Style="display: none">
                                        <asp:GridView ID="DetailsControl" DataKeyNames="Codigo" CssClass="ChildGrid" runat="server" AutoGeneratedColumns="True">
                                            <Columns>

                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnReceiptLocation" ToolTip="Ubicación del recibo" CssClass="btn btn-sm btn-outline-dark pb-2" runat="server" CommandName="ReceiptLocationMap" CommandArgument='<%# Container.DataItemIndex %>'>
<i class="bi bi-geo-alt-fill"></i>
                                                        </asp:LinkButton>


                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">

                                <ItemTemplate>
                                    <asp:LinkButton ID="btnClientsByCollectorMap" ToolTip="Mapa de clientes" CssClass="btn btn-sm btn-outline-info pb-2" runat="server" CommandName="ClientsByCollectorMap" CommandArgument='<%# Container.DataItemIndex %>'>
<i class="bi bi-geo-alt-fill"></i>

                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnRouteOfReceiptsMap" ToolTip="Mapa de ruta de recibos" CssClass="btn btn-sm btn-outline-warning pb-2" runat="server" CommandName="RouteOfReceiptsMap" CommandArgument='<%# Container.DataItemIndex %>'>
<i class="bi bi-geo-alt-fill"></i>

                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="col-lg-4">
                </div>
            </div>
        </div>
        <div class="col-lg-6">
            <asp:Label ID="DetailsTitle" runat="server" Text="GridView Title" CssClass="lead"></asp:Label>

            <div class="table-responsive">
                <%--                <asp:GridView ID="DetailsControl" DataKeyNames="Codigo" CssClass="table table-sm table-striped table-hover table-bordered border-primary-subtle" runat="server" AutoGeneratedColumns="True" OnRowCommand="DetailsControl_RowCommand" OnRowDataBound="DetailsControl_RowDataBound" Visible="False">
                                            <Columns>

                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnReceiptLocation" ToolTip="Ubicación del recibo" CssClass="btn btn-sm btn-outline-dark pb-2" runat="server" CommandName="ReceiptLocationMap" CommandArgument='<%# Container.DataItemIndex %>'>
<i class="bi bi-geo-alt-fill"></i>

                                    </asp:LinkButton>


                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                </asp:GridView>--%>
            </div>
        </div>
    </div>
    <div id="alertPlaceholder" runat="server"></div>
    <!-- Include jQuery (if necessary) -->
    <!-- <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script> -->

    <script type="module">
        // Assuming you're not using jQuery and want to use modern JavaScript

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


</asp:Content>
