using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using ezBank.Classes;

namespace ezBank.Views
{
    public partial class frmCobranza : System.Web.UI.Page
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
                
                string Query = "SELECT DISTINCT PRODUCTO FROM zellConvenios order by PRODUCTO DESC";
                DataTable Resultado = principalClass.Read(Query);

                cmbProducto.DataSource = Resultado;
                cmbProducto.DataTextField = "PRODUCTO";
                cmbProducto.DataBind();

                //////
                
                string Query2 = "SELECT DISTINCT CONVERT(VARCHAR,A.SIGPAGO,126) AS Fecha FROM zellSaldosCartera as A " +
                    "LEFT JOIN zellConvenios AS B ON A.Dependencia=B.vDescription " +
                    "WHERE A.SigPago>=CONVERT(VARCHAR,GETDATE()-7,126)";
                DataTable Resultado2 = principalClass.Read(Query2);

                cmbProximoPago.DataSource = Resultado2;
                cmbProximoPago.DataTextField = "Fecha";
                cmbProximoPago.DataBind();

                /////

                
                string Query3 = "SELECT DISTINCT SUPERCONVENIO FROM zellConvenios " +
                    " ORDER BY SuperConvenio";
                DataTable Resultado3 = principalClass.Read(Query3);

                cmbConvenio.DataSource = Resultado3;
                cmbConvenio.DataTextField = "SUPERCONVENIO";
                cmbConvenio.DataBind();

            }
            else
            {
            }
        }



     

        protected void cmbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Producto = cmbProducto.Text;
            string Query = "SELECT DISTINCT SUPERCONVENIO FROM zellConvenios " +
                "WHERE Producto='"+ Producto + "' ORDER BY SuperConvenio";
            DataTable Resultado = principalClass.Read(Query);

            cmbConvenio.DataSource = Resultado;
            cmbConvenio.DataTextField = "SUPERCONVENIO";
            cmbConvenio.DataBind();
        }

        protected void cmbConvenio_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Convenio = cmbConvenio.Text;
            string Query = "SELECT DISTINCT CONVERT(VARCHAR,A.SIGPAGO,126) AS Fecha FROM zellSaldosCartera as A " +
                "LEFT JOIN zellConvenios AS B ON A.Dependencia=B.vDescription " +
                "WHERE B.SuperConvenio='" + Convenio + "' " +
                "AND A.SigPago>=CONVERT(VARCHAR,GETDATE()-7,126)";
            DataTable Resultado = principalClass.Read(Query);

            cmbProximoPago.DataSource = Resultado;
            cmbProximoPago.DataTextField = "Fecha";
            cmbProximoPago.DataBind();
        }

        protected void cmbBancoCobranza_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Banco = cmbBancoCobranza.Text;

            if (Banco == "Banorte")
            {
                panelEmisoraBanco.Visible = true;
                cmbBancoEmisora.Items.Clear();
                cmbBancoEmisora.Items.Add("En Linea");
                cmbBancoEmisora.Items.Add("Especializada");
                panelHoraSantander.Visible = false;
            }
            else if(Banco=="Banamex")
            {
                panelEmisoraBanco.Visible = false;
                panelHoraSantander.Visible = false;
            }
            else if (Banco == "Santander")
            {
                panelEmisoraBanco.Visible = true;
                cmbBancoEmisora.Items.Clear();
                cmbBancoEmisora.Items.Add("Banco a Banco");
                cmbBancoEmisora.Items.Add("Interbancario");
                panelHoraSantander.Visible = true;
            }
            else if(Banco=="BBVA Bancomer")
            {
                panelEmisoraBanco.Visible = true;
                cmbBancoEmisora.Items.Clear();
                cmbBancoEmisora.Items.Add("Parciales");
                cmbBancoEmisora.Items.Add("Post Nomina");
                cmbBancoEmisora.Items.Add("Post Nomina Sin Reintentos");
                cmbBancoEmisora.Items.Add("Tradicional");
                panelHoraSantander.Visible = false;
            }
            else
            {
                panelEmisoraBanco.Visible = false;
                panelHoraSantander.Visible = false;
                cmbBancoEmisora.Items.Clear();
            }
        }

        protected void btnPreviewEstrategia_Click(object sender, EventArgs e)
        {

            string metodoCobranza = cmbMetodoCobranza.Text;

            if (metodoCobranza=="") {
                return;
            } else if (metodoCobranza=="Consulta de Cartera")
            {            
            panelPreviewEstrategia.Visible = true;

            string producto = cmbProducto.Text;
            string convenio = cmbConvenio.Text;
            string sigpago = cmbProximoPago.Text;
            string ultimopagox = txtDiasUltimoPago.Text;
            string atrasox = txtDiasAtraso.Text;
            string banco = cmbBancoInicial.Text;

            int ultimopago = Convert.ToInt32(ultimopagox);
            int atraso = Convert.ToInt32(atrasox);
            int idproceso = 1;

            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection connection = new SqlConnection(cs);
            connection.Open();
            SqlCommand command = new SqlCommand("SP_PREVIEW_ESTRATEGIA_COBRANZA", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PRODUCTO", producto);
            command.Parameters.AddWithValue("@CONVENIO", convenio);
            command.Parameters.AddWithValue("@SIGPAGO", sigpago);
            command.Parameters.AddWithValue("@ULTIMOPAGO", ultimopago);
            command.Parameters.AddWithValue("@ATRASO", atraso);
            command.Parameters.AddWithValue("@BANCO", banco);
            command.Parameters.AddWithValue("@IDPROCESO", idproceso);

            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            
            dgvPreviewEstrategia.DataSource = dt;
            dgvPreviewEstrategia.DataBind();
            } else if(metodoCobranza=="Carga Excel")
            {
                panelPreviewEstrategia.Visible = true;

                //Save the uploaded Excel file.
                string filePath = Server.MapPath("~/CSV_Files/") + "Domiciliacion.xlsx";
                uploadCSVCobranza.SaveAs(filePath);

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
                        SqlCommand sp = new SqlCommand("SP_MANUAL_DOMICILIACION", con);
                        sp.CommandType = CommandType.StoredProcedure;
                        sp.CommandTimeout = 900;
                        sp.Parameters.AddWithValue("@tblDomiciliacion", dt);

                        con.Open();
                        sp.ExecuteNonQuery();
                        con.Close();


                        ///panelPreview.Visible = true;
                        ///
                        SqlCommand sp2 = new SqlCommand("SP_REPORTES", con);
                        sp2.CommandType = CommandType.StoredProcedure;
                        sp2.CommandTimeout = 900;
                        sp2.Parameters.AddWithValue("@idReporte", 21);                        
                        con.Open();

                        SqlDataAdapter da = new SqlDataAdapter(sp2);
                        DataTable dt2 = new DataTable();
                        da.Fill(dt2);

                        dgvPreviewEstrategia.DataSource = dt2;
                        dgvPreviewEstrategia.DataBind();

                        con.Close();

                    }
                }
                    }
            btnGenerarCobranza.Visible = true;
        }

        protected void cmbEstrategia_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Estrategia = cmbEstrategia.Text;
            

            if (Estrategia=="Cuota Actual")
            {
                panelParticionCuotaActual.Visible = true;
                panelCuotasVencidas.Visible = false;
                ocultarCuotasVencidas();
            }
            else if (Estrategia== "Vencido + Cuota Actual")
            {
                panelCuotasVencidas.Visible = true;
                panelParticionCuotaActual.Visible = true;
            }
            else if(Estrategia == "Vencido")
            {
                panelParticionCuotaActual.Visible = false;
                panelCuotasVencidas.Visible = true;                
            }
            else
            {
                panelParticionCuotaActual.Visible = false;
                panelCuotasVencidas.Visible = false;
                ocultarCuotasVencidas();
            }

        }

        protected void cmbMetodoCobranza_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Canal = cmbMetodoCobranza.Text;
            panelPreviewDomiciliacion.Visible = false;

            if (Canal == "")
            {
                panelCargaCSV.Visible = false;
                panelEstrategiaCobranza.Visible = false;
                panelBancoCobranza.Visible = false;
                panelPreviewDomiciliacion.Visible = false;
                panelRespuestasCobranza.Visible = false;
            }
            else if (Canal == "Carga Excel")
            {
                panelCargaCSV.Visible = true;
                panelEstrategiaCobranza.Visible = false;
                panelBancoCobranza.Visible = true;
                panelPreviewDomiciliacion.Visible = true;
                panelRespuestasCobranza.Visible = false;
            }
            else if (Canal == "Consulta de Cartera")
            {
                panelCargaCSV.Visible = false;
                panelEstrategiaCobranza.Visible = true;
                panelBancoCobranza.Visible = true;
                panelPreviewDomiciliacion.Visible = true;
                panelRespuestasCobranza.Visible = false;
            }
            else if (Canal == "Carga de Respuestas Bancarias")
            {
                panelCargaCSV.Visible = false;
                panelEstrategiaCobranza.Visible = false;
                panelBancoCobranza.Visible = false;
                panelPreviewDomiciliacion.Visible = false;
                panelRespuestasCobranza.Visible = true;
            }
        }

        protected void txtCuotasVencidas_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cuotas = txtCuotasVencidas.Text;
            ocultarCuotasVencidas();

            if (cuotas == "1")
            {
                panelCuotaVencida1.Visible = true;
            }else if (cuotas == "2")
            {
                panelCuotaVencida1.Visible = true;
                panelCuotaVencida2.Visible = true;
            }else if (cuotas == "3")
            {
                panelCuotaVencida1.Visible = true;
                panelCuotaVencida2.Visible = true;
                panelCuotaVencida3.Visible = true;
            }else if (cuotas == "4")
            {
                panelCuotaVencida1.Visible = true;
                panelCuotaVencida2.Visible = true;
                panelCuotaVencida3.Visible = true;
                panelCuotaVencida4.Visible = true;
            }
        }

        protected void ocultarCuotasVencidas()
        {
            panelCuotaVencida1.Visible = false;
            panelCuotaVencida2.Visible = false;
            panelCuotaVencida3.Visible = false;
            panelCuotaVencida4.Visible = false;
        }

        protected void cmbParticionCuotaActual_SelectedIndexChanged(object sender, EventArgs e)
        {
            string particionactual = cmbParticionCuotaActual.Text;
            string particionv1 = cmbParticionVencida1.Text;
            string particionv2 = cmbParticionVencida2.Text;
            string particionv3 = cmbParticionVencida3.Text;
            string particionv4 = cmbParticionVencida4.Text;

            if (particionactual== "")
            {
                return;
            }else if(particionactual == particionv1|| particionactual == particionv2 || particionactual == particionv3 || particionactual == particionv4)
            {
                Response.Write("<script>alert('CUIDADO! No puedes repetir la misma partición en más de una cuota. SELECCIONA UNA PARTICION QUE NO HAYAS USADO EN OTRA CUOTA');</script>");
                cmbParticionCuotaActual.Text = "";
            }
        }

        protected void cmbParticionVencida1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string particionactual = cmbParticionCuotaActual.Text;
            string particionv1 = cmbParticionVencida1.Text;
            string particionv2 = cmbParticionVencida2.Text;
            string particionv3 = cmbParticionVencida3.Text;
            string particionv4 = cmbParticionVencida4.Text;

            if (particionv1 == "")
            {
                return;
            }
            else if (particionactual == particionv1 || particionv1 == particionv2 || particionv1 == particionv3 || particionv1 == particionv4)
            {
                Response.Write("<script>alert('CUIDADO! No puedes repetir la misma partición en más de una cuota. SELECCIONA UNA PARTICION QUE NO HAYAS USADO EN OTRA CUOTA');</script>");
                cmbParticionVencida1.Text = "";
            }
        }

        protected void cmbParticionVencida2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string particionactual = cmbParticionCuotaActual.Text;
            string particionv1 = cmbParticionVencida1.Text;
            string particionv2 = cmbParticionVencida2.Text;
            string particionv3 = cmbParticionVencida3.Text;
            string particionv4 = cmbParticionVencida4.Text;

            if (particionv2 == "")
            {
                return;
            }
            else if (particionactual == particionv2 || particionv1 == particionv2 || particionv2 == particionv3 || particionv2 == particionv4)
            {
                Response.Write("<script>alert('CUIDADO! No puedes repetir la misma partición en más de una cuota. SELECCIONA UNA PARTICION QUE NO HAYAS USADO EN OTRA CUOTA');</script>");
                cmbParticionVencida2.Text = "";
            }
        }

        protected void cmbParticionVencida3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string particionactual = cmbParticionCuotaActual.Text;
            string particionv1 = cmbParticionVencida1.Text;
            string particionv2 = cmbParticionVencida2.Text;
            string particionv3 = cmbParticionVencida3.Text;
            string particionv4 = cmbParticionVencida4.Text;

            if (particionv3 == "")
            {
                return;
            }
            else if (particionactual == particionv3 || particionv1 == particionv3 || particionv2 == particionv3 || particionv3 == particionv4)
            {
                Response.Write("<script>alert('CUIDADO! No puedes repetir la misma partición en más de una cuota. SELECCIONA UNA PARTICION QUE NO HAYAS USADO EN OTRA CUOTA');</script>");
                cmbParticionVencida3.Text = "";
            }
        }

        protected void cmbParticionVencida4_SelectedIndexChanged(object sender, EventArgs e)
        {
            string particionactual = cmbParticionCuotaActual.Text;
            string particionv1 = cmbParticionVencida1.Text;
            string particionv2 = cmbParticionVencida2.Text;
            string particionv3 = cmbParticionVencida3.Text;
            string particionv4 = cmbParticionVencida4.Text;

            if (particionv4 == "")
            {
                return;
            }
            else if (particionactual == particionv4 || particionv1 == particionv4 || particionv2 == particionv4 || particionv3 == particionv4)
            {
                Response.Write("<script>alert('CUIDADO! No puedes repetir la misma partición en más de una cuota. SELECCIONA UNA PARTICION QUE NO HAYAS USADO EN OTRA CUOTA');</script>");
                cmbParticionVencida4.Text = "";
            }
        }

        protected void btnGenerarCobranza_Click(object sender, EventArgs e)
        {
            string banco = cmbBancoCobranza.Text;
            string emisora = cmbBancoEmisora.Text;
            string HoraSantander = cmbHoraSantander.Text;

            int RecibeParametros = 0;
            string SP = null;
            string Parametros = null;
            string nombreParametros = null;

            string Parametros2 = null;
            string nombreParametros2 = null;

            DataTable Emisoraidtable;
            string Emisoraid;

            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);

            if (banco == "Banamex")
            {
                RecibeParametros = 0;
                SP = "SP_DOMI_BANAMEX";
            }

            if (banco == "Banorte")
            {                
                SP = "SP_DOMI_BANORTE";
                RecibeParametros = 1;                
                Emisoraidtable = getEmisoraID(emisora, banco);
                Emisoraid= Emisoraidtable.Rows[0][0].ToString();
                Parametros = Emisoraid;
                nombreParametros = "@emisora";
            }
            if (banco == "BBVA Bancomer")
            {
                SP = "SP_DOMI_BBVA_BANCOMER";
                RecibeParametros = 1;
                Emisoraidtable = getEmisoraID(emisora, banco);
                Emisoraid = Emisoraidtable.Rows[0][0].ToString();
                Parametros = Emisoraid;
                nombreParametros = "@emisora";
            }if (banco == "Santander")
            {
                SP = "SP_DOMI_SANTANDER";
                RecibeParametros = 2;                                
                Parametros = emisora;
                nombreParametros = "@TipoArchivo";
                Parametros2 = HoraSantander;
                nombreParametros2 = "@HoraEjecucion";
            }


            SqlCommand sp = new SqlCommand(SP, con);
            sp.CommandType = CommandType.StoredProcedure;
            sp.CommandTimeout = 900;
            
            if (RecibeParametros == 1) {
                sp.Parameters.AddWithValue(nombreParametros, Parametros);
            }
            if (RecibeParametros == 2)
            {
                sp.Parameters.AddWithValue(nombreParametros, Parametros);
                sp.Parameters.AddWithValue(nombreParametros2, Parametros2);
            }
            
            con.Open();
            sp.ExecuteNonQuery();
            con.Close();

            DownloadCobranza(banco);

        }

        static public DataTable getEmisoraID(string emisora, string Banco)
        {
            string ConexionConfig = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection conexion = new SqlConnection(ConexionConfig);

            string consulta = "SELECT TOP 1 EMISORA FROM catEmisoras " +
                "WHERE Banco = '" + Banco + "' AND Descripcion = '" + emisora + "'";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            DataTable tablaResultado = new DataTable();

            conexion.Open();
            comando.CommandTimeout = 9000;
            adaptador.Fill(tablaResultado);
            conexion.Close();
            return tablaResultado;
        }

        protected void DownloadCobranza(string Banco)
        {
            string QueryFileName = "SELECT ARCHIVO FROM optConsecutivos WHERE BANCO = '"+Banco+"' " +
                "AND FECHA = CONVERT(VARCHAR, GETDATE(), 126) AND " +
                "Consecutivo = dbo.fn_getConsecutivoDomi('"+Banco+"', CONVERT(VARCHAR, GETDATE(), 126))";
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

        protected void btnRespuestaCobranza_Click(object sender, EventArgs e)
        {
            string BancoRespuesta = cmbBancoRespuesta.Text;
            string spNombre = null;
            string FileName = System.IO.Path.GetFileName(fuRespuestaCobranza.FileName);
            string FileExtension = System.IO.Path.GetExtension(fuRespuestaCobranza.FileName);
            string filePath = Server.MapPath("~/CSV_Files/") + "RespuestaBancaria" + FileExtension;
            fuRespuestaCobranza.SaveAs(filePath);

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
            SqlCommand sp = new SqlCommand("SP_RESPUESTAS_COBRANZA", con);
            sp.CommandType = CommandType.StoredProcedure;
            sp.CommandTimeout = 900;
            sp.Parameters.AddWithValue("@tblRespuestasCobranza", dt);


            if (BancoRespuesta == "Santander") { spNombre = "SP_RESPUESTAS_COBRANZA_SANTANDER"; }
            if (BancoRespuesta == "Banamex") { spNombre = "SP_RESPUESTAS_COBRANZA_BANAMEX"; }
            if (BancoRespuesta == "Banorte En Linea") { spNombre = "SP_RESPUESTAS_COBRANZA_BANORTE_ENLINEA"; }

            SqlCommand sp2 = new SqlCommand(spNombre, con);
            sp2.CommandType = CommandType.StoredProcedure;
            sp2.CommandTimeout = 900;
            sp2.Parameters.AddWithValue("@nombreArchivo", FileName);


            con.Open();
            sp.ExecuteNonQuery();
            sp2.ExecuteNonQuery();
            con.Close();


            ///////ACTUALIZO VISTA PREVIA DE RESPUESTAS COBRANZA
            SqlCommand sp3 = new SqlCommand("SP_REPORTES", con);
            sp3.CommandType = CommandType.StoredProcedure;
            sp3.Parameters.AddWithValue("@idReporte", 24);
            con.Open();

            SqlDataAdapter da3 = new SqlDataAdapter(sp3);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);

            dgvRespuestasCobranza.DataSource = dt3;
            dgvRespuestasCobranza.DataBind();

            con.Close();
            btnDownloadRespuestas.Visible = true;

            Response.Write("<script>alert('Respuestas de Cobranza cargadas exitosamente');</script>");

           
        }

        protected void btnDownloadRespuestas_Click(object sender, EventArgs e)
        {

            string query = "SELECT A.FECHA,A.BANCO, A.EMISORA, A.ARCHIVO, A.CREDITO, A.MONTO, " +
                "A.RESPUESTA, B.Descripcion FROM pasoRespuestasCobranza AS A "+
                " LEFT JOIN catRespuestasCobranza AS B "+
                " ON A.Respuesta = B.CodigoFacturacion AND A.Banco = B.BANCO "+
                "ORDER BY Credito";
                
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
                            ds.Tables[0].TableName = "Cobranza";
                          
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
                                Response.AddHeader("content-disposition", "attachment;filename= RespuestasCobranza.xlsx");
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