using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using ezBank.Classes;
using System.IO;

namespace ezBank.Views
{
    public partial class frmCalendarioCobro : System.Web.UI.Page
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
                mostrarCalendarioPagos();
                limpiarFormularios();
            }
            else
            {
            }
            

        }


        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        protected void mostrarCalendarioPagos()
        {            
            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlCommand sp = new SqlCommand("SP_REPORTES", con);
            sp.CommandType = CommandType.StoredProcedure;
            sp.CommandTimeout = 900;
            sp.Parameters.AddWithValue("@IdReporte", 4);

            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(sp);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();


            dgv_CalendarioPagos.DataSource = dt;
            dgv_CalendarioPagos.DataBind();
           
        }

        protected void limpiarFormularios()
        {
            btnGuardar.Visible = true;
            btnEditar.Visible = false;
            btnEliminar.Visible = false;

            txtConcepto.Text = "";
            dtp_FechaPago.Text = DateTime.Now.ToString();
            txtComentarios.Text = "";
            cmbEstatus.Text = "Pendiente";
        }

        protected void ocultarAlertas()
        {
            panel_AlertaGuardar.Visible = false;
            panel_AlertaError.Visible = false;
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            ocultarAlertas();
            limpiarFormularios();
            panelNuevoEvento.Visible = true;
        }

        

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string Usuario = Session["UsuarioGlobal"].ToString();
            string Concepto = txtConcepto.Text;
            string Comentarios = txtComentarios.Text;
            string Estatus = cmbEstatus.Text;
            DateTime FechaPagoX = DateTime.ParseExact(dtp_FechaPago.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            string FechaPago = FechaPagoX.ToString("yyyy-MM-dd");

            string Query= "INSERT INTO OPTCALENDARIOPAGOS(FECHACREACION, USUARIO, CONCEPTO,FECHAPAGO, COMENTARIO, ESTATUS) VALUES(CONVERT(VARCHAR(10),GETDATE(),126), '" + Usuario + "','" + Concepto + "','" + FechaPago + "','" + Comentarios + "','" + Estatus + "')";
            
            int rowsAffected = principalClass.CUD(Query);

            if (rowsAffected == 1)
            {
                panel_AlertaGuardar.Visible = true;
            }
            else
            {
                panel_AlertaError.Visible = true;
            }
            
            mostrarCalendarioPagos();
            limpiarFormularios();

        }

        protected void dgv_CalendarioPagos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ocultarAlertas();
            panelNuevoEvento.Visible = true;

            txtId.Text = dgv_CalendarioPagos.SelectedRow.Cells[1].Text;
            txtConcepto.Text = dgv_CalendarioPagos.SelectedRow.Cells[4].Text;
            dtp_FechaPago.Text = dgv_CalendarioPagos.SelectedRow.Cells[5].Text;
            txtComentarios.Text = dgv_CalendarioPagos.SelectedRow.Cells[6].Text;
            cmbEstatus.Text= dgv_CalendarioPagos.SelectedRow.Cells[7].Text;

            btnGuardar.Visible = false;
            btnEditar.Visible = true;
            btnEliminar.Visible = true;
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            string id = txtId.Text;
            string Concepto = txtConcepto.Text;
            string Comentarios = txtComentarios.Text;
            string Estatus = cmbEstatus.Text;
            DateTime FechaPagoX = DateTime.ParseExact(dtp_FechaPago.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            string FechaPago = FechaPagoX.ToString("yyyy-MM-dd");

            string Query = "UPDATE OPTCALENDARIOPAGOS SET CONCEPTO='" + Concepto + "', FECHAPAGO='" + FechaPago + "', COMENTARIO='" + Comentarios + "',ESTATUS='" + Estatus + "' WHERE ID='" + id + "'";
            int rowsAffected = principalClass.CUD(Query);

            if (rowsAffected == 1)
            {
                panel_AlertaGuardar.Visible = true;
            }
            else
            {
                panel_AlertaError.Visible = true;
            }

            mostrarCalendarioPagos();
            limpiarFormularios();
        }


      


        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            string id = txtId.Text;
            string Query = "DELETE FROM OPTCALENDARIOPAGOS WHERE ID='" + id + "'";
            int rowsAffected = principalClass.CUD(Query);

            if (rowsAffected == 1)
            {
                panel_AlertaGuardar.Visible = true;
            }
            else
            {
                panel_AlertaError.Visible = true;
            }

            mostrarCalendarioPagos();
            limpiarFormularios();
        }

        protected void dgv_CalendarioPagos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {                
                if (e.Row.Cells[8].Text == "Vencido")
                {
                    e.Row.CssClass = "table-danger";
                }                    
                else if (e.Row.Cells[7].Text == "Liquidado")
                {
                    e.Row.CssClass = "table-primary";
                }
            }
        }


   

        protected void btnDownExcel_Click(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["miConexion"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlCommand sp = new SqlCommand("SP_REPORTES", con);
            sp.CommandType = CommandType.StoredProcedure;
            sp.CommandTimeout = 900;
            sp.Parameters.AddWithValue("@IdReporte", 4);

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
    }
}