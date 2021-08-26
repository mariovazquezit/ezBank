<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmReportes.aspx.cs" Inherits="ezBank.Views.frmReportes" %>

<%@ Register Src="~/Classes/uc_menuPrincipal.ascx" TagPrefix="uc1" TagName="uc_menuprincipal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:uc_menuprincipal runat="server" id="uc_menuprincipal" />       
    <div class="row">
        <div class="col-md-12 col-xs-12" style="display:flex;align-items:center;flex-direction:column;">
            <h3 class="card-title"> Reportes </h3>
        </div>
        <div class="col-sm-12">
            <div class="card">
                <div class="card-body" style="height:100vh;">
                    <div class="row space">
                        <div class="col-md-3">
                            <label for="inputEmail4" class="form-label">Reporte</label>
                            <asp:DropDownList ID="cmbReporte" runat="server"  CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="cmbReporte_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="col-md-9">
                            <asp:Panel ID="panel_Fechas" runat="server" Visible="true">
                                <div class="row">
                                    <div class="col-md-3 col-xs-12">
                                        <label for="inputEmail4" class="form-label">Fecha Inicial</label>
                                        <asp:TextBox ID="dtp_FechaInicial" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                    </div>

                                    <div class="col-md-3 col-xs-12">      
                                        <label for="inputEmail4" class="form-label">Fecha Final</label>
                                        <asp:TextBox ID="dtp_FechaFinal" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                    </div>


                                      <div class="col-md-3 col-xs-12">      
                                            <label for="inputEmail4" class="form-label text-white">_</label>
                                            <asp:LinkButton ID="btnGenerarReporte" runat="server" CssClass="btn btn-primary btn-block font-btn" OnClick="btnGenerarReporte_Click" >Generar</asp:LinkButton>            
                                        </div>
                                </div>
                                
                            </asp:Panel>
                        </div>
                       
                    </div>
                  
                </div>
        </div>
    </div>
</div>

</asp:Content>
