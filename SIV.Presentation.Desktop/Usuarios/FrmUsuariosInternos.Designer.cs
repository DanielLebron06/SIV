namespace SIV.Presentation.Desktop.Usuarios
{
    partial class FrmUsuariosInternos
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnDesactivar;
        private System.Windows.Forms.DataGridView dgvUsuarios;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnDesactivar = new System.Windows.Forms.Button();
            this.dgvUsuarios = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).BeginInit();
            this.SuspendLayout();
            
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Text = "Usuarios Internos";
            
            this.btnNuevo.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevo.ForeColor = System.Drawing.Color.White;
            this.btnNuevo.Location = new System.Drawing.Point(20, 60);
            this.btnNuevo.Size = new System.Drawing.Size(120, 35);
            this.btnNuevo.Text = "Nuevo Usuario";
            this.btnNuevo.Click += new System.EventHandler(this.BtnNuevo_Click);
            
            this.btnDesactivar.BackColor = System.Drawing.Color.Orange;
            this.btnDesactivar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDesactivar.ForeColor = System.Drawing.Color.White;
            this.btnDesactivar.Location = new System.Drawing.Point(150, 60);
            this.btnDesactivar.Size = new System.Drawing.Size(150, 35);
            this.btnDesactivar.Text = "Activar/Desactivar";
            this.btnDesactivar.Click += new System.EventHandler(this.BtnDesactivar_Click);
            
            this.dgvUsuarios.AllowUserToAddRows = false;
            this.dgvUsuarios.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvUsuarios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsuarios.BackgroundColor = System.Drawing.Color.White;
            this.dgvUsuarios.ColumnHeadersVisible = true;
            this.dgvUsuarios.Location = new System.Drawing.Point(20, 110);
            this.dgvUsuarios.MultiSelect = false;
            this.dgvUsuarios.ReadOnly = true;
            this.dgvUsuarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsuarios.Size = new System.Drawing.Size(740, 450);
            
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.dgvUsuarios);
            this.Controls.Add(this.btnDesactivar);
            this.Controls.Add(this.btnNuevo);
            this.Controls.Add(this.lblTitle);
            this.Text = "Gestión de Usuarios Internos";
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
