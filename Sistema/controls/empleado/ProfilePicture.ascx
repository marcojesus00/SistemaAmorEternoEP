<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" CodeBehind="ProfilePicture.ascx.vb" Inherits="Sistema.ProfilePicture" %>

<script src="helpers/js/fileUploadValidation.js" type="text/javascript"></script>
<script type="text/javascript">


    function handleVal() {
        var linkButton = document.getElementById('<%= PreviewButton0.ClientID %>');
        var fileInput = document.getElementById('<%= File1.ClientID %>');
        var lbl = document.getElementById('<%= lblUploadMessage.ClientID %>');
        console.log(" initi")

        if (fileInput.files.length === 0) {
            linkButton.disabled = true;
            linkButton.classList.add('btn', 'btn-secondary');
            lbl.style.color = "red"
            console.log(" length === 0")

        } else {
            linkButton.disabled = false;
            linkButton.classList.remove('btn', 'btn-secondary');
            lbl.style.color = "white"

            linkButton.classList.add('btn', 'btn-primary');
            console.log(" length > 0")

        }
    }

    function handleChange(input) {
        console.log(" file chasnge")

        validateFileSize(input, 10);
        handleVal();

    }
</script>

<div class="modal fade" id="ChangeProfilePictureModal" tabindex="-1" aria-labelledby="ChangeProfilePictureModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="fileUploadModalLabel">Foto de perfil</h5>
            </div>
            <div class="modal-body">
                <div class="form-group text-center">
                    <asp:FileUpload ID="File1" CssClass="form-control-file text-center"  onchange="handleChange(this)" runat="server" />
                </div>
                <div class="text-center pt-4">

                    <asp:LinkButton ID="PreviewButton0" CssClass="btn btn-secondary" runat="server" Text="Vista previa"></asp:LinkButton>
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

    function showModal() {
        var myModal2 = new bootstrap.Modal(document.getElementById('ChangeProfilePictureModal'));
        if (myModal2) {
            //console.log('modal 2: ', myModal2);
            myModal2.show();

        }
        else {
            //console.log('no modal 2');


        }
    }
</script>
