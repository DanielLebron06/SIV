namespace SIV.Presentation.Desktop.Formularios
{
    partial class FrmLogin
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Button btnLogin;

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
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblError = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            
            this.lblTitle.Text = "SISTEMA DE INFORMACIÓN DE VUELOS";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 30);
            this.lblTitle.Size = new System.Drawing.Size(350, 30);
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            this.lblEmail.Text = "Correo Electrónico:";
            this.lblEmail.Location = new System.Drawing.Point(50, 90);
            this.lblEmail.Size = new System.Drawing.Size(300, 20);
            
            this.txtEmail.Location = new System.Drawing.Point(50, 110);
            this.txtEmail.Size = new System.Drawing.Size(280, 25);
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
            
            this.lblPassword.Text = "Contraseña:";
            this.lblPassword.Location = new System.Drawing.Point(50, 150);
            this.lblPassword.Size = new System.Drawing.Size(300, 20);
            
            this.txtPassword.Location = new System.Drawing.Point(50, 170);
            this.txtPassword.Size = new System.Drawing.Size(280, 25);
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPassword.PasswordChar = '*';
            
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(50, 205);
            this.lblError.Size = new System.Drawing.Size(280, 20);
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            this.btnLogin.Text = "INGRESAR";
            this.btnLogin.Location = new System.Drawing.Point(50, 240);
            this.btnLogin.Size = new System.Drawing.Size(280, 40);
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            
            this.ClientSize = new System.Drawing.Size(400, 350);
            this.MinimumSize = new System.Drawing.Size(400, 350);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btnLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.White;
            this.Text = "SIV - Iniciar Sesión";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}