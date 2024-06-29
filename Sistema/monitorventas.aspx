<%@ Page Language="vb" AutoEventWireup="false"  EnableEventValidation="false" CodeBehind="monitorventas.aspx.vb" Inherits="Sistema.monitorventas" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register TagPrefix="uc" TagName="CorrectSalesDataClient" Src="~/controls/monitorDeVentas/CorrectDataClient.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="viewport" content="initial-scale=1.0, user-scalable=yes"/>
    <title>Monitor de Ventas</title>
    <link rel="shortcut icon" type="image/x-icon" href="~/imagenes/logo.ico" />
<%--    <link href="css/BootStrap.min.css" rel="stylesheet" type="text/css"/>    --%>
    <link href="css/DataGrid.css" rel="stylesheet" type="text/css"/>    
    <link href="css/inicio.css" rel="stylesheet" type="text/css"/>    
<%--      <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>--%>
    <link rel="shortcut icon" type="image/x-icon" href="svgs/solid/drafting-compass.svg"/>

  <!-- Custom styles for this template -->
  <link href="css/simple-sidebar.css" rel="stylesheet"/>
  <link rel="stylesheet" href="css4/all.min.css"/>

     <!-- Bootstrap core CSS -->  
<%--  <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>--%>
  <link rel="shortcut icon" type="image/x-icon" href="svgs/solid/drafting-compass.svg"/>
        <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous" />



  <script src="https://kit.fontawesome.com/9189b1e7bc.js" ></script>
    
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
            <div><h4 style="width:100%; text-align:center">Monitor de Ventas</h4></div>
           
            <div class="container-fluid" style="padding-top:5px;"> 
                    <div class="row">
                      <div class="col-sm-4 col-d-3 col-lg-2">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="txtFechalb" class="input-group-text" style="width: 75px">Fecha</label>
                           </div>
                           <asp:TextBox TextMode="Date" class="form-control form-control-sm" id="txtFecha" runat="server"></asp:TextBox>
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
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dlRun" class="input-group-text" style="width: 75px">Run</label>
                           </div>
                           <asp:DropDownList ID="dlRun" runat="server" CssClass="form-control form-control-sm">
                               
                           </asp:DropDownList>
                       </div>
                   </div>  
                </div>
                  <div class="row">
                      <div class="col-sm-4 col-d-3 col-lg-2">
                       <input type="text" class="form-control form-control-sm" placeholder="Codigo Vendedor" id="txtCobrador" runat="server" />
                         
                    </div>
                      <div class="row p-2">
                          
    <div class="col-md-2 mb-2">
        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-sm btn-primary w-100" />
    </div>
    <div class="col-md-2 mb-2">
        <asp:Button ID="btnArreglarVenta" runat="server" Text="Arreglar una venta" ToolTip="Boton en Proceso, completo en un 80%..." Visible="false" CssClass="btn btn-sm btn-primary w-100" />
    </div>
                                 <div class="col-auto">
            <asp:LinkButton Visible="false" ID="btnAdvanced" ToolTip="IR A DASHBOARD AVANZADO" CssClass="btn btn-sm btn-outline-warning" runat="server">
                <i class="bi bi-coin"></i> AVANZADO
            </asp:LinkButton>
        </div>
                      </div>

                          </div>
               
              
          </div>
           
            <div class="container-fluid" style="padding-top:5px;"> 
                <div class="table-responsive">

                    <asp:GridView ID="gvMonitor" runat="server" CssClass="table table-sm table-striped table-hover" DataKeyNames="Codigo" BackColor="white" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" AllowSorting="True">
                        <AlternatingRowStyle BackColor="#DCDCDC" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <img alt="" src="imagenes/plus.png" />
                                    <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                        <asp:GridView ID="gvDetalle" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid">
                                            <AlternatingRowStyle BackColor="#eeeeee" />
                                            <Columns>
                                                <asp:BoundField DataField="Codigo" HeaderText="Codigo" ItemStyle-Width="120px" />
                                                <asp:BoundField DataField="Cobrado" HeaderText="Cobrado" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="Hora" HeaderText="Hora" ItemStyle-Width="60px" />
                                                <asp:BoundField DataField="Codigo Cliente" HeaderText="Cod. Cliente" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="Nombre_clie" HeaderText="Nombre Cliente" ItemStyle-Width="185px" ControlStyle-Font-Size="Small" />
                                                <asp:BoundField DataField="identidad" HeaderText="Identidad" ItemStyle-Width="110px" ControlStyle-Font-Size="Small" />
                                                <asp:BoundField DataField="CONT_VALCUO" HeaderText="Cuota" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="CONT_NUMCUO" HeaderText="N. Cuotas" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="SERVI1DES" HeaderText="Producto" ItemStyle-Width="280px" />
                                                <asp:BoundField DataField="CONCEPTO" HeaderText="Motivo" ItemStyle-Width="100px" ControlStyle-Font-Size="Small" />
                                                <asp:BoundField DataField="Dir_cliente" HeaderText="Direccion" ItemStyle-Width="360px" ControlStyle-Font-Size="Small" />
                                                <asp:BoundField DataField="Telefono" HeaderText="Telefono" ItemStyle-Width="360px" ControlStyle-Font-Size="Small" />
                                                <%--<asp:ButtonField CommandName ="Ver" Text="Ubicacion" ControlStyle-CssClass ="fa-solid fa-screwdriver-wrench"/>--%>
                                                <asp:BoundField DataField="ClientesSistema" HeaderText="ClienteSistema" ItemStyle-Width="185px" ControlStyle-Font-Size="Small" />
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>

                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:ButtonField CommandName="Liquidar" Text="Liquidar" Visible="False" />
                            <asp:ButtonField Text="Imprimir" CommandName="Imprimir" Visible="false" />
                            <asp:ButtonField Text="Mapa" CommandName="Mapa" Visible="true" />
                            <%--<asp:ButtonField CommandName="ver" Text="Ver" Visible="true"/>--%>
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="black" />
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#0000A9" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#000065" />
                    </asp:GridView>
                    <br />
                <asp:GridView ID="gvMonitor2" runat="server"  CssClass="table table-sm table-striped table-hover" DataKeyNames="Codigo" BackColor="White" 
                    BorderColor="#DEDFDE" BorderStyle="None" 
                    BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" AllowSorting="True"><AlternatingRowStyle BackColor="White" />
                    
                    <Columns>   
                    <%-- <asp:ButtonField Text="Ver" CommandName ="Ver" Visible="true"/>   --%>
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" /><HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" /><RowStyle BackColor="#F7F7DE" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" /><SortedAscendingCellStyle BackColor="#FBFBF2" />
                    <SortedAscendingHeaderStyle BackColor="#848384" /><SortedDescendingCellStyle BackColor="#EAEAD3" /><SortedDescendingHeaderStyle BackColor="#575357" />

                </asp:GridView> 
                    </div>
                <asp:GridView ID="gvDetalle2" runat="server"  DataKeyNames="Codigo" AllowSorting="True" Visible="False" CssClass="ChildGrid table-sm table-striped table-hover" AutoGenerateColumns="False" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4"><Columns>
                    <asp:ButtonField CommandName="Anular" Text="Anular"/>
                     <asp:ButtonField CommandName ="Ubicacion" Text="Ubicacion"/>
                    <asp:BoundField DataField="Codigo" HeaderText="Codigo" ItemStyle-Width="120px" ><ItemStyle Width="120px" /></asp:BoundField>
                    <asp:BoundField DataField="Cobrado" HeaderText="Cobrado" ItemStyle-Width="80px" ><ItemStyle Width="80px" /></asp:BoundField>
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" ItemStyle-Width="80px" ><ItemStyle Width="80px" /></asp:BoundField>
                    <asp:BoundField DataField="Hora" HeaderText="Hora" ItemStyle-Width="60px" ><ItemStyle Width="60px" /></asp:BoundField>
                    <asp:BoundField DataField="Codigo Cliente" HeaderText="Cod. Cliente" ItemStyle-Width="80px" ><ItemStyle Width="80px" /></asp:BoundField>
                    <asp:BoundField DataField="Nombre_clie" HeaderText="Nombre Cliente" ItemStyle-Width="220px" ControlStyle-Font-Size="Small" ><ControlStyle Font-Size="Small" /><ItemStyle Width="220px" /> </asp:BoundField>
                    <asp:BoundField DataField="identidad" HeaderText="Identidad" ItemStyle-Width="110px" ControlStyle-Font-Size="Small" ><ControlStyle Font-Size="Small" /><ItemStyle Width="110px" /></asp:BoundField>
                    <asp:BoundField DataField="CONT_VALCUO" HeaderText="Cuota" ItemStyle-Width="80px" ><ItemStyle Width="80px" /></asp:BoundField>
                    <asp:BoundField DataField="CONT_NUMCUO" HeaderText="N. Cuotas" ItemStyle-Width="80px" ><ItemStyle Width="80px" /></asp:BoundField>
                    <asp:BoundField DataField="SERVI1DES" HeaderText="Producto" ItemStyle-Width="280px" ><ItemStyle Width="280px" /></asp:BoundField>
                    <asp:BoundField DataField="CONCEPTO" HeaderText="Motivo" ItemStyle-Width="100px" ControlStyle-Font-Size="Small" ><ControlStyle Font-Size="Small" /><ItemStyle Width="100px" /></asp:BoundField>
                    <asp:BoundField DataField="Dir_cliente" HeaderText="Direccion" ItemStyle-Width="360px" ControlStyle-Font-Size="Small" ><ControlStyle Font-Size="Small" /><ItemStyle Width="360px" /></asp:BoundField>
                    <asp:BoundField DataField="ClientesSistema" HeaderText="ClientesSistema" ItemStyle-Width="185px" ControlStyle-Font-Size="Small" ><ControlStyle Font-Size="Small" /><ItemStyle Width="185px" /> </asp:BoundField>
                                                                                                                                                                                                                                                               </Columns>
                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" /><HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" /><PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" /><RowStyle BackColor="White" ForeColor="#003399" />
                    <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" /><SortedAscendingCellStyle BackColor="#EDF6F6" />
                    <SortedAscendingHeaderStyle BackColor="#0D4AC4" /><SortedDescendingCellStyle BackColor="#D6DFDF" />
                    <SortedDescendingHeaderStyle BackColor="#002876" /></asp:GridView> 
                <br /><br />
                
            <asp:ImageButton ID="btnSalir" ToolTip="Regresar" runat="server" Height="30px" ImageUrl="~/imagenes/atras.png" Width="30px" style="position:fixed; right:10px; top:10px;"/>
            <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </div>
            <div style="width:100%; height:30px;  position:fixed; bottom:0px; background-color:#202020; left: 0px; color: #fff">
            <asp:Label ID="lblTotal" runat="server" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblRecibos" runat="server" Text="" />&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblVisitados" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblVentas" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblVerdes" Visible ="false" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblCobradores" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblHora" runat="server" Text="" ForeColor="White"/>
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
                                <asp:TableCell ColumnSpan="2" HorizontalAlign="Center"><asp:Button runat="server" ID="btnAceptar" Text="Aceptar" CssClass="btn btn-primary btn-sm"/>&nbsp
                                <asp:Button runat="server" ID="btnCancelar" Text="Cancelar" CssClass="btn btn-secondary btn-sm"/></asp:TableCell>
                            </asp:TableRow>
                        </asp:table></div><div class="col-md-4"></div>
                </div>
            </div>
        </asp:Panel> 
        <asp:Panel ID="PanelImpresion" runat="server" Visible="false">   
            <div>
                <asp:Button ID="btnProcesar" ToolTip="Procesar" Text="Procesar" runat="server" Height="30px" CssClass="btn btn-primary btn-sm" style=" position:fixed; left:7px; top:13px;"/><asp:ImageButton ID="btnRegresar" ToolTip="Regresar" runat="server" Height="30px" style="position:fixed; right:10px; top:10px;" ImageUrl="~/imagenes/atras.png" Width="30px"/>
            </div>
            <div style="width:100%; height:100%;">
            <iframe id="ifRepote" runat="server" style="position:fixed; width:100%; height:100%; top:42px;"></iframe>
            </div>
            
        </asp:Panel>
        <%-- Panel Para Editar Venta --%>
        <div class="container-fluid" style="background-color: rgba(0,0,0,0.6); position: absolute; left: 0; top: 0; bottom: 0" id="PanelEditarVenta" runat="server" visible="false">
            <div  style="padding-left: 15%; padding-right: 15%; padding-top: 40px;">
                <div class="row" style="background-color: white">
                    <h2 style="padding-left: 40%; padding-right: 5%; padding-top: 10px;">Editar Venta</h2>
                </div>
                <%-- Cuerpo del Modal --%><div class="row" style="background-color: white">
                    <div class="col-lg-4">
                        <div class="input-group input-group-sm">
                            <div class="input-group-prepend">
                                <span class="input-group-text" style="min-width: 100px;" >Vendedor</span>
                            </div>
                            <asp:TextBox TextMode="SingleLine" CssClass="form-control form-control-sm" placeholder="Código" ID="txtCodVendEV" runat="server" OnTextChanged="txtVendEV_TextChanged" AutoPostBack="true" />
                            <div class="input-group-append">
                                <span class="input-group-text input-group-">
                                    <asp:LinkButton ID="btnBusVendEdt" runat="server" CssClass="fas fa-search text-secondary"></asp:LinkButton></span>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4">
                        <div class="input-group input-group-sm">
                            <span class="form-control form-control-sm" style="min-width: 100px;" id="txtnombreVendArr" runat="server"></span>
                        </div>
                    </div>
                    <div class="col-lg-4">
                        <div class="input-group input-group-sm">
                            <div class="input-group-prepend">
                                <span class="input-group-text" style="min-width: 100px;">Cambiar estatus</span>
                            </div>
                            <asp:DropDownList ID="dlempresaArr" runat="server" CssClass="form-control form-control-sm" Style="width: 95px" OnTextChanged="dlempresaArr_TextChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>

                        .                   
                    </div>

                </div>

                <div class="row" style="background-color: white">
                    <div class="col-lg-4">
                        <div class="input-group input-group-sm">
                            <div class="input-group-prepend">
                                <span class="input-group-text" style="min-width: 100px;">Cliente</span>
                            </div>
                            <%-- OnTextChanged="txtCodClienteapp_TextChanged" AutoPostBack="true" --%><asp:TextBox TextMode="SingleLine" CssClass="form-control form-control-sm" placeholder="Código" ID="txtCodClienteapp" runat="server" />
                            <div class="input-group-append">
                                <span class="input-group-text input-group-" >
                                    <asp:LinkButton ID="btnBuscClienVE" runat="server" CssClass="fas fa-search text-secondary"></asp:LinkButton></span>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4">
                        <div class="input-group input-group-sm">

                            <label class="form-control form-control-sm" id="lblNameClientapp" runat="server"></label>
                        </div>
                    </div>
                    <div class="col-lg-4">
                        <%--<label class="btn btn-group-sm btn-dark" style="grid-column-end">Liquidar</label>--%>
                        <div class="input-group input-group-sm">
                            <div class="input-group-prepend">
                                <span class="input-group-text" style="min-width: 100px;" >Estatus</span>
                            </div>
                            <asp:DropDownList ID="dlstatusvend" runat="server" CssClass="form-control form-control-sm" Style="width: 95px"></asp:DropDownList>
                        </div>
                        <%--  <asp:LinkButton ID="LinkButton2" runat="server" Style="font-size: x-large">  Liquidar </asp:LinkButton>

                        <asp:LinkButton ID="LinkButton1" runat="server" Style="position: absolute; right: 8%; font-size: x-large">  Pausar </asp:LinkButton>--%>
                    </div>
                </div>
                                <div class="row" style="background-color: white">                <uc:CorrectSalesDataClient ID="CorrectSalesDataClient1" runat="server" />
</div>

                <%-- Identidad --%>
            </div>
        </div>


        <div class="row" style="background-color: white; padding-top: 15px">
            <div class="col">
                <asp:Label ID="lblMsjError" runat="server"></asp:Label>
            </div>

<%--        <asp:LinkButton ID="btnCancelarC" runat="server" Style="position: absolute; right: 15%; top: 38px; font-size: x-large">  X </asp:LinkButton>--%>
        </div>
        <%--         <div style="position:fixed; bottom:60px; padding-left:35px;" >               
               <asp:Label ID="lblMsjError" runat="server">Prueba del Mensaje</asp:Label>
           </div>--%></div><%-- Panel de Vendedores --%>
        <div class="container-fluid" style="background-color: rgba(0,0,0,0.6); position: absolute; left: 0; top: 0; bottom: 0" id="PanelVendedoresEditar" runat="server" visible="false">
            <div style="padding-left: 15%; padding-right: 15%; padding-top: 40px;">
                <div class="row" style="background-color: white; padding-top: 15px;">
                    <div class="col">

                        <asp:TextBox ID="txtBuscarVended" runat="server" class="form-control form-control-sm" placeholder="Codigo o Nombre de cliente..." Width="50%" TextMode="SingleLine" OnTextChanged="txtBuscarVendedorV_TextChanged" AutoPostBack="true" />
                    </div>
                </div>
                <div class="row" style="background-color: white">
                    <div class="col">
                        <asp:GridView ID="gvvendEditVent" runat="server" CssClass="table table-sm table-bordered table-hover" EmptyDataText="No se econtraron clientes">
                            <Columns>
                                <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-arrow-right text-primary" SelectText="" ShowSelectButton="True"></asp:CommandField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <asp:LinkButton ID="btnCerarPVend" runat="server" Style="position: absolute; right: 15%; top: 38px; font-size: x-large"><i class="far fa-times-circle text-secondary"></i></asp:LinkButton>
        </div>
        <%-- Panel Clientes --%>
        <div class="container-fluid" style="background-color: white; position: absolute; left: 0; top: 0; bottom: 0" id="PanelClientesVE" runat="server" visible="false">
            <div style="padding-left: 5%; padding-right: 5%; padding-top: 40px;">
                <div class="row" style="background-color: white; padding-top: 15px;">
                    <div class="col">
                        <asp:TextBox ID="txtBuscarCliente" runat="server" class="form-control form-control-sm" placeholder="Codigo o Nombre de cliente..." Width="50%" TextMode="SingleLine" OnTextChanged="txtBuscarCliente_TextChanged" AutoPostBack="true" />
                    </div>
                </div>
                <div class="row" style="background-color: white">
                    <div class="col">
                        <asp:GridView ID="gvClientesVE" HtmlEnCode="false" runat="server" CssClass="table table-sm table-bordered table-hover" EmptyDataText="No se econtraron clientes">
                            <Columns>
                                <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-arrow-right text-primary" SelectText="" ShowSelectButton="True"></asp:CommandField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <asp:LinkButton ID="btnCerrarCliapp" runat="server" Style="position: absolute; right: 5%; top: 38px; font-size: x-large"><i class="far fa-times-circle text-secondary"></i></asp:LinkButton>
        </div>
        <%-- Panel Contrato --%>
        <div class="container-fluid" style="background-color: white; position: absolute; left: 0; top: 0; bottom: 0" id="PanelProductosApp" runat="server" visible="false">
            <div style="padding-left: 5%; padding-right: 5%; padding-top: 40px;">
                <div class="row" style="background-color: white; padding-top: 15px;">
                    <div class="col">
                        <asp:TextBox ID="txtBuscarProductoApp" runat="server" class="form-control form-control-sm" placeholder="Codigo o Nombre de cliente..." Width="50%" TextMode="SingleLine" />
                    </div>
                </div>
                <div class="row" style="background-color: white">
                    <div class="col">
                        <asp:GridView ID="gvDetalleProductosContrato" runat="server" CssClass="table table-sm table-bordered table-hover" EmptyDataText="No se econtraron clientes">
                            <Columns>
                                <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-arrow-right text-primary" SelectText="" ShowSelectButton="True"></asp:CommandField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
               </div>
               <asp:LinkButton ID="btnCerrarPanelProductosApp" runat="server" Style="position: absolute; right: 5%; top: 38px; font-size: x-large"><i class="far fa-times-circle text-secondary"></i></asp:LinkButton></div>
        <div class="container-fluid" style="background-color: rgba(0,0,0,0.6); position:absolute; left: 0; top: 0; bottom: 0" id="PanelConfirmacion" runat="server" visible="false">
               <div style="padding-left: 30%; padding-right: 30%; padding-top: 40px;">
                   <div class="row" style="background-color: white; padding-top: 15px;">
                       <div class="col-8">
                           <div class="input-group-prepend align-content-center" style="text-align: left"">
                               
                           <label class="alert-primary align-content-center">Está seguro qué desea grabar este documento?</label> </div></div></div><div class="row" style="background-color: white; padding-top: 15px;">
                       <div class="col-8">
                           <div class="col" style="text-align: right">
                               <asp:Button ID="BtnSiSalvarCamb" runat="server" Text="   Salvar   " CssClass="btn btn-sm btn-success" />&nbsp&nbsp<asp:Button ID="BtnNoSalvar" runat="server" Text="Cancelar" CssClass="btn btn-sm btn-secondary" />
                           </div>
                       </div>
                     
                   </div>
             
               </div>
           </div>
     <%-- Panel Confirmacion Anular --%>
     <div class="container-fluid" style="background-color: rgba(0,0,0,0.6); position:absolute; left: 0; top: 0; bottom: 0" id="PanelConfirmacion2" runat="server" visible="false">
               <div style="padding-left: 30%; padding-right: 30%; padding-top: 40px;">
                   <div class="row" style="background-color: white; padding-top: 15px;">
                       <div class="col-12">
                           <div class="input-group-prepend align-content-center" style="text-align: left"">
                               
                           <label class="alert-primary align-content-center">Está seguro Que desea Cambiar el Estatus de La venta?</label> </div></div>

                   </div>
                   <div class="row" style="background-color: white; padding-top: 15px;">
                        <div class="col-12">
                        <div class="input-group input-group-sm">
                            <div class="input-group-prepend">
                                <label class="input-group-text" style="width: 110px">Comentario</label> 

                            </div><asp:TextBox ID="txtmotivoCambSta" runat="server" CssClass="form-control form-control-sm" placeholder="Motivo" TextMode="SingleLine" />                   
                            
                        
                        </div>
                    </div>
                   </div>
                   <div class="row" style="background-color: white; padding-top: 15px;">
                       <div class="col-8">
                           <div class="col" style="text-align: right">
                               <asp:Button ID="btnCambStatus" runat="server" Text="   SÍ   " CssClass="btn btn-sm btn-success" />&nbsp&nbsp<asp:Button ID="BtnCerrarStatus" runat="server" Text="No" CssClass="btn btn-sm btn-secondary" />
                           </div>
                       </div>
                     
                   </div>
             
               </div>
           </div>
               <div id="alertPlaceholder" runat="server"></div>


        <script src="js/JQuery.js"></script>
        <script src="js/popper.min.js"></script>
        <script src="js/bootstrap.min.js"></script><script src="js/Jquery1.8.3.js"></script>
        <script type="text/javascript">
            $("[src*=plus]").live("click", function () {
                $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
                $(this).attr("src", "imagenes/minus.png");
            });
            $("[src*=minus]").live("click", function () {
                $(this).attr("src", "imagenes/plus.png");
                $(this).closest("tr").next().remove();
            });


            //const btnArreglar = document.getElementById('btnArreglarVenta');
            //        grid = document.getElementById

            //btnArreglar.addEventListener('mouseover', () => {

            //});


        </script>        
    </form>
</body>
</html>