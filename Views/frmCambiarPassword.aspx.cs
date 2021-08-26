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
    public partial class frmCambiarPassword : System.Web.UI.Page
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

                String Usuario = Session["UsuarioGlobal"].ToString();
                
                String Consulta = "SELECT TOP 1 id, Nombre, Perfil FROM optUsuarios WHERE Usuario='" + Usuario+"'";
                string ConexionConfig = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
                SqlConnection conexion = new SqlConnection(ConexionConfig);
                SqlCommand comando = new SqlCommand(Consulta, conexion);
                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                DataTable tablaResultado = new DataTable();

                conexion.Open();
                adaptador.Fill(tablaResultado);
                String IdUsuario = tablaResultado.Rows[0][0].ToString();
                String Nombre = tablaResultado.Rows[0][1].ToString();
                String Perfil = tablaResultado.Rows[0][2].ToString();

                txtID.Text = IdUsuario;
                txtUsuario.Text = Usuario;
                txtNombre.Text = Nombre;
                txtPerfil.Text = Perfil;

                conexion.Close();


            }
            else
            {
               
            }

        }

        protected void btnCambiarPassword_Click(object sender, EventArgs e)
        {
            string PasswordAnterior = txtPasswordAnterior.Text;
            string PasswordNuevo = txtPasswordNuevo.Text;
            string Usuario = txtUsuario.Text;

            if (PasswordAnterior=="" || PasswordNuevo=="" || PasswordAnterior== null || PasswordNuevo == null)
            {
                Response.Write("<script>alert('Error: Las contraseñas no pueden estar vacías');</script>");
                return;
            }

            String Consulta = "SELECT TOP 1 Password FROM optUsuarios WHERE Usuario='" + Usuario + "'";
            string ConexionConfig = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection conexion = new SqlConnection(ConexionConfig);
            SqlCommand comando = new SqlCommand(Consulta, conexion);
            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            DataTable tablaResultado = new DataTable();

            conexion.Open();
            adaptador.Fill(tablaResultado);
            String Password = tablaResultado.Rows[0][0].ToString();                        
            conexion.Close();

            if (Password != PasswordAnterior)
            {
                Response.Write("<script>alert('Error: La Contraseña anterior no coincide con la Contraseña Actual');</script>");
                return;
            }else if (Password==PasswordAnterior) {

                string Query = "UPDATE optUsuarios SET Password='" + PasswordNuevo + "' WHERE Usuario='" + Usuario + "'";
                SqlCommand comando2 = new SqlCommand(Query, conexion);
                SqlDataAdapter adaptador2 = new SqlDataAdapter(comando2);

                int rowsAffected = principalClass.CUD(Query);

                if (rowsAffected == 1)
                {
                    Response.Write("<script>alert('Contraseña actualizada satisfactoriamente');</script>");
                }
                else
                {
                    Response.Write("<script>alert('La Contraseña no pudo ser actualizada. Intente de nuevo.');</script>");
                }
            }
            }

        }
    }
