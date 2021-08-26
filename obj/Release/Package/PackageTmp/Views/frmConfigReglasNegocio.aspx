<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmConfigReglasNegocio.aspx.cs" Inherits="ezBank.Views.frmConfigReglasNegocio" %>

<%@ Register Src="~/Classes/uc_menuPrincipal.ascx" TagPrefix="uc1" TagName="uc_menuprincipal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:uc_menuprincipal runat="server" id="uc_menuprincipal" />       

     <div class="row">
        <div class="col-md-12 col-xs-12" style="display:flex;align-items:center;flex-direction:column;">
            <h3 class="card-title"> Configuración de Reglas de Negocio </h3>
        </div>
        <div class="col-md-12 col-xs-12">
            <div class="card col-md-12 col-xs-12 space">
                <div class="card-body col-md-12 col-xs-12 d-flex justify-content-center">
                    <asp:LinkButton ID="btncatDatosDomi" runat="server" CssClass="col-md-3 btn btn-secondary btn-font btn-space text-white space" OnClick="btncatDatosDomi_Click">Datos Domiciliación</asp:LinkButton>   
                    <asp:LinkButton ID="btncatEmisoras" runat="server" CssClass="col-md-3 btn btn-secondary btn-font  btn-space text-white space" OnClick="btncatEmisoras_Click" >Emisoras</asp:LinkButton>
                    <asp:LinkButton ID="btnCatPerfiles" runat="server" CssClass="col-md-3 btn btn-secondary btn-font btn-space text-white space" OnClick="btnCatPerfiles_Click" >Perfiles</asp:LinkButton>                              
                </div>
            </div> <!-- btn -->
            <asp:Panel ID="panelCatDatosDomi" runat="server" Visible="true">
                <div class="col-md-12 d-flex">
                    <div class="card col-md-4 col-xs-12" style="margin-right:2%;">
                        <div class="card-header">
                            <asp:LinkButton ID="btnNuevoDatosDomi" runat="server" CssClass="btn btn-success btn-font btn-space col-md-12 space" OnClick="btnNuevoDatosDomi_Click" >Nuevo</asp:LinkButton>      
                            <hr />
                        </div>
                        <div class="card-body">
                            <div class="form-row d-flex justify-content-center">          
                                <div class="col-md-12">
                                    <label>Banco</label>
                                    <asp:TextBox ID="txtDomiBanco" runat="server" CssClass="form-control" MaxLength="50" ></asp:TextBox>
                                </div>
                                <div class="col-md-12">
                                    <label>Razon Social</label>
                                    <asp:TextBox ID="txtDomiRazonSocial" runat="server" CssClass="form-control" MaxLength="40" ></asp:TextBox>
                                </div>
                                <div class="col-md-12">
                                    <label>RFC</label>
                                    <asp:TextBox ID="txtDomiRFC" runat="server" CssClass="form-control" MaxLength="20" ></asp:TextBox>
                                </div>
                                <div class="col-md-12">
                                    <label>IdCliente</label>
                                    <asp:TextBox ID="txtDomiIdCliente" runat="server" CssClass="form-control" MaxLength="12" ></asp:TextBox>
                                </div>
                                <div class="col-md-12">
                                    <label>Descripción Pago</label>
                                    <asp:TextBox ID="txtDomiDescripcionPago" runat="server" CssClass="form-control" MaxLength="40" ></asp:TextBox>
                                </div>
             
                                <div class="col-md-12" style="margin-top:2%;">
                                    <hr />                                    
                                    <asp:LinkButton ID="btnDomiGuardar" runat="server" CssClass="btn btn-primary btn-font btn-space btn-block col-md-12 space" OnClick="btnDomiGuardar_Click" >Guardar</asp:LinkButton>      
                                    <asp:LinkButton ID="btnDomiEditar" runat="server" visible="false" CssClass="btn btn-warning btn-font btn-space col-md-4 space" OnClick="btnDomiEditar_Click" >Editar</asp:LinkButton>      
                                    <asp:LinkButton ID="btnDomiEliminar" runat="server" visible="false"  CssClass="btn btn-danger btn-font btn-space col-md-4 space" OnClientClick="return confirm('¿Estás seguro de que quieres eliminar este Dato de Domiciliacion?');" OnClick="btnDomiEliminar_Click" >Eliminar</asp:LinkButton>      
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card card-body col-md-8 col-xs-12">
                        <asp:GridView ID="dgvCatDatosDomi" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" AutoGenerateColumns="True" AutoGenerateSelectButton="True" OnSelectedIndexChanged="dgvCatDatosDomi_SelectedIndexChanged" >
                            <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                        </asp:GridView>
                    </div>
                </div>   
             </asp:Panel>
            <asp:Panel ID="panelCatEmisoras" runat="server" Visible="false">
                <div class="col-md-12 d-flex">
                    <div class="card col-md-4 col-xs-12" style="margin-right: 2%;">
                        <div class="card-header">
                                  <asp:LinkButton ID="btnNuevoEmisora" runat="server" CssClass="btn btn-success btn-font col-md-12 space" OnClick="btnNuevoEmisora_Click" >Nuevo</asp:LinkButton>      
                        </div>
                        <div class="card-body">
                            <div class="form-row">
                                <div class="col-md-12">
                                  <label>Id</label>
                                    <asp:TextBox ID="txtEmisoraID" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-12">
                                  <label>Emisora</label>
                                  <asp:TextBox ID="txtEmisora" runat="server" CssClass="form-control" MaxLength="5" ></asp:TextBox>
                                </div>
                                <div class="col-md-12">
                                    <label>Banco</label>
                                    <asp:TextBox ID="txtEmisoraBanco" runat="server" CssClass="form-control" MaxLength="50" ></asp:TextBox> 
                                </div>
                                <div class="col-md-12">
                                  <label>Descripcion</label>
                                  <asp:TextBox ID="txtEmisoraDescripcion" runat="server" CssClass="form-control" MaxLength="30" ></asp:TextBox>
                                </div>
             
                                <div class="col-md-12">
                                     <hr />
                                     <asp:LinkButton ID="btnEmisoraGuardar" runat="server" CssClass="btn btn-primary btn-font col-md-12 space btn-block" OnClick="btnEmisoraGuardar_Click" >Guardar</asp:LinkButton>      
                                     <asp:LinkButton ID="btnEmisoraEditar" runat="server" visible="false" CssClass="btn btn-warning btn-font col-md-4 space" OnClick="btnEmisoraEditar_Click" >Editar</asp:LinkButton>      
                                     <asp:LinkButton ID="btnEmisoraEliminar" runat="server" visible="false"  CssClass="btn btn-danger btn-font col-md-4 space" OnClientClick="return confirm('¿Estás seguro de que quieres eliminar a esta Emisora?');" OnClick="btnEmisoraEliminar_Click" >Eliminar</asp:LinkButton>      
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-8 card card-body" style="display:flex;align-items:center;">
                        <asp:GridView ID="dgvCatEmisoras" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" AutoGenerateColumns="True" AutoGenerateSelectButton="True" OnSelectedIndexChanged="dgvCatEmisoras_SelectedIndexChanged" >
                            <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                        </asp:GridView>
                     </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="panelCatPerfiles" runat="server" Visible="false">
                <div class="col-md-12 d-flex">
                    <div class="card col-md-4" style="margin-right:2%;">
                        <div class="card-body">
                            <asp:LinkButton ID="btnNuevoPerfil" runat="server" CssClass="btn btn-success btn-font space col-md-12" OnClick="btnNuevoPerfil_Click" >Nuevo</asp:LinkButton>
                            <hr />
                            <div class="form-row">
                                <div class="col-md-12">
                                  <label>Id</label>
                                    <asp:TextBox ID="txtPerfilId" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-12">
                                  <label>Perfil</label>
                                  <asp:TextBox ID="txtPerfilPerfil" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                </div> 
                                <div class="col-md-6">
                                    <label>Calendario</label>
                                    <asp:TextBox ID="txtPerfilCalendario" runat="server" CssClass="form-control"  TextMode="Number"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                  <label>Afiliaciones</label>
                                  <asp:TextBox ID="txtPerfilAfiliaciones" runat="server" CssClass="form-control"  TextMode="Number"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                  <label>Cobranza</label>
                                  <asp:TextBox ID="txtPerfilCobranza" runat="server" CssClass="form-control"  TextMode="Number"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                  <label>Alta de Usuarios</label>
                                  <asp:TextBox ID="txtPerfilAltasUsuario" runat="server" CssClass="form-control"  TextMode="Number" ></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                  <label>Reglas de Negocio</label>
                                  <asp:TextBox ID="txtPerfilReglasNegocio" runat="server" CssClass="form-control"  TextMode="Number"></asp:TextBox>
                                </div>
                 
                                <div class="col-md-6">
                                  <label>Expediente</label>
                                  <asp:TextBox ID="txtPerfilExpediente" runat="server" CssClass="form-control" TextMode="Number" ></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                  <label>Reportes</label>
                                  <asp:TextBox ID="txtPerfilReportes" runat="server" CssClass="form-control"  TextMode="Number"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                  <label>Catálogos</label>
                                  <asp:TextBox ID="txtPerfilCatalogos" runat="server" CssClass="form-control"  TextMode="Number"></asp:TextBox>
                                </div>
                                <div class="col-md-12">
                                  <label>Carga Archivos</label>
                                  <asp:TextBox ID="txtPerfilCargaArchivos" runat="server" CssClass="form-control"  TextMode="Number"></asp:TextBox>
                                </div>
                                <div class="col-md-12">
                                    <hr />
                                    <asp:LinkButton ID="btnPerfilGuardar" runat="server" CssClass="btn btn-primary btn-font space col-md-12 btn-block" OnClick="btnPerfilGuardar_Click" >Guardar</asp:LinkButton>      
                                    <asp:LinkButton ID="btnPerfilEditar" runat="server" visible="false" CssClass="btn btn-warning btn-font space col-md-12" OnClick="btnPerfilEditar_Click" >Editar</asp:LinkButton>      
                                    <asp:LinkButton ID="btnPerfilEliminar" runat="server" visible="false"  CssClass="btn btn-danger btn-font space col-md-12 " OnClientClick="return confirm('¿Estás seguro de que quieres eliminar este Perfil?');" OnClick="btnPerfilEliminar_Click" >Eliminar</asp:LinkButton>      
                                </div>
                            </div>
                         </div>
                    </div>
                    <div class="card card-body col-md-8 col-xs-12">
                        <asp:GridView ID="dgvCatPerfiles" runat="server" CssClass="table table-responsive table-bordered table-hover table-sm" AutoGenerateColumns="True" AutoGenerateSelectButton="True" OnSelectedIndexChanged="dgvCatPerfiles_SelectedIndexChanged" >
                            <SelectedRowStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="#333333" />
                       </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
        </div>
     </div>


</asp:Content>
