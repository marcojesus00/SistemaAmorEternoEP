<%@ Page Language="vb" EnableEventValidation ="false" AutoEventWireup="false" CodeBehind="PantallaServicios.aspx.vb" Inherits="Sistema.PantallaServicios" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/> 
   
    <link rel="stylesheet" type="text/css" href="~/css/posiciones_Diamante.css"/> 
    <link rel="stylesheet" type="text/css" href="~/css/posiciones_lider.css"/> 
    <title></title>
    <script type="text/javascript">
        function zoom() {
            document.body.style.zoom = "124%" --
        }
    </script>
</head>
   
<body onload="zoom()" >
     <form id="form1" runat="server">
         <div>
    <%--         <asp:Table ID="Table1" runat="server">
                 <asp:TableRow runat="server">
                     <asp:TableCell runat="server">
                         <asp:Label ID="Label1" runat="server" Text="Desde: " /></asp:TableCell><asp:TableCell runat="server">
                             <asp:TextBox ID="txtF1" runat="server" TextMode="Date" /></asp:TableCell>
                 </asp:TableRow>
                 <asp:TableRow runat="server">
                     <asp:TableCell runat="server">
                         <asp:Label ID="Label2" runat="server" Text="Hasta: " /></asp:TableCell><asp:TableCell runat="server">
                             <asp:TextBox ID="txtF2" runat="server" TextMode="Date" /></asp:TableCell>
                 </asp:TableRow>
                 <asp:TableRow runat="server">
                     <asp:TableCell runat="server" ColumnSpan="2" HorizontalAlign="Center">
                         <asp:Button ID="btnBuscar" Text="Buscar" runat="server" /></asp:TableCell>
                 </asp:TableRow>
             </asp:Table>--%>

             <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                 <ContentTemplate>
                     <asp:Timer ID="Timer1" runat="server" />


                     <%-- Llenar Grid de Vendedores--%>
                     <asp:Panel ID="Panel12" runat="server">



                         <%-- Grid Categoria Oro --%>
                         <asp:GridView ID="GridView5" Caption="Servicios Entregados" CaptionAlign="Top" runat="server" AllowPaging="True" CellPadding="3" CssClass="Display" PageSize="20" PageIndex="1" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                             <AlternatingRowStyle CssClass="Grid" />
                             <FooterStyle BackColor="White" ForeColor="#000066" />
                             <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                             <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                             <RowStyle ForeColor="#000066" />
                             <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                             <SortedAscendingCellStyle BackColor="#F1F1F1" />
                             <SortedAscendingHeaderStyle BackColor="#007DBB" />
                             <SortedDescendingCellStyle BackColor="#CAC9C9" />
                             <SortedDescendingHeaderStyle BackColor="#00547E" />
                         </asp:GridView>


                     </asp:Panel>


                 </ContentTemplate>
                 <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                 </Triggers>
             </asp:UpdatePanel>



         </div>   
    </form>
</body>
</html>
