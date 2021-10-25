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
                
                string Query2 = "SELECT DISTINCT CONVERT(VARCHAR,SIGPAGO,126) AS FECHA FROM zellSaldosCartera "+
                                " WHERE SigPago IS NOT NULL "+
                                " ORDER BY CONVERT(VARCHAR, SIGPAGO,126) ";
                DataTable Resultado2 = principalClass.Read(Query2);

                cmbProximoPago.DataSource = Resultado2;
                cmbProximoPago.DataTextField = "FECHA";
                cmbProximoPago.DataBind();

                /////


                string Query3 = "select distinct b.SuperConvenio from zellSaldosCartera as a " +
                    " left join zellConvenios as b " +
                    " on a.Dependencia = b.vDescription " +
                    " Order by  b.SuperConvenio ";

                DataTable Resultado3 = principalClass.Read(Query3);

                cmbConvenio.DataSource = Resultado3;
                cmbConvenio.DataTextField = "SUPERCONVENIO";
                cmbConvenio.DataBind();

            }
            else
            {
            }
        }


        protected void actualizarLabelEmisora()
        {
            try { 
            string Banco = cmbBancoCobranza.Text;
            string Emisora = cmbBancoEmisora.Text;
            string query = "SELECT Descripcion from catEmisoras where Banco='" + Banco + "' and Emisora='" + Emisora + "'";
            DataTable Resultado = principalClass.Read(query);
            string Descripcion = Resultado.Rows[0][0].ToString();
            lblEmisora.Text = Descripcion;
            }
            catch { 
                lblEmisora.Text = ""; }
        }
     
        protected void actualizarMetodoCobro()
        {
            string MetodoCobro = cmbMetodoCobro.Text;
            string Banco = cmbBancoCobranza.Text;
            if (MetodoCobro == "CLABE")
            {
                lblMetodoCobro.Text = Banco + " y Otros, por CLABE";
            }else if (MetodoCobro == "Cuenta") {
                lblMetodoCobro.Text = Banco + " por Cuenta, Otros por CLABE";
            }            
        }


        protected void actualizaArchivoSalida()
        {
            string ArchivoSalida = cmbArchivoSalida.Text;
            string Banco = cmbBancoCobranza.Text;
            if (ArchivoSalida == "Global")
            {
                lblArchivoSalida.Text = "Interbancario + Banco a Banco";
            }else if (ArchivoSalida == "Interbancario")
            {
                lblArchivoSalida.Text = "Todos los Bancos, excepto " + Banco;
            }
            else if (ArchivoSalida == "Banco a Banco")
            {
                lblArchivoSalida.Text = "Sólo Créditos " + Banco;
            }
        }

        protected void cmbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Producto = cmbProducto.Text;
            string Query = "SELECT DISTINCT A.Dependencia FROM zellSaldosCartera AS A "+
                            " LEFT JOIN zellConvenios AS B "+
                            " ON A.Dependencia = B.vDescription "+
                            " WHERE b.Producto = '"+Producto+"' ORDER BY A.Dependencia ";
            DataTable Resultado = principalClass.Read(Query);

            cmbConvenio.DataSource = Resultado;
            cmbConvenio.DataTextField = "Dependencia";
            cmbConvenio.DataBind();
        }

        protected void cmbConvenio_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Convenio = cmbConvenio.Text;
            string Query = "SELECT DISTINCT CONVERT(VARCHAR,SIGPAGO,126) AS FECHA FROM zellSaldosCartera "+
                            " WHERE SigPago IS NOT NULL "+
                            " ORDER BY CONVERT(VARCHAR, SIGPAGO,126) ";
            DataTable Resultado = principalClass.Read(Query);

            cmbProximoPago.DataSource = Resultado;
            cmbProximoPago.DataTextField = "FECHA";
            cmbProximoPago.DataBind();
        }

        protected void cmbBancoCobranza_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Banco = cmbBancoCobranza.Text;
            string query = null;
            DataTable Resultado;

            if (Banco == "Banorte")
            {
                panelEmisoraBanco.Visible = true;

                query = "SELECT Emisora from catEmisoras where Banco = '" + Banco + "'";
                Resultado = principalClass.Read(query);

                cmbBancoEmisora.DataSource = Resultado;
                cmbBancoEmisora.DataTextField = "Emisora";
                cmbBancoEmisora.DataBind();                

                panelHoraSantander.Visible = false;

                panelTipodeCobro.Visible = true;
                cmbTipoCobro.Items.Clear();
                cmbTipoCobro.Items.Add("En Linea");
                cmbTipoCobro.Items.Add("Tradicional");

                panelModalidad.Visible = true;
                panelArchivoSalida.Visible = true;

                actualizarLabelEmisora();
                actualizarMetodoCobro();
                actualizaArchivoSalida();
                actualizarTipoCobro();
            }
            else if(Banco=="Banamex")
            {
                panelEmisoraBanco.Visible = false;
                panelHoraSantander.Visible = false;
                panelTipodeCobro.Visible =true;
                panelModalidad.Visible = true;
                panelArchivoSalida.Visible = true;
                cmbTipoCobro.Items.Clear();                
                cmbTipoCobro.Items.Add("En Linea");
                cmbTipoCobro.Items.Add("Interbancario");
                actualizarLabelEmisora();
                actualizarMetodoCobro();
                actualizaArchivoSalida();
                actualizarTipoCobro();

            }
            else if (Banco == "Santander")
            {
                query = "select idCliente from catDatosDomi where Banco = '"+ Banco + "'";
                Resultado = principalClass.Read(query);
                                
                panelTipodeCobro.Visible = true;
                panelModalidad.Visible = true;
                panelArchivoSalida.Visible = true;

                panelEmisoraBanco.Visible = true;
                cmbBancoEmisora.DataSource = Resultado;
                cmbBancoEmisora.DataTextField = "idCliente";
                cmbBancoEmisora.DataBind();      
                
                cmbTipoCobro.Items.Clear();
                cmbTipoCobro.Items.Add("Banco a Banco");
                cmbTipoCobro.Items.Add("Interbancario");
                panelHoraSantander.Visible = true;
                
                actualizarTipoCobro();
                actualizarLabelEmisora();
                actualizarMetodoCobro();
                actualizaArchivoSalida();                
            }
            else if(Banco=="BBVA Bancomer")
            {
                panelEmisoraBanco.Visible = true;                
                panelHoraSantander.Visible = false;
                panelTipodeCobro.Visible = true;
                panelModalidad.Visible = true;
                panelArchivoSalida.Visible = true;

                cmbTipoCobro.Items.Clear();
                cmbTipoCobro.Items.Add("Banco a Banco");
                cmbTipoCobro.Items.Add("Interbancario");
                
                query = "SELECT Emisora from catEmisoras where Banco = '" + Banco + "'";
                Resultado = principalClass.Read(query);

                cmbBancoEmisora.Items.Clear();
                cmbBancoEmisora.DataSource = Resultado;
                cmbBancoEmisora.DataTextField = "Emisora";
                cmbBancoEmisora.DataBind();

                actualizarTipoCobro();
                actualizarLabelEmisora();
                actualizarMetodoCobro();
                actualizaArchivoSalida();

            }
            else
            {
                panelEmisoraBanco.Visible = false;
                panelHoraSantander.Visible = false;
                panelTipodeCobro.Visible = false;
                panelModalidad.Visible = false;
                panelArchivoSalida.Visible = false;
                cmbBancoEmisora.Items.Clear();

            }
        }

        protected void ConsultaCartera()
        {


            int diasultimopago = Int32.Parse(txtDiasUltimoPago.Text);
            string bancoCliente = cmbBancoInicial.Text;
            string estrategia = cmbEstrategia.Text;
            string particionCuotaActual = cmbParticionCuotaActual.Text;
            int VencidosXCobrar = Int32.Parse(txtCuotasVencidas.Text);
            string cuotaVencida1 = cmbParticionVencida1.Text;
            string cuotaVencida2 = cmbParticionVencida2.Text;
            string cuotaVencida3 = cmbParticionVencida3.Text;
            string cuotaVencida4 = cmbParticionVencida4.Text;
           
            //// ASIGNO VALORES A LA VARIABLE CONVENIO
            List<string> listConvenios = new List<string>();
            foreach (int index in cmbConvenio.GetSelectedIndices())
            {
                listConvenios.Add(cmbConvenio.Items[index].Value);
            }
            var convenio = String.Join("|", listConvenios.ToArray());

            ///// ASIGNO VALORES A LA VARIABLE PROXIMO PAGO
            List<string> listProximoPago = new List<string>();
            foreach (int index in cmbProximoPago.GetSelectedIndices())
            {
                listProximoPago.Add(cmbProximoPago.Items[index].Value);
            }
            var proximoPago = String.Join("|", listProximoPago.ToArray());


            /// ASIGNO VALORES A LA VARIABLE BANCO DEL CLIENTE
            List<string> listBancoCliente = new List<string>();
            foreach (int index in cmbBancoInicial.GetSelectedIndices())
            {
                listBancoCliente.Add(cmbBancoInicial.Items[index].Value);
            }
            var bancoInicial = String.Join("| ", listBancoCliente.ToArray());
            ///////

            if (estrategia == "" || estrategia == " " || estrategia == null)
            {
                Response.Write("<script>alert('Advertencia! Tienes que elegir una Estrategia');</script>");
                return;
            }

            if ((estrategia == "Vencido + Cuota Actual" || estrategia == "Vencido") && (VencidosXCobrar <= 0))
            {
                Response.Write("<script>alert('Advertencia! Tienes que elegir al menos 1 cuota Vencida');</script>");
                return;
            }

            if ((particionCuotaActual == "" || particionCuotaActual == null || particionCuotaActual == " ") &&
                (estrategia == "Cuota Actual" || estrategia == "Vencido + Cuota Actual"))
            {
                Response.Write("<script>alert('Advertencia! Tienes que elegir una Particion para la Cuota Actual');</script>");
                return;
            }

            if ((VencidosXCobrar == 4) &&
                (cuotaVencida1 == "" || cuotaVencida1 == " " || cuotaVencida1 == null ||
                cuotaVencida2 == "" || cuotaVencida2 == " " || cuotaVencida2 == null ||
                cuotaVencida3 == "" || cuotaVencida3 == " " || cuotaVencida3 == null ||
                cuotaVencida4 == "" || cuotaVencida4 == " " || cuotaVencida4 == null)
                )
            {
                Response.Write("<script>alert('Advertencia! Tienes que elegir una partición para cada Cuota Vencida');</script>");
                return;
            }

            if ((VencidosXCobrar == 3) &&
                (cuotaVencida1 == "" || cuotaVencida1 == " " || cuotaVencida1 == null ||
                cuotaVencida2 == "" || cuotaVencida2 == " " || cuotaVencida2 == null ||
                cuotaVencida3 == "" || cuotaVencida3 == " " || cuotaVencida3 == null))
            {
                Response.Write("<script>alert('Advertencia! Tienes que elegir una partición para cada Cuota Vencida');</script>");
                return;
            }

            if ((VencidosXCobrar == 2) &&
               (cuotaVencida1 == "" || cuotaVencida1 == " " || cuotaVencida1 == null ||
               cuotaVencida2 == "" || cuotaVencida2 == " " || cuotaVencida2 == null)
               )
            {
                Response.Write("<script>alert('Advertencia! Tienes que elegir una partición para cada Cuota Vencida');</script>");
                return;
            }

            if ((VencidosXCobrar == 1) &&
               (cuotaVencida1 == "" || cuotaVencida1 == " " || cuotaVencida1 == null)
               )
            {
                Response.Write("<script>alert('Advertencia! Tienes que elegir una partición para cada Cuota Vencida');</script>");
                return;
            }

            //if 
            //Response.Write("<script>alert('CUIDADO! No puedes repetir la misma partición en más de una cuota. SELECCIONA UNA PARTICION QUE NO HAYAS USADO EN OTRA CUOTA');</script>");


            //int ultimopago = Convert.ToInt32(ultimopagox);            
            //int idproceso = 1;

            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection connection = new SqlConnection(cs);

            SqlCommand command = new SqlCommand("SP_PREVIEW_ESTRATEGIA_COBRANZA", connection);
            command.CommandTimeout = 1000;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CONVENIO", convenio);
            command.Parameters.AddWithValue("@PROXIMOPAGO", proximoPago);
            command.Parameters.AddWithValue("@DIASULTIMOPAGO", diasultimopago);
            command.Parameters.AddWithValue("@BANCOCLIENTE", bancoInicial);
            command.Parameters.AddWithValue("@ESTRATEGIA", estrategia);
            command.Parameters.AddWithValue("@ParticionCuotaActual", particionCuotaActual);
            command.Parameters.AddWithValue("@VencidosXCobrar", VencidosXCobrar);
            command.Parameters.AddWithValue("@CuotaVencida1", cuotaVencida1);
            command.Parameters.AddWithValue("@CuotaVencida2", cuotaVencida2);
            command.Parameters.AddWithValue("@CuotaVencida3", cuotaVencida3);
            command.Parameters.AddWithValue("@CuotaVencida4", cuotaVencida4);

            connection.Open();
            //command.ExecuteNonQuery();

            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt2 = new DataTable();
            da.Fill(dt2);

            dgvPreviewEstrategia.DataSource = dt2;
            dgvPreviewEstrategia.DataBind();


            connection.Close();


            //SqlDataAdapter da = new SqlDataAdapter(command);
            //DataTable dt = new DataTable();
            //da.Fill(dt);
            //dgvPreviewEstrategia.DataSource = dt;
            //dgvPreviewEstrategia.DataBind();
        }

        protected void btnPreviewEstrategia_Click(object sender, EventArgs e)
        {

            string metodoCobranza = cmbMetodoCobranza.Text;

            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);

            if (metodoCobranza=="") {
                return;
            } else if (metodoCobranza=="Consulta de Cartera")
            {            
            panelPreviewEstrategia.Visible = true;                
                btnConstruirArchivo.Visible = true;
                btnValidaEstrategia.Visible = true;
                ConsultaCartera();
                actualizaAlertasCobranza();
                return;
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
            btnConstruirArchivo.Visible = true;
            btnValidaEstrategia.Visible = true;
            actualizaAlertasCobranza();            
        }


        protected void actualizaAlertasCobranza()
        {
            ////ACTUALIZO ALERTA DE TOTALES
            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            int idReporte = 47;
            SqlCommand sp3 = new SqlCommand("SP_REPORTES", con);
            sp3.CommandType = CommandType.StoredProcedure;
            sp3.CommandTimeout = 900;
            sp3.Parameters.AddWithValue("@idReporte", idReporte);
            con.Open();
            SqlDataAdapter da3 = new SqlDataAdapter(sp3);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);

            string AlertaTotales = dt3.Rows[0][0].ToString();
            string AlertaTotalMonto = dt3.Rows[0][1].ToString(); ;
            string AlertaTotalBanorte = dt3.Rows[0][2].ToString(); ;
            string AlertaTotalBBVA = dt3.Rows[0][3].ToString(); ;
            string AlertaTotalSANTANDER = dt3.Rows[0][4].ToString(); ;
            string AlertaTotalBANAMEX = dt3.Rows[0][5].ToString(); ;
            string AlertaTotalOTROS = dt3.Rows[0][6].ToString(); ;

            lblAlertaTotales.Text = "Resumen -> Créditos Totales: " + AlertaTotales +
                " /Monto Total: " + AlertaTotalMonto;
            //" /Banorte: " + AlertaTotalBanorte +
            //" /BBVA: " + AlertaTotalBBVA +
            //" /Santander: " + AlertaTotalSANTANDER +
            //" /Banamex: " + AlertaTotalBANAMEX +
            //" /Otros: " + AlertaTotalOTROS;
        }

        protected void LimpiarCuotasVencidas()
        {
            txtCuotasVencidas.Items.Clear();
            txtCuotasVencidas.Items.Add("0");
            txtCuotasVencidas.Items.Add("1");
            txtCuotasVencidas.Items.Add("2");
            txtCuotasVencidas.Items.Add("3");
            txtCuotasVencidas.Items.Add("4");
        }

        protected void cmbEstrategia_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Estrategia = cmbEstrategia.Text;
            

            if (Estrategia=="Cuota Actual")
            {
                panelParticionCuotaActual.Visible = true;
                panelCuotasVencidas.Visible = false;
                ocultarCuotasVencidas();
                LimpiarCuotasVencidas();
            }
            else if (Estrategia== "Vencido + Cuota Actual" || Estrategia == "MultiCuotas Integrales")
            {

                panelCuotasVencidas.Visible = true;
                panelParticionCuotaActual.Visible = true;
                LimpiarCuotasVencidas();
            }
            else if(Estrategia == "Vencido")
            {
                panelParticionCuotaActual.Visible = false;
                panelCuotasVencidas.Visible = true;
                LimpiarCuotasVencidas();
            }else if(Estrategia== "Vencido Sin Particion")
            {                
                panelCuotasVencidas.Visible = true;
                LimpiarCuotasVencidas();
                txtCuotasVencidas.Items.Add("5");
                txtCuotasVencidas.Items.Add("6");
                txtCuotasVencidas.Items.Add("7");
                txtCuotasVencidas.Items.Add("8");
                txtCuotasVencidas.Items.Add("9");
                txtCuotasVencidas.Items.Add("10");
            }else
            {
                panelParticionCuotaActual.Visible = false;
                panelCuotasVencidas.Visible = false;
                ocultarCuotasVencidas();
                LimpiarCuotasVencidas();
            }

            if (Estrategia == "MultiCuotas Integrales")
            {
                panelParticionCuotaActual.Visible = false;
                panelCuotaVencida1.Visible = false;
                panelCuotaVencida2.Visible = false;
                panelCuotaVencida3.Visible = false;
                panelCuotaVencida4.Visible = false;
                Response.Write("<script>alert('CUIDADO! Se enviará la cantidad de veces seleccionada a Cobro por la Totalidad del Pago del Período cada una');</script>");
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
            string Estrategia = cmbEstrategia.Text;
                        
            ocultarCuotasVencidas();

            if (cuotas == "1" && Estrategia!= "Vencido Sin Particion")
            {
                panelCuotaVencida1.Visible = true;
            }else if (cuotas == "2" && Estrategia != "Vencido Sin Particion")
            {
                panelCuotaVencida1.Visible = true;
                panelCuotaVencida2.Visible = true;
            }else if (cuotas == "3" && Estrategia != "Vencido Sin Particion")
            {
                panelCuotaVencida1.Visible = true;
                panelCuotaVencida2.Visible = true;
                panelCuotaVencida3.Visible = true;
            }else if (cuotas == "4" && Estrategia != "Vencido Sin Particion")
            {
                panelCuotaVencida1.Visible = true;
                panelCuotaVencida2.Visible = true;
                panelCuotaVencida3.Visible = true;
                panelCuotaVencida4.Visible = true;
            }

            if (Estrategia == "Vencido Sin Particion")
            {
                panelParticionCuotaActual.Visible = false;
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
            //string emisora = cmbBancoEmisora.Text;
            //string HoraSantander = cmbHoraSantander.Text;

            //int RecibeParametros = 0;
            //string SP = null;
            //string Parametros = null;
            //string nombreParametros = null;

            //string Parametros2 = null;
            //string nombreParametros2 = null;

            //DataTable Emisoraidtable;
            //string Emisoraid;

            //string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            //SqlConnection con = new SqlConnection(cs);

            //if (banco == "Banamex")
            //{
            //    RecibeParametros = 0;
            //    SP = "SP_DOMI_BANAMEX";
            //}

            //if (banco == "Banorte")
            //{                
            //    SP = "SP_DOMI_BANORTE";
            //    RecibeParametros = 1;                
            //    Emisoraidtable = getEmisoraID(emisora, banco);
            //    Emisoraid= Emisoraidtable.Rows[0][0].ToString();
            //    Parametros = Emisoraid;
            //    nombreParametros = "@emisora";
            //}
            //if (banco == "BBVA Bancomer")
            //{
            //    SP = "SP_DOMI_BBVA_BANCOMER";
            //    RecibeParametros = 1;
            //    Emisoraidtable = getEmisoraID(emisora, banco);
            //    Emisoraid = Emisoraidtable.Rows[0][0].ToString();
            //    Parametros = Emisoraid;
            //    nombreParametros = "@emisora";
            //}if (banco == "Santander")
            //{
            //    SP = "SP_DOMI_SANTANDER";
            //    RecibeParametros = 2;                                
            //    Parametros = emisora;
            //    nombreParametros = "@TipoArchivo";
            //    Parametros2 = HoraSantander;
            //    nombreParametros2 = "@HoraEjecucion";
            //}


            //SqlCommand sp = new SqlCommand(SP, con);
            //sp.CommandType = CommandType.StoredProcedure;
            //sp.CommandTimeout = 900;
            
            //if (RecibeParametros == 1) {
            //    sp.Parameters.AddWithValue(nombreParametros, Parametros);
            //}
            //if (RecibeParametros == 2)
            //{
            //    sp.Parameters.AddWithValue(nombreParametros, Parametros);
            //    sp.Parameters.AddWithValue(nombreParametros2, Parametros2);
            //}
            
            //con.Open();
            //sp.ExecuteNonQuery();
            //con.Close();

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

          

            if (BancoRespuesta=="Santander" || BancoRespuesta == "Banamex" ||
                BancoRespuesta == "Banorte En Linea" || BancoRespuesta == "BBVA Bancomer" ||
                BancoRespuesta == "Santander Validaciones")
            {
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
            }

    
            if (BancoRespuesta == "Banorte Especializada" || BancoRespuesta == "BX+")
                {

                    char Delimiter='|';
                    if (BancoRespuesta == "BX+")
                    {
                        Delimiter = ',';
                    }

                using (FileStream stream = File.OpenRead(filePath))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = reader.ReadLine();
                        while (line != null)
                        {
                            string[] items = line.Split(Delimiter);
                            if (items.Length > dt.Columns.Count)
                            {
                                for (int i = dt.Columns.Count; i < items.Length; i++)
                                {
                                    dt.Columns.Add("Column " + i);
                                }
                            }
                            var newRow = dt.NewRow();
                            for (var j = 0; j < items.Length; j++)
                            {
                                newRow[j] = items[j];
                            }
                            dt.Rows.Add(newRow);
                            line = reader.ReadLine();
                        }
                    }
                }

                foreach (DataColumn column in dt.Columns)
                {
                    string cName = dt.Rows[0][column.ColumnName].ToString();
                    if (!dt.Columns.Contains(cName) && cName != "")
                    {
                        column.ColumnName = cName;
                    }

                }

                dt.Rows[0].Delete(); 
            }


            /////////
            ///
            //// insertar Datatable en la base de datos                        
            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            string StoredP_Nombre;

            if (BancoRespuesta == "Banorte Especializada")
            {
                StoredP_Nombre = "SP_RESPUESTAS_COBRANZA_BANORTE_ESPECIALIZADA";
            }else if (BancoRespuesta == "BX+")
                {
                    StoredP_Nombre = "SP_RESPUESTAS_COBRANZA_BXMas";
                }
                else
            {
                StoredP_Nombre = "SP_RESPUESTAS_COBRANZA";
            }

            SqlCommand sp = new SqlCommand(StoredP_Nombre, con);
            sp.CommandType = CommandType.StoredProcedure;
            sp.CommandTimeout = 900;
            sp.Parameters.AddWithValue("@tblRespuestasCobranza", dt);

            if (BancoRespuesta == "Banorte Especializada"){sp.Parameters.AddWithValue("@nombreArchivo", FileName); }
            if (BancoRespuesta == "BX+") { sp.Parameters.AddWithValue("@nombreArchivo", FileName); }
            if (BancoRespuesta == "Santander") { spNombre = "SP_RESPUESTAS_COBRANZA_SANTANDER"; }
            if (BancoRespuesta == "Banamex") { spNombre = "SP_RESPUESTAS_COBRANZA_BANAMEX"; }
            if (BancoRespuesta == "Banorte En Linea") { spNombre = "SP_RESPUESTAS_COBRANZA_BANORTE_ENLINEA"; }            
            if (BancoRespuesta == "BBVA Bancomer") { spNombre = "SP_RESPUESTAS_COBRANZA_BBVA"; }

            con.Open();
            sp.ExecuteNonQuery();

            if (BancoRespuesta != "Banorte Especializada" && BancoRespuesta != "BX+"
                    && BancoRespuesta != "Santander Validaciones") { 
                SqlCommand sp2 = new SqlCommand(spNombre, con);
                sp2.CommandType = CommandType.StoredProcedure;
                sp2.CommandTimeout = 900;
                sp2.Parameters.AddWithValue("@nombreArchivo", FileName);
                sp2.ExecuteNonQuery();
            }
                       
            con.Close();
                

                ///////ACTUALIZO VISTA PREVIA DE RESPUESTAS COBRANZA
                int idReporte;

                if (BancoRespuesta == "Santander Validaciones")
                {
                    idReporte = 18;
                }
                else
                {
                    idReporte = 24;
                }
                
            SqlCommand sp3 = new SqlCommand("SP_REPORTES", con);
            sp3.CommandType = CommandType.StoredProcedure;
            sp3.Parameters.AddWithValue("@idReporte", idReporte);
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
            string BancoRespuesta = cmbBancoRespuesta.Text;
            string query;

            if (BancoRespuesta=="Santander Validaciones")
            {
                query = "SELECT * FROM pasoDomiciliacion";
            }
            else
            {
              query = "SELECT A.FECHA,A.BANCO, A.EMISORA, A.ARCHIVO, A.CREDITO, A.MONTO, " +
              "A.RESPUESTA, B.Descripcion FROM pasoRespuestasCobranza AS A " +
              " LEFT JOIN catRespuestasCobranza AS B " +
              " ON A.Respuesta = B.CodigoFacturacion AND A.Banco = B.BANCO " +
              "ORDER BY Credito";
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

        protected void cmbBancoEmisora_SelectedIndexChanged(object sender, EventArgs e)
        {            
            string Banco = cmbBancoCobranza.Text;
            string Emisora = cmbBancoEmisora.Text;

            if (Banco == "Banorte" || Banco=="BBVA Bancomer")
            {
                actualizarLabelEmisora();
            }
        }

        protected void btnDescargaEjemploCobranzaLayout_Click(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlCommand sp = new SqlCommand("SP_REPORTES", con);
            sp.CommandType = CommandType.StoredProcedure;
            sp.CommandTimeout = 900;
            sp.Parameters.AddWithValue("@IdReporte", 29);

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
                Response.AddHeader("content-disposition", "attachment;filename=EjemploCobranza.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        protected void btnNuevoCobranza_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmCobranza.aspx");
        }

        protected void cmbMetodoCobro_SelectedIndexChanged(object sender, EventArgs e)
        {
            string archivoSalida = cmbArchivoSalida.Text;
            string MetodoCobro = cmbMetodoCobro.Text;

            if (MetodoCobro == "Cuenta" && archivoSalida == "Interbancario")
            {
                Response.Write("<script>alert('ADVERTENCIA: No se pueden cobrar Interbancarios por Cuenta');</script>");
                cmbMetodoCobro.Text = "CLABE";
                actualizarMetodoCobro();
                return;
            }

            actualizarMetodoCobro();
        }

        protected void cmbArchivoSalida_SelectedIndexChanged(object sender, EventArgs e)
        {
            string archivoSalida = cmbArchivoSalida.Text;
            string MetodoCobro = cmbMetodoCobro.Text;
            string TipoCobro = cmbTipoCobro.Text;
            string Banco = cmbBancoCobranza.Text;

            if (MetodoCobro == "Cuenta" && archivoSalida == "Interbancario")
            {
                Response.Write("<script>alert('ADVERTENCIA: No se pueden cobrar Interbancarios por Cuenta');</script>");
                cmbMetodoCobro.Text = "CLABE";
                actualizarMetodoCobro();
                return;
            }


            if (TipoCobro == "En Linea" && archivoSalida == "Interbancario")
            {
                Response.Write("<script>alert('ADVERTENCIA: No se pueden cobrar Interbancarios En Linea');</script>");
                if (Banco == "Banamex" || Banco == "BBVA Bancomer") { cmbTipoCobro.Text = "Interbancario"; }
                else { cmbTipoCobro.Text = "Tradicional"; }
                
                actualizarTipoCobro();
                return;
            }

            actualizaArchivoSalida();
            
        }

        protected void actualizarTipoCobro()
        {
            string Banco = cmbBancoCobranza.Text;
            string TipoCobro = cmbTipoCobro.Text;            

            if (TipoCobro == "En Linea")
            {
                lblTipoCobro.Text = "Sólo " + Banco ;
            }
            else if (TipoCobro == "Tradicional")
            {
                lblTipoCobro.Text = Banco + " y Otros";
            }else if (TipoCobro == "Banco a Banco")
            {
                lblTipoCobro.Text = "Sólo " + Banco  ;
            }
            else if (TipoCobro == "Interbancario")
            {
                lblTipoCobro.Text = Banco + " y Otros";
            }

        }

        protected void cmbTipoCobro_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Banco = cmbBancoCobranza.Text;
            string TipoCobro = cmbTipoCobro.Text;
            string archivoSalida = cmbArchivoSalida.Text;
            
            if (Banco == "Santander" && TipoCobro == "Banco a Banco")
            {
                panelHoraSantander.Visible = true;
            }
            else
            {
                panelHoraSantander.Visible = false;
            }


            if (TipoCobro == "En Linea" && archivoSalida == "Interbancario")
            {
                Response.Write("<script>alert('ADVERTENCIA: No se pueden cobrar Interbancarios En Linea');</script>");

                if (Banco == "Banamex" || Banco == "BBVA Bancomer") { cmbTipoCobro.Text = "Interbancario"; }
                else{ cmbTipoCobro.Text = "Tradicional"; }
                
                actualizarTipoCobro();
                return;
            }

            actualizarTipoCobro();

        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
                     
            List<string> listConvenios = new List<string>();
            foreach (int index in cmbConvenio.GetSelectedIndices())
            {
                listConvenios.Add(cmbConvenio.Items[index].Value);
            }
            var result = String.Join("| ", listConvenios.ToArray());

            Response.Write("<script>alert('"+ result + "');</script>");
        }

        protected void btnConstruirArchivo_Click(object sender, EventArgs e)
        {
            string Banco = cmbBancoCobranza.Text;
            string MetodoCobro = cmbMetodoCobro.Text;
            string ArchivoSalida = cmbArchivoSalida.Text;
            
            string Emisora = cmbBancoEmisora.Text;
            string TipoCobro = cmbTipoCobro.Text;
            string HoraEjecucion = cmbHoraSantander.Text;
            string Indomiciliable = cmbIndomiciliables.Text;

            string SPNombre = null;
            int DGV_idHeader = 0;
            int DGV_idBody = 0;
            int DGV_idFooter = 0;

            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlCommand sp = new SqlCommand("SP_DOMI_PRECONSTRUCTOR", con);
            sp.CommandType = CommandType.StoredProcedure;
            sp.CommandTimeout = 900;
            sp.Parameters.AddWithValue("@BANCO", Banco);
            sp.Parameters.AddWithValue("@METODOCOBRO", MetodoCobro);
            sp.Parameters.AddWithValue("@ARCHIVOSALIDA", ArchivoSalida);
            sp.Parameters.AddWithValue("@INDOMICILIABLE", Indomiciliable);

            if (Banco == "Banorte") {
                SPNombre = "SP_DOMI_BANORTE";
                DGV_idHeader = 36;
                DGV_idBody = 37;
            }
            if (Banco == "Banamex") {
                SPNombre = "SP_DOMI_BANAMEX";
                DGV_idHeader = 38;
                DGV_idBody = 39;
                DGV_idFooter = 40;
            }
            if (Banco == "BBVA Bancomer") {
                SPNombre = "SP_DOMI_BBVA_BANCOMER";
                DGV_idHeader = 41;
                DGV_idBody = 42;
                DGV_idFooter = 43;
            }
            if (Banco == "Santander") {
                SPNombre = "SP_DOMI_SANTANDER";
                DGV_idHeader = 44;
                DGV_idBody = 45;
                DGV_idFooter = 46;
            }

            SqlCommand sp2 = new SqlCommand(SPNombre, con);
            sp2.CommandType = CommandType.StoredProcedure;
            sp2.CommandTimeout = 900;
            sp2.Parameters.AddWithValue("@Emisora", Emisora);
            sp2.Parameters.AddWithValue("@TIPOCOBRO", TipoCobro);
            sp2.Parameters.AddWithValue("@METODOCOBRO", MetodoCobro);
            sp2.Parameters.AddWithValue("@ARCHIVOSALIDA", ArchivoSalida);
            sp2.Parameters.AddWithValue("@HORAEJECUCION", HoraEjecucion);

            con.Open();
            sp.ExecuteNonQuery();
            sp2.ExecuteNonQuery();
            con.Close();


            ///
            DataTable DGVPreview = new DataTable();
            DGVPreview = principalClass.SP_Reportes(DGV_idHeader);
            dgvPreviewEstrategia.DataSource = DGVPreview;
            dgvPreviewEstrategia.DataBind();

            DGVPreview = principalClass.SP_Reportes(DGV_idBody);
            dgvBodyCobranza.DataSource = DGVPreview;
            dgvBodyCobranza.DataBind();

            if (Banco != "Banorte") { 
            DGVPreview = principalClass.SP_Reportes(DGV_idFooter);
            dgvFooterCobranza.DataSource = DGVPreview;
            dgvFooterCobranza.DataBind();
            }

            btnValidacionExcel.Visible = true;
            btnGenerarCobranza.Visible = true;


            string QueryFileName = "SELECT ARCHIVO FROM optConsecutivos WHERE BANCO = '" + Banco + "' " +
            "AND FECHA = CONVERT(VARCHAR, GETDATE(), 126) AND " +
            "Consecutivo = dbo.fn_getConsecutivoDomi('" + Banco + "', CONVERT(VARCHAR, GETDATE(), 126))";
            DataTable TableFileName = principalClass.Read(QueryFileName);
            string FileName = TableFileName.Rows[0][0].ToString();

            lblFILENAME.Text = "El nombre del Archivo será: "+FileName;
        }

        protected void btnValidacionExcel_Click(object sender, EventArgs e)
        {
            string Banco = cmbBancoCobranza.Text;
            string QueryFileName = "SELECT ARCHIVO FROM optConsecutivos WHERE BANCO = '" + Banco + "' " +
               "AND FECHA = CONVERT(VARCHAR, GETDATE(), 126) AND " +
               "Consecutivo = dbo.fn_getConsecutivoDomi('" + Banco + "', CONVERT(VARCHAR, GETDATE(), 126))";
            DataTable TableFileName = principalClass.Read(QueryFileName);
            string FileName = TableFileName.Rows[0][0].ToString();

            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;

            string query = null;
            
            if (Banco == "Banorte")
            {
                query = "SELECT * FROM pasoBanorteHeader;" +
                "SELECT * FROM pasoBanorteBody;"+
                "SELECT NULL FROM pasoBanorteHeader";
            }else if (Banco=="Banamex")
            {
                query = "SELECT * FROM pasoBanamexHeader;" +
                "SELECT * FROM pasoBanamexBody;" +
                "SELECT * FROM pasoBanamexFooter";
            }else if (Banco == "BBVA Bancomer")
            {
                query = "SELECT * FROM pasoBBVAHeader;" +
                "SELECT * FROM pasoBBVABody;" +
                "SELECT * FROM pasoBBVAFooter";
            }else if (Banco == "Santander")
            {
                query = "SELECT * FROM pasoSantanderHeader;" +
                "SELECT * FROM pasoSantanderBody;" +
                "SELECT * FROM pasoSantanderFooter";
            }



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
                            ds.Tables[2].TableName = "Footer";
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
                                Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");
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

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void btnValidaEstrategia_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";

            String Filename = "Descarga" + DateTime.Now + ".xls";

            StringWriter swriter = new StringWriter();
            HtmlTextWriter hTextWriter = new HtmlTextWriter(swriter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";

            Response.AddHeader("Content-Disposition", "attachment; filename=" + Filename);

            dgvPreviewEstrategia.RenderControl(hTextWriter);
            Response.Write(swriter.ToString());
            Response.End();
        }
    }
}