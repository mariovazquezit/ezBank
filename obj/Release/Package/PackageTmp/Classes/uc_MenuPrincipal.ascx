<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="uc_MenuPrincipal.ascx.cs" Inherits="ezBank.Classes.uc_MenuPrincipal" %>

<div class="wrapper">
    <nav id="sidebar">
        <div class="sidebar-header">
            <h3>ezBank</h3>
            <hr>
        </div>
        <ul class="list-unstyled components">
            <li> 
                <a class="nav-link" href="frmMenuPrincipal.aspx">      
                   <i class="fas fa-comment-dollar fa-lg"></i>
                    Inicio
                 </a>
            </li>
            <li> <a class="nav-link" href="frmExpediente.aspx"><i class="fas fa-hand-holding-usd fa-lg"></i>Expediente Crediticio</a> </li>
            <li> <a class="nav-link" href="frmCalendarioCobro.aspx"><i class="fas fa-calendar-check fa-lg"></i>Calendario</a> </li>
            <li> <a class="nav-link" href="frmAfiliaciones.aspx"><i class="fas fa-check-circle fa-lg"></i>Afiliaciones</a> </li>
            <li> <a class="nav-link" href="frmCobranza.aspx"><i class="fas fa-hand-holding-usd fa-lg"></i>Cobranza</a> </li>
            <li> <a class="nav-link" href="frmReportes.aspx"><i class="fas fa-file-excel fa-lg"></i>Reportes</a> </li>
            <li> <a class="nav-link" href="frmCargas.aspx"><i class="fas fa-file-invoice-dollar fa-lg"></i>Carga de Archivos</a> </li>
            <li class="dropdown">
                <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown3" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i class="fas fa-wrench"></i>Configuración 
                </a>
                <div class="dropdown-menu" aria-labelledby="navbarDropdown">           
                    <a class="dropdown-item" href="frmConfigAltasUsuario.aspx"><i class="fas fa-users fa-lg"></i>Alta de Usuarios </a>
                        <div class="dropdown-divider"></div>
                    <a class="dropdown-item"  href="frmConfigReglasNegocio.aspx"><i class="fas fa-cogs fa-lg"></i>Reglas de Negocio</a>
                    <a class="dropdown-item" href="frmConfigCatalogos.aspx"><i class="fas fa-digital-tachograph fa-lg"></i>Catálogos</a>            
                </div>
            </li>
            
        </ul>
    </nav>
    <div class="content">
        <nav class="navbar navbar-expand-lg navbar-light bg-light"> <button type="button" id="sidebarCollapse" class="btn btn-info"> <i class="fa fa-align-justify"></i> </button> <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation"> <span class="navbar-toggler-icon"></span> </button>
            <div class="collapse navbar-collapse" id="navbarNav">
                <ul class="navbar-nav ml-auto">
                   <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle my-profile" href="#" id="navbarDropdown4" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                          <i class="far fa-id-badge fa-lg"></i>Perfil 
                        </a>
                        <div class="dropdown-menu" aria-labelledby="navbarDropdown">           
                            <a class="dropdown-item" href="frmCambiarPassword.aspx"> <i class="fas fa-unlock-alt fa-lg"></i>Cambiar Contraseña</a>
                            <div class="dropdown-divider"></div>                     
                            <asp:LinkButton ID="LinkButton1" CssClass="dropdown-item btn btn-outline-info" runat="server" OnClick="hyperlink_cerrarSesion_Click"> <i class="fas fa-door-open fa-lg"></i> Cerrar Sesión</asp:LinkButton>
                        </div>
                    </li>
                </ul>
            </div>
        </nav>
    
        <div class="content-wrapper">