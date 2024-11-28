<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UbicacionLotes.aspx.vb" Inherits="Sistema.UbicacionLotes" %>

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


        <div class="container-fluid" style="margin-top:5px;"> 

            <div><h4 style="width:100%; text-align:center">Ubicacion de Lote de Clientes</h4></div>
            <br />
    <%--        <asp:Table ID="Table1" runat="server" Style="margin-bottom:20px;">
                <asp:TableRow runat="server">                    
                    <asp:TableCell runat="server">Cliente:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="txtCliente" runat="server" placeholder="Buscar por" CssClass="form-control form-control-sm" Width="200" Height="24" Font-Size="Small"/></asp:TableCell></asp:TableRow><asp:TableRow runat="server">                    
                    <asp:TableCell runat="server">&nbsp</asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="TxtCliente2" runat="server" placeholder="Tambien por" CssClass="form-control form-control-sm" Width="200" Height="24" Font-Size="Small"/></asp:TableCell></asp:TableRow><asp:TableRow runat="server">                    
                    <asp:TableCell runat="server" ColumnSpan="2">&nbsp</asp:TableCell></asp:TableRow><asp:TableRow runat="server">                    
                    <asp:TableCell runat="server" ColumnSpan="2"><asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-dark btn-sm"/></asp:TableCell>                   
                </asp:TableRow>
            </asp:Table>--%>
            
                                 <div class="row">
                        <div class="col-sm-12 col-lg-3 col-md-6">
                           <div class="input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <label for="dvale" class="input-group-text" style="width: 110px">Codigo Cliente</label>                            
                               </div>
                               <asp:TextBox CssClass="form-control form-control-sm" ID="txtCliente" runat="server" placeholder="Codigo de Cliente" Enabled="true"/>
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
                               <asp:TextBox CssClass="form-control form-control-sm" ID="TxtCliente2" runat="server" Enabled="true" placeholder="Buscar por Nombre   ..."  />
                           </div>
                       </div>
                         <div class="col-sm-12 col-lg-3 col-md-6">
                           <div class="input-group input-group-sm">
                           
                               <asp:TextBox CssClass="form-control form-control-sm" ID="TxtCliente3" runat="server" Enabled="true" placeholder=" Buscar Por Nombre   ..." />
                           </div>
                       </div>
                     </div>
            <br />
             <div class="row">
                   <div class="col-3 col-sm-12 col-lg-3">
                   <div class="input-group input-group-sm">
                       <asp:TableCell runat="server" ColumnSpan="2"><asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-dark btn-sm"/></asp:TableCell>                   
                    </div>
                   </div>

               </div>

             <%--<asp:linkbutton data-toggle="modal"  class="btn btn-outline-primary" id="btnhacercambio"  runat="server" >Realizar Cambio</asp:linkbutton>--%>
            <br />
            <asp:GridView ID="gvClientes" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical"><AlternatingRowStyle BackColor="#CCCCCC" />
                <Columns>
                    <asp:Buttonfield  Text="Detalle" CommandName="Detalle" />

                </Columns>
                <FooterStyle BackColor="#CCCCCC" /><HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" /><SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" /><SortedAscendingHeaderStyle BackColor="#808080" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" /><SortedDescendingHeaderStyle BackColor="#383838" />

            </asp:GridView>
            <asp:SqlDataSource ID="SqlJardines" runat="server" ConnectionString="<%$ ConnectionStrings:FunamorConexion %>" SelectCommand="SELECT JARD_NOMBRE, JARD_CODIGO FROM JARDINES"></asp:SqlDataSource>
          
            
  <br /><br /><asp:ImageButton ID="btnSalir" runat="server" Height="30px" ImageUrl="~/imagenes/atras.png" Width="30px" style="position:fixed; right:10px; top:10px;"/>
        <asp:Label ID="lblMsg" runat="server"></asp:Label></><div style="width:100%; height:30px;  position:fixed; bottom:0px; background-color:#202020; left: 0px; color: #fff">
            <asp:Label ID="lblTotal" runat="server" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblClientes" runat="server" Text="" />&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblConVisitas" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblSinVisitas" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblRendimientoVisitas" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="LblConCobro" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblSinCobro" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblRendimientoCobro" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblHora" runat="server" Text="" ForeColor="White"/>
    </div>
                                                                 
                                          
                                               
         </div>                                 

                      
          <div class="container-fluid" style="background-color: rgba(0,0,0,0.6); position:absolute; left: 0; top: 0; bottom: 0" id="PanelUbicacion" runat="server" visible="false">
                            
                 <div style="padding-left: 30%; padding-right: 30%; padding-top: 55px;">
                   
                   <div class="row" style="background-color: white; padding-top: 15px;">
                       <div class="col">
                           <div class="input-group-prepend align-content-center " style="text-align: left"">
                                <h4 class="alert-dark">Cambio de Ubicacion de Terreno</h4>
                           <%--<label class="alert-primary align-content-center">Está seguro Que desea Grabar Esta Modificacion?</label>--%>
                            </div>
                       </div>
  

                    </div>
                             <div class="row" style="background-color: white; padding-top: 15px;">
                       <div class="col-8">
                           <div class="input-group input-group-sm">                         
                          
                        </div>
                       </div>
                     
                   </div>
                  <div class="row" style="background-color: white">
                    <div class="col-3">
                        <div class="input-group input-group-sm">
                      
                            <asp:TextBox TextMode="SingleLine" CssClass="form-control form-control-sm" placeholder="Codigo" ID="txtNccodigo" OnTextChanged="txtNccodigoBuscar_TextChanged" AutoPostBack="true" runat="server" />
                       
                        </div>
                    </div>
                    <div class="col">
                        <div class="input-group input-group-sm">

                            <asp:TextBox TextMode="SingleLine" CssClass="form-control form-control-sm" placeholder="Nombre Cliente" enabled="false"   ID="txtNombre" runat="server" />
                        </div>
                    </div>
              
                </div>
                     <div class="row" style="background-color: white; padding-top: 15px;">
                       <div class="col-3">

                           <div class="input-group input-group-sm">
                                <div class="input-group-prepend">
                            <label class="input-group-text">Num. Ubicacion</label>
                           </div>
                               </div>
                 
                        
                       </div>
                         <div class="col-3">
                              <div class="input-group input-group-sm">
                                <div class="input-group-prepend">
                            <label class="input-group-text" >Jardin</label>
                           </div>
                               </div>
                         </div>
                     </div>
                         <div class="row" style="background-color: white; padding-top: 15px;">
                                   <div class="col-3">
                        <div class="input-group input-group-sm">
                      
                            <asp:TextBox TextMode="Number" CssClass="form-control form-control-sm" placeholder="Numero" id="txtubicacion" runat="server" />
                       
                        </div>
                    </div>
                             <div class="col-4">
                                 <div class="input input-group-prepend">
                                 <asp:DropDownList ID="DropJardin" runat="server" DataSourceID="SqlJardines" DataTextField="JARD_NOMBRE" DataValueField="JARD_CODIGO"></asp:DropDownList>
                             </div></div>
                         </div>
                   
                    <div class="row" style="background-color: white; padding-top: 15px;">
                        <div class="col">
                         
                               <div class="input-group input-group-sm">                                          
                           
                        </div>
                        </div>
                        </div>
                   <div class="row" style="background-color: white; padding-top: 15px;">
                       <div class="col-8">
                           <div class="col" style="text-align: right">
                               <asp:Button ID="btnsalvarUb" runat="server" Text="   Salvar   " CssClass="btn btn-sm btn-success" />&nbsp&nbsp<asp:Button ID="idcancelarub" runat="server" Text="Cancelar" CssClass="btn btn-sm btn-secondary" />
                           </div>
                       </div>
                     
                   </div>
               <div class="row" style="background-color: white; padding-top: 15px">
                    <div class="col">
                    <asp:Label ID="lblMsjError" runat="server"></asp:Label>
                    </div>
                </div>
               </div>   
                  <div id="alertPlaceholder" runat="server"></div>

           </div>


        <script src="js/JQuery.js"></script><script src="js/popper.min.js"></script><script src="js/bootstrap.min.js"></script><script src="js/Jquery1.8.3.js"></script><script type="text/javascript">
            $("[src*=plus]").live("click", function () {
                $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
                $(this).attr("src", "imagenes/minus.png");
            });
            $("[src*=minus]").live("click", function () {
                $(this).attr("src", "imagenes/plus.png");
                $(this).closest("tr").next().remove();
            });
        </script></form></body></html>