<%@ Page Title="" Async="true" Language="vb" AutoEventWireup="false" MasterPageFile="~/shared/Map/MapIFrame.master" CodeBehind="MapClustered.aspx.vb" Inherits="Sistema.MapClusteredPage" %>
<asp:Content ID="h1" ContentPlaceHolderID="Headplace" runat="server">
    <style>

    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Map1" runat="server">
    <div class="row">
        <div class="col-auto">
            <asp:PlaceHolder ID="MapTitleHolder" runat="server"></asp:PlaceHolder>
            </div>
    <div class="col-auto">
        <button text="cerrar" id="Close2" class="btn btn-lg btn-danger btn-close border border-5 border-danger" data-toggle="tooltip" title="Cerrar mapa">
           
        </button>
    </div>
    </div>
        <div id="progress"><div id="progress-bar"></div></div>

    <div id="mapContainer" class="container-fluid" style="width: 100%; height: 500px;"></div>
    <div id="alertPlaceholder" runat="server"></div>
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            const tooltips = document.querySelectorAll('[data-toggle="tooltip"]');
            tooltips.forEach(function (tooltip) {
                new bootstrap.Tooltip(tooltip);
            });
        });
    </script>
</asp:Content>
