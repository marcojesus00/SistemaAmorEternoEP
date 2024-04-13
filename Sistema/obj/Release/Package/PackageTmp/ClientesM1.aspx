<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ClientesM1.aspx.vb" Inherits="Sistema.ClientesM1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

 

    <!-- Bootstrap core CSS -->  
  <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
    <link rel="shortcut icon" type="image/x-icon" href="svgs/solid/drafting-compass.svg"/>

  <!-- Custom styles for this template -->
  <link href="css/simple-sidebar.css" rel="stylesheet"/>
  <link rel="stylesheet" href="css/all.min.css" />


    <style>
        body {
     background-image: linear-gradient(135deg, #fdfcfb 0%, #e2d1c3 100%);
}

.titulo {
     font-size: 55px;
     text-transform: uppercase;
     letter-spacing: 7px;
}
.thumbnail-image {
    max-width: 20px; /* Ancho máximo de la miniatura */
    max-height: 20px; /* Altura máxima de la miniatura */
    border: 2px solid #ccc; /* Borde de 2 píxeles con color gris claro */
    border-radius: 5px; /* Bordes redondeados */
    margin: 10px; /* Margen alrededor de la miniatura */
    cursor: pointer; /* Cambia el cursor al pasar por encima de la miniatura */
}
    </style>
</head>
    
<body>
    <form id="form1" runat="server">
     <div><h4 style="width:100%; text-align:center">Detalle de Clientes - Bitacora</h4></div>
        <main class="container">
        <div>
            <asp:ImageButton ID="btnSalir" ToolTip="Regresar" runat="server" Height="30px" ImageUrl="~/imagenes/atras.png" Width="30px" style="position:fixed; right:10px; top:10px;"/>
        </div>
        <div>
          
            <br />
        </div>
             <div class="container-fluid" style="padding-top: 5px;" id="PanelFactura" runat="server">
               <div class="row">
                       <div class="col col-3">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="usuario" runat="server" class="input-group-text" style="width: 95px" >Cliente</label>
                           </div>
                           <asp:TextBox TextMode="SingleLine" CssClass="form-control form-control-sm" ID="txtCliente" runat="server" Enabled="true" />
                       </div>
                   </div>
                    <div class="col col-3">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dvale" class="input-group-text" style="width: 95px">Fecha</label>
                            
                           </div>
                           <asp:TextBox CssClass="form-control form-control-sm" ID="txtFechaContrato" runat="server" Enabled="false"/>
                       </div>
                   </div>  
                   <div class="col col-3">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dvale" class="input-group-text" style="width: 95px">Contrato</label>
                              <%-- <asp:DropDownList ID="dlNumeracion" runat="server" CssClass="input-group-text" ToolTip="Serie de Empresa..." OnSelectedIndexChanged="dlNumeracion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                           </div>
                           <asp:TextBox TextMode="number" CssClass="form-control form-control-sm" ID="txtContrato" runat="server" Enabled="false" ToolTip="Numero de Contrato    ..." />
                       </div>
                        
                   </div>
                   <div class="col-3">
                   <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dvale" class="input-group-text" style="width: 95px">Cobrador</label>
                              <%-- <asp:DropDownList ID="dlNumeracion" runat="server" CssClass="input-group-text" ToolTip="Serie de Empresa..." OnSelectedIndexChanged="dlNumeracion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                           </div>
                           <asp:TextBox TextMode="number" CssClass="form-control form-control-sm" ID="txtclCobrador" runat="server" Enabled="false" ToolTip="Cobrador   ..." />
                       </div>
            </div>
               
                                   
                  
               </div>
            </div>
        <br />
        <%--  <div class="table table-hover">
              <div class="col-3">
                <div class="row">
                   
                        <img id="imgfoto" runat="server" width="250" height="150" style="border:1px"/>
                        
                        </div> 
              
                <div class="row">
                   
                        <asp:Button CssClass="alert-success" Text="Guardar" runat="server" ID="btnguardarimg"/>
                  <asp:FileUpload ID="archivoInput" cssClass="col-md-6" style="padding:10px;" runat="server" Visible="false" onchange="return validarExt()"/>
                    <%--<input type="file" value="....." id="btnAgregar" runat="server"  />
                  
                  </div>
                  
            </div>
              </div>--%>

             <section class="text-left;">
           <%--      <asp:Button ID="btnGuardar" runat="server" Text="Guardar Archivo" OnClick="btnGuardar_Click" />--%>

                <div>
                    <table style="width:72%;">
                     
                           <img id="imagenPrevia" src="#" alt="" />

<%--                           <embed id="imgFoto" runat="server" class="foto" height="160" src="" onclick="window.open('imagenes/logo.png','_blank','scrollbars=yes,resizable=yes,top=5,left=5,width=700,height=700')" />--%>
                   
                    </table>
                    
                    <%--<asp:FileUpload ID="FileUpload" cssClass="col-md-3" style="padding:10px;" runat="server" Visible="true" onchange="return validarExt()"/>                --%>
                    <asp:FileUpload ID="FileUpload" cssClass="col-md-6" style="padding:10px;" runat="server" Visible="true" onchange="previsualizarImagen()" />                
                    <br />
                    <asp:TextBox ID="TxtComentario" runat="server" style="height:" ToolTip="Puedes Agregar una nota al archivo"> </asp:TextBox>
                </div>     
                 <br />
                 <br />
                 <br /> 
            <asp:Button ID="btnGuardarFile"  CssClass="btn btn-success" runat="server" Text="Guardar Archivo" />
                <asp:ScriptManager ID="sm" runat="server" EnablePageMethods="true" />
                 
		    </section>

            <br />
            <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>

            <div class="col col-sm-12">
                <div class="row galeria">
                    <div class="col col-sm-12 col-lg-3 col-md-4">

                        <div class="material-placeHolder">
                            <asp:Repeater ID="rptPDFList" runat="server">
                                <ItemTemplate>
                                    
                                    <p> <img src="imagenes/image_pdf.png" <%# Eval("RutaFile") %> alt="Miniatura del PDF" class="thumbnail-image" />
                                        <a download='<%# Eval("RutaFile") %>' href='<%# Eval("RutaFile") %>'><%# Eval("Comentario") %></a>
                                        &nbsp; (<a href='<%# Eval("RutaFile") %>' target="_blank, '<%# Eval("RutaFile") %>'">Abrir</a> )
                                                
                                        
                                    </p>
                                    <asp:Button Text="Editar" ID="btnEditar" runat="server"/>
                                </ItemTemplate>
                                
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>
    </main>



    </form>
    

        <script src="https://cdnjs.cloudflare.com/ajax/libs/materialize/1.0.0/js/materialize.min.js"></script>
    <script src="js/mainFM.js"></script>
    <script> 
        function previsualizarImagen() {
            var FileUpload = document.getElementById('FileUpload');
            var imagenPrevia = document.getElementById('imagenPrevia');
            //var btnguardar = document.getElementById('btnGuardarFile').visible= 'true'

            

            if (FileUpload.files && FileUpload.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    imagenPrevia.src = e.target.result;
                };

                reader.readAsDataURL(FileUpload.files[0]);
            }
        }

<%--        function descargarArchivo() {
            // Realiza alguna lógica de JavaScript si es necesario
            // Por ejemplo, puedes mostrar un mensaje de confirmación al usuario

            // Luego, ejecuta la función del lado del servidor (evento Click del botón)
            __doPostBack('<%= btnDescargar.UniqueID %>', '');
           }--%>

    function validarExt() {
            var archivoInput = document.getElementById('FuLogo');
            var archivoRuta = archivoInput.value;
            var extPermitidas = /(.jpg)$/i;

            if (!extPermitidas.exec(archivoRuta)) {
                alert('Asegurese de haber seleccionado una Imagen JPG');
                archivoInput.value = '';
                return false;
            }
        
            else {
                //Mostrar Imagen
                if (FileUpload.files && FileUpload.files[0]) {
                    var visor = new FileReader();
                    visor.onload = function (e) {
                        document.getElementById("imgFoto").visible = "true";
                        document.getElementById('visorArchivo').innerHTML = '<embed src="' + e.target.result + '" height="160"/>';
                    };
                    visor.readAsDataURL(archivoInput.files[0]);
                }
                }
        }
    </script>
</body>
</html>

