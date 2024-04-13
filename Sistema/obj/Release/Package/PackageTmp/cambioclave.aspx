<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="cambioclave.aspx.vb" Inherits="Sistema.cambioclave" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    <title>Cambio de Contraseña</title>    
    <link rel="shortcut icon" type="image/x-icon" href="~/imagenes/logo.ico"/>    
    <link href="css/BootStrap.min.css" rel="stylesheet" type="text/css"/> 
    <link href="css/3.3/BootStrap.min.css" rel="stylesheet" type="text/css"/>    
    
</head>
<body>    
    <form id="form1" runat="server">
        <div class="container-fluid" style="padding-top:10px;">
            <asp:Table ID="Table1" runat="server" HorizontalAlign="Center" Width="279px">
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server" HorizontalAlign="Left">Anterior:</asp:TableCell>
                    <asp:TableCell runat="server" HorizontalAlign="Left"><asp:TextBox ID="txtAnterior" TextMode="Password" runat="server" CssClass="form-control form-control-sm" Width="200" Height="24" Font-Size="Small"/></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server" HorizontalAlign="Left">Nueva:</asp:TableCell>
                    <asp:TableCell runat="server" HorizontalAlign="Left"><asp:TextBox ID="txtNueva" TextMode="Password" runat="server" CssClass="form-control form-control-sm" Width="200" Height="24" Font-Size="Small"/></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server" HorizontalAlign="Left">Confirmar:</asp:TableCell>
                    <asp:TableCell runat="server" HorizontalAlign="Left"><asp:TextBox ID="txtConfirmar" TextMode="Password" runat="server" CssClass="form-control form-control-sm" Width="200" Height="24" Font-Size="Small"/></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server" ColumnSpan ="2" HorizontalAlign="Center" style="padding:20px;"><asp:Button ID="btnCambiar" runat="server" Text="Cambiar" CssClass="btn btn-dark btn-sm"/></asp:TableCell>                 
                </asp:TableRow>
            </asp:Table>
            <asp:ImageButton ID="btnSalir" runat="server" Height="30px" ImageUrl="~/imagenes/atras.png" Width="30px" style="position:fixed; right:10px; top:10px;"/>
        </div>
        <script src="js/JQuery.js"></script>
        <script src="js/popper.min.js"></script>
        <script src="js/bootstrap.min.js"></script>
    </form>
</body>
</html>
