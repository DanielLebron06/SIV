using SIV.Presentation.Desktop.Services;
using SIV.Presentation.Desktop.Services.Dtos;
using SIV.Presentation.Desktop.Services.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Formularios.Reportes
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
        }

        private async System.Threading.Tasks.Task LoadAuditLog()
        {
            if (_reporteService == null)
                return;

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
                MessageBox.Show(MensajesError.ObtenerMensaje(ex), "Error al cargar auditoria", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
