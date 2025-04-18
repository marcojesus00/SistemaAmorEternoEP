﻿<%@ Page Title="Detalle cartera de cobrador" Language="vb" AutoEventWireup="false" MasterPageFile="~/Dashboards/Dashboard.master" CodeBehind="DetalleCarteraDeCobrador.aspx.vb" Inherits="Sistema.DetalleCarteraDeCobrador" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Filters" runat="server">



        <h2 class=" text-center">Detalle de cartera de cobrador</h2>
    <div class="container-fluid container-lg mt-1 mb-1">
        <div class="border border-dark-subtle p-2">
            <div class="row">
                <div class="card">
                    <div class="card-header text-center">
                        <div class="card-title"><%= Session("NombreCobradorSeleccionado") %></div>
                    </div>
                </div>

          
                <div class="col-lg-3  pt-1">
    
                      <asp:LinkButton  ID="WhatsAppToAll"  OnClick="WhatsAppToAll_Click" ToolTip="Teléfonos de los clienes según su validez" CssClass="btn btn-outline-primary" runat="server">
<i class="bi bi-whatsapp"></i> Envío estados
                            </asp:LinkButton>
                  
                </div>
                                               <div class="col-lg-3  pt-1">
    
               
                                    <asp:LinkButton  ID="BtnMonitor"  OnClick="Monitor_click" ToolTip="Monitoree los estados enviados" CssClass="btn btn-primary" runat="server">
<i class="bi bi-eye-fill"></i> Monitor de mensajes
                            </asp:LinkButton>
                </div>
                                                       <div class="col-lg-1 col-1">

      
                        </div>

            </div>
        </div>
    </div>

</asp:Content>








<asp:Content ID="Content2" ContentPlaceHolderID="Table" runat="server" >
                    <div class="container-fluid container-lg">

    <asp:Panel ID="PnlGoodAndBadPhones" runat="server" >
        <div class="card">
            <div class="card-header  text-end">

                                      <asp:LinkButton  ID="btnExitWhatsapToAll"  OnClick="btnExitWhatsapToAll_Click" ToolTip="Cerrar" CssClass="btn btn-sm btn-outline-danger" runat="server">
<i class="bi bi-x-circle"></i> Cerrar
                            </asp:LinkButton>
            </div>
            <div class="card-body">
                <div class="card-title">
                        <asp:Literal ID="CardTitleLiteral" runat="server"></asp:Literal>

                </div>
                <div class="table table-responsive">
                                        <asp:GridView ID="SendGridview" runat="server" DataKeyNames="Codigo" CssClass="table  table-sm table-hover" AutoGenerateColumns="True" AllowPaging="true" OnPageIndexChanging="SendGridview_PageIndexChanging" PageSize="8" EmptyDataText="No se encontraron resultados" >

                                        </asp:GridView>
                </div>

                           <div class="list-group-item-action">
                      <asp:LinkButton  ID="btnCorrupPhones"  OnClick="btnCorrupPhones_Click" ToolTip="" CssClass="btn btn-sm btn-outline-danger" runat="server">
<i class="bi bi-exclamation-circle"></i> Ver clientes con telefono malo
                            </asp:LinkButton>

                                     <asp:LinkButton  ID="btnSendMassiveWhatsApp"  OnClick="btnSendMassiveWhatsApp_Click"     OnClientClick="if (this.innerText === 'Procesando...') return false; this.innerText = 'Procesando...'; return true;" 
 ToolTip="Enviar WhatsApp a todos" CssClass="btn btn-sm btn-outline-success" runat="server">
<i class="bi bi-whatsapp"></i> Enviar WhatsApp a todos
                            </asp:LinkButton>
                       
                               <asp:DropDownList CssClass="" ID="DdlIntance" runat="server">
                                    <asp:ListItem Text="Automático" Value="0"></asp:ListItem>
                                      <asp:ListItem Text="Teléfono de Saul" Value="1"></asp:ListItem>
                                      <asp:ListItem Text="Teléfono de Nora" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Teléfono de Abdul" Value="3"></asp:ListItem>

                               </asp:DropDownList>
           </div>
            </div>

        </div>


    </asp:Panel>




                            <asp:Panel ID="PnlBasPhones" runat="server" >
        <div class="card">
            <div class="card-header  text-end">

                                      <asp:LinkButton  ID="LinkButton1"  OnClick="btnExitWhatsapToAll_Click" ToolTip="Cerrar" CssClass="btn btn-sm btn-outline-danger" runat="server">
<i class="bi bi-x-circle"></i> Cerrar
                            </asp:LinkButton>
            </div>
            <div class="card-body">
                <div class="card-title">
                        Teléfonos malos

                </div>
                <div class="table table-responsive">
                                        <asp:GridView ID="GridViewBadPhones" runat="server" DataKeyNames="Codigo" CssClass="table  table-sm table-hover" AutoGenerateColumns="True" AllowPaging="true"  OnPageIndexChanging="GridViewBadPhones_PageIndexChanging" PageSize="8" EmptyDataText="No se encontraron resultados" >

                                        </asp:GridView>
                </div>
                           <div class="list-group-item-action">


                                     <asp:LinkButton  ID="LinkButton3"  OnClick="WhatsAppToAll_Click" ToolTip="" CssClass="btn btn-sm btn-outline-primary" runat="server">
<i class="bi bi-whatsapp"></i> Teléfonos válidos
                            </asp:LinkButton>
                               

                                     <asp:LinkButton  ID="LinkButtonDownloadExcel"  OnClick="btnDownloadExcel_Click" ToolTip="" CssClass="btn btn-sm btn-outline-success" runat="server">
<i class="bi bi-file-excel"></i> Descargar excel
                            </asp:LinkButton>
                               
                                     <asp:LinkButton  ID="LinkButtonDownloadPDF"  OnClick="btnDownloadPDF_Click" ToolTip="" CssClass="btn btn-sm btn-outline-danger" runat="server">
<i class="bi bi-file-pdf"></i> Descargar PDF
                            </asp:LinkButton>

           </div>
            </div>

        </div>

    </asp:Panel>




    <asp:panel runat="server" id="PnlPrimary">
   

            <div class="table-responsive">
                    <asp:GridView ID="DashboardGridview" runat="server" DataKeyNames="Codigo" CssClass="table  table-sm table-hover" AutoGenerateColumns="True" EmptyDataText="No se encontraron resultados"  >
                        <Columns>

                        </Columns>
                    </asp:GridView>
                 <nav aria-label="Page navigation">
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-2">
                                    <asp:Label runat="server" ID="lblTotalCount" Text=' <%# "Total: " & TotalItems & itemText %>'></asp:Label>

                                </div>
                                <div class="col-10">
                                    <ul class="pagination justify-content-center">

                                        <li class="page-item" runat="server" id="PreviousPage">
                                            <asp:LinkButton ID="lnkbtnPrevious" runat="server" Text="&laquo; Previa" CssClass="page-link" OnClick="lnkbtnPrevious_Click" Enabled='<%# PageNumber > 1 %>'></asp:LinkButton>
                                        </li>

                                        <asp:Repeater ID="rptPager" runat="server">
                                            <ItemTemplate>
                                                <li class="page-item <%# pagination.IsActivePage(Container.DataItem, PageNumber) %>">
                                                    <asp:LinkButton ID="lnkbtnPage" runat="server" Text='<%# Container.DataItem %>' CssClass="page-link" OnClick="lnkbtnPage_Click"></asp:LinkButton>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                        <li class="page-item" runat="server" id="NextPage">
                                            <asp:LinkButton ID="lnkbtnNext" runat="server" Text="Siguiente &raquo;" CssClass="page-link" OnClick="lnkbtnNext_Click" Enabled='<%# PageNumber < TotalPages %>'></asp:LinkButton>
                                        </li>
                                    </ul>

                                </div>
                            </div>

                        </div>

                    </nav>
                </div>
    

    </asp:panel>

        </div>

        <div id="alertPlaceholder" runat="server"></div>

</asp:Content>

