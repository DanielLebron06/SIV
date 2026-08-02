namespace SIV.Presentation.Desktop.Formularios.Administrativo
{
    partial class FrmCrearAerolinea
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtCodigoIATA;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.txtCodigoIATA = new System.Windows.Forms.TextBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Text = "Nueva Aerolinea";

            this.lblNombre.Location = new System.Drawing.Point(20, 60);
            this.lblNombre.Size = new System.Drawing.Size(300, 20);
            this.lblNombre.Text = "Nombre:";

            this.txtNombre.Location = new System.Drawing.Point(20, 80);
            this.txtNombre.Size = new System.Drawing.Size(340, 25);

            this.lblCodigo.Location = new System.Drawing.Point(20, 115);
            this.lblCodigo.Size = new System.Drawing.Size(300, 20);
            this.lblCodigo.Text = "Codigo IATA (2 caracteres):";

            this.txtCodigoIATA.Location = new System.Drawing.Point(20, 135);
            this.txtCodigoIATA.MaxLength = 2;
            this.txtCodigoIATA.Size = new System.Drawing.Size(340, 25);

            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(20, 180);
            this.btnGuardar.Size = new System.Drawing.Size(160, 35);
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);

            this.btnCancelar.BackColor = System.Drawing.Color.Gray;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(200, 180);
            this.btnCancelar.Size = new System.Drawing.Size(160, 35);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);

            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(400, 240);
            this.MinimumSize = new System.Drawing.Size(400, 240);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.txtCodigoIATA);
            this.Controls.Add(this.lblCodigo);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Crear Aerolinea";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
