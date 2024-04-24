<%@ Control Language="VB" AutoEventWireup="true" CodeBehind="Sidebar.ascx.vb" Inherits="Sistema.Sidebar" %>
  
<div class="sidebar-container pl-3  text-light h-100">

    <div class="container-fluid d-flex flex-column h-100" style="padding-top:10px;">
    <div class="">
        <div id="accordion" class="shadow-sm">                    
            <div class="card">
                <div class="card-header  text-light">
                    <a class="collapsed card-link btn-block" data-toggle="collapse" href="#collapseTwo"><i class="fas fa-tags" style="font-size: 22px;"></i>&nbsp&nbsp Caja</a>
                </div>
                <div id="collapseTwo" class="collapse" data-parent="#accordion">
                    <div class="card-body  text-light">
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
            <div class="card  text-light">
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
            <div class="card  text-light">
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
            <div class="card  text-light">
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
            <div class="card  text-light">
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
                <div class="card  text-light">
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
            <div class="card  text-light" id="MenuProduccion" runat="server" visible="false">
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

            <div class="card  text-light">
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
            <div class="card  text-light">
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
              <div class="card  text-light">
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

            <div class="card  text-light">
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
       
              <div class="row flex-grow-1">
                <div class="col-sm-2 sidebar">
                 </div>
                <div class="col-sm-10 main-content">
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

        </div>
           </div>


    </div>

</div>