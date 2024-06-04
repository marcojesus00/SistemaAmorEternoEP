<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="NavBar.ascx.vb" Inherits="Sistema.NavBar" %>

<nav class="navbar navbar-dark bg-dark">
    <div class="container text-center w-100">
        <div class="row w-100">
            <div class="col-10">
                <asp:LinkButton class="navbar-brand" ID="Linkbutton1" runat="server">
                   Memorial's Amor Eterno Desarrollo
                </asp:LinkButton>
            </div>

            <div class="col-1">
                <asp:LinkButton ID="back" runat="server" class="btn btn-outline-info  ">
                     Atrás
                     <i class="bi-arrow-return-left align-content-end" ></i>
                </asp:LinkButton>
            </div>

            <div class="col-1">
                <asp:LinkButton ID="logOut" runat="server" class="btn btn-outline-danger  ">
                     Cerrrar sesión
                </asp:LinkButton>

            </div>

        </div>



    </div>


</nav>
