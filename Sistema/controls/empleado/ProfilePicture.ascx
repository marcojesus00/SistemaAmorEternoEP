<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" CodeBehind="ProfilePicture.ascx.vb" Inherits="Sistema.ProfilePicture" %>

<script src="helpers/js/fileUploadValidation.js" type="text/javascript"></script>

<script type="text/javascript">


    function handleVal() {
        var linkButton = document.getElementById('<%= PreviewButton0.ClientID %>');
        var fileInput = document.getElementById('<%= File1.ClientID %>');
        var lbl = document.getElementById('<%= lblUploadMessage.ClientID %>');

        if (fileInput.files.length === 0) {
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

    function handleChange(input) {

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


                <ul class="nav nav-tabs" id="myTab" role="tablist">
                    <li class="nav-item" role="presentation">
                        <a class="nav-link active" id="upload-tab" data-bs-toggle="tab" href="#upload" role="tab" aria-controls="upload" aria-selected="true">Subir imagen</a>
                    </li>
                    <li class="nav-item" role="presentation">
                        <a class="nav-link" id="takePhoto-tab" data-bs-toggle="tab" href="#takePhoto" role="tab" aria-controls="takePhoto" aria-selected="false">Tomar foto</a>
                    </li>

                </ul>

                <!-- Tab Content -->
                <div class="tab-content" id="myTabContent">
                    <div class="tab-pane fade show active" id="upload" role="tabpanel" aria-labelledby="upload-tab">
                        <p>Seleccione una imagen.</p>
                        <div class="form-group text-center">
                            <asp:FileUpload ID="File1" CssClass="form-control-file text-center" onchange="handleChange(this)" runat="server" />
                        </div>
                                              <div class="text-center pt-4">

                    <asp:LinkButton ID="PreviewButton0" CssClass="btn btn-secondary" runat="server" Text="Vista previa"></asp:LinkButton>
                </div>
                    </div>



                    <div class="tab-pane fade" id="takePhoto" role="tabpanel" aria-labelledby="takePhoto-tab">
                        <p>Profile content goes here.</p>
                        <div class="text-center pt-4">
                            <div id="my_camera" style="width: 320px; height: 240px;"></div>
                            <div id="results" style="padding: 0px; border: 1px solid; background: #ccc;"></div>
                            <button type="button" class="btn btn-primary" onclick="take_snapshot()">Capturar foto</button>
                        </div>
                                                         <div class="text-center pt-4">

                    <asp:LinkButton ID="PreviewButton1" CssClass="btn btn-secondary" runat="server" Text="Vista previa1"></asp:LinkButton>
                </div>

                    </div>
           
                </div>

        

      

            </div>
            <div class="modal-footer">
  
                <div class="text-center mt-2">
                    <asp:Label ID="lblUploadMessage" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>
    </div>
</div>

<asp:HiddenField ID="HiddenFieldImageData" runat="server" />


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
<%--<script type="text/javascript">

    function showModal() {
        var myModal2 = new bootstrap.Modal(document.getElementById('ChangeProfilePictureModal'));
        if (myModal2) {
            myModal2.show();

        }
        else {


        }
    }
</script>--%>
<script src="https://cdn.jsdelivr.net/npm/webcamjs/webcam.min.js"></script>
<script type="text/javascript">
    function showModal() {
        var myModal2 = new bootstrap.Modal(document.getElementById('ChangeProfilePictureModal'));
        if (myModal2) {
            myModal2.show();
        }
        Webcam.set({
            width: 320,
            height: 240,
            image_format: 'jpeg',
            jpeg_quality: 90
        });
        Webcam.attach('#my_camera');
        var cameraPreview = document.getElementById('my_camera');

        cameraPreview.style.display = 'block';
        var results = document.getElementById('results');

        results.style.display = 'none';
    }

    function take_snapshot() {
        Webcam.snap(function (data_uri) {
            var cameraPreview = document.getElementById('my_camera');
            var results = document.getElementById('results');

            results.style.display = 'block';
            cameraPreview.style.display = 'none';
            document.getElementById('results').innerHTML = '<img src="' + data_uri + '"/>';
            // Save the image to a hidden field for further processing if needed
            document.getElementById('<%= HiddenFieldImageData.ClientID %>').value = data_uri;
        });
    }
</script>
