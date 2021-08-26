using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ezBank.Classes;

namespace ezBank.Views
{
    public partial class frmExpediente : System.Web.UI.Page
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

        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            try { 
            string tipoBusqueda = cmbBuscarCliente.Text;
            string DatoBusqueda = txtBuscar.Text.Trim();
            string FiltroConsulta = null;
            string QueryFiltroConsulta;

            if (tipoBusqueda == "Credito")
            {
                FiltroConsulta = " idcredito ";
            }
            else if (tipoBusqueda == "Cliente Unico")
            {
                FiltroConsulta = " idPersona ";

            }
            else if (tipoBusqueda == "Nombre")
            {
                FiltroConsulta = " Nombre ";
            }

            QueryFiltroConsulta = " WHERE " + FiltroConsulta + " = '" + DatoBusqueda + "' ";


            DataTable Expediente = new DataTable();
            Expediente = principalClass.SP_Consulta(1, QueryFiltroConsulta);

            string idSolicitud = Expediente.Rows[0][0].ToString();
            string idCredito = Expediente.Rows[0][1].ToString();
            string idPersona = Expediente.Rows[0][2].ToString();
            string Nombre = Expediente.Rows[0][3].ToString();
            string SuperConvenio = Expediente.Rows[0][4].ToString();
            string Dependencia = Expediente.Rows[0][5].ToString();
            string CLABE = Expediente.Rows[0][6].ToString();
            string Banco = Expediente.Rows[0][7].ToString();
            string Sucursal = Expediente.Rows[0][8].ToString();
            string Producto = Expediente.Rows[0][9].ToString();
            string TipoFinanciamiento = Expediente.Rows[0][10].ToString();
            string Frecuencia = Expediente.Rows[0][11].ToString();
            string Monto = Expediente.Rows[0][12].ToString();

            string Pagos = Expediente.Rows[0][13].ToString();
            string ImportePago = Expediente.Rows[0][14].ToString();
            string FechaDesembolso = Expediente.Rows[0][15].ToString();
            string AmortPagadas = Expediente.Rows[0][16].ToString();
            string Avance = Expediente.Rows[0][17].ToString();
            string Estatus = Expediente.Rows[0][18].ToString();
            string PrimerPagoTeorico = Expediente.Rows[0][19].ToString();
            string UltimoPagoTeorico = Expediente.Rows[0][20].ToString();
            string DiasAtraso = Expediente.Rows[0][21].ToString();
            string Vencido = Expediente.Rows[0][22].ToString();
            string UltimoPagoAplicado = Expediente.Rows[0][23].ToString();
            string MontoUltimoPago = Expediente.Rows[0][24].ToString();
            string DiasUltimoPago = Expediente.Rows[0][25].ToString();
            string SigPago = Expediente.Rows[0][26].ToString();


            txtSolicitud.Text = idSolicitud;
            txtCredito.Text = idCredito;
            txtClienteUnico.Text = idPersona;
            txtNombre.Text = Nombre;
            txtDependencia.Text = SuperConvenio;
            txtSubDependencia.Text = Dependencia;
            txtCLABE.Text = CLABE;
            txtBanco.Text = Banco;
            txtSucursal.Text = Sucursal;
            txtProducto.Text = Producto;
            txtSubProducto.Text = TipoFinanciamiento;
            txtFrecuencia.Text = Frecuencia;
            txtOtorgado.Text = Monto;
            txtAmortTotales.Text = Pagos;
            txtCuotaxAmort.Text = ImportePago;
            txtDesembolso.Text = FechaDesembolso;
            txtAmortPagadas.Text = AmortPagadas;
            txtAvance.Text = Avance;
            txtEstatus.Text = Estatus;
            txtPrimerPagoTeorico.Text = PrimerPagoTeorico;
            txtUltimoPagoTeorico.Text = UltimoPagoTeorico;
            txtDiasAtraso.Text = DiasAtraso;
            txtSaldoVencido.Text = Vencido;
            txtUltimoPagoAplicado.Text = UltimoPagoAplicado;
            txtMontoUltimoPago.Text = MontoUltimoPago;
            txtDiasUltimoPago.Text = DiasUltimoPago;
            txtSiguientePago.Text = SigPago;
               
                ActualizaExpediente(idCredito);

                panelExpedientebuttons.Visible = true;
                panelExpediente.Visible = true;
            }
            catch
            {
                Response.Write("<script>alert('Cliente no localizado');</script>");

            }
        }

        protected void ActualizaExpediente(string idCredito)
        {
            DataTable CLABEadicional = new DataTable();
            DataTable Afiliacion = new DataTable();
            DataTable ListaExclusion = new DataTable();
            DataTable Bitacora = new DataTable();
            DataTable Multicreditos = new DataTable();
            DataTable AfiliacionRespuestas = new DataTable();
            DataTable LogDomiciliacion = new DataTable();
            DataTable LogPagos = new DataTable();

            CLABEadicional = principalClass.SP_Consulta(2, idCredito);
            Afiliacion = principalClass.SP_Consulta(3, idCredito);
            ListaExclusion = principalClass.SP_Consulta(4, idCredito);
            Bitacora = principalClass.SP_Consulta(5, idCredito);
            Multicreditos = principalClass.SP_Consulta(7, idCredito);
            AfiliacionRespuestas = principalClass.SP_Consulta(8, idCredito);
            LogDomiciliacion = principalClass.SP_Consulta(6, idCredito);
            LogPagos = principalClass.SP_Consulta(9, idCredito);


            dgvCLABE.DataSource = CLABEadicional;
            dgvCLABE.DataBind();

            dgvAfiliaciones.DataSource = Afiliacion;
            dgvAfiliaciones.DataBind();

            dgvExclusiones.DataSource = ListaExclusion;
            dgvExclusiones.DataBind();

            dgvBitacora.DataSource = Bitacora;
            dgvBitacora.DataBind();

            dgvMulticreditos.DataSource = Multicreditos;
            dgvMulticreditos.DataBind();

            dgvAfiliacionRespuestas.DataSource = AfiliacionRespuestas;
            dgvAfiliacionRespuestas.DataBind();

            dgvLogDomiciliacion.DataSource = LogDomiciliacion;
            dgvLogDomiciliacion.DataBind();

            dgvLogPagos.DataSource = LogPagos;
            dgvLogPagos.DataBind();

        }

        protected void btnListaNegra_Click(object sender, EventArgs e)
        {                        
            ocultarPaneles();
            panelListaNegra.Visible = true;
        }

        protected void btnCLABEAdicional_Click(object sender, EventArgs e)
        {
            ocultarPaneles();
            panelCLABEAdicional.Visible = true;
        }

        protected void btnBitacora_Click(object sender, EventArgs e)
        {
            ocultarPaneles();
            panelBitacora.Visible = true;
        }

        protected void ocultarPaneles()
        {
            panelListaNegra.Visible = false;
            panelBitacora.Visible = false;
            panelCLABEAdicional.Visible = false;
            panelLogPagos.Visible = false;
            panelExpediente.Visible = false;
            panelLogDomiciliacion.Visible = false;
            panelAfiliacion.Visible = false;
            
        }

        protected void limpiarformularios()
        {
            txtCLABEAdicional.Text = "";
            txtCLABEComentarios.Text = "";
            //txtBlackList.Text = "";
           // txtBitacoraAccion.Text = "";
            txtBitacoraComentarios.Text = "";
        }


        protected void btnCLABEAdicionalGUARDAR_Click(object sender, EventArgs e)
        {
            string Credito = txtCredito.Text;
            string CLABE = txtCLABEAdicional.Text;
            string Comentarios = txtCLABEComentarios.Text;

            if (CLABE == "" || Comentarios == "" || CLABE == null || Comentarios == null)
            {
                Response.Write("<script>alert('Capture todos los datos. No puede haber campos vacíos');</script>");
                return;
            }

            if (CLABE.Length < 18)
            {
                Response.Write("<script>alert('No puede registrar una CLABE con menos de 18 dígitos');</script>");
                return;
            }

            

            string Accion = "CLABE adicional";
            string Comentariosx = "Se agrega nueva CLABE " + CLABE;
            BitacoraGUARDAR(Accion, Comentariosx, Credito);

            string Query = "INSERT INTO optCLABEs (Credito, CLABE, Comentarios) VALUES('" + Credito + "','" + CLABE + "','" + Comentarios + "')";
            GuardarDatos(Query, Credito);

            

        }




        protected void btnBlackListGUARDAR_Click(object sender, EventArgs e)
        {
            string Credito = txtCredito.Text;

            try
            {
                DateTime datex = DateTime.ParseExact(txtBlackList.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            string BlackList = datex.ToString("yyyy-MM-dd");

                string Accion = "Lista Exclusion";
                string Comentarios = "Se Genera exclusion hasta el dia " + BlackList;
                BitacoraGUARDAR(Accion, Comentarios, Credito);

                string Query = "INSERT INTO optBlackList (Credito, ExcepcionHasta) VALUES('" + Credito + "','" + BlackList + "')";
                GuardarDatos(Query, Credito);
            }
            catch
            {
                Response.Write("<script>alert('Seleccione una Fecha de Exclusion');</script>");

            }
        }

        protected void btnBitacoraGUARDAR_Click(object sender, EventArgs e)
        {
            string Accion = txtBitacoraAccion.Text;
            string Comentarios = txtBitacoraComentarios.Text;
            string Credito = txtCredito.Text;

            if (Comentarios=="" || Comentarios== null)
            {
                Response.Write("<script>alert('Capture todos los datos. No puede haber campos vacíos');</script>");
                return;
            }

            BitacoraGUARDAR(Accion, Comentarios, Credito);
        }        

        protected void BitacoraGUARDAR(string Accion, string Comentarios, string Credito)
        {
            DateTime datex = DateTime.Now;
            string Fecha = datex.ToString("yyyy-MM-dd hh:mm:ss");
            string Usuario = Session["UsuarioGlobal"].ToString();

            string Query = "INSERT INTO optBitacora (Fecha, Usuario, Accion, Comentario, Credito) " +
             "VALUES('" + Fecha + "','" + Usuario + "','" + Accion + "','" + Comentarios + "','" + Credito + "')";
            GuardarDatos(Query, Credito);
        }

        protected void GuardarDatos(string Query, string Credito)
        {
            int rowsaffected = principalClass.CUD(Query);
            string Alerta = principalClass.alertasCUD(rowsaffected);
            Response.Write(Alerta);
            limpiarformularios();
            ocultarPaneles();
            ActualizaExpediente(Credito);
        }

        protected void btnPanelExpediente_Click(object sender, EventArgs e)
        {
            ocultarPaneles();
            ocultarPanelesPrincipales();
            panelExpediente.Visible = true;
        }

        protected void ocultarPanelesPrincipales()
        {
            panelExpediente.Visible = false;
            panelLogDomiciliacion.Visible = false;
            panelLogPagos.Visible = false;
        }

        protected void btnPanelCobranza_Click(object sender, EventArgs e)
        {
            ocultarPaneles();
            ocultarPanelesPrincipales();
            panelLogDomiciliacion.Visible = true;
        }

        protected void btnPanelPagos_Click(object sender, EventArgs e)
        {
            ocultarPaneles();
            ocultarPanelesPrincipales();
            panelLogPagos.Visible = true;
        }

        protected void btnPanelAfiliacion_Click(object sender, EventArgs e)
        {
            ocultarPaneles();
            ocultarPanelesPrincipales();
            panelAfiliacion.Visible = true;
        }
    }
}