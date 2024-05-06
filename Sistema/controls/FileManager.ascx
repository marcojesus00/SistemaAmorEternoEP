<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" CodeBehind="FileManager.ascx.vb" Inherits="Sistema.FileManager" %>
<%--<form id="form2" method="post" enctype="multipart/form-data">
        <div class="row justify-content-center">
            <div class="col-md-8">
                <div class="card">
                    <div class="card-body">
                        <div class="card-title text-center mb-4">
                            Agregar archivo
                        </div>

                        <div class="form-group text-center">
                            <asp:FileUpload ID="File1" CssClass="form-control-file text center"  runat="server" />

                            </div>
                        <div class="text-center">
                            <br />

                            <asp:TextBox ID="TextBoxDescription" AutoPostBack="True" Placeholder="Descripción" OnTextChanged="TextBoxDescription_TextChanged" runat="server"></asp:TextBox>
                            <br />
                            <br />
                            <asp:LinkButton ID="UploadFile" CssClass="btn btn-primary pb-2" runat="server" Text="Subir"></asp:LinkButton>

                        </div>
                        <div class="text-center mt-2">
                            <asp:Label ID="lblUploadMessage" runat="server" Text=""></asp:Label>
                            </
                        </div>
                    </div>
                </div>
            </div>

        </div>

</form>--%>
    <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#fileUploadModal">
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
                    <div class="form-group text-center">
                        <asp:FileUpload ID="File1"  CssClass="form-control-file text-center"  runat="server" />
                    </div>
                    <div class="text-center">
                        <br />
                        <asp:TextBox ID="TextBoxDescription" AutoPostBack="True" Placeholder="Descripción" OnTextChanged="TextBoxDescription_TextChanged" runat="server"></asp:TextBox>
                        <br />
                        <br />
                        <asp:LinkButton ID="UploadFile" CssClass="btn btn-primary pb-2" runat="server" Text="Subir"></asp:LinkButton>
                    </div>
                    <div class="text-center mt-2">
                        <asp:Label ID="lblUploadMessage" runat="server" Text=""></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="w-100 table-responsive">

        <asp:GridView ID="MyGridView" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" OnRowCommand="MyGridView_RowCommand" OnRowDeleting="MyGridView_RowDeleting"  OnRowDataBound="MyGridView_RowDataBound" DataKeyNames="Id" EnableViewState="true" AllowSorting="true">
            <Columns>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDelete" CssClass="btn btn-danger pb-2" runat="server" CommandName="Delete" Text="Borrar" OnClientClick="return confirm('¿Está seguro que desea eliminar este documento?');"></asp:LinkButton>

                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnDownload" CssClass="btn btn-info pb-2" runat="server" CommandName="DownloadFile" Text="Descargar" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>

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

<div id="alertPlaceholder" runat="server"></div>


<script>
  // JavaScript to handle showing alerts
  function showAlert(message, alertType) {
    var alertClass = 'alert-' + alertType;
    var alertHtml = '<div class="alert ' + alertClass + ' alert-dismissible fade show" role="alert">' + message +
      '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div>';
    $('#fileUploadModal .modal-body').prepend(alertHtml);
  }
</script>
