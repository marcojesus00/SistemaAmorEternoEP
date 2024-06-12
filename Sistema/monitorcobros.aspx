<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="monitorcobros.aspx.vb" Inherits="Sistema.monitorcobros" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="viewport" content="initial-scale=1.0, user-scalable=yes"/>
    <title>Monitor de Cobros</title>
    <link rel="shortcut icon" type="image/x-icon" href="~/imagenes/logo.ico"/>
    <link href="css/BootStrap.min.css" rel="stylesheet" type="text/css"/>    
    <link href="css/DataGrid.css" rel="stylesheet" type="text/css"/>    
    <link href="css/inicio.css" rel="stylesheet" type="text/css"/>    

    <style>
h4:hover {
    background-color: aliceblue;
    color: blue;
}

 /*#gvMonitor tr.rowHover:hover
        {
            background-color: #FFEB9C;
            border-top: solid;
            color:#9C6500;
        }*/


</style>
</head>
<body>  
    <form id="form1" runat="server">
        <asp:Panel ID="Panel1" runat="server" HorizontalAlign="left">
            <div class="container-fluid" style="padding-top:5px;">       
              
                   <div><h4 style="width:100%; text-align:center">Monitor de Cobros</h4></div>
     
            <div class="container-fluid" style="padding-top:5px;"> 
                    <div class="row">
                      <div class="col-sm-4 col-d-3 col-lg-2">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="txtFechalb" class="input-group-text" style="width: 75px">Fecha</label>
                             
                           </div>
                           <%--<input type="date" class="form-control form-control-sm" id="txtFecha" runat="server" />--%>
                           <asp:TextBox runat="server" TextMode="Date" CssClass="form-control form-control-sm" ID="txtFecha"></asp:TextBox>
                       </div>
                   </div>
                    
               </div>
                <div class="row">
                 <div class="col-sm-4 col-d-3 col-lg-2">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dlAlmacen" class="input-group-text" style="width: 75px">Mostrar</label>
                           </div>
                           <asp:DropDownList ID="dlMostrar" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                       </div>
                   </div>  
                </div>
                 <div class="row">
                 <div class="col-sm-4 col-d-3 col-lg-2">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dlAlmacen" class="input-group-text" style="width: 75px">Lider</label>
                           </div>
                           <asp:DropDownList ID="dllider" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                       </div>
                   </div>  
                </div>
                 <div class="row">
                 <div class="col-sm-4 col-d-3 col-lg-2">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dlAlmacen" class="input-group-text" style="width: 75px">Zona</label>
                           </div>
                           <asp:DropDownList ID="dlzona" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                       </div>
                   </div>  
                </div>
                  <div class="row">
                      <div class="col-sm-4 col-d-3 col-lg-2">
                       <input type="text" class="form-control form-control-sm" placeholder="Codigo Cobrador" id="txtCobrador" runat="server" />
                    </div>
                        <asp:Button ID="btnBuscar" runat="server" Text="   Buscar   " CssClass="btn btn-sm btn-primary" />&nbsp&nbsp
                   </div>
                
          </div>
               <%-- <asp:Table ID="Table1" runat="server" Style="margin-bottom:20px;">
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">Fecha:</asp:TableCell>
                        <asp:TableCell runat="server"><asp:TextBox ID="txtFecha" CssClass="form-control form-control-sm" runat="server" TextMode="Date" Height="24" /></asp:TableCell></asp:TableRow><asp:TableRow runat="server">                    
                        <asp:TableCell runat="server">Mostrar:</asp:TableCell><asp:TableCell runat="server"><asp:DropDownList ID="dlMostrar" CssClass="form-control form-control-sm" runat="server" Width="200" Height="24" Font-Size="Small"/></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server">  
                        <asp:TableCell runat="server">Lider:</asp:TableCell><asp:TableCell runat="server"><asp:DropDownList ID="dlLider" CssClass="form-control form-control-sm" runat="server" Width="200" Height="24" Font-Size="Small"/></asp:TableCell>
                    </asp:TableRow>     
                    <asp:TableRow runat="server">  
                        <asp:TableCell runat="server">Zona:</asp:TableCell><asp:TableCell runat="server"><asp:DropDownList ID="dlZona" CssClass="form-control form-control-sm" runat="server" Width="200" Height="24" Font-Size="Small"/></asp:TableCell>
                    </asp:TableRow>  
                    <asp:TableRow runat="server">                    
                        <asp:TableCell runat="server">Cobrador:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="txtCobrador" CssClass="form-control form-control-sm" runat="server" Width="200" Height="24" Font-Size="Small" /></asp:TableCell></asp:TableRow><asp:TableRow runat="server">                    
                        <asp:TableCell runat="server" ColumnSpan="2">&nbsp</asp:TableCell></asp:TableRow><asp:TableRow runat="server">                    
                        <asp:TableCell runat="server" ColumnSpan="2"><asp:Button CssClass="btn btn-dark btn-sm" ID="btnBuscar" runat="server" Text="Buscar" /></asp:TableCell>                   
                    </asp:TableRow>
                </asp:Table>--%>

             
                <asp:GridView ID="gvMonitor" runat="server"  DataKeyNames="Codigo" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" AllowSorting="True"><AlternatingRowStyle BackColor="#DCDCDC" /><Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <img alt = "" src="imagenes/plus.png" />
                                <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                    <asp:GridView ID="gvDetalle" runat="server" AutoGenerateColumns="false"  CssClass="ChildGrid"><AlternatingRowStyle BackColor="#eeeeee" />
                                        <Columns>                                       
                                            <asp:BoundField DataField="Codigo" HeaderText="Codigo" ItemStyle-Width="120px" />
                                            <asp:BoundField DataField="Cobrado" HeaderText="Cobrado" ItemStyle-Width="80px" />
                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" ItemStyle-Width="80px" />
                                            <asp:BoundField DataField="Hora" HeaderText="Hora" ItemStyle-Width="60px" />
                                            <asp:BoundField DataField="Codigo Cliente" HeaderText="Codigo Cliente" ItemStyle-Width="100px" />
                                            <asp:BoundField DataField="Nombre_clie" HeaderText="Nombre Cliente" ItemStyle-Width="185px" ControlStyle-Font-Size="Small" />
                                            <asp:BoundField DataField="CONCEPTO" HeaderText="Motivo" ItemStyle-Width="240px" ControlStyle-Font-Size="Small" />
                                            <asp:BoundField DataField="Dir_cliente" HeaderText="Direccion" ItemStyle-Width="360px" ControlStyle-Font-Size="Small" />                                            
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>  
                        <asp:ButtonField CommandName="Liquidar" Text="Liquidar" Visible="False"/><asp:ButtonField Text="Imprimir" CommandName ="Imprimir" Visible="false"/><asp:ButtonField Text="Mapa" CommandName ="Mapa" Visible="true"/>
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" /><HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" /><PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" /><SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" /><SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#0000A9" /><SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#000065" /></asp:GridView> 
                <br />
                <asp:GridView ID="gvMonitor2" runat="server"  DataKeyNames="Codigo" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" AllowSorting="True"><Columns>                            
                    </Columns>
                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" /><HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" /><PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" /><RowStyle BackColor="White" ForeColor="#003399" /><SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" /><SortedAscendingCellStyle BackColor="#EDF6F6" /><SortedAscendingHeaderStyle BackColor="#0D4AC4" /><SortedDescendingCellStyle BackColor="#D6DFDF" /><SortedDescendingHeaderStyle BackColor="#002876" /></asp:GridView> 
                <asp:GridView ID="gvDetalle2" runat="server"  DataKeyNames="Codigo" AllowSorting="True" Visible="False" CssClass="ChildGrid" AutoGenerateColumns="False" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4"><Columns>
                    <asp:ButtonField CommandName="Anular" Text="Anular"/>
                    <asp:BoundField DataField="Codigo" HeaderText="Codigo" ItemStyle-Width="120px" ><ItemStyle Width="120px"></ItemStyle></asp:BoundField>
                    <asp:BoundField DataField="Cobrado" HeaderText="Cobrado" ItemStyle-Width="80px" ><ItemStyle Width="80px"></ItemStyle></asp:BoundField>
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" ItemStyle-Width="80px" ><ItemStyle Width="80px"></ItemStyle></asp:BoundField>
                    <asp:BoundField DataField="Hora" HeaderText="Hora" ItemStyle-Width="60px" ><ItemStyle Width="60px"></ItemStyle></asp:BoundField>
                    <asp:BoundField DataField="Codigo Cliente" HeaderText="Codigo Cliente" ItemStyle-Width="100px" ><ItemStyle Width="100px"></ItemStyle></asp:BoundField>
                    <asp:BoundField DataField="Nombre_clie" HeaderText="Nombre Cliente" ItemStyle-Width="185px" ControlStyle-Font-Size="Small" ><ControlStyle Font-Size="Small"></ControlStyle>

    <ItemStyle Width="180px"></ItemStyle>
    </asp:BoundField><asp:BoundField DataField="CONCEPTO" HeaderText="Motivo" ItemStyle-Width="220px" ControlStyle-Font-Size="Small" ><ControlStyle Font-Size="Small"></ControlStyle>

    <ItemStyle Width="220px"></ItemStyle>
    </asp:BoundField></Columns><FooterStyle BackColor="#99CCCC" ForeColor="#003399" /><HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" /><PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" /><RowStyle BackColor="White" ForeColor="#003399" /><SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" /><SortedAscendingCellStyle BackColor="#EDF6F6" /><SortedAscendingHeaderStyle BackColor="#0D4AC4" /><SortedDescendingCellStyle BackColor="#D6DFDF" /><SortedDescendingHeaderStyle BackColor="#002876" /></asp:GridView> 
                <br /><br />
                
            <asp:ImageButton ID="btnSalir" ToolTip="Regresar" runat="server" Height="30px" ImageUrl="~/imagenes/atras.png" Width="30px" style="position:fixed; right:10px; top:10px;"/>
            <asp:Label ID="lblMsg" runat="server"></asp:Label></div><div style="width:100%; height:30px;  position:fixed; bottom:0px; background-color:#202020; left: 0px; color: #fff">
            <asp:Label ID="lblTotal" runat="server" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblRecibos" runat="server" Text="" />&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblVisitados" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblCobradores" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblHora" runat="server" Text="" ForeColor="White"/>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="Panel2" Visible="false">
            <div class="container-fluid" style="padding-top:20px;">
                <div class="row">
                    <div class="col-md-4"></div>
                    <div class="col-md-4 login alert alert-info">
                        <asp:table runat="server" Width="100%" HorizontalAlign="Center">
                            <asp:tablerow>
                                <asp:tablecell ColumnSpan="2" HorizontalAlign="Center"><h4 class="alert-heading">Anular el recibo: <asp:Label ID="lblMensaje" runat="server"/></h4></asp:tablecell></asp:tablerow><asp:tablerow>
                                <asp:tablecell ColumnSpan="2" HorizontalAlign="Center"><b><asp:Label ID="lblMensaje1" runat="server"/></b></asp:tablecell></asp:tablerow><asp:tablerow>
                                <asp:tablecell ColumnSpan="2" HorizontalAlign="Center">Por Valor de: <b><asp:Label ID="lblMensaje2" runat="server"/></b></asp:tablecell></asp:tablerow><asp:TableRow>                                                             
                                 <asp:TableCell HorizontalAlign="Right">Comentario: </asp:TableCell><asp:TableCell HorizontalAlign="left"><asp:TextBox runat="server" ID="txtComentario" CssClass="form-control form-control-sm" Height ="24" Width="150"></asp:TextBox></asp:TableCell></asp:TableRow><asp:TableRow>
                                <asp:TableCell HorizontalAlign="Right">Clave: </asp:TableCell><asp:TableCell HorizontalAlign="left"><asp:TextBox runat="server" ID="txtClave" TextMode="Password" CssClass="form-control form-control-sm" Height ="24" Width="150"></asp:TextBox></asp:TableCell></asp:TableRow><asp:TableRow>
                                <asp:TableCell ColumnSpan="2">&nbsp</asp:TableCell></asp:TableRow><asp:TableRow>
                                <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                                <asp:Button runat="server" ID="btnAceptar" Text="Aceptar" CssClass="btn btn-primary btn-sm"/>&nbsp
                                <asp:Button runat="server" ID="btnCancelar" Text="Cancelar" CssClass="btn btn-secondary btn-sm"/></asp:TableCell>
                            </asp:TableRow>
                        </asp:table></div><div class="col-md-4"></div>
                </div>
            </div>
        </asp:Panel> 
        <asp:Panel ID="PanelImpresion" runat="server" Visible="false">   
            <div>
                <asp:Button ID="btnProcesar" ToolTip="Procesar" Text="Procesar" runat="server" Height="30px" CssClass="btn btn-primary btn-sm" style=" position:fixed; left:10px; top:10px;"/>                
                <asp:ImageButton ID="btnRegresar" ToolTip="Regresar" runat="server" Height="30px" style="position:fixed; right:10px; top:10px;" ImageUrl="~/imagenes/atras.png" Width="30px"/>
            </div>
            <div style="width:100%; height:100%;">
            <iframe id="ifRepote" runat="server" style="position:fixed; width:100%; height:100%; top:42px;"></iframe>
            </div>
        </asp:Panel>   
<%--        <!-- Stream video via webcam -->
        <div class="video-wrap">
            <video id="video" playsinline autoplay></video>            
        </div>

        <!-- Trigger canvas web API -->
        <div class="controller">
            <button id="snap">Capture</button>
        </div>

        <!-- Webcam video snapshot -->
        <canvas id="canvas" width="640" height="480"></canvas>--%>
     
                <script src="js/JQuery.js"></script><script src="js/popper.min.js"></script><script src="js/bootstrap.min.js"></script><script src="js/Jquery1.8.3.js"></script><script type="text/javascript">
            $("[src*=plus]").live("click", function () {
                $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
                $(this).attr("src", "imagenes/minus.png");
            });
            $("[src*=minus]").live("click", function () {
                $(this).attr("src", "imagenes/plus.png");
                $(this).closest("tr").next().remove();
            });
        </script>
        <%--<script src="js/Camara.js"></script>--%>
    </form>
</body>
</html>