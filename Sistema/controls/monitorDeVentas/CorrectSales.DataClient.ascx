<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" CodeBehind="CorrectSales.DataClient.ascx.vb" Inherits="Sistema.DataClient" %>



<div class="container">
    <div class=" align-content-start ">
        <div class="card text-bg-light  border-dark" style="width: 36rem;">
            <div class="card-header">
                Datos de cliente
            </div>

            <div class="text-center">


                <div class="card-body">


                    <div class="form-group text-center">
                        <form class="row row-cols-lg-auto g-3 p'3 align-items-end">
                            <div class="col-6">
                                <div class="input-group input-group-sm">
                                    <div class="input-group-prepend">
                                        <label class="input-group-text" style="width: 100px">Identidad</label>
                                    </div>
                                    <asp:TextBox ID="txtidentiCliapp" runat="server" CssClass="form-control form-control-sm" placeholder="10031983..." TextMode="SingleLine" />
                                </div>
                            </div>
                            <div class="col-6">
                                <div class="input-group input-group-sm">
                                    <div class="input-group-prepend">
                                        <label class="input-group-text" style="width: 100px">Celular</label>
                                    </div>
                                    <asp:TextBox ID="TextBoxCelular" runat="server" CssClass="form-control form-control-sm" placeholder="95256315" TextMode="SingleLine" />
                                </div>
                            </div>
                            <div class="col-6">
                                <div class="input-group input-group-sm">
                                    <div class="input-group-prepend">
                                        <label class="input-group-text" style="width: 100px">Teléfono</label>
                                    </div>
                                    <asp:TextBox ID="TextBoxPhone" runat="server" CssClass="form-control form-control-sm" placeholder="95256315" TextMode="SingleLine" />
                                </div>
                            </div>
                            <div class="col-6">

                                <div class="input-group input-group-sm">
                                    <div class="input-group-prepend">
                                        <label class="input-group-text" style="width: 100px">Prima</label>
                                    </div>
                                    <asp:TextBox ID="TxtPrimaApp" runat="server" CssClass="form-control form-control-sm" TextMode="Number" Enabled="false" />
                                </div>
                            </div>

                            <div class="col-6">
                                <div class="input-group input-group-sm">
                                    <div class="input-group-prepend">
                                        <label class="input-group-text" style="width: 100px">Depto</label>
                                    </div>
                                    <asp:DropDownList ID="dlDeptoCliente" runat="server" CssClass="form-control form-control-sm" Style="width: 95px" OnTextChanged="dlDeptoCliente_TextChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>

                            </div>
                            <div class="col-6">
                                <div class="input-group input-group-sm">
                                    <div class="input-group-prepend">
                                        <label class="input-group-text" style="width: 100px">Departamento</label>
                                    </div>
                                    <asp:TextBox ID="TextBoxDepartment" runat="server" CssClass="form-control form-control-sm" placeholder="Yoro" TextMode="SingleLine" />
                                </div>
                            </div>
                                                        <div class="col-6">
                                <div class="input-group input-group-sm">
                                    <div class="input-group-prepend">
                                        <label class="input-group-text" style="width: 100px">Municipio</label>
                                    </div>
                                    <asp:TextBox ID="TextBoxCity" runat="server" CssClass="form-control form-control-sm" placeholder="El Progreso" TextMode="SingleLine" />
                                </div>
                            </div>

                            <div class="col-6">
                                <div class="input-group input-group-sm">
                                    <div class="input-group-prepend">
                                        <label class="input-group-text" style="width: 100px">Ciudad</label>
                                    </div>
                                    <asp:DropDownList ID="dlCiudadCliente" runat="server" CssClass="form-control form-control-sm" Style="width: 95px" OnTextChanged="dlCiudadCliente_TextChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>

                            </div>

                            <div class="col-12">
                                <div class="input-group input-group-sm">
                                    <div class="input-group-prepend">
                                        <%--<label class="input-group-text" style="width: 110px">Direccion</label>--%>
                                    </div>
                                    <p class="list-group-item list-group-item-action bg-light">
                                        <i class="far fa-comment-alt" style="font-size: large; text-align: start"></i>&nbsp Direccion<asp:TextBox ID="txtdir1Cliapp" TextMode="MultiLine" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>

                                    </p>
                                    <%--<asp:TextBox ID="txtdir1Cliapp" runat="server" CssClass="form-control form-control-sm" placeholder="Direccion Completa..." TextMode="SingleLine" />--%>
                                </div>

                            </div>


                        </form>
                    </div>


                </div>
            </div>

        </div>
    </div>
</div>



