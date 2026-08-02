namespace SIV.Presentation.Desktop.Formularios.Operativo
{
    partial class FrmHistorialVuelo
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ListBox lstHistorial;

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
            this.lstHistorial = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Text = "Línea de Tiempo del Vuelo";
            
            this.lstHistorial.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lstHistorial.Location = new System.Drawing.Point(20, 60);
            this.lstHistorial.Size = new System.Drawing.Size(440, 280);
            
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(500, 400);
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Controls.Add(this.lstHistorial);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Historial de Vuelo";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
