<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmCobranza.aspx.cs" Inherits="ezBank.Views.frmCobranza" %>

<%@ Register Src="~/Classes/uc_menuPrincipal.ascx" TagPrefix="uc1" TagName="uc_menuprincipal" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:uc_menuprincipal runat="server" id="uc_menuprincipal" />       
        <div class="row">
            <div class="col-md-12 col-xs-12" style="display:flex;align-items:center;flex-direction:column;">
                <h3 class="card-title"> Cobranza Domiciliada  </h3>
            </div>
            <div class="col-sm-12">
                <div class="card">
                    <div class="card-body"style="height:100vh;">
                        <div class="form-row"> 
                          <div class="col-md-3">
                              <label class="font-weight-bold">Canal de Cobranza</label>
                                <asp:DropDownList ID="cmbMetodoCobranza" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="cmbMetodoCobranza_SelectedIndexChanged" >
                                    <asp:ListItem> </asp:ListItem>
                                    <asp:ListItem>Carga Excel</asp:ListItem>
                                    <asp:ListItem>Consulta de Cartera</asp:ListItem>
                                    <asp:ListItem>Carga de Respuestas Bancarias</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                       
                        </div>

                        <asp:Panel ID="panelEstrategiaCobranza" runat="server" Visible="false">
                            <hr />
                            <div class="form-row"> 
                                <div class="col-md-2">
                                    <label>Producto</label>
                                        <asp:DropDownList ID="cmbProducto" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="cmbProducto_SelectedIndexChanged" >                  
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3">
                                <label>Convenio/Dependencia</label>
                                <asp:DropDownList ID="cmbConvenio" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="cmbConvenio_SelectedIndexChanged">                  
                                </asp:DropDownList>
                           </div>

                                <div class="col-md-2">
                                    <label>Proximo Pago</label>
                                    <asp:DropDownList ID="cmbProximoPago" runat="server" CssClass="form-control" AutoPostBack="True" >                  
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <label>Días desde último Pago</label>
                                   <asp:TextBox ID="txtDiasUltimoPago" runat="server" CssClass="form-control" TextMode="Number" >120</asp:TextBox>
                                </div>

                                  <div class="col-md-2">
                                    <label>Máximo días de Atraso</label>
                                   <asp:TextBox ID="txtDiasAtraso" runat="server" CssClass="form-control" TextMode="Number" >0</asp:TextBox>
                                </div>

                                  <div class="col-md-2">
                                    <label>Banco del Cliente</label>
                                  <asp:DropDownList ID="cmbBancoInicial" runat="server" CssClass="form-control">                                                      
                                    <asp:ListItem Selected>Todos</asp:ListItem>
                                      <asp:ListItem>Banorte</asp:ListItem>
                                     <asp:ListItem>Banamex</asp:ListItem>
                                     <asp:ListItem>BBVA Bancomer</asp:ListItem>
                                     <asp:ListItem>Santander</asp:ListItem>
                                      <asp:ListItem>Tarjeta</asp:ListItem>
                                      <asp:ListItem>BX+</asp:ListItem>
                                      <asp:ListItem>Otros</asp:ListItem>
                                </asp:DropDownList>
                                 </div>

                                <div class="col-md-3">
                                    <label>Estrategia</label>
                                    <asp:DropDownList ID="cmbEstrategia" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="cmbEstrategia_SelectedIndexChanged">                  
                                        <asp:ListItem Selected></asp:ListItem> 
                                        <asp:ListItem>Cuota Actual</asp:ListItem>
                                         <asp:ListItem>Vencido + Cuota Actual</asp:ListItem>
                                         <asp:ListItem>Vencido</asp:ListItem>
                                    </asp:DropDownList>
                               </div>
                                                             
                           </div>                               
                   

                       
                            <hr />
                            <h6 class="text-primary font-weight-bold">Particiones por Cuota</h6>

                            <div class="card">
                                        <div class="card-body bg-light">
                                     <div class="form-row"> 

                           <div class="col-md-2">
                                <asp:Panel ID="panelParticionCuotaActual" runat="server" Visible="false">
                                    <label class="text-primary">Cuota Actual</label>
                                    <asp:DropDownList ID="cmbParticionCuotaActual" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="cmbParticionCuotaActual_SelectedIndexChanged"  >                                                          
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem>Sin Particion</asp:ListItem>
                                        <asp:ListItem>60/40</asp:ListItem>
                                        <asp:ListItem>70/30</asp:ListItem>
                                        <asp:ListItem>80/20</asp:ListItem>
                                        <asp:ListItem>90/10</asp:ListItem>
                                    </asp:DropDownList>
                                    </asp:Panel>
                                </div>
                        
                                               
                                    <div class="col-md-2">   
                                        <asp:Panel ID="panelCuotasVencidas" runat="server" Visible="false">                                                                                                          
                                      <label class="text-primary">Vencidas por Cobrar</label>
                                        <asp:DropDownList ID="txtCuotasVencidas" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="txtCuotasVencidas_SelectedIndexChanged"  >                                                           
                                            <asp:ListItem>0</asp:ListItem> 
                                            <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                    </asp:DropDownList>                                      
                                        </asp:Panel>
                                </div>
                                                    
                                <div class="col-md-2">   
                                    <asp:Panel ID="panelCuotaVencida1" runat="server" Visible="false">
                                      <label class="text-primary">Cuota Vencida 1</label>
                                        <asp:DropDownList ID="cmbParticionVencida1" runat="server" CssClass="form-control" OnSelectedIndexChanged="cmbParticionVencida1_SelectedIndexChanged" AutoPostBack="True"  >                  
                                         <asp:ListItem></asp:ListItem>
                                            <asp:ListItem>Sin Particion</asp:ListItem>
                                        <asp:ListItem>60/40</asp:ListItem>
                                        <asp:ListItem>70/30</asp:ListItem>
                                        <asp:ListItem>80/20</asp:ListItem>
                                        <asp:ListItem>90/10</asp:ListItem>
                                        </asp:DropDownList>
                                        </asp:Panel>
                                </div>
                                <div class="col-md-2">   
                                    <asp:Panel ID="panelCuotaVencida2" runat="server" Visible="false">
                                      <label class="text-primary">Cuota Vencida 2</label>
                                        <asp:DropDownList ID="cmbParticionVencida2" runat="server" CssClass="form-control" OnSelectedIndexChanged="cmbParticionVencida2_SelectedIndexChanged" AutoPostBack="True"  >                                                          
                                     <asp:ListItem></asp:ListItem>
                                            <asp:ListItem>Sin Particion</asp:ListItem>
                                        <asp:ListItem>60/40</asp:ListItem>
                                        <asp:ListItem>70/30</asp:ListItem>
                                        <asp:ListItem>80/20</asp:ListItem>
                                        <asp:ListItem>90/10</asp:ListItem>
                                        </asp:DropDownList>
                                        </asp:Panel>
                                </div>
                                         <div class="col-md-2">   
                                             <asp:Panel ID="panelCuotaVencida3" runat="server" Visible="false">
                                      <label class="text-primary">Cuota Vencida 3</label>
                                        <asp:DropDownList ID="cmbParticionVencida3" runat="server" CssClass="form-control" OnSelectedIndexChanged="cmbParticionVencida3_SelectedIndexChanged" AutoPostBack="True"  >                                                          
                                     <asp:ListItem></asp:ListItem>
                                            <asp:ListItem>Sin Particion</asp:ListItem>
                                        <asp:ListItem>60/40</asp:ListItem>
                                        <asp:ListItem>70/30</asp:ListItem>
                                        <asp:ListItem>80/20</asp:ListItem>
                                        <asp:ListItem>90/10</asp:ListItem>
                                        </asp:DropDownList>
                                                  </asp:Panel>
                                </div>
                                         <div class="col-md-2">   
                                             <asp:Panel ID="panelCuotaVencida4" runat="server" Visible="false">
                                      <label class="text-primary">Cuota Vencida 4</label>
                                        <asp:DropDownList ID="cmbParticionVencida4" runat="server" CssClass="form-control" OnSelectedIndexChanged="cmbParticionVencida4_SelectedIndexChanged" AutoPostBack="True"  >                                                      
                                     <asp:ListItem></asp:ListItem>
                                            <asp:ListItem>Sin Particion</asp:ListItem>
                                        <asp:ListItem>60/40</asp:ListItem>
                                        <asp:ListItem>70/30</asp:ListItem>
                                        <asp:ListItem>80/20</asp:ListItem>
                                        <asp:ListItem>90/10</asp:ListItem>
                                        </asp:DropDownList>
                                                 </asp:Panel>
                                </div>
                                      
                                   



                                            </div>
                                         </div>
                                         </div>
                        <hr />
                                                                                                           
    </asp:Panel>

                     
                        <asp:Panel ID="panelPreviewDomiciliacion" runat="server" Visible="true">

                                        <asp:Panel ID="panelBancoCobranza" runat="server" Visible="false">
                        <hr />    
                        <div class="form-row">                    
                            <div class="col-md-2">
                                <label>Banco para Domiciliación</label>
                                <asp:DropDownList ID="cmbBancoCobranza" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="cmbBancoCobranza_SelectedIndexChanged">                  
                                    <asp:ListItem Selected>Seleccionar</asp:ListItem>                
                                    <asp:ListItem>Banorte</asp:ListItem>
                                     <asp:ListItem>Banamex</asp:ListItem>
                                     <asp:ListItem>BBVA Bancomer</asp:ListItem>
                                     <asp:ListItem>Santander</asp:ListItem>
                                     <asp:ListItem>Tarjeta</asp:ListItem>
                                    <asp:ListItem>BX+</asp:ListItem>                                  
                                </asp:DropDownList>
                             </div>
              
                            <div class="col-md-2">
                                <asp:Panel ID="panelEmisoraBanco" runat="server" Visible="false">
                                    <label>Emisora</label>
                                    <asp:DropDownList ID="cmbBancoEmisora" runat="server" CssClass="form-control" >                                                                                 
                                    </asp:DropDownList>                 
                                </asp:Panel>
                           </div>

                             <div class="col-md-2">
                                <asp:Panel ID="panelHoraSantander" runat="server" Visible="false">
                                    <label>Hora de Ejecucion</label>
                                    <asp:DropDownList ID="cmbHoraSantander" runat="server" CssClass="form-control" >                                                                                 
                                        <asp:ListItem>00</asp:ListItem>
                                        <asp:ListItem>01</asp:ListItem>
                                        <asp:ListItem>02</asp:ListItem>
                                        <asp:ListItem>03</asp:ListItem>
                                        <asp:ListItem>04</asp:ListItem>
                                        <asp:ListItem>05</asp:ListItem>
                                        <asp:ListItem>06</asp:ListItem>
                                        <asp:ListItem>07</asp:ListItem>
                                        <asp:ListItem>08</asp:ListItem>
                                        <asp:ListItem>09</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                        <asp:ListItem>13</asp:ListItem>
                                        <asp:ListItem>14</asp:ListItem>
                                        <asp:ListItem>15</asp:ListItem>
                                        <asp:ListItem>16</asp:ListItem>
                                        <asp:ListItem>17</asp:ListItem>
                                        <asp:ListItem>18</asp:ListItem>
                                        <asp:ListItem>19</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
                                        <asp:ListItem>21</asp:ListItem>
                                        <asp:ListItem>22</asp:ListItem>
                                        <asp:ListItem>23</asp:ListItem>
                                    </asp:DropDownList>                 
                                </asp:Panel>
                           </div>

                            <div class="col-md-2">
                                 <asp:Panel ID="panelCargaCSV" runat="server" Visible="false">                                
                                <label>Archivo para Carga</label>
                                <asp:FileUpload ID="uploadCSVCobranza" runat="server" CssClass="form-control" />                                
                            </asp:Panel>
                                </div>


                            <div class="col-md-2">   
                                      <label class="text-white">P</label>
                                      <asp:LinkButton ID="btnPreviewEstrategia" runat="server" CssClass="btn btn-info font-btn btn-block" OnClick="btnPreviewEstrategia_Click">  <i class="far fa-eye fa-spin"></i> Vista Previa</asp:LinkButton>
                                </div>                             

                            <div class="col-md-2" style="display:flex;align-items:flex-end;">
                                <asp:LinkButton ID="btnGenerarCobranza" runat="server" CssClass="btn btn-success font-btn btn-block" OnClick="btnGenerarCobranza_Click" Visible="False">Generar Layout</asp:LinkButton>
                            </div>

                        </div>

                    </asp:Panel>

                            <hr />
                     
                        <asp:Panel ID="panelPreviewEstrategia" runat="server" Visible="false">
                             <div class="card">
                                 
                    <h5 class="text-primary">Vista Previa de Estrategia de Cobranza</h5>
                     <asp:GridView ID="dgvPreviewEstrategia" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" AutoGenerateColumns="True" >
                        <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                     </asp:GridView>
                </div>
                        </asp:Panel>

                        </asp:Panel>


                             <asp:Panel ID="panelRespuestasCobranza" runat="server" visible="false">
                                 <hr />
                             <div class="form-row">                    
                            <div class="col-md-3">
                            
                                  <label class="font-weight-bold">Respuesta de Banco</label>
                                <asp:DropDownList ID="cmbBancoRespuesta" runat="server" CssClass="form-control">                                                      
                                    <asp:ListItem>Banamex</asp:ListItem>
                                    <asp:ListItem>Banorte En Linea</asp:ListItem>
                                     <asp:ListItem>Banorte Especializada</asp:ListItem>
                                     <asp:ListItem>BBVA Bancomer</asp:ListItem>
                                     <asp:ListItem>Santander</asp:ListItem>                                    
                                </asp:DropDownList>
                             </div>
                                <div class="col-md-2">
                                    <label class="text-white">P</label>
                                    <asp:FileUpload ID="fuRespuestaCobranza" runat="server" />
                                </div>
                                  <div class="col-md-3">
                                      <label class="text-white">P</label>
                                    <asp:LinkButton ID="btnRespuestaCobranza" runat="server" CssClass="btn btn-primary font-btn btn-block" OnClick="btnRespuestaCobranza_Click">Carga</asp:LinkButton>
                                </div>
                                  <div class="col-md-2">
                                     
                                </div>
                                  <div class="col-md-2">
                                      <label class="text-white">P</label>
                                    <asp:LinkButton ID="btnDownloadRespuestas" runat="server" CssClass="btn btn-success font-btn btn-block" Visible="false" OnClick="btnDownloadRespuestas_Click">Descargar</asp:LinkButton>
                                </div>
                            </div>    
                                 <hr />

                                   <asp:GridView ID="dgvRespuestasCobranza" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" AutoGenerateColumns="True" >
                        <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                     </asp:GridView>

                        </asp:Panel>



                    </div>
                </div>
            </div>
        </div>



</asp:Content>
