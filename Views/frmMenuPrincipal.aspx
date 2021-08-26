<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmMenuPrincipal.aspx.cs" Inherits="ezBank.Views.frmMenuPrincipal" %>

<%@ Register Src="~/Classes/uc_menuPrincipal.ascx" TagPrefix="uc1" TagName="uc_menuprincipal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:uc_menuprincipal runat="server" id="uc_menuprincipal" />       

    <div class="row">
        <div class="col-md-12 col-xs-12" style="display:flex;align-items:center;flex-direction:column;">
            <h3>Bienvenido al Sistema ezBank - Conciliador de Cartera</h3>
            <asp:Label ID="lblUsuario" runat="server" Cssclass="text-primary font-weight-bold" Font-Bold="True"></asp:Label>
            <hr />
        </div>
        <div class="col-md-12 col-xs-12" style="display:flex;justify-content:center;">
            <div class="col-md-6 col-xs-12 card card-body" style="display:flex; align-items:center;">
                <h3 class="text-primary">Proyectado a 7 días</h3>
                <asp:GridView ID="dgvExigible" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" AutoGenerateColumns="True">
                    <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                </asp:GridView>
             </div>
            <div class="col-md-6 col-xs-12">
                <div class="row" style="margin: 2px;">
                    <div class="col-md-6 col-xs-12">
                        <div class="card">
                            <div class="card-header">
                                <h5>Afiliados Totales</h5>
                            </div>
                            <div class="card-body">
                            <h2><asp:Label ID="lblAfiliadosTotalCreditos" runat="server" cssClass="text-success text-center"></asp:Label></h2>
                        </div>
                        </div>
                    </div>
                    <div class="col-md-6 col-xs-12">
                        <div class="card">
                            <div class="card-header">
                                <h5>Pendientes por Afiliar</h5>
                            </div>
                            <div class="card-body">
                                <h2><asp:Label ID="lblAfiliadosPendientes" runat="server" cssClass="text-danger"></asp:Label></h2>
                            </div>
                        </div>
                     </div>
                </div>

                <div class="col-sm-12 col-xs-12" style="margin-top: 5%;">
                    <div class="card card-body">
                        <asp:literal ID="GraficoCalendario" runat="server"></asp:literal>
                    </div>
                 </div>
            </div>
        </div>
     </div>
    </div> <!-- end content-wrapper-->
    </div> <!-- end contentr-->
    </div> <!-- end wrapper-->
</asp:Content>
