<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" CodeBehind="CorrectContract.ascx.vb" Inherits="Sistema.CorrectContract" %>

<script src="helpers\js\textFormatting.js">

</script>

<div class="container container-fluid ">
    <div class=" align-content-start ">
        <div class="card text-bg-light  border-dark" style="width: 36rem;">
            <div class="card-header">
                Datos del plan adquirido
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <div class="input-group input-group-sm">

                                <asp:TextBox ID="txtprod1" runat="server" CssClass="form-control form-control-sm" placeholder="Producto..." TextMode="SingleLine" OnTextChanged="txtprod1_TextChanged" AutoPostBack="true" />
                                <div class="input-group-append">
                                    <label class="input-group-text input-group-">
                                        <asp:LinkButton ID="btnBuscarProducto" runat="server" CssClass="fas fa-search text-secondary"></asp:LinkButton></label>
                                </div>
                            </div>
                        </div>

                    </div>

                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="input-group input-group-sm">
                                <div class="input-group-prepend">
                                    <label class="input-group-text" style="width: 100px">Cuota</label>
                                </div>
                                <asp:TextBox ID="txtcuotaApp" runat="server" CssClass="form-control form-control-sm" placeholder="Valor Cuota..." TextMode="Number" OnTextChanged="txtvalorcontApp_TextChanged" AutoPostBack="true" />

                            </div>
                        </div>
                        <div class="form-group">

                            <div class="input-group input-group-sm">
                                <div class="input-group-prepend">
                                    <label class="input-group-text" style="width: 100px">N. letras</label>
                                </div>
                                <asp:TextBox ID="txtLetraApp" runat="server" CssClass="form-control form-control-sm" placeholder=".." TextMode="Number" OnTextChanged="txtvalorcontApp_TextChanged" AutoPostBack="true" />
                            </div>
                        </div>


                    </div>
                    <div class="col-md-6">
                        <div class="form-group">

                            <div class="input-group input-group-sm">
                                <div class="input-group-prepend">
                                    <label class="input-group-text" style="width: 100px">Valor</label>
                                </div>
                                <asp:TextBox ID="txtvalorcontApp" runat="server" CssClass="form-control form-control-sm" placeholder="Valor..." TextMode="Number" OnTextChanged="txtvalorcontApp_TextChanged" AutoPostBack="true" />
                            </div>


                            <div class="form-group">
                                <div class="input-group input-group-sm">
                                    <div class="input-group-prepend">
                                        <label class="input-group-text" style="width: 100px">Cantidad</label>
                                    </div>
                                    <asp:TextBox ID="txtcanti1app" runat="server" CssClass="form-control form-control-sm" placeholder="Cant.." TextMode="SingleLine" />
                                </div>
                            </div>

                        </div>
                    </div>

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
