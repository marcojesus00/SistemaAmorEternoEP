<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="inicioOld1.aspx.vb" Inherits="Sistema.inicioOld1" %>

<!DOCTYPE html>

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Login | Grupo Amor Eterno </title>
    <link rel="stylesheet" href="css/style.css" />
    <link href="css/HomeDef.css" rel="stylesheet" />
    <link href='https://unpkg.com/boxicons@2.1.4/css/boxicons.min.css' rel='stylesheet' />
    <script src="js/JQuery.js"></script>
    <script src="js/popper.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
      <%--  <div class="logo-container">
            <img src="imagenes/LogoAmorEterno1.jpeg" alt="Logo" class="" />
        </div>--%>

        <div class="wrapper">
            <h1>Login</h1>
            <div class="input-box">
                <asp:TextBox ID="txtUsuario" CssClass="TextBox" runat="server" placeholder="Usuario" TextMode="SingleLine" Required="true" />
                <i class='bx bxs-user'></i>
            </div>
            <div class="input-box">
                <asp:TextBox ID="txtPassword" TextMode="Password" CssClass="TextBox" runat="server" placeholder=" Clave" Required="true" />
                <i class='bx bxs-lock-alt'></i>
            </div>
            <div class="remember-forgot">
                <label><asp:CheckBox ID="chkRemember" runat="server" Text="Remember Me" /></label>
            </div>
            <asp:Button CssClass="btn" ID="Button" runat="server" Text="Login" />
        </div>
    </form>
</body>
</html>
