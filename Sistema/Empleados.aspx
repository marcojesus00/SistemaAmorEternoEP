<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Empleados.aspx.vb" Inherits="Sistema.Empleados" EnableEventValidation="false" %>
<%@ Register TagPrefix="uc" TagName="FileManager" Src="~/controls/FileManager.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Vendedor</title>
     <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    <link rel="shortcut icon" type="image/x-icon" href="~/imagenes/logo.ico"/>    
    <link href="css/BootStrap.min.css" rel="stylesheet" type="text/css"/> 
    <link href="css/3.3/BootStrap.min.css" rel="stylesheet" type="text/css"/>  
    <link href="css/estilos.css" rel="stylesheet" type="text/css"/>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css" rel="stylesheet"/> 

</head>
<body>    
    <form id="form1" runat="server">
        <div class="container-fluid" style="padding-top:5px;">
            <section class="text-left;">
                <div>
                    <table style="width:72%;">
                       <tr>
                           
                       </tr>
                    </table>
                    <asp:FileUpload ID="archivoInput" cssClass="col-md-6" style="padding:10px;" runat="server" Visible="false" onchange="return validarExt()"/>                                                 
                </div>     
                <asp:ScriptManager ID="sm" runat="server" EnablePageMethods="true" />
		    </section>
            <br />
            <asp:Table CellPadding="3" ID ="Table1" runat="server">   
        
                <asp:TableRow runat="server">  
                    <asp:TableCell ColumnSpan="2" runat="server" ID="visorArchivo">
                        <embed class="center-block" style="padding:10px;" id="imgFoto" runat="server" height="160" />
                    </asp:TableCell>                    
                    <asp:TableCell runat="server"></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan ="2">
                        <button id="btnCamara" runat="server" type="button" class="btn btn-primary center-block" style="width:130px; padding:5px;" data-toggle="modal" data-target="#modalCamara" onclick="init();">Tomar foto</button>
                    </asp:TableCell>
                    </asp:TableRow>
                <asp:TableRow runat="server">                    
                    <asp:TableCell runat="server">Codigo:</asp:TableCell>
                    <asp:TableCell runat="server"><asp:TextBox ID="txtCodigo" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="24" Width="150" Enabled="false" /></asp:TableCell></asp:TableRow><asp:TableRow runat="server">                    
                    <asp:TableCell runat="server">Nombre Completo:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="txtNombre" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="24" Width="300" Font-Size="Small" /></asp:TableCell></asp:TableRow><asp:TableRow runat="server">                    
                    <asp:TableCell runat="server">Direccion:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="txtDireccion" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="24" Width="300" /></asp:TableCell></asp:TableRow><asp:TableRow runat="server">                    
                    <asp:TableCell runat="server">Identidad:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="txtIdentidad" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="24" /></asp:TableCell></asp:TableRow><asp:TableRow runat="server">                    
                    <asp:TableCell runat="server">Fecha Nacimiento:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="txtFechaN" CssClass="form-control form-control-sm" runat="server" TextMode="Date" Height="24" /></asp:TableCell></asp:TableRow><asp:TableRow runat="server">                    
                    <asp:TableCell runat="server">Sexo:</asp:TableCell><asp:TableCell runat="server"><asp:DropDownList ID="txtSexo" CssClass="form-control form-control-sm" runat="server" Height="28" Width="150" >
                        <asp:ListItem Selected="True" Value="M">Masculino</asp:ListItem>
                        <asp:ListItem Value="F">Femenino</asp:ListItem>
                    </asp:DropDownList></asp:TableCell><asp:TableCell><asp:Label ID="lblEdadAcual" runat="server" Text=""></asp:Label></asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                    <asp:TableCell runat="server">Ciudad Nacimiento:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="txtCiudad" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="24" /></asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                    <asp:TableCell runat="server">Estado Civil:</asp:TableCell><asp:TableCell runat="server"><asp:DropDownList ID="txtCivil" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="28" Width ="150">
                        <asp:ListItem Selected="True" Value="S">Soltero</asp:ListItem>
                        <asp:ListItem Value="C">Casado</asp:ListItem>
                        <asp:ListItem Value="U">Union Libre</asp:ListItem>
                        <asp:ListItem Value="D">Divorciado</asp:ListItem>
                        <asp:ListItem Value="V">Viudo</asp:ListItem>
                    </asp:DropDownList></asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                    <asp:TableCell runat="server">Cargo:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="txtCargo" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="24" Width="70"/></asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                    <asp:TableCell runat="server">Departamento:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="txtDepto" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="24" Width="70"/></asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                    <asp:TableCell runat="server">Contrato:</asp:TableCell><asp:TableCell runat="server"><asp:DropDownList ID="txtTmpPer" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="28" Width="150">
                        <asp:ListItem Selected="True" Value="T">Temporal</asp:ListItem>
                        <asp:ListItem Value="P">Permanente</asp:ListItem>
                    </asp:DropDownList></asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                    <asp:TableCell runat="server">Fecha Ingreso:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="txtFechaI" CssClass="form-control form-control-sm" runat="server" TextMode="Date" Height="24" /></asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                    <asp:TableCell runat="server">Fecha Salida:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="txtFechaS" CssClass="form-control form-control-sm" runat="server" TextMode="Date" Height="24" /></asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                    <asp:TableCell runat="server">Motivo Salida:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="TxtMotivo" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="24" Width="300" /></asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                    <asp:TableCell runat="server">Sueldo:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="txtSueldo" CssClass="form-control form-control-sm" runat="server" TextMode="Number" Height="24" /></asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                    <asp:TableCell runat="server">Forma Pago:</asp:TableCell><asp:TableCell runat="server"><asp:DropDownList ID="txtTipoPlan" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="28" Width = "150" >
                        <asp:ListItem Selected="True" Value="S">Semanal</asp:ListItem>
                        <asp:ListItem Value="Q">Quincenal</asp:ListItem>
                        <asp:ListItem Value="M">Mensual</asp:ListItem>
                    </asp:DropDownList></asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                    <asp:TableCell runat="server">Activo:</asp:TableCell><asp:TableCell runat="server"><asp:DropDownList ID="txtActivo" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="28" Width="150" >
                        <asp:ListItem Value="A">Activo</asp:ListItem>
                        <asp:ListItem Value="R">Retirado</asp:ListItem>
                    </asp:DropDownList></asp:TableCell></asp:TableRow><asp:TableRow runat="server">                       
                    <asp:TableCell runat="server" ColumnSpan="2">&nbsp</asp:TableCell></asp:TableRow><asp:TableRow runat="server">                       
                    <asp:TableCell runat="server" HorizontalAlign="Right"><asp:Button ID="btnActualizar" runat="server" Text="Guardar" CssClass="btn btn-primary btn-sm"/></asp:TableCell>   
                    <asp:TableCell runat="server" HorizontalAlign="Right"><asp:ImageButton ID="btnimprimir" ToolTip="Imprimir Ficha" runat="server" Height="28px" ImageUrl="~/imagenes/Printer.png" Width="28px" OnClientClick="window.open('reportes.aspx')"/> </asp:TableCell> 
                </asp:TableRow>
            </asp:Table>  

             <uc:FileManager ID="FileManager1" runat="server" />.

            <br />

            <br />
        </div>
        <asp:ImageButton  style="padding-top:10px; position:absolute; top:10px; right:30px;" ID="btnSalir" ToolTip="Cancelar" runat="server" Height="28px" ImageUrl="~/imagenes/cancelar.png"  /> 
        <div id="modalCamara" class="modal hide" role="dialog" data-backdrop="static" data-keyboard="false" >
          <div class="modal-dialog">

            <!-- Contenido -->
            <div class="modal-content">
              <div class="modal-header">
                  <h4 class="modal-title" id="tituloModal">Camara:</h4><button type="button" class="close" data-dismiss="modal" onclick="apagar();">&times;</button></div><div class="modal-body">
                <div class="video-wrap">
                    <video id="video" class="center-block"  style="width:400px; height:300px;" autoplay="autoplay" playsinline=""></video>
                </div>
                <div id="cf" class="row" style="margin-bottom:10px;">
                    <canvas runat="server" id="canvas" class="center-block" style="display: block; overflow:hidden; border:1px solid black;" ></canvas>
                </div>
                <div>
                    <img src="/imagenes/4-3 grid.png" id="grid43" class="hidden"/>
                    <img src="/imagenes/16-9 grid.png" id="grid169" class="hidden"/>
                </div>
                <div class="row">
                    <button class="center-block col-sm-4 btn btn-default" id="btnPausa" onclick="pausar();">Capturar</button></div><div class="row" style="padding-top: 5px;">
                    <button class="col-sm-offset-1 col-sm-4 btn btn-danger" " id="btnResumir" onclick="resumir();">Descartar</button><!--<asp:Button runat="server" style="padding-left:10px;" Cssclass="col-sm-offset-2 col-sm-4 btn btn-success " id="btnGuardar" Text="Guardar" on ="guardar();"/>--><button style="padding-left:10px;" class="col-sm-offset-2 col-sm-4 btn btn-success " id="btnGuardar" onclick ="guardar();">Guardar</button></div></div><div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal" onclick="apagar();">Cancelar</button></div>
            </div>
          </div>
        </div>
        
        <script src="js/JQuery.js"></script>
        <script src="js/popper.min.js"></script>
        <script src="js/bootstrap.min.js"></script>
        <script type="text/javascript">
        function validarExt()
        {
            var archivoInput = document.getElementById('archivoInput');
            var archivoRuta = archivoInput.value;
            var extPermitidas = /(.jpg|.png|.jpeg)$/i;

            if(!extPermitidas.exec(archivoRuta)){
                alert('Asegurese de haber seleccionado una Imagen');
                archivoInput.value = '';
                return false;
            }
            else
            {
                //Mostrar Imagen
                if (archivoInput.files && archivoInput.files[0]) 
                {
                    var visor = new FileReader();
                    visor.onload = function(e) 
                    {
                        document.getElementById("imgFoto").visible = "False";                           
                        document.getElementById('visorArchivo').innerHTML = '<embed src="'+e.target.result+'" height="160"/>';
                    };
                    visor.readAsDataURL(archivoInput.files[0]);     
                }
            }
        }   
            'use strict';
            var video2 = document.getElementById('video');
            var btnPausar = document.getElementById('btnPausar');
            var btnResumir = document.getElementById('btnResumir');
            var btnGuardar = document.getElementById('btnGuardar');                                                                                                                                                                                                                                                                           
            var dataURL;
            var aspRatio;
            var width;
            var height;
            const constraints =
            {
                audio: false,
                video: {
                    width: 1920, height: 1080
                }
            };

            

            async function iniciar() {
                try {
                    const stream = await navigator.mediaDevices.getUserMedia(constraints);
                    handlerExito(stream);
                }
                catch(e){
                    alert("error")
                }
            }

            function handlerExito(stream) {
                window.stream = stream;
                video2.srcObject = stream;
            }


            function init() //iniciar el componente de la camara
            {
                iniciar();

                btnResumir.classList.add('hidden');
                btnGuardar.classList.add('hidden');
                video.classList.add('hidden');

                video.addEventListener("loadedmetadata", function (e) {
                    this.style.display = "none";
                    var cf = document.getElementById('cf');
                    var tituloModal = document.getElementById('tituloModal');
                    var scale;
                    var canvas = document.getElementById('canvas');
                    var context;
                    width = this.videoWidth;
                    height = this.videoHeight;
                    aspRatio = width / height;
                    tituloModal.innerHTML = "Camara: " + this.videoWidth + "x" + this.videoHeight;
                    if (aspRatio == (1920 / 1080)) // Todas las camaras de 16:9
                    {
                        //alert("cfg 1 : " + aspRatio + "- FHD - " + "h: " + this.videoHeight + " w: " + this.videoWidth);
                        //configurar la resolucion
                        //primero agrandar el div que contiene al canvas
                        cf.width = 640;
                        cf.heigth = 360;
                        //luego ajustar el canvas a la resolucion deseada
                        canvas.width = this.videoWidth;
                        canvas.height = this.videoHeight;
                        // crear la escala
                        scale = this.videoWidth / 400;

                        //configurar lo que se mostrara en pantalla (16:9)
                        canvas.style.width = "400px";
                        canvas.style.heigth = "225px";

                        context = canvas.getContext('2d');
                        video2.addEventListener('play', function () { var i = window.setInterval(function () { context.drawImage(video2, 0, 0, 400, 225) }, 20); }, false);
                        var grid = document.getElementById("grid169");
                        grid.classList.remove("hidden");
                        grid.style.cssText = "position: absolute; top:20px; left:50px;";
                    }
                    else if (aspRatio == (640 / 480)) //Normalmente solo camaras VGA y QVGA 4:3
                    {
                        //alert("cgf 3 : " + aspRatio + "- VGA - " + "h: " + this.videoHeight + " w: " + this.videoWidth);
                        //configurar la resolucion
                        //primero agrandar el div que contiene al canvas
                        cf.width = this.videoWidth;
                        cf.heigth = this.videoHeight;
                        //luego ajustar el canvas a la resolucion deseada
                        canvas.width = this.videoWidth;
                        canvas.height = this.videoHeight;
                        // crear la escala
                        scale = this.videoWidth / 400;

                        //configurar lo que se mostrara en pantalla (4:3)
                        canvas.style.width = "400px";
                        canvas.style.heigth = "300px";

                        //dibujar la foto
                        context = canvas.getContext('2d');
                        video2.addEventListener('play', function () { var i = window.setInterval(function () { context.drawImage(video2, 0, 0, 400, 300) }, 20); }, false);
                        //dibujar las lineas guia sobre el canvas
                        var grid = document.getElementById("grid43");
                        grid.classList.remove("hidden");
                        grid.style.cssText = "position: absolute; top:20px; left:50px;";
                    }
                    else {
                        alert("No se reconoce la resolucion de esta camara");
                    }
                    
                    context.setTransform(scale, 0, 0, scale, 0, 0); //aplicar la escala
                    
                }, false);
            }

            function pausar() //tomar foto
            {
                event.preventDefault();
                video2.pause();
                btnResumir.classList.remove('hidden');
                btnGuardar.classList.remove('hidden');
            }

            function resumir() //descartar foto
            {
                event.preventDefault();
                video2.play();
                btnResumir.classList.add('hidden');
                btnGuardar.classList.add('hidden');
            }

            function apagar() //apagar la camara
            {
                stream.getTracks().forEach(function (track) {
                    track.stop();
                });
            }

            function guardar()
            {
                var codigoEmpleado = $("#txtCodigo").val();
                if (codigoEmpleado == "") //si el numero de empleado esta vacio...
                {
                    alert("Error: No ha seleccionado un empleado");
                    return;
                }

                //crear un canvas temporal para hacer el recorte
                var ctxRecorte;
                var canvasRecorte = document.createElement("canvas");
                canvasRecorte.width = canvas.width;
                canvasRecorte.height = canvas.height;
                ctxRecorte = canvasRecorte.getContext("2d");

                var region = new Path2D();
                var xo, yo, wr, hr;
                xo = canvas.width * 0.275625; //colocar el punto inicial x en el 27% de la imagen
                wr = canvas.width - (xo * 2); //longitud a recorrer horizontalmente desde el punto x de inicio
                yo = canvas.height * 0.10; //colocar el punto inicial y en el 10% de la imagen
                hr = canvas.height - (yo * 2); //longitud a recorrer verticalmente desde el punto y de inicio
                region.rect(xo, yo, wr, hr); //crear el rectangulo de recorte
                ctxRecorte.clip(region, "evenodd"); //recortar del canvas
                ctxRecorte.drawImage(canvas, 0, 0); //dibujar en el canvas

                //crear un 3er canvas que contendra la foto final
                
                var canvasFinal = document.createElement("canvas");
                var ctxFinal;
                canvasFinal.width = wr;
                canvasFinal.height = hr;
                ctxFinal = canvasFinal.getContext("2d");
                ctxFinal.transform(1, 0, 0, 1, -xo, -yo); //mover la foto hacia la posicion (0,0);
                ctxFinal.drawImage(canvasRecorte, 0, 0);
                var datosCanvasFinal = canvasFinal.toDataURL("image/png", 1.0);
                datosCanvasFinal = datosCanvasFinal.replace('data:image/png;base64,', '');
                //crear los datos a pasar
                var datos = "{ 'datosImg' : '" + datosCanvasFinal + "' , 'str' : '" + codigoEmpleado + "'}";

                llamarFuncionServidor("Empleados.aspx/SubirFoto", datos, callback);
            }

            function callback(result) {
                alert("Foto guardada con exito");
                location.reload(); //refrescar la pagina
            }

            function llamarFuncionServidor(urlFunc, datos, CallBackFunc) { //llamar un webMethod con AJAX para enviar el string de datos de la img

                $.ajax({
                    type: "POST",
                    url: urlFunc,
                    contentType: "application/json; charset=utf-8",
                    data: datos,
                    //dataType: "json",
                    success: function (result) {
                        if (CallBackFunc != null && typeof CallBackFunc != 'nada') {
                            CallBackFunc(result);
                        }

                    },
                    error: function (result) {
                        alert('error occured');
                        alert(result.responseText);
                        window.location.href = "FrmError.aspx?Exception=" + result.responseText;
                    },
                    async: false
                });
            }

        </script>
        <p><asp:Label ID="lblMsg" runat="server" Text=""></asp:Label></p>

    </form></body></html>