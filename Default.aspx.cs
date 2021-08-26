using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ezBank
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Session["UsuarioGlobal"]="";
            Session["PerfilGlobal"]="";
                           
        }

        protected void btnInicioSesion_Click(object sender, EventArgs e)
        {
            String Usuario = txtUsuario.Text;
            String Password = txtPassword.Text;
                        
            if (Usuario=="" || Password == "")
            {
                mostrarPanelInvalido();
                return;
            }

            String validaUsuario = "SELECT Usuario, Nombre, Perfil FROM optUsuarios WHERE Usuario='" + Usuario + "' AND Password = '" + Password + "' AND Estatus = 'ACTIVO' ";

            string ConexionConfig = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection conexion = new SqlConnection(ConexionConfig);
            SqlCommand comando = new SqlCommand(validaUsuario, conexion);
            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            DataTable tablaResultado = new DataTable();

            try
            {

            
                conexion.Open();
                adaptador.Fill(tablaResultado); 

                String PermisosPerfil = tablaResultado.Rows[0][2].ToString();
                int Registros = tablaResultado.Rows.Count;

                conexion.Close();

                if (Registros == 1)
                {
                    Session["UsuarioGlobal"] = Usuario.ToUpper();
                    Session["PerfilGlobal"] = PermisosPerfil;

                    SqlCommand SP_Login = new SqlCommand("SP_InicioSesion", conexion);
                    SP_Login.CommandType = CommandType.StoredProcedure;
                    SP_Login.Parameters.AddWithValue("@Usuario", Usuario);
                    SP_Login.Parameters.AddWithValue("@Proceso", "LOGIN");

                    conexion.Open();
                    SP_Login.ExecuteNonQuery();
                    conexion.Close();

                    Response.Redirect("Views/frmMenuPrincipal.aspx");
                }
                else
                {
                    Response.Write("<script>alert('Usuario o Contraseña inválidos');</script>");
                }
            }catch (Exception ex){
                mostrarPanelInvalido();
            }


        }

        protected void mostrarPanelInvalido()
        {
            panelUsuarioinvalido.Visible = true;
        }
    }
}