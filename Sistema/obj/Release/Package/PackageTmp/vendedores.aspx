<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="vendedores.aspx.vb" Inherits="Sistema.vendedores" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Mantenimiento Vendedor</title>
     <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
  <!-- Bootstrap core CSS -->  
  <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
    <link rel="shortcut icon" type="image/x-icon" href="svgs/solid/drafting-compass.svg"/>

  <!-- Custom styles for this template -->
  <link href="css/simple-sidebar.css" rel="stylesheet"/>
  <link rel="stylesheet" href="css/all.min.css" />
</head>
<body>    
    <form id="form1" runat="server">
        <div class="container-fluid" style="padding-top:5px;">

            <div><h4 style="width:100%; text-align:center">Mantenimiento de Vendedores</h4></div>
            <div class="container-fluid" >

                 <div class="row">
                   <div class="col-sm-12 col-md-4 col-lg-4">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <asp:label ID="lbl" runat="server" CssClass="input-group-text" ToolTip="" Text="Vendedor"></asp:label>
                           </div>
                           <asp:TextBox  CssClass="form-control form-control-sm" ID="txtvendedorN1" runat="server" ToolTip="Codigo o Nombre 1" />
                           <asp:TextBox  CssClass="form-control form-control-sm" ID="txtvendedorN2" runat="server" ToolTip="Codigo o Nombre 2" />
                       </div>

                   </div>
               
               </div>
                <div class="row">
                     <div class="col-sm-12 col-md-4 col-lg-4">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <asp:label ID="mac" runat="server" CssClass="input-group-text" ToolTip="" Text="MAC"></asp:label>
                           </div>
                           <asp:TextBox  CssClass="form-control form-control-sm" ID="txtMac" runat="server" ToolTip="Mac del Equipo" />
                          
                       </div>

                   </div>
                </div>
                <br />
                <div>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-dark btn-sm"/>

                </div>
            </div>
            <asp:GridView ControlStyle-CssClass="fas fa-arrow-right text-dark table-hover" ID="gvVendedores" runat="server" BackColor="White" 
                BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" AllowSorting="True" AutoGenerateEditButton="True" CssClass="fas fa-arrow-right text-dark" GridLines="Vertical">
                <AlternatingRowStyle BackColor="#DCDCDC" />
                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#0000A9" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#000065" />
            </asp:GridView>
            <asp:ImageButton ID="btnSalir" ToolTip="Regresar" runat="server" Height="30px" ImageUrl="~/imagenes/atras.png" Width="30px" style="position:fixed; right:10px; top:10px;"/>            
        </div>
        <script src="js/JQuery.js"></script>
        <script src="js/popper.min.js"></script>
        <script src="js/bootstrap.min.js"></script>
    <p><asp:Label ID="lblMsg" runat="server" Text=""></asp:Label></p></form></body></html>