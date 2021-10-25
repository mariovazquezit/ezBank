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


                string idAfiliacionMaximo;
                string Consulta2 = "select dbo.fn_getAfiliacionConsecutivoMaximo(1)";
                SqlCommand Comando2 = new SqlCommand(Consulta2, con);
                SqlDataAdapter Adaptador2 = new SqlDataAdapter(Comando2);
                DataTable tabla2 = new DataTable();



                con.Open();
                Adaptador.Fill(tabla);
                cmbEmisora.DataSource = tabla;
                cmbEmisora.DataTextField = "EMISORA";
                cmbEmisora.DataBind();

                Adaptador2.Fill(tabla2);
                idAfiliacionMaximo = tabla2.Rows[0][0].ToString();
                txtIdAfiliacion.Text = idAfiliacionMaximo;
                con.Close();

                string idEmisora = cmbEmisora.Text;
                actualizarNombreEmisora(idEmisora);

                string TipoArchivo = cmbTipoArchivo.Text;
                actualizarTipoArchivo(TipoArchivo);

                string AfiliarXPor = cmbAfiliarClabeCuenta.Text;
                actualizarAfiliarpor(AfiliarXPor);

            }
            else
            {
            }


        }

        


        protected void actualizarAfiliarpor(string AfiliarPor)
        {
            string TipoArchivo = cmbTipoArchivo.Text;
            
            if (AfiliarPor=="Cuenta" && TipoArchivo == "Interbancario")
            {
                Response.Write("<script>alert('ADVERTENCIA: No se pueden afiliar Interbancarios por Cuenta');</script>");
                cmbAfiliarClabeCuenta.Text = "CLABE";
                return;
            }
            
            if (AfiliarPor == "Cuenta")
            {
                lblAfiliarPor.Text = "Banorte por Cuenta, Otros por CLABE";
            }else if (AfiliarPor == "CLABE")
            {
                lblAfiliarPor.Text = "Banorte y Otros, por CLABE";
            }
            
             
        }

        protected void actualizarTipoArchivo(string TipoArchivo)
        {

            string AfiliarPor = cmbAfiliarClabeCuenta.Text;

            if (AfiliarPor == "Cuenta" && TipoArchivo == "Interbancario")
            {
                Response.Write("<script>alert('ADVERTENCIA: No se pueden afiliar Interbancarios por Cuenta');</script>");
                cmbAfiliarClabeCuenta.Text = "CLABE";
                return;
            }


            if (TipoArchivo == "Global")
            {
                lblTipoArchivo.Text = "Interbancario + Banco a Banco";
            }else if (TipoArchivo == "Banco a Banco")
            {
                lblTipoArchivo.Text = "Sólo Créditos Banorte";
            }
            else if (TipoArchivo == "Interbancario")
            {
                lblTipoArchivo.Text = "Todos los Bancos, excepto Banorte";
            }

        }

        protected void actualizarNombreEmisora(string idEmisora)
        {
            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            string Consulta = "select Descripcion from catEmisoras where Emisora='"+idEmisora+"' and Banco='Banorte'";
            SqlCommand Comando = new SqlCommand(Consulta, con);
            SqlDataAdapter Adaptador = new SqlDataAdapter(Comando);
            DataTable tabla = new DataTable();

            string NombreEmisora;

            Adaptador.Fill(tabla);
            NombreEmisora = tabla.Rows[0][0].ToString();
            lblNombreEmisora.Text = NombreEmisora;
            
           
        }


        protected void btnDownloadAfiliadosPendientes_Click(object sender, EventArgs e)
        {

            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;

            string EmisoraEnLinea = "SELECT TOP 1 Emisora FROM catEmisoras WHERE Banco='Banorte' and Descripcion='SIN REINTENTOS'";
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
                ///panelBancoDocumentacion.Visible = false;
            }
            else if (cmbMetodoAfiliacion.Text == "Carga Excel")
            {
                panelAfiliacion.Visible = true;
                panelCargaCSV.Visible = true;
                panelCargaRespuestas.Visible = false;
                ///panelBancoDocumentacion.Visible = true;
            }
            else if (cmbMetodoAfiliacion.Text == "Consulta de Cartera")
            {
                panelAfiliacion.Visible = true;
                panelCargaCSV.Visible = false;
                panelCargaRespuestas.Visible = false;
                ///panelBancoDocumentacion.Visible = true;
            }
            else if (cmbMetodoAfiliacion.Text == "Carga de Respuestas Bancarias")
            {
                panelAfiliacion.Visible = false;
                panelCargaCSV.Visible = false;
                panelCargaRespuestas.Visible = true;
                ///panelBancoDocumentacion.Visible = false;
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
            string TipoArchivo = cmbTipoArchivo.Text;
            string AfiliarPor = cmbAfiliarClabeCuenta.Text;
            string Emisora = cmbEmisora.Text;
            long SiguienteAfiliacion = Int64.Parse(txtIdAfiliacion.Text);

            if (SiguienteAfiliacion<=0){
                Response.Write("<script>alert('ERROR: El Id de Afiliación debe ser mayor o igual a 1');</script>");
            }

            //string CanalAfiliacion = cmbMetodoAfiliacion.Text;
            //int idCanalAfiliacion = 0;
            //if (CanalAfiliacion == "Consulta de Cartera")
            //{
            //    idCanalAfiliacion = 1;
            //}
            //else if (CanalAfiliacion == "Carga Excel")
            //{
            //    idCanalAfiliacion = 2;
            //}


            string ConexionConfig = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection conexion = new SqlConnection(ConexionConfig);

            SqlCommand SP_AFILIACION = new SqlCommand("SP_AFILIACION_BANORTE", conexion);
            SP_AFILIACION.CommandType = CommandType.StoredProcedure;
            SP_AFILIACION.Parameters.AddWithValue("@TipoArchivo", TipoArchivo);
            SP_AFILIACION.Parameters.AddWithValue("@AfiliarPor", AfiliarPor);
            SP_AFILIACION.Parameters.AddWithValue("@Emisora", Emisora);
            SP_AFILIACION.Parameters.AddWithValue("@SiguienteAfiliacion", SiguienteAfiliacion);

            conexion.Open();
            SP_AFILIACION.ExecuteNonQuery();
            conexion.Close();

            //////////////
            panelPreview.Visible = true;
            DataTable DGVPreview = new DataTable();
            DGVPreview = principalClass.SP_Reportes(34);

            dgvPendientesAfiliacion.DataSource = DGVPreview;
            dgvPendientesAfiliacion.DataBind();
            
            DGVPreview = principalClass.SP_Reportes(35);

            dgvAfiliacionBody.DataSource = DGVPreview;
            dgvAfiliacionBody.DataBind();

            btnDescargarArchivo.Visible = true;
            btnValidacionExcel.Visible = true;

            string QueryFileName = "SELECT ARCHIVO FROM OPTCONSECUTIVOSAFILIACION " +
               "WHERE BANCO = 'BANORTE' AND FECHA = CONVERT(VARCHAR, GETDATE(), 126) " +
               "AND CONSECUTIVO = dbo.fn_getConsecutivoAfiliacion('BANORTE', CONVERT(VARCHAR, GETDATE(), 126))";
            DataTable TableFileName = principalClass.Read(QueryFileName);
            string FileName = TableFileName.Rows[0][0].ToString();
            lblFileName.Text ="El nombre del Archivo será: " + FileName;

            Response.Write("<script>alert('ADVERTENCIA: Recuerda que la Banca sólo te deja subir un archivo de afiliación por Emisora al día');</script>");

           
            ////
            ///
          
        }


        protected void DownloadAfiliacion()
        {
            string QueryFileName = "SELECT ARCHIVO FROM OPTCONSECUTIVOSAFILIACION " +
                "WHERE BANCO = 'BANORTE' AND FECHA = CONVERT(VARCHAR, GETDATE(), 126) " +
                "AND CONSECUTIVO = dbo.fn_getConsecutivoAfiliacion('BANORTE', CONVERT(VARCHAR, GETDATE(), 126))";
            DataTable TableFileName = principalClass.Read(QueryFileName);
            string FileName = TableFileName.Rows[0][0].ToString();
            lblFileName.Text = FileName;
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
            int idReporte = 0;

            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);

            if (MetodoCobranza == "")
            {
                return;
            }
            else if (MetodoCobranza == "Consulta de Cartera")
            {

                if (Tipoarchivo == "Global")
                {
                    idReporte = 30;
                }

                if (Tipoarchivo == "Banco a Banco")
                {
                    idReporte = 31;
                }
                if (Tipoarchivo == "Interbancario")
                {
                    idReporte = 32;
                }
             
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
                        
                        SqlCommand sp = new SqlCommand("SP_MANUAL_AFILIACION", con);
                        sp.CommandType = CommandType.StoredProcedure;
                        sp.CommandTimeout = 9000;
                        sp.Parameters.AddWithValue("@tblAfiliacion", dt);
                        sp.Parameters.AddWithValue("@TipoArchivo", Tipoarchivo);

                        con.Open();
                        sp.ExecuteNonQuery();
                        con.Close();

                        idReporte = 22;
                      
                    }
                }

            }

            SqlCommand sp2 = new SqlCommand("SP_REPORTES", con);
            sp2.CommandType = CommandType.StoredProcedure;
            sp2.CommandTimeout = 900;
            sp2.Parameters.AddWithValue("@idReporte", idReporte);
            con.Open();

            SqlDataAdapter da = new SqlDataAdapter(sp2);
            DataTable dt2 = new DataTable();
            da.Fill(dt2);

            panelPreview.Visible = true;
            btnGenerarArchivo.Visible = true;
            dgvPendientesAfiliacion.DataSource = dt2;
            dgvPendientesAfiliacion.DataBind();
            con.Close();
            
            idReporte = 33;
            

                SqlCommand sp3 = new SqlCommand("SP_REPORTES", con);
                sp3.CommandType = CommandType.StoredProcedure;
                sp3.CommandTimeout = 900;
                sp3.Parameters.AddWithValue("@idReporte", idReporte);
          con.Open();
                SqlDataAdapter da3 = new SqlDataAdapter(sp3);
                DataTable dt3 = new DataTable();
                da3.Fill(dt3);

            string AlertaTotales= dt3.Rows[0][0].ToString();
            string AlertaInterbancario= dt3.Rows[0][2].ToString(); ;
            string AlertaBancoABanco= dt3.Rows[0][1].ToString(); ;

            lblAlertaTotales.Text = "Resumen -> Créditos Totales: " + AlertaTotales +
                " /Banco a Banco: " + AlertaBancoABanco + " /Interbancario: " + AlertaInterbancario;

            con.Close();


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

        protected void btnAfiliacionEjemploLayout_Click(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlCommand sp = new SqlCommand("SP_REPORTES", con);
            sp.CommandType = CommandType.StoredProcedure;
            sp.CommandTimeout = 900;
            sp.Parameters.AddWithValue("@IdReporte", 17);

            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(sp);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            using (XLWorkbook wb = new XLWorkbook())
            {
                dt.TableName = "Ejemplo";
                wb.Worksheets.Add(dt);
                //Export the Excel file.
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=EjemploAfiliacion.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        protected void cmbEmisora_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idEmisora = cmbEmisora.Text;
            actualizarNombreEmisora(idEmisora);
        }

        protected void cmbTipoArchivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string TipoArchivo = cmbTipoArchivo.Text;
            actualizarTipoArchivo(TipoArchivo);
        }

        protected void cmbAfiliarClabeCuenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            string AfiliarXPor = cmbAfiliarClabeCuenta.Text;
            actualizarAfiliarpor(AfiliarXPor);

        }

        protected void btnDescargarArchivo_Click(object sender, EventArgs e)
        {
            DownloadAfiliacion();
        }

        protected void btnValidacionExcel_Click(object sender, EventArgs e)
        {

            string QueryFileName = "SELECT ARCHIVO FROM OPTCONSECUTIVOSAFILIACION " +
              "WHERE BANCO = 'BANORTE' AND FECHA = CONVERT(VARCHAR, GETDATE(), 126) " +
              "AND CONSECUTIVO = dbo.fn_getConsecutivoAfiliacion('BANORTE', CONVERT(VARCHAR, GETDATE(), 126))";
            DataTable TableFileName = principalClass.Read(QueryFileName);
            string FileName = TableFileName.Rows[0][0].ToString();

            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;            
            
            string query = "SELECT * FROM pasoAfiliacionBanorteHeader;"+
                "SELECT * FROM pasoAfiliacionBanorteBody;";

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
                            ds.Tables[0].TableName = "Header";
                            ds.Tables[1].TableName = "Body";
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
                                Response.AddHeader("content-disposition", "attachment;filename="+FileName+".xlsx");
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
