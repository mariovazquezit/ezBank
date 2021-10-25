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
                            <div class="col-md-6">
                             </div>
                             <div class="col-md-3">
                                 <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">LinkButton</asp:LinkButton>
                                   <asp:LinkButton ID="btnNuevoCobranza" runat="server" CssClass="btn btn-block btn-success" OnClick="btnNuevoCobranza_Click"> <i class="fas fa-plus"></i>Nuevo</asp:LinkButton> 
                             </div>
                       
                        </div>

                        <asp:Panel ID="panelEstrategiaCobranza" runat="server" Visible="false">                            
                            <div class="card">
                                <div class="card-body">                                
                            <div class="form-row"> 
                                <div class="col-md-2">
                                    <label>Producto</label>
                                        <asp:DropDownList ID="cmbProducto" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="cmbProducto_SelectedIndexChanged" >                  
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3">
                                <label>Convenio/Dependencia</label>
                                    <asp:ListBox ID="cmbConvenio" runat="server" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
                                <%--<asp:DropDownList ID="cmbConvenio" runat="server" CssClass="form-control" SelectionMode="Multiple" AutoPostBack="True" OnSelectedIndexChanged="cmbConvenio_SelectedIndexChanged">                  
                                </asp:DropDownList>--%>
                           </div>

                                <div class="col-md-3">
                                    <label>Proximo Pago (Desde Hoy hasta:)</label>
                                    <asp:ListBox ID="cmbProximoPago" SelectionMode="Multiple" CssClass="form-control" runat="server"></asp:ListBox>
                                    <%--<asp:DropDownList ID="cmbProximoPago" runat="server" CssClass="form-control" SelectionMode="Multiple" AutoPostBack="True" >                  
                                    </asp:DropDownList>--%>
                                </div>
                                <div class="col-md-2">
                                    <label>Días desde último Pago</label>
                                   <asp:TextBox ID="txtDiasUltimoPago" runat="server" CssClass="form-control" TextMode="Number" min="0" >120</asp:TextBox>
                                </div>

<%--                                  <div class="col-md-2">
                                    <label>Máximo días de Atraso</label>
                                   <asp:TextBox ID="txtDiasAtraso" runat="server" CssClass="form-control" TextMode="Number" >0</asp:TextBox>
                                </div>--%>

                                  <div class="col-md-2">
                                    <label>Banco del Cliente</label>
                                      <asp:ListBox ID="cmbBancoInicial" runat="server" SelectionMode="Multiple" CssClass="form-control">
                                            <asp:ListItem>Todos</asp:ListItem>
                                          <asp:ListItem>Banorte</asp:ListItem>
                                         <asp:ListItem>Banamex</asp:ListItem>
                                         <asp:ListItem>BBVA Bancomer</asp:ListItem>
                                         <asp:ListItem>Santander</asp:ListItem>
                                          <asp:ListItem>Tarjeta</asp:ListItem>
                                          <asp:ListItem>Otros</asp:ListItem>
                                      </asp:ListBox>
                                  <%--<asp:DropDownList ID="cmbBancoInicial" runat="server" CssClass="form-control">                                                      
                                    <asp:ListItem Selected>Todos</asp:ListItem>
                                      <asp:ListItem>Banorte</asp:ListItem>
                                     <asp:ListItem>Banamex</asp:ListItem>
                                     <asp:ListItem>BBVA Bancomer</asp:ListItem>
                                     <asp:ListItem>Santander</asp:ListItem>
                                      <asp:ListItem>Tarjeta</asp:ListItem>
                                      <asp:ListItem>Otros</asp:ListItem>
                                </asp:DropDownList>--%>
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
                              </div>
                            </div>
                                                  

                            <div class="card">
                                        <div class="card-body bg-light">
                                            <h6 class="text-primary font-weight-bold">Particiones por Cuota</h6>
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
                                                                                                           
    </asp:Panel>

                     
                        <asp:Panel ID="panelPreviewDomiciliacion" runat="server" Visible="true">

                                        <asp:Panel ID="panelBancoCobranza" runat="server" Visible="false">
                        <div class="card">
                            <div class="card-body">                            
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
                                    <asp:DropDownList ID="cmbBancoEmisora" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="cmbBancoEmisora_SelectedIndexChanged" >                                                                                 
                                    </asp:DropDownList> 
                                    <asp:Label ID="lblEmisora" runat="server" CssClass="text-primary font-weight-bold"></asp:Label>
                                </asp:Panel>
                           </div>

                              <div class="col-md-2">
                                <asp:Panel ID="panelTipodeCobro" runat="server" Visible="false">
                                    <label>Tipo de Cobro</label>
                                    <asp:DropDownList ID="cmbTipoCobro" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="cmbTipoCobro_SelectedIndexChanged" >                                                                                 
                                    </asp:DropDownList> 
                                    <asp:Label ID="lblTipoCobro" runat="server" CssClass="text-primary font-weight-bold"></asp:Label>
                                </asp:Panel>
                           </div>

                            
                            <div class="col-md-2">                                
                                <asp:Panel ID="panelModalidad" runat="server" Visible="false">
                                    <label>Método de Cobro</label>
                                    <asp:DropDownList ID="cmbMetodoCobro" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="cmbMetodoCobro_SelectedIndexChanged" >                                                                                 
                                        <asp:ListItem>Cuenta</asp:ListItem>
                                        <asp:ListItem Selected>CLABE</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="lblMetodoCobro" runat="server" CssClass="text-primary font-weight-bold"></asp:Label>
                                 </asp:Panel>
                           </div>
                                <div class="col-md-2">                                
                                    <asp:Panel ID="panelArchivoSalida" runat="server" Visible="false">
                                    <label>Archivo de Salida</label>
                                    <asp:DropDownList ID="cmbArchivoSalida" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="cmbArchivoSalida_SelectedIndexChanged" >                                                                                 
                                         <asp:ListItem Selected>Global</asp:ListItem>                
                                        <asp:ListItem>Banco a Banco</asp:ListItem>
                                        <asp:ListItem>Interbancario</asp:ListItem>
                                    </asp:DropDownList>    
                                        <asp:Label ID="lblArchivoSalida" runat="server" CssClass="text-primary font-weight-bold"></asp:Label>
                                        </asp:Panel>
                           </div>
                                

                             <div class="col-md-2">
                                <asp:Panel ID="panelHoraSantander" runat="server" Visible="false">
                                    <label>Hora de Ejecucion</label>
                                    <asp:DropDownList ID="cmbHoraSantander" runat="server" CssClass="form-control" >                                                                                                                         
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
                                    </asp:DropDownList>                 
                                </asp:Panel>
                           </div>
                            </div>
                                </div>
                            </div>

                            <div class="card">
                                <div class="card-body">  
                                     <div class="form-row">  
                            <div class="col-md-2">
                                 <asp:Panel ID="panelCargaCSV" runat="server" Visible="false">                                
                                <label>Archivo para Carga</label>
                                <asp:FileUpload ID="uploadCSVCobranza" runat="server" CssClass="form-control" />  
                                <asp:LinkButton ID="btnDescargaEjemploCobranzaLayout" runat="server" CssClass="btn btn-primary btn-block" OnClick="btnDescargaEjemploCobranzaLayout_Click" >  <i class="fas fa-question"></i> Descargar un Ejemplo</asp:LinkButton>
                            </asp:Panel>
                                </div>


                                      
                                            <div class="col-md-10">   
                                       <div class="btn-group" role="group" aria-label="Basic example">
                                <asp:LinkButton ID="btnPreviewEstrategia" runat="server" CssClass="btn btn-info" OnClick="btnPreviewEstrategia_Click">  <i class="far fa-eye fa-spin"></i> Vista Previa</asp:LinkButton>                                             
                                <asp:LinkButton ID="btnValidaEstrategia" runat="server" CssClass="btn btn-dark" visible="false" OnClick="btnValidaEstrategia_Click">  <i class="far fa-eye fa-spin"></i> Valida Estrategia</asp:LinkButton>                                            
                                <asp:LinkButton ID="btnConstruirArchivo" runat="server" Visible="false" CssClass="btn btn-secondary " OnClick="btnConstruirArchivo_Click" > Construir Archivo</asp:LinkButton>                              
                                <asp:LinkButton ID="btnValidacionExcel" runat="server" CssClass="btn btn-info" Visible="False" OnClick="btnValidacionExcel_Click" >  Validación en Excel </asp:LinkButton>
                                <asp:LinkButton ID="btnGenerarCobranza" runat="server" CssClass="btn btn-success " OnClick="btnGenerarCobranza_Click" Visible="False">Descargar Archivo</asp:LinkButton>
                                           </div>
                                                <asp:Label ID="lblAlertaTotales" runat="server" CssClass="text-primary font-weight-bold"></asp:Label>
                            </div>

                        </div>
                            </div>
                        </div>
                    </asp:Panel>

                            
                     
                        <asp:Panel ID="panelPreviewEstrategia" runat="server" Visible="false">
                             <div class="card">
                                <div class="card-body" >
                                 
                      <asp:Label ID="lblFILENAME" runat="server" CssClass="text-primary font-weight-bold"></asp:Label>
                    <%--<h5 class="text-primary">Vista Previa de Estrategia de Cobranza</h5>--%>
                     <asp:GridView ID="dgvPreviewEstrategia" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" AutoGenerateColumns="True" >
                        <HeaderStyle CssClass="thead-dark" />
                         <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                     </asp:GridView>

                     <asp:GridView ID="dgvBodyCobranza" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" AutoGenerateColumns="True" >
                        <HeaderStyle CssClass="thead-dark" />
                                      <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                     </asp:GridView>

                    <asp:GridView ID="dgvFooterCobranza" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" AutoGenerateColumns="True" >
                        <HeaderStyle CssClass="thead-dark" />
                                      <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                     </asp:GridView>
                    </div>
                </div>
                        </asp:Panel>

                        </asp:Panel>


                             <asp:Panel ID="panelRespuestasCobranza" runat="server" visible="false">
                                 
                             <div class="form-row">                    
                            <div class="col-md-3">
                            
                                  <label class="font-weight-bold">Respuesta de Banco</label>
                                <asp:DropDownList ID="cmbBancoRespuesta" runat="server" CssClass="form-control">                                                      
                                    <asp:ListItem>Banamex</asp:ListItem>
                                    <asp:ListItem>Banorte En Linea</asp:ListItem>
                                     <asp:ListItem>Banorte Especializada</asp:ListItem>
                                     <asp:ListItem>BBVA Bancomer</asp:ListItem>
                                     <asp:ListItem>Santander</asp:ListItem> 
                                     <asp:ListItem>Santander Validaciones</asp:ListItem>
                                     <asp:ListItem>Tarjeta</asp:ListItem>
                                    <asp:ListItem>BX+</asp:ListItem>
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
                                

                                   <asp:GridView ID="dgvRespuestasCobranza" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" AutoGenerateColumns="True" >
                        <HeaderStyle CssClass="thead-dark" />
                                       <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                     </asp:GridView>


                                  

                        </asp:Panel>



                    </div>
                </div>
            </div>
        </div>



</asp:Content>
