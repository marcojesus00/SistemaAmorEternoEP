<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="reportes.aspx.vb" Inherits="Sistema.reportes" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Reportes</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <link rel="shortcut icon" type="image/x-icon" href="~/imagenes/logo.ico" />
    <link href="css/BootStrap.min.css" rel="stylesheet" type="text/css" />
    <link href="css/3.3/BootStrap.min.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid" style="padding-top: 5px;">
            <asp:Table ID="Table1" runat="server">
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:Label ID="Label1" runat="server" Text="Desde la Fecha:" />
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:TextBox ID="txtFecha1" runat="server" TextMode="Date" CssClass="form-control form-control-sm" Width="200" Height="26" Font-Size="Small"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:Label ID="Label2" runat="server" Text="Hasta la Fecha:" />
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:TextBox ID="txtFecha2" runat="server" TextMode="Date" CssClass="form-control form-control-sm" Width="200" Height="26" Font-Size="Small"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:Label ID="Label3" runat="server" Text="Codigo Cobrador:" />
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:TextBox ID="txtCobrador" runat="server" CssClass="form-control form-control-sm" Width="200" Height="24" Font-Size="Small" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:Label ID="Label4" Visible="false" runat="server" Text="Cobrador:" />
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:TextBox ID="txtlider" Visible="false" runat="server" CssClass="form-control form-control-sm" Width="200" Height="24" Font-Size="Small" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:Label ID="Label5" runat="server" Text="Agrupado por:" Visible="false" />
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:DropDownList ID="dlAgrupa" runat="server" AutoPostBack="true" Visible="false" CssClass="form-control form-control-sm" Width="200" Height="27" Font-Size="Small" OnTextChanged="dlAgrupa_TextChanged"></asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server"></asp:TableCell>

                    <asp:TableCell runat="server">
                        <asp:Button ID="btnEjecutar" runat="server" Text="Generar" CssClass="btn btn-dark btn-sm" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:Panel ID="pnlDialog" runat="server" CssClass="dialog-panel pt-4" Visible="False">
                <div class="row">
                    <div class="col-3">
                        <div class="row">

<%--                            <div class="col-3">

                                <asp:Label CssClass="" ID="lblMessage" runat="server" Text="Cobrador:"></asp:Label>


                            </div>--%>
                            <div class="col-6">
                                <asp:TextBox style="max-height:stretch" ID="txtCodigoCObrador" PlaceHolder="Código cobrador" runat="server"></asp:TextBox>


                            </div>
                            <div class="col-4">

                                <asp:Button ID="btnSubmit" runat="server" Text="Ver reporte" CssClass="btn btn-dark btn-sm" OnClick="btnSubmit_Click" />


                            </div>
                        </div>
                        <asp:Label CssClass="" ID="lblError" runat="server"></asp:Label>

                    </div>

                </div>


            </asp:Panel>
            <asp:ImageButton ID="btnRegresar" runat="server" Height="30px" ImageUrl="~/imagenes/atras.png" Width="30px" Style="position: fixed; right: 10px; top: 10px;" />
            <CR:CrystalReportViewer ID="crvInformes" runat="server" AutoDataBind="true" HasCrystalLogo="False" Height="50px" PrintMode="ActiveX" ToolPanelView="None" Width="350px" />
        </div>
        <asp:Label ID="lblMsg" runat="server">

        </asp:Label>
        <script src="js/JQuery.js"></script>
        <script src="js/popper.min.js"></script>
        <script src="js/bootstrap.min.js"></script>
    </form>
</body>
</html>
