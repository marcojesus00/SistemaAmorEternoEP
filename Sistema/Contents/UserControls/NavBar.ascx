<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="NavBar.ascx.vb" Inherits="Sistema.NavBar" %>

                <nav class="navbar navbar-dark bg-dark">
                    <div class="container-fluid pr-lg-5">

                        <asp:LinkButton class="navbar-brand" ID="Linkbutton1" runat="server">
                   Memorial's Amor Eterno Test
                        </asp:LinkButton>
                        <asp:LinkButton ID="LinkButton2" runat="server" class="btn btn-outline-danger  ">
                     Cerrrar sesión &nbsp
                     <i class="text-sm-rigth fas fa-door-open" ></i>

                        </asp:LinkButton>
                        <button type="button" class="btn btn-primary">
                            <span class="bi-arrow-return-left align-content-end"></span>Regresar
                        </button>
                        <%--                      <div>
                                      <asp:ImageButton ID="btnSalir" ToolTip="Regresar" runat="server" Height="30px" ImageUrl="~/imagenes/atras.png" Width="30px" style="position:fixed; right:10px; top:10px;"/>--%>
                    </div>


        </nav>