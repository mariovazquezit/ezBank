<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmCargas.aspx.cs" Inherits="ezBank.Views.frmCargas" %>

<%@ Register Src="~/Classes/uc_menuPrincipal.ascx" TagPrefix="uc1" TagName="uc_menuprincipal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <uc1:uc_menuprincipal runat="server" id="uc_menuprincipal" />       
     <div class="row">
         <div class="col-md-12 col-xs-12" style="display:flex;align-items:center;flex-direction:column;">
            <h3 class="card-title"> Carga Masiva de Datos </h3>
        </div>
           <div class="col-sm-12">
                <div class="card">
                    <div class="card-body" style="height:100vh;">
                        <div class="row">
                            <div class="col-md-3 col-xs-12">
                                <label for="inputEmail4" class="form-label">Archivo</label>
                                <asp:DropDownList ID="cmbTipoArchivo" runat="server"  CssClass="form-control" AutoPostBack="True" >
                                <asp:ListItem>Cartera</asp:ListItem>                                
                                <asp:ListItem>Respuestas de Cobranza Banorte Especializada</asp:ListItem>
                                <asp:ListItem>Respuestas de Cobranza BBVA Bancomer</asp:ListItem>
                                <asp:ListItem>CLABES</asp:ListItem>
                                <asp:ListItem>Excepciones de Pago</asp:ListItem>
                            </asp:DropDownList>
                            </div>
                            <div class="col-md-3 col-xs-12" style="display:flex;align-items:flex-end;">
                                <asp:FileUpload ID="fileupload_archivo" runat="server" CssClass="form-control"/>
                            </div>
                            <div class="col-md-3 col-xs-12" style="display:flex;align-items:flex-end;">
                                <asp:LinkButton ID="btnCargar" runat="server" CssClass="btn btn-primary btn-font btn-block" OnClick="btnCargar_Click" >Cargar</asp:LinkButton>
                            </div>
                            <div class="col-md-3 col-xs-12" style="display:flex;align-items:flex-end;">
                                <asp:LinkButton ID="btnDescargaEjemplo" runat="server" CssClass="btn btn-info btn-font btn-block" OnClick="btnDescargaEjemplo_Click" >Descargar un Ejemplo</asp:LinkButton>            
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
