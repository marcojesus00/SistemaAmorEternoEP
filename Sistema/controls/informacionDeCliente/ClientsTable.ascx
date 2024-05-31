<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" CodeBehind="ClientsTable.ascx.vb" Inherits="Sistema.ClientsTable" %>


<script src="helpers/js/fileUploadValidation.js" type="text/javascript"></script>
<%--<script type="text/javascript">--%>


<%--    function handleValidation() {
        var textBoxValue = document.getElementById('<%= TextBoxDescription.ClientID %>').value;
        var linkButton = document.getElementById('<%= UploadFile.ClientID %>');
        var fileInput = document.getElementById('<%= File1.ClientID %>');
        var lbl = document.getElementById('<%= lblUploadMessage.ClientID %>');

        if (textBoxValue.trim() === '' || fileInput.files.length === 0) {
            linkButton.disabled = true;
            linkButton.classList.add('btn', 'btn-secondary');
            lbl.style.color = "red"

        } else {
            linkButton.disabled = false;
            linkButton.classList.remove('btn', 'btn-secondary');
            lbl.style.color = "white"

            linkButton.classList.add('btn', 'btn-primary');

        }
    }
    function handleInputChange() {
        handleValidation()
    }

    function handleFileChange(input) {
        handleValidation();
        validateFileSize(input, 10);
    }
</script>
<button type="button" class="btn btn-primary p-2" data-bs-toggle="modal" data-bs-target="#fileUploadModal">
    <i class="bi bi-plus-lg"></i>

    Agregar archivo
</button>

<div class="modal fade" id="fileUploadModal" tabindex="-1" aria-labelledby="fileUploadModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content min">
            <div class="modal-header">
                <h5 class="modal-title" id="fileUploadModalLabel">Subir archivo</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <div class="text-center d-flex justify-content-center">
                    <div class="form-group ">
                        <asp:FileUpload ID="File1" CssClass="form-control-file" onchange="handleFileChange(this);" runat="server" />
                    </div>
                </div>

                <div class="text-center">
                    <br />

                    <asp:TextBox ID="TextBoxDescription" onkeyup="handleInputChange()" MaxLength="28" Placeholder="Descripción requerida" runat="server" AutoPostBack="false"></asp:TextBox>

                    <br />
                    <br />
                    <asp:LinkButton ID="UploadFile" CssClass="btn btn-secondary" runat="server" Text="Subir"></asp:LinkButton>
                </div>
                <div class="text-center mt-2">
                    <asp:Label ID="lblUploadMessage" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>
    </div>
</div>
--%>

<div class="w-100 table-responsive">

    <div class="container mt-5">
        <div class="mb-3 ">
<%--            <asp:DropDownList ID="ddlAreArchived" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlDocsState_SelectedIndexChanged"></asp:DropDownList>--%>
        </div>
    </div>

    <asp:GridView ID="MyGridView" runat="server" CssClass="table  table-sm table-striped table-hover" AutoGenerateColumns="False" EmptyDataText="No se encontraron resultados" OnRowCommand="MyGridView_RowCommand"   EnableViewState="true" AllowPaging="true" AllowSorting="true">
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
