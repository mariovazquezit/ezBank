<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmConfigCatalogos.aspx.cs" Inherits="ezBank.Views.frmConfigCatalogos" %>

<%@ Register Src="~/Classes/uc_menuPrincipal.ascx" TagPrefix="uc1" TagName="uc_menuprincipal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:uc_menuprincipal runat="server" id="uc_menuprincipal" />       
        <div class="row">
            <div class="col-md-12 col-xs-12" style="display:flex;align-items:center;flex-direction:column;">
                <h3 class="card-title"> Configuración de Catálogos </h3>
            </div>
           <div class="col-md-12 col-xs-12">
                <div class="card col-md-12 col-xs-12 space">
                  <div class="card-body d-flex justify-content-around">                    
                      <div class="btn-group" role="group" aria-label="Basic example">

                     <asp:LinkButton ID="btnCatBancos" runat="server" CssClass="btn btn-secondary text-white  btn-font " OnClick="btnCatBancos_Click">Bancos</asp:LinkButton>   
                     <asp:LinkButton ID="btnCatRespuestasCobranza" runat="server" CssClass="btn btn-secondary text-white  btn-font " OnClick="btnCatRespuestasCobranza_Click">Respuestas Cobranza</asp:LinkButton>
                     <asp:LinkButton ID="btnCatOperacionesBBVA" runat="server" CssClass="btn btn-secondary text-white  btn-font" OnClick="btnCatOperacionesBBVA_Click">Operaciones BBVA</asp:LinkButton>                          
                     <asp:LinkButton ID="btnCatConsecutivos" runat="server" CssClass="btn btn-secondary btn-font" OnClick="btnCatConsecutivos_Click">Consecutivos</asp:LinkButton>             
                  </div>
                      </div>
                </div>
               <div class="card card-body col-md-12 col-xs-12" style="display:flex; align-items:center;">
                   <asp:LinkButton ID="btnExcel" runat="server" CssClass="btn btn-success btn-font col-md-2 space" OnClick="btnExcel_Click">Descargar Catalogos</asp:LinkButton>
                   
                   <hr />                   
                   <asp:GridView ID="dgvCatalogos" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" AutoGenerateColumns="True" >
                       <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                  </asp:GridView>
               </div>
         </div>
        </div>

</asp:Content>
