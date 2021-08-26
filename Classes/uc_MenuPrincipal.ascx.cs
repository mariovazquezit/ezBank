using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ezBank.Classes
{
    public partial class uc_MenuPrincipal : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void hyperlink_cerrarSesion_Click(object sender, EventArgs e)
        {

            string Usuario = Session["UsuarioGlobal"].ToString();
            string ConexionConfig = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection conexion = new SqlConnection(ConexionConfig);

            SqlCommand SP_Login = new SqlCommand("SP_InicioSesion", conexion);
            SP_Login.CommandType = CommandType.StoredProcedure;
            SP_Login.Parameters.AddWithValue("@Usuario", Usuario);
            SP_Login.Parameters.AddWithValue("@Proceso", "LOGOUT");

            conexion.Open();
            SP_Login.ExecuteNonQuery();
            conexion.Close();

            Response.Redirect("../Default.aspx");
        }
    }
}