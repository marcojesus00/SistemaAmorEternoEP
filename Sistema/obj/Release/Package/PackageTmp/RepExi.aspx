<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RepExi.aspx.vb" Inherits="Sistema.RepExi" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Existencias</title>
 <!-- Bootstrap core CSS -->  
  <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
    <link rel="shortcut icon" type="image/x-icon" href="svgs/solid/drafting-compass.svg"/>

  <!-- Custom styles for this template -->
  <link href="css/simple-sidebar.css" rel="stylesheet"/>
  <link rel="stylesheet" href="css/all.min.css" />
    <style>
        .row {padding:1px;
        }  
           .Boton:hover{
            transform:scale(1.05);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

   <div class="d-flex" id="wrapper">



    <!-- Page Content -->
       <div id="page-content-wrapper">

           <nav class="navbar navbar-expand-lg navbar-light bg-light border-bottom">              
               <h4 style="width:100%; text-align:center">Existencias</h4>
               <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                   <span class="navbar-toggler-icon"></span>
               </button>

               <div class="collapse navbar-collapse" id="navbarSupportedContent">
                   <ul class="navbar-nav ml-auto mt-2 mt-lg-0">
                       <li class="nav-item active">
                           <a class="btn btn-light" onclick="window.open('PRINCIPAL.aspx','_self')"><i class="fas fa-home" style="font-size: x-large; color: darkslategray"></i></a>
                       </li>
                   </ul>
               </div>
           </nav>           

           <div class="container-fluid" id="PanelReimprimir" runat="server">
               <div style="padding-left: 15%; padding-right: 15%;">
                   <div class="row" style="background-color: white; padding-top: 15px;">
                       <div class="col-6">
                           <div class="input-group input-group-sm">
                               <asp:TextBox ID="txtBuscar" runat="server" class="form-control form-control-sm" placeholder="Producto..." OnTextChanged="txtBuscar_TextChanged" AutoPostBack="true" />
                               <asp:DropDownList ID="dlAlmacen" runat="server" CssClass="form-control form-control-sm" OnSelectedIndexChanged="txtBuscar_TextChanged" AutoPostBack="true"></asp:DropDownList>
                               <div class="input-group-append">
                                   <label class="input-group-text">
                                       <asp:LinkButton ID="BtnBuscar" runat="server" class="fas fa-search text-secondary"></asp:LinkButton></label>
                               </div>
                           </div>
                       </div>                       
                       <div class="col">
                           <div style="padding-top:4px; text-align:right;">
                               <a id="btnExcel" runat="server" style="font-size:x-large" class="fas fa-file-download Boton text-dark"></a>&nbsp&nbsp
                               <a id="btnImprimir" runat="server" style="font-size:x-large" class="fas fa-print Boton text-dark"></a>
                           </div>
                       </div> 
                   
                   </div>
                   <div class="row" style="background-color: white">
                       <div class="col">
                           <asp:GridView ID="GvPrincipal" runat="server" CssClass="table table-sm table-bordered table-hover" EmptyDataText="No se econtraron resultados" AllowSorting="true">                             
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
    $("#menu-toggle").click(function(e) {
        e.preventDefault();
        $("#wrapper").toggleClass("toggled");
    });     
    </script>
    </form>  
</body>
</html>
