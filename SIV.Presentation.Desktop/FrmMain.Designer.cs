namespace SIV.Presentation.Desktop
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblSignalRStatus;
        private System.Windows.Forms.Label lblUser;

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
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblSignalRStatus = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlTopBar.SuspendLayout();
            this.SuspendLayout();
            
            this.pnlSidebar.AutoScroll = true;
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Width = 200;
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Height = 50;
            this.pnlTopBar.BackColor = System.Drawing.Color.White;
            
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(20, 15);
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            
            this.lblSignalRStatus.Text = "🔴 Desconectado";
            this.lblSignalRStatus.AutoSize = true;
            this.lblSignalRStatus.Location = new System.Drawing.Point(800, 15);
            this.lblSignalRStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSignalRStatus.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            
            this.pnlTopBar.Controls.Add(this.lblUser);
            this.pnlTopBar.Controls.Add(this.lblSignalRStatus);
            
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.BackColor = System.Drawing.Color.White;
            
            this.ClientSize = new System.Drawing.Size(1024, 700);
            this.MinimumSize = new System.Drawing.Size(1024, 600);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlTopBar);
            this.Controls.Add(this.pnlSidebar);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.Text = "SIV - Sistema de Información de Vuelos";
            this.pnlTopBar.ResumeLayout(false);
            this.pnlTopBar.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}