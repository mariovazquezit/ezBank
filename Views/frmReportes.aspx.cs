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
using System.IO;
using ClosedXML.Excel;

namespace ezBank.Views
{
    public partial class frmReportes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                String FechaActual = DateTime.Now.ToString("yyyy-MM-dd");
                dtp_FechaInicial.Text = FechaActual;
                dtp_FechaFinal.Text = FechaActual;

                string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
                SqlConnection con = new SqlConnection(cs);
                SqlCommand sp = new SqlCommand("SP_REPORTES", con);
                sp.CommandType = CommandType.StoredProcedure;
                sp.CommandTimeout = 900;
                sp.Parameters.AddWithValue("@idReporte", 0);

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(sp);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbReporte.DataSource = dt;
                cmbReporte.DataTextField = "Reporte";
                cmbReporte.DataBind();
                con.Close();                

            }

        }

        protected void ocultarFechas()
        {
            panel_Fechas.Visible = false;
        }


        static public string ReporteRequiereFecha(string Reporte)
        {            
            string Consulta = "SELECT REQUIEREFECHA FROM CATREPORTES WHERE REPORTE='" + Reporte + "'";
            DataTable RequiereFechaTabla = new DataTable();

            RequiereFechaTabla = principalClass.Read(Consulta);

            string RequiereFecha = RequiereFechaTabla.Rows[0][0].ToString();
            return RequiereFecha;
        }

        protected void cmbReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Reporte = cmbReporte.Text;
            string RequiereFecha=ReporteRequiereFecha(Reporte);

            if (RequiereFecha == "0")
            {
                ocultarFechas();
            }
            else if (RequiereFecha == "1")
            {
                panel_Fechas.Visible = true;
            }

        }

        protected void btnGenerarReporte_Click(object sender, EventArgs e)
        {
            DateTime BeginDatex = DateTime.ParseExact(dtp_FechaInicial.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime FinishDatex= DateTime.ParseExact(dtp_FechaFinal.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            string Reporte = cmbReporte.Text;
            string BeginDate = BeginDatex.ToString("yyyy-MM-dd");
            string FinishDate = FinishDatex.ToString("yyyy-MM-dd");
            string query = null;
            int NumeroHojas = 0;

            if (Reporte == "Reporte de Login")
            {
                NumeroHojas = 1;
                query = "SELECT Distinct Usuario, convert(varchar, fecha, 23) as Fecha, " +
                    "dbo.fn_getLoginLogOut(Usuario, convert(varchar, fecha, 23), 'Login') as [Login], " +
                    "dbo.fn_getLoginLogOut(Usuario, convert(varchar, fecha, 23), 'Logout') as [Logout]" +
                    "from optLogin WHERE CONVERT(VARCHAR, FECHA, 23) >= '" + BeginDate + "'" +
                    "AND CONVERT(VARCHAR, FECHA,23)<= '" + FinishDate + "'";
            }else if (Reporte == "Calendario de Pagos")
            {
                NumeroHojas = 1;
                query = "SELECT CONVERT(VARCHAR, A.SIGPAGO, 126) AS Fecha, B.Producto,B.SuperConvenio as Convenio,COUNT(A.IDCREDITO) AS Creditos, " +
                        "SUM(A.IMPORTEPAGO) AS Exigible FROM ZELLSALDOSCARTERA AS A " +
                        " LEFT JOIN ZELLCONVENIOS AS B " +
                        " ON A.Dependencia = B.vDescription " +
                        " WHERE A.SIGPAGO >= '"+BeginDate+"' AND A.SIGPAGO <= '"+FinishDate+"' " +
                        " And DiasUltimoPago<= 120 AND A.PagoenExceso >= 0 " +
                        " AND(AmortPagadas / Pagos) < 1 " +
                        " AND(A.ESTATUS = 'Activo') " +
                        " GROUP BY CONVERT(VARCHAR, A.SIGPAGO, 126),B.SuperConvenio, B.Producto " +
                        " ORDER BY CONVERT(VARCHAR, A.SIGPAGO,126),B.SuperConvenio, B.Producto";                    
            }else if (Reporte == "Listas de Exclusion")
            {
                NumeroHojas = 1;
                query = "SELECT Credito, ExcepcionHasta from optBlackList";
            }else if (Reporte == "Listas de CLABES adicionales")
            {
                NumeroHojas = 1;
                query = "select Credito, CLABE, Comentarios from optCLABEs";
            }else if (Reporte == "Detalle de Archivos de Cobranza")
            {
                NumeroHojas = 3;
                query = "SELECT * FROM optConsecutivos WHERE FECHA>= '" + BeginDate + "' " +
                    "AND FECHA<= '" + FinishDate + "' " +
                    "ORDER BY FECHA;" +
                    "SELECT * FROM optLogDomiciliacion WHERE FECHACOBRO>= '" + BeginDate + "' " +
                    "AND FECHACOBRO<= '" + FinishDate + "' " +
                    "ORDER BY FECHACOBRO;"+
                "SELECT A.FECHA, A.BANCO, A.EMISORA, A.ARCHIVO, A.CREDITO, A.MONTO, A.RESPUESTA, B.DESCRIPCION, A.BUCKET, A.REGISTRO FROM OPTRESPUESTASCOBRANZA AS A "+
                    " LEFT JOIN CATRESPUESTASCOBRANZA AS B " +
                    "ON A.RESPUESTA = B.CODIGOFACTURACION "+
                    "WHERE B.BANCO = 'SANTANDER'"+
                    "AND FECHA>= '" +BeginDate+"' AND FECHA<= '"+ FinishDate + "'";
            }
            //else if (Reporte == "Detalle de Cobranza Domiciliada")
            //{
            //    query = "SELECT * FROM optLogDomiciliacion WHERE FECHACOBRO>= '" + BeginDate + "' " +
            //        "AND FECHACOBRO<= '" + FinishDate + "' " +
            //        "ORDER BY FECHACOBRO";
            //}
            else if (Reporte == "Detalle de Archivos de Afiliacion")
            {
                NumeroHojas = 2;
                query = "SELECT * FROM OPTCONSECUTIVOSAFILIACION WHERE FECHA>= '" + BeginDate + "' " +
                    "AND FECHA<= '" + FinishDate + "' " +
                    "ORDER BY FECHA;" +
                    "SELECT DISTINCT A.Fecha, A.Emisora, A.Credito, A.Respuesta, B.Descripcion,A.Archivo,A.Registro " +
                    " FROM optRespuestasAfiliacion as A " +
                    " LEFT JOIN catRespuestasCobranza AS B " +
                    " ON A.Respuesta = B.CodigoFacturacion " +
                    " WHERE B.Banco = 'BANORTE' AND A.FECHA>='"+BeginDate+"' AND A.FECHA<='"+FinishDate+"'";
            }


            ///GENERO EL REPORTE EN EXCEL
            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;            
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet ds = new DataSet())
                       {
                            sda.Fill(ds);
                            //Set Name of DataTables.
                            ds.Tables[0].TableName = "Reporte";

                            if (NumeroHojas == 2)
                            {
                                ds.Tables[1].TableName = "Detalle";
                            }

                            if (NumeroHojas == 3)
                            {
                                ds.Tables[1].TableName = "Detalle";
                                ds.Tables[2].TableName = "Respuestas Bancarias";
                            }


                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                foreach (DataTable dt in ds.Tables)
                                {
                                    //Add DataTable as Worksheet.
                                    wb.Worksheets.Add(dt);
                                }

                                //Export the Excel file.
                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename="+Reporte+".xlsx");
                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);
                                    Response.Flush();
                                    Response.End();
                                }
                            }
                        }
                    }
                }
            }


        }
    }
}