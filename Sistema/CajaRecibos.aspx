<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CajaRecibos.aspx.vb" Inherits="Sistema.CajaRecibos" %>


<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pago Recibido cliente</title>
 <!-- Bootstrap core CSS -->  
  <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
    <link rel="shortcut icon" type="image/x-icon" href="svgs/solid/drafting-compass.svg"/>

  <!-- Custom styles for this template -->
  <link href="css/simple-sidebar.css" rel="stylesheet"/>
  <link rel="stylesheet" href="css/all.min.css" />
    <script src="https://kit.fontawesome.com/9189b1e7bc.js" ></script>
    <style>
        .row {padding:1px;
        }       
    </style>
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnAgregar">

   <div class="d-flex" id="wrapper">

    <!-- Sidebar -->
    <div class="bg-light border-right" id="sidebar-wrapper">
      <div class="sidebar-heading text-primary" style="font-weight:bolder">Caja</div>
      <div class="list-group list-group-flush"> 
        <a runat="server" id="barMedio" class="list-group-item list-group-item-action bg-light"><i class="fas fa-credit-card" style="font-size:large; text-align:center; cursor:pointer"></i>&nbsp Pago&nbsp<label id="lblPagado" class="badge badge-secondary" runat="server">0.00</label></a>        
        <a runat="server" id="barNuevo" class="list-group-item list-group-item-action bg-light"><i class="far fa-file-alt" style="font-size:large; text-align:center"></i>&nbsp Nuevo</a>
        <a runat="server" id="barBuscar" class="list-group-item list-group-item-action bg-light"><i class="fas fa-search" style="font-size:large; text-align:center"></i>&nbsp Buscar</a>
        <a runat="server" id="barCancelar" class="list-group-item list-group-item-action bg-light" visible="false"><i class="far fa-trash-alt" style="font-size:large; text-align:center"></i>&nbsp Cancelar</a>
        <a runat="server" id="barReimpresion" class="list-group-item list-group-item-action bg-light" visible="false"><i class="fas fa-print" style="font-size:large; text-align:center"></i>&nbsp Imprimir</a>        
        <%--<a runat="server" id="barCopiar" class="list-group-item list-group-item-action bg-light"><i class="far fa-copy" style="font-size:large; text-align:center"></i>&nbsp Copiar de</a>--%>
        <p class="list-group-item list-group-item-action bg-light"><i class="far fa-comment-alt" style="font-size:large; text-align:center"></i>&nbsp Comentarios<asp:TextBox ID="txtComentarios" TextMode="MultiLine" onkeypress="if (this.value.length > 200) { return false; }" runat="server" CssClass="form-control form-control-sm"></asp:TextBox></p>        
      </div>
    </div>
    <!-- /#sidebar-wrapper -->

    <!-- Page Content -->
       <div id="page-content-wrapper">

           <nav class="navbar navbar-expand-lg navbar-light bg-light border-bottom">
               <button class="btn btn-light" id="menu-toggle"><i class="fas fa-bars" style="font-size: x-large; color: darkslategray"></i></button>
               <h4 style="width:100%; text-align:center">Pago Recibido <asp:Label ID="lblEstado" runat="server" Text="(Abierto)" style="font-size:medium"></asp:Label></h4>
               <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                   <span class="navbar-toggler-icon"></span>
               </button>

               <div class="collapse navbar-collapse" id="navbarSupportedContent">
                   <ul class="navbar-nav ml-auto mt-2 mt-lg-0">
                       <li class="nav-item active">
                           <a class="btn btn-light" onclick="window.open('Principal.aspx','_self')"><i class="fas fa-home" style="font-size: x-large; color: darkslategray"></i></a>
                       </li>
                   </ul>
               </div>
           </nav>

           <div class="container-fluid" style="padding-top: 5px;" id="PanelFactura" runat="server">
               <div class="row">
                   <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <asp:Label runat="server" class="input-group-text">Recibo de </asp:Label>
                               <%-- OnSelectedIndexChanged="dlNumeracion_SelectedIndexChanged" AutoPostBack="true" --%>
                           </div>
                               <asp:DropDownList ID="dlRecibo" runat="server" CssClass="input-group-text" ToolTip="" >
                                   <asp:ListItem Selected="True" Value="Cliente">Cliente</asp:ListItem>
                                   <%--<asp:ListItem Value="Cobrador">Cobrador</asp:ListItem>--%>
                               </asp:DropDownList>
                           <%--<asp:TextBox TextMode="number" CssClass="form-control form-control-sm" ID="txtNumeracion" runat="server" ToolTip="Tipo de Recibo de Caja..." />--%>
                       </div>
                   </div>
                   <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dlAlmacen" class="input-group-text" style="width: 95px">Empresa</label>
                           </div>
                           <asp:DropDownList ID="dlEmpresa" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                       </div>
                   </div>                   
                   <div class="col" style="text-align: right">
                       <asp:Button ID="btnCrear" runat="server" Text="    Crear    " CssClass="btn btn-sm btn-primary" />
                   </div>

               </div>
               <div class="row">
                   <div class="col-4">
                       <div class="input-group input-group-sm">
                           <%-- OnTextChanged="TxtCodigoCliente_TextChanged" AutoPostBack="true" --%>
                           <asp:TextBox TextMode="SingleLine" CssClass="form-control form-control-sm" placeholder="Codigo" ID="txtCodigo" MaxLength="30" runat="server"  />
                           <div class="input-group-append">
                               <label class="input-group-text input-group-"><asp:LinkButton ID="btnBuscarCliente" runat="server" CssClass="fas fa-search text-secondary"></asp:LinkButton></label>
                           </div>                           
                       </div>                       
                   </div>
                     <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="txtVencimiento" class="input-group-text" style="width: 95px">Documento</label>
                           </div>
                           <label class="form-control form-control-sm" id="Label3" runat="server"></label>
                       </div>
                   </div>
              
                    <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="txtVencimiento" class="input-group-text" style="width: 95px">Fecha</label>
                           </div>
                           <input type="date" class="form-control form-control-sm" id="txtfecha" runat="server" />
                       </div>
                   </div>
               </div>
               <div class="row">
                   <div class="col-5">
                       <input type="text" class="form-control form-control-sm" placeholder="Nombre" id="txtNombre" maxlength="50" runat="server" />
                   </div>
                 
                        <div class="col-6">
                       <div class="input-group input-group-sm">
                          <div class="input-group-prepend">
                               <label for="txtdireccion" class="input-group-text" style="width: 95px">Direccion</label>
                           </div>
                           <label class="form-control form-control-sm" id="Label1" runat="server"></label>
                       </div>
                   </div>
                   <%--<div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label class="input-group-text" style="width: 110px"></label>
                           </div>
                           <label class="form-control form-control-sm" id="lblLista" runat="server"></label>
                       </div>
                   </div>  --%>     
               </div>
               <br />
               <div class="row">
                    <div class="col" style="text-align:left">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label class="input-group-text" style="width: 110px">Valor de Cuota</label>
                           </div>
                           <input type="text" class="form-control form-control-sm" placeholder="Cuota" id="txtcuota" maxlength="30" runat="server" />
                       </div>
                   </div> 
               
                   <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dlVendedor" class="input-group-text" style="width: 95px">Vendedor</label>
                           </div>
                           <label class="form-control form-control-sm" id="lblNameVendedor" runat="server"></label>
                       </div>
                   </div>
                    <div class="col" style="text-align:left">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label class="input-group-text" style="width: 110px">Valor de Cuota</label>
                           </div>
                           <label class="form-control form-control-sm" id="lblFormaPago" runat="server"></label>
                       </div>
                   </div>                                      
               </div>
           
           
           <div class="row">
               <div class="col-2">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="codcobrador" class="input-group-text" style="width: 95px">Cobrador</label>
                           </div>
                           <input type="text" class="form-control form-control-sm" placeholder="Codigo" id="txtcodcobrador" maxlength="30" runat="server" />
                       </div>
                   </div>
               <div class="col-4">
                           <label class="form-control form-control-sm" id="lblnombreCobr" runat="server"></label>

               </div>
                  <div class="col-4">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dlconsepto" class="input-group-text" style="width: 95px">Concepto</label>
                           </div>
                           <asp:DropDownList ID="dlconsepto" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                       </div>
                   </div> 
           </div>
               <br />                 <div class="row align-content-center">
                   <div class="col col-8 align-content-center">
                       <div  class="input-group input-group-sm alert-dark small ">
                       <label>En caso de Ser Preparaciones/Transporte/placa</label>
                       </div>
                   </div>
               </div>
               <div class="row">
                    <div class="col-4">
                       <div class="input-group input-group-sm">
                           <%-- OnTextChanged="TxtCodigoCliente_TextChanged" AutoPostBack="true" --%>
                           <asp:TextBox TextMode="SingleLine" CssClass="form-control form-control-sm" placeholder="Codigo Contrato" ID="txtcoddif" MaxLength="30" runat="server"  />
                           <div class="input-group-append">
                               <label class="input-group-text input-group-">
                                   <asp:LinkButton ID="LinkButton1" runat="server" CssClass="fas fa-search text-secondary"></asp:LinkButton></label>
                           </div>                           
                       </div>                       
                   </div>
                   <div class="col-4">
                       <label class="form-control form-control-sm" id="Label2" runat="server"></label>
                   </div>

        <%--       <div class="row" style="padding-top: 15px">
                       <div class="col">
                           <div class="input-group input-group-sm">
                               
                               <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control form-control-sm" placeholder="Buscar producto..."  />
                               <div class="input-group-append">
                                   <label class="input-group-text">
                                       <asp:LinkButton ID="btnBuscarProductos2" runat="server" CssClass="fas fa-search text-secondary"></asp:LinkButton></label>
                               </div>
                           </div>
                       </div>
                       <div class="col"></div>
                   </div>--%>

               </div>
               <div class="row">
                    <div class="col-4">
                       <div class="input-group input-group-sm">
                           <%-- OnTextChanged="TxtCodigoCliente_TextChanged" AutoPostBack="true" --%>
                           <asp:TextBox TextMode="SingleLine" CssClass="form-control form-control-sm" placeholder="Difunto" ID="txtdifunto" MaxLength="30" runat="server"  />
                           <div class="input-group-append">
                              
                           </div>                           
                       </div>                       
                   </div>
               </div>

           </div>                 

           <div class="container-fluid" style="background:white; position: absolute; left: 0; top: 0; bottom: 0" id="PanelProductos" runat="server" visible="false">               
               <div style="padding-left: 15%; padding-right: 15%; padding-top: 40px;">
                   <div class="row" style="padding-top: 15px">
                       <div class="col">
                           <div class="input-group input-group-sm">
                               <%-- OnTextChanged="TxtProductos_TextChanged" AutoPostBack="true" --%>
                               <asp:TextBox ID="txtProductos" runat="server" CssClass="form-control form-control-sm" placeholder="Buscar producto..."  />
                               <div class="input-group-append">
                                   <label class="input-group-text">
                                       <asp:LinkButton ID="btnBuscarProductos" runat="server" CssClass="fas fa-search text-secondary"></asp:LinkButton></label>
                               </div>
                           </div>
                       </div>
                       <div class="col" style="text-align: right"><asp:LinkButton ID="btnCerrar" runat="server" Style="font-size: x-large"><i class="far fa-times-circle text-secondary"></i></asp:LinkButton></div>
                   </div>
                   <div class="row" style="padding-top: 15px">
                       <div class="col input-group input-group-sm">
                           <div class="input-group-prepend">
                               <span class="input-group-text">Cod</span>
                           </div>
                           <label id="txtCodProducto" runat="server" class="form-control form-control-sm"></label>
                       </div>
                       <div class="col input-group input-group-sm" style="padding-left: 0px">
                           <div class="input-group-prepend">
                               <span class="input-group-text">Cant</span>
                           </div>
                           <asp:TextBox ID="txtCantidad" runat="server" CssClass="form-control form-control-sm" TextMode="Number" />
                       </div>
                       <div class="col input-group input-group-sm" style="display: none">
                           <div class="input-group-prepend">
                               <span class="input-group-text">Lps</span>
                           </div>
                           <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control form-control-sm" TextMode="Number" />
                       </div>
                       <div class="col input-group input-group-sm" style="padding-left: 0px">
                           <div class="input-group-prepend">
                               <span class="input-group-text">Imp</span>
                           </div>
                           <asp:DropDownList ID="dlImpuesto" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                       </div>
                       <div class="col" style="display: none">
                           <asp:TextBox ID="txtNomProducto" runat="server" CssClass="form-control form-control-sm" />
                       </div>
                       <div class="col" style="display: none">
                           <asp:TextBox ID="txtCosto" runat="server" CssClass="form-control form-control-sm" />
                       </div>
                       <div class="col" style="display: none">
                           <asp:TextBox ID="txtFracciones" runat="server" CssClass="form-control form-control-sm" />
                       </div>
                       <div class="col">
                           <asp:LinkButton ID="btnAgregar" runat="server"><i class="fa fa-plus text-primary" style="font-size:x-large;padding-left:2px;padding-right:2px"></i></asp:LinkButton>
                       </div>
                       <div class="col form-check">
                           <input class="form-check-input" type="checkbox" value="" id="chkFijo" runat="server" />
                           <label class="form-check-label" for="chkFijo">
                               Mantener ventana
                           </label>
                       </div>
                   </div>
                   <div class="row" style="background-color: white">
                       <div class="col">
                           <asp:GridView ID="gvProductos" runat="server" CssClass="table table-sm table-bordered table-hover" EmptyDataText="No se econtraron productos" AutoGenerateColumns="false">
                               <Columns>
                                   <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-arrow-right text-primary" SelectText="" ShowSelectButton="True"></asp:CommandField>
                                   <asp:BoundField DataField="Codigo" HeaderText="Codigo"></asp:BoundField>
                                   <asp:BoundField DataField="Descripcion" HeaderText="Descripcion"></asp:BoundField>
                                   <asp:BoundField DataField="Precio" HeaderText="Precio"></asp:BoundField>
                                   <asp:BoundField DataField="Stock" HeaderText="Stock"></asp:BoundField>
                                   <asp:BoundField DataField="Costo" HeaderText="Costo" ItemStyle-ForeColor="Transparent"></asp:BoundField>
                                   <asp:BoundField DataField="Fracciones" HeaderText="Frac"></asp:BoundField>
                               </Columns>
                           </asp:GridView>
                       </div>
                   </div>
               </div>               
           </div> 

           <div class="container-fluid" style="background-color: rgba(0,0,0,0.6); position:absolute; left: 0; top: 0; bottom: 0" id="PanelClientes" runat="server" visible="false">
               <div style="padding-left: 15%; padding-right: 15%; padding-top: 40px;">
                   <div class="row" style="background-color: white; padding-top: 15px;">                   
                       <div class="col">
                           <%-- OnTextChanged="TxtBuscarCliente_TextChanged" AutoPostBack="true" --%>
                           <asp:TextBox ID="txtBuscarCliente" runat="server" CssClass="form-control form-control-sm" placeholder="Codigo o Nombre de cliente..." Width="50%" TextMode="SingleLine" />
                       </div>                    
                   </div>
                   <div class="row" style="background-color: white">
                       <div class="col">
                           <asp:GridView ID="gvClientes" runat="server" CssClass="table table-sm table-bordered table-hover" EmptyDataText="No se econtraron clientes">
                               <Columns>
                                   <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-arrow-right text-primary" SelectText="" ShowSelectButton="True"></asp:CommandField>
                               </Columns>
                           </asp:GridView>
                       </div>                       
                   </div>
               </div>
               <asp:LinkButton ID="btnCancelarC" runat="server" Style="position: absolute; right: 15%; top: 38px; font-size: x-large"><i class="far fa-times-circle text-secondary"></i></asp:LinkButton>
           </div>

           <div class="container-fluid" style="background-color: rgba(0,0,0,0.2); position: absolute; left: 0; top: 0; bottom: 0" id="PanelPagos" runat="server" visible="false">
               <div style="padding-left: 15%; padding-right: 15%; padding-top: 40px;">
                   <div class="row" style="background-color: white; padding-top: 15px">
                       <div class="col input-group input-group-sm">
                           <div class="input-group-prepend">
                               <span class="input-group-text">Valor</span>
                           </div>
                           <asp:TextBox ID="txtMontoP" runat="server" CssClass="form-control form-control-sm" TextMode="Number" />
                       </div>                                     
                       <div class="col input-group input-group-sm" style="padding-left: 0px">
                           <div class="input-group-prepend">
                               <span class="input-group-text">Medio</span>
                           </div>
                           <%-- OnSelectedIndexChanged="dlMedio_SelectedIndexChanged" AutoPostBack="true" --%>
                           <asp:DropDownList ID="dlMedio" runat="server" CssClass="form-control form-control-sm" >
                               <asp:ListItem Selected="True">Efectivo</asp:ListItem>
                               <asp:ListItem>Tarjeta</asp:ListItem>
                               <asp:ListItem>Cheque</asp:ListItem>
                               <asp:ListItem>Transferencia</asp:ListItem>
                           </asp:DropDownList>
                       </div>
                       <div class="col input-group input-group-sm" style="padding-left: 0px" runat="server" id="colRef" visible="false">
                           <div class="input-group-prepend">
                               <span class="input-group-text">Num</span>
                           </div>
                           <asp:TextBox ID="txtRef" runat="server" CssClass="form-control form-control-sm" TextMode="SingleLine" />
                       </div> 
                       <div class="col input-group input-group-sm" style="padding-left: 0px" id="colBanco" runat="server" visible="false">
                           <div class="input-group-prepend">
                               <span class="input-group-text">Banco</span>
                           </div>
                           <asp:DropDownList ID="dlBanco" runat="server" CssClass="form-control form-control-sm">
                               <asp:ListItem Selected="True">Ninguno</asp:ListItem>
                               <asp:ListItem>BAC</asp:ListItem>
                               <asp:ListItem>Atlantida</asp:ListItem>
                               <asp:ListItem>Fihcosa</asp:ListItem>
                               <asp:ListItem>Banpais</asp:ListItem>
                               <asp:ListItem>BanPais</asp:ListItem>
                               <asp:ListItem>Promerica</asp:ListItem>
                               <asp:ListItem>Occidente</asp:ListItem>
                               <asp:ListItem>BanRural</asp:ListItem>
                           </asp:DropDownList>
                       </div>
                       <div class="col input-group input-group-sm" style="padding-left: 0px">
                           <div class="input-group-prepend">
                               <span class="input-group-text" style="font-weight: bold">Saldo</span>
                           </div>
                           <label id="lblSaldo" runat="server" class="form-control form-control-sm" style="font-weight: bold">0.00</label>
                       </div> 
                       <div class="col">
                           <asp:LinkButton ID="btnAgregarPago" runat="server"><i class="fa fa-plus text-primary" style="font-size:x-large;padding-left:2px;padding-right:2px"></i></asp:LinkButton>
                       </div>                  
                   </div>
                   <div class="row" style="background-color: white">
                       <div class="col">
                           <asp:GridView ID="gvPagos" runat="server" CssClass="table table-sm table-bordered table-hover" EmptyDataText="Favor ingresar pago">
                               <Columns>
                                   <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-trash-alt text-danger" ShowDeleteButton="True" DeleteText=""></asp:CommandField>
                               </Columns>
                           </asp:GridView>
                       </div>
                   </div>
                    
               </div>
               <asp:LinkButton ID="btnCerrarPago" runat="server" Style="position: absolute; right: 15%; top: 38px; font-size: x-large"><i class="far fa-times-circle text-secondary"></i></asp:LinkButton>
           </div>  

           <div class="container-fluid" style="background-color: rgba(0,0,0,0.6); position: absolute; left: 0; top: 0; bottom: 0" id="PanelBuscar" runat="server" visible="false">
               <div style="padding-left: 15%; padding-right: 15%; padding-top: 40px;">
                   <div class="row" style="background-color: white; padding-top: 15px;">
                       <div class="col">
                           <div class="input-group input-group-sm">
                               <%-- OnTextChanged="TxtBuscarBus_TextChanged" AutoPostBack="true" --%>
                               <asp:TextBox ID="txtBuscarBus" runat="server" CssClass="form-control form-control-sm" placeholder="Cliente..."  />
                               <asp:TextBox ID="txtF1Bus" TextMode="Date" runat="server" CssClass="form-control form-control-sm" />
                               <asp:TextBox ID="txtF2Bus" TextMode="Date" runat="server" CssClass="form-control form-control-sm" />
                               <div class="input-group-append">
                                   <label class="input-group-text">
                                       <asp:LinkButton ID="btnBuscarBus" runat="server" CssClass="fas fa-search text-secondary"></asp:LinkButton></label>
                               </div>
                           </div>
                       </div>
                       <div class="col" style="text-align: right">
                           <asp:LinkButton ID="btnCancelarBuscar" runat="server" Style="font-size: x-large"><i class="far fa-times-circle text-secondary"></i></asp:LinkButton>
                       </div>
                   </div>
                   <div class="row" style="background-color: white">
                       <div class="col">
                           <asp:GridView ID="GvBuscar" runat="server" CssClass="table table-sm table-bordered table-hover" EmptyDataText="No se econtraron datos" AutoGenerateColumns="false">
                               <Columns>
                                   <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-arrow-right text-primary" SelectText="" ShowSelectButton="True"></asp:CommandField>
                                   <asp:BoundField DataField="Numero" HeaderText="Numero"></asp:BoundField>
                                   <asp:BoundField DataField="Fecha Sis" HeaderText="Fecha Sis"></asp:BoundField>
                                   <asp:BoundField DataField="Fecha Doc" HeaderText="Fecha Doc"></asp:BoundField>
                                   <asp:BoundField DataField="Codigo" HeaderText="Codigo"></asp:BoundField>
                                   <asp:BoundField DataField="Nombre" HeaderText="Nombre"></asp:BoundField>
                                   <asp:BoundField DataField="Total" HeaderText="Total"></asp:BoundField>
                                   <asp:BoundField DataField="Estado" HeaderText="Estado"></asp:BoundField>
                               </Columns>
                           </asp:GridView>
                       </div>
                   </div>
               </div>
           </div>

           <div class="container-fluid" style="background-color: rgba(0,0,0,0.6); position: absolute; left: 0; top: 0; bottom: 0" id="PanelCopiar" runat="server" visible="false">
               <div style="padding-left: 15%; padding-right: 15%; padding-top: 40px;">
                   <div class="row" style="background-color: white; padding-top: 15px;">
                       <div class="col">
                           <div class="input-group input-group-sm">
                               <%--OnTextChanged="TxtCopiar_TextChanged" AutoPostBack="true"--%>
                               <asp:TextBox ID="txtCopiar" runat="server" CssClass="form-control form-control-sm" placeholder="Cliente..."  />
                               <asp:TextBox ID="txtF1Copiar" TextMode="Date" runat="server" CssClass="form-control form-control-sm" />
                               <asp:TextBox ID="txtF2Copiar" TextMode="Date" runat="server" CssClass="form-control form-control-sm" />
                               <div class="input-group-append">
                                   <label class="input-group-text">
                                       <asp:LinkButton ID="btnBuscarCopiar" runat="server" CssClass="fas fa-search text-secondary"></asp:LinkButton></label>
                               </div>
                           </div>
                       </div>
                       <div class="col" style="text-align: right">
                           <asp:LinkButton ID="btnCancelarCopiar" runat="server" Style="font-size: x-large"><i class="far fa-times-circle text-secondary"></i></asp:LinkButton></div>
                   </div>
                   <div class="row" style="background-color: white">
                       <div class="col">
                           <asp:GridView ID="GvCopiar" runat="server" CssClass="table table-sm table-bordered table-hover" EmptyDataText="No se econtraron datos" AutoGenerateColumns="false">
                               <Columns>
                                   <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-arrow-right text-primary" SelectText="" ShowSelectButton="True"></asp:CommandField>
                                   <asp:BoundField DataField="Numero" HeaderText="Numero"></asp:BoundField>
                                   <asp:BoundField DataField="Fecha Sis" HeaderText="Fecha Sis"></asp:BoundField>
                                   <asp:BoundField DataField="Fecha Doc" HeaderText="Fecha Doc"></asp:BoundField>
                                   <asp:BoundField DataField="Codigo" HeaderText="Codigo"></asp:BoundField>
                                   <asp:BoundField DataField="Nombre" HeaderText="Nombre"></asp:BoundField>
                                   <asp:BoundField DataField="Total" HeaderText="Total"></asp:BoundField>
                                   <asp:BoundField DataField="Estado" HeaderText="Estado"></asp:BoundField>
                               </Columns>
                           </asp:GridView>
                       </div>
                   </div>
               </div>
           </div>

           <div id="PanelTotales" runat="server" style="width: 300px; height: auto; position: fixed; right: 0px; bottom: 10px">
               <%--<div class="row input-group input-group-sm">
                   <div class="input-group-prepend">
                       <span class="input-group-text" style="width: 80px">Sub Total</span>
                   </div>
                   <label id="lblSubTotal" runat="server" class="form-control form-control-sm">0.00</label>
                   <div class="input-group-append">
                       <span class="input-group-text">L</span>
                   </div>
               </div>--%>
               <%--<div class="row input-group input-group-sm">
                   <div class="input-group-prepend">
                       <span class="input-group-text" style="width: 80px">Descuento</span>
                   </div>
                   <label id="lblDescuento" runat="server" class="form-control form-control-sm">0.00</label>
                   <div class="input-group-append">
                       <span class="input-group-text">L</span>
                   </div>
               </div>--%>
              <%-- <div class="row input-group input-group-sm">
                   <div class="input-group-prepend">
                       <span class="input-group-text" style="width: 80px">Impuesto</span>
                   </div>
                   <label id="lblImpuesto" runat="server" class="form-control form-control-sm">0.00</label>
                   <div class="input-group-append">
                       <span class="input-group-text">L</span>
                   </div>
               </div>      --%>         
               <div class="row input-group input-group-sm">
                   <div class="input-group-prepend">
                       <span class="input-group-text" style="width: 80px; font-weight: bold">Total</span>
                   </div>
                   <label id="lblTotal" runat="server" class="form-control form-control-sm" style="font-weight: bold">0.00</label>
                   <div class="input-group-append">
                       <span class="input-group-text" style="font-weight: bold">L</span>
                   </div>
               </div>
               <div class="row input-group input-group-sm">
                   <div class="input-group-prepend">
                       <span class="input-group-text" style="width: 80px; font-weight: bold">Cambio</span>
                   </div>
                   <label id="lblCambio" runat="server" class="form-control form-control-sm" style="font-weight: bold">0.00</label>
                   <div class="input-group-append">
                       <span class="input-group-text" style="font-weight: bold">L</span>
                   </div>
               </div>
           </div>
           
           <div class="container-fluid" style="background-color: rgba(0, 0, 0,0.8); position: absolute; left: 0; top: 0; bottom: 0" id="PanelCancelar" runat="server" visible="false">
               <div style="padding-left: 38%; padding-right: 38%;padding-top:5%">
                   <div class="row rounded" style="background-color:white; padding: 5px 20px 15px 20px">
                       <div class="col" style="width:100%;text-align:center">
                           <div class="row" style="padding-bottom:3px">
                               <div class="col"><label>¿Desea cancelar el documento? esta acción es permanente.</label></div>
                           </div>  
                           <div class="row">
                               <div class="col">
                                   <asp:Button ID="btnAceptarCan" runat="server" Text="   Si   " CssClass="btn btn-primary btn-sm"/>&nbsp &nbsp &nbsp <asp:Button ID="bntCancelarCan" runat="server" Text="  No  " CssClass="btn btn-secondary btn-sm"/></div>
                           </div>                               
                       </div>                                        
                   </div>                         
               </div>               
           </div>

           <div style="position:fixed; bottom:20px; padding-left:15px;" >               
               <asp:Label ID="lblMsg" runat="server"></asp:Label>
           </div>

            <br />
            <br />
            <br />
            <br />
            <br />

       </div>
    <!-- /#page-content-wrapper -->

  </div>
   <!-- /#wrapper -->

    <!-- Bootstrap core JavaScript -->
    <script src="vendor/jquery/jquery.min.js"></script>
    <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

    <!-- Menu Toggle Script -->
    <script>
        $("#menu-toggle").click(function (e) {
            e.preventDefault();
            $("#wrapper").toggleClass("toggled");
        });
    </script>
    </form>  
</body>
</html>
