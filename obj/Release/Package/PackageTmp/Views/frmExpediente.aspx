<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmExpediente.aspx.cs" Inherits="ezBank.Views.frmExpediente" %>

<%@ Register Src="~/Classes/uc_menuPrincipal.ascx" TagPrefix="uc1" TagName="uc_menuprincipal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <uc1:uc_menuprincipal runat="server" id="uc_menuprincipal" />       
     <div class="row">
        <div class="col-sm-12">
            <div class="card">
                <div class="card-body">
                    <div class="form-row">
                        <div class="col-md-3 col-sx-3" style="display:flex;align-items:center;">
                            <label>Buscar Cliente por:</label>
                         </div>

                        <div class="col-md-3 col-sx-3">
                          <asp:DropDownList ID="cmbBuscarCliente" runat="server" CssClass="form-control">
                              <asp:ListItem>Credito</asp:ListItem>
                              <asp:ListItem>Cliente Unico</asp:ListItem>
                              <asp:ListItem>Nombre</asp:ListItem>                              
                           </asp:DropDownList>
                         </div>
                        <div class="col-md-3 col-sx-3">
                             <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3 col-sx-3">
                              <asp:LinkButton ID="btnBuscarCliente" runat="server" CssClass="btn btn-primary btn-block font-btn" OnClick="btnBuscarCliente_Click"> <i class="fas fa-search-plus"></i> Buscar</asp:LinkButton>
                        </div>
                    </div>
                </div>  
            </div>
              
      

            <asp:Panel ID="panelbusquedaAvanzada" runat="server" Visible="false">
                 <hr />
                 <asp:GridView ID="dgvBusquedaAvanzada" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" AutoGenerateSelectButton="True" >
                            <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                        </asp:GridView>
            </asp:Panel>

             <hr />
            <asp:Panel ID="panelExpedientebuttons" runat="server" Visible="false">
             <div class="card">
              <div class="card-body">
             <asp:LinkButton ID="btnPanelExpediente" runat="server" CssClass="btn btn-primary" OnClick="btnPanelExpediente_Click">Información</asp:LinkButton> 
                  <asp:LinkButton ID="btnPanelAfiliacion" runat="server" CssClass="btn btn-primary" OnClick="btnPanelAfiliacion_Click">Afiliación</asp:LinkButton>   
                  <asp:LinkButton ID="btnPanelCobranza" runat="server" CssClass="btn btn-primary" OnClick="btnPanelCobranza_Click">Cobranza</asp:LinkButton>   
                  <asp:LinkButton ID="btnPanelPagos" runat="server" CssClass="btn btn-primary" OnClick="btnPanelPagos_Click">Pagos</asp:LinkButton>   
                  <asp:LinkButton ID="btnListaNegra" runat="server" CssClass="btn btn-info" OnClick="btnListaNegra_Click">Exclusión</asp:LinkButton>   
                  <asp:LinkButton ID="btnCLABEAdicional" runat="server" CssClass="btn btn-info" OnClick="btnCLABEAdicional_Click">CLABEs</asp:LinkButton>   
                  <asp:LinkButton ID="btnBitacora" runat="server" CssClass="btn btn-info" OnClick="btnBitacora_Click">Bitácora</asp:LinkButton>   
            </div>
    </div>
                </asp:Panel>

            <asp:Panel ID="panelCLABEAdicional" runat="server" Visible="false">
            <div class="card">
                <div class="card-body bg-light">
                 
                    <div class="form-row">
                        <div class="col-md-4">
    <label>CLABE Adicional:</label>           
     <asp:TextBox ID="txtCLABEAdicional" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>    
               </div>
                        <div class="col-md-5">
    <label>Comentarios:</label>            
     <asp:TextBox ID="txtCLABEComentarios" runat="server" CssClass="form-control" MaxLength="99"></asp:TextBox>    
                           </div>
                        <div class="col-md-3">
                             <label class="text-light">__</label>  
     <asp:LinkButton ID="btnCLABEAdicionalGUARDAR" runat="server" CssClass="btn btn-success btn-lg btn-block" OnClick="btnCLABEAdicionalGUARDAR_Click">Agregar</asp:LinkButton>    
            </div>
                        </div>
                </div>
            </div>                
                </asp:Panel>

            <asp:Panel ID="panelListaNegra" runat="server" Visible="false">
               <div class="card">
                <div class="card-body bg-light">
                 
                    <div class="form-row">
                        <div class="col-md-4">
    <label>Excluir Cobro hasta :</label>           
     <asp:TextBox ID="txtBlackList" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>    
               </div>
                       
                        <div class="col-md-3">
                             <label class="text-light">__</label>  
     <asp:LinkButton ID="btnBlackListGUARDAR" runat="server" CssClass="btn btn-success btn-lg btn-block" OnClick="btnBlackListGUARDAR_Click">Agregar</asp:LinkButton>    
            </div>
                        </div>
                </div>
            </div>   
                </asp:Panel>


            <asp:Panel ID="panelBitacora" runat="server" Visible="false">
            <div class="card">
                <div class="card-body bg-light">
                
                         <div class="form-row">
                        <div class="col-md-4">
    <label>Accion:</label>           
                            <asp:DropDownList ID="txtBitacoraAccion" runat="server" CssClass="form-control">
                                <asp:ListItem>Seguimiento</asp:ListItem>
                            </asp:DropDownList>
               </div>
                        <div class="col-md-5">
    <label>Comentarios:</label>            
     <asp:TextBox ID="txtBitacoraComentarios" runat="server" CssClass="form-control"></asp:TextBox>    
                           </div>
                        <div class="col-md-3">
                             <label class="text-light">__</label>  
     <asp:LinkButton ID="btnBitacoraGUARDAR" runat="server" CssClass="btn btn-success btn-lg btn-block" OnClick="btnBitacoraGUARDAR_Click">Agregar</asp:LinkButton>    
            </div>
                        </div>
                
                </div>
            </div>   
                </asp:Panel>

</div> <!-- end content-wrapper-->
</div> <!-- end contentr-->



    <asp:Panel ID="panelExpediente" runat="server" Visible="false">
           <div class="row">
           <div class="col-sm-12">
    
      <div class="card">
        <div class="card-header">
                    <h4 class="text-primary">General</h4>
            </div>
                        <div class="card-body">
       <div class="form-row"> 
           <div class="col-md-1">
    <label>Solicitud:</label>
     <asp:TextBox ID="txtSolicitud" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>
           <div class="col-md-1">
            <label>Crédito:</label>
         <asp:TextBox ID="txtCredito" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>
           <div class="col-md-2">
                <label>Cliente Único:</label>
         <asp:TextBox ID="txtClienteUnico" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>
           <div class="col-md-4">
              <label>Nombre:</label>
         <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>
           </div>
            <hr />
                   <div class="form-row"> 
           <div class="col-md-3">
            <label>Dependencia:</label>
         <asp:TextBox ID="txtDependencia" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>
           <div class="col-md-3">
        <label>SubDependencia:</label>
         <asp:TextBox ID="txtSubDependencia" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
               </div>
           <div class="col-md-3">
            <label>CLABE:</label>
         <asp:TextBox ID="txtCLABE" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>         
             <div class="col-md-2">
            <label>Banco:</label>
         <asp:TextBox ID="txtBanco" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>     
            <div class="col-md-2">
            <label>Sucursal:</label>
         <asp:TextBox ID="txtSucursal" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>     
      </div>
         </div>
          </div>

    <div class="card">
        <div class="card-header">
                    <h4 class="text-primary">Financiamiento</h4>
        </div>

        <div class="card-body">
       <div class="form-row"> 
           <div class="col-md-2">
    <label>Producto:</label>
     <asp:TextBox ID="txtProducto" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>
           <div class="col-md-2">
            <label>SubProducto:</label>
         <asp:TextBox ID="txtSubProducto" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>
           <div class="col-md-2">
                <label>Frecuencia:</label>
         <asp:TextBox ID="txtFrecuencia" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>
           <div class="col-md-2">
              <label>Otorgado:</label>
         <asp:TextBox ID="txtOtorgado" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>
           <div class="col-md-2">
            <label>Amort. Totales:</label>
         <asp:TextBox ID="txtAmortTotales" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>
           <div class="col-md-2">
        <label>Cuota x Amort.:</label>
         <asp:TextBox ID="txtCuotaxAmort" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
               </div>
           <div class="col-md-2">
            <label>Desembolso:</label>
         <asp:TextBox ID="txtDesembolso" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>
          <div class="col-md-2">
            <label>Amort. Pagadas:</label>
         <asp:TextBox ID="txtAmortPagadas" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>
              <div class="col-md-2">
            <label>% Avance:</label>
         <asp:TextBox ID="txtAvance" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>
      </div>




        </div>
               </div>


                   <div class="card">
        <div class="card-header">
                    <h4 class="text-primary">Comportamiento</h4>
        </div>

        <div class="card-body">
       <div class="form-row"> 
           <div class="col-md-1">
    <label>Estatus:</label>
     <asp:TextBox ID="txtEstatus" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>
           <div class="col-md-2">
            <label>1er Pago Teorico:</label>
         <asp:TextBox ID="txtPrimerPagoTeorico" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>
           <div class="col-md-2">
                <label>Ultimo Pago Teorico:</label>
         <asp:TextBox ID="txtUltimoPagoTeorico" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>
           <div class="col-md-2">
              <label>Dias Atraso:</label>
         <asp:TextBox ID="txtDiasAtraso" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>
           <div class="col-md-2">
            <label>Saldo Vencido:</label>
         <asp:TextBox ID="txtSaldoVencido" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>
           <div class="col-md-2">
        <label>Ultimo Pago Aplicado:</label>
         <asp:TextBox ID="txtUltimoPagoAplicado" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
               </div>
           <div class="col-md-2">
        <label>Monto Ultimo Pago:</label>
         <asp:TextBox ID="txtMontoUltimoPago" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
               </div>
           <div class="col-md-2">
            <label>Dias desde Ultimo Pago:</label>
         <asp:TextBox ID="txtDiasUltimoPago" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>
          <div class="col-md-2">
            <label>Siguiente Pago:</label>
         <asp:TextBox ID="txtSiguientePago" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>    
               </div>            
      </div>




        </div>
               </div>
               <div class="form-row">
               <div class="col-md-3">
                <div class="card">
                   <div class="card-header">
                        <h5 class="text-primary">Afiliaciones</h5>
                   </div>
                   <div class="card-body">
                         <asp:GridView ID="dgvAfiliaciones" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" >
                            <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                        </asp:GridView>
                   </div>
               </div>
                   </div>

               <div class="col-md-4">
                    <div class="card">
                   <div class="card-header">
                        <h5 class="text-primary">Histórico de Créditos</h5>
                   </div>
                   <div class="card-body">
                         <asp:GridView ID="dgvMulticreditos" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" >
                            <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                        </asp:GridView>
                   </div>
               </div>
               </div>

                    <div class="col-md-5">
                      <div class="card">
                   <div class="card-header">
                        <h5 class="text-primary">CLABEs Adicionales</h5>
                   </div>
                   <div class="card-body">
                         <asp:GridView ID="dgvCLABE" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" >
                            <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                        </asp:GridView>
                   </div>
               </div>
                   </div>

                </div>

            

                <div class="card">
                   <div class="card-header">
                        <h4 class="text-primary">Exclusiones</h4>
                   </div>
                   <div class="card-body">
                         <asp:GridView ID="dgvExclusiones" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" >
                            <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                        </asp:GridView>
                   </div>
               </div>

                  <div class="card">
                   <div class="card-header">
                        <h4 class="text-primary">Bitacora</h4>
                   </div>
                   <div class="card-body">
                         <asp:GridView ID="dgvBitacora" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" >
                            <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                        </asp:GridView>
                   </div>
               </div>

  </div>
               </div>
</asp:Panel>


    <asp:Panel ID="panelLogDomiciliacion" runat="server" Visible="false">
     <div class="card">
                   <div class="card-header">
                        <h4 class="text-primary">Histórico de Cobranza</h4>
                   </div>
                   <div class="card-body">
                         <asp:GridView ID="dgvLogDomiciliacion" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" >
                            <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                        </asp:GridView>
                   </div>
               </div>
    </asp:Panel>



     <asp:Panel ID="panelLogPagos" runat="server" Visible="false">
     <div class="card">
                   <div class="card-header">
                        <h4 class="text-primary">Histórico de Pagos</h4>
                   </div>
                   <div class="card-body">
                         <asp:GridView ID="dgvLogPagos" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" >
                            <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                        </asp:GridView>
                   </div>
               </div>
    </asp:Panel>


       <asp:Panel ID="panelAfiliacion" runat="server" Visible="false">
     <div class="card">
                   <div class="card-header">
                        <h4 class="text-primary">Histórico de Afiliaciones</h4>
                   </div>
                   <div class="card-body">
                         <asp:GridView ID="dgvAfiliacionRespuestas" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" >
                            <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                        </asp:GridView>
                   </div>
               </div>
    </asp:Panel>


</asp:Content>
