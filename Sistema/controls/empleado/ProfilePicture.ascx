﻿<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" CodeBehind="ProfilePicture.ascx.vb" Inherits="Sistema.ProfilePicture" %>



<div class="modal fade" id="ChangeProfilePictureModal" tabindex="-1" aria-labelledby="ChangeProfilePictureModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="fileUploadModalLabel">Subir archivo</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <div class="form-group text-center">
                    <asp:FileUpload  onchange="SetImagePreview(this)" ID="File1" CssClass="form-control-file text-center" runat="server" />
                </div>
                <div class="text-center">

                    <asp:LinkButton ID="PreviewButton" CssClass="btn btn-primary p-2" runat="server" Text="Vista previa"></asp:LinkButton>
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
                    <button type="button" id="changePhotoButton" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#ChangeProfilePictureModal">
                        Cambiar foto
                    </button>
                    <div class="text-center">
                        <div class="row gx-5">
                            <asp:LinkButton ID="UploadFile"  CssClass="btn btn-success p-2" runat="server" Text="Subir" Visible="false"></asp:LinkButton>

                            <asp:LinkButton ID="CancelUpload" CssClass="btn btn-danger p-2" runat="server" Text="Cancelar" Visible="false"></asp:LinkButton>

                        </div>
                    </div>


                </div>
            </div>

        </div>
    </div>
</div>
