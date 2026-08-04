namespace SIV.Presentation.Desktop.Vuelos
{
    partial class FrmCambiosOperativos
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabEstado;
        private System.Windows.Forms.TabPage tabRetrasoAdelanto;
        private System.Windows.Forms.TabPage tabPuerta;

        private System.Windows.Forms.Label lblEst;
        private System.Windows.Forms.ComboBox cmbEstado;
        private System.Windows.Forms.Label lblMotivoEstado;
        private System.Windows.Forms.TextBox txtMotivoEstado;
        private System.Windows.Forms.Button btnActualizarEstado;

        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.ComboBox cmbTipo;
        private System.Windows.Forms.Label lblHora;
        private System.Windows.Forms.DateTimePicker dtpHora;
        private System.Windows.Forms.Label lblMotivoTiempo;
        private System.Windows.Forms.TextBox txtMotivoTiempo;
        private System.Windows.Forms.Button btnGuardarTiempo;

        private System.Windows.Forms.Label lblPuerta;
        private System.Windows.Forms.TextBox txtPuerta;
        private System.Windows.Forms.Label lblMotivoPuerta;
        private System.Windows.Forms.TextBox txtMotivoPuerta;
        private System.Windows.Forms.Button btnGuardarPuerta;

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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabEstado = new System.Windows.Forms.TabPage();
            this.lblEst = new System.Windows.Forms.Label();
            this.cmbEstado = new System.Windows.Forms.ComboBox();
            this.lblMotivoEstado = new System.Windows.Forms.Label();
            this.txtMotivoEstado = new System.Windows.Forms.TextBox();
            this.btnActualizarEstado = new System.Windows.Forms.Button();
            
            this.tabRetrasoAdelanto = new System.Windows.Forms.TabPage();
            this.lblTipo = new System.Windows.Forms.Label();
            this.cmbTipo = new System.Windows.Forms.ComboBox();
            this.lblHora = new System.Windows.Forms.Label();
            this.dtpHora = new System.Windows.Forms.DateTimePicker();
            this.lblMotivoTiempo = new System.Windows.Forms.Label();
            this.txtMotivoTiempo = new System.Windows.Forms.TextBox();
            this.btnGuardarTiempo = new System.Windows.Forms.Button();
            
            this.tabPuerta = new System.Windows.Forms.TabPage();
            this.lblPuerta = new System.Windows.Forms.Label();
            this.txtPuerta = new System.Windows.Forms.TextBox();
            this.lblMotivoPuerta = new System.Windows.Forms.Label();
            this.txtMotivoPuerta = new System.Windows.Forms.TextBox();
            this.btnGuardarPuerta = new System.Windows.Forms.Button();
            
            this.tabControl.SuspendLayout();
            this.tabEstado.SuspendLayout();
            this.tabRetrasoAdelanto.SuspendLayout();
            this.tabPuerta.SuspendLayout();
            this.SuspendLayout();
            
            this.tabControl.Controls.Add(this.tabEstado);
            this.tabControl.Controls.Add(this.tabRetrasoAdelanto);
            this.tabControl.Controls.Add(this.tabPuerta);
            this.tabControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.tabControl.Location = new System.Drawing.Point(10, 100);
            this.tabControl.Size = new System.Drawing.Size(410, 340);
            
            this.tabEstado.Controls.Add(this.lblEst);
            this.tabEstado.Controls.Add(this.cmbEstado);
            this.tabEstado.Controls.Add(this.lblMotivoEstado);
            this.tabEstado.Controls.Add(this.txtMotivoEstado);
            this.tabEstado.Controls.Add(this.btnActualizarEstado);
            this.tabEstado.Text = "Estado";
            this.tabEstado.BackColor = System.Drawing.Color.White;
            
            this.lblEst.Location = new System.Drawing.Point(20, 20);
            this.lblEst.Size = new System.Drawing.Size(150, 20);
            this.lblEst.Text = "Nuevo Estado:";
            
            this.cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstado.Location = new System.Drawing.Point(20, 40);
            this.cmbEstado.Size = new System.Drawing.Size(350, 25);
            
            this.lblMotivoEstado.Location = new System.Drawing.Point(20, 80);
            this.lblMotivoEstado.Size = new System.Drawing.Size(250, 20);
            this.lblMotivoEstado.Text = "Motivo (Obligatorio para Cancelar):";
            
            this.txtMotivoEstado.Location = new System.Drawing.Point(20, 100);
            this.txtMotivoEstado.Multiline = true;
            this.txtMotivoEstado.Size = new System.Drawing.Size(350, 60);
            
            this.btnActualizarEstado.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnActualizarEstado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActualizarEstado.ForeColor = System.Drawing.Color.White;
            this.btnActualizarEstado.Location = new System.Drawing.Point(20, 180);
            this.btnActualizarEstado.Size = new System.Drawing.Size(350, 40);
            this.btnActualizarEstado.Text = "ACTUALIZAR";
            this.btnActualizarEstado.Click += new System.EventHandler(this.BtnActualizarEstado_Click);
            
            this.tabRetrasoAdelanto.Controls.Add(this.lblTipo);
            this.tabRetrasoAdelanto.Controls.Add(this.cmbTipo);
            this.tabRetrasoAdelanto.Controls.Add(this.lblHora);
            this.tabRetrasoAdelanto.Controls.Add(this.dtpHora);
            this.tabRetrasoAdelanto.Controls.Add(this.lblMotivoTiempo);
            this.tabRetrasoAdelanto.Controls.Add(this.txtMotivoTiempo);
            this.tabRetrasoAdelanto.Controls.Add(this.btnGuardarTiempo);
            this.tabRetrasoAdelanto.Text = "Retraso/Adelanto";
            this.tabRetrasoAdelanto.BackColor = System.Drawing.Color.White;
            
            this.lblTipo.Location = new System.Drawing.Point(20, 20);
            this.lblTipo.Size = new System.Drawing.Size(100, 20);
            this.lblTipo.Text = "Tipo:";
            
            this.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipo.Location = new System.Drawing.Point(20, 40);
            this.cmbTipo.Size = new System.Drawing.Size(150, 25);
            
            this.lblHora.Location = new System.Drawing.Point(20, 80);
            this.lblHora.Size = new System.Drawing.Size(150, 20);
            this.lblHora.Text = "Nueva Hora Estimada:";
            
            this.dtpHora.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpHora.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpHora.Location = new System.Drawing.Point(20, 100);
            this.dtpHora.Size = new System.Drawing.Size(200, 25);
            
            this.lblMotivoTiempo.Location = new System.Drawing.Point(20, 140);
            this.lblMotivoTiempo.Size = new System.Drawing.Size(150, 20);
            this.lblMotivoTiempo.Text = "Motivo:";
            
            this.txtMotivoTiempo.Location = new System.Drawing.Point(20, 160);
            this.txtMotivoTiempo.Multiline = true;
            this.txtMotivoTiempo.Size = new System.Drawing.Size(350, 60);
            
            this.btnGuardarTiempo.BackColor = System.Drawing.Color.Orange;
            this.btnGuardarTiempo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardarTiempo.ForeColor = System.Drawing.Color.White;
            this.btnGuardarTiempo.Location = new System.Drawing.Point(20, 240);
            this.btnGuardarTiempo.Size = new System.Drawing.Size(350, 40);
            this.btnGuardarTiempo.Text = "GUARDAR";
            this.btnGuardarTiempo.Click += new System.EventHandler(this.BtnGuardarTiempo_Click);
            
            this.tabPuerta.Controls.Add(this.lblPuerta);
            this.tabPuerta.Controls.Add(this.txtPuerta);
            this.tabPuerta.Controls.Add(this.lblMotivoPuerta);
            this.tabPuerta.Controls.Add(this.txtMotivoPuerta);
            this.tabPuerta.Controls.Add(this.btnGuardarPuerta);
            this.tabPuerta.Text = "Puerta";
            this.tabPuerta.BackColor = System.Drawing.Color.White;
            
            this.lblPuerta.Location = new System.Drawing.Point(20, 20);
            this.lblPuerta.Size = new System.Drawing.Size(150, 20);
            this.lblPuerta.Text = "Nueva Puerta:";
            
            this.txtPuerta.Location = new System.Drawing.Point(20, 40);
            this.txtPuerta.Size = new System.Drawing.Size(350, 25);
            
            this.lblMotivoPuerta.Location = new System.Drawing.Point(20, 80);
            this.lblMotivoPuerta.Size = new System.Drawing.Size(150, 20);
            this.lblMotivoPuerta.Text = "Motivo:";
            
            this.txtMotivoPuerta.Location = new System.Drawing.Point(20, 100);
            this.txtMotivoPuerta.Multiline = true;
            this.txtMotivoPuerta.Size = new System.Drawing.Size(350, 60);
            
            this.btnGuardarPuerta.BackColor = System.Drawing.Color.Green;
            this.btnGuardarPuerta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardarPuerta.ForeColor = System.Drawing.Color.White;
            this.btnGuardarPuerta.Location = new System.Drawing.Point(20, 180);
            this.btnGuardarPuerta.Size = new System.Drawing.Size(350, 40);
            this.btnGuardarPuerta.Text = "GUARDAR PUERTA";
            this.btnGuardarPuerta.Click += new System.EventHandler(this.BtnGuardarPuerta_Click);
            
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(430, 460);
            this.MinimumSize = new System.Drawing.Size(430, 460);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cambios Operativos";
            this.tabControl.ResumeLayout(false);
            this.tabEstado.ResumeLayout(false);
            this.tabEstado.PerformLayout();
            this.tabRetrasoAdelanto.ResumeLayout(false);
            this.tabRetrasoAdelanto.PerformLayout();
            this.tabPuerta.ResumeLayout(false);
            this.tabPuerta.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
