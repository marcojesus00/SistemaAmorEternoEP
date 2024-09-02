<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Inhumados.aspx.vb" Inherits="Sistema.Inhumados" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="viewport" content="initial-scale=1.0, user-scalable=yes"/>
    <title>Clientes</title>
    <link rel="shortcut icon" type="image/x-icon" href="~/imagenes/logo.ico" />
    <link href="css/BootStrap.min.css" rel="stylesheet" type="text/css"/> 
    <link href="css/DataGrid.css" rel="stylesheet" type="text/css"/>   
</head>
<body>
    <meta name="viewport" content="initial-scale=1.0, user-scalable=yes"/>
    <form id="form1" runat="server">
        <div><h4 style="width:100%; text-align:center">Personas Ihumadas Parque Amor Eterno</h4></div>
         <div class="container-fluid" style="margin-top:15px;">   
            
            <div class="row">
                    <div class="col-sm-12 col-lg-3 col-md-6">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dvale" class="input-group-text" style="width: 110px">Codigo Cliente</label>                            
                           </div>
                           <asp:TextBox CssClass="form-control form-control-sm" ID="txtcodigo" runat="server" placeholder="Codigo de Cliente" Enabled="true"/>
                       </div>
                   </div> 
                   <div class="col-sm-12 col-lg-3 col-md-6">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label  runat="server" class="input-group-text" style="width: 110px" >Identidad</label>
                           </div>
                           <asp:TextBox TextMode="SingleLine" CssClass="form-control form-control-sm" placeholder="Buscar Por Identidad" ID="txtidentidad" runat="server" Enabled="true" />
                       </div>
                   </div>
                  </div>
                 <div class="row">
                   <div class="col-sm-12 col-lg-3 col-md-6" >
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dvale" class="input-group-text" style="width: 110px">Nombre</label>
                              <%-- <asp:DropDownList ID="dlNumeracion" runat="server" CssClass="input-group-text" ToolTip="Serie de Empresa..." OnSelectedIndexChanged="dlNumeracion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                           </div>
                           <asp:TextBox CssClass="form-control form-control-sm" ID="TxtCliente1" runat="server" Enabled="true" placeholder="Buscar por Nombre   ..."  />
                       </div>
                   </div>
                     <div class="col-sm-12 col-lg-3 col-md-6">
                       <div class="input-group input-group-sm">
                           
                           <asp:TextBox CssClass="form-control form-control-sm" ID="TxtCliente2" runat="server" Enabled="true" placeholder=" Buscar Por Nombre   ..." />
                       </div>
                   </div>
                 </div> &nbsp
             <div class="row align-content-center">
                   <div class="col col-8 align-content-center">
                       <div  class="input-group input-group-lg alert-primary">
                       <label>Filtrar tambien Por Persona Ihumada</label>
                       </div>
                   </div>
               </div>
             <div class="row">
                   <div class="col-sm-12 col-lg-3 col-md-6" >
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dvale" class="input-group-text" style="width: 110px">Difunto</label>
                              <%-- <asp:DropDownList ID="dlNumeracion" runat="server" CssClass="input-group-text" ToolTip="Serie de Empresa..." OnSelectedIndexChanged="dlNumeracion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                           </div>
                           <asp:TextBox CssClass="form-control form-control-sm" ID="txtBenef1" runat="server" Enabled="true" placeholder="Nombre Difunto   ..."  />
                       </div>
                   </div>
                     <div class="col-sm-12 col-lg-3 col-md-6">
                       <div class="input-group input-group-sm">
                           
                           <asp:TextBox CssClass="form-control form-control-sm" ID="txtbenef2" runat="server" Enabled="true" placeholder=" Nombre 2   ..." />
                       </div>
                   </div>
                 </div>

              <div class="row">
                   <div class="col-sm-12 col-lg-3 col-md-6" >
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dvale" class="input-group-text" style="width: 110px">Fecha desde</label>
                              <%-- <asp:DropDownList ID="dlNumeracion" runat="server" CssClass="input-group-text" ToolTip="Serie de Empresa..." OnSelectedIndexChanged="dlNumeracion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                           </div>
                           <asp:TextBox TextMode="Date" CssClass="form-control form-control-sm" ID="txtFecha1" runat="server" Enabled="true"  />
                       </div>
                   </div>
                  </div>
                <div class="row">
                   <div class="col-sm-12 col-lg-3 col-md-6" >
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dvale" class="input-group-text" style="width: 110px">Fecha Hasta</label>
                              <%-- <asp:DropDownList ID="dlNumeracion" runat="server" CssClass="input-group-text" ToolTip="Serie de Empresa..." OnSelectedIndexChanged="dlNumeracion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                           </div>
                           <asp:TextBox  TextMode="Date" CssClass="form-control form-control-sm" ID="txtfecha2" runat="server" Enabled="true" />
                       </div>
                   </div>
                  </div>
              <div class="row">
                   <div class="col-sm-12 col-lg-3 col-md-6" >
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dvale" class="input-group-text" style="width: 110px">Jardin</label>
                              <asp:DropDownList ID="dlJardin" runat="server" CssClass="input-group-text" ToolTip="Jardin" ></asp:DropDownList>
                           </div>
                           
                       </div>
                   </div>
                  </div>

            <br/>
               <div class="row">
                   <div class="col-3 col-sm-12 col-lg-3">
                   <div class="input-group input-group-sm">
                       <asp:TableCell runat="server" ColumnSpan="2"><asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-dark btn-sm"/></asp:TableCell>                   
                    </div>
                   </div>

               </div>
 
            <asp:GridView ID="gvClientes" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical"><AlternatingRowStyle BackColor="#CCCCCC" />
                <Columns><asp:ButtonField Text="Detalle" CommandName="Detalle" />
                </Columns><FooterStyle BackColor="#CCCCCC" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" /><PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" /><SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" /><SortedAscendingCellStyle BackColor="#F1F1F1" /><SortedAscendingHeaderStyle BackColor="#808080" /><SortedDescendingCellStyle BackColor="#CAC9C9" /><SortedDescendingHeaderStyle BackColor="#383838" /></asp:GridView>
            <br /><br /><asp:ImageButton ID="btnSalir" runat="server" Height="30px" ImageUrl="~/imagenes/atras.png" Width="30px" style="position:fixed; right:10px; top:10px;"/>
        <asp:Label ID="lblMsg" runat="server"></asp:Label>

      
        
        <div style="width:100%; height:30px;  position:fixed; bottom:0px; background-color:#202020; left: 0px; color: #fff">
            <asp:Label ID="lblTotal" runat="server" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblClientes" runat="server" Text="" />&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblConVisitas" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblSinVisitas" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblRendimientoVisitas" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="LblConCobro" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblSinCobro" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblRendimientoCobro" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblHora" runat="server" Text="" ForeColor="White"/>
    </div>
        
            </div>
    <%--    <div class="container-fluid" style="margin-top:5px;">   
            
            <asp:Table ID="Table1" runat="server" Style="margin-bottom:20px;">
                      <asp:TableRow runat="server">                    
                    <asp:TableCell runat="server">Desde:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox TextMode="Date" ID="fecha1Inh" runat="server" CssClass="form-control form-control-sm" Width="200" Height="24" Font-Size="Small"/></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">                    
                    <asp:TableCell runat="server">Hasta</asp:TableCell><asp:TableCell runat="server"><asp:TextBox TextMode="Date" ID="Fecha2Inh" runat="server"  CssClass="form-control form-control-sm" Width="200" Height="24" Font-Size="Small"/><asp:TableCell runat="server">&nbsp &nbsp</asp:TableCell><asp:TableCell runat="server" ColumnSpan="2"><asp:Button ID="BtnabrirPDF" runat="server" Text="DescargarPDF" Visible="false" CssClass="btn btn-danger  btn-sm"/></asp:TableCell>                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">                    
                    <asp:TableCell runat="server">Tomado Por:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="txtCliente" runat="server" placeholder="Buscar por" CssClass="form-control form-control-sm" Width="200" Height="24" Font-Size="Small"/></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">                    
                    <asp:TableCell runat="server">&nbsp</asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="TxtCliente2" runat="server" placeholder="Tambien por" CssClass="form-control form-control-sm" Width="200" Height="24" Font-Size="Small"/></asp:TableCell>
                </asp:TableRow>
           
                <asp:TableRow runat="server">                    
                    <asp:TableCell runat="server" ColumnSpan="2">&nbsp</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">                    
                    <asp:TableCell runat="server" ColumnSpan="2"><asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-dark btn-sm"/></asp:TableCell>                   
                </asp:TableRow>
            </asp:Table>
            <asp:GridView ID="gvClientes" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
                ForeColor="Black" GridLines="Vertical"><AlternatingRowStyle
                BackColor="#CCCCCC" />
                <Columns>
                <asp:ButtonField Text="Detalle"  CommandName="Detalle"  runat="server"/></Columns>
                <FooterStyle BackColor="#CCCCCC" /><HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#808080" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#383838" />
            </asp:GridView>
            <br /><br /><asp:ImageButton ID="btnSalir" runat="server" Height="30px" ImageUrl="~/imagenes/atras.png" Width="30px" style="position:fixed; right:10px; top:10px;"/>
        <asp:Label ID="lblMsg" runat="server"></asp:Label></div>--%>
 <%--       <div style="width:100%; height:30px;  position:fixed; bottom:0px; background-color:#202020; left: 0px; color: #fff">
            <asp:Label ID="lblTotal" runat="server" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblClientes" runat="server" Text="" />&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblConVisitas" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblSinVisitas" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblRendimientoVisitas" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="LblConCobro" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblSinCobro" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblRendimientoCobro" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblHora" runat="server" Text="" ForeColor="White"/>
    </div>--%>
        <script src="js/JQuery.js"></script>
        <script src="js/popper.min.js"></script>
        <script src="js/bootstrap.min.js"></script>
        <script src="js/Jquery1.8.3.js"></script>
        <script type="text/javascript">
            $("[src*=plus]").live("click", function () {
                $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
                $(this).attr("src", "imagenes/minus.png");
            });
            $("[src*=minus]").live("click", function () {
                $(this).attr("src", "imagenes/plus.png");
                $(this).closest("tr").next().remove();
            });
        </script>
    </form>
</body>
</html>