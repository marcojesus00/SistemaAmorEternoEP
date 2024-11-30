<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" CodeBehind="ClientsTable.ascx.vb" Inherits="Sistema.ClientsTable" %>


<asp:Panel ID="PnlToCash" runat="server">
<%--    <div class="modal fade" id="myModal" tabindex="-1" aria-labelledby="exampleModalLabel" ">--%>
    <div class="row">
    <div class="col-8">
    <div class="card">
<%--        <div class="modal-content">--%>
            <div class="">
<%--                <div class="card-title" id="exampleModalLabel">Pasar a contado</div>--%>
                <asp:Label ID="CardTitleLabel" runat="server" CssClass="card-title" Text="Default Title"></asp:Label>

            </div>
            <div class="card-body">
               <asp:DropDownList Id="DdlRtipoDebi" runat="server"
                   ></asp:DropDownList>
            </div>
            <div class="card-footer">
                <asp:Linkbutton Id="LinkbuttonClose" runat="server" class="btn btn-danger" OnClick="LinkbuttonClose_Click">Cancelar</asp:Linkbutton>
                <asp:Linkbutton Id="SaveToCash" runat="server" class="btn btn-primary" OnClick="SaveToCash_Click">Guardar</asp:Linkbutton>
            </div>
        </div>
</div>
</div>
</asp:Panel>

<div class="row">
    <div class="col-8">
        <div class=" table-responsive">

            <h4>Clientes</h4>

            <asp:GridView ID="MyGridView" runat="server" DataKeyNames="CodigoCliente" CssClass="table  table-sm table-striped table-hover" AutoGenerateColumns="False" EmptyDataText="No se encontraron resultados" OnRowCommand="MyGridView_RowCommand" OnRowDataBound="MyGridView_RowDataBound" EnableViewState="true" AllowPaging="true" AllowSorting="true">
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnDownload" ToolTip="Mostrar documentos" CssClass="btn btn-outline-info pb-2" runat="server" CommandName="ShowDocs" CommandArgument='<%# Container.DataItemIndex %>'>
                        <i class="bi bi-caret-down-fill"></i>
                            </asp:LinkButton>
                                 <asp:LinkButton ID="LinkButtonFromCreditToCash" ToolTip="pasar a contado" CssClass="btn btn-outline-warning pb-2" runat="server" CommandName="FromCreditToCash" CommandArgument='<%# Container.DataItemIndex %>'>
                        <i class="bi bi-currency-exchange"></i>
                            </asp:LinkButton>
                        </ItemTemplate>


                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:PlaceHolder ID="IconPlaceholder" runat="server"></asp:PlaceHolder>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CodigoCliente" HeaderText="Código de Cliente" />
                    <asp:BoundField DataField="NombreCliente" HeaderText="Nombre" />
                    <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                    <asp:BoundField DataField="Celular" HeaderText="Celular" />

                    <asp:BoundField DataField="Vendedor" HeaderText="Vendedor" />
                    <asp:BoundField DataField="Cobrador" HeaderText="Cobrador" />


                </Columns>
            </asp:GridView>
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        </div>

    </div>

    <div class="col-4">
        <div class=" table-responsive">
            <h4>Documentos</h4>
            <asp:GridView runat="server" ID='GridViewDocs' DataKeyNames="Id" OnRowCommand="GridViewDocs_RowCommand" AutoGenerateColumns="False" CssClass="table  table-sm table-striped table-hover" EmptyDataText="No se encontraron resultados" Visible="true">
                <Columns>
                    <asp:BoundField DataField="NombreDelDocumento" HeaderText="Nombre" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:PlaceHolder ID="IconPlaceholder" runat="server"></asp:PlaceHolder>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Comentario" HeaderText="Comentario" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnDownloadDocs" ToolTip="Descagar documento" CssClass="btn btn-outline-info pb-2" runat="server" CommandName="DownloadFile" CommandArgument='<%# Container.DataItemIndex %>'>
                <i class="bi bi-arrow-down"></i>

                            </asp:LinkButton>
                        </ItemTemplate>


                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

        </div>


    </div>
                   <div id="alertPlaceholder" runat="server"></div>

</div>

