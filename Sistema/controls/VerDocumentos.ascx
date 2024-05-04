<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" CodeBehind="VerDocumentos.ascx.vb" Inherits="Sistema.VerDocumentos" %>
<form id="form2" method="post" enctype="multipart/form-data">
    <div class="container p-5">
        <div class="row justify-content-center">
            <div class="col-md-8">
                <div class="card">
                    <div class="card-body">
                        <div class="card-title text-center mb-4">
                            Agregar documento
                        </div>

                        <div class="form-group">
                            <asp:FileUpload ID="File1" CssClass="form-control-file" runat="server" />
                        </div>
                        <div class="text-center">
<%--                            <asp:Button class="btn btn-primary" ID="UploadDocuments" runat="server" Text="Subir" />--%>
                         <asp:LinkButton ID="UploadFile" CssClass="btn btn-primary pb-2" runat="server" Text="Subir" ></asp:LinkButton>

                        </div>
                        <div class="text-center mt-2">
                            <asp:Label ID="lblUploadMessage" runat="server" Text=""></asp:Label>
                            </
                        </div>
                    </div>
                </div>
            </div>

        </div>
                </div>

</form>

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




