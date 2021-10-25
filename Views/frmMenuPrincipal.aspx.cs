using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Globalization;
using ezBank.Classes;

namespace ezBank.Views
{
    public partial class frmMenuPrincipal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UsuarioGlobal"] =="" || Session["PerfilGlobal"] =="" ||
                Session["UsuarioGlobal"] ==null || Session["PerfilGlobal"] == null)
            {
                Response.Write("<script>alert('Sesión inválida');</script>");
                Response.Redirect("../Default.aspx");
                return;
            }

            String Usuario = Session["UsuarioGlobal"].ToString();
            String Perfil = Session["PerfilGlobal"].ToString();
            String Consulta = "SELECT Nombre from optUsuarios where Usuario='" + Usuario + "'";
            string ConexionConfig = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection conexion = new SqlConnection(ConexionConfig);
            SqlCommand comando = new SqlCommand(Consulta, conexion);
            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            DataTable tablaResultado = new DataTable();

            conexion.Open();
            adaptador.Fill(tablaResultado);
            String nombreCompleto = tablaResultado.Rows[0][0].ToString();
            lblUsuario.Text = Usuario + " - " + nombreCompleto +" - " + Perfil;
            conexion.Close();

            ///// ETIQUETA TOTAL CREDITOS AFILIADOS

           
            SqlCommand sp = new SqlCommand("SP_REPORTES", conexion);
            sp.CommandType = CommandType.StoredProcedure;
            sp.CommandTimeout = 900;
            sp.Parameters.AddWithValue("@idReporte", 14);

            conexion.Open();
            SqlDataAdapter da = new SqlDataAdapter(sp);
            DataTable dt = new DataTable();
            da.Fill(dt);
            string AfiliadosTotalCreditos=dt.Rows[0][0].ToString();
            conexion.Close();

            lblAfiliadosTotalCreditos.Text = AfiliadosTotalCreditos;


            ///// ETIQUETA TOTAL CREDITOS PENDIENTES POR AFILIAR

            SqlCommand sp2 = new SqlCommand("SP_REPORTES", conexion);
            sp2.CommandType = CommandType.StoredProcedure;
            sp2.CommandTimeout = 900;
            sp2.Parameters.AddWithValue("@idReporte", 13);

            conexion.Open();
            SqlDataAdapter da2 = new SqlDataAdapter(sp2);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            
            string AfiliadosPendientes = dt2.Rows[0][0].ToString();
            conexion.Close();

            lblAfiliadosPendientes.Text = AfiliadosPendientes;



            ////PROYECTADO A 7 DIAS
            ///
            String Consultax = "SELECT CONVERT(VARCHAR,A.SIGPAGO,126) AS Fecha, B.Producto ,B.SuperConvenio as Convenio,COUNT(A.IDCREDITO) AS Creditos,FORMAT(SUM(A.IMPORTEPAGO),'c') AS Exigible " +
                "FROM ZELLSALDOSCARTERA AS A " +
                "LEFT JOIN ZELLCONVENIOS AS B " +
                "ON A.Dependencia = B.vDescription " +
                "WHERE A.SIGPAGO IS NOT NULL AND A.SIGPAGO>=getdate()-3 and A.SIGPAGO<=getdate()+7 And DiasUltimoPago<= 120  AND(AmortPagadas / Pagos) < 1 " +
                "AND(A.ESTATUS = 'Activo') " +
                "GROUP BY CONVERT(VARCHAR, A.SIGPAGO, 126),B.SuperConvenio, B.Producto " +
                  "ORDER BY CONVERT(VARCHAR, A.SIGPAGO,126),B.SuperConvenio, B.Producto ";  
            dgvExigible.DataSource = principalClass.Read(Consultax);
            dgvExigible.DataBind();




            ///// ULTIMA ACTUALIZACION DE CARTERA
            SqlCommand sp3 = new SqlCommand("SP_REPORTES", conexion);
            sp3.CommandType = CommandType.StoredProcedure;
            sp3.CommandTimeout = 900;
            sp3.Parameters.AddWithValue("@idReporte",27);

            conexion.Open();
            SqlDataAdapter da3 = new SqlDataAdapter(sp3);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            string ultimaActualizacionCartera = dt3.Rows[0][0].ToString();
            conexion.Close();

            lblUltimaActualizacion.Text = "Ultima Actualización de Cartera: " +  ultimaActualizacionCartera;

            /////GRAFICO CALENDARIO PAGOS
            ///



            String QueryGrafico = "select count(CASE WHEN ESTATUS = 'PENDIENTE' AND FECHAPAGO>= GETDATE() THEN 1 END) AS Vigentes, count(CASE WHEN ESTATUS = 'PENDIENTE' AND FECHAPAGO < GETDATE() THEN 1 END) AS Vencidos from optCalendarioPagos";
            SqlCommand comando2 = new SqlCommand(QueryGrafico, conexion);
            DataTable tablaResultado2 = new DataTable();
            conexion.Open();
            SqlDataReader lectorDatos = comando2.ExecuteReader();
            tablaResultado2.Load(lectorDatos, LoadOption.OverwriteChanges);
            conexion.Close();


            String Chart = "";
            Chart = "<canvas id=\"doughnut-chart\" width=\"50%\" height=\"50\"></canvas>";
            Chart += "<script> new Chart(document.getElementById(\"doughnut-chart\"),";
            Chart +="{type: 'doughnut', data:{ labels:['Vigentes','Vencidos'],";
            Chart += "datasets:[{data: [";

            ///get Data from Database and add to chart
            string Valor = "";
            for (int i = 0; i < tablaResultado2.Columns.Count; i++)
                Valor += tablaResultado2.Rows[0][i].ToString() + ",";
            Valor = Valor.Substring(0, Valor.Length - 1);
            Chart += Valor;

            Chart += "], backgroundColor: ['rgb(46,139,87)','rgb(250,128,114)'],  "; ///Chart Color
            Chart += "hoverOffset: 4}]";//Chart Title
            Chart += "}, options:{ responsive: true, plugins:{title:{display:true, text:'Calendario de Pagos'}}}});";
            Chart += "</script>";

            GraficoCalendario.Text = Chart;


            //String QueryGrafico = "select id FROM optCalendarioPagos";
            //    ////"select count(CASE WHEN ESTATUS = 'PENDIENTE' AND FECHAPAGO>= GETDATE() THEN 1 END) AS Vigentes, count(CASE WHEN ESTATUS = 'PENDIENTE' AND FECHAPAGO < GETDATE() THEN 1 END) AS Vencidos from optCalendarioPagos";
            //SqlCommand comando2 = new SqlCommand(QueryGrafico, conexion);
            //DataTable tablaResultado2= new DataTable();
            //conexion.Open();
            //SqlDataReader lectorDatos = comando2.ExecuteReader();
            //tablaResultado2.Load(lectorDatos, LoadOption.OverwriteChanges);
            //conexion.Close();


            //String Chart = "";
            //Chart = "<canvas id=\"line-chart\" width=\"100%\" height=\"70\"></canvas>";
            //Chart += "<script>";
            //Chart += "new Chart(document.getElementById(\"line-chart\"),{type: 'line', data:{ labels:[";

            //for (int i = 0; i < 100; i++)
            //    Chart += i.ToString() + ",";
            //Chart.Substring(0, Chart.Length - 1);                       
            //Chart += "],datasets:[{data: [";

            /////get Data from Database and add to chart
            //string Valor = "";
            //for (int i = 0; i < tablaResultado2.Columns.Count; i++)
            //    Valor += tablaResultado2.Rows[i]["id"].ToString() + ",";
            //Valor = Valor.Substring(0, Valor.Length - 1);
            //Chart += Valor;

            //Chart += "], label: \"Africa\", borderColor: \"#3e95cd\", fill:false}"; ///Chart Color
            //Chart += "]}, options: {title: {display:true, text:'Your Chart Title'}}";//Chart Title
            //Chart += "});";
            //Chart += "</script>";

            //GraficoCalendario.Text = Chart;
        }
    }
}