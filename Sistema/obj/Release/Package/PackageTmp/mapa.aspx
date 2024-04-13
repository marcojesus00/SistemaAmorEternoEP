<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="mapa.aspx.vb" Inherits="Sistema.mapa" %>

<%@ Register Assembly="GMaps" Namespace="Subgurim.Controles" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="viewport" content="initial-scale=1.0, user-scalable=yes"/>
    <title></title>
    <link rel="stylesheet" href="https://netdna.bootstrapcdn.com/bootstrap/3.0.3/css/bootstrap.min.css"/>
    <link rel="stylesheet" href="https://netdna.bootstrapcdn.com/bootstrap/3.0.3/css/bootstrap-theme.min.css"/>
    <script src="https://code.jquery.com/jquery-1.10.2.min.js"></script>
    <script src="https://netdna.bootstrapcdn.com/bootstrap/3.0.3/js/bootstrap.min.js"></script>
   
    
    <script async defer src="https://maps.googleapis.com/maps/api/js?key=AIzaSyA9Jd6tfIGdNpgSrBIZ4ogstguDneOuZn0&callback=initMap" type="text/javascript"></script>
</head>
<body>    
    <form id="form1" runat="server">
        <button type="button" data-toggle="modal" data-target="#ModalMap" class="btn btn-default">
            <span class="glyphicon glyphicon-map-marker"></span> <span id="ubicacion">Seleccionar Ubicación:</span>
        </button>

        <div style="width:100%; height:100%; position:fixed; bottom:0px; left: 0px;">
            <cc1:GMap ID="GMap1" runat="server" enableHookMouseWheelToZoom="True" enableRotation="True" Height="100%" Width="100%" enableGKeyboardHandler="True" enableGoogleBar="True" Libraries="None" />
        </div>
        <%--<asp:ImageButton ID="btnSalir" runat="server" Height="30px" ImageUrl="~/imagenes/atras.png" Width="30px" style="position:fixed; right:10px; top:10px;"/>--%>   
                    
        <asp:Button ID="Vista" runat="server" Text="Satelite" style="position:fixed; right:10px; top:10px;"/>
                    
    </form>
</body>
</html>
