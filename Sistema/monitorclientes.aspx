<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="monitorclientes.aspx.vb" Inherits="Sistema.monitorclientes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="viewport" content="initial-scale=1.0, user-scalable=yes"/>
    <title>Clientes</title>
    <link rel="shortcut icon" type="image/x-icon" href="~/imagenes/logo.ico" />
    <link href="css/BootStrap.min.css" rel="stylesheet" type="text/css"/> 
    <link href="css/DataGrid.css" rel="stylesheet" type="text/css"/>   
    
           <style>

               .Principal{
            background-color:rgba(0,0,0,0.6); position:absolute; left: 0; top: 0; bottom: 0
        }
        .Principal-2{
            padding-left: 35%; padding-right: 35%; padding-top: 55px;
        }
        .Principal-3{
            background-color: white; padding-top: 15px;
        }
        .Principal-4{
            background-color: white; padding-top: 15px; padding-left: 20%;
        }
        .Principal-BotonCerrar{
            position: absolute; right: 33%; top: 35px; font-size: x-large;
        }

        @media (max-width: 768px){
            .Principal{
            background-color:rgba(0,0,0,0.6); position:absolute; left: 0; top: 0; bottom: 0
        }
        .Principal-2{
            padding-left: 5%; padding-right: 5%; padding-top: 25px;
        }
        .Principal-3{
            background-color: white; padding-top: 15px;
        }
        .Principal-4{
            background-color: white; padding-top: 15px; padding-left: 20%;
        }
        .Principal-BotonCerrar{
            position: absolute; right: 1%; top: 2px; font-size: x-large;
        }
        }
h4:hover {
    background-color: aliceblue;
    color: blue;
 
}

.Boton{
    width:80%;


}

/*  .boton{
        font-size:10px;
        font-family:Verdana,Helvetica;
        font-weight:bold;
        color:white;
        background:#538833;
        border:0px;
        width:80px;
        height:19px;
        cursor:pointer;
       }
*/


</style>
</head>

<body>
    <meta name="viewport" content="initial-scale=1.0, user-scalable=yes"/>
    <form id="form1" runat="server">
         <div><h4 style="width:100%; text-align:center">Historial de Movimientos</h4></div>
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
                     </div><br/>
             <div class="row align-content-center">
                   <div class="col col-8 align-content-center">
                       <div  class="input-group input-group-lg alert-primary">
                       <label>Filtrar tambien Por</label>
                       </div>
                   </div>
               </div>   
             <div class="row">
                   <div class="col-sm-12 col-lg-3 col-md-6" >
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dvale" class="input-group-text" style="width: 110px">Beneficiario</label>
                              <%-- <asp:DropDownList ID="dlNumeracion" runat="server" CssClass="input-group-text" ToolTip="Serie de Empresa..." OnSelectedIndexChanged="dlNumeracion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                           </div>
                           <asp:TextBox CssClass="form-control form-control-sm" ID="txtBenef1" runat="server" Enabled="true" placeholder="Nombre Beneficiario   ..."  />
                       </div>
                   </div>
                     <div class="col-sm-12 col-lg-3 col-md-6">
                       <div class="input-group input-group-sm">
                           
                           <asp:TextBox CssClass="form-control form-control-sm" ID="txtbenef2" runat="server" Enabled="true" placeholder=" Apellido Beneficiario   ..." />
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
                 
         

                  
              <%--  </div>

           <asp:Table ID="Table1" runat="server" Style="margin-bottom:20px;">
                <asp:TableRow runat="server">                    
                    <asp:TableCell runat="server">Cliente:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="txtCliente" runat="server" placeholder="Buscar por" CssClass="form-control form-control-sm" Width="200" Height="24" Font-Size="Small"/></asp:TableCell>
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
            </asp:Table>--%>
            <asp:GridView ID="gvClientes" runat="server" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4">
                <Columns>
                    
                    <asp:ButtonField Text="Detalle" CommandName="Detalle" />
                </Columns><FooterStyle BackColor="#CCCCCC" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" /><PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <RowStyle BackColor="White" ForeColor="#003399" />
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" /><SortedAscendingCellStyle BackColor="#F1F1F1" /><SortedAscendingHeaderStyle BackColor="#808080" /><SortedDescendingCellStyle BackColor="#CAC9C9" /><SortedDescendingHeaderStyle BackColor="#383838" /></asp:GridView>
            <br /><br /><asp:ImageButton ID="btnSalir" runat="server" Height="30px" ImageUrl="~/imagenes/atras.png" Width="30px" style="position:fixed; right:10px; top:10px;"/>
        <asp:Label ID="lblMsg" runat="server"></asp:Label></div><div style="width:100%; height:30px;  position:fixed; bottom:0px; background-color:#202020; left: 0px; color: #fff">
            <asp:Label ID="lblTotal" runat="server" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblClientes" runat="server" Text="" />&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblConVisitas" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblSinVisitas" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblRendimientoVisitas" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="LblConCobro" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblSinCobro" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblRendimientoCobro" runat="server" Text="" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblHora" runat="server" Text="" ForeColor="White"/>
    </div>

                <%-- Panel de Confirmacion --%>
         <div class="container-fluid Principal" id="PanelClienteMovimiento" runat="server" visible="false">
               <div class="Principal-2">
                   <div class="row Principal-3">
                       <div class="col">
                           <div class="input-group-prepend align-content-center" style="text-align: left"">
                               
                           <label class="alert-primary align-content-center" id="LblClienteModal" runat="server"></label>
                            </div>
                       </div>
                    </div>
                                   
                   <div class="row Principal-4">
                       <div class="col">
                           <div class="col-8" style="text-align: center">
                               <asp:Button ID="BtnEstadoDCuent" runat="server" Text="   Estado de Cuenta   "  CssClass="btn btn-sm btn-success Botones" />&nbsp&nbsp
                               
                           </div>
                       </div>
                   <%--<asp:ButtonField Text="VerBitacora" CommandName="VerBitacora" ControlStyle-CssClass="fas fa-address-book" />--%>
                   </div>

                   <div class="row Principal-4">
                          <div class="col">
                           <div class="col-8" style="text-align: center">
                               <asp:Button ID="BtnBitacora" runat="server" Text="   Ver Bitacora   " CssClass="btn btn-sm btn-success Botones" />&nbsp&nbsp                             
                           </div>
                       </div>
                   </div>
                 <div class="row Principal-4" >
                          <div class="col">
                           <div class="col-8" style="text-align: center">
                               <asp:Button ID="BtnHistorial" runat="server" Text="   Historial Visitas   " CssClass="btn btn-sm btn-success Botones" />&nbsp&nbsp                             
                           </div>
                       </div>
                   </div>
                 <div class="row Principal-4" >
                          <div class="col">
                           <div class="col-8" style="text-align: center">
                               <asp:Button ID="btnTipoIngresoCl" runat="server" Text="   Tipos de Ingreso   " CssClass="btn btn-sm btn-success Botones" />&nbsp&nbsp                             
                           </div>
                       </div>
                   </div>
                      <div class="row Principal-4" >
                          <div class="col">
                           <div class="col-8" style="text-align: center">
                               <asp:Button ID="btnRecibos" runat="server" Text="   Recibos   " CssClass="btn btn-sm btn-success Botones" />&nbsp&nbsp                             
                           </div>
                       </div>
                   </div>
                    <div class="row Principal-4" >
                       <div class="col">
                           <div class="col-8" style="text-align: center">
                               <asp:Button ID="BtnCrearEvent" runat="server" Text="  Crear Evento   " CssClass="btn btn-sm btn-success Botones" />&nbsp&nbsp
                               
                           </div>
                       </div>
                       <br />
                   
                   </div>

                     <div class="row Principal-4" >
                       <div class="col">
                           <div class="col-8" style="text-align: center">
                               <asp:Button ID="btnFinalizarEvento" runat="server" Enabled="false" Text="  Finalizar Evento   " CssClass="btn btn-sm btn-success Botones" />&nbsp&nbsp
                               
                           </div>
                       </div>
                       <br />
                   
                   </div>

                          <asp:LinkButton ID="btnCerrarVentana" runat="server" CssClass="Principal-BotonCerrar">X<i class="far fa-times-circle text-secondary"></i></asp:LinkButton>
               </div>
           </div>

                <%-- Panel de Confirmacion --%>
     <%--    <div class="container-fluid Principal" id="Div1" runat="server" visible="false">
               <div class="Principal-2">
                   <div class="row Principal-3" >
                       <div class="col">
                           <div class="input-group-prepend align-content-center" style="text-align: center"">
                               
                           <label class="alert-primary align-content-center" id="Label1" runat="server"></label>
                            </div>
                       </div>
                    </div>
                   
                   <div class="row Principal-4" id="idPantalla" runat="server">
                       <div class="col">
                           <div class="col-8" style="text-align: center">
                               <asp:Button ID="btnContrato" runat="server" Text="   Contrato   "  CssClass="btn btn-sm btn-success Botones"/>&nbsp&nbsp
                               
                           </div>
                       </div>
                   
                   </div>
                   
                   <div class="row Principal-4" >
                       <div class="col">
                           <div class="col-8" style="text-align: center">
                               <asp:Button ID="Button1" runat="server" Text="   Estado de Cuenta   "  CssClass="btn btn-sm btn-success Botones" />&nbsp&nbsp
                               
                           </div>
                       </div>
                       <br />
                   
                   </div>
                        <div class="row Principal-4" >
                       <div class="col">
                           <div class="col-8" style="text-align: center">
                               <asp:Button ID="BtnCrearEvent" runat="server" Text="  Crear Evento   " Enabled="false" CssClass="btn btn-sm btn-success Botones" />&nbsp&nbsp
                               
                           </div>
                       </div>
                       <br />
                   
                   </div>
         
                          <asp:LinkButton ID="BtnCerrarEvento" runat="server" CssClass="Principal-BotonCerrar">X<i class="fas fa-times-circle text-secondary"></i></asp:LinkButton>
               </div>
             
           </div>--%>
 
         <div class="container-fluid Principal" id="PanelCrearEvento" runat="server" visible="false">
               <div class="Principal-2">
                   <div class="row Principal-3" >
                       <div class="col">
                           <div class="input-group-prepend align-content-center" style="text-align: center"">
                               
                           <label class="alert-primary align-content-center" id="idCliente" runat="server">Crear Evento para Nota de Duelo</label>
                            </div>
                       </div>
                    </div>
                   
                   <div class="row Principal-3" id="div2" runat="server">
                      <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="txtVencimiento" class="input-group-text" style="width: 120px">Nombre Difunto</label>
                           </div>
                           <input type="text" class="form-control form-control-sm" required="required" id="txtdifunto" runat="server" />
                       </div>
                   </div>
                  
                   </div>
                   <div class="row Principal-3" id="div3" runat="server">
                      <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="txtVencimiento" class="input-group-text" style="width: 120px">LugarVelacion</label>
                           </div>
                           <asp:DropDownList ID="dlsalas" runat="server" OnTextChanged="dlsalas_TextChanged" AutoPostBack="true"></asp:DropDownList>
                       </div>
                   </div>
                  
                   </div>
                   
                    <div class="row Principal-3" >
                       <div class="col">
                     
                                 <div class="input-group input-group-sm" >
                       <asp:TextBox ID="txtSalaDetalle" TextMode="MultiLine" Visible="false" runat="server" ToolTip="Especificar Sala de Velacion" CssClass="form-control form-control-sm"></asp:TextBox>

                           
                       </div>
                   </div>
                    
                 
                   </div>
                   <div class="row Principal-3" >
                       <div class="col">                    
                                 <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="txtDireccionSepelio" class="input-group-text" style="width: 125px">Direccion Velacion</label>
                           </div>
                             <p class="list-group-item list-group-item-action bg-light"><i class="far fa-comment-alt" style="font-size:large; text-align:center">
                         </i>&nbsp Direccion<asp:TextBox ID="txtDirecVel" TextMode="MultiLine"  runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </p>
                       </div>
                   </div>
                    
                 
                   </div>
                   <div class="row Principal-3">
                       <div class="col">
                           <div class="input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <label for="txtFechaEntrega" class="input-group-text" style="width: 120px">Fecha Velacion</label>
                               </div>
                               <asp:TextBox TextMode="date" class="form-control form-control-sm" ID="txtfechaVelacion" runat="server"> </asp:TextBox>
                               <div class="input-group-prepend alert-info">
                                   <label class="input-group-text" style="width: 120px">HoraVelacion.</label>
                               </div>
                               <input type="time" runat="server" id="txtHoraVela" name="hora" min="18:00" max="21:00" step="3600" />
                           </div>
                       </div>
                   </div>
                   <div class="row Principal-3">
                       <div class="col">
                           <div class="input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <label for="txttelefono" class="input-group-text" style="width: 115px">Tel.Contacto</label>
                               </div>
                               <input type="text" class="form-control form-control-sm" id="txtcontacto" runat="server" />

                           </div>
                       </div>
                       <div class="col">
                           <div class="input-group input-group-sm">

                               <div class="input-group-prepend">
                                   <label for="txtDireccionSepelio" class="input-group-text" style="width: 200px">Sucursal</label>
                               </div>
                               <asp:DropDownList ID="dlsucursal" class="input-group-text-sm" runat="server">
                                                                     
                               </asp:DropDownList>
                           </div>

                       </div>


                   </div>
                   <div class="row Principal-3" id="div4" runat="server">
                       <div class="col">
                           <div class="input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <label for="" class="alert-primary" style="width: 120px">Datos de Sepelio</label>
                               </div>

                           </div>
                       </div>

                   </div>
                   <div class="row Principal-3" id="div3Sep" runat="server" visible="false">
                       <div class="col">
                           <div class="input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <label for="txtFechaEntrega" class="input-group-text" style="width: 120px">Fecha Sepelio</label>
                               </div>
                               <asp:TextBox TextMode="date" class="form-control form-control-sm" ID="txtfechaSep" runat="server"> </asp:TextBox>
                               <div class="input-group-prepend alert-info">
                                   <label class="input-group-text" style="width: 120px">HoraVelacion.</label>
                               </div>
                               <input type="time" runat="server" id="txthorasep" name="hora" min="18:00" max="21:00" step="3600" />
                           </div>


                       </div>
                   </div>
                 
                         <div class="row Principal-3" >
                       <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="txtDireccionSepelio" class="input-group-text" style="width: 125px">Direccion Sepelio</label>
                           </div>
                             <p class="list-group-item list-group-item-action bg-light"><i class="far fa-comment-alt" style="font-size:large; text-align:center">

                         </i>&nbsp Direccion<asp:TextBox ID="txtdireccSep" TextMode="MultiLine"  runat="server" CssClass="form-control form-control-sm"></asp:TextBox>

                            </p>
                       </div>
                   </div>
                       
                 
                   </div>
                   <div class="row Principal-3">
                       
                            <div class="col card">
                           <asp:LinkButton ID="BtnAgregarND" runat="server"><i class=" btn fa fa-plus text-success" style="font-size:x-large;padding-left:2px;padding-right:2px">Guardar</i></asp:LinkButton>
                               
                       </div>  
                       <div class="col card">
                            <asp:LinkButton ID="btnReimprimir" runat="server" Visible="false"><i class=" btn fa fa-plus text-success" style="font-size:x-large;padding-left:2px;padding-right:2px">Reimprimir</i></asp:LinkButton>
                       </div>
                      
                   </div>
 <asp:LinkButton ID="btnCerrarEvento" runat="server" Visible="false" CssClass="Principal-BotonCerrar">X<i class="fas fa-times-circle text-secondary"></i></asp:LinkButton>
              </div>  
             </div>
        <%-- Llenar datos de nota de duelo --%>
        <div class="container-fluid Principal" id="ConfirmarNotaDuelo" runat="server" visible="false">
            <div class="Principal-2">
                <div class="row Principal-3">
                    <div class="col-8">
                           <div class="input-group-prepend align-content-center" style="text-align: left"">
                               
                           <label class="alert-primary align-content-center">Está seguro Que crear Nota de Duelo?</label>
                            </div>
                       </div>
                </div>
                <div class="row Principal-3">
                <div class="col-8">
                           <div class="col" style="text-align: right">
                               <asp:Button ID="BtnGuardarSi" runat="server" Text="   Salvar   " CssClass="btn btn-sm btn-success" />&nbsp&nbsp<asp:Button ID="BtnGuardarNo" runat="server" Text="Cancelar" CssClass="btn btn-sm btn-secondary" />
                           </div>
                       </div> 
                </div>
            </div>
        </div>
        <%-- Desea Crear documento de Nota de Duelo --%>
               <div class="container-fluid Principal" id="Div1" runat="server" visible="false">
            <div class="Principal-2">
                <div class="row Principal-3">
                    <div class="col-8">
                           <div class="input-group-prepend align-content-center" style="text-align: left"">
                               
                           <label class="alert-primary align-content-center">Está seguro Que crear Nota de Duelo?</label>
                            </div>
                       </div>
                </div>
                <div class="row Principal-3">
                <div class="col-8">
                           <div class="col" style="text-align: right">
                               <asp:Button ID="Button1" runat="server" Text="   Salvar   " CssClass="btn btn-sm btn-success" />&nbsp&nbsp<asp:Button ID="Button2" runat="server" Text="Cancelar" CssClass="btn btn-sm btn-secondary" />
                           </div>
                       </div> 
                </div>
            </div>
        </div>

        <%-- Finalizar Evento --%>
          <div class="container-fluid Principal" id="DivFinalizarEvento" runat="server" visible="false">
            <div class="Principal-2">
                <div class="row Principal-3">
                    <div class="col-8">
                           <div class="input-group-prepend align-content-center" style="text-align: left"">
                               
                           <label class="alert-primary align-content-center">Está seguro que el servicio al cliente Finalizó?</label>
                            </div>
                       </div>
                </div>
                <div class="row Principal-3">
                <div class="col-8">
                           <div class="col" style="text-align: right">
                               <asp:Button ID="btnSiFinalizarEvento" runat="server" Text="   Si   " CssClass="btn btn-sm btn-success" />&nbsp&nbsp<asp:Button ID="btnNoFinalizar" runat="server" Text="Cancelar" CssClass="btn btn-sm btn-secondary" />
                           </div>
                       </div> 
                </div>
               <%-- <asp:LinkButton ID="btnCerrarFinalizar" runat="server" Visible="false" CssClass="Principal-BotonCerrar">X<i class="fas fa-times-circle text-secondary"></i></asp:LinkButton>--%>
            </div>
        </div>

                <%-- Panel Envio de Estado de Cuenta Por whatsapp --%>
        <div class="container-fluid Principal" id="PanelEnviarWhatsapp" runat="server" visible="false">
            <div class="Principal-2">
                <div class="row Principal-3">

                        
                       <div class="col">
                           <div class="input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <label for="txttelefono" class="input-group-text" style="width: 135px">Telefono Whatsapp</label> <i class="fas fa-solid fa-whatsapp" style="color: #49e44c;"></i>
                               </div>
                               <%--<input type="text" class="form-control form-control-sm" id="TxtTelefonoWhatsapp" runat="server" />&nbsp--%>
                               <asp:TextBox class="form-control form-control-sm" TextMode="Number" ID="TxtTelefonoWhats" runat="server"></asp:TextBox>

                         
                           <asp:Button ID="btnEnviarWhatsapp" runat="server" Text="   Enviar por WhatsApp  "  CssClass="btn btn-sm btn-success" />
                       </div>
                     </div>
                     
                </div>
                 <div class="row Principal-3">                      
                       <div class="col">
                           <div class="input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <label for="txttelefono" class="input-group-text" style="width: 135px">Correo / Email</label> <i class="fas fa-solid fa-whatsapp" style="color: #49e44c;"></i>
                               </div>
                               <%--<input type="text" class="form-control form-control-sm" id="txtCorreoCliente" runat="server" />--%>
                               <asp:TextBox class="form-control form-control-sm" TextMode="Email" ID="TxtCorreoCliente" runat="server"></asp:TextBox>

                           </div>
                       </div>
                     
                </div>
          
 
                  <div class="row Principal-3">
                 
                </div>

                  <div class="row Principal-3">
                      <div class="col">

                      </div>
                        <div class="col">
                           <div class="input-group input-group-sm">                             
                                <asp:Button ID="btnEnviarPorCorreo" runat="server" Text="  Enviar Por Correo  " CssClass="btn btn-sm btn-success" />
                           </div>
                       </div>
                </div>

                <div class="row Principal-3">
                <div class="col-8">
                           <div class="col" style="text-align: right"><i class="fa-brands fa-whatsapp" style="color: #49e44c;"></i>
                               <asp:Button ID="btnImprimirPdf" runat="server" Text="   Descargar PDF  "  CssClass="btn btn-sm btn-success" />&nbsp&nbsp
                               <asp:Button ID="btnCerrarDivWhatsapp" runat="server" Text="Cancelar" CssClass="btn btn-sm btn-secondary" />
                           </div>
                       </div> 
                </div>
            </div>
        </div>

        <%-- Modal de Confirmacion --%>

              <div class="container-fluid Principal" id="PanelConfirmaCorreoEnviado" runat="server" visible="false">
            <div class="Principal-2">
                <div class="row Principal-3">
                    <div class="col-8">
                           <div class="input-group-prepend align-content-center" style="text-align: left"">
                               
                           <label class="alert-primary align-content-center">Correo Enviado Correctamente</label>
                            </div>
                       </div>
                </div>
                <div class="row Principal-3">
                <div class="col-8">
                           <div class="col" style="text-align: right">
                               <asp:Button ID="btnCerrarConfCorreo" runat="server" Text="   OK   " CssClass="btn btn-sm btn-success" />
                           </div>
                       </div> 
                </div>
               <%-- <asp:LinkButton ID="btnCerrarFinalizar" runat="server" Visible="false" CssClass="Principal-BotonCerrar">X<i class="fas fa-times-circle text-secondary"></i></asp:LinkButton>--%>
            </div>
        </div>


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