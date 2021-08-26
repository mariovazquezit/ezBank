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
using System.IO;
using System.Collections.Generic;
using ClosedXML.Excel;

namespace ezBank.Views
{
    public partial class frmAfiliaciones : System.Web.UI.Page
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
                string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
                SqlConnection con = new SqlConnection(cs);
                string Consulta = "SELECT EMISORA FROM catEmisoras WHERE BANCO='BANORTE' ORDER BY EMISORA";
                SqlCommand Comando = new SqlCommand(Consulta, con);
                SqlDataAdapter Adaptador = new SqlDataAdapter(Comando);
                DataTable tabla = new DataTable();

                con.Open();
                Adaptador.Fill(tabla);
                cmbEmisora.DataSource = tabla;
                cmbEmisora.DataTextField = "EMISORA";
                cmbEmisora.DataBind();
                con.Close();



            }
            else
            {
            }


        }

        protected void btnDownloadAfiliadosPendientes_Click(object sender, EventArgs e)
        {

            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;

            string EmisoraEnLinea = "SELECT TOP 1 Emisora FROM catEmisoras WHERE Banco='Banorte' and Descripcion='En Linea'";
            string EmisoraEspecializada = "SELECT TOP 1 Emisora FROM catEmisoras WHERE Banco='Banorte' and Descripcion='ESPECIALIZADA'";

            SqlConnection conexion = new SqlConnection(cs);


            SqlCommand comando = new SqlCommand(EmisoraEnLinea, conexion);
            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            DataTable tablaResultado = new DataTable();

            SqlCommand comando2 = new SqlCommand(EmisoraEspecializada, conexion);
            SqlDataAdapter adaptador2 = new SqlDataAdapter(comando2);
            DataTable tablaResultado2 = new DataTable();


            conexion.Open();
            comando.CommandTimeout = 9000;
            comando2.CommandTimeout = 9000;
            adaptador.Fill(tablaResultado);
            adaptador2.Fill(tablaResultado2);

            string enlinea = tablaResultado.Rows[0][0].ToString();
            string especializada = tablaResultado2.Rows[0][0].ToString();

            conexion.Close();


            string query = "SELECT idCredito, Nombre, Dependencia, TipoFinanciamiento, " +
                "CONVERT(VARCHAR, PrimerPagoTeorico, 126) AS PrimerPagoTeorico FROM zellSaldosCartera " +
                "WHERE dbo.fn_getEmisoraExitosaAfiliada(IDCREDITO, '" + enlinea + "') = 0 " +
                "OR dbo.fn_getEmisoraExitosaAfiliada(IDCREDITO, '" + especializada + "') = 0 ORDER BY idCredito";


            ///GENERO EL REPORTE EN EXCEL            
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        cmd.CommandTimeout = 9000;
                        sda.SelectCommand = cmd;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);
                            //Set Name of DataTables.                            
                            ds.Tables[0].TableName = "Afiliaciones_Pendientes";

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
                                Response.AddHeader("content-disposition", "attachment;filename=AfiliacionesPendientes.xlsx");
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



            //DownloadCSV(15);
        }

        protected void cmbMetodoAfiliacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelPreview.Visible = false;

            if (cmbMetodoAfiliacion.Text == "")
            {
                panelAfiliacion.Visible = false;
                panelCargaCSV.Visible = false;
                panelCargaRespuestas.Visible = false;
                panelBancoDocumentacion.Visible = false;
            }
            else if (cmbMetodoAfiliacion.Text == "Carga Excel")
            {
                panelAfiliacion.Visible = true;
                panelCargaCSV.Visible = true;
                panelCargaRespuestas.Visible = false;
                panelBancoDocumentacion.Visible = true;
            }
            else if (cmbMetodoAfiliacion.Text == "Consulta de Cartera")
            {
                panelAfiliacion.Visible = true;
                panelCargaCSV.Visible = false;
                panelCargaRespuestas.Visible = false;
                panelBancoDocumentacion.Visible = true;
            }
            else if (cmbMetodoAfiliacion.Text == "Carga de Respuestas Bancarias")
            {
                panelAfiliacion.Visible = false;
                panelCargaCSV.Visible = false;
                panelCargaRespuestas.Visible = true;
                panelBancoDocumentacion.Visible = false;
            }


        }

        protected void btnDownloadAfiliadosExitosos_Click(object sender, EventArgs e)
        {

            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;

            string query = "select Credito, EmisoraAfiliada, EstatusAfiliacion from optafiliacionesexitosas";


            ///GENERO EL REPORTE EN EXCEL            
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        cmd.CommandTimeout = 9000;
                        sda.SelectCommand = cmd;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);
                            //Set Name of DataTables.
                            ds.Tables[0].TableName = "Afiliaciones_Exitosas";

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
                                Response.AddHeader("content-disposition", "attachment;filename=AfiliacionesExitosas.xlsx");
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



            //DownloadCSV(16);
        }


        public void DownloadCSV(int Reportex)
        {
            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlCommand sp = new SqlCommand("SP_REPORTES", con);
            sp.CommandType = CommandType.StoredProcedure;
            sp.CommandTimeout = 900;
            sp.Parameters.AddWithValue("@IdReporte", Reportex);

            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(sp);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();


            ///// Construyo el archivo CSV
            string adjunto = "attachment; filename=Descarga.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", adjunto);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            foreach (DataColumn dc in dt.Columns)
            {
                Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            Response.Write("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    Response.Write(tab + dr[i].ToString());
                    tab = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
        }

        protected void btnGenerarArchivo_Click(object sender, EventArgs e)
        {
            string Emisora = cmbEmisora.Text;
            string TipoArchivo = cmbTipoArchivo.Text;
            string CanalAfiliacion = cmbMetodoAfiliacion.Text;
            int idCanalAfiliacion = 0;

            if (CanalAfiliacion == "Consulta de Cartera")
            {
                idCanalAfiliacion = 1;
            }
            else if (CanalAfiliacion == "Carga Excel")
            {
                idCanalAfiliacion = 2;
            }


            string ConexionConfig = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection conexion = new SqlConnection(ConexionConfig);

            SqlCommand SP_AFILIACION = new SqlCommand("SP_OPERACIONESCARTERA", conexion);
            SP_AFILIACION.CommandType = CommandType.StoredProcedure;
            SP_AFILIACION.Parameters.AddWithValue("@idProceso", idCanalAfiliacion);
            SP_AFILIACION.Parameters.AddWithValue("@valor", Emisora);
            SP_AFILIACION.Parameters.AddWithValue("@valor2", TipoArchivo);

            conexion.Open();
            SP_AFILIACION.ExecuteNonQuery();
            conexion.Close();

            //////////////
            panelPreview.Visible = true;
            DataTable DGVPreview = new DataTable();
            DGVPreview = principalClass.SP_Reportes(17);

            dgvPendientesAfiliacion.DataSource = DGVPreview;
            dgvPendientesAfiliacion.DataBind();
            ////
            ///
            DownloadAfiliacion();

            Response.Write("<script>alert('ADVERTENCIA: Recuerda que la Banca Electrónica sólo te deja subir un archivo de afiliación por Emisora al día');</script>");

        }


        protected void DownloadAfiliacion()
        {
            string QueryFileName = "SELECT ARCHIVO FROM OPTCONSECUTIVOSAFILIACION " +
                "WHERE BANCO = 'BANORTE' AND FECHA = CONVERT(VARCHAR, GETDATE(), 126) " +
                "AND CONSECUTIVO = dbo.fn_getConsecutivoAfiliacion('BANORTE', CONVERT(VARCHAR, GETDATE(), 126))";
            DataTable TableFileName = principalClass.Read(QueryFileName);
            string FileName = TableFileName.Rows[0][0].ToString();

            /////////
            ///
            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlCommand sp = new SqlCommand("SP_REPORTES", con);
            sp.CommandType = CommandType.StoredProcedure;
            sp.CommandTimeout = 900;
            sp.Parameters.AddWithValue("@IdReporte", 18);

            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(sp);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();


            ///// Construyo el archivo CSV
            string adjunto = "attachment; filename=" + FileName.ToString();
            Response.ClearContent();
            Response.AddHeader("content-disposition", adjunto);
            ////Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            foreach (DataColumn dc in dt.Columns)
            {
                //Response.Write(tab + dc.ColumnName);
                //tab = "\t";
            }
            //Response.Write("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    Response.Write(tab + dr[i].ToString());
                    tab = "\t";
                }
                Response.Write("\n");
            }
            Response.End();

        }

        protected void btnVistaPreviaAfiliaciones_Click(object sender, EventArgs e)
        {
            string MetodoCobranza = cmbMetodoAfiliacion.Text;
            string Tipoarchivo = cmbTipoArchivo.Text;
            string CONSULTAX = null;

            if (MetodoCobranza == "")
            {
                return;
            }
            else if (MetodoCobranza == "Consulta de Cartera")
            {

                if (Tipoarchivo == "Banco a Banco")
                {
                    CONSULTAX = "SELECT IDCREDITO AS Credito, CONVERT(VARCHAR,PRIMERPAGOTEORICO,23) AS PrimerPago, " +
                        "TIPOFINANCIAMIENTO as Producto,Dependencia, dbo.fn_getCLABEsinComilla(CLABE) AS CLABE, dbo.fn_getCuentaXClabe(CLABE) AS Cuenta, dbo.fn_getBancoNombrexCLABE(CLABE) as Banco " +
                        " FROM zellSaldosCartera " +
                        "WHERE dbo.fn_getBancoNombrexCLABE(CLABE)='BANORTE' AND " +
                        "(dbo.fn_getEmisoraExitosaAfiliada(IDCREDITO, '00950') = 0 OR " +
                        "dbo.fn_getEmisoraExitosaAfiliada(IDCREDITO, '01077') = 0 )" +
                        "ORDER BY PrimerPagoTeorico ASC";
                }
                if (Tipoarchivo == "Interbancario")
                {
                    CONSULTAX = "SELECT IDCREDITO AS Credito, CONVERT(VARCHAR,PRIMERPAGOTEORICO,23) AS PrimerPago, " +
                        "TIPOFINANCIAMIENTO as Producto,Dependencia, dbo.fn_getCLABEsinComilla(CLABE) AS CLABE, dbo.fn_getBancoNombrexCLABE(CLABE) as Banco " +
                        " FROM zellSaldosCartera " +
                        "WHERE dbo.fn_getBancoNombrexCLABE(CLABE)<>'BANORTE' AND " +
                        "(dbo.fn_getEmisoraExitosaAfiliada(IDCREDITO, '00950') = 0 OR " +
                        "dbo.fn_getEmisoraExitosaAfiliada(IDCREDITO, '01077') = 0 )" +
                        "ORDER BY PrimerPagoTeorico ASC";
                }

                panelPreview.Visible = true;
                btnGenerarArchivo.Visible = true;
                dgvPendientesAfiliacion.DataSource = principalClass.Read(CONSULTAX);
                dgvPendientesAfiliacion.DataBind();
            }
            else if (MetodoCobranza == "Carga Excel")
            {
                //Save the uploaded Excel file.
                string filePath = Server.MapPath("~/CSV_Files/") + "Afiliaciones.xlsx";
                uploadCSVAfiliacion.SaveAs(filePath);

                //Open the Excel file using ClosedXML.
                using (XLWorkbook workBook = new XLWorkbook(filePath))
                {
                    //Read the first Sheet from Excel file.
                    IXLWorksheet workSheet = workBook.Worksheet(1);

                    //Create a new DataTable.
                    DataTable dt = new DataTable();

                    //Loop through the Worksheet rows.
                    bool firstRow = true;
                    foreach (IXLRow row in workSheet.Rows())
                    {
                        //Use the first row to add columns to DataTable.
                        if (firstRow)
                        {
                            foreach (IXLCell cell in row.Cells())
                            {
                                dt.Columns.Add(cell.Value.ToString());
                            }
                            firstRow = false;
                        }
                        else
                        {
                            //Add rows to DataTable.
                            dt.Rows.Add();
                            int i = 0;
                            foreach (IXLCell cell in row.Cells())
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                i++;
                            }
                        }


                        //////
                        ////
                        ///
                        ////
                        //// insertar Datatable en la base de datos                        
                        string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
                        SqlConnection con = new SqlConnection(cs);
                        SqlCommand sp = new SqlCommand("SP_MANUAL_AFILIACION", con);
                        sp.CommandType = CommandType.StoredProcedure;
                        sp.CommandTimeout = 900;
                        sp.Parameters.AddWithValue("@tblAfiliacion", dt);
                        sp.Parameters.AddWithValue("@TipoArchivo", Tipoarchivo);

                        con.Open();
                        sp.ExecuteNonQuery();
                        con.Close();



                        SqlCommand sp2 = new SqlCommand("SP_REPORTES", con);
                        sp2.CommandType = CommandType.StoredProcedure;
                        sp2.CommandTimeout = 900;
                        sp2.Parameters.AddWithValue("@idReporte", 22);
                        con.Open();

                        SqlDataAdapter da = new SqlDataAdapter(sp2);
                        DataTable dt2 = new DataTable();
                        da.Fill(dt2);



                        panelPreview.Visible = true;
                        btnGenerarArchivo.Visible = true;
                        dgvPendientesAfiliacion.DataSource = dt2;
                        dgvPendientesAfiliacion.DataBind();


                    }
                }

            }


        }

        protected void btnCargarRespuesta_Click(object sender, EventArgs e)
        {
            string FileName= System.IO.Path.GetFileName(uploadRespuestaBancaria.FileName);
            string FileExtension = System.IO.Path.GetExtension(uploadRespuestaBancaria.FileName);
            string filePath = Server.MapPath("~/CSV_Files/") + "RespuestaBancaria" + FileExtension;
            uploadRespuestaBancaria.SaveAs(filePath);

            DataTable dt = new DataTable();
            DataColumn col = new DataColumn("Respuesta");
            col.DataType = System.Type.GetType("System.String");
            dt.Columns.Add(col);

            string[] aa = File.ReadAllLines(filePath);
            foreach (var item in aa)
            {
                DataRow dr = dt.NewRow();
                dr[0] = item.ToString();
                dt.Rows.Add(dr);
            }

            /////////
            ///
            //// insertar Datatable en la base de datos                        
            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlCommand sp = new SqlCommand("SP_RESPUESTAS_AFILIACION", con);
            sp.CommandType = CommandType.StoredProcedure;
            sp.CommandTimeout = 900;
            sp.Parameters.AddWithValue("@tblRespuestasAfiliacion", dt);
            sp.Parameters.AddWithValue("@nombreArchivo", FileName);

            con.Open();
            sp.ExecuteNonQuery();
            con.Close();





            ///////ACTUALIZO VISTA PREVIA DE RESPUESTAS COBRANZA
            SqlCommand sp3 = new SqlCommand("SP_REPORTES", con);
            sp3.CommandType = CommandType.StoredProcedure;
            sp3.Parameters.AddWithValue("@idReporte", 25);
            con.Open();

            SqlDataAdapter da3 = new SqlDataAdapter(sp3);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);

            dgvRespuestasAfiliacion.DataSource = dt3;
            dgvRespuestasAfiliacion.DataBind();

            con.Close();

            btnDownloadRespuestas.Visible = true;

            Response.Write("<script>alert('Respuestas de Afiliación cargadas exitosamente');</script>");
        }

        protected void btnNuevaAfiliacion_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmAfiliaciones.aspx");
        }

        protected void btnDownloadRespuestas_Click(object sender, EventArgs e)
        {
            string query = "SELECT CONVERT(VARCHAR, A.FECHA,126) AS FECHA, A.EMISORA,A.ARCHIVO, A.CREDITO,A.RESPUESTA, " +
                "B.Descripcion FROM PasoRespuestasAfiliacion AS A "+
                " LEFT JOIN catRespuestasCobranza AS B "+
                " ON A.Respuesta = B.CodigoFacturacion "+
                " WHERE LEN(A.RESPUESTA)> 0 AND B.Banco = 'BANORTE'";

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
                            ds.Tables[0].TableName = "Afiliacion";

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
                                Response.AddHeader("content-disposition", "attachment;filename= RespuestasAfiliacion.xlsx");
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
