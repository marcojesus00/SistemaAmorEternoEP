﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/shared/Map/MapIFrame.master" CodeBehind="Map.aspx.vb" Inherits="Sistema.MapPage" %>
<asp:Content ID="h1" ContentPlaceHolderID="Headplace" runat="server">
    <style>
/*                button#Close2:hover svg {
            fill: #a00c0c;
            stroke: #a00c0c;
        }
                        button#Close2 svg {
            fill: #d80e0e;
            stroke: #d80e0e;
            stroke-width: 7;
            width: 40px;
            height: 40px;
            margin-left: 5px;
        }
               */        
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Map1" runat="server">
    <div class="row">
        <div class="col-auto">
            <asp:PlaceHolder ID="MapTitleHolder" runat="server"></asp:PlaceHolder>
            </div>
    <div class="col-auto">
        <button text="cerrar" id="Close2" class="btn btn-lg btn-danger btn-close border border-5 border-danger" data-toggle="tooltip" title="Cerrar mapa">
           
<%--                  <svg fill="#d80e0e" version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 401.411 401.411" xml:space="preserve" stroke="#d80e0e" stroke-width="7.626809">
            <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
            <g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g>
            <g id="SVGRepo_iconCarrier">
                <g>
                    <g>
                        <path d="M214.798,194.621c0.853,0,1.709-0.11,2.551-0.337l63.846-17.107c3.402-0.91,6.06-3.566,6.97-6.97 c0.912-3.4-0.061-7.031-2.551-9.52l-16.398-16.398L396.678,16.826c3.852-3.849,3.852-10.091,0-13.938 c-3.846-3.851-10.093-3.851-13.938,0L255.276,130.351l-16.397-16.397c-2.491-2.493-6.118-3.468-9.521-2.552 c-3.4,0.91-6.061,3.567-6.971,6.97l-17.108,63.842c-0.912,3.4,0.06,7.03,2.551,9.521C209.702,193.608,212.218,194.621,214.798,194.621z"></path>
                        <path d="M269.214,257.118l16.396-16.396c2.491-2.488,3.463-6.12,2.551-9.521c-0.909-3.399-3.566-6.06-6.97-6.969l-63.84-17.11 c-3.389-0.914-7.026,0.057-9.521,2.551c-2.49,2.488-3.463,6.117-2.551,9.521l17.104,63.845c0.91,3.402,3.566,6.061,6.97,6.969 c0.843,0.228,1.698,0.338,2.551,0.338c2.58,0,5.098-1.014,6.97-2.889l16.4-16.399l127.46,127.466 c1.926,1.925,4.446,2.888,6.97,2.888s5.046-0.963,6.97-2.888c3.851-3.85,3.851-10.092,0-13.938L269.214,257.118z"></path>
                        <path d="M179.021,118.371c-0.91-3.402-3.569-6.06-6.969-6.97c-3.389-0.911-7.032,0.059-9.52,2.552l-16.398,16.397L18.67,2.888 c-3.846-3.851-10.093-3.851-13.938,0c-3.851,3.848-3.851,10.09,0,13.938l127.462,127.463l-16.397,16.398 c-2.491,2.488-3.463,6.119-2.551,9.52c0.91,3.403,3.566,6.06,6.969,6.97l63.845,17.107c0.842,0.227,1.699,0.337,2.551,0.337 c2.58,0,5.097-1.016,6.969-2.888c2.491-2.49,3.463-6.12,2.551-9.521L179.021,118.371z"></path>
                        <path d="M184.058,207.123l-63.84,17.11c-3.403,0.909-6.06,3.568-6.969,6.969c-0.912,3.4,0.06,7.032,2.551,9.521l16.396,16.396 L4.734,384.584c-3.85,3.848-3.85,10.09,0,13.938c1.925,1.925,4.447,2.888,6.969,2.888s5.046-0.963,6.969-2.888l127.46-127.466 l16.4,16.399c1.873,1.875,4.39,2.889,6.969,2.889c0.852,0,1.708-0.11,2.551-0.338c3.402-0.908,6.06-3.565,6.969-6.969 l17.105-63.845c0.912-3.403-0.06-7.032-2.551-9.521C191.086,207.185,187.451,206.211,184.058,207.123z"></path>
                    </g>
                </g>
            </g>
        </svg>--%>
        </button>
    </div>
    </div>

    <div id="mapContainer" class="container-fluid" style="width: 100%; height: 500px;"></div>
    <div id="alertPlaceholder" runat="server"></div>
    <script>
    $(document).ready(function () {
        $('[data-toggle="tooltip"]').tooltip();
    });
</script>
</asp:Content>
