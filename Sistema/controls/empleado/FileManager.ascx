<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" CodeBehind="FileManager.ascx.vb" Inherits="Sistema.FileManager" %>

<button type="button" class="btn btn-primary p-2" data-bs-toggle="modal" data-bs-target="#fileUploadModal">
    <i class="bi bi-plus-lg"></i>

    Agregar archivo
</button>

<div class="modal fade" id="fileUploadModal" tabindex="-1" aria-labelledby="fileUploadModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="fileUploadModalLabel">Subir archivo</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <div class="text-center d-flex justify-content-center">
                    <div class="form-group ">
                        <asp:FileUpload ID="File1" CssClass="form-control-file" runat="server" />
                    </div>
                </div>

                <div class="text-center">
                    <br />

                    <asp:TextBox ID="TextBoxDescription" MaxLength="35" Placeholder="Descripción requerida" runat="server" AutoPostBack="false"></asp:TextBox>
                    <div class="invalid-feedback">
                        La descripción es requerida.
                    </div>
                    <br />
                    <br />
                    <asp:LinkButton ID="UploadFile" CssClass="btn btn-primary" runat="server" Text="Subir"></asp:LinkButton>
                </div>
                <div class="text-center mt-2">
                    <asp:Label ID="lblUploadMessage" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>
    </div>
</div>


<div class="w-100 table-responsive pt-2">

    <asp:GridView ID="MyGridView" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" OnRowCommand="MyGridView_RowCommand" OnRowDeleting="MyGridView_RowDeleting" OnRowDataBound="MyGridView_RowDataBound" DataKeyNames="Id" EnableViewState="true" AllowSorting="true">
        <Columns>
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkDelete" ToolTip="Eliminar documento" CssClass="btn btn-outline-danger pb-2" runat="server" CommandName="Delete" Text="Borrar" OnClientClick="return confirm('¿Está seguro que desea eliminar este documento?');">
<i class="bi bi-trash3-fill"></i>
                    </asp:LinkButton>

                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <asp:LinkButton ID="btnDownload" ToolTip="Descagar documento" CssClass="btn btn-outline-info pb-2" runat="server" CommandName="DownloadFile" CommandArgument='<%# Container.DataItemIndex %>'>
<i class="bi bi-arrow-down"></i>

                    </asp:LinkButton>

                </ItemTemplate>


            </asp:TemplateField>
            <asp:BoundField DataField="NombreDelArchivo" HeaderText="Archivo" />
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <asp:PlaceHolder ID="IconPlaceholder" runat="server"></asp:PlaceHolder>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
            <asp:BoundField DataField="Id" HeaderText="ID" Visible="false" />

        </Columns>
    </asp:GridView>
    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
</div>
