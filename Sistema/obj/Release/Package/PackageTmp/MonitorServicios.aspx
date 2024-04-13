<%@ Page Language="vb" EnableEventValidation ="false" AutoEventWireup="false" CodeBehind="MonitorServicios.aspx.vb" Inherits="Sistema.MonitorServicios" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" type="text/css" href="~/css/posiciones.css"/>     
    <link rel="stylesheet" type="text/css" href="~/css/posiciones-lider.css"/> 
    <title></title>
    <script type="text/javascript">
        function zoom() {
            document.body.style.zoom = "78%" 
        }
</script>
</head>
    
<body onload="zoom()" >
     <form id="form1" runat="server">
        <div> 
            <asp:Table ID="Table1" runat="server" >
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server"><asp:Label ID="Label1" runat="server" Text="Desde: "/></asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="txtF1" runat="server" TextMode="Date" /></asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                    <asp:TableCell runat="server"><asp:Label ID="Label2" runat="server" Text="Hasta: "/></asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="txtF2" runat="server" TextMode="Date" /></asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                    <asp:TableCell runat="server" ColumnSpan="2" HorizontalAlign="Center"><asp:Button ID="btnBuscar" Text="Buscar" runat="server"/></asp:TableCell>
                </asp:TableRow>
            </asp:Table>

            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>                
                <asp:Timer ID="Timer1" runat="server"/>
    
                <asp:Panel ID="Panel1" runat="server">
                    <asp:Chart runat="server" ID="GraficaLider" CssClass="Grafica" >
                        <series>
                            <asp:Series Name="Series1" ChartArea="ChartArea1" IsValueShownAsLabel="True" />
                        </series>
                        <chartareas>
                            <asp:ChartArea Name="ChartArea1">
                                <AxisX><MajorGrid Enabled="False" />
                                </AxisX></asp:ChartArea></chartareas>
                        <Titles><asp:Title Name="Title1" Text="" Font="Microsoft Sans Serif, 25pt, style=Bold" ForeColor="RoyalBlue">
                                </asp:Title>
                        </Titles>
                    </asp:Chart>                 
                    <asp:Image ID="Image38" runat="server" CssClass="image1" ImageUrl="~/imagenes/Emoji Smiley-01.png" />
                    <asp:Image ID="Image39" runat="server" CssClass="image2" ImageUrl="~/imagenes/Emoji Smiley-02.png" />
                    <asp:Image ID="Image40" runat="server" CssClass="image3" ImageUrl="~/imagenes/Emoji Smiley-03.png" />
                    <asp:Image ID="Image41" runat="server" CssClass="image4" ImageUrl="~/imagenes/Emoji Smiley-04.png" />
                    <asp:Image ID="Image42" runat="server" CssClass="image5" ImageUrl="~/imagenes/Emoji Smiley-05.png" />
                    <asp:Image ID="Image43" runat="server" CssClass="image6" ImageUrl="~/imagenes/Emoji Smiley-06.png" />
                    <asp:Image ID="Image44" runat="server" CssClass="image7" ImageUrl="~/imagenes/Emoji Smiley-16.png" />
                    <asp:Image ID="Image45" runat="server" CssClass="image8" ImageUrl="~/imagenes/Emoji Smiley-17.png" />
                    <asp:Image ID="Image46" runat="server" CssClass="image9" ImageUrl="~/imagenes/Emoji Smiley-20.png" />
                    <asp:Image ID="Image47" runat="server" CssClass="image10" ImageUrl="~/imagenes/Emoji Smiley-21.png" />
                    <asp:Image ID="Image48" runat="server" CssClass="image11" ImageUrl="~/imagenes/Emoji Smiley-22.png" />
                    <asp:Image ID="Image49" runat="server" CssClass="image12" ImageUrl="~/imagenes/Emoji Smiley-22.png" />
                </asp:Panel>

                <asp:Panel ID="Panel2" runat="server">
                    <asp:Chart runat="server" ID="GraficaVendedor" CssClass="Grafica" ><series><asp:Series Name="Series1" ChartArea="ChartArea1" IsValueShownAsLabel="True" /></series><chartareas><asp:ChartArea Name="ChartArea1"><AxisX><MajorGrid Enabled="False" /></AxisX></asp:ChartArea></chartareas><Titles><asp:Title Name="Title1" Text="" Font="Microsoft Sans Serif, 25pt, style=Bold" ForeColor="RoyalBlue"></asp:Title></Titles></asp:Chart>
                    <asp:Image ID="Image1" runat="server" CssClass="imagen1" ImageUrl="~/imagenes/Emoji Smiley-01.png" />
                    <asp:Image ID="Image2" runat="server" CssClass="imagen2" ImageUrl="~/imagenes/Emoji Smiley-02.png" />
                    <asp:Image ID="Image3" runat="server" CssClass="imagen3" ImageUrl="~/imagenes/Emoji Smiley-03.png" />
                    <asp:Image ID="Image4" runat="server" CssClass="imagen4" ImageUrl="~/imagenes/Emoji Smiley-04.png" />
                    <asp:Image ID="Image5" runat="server" CssClass="imagen5" ImageUrl="~/imagenes/Emoji Smiley-05.png" />
                    <asp:Image ID="Image6" runat="server" CssClass="imagen6" ImageUrl="~/imagenes/Emoji Smiley-06.png" />
                    <asp:Image ID="Image7" runat="server" CssClass="imagen7" ImageUrl="~/imagenes/Emoji Smiley-16.png" />
                    <asp:Image ID="Image8" runat="server" CssClass="imagen8" ImageUrl="~/imagenes/Emoji Smiley-17.png" />
                    <asp:Image ID="Image9" runat="server" CssClass="imagen9" ImageUrl="~/imagenes/Emoji Smiley-20.png" />
                    <asp:Image ID="Image10" runat="server" CssClass="imagen10" ImageUrl="~/imagenes/Emoji Smiley-21.png" />
                    <asp:Image ID="Image11" runat="server" CssClass="imagen11" ImageUrl="~/imagenes/Emoji Smiley-22.png" />
                    <asp:Image ID="Image12" runat="server" CssClass="imagen12" ImageUrl="~/imagenes/Emoji Smiley-22.png" />
                    <asp:Image ID="Image13" runat="server" CssClass="imagen13" ImageUrl="~/imagenes/Emoji Smiley-22.png" />
                    <asp:Image ID="Image14" runat="server" CssClass="imagen14" ImageUrl="~/imagenes/Emoji Smiley-24.png" />
                    <asp:Image ID="Image15" runat="server" CssClass="imagen15" ImageUrl="~/imagenes/Emoji Smiley-24.png" />
                </asp:Panel>

                <asp:Panel ID="Panel3" runat="server">
                    <asp:GridView ID="gvPosiciones" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" CssClass="Display" ForeColor="#333333" GridLines="None" PageSize="30"><AlternatingRowStyle BackColor="White" CssClass="Grid" /><Columns><asp:BoundField DataField="Posicion" HeaderText="N."><ItemStyle Font-Bold="True" ForeColor="#CC3300" /></asp:BoundField><asp:BoundField DataField="Codigo" HeaderText="Codigo" /><asp:BoundField DataField="Vendedor" HeaderText="Vendedor"><ItemStyle Wrap="True" /></asp:BoundField><asp:BoundField DataField="Ventas" HeaderText="Vts." /></Columns><EditRowStyle BackColor="#2461BF" /><FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" /><HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" /><PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" /><RowStyle BackColor="White" /><SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" /><SortedAscendingCellStyle BackColor="#F5F7FB" /><SortedAscendingHeaderStyle BackColor="#6D95E1" /><SortedDescendingCellStyle BackColor="#E9EBEF" /><SortedDescendingHeaderStyle BackColor="#4870BE" /></asp:GridView>
                    <asp:GridView ID="gvPosiciones0" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" CssClass="Display" ForeColor="#333333" GridLines="None" PageIndex="1" PageSize="30"><AlternatingRowStyle BackColor="White" CssClass="Grid"  /><Columns><asp:BoundField DataField="Posicion" HeaderText="N."><ItemStyle Font-Bold="True" ForeColor="#CC3300" /></asp:BoundField><asp:BoundField DataField="Codigo" HeaderText="Codigo" /><asp:BoundField DataField="Vendedor" HeaderText="Vendedor"><ItemStyle Wrap="True" /></asp:BoundField><asp:BoundField DataField="Ventas" HeaderText="Vts." /></Columns><EditRowStyle BackColor="#2461BF" /><FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" /><HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" /><PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" /><RowStyle BackColor="White"  /><SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" /><SortedAscendingCellStyle BackColor="#F5F7FB" /><SortedAscendingHeaderStyle BackColor="#6D95E1" /><SortedDescendingCellStyle BackColor="#E9EBEF" /><SortedDescendingHeaderStyle BackColor="#4870BE" /></asp:GridView>
                    <asp:GridView ID="gvPosiciones1" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" CssClass="Display" ForeColor="#333333" GridLines="None" PageIndex="2" PageSize="30"><AlternatingRowStyle BackColor="White" CssClass="Grid" /><Columns><asp:BoundField DataField="Posicion" HeaderText="N."><ItemStyle Font-Bold="True" ForeColor="#CC3300" /></asp:BoundField><asp:BoundField DataField="Codigo" HeaderText="Codigo" /><asp:BoundField DataField="Vendedor" HeaderText="Vendedor"><ItemStyle Wrap="True" /></asp:BoundField><asp:BoundField DataField="Ventas" HeaderText="Vts." /></Columns><EditRowStyle BackColor="#2461BF" /><FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" /><HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" /><PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" /><RowStyle BackColor="White" /><SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" /><SortedAscendingCellStyle BackColor="#F5F7FB" /><SortedAscendingHeaderStyle BackColor="#6D95E1" /><SortedDescendingCellStyle BackColor="#E9EBEF" /><SortedDescendingHeaderStyle BackColor="#4870BE" /></asp:GridView>
                    <asp:GridView ID="gvPosiciones2" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" CssClass="Display" ForeColor="#333333" GridLines="None" PageIndex="3" PageSize="30"><AlternatingRowStyle BackColor="White" CssClass="Grid" /><Columns><asp:BoundField DataField="Posicion" HeaderText="N."><ItemStyle Font-Bold="True" ForeColor="#CC3300" /></asp:BoundField><asp:BoundField DataField="Codigo" HeaderText="Codigo" /><asp:BoundField DataField="Vendedor" HeaderText="Vendedor"><ItemStyle Wrap="True" /></asp:BoundField><asp:BoundField DataField="Ventas" HeaderText="Vts." /></Columns><EditRowStyle BackColor="#2461BF" /><FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" /><HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" /><PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" /><RowStyle BackColor="White" /><SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" /><SortedAscendingCellStyle BackColor="#F5F7FB" /><SortedAscendingHeaderStyle BackColor="#6D95E1" /><SortedDescendingCellStyle BackColor="#E9EBEF" /><SortedDescendingHeaderStyle BackColor="#4870BE" /></asp:GridView>
                </asp:Panel>

                <asp:Panel ID="Panel4" runat="server">
                    <asp:Image ID="An1" runat="server" CssClass="Anuncio" Width="720px" ImageUrl="~/imagenes/Im1.png" />
                    <asp:Image ID="An2" runat="server" CssClass="Anuncio" Width="720px" ImageUrl="~/imagenes/Im2.png" />
                </asp:Panel>

                <asp:Panel ID="Panel5" runat="server">
                    <asp:Chart runat="server" ID="GraficaCobrador" CssClass="Grafica" ><series><asp:Series Name="Series1" ChartArea="ChartArea1" IsValueShownAsLabel="True" ChartType="Column" /></series><chartareas><asp:ChartArea Name="ChartArea1" AlignmentOrientation="None"><AxisY><LabelStyle ForeColor="White" /></AxisY><AxisX><MajorGrid Enabled="False" /></AxisX><Area3DStyle Inclination="10" Rotation="10" /></asp:ChartArea></chartareas><Titles><asp:Title Name="Title1" Text="" Font="Microsoft Sans Serif, 25pt, style=Bold" ForeColor="RoyalBlue"></asp:Title></Titles></asp:Chart>
                    <asp:Image ID="Image23" runat="server" CssClass="imagen1" ImageUrl="~/imagenes/Emoji Smiley-01.png" />
                    <asp:Image ID="Image24" runat="server" CssClass="imagen2" ImageUrl="~/imagenes/Emoji Smiley-02.png" />
                    <asp:Image ID="Image25" runat="server" CssClass="imagen3" ImageUrl="~/imagenes/Emoji Smiley-03.png" />
                    <asp:Image ID="Image26" runat="server" CssClass="imagen4" ImageUrl="~/imagenes/Emoji Smiley-04.png" />
                    <asp:Image ID="Image27" runat="server" CssClass="imagen5" ImageUrl="~/imagenes/Emoji Smiley-05.png" />
                    <asp:Image ID="Image28" runat="server" CssClass="imagen6" ImageUrl="~/imagenes/Emoji Smiley-06.png" />
                    <asp:Image ID="Image29" runat="server" CssClass="imagen7" ImageUrl="~/imagenes/Emoji Smiley-16.png" />
                    <asp:Image ID="Image30" runat="server" CssClass="imagen8" ImageUrl="~/imagenes/Emoji Smiley-17.png" />
                    <asp:Image ID="Image31" runat="server" CssClass="imagen9" ImageUrl="~/imagenes/Emoji Smiley-20.png" />
                    <asp:Image ID="Image32" runat="server" CssClass="imagen10" ImageUrl="~/imagenes/Emoji Smiley-21.png" />
                    <asp:Image ID="Image33" runat="server" CssClass="imagen11" ImageUrl="~/imagenes/Emoji Smiley-22.png" />
                    <asp:Image ID="Image34" runat="server" CssClass="imagen12" ImageUrl="~/imagenes/Emoji Smiley-22.png" />
                    <asp:Image ID="Image35" runat="server" CssClass="imagen13" ImageUrl="~/imagenes/Emoji Smiley-22.png" />
                    <asp:Image ID="Image36" runat="server" CssClass="imagen14" ImageUrl="~/imagenes/Emoji Smiley-24.png" />
                    <asp:Image ID="Image37" runat="server" CssClass="imagen15" ImageUrl="~/imagenes/Emoji Smiley-24.png" />
                </asp:Panel>

                <asp:Panel ID="Panel6" runat="server">
                    <asp:GridView ID="GridView1" CaptionAlign="Top" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" CssClass="Display" ForeColor="#333333" GridLines="None" PageSize="30"><AlternatingRowStyle BackColor="White" CssClass="Grid" /><Columns><asp:BoundField DataField="Numero" HeaderText="N."><ItemStyle Font-Bold="True" ForeColor="#CC3300" /></asp:BoundField><asp:BoundField DataField="Codigo" HeaderText="Codigo"><ItemStyle Wrap="True" /></asp:BoundField><asp:BoundField DataField="Cobrador" HeaderText="Cobrador" /><asp:BoundField DataField="Fecha" HeaderText="Ultima Liquidacion" /></Columns><EditRowStyle BackColor="#2461BF" /><FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" /><HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" /><PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" /><RowStyle BackColor="White" /><SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" /><SortedAscendingCellStyle BackColor="#F5F7FB" /><SortedAscendingHeaderStyle BackColor="#6D95E1" /><SortedDescendingCellStyle BackColor="#E9EBEF" /><SortedDescendingHeaderStyle BackColor="#4870BE" /></asp:GridView>
                    <asp:GridView ID="GridView2" CaptionAlign="Top" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" CssClass="Display" ForeColor="#333333" GridLines="None" PageSize="30" PageIndex="1"><AlternatingRowStyle BackColor="White" CssClass="Grid" /><Columns><asp:BoundField DataField="Numero" HeaderText="N."><ItemStyle Font-Bold="True" ForeColor="#CC3300" /></asp:BoundField><asp:BoundField DataField="Codigo" HeaderText="Codigo"><ItemStyle Wrap="True" /></asp:BoundField><asp:BoundField DataField="Cobrador" HeaderText="Cobrador" /><asp:BoundField DataField="Fecha" HeaderText="Ultima Liquidacion" /></Columns><EditRowStyle BackColor="#2461BF" /><FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" /><HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" /><PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" /><RowStyle BackColor="White" /><SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" /><SortedAscendingCellStyle BackColor="#F5F7FB" /><SortedAscendingHeaderStyle BackColor="#6D95E1" /><SortedDescendingCellStyle BackColor="#E9EBEF" /><SortedDescendingHeaderStyle BackColor="#4870BE" /></asp:GridView>
                    <asp:GridView ID="GridView3" CaptionAlign="Top" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" CssClass="Display" ForeColor="#333333" GridLines="None" PageSize="30" PageIndex="2"><AlternatingRowStyle BackColor="White" CssClass="Grid" /><Columns><asp:BoundField DataField="Numero" HeaderText="N."><ItemStyle Font-Bold="True" ForeColor="#CC3300" /></asp:BoundField><asp:BoundField DataField="Codigo" HeaderText="Codigo"><ItemStyle Wrap="True" /></asp:BoundField><asp:BoundField DataField="Cobrador" HeaderText="Cobrador" /><asp:BoundField DataField="Fecha" HeaderText="Ultima Liquidacion" /></Columns><EditRowStyle BackColor="#2461BF" /><FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" /><HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" /><PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" /><RowStyle BackColor="White" /><SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" /><SortedAscendingCellStyle BackColor="#F5F7FB" /><SortedAscendingHeaderStyle BackColor="#6D95E1" /><SortedDescendingCellStyle BackColor="#E9EBEF" /><SortedDescendingHeaderStyle BackColor="#4870BE" /></asp:GridView>
                </asp:Panel>

                <asp:Panel ID="Panel7" runat="server">
                    <asp:Chart runat="server" ID="GraficaVisitas" CssClass="Grafica" ><Series><asp:Series ChartArea="ChartArea1" Name="Series1"></asp:Series></Series><chartareas><asp:ChartArea Name="ChartArea1"><AxisX><MajorGrid Enabled="False" /></AxisX></asp:ChartArea></chartareas><Titles><asp:Title Name="Title1" Text="" Font="Microsoft Sans Serif, 25pt, style=Bold" ForeColor="RoyalBlue"></asp:Title></Titles></asp:Chart>
                </asp:Panel>

                <asp:Panel ID="Panel8" runat="server">
                    <asp:Chart runat="server" ID="GraficaVisitas1" CssClass="Grafica" ><Series><asp:Series ChartArea="ChartArea1" Name="Series1"></asp:Series></Series><chartareas><asp:ChartArea Name="ChartArea1"><AxisX><MajorGrid Enabled="False" /></AxisX></asp:ChartArea></chartareas><Titles><asp:Title Name="Title1" Text="" Font="Microsoft Sans Serif, 25pt, style=Bold" ForeColor="RoyalBlue"></asp:Title></Titles></asp:Chart>
                </asp:Panel>

                <asp:Panel ID="Panel9" runat="server">
                    <asp:Chart runat="server" ID="GraficaVisitas2" CssClass="Grafica" ><Series><asp:Series ChartArea="ChartArea1" Name="Series1"></asp:Series></Series><chartareas><asp:ChartArea Name="ChartArea1"><AxisX><MajorGrid Enabled="False" /></AxisX></asp:ChartArea></chartareas><Titles><asp:Title Name="Title1" Text="" Font="Microsoft Sans Serif, 25pt, style=Bold" ForeColor="RoyalBlue"></asp:Title></Titles></asp:Chart>
                </asp:Panel>

                <asp:Panel ID="Panel10" runat="server">
                    <asp:Chart runat="server" ID="GraficaVentasDiamante" CssClass="Grafica" ><series><asp:Series Name="Series1" ChartArea="ChartArea1" IsValueShownAsLabel="True" /></series><chartareas><asp:ChartArea Name="ChartArea1"><AxisX><MajorGrid Enabled="False" /></AxisX></asp:ChartArea></chartareas><Titles><asp:Title Name="Title1" Text="" Font="Microsoft Sans Serif, 25pt, style=Bold" ForeColor="RoyalBlue"></asp:Title></Titles></asp:Chart>
                </asp:Panel>

                <asp:Panel ID="Panel11" runat="server">
                    <asp:Chart runat="server" ID="GraficaVentasOro" CssClass="Grafica" ><series><asp:Series Name="Series1" ChartArea="ChartArea1" IsValueShownAsLabel="True" /></series><chartareas><asp:ChartArea Name="ChartArea1"><AxisX><MajorGrid Enabled="False" /></AxisX></asp:ChartArea></chartareas><Titles><asp:Title Name="Title1" Text="" Font="Microsoft Sans Serif, 25pt, style=Bold" ForeColor="RoyalBlue"></asp:Title></Titles></asp:Chart>
                </asp:Panel>

                <asp:Panel ID="Panel12" runat="server">
                    <asp:Chart runat="server" ID="GraficaVentasPlata" CssClass="Grafica" ><series><asp:Series Name="Series1" ChartArea="ChartArea1" IsValueShownAsLabel="True" /></series><chartareas><asp:ChartArea Name="ChartArea1"><AxisX><MajorGrid Enabled="False" /></AxisX></asp:ChartArea></chartareas><Titles><asp:Title Name="Title1" Text="" Font="Microsoft Sans Serif, 25pt, style=Bold" ForeColor="RoyalBlue"></asp:Title></Titles></asp:Chart>
                </asp:Panel>

                <asp:Panel ID="Panel13" runat="server">
                    <asp:Chart runat="server" ID="GraficaVentasBronce" CssClass="Grafica" ><series><asp:Series Name="Series1" ChartArea="ChartArea1" IsValueShownAsLabel="True" /></series><chartareas><asp:ChartArea Name="ChartArea1"><AxisX><MajorGrid Enabled="False" /></AxisX></asp:ChartArea></chartareas><Titles><asp:Title Name="Title1" Text="" Font="Microsoft Sans Serif, 25pt, style=Bold" ForeColor="RoyalBlue"></asp:Title></Titles></asp:Chart>
                </asp:Panel>

                </ContentTemplate><Triggers><asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" /></Triggers>
            </asp:UpdatePanel>
            
        </div>   
    </form>
</body>
</html>
