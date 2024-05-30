<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/shared/AE.Master" CodeBehind="Informacion.aspx.vb" Inherits="Sistema.Informacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="h-100">

        <div class="container mt-5">
            <div class="row">
                <div class="">
                    <div class="form-group">
                        <label for="textBoxCode">Buscar por código:</label>
                        <asp:TextBox ID="textBoxCode" runat="server" MaxLength="28" CssClass="form-control" placeholder="Código de cliente"></asp:TextBox>
                    </div>
<%--                    <asp:Button ID="Search" runat="server" CssClass="btn btn-primary" Text="Buscar" />--%>
                </div>
            </div>

        </div>
    </div>

</asp:Content>
