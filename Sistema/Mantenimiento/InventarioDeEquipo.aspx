<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Shared/AE.Master" CodeBehind="InventarioDeEquipo.aspx.vb" Inherits="Sistema.InventarioDeEquipo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DropDownList ID="DDLCategory" runat="server" Placeholder="Categoria">

    </asp:DropDownList>
        <asp:DropDownList ID="DdlModel" runat="server" Placeholder="Marca">

    </asp:DropDownList>

            <asp:DropDownList ID="DdlBrand" runat="server" Placeholder="Marca">

    </asp:DropDownList>
    <asp:TextBox runat="server" Id="TxtboxSerialNumber" Placeholder="Número de serie">

    </asp:TextBox>
        <asp:TextBox runat="server" Id="TxtboxEmployeeName" Placheholder="Empleado">

    </asp:TextBox>

    <asp:LinkButton Id="lnkButtonAssingn" runat="server">
        Asignar
    </asp:LinkButton>
</asp:Content>
