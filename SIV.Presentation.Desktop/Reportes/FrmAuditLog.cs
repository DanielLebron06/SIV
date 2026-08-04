using SIV.Presentation.Desktop.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Reportes
{
    public class FrmAuditLog : Form
    {
        private readonly IReporteService _reporteService;
        private DataGridView dgvLog;
        private ComboBox cmbModulo;

        public FrmAuditLog(IReporteService reporteService) : this()
        {
            _reporteService = reporteService;
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;
            _ = LoadAuditLog();
        }

        public FrmAuditLog()
        {
            InitializeComponentProgrammatic();
        }

        private void InitializeComponentProgrammatic()
        {
            this.Text = "Log de Auditoría Inmutable";
            this.BackColor = Color.White;
            this.MinimumSize = new System.Drawing.Size(800, 600);

            var lblTitle = new Label { Text = "Log de Auditoría (Read-Only)", Font = new Font("Segoe UI", 16, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true };
            this.Controls.Add(lblTitle);

            var lblFiltros = new Label { Text = "Módulo:", Location = new Point(20, 60), Size = new Size(60, 20) };
            cmbModulo = new ComboBox { Location = new Point(80, 60), Size = new Size(150, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbModulo.Items.AddRange(new object[] { "Todos", "Vuelos", "Usuarios", "Notificaciones" });
            cmbModulo.SelectedIndex = 0;

            var btnBuscar = new Button { Text = "Filtrar", Location = new Point(240, 58), Size = new Size(100, 30), BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnBuscar.Click += async (s, e) => await LoadAuditLog();

            this.Controls.Add(lblFiltros); this.Controls.Add(cmbModulo);
            this.Controls.Add(btnBuscar);

            dgvLog = new DataGridView
            {
                Location = new Point(20, 100),
                Size = new Size(740, 460),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                BackgroundColor = Color.White,
                RowHeadersVisible = false
            };
            this.Controls.Add(dgvLog);

            AjusteLayout.AjustarFormulario(this);
        }

        private async System.Threading.Tasks.Task LoadAuditLog()
        {
            if (_reporteService == null)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(async () => await LoadAuditLog()));
                return;
            }

            try
            {
                FiltroAuditoriaDTO filtros = null;
                if (cmbModulo.SelectedIndex > 0 && Enum.TryParse<Modulo>(cmbModulo.SelectedItem.ToString(), out var modulo))
                {
                    filtros = new FiltroAuditoriaDTO { Modulo = modulo };
                }

                var logs = await _reporteService.ObtenerLogAuditoriaAsync(filtros);
                if (logs != null)
                {
                    dgvLog.DataSource = logs;
                }
            }
            catch (Exception ex)
            {
                var mensaje = MensajesError.ObtenerMensaje(ex);
                if (InvokeRequired)
                    Invoke((MethodInvoker)delegate { MessageBox.Show(mensaje, "Error al cargar auditoria", MessageBoxButtons.OK, MessageBoxIcon.Error); });
                else
                    MessageBox.Show(mensaje, "Error al cargar auditoria", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
