<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Dashboards/Dashboard.master" CodeBehind="CobrosDashboard.aspx.vb" Inherits="Sistema.CobrosDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Filters" runat="server">

    <div class="container mt-3">
        <div class="row mb-2">
            <div class="col-md-3">
                <asp:TextBox ID="startDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                <label for="startDate" class="text-left  text-secondary">Fecha inicial</label>

            </div>
            <div class="col-md-3">
                <asp:TextBox ID="endDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                <label for="endDate" class="text-left  text-secondary">Fecha final</label>

            </div>
                        <div class="col-sm-3">
                <asp:TextBox ID="zone" runat="server" CssClass="form-control"></asp:TextBox>
            <label for="zone" class="text-left  text-secondary">Zona</label>

            </div>

            <div class="col-sm-3">
                <asp:TextBox ID="company" runat="server" CssClass="form-control"></asp:TextBox>
                            <label for="company" class="text-left  text-secondary">Empresa</label>

            </div>
        </div>

        <div class="row mb-2">
            <div class="col-md-3">
                <asp:TextBox ID="code" runat="server" CssClass="form-control"></asp:TextBox>
                <label for="code" class="text-left  text-secondary">Código de cobrador</label>

            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="leader" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Líder actual" Value="current" />
                    <asp:ListItem Text="Líder anterior" Value="previous" />
                </asp:DropDownList>
                                <label for="leader" class="text-left text-secondary">Estado del lider</label>

            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Abdul" Value="current" />
                    <asp:ListItem Text="Líder Anterior" Value="previous" />
                </asp:DropDownList>
                                <label for="DropDownList1" class="text-left  text-secondary">Lider</label>

            </div>

            <div class="col-sm-3">
                <div class="form-check">
                    <asp:CheckBox ID="supervised" runat="server" />
                    <label for="supervised" class="form-check-label">Incluir supervisado</label>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12 d-flex justify-content-center">
                <asp:Button ID="submitButton" runat="server" Text="Aplicar Filtros" CssClass="btn btn-primary" />
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Table" runat="server">
</asp:Content>
