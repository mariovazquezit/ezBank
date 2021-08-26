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
            int id = 0;

            if (Ejemplo == "Cartera") { id = 12; }
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

            if (tipoArchivo == "Cartera")
            {
                nombreArchivo = "SaldosCartera.xlsx";
                nombreSP = "SP_MANUAL_SALDOSCARTERA";
                nombretablaSP = "@tblSaldosCartera";
            }
            else if (tipoArchivo == "CLABES")
            {
                nombreArchivo = "CLABES.xlsx";
                nombreSP = "SP_MANUAL_importaCLABEs";
                nombretablaSP = "@tblCLABES";
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

                    con.Open();
                    sp.ExecuteNonQuery();
                    con.Close();
                   
                    Response.Write("<script>alert('Información cargada exitosamente');</script>");

               
            }
        }
    }
}