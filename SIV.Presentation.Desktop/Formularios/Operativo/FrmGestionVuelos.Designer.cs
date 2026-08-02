namespace SIV.Presentation.Desktop.Formularios.Operativo
{
    partial class FrmGestionVuelos
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.FlowLayoutPanel pnlAcciones;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.ComboBox cmbEstadoFiltro;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Button btnCrearVuelo;
        private System.Windows.Forms.Button btnCambiosOperativos;
        private System.Windows.Forms.Button btnHistorial;
        private System.Windows.Forms.DataGridView dgvVuelos;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlAcciones = new System.Windows.Forms.FlowLayoutPanel();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.cmbEstadoFiltro = new System.Windows.Forms.ComboBox();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.btnCrearVuelo = new System.Windows.Forms.Button();
            this.btnCambiosOperativos = new System.Windows.Forms.Button();
            this.btnHistorial = new System.Windows.Forms.Button();
            this.dgvVuelos = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVuelos)).BeginInit();
            this.pnlAcciones.SuspendLayout();
            this.SuspendLayout();

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Text = "Gestion de Vuelos";

            this.pnlAcciones.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.pnlAcciones.AutoSize = false;
            this.pnlAcciones.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.pnlAcciones.Location = new System.Drawing.Point(20, 55);
            this.pnlAcciones.Size = new System.Drawing.Size(984, 35);
            this.pnlAcciones.WrapContents = false;

            this.txtBuscar.Location = new System.Drawing.Point(3, 3);
            this.txtBuscar.Size = new System.Drawing.Size(200, 25);

            this.cmbEstadoFiltro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstadoFiltro.Location = new System.Drawing.Point(209, 3);
            this.cmbEstadoFiltro.Size = new System.Drawing.Size(150, 25);

            this.btnFiltrar.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnFiltrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFiltrar.ForeColor = System.Drawing.Color.White;
            this.btnFiltrar.Location = new System.Drawing.Point(365, 3);
            this.btnFiltrar.Size = new System.Drawing.Size(90, 30);
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.Click += new System.EventHandler(this.BtnFiltrar_Click);

            this.btnCrearVuelo.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnCrearVuelo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCrearVuelo.ForeColor = System.Drawing.Color.White;
            this.btnCrearVuelo.Location = new System.Drawing.Point(461, 3);
            this.btnCrearVuelo.Size = new System.Drawing.Size(110, 30);
            this.btnCrearVuelo.Text = "Crear Vuelo";
            this.btnCrearVuelo.Click += new System.EventHandler(this.BtnCrearVuelo_Click);

            this.btnCambiosOperativos.BackColor = System.Drawing.Color.Orange;
            this.btnCambiosOperativos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCambiosOperativos.ForeColor = System.Drawing.Color.White;
            this.btnCambiosOperativos.Location = new System.Drawing.Point(577, 3);
            this.btnCambiosOperativos.Size = new System.Drawing.Size(140, 30);
            this.btnCambiosOperativos.Text = "Cambios Operativos";
            this.btnCambiosOperativos.Click += new System.EventHandler(this.BtnCambiosOperativos_Click);

            this.btnHistorial.BackColor = System.Drawing.Color.Gray;
            this.btnHistorial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistorial.ForeColor = System.Drawing.Color.White;
            this.btnHistorial.Location = new System.Drawing.Point(723, 3);
            this.btnHistorial.Size = new System.Drawing.Size(90, 30);
            this.btnHistorial.Text = "Historial";
            this.btnHistorial.Click += new System.EventHandler(this.BtnHistorial_Click);

            this.pnlAcciones.Controls.Add(this.txtBuscar);
            this.pnlAcciones.Controls.Add(this.cmbEstadoFiltro);
            this.pnlAcciones.Controls.Add(this.btnFiltrar);
            this.pnlAcciones.Controls.Add(this.btnCrearVuelo);
            this.pnlAcciones.Controls.Add(this.btnCambiosOperativos);
            this.pnlAcciones.Controls.Add(this.btnHistorial);

            this.dgvVuelos.AllowUserToAddRows = false;
            this.dgvVuelos.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvVuelos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVuelos.BackgroundColor = System.Drawing.Color.White;
            this.dgvVuelos.Location = new System.Drawing.Point(20, 100);
            this.dgvVuelos.MultiSelect = false;
            this.dgvVuelos.ReadOnly = true;
            this.dgvVuelos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVuelos.Size = new System.Drawing.Size(984, 480);

            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1024, 600);
            this.MinimumSize = new System.Drawing.Size(1024, 600);
            this.Controls.Add(this.dgvVuelos);
            this.Controls.Add(this.pnlAcciones);
            this.Controls.Add(this.lblTitle);
            this.Text = "Gestion de Vuelos";
            ((System.ComponentModel.ISupportInitialize)(this.dgvVuelos)).EndInit();
            this.pnlAcciones.ResumeLayout(false);
            this.pnlAcciones.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
