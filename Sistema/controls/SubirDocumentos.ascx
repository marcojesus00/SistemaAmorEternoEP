<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SubirDocumentos.ascx.vb" Inherits="Sistema.SubirDocumentos" %>

<%--<button type="button" class="btn btn-primary p-5" data-toggle="modal" data-target="#myModal">
  Agregar documento
</button>--%>

<form id="form1" method="post" enctype="multipart/form-data">
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
                            <asp:Button class="btn btn-primary" ID="UploadDocuments" runat="server" Text="Subir" />
                        </div>
                        <div class="text-center mt-2">
                            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                            </
                        </div>
                    </div>
                </div>
            </div>

        </div>
</form>

<%--<div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="exampleModalLabel">Agregar documentos</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
       <form id="form1" method="post" enctype="multipart/form-data">
          <div class="form-group">
            <asp:FileUpload ID="File1" CssClass="form-control-file" runat="server" />
          </div>
          <div class="text-center">
            <asp:Button class="btn btn-primary" ID="UploadDocuments" runat="server" Text="Subir" />
          </div>
          <div class="text-center mt-2">
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
          </div>
        </form>
      </div>
    </div>
  </div>
</div>--%>