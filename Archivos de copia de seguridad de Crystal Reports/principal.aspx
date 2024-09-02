<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="principal.aspx.vb" Inherits="Sistema.principal" %>

<!DOCTYPE html>
   

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>MEMORIAL'S</title>    
    <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
    <link rel="shortcut icon" type="image/x-icon" href="svgs/solid/drafting-compass.svg"/>
    <link href="css/StyleResponsive.css" rel="stylesheet" />

    <script src="js/all.min.js"></script>
        <style type="text/css">
            .Boton:hover {
                transform: scale(1.05);
            }

            p {
                font-size: small;
                width: 100%;
                text-align: center;
            }

            .card-link {
                color: black;
            }
        </style>
    
</head>
<body > 
    <meta name="viewport" content="initial-scale=1.0, user-scalable=yes"/>
    <asp:SqlDataSource ID="SqlDataLider" runat="server" ConnectionString="<%$ ConnectionStrings:FunamorConexion %>" 
        SelectCommand = " Select 'Todos' Lider, '0' codigo union all
        SELECT  Ltrim(rtrim(Cod_vendedo))+' - '+ Nombre_vend, cod_vendedo FROM FUNAMOR..[VENDEDOR] 
        where  [Vend_status] = 'A' and cod_vendedo = vend_lider and Cod_vendedo <> vend_suplid ">
        </asp:SqlDataSource>
     
     <asp:SqlDataSource ID="SqlDataSuper" runat="server" ConnectionString="<%$ ConnectionStrings:FunamorConexion %>" 
        SelectCommand = " Select '------' Super,'0' codigo  
        union all
        SELECT  Ltrim(rtrim(Cod_vendedo))+' - '+ Nombre_vend, cod_vendedo FROM FUNAMOR..[VENDEDOR] 
        where  [Vend_status] = 'A'  and Cod_vendedo = vend_suplid ">
    </asp:SqlDataSource>

     <asp:SqlDataSource ID="SqlDataLiderCobros" runat="server" ConnectionString="<%$ ConnectionStrings:FunamorConexion %>" SelectCommand="Select 'TODOS' Lider,'0' codigo  
        union all
        SELECT  Ltrim(rtrim(codigo_cobr))+' - '+ nombre_cobr, codigo_cobr FROM FUNAMOR..COBRADOR 
        where COBR_STATUS  = 'A'  and codigo_cobr  = cob_lider">
        </asp:SqlDataSource>

    <form id="Principal" runat="server">               
           <header class="header">
            <nav class="nav">              
                <asp:linkbutton CssClass="logo nav-link" ID="btnVersion" runat="server">
                   Memorial's Amor Eterno 
                    <%--<i class="text-sm-rigth fas fa-campground" ></i>--%>
                </asp:linkbutton>
              
               
                  
                 <asp:LinkButton ID="salir"  runat="server" CssClass="logo nav-link">
                     salir&nbsp
                     <i class="text-sm-rigth fas fa-door-open" ></i>

                 </asp:LinkButton>                          
                   
          </nav>
         </header>
         
          <div class="container-fluid" style="padding-top: 5px;" id="Div" runat="server">
            <div class="input-group input-group-sm">
                <label id="lblBienvenido" runat="server" style="color:darkgray; font-display:inherit"  ></label>
            </div>
        </div>
        <div class="col-sm-12 col-md-4 col-lg-2 row container-fluid" style="padding-top:10px;">
            <div class="col">
                <div id="accordion" class="shadow-sm">                    
                    <div class="card">
                        <div class="card-header">
                            <a class="collapsed card-link btn-block" data-toggle="collapse" href="#collapseTwo"><i class="fas fa-tags" style="font-size: 22px;"></i>&nbsp&nbsp Caja</a>
                        </div>
                        <div id="collapseTwo" class="collapse" data-parent="#accordion">
                            <div class="card-body">
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton ID="Caja_Cobros" runat="server" Enabled="false"><i class="fas fa-arrow-right"></i>&nbsp Cobros</asp:LinkButton>                                                

                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton ID="Caja_Ventas" runat="server" Enabled="false"><i class="fas fa-arrow-right"></i>&nbsp Ventas</asp:LinkButton>                                                

                                    </div>
                                </div>
              <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton ID="btncajaclie" runat="server" Enabled ="false" ><i class="fas fa-arrow-right"></i>&nbsp Recibos Clientes</asp:LinkButton>                                                

                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton ID="caja_vales" runat="server" Enabled ="false" ><i class="fas fa-arrow-right"></i>&nbsp Vales</asp:LinkButton>                                                

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card">
                        <div class="card-header">
                            <a class="collapsed card-link btn-block" data-toggle="collapse" href="#collapseThree"><i class="fas fa-shopping-basket" style="font-size: 22px;"></i>&nbsp&nbsp Monitor</a>
                        </div>
                        <div id="collapseThree" class="collapse" data-parent="#accordion">
                            <div class="card-body">
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton ID="Monitor_Cobros" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Monitor de Cobros</asp:LinkButton>                                                </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton ID="Monitor_Ventas" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Monitor de Ventas</asp:LinkButton></div>
                                </div>
                                <%--<div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnComNC" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Notas de credito</asp:LinkButton></div>
                                </div>--%>
                            </div>
                        </div>
                    </div>
                    <div class="card">
                        <div class="card-header">
                            <a class="collapsed card-link btn-block" data-toggle="collapse" href="#collapsefour"><i class="fas fa-pallet" style="font-size: 22px;"></i>&nbsp&nbsp Reporte Cobros</a>
                        </div>
                        <div id="collapsefour" class="collapse" data-parent="#accordion">
                            <div class="card-body">
                               <div class="row table">
                                    <div class="col">                    
                                                <asp:LinkButton ID="Estadisticas_Visitas" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Estadisticas Visitas</asp:LinkButton>                                                
                                          </div>
                                 </div>
                                 <div class="row table">
                                    <div class="col">                    
                                                <asp:LinkButton ID="Clientes_sin_Visitar" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Clientes sin Visitar</asp:LinkButton>                                                
                                            </div>
                                     </div>
                                 <div class="row table">
                                    <div class="col">                    
                                                <asp:LinkButton ID="Clientes_Sin_Cobro" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Clientes sin Cobro</asp:LinkButton>                                                
                                            </div>
                                  </div>
                                 <div class="row table">
                                    <div class="col">                    
                                                <asp:LinkButton ID="Recibos_Nulos" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Recibos Nulos</asp:LinkButton>                                               
                                    </div>
                                     </div>
                                 <div class="row table">
                                    <div class="col">                    
                                                <asp:LinkButton ID="No_Estaba" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp No Estaba</asp:LinkButton>                                                
                                    </div>
                                    </div>
                                 <div class="row table">
                                    <div class="col">
                                                <asp:LinkButton ID="Estadistica_Cobradores" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Estadistica Cobradores</asp:LinkButton>                                                
                                    </div>
                                     </div>
                                 <div class="row table">
                                    <div class="col">
                    
                                                <asp:LinkButton ID="No_Quiere_Continuar" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp No Quiere Continuar</asp:LinkButton>                                                
                                           </div>
                                     </div>
                                 <div class="row table">
                                    <div class="col">
                    
                                                <asp:LinkButton ID="No_cobro_Visitas" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp No cobro Visitas</asp:LinkButton>                                                
                                            </div>
                                     </div>
                                  <div class="row table">
                                    <div class="col">                    
                                                <asp:LinkButton ID="btnMonVisitasCobr" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Monitor Visitar</asp:LinkButton>                                                
                                    </div>
                                     </div>
                                   <div class="row table">
                                    <div class="col">                    
                                                <asp:LinkButton ID="btnCobrosResXFecha" runat="server" Enabled="false" ToolTip="Cobro Resumido por Dia x Cobrador Con Valor x Empresa">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Cobro Resumido x Dia x Cobrador </asp:LinkButton>                                                
                                    </div>
                                     </div>
                                   <div class="row table">
                                    <div class="col">                    
                                                <asp:LinkButton ID="btnCarteraCobrCliente" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Cartera Cobrador Datos Cliente</asp:LinkButton>                                                
                                    </div>
                                     </div>
                    
                            </div>
                        </div>
                    </div>
                    <div class="card">
                        <div class="card-header">
                            <a class="collapsed card-link btn-block" data-toggle="collapse" href="#collapsefive"><i class="fas fa-align-left" style="font-size: 22px;"></i>&nbsp&nbsp Reporte de Ventas</a>
                        </div>
                        <div id="collapsefive" class="collapse" data-parent="#accordion">
                            <div class="card-body">
                                <div class="row table">
                                    <div class="col">
                        
                                                <asp:LinkButton ID="Ventas_Rendimiento" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Ventas Rend Lider</asp:LinkButton>                                                
                                      </div>
                                    </div>
                                  <div class="row table">
                                    <div class="col">
                        
                                                <asp:LinkButton ID="rtpVentDetXF" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Ventas Detallada X Fecha</asp:LinkButton>                                                
                                      </div>
                                    </div>
                                 <div class="row table">
                                    <div class="col">
                        
                                                <asp:LinkButton ID="Ventas_Resindidas" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Ventas Resindidas</asp:LinkButton>                                                
                                 
                                       </div>
                                     </div>
                                 <div class="row table">
                                    <div class="col">
                        

                                      <asp:LinkButton data-toggle="modal" href="#idmodalfirma" ID="Idfirma" runat="server" role="Document">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Firma de Clientes</asp:LinkButton>                                                
                                        </div>
                                     </div>
                            </div>
                        </div>
                    </div>
                    <div class="card">
                        <div class="card-header">
                            <a class="collapsed card-link btn-block" data-toggle="collapse" href="#collapsesix"><i class="fas fa-chart-pie" style="font-size: 22px;"></i>&nbsp&nbsp Informes</a>
                        </div>
                        <div id="collapsesix" class="collapse" data-parent="#accordion">
                            <div class="card-body">
                                
                             <div class="row table">
                                    <div class="col">
                                  
                                     <asp:linkbutton data-toggle="modal"  href="#idModalDiasLabCob"  id="btnNoCobro"  runat="server" Enabled="false" >&nbsp <i class="fas fa-arrow-right"></i>&nbsp Reporte Dias Laborados</asp:linkbutton>                                              
                                   </div>
                                    </div>
                                 <div class="row table">
                                    <div class="col">
                                  
                                
                                     <asp:linkbutton data-toggle="modal"  href="#idEntregasXfecha"  id="btnEntreXF"  runat="server" Enabled="false" >&nbsp <i class="fas fa-arrow-right"></i>&nbsp Entregas x Fecha</asp:linkbutton>                                              
                                   </div>
                                     </div>

                            </div>
                        </div>
                    </div>
                        <div class="card">
                        <div class="card-header">
                            <a class="collapsed card-link btn-block" data-toggle="collapse" href="#collapseClientes"><i class="fas fa-user-tag" style="font-size: 22px;"></i>&nbsp&nbsp Clientes</a>
                        </div>
                        <div id="collapseClientes" class="collapse" data-parent="#accordion">
                            <div class="card-body">
                              <div class="row table">
                               <div class="col">

                                   <asp:LinkButton ID="Movimiento_Cliente" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Movimiento de Cliente</asp:LinkButton>
                               </div>
                           </div>
                                <div class="row table">
                                    <div class="col">

                                        <asp:LinkButton ID="btnInformacionDeClientes" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Información de Cliente</asp:LinkButton>
                                    </div>
                                </div>
                                  <div class="row table">
                               <div class="col">
                                   <asp:LinkButton ID="btninhumados" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Ihumados</asp:LinkButton>
                               </div>
                           </div>
                            <div class="row table">
                               <div class="col">
                                   <asp:LinkButton ID="btnmovcliehist" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Historial Visitas Cliente</asp:LinkButton>
                               </div>
                           </div>
                           <div class="row table">
                               <div class="col">
                                   <asp:LinkButton ID="TrasladoP" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Traslado Cliente</asp:LinkButton>
                               </div>
                           </div>
                            </div>
                        </div>
                    </div>
                    <div class="card" id="MenuProduccion" runat="server" visible="false">
                        <div class="card-header">
                            <a class="collapsed card-link btn-block" data-toggle="collapse" href="#collapseProduccion"><i class="fas fa-user-tag" style="font-size: 22px;"></i>&nbsp&nbsp Clientes</a>
                        </div>
                        <div id="collapseProduccion" class="collapse" data-parent="#accordion">
                            <div class="card-body">
                              <div class="row table">
                               <div class="col">

                                   <asp:LinkButton ID="LinkButton8" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Orden de Produccion</asp:LinkButton>
                               </div>
                           </div>
                                  <div class="row table">
                               <div class="col">
                                   <asp:LinkButton ID="LinkButton9" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Reportes</asp:LinkButton>
                               </div>
                           </div>
                           
                          
                            </div>
                        </div>
                    </div>

                    <div class="card">
                        <div class="card-header">
                            <a class="collapsed card-link btn-block" data-toggle="collapse" href="#collapseInventario"><i class="fas fa-warehouse" style="font-size: 22px;"></i>&nbsp&nbsp Inventario</a>
                        </div>
                        <div id="collapseInventario" class="collapse" data-parent="#accordion">
                            <div class="card-body">
                                <div class="row table">
                                    <div class="col">                     
                                                <asp:LinkButton ID="btnConsultaInv" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Consulta Inventario</asp:LinkButton>                                                
                                         </div>
                                    </div>
                                <div class="row table">
                                    <div class="col">
                                                <asp:LinkButton ID="BtnEntradas" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Entrada de Mercancias</asp:LinkButton>                                                
                                           </div>
                                    </div>
                                   <div class="row table">
                                    <div class="col">   
                                                <asp:LinkButton ID="BtnSalidas" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Salida de Mercancias</asp:LinkButton>                                                
                                           </div>
                                       </div>
                                 <div class="row table">
                                    <div class="col">   
                                                <asp:LinkButton ID="BtnEntregasSPS" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Entregas SPS</asp:LinkButton>                                                
                                           </div>
                                       </div>
                                 <div class="row table">
                                    <div class="col">   
                                                <asp:LinkButton ID="BtnTraslados" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Traslado de Inventario</asp:LinkButton>                                                
                                           </div>
                                       </div>
                                </div>
                            </div>
                   </div>
                    <div class="card">
                        <div class="card-header">
                            <a class="collapsed card-link btn-block" data-toggle="collapse" href="#collapseMant"><i class="fas fa-cogs" style="font-size: 22px;"></i>&nbsp&nbsp Mantenimiento</a>
                        </div>
                        <div id="collapseMant" class="collapse" data-parent="#accordion">
                            <div class="card-body">
                                <div class="row table">
                                    <div class="col">                     
                                                <asp:LinkButton ID="Cobrador" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Editar Cobrador</asp:LinkButton>                                                
                                         </div>
                                    </div>
                                <div class="row table">
                                    <div class="col">
                                                <asp:LinkButton ID="Vendedor" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Editar Vendedor</asp:LinkButton>                                                
                                           </div>
                                    </div>
                                   <div class="row table">
                                    <div class="col">   
                                                <asp:LinkButton ID="Empleado" runat="server" Enabled="false">&nbsp <i class="fas fa-arrow-right"></i>&nbsp Empleado</asp:LinkButton>                                                
                                           </div>
                                       </div>
                                </div>
                            </div>
                   </div>
                      <div class="card">
                        <div class="card-header">
                            <a class="collapsed card-link btn-block" data-toggle="collapse" href="#collapseGestion"><i class="fas fa-boxes" style="font-size: 22px;"></i>&nbsp&nbsp Gestion </a>
                        </div>
                        <div id="collapseGestion" class="collapse" data-parent="#accordion">
                            <div class="card-body">
                                <div class="row table">
                                    <div class="col">
                                               <asp:LinkButton ID="btnGestionCobro" runat="server" Enable="false"  >&nbsp <i class="fas fa-arrow-right"></i>&nbsp Gestion Cobro</asp:LinkButton>                                                
                                    </div>
                                    </div>  
                                <div class="row table">
                                    <div class="col">
                                               <asp:LinkButton ID="UbicacionLotes" runat="server" Enable="false" >&nbsp <i class="fas fa-arrow-right"></i>&nbsp Ubicacion Lotes</asp:LinkButton>                                                
                                    </div>
                                    </div>
                                  <div class="row table">
                                    <div class="col">
                                               <asp:LinkButton ID="LinkButton4" runat="server" Enable="false" >&nbsp <i class="fas fa-arrow-right"></i>&nbsp Eventos</asp:LinkButton>                                                
                                    </div>
                                    </div>
                                  <div class="row table">
                                    <div class="col">
                                               <asp:LinkButton ID="btnPantallaServicios" runat="server" Enable="true" >&nbsp <i class="fas fa-arrow-right"></i>&nbsp Pantalla Servicios</asp:LinkButton>                                                
                                    </div>
                                    </div>
                                </div>
                            </div>
                           </div>

                    <div class="card">
                        <div class="card-header">
                            <a class="collapsed card-link btn-block" data-toggle="collapse" href="#collapseEleven"><i class="fas fa-user-cog" style="font-size: 22px;"></i>&nbsp&nbsp<asp:Label ID="lblUsuario" runat="server" Text="Usuario"/></a>
                        </div>
                        <div id="collapseEleven" class="collapse" data-parent="#accordion">
                            <div class="card-body">
                                <div class="row table">
                                    <div class="col">
                                              <asp:LinkButton ID="Cambio_Contraseña" runat="server">Cambiar Contraseña</asp:LinkButton>
                                         </div>
                                    </div>
                                <div class="row table">
                                    <div class="col">
                                                <span class="glyphicon glyphicon-log-out text-danger"></span>&nbsp<asp:LinkButton CssClass="text-danger" ID="LinkButton3" runat="server">Cerrar Sessión</asp:LinkButton>
                                       </div>
                                    </div>
                                </div>
                            </div>
                   </div>
               

               <%--     <div class="card">
                        <div class="card-header">
                            <a class="collapsed card-link btn-block" data-toggle="collapse" href="#collapseDiez"><i class="fas fa-cogs" style="font-size: 22px;"></i>&nbsp&nbsp Ajustes</a>
                        </div>
                        <div id="collapseDiez" class="collapse" data-parent="#accordion">
                            <div class="card-body">
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="Cambio_Contraseña" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Cambio de clave</asp:LinkButton></div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnParametros" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Parametros y maestros</asp:LinkButton></div>
                                </div>
                            </div>
                        </div>
                    </div>--%>
                     <div class="card">
                        <div class="card-header">

                            </div>
                         </div>
                     <div class="card">
                        <div class="card-header">
                            </div>
                         </div>
                </div>
                   </div>
            </div>
               <%-- <div class="col">
                <div class="row">
                    <div class="col-sm-4 col-d-4 col-lg-4">
                        <div class="card shadow-sm">
                            <div class="card-header" style="font-weight:bold">
                                <h5>Notas importantes</h5>
                            </div>                        
                            <div class="card-body">                                
                                <pre class="card-text" runat="server" id="LblMensaje"></pre>                                
                            </div>
                        </div>
                    </div> 
                </div>
            </div>--%>
   

        <div class="col-sm-12 col-md-4 col-lg-2 container-fluid" style="background-color: rgba(0, 0, 0,0.8); position: absolute; left: 0; top: 0; bottom: 0" id="PanelUsuario" runat="server" visible="false">
               <div style="padding-left: 38%; padding-right: 38%;padding-top:5%">
                   <div class="row rounded" style="background-color:white; padding: 5px 20px 15px 20px">
                       <div class="col" style="width:100%;text-align:center">
                           <div class="row">
                               <div class="col"><img src="imagenes/logo.jpg" width="150" alt="..."/></div>
                           </div>                           
                           <div class="row" style="padding-bottom:3px">
                               <div class="col input-group input-group-sm">
                                   <div class="input-group-prepend">
                                       <span class="input-group-text" style="width: 30px"><i class="fas fa-user"></i></span>
                                   </div>
                                   <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control form-control-sm" TextMode="SingleLine" placeholder="Usuario" />
                               </div>
                           </div>
                           <div class="row" style="padding-bottom:3px">
                               <div class="col input-group input-group-sm">
                                   <div class="input-group-prepend">
                                       <span class="input-group-text" style="width: 30px"><i class="fas fa-key"></i></span>
                                   </div>
                                   <asp:TextBox ID="txtClave" runat="server" CssClass="form-control form-control-sm" TextMode="Password" placeholder="Contraseña" />
                               </div>
                            </div>
                           <div class="row" style="padding-bottom:8px">
                               <div class="col input-group input-group-sm">
                                   <div class="input-group-prepend">
                                       <span class="input-group-text" style="width: 30px"><i class="fas fa-database"></i></span>
                                   </div>
                                   <asp:DropDownList ID="dlDB" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                               </div>
                           </div> 
                           <div class="row">
                               <div class="col">
                                   <asp:Button ID="btnAceptarUsu" runat="server" Text="Aceptar" CssClass="btn btn-primary btn-sm"/></div>
                           </div>                               
                       </div>                                        
                   </div>                         
               </div>               
           </div>

        <div class="col-sm-12 col-md-4 col-lg-2 container-fluid" style="background-color: rgba(0,0,0,0.7); position: absolute; left: 0; top: 0; bottom: 0" id="PanelCambioPass" runat="server" visible="false">
               <div style="padding-left: 40%; padding-right: 40%;padding-top:15%">
                   <div class="row rounded" style="background-color:white; padding: 15px 20px 15px 20px">
                       <div class="col" style="width:100%;text-align:center">
                           <div class="row" style="padding-bottom:3px">
                               <div class="col input-group input-group-sm">
                                   <div class="input-group-prepend">
                                       <span class="input-group-text" style="width: 30px"><i class="fas fa-user"></i></span>
                                   </div>
                                   <asp:TextBox ID="txtCamUsuario" runat="server" CssClass="form-control form-control-sm" TextMode="SingleLine" placeholder="Usuario" />
                               </div>
                           </div>
                           <div class="row" style="padding-bottom:4px">
                               <div class="col input-group input-group-sm">
                                   <div class="input-group-prepend">
                                       <span class="input-group-text" style="width: 30px"><i class="fas fa-key"></i></span>
                                   </div>
                                   <asp:TextBox ID="txtCamPass" runat="server" CssClass="form-control form-control-sm" TextMode="Password" placeholder="Clave actual" />
                               </div>
                            </div>
                           <div class="row" style="padding-bottom:4px">
                               <div class="col input-group input-group-sm">
                                   <div class="input-group-prepend">
                                       <span class="input-group-text" style="width: 30px"><i class="fas fa-file"></i></span>
                                   </div>
                                   <asp:TextBox ID="txtCamPass1" runat="server" CssClass="form-control form-control-sm" TextMode="Password" placeholder="Clave nueva" />
                               </div>
                            </div>
                           <div class="row" style="padding-bottom:4px">
                               <div class="col input-group input-group-sm">
                                   <div class="input-group-prepend">
                                       <span class="input-group-text" style="width: 30px"><i class="fas fa-file"></i></span>
                                   </div>
                                   <asp:TextBox ID="txtCamPass2" runat="server" CssClass="form-control form-control-sm" TextMode="Password" placeholder="Confirmar nueva" />
                               </div>
                            </div>
                           <div class="row">
                               <div class="col">
                                   <asp:Button ID="btnAceptarPass" runat="server" Text="Aceptar" CssClass="btn btn-primary btn-sm"/>&nbsp&nbsp<input id="btnCancelarPass" runat="server" type="button" value="Cancelar" class="btn btn-secondary btn-sm"/></div>
                           </div>                           
                       </div>                                        
                   </div>                         
               </div>               
           </div>

       <%-- <div class="container-fluid" style="background-color: rgba(0,0,0,0.7); position: absolute; left: 0; top: 0; bottom: 0" id="PanelVersion" runat="server" visible="false">
               <div style="padding-left: 40%; padding-right: 40%;padding-top:15%">
                   <div class="row rounded" style="background-color:white; padding: 15px 20px 15px 20px">
                       <div class="col" style="width:100%;text-align:center">
                           <div class="row" style="padding-bottom:3px">
                             <%--<pre style="font-weight:bold"> 
     Honduras Agosto 2020 
    Version 1.0.1</pre>
                           </div>                    
                           <div class="row">
                               <div class="col"><asp:Button ID="btnAceptarVer" runat="server" Text="Aceptar" CssClass="btn btn-primary btn-sm"/></div>
                           </div>                           
                       </div>                                        
                   </div>                         
               </div>               
           </div>               --%>

        <%-- Bloques de  Popup Menu --%>

        <asp:Label ID="lblMsg" runat="server"></asp:Label>


        <div class="container">
        <%--<asp:LinkButton ID="ModRendLider" runat="server" data-toggle="modal" href="#idmodalRendLider" role="Document" ToolTip="Reporte de Rendimiento De lideres">Rendimiento Lider</asp:LinkButton>--%>
                <div id="idmodalRendLider" class="modal">
                    <div class="modal-dialog modal-sm">
                        <div class="modal-content">
                            <!-- Modal Header -->
                            <div class="modal-header align-content-center">
                                <h4 class="align-content-center">Rendimiento Ventas</h4>
                                <button class="close btn btn-danger btn-lg" data-dismiss="modal" data-whatever="" type="button">
                                                                            
                                </button>
                            </div>
                            <!--Cuerpo del Modal-->
                            <div class="modal-body">
                                <div id="formventasrend">
                                    <div class="form-group">
                                        <label class="col-form-label" for="message-text">
                                        <font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Fecha desde</font></font></label>
                                        <asp:TextBox ID="RendFechaDesde1" runat="server" Font-Size="Small" TextMode="Date"> </asp:TextBox>
                                        
                                        <br/>
                                        <label class="col-form-label" for="message-text">
                                        <font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Fecha Hasta</font></font></label>
                                        <asp:TextBox ID="RendFechaHasta2" runat="server" Font-Size="Small" TextMode="Date"></asp:TextBox>
                                       
                                        <br />
                                        <div class="form-group">
                                         
                                            <asp:CheckBox ID="ChekHist" runat="server" Text="Segun Historia" Checked="True" />
                                        
                                            <asp:CheckBox ID="CheckSuper" runat="server" Text="Solo Supervisores" />


                                            </div>
                                    </div>
                                    <div class="form-group">
                                        <table>
                                            <tr id="tr2" runat="server">
                                                <td>
                                                    <label class="col-form-label" for="message-text">
                                                    <font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Lider:</font></font></label>

                                                </td>
                                                
                                                <td>
                                                    <asp:DropDownList ID="DropLider1"  ToolTip="Lider" placeholder="Lider" runat="server" CssClass="form-control form-control-sm" Height="27px" Font-Size="Small" Width="200px">
                                                   
                                                    
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                     
                                        </table>
                                           
                                    </div>
                                </div>
                            </div>
                            <!-- Modal footer -->
                            <div class="modal-footer">
                                <asp:LinkButton id="BuscarRendLider1" runat="server" class="btn btn-success" type="button">Buscar</asp:LinkButton>
                                <asp:LinkButton id="LinkButton1" class="btn btn-outline-danger" data-dismiss="modal" type="button" runat="server">Cancelar</asp:LinkButton>
                            
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        <%-- Fin Modal Para Reporte de Rendimiento de Lideres --%>

        <%-- Reporte De Ventas Por Fecha --%>

                <div class="container">
        
                <div id="idmodalVenXfecha" class="modal">
                    <div class="modal-dialog modal-sm">
                        <div class="modal-content">
                            <!-- Modal Header -->
                            <div class="modal-header align-content-center">
                                <h4 class="align-content-center">Ventas Detalladas Por Fecha</h4>
                                <button class="close btn btn-danger btn-lg" data-dismiss="modal" data-whatever="" type="button">
                                                                            
                                </button>
                            </div>
                            <!--Cuerpo del Modal-->
                            <div class="modal-body">
                                <div id="">
                                    <div class="form-group">
                                        <label class="col-form-label" for="message-text">
                                        <font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Fecha desde</font></font></label>
                                        <asp:TextBox ID="txtVenxfec1" runat="server" Font-Size="Small" TextMode="Date"> </asp:TextBox>
                                        
                                        <br/>
                                        <label class="col-form-label" for="message-text">
                                        <font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Fecha Hasta</font></font></label>
                                        <asp:TextBox ID="txtVenxfec2" runat="server" Font-Size="Small" TextMode="Date"></asp:TextBox>
                                       
                                        <br />
                                    
                                    </div>
                                    <div class="form-group">
                                      
                      <div class="input-group input-group-sm">
                            <div class="input-group-prepend">
                                <label class="input-group-text" style="width: 80px">Lider</label>
                            </div>
                            <asp:DropDownList ID="dlVentasporFecha"  ToolTip="Lider" placeholder="Lider" runat="server" CssClass="form-control form-control-sm" Height="30px" Font-Size="Small" Width="150px">
                         </asp:DropDownList>
                                </div>
                                        <br />
                        <div class="input-group input-group-sm">
                            <div class="input-group-prepend">
                                <label class="input-group-text" style="width: 80px">Vendedor</label>
                            </div>
                            <asp:TextBox ID="txtvendrxfec" runat="server" CssClass="form-control form-control-sm" placeholder="Codigo.." TextMode="SingleLine" />
                        </div>
                                             

                                       
                                           
                                    </div>
                                </div>
                            </div>
                            <!-- Modal footer -->
                            <div class="modal-footer">
                                <asp:LinkButton id="btnBusvenxfecha" runat="server" class="btn btn-success" type="button">Buscar</asp:LinkButton>
                                <asp:LinkButton id="btnCan" class="btn btn-outline-danger" data-dismiss="modal" type="button" runat="server">Cancelar</asp:LinkButton>
                            
                            </div>
                        </div>
                    </div>
                </div>
            </div>
                    



        <%-- Inicio Modal Dias Laborados --%>
          <div class="container">
        <div class="modal" id="idModalDiasLabCob">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">

                                                            <!-- Modal Header -->
                                                            <div class="modal-header">
                                                                                <h4>Informe Dias Laborados Por Gestor</h4><button type="button" class="close btn btn-danger btn-lg" data-dismiss="modal" data-whatever="">&times;</button></div>
                                                            <!--Cuerpo del Modal-->
                                                            <div class="modal-body">
                                                                <div id="formNocobro">
                                                                        
                                                                    <table>
                                                                            
                                                                            <tr>
                                                                                <td>
                                                                                    <label for="message-text" class="col-form-label"><font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Fecha Desde</label> </td>
                                                                                <td>
                                                                                    <input id="DiasFecha1" runat="server" type="Date" /> 
                                                                                    </td>

                                                                            </tr>
                                                                        <tr>

                                                                                <td>
                                                                                    <label for="message-text" class="col-form-label"><font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Fecha Hasta</label> </td>
                                                                                    <td>
                                                                                    <input id="DiasFecha2" runat="server" type="Date" /> 
                                                                                    </td>

                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <label for="message-text" class="col-form-label"><font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Codigo Gestor</label> 
                                                                            </td>
                                                                        <td>
                                                                            <input id="idDiasCobrador" runat="server" type="text" />
                                                                                
                                                                        </td>
                                                                        <td>
                                                                            <asp:CheckBox id="CkeckDetalle" runat="server" ToolTip="Chek para ver reporte detallado por cobrador"/>
                                                                                
                                                                        </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="DropDownList1" ToolTip="Agrupado Por Lider" placeholder="Motivo" runat="server" CssClass="form-control form-control-sm" Height="29px" Font-Size="Small">
                                                                                    <asp:ListItem Value="COBRADOR">COBRADOR</asp:ListItem>
                                                                                    <asp:ListItem Value="VENDEDOR">VENDEDOR</asp:ListItem>
                                                                                       
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <label for="message-text" class="col-form-label">Por Lider:</label>
                                                                                &nbsp</td>
                                                                            <td>
                                                                                   <asp:DropDownList ID="DropdDiasLiderCob" DataSourceId="SqlDataLiderCobros"  DataTextField="Lider" DataValueField="codigo" ToolTip="Lider" placeholder="Lider" runat="server" CssClass="form-control form-control-sm" Height="27px" Font-Size="Small" Width="200px">                                                                                                      
                                                                     </asp:DropDownList>                                                              
                                                                            </td>                                                                                
                                                                        </tr>                                                                           
                                                                    </table>                                                                   
                                                            </div>

                                                            <!-- Modal footer -->
                                                            <div class="modal-footer">
                                                                <button id="idcancelarCc" type="button" class="btn btn-outline-danger" data-dismiss="modal">Cancelar</button>
                                                                <asp:LinkButton ID="btnBuscarDias" type="button" runat="server" class="btn btn-success">Aceptar</asp:LinkButton></div></div>

                                                        </div>

                                                    </div>

        </div>
                                        </div>
                                          
         <%-- Reporte Entregas por Fecha --%>                               
          <div class="container">
        <div class="modal" id="idEntregasXfecha">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">

                                                            <!-- Modal Header -->
                                                            <div class="modal-header">
                                                                                <h4>Reporte de Entregas X Fecha</h4><button type="button" class="close btn btn-danger btn-lg" data-dismiss="modal" data-whatever="">&times;</button></div>
                                                            <!--Cuerpo del Modal-->
                                                            <div class="modal-body">
                                                                <div id="">
                                                                        
                                                                    <table>
                                                                            
                                                                            <tr>
                                                                                <td>
                                                                                    <label for="message-text" class="col-form-label"><font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Fecha Desde</label> </td>
                                                                                <td>
                                                                                    <input id="fecha1Ent1" runat="server" type="Date" /> 
                                                                                    </td>

                                                                            </tr>
                                                                        <tr>

                                                                                <td>
                                                                                    <label for="message-text" class="col-form-label"><font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Fecha Hasta</label> </td>
                                                                                    <td>
                                                                                    <input id="fecha1Ent2" runat="server" type="Date" /> 
                                                                                    </td>

                                                                        </tr>
                                                                                                                                               
                                                                    </table>

                                                                   
                                                            </div>

                                                            <!-- Modal footer -->
                                                            <div class="modal-footer">
                                                                <button id="idcancelarcx" type="button" class="btn btn-outline-danger" data-dismiss="modal">Cancelar</button>
                                                                <asp:LinkButton ID="btnbuscarEntr1" type="button" runat="server" class="btn btn-success">Buscar</asp:LinkButton></div></div>

                                                        </div>

                                                    </div>

        </div>
                                        </div>
        <div class="container">
                                        <div class="modal" id="idmodalfirma">
                                            <div class="modal-dialog">
                                                <div class="modal-content">

                                                    <!-- Modal Header -->
                                                    <div class="modal-header align-content-center">
                                                        <h4 class="align-content-center" >Firma de Clientes</h4>
                                                        <button type="button" class="close btn btn-danger btn-lg" data-dismiss="modal" >&times;</button>
                                                    </div>
                                                    <!--Cuerpo del Modal-->               
                                               <div class="modal-body" >
                                            <div>
                                              <div class="form-group">
                                                <label for="message-text" class="col-form-label"><font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Codigo Cliente:</font></font></label>
                                                <input id="CodClienteapp" type="text" iclass="form-control" runat="server" />
                                                 </div>
                                              <div class="form-group">
                                                <label for="message-text" class="col-form-label"><font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Codigo de Vendedor:</font></font></label>
                                                <input id="CodVendedor" type="text"  iclass="form-control"  maxlength="5" runat="server" onkeyup="ActivarBoton()"/>
                                              </div>
                                            </div>
                                          </div>
                                                <!-- Modal footer -->
                                                <div class="modal-footer">
                                            <asp:LinkButton Id="LinkButton2" runat="server" type="button" class="btn btn-outline-danger" data-dismiss="modal">Cancelar</asp:LinkButton>
                                            <asp:LinkButton Id="IdBuscarFirma"  type="button" runat="server" class="btn btn-outline-success" >Buscar</asp:LinkButton>
                                          </div>

                                            </div>
                                        </div>
                                    </div>
            </div>

        
        <%-- Reporte De Cartera Cobrador --%>

                <div class="container">
        
                <div id="idmodalCobrCarteraCliente" class="modal">
                    <div class="modal-dialog modal-sm">
                        <div class="modal-content">
                            <!-- Modal Header -->
                            <div class="modal-header align-content-center">
                                <h4 class="align-content-center">Cartera Detalladas Por Cobrador</h4>
                                <button class="close btn btn-danger btn-lg" data-dismiss="modal" data-whatever="" type="button">
                                                                            
                                </button>
                            </div>
                            <!--Cuerpo del Modal-->
                            <div class="modal-body">
                                <div id="">
                                  <%--  <div class="form-group">
                                        <label class="col-form-label" for="message-text">
                                        <font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Fecha desde</font></font></label>
                                        <asp:TextBox ID="TextBox1" runat="server" Font-Size="Small" TextMode="Date"> </asp:TextBox>
                                        
                                        <br/>
                                        <label class="col-form-label" for="message-text">
                                        <font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Fecha Hasta</font></font></label>
                                        <asp:TextBox ID="TextBox2" runat="server" Font-Size="Small" TextMode="Date"></asp:TextBox>
                                       
                                        <br />
                                    
                                    </div>--%>
                                    <div class="form-group">
                                      
                      <div class="input-group input-group-sm">
                            <div class="input-group-prepend">
                                <label class="input-group-text" style="width: 80px">Lider</label>
                            </div>
                            <asp:DropDownList ID="DropDownList2"  ToolTip="Lider" placeholder="Lider" runat="server" CssClass="form-control form-control-sm" Height="30px" Font-Size="Small" Width="150px">
                         </asp:DropDownList>
                                </div>
                                        <br />
                        <div class="input-group input-group-sm">
                            <div class="input-group-prepend">
                                <label class="input-group-text" style="width: 80px">Cobrador</label>
                            </div>
                            <asp:TextBox ID="txtCodCobrCarteCl" runat="server" CssClass="form-control form-control-sm" placeholder="Codigo.." TextMode="SingleLine" />
                        </div>
                                             

                                       
                                           
                                    </div>
                                </div>
                            </div>
                            <!-- Modal footer -->
                            <div class="modal-footer">
                                <asp:LinkButton id="btnBuscarCarCobrCl" runat="server" class="btn btn-success" type="button">Buscar</asp:LinkButton>
                                <asp:LinkButton id="LinkButton6" class="btn btn-outline-danger" data-dismiss="modal" type="button" runat="server">Cancelar</asp:LinkButton>
                            
                            </div>
                        </div>
                    </div>
                </div>
            </div>
                    
           <%-- Reporte De Cartera Cobrador --%>

                <div class="container">
        
                <div id="idmodalCobroResumidoXfechxCobr" class="modal">
                    <div class="modal-dialog modal-sm">
                        <div class="modal-content">
                            <!-- Modal Header -->
                            <div class="modal-header align-content-center">
                                <h4 class="align-content-center">Cobros Resumidos x Dia</h4>
                                <button class="close btn btn-danger btn-lg" data-dismiss="modal" data-whatever="" type="button">
                                                                            
                                </button>
                            </div>
                            <!--Cuerpo del Modal-->
                            <div class="modal-body">
                                 <div id="">
                                                                        
                                        <table>
                                                                            
                                                <tr>
                                                    <td>
                                                        <label for="message-text" class="col-form-label"><font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Fecha Desde</label> </td>
                                                    <td>
                                                        <input id="txtFecha1ResumCobro" runat="server" type="Date" /> 
                                                        </td>

                                                </tr>
                                            <tr>

                                                    <td>
                                                        <label for="message-text" class="col-form-label"><font style="vertical-align: inherit;"><font style="vertical-align: inherit;">Fecha Hasta</label> </td>
                                                        <td>
                                                        <input id="txtFecha2ResumCobro" runat="server" type="Date" /> 
                                                        </td>

                                            </tr>
                             <tr>
                                  <td>
                            <asp:TextBox ID="TxtCobroResumCobrador" runat="server" CssClass="form-control form-control-sm" placeholder="Codigo.." TextMode="SingleLine" />
                        </td></tr>                          
                                        </table>
                                                                   
                                </div>
                            </div>
                            <!-- Modal footer -->
                            <div class="modal-footer">
                                <asp:LinkButton id="LinkButton5" runat="server" class="btn btn-success" type="button">Buscar</asp:LinkButton>
                                <asp:LinkButton id="LinkButton7" class="btn btn-outline-danger" data-dismiss="modal" type="button" runat="server">Cancelar</asp:LinkButton>
                            
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        
        <div style="width:100%; height:40px; font-size:smaller; position:fixed; bottom:0px; background-color:#202020; left: 0px; color: white;"><div style="padding-top:10px;padding-left:10px;">Soporte &nbsp&nbsp<i class="far fa-envelope"></i>&nbsp<a id="Mailito" href="mailto:marco.mejia@amoreternohn.com">marco.mejia@amoreternohn.com</a>&nbsp&nbsp&nbsp<i class="fab fa-whatsapp"></i><a target="_blank" href="https://api.whatsapp.com/send?phone=50433251916&text=Hola,%20tendo%20una%20consulta?">+504 3325-1916</a></div></div>
        <script src="js/JQuery.js"></script>
        <script src="js/popper.min.js"></script>
        <script src="js/bootstrap.min.js"></script>        
        <script type="text/javascript">
            $('#myModal').on('shown.bs.modal', function () {
                $('#myInput').trigger('focus')
            })
        </script>


    </form>
</body>
</html>

