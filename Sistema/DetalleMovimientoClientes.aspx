<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DetalleMovimientoClientes.aspx.vb" Inherits="Sistema.DetalleMovimientoClientes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
             <div class="row">
                   <div class="col">
                       <asp:GridView ID="gvDetalle" runat="server" CssClass="table table-sm table-bordered table-hover" AutoGenerateColumns="False">                           
                           <Columns>
                               <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-edit text-primary" EditText="" ShowEditButton="True">
                                   <ControlStyle CssClass="fas fa-edit text-primary"></ControlStyle>

                                   <ItemStyle HorizontalAlign="Center"></ItemStyle>
                               </asp:CommandField>
                               <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-trash-alt text-danger" DeleteText="" ShowDeleteButton="True">
                                   <ControlStyle CssClass="fas fa-trash-alt text-danger"></ControlStyle>

                                   <ItemStyle HorizontalAlign="Center"></ItemStyle>
                               </asp:CommandField>
                               <asp:TemplateField HeaderText="Codigo">                                  
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Codigo") %>' ID="Label1"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Producto">
                                   <EditItemTemplate>
                                       <asp:TextBox CssClass="form-control form-control-sm" runat="server" Text='<%# Bind("Producto") %>' ID="gvtxtProducto"></asp:TextBox>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Producto") %>' ID="Label2"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Cantidad">
                                   <EditItemTemplate>
                                       <asp:TextBox CssClass="form-control form-control-sm" TextMode="Number" runat="server" Text='<%# Bind("Cantidad") %>' ID="gvtxtCantidad"></asp:TextBox>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Cantidad") %>' ID="Label3"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Precio">
                                   <EditItemTemplate>
                                       <asp:TextBox CssClass="form-control form-control-sm" TextMode="Number" runat="server" Text='<%# Bind("PrecioConDes") %>' ID="gvtxtPrecio"></asp:TextBox>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("PrecioConDes") %>' ID="Label4"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="% Descuento ">
                                   <EditItemTemplate>
                                       <asp:TextBox CssClass="form-control form-control-sm" TextMode="Number" runat="server" Text='<%# Bind("Descuento") %>' ID="gvtxtDescuento"></asp:TextBox>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Descuento") %>' ID="Label5"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Impuesto">
                                   <EditItemTemplate>
                                       <asp:DropDownList CssClass="form-control form-control-sm" runat="server" Text='<%# Bind("Impuesto") %>' ID="gvtxtImpuesto" DataSourceID="SqlDataSource1" DataTextField="Descripcion" DataValueField="Descripcion">
                                       </asp:DropDownList>
                                       <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:SERPConnectionString %>' SelectCommand="SELECT Descripcion, Porcentaje
                 FROM Impuestos
"></asp:SqlDataSource>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Impuesto") %>' ID="Label6"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Total">
                                   <EditItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Total") %>' ID="gvtxtTotal"></asp:Label>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Total") %>' ID="Label7"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>

                           </Columns>
                       </asp:GridView>
                   </div>
               </div>
        </div>
    </form>
</body>
</html>
