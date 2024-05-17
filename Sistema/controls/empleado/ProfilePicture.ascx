<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" CodeBehind="ProfilePicture.ascx.vb" Inherits="Sistema.ProfilePicture" %>



<div class="modal fade" id="ChangeProfilePictureModal" tabindex="-1" aria-labelledby="ChangeProfilePictureModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="fileUploadModalLabel">Foto de perfil</h5>
            </div>
            <div class="modal-body">
                <div class="form-group text-center">
                    <asp:FileUpload ID="File1" CssClass="form-control-file text-center" runat="server" />
                </div>
                <div class="text-center pt-4">

                    <asp:LinkButton ID="PreviewButton0" CssClass="btn btn-primary p-2" runat="server" Text="Vista previa"></asp:LinkButton>
                </div>
                <div class="text-center mt-2">
                    <asp:Label ID="lblUploadMessage" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>
    </div>
</div>



<div class="container">
    <div class="row justify-content-center ">
        <div class="card text-bg-light  border-dark" style="width: 18rem;">
            <div class="card-header">
                Empleado <%=Session("Codigo_Empleado").ToString()%>
            </div>
            <div class="card-img-top text-center d-flex align-items-center">

                <div style="max-width: 200px; max-height: 250px; overflow: hidden; margin: 0 auto;">
                    <asp:Image ID="imgProfile" runat="server" Style="width: 100%; height: auto;" />
                </div>

            </div>
            <div class="text-center">
                <div class="card-body">
                    <h5 class="card-subtitle  text-body-secondary"><%=Session("nombreDeEmpleado").ToString()%></h5>
                </div>

                <div class="card-body">
                    <asp:Button ID="changePhotoButton" OnClientClick="showModal(); return false;" CssClass="btn btn-primary p-2" runat="server" Text="Cambio de foto"></asp:Button>


                    <div class="form-group text-center">
                    </div>

                    <div class="text-center">
                        <div class="row gx-5">
                            <asp:LinkButton ID="UploadFile" CssClass="btn btn-success p-2" runat="server" Text="Subir" Visible="false"></asp:LinkButton>

                            <asp:LinkButton ID="CancelUpload" CssClass="btn btn-danger p-2" runat="server" Text="Cancelar" Visible="false"></asp:LinkButton>

                        </div>
                    </div>


                </div>
            </div>

        </div>
    </div>
</div>
<script type="text/javascript">
    document.addEventListener('DOMContentLoaded', function () {
        var button = document.getElementById('<%=changePhotoButton.ClientID %>');

        if (button) {
            console.log('htmlid: ', button);
            button.addEventListener('click', function () {
                var myModal = new bootstrap.Modal(document.getElementById('ChangeProfilePictureModal'));
                myModal.show();
            });
        }
    });

    function showModal() {
        var myModal = new bootstrap.Modal(document.getElementById('ChangeProfilePictureModal'));
        myModal.show();
    }
    myModal.addEventListener('hidden.bs.modal', function () {
        document.querySelector('.modal-backdrop').remove();
    });
</script>
