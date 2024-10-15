<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="inicio.aspx.vb" Inherits="Sistema.inicio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Grupo Amor Eterno</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    <%--<meta name="viewport" content="initial-scale=1.0, user-scalable=yes"/>--%>    

    <link rel="shortcut icon" type="image/x-icon" href="~/imagenes/logo.ico" />
    <link href="css/BootStrap.min.css" rel="stylesheet" type="text/css"/> 
    <link rel="stylesheet" type="text/css" href="~/css/inicio.css"/> 
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel runat="server" HorizontalAlign="Center">
        <div>
            <img  alt="Imagen" width="220" src="imagenes/LogoActual.jpg"/>            
            <%--<h1>Grupo Amor Eterno</h1>--%>
        </div>
        
        <div class="login">
             <asp:Table ID="Table1" runat="server" Width="100%" HorizontalAlign="Center">                
                <asp:TableRow runat="server" >                
                    <asp:TableCell runat="server">
                        <asp:TextBox ID="txtUsuario" CssClass="TextBox" runat="server" placeholder=" Usuario" />
                    </asp:TableCell></asp:TableRow><asp:TableRow runat="server" >
                     <asp:TableCell runat="server">
                         <asp:TextBox ID="txtPassword" TextMode="Password" class="TextBox" runat="server" placeholder=" Clave" />
                     </asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                     <asp:TableCell runat="server">
                         <asp:DropDownList ID="dlCompania" class="TextBox" runat="server"/>
                     </asp:TableCell></asp:TableRow><asp:TableRow runat="server"> 
                         
                     <asp:TableCell runat="server">
                         
                         <asp:Button CssClass="btn btn-danger"  ID="Button" runat="server" Text="Perro" />
                     </asp:TableCell></asp:TableRow></asp:Table></div></asp:Panel><script src="js/JQuery.js"></script><script src="js/popper.min.js"></script><script src="js/bootstrap.min.js"></script></form></body></html>