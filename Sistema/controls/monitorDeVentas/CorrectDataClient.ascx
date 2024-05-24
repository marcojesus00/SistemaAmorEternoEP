<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" CodeBehind="CorrectDataClient.ascx.vb" Inherits="Sistema.DataClient" %>

<script src="helpers\js\textFormatting.js">

</script>

<div class="container container-fluid ">
    <div class=" align-content-start ">
        <div class="card text-bg-light  border-dark" style="width: 36rem;">
            <div class="card-header">
                Datos de cliente
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="input-group input-group-sm">
                                <div class="input-group-prepend">
                                    <label class="input-group-text" style="width: 100px">Identidad</label>
                                </div>
                                <asp:TextBox ID="txtidentiCliapp" onkeypress="formatNumbersWithDashesKeyPress(event, this)" runat="server" CssClass="form-control form-control-sm" placeholder="100319830001" TextMode="SingleLine" />

                            </div>
                        </div>
                        <div class="form-group">

                            <div class="input-group input-group-sm">
                                <div class="input-group-prepend">
                                    <label class="input-group-text" style="width: 100px">Celular</label>
                                </div>
                                <asp:TextBox ID="TextBoxCelular" onkeypress="formatNumbersWithDashesKeyPress(event, this)"  runat="server" CssClass="form-control form-control-sm" placeholder="00000000" TextMode="SingleLine" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="input-group input-group-sm">
                                <div class="input-group-prepend">
                                    <label class="input-group-text" style="width: 100px">Teléfono</label>
                                </div>
                                <asp:TextBox ID="TextBoxPhone" onkeypress="formatNumbersWithDashesKeyPress(event, this)"  runat="server" CssClass="form-control form-control-sm" placeholder="00000000" TextMode="SingleLine" />
                            </div>
                        </div>


                    </div>
                    <div class="col-md-6">
                        <div class="form-group">

                            <div class="input-group input-group-sm">
                                <div class="input-group-prepend">
                                    <label class="input-group-text" style="width: 100px">Depto</label>
                                </div>
                                <asp:DropDownList ID="dlDeptoCliente" runat="server" CssClass="form-control form-control-sm" Style="width: 95px" OnTextChanged="dlDeptoCliente_TextChanged" AutoPostBack="true"></asp:DropDownList>
                            </div>


                            <div class="form-group">
                                <div class="input-group input-group-sm">
                                    <div class="input-group-prepend">
                                        <label class="input-group-text" style="width: 100px">Municipio</label>
                                    </div>
                                    <asp:DropDownList ID="dlCiudadCliente" runat="server" CssClass="form-control form-control-sm" Style="width: 95px" OnTextChanged="dlCiudadCliente_TextChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>
                            </div>
                            
                        <div class="form-group">

                            <div class="input-group input-group-sm">
                                <div class="input-group-prepend">
                                    <label class="input-group-text" style="width: 100px">Prima</label>
                                </div>
                                <asp:TextBox ID="TxtPrimaApp" runat="server" CssClass="form-control form-control-sm" TextMode="Number" Enabled="false" />
                            </div>
                        </div>
                        </div>
                    </div>

                    <div class="col-md-12">
                        <div class="form-group">
                            
                                <div class="input-group input-group-sm">
                                    <p class="list-group-item list-group-item-action bg-light">
                                        <i class="far fa-comment-alt" style="font-size: large; text-align: start"></i>&nbsp Dirección
                                        <asp:TextBox ID="txtdir1Cliapp" TextMode="SingleLine" placeholder="Línea 1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        <asp:TextBox ID="TextBoxAddress2" TextMode="SingleLine" placeholder="Línea 2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        <asp:TextBox ID="TextBoxAddress3" TextMode="SingleLine" placeholder="Línea 3" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>

                                    </p>
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
        formatNumbersWithDashesKeyPress(event,textbox)
    }
</script>