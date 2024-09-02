<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="monitorventas1.aspx.vb" Inherits="Sistema.monitorventas1" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cuentas por cobrar</title>
 <!-- Bootstrap core CSS -->  
  <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
    <link rel="shortcut icon" type="image/x-icon" href="svgs/solid/drafting-compass.svg"/>

  <!-- Custom styles for this template -->
  <link href="css/simple-sidebar.css" rel="stylesheet"/>
  <link rel="stylesheet" href="css/all.min.css" />
    <style>
        .row {padding:1px;
        }  
        .Boton:hover{transform:scale(1.05);
        }

        .nav-toggle {
    color: green;
    background: none;
    border: none;
    font-size: 30px;
    padding: 0 20px;
    line-height: 60px;
    cursor: pointer;
    display: none;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">


    <!-- Page Content -->
      


       <header class="card-header">
            <button  aria-label="Atras" onclick="FNAtras()">
            <i class="fas fa-angle-left"></i> 
                </button>

           <a href="#"  class="logo nav-link" style="text-align:center; width:100%; height: 80px; position: fixed;    top: 0;   left: 0;font-size: 30px">Monitor De Ventas</a><label style="align-items:self-start" id="lblTitulo" runat="server"></label>
       </header>
<%--           <div class="navbar navbar-expand-lg navbar-light bg-light border-bottom">              
               <h4 style="width:100%; text-align:center">Cuentas por cobrar</h4>
               <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                   <span class="navbar-toggler-icon"></span>
               </button>

               <div class="collapse navbar-collapse" id="navbarSupportedContent">
                   <ul class="navbar-nav ml-auto mt-2 mt-lg-0">
                       <li class="nav-item active">
                           <a class="btn btn-light" onclick="window.open('principal.aspx','_self')"><i class="fas fa-home" style="font-size: x-large; color: darkslategray"></i></a>
                       </li>
                   </ul>
               </div>
           </div>        --%>   


            <div class="container-fluid" style="padding-top:5px;"> 
             <div class="col">
                 <div class="row">
                   <div class="col-sm-12 col-lg-4 col-md-6">
                     
                 <div class="row">
                      <div class="col">
                     <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dlModo" class="input-group-text" style="width: 75px">Modo</label>
                           </div>
                           <asp:DropDownList ID="DropMode" runat="server" CssClass="form-control form-control-sm" OnTextChanged="DropMode_TextChanged" AutoPostBack="true"></asp:DropDownList>
                       </div>
                   </div>
                  </div>
                    <div class="row" id="RowFecha1" runat="server">
                      <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="txtFechalb" class="input-group-text" style="width: 75px">Fecha</label>
                           </div>
                           <asp:TextBox TextMode="Date" class="form-control form-control-sm" id="txtFecha" runat="server"></asp:TextBox>
                       </div>
                   </div>
                  </div>
                 <div class="row" id="RowFecha2" runat="server" visible="false">
                      <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="txtFechalb" class="input-group-text" style="width: 75px">Fecha 2</label>
                           </div>
                           <asp:TextBox TextMode="Date" class="form-control form-control-sm" id="Fecha2" runat="server" Visible="true"></asp:TextBox>
                       </div>
                   </div>
                  </div>

                <div class="row">
                 <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dlAlmacen" class="input-group-text" style="width: 75px">Mostrar</label>
                           </div>
                           <asp:DropDownList ID="dlMostrar" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                       </div>
                   </div>  
                </div>
                 <div class="row">
                 <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dlAlmacen" class="input-group-text" style="width: 75px">Lider</label>
                           </div>
                           <asp:DropDownList ID="dllider" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                       </div>
                   </div>  
                </div>
                 <div class="row">
                 <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dlAlmacen" class="input-group-text" style="width: 75px">Zona</label>
                           </div>
                           <asp:DropDownList ID="dlzona" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                       </div>
                   </div>  
                </div>
                <div class="row">
                 <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dlRun" class="input-group-text" style="width: 75px">Run</label>
                           </div>
                           <asp:DropDownList ID="dlRun" runat="server" CssClass="form-control form-control-sm">
                               
                           </asp:DropDownList>
                       </div>
                   </div>  
                </div>
                   <div class="row" id="rowclienteIdend" runat="server" visible="false">
                        <div class="col">
                           <div class="input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <label for="dvale" class="input-group-text" style="width: 110px">Codigo Cliente</label>                            
                               </div>
                               <asp:TextBox CssClass="form-control form-control-sm" ID="txtCodClienteApp" runat="server" placeholder="Codigo de Cliente App" Enabled="true"/>
                           </div>
                       </div> 
                       <div class="col" id="DivClienteIden">
                           <div class="input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <label  runat="server" class="input-group-text" style="width: 110px" >Identidad</label>
                               </div>
                               <asp:TextBox TextMode="SingleLine" CssClass="form-control form-control-sm" placeholder="Buscar Por Identidad" ID="txtidentidadClienteApp" runat="server" Enabled="true" />
                           </div>
                       </div>
                      </div>
                <div class="row" id="rowClienteNombre" runat="server" visible="false">
                       <div class="col">
                           <div class="input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <label for="dvale" class="input-group-text" style="width: 110px">Nombre</label>
                                 
                               </div>
                               <asp:TextBox CssClass="form-control form-control-sm" ID="TxtCliente1" runat="server" Enabled="true" placeholder="Buscar por Nombre   ..."  />
                           </div>
                       </div>
                         <div class="col-columns">
                           <div class="input-group input-group-sm">
                           
                               <asp:TextBox CssClass="form-control form-control-sm" ID="TxtCliente2" runat="server" Enabled="true" placeholder=" Buscar Por Nombre   ..." />
                           </div>
                       </div>
                     </div>
                       <br/>
                  <div class="row">

                      <div class="col">
                       <input type="text" class="form-control form-control-sm" placeholder="Codigo Vendedor" id="txtCobrador" runat="server" />
                         
                    </div>
                       <asp:Button ID="btnBuscarv" runat="server" Text="   Buscar   " CssClass="btn btn-sm btn-primary" />&nbsp&nbsp
                       
                     
                   </div>
              
               </div>
                
            <div class="col">
                
                        <div class="card shadow-sm" id="RowGrafico" runat="server" visible="false">
                            <div class="card-header">
                                <h5>Top Lideres</h5>
                            </div>
                            <div class="card-body">
                                <asp:Chart runat="server" ID="GraTopVen" BackColor="WhiteSmoke">
                                    <Series>
                                        <asp:Series Name="Series1" ChartArea="ChartArea1" IsValueShownAsLabel="True" ChartType="Bar" />
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1">
                                            <AxisX>
                                                <MajorGrid Enabled="False" />
                                            </AxisX>
                                        </asp:ChartArea>
                                    </ChartAreas>                          
                                </asp:Chart>                      
                            </div>
                        </div>


            </div>
           </div>
                 </div>
           </div>
               

                 <%--  <div class="row" style="background-color: white; padding-top: 15px;">
                       <div class="col-8">
                           <div class="input-group input-group-sm">
                               <asp:TextBox ID="txtBuscar" runat="server" class="form-control form-control-sm" placeholder="Cliente..." OnTextChanged="txtBuscar_TextChanged" AutoPostBack="true" />
                               <asp:DropDownList ID="dlVendedor" runat="server" CssClass="form-control form-control-sm" OnSelectedIndexChanged="txtBuscar_TextChanged" AutoPostBack="true"></asp:DropDownList>                               
                               <div class="input-group-append">
                                   <label class="input-group-text">
                                       <asp:LinkButton ID="btnBuscar" runat="server" class="fas fa-search text-secondary"></asp:LinkButton></label>
                               </div>
                           </div>
                       </div>                       
                       <div class="col">
                           <div style="padding-top:4px; text-align:right;">
                               <a id="btnExcel" runat="server" style="font-size:x-large" class="fas fa-file-download Boton text-dark"></a>&nbsp                            
                           </div>
                       </div> 
                   
                   </div>--%>
                   <div class="row" style="background-color: white; padding-top:5px">
                       <div class="col">
                           <asp:GridView ID="GvPrincipal" runat="server" RowStyle-HorizontalAlign="Right" CssClass="table table-sm table-hover table-bordered" EmptyDataText="No se econtraron resultados" AllowSorting="true" AutoGenerateColumns="False">                             
                               <Columns>
                                    <asp:ButtonField CommandName="Liquidar" Text="Liquidar" Visible="False"/>
                        <asp:ButtonField Text="Imprimir" CommandName ="Imprimir" Visible="false"/>
                        <asp:ButtonField Text="Mapa" CommandName ="Mapa" Visible="true"/>
                                   <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                       <ItemTemplate>
                                           <img alt="" src="svgs/solid/angle-down.svg" width="15" style="padding: 0px;" />
                                           <asp:Panel ID="pnlOrders" runat="server" Style="display: none; padding: 0px;">
                                               <asp:GridView ID="GVDetalle" runat="server" RowStyle-HorizontalAlign="Right" AutoGenerateColumns="false" CssClass="table-bordered table-info">
                                                   <Columns>
                                                       <asp:BoundField DataField="Codigo" HeaderText="Doc" />
                                                       <asp:BoundField DataField="Cobrado" HeaderText="Cobrado" />
                                                       <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                                       <asp:BoundField DataField="Hora" HeaderText="Hora" />
                                                       <asp:BoundField DataField="Codigo Cliente" HeaderText="Cod.Cliente" />
                                                       <asp:BoundField DataField="Nombre_clie" HeaderText="NombreCliente" />
                                                       <asp:BoundField DataField="identidad" HeaderText="Identidad" />
                                                       <asp:BoundField DataField="RCodvend" HeaderText="Id" />
                                                       <asp:BoundField DataField="cont_valcuo" HeaderText="Cuota" />
                                                       <asp:BoundField DataField="Cont_numcuo" HeaderText="N.Cuota" />
                                                       <asp:BoundField DataField="servi1des" HeaderText="Producto" />
                                                       <asp:BoundField DataField="Motivo" HeaderText="Motivo" />
                                                       <asp:BoundField DataField="dir_cliente" HeaderText="Cuota" />
                                                       <asp:BoundField DataField="Telefono" HeaderText="N.Cuota" />
                                                       <asp:BoundField DataField="Liquida" HeaderText="Liquida" />
                                                       <asp:BoundField DataField="Liquida2" HeaderText="Procesado" />                                                     
                                                       <asp:BoundField DataField="ClientesSistema" HeaderText="ClienteSistema" />                                                       
                                                   </Columns>
                                               </asp:GridView>
                                           </asp:Panel>
                                       </ItemTemplate>   

                                   </asp:TemplateField>
                                   <asp:BoundField DataField="Codigo" HeaderText="Codigo" SortExpression="Codigo" ItemStyle-HorizontalAlign="Left" />
                                   <asp:BoundField DataField="Vendedor" HeaderText="Vendedor" SortExpression="Vendedor" ItemStyle-HorizontalAlign="Left" />
                                   <asp:BoundField DataField="Cobrado" HeaderText="Cobrado" SortExpression="Cobrado" />
                                   <asp:BoundField DataField="Primer Visita" HeaderText="Primer Visita" SortExpression="Primer Visita" />
                                   <asp:BoundField DataField="Ultima Visita" HeaderText="Ultima Visita" SortExpression="Ultima Visita" />
                                   <asp:BoundField DataField="Recibos" HeaderText="Recibos" SortExpression="Recibos" />
                                   <asp:BoundField DataField="Visitados" HeaderText="Visitados" SortExpression="Visitados" />
                                   <asp:BoundField DataField="Ventas" HeaderText="Ventas" SortExpression="Ventas" />
                                   <asp:BoundField DataField="Verdes" HeaderText="Verdes" SortExpression="Verdes" />
                                   <asp:BoundField DataField="Lider" HeaderText="Lider" SortExpression="Lider" />
                                   <asp:BoundField DataField="Liquida" HeaderText="Liquida" SortExpression="Liquida" />
                                   <asp:BoundField DataField="Procesado" HeaderText="Procesado" SortExpression="Procesado" />
                                   <asp:BoundField DataField="Zona" HeaderText="Zona" SortExpression="Zona" />
                                   <asp:BoundField DataField="Telefono" HeaderText="Telefono" SortExpression="Telefono" />

                                  


                                       <asp:ButtonField CommandName="imprimir" Text="Ver" ControlStyle-ForeColor="#3333ff">
                                        <ControlStyle ForeColor="#0000ff" CssClass="label label-info"></ControlStyle>
                                </asp:ButtonField>
                               </Columns>
                           </asp:GridView>
                       </div>
                   </div>
         <%--      </div>
           </div>--%>

           <div style="visibility: hidden">
            <asp:GridView ID="gvExcel" runat="server"></asp:GridView>
           </div>

           <div style="position:fixed; bottom:20px; padding-left:15px;" >               
               <asp:Label ID="lblMsg" runat="server"></asp:Label>
           </div>
<div style="width:100%; height:30px;  position:fixed; bottom:0px; background-color:#202020; left: 0px; color: #fff">
            <asp:Label ID="lblTotal" runat="server" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblRecibos" runat="server" Text="" />&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblVisitados" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblVentas" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblVerdes" Visible ="false" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblCobradores" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblHora" runat="server" Text="" ForeColor="White"/>
            </div>

            <br />
            <br />
            <br />
            <br />
            <br />


    <!-- /#page-content-wrapper -->

   <!-- /#wrapper -->

    <!-- Bootstrap core JavaScript -->
    <script src="vendor/jquery/jquery.min.js"></script>
    <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

    <!-- Menu Toggle Script -->
    <script src="js/Jquery1.8.3.js"></script>
    <script type="text/javascript">
        $("[src*=angle-down]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "svgs/solid/angle-up.svg");
        });
        $("[src*=angle-up]").live("click", function () {
            $(this).attr("src", "svgs/solid/angle-down.svg");
            $(this).closest("tr").next().remove();
        });
        //$("[src*=plus]").live("click", function () {
        //    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
        //    $(this).attr("src", "imagenes/minus.png");
        //});
        //$("[src*=minus]").live("click", function () {
        //    $(this).attr("src", "imagenes/plus.png");
        //    $(this).closest("tr").next().remove();
        //});
    </script>
    <script>
        $("#menu-toggle").click(function (e) {
            e.preventDefault();
            $("#wrapper").toggleClass("toggled");
        });
    </script>
    </form>  
</body>
</html>
