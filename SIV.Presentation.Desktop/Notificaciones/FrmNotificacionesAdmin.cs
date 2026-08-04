using SIV.Presentation.Desktop.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Notificaciones
{
    public class FrmNotificacionesAdmin : Form
    {
        private readonly INotificacionService _notificacionService;
        private DataGridView dgvNotificaciones;
        private TextBox txtVuelo;
        private TextBox txtUsuario;
        private ComboBox cmbEstadoLeida;
        private CheckBox chkFechaDesde;
        private DateTimePicker dtpFechaDesde;
        private CheckBox chkFechaHasta;
        private DateTimePicker dtpFechaHasta;
        private Label lblResultado;

        public FrmNotificacionesAdmin(INotificacionService notificacionService) : this()
        {
            _notificacionService = notificacionService;
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;
            Recargar();
        }

        public FrmNotificacionesAdmin()
        {
            InitializeComponentProgrammatic();
        }

        private void InitializeComponentProgrammatic()
        {
            this.Text = "Auditoría de Notificaciones";
            this.BackColor = Color.White;
            this.MinimumSize = new Size(900, 620);

            var lblTitle = new Label { Text = "Monitor de Notificaciones Enviadas", Font = new Font("Segoe UI", 16, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true };
            this.Controls.Add(lblTitle);

            var lblVuelo = new Label { Text = "Vuelo:", Location = new Point(20, 60), Size = new Size(60, 20) };
            txtVuelo = new TextBox { Location = new Point(80, 58), Size = new Size(180, 25) };

            var lblUsuario = new Label { Text = "Usuario:", Location = new Point(280, 60), Size = new Size(60, 20) };
            txtUsuario = new TextBox { Location = new Point(340, 58), Size = new Size(180, 25) };

            var lblEstado = new Label { Text = "Estado:", Location = new Point(540, 60), Size = new Size(60, 20) };
            cmbEstadoLeida = new ComboBox { Location = new Point(600, 58), Size = new Size(130, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbEstadoLeida.Items.AddRange(new object[] { "Todos", "Leídas", "No leídas" });
            cmbEstadoLeida.SelectedIndex = 0;

            chkFechaDesde = new CheckBox { Text = "Desde:", Location = new Point(20, 95), Size = new Size(70, 20) };
            dtpFechaDesde = new DateTimePicker { Location = new Point(90, 93), Size = new Size(130, 25), Enabled = false, Format = DateTimePickerFormat.Short };

            chkFechaHasta = new CheckBox { Text = "Hasta:", Location = new Point(240, 95), Size = new Size(70, 20) };
            dtpFechaHasta = new DateTimePicker { Location = new Point(310, 93), Size = new Size(130, 25), Enabled = false, Format = DateTimePickerFormat.Short };

            var btnBuscar = new Button { Text = "Buscar", Location = new Point(470, 93), Size = new Size(100, 25), BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            var btnLimpiar = new Button { Text = "Limpiar", Location = new Point(580, 93), Size = new Size(100, 25), FlatStyle = FlatStyle.Flat };

            lblResultado = new Label { Text = string.Empty, Location = new Point(700, 97), Size = new Size(160, 20), ForeColor = Color.FromArgb(0, 122, 204) };

            this.Controls.AddRange(new Control[] { lblVuelo, txtVuelo, lblUsuario, txtUsuario, lblEstado, cmbEstadoLeida, chkFechaDesde, dtpFechaDesde, chkFechaHasta, dtpFechaHasta, btnBuscar, btnLimpiar, lblResultado });

            dgvNotificaciones = new DataGridView
            {
                Location = new Point(20, 130),
                Size = new Size(840, 440),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                RowHeadersVisible = false
            };
            dgvNotificaciones.CellDoubleClick += dgvNotificaciones_CellDoubleClick;
            this.Controls.Add(dgvNotificaciones);

            chkFechaDesde.CheckedChanged += (s, e) => dtpFechaDesde.Enabled = chkFechaDesde.Checked;
            chkFechaHasta.CheckedChanged += (s, e) => dtpFechaHasta.Enabled = chkFechaHasta.Checked;
            btnBuscar.Click += (s, e) => Recargar();
            btnLimpiar.Click += (s, e) =>
            {
                txtVuelo.Text = string.Empty;
                txtUsuario.Text = string.Empty;
                cmbEstadoLeida.SelectedIndex = 0;
                chkFechaDesde.Checked = false;
                chkFechaHasta.Checked = false;
                dtpFechaDesde.Value = DateTime.Today;
                dtpFechaHasta.Value = DateTime.Today;
                Recargar();
            };

            AjusteLayout.AjustarFormulario(this);
        }

        private void Recargar()
        {
            Guid? vueloId = null;
            string numeroVuelo = txtVuelo.Text.Trim();
            if (Guid.TryParse(numeroVuelo, out var vueloParseado))
            {
                vueloId = vueloParseado;
                numeroVuelo = string.Empty;
            }

            Guid? usuarioId = null;
            string emailUsuario = txtUsuario.Text.Trim();
            if (Guid.TryParse(emailUsuario, out var usuarioParseado))
            {
                usuarioId = usuarioParseado;
                emailUsuario = string.Empty;
            }

            bool? leida = null;
            if (cmbEstadoLeida.SelectedIndex == 1)
                leida = true;
            else if (cmbEstadoLeida.SelectedIndex == 2)
                leida = false;

            DateTime? fechaInicio = chkFechaDesde.Checked ? dtpFechaDesde.Value : (DateTime?)null;
            DateTime? fechaFin = null;
            if (chkFechaHasta.Checked)
                fechaFin = dtpFechaHasta.Value.Date.AddDays(1).AddTicks(-1);

            _ = LoadNotificaciones(vueloId, numeroVuelo, usuarioId, emailUsuario, fechaInicio, fechaFin, leida);
        }

        private async Task LoadNotificaciones(Guid? vueloId, string numeroVuelo, Guid? usuarioId, string emailUsuario, DateTime? fechaInicio, DateTime? fechaFin, bool? leida)
        {
            if (_notificacionService == null)
                return;

            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { _ = LoadNotificaciones(vueloId, numeroVuelo, usuarioId, emailUsuario, fechaInicio, fechaFin, leida); });
                return;
            }

            try
            {
                var notificaciones = await _notificacionService.ObtenerNotificacionesAdminAsync(vueloId, numeroVuelo, usuarioId, emailUsuario, fechaInicio, fechaFin, leida);
                var filas = notificaciones?.Select(n => new
                {
                    n.Id,
                    n.VueloId,
                    Vuelo = n.NumeroVuelo,
                    n.UsuarioId,
                    Usuario = n.EmailUsuario,
                    n.Titulo,
                    n.Mensaje,
                    FechaEnvio = n.FechaEnvio.ToString("dd/MM/yyyy HH:mm"),
                    Estado = n.Leida ? "Leída" : "No leída"
                }).ToList();

                dgvNotificaciones.DataSource = filas;

                if (dgvNotificaciones.Columns.Count > 0)
                {
                    if (dgvNotificaciones.Columns.Contains("VueloId"))
                        dgvNotificaciones.Columns["VueloId"].HeaderText = "Vuelo Id";
                    if (dgvNotificaciones.Columns.Contains("UsuarioId"))
                        dgvNotificaciones.Columns["UsuarioId"].HeaderText = "Usuario Id";
                }

                lblResultado.Text = filas != null ? $"{filas.Count} resultado(s)" : string.Empty;
            }
            catch (Exception ex)
            {
                var mensaje = MensajesError.ObtenerMensaje(ex);
                if (InvokeRequired)
                    Invoke((MethodInvoker)delegate { MessageBox.Show(mensaje, "Error al cargar notificaciones", MessageBoxButtons.OK, MessageBoxIcon.Error); });
                else
                    MessageBox.Show(mensaje, "Error al cargar notificaciones", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void dgvNotificaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var row = dgvNotificaciones.Rows[e.RowIndex];
            if (row.Cells["Id"].Value == null)
                return;

            var notificacionId = (Guid)row.Cells["Id"].Value;
            try
            {
                await _notificacionService.MarcarNotificacionLeidaAsync(notificacionId);
                MessageBox.Show("Notificación marcada como leída.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Recargar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(MensajesError.ObtenerMensaje(ex), "Error al marcar como leída", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
