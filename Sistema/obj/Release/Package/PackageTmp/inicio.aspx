<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="inicio.aspx.vb" Inherits="Sistema.inicio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
<meta name="viewport" content="initial-scale=1.0, user-scalable=yes"/>    
    <form id="form1" runat="server">
        <div>
            <asp:Image ID="Image1" runat="server" ImageUrl="~/imagenes/LOGO.bmp" />
        </div>
        
        <div>
             <asp:Table ID="Table1" runat="server" Width="279px" HorizontalAlign="Center">                
                <asp:TableRow runat="server"><asp:TableCell runat="server" HorizontalAlign="Left">Usuario:</asp:TableCell><asp:TableCell runat="server" HorizontalAlign="Left"><asp:TextBox ID="txtUsuario" runat="server"/></asp:TableCell></asp:TableRow>
                <asp:TableRow runat="server"><asp:TableCell runat="server" HorizontalAlign="Left">Clave:</asp:TableCell><asp:TableCell runat="server" HorizontalAlign="Left"><asp:TextBox ID="txtPassword" TextMode="Password" runat="server"/></asp:TableCell></asp:TableRow>
                <asp:TableRow runat="server"><asp:TableCell runat="server" HorizontalAlign="Left">Compañia:</asp:TableCell><asp:TableCell runat="server" HorizontalAlign="Left"><asp:DropDownList ID="dlCompania" runat="server"></asp:DropDownList></asp:TableCell></asp:TableRow>
                <asp:TableRow runat="server"><asp:TableCell runat="server"></asp:TableCell><asp:TableCell runat="server" HorizontalAlign="Left"><asp:Button OnClick="Button1_Click" ID="Button" runat="server" Text="Iniciar" /></asp:TableCell></asp:TableRow>           
            </asp:Table>

        </div>
    </form>
</body>
</html>
