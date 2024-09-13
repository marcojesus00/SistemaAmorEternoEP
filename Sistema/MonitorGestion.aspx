<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MonitorGestion.aspx.vb" Inherits="Sistema.MonitorGestion" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="initial-scale=1.0, user-scalable=yes"/>
    <title>Monitor de Cobros</title>
    <link rel="shortcut icon" type="image/x-icon" href="~/imagenes/logo.ico"/>
    <link href="css/BootStrap.min.css" rel="stylesheet" type="text/css"/> 
    <link href="css/DataGrid.css" rel="stylesheet" type="text/css"/>   

    <style type="text/css">
        #iFrameCuadre {
            margin-right: 3px;
        }
        .auto-style3 {
            position: fixed;
            right: 113px;
            top: 19px;
        }
        .auto-style4 {
            position: fixed;
            right: 70px;
            top: 19px;
        }
        </style>
   
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel ID="Panel1" runat="server" HorizontalAlign="left">
        <div class="container-fluid" style="margin-top:5px;">          
            <asp:Table ID="Table1" runat="server" Style="margin-bottom:20px;" ToolTip="Filtre su búsqueda">
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">Fecha:</asp:TableCell>
                    <asp:TableCell runat="server"><asp:TextBox ID="txtFecha" CssClass="form-control form-control-sm" runat="server" TextMode="Date" Height="24" Font-Size="Small" /></asp:TableCell></asp:TableRow><asp:TableRow runat="server">                    
                    <asp:TableCell runat="server">Motivo:</asp:TableCell><asp:TableCell runat="server"><asp:DropDownList ID="dlMostrar" CssClass="form-control form-control-sm" runat="server" Width="200" Height="24" Font-Size="Small" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">  
                    <asp:TableCell runat="server">Lider:</asp:TableCell><asp:TableCell runat="server"><asp:DropDownList ID="dlLider" CssClass="form-control form-control-sm" runat="server" Width="200" Height="24" Font-Size="Small" /></asp:TableCell>
                </asp:TableRow>  
                <%--<asp:TableRow runat="server">  
                    <asp:TableCell runat="server">Zona:</asp:TableCell><asp:TableCell runat="server"><asp:DropDownList ID="dlZona" CssClass="form-control form-control-sm" runat="server" Width="200" Height="24" Font-Size="Small" /></asp:TableCell>
                </asp:TableRow>  --%>
       <%--         <asp:TableRow runat="server">                    
                    <asp:TableCell runat="server">Cobrador:</asp:TableCell><asp:TableCell runat="server"><asp:TextBox ID="txtCobrador" CssClass="form-control form-control-sm" runat="server" Width="200" Height="24" Font-Size="Small" />

                                                                           </asp:TableCell></asp:TableRow>--%>
                <asp:TableRow runat="server">                    
                    <asp:TableCell runat="server" ColumnSpan="2">&nbsp</asp:TableCell></asp:TableRow><asp:TableRow runat="server">                    
                    <asp:TableCell runat="server" ColumnSpan="2">
                    <asp:Button ID="btnBuscar" CssClass="btn btn-dark btn-sm" runat="server" Text="Buscar" /></asp:TableCell>                   
                </asp:TableRow>
            </asp:Table>

    </div>      
          <asp:ImageButton ID="btnAtras" ToolTip="Regresar" runat="server" Height="30px" ImageUrl="~/imagenes/atras.png" Width="30px" style="position:fixed; right:10px; top:10px;"/>
            <div>
                 <asp:linkbutton data-toggle="modal"  href="#idModalNoCobro" class="btn btn-outline-primary" id="btnNoCobro"  runat="server" >Agregar Nuevo</asp:linkbutton></div><div style="width:100%; height:30px;  position:fixed; bottom:0px; background-color:#202020; left: 0px; color: #fff">

            <asp:Label ID="lblTotal" runat="server" ForeColor="White"/>&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblRecibos" runat="server" Text="" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblHora" runat="server" Text="" ForeColor="White"/>
        </div>
        <asp:GridView ID="GridCuadre" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" CssClass="ChildGrid">
            <Columns><asp:BoundField DataField="Codigo" HeaderText="Codigo" SortExpression="Codigo" />
                <asp:BoundField DataField="Nombre_Cliente" HeaderText="Nombre_Cliente" SortExpression="Nombre_Cliente" />
                <asp:BoundField DataField="Fecha" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Fecha" SortExpression="Fecha" />
                <asp:BoundField DataField="Hora" HeaderText="Hora" SortExpression="Hora" />
                <asp:BoundField DataField="Cobrador" HeaderText="Cobrador" ReadOnly="True" SortExpression="Cobrador" />
                <asp:BoundField DataField="Motivo" HeaderText="Motivo" SortExpression="Motivo" />
                <asp:BoundField DataField="Lider" HeaderText="Lider" SortExpression="Lider" />
            <asp:BoundField DataField="Comentario" HeaderText="Comentario" SortExpression="Comentario" />

            <asp:BoundField HeaderText="Visitado" /><asp:BoundField HeaderText="Bajado" /></Columns><FooterStyle BackColor="#99CCCC" ForeColor="#003399" /><HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
            <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" /><RowStyle BackColor="White" ForeColor="#003399" />
            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" /><SortedAscendingCellStyle BackColor="#EDF6F6" />
            <SortedAscendingHeaderStyle BackColor="#0D4AC4" /><SortedDescendingCellStyle BackColor="#D6DFDF" />
            <SortedDescendingHeaderStyle BackColor="#002876" />

            </asp:GridView></asp:Panel>
               <asp:Panel ID="PanelImpresion" runat="server" Visible="false">   
            <div>
                               
                <asp:ImageButton ID="btnRegresar" ToolTip="Regresar" runat="server" Height="30px" ImageUrl="~/imagenes/atras.png" Width="30px" CssClass="auto-style4"/><asp:ImageButton ID="btnSalir" runat="server" Height="30px" ImageUrl="~/imagenes/atras.png" Width="30px" CssClass="auto-style3"/></div>
            <div style="width:100%; height:100%;">
            <iframe id="ifRepote" runat="server" style="position:fixed; width:100%; height:100%; top:42px;"></iframe>
            </div>
        </asp:Panel>   
        <div class="container">
        <div class="modal" id="idModalNoCobro">
                                                        <div class="modal-dialog">
                                                            <div class="modal-content">

                                                                <!-- Modal Header -->
                                                                <div class="modal-header align-content-center">
                                                                                   <h4> Gestion de Cobro Gerencia</h4><button type="button" class="close btn btn-danger btn-lg" data-dismiss="modal" data-whatever="">&times;</button></div><!--Cuerpo del Modal--><div class="modal-body">
                                                                    <div id="formNocobro">
                                                                        <table>
                                                                            
                                                                                <tr>
                                                                                    <td>
                                                                                        <label for="message-text" class="col-form-label" >Fecha de Gestion</label> </td><td>
                                                                                        <input id="NCFechallamada" runat="server" type="Date" font-size="Small" /> </td></tr><tr>
                                                                                    <td>
                                                                                        <label for="message-text" class="col-form-label">Codigo Cliente</label> </td><td><input id="txtNccodigo" runat="server" cssclass="form-control form-control-sm" height="24" width="150" /> </td><td>
                                                                                        
                                                                                        <asp:DropDownList id="dropempcli" placeholder="Empresa" runat="server" CssClass="form-control form-control-sm" Height="27px" Width="80" Font-Size="Small" Font-Names="dropempcli">
                                                                                            <asp:ListItem Value="Empresa">-Empresa-</asp:ListItem>
                                                                                            <asp:ListItem Value="M01">M01</asp:ListItem>
                                                                                            <asp:ListItem Value="M02">M02</asp:ListItem>
                                                                                            <asp:ListItem Value="M03">M03</asp:ListItem>
                                                                                            <asp:ListItem Value="M04">M04</asp:ListItem>
                                                                                            <asp:ListItem Value="P01">P01</asp:ListItem>
                                                                                        </asp:DropDownList></td></tr><tr>
                                                                                        <td>
                                                                                            <label for="message-text" class="col-form-label">Atendido Por:</label> </td><td>
                                                                                            <input id="NCnombretxt" runat="server" type="text" font-size="Small" width="270" name="NCnombretxt" /> </td></tr><tr>
                                                                                    <td>
                                                                                        <label for="message-text" class="col-form-label">Gestion:</label> &nbsp</td><td>
                                                                                        <asp:DropDownList ID="DropidMotivo" ToolTip="Motivo" placeholder="Motivo" runat="server" CssClass="form-control form-control-sm" Height="29px" Font-Size="Small">
                                                                                            <asp:ListItem Value="Seleccione">--Seleccione---</asp:ListItem><asp:ListItem Value="Suspendido">Suspendido</asp:ListItem><asp:ListItem Value="NoUbicado">No Ubicado</asp:ListItem><asp:ListItem Value="SinTrabajo">Sin Trabajo</asp:ListItem><asp:ListItem Value="AgendarVisita">Agendar Visita</asp:ListItem></asp:DropDownList></td><td> <label for="message-text"  class="col-form-label" >Cobrador</label></td></tr><tr>
                                                                                <td>
                                                                                        <label for="message-text"  class="col-form-label" >Fecha de Visita</label> </td><td>
                                                                                     <input id="FechaVisita" runat="server" visible="true" type="Date" font-size="Small" /> <td><input type="text" runat="server" id="idcobrGestion" /></td></td></tr><tr>
                                                                                    
                                                                                        <td>
                                                                                            <label for="message-text" class="col-form-label">Comentario</label> </td><td>
                                                                                            <input id="txtcomentarioNC" type="text" iclass="form-control" runat="server" name="txtcomentarioNC" /> </td></tr></table></div><div>
                                                                        <label id="LblError" runat="server"></label>
                                                                    </div>
                                                                </div>

                                                                <!-- Modal footer -->
                                                                <div class="modal-footer">
                                                                    <button id="idcancelar" type="button" class="btn btn-outline-danger" data-dismiss="modal">Cancelar</button><asp:LinkButton ID="btnsavarNC" type="button" runat="server" class="btn btn-success">Salvar</asp:LinkButton></div></div></div></div></div>
        <script src="js/JQuery.js"></script><script src="js/popper.min.js"></script><script src="js/bootstrap.min.js"></script><script src="js/Jquery1.8.3.js"></script><script type="text/javascript">

            $("[src*=plus]").live("click", function () {
                $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
                $(this).attr("src", "imagenes/minus.png");
            });
            $("[src*=minus]").live("click", function () {
                $(this).attr("src", "imagenes/plus.png");
                $(this).closest("tr").next().remove();
            });

        </script></form></body></html>