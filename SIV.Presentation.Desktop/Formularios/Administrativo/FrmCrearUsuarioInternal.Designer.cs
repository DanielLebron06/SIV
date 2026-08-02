namespace SIV.Presentation.Desktop.Formularios.Administrativo
{
    partial class FrmCrearUsuarioInternal
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblRol;
        private System.Windows.Forms.ComboBox cmbRol;
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
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblRol = new System.Windows.Forms.Label();
            this.cmbRol = new System.Windows.Forms.ComboBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Text = "Nuevo Usuario Interno";

            this.lblEmail.Location = new System.Drawing.Point(20, 60);
            this.lblEmail.Size = new System.Drawing.Size(300, 20);
            this.lblEmail.Text = "Email:";

            this.txtEmail.Location = new System.Drawing.Point(20, 80);
            this.txtEmail.Size = new System.Drawing.Size(340, 25);

            this.lblPassword.Location = new System.Drawing.Point(20, 115);
            this.lblPassword.Size = new System.Drawing.Size(300, 20);
            this.lblPassword.Text = "Password:";

            this.txtPassword.Location = new System.Drawing.Point(20, 135);
            this.txtPassword.Size = new System.Drawing.Size(340, 25);
            this.txtPassword.PasswordChar = '*';

            this.lblRol.Location = new System.Drawing.Point(20, 170);
            this.lblRol.Size = new System.Drawing.Size(300, 20);
            this.lblRol.Text = "Rol:";

            this.cmbRol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRol.Location = new System.Drawing.Point(20, 190);
            this.cmbRol.Size = new System.Drawing.Size(340, 25);

            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(20, 240);
            this.btnGuardar.Size = new System.Drawing.Size(160, 35);
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);

            this.btnCancelar.BackColor = System.Drawing.Color.Gray;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(200, 240);
            this.btnCancelar.Size = new System.Drawing.Size(160, 35);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);

            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.cmbRol);
            this.Controls.Add(this.lblRol);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Crear Usuario Interno";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
