namespace SIV.Presentation.Desktop.Vuelos
{
    partial class FrmCrearVuelo
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblNum;
        private System.Windows.Forms.TextBox txtNumeroVuelo;
        private System.Windows.Forms.Label lblAero;
        private System.Windows.Forms.ComboBox cmbAerolinea;
        private System.Windows.Forms.Label lblOrigen;
        private System.Windows.Forms.ComboBox cmbOrigen;
        private System.Windows.Forms.Label lblDestino;
        private System.Windows.Forms.ComboBox cmbDestino;
        private System.Windows.Forms.Label lblSalida;
        private System.Windows.Forms.DateTimePicker dtpSalida;
        private System.Windows.Forms.Label lblLlegada;
        private System.Windows.Forms.DateTimePicker dtpLlegada;
        private System.Windows.Forms.Button btnGuardar;

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
            this.lblNum = new System.Windows.Forms.Label();
            this.txtNumeroVuelo = new System.Windows.Forms.TextBox();
            this.lblAero = new System.Windows.Forms.Label();
            this.cmbAerolinea = new System.Windows.Forms.ComboBox();
            this.lblOrigen = new System.Windows.Forms.Label();
            this.cmbOrigen = new System.Windows.Forms.ComboBox();
            this.lblDestino = new System.Windows.Forms.Label();
            this.cmbDestino = new System.Windows.Forms.ComboBox();
            this.lblSalida = new System.Windows.Forms.Label();
            this.dtpSalida = new System.Windows.Forms.DateTimePicker();
            this.lblLlegada = new System.Windows.Forms.Label();
            this.dtpLlegada = new System.Windows.Forms.DateTimePicker();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Text = "Registrar Nuevo Vuelo";
            
            this.lblNum.Location = new System.Drawing.Point(20, 60);
            this.lblNum.Size = new System.Drawing.Size(300, 20);
            this.lblNum.Text = "Número de Vuelo (IATA):";
            
            this.txtNumeroVuelo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.txtNumeroVuelo.Location = new System.Drawing.Point(20, 80);
            this.txtNumeroVuelo.Size = new System.Drawing.Size(340, 25);
            
            this.lblAero.Location = new System.Drawing.Point(20, 110);
            this.lblAero.Size = new System.Drawing.Size(300, 20);
            this.lblAero.Text = "Aerolínea:";
            
            this.cmbAerolinea.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.cmbAerolinea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAerolinea.Location = new System.Drawing.Point(20, 130);
            this.cmbAerolinea.Size = new System.Drawing.Size(340, 25);
            
            this.lblOrigen.Location = new System.Drawing.Point(20, 160);
            this.lblOrigen.Size = new System.Drawing.Size(300, 20);
            this.lblOrigen.Text = "Origen:";
            
            this.cmbOrigen.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.cmbOrigen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrigen.Location = new System.Drawing.Point(20, 180);
            this.cmbOrigen.Size = new System.Drawing.Size(340, 25);
            
            this.lblDestino.Location = new System.Drawing.Point(20, 210);
            this.lblDestino.Size = new System.Drawing.Size(300, 20);
            this.lblDestino.Text = "Destino:";
            
            this.cmbDestino.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.cmbDestino.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDestino.Location = new System.Drawing.Point(20, 230);
            this.cmbDestino.Size = new System.Drawing.Size(340, 25);
            
            this.lblSalida.Location = new System.Drawing.Point(20, 260);
            this.lblSalida.Size = new System.Drawing.Size(150, 20);
            this.lblSalida.Text = "Salida Planificada:";
            
            this.dtpSalida.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            this.dtpSalida.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpSalida.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSalida.Location = new System.Drawing.Point(20, 280);
            this.dtpSalida.Size = new System.Drawing.Size(150, 25);
            
            this.lblLlegada.Location = new System.Drawing.Point(190, 260);
            this.lblLlegada.Size = new System.Drawing.Size(150, 20);
            this.lblLlegada.Text = "Llegada Planificada:";
            
            this.dtpLlegada.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            this.dtpLlegada.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpLlegada.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpLlegada.Location = new System.Drawing.Point(190, 280);
            this.dtpLlegada.Size = new System.Drawing.Size(150, 25);
            
            this.btnGuardar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(20, 330);
            this.btnGuardar.Size = new System.Drawing.Size(340, 40);
            this.btnGuardar.Text = "GUARDAR";
            this.btnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(400, 400);
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.dtpLlegada);
            this.Controls.Add(this.lblLlegada);
            this.Controls.Add(this.dtpSalida);
            this.Controls.Add(this.lblSalida);
            this.Controls.Add(this.cmbDestino);
            this.Controls.Add(this.lblDestino);
            this.Controls.Add(this.cmbOrigen);
            this.Controls.Add(this.lblOrigen);
            this.Controls.Add(this.cmbAerolinea);
            this.Controls.Add(this.lblAero);
            this.Controls.Add(this.txtNumeroVuelo);
            this.Controls.Add(this.lblNum);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Crear Vuelo";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
