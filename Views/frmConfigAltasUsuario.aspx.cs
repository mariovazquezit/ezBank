using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ezBank.Classes;

namespace ezBank.Views
{
    public partial class frmConfigAltasUsuario : System.Web.UI.Page
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
                limpiarFormularios();
                showGridViewAltasUsuario();
            }


        }

        protected void showGridViewAltasUsuario()
        {
            String Consulta = "SELECT id, Usuario, Nombre, Password, Perfil, Estatus FROM optUsuarios";
            dgvAltasUsuario.DataSource = principalClass.Read(Consulta);
            dgvAltasUsuario.DataBind();
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            showPanelNuevoUsuario();
            btnGuardar.Visible = true;
            limpiarFormularios();
            ocultarAlertas();
        }


        protected void showPanelNuevoUsuario()
        {
            panel_NuevoUsuario.Visible = true;            
        }

        protected void dgvAltasUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            showPanelNuevoUsuario();
            ocultarAlertas();
            btnEditar.Visible = true;
            btnEliminar.Visible = true;
            btnGuardar.Visible = false;


            txtID.Text = dgvAltasUsuario.SelectedRow.Cells[1].Text;
            txtUsuario.Text= dgvAltasUsuario.SelectedRow.Cells[2].Text;
            txtNombre.Text= dgvAltasUsuario.SelectedRow.Cells[3].Text;
            txtPassword.Text= dgvAltasUsuario.SelectedRow.Cells[4].Text;
            cmbPerfil.Text= dgvAltasUsuario.SelectedRow.Cells[5].Text;
            cmbEstatus.Text=dgvAltasUsuario.SelectedRow.Cells[6].Text;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            ocultarAlertas();

            string Usuario = txtUsuario.Text;
                string Nombre = txtNombre.Text;
                string Password = txtPassword.Text;
                string Perfil = cmbPerfil.Text;
                string Estatus = cmbEstatus.Text;

                string Query = "INSERT INTO optUsuarios (Usuario, Nombre, Password, Perfil ,Estatus) VALUES('" + Usuario + "','" + Nombre + "','" + Password + "','" + Perfil + "','" + Estatus + "')";

                int rowsAffected = principalClass.CUD(Query);

                if (rowsAffected == 1)
                {
                    panel_AlertaGuardar.Visible = true;
                }
                else
                {
                    panel_AlertaError.Visible = true;
                }

                showGridViewAltasUsuario();
                limpiarFormularios();

        }

        protected void ocultarAlertas()
        {
            panel_AlertaGuardar.Visible = false;
            panel_AlertaError.Visible = false;
        }

        protected void limpiarFormularios()
        {
            btnGuardar.Visible = true;
            btnEditar.Visible = false;
            btnEliminar.Visible = false;

            txtID.Text = "";
            txtUsuario.Text = "";
            txtNombre.Text = "";
            txtPassword.Text = "";

            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            string Consulta = "SELECT PERFIL FROM CATPERFILES";
            SqlCommand Comando = new SqlCommand(Consulta, con);
            SqlDataAdapter Adaptador = new SqlDataAdapter(Comando);
            DataTable tabla = new DataTable();

            con.Open();
            Adaptador.Fill(tabla);
            cmbPerfil.DataSource = tabla;
            cmbPerfil.DataTextField = "PERFIL";
            cmbPerfil.DataBind();
            con.Close();
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            ocultarAlertas();

            string idUsuario = txtID.Text;
            string Usuario = txtUsuario.Text;
            string Nombre = txtNombre.Text;
            string Password = txtPassword.Text;
            string Perfil = cmbPerfil.Text;
            string Estatus = cmbEstatus.Text;

            string Query = "UPDATE optUsuarios SET Usuario='" + Usuario + "', Nombre='" + Nombre + "', Password='" + Password + "', Perfil='" + Perfil + "', Estatus='" + Estatus + "' WHERE id=" + idUsuario + "";                

            int rowsAffected = principalClass.CUD(Query);

            if (rowsAffected == 1)
            {
                panel_AlertaGuardar.Visible = true;
            }
            else
            {
                panel_AlertaError.Visible = true;
            }

            showGridViewAltasUsuario();
            limpiarFormularios();
            
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            ocultarAlertas();

            string idUsuario = txtID.Text;
            string Query = "DELETE FROM optUsuarios WHERE id=" + idUsuario + "";

            int rowsAffected = principalClass.CUD(Query);

            if (rowsAffected == 1)
            {
                panel_AlertaGuardar.Visible = true;
            }
            else
            {
                panel_AlertaError.Visible = true;
            }

            showGridViewAltasUsuario();
            limpiarFormularios();
        }
    }
}