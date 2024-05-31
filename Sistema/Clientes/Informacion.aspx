<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/shared/AE.Master" CodeBehind="Informacion.aspx.vb" Inherits="Sistema.Informacion" %>
<%@ Register TagPrefix="uc" TagName="ClientsTable" Src="~/controls/informacionDeCliente/ClientsTable.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="h-100">
        <p class="fs-2">Información de los clientes</p>

        <div class="container mt-5">
            <div class="row">
                <div class="col-4">
                    <div class="form-group d-flex align-items-center">
                        <label for="textBoxCode" class="p-2">Buscar</label>
                        <asp:TextBox ID="textBoxCode" runat="server" MaxLength="28" CssClass="form-control p-2" placeholder="Nombre, código o identidad"></asp:TextBox>
                        <i class="bi bi-search p-2"></i>
                    </div>
                </div>
            </div>

        </div>
                               <uc:ClientsTable class="w-100 " ID="ClientsTable1" runat="server" />

    </div>
     <div id="alertPlaceholder" runat="server"></div>

</asp:Content>
