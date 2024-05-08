<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Empleados.aspx.vb" Inherits="Sistema.Empleados" EnableEventValidation="false" %>

<%@ Register TagPrefix="uc" TagName="FileManager" Src="~/controls/FileManager.ascx" %>
<%@ Register TagPrefix="uc" TagName="ProfilePicture" Src="~/controls/ProfilePicture.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Vendedor</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <link rel="shortcut icon" type="image/x-icon" href="~/imagenes/logo.ico" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous" />

</head>
<body>

    <form id="form1" runat="server">

        <div class="accordion" id="accordionExample">

            <asp:PlaceHolder ID="EmployeeCard" runat="server">

                <div class="accordion-item">
                    <h2 class="accordion-header">
                        <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                            Información general
                        </button>
                    </h2>
                    <div id="collapseOne" class="accordion-collapse collapse show" data-bs-parent="#accordionExample">
                        <div class="accordion-body">

                       <uc:ProfilePicture class="w-100 " ID="ProfilePicture1" runat="server" />

                        </div>
                    </div>
                </div>


            </asp:PlaceHolder>

            <div class="accordion-item">

                <h2 class="accordion-header">
                    <button class='<% If anEmployeeIsSelected Then Response.Write("accordion-button collapsed") Else Response.Write("accordion-button") %>'
                        type="button" data-bs-toggle="collapse" data-bs-target="#collapseTwo" aria-expanded='<% If anEmployeeIsSelected Then Response.Write("false") Else Response.Write("true") %>' aria-controls="collapseTwo">
                        Datos de empleado
                    </button>
                </h2>
                <div id="collapseTwo" class='<% If anEmployeeIsSelected Then Response.Write("accordion-collapse collapse ") Else Response.Write("accordion-collapse collapse show") %>' data-bs-parent="#accordionExample">
                    <div class="accordion-body">
                        <div class="container-fluid" style="padding-top: 5px;">
                            <section class="text-left;">

                                <asp:ScriptManager ID="sm" runat="server" EnablePageMethods="true" />
                            </section>
                            <asp:Table CellPadding="3" ID="Table1" runat="server">
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server">Codigo:</asp:TableCell>
                                    <asp:TableCell runat="server">
                                        <asp:TextBox ID="txtCodigo" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="24" Width="150" Enabled="false" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server">Nombre Completo:</asp:TableCell><asp:TableCell runat="server">
                                        <asp:TextBox ID="txtNombre" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="24" Width="300" Font-Size="Small" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server">Direccion:</asp:TableCell><asp:TableCell runat="server">
                                        <asp:TextBox ID="txtDireccion" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="24" Width="300" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server">Identidad:</asp:TableCell><asp:TableCell runat="server">
                                        <asp:TextBox ID="txtIdentidad" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="24" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server">Fecha Nacimiento:</asp:TableCell><asp:TableCell runat="server">
                                        <asp:TextBox ID="txtFechaN" CssClass="form-control form-control-sm" runat="server" TextMode="Date" Height="24" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server">Sexo:</asp:TableCell><asp:TableCell runat="server">
                                        <asp:DropDownList ID="txtSexo" CssClass="form-control form-control-sm" runat="server" Height="28" Width="150">
                                            <asp:ListItem Selected="True" Value="M">Masculino</asp:ListItem>
                                            <asp:ListItem Value="F">Femenino</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:TableCell><asp:TableCell>
                                        <asp:Label ID="lblEdadAcual" runat="server" Text=""></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server">Ciudad Nacimiento:</asp:TableCell><asp:TableCell runat="server">
                                        <asp:TextBox ID="txtCiudad" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="24" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server">Estado Civil:</asp:TableCell><asp:TableCell runat="server">
                                        <asp:DropDownList ID="txtCivil" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="28" Width="150">
                                            <asp:ListItem Selected="True" Value="S">Soltero</asp:ListItem>
                                            <asp:ListItem Value="C">Casado</asp:ListItem>
                                            <asp:ListItem Value="U">Union Libre</asp:ListItem>
                                            <asp:ListItem Value="D">Divorciado</asp:ListItem>
                                            <asp:ListItem Value="V">Viudo</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server">Cargo:</asp:TableCell><asp:TableCell runat="server">
                                        <asp:TextBox ID="txtCargo" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="24" Width="70" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server">Departamento:</asp:TableCell><asp:TableCell runat="server">
                                        <asp:TextBox ID="txtDepto" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="24" Width="70" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server">Contrato:</asp:TableCell><asp:TableCell runat="server">
                                        <asp:DropDownList ID="txtTmpPer" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="28" Width="150">
                                            <asp:ListItem Selected="True" Value="T">Temporal</asp:ListItem>
                                            <asp:ListItem Value="P">Permanente</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server">Fecha Ingreso:</asp:TableCell><asp:TableCell runat="server">
                                        <asp:TextBox ID="txtFechaI" CssClass="form-control form-control-sm" runat="server" TextMode="Date" Height="24" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server">Fecha Salida:</asp:TableCell><asp:TableCell runat="server">
                                        <asp:TextBox ID="txtFechaS" CssClass="form-control form-control-sm" runat="server" TextMode="Date" Height="24" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server">Motivo Salida:</asp:TableCell><asp:TableCell runat="server">
                                        <asp:TextBox ID="TxtMotivo" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="24" Width="300" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server">Sueldo:</asp:TableCell><asp:TableCell runat="server">
                                        <asp:TextBox ID="txtSueldo" CssClass="form-control form-control-sm" runat="server" TextMode="Number" Height="24" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server">Forma Pago:</asp:TableCell><asp:TableCell runat="server">
                                        <asp:DropDownList ID="txtTipoPlan" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="28" Width="150">
                                            <asp:ListItem Selected="True" Value="S">Semanal</asp:ListItem>
                                            <asp:ListItem Value="Q">Quincenal</asp:ListItem>
                                            <asp:ListItem Value="M">Mensual</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server">Activo:</asp:TableCell><asp:TableCell runat="server">
                                        <asp:DropDownList ID="txtActivo" CssClass="form-control form-control-sm" runat="server" TextMode="SingleLine" Height="28" Width="150">
                                            <asp:ListItem Value="A">Activo</asp:ListItem>
                                            <asp:ListItem Value="R">Retirado</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server" ColumnSpan="2">&nbsp</asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server" HorizontalAlign="Right">
                                        <asp:Button ID="btnActualizar" runat="server" CssClass="btn btn-primary btn-sm" />
                                    </asp:TableCell>
                                    <asp:TableCell runat="server" HorizontalAlign="Right">
                                        <asp:ImageButton ID="btnimprimir" ToolTip="Imprimir Ficha" runat="server" Height="28px" ImageUrl="~/imagenes/Printer.png" Width="28px" OnClientClick="window.open('reportes.aspx')" />
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                        <asp:ImageButton Style="padding-top: 10px; position: absolute; top: 10px; right: 30px;" ID="btnSalir" ToolTip="Cancelar" runat="server" Height="28px" ImageUrl="~/imagenes/cancelar.png" />

                        <script src="js/JQuery.js"></script>
                        <p>
                            <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                        </p>

                    </div>

                </div>
            </div>

            <asp:PlaceHolder ID="Documents" runat="server">

                <div class="accordion-item">

                    <h2 class="accordion-header">
                        <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseThree" aria-expanded="false" aria-controls="collapseThree">
                            Documentos
                        </button>
                    </h2>
                    <div id="collapseThree" class="accordion-collapse collapse" data-bs-parent="#accordionExample">
                        <div class="accordion-body">
                            <div class="container-fluid w-100">
                                <div class="row">
                                    <div class="col">
                                        <uc:FileManager class="w-100 " ID="FileManager1" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </asp:PlaceHolder>


        </div>
        <div id="alertPlaceholder" runat="server"></div>

    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" crossorigin="anonymous"></script>
</body>
</html>
