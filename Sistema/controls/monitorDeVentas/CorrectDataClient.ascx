<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" CodeBehind="CorrectDataClient.ascx.vb" Inherits="Sistema.DataClient" %>

<script src="helpers\js\textFormatting.js">

</script>

<div class="container-fluid text-bg-light ">
    <div class="row pt-2">
        <div class="col-lg-6 pb-2">
            <div class="card text-bg-light  border-dark">
                <div class="card-header">
                    Datos de cliente
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-6">
                            <div class="form-group">
                                <div class="input-group input-group-sm">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text"  style="min-width: 100px;">Identidad</span>
                                    </div>
                                    <asp:TextBox ID="txtidentiCliapp"  runat="server" CssClass="form-control form-control-sm" placeholder="0801..." TextMode="SingleLine" />

                                </div>
                            </div>
                            <div class="form-group">

                                <div class="input-group input-group-sm">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text"  style="min-width: 100px;">Celular</span>
                                    </div>
                                    <asp:TextBox ID="TextBoxCelular"  runat="server" CssClass="form-control form-control-sm" placeholder="8000..." TextMode="SingleLine" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="input-group input-group-sm">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text"  style="min-width: 100px;">Teléfono</span>
                                    </div>
                                    <asp:TextBox ID="TextBoxPhone"  runat="server" CssClass="form-control form-control-sm" placeholder="9000..." TextMode="SingleLine" />
                                </div>
                            </div>


                        </div>
                        <div class="col-lg-6">
                            <div class="form-group">

                                <div class="input-group input-group-sm">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text"  style="min-width: 100px;">Depto</span>
                                    </div>
                                    <asp:DropDownList ID="dlDeptoCliente" runat="server" CssClass="form-control form-control-sm" OnTextChanged="dlDeptoCliente_TextChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>


                                <div class="form-group">
                                    <div class="input-group input-group-sm">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"  style="min-width: 100px;">Municipio</span>
                                        </div>
                                        <asp:DropDownList ID="dlCiudadCliente" runat="server" CssClass="form-control form-control-sm" OnTextChanged="dlCiudadCliente_TextChanged" ></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">

                                    <div class="input-group input-group-sm">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"  style="min-width: 100px;">Prima</span>
                                        </div>
                                        <asp:TextBox ID="TxtPrimaApp" runat="server" CssClass="form-control form-control-sm" TextMode="Number" Enabled="false" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-12 p-2">
                            <div class="form-group">

                                <div class="input-group input-group-sm">
                                    <div class="row">
                                        <i class="far fa-comment-alt pb-1" style="font-size: large; text-align: start">Dirección </i>
                                    </div>
                                    <div class="row p-1">
                                        <asp:TextBox ID="txtdir1Cliapp" TextMode="SingleLine" placeholder="Línea 1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        <asp:TextBox ID="TextBoxAddress2" TextMode="SingleLine" placeholder="Línea 2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        <asp:TextBox ID="TextBoxAddress3" TextMode="SingleLine" placeholder="Línea 3" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>

                                    </div>

                                </div>
                            </div>

                        </div>

                    </div>


                </div>
            </div>

        </div>

        <div class="col-lg-6">
            <div class="card text-bg-light mb-6 border-dark">
                <div class="card-header">
                    Datos del plan adquirido
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <div class="input-group input-group-sm">

                                    <asp:TextBox ID="textBoxProductNombre1" runat="server" CssClass="form-control form-control-sm" placeholder="Producto..." TextMode="SingleLine" OnTextChanged="txtprod1_TextChanged" AutoPostBack="true" />
                                    <div class="input-group-append">
                                        <span class="input-group-text input-group-">
                                            <asp:LinkButton ID="btnBuscarProducto" runat="server" CssClass="bi bi-search"></asp:LinkButton></span>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="col-lg-6">
                            <div class="form-group">
                                <div class="input-group input-group-sm">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text"  style="min-width: 100px;">Cuota</span>
                                    </div>
                                    <asp:TextBox ID="textBoxCuotaContratoApp" runat="server" CssClass="form-control form-control-sm" placeholder="Valor cuota..." TextMode="Number" OnTextChanged="txtvalorcontApp_TextChanged" AutoPostBack="true" />

                                </div>
                            </div>
                            <div class="form-group">

                                <div class="input-group input-group-sm">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text"  style="min-width: 100px;">N. letras</span>
                                    </div>
                                    <asp:TextBox ID="textBoxLetraContratoApp" runat="server" CssClass="form-control form-control-sm" placeholder=".." TextMode="Number" OnTextChanged="txtvalorcontApp_TextChanged" AutoPostBack="true" />
                                </div>
                            </div>


                        </div>
                        <div class="col-lg-6">
                            <div class="form-group">

                                <div class="input-group input-group-sm">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text"  style="min-width: 100px;">Valor</span>
                                    </div>
                                    <asp:TextBox ID="textBoxValorContratoApp" runat="server" CssClass="form-control form-control-sm" placeholder="Valor..." TextMode="Number" OnTextChanged="txtvalorcontApp_TextChanged" AutoPostBack="true" />
                                </div>


                                <div class="form-group">
                                    <div class="input-group input-group-sm">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"  style="min-width: 100px;">Cantidad</span>
                                        </div>
                                        <asp:TextBox ID="textBoxCantidadProducto1app" runat="server" CssClass="form-control form-control-sm" placeholder="0..." TextMode="SingleLine" />
                                    </div>
                                </div>

                            </div>
                        </div>

                    </div>


                </div>
            </div>
            <hr class="p-2" />
                                <asp:Button ID="ButtonManySalesSameClient" OnClick="ButtonManySalesSameClient_Click" runat="server" Text="Corregir muchas ventas mismo cliente" CssClass="btn btn-sm btn-danger" />

            <div class="row justify-content-end pt-md-5 pr-1">
                <div class="col-auto">
                    <asp:Button ID="btnGuardarCamb" runat="server" Enabled="true" Text="   Salvar   " CssClass="btn btn-sm btn-success" />
                </div>
                <div class="col-auto">
                    <asp:Button ID="btnCanModalCl" runat="server" Text="   Cancelar   " CssClass="btn btn-sm btn-danger" />

                </div>

            </div>
        </div>
    </div>
</div>








<script type="text/javascript">
    function formatIDKeyPress(event, textbox) {
        formatNumbersWithDashesKeyPress(event, textbox)
    }
</script>
