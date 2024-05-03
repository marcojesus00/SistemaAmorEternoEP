<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" CodeBehind="VerDocumentos.ascx.vb" Inherits="Sistema.VerDocumentos" %>

<form id="form1">
    <div class="container p-5 table-responsive">
        <h4>Documentos de este empleado</h4>

        <asp:GridView ID="MyGridView" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" OnRowCommand="MyGridView_RowCommand" OnRowDeleting="MyGridView_RowDeleting" DataKeyNames="Id" EnableViewState="true">
            <Columns>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDelete" CssClass="btn btn-primary pb-2" runat="server" CommandName="Delete" Text="Borrar" OnClientClick="return confirm('¿Está seguro que desea eliminar este documento?');"></asp:LinkButton>

                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnDownload" CssClass="btn btn-primary pb-2" runat="server" CommandName="DownloadFile" Text="Descargar" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>

                    </ItemTemplate>

                </asp:TemplateField>
                <asp:BoundField DataField="NombreDelArchivo" HeaderText="Archivo" />
                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                <asp:BoundField DataField="Id" HeaderText="ID" Visible="false" />

            </Columns>
        </asp:GridView>
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
    </div>
</form>
