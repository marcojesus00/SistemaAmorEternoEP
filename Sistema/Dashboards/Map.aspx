<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Dashboards/Map.master" CodeBehind="Map.aspx.vb" Inherits="Sistema.MapPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Map1" runat="server">
                <asp:PlaceHolder ID="MapTitleHolder" runat="server"></asp:PlaceHolder>

    <div id="mapContainer" style="width: 800px; height: 500px;"></div>
            <div id="alertPlaceholder" runat="server"></div>

</asp:Content>
