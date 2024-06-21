<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="NavBar.ascx.vb" Inherits="Sistema.NavBar" %>
<%@ OutputCache Duration="60" VaryByParam="None" %>

<nav class="navbar navbar-expand-lg navbar-dark bg-dark">
    <div class="container-fluid">
        <asp:LinkButton class="navbar-brand" ID="Linkbutton1" runat="server">
            Memorial's Amor Eterno Desarrollo
        </asp:LinkButton>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav ms-auto">
                <li class="nav-item">
                    <asp:LinkButton ID="back" runat="server" class="btn btn-outline-info me-2">
                        Atrás
                        <i class="bi bi-arrow-return-left align-content-end"></i>
                    </asp:LinkButton>
                </li>
                <li class="nav-item">
                    <asp:LinkButton ID="logOut" runat="server" class="btn btn-outline-danger">
                        Salir
                        <i class="bi bi-x-lg align-content-end"></i>
                    </asp:LinkButton>
                </li>
            </ul>
        </div>
    </div>
</nav>
