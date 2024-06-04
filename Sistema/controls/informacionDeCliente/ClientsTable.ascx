<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" CodeBehind="ClientsTable.ascx.vb" Inherits="Sistema.ClientsTable" %>

<div class="row">
    <div class="col-8">
        <div class=" table-responsive">

            <h4>Clientes</h4>

            <asp:GridView ID="MyGridView" runat="server" DataKeyNames="CodigoCliente" CssClass="table  table-sm table-striped table-hover" AutoGenerateColumns="False" EmptyDataText="No se encontraron resultados" OnRowCommand="MyGridView_RowCommand" EnableViewState="true" AllowPaging="true" AllowSorting="true">
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnDownload" ToolTip="Mostrar documentos" CssClass="btn btn-outline-info pb-2" runat="server" CommandName="ShowDocs" CommandArgument='<%# Container.DataItemIndex %>'>
                        <i class="bi bi-caret-down-fill"></i>
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
</div>

