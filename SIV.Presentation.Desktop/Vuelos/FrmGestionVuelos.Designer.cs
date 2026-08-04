using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Vuelos
{
    partial class FrmGestionVuelos
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.FlowLayoutPanel pnlAcciones;
        private System.Windows.Forms.Label lblBuscar;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.ComboBox cmbEstadoFiltro;
        private System.Windows.Forms.Label lblAerolinea;
        private System.Windows.Forms.ComboBox cmbAerolinea;
        private System.Windows.Forms.Label lblOrigen;
        private System.Windows.Forms.ComboBox cmbOrigen;
        private System.Windows.Forms.Label lblDestino;
        private System.Windows.Forms.ComboBox cmbDestino;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Button btnCrearVuelo;
        private System.Windows.Forms.Button btnCambiosOperativos;
        private System.Windows.Forms.Button btnAsignarPuerta;
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
            this.lblBuscar = new System.Windows.Forms.Label();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.lblEstado = new System.Windows.Forms.Label();
            this.cmbEstadoFiltro = new System.Windows.Forms.ComboBox();
            this.lblAerolinea = new System.Windows.Forms.Label();
            this.cmbAerolinea = new System.Windows.Forms.ComboBox();
            this.lblOrigen = new System.Windows.Forms.Label();
            this.cmbOrigen = new System.Windows.Forms.ComboBox();
            this.lblDestino = new System.Windows.Forms.Label();
            this.cmbDestino = new System.Windows.Forms.ComboBox();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.btnCrearVuelo = new System.Windows.Forms.Button();
            this.btnCambiosOperativos = new System.Windows.Forms.Button();
            this.btnAsignarPuerta = new System.Windows.Forms.Button();
            this.btnHistorial = new System.Windows.Forms.Button();
            this.dgvVuelos = new System.Windows.Forms.DataGridView();
            this.pnlAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVuelos)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(246, 37);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Gestion de Vuelos";
            // 
            // pnlAcciones
            // 
            this.pnlAcciones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlAcciones.Controls.Add(this.lblBuscar);
            this.pnlAcciones.Controls.Add(this.txtBuscar);
            this.pnlAcciones.Controls.Add(this.lblEstado);
            this.pnlAcciones.Controls.Add(this.cmbEstadoFiltro);
            this.pnlAcciones.Controls.Add(this.lblAerolinea);
            this.pnlAcciones.Controls.Add(this.cmbAerolinea);
            this.pnlAcciones.Controls.Add(this.lblOrigen);
            this.pnlAcciones.Controls.Add(this.cmbOrigen);
            this.pnlAcciones.Controls.Add(this.lblDestino);
            this.pnlAcciones.Controls.Add(this.cmbDestino);
            this.pnlAcciones.Controls.Add(this.btnFiltrar);
            this.pnlAcciones.Controls.Add(this.btnCrearVuelo);
            this.pnlAcciones.Controls.Add(this.btnCambiosOperativos);
            this.pnlAcciones.Controls.Add(this.btnAsignarPuerta);
            this.pnlAcciones.Controls.Add(this.btnHistorial);
            this.pnlAcciones.Location = new System.Drawing.Point(20, 55);
            this.pnlAcciones.Name = "pnlAcciones";
            this.pnlAcciones.Size = new System.Drawing.Size(966, 70);
            this.pnlAcciones.TabIndex = 1;
            // 
            // lblBuscar
            // 
            this.lblBuscar.Location = new System.Drawing.Point(3, 0);
            this.lblBuscar.Name = "lblBuscar";
            this.lblBuscar.Size = new System.Drawing.Size(55, 25);
            this.lblBuscar.TabIndex = 0;
            this.lblBuscar.Text = "Buscar:";
            this.lblBuscar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBuscar
            // 
            this.txtBuscar.Location = new System.Drawing.Point(64, 3);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(150, 22);
            this.txtBuscar.TabIndex = 1;
            // 
            // lblEstado
            // 
            this.lblEstado.Location = new System.Drawing.Point(220, 0);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(50, 25);
            this.lblEstado.TabIndex = 2;
            this.lblEstado.Text = "Estado:";
            this.lblEstado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbEstadoFiltro
            // 
            this.cmbEstadoFiltro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstadoFiltro.Location = new System.Drawing.Point(276, 3);
            this.cmbEstadoFiltro.Name = "cmbEstadoFiltro";
            this.cmbEstadoFiltro.Size = new System.Drawing.Size(115, 24);
            this.cmbEstadoFiltro.TabIndex = 3;
            // 
            // lblAerolinea
            // 
            this.lblAerolinea.Location = new System.Drawing.Point(397, 0);
            this.lblAerolinea.Name = "lblAerolinea";
            this.lblAerolinea.Size = new System.Drawing.Size(65, 25);
            this.lblAerolinea.TabIndex = 4;
            this.lblAerolinea.Text = "Aerolínea:";
            this.lblAerolinea.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbAerolinea
            // 
            this.cmbAerolinea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAerolinea.Location = new System.Drawing.Point(468, 3);
            this.cmbAerolinea.Name = "cmbAerolinea";
            this.cmbAerolinea.Size = new System.Drawing.Size(120, 24);
            this.cmbAerolinea.TabIndex = 5;
            // 
            // lblOrigen
            // 
            this.lblOrigen.Location = new System.Drawing.Point(594, 0);
            this.lblOrigen.Name = "lblOrigen";
            this.lblOrigen.Size = new System.Drawing.Size(55, 25);
            this.lblOrigen.TabIndex = 6;
            this.lblOrigen.Text = "Origen:";
            this.lblOrigen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbOrigen
            // 
            this.cmbOrigen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrigen.Location = new System.Drawing.Point(655, 3);
            this.cmbOrigen.Name = "cmbOrigen";
            this.cmbOrigen.Size = new System.Drawing.Size(90, 24);
            this.cmbOrigen.TabIndex = 7;
            // 
            // lblDestino
            // 
            this.lblDestino.Location = new System.Drawing.Point(751, 0);
            this.lblDestino.Name = "lblDestino";
            this.lblDestino.Size = new System.Drawing.Size(60, 25);
            this.lblDestino.TabIndex = 8;
            this.lblDestino.Text = "Destino:";
            this.lblDestino.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbDestino
            // 
            this.cmbDestino.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDestino.Location = new System.Drawing.Point(817, 3);
            this.cmbDestino.Name = "cmbDestino";
            this.cmbDestino.Size = new System.Drawing.Size(90, 24);
            this.cmbDestino.TabIndex = 9;
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnFiltrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFiltrar.ForeColor = System.Drawing.Color.White;
            this.btnFiltrar.Location = new System.Drawing.Point(3, 33);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(90, 30);
            this.btnFiltrar.TabIndex = 10;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = false;
            this.btnFiltrar.Click += new System.EventHandler(this.BtnFiltrar_Click);
            // 
            // btnCrearVuelo
            // 
            this.btnCrearVuelo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnCrearVuelo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCrearVuelo.ForeColor = System.Drawing.Color.White;
            this.btnCrearVuelo.Location = new System.Drawing.Point(99, 33);
            this.btnCrearVuelo.Name = "btnCrearVuelo";
            this.btnCrearVuelo.Size = new System.Drawing.Size(110, 30);
            this.btnCrearVuelo.TabIndex = 11;
            this.btnCrearVuelo.Text = "Crear Vuelo";
            this.btnCrearVuelo.UseVisualStyleBackColor = false;
            this.btnCrearVuelo.Click += new System.EventHandler(this.BtnCrearVuelo_Click);
            // 
            // btnCambiosOperativos
            // 
            this.btnCambiosOperativos.BackColor = System.Drawing.Color.Orange;
            this.btnCambiosOperativos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCambiosOperativos.ForeColor = System.Drawing.Color.White;
            this.btnCambiosOperativos.Location = new System.Drawing.Point(215, 33);
            this.btnCambiosOperativos.Name = "btnCambiosOperativos";
            this.btnCambiosOperativos.Size = new System.Drawing.Size(140, 30);
            this.btnCambiosOperativos.TabIndex = 12;
            this.btnCambiosOperativos.Text = "Cambios Operativos";
            this.btnCambiosOperativos.UseVisualStyleBackColor = false;
            this.btnCambiosOperativos.Click += new System.EventHandler(this.BtnCambiosOperativos_Click);
            // 
            // btnAsignarPuerta
            // 
            this.btnAsignarPuerta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(122)))), ((int)(((byte)(87)))));
            this.btnAsignarPuerta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAsignarPuerta.ForeColor = System.Drawing.Color.White;
            this.btnAsignarPuerta.Location = new System.Drawing.Point(361, 33);
            this.btnAsignarPuerta.Name = "btnAsignarPuerta";
            this.btnAsignarPuerta.Size = new System.Drawing.Size(110, 30);
            this.btnAsignarPuerta.TabIndex = 14;
            this.btnAsignarPuerta.Text = "Asignar Puerta";
            this.btnAsignarPuerta.UseVisualStyleBackColor = false;
            this.btnAsignarPuerta.Click += new System.EventHandler(this.BtnAsignarPuerta_Click);
            // 
            // btnHistorial
            // 
            this.btnHistorial.BackColor = System.Drawing.Color.Gray;
            this.btnHistorial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistorial.ForeColor = System.Drawing.Color.White;
            this.btnHistorial.Location = new System.Drawing.Point(477, 33);
            this.btnHistorial.Name = "btnHistorial";
            this.btnHistorial.Size = new System.Drawing.Size(90, 30);
            this.btnHistorial.TabIndex = 13;
            this.btnHistorial.Text = "Historial";
            this.btnHistorial.UseVisualStyleBackColor = false;
            this.btnHistorial.Click += new System.EventHandler(this.BtnHistorial_Click);
            // 
            // dgvVuelos
            //
            this.dgvVuelos.AllowUserToAddRows = false;
            this.dgvVuelos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvVuelos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVuelos.BackgroundColor = System.Drawing.Color.White;
            this.dgvVuelos.ColumnHeadersHeight = 29;
            this.dgvVuelos.Location = new System.Drawing.Point(20, 135);
            this.dgvVuelos.MultiSelect = false;
            this.dgvVuelos.Name = "dgvVuelos";
            this.dgvVuelos.ReadOnly = true;
            this.dgvVuelos.RowHeadersWidth = 51;
            this.dgvVuelos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVuelos.Size = new System.Drawing.Size(966, 445);
            this.dgvVuelos.TabIndex = 0;
            this.dgvVuelos.SelectionChanged += new System.EventHandler(this.DgvVuelos_SelectionChanged);
            // 
            // FrmGestionVuelos
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1006, 600);
            this.Controls.Add(this.dgvVuelos);
            this.Controls.Add(this.pnlAcciones);
            this.Controls.Add(this.lblTitle);
            this.MinimumSize = new System.Drawing.Size(1024, 600);
            this.Name = "FrmGestionVuelos";
            this.Text = "Gestion de Vuelos";
            this.pnlAcciones.ResumeLayout(false);
            this.pnlAcciones.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVuelos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
