using ezBank.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ezBank.Views
{
    public partial class frmConfigReglasNegocio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioGlobal"] == "" || Session["PerfilGlobal"] == "" ||
          Session["UsuarioGlobal"] == null || Session["PerfilGlobal"] == null)
            {
                Response.Write("<script>alert('Sesión inválida');</script>");
                Response.Redirect("../Default.aspx");
                return;
            }




            if (!Page.IsPostBack)
            {
                showCatalogos();
                
            }


        }


        protected void ocularPaneles() {
            panelCatDatosDomi.Visible = false;
            panelCatEmisoras.Visible = false;
            panelCatPerfiles.Visible = false;
            panelCatConfiguracion.Visible = false;
        }

        protected void showConfiguracion()
        {

            string Query = "select valorString from  catConfiguracion";

            string ConexionConfig = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection conexion = new SqlConnection(ConexionConfig);
            SqlCommand comando = new SqlCommand(Query, conexion);
            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            DataTable tablaResultado = new DataTable();

            conexion.Open();
            adaptador.Fill(tablaResultado);

            string nombreSistema = tablaResultado.Rows[0][0].ToString();
            string nombreFinanciera = tablaResultado.Rows[1][0].ToString();

            conexion.Close();

            txtConfiguracionNombreFinanciera.Text = nombreFinanciera;
            txtConfiguracionNombreSistema.Text = nombreSistema;
        }

        protected void showCatalogos()
        {
            String Consulta = "SELECT * FROM catDatosDomi";
            dgvCatDatosDomi.DataSource = principalClass.Read(Consulta);
            dgvCatDatosDomi.DataBind();

            Consulta = "SELECT * FROM catEmisoras ORDER BY ID ASC";
            dgvCatEmisoras.DataSource = principalClass.Read(Consulta);
            dgvCatEmisoras.DataBind();

            Consulta = "SELECT * FROM catPerfiles ORDER BY ID ASC";
            dgvCatPerfiles.DataSource = principalClass.Read(Consulta);
            dgvCatPerfiles.DataBind();


        }

        protected void btncatDatosDomi_Click(object sender, EventArgs e)
        {
            ocularPaneles();
            panelCatDatosDomi.Visible = true;
        }

        protected void btncatEmisoras_Click(object sender, EventArgs e)
        {
            ocularPaneles();
            panelCatEmisoras.Visible = true;
        }

        protected void btnCatPerfiles_Click(object sender, EventArgs e)
        {
            ocularPaneles();
            panelCatPerfiles.Visible = true;
        }

        protected void dgvCatDatosDomi_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Banco = dgvCatDatosDomi.SelectedRow.Cells[1].Text;
            string RazonSocial = dgvCatDatosDomi.SelectedRow.Cells[2].Text;
            string RFC = dgvCatDatosDomi.SelectedRow.Cells[3].Text;
            string IdCliente = dgvCatDatosDomi.SelectedRow.Cells[4].Text;
            string DescripcionPago = dgvCatDatosDomi.SelectedRow.Cells[5].Text;

            txtDomiBanco.Text = Banco;
            txtDomiRazonSocial.Text = RazonSocial;
            txtDomiRFC.Text = RFC;
            txtDomiIdCliente.Text = IdCliente;
            txtDomiDescripcionPago.Text = DescripcionPago;

            btnDomiGuardar.Visible = false;
            btnDomiEditar.Visible = true;
            btnDomiEliminar.Visible = true;

        }

        protected void dgvCatEmisoras_SelectedIndexChanged(object sender, EventArgs e)
        {
            string EmisoraID = dgvCatEmisoras.SelectedRow.Cells[1].Text;
            string Emisora = dgvCatEmisoras.SelectedRow.Cells[2].Text;
            string Banco = dgvCatEmisoras.SelectedRow.Cells[3].Text;
            string Descripcion = dgvCatEmisoras.SelectedRow.Cells[4].Text;
            string RFC= dgvCatEmisoras.SelectedRow.Cells[5].Text;

            if (Emisora == "&nbsp;")
            {
                Emisora = "";
            }

            if (Banco == "&nbsp;")
            {
                Banco = "";
            }

            if (Descripcion == "&nbsp;")
            {
                Descripcion = "";
            }

            txtEmisoraID.Text = EmisoraID;
            txtEmisora.Text = Emisora;
            txtEmisoraBanco.Text = Banco;
            txtEmisoraDescripcion.Text = Descripcion;
            txtRFC.Text = RFC;

            btnEmisoraGuardar.Visible = false;
            btnEmisoraEditar.Visible = true;
            btnEmisoraEliminar.Visible = true;
        }

        protected void dgvCatPerfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtPerfilId.Text = dgvCatPerfiles.SelectedRow.Cells[1].Text;
            txtPerfilPerfil.Text = dgvCatPerfiles.SelectedRow.Cells[2].Text;
            txtPerfilCalendario.Text = dgvCatPerfiles.SelectedRow.Cells[3].Text;
            txtPerfilAfiliaciones.Text = dgvCatPerfiles.SelectedRow.Cells[4].Text;
            txtPerfilCobranza.Text = dgvCatPerfiles.SelectedRow.Cells[5].Text;
            txtPerfilAltasUsuario.Text = dgvCatPerfiles.SelectedRow.Cells[6].Text;
            txtPerfilReglasNegocio.Text = dgvCatPerfiles.SelectedRow.Cells[7].Text;
            txtPerfilExpediente.Text = dgvCatPerfiles.SelectedRow.Cells[8].Text;
            txtPerfilReportes.Text = dgvCatPerfiles.SelectedRow.Cells[9].Text;
            txtPerfilCatalogos.Text = dgvCatPerfiles.SelectedRow.Cells[10].Text;
            txtPerfilCargaArchivos.Text = dgvCatPerfiles.SelectedRow.Cells[11].Text;

            btnPerfilEditar.Visible = true;
            btnPerfilEliminar.Visible = true;
            btnPerfilGuardar.Visible = false;
        }

        protected void btnNuevoDatosDomi_Click(object sender, EventArgs e)
        {
            limpiarFormularios();
        }

        protected void btnNuevoEmisora_Click(object sender, EventArgs e)
        {
            limpiarFormularios();
        }

        protected void btnNuevoPerfil_Click(object sender, EventArgs e)
        {
            limpiarFormularios();
        } 

        protected void limpiarFormularios()
        {
            txtPerfilId.Text = "";
            txtPerfilPerfil.Text = "";
            txtPerfilCalendario.Text = "0";
            txtPerfilAfiliaciones.Text = "0";
            txtPerfilAltasUsuario.Text = "0";
            txtPerfilCobranza.Text = "0";
            txtPerfilExpediente.Text = "0";
            txtPerfilReglasNegocio.Text = "0";
            txtPerfilReportes.Text = "0";
            txtPerfilCatalogos.Text = "0";
            txtPerfilCargaArchivos.Text = "0";

            txtEmisoraID.Text = "";
            txtEmisoraBanco.Text = "";
            txtEmisoraDescripcion.Text = "";
            txtEmisora.Text = "";
            txtRFC.Text = "";

            txtDomiBanco.Text = "";
            txtDomiRazonSocial.Text = "";
            txtDomiRFC.Text = "";
            txtDomiIdCliente.Text = "";
            txtDomiDescripcionPago.Text = "";

            btnDomiEditar.Visible = false;
            btnEmisoraEditar.Visible = false;
            btnPerfilEditar.Visible = false;

            btnDomiEliminar.Visible = false;
            btnEmisoraEliminar.Visible = false;
            btnPerfilEliminar.Visible = false;

            btnDomiGuardar.Visible = true;
            btnEmisoraGuardar.Visible = true;
            btnPerfilGuardar.Visible = true;
            showCatalogos();
        }

        protected void btnDomiGuardar_Click(object sender, EventArgs e)
        {
            string Banco = txtDomiBanco.Text;
            string RazonSocial = txtDomiRazonSocial.Text;
            string RFC = txtDomiRFC.Text;
            string IdCliente = txtDomiIdCliente.Text;
            string DescripcionPago = txtDomiDescripcionPago.Text;

            if(Banco=="" || DescripcionPago == "")
            {
                Response.Write("<script>alert('No puede haber campos vacíos');</script>");
                return;
            }

            string Query = "INSERT INTO catDatosDomi (Banco, RazonSocial, RFC, IdCliente, DescripcionPago) " +
                "VALUES('" + Banco + "','" + RazonSocial + "','" + RFC + "','" + IdCliente + "','" + DescripcionPago + "')";

            int rowsAffected = principalClass.CUD(Query);

            string Alerta= principalClass.alertasCUD(rowsAffected);
            Response.Write(Alerta);
            limpiarFormularios();            
        }

        protected void btnEmisoraGuardar_Click(object sender, EventArgs e)
        {
            string Emisora = txtEmisora.Text;
            string Banco = txtEmisoraBanco.Text;
            string Descripcion = txtEmisoraDescripcion.Text;
            string RFC = txtRFC.Text;

            if (Banco == "" || Emisora == "")
            {
                Response.Write("<script>alert('No puede haber campos vacíos');</script>");
                return;
            }

            string Query = "INSERT INTO catEmisoras (Emisora, Banco, Descripcion, RFC) " +
                "VALUES('" + Emisora + "','" + Banco + "','" + Descripcion + "','"+RFC+"')";

            int rowsAffected = principalClass.CUD(Query);

            string Alerta = principalClass.alertasCUD(rowsAffected);
            Response.Write(Alerta);
            limpiarFormularios();
        }

        protected void btnPerfilGuardar_Click(object sender, EventArgs e)
        {
            string Perfil = txtPerfilPerfil.Text;
            string Calendario = txtPerfilCalendario.Text;
            string Afiliaciones = txtPerfilAfiliaciones.Text;
            string Cobranza = txtPerfilCobranza.Text;
            string altaUsuarios= txtPerfilAltasUsuario.Text;
            string ReglasNegocio = txtPerfilReglasNegocio.Text;
            string Expediente = txtPerfilExpediente.Text;
            string Reportes = txtPerfilReportes.Text;
            string Catalogos = txtPerfilCatalogos.Text;
            string CargaArchivos = txtPerfilCargaArchivos.Text;

            if (Perfil == "")
            {
                Response.Write("<script>alert('No puede haber campos vacíos');</script>");
                return;
            }

            string Query = "INSERT INTO catPerfiles (Perfil,Calendario, Afiliaciones, Cobranza, AltaUsuarios," +
                "ReglasNegocio, ExpedienteCrediticio, Reportes, Catalogos, CargaCartera) " +
                "VALUES('"+Perfil+"','" + Calendario + "','" + Afiliaciones + "','" + Cobranza + "','" + altaUsuarios + "'," +
                "'" + ReglasNegocio + "','" + Expediente + "','" + Reportes + "','" + Catalogos + "','" + CargaArchivos + "')";

            int rowsAffected = principalClass.CUD(Query);

            string Alerta = principalClass.alertasCUD(rowsAffected);
            Response.Write(Alerta);
            limpiarFormularios();
        }

        protected void btnDomiEditar_Click(object sender, EventArgs e)
        {
            string Banco = txtDomiBanco.Text;
            string RazonSocial = txtDomiRazonSocial.Text;
            string RFC = txtDomiRFC.Text;
            string IdCliente = txtDomiIdCliente.Text;
            string DescripcionPago = txtDomiDescripcionPago.Text;

            if (Banco == "" || RazonSocial == "" || RFC == "" || IdCliente == "" || DescripcionPago == "")
            {
                Response.Write("<script>alert('No puede haber campos vacíos');</script>");
                return;
            }

            string Query = "UPDATE catDatosDomi SET RazonSocial='"+RazonSocial+"',RFC='"+RFC+"', " +
                "IdCliente='"+IdCliente+"',DescripcionPago='"+DescripcionPago+"' WHERE Banco='" + Banco + "'";

            int rowsAffected = principalClass.CUD(Query);
            string Alerta = principalClass.alertasCUD(rowsAffected);
            Response.Write(Alerta);
            limpiarFormularios();

        }

        protected void btnEmisoraEditar_Click(object sender, EventArgs e)
        {
            string idEmisora = txtEmisoraID.Text;
            string Emisora = txtEmisora.Text;
            string Banco = txtEmisoraBanco.Text;
            string Descripcion = txtEmisoraDescripcion.Text;
            string RFC = txtRFC.Text;

            string Query = "UPDATE catEmisoras SET Emisora='" + Emisora + "',Banco='" + Banco + "', " +
                "Descripcion='" + Descripcion + "',RFC='"+RFC+"' WHERE Id=" + idEmisora + "";

            int rowsAffected = principalClass.CUD(Query);
            string Alerta = principalClass.alertasCUD(rowsAffected);
            Response.Write(Alerta);
            limpiarFormularios();
        }

        protected void btnPerfilEditar_Click(object sender, EventArgs e)
        {
            string idPerfil = txtPerfilId.Text;
            string Perfil = txtPerfilPerfil.Text;
            string Calendario = txtPerfilCalendario.Text;
            string Afiliaciones = txtPerfilAfiliaciones.Text;
            string Cobranza = txtPerfilCobranza.Text;
            string altaUsuarios = txtPerfilAltasUsuario.Text;
            string ReglasNegocio = txtPerfilReglasNegocio.Text;
            string Expediente = txtPerfilExpediente.Text;
            string Reportes = txtPerfilReportes.Text;
            string Catalogos = txtPerfilCatalogos.Text;
            string CargaArchivos = txtPerfilCargaArchivos.Text;


            string Query = "UPDATE catPerfiles SET Perfil='"+Perfil+"',Calendario="+Calendario+"," +
                "Afiliaciones=" + Afiliaciones + ",Cobranza=" + Cobranza + ",altaUsuarios=" + altaUsuarios + "," +
                "ReglasNegocio=" + ReglasNegocio + ",ExpedienteCrediticio=" + Expediente + "," +
                "Reportes=" + Reportes + ",Catalogos=" + Catalogos + ",CargaCartera=" + CargaArchivos + "" +
                " WHERE Id=" + idPerfil + "";

            int rowsAffected = principalClass.CUD(Query);
            string Alerta = principalClass.alertasCUD(rowsAffected);
            Response.Write(Alerta);
            limpiarFormularios();
        }

        protected void btnPerfilEliminar_Click(object sender, EventArgs e)
        {
            string idPerfil = txtPerfilId.Text;
            string Query = "DELETE FROM catPerfiles WHERE id=" + idPerfil + "";
            int rowsAffected = principalClass.CUD(Query);

            string Alerta = principalClass.alertasCUD(rowsAffected);
            Response.Write(Alerta);
            limpiarFormularios();
        }

        protected void btnEmisoraEliminar_Click(object sender, EventArgs e)
        {

            string idEmisora = txtEmisoraID.Text;
            string Query = "DELETE FROM catEmisoras WHERE id=" + idEmisora + "";
            int rowsAffected = principalClass.CUD(Query);

            string Alerta = principalClass.alertasCUD(rowsAffected);
            Response.Write(Alerta);
            limpiarFormularios();
        }

        protected void btnDomiEliminar_Click(object sender, EventArgs e)
        {
            string Banco = txtDomiBanco.Text;
            string Query = "DELETE FROM catDatosDomi WHERE Banco='" + Banco + "'";
            int rowsAffected = principalClass.CUD(Query);

            string Alerta = principalClass.alertasCUD(rowsAffected);
            Response.Write(Alerta);
            limpiarFormularios();
        }

        protected void btnCatConfiguracion_Click(object sender, EventArgs e)
        {
            ocularPaneles();
            showConfiguracion();
            panelCatConfiguracion.Visible = true;
        }

        protected void btnCatConfiguracion_GUARDAR_Click(object sender, EventArgs e)
        {            
            string NombreSistema=txtConfiguracionNombreSistema.Text;
            string NombreFinanciera = txtConfiguracionNombreFinanciera.Text;

            string Query = "update catConfiguracion set ValorString = '"+NombreSistema+"'"+
                            "where Configuracion = 'Nombre Sistema'";

            string Query2 = "update catConfiguracion set ValorString = '" + NombreFinanciera + "'" +
                            "where Configuracion = 'Nombre Financiera'";

            int rowsAffected = principalClass.CUD(Query);
            int rowsAffected2 = principalClass.CUD(Query2);

            string Alerta = principalClass.alertasCUD(rowsAffected);
            Response.Write(Alerta);            
        }
    }
}