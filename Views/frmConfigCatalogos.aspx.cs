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
    public partial class frmConfigCatalogos : System.Web.UI.Page
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
                btnExcel.Visible = false;
            }
            else
            {
                ///Codigo que se ejecutara cuando ocurra el PostBack
            }

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }


        protected void btnCatBancos_Click(object sender, EventArgs e)
        {
            String Consulta = "SELECT idBanco, Banco FROM catBancosDomiciliacion";
            actualizaGridCatalogo(Consulta);            
        }


        protected void actualizaGridCatalogo(string Consultax)
        {
            dgvCatalogos.DataSource = principalClass.Read(Consultax);
            dgvCatalogos.DataBind();
            btnExcel.Visible = true;
        }

        protected void btnCatOperacionesBBVA_Click(object sender, EventArgs e)
        {
            String Consulta = "SELECT ID, IDBBVA, TIPOID, DESCRIPCION FROM catBBVAOperaciones";
            actualizaGridCatalogo(Consulta);            
        }

        protected void btnCatConsecutivos_Click(object sender, EventArgs e)
        {
            String Consulta = "SELECT * FROM catConsecutivoLetras";
            actualizaGridCatalogo(Consulta);            
        }

        protected void btnCatRespuestasCobranza_Click(object sender, EventArgs e)
        {
            String Consulta = "SELECT Banco, CodigoFacturacion, Descripcion FROM catRespuestasCobranza";
            actualizaGridCatalogo(Consulta);
        }

   

        protected void btnExcel_Click(object sender, EventArgs e)
        {

            string query = "SELECT idBanco, Banco FROM catBancosDomiciliacion;" +
                "SELECT Banco, CodigoFacturacion, Descripcion FROM catRespuestasCobranza;"+
                "SELECT ID, IDBBVA, TIPOID, DESCRIPCION FROM catBBVAOperaciones;" +
                 "SELECT * FROM catConsecutivoLetras";
                            

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
                            ds.Tables[0].TableName = "Bancos";
                            ds.Tables[1].TableName = "RespuestasCobranza";
                            ds.Tables[2].TableName = "Operaciones_BBVA";
                            ds.Tables[3].TableName = "Consecutivos_Letras";

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
                                Response.AddHeader("content-disposition", "attachment;filename=Catalogos.xlsx");
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

        
