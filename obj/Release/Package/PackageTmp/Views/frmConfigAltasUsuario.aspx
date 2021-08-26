<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmConfigAltasUsuario.aspx.cs" Inherits="ezBank.Views.frmConfigAltasUsuario" %>

<%@ Register Src="~/Classes/uc_menuPrincipal.ascx" TagPrefix="uc1" TagName="uc_menuprincipal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:uc_menuprincipal runat="server" id="uc_menuprincipal" />       
    <div class="row">
        <div class="col-md-12 col-xs-12" style="display:flex;align-items:center;flex-direction:column;">
            <h3 class="card-title"> Altas de Usuario </h3>
        </div>
        <div class="col-sm-12">
            <div class="card">
                <div class="card-body" style="height:100vh;">
                    <div class="row d-flex justify-content-end" style="margin-right: 2%;">
                        <asp:LinkButton ID="btnNuevo" runat="server" CssClass="btn btn-success btn-font btn-block col-md-2" OnClick="btnNuevo_Click">Nuevo</asp:LinkButton>      
                    </div>
                    <asp:Panel ID="panel_NuevoUsuario" runat="server" Visible="false">
                        <div class="col-md-12 col-xs-12 space">
                            <asp:Panel ID="panel_AlertaGuardar" runat="server" Visible="false">
                                <div class="alert alert-success" role="alert">
                                    ¡Usuario actualizado Satisfactoriamente!
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="panel_AlertaError" runat="server" Visible="false">
                                <div class="alert alert-danger" role="alert">
                                    Error: El usuario no pudo ser registrado. Intente de Nuevo.
                                </div>
                            </asp:Panel>

                            <div class="form-row" style="margin-top: 3%;" >
                                <div class="col-md-2">
                                    <label>Id</label>
                                    <asp:TextBox ID="txtID" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <label>Usuario</label>
                                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" ></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <label>Nombre</label>
                                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" ></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <label>Password</label>
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" ></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                        <label>Perfil</label>
                                        <asp:DropDownList ID="cmbPerfil" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                <div class="col-md-2">
                                    <label>Estatus</label>
                                    <asp:DropDownList ID="cmbEstatus" runat="server" CssClass="form-control">
                                        <asp:ListItem>Activo</asp:ListItem>
                                        <asp:ListItem>Inactivo</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-12 col-xs-12 d-flex justify-content-end" style="margin-top: 3%;">
                                    <label class="text-white ">_</label>
                                    <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn col-md-2 btn-primary btn-font btn-block btn-space" OnClick="btnGuardar_Click">Guardar</asp:LinkButton>      
                                    <asp:LinkButton ID="btnEditar" runat="server" visible="false" CssClass="btn col-md-2 btn-warning btn-font btn-space" OnClick="btnEditar_Click">Editar</asp:LinkButton>      
                                    <asp:LinkButton ID="btnEliminar" runat="server" visible="false"  CssClass="btn col-md-2 btn-danger btn-font " OnClientClick="return confirm('¿Estás seguro de que quieres eliminar a este Usuario?');" OnClick="btnEliminar_Click" >Eliminar</asp:LinkButton>      
                                </div>
                            </div>
                         </div>
                    </asp:Panel>
                    <div class="col-md-12 col-xs-12 space d-flex justify-content-center">
                        <asp:GridView ID="dgvAltasUsuario" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" AutoGenerateColumns="True" AutoGenerateSelectButton="True" OnSelectedIndexChanged="dgvAltasUsuario_SelectedIndexChanged">
                            <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                        </asp:GridView>
                    </div>
                    

                </div>
            </div>
        </div>
    </div>

</asp:Content>
