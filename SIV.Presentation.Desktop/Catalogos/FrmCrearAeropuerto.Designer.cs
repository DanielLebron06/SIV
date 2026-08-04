namespace SIV.Presentation.Desktop.Catalogos
{
    partial class FrmCrearAeropuerto
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtCodigoIATA;
        private System.Windows.Forms.Label lblCiudad;
        private System.Windows.Forms.TextBox txtCiudad;
        private System.Windows.Forms.Label lblPais;
        private System.Windows.Forms.TextBox txtPais;
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
            this.lblCiudad = new System.Windows.Forms.Label();
            this.txtCiudad = new System.Windows.Forms.TextBox();
            this.lblPais = new System.Windows.Forms.Label();
            this.txtPais = new System.Windows.Forms.TextBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Text = "Nuevo Aeropuerto";

            this.lblNombre.Location = new System.Drawing.Point(20, 60);
            this.lblNombre.Size = new System.Drawing.Size(300, 20);
            this.lblNombre.Text = "Nombre:";

            this.txtNombre.Location = new System.Drawing.Point(20, 80);
            this.txtNombre.Size = new System.Drawing.Size(340, 25);

            this.lblCodigo.Location = new System.Drawing.Point(20, 115);
            this.lblCodigo.Size = new System.Drawing.Size(300, 20);
            this.lblCodigo.Text = "Codigo IATA (3 caracteres):";

            this.txtCodigoIATA.Location = new System.Drawing.Point(20, 135);
            this.txtCodigoIATA.MaxLength = 3;
            this.txtCodigoIATA.Size = new System.Drawing.Size(340, 25);

            this.lblCiudad.Location = new System.Drawing.Point(20, 170);
            this.lblCiudad.Size = new System.Drawing.Size(300, 20);
            this.lblCiudad.Text = "Ciudad:";

            this.txtCiudad.Location = new System.Drawing.Point(20, 190);
            this.txtCiudad.Size = new System.Drawing.Size(340, 25);

            this.lblPais.Location = new System.Drawing.Point(20, 225);
            this.lblPais.Size = new System.Drawing.Size(300, 20);
            this.lblPais.Text = "Pais:";

            this.txtPais.Location = new System.Drawing.Point(20, 245);
            this.txtPais.Size = new System.Drawing.Size(340, 25);

            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(20, 290);
            this.btnGuardar.Size = new System.Drawing.Size(160, 35);
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);

            this.btnCancelar.BackColor = System.Drawing.Color.Gray;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(200, 290);
            this.btnCancelar.Size = new System.Drawing.Size(160, 35);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);

            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(400, 350);
            this.MinimumSize = new System.Drawing.Size(400, 350);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.txtPais);
            this.Controls.Add(this.lblPais);
            this.Controls.Add(this.txtCiudad);
            this.Controls.Add(this.lblCiudad);
            this.Controls.Add(this.txtCodigoIATA);
            this.Controls.Add(this.lblCodigo);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Crear Aeropuerto";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
