namespace SIV.Presentation.Desktop.Formularios.Administrativo
{
    partial class FrmCatalogos
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabAerolineas;
        private System.Windows.Forms.TabPage tabAeropuertos;
        private System.Windows.Forms.Button btnNuevaAerolinea;
        private System.Windows.Forms.Button btnEliminarAerolinea;
        private System.Windows.Forms.DataGridView dgvAerolineas;
        private System.Windows.Forms.Button btnNuevoAeropuerto;
        private System.Windows.Forms.Button btnDesactivarAeropuerto;
        private System.Windows.Forms.DataGridView dgvAeropuertos;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabAerolineas = new System.Windows.Forms.TabPage();
            this.btnNuevaAerolinea = new System.Windows.Forms.Button();
            this.btnEliminarAerolinea = new System.Windows.Forms.Button();
            this.dgvAerolineas = new System.Windows.Forms.DataGridView();
            this.tabAeropuertos = new System.Windows.Forms.TabPage();
            this.btnNuevoAeropuerto = new System.Windows.Forms.Button();
            this.btnDesactivarAeropuerto = new System.Windows.Forms.Button();
            this.dgvAeropuertos = new System.Windows.Forms.DataGridView();
            this.tabControl.SuspendLayout();
            this.tabAerolineas.SuspendLayout();
            this.tabAeropuertos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAerolineas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAeropuertos)).BeginInit();
            this.SuspendLayout();

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Text = "Catalogos del Sistema";

            this.tabControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.tabControl.Location = new System.Drawing.Point(20, 60);
            this.tabControl.Size = new System.Drawing.Size(840, 520);
            this.tabControl.Controls.Add(this.tabAerolineas);
            this.tabControl.Controls.Add(this.tabAeropuertos);

            this.tabAerolineas.BackColor = System.Drawing.Color.White;
            this.tabAerolineas.Text = "Aerolineas";
            this.tabAerolineas.Controls.Add(this.btnNuevaAerolinea);
            this.tabAerolineas.Controls.Add(this.btnEliminarAerolinea);
            this.tabAerolineas.Controls.Add(this.dgvAerolineas);

            this.btnNuevaAerolinea.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnNuevaAerolinea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevaAerolinea.ForeColor = System.Drawing.Color.White;
            this.btnNuevaAerolinea.Location = new System.Drawing.Point(10, 10);
            this.btnNuevaAerolinea.Size = new System.Drawing.Size(130, 35);
            this.btnNuevaAerolinea.Text = "Nueva Aerolinea";
            this.btnNuevaAerolinea.Click += new System.EventHandler(this.BtnNuevaAerolinea_Click);

            this.btnEliminarAerolinea.BackColor = System.Drawing.Color.Red;
            this.btnEliminarAerolinea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminarAerolinea.ForeColor = System.Drawing.Color.White;
            this.btnEliminarAerolinea.Location = new System.Drawing.Point(150, 10);
            this.btnEliminarAerolinea.Size = new System.Drawing.Size(100, 35);
            this.btnEliminarAerolinea.Text = "Desactivar";
            this.btnEliminarAerolinea.Click += new System.EventHandler(this.BtnEliminarAerolinea_Click);

            this.dgvAerolineas.AllowUserToAddRows = false;
            this.dgvAerolineas.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvAerolineas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAerolineas.BackgroundColor = System.Drawing.Color.White;
            this.dgvAerolineas.Location = new System.Drawing.Point(10, 55);
            this.dgvAerolineas.MultiSelect = false;
            this.dgvAerolineas.ReadOnly = true;
            this.dgvAerolineas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAerolineas.Size = new System.Drawing.Size(810, 420);

            this.tabAeropuertos.BackColor = System.Drawing.Color.White;
            this.tabAeropuertos.Text = "Aeropuertos";
            this.tabAeropuertos.Controls.Add(this.btnNuevoAeropuerto);
            this.tabAeropuertos.Controls.Add(this.btnDesactivarAeropuerto);
            this.tabAeropuertos.Controls.Add(this.dgvAeropuertos);

            this.btnNuevoAeropuerto.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnNuevoAeropuerto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevoAeropuerto.ForeColor = System.Drawing.Color.White;
            this.btnNuevoAeropuerto.Location = new System.Drawing.Point(10, 10);
            this.btnNuevoAeropuerto.Size = new System.Drawing.Size(140, 35);
            this.btnNuevoAeropuerto.Text = "Nuevo Aeropuerto";
            this.btnNuevoAeropuerto.Click += new System.EventHandler(this.BtnNuevoAeropuerto_Click);

            this.btnDesactivarAeropuerto.BackColor = System.Drawing.Color.Orange;
            this.btnDesactivarAeropuerto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDesactivarAeropuerto.ForeColor = System.Drawing.Color.White;
            this.btnDesactivarAeropuerto.Location = new System.Drawing.Point(160, 10);
            this.btnDesactivarAeropuerto.Size = new System.Drawing.Size(100, 35);
            this.btnDesactivarAeropuerto.Text = "Desactivar";
            this.btnDesactivarAeropuerto.Click += new System.EventHandler(this.BtnDesactivarAeropuerto_Click);

            this.dgvAeropuertos.AllowUserToAddRows = false;
            this.dgvAeropuertos.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvAeropuertos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAeropuertos.BackgroundColor = System.Drawing.Color.White;
            this.dgvAeropuertos.Location = new System.Drawing.Point(10, 55);
            this.dgvAeropuertos.MultiSelect = false;
            this.dgvAeropuertos.ReadOnly = true;
            this.dgvAeropuertos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAeropuertos.Size = new System.Drawing.Size(810, 420);

            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(880, 600);
            this.MinimumSize = new System.Drawing.Size(880, 600);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.lblTitle);
            this.Text = "Gestion de Catalogos";
            this.tabControl.ResumeLayout(false);
            this.tabAerolineas.ResumeLayout(false);
            this.tabAeropuertos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAerolineas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAeropuertos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
