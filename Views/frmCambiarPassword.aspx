<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmCambiarPassword.aspx.cs" Inherits="ezBank.Views.frmCambiarPassword" %>

<%@ Register Src="~/Classes/uc_menuPrincipal.ascx" TagPrefix="uc1" TagName="uc_menuprincipal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:uc_menuprincipal runat="server" id="uc_menuprincipal" />       

    <div class="row">
        <div class="col-md-12 col-xs-12" style="display:flex;align-items:center;flex-direction:column;">
            <h3 class="card-title"> Perfil </h3>
        </div>
        <div class="col-md-12 col-xs-12">
            <div class="row d-flex justify-content-center">
                <div class="card col-md-4 col-xs-12" style="margin-right:3%;">
                  <div class="card-body">        
                        <div class="form-row" style="display:flex;flex-direction:column;">
                            <div class="col-md-12 col-xs-12 d-flex justify-content-center">
                                <i class="far fa-user-circle" style="font-size:10em;color:#3B3BB6" ></i>
                            </div>
                            <div class="col-md-12 col-xs-12 space">
                              <label>Id</label>
                                <asp:TextBox ID="txtID" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col-md-12 col-xs-12 space">
                              <label>Usuario</label>
                              <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col-md-12 col-xs-12 space">
                              <label>Nombre</label>
                              <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col-md-12 col-xs-12">
                                <label>Perfil</label>
                                <asp:TextBox ID="txtPerfil" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>                                   
                            </div>
                
                        </div>    
                    </div>
                </div>
                <div class="card col-md-4 col-xs-12">
                    <div class="card-body"style="display:flex;justify-content:center;align-items:center;">
                        <div class="form-row">
                            <div class="col-md-12 col-xs-12 space">
                                <label>Password Anterior</label>
                                <asp:TextBox ID="txtPasswordAnterior" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>                                   
                            </div>   
                            <div class="col-md-12 col-xs-12 space">
                                <label>Password Nuevo</label>
                                <asp:TextBox ID="txtPasswordNuevo" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>                                   
                            </div>   
                            <div class="col-md-12 col-xs-12 d-flex justify-content-center">
                                <label class="text-white space">_</label>
                                <asp:LinkButton ID="btnCambiarPassword" runat="server" CssClass="btn btn-primary col-md-8 btn-font btn-block" OnClick="btnCambiarPassword_Click">Cambiar Contraseña</asp:LinkButton>      
                            </div>
                        </div>
                     </div>
                </div>
            </div>

        </div>
     </div>
</div> <!-- end content-wrapper-->
</div> <!-- end contentr-->
</div> <!-- end wrapper-->
</asp:Content>
