<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RptVisitasClientes.aspx.vb" Inherits="Sistema.RptVisitasClientes" %>

<%--<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RepCxc.aspx.vb" Inherits="SERP_POS.RepCxc" EnableEventValidation="false" %>--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MonitorVisitas</title>
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

        .Scroll{
             overflow-y: auto; 
        overflow-x:auto;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

   <div class="d-flex" id="wrapper">

    <!-- Page Content -->
       <div id="page-content-wrapper">

           <nav class="navbar navbar-expand-lg navbar-light bg-light border-bottom">              
               <h4 style="width:100%; text-align:center">Monitor Visitas</h4>
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

           <div class="container-fluid" id="PanelReimprimir" runat="server">
               <div style="padding-left: 5%; padding-right: 5%;">
               <%--    <div class="row" style="background-color: white; padding-top: 15px;">
                       <div class="col-8">
                           <div class="input-group input-group-sm">
                               <asp:TextBox ID="txtBuscar" runat="server" class="form-control form-control-sm" placeholder="Cliente..."  />
                               <asp:DropDownList ID="dlVendedor" runat="server" CssClass="form-control form-control-sm" ></asp:DropDownList>                               
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
                    <div class="row">
                  <%-- <div class="col-4">
                       <input type="text" class="form-control form-control-sm" placeholder="Nombre" id="Text1" maxlength="50" runat="server" />
                   </div>--%>
                   <div class="col-lg-4 col-sm-12 col-xlg-3">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="txtVencimiento" class="input-group-text" style="width: 95px">Fecha Desde</label>
                           </div>
                           <input type="date" class="form-control form-control-sm" id="txtfecha1" runat="server" />
                       </div>
                   </div>
                 <%--  <div class="col-4">    
                   </div>     --%>  
               </div>
                    <div class="row">
                 <%--  <div class="col-4">
                       <input type="text" class="form-control form-control-sm" placeholder="Nombre" id="Text2" maxlength="50" runat="server" />
                   </div>--%>
                   <div class="col-lg-4 col-sm-12 col-xlg-3">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="txtVencimiento" class="input-group-text" style="width: 95px">Fecha Hasta</label>
                           </div>
                           <input type="date" class="form-control form-control-sm" id="txtfecha2" runat="server" />
                       </div>
                   </div>
                <%--   <div class="col-4">    
                   </div>  --%>     
               </div>
              <div class="row">
                  <%-- <div class="col-4">
                       <input type="text" class="form-control form-control-sm" placeholder="Nombre" id="txtNombre" maxlength="50" runat="server" />
                   </div>--%>
                   <div class="col-lg-4 col-sm-12 col-xlg-3">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="txtVencimiento" class="input-group-text" style="width: 95px">Cobrador</label>
                           </div>
                           <input type="text" class="form-control form-control-sm" id="txtCobrador" runat="server" />
                       </div>
                   </div>
                 <%--  <div class="col-4">    
                   </div>  --%>     
               </div>

                   <div class="row">
                       <div class="col-lg-4 col-sm-12 col-xlg-3">
                          <div class="input-group-append">
                                   <label class="input-group-text">
                                       <asp:LinkButton ID="btnBuscar" runat="server" class="fas fa-search text-primary">Buscar</asp:LinkButton></label>
                               </div>
                   </div> </div>


                   <div class="row Scroll" style="background-color: white">
                       <div class="col">
                           <asp:GridView ID="GvPrincipal" runat="server" RowStyle-HorizontalAlign="Right" CssClass="table table-sm table-hover table-bordered" EmptyDataText="No se econtraron resultados" AllowSorting="True" AutoGenerateColumns="False">                             
                               <Columns>
                                   <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                       <ItemTemplate>
                                           <img alt="" src="svgs/solid/angle-down.svg" width="15" style="padding: 0px;" />
                                           <asp:Panel ID="pnlOrders" runat="server" Style="display: none; padding: 0px;">
                                               <asp:GridView ID="GVDetalle" runat="server" RowStyle-HorizontalAlign="Right" AutoGenerateColumns="false" CssClass="table-bordered table-info">
                                                   <Columns>
                                                       <asp:BoundField DataField="Codigo" HeaderText="Codigo" />
                                                       <asp:BoundField DataField="Nombre_clie" HeaderText="Nombre_clie" />
                                                       <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                                       <asp:BoundField DataField="Hora" HeaderText="Hora" />
                                                       <asp:BoundField DataField="Cobrado" HeaderText="Cobrado" />
                                                       <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                                       <asp:BoundField DataField="Zona" HeaderText="Zona" />
                                                   </Columns>
                                               </asp:GridView>
                                           </asp:Panel>
                                       </ItemTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                   </asp:TemplateField>
                                   <asp:BoundField DataField="Codigo" HeaderText="Codigo" SortExpression="Codigo" ItemStyle-HorizontalAlign="Left" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                   </asp:BoundField>
                                   <asp:BoundField DataField="Nombre_clie" HeaderText="Nombre_clie" SortExpression="Nombre_clie" ItemStyle-HorizontalAlign="Left" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                   </asp:BoundField>
                                   <asp:BoundField DataField="UltimaFechaAsignado" HeaderText="UltimaFechaAsignado" SortExpression="UltimaFechaAsignado" />
                                   <asp:BoundField DataField="Cobrado" HeaderText="Cobrado" SortExpression="Cobrado" />
                                   <asp:BoundField DataField="HoraPrimerVisita" HeaderText="HoraPrimerVisita" SortExpression="HoraPrimerVisita"/>
                                   <asp:BoundField DataField="FechaPrimerVisita" HeaderText="FechaPrimerVisita" SortExpression="FechaPrimerVisita" DataFormatString="{0:MM/dd/yyyy}"/>
                                   <asp:BoundField DataField="HoraUltimaVisita" HeaderText="HoraUltimaVisita" SortExpression="HoraUltimaVisita" />
                                   <asp:BoundField DataField="UltimaFechaVisita" HeaderText="UltimaFechaVisita" SortExpression="UltimaFechaVisita" DataFormatString="{0:MM/dd/yyyy}" />
                                   <asp:BoundField DataField="Recibos" HeaderText="Recibos" SortExpression="Recibos" />
                                   <asp:BoundField DataField="NVisitas" HeaderText="NVisitas" SortExpression="NVisitas" />                                   
                                   <asp:BoundField DataField="Cobrador" HeaderText="Cobrador" SortExpression="Cobrador" />
                                   <asp:BoundField DataField="Zona" HeaderText="Zona" SortExpression="Zona" />
                                   <asp:BoundField DataField="Telefono" HeaderText="Telefono" SortExpression="Telefono" />
                                   <asp:BoundField DataField="CobradorAsignadoAct" HeaderText="CobradorAsignadoAct" SortExpression="CobradorAsignadoAct" />
                                   <asp:BoundField DataField="vendedor" HeaderText="Vendedor" SortExpression="Vendedor" />
                                   <asp:BoundField DataField="Saldo_actua" HeaderText="Saldo_Actua" SortExpression="Saldo" />
                               </Columns>

<RowStyle HorizontalAlign="Right"></RowStyle>
                           </asp:GridView>
                       </div>
                   </div>
               </div>
           </div>

           <div style="visibility: hidden">
            <asp:GridView ID="gvExcel" runat="server"></asp:GridView>
           </div>

           <div style="position:fixed; bottom:20px; padding-left:15px;" >               
               <asp:Label ID="lblMsg" runat="server"></asp:Label>
           </div>

           <div style="width:100%; height:40px; font-size:smaller; position:fixed; bottom:0px; background-color:#202020; left: 0px; color: white;"><div style="padding-top:5px;padding-left:10px;">
               <asp:Label ID="LblTotalClientesAsignados" style="font-size:medium" runat="server"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblRecibos" style="font-size:medium" runat="server"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblTotalCosto" style="font-size:medium" runat="server"/>&nbsp&nbsp&nbsp<asp:Label ID="lblTotalGanancia" runat="server"/></div></div>

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
