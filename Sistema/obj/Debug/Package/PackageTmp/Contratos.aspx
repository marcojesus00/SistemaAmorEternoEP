<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Contratos.aspx.vb" Inherits="Sistema.Contratos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Mantenimiento Cobrador</title>
     <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    <link rel="shortcut icon" type="image/x-icon" href="~/imagenes/logo.ico"/>    
    <link href="css/BootStrap.min.css" rel="stylesheet" type="text/css"/> 
    <link href="css/3.3/BootStrap.min.css" rel="stylesheet" type="text/css"/>   
</head>
<body>
    
    <form id="form1" runat="server">
        <div class="container-fluid" style="padding-top:5px;">
            <asp:Table ID="Table1" runat="server" Style="margin-bottom:20px;">   
                <asp:TableRow runat="server">
                    <asp:TableCell>Buscar:</asp:TableCell>
                    <asp:TableCell runat="server"><asp:TextBox ID="txtCobrador" ToolTip="Codigo, Nombre o Identidad" placeholder="Codigo, Nombre o Identidad" runat="server" CssClass="form-control form-control-sm" Width="250" Height="24" Font-Size="Small"/></asp:TableCell></asp:TableRow><asp:TableRow runat="server">  
                        <asp:TableCell>Distribuidora:</asp:TableCell><asp:TableCell runat="server">
                            <asp:DropDownList ID="dlDistribuidora" ToolTip="Distribuidora" placeholder="Distribuidora" runat="server" CssClass="form-control form-control-sm" Width="250px" Height="26px" Font-Size="Small">
                            <asp:ListItem Selected="True"></asp:ListItem>
                        </asp:DropDownList></asp:TableCell></asp:TableRow><asp:TableRow runat="server">    
                    <asp:TableCell>Estado:</asp:TableCell><asp:TableCell runat="server"><asp:DropDownList ID="DlActivo" runat="server" CssClass="form-control form-control-sm" Width="150" Height="26" Font-Size="Small"/></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">   
                    <asp:TableCell>Contrato:</asp:TableCell><asp:TableCell runat="server"><asp:DropDownList ID="DLContrato" runat="server" CssClass="form-control form-control-sm" Width="150" Height="26" Font-Size="Small"/></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">        
                    <asp:TableCell runat="server" ColumnSpan="2">&nbsp</asp:TableCell></asp:TableRow><asp:TableRow runat="server">   
                    <asp:TableCell></asp:TableCell><asp:TableCell runat="server"><asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary btn-sm"/></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:GridView ID="gvCobradores" runat="server" BorderColor="#999999" Width="57%" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" ForeColor="#333333" GridLines="None" AllowSorting="True" AutoGenerateColumns="False"><AlternatingRowStyle BackColor="White" /><EditRowStyle BackColor="#2461BF" /><FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" /><HeaderStyle BackColor="#507CD1" Font-Bold="True" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" ForeColor="White" /><PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" /><RowStyle BackColor="#EFF3FB" /><SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" /><SortedAscendingCellStyle BackColor="#F5F7FB" /><SortedAscendingHeaderStyle BackColor="#6D95E1" /><SortedDescendingCellStyle BackColor="#E9EBEF" /><SortedDescendingHeaderStyle BackColor="#4870BE" /><Columns>
                    
                    <asp:BoundField DataField="P_num_emple" HeaderText="Codigo" SortExpression="P_num_emple"/>
                    <asp:BoundField DataField="P_nomb_empl" HeaderText="Nombre Completo" SortExpression="P_nomb_empl" />
                    <asp:BoundField DataField="P_identidad" HeaderText="Identidad" SortExpression="P_identidad" />
                    <asp:BoundField DataField="P_status" HeaderText="Activo" SortExpression="P_status"/>    
                    <asp:ButtonField CommandName="Codigo" Text="Ver" ControlStyle-ForeColor="DarkViolet">
                        <ControlStyle ForeColor="White" CssClass="label label-info"></ControlStyle>
</asp:ButtonField>
                

            </Columns>
            </asp:GridView>
            <br />
            <br />

            <asp:ImageButton ID="btnSalir" ToolTip="Regresar" runat="server" Height="30px" ImageUrl="~/imagenes/atras.png" Width="30px" style="position:fixed; left:55%; top:10px;"/>            
        </div>
        <asp:Panel ID="PanelEmpleado" runat="server"> 
            <div style="width:42%; height:100%;">
                <iframe id="ifEmpleado" runat="server" src="Empleados.aspx" style="position:fixed; width:42%; height:100%; left:58%; top:0px;"></iframe>
                
            </div>
        </asp:Panel>
        <div style="width:58%; height:30px; position:fixed; bottom:0px; vertical-align:middle; background-color:#202020; left: 0px; color: #fff">
            &nbsp&nbsp<asp:Label ID="lblbarra" runat="server" Text=""></asp:Label></div><script src="js/JQuery.js"></script><script src="js/popper.min.js"></script><script src="js/bootstrap.min.js"></script>
        </form>
</body>
</html>
<!--<asp:ButtonField Text="SAP" CommandName="SAP" ControlStyle-CssClass="label label-info"><ControlStyle ForeColor="White" /></asp:ButtonField>-->
