<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmAfiliaciones.aspx.cs" Inherits="ezBank.Views.frmAfiliaciones" %>

<%@ Register Src="~/Classes/uc_menuPrincipal.ascx" TagPrefix="uc1" TagName="uc_menuprincipal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:uc_menuprincipal runat="server" id="uc_menuprincipal" />
    <div class="row">
        <div class="col-md-12 col-xs-12" style="display:flex;align-items:center;flex-direction:column;">
            <h3>Panel de Afiliaciones</h3>
        </div>
        <div class="col-sm-12">
            <div class="card card-body" style="height:100vh;">
                                                                     
                <div class="space">
                    <div class="form-row space"> 
                                                                        
                        <div class="col-md-3">                           
                            <label class="font-weight-bold">Canal de Afiliacion</label>
                            <asp:DropDownList ID="cmbMetodoAfiliacion" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="cmbMetodoAfiliacion_SelectedIndexChanged">
                                <asp:ListItem> </asp:ListItem>
                                <asp:ListItem>Carga Excel</asp:ListItem>
                                <asp:ListItem>Consulta de Cartera</asp:ListItem>
                                <asp:ListItem>Carga de Respuestas Bancarias</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-3"></div>

                        <div class="col-md-2">  
                            <asp:LinkButton ID="btnNuevaAfiliacion" runat="server" CssClass="btn btn-block btn-success" OnClick="btnNuevaAfiliacion_Click"> <i class="fas fa-plus"></i>Nuevo</asp:LinkButton> 
                            </div>

                           <div class="col-md-2">
                        <asp:LinkButton ID="btnDownloadAfiliadosExitosos" runat="server" CssClass="btn font-btn btn-block btn-download" OnClick="btnDownloadAfiliadosExitosos_Click" > <i class="fas fa-download"></i>Afiliaciones Exitosas</asp:LinkButton>                       
                    </div>
                    <div class="col-md-2">
                        <asp:LinkButton ID="btnDownloadAfiliadosPendientes" runat="server" CssClass="btn btn-block btn-warning" OnClick="btnDownloadAfiliadosPendientes_Click" ><i class="fas fa-download"></i> Pendientes</asp:LinkButton>                       
                    </div>
                      

                    </div>
                    <asp:Panel ID="panelAfiliacion" runat="server" Visible="false">
                        <hr />
                        <div class="form-row"> 
                            <div class="col-md-2">
                                <label>Tipo de Archivo</label>
                                <asp:DropDownList ID="cmbTipoArchivo" runat="server" CssClass="form-control">
                                    <asp:ListItem>Banco a Banco</asp:ListItem>
                                    <asp:ListItem>Interbancario</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                           <div class="col-md-2">
                              <label>Emisora</label>
                               <asp:DropDownList ID="cmbEmisora" runat="server" CssClass="form-control" ></asp:DropDownList>
                            </div>       
                            
                         <div class="col-md-2">
                        <asp:Panel ID="panelCargaCSV" runat="server" Visible="false">                            
                                <label>Archivo para Carga</label>
                                <asp:FileUpload ID="uploadCSVAfiliacion" runat="server" CssClass="form-control" />                           
                        </asp:Panel>
                        </div>


                            <div class="col-md-2">
                                 <label class="text-white">_</label>
                            <asp:LinkButton ID="btnVistaPreviaAfiliaciones" runat="server" CssClass="btn btn-block btn-info" OnClick="btnVistaPreviaAfiliaciones_Click" > <i class="far fa-eye fa-spin"></i> Vista Previa</asp:LinkButton>                       
                                </div>
                            <div class="col-md-1">
                                 <label class="text-white">_</label>
                                 <asp:LinkButton ID="btnGenerarArchivo" runat="server" CssClass="btn btn-primary btn-block" OnClick="btnGenerarArchivo_Click" Visible="False" >Generar</asp:LinkButton>                       
                            </div>
                       </div>
                 </asp:Panel>
                    
                        <asp:Panel ID="panelCargaRespuestas" runat="server" Visible="false">
                    <hr />
                     <div class="form-row"> 
                      <div class="col-md-3">                        
                                <label class="font-weight-bold">Respuesta Bancaria</label>
                                <asp:FileUpload ID="uploadRespuestaBancaria" runat="server" CssClass="form-control" />
                        </div>

                    <div class="col-md-3">                                                
                       <label class="text-white">__</label>                    
                        <asp:LinkButton ID="btnCargarRespuesta" runat="server" CssClass="btn btn-primary btn-block"  Visible="true" OnClick="btnCargarRespuesta_Click" >Cargar</asp:LinkButton>                       
                    </div>
                          <div class="col-md-4"></div>

                          <div class="col-md-2">
                                      <label class="text-white">P</label>
                                    <asp:LinkButton ID="btnDownloadRespuestas" runat="server" CssClass="btn btn-success font-btn btn-block" Visible="false" OnClick="btnDownloadRespuestas_Click" >Descargar</asp:LinkButton>
                                </div>

                    </div>

                            <hr />
                                <asp:GridView ID="dgvRespuestasAfiliacion" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" AutoGenerateColumns="True" >
                        <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                     </asp:GridView>

                  

                </asp:Panel>    

                </div>
            
                <asp:Panel ID="panelBancoDocumentacion" runat="server" Visible="false">
                        <div class="card bg-light">          
                            <div class="card-body">
                                 <label class="text-success font-weight-bold">Banco a Banco: Sólo considera Créditos con CLABE de Banorte</label>                                 
                                 <label class="text-success font-weight-bold">Interbancario: Considera Créditos con CLABE de todos los Bancos, a excepción de Banorte</label>                                 
                            </div>
                            </div>      
                    </asp:Panel>
                
                <div class="card">
                <asp:Panel ID="panelPreview" runat="server" visible="false">
                    <h5 class="text-primary">Pendientes por Afiliar</h5>
                     <asp:GridView ID="dgvPendientesAfiliacion" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" AutoGenerateColumns="True" >
                        <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                     </asp:GridView>
                    </asp:Panel>
                   </div>


                           

            </div>
        </div>
     </div>  

</asp:Content>
    