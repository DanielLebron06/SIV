using SIV.Presentation.Desktop.Services;
using SIV.Presentation.Desktop.Services.Dtos;
using SIV.Presentation.Desktop.Services.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Formularios.Administrativo
{
    public class FrmNotificacionesAdmin : Form
    {
        private readonly IUserService _userService;
        private DataGridView dgvNotificaciones;

        public FrmNotificacionesAdmin(IUserService userService) : this()
        {
            _userService = userService;
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;
            LoadNotificaciones();
        }

        public FrmNotificacionesAdmin()
        {
            InitializeComponentProgrammatic();
        }

        private void InitializeComponentProgrammatic()
        {
            this.Text = "Auditoría de Notificaciones";
            this.BackColor = Color.White;

            var lblTitle = new Label { Text = "Monitor de Notificaciones Enviadas", Font = new Font("Segoe UI", 16, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true };
            this.Controls.Add(lblTitle);

            var lblFiltro = new Label { Text = "Filtrar por Vuelo o Evento:", Location = new Point(20, 60), Size = new Size(180, 20) };
            var txtFiltro = new TextBox { Location = new Point(20, 80), Size = new Size(300, 25) };
            var btnBuscar = new Button { Text = "Buscar", Location = new Point(330, 80), Size = new Size(100, 25), BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };

            this.Controls.Add(lblFiltro);
            this.Controls.Add(txtFiltro);
            this.Controls.Add(btnBuscar);

            dgvNotificaciones = new DataGridView
            {
                Location = new Point(20, 120),
                Size = new Size(740, 440),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                RowHeadersVisible = false
            };
            this.Controls.Add(dgvNotificaciones);
        }

        private async void LoadNotificaciones()
        {
            if (_userService == null)
                return;

            try
            {
                var notificaciones = await _userService.ObtenerNotificacionesAsync();
                if (notificaciones != null)
                {
                    dgvNotificaciones.DataSource = notificaciones;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(MensajesError.ObtenerMensaje(ex), "Error al cargar notificaciones", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
