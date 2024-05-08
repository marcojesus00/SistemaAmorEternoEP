﻿<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" CodeBehind="ProfilePicture.ascx.vb" Inherits="Sistema.ProfilePicture" %>



<div class="modal fade" id="fileUploadModal" tabindex="-1" aria-labelledby="fileUploadModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="fileUploadModalLabel">Subir archivo</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <div class="form-group text-center">
                    <asp:FileUpload ID="File1" CssClass="form-control-file text-center" runat="server" />
                </div>
                <div class="text-center">

                    <asp:LinkButton ID="UploadFile" CssClass="btn btn-primary pb-2" runat="server" Text="Subir"></asp:LinkButton>
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
        <div class="card" style="width: 18rem;">
            <asp:Image ID="imgProfile" cssClass="text-center" runat="server" Width="250" Height="250" />
            <div class="text-center">
                <div class="card-body">
                    <h5 class="card-title">Empleado</h5>
<%--                    <p class="card-text">~/<%=ruta%></p>--%>
                </div>
                <ul class="list-group list-group-flush">
<%--                    <li class="list-group-item">Puesto</li>--%>
                </ul>
                <div class="card-body">
<%--                    <a href="#" class="card-link">Cambiar foto</a>--%>
                    <button type="button" class="btn btn-primary p-2" data-bs-toggle="modal" data-bs-target="#fileUploadModal">
                        Cambiar foto
                    </button>
                </div>
            </div>

        </div>
    </div>
</div>
