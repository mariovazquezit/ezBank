using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ezBank.Classes
{
    public class principalClass
    {

        static public DataTable SP_Consulta(int idConsulta, string Valor)
        {
            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlCommand sp = new SqlCommand("SP_CONSULTAS", con);
            sp.CommandType = CommandType.StoredProcedure;
            sp.CommandTimeout = 900;
            sp.Parameters.AddWithValue("@IdConsulta", idConsulta);
            sp.Parameters.AddWithValue("@Valor", Valor);

            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(sp);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            return dt;
        }

        static public DataTable SP_Reportes(int idReporte)
        {
            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlCommand sp = new SqlCommand("SP_REPORTES", con);
            sp.CommandType = CommandType.StoredProcedure;
            sp.CommandTimeout = 900;
            sp.Parameters.AddWithValue("@idReporte", idReporte);            

            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(sp);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            return dt;
        }
        static public int CUD(string SQLCommand)
        {
            string ConexionConfig = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection conexion = new SqlConnection(ConexionConfig);
            SqlCommand comando = new SqlCommand(SQLCommand, conexion);
            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            DataTable tablaResultado = new DataTable();

            int RegistrosAfectados;

            conexion.Open();
            comando.CommandTimeout = 9000;

             if (comando.ExecuteNonQuery() == 1)
            {
                RegistrosAfectados = 1;
            }
            else
            {
                RegistrosAfectados = 0;
            }

            conexion.Close();
            return RegistrosAfectados;
        }

        static public DataTable Read(string SQLCommand)
        {
            string ConexionConfig = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection conexion = new SqlConnection(ConexionConfig);
            SqlCommand comando = new SqlCommand(SQLCommand, conexion);
            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            DataTable tablaResultado = new DataTable();

            conexion.Open();
            comando.CommandTimeout = 9000;
            adaptador.Fill(tablaResultado);
            conexion.Close();
            return tablaResultado;
        }

        static public string alertasCUD(int resultado)
        {
            string Alerta;

            if (resultado == 1)
            {
                Alerta = "<script>alert('Información registrada satisfactoriamente');</script>";
            }
            else
            {
                Alerta = "<script>alert('Error en el registro. Intente de nuevo.');</script>";
            }

            return Alerta;
        }





    }
}