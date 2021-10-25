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
using ClosedXML.Excel;
using System.IO;

namespace ezBank.Views
{
    public partial class frmCargas : System.Web.UI.Page
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

            }
        }

        protected void btnDescargaEjemplo_Click(object sender, EventArgs e)
        {
            string Ejemplo = cmbTipoArchivo.Text;
            string ERP = cmbERPCartera.Text;
            int id = 0;

            if (Ejemplo == "Cartera" && ERP=="Zell") { id = 12; }
            if (Ejemplo == "Cartera" && ERP == "Cronos") { id = 28; }
            if (Ejemplo == "Cartera" && ERP == "CIB") { id = 28; }
            if (Ejemplo == "Cartera" && ERP == "Layout Basico") { id = 48; }
            if (Ejemplo == "CLABES") { id = 19; }
            if (Ejemplo == "Excepciones de Pago") { id = 20; }

            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlCommand sp = new SqlCommand("SP_REPORTES", con);
            sp.CommandType = CommandType.StoredProcedure;
            sp.CommandTimeout = 900;
            sp.Parameters.AddWithValue("@IdReporte", id);

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
                Response.AddHeader("content-disposition", "attachment;filename=" + Ejemplo + ".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }

        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            if (fileupload_archivo.HasFile == false)
            {
                Response.Write("<script>alert('Error: No has seleccionado ningun archivo para cargar');</script>");
                return;
            }

            string tipoArchivo = cmbTipoArchivo.Text;
            string nombreArchivo = null;
            string nombreSP = null;
            string nombretablaSP = null;
            string ERPCartera = cmbERPCartera.Text;

            if (tipoArchivo == "Cartera" && ERPCartera=="Zell")
            {
                nombreArchivo = "SaldosCartera.xlsx";
                nombreSP = "SP_MANUAL_SALDOSCARTERA_ZELL";
                nombretablaSP = "@tblSaldosCartera";
            }
            else if (tipoArchivo == "Cartera" && ERPCartera == "Layout Basico")
            {
                nombreArchivo = "SaldosCartera.xlsx";
                nombreSP = "SP_MANUAL_SALDOSCARTERA_BASICO";
                nombretablaSP = "@tblSaldosCartera";
            }
            else if (tipoArchivo == "Cartera" && ERPCartera == "CIB")
            {
                nombreArchivo = "SaldosCartera.xlsx";
                nombreSP = "SP_MANUAL_SALDOSCARTERA_CIB";
                nombretablaSP = "@tblSaldosCartera";
            }else if (tipoArchivo == "CLABES")
            {
                nombreArchivo = "CLABES.xlsx";
                nombreSP = "SP_MANUAL_importaCLABEs";
                nombretablaSP = "@tblCLABES";
            }
            else if (tipoArchivo == "Excepciones de Pago")
            {
                nombreArchivo = "ExcepcionesPago.xlsx";
                nombreSP = "SP_MANUAL_importaBLACKLIST";
                nombretablaSP = "@tblBlackList";
            }

            //Save the uploaded Excel file.
            string filePath = Server.MapPath("~/CSV_Files/") + nombreArchivo;
            fileupload_archivo.SaveAs(filePath);

            //Open the Excel file using ClosedXML.
            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                //Read the first Sheet from Excel file.
                IXLWorksheet workSheet = workBook.Worksheet(1);

                //Create a new DataTable.
                DataTable dt = new DataTable();

                //Add columns to Datatable                
                foreach (IXLRow row in workSheet.Rows())
                {
                       foreach (IXLCell cell in row.Cells())
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                    break;
                }
                //Add rows to DataTable.                                         
                        int TotalColumns = dt.Columns.Count;
                        int TotalRows = workSheet.Rows().Count();
                        int CurrentRow = 2;
                        int CurrentColumn = 1;


                        for (int j=1; j<=(TotalRows-1); j++)
                        {
                            dt.Rows.Add();
                            for (int i = 0; i <TotalColumns; i++)
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = workSheet.Cell(CurrentRow, CurrentColumn).GetFormattedString();
                                CurrentColumn = CurrentColumn + 1;
                            }
                            CurrentRow = CurrentRow + 1;
                            CurrentColumn = 1;
                            
                        }                                       
                //// insertar Datatable en la base de datos                        
                string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
                    SqlConnection con = new SqlConnection(cs);
                    SqlCommand sp = new SqlCommand(nombreSP, con);
                    sp.CommandType = CommandType.StoredProcedure;
                    sp.CommandTimeout = 900;
                    sp.Parameters.AddWithValue(nombretablaSP, dt);

                try { 
                    con.Open();
                    sp.ExecuteNonQuery();
                    con.Close();
                    Response.Write("<script>alert('Información cargada exitosamente');</script>");
                }
                catch
                {
                    Response.Write("<script>alert('ALERTA: No se cargó la información en el Sistema porque el Archivo seleccionado tiene un error');</script>");
                }
                    

               
            }
        }

        protected void cmbTipoArchivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string TipoArchivo = cmbTipoArchivo.Text;

            if (TipoArchivo == "Cartera")
            {
                panelERP.Visible = true;
            }
            else
            {
                panelERP.Visible = false;
            }
        }
    }
}