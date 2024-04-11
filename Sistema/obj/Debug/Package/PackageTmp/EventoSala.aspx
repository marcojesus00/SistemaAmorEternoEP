<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EventoSala.aspx.vb" Inherits="Sistema.EventoSala" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

     <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
    <link rel="shortcut icon" type="image/x-icon" href="svgs/solid/drafting-compass.svg"/>
    <link href="css/StyleResponsive.css" rel="stylesheet" />

    <style>
        
        #div1 {
    width: 300px;
    padding: 25px;
    height:650px;
    border: 10px solid navy;
    border-color:forestgreen;
    margin: 15px;
    background-color:whitesmoke;
}

        #div2 {
    width: 300px;
    padding: 25px;
    height:650px;
    border: 10px solid navy;
    margin: 15px;
    border-color:forestgreen;
    background-color:whitesmoke;
    opacity:0,4;
}
       #div3 {
    width: 300px;
    padding: 25px;
    height:650px;
    border: 10px solid navy;
    margin: 15px;
    border-color:forestgreen;
    background-color:whitesmoke;
}

                       
    #h1{
   /*     font: 150% sans-serif;
         font-size: x-large;*/
    }
   body{
       background-image:url("imagenes/LogoActual.jpg");
       background-position:inherit;
       /* background-color:aquamarine;*/

    }


   #divtitulo{
       background-color:aliceblue;
   }

   .parpadea {
  
  animation-name: parpadeo;
  animation-duration: 1s;
  animation-timing-function: linear;
  animation-iteration-count: infinite;

  -webkit-animation-name:parpadeo;
  -webkit-animation-duration: 1s;
  -webkit-animation-timing-function: linear;
  -webkit-animation-iteration-count: infinite;
}

@-moz-keyframes parpadeo{  
  0% { opacity: 1.0; }
  50% { opacity: 0.0; }
  100% { opacity: 1.0; }
}

@-webkit-keyframes parpadeo {  
  0% { opacity: 1.0; }
  50% { opacity: 0.0; }
   100% { opacity: 1.0; }
}

@keyframes parpadeo {  
  0% { opacity: 1.0; }
   50% { opacity: 0.0; }
  100% { opacity: 1.0; }
}

img {
 max-width: 100%;
 max-height: 100%;
}
        .cat {
            height: 150px;
            width: 200px;          
        }
 

    </style>

</head>
<body>

    <form id="form1" runat="server">
        <div class="container-fluid" style="padding-top:3px;" id="fondo">       
        <div id="divtitulo"  runat="server" class="row parpadea" style="justify-content:center"> 
            <h1 id="h1" runat="server">SALAS DE VELACION MEMORIAL</h1>
        </div>


        <div class="row">
            <div class="col" id="div1">
                <div class="row">
                    <div class="col cat">
                            <img src="/imagenes/Pruebasala.jpg" />
                    </div>
                   <div class="col">
                           <h3>09 - Enero - 1950</h3>
                       <h3>31 - Mayo - 2022 </h3>
                   </div>                     

                 </div> 
          
                
              
            <%--        <div class="col">
                    
                    <label id="lblFechaNac"></label>
                        </div>
                --%>
                <div class="row">
                    <div class="col input-group input-group-sm">
                     <h3>   Difunto:</h3>
                    </div>
                    <label id="lblDinfuto" runat="server" class="form-control form-control-sm" />
                </div>
               
                 <div class="row">
                    <div class="row">
                        <div class="col input-group input-group-sm">
                          <h4> Sala:</h4> 
                       </div>
                        <label id="Label1" runat="server" class="form-control form-control-sm" />
                    </div> 
                </div>
                <div class="row">
                    <div class="row">
                        <div class="col input-group input-group-sm">
                          <h3> Hora Entrada:</h3> 
                        </div>
                        <label id="lblhoraEntrada" runat="server" class="form-control form-control-sm" />
                    </div>
                </div>
              
                  <div class="row">
                    <div class="row">
                        <div class="col input-group input-group-sm">
                          <h3> Hora Salida:</h3> 
                        </div>
                        <label id="lblhorasalida" runat="server" class="form-control form-control-sm" />
                    </div>
                </div>
             
                 <div class="row">
                    <div class="row">
                        <div class="col input-group input-group-sm">
                          <h3> Sepelio:</h3> 
                        </div>
                        <label id="lblSepelio" runat="server" class="form-control form-control-sm" />
                    </div>
                </div>
            </div>

            <div class="col " id="div2">

                <div class="row">
                    <div class="col input-group input-group-sm">
                     <h3>   Difunto:</h3>
                    </div>
                    <label id="Label2" runat="server" class="form-control form-control-sm" />
                </div>
             
                 <div class="row">
                    <div class="row">
                        <div class="col input-group input-group-sm">
                          <h4> Sala:</h4> 
                       </div>
                        <label id="Label3" runat="server" class="form-control form-control-sm" />
                    </div> 
                </div>
                <div class="row">
                    <div class="row">
                        <div class="col input-group input-group-sm">
                          <h3> Hora Entrada:</h3> 
                        </div>
                        <label id="Label4" runat="server" class="form-control form-control-sm" />
                    </div>
                </div>
                <br />
                  <div class="row">
                    <div class="row">
                        <div class="col input-group input-group-sm">
                          <h3> Hora Salida:</h3> 
                        </div>
                        <label id="Label5" runat="server" class="form-control form-control-sm" />
                    </div>
                </div>
                <br />
                 <div class="row">
                    <div class="row">
                        <div class="col input-group input-group-sm">
                          <h3> Sepelio:</h3> 
                        </div>
                        <label id="Label6" runat="server" class="form-control form-control-sm" />
                    </div>
                </div>

            </div>
             <div class="col" id="div3">
                 <div class="row">
                    <div class="col input-group input-group-sm">
                     <h3>   Difunto:</h3>
                    </div>
                    <label id="Label7" runat="server" class="form-control form-control-sm" />
                </div>
                <br />
                 <div class="row">
                    <div class="row">
                        <div class="col input-group input-group-sm">
                          <h4> Sala:</h4> 
                       </div>
                        <label id="Label8" runat="server" class="form-control form-control-sm" />
                    </div> 
                </div>
                <div class="row">
                    <div class="row">
                        <div class="col input-group input-group-sm">
                          <h3> Hora Entrada:</h3> 
                        </div>
                        <label id="Label9" runat="server" class="form-control form-control-sm" />
                    </div>
                </div>
                <br />
                  <div class="row">
                    <div class="row">
                        <div class="col input-group input-group-sm">
                          <h3> Hora Salida:</h3> 
                        </div>
                        <label id="Label10" runat="server" class="form-control form-control-sm" />
                    </div>
                </div>
                <br />
                 <div class="row">
                    <div class="row">
                        <div class="col input-group input-group-sm">
                          <h3> Sepelio:</h3> 
                        </div>
                        <label id="Label11" runat="server" class="form-control form-control-sm" />
                    </div>
                </div>
             </div>

        </div>
        </div>
        <script src="js/JQuery.js"></script>
        <script src="js/popper.min.js"></script>
        <script src="js/bootstrap.min.js"></script>        
       
    </form>


    
</body>
</html>
