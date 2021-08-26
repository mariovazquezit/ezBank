<%@ Page Title="ezBank" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ezBank._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    
<div class="row d-flex justify-content-center">
    <div class="col-md-12" style="height:10vh;"></div>
    <div class="col-sm-4 col-xs-12">
        <div class="card">
            <div class="card-header">
                <h2 class="text-center nexa-b txt-color">
                    ezBank
                </h2>
            </div>
            <div class="card-body">
                <div class="row d-flex justify-content-center">
                    <div class="input-icons col-md-10 col-xs-12">
                        <i class="far fa-user icon"></i>
				        <asp:TextBox ID="txtUsuario" runat="server" CssClass="input-field" PlaceHolder="Mi Usuario"></asp:TextBox>						
		            </div>
                </div>
                <div class="row d-flex justify-content-center">
                    <div class="input-icons col-md-10 col-xs-12">
					    <i class="fas fa-key icon"></i>
					    <asp:TextBox ID="txtPassword" runat="server" CssClass="input-field" TextMode="Password" Placeholder="Mi Contraseña"></asp:TextBox>
				    </div>
                 </div>
                <div class="row d-flex justify-content-center">
                    <div class="">
                        <asp:LinkButton ID="btnInicioSesion" runat="server" CssClass="btn btn-cotinue" OnClick="btnInicioSesion_Click">
                            Iniciar sesión
                        </asp:LinkButton>
                    </div>
                </div>

                </div>
                <div class="card-footer">
                   <div class="d-flex justify-content-center text-dark">
                        <label class="text-success" style="font-style:italic; color:#4C4C4C !important">Conciliador de Cartera en la Nube</label>                    
                    </div>
                 </div>
             </div>
             <asp:Panel ID="panelUsuarioinvalido" runat="server" Visible="false">
             <div class="alert alert-danger d-flex align-items-center" role="alert">
                 <i class="fas fa-exclamation-triangle fa-lg fa-spin"></i>
                 <div>
                    Usuario o Contraseña inválidos
                 </div>
                  
             </div>
             </asp:Panel>
                    
        </div>

</div>

</asp:Content>
