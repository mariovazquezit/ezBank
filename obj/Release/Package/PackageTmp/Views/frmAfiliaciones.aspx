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
                        
                            <div class="card">
                            <div class="card-body">
                                <div class="form-row"> 
                            <div class="col-md-2">
                                <label>Tipo de Archivo</label>
                                <asp:DropDownList ID="cmbTipoArchivo" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="cmbTipoArchivo_SelectedIndexChanged">
                                    <asp:ListItem Selected>Global</asp:ListItem>
                                    <asp:ListItem>Banco a Banco</asp:ListItem>
                                    <asp:ListItem>Interbancario</asp:ListItem>
                                </asp:DropDownList>
                                <asp:Label ID="lblTipoArchivo" runat="server" CssClass="text-primary font-weight-bold"></asp:Label>
                            </div>
                              <div class="col-md-2">
                              <label>Afiliar por</label>
                               <asp:DropDownList ID="cmbAfiliarClabeCuenta" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="cmbAfiliarClabeCuenta_SelectedIndexChanged" >
                                   <asp:ListItem Selected>CLABE</asp:ListItem>
                                    <asp:ListItem>Cuenta</asp:ListItem>
                                   </asp:DropDownList>
                                  <asp:Label ID="lblAfiliarPor" runat="server" CssClass="text-primary font-weight-bold"></asp:Label>
                            </div>    
                           <div class="col-md-2">
                              <label>Emisora</label>
                               <asp:DropDownList ID="cmbEmisora" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="cmbEmisora_SelectedIndexChanged" ></asp:DropDownList>
                               <asp:Label ID="lblNombreEmisora" runat="server" CssClass="text-primary font-weight-bold"></asp:Label>
                            </div>    
                             <div class="col-md-2">
                              <label>Siguiente Afiliacion</label>
                               <asp:TextBox ID="txtIdAfiliacion" runat="server" min="1" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                 <asp:Label ID="lblSiguienteAfiliacion" runat="server" CssClass="text-secondary font-weight-bold" Text="Consecutivo recomendado"></asp:Label>
                            </div>   
                            
                      <div class="col-md-2">
                        <asp:Panel ID="panelCargaCSV" runat="server" Visible="false">  
                                <label>Archivo para Carga</label>  
                                <asp:FileUpload ID="uploadCSVAfiliacion" runat="server" CssClass="form-control" />     
                                  <asp:LinkButton ID="btnAfiliacionEjemploLayout" runat="server" CssClass="btn btn-primary" OnClick="btnAfiliacionEjemploLayout_Click" ><i class="fas fa-question"></i> Descargar un ejemplo</asp:LinkButton>                       
                        </asp:Panel>
                    </div>


                            <div class="col-md-2">
                                 <label class="text-white">_</label>
                            <asp:LinkButton ID="btnVistaPreviaAfiliaciones" runat="server" CssClass="btn btn-block btn-info" OnClick="btnVistaPreviaAfiliaciones_Click" > <i class="far fa-eye fa-spin"></i> Vista Previa</asp:LinkButton>                       
                                </div>
                           </div>
                                </div>
                                </div>

                         <div class="card">
                                <div class="card-body bg-light">
                            <div class="col-md-12">
                                
                                <div class="btn-group" role="group" aria-label="Basic example">
                                <asp:LinkButton ID="btnGenerarArchivo" runat="server" CssClass="btn btn-secondary" OnClick="btnGenerarArchivo_Click" Visible="False" > <i class="fas fa-hammer"></i> Construir Archivo </asp:LinkButton>                                
                                    <asp:LinkButton ID="btnValidacionExcel" runat="server" CssClass="btn btn-info" Visible="False" OnClick="btnValidacionExcel_Click"  > <i class="far fa-file-excel"></i> Validación en Excel </asp:LinkButton>
                                    <asp:LinkButton ID="btnDescargarArchivo" runat="server" CssClass="btn btn-success" Visible="False" OnClick="btnDescargarArchivo_Click" > <i class="far fa-smile"></i> Descargar Archivo </asp:LinkButton>
                                </div>
                                
                                <asp:Label ID="lblAlertaTotales" runat="server" CssClass="text-primary font-weight-bold"></asp:Label>
                                </div>
                                     </div>
                            </div>
                            
                 </asp:Panel>
                    
                        <asp:Panel ID="panelCargaRespuestas" runat="server" Visible="false">
                    <div class="card">
                        <div class="card-body">

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
                   </div>
                    </div>


                            
                                <asp:GridView ID="dgvRespuestasAfiliacion" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" AutoGenerateColumns="True" >
                        <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                     </asp:GridView>

                  

                </asp:Panel>    

                </div>
            
          <%--      <asp:Panel ID="panelBancoDocumentacion" runat="server" Visible="false">
                        <div class="card bg-light">          
                            <div class="card-body">
                                 <label class="text-success font-weight-bold">Banco a Banco: Sólo considera Créditos con CLABE de Banorte</label>   
                                <hr />
                                 <label class="text-success font-weight-bold">Interbancario: Considera Créditos con CLABE de todos los Bancos, a excepción de Banorte</label>                                 
                            </div>
                            </div>      
                    </asp:Panel>--%>
                
                <div class="card">
                    <div class="card-body">
                <asp:Panel ID="panelPreview" runat="server" visible="false">
                    
                    <asp:Label ID="lblFileName" runat="server" CssClass="text-success font-weight-bold"></asp:Label>

                     <asp:GridView ID="dgvPendientesAfiliacion" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" AutoGenerateColumns="True" >
                         <HeaderStyle CssClass="thead-dark" />
                        <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                     </asp:GridView>

                    <asp:GridView ID="dgvAfiliacionBody" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" AutoGenerateColumns="True" >
                         <HeaderStyle CssClass="thead-dark" />
                        <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                     </asp:GridView>

                    </asp:Panel>
                    </div>
                   </div>


                           

            </div>
        </div>
     </div>  

</asp:Content>
    