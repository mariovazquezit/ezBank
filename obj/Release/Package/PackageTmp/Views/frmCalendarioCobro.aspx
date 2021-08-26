<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmCalendarioCobro.aspx.cs" Inherits="ezBank.Views.frmCalendarioCobro" %>

<%@ Register Src="~/Classes/uc_menuPrincipal.ascx" TagPrefix="uc1" TagName="uc_menuprincipal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:uc_menuprincipal runat="server" id="uc_menuprincipal" />       
    <div class="row">
        <div class="col-md-12 col-xs-12" style="display:flex;align-items:center;flex-direction:column;">
            <h3 class="card-title"> Calendario de Cobro </h3>
        </div>
        <div class="col-md-12 col-xs-12">
            <div class="card card-body">
                <div class="row d-flex justify-content-end space">
                    <asp:Label ID="lblUsuario" runat="server" Cssclass="text-primary font-weight-bold" Font-Bold="True"></asp:Label>
                    <asp:LinkButton ID="btnNuevo" runat="server" CssClass="btn btn-success btn-block col-md-2 font-btn" OnClick="btnNuevo_Click">Nuevo</asp:LinkButton>      
                    <asp:LinkButton ID="btnDownExcel" runat="server" CssClass="btn btn-download col-md-2 font-btn" OnClick="btnDownExcel_Click"  >Descargar una Copia</asp:LinkButton>
                </div>
                <div class="row d-flex justify-content-center">
                    <asp:Panel ID="panel_AlertaGuardar" runat="server" Visible="false">
                        <div class="alert alert-success" role="alert">
                            ¡Evento actualizado en el Calendario Satisfactoriamente!
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="panel_AlertaError" runat="server" Visible="false">
                        <div class="alert alert-danger" role="alert">
                            Error: El evento no pudo ser registrado. Intente de Nuevo.
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="panelNuevoEvento" runat="server" Visible="false">
                        <div class="row">
                            
                                <!--label for="inputEmail4" class="form-label">Id</!--label-->
                                <asp:TextBox ID="txtId" runat="server" CssClass="form-control" Enabled="false" Visible="false"></asp:TextBox>
                          
                            <div class="col-md-3">
                                <label for="inputEmail4" class="form-label">Concepto</label>
                                <asp:TextBox ID="txtConcepto" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="col-md-3">      
                                <label for="inputEmail4" class="form-label">Fecha de Pago</label>
                                <asp:TextBox ID="dtp_FechaPago" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>

                            <div class="col-md-3">           
                                <label for="inputEmail4" class="form-label">Comentarios</label>
                                <asp:TextBox ID="txtComentarios" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
        
                            <div class="col-md-2">      
                                <label for="inputEmail4" class="form-label">Estatus</label>
                                <asp:DropDownList ID="cmbEstatus" runat="server" CssClass="form-control">
                                    <asp:ListItem>Pendiente</asp:ListItem>
                                    <asp:ListItem>Liquidado</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-1"></div>
                            <div class="col-md-12 d-flex justify-content-center" style="margin-top: 2%;">
                                <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-primary btn-block font-btn col-md-3" OnClick="btnGuardar_Click">Guardar</asp:LinkButton>
                                <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-warning font-btn  col-md-3" Visible="false" OnClick="btnEditar_Click">Editar</asp:LinkButton>
                                <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-danger font-btn  col-md-3" Visible="false" OnClientClick="return confirm('¿Estás seguro de que quieres eliminar este Evento del Calendario?');" OnClick="btnEliminar_Click">Eliminar</asp:LinkButton>
                            </div>
                        </div>  
                    </asp:Panel>

                 </div>
                <hr />
                <div class="row" style="display:flex;justify-content:center;">
                    <asp:GridView ID="dgv_CalendarioPagos" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" AutoGenerateColumns="True" AutoGenerateSelectButton="True" OnSelectedIndexChanged="dgv_CalendarioPagos_SelectedIndexChanged" OnRowDataBound="dgv_CalendarioPagos_RowDataBound" >
                        <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                     </asp:GridView>
                </div>
                
             </div>
        </div>
    </div>
</div> <!-- end content-wrapper-->
</div> <!-- end contentr-->
</div> <!-- end wrapper-->
</asp:Content>

