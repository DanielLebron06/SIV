using SIV.Presentation.Desktop.Auth;
using SIV.Presentation.Desktop.Catalogos;
using SIV.Presentation.Desktop.Common;
using SIV.Presentation.Desktop.Notificaciones;
using SIV.Presentation.Desktop.Reportes;
using SIV.Presentation.Desktop.Usuarios;
using SIV.Presentation.Desktop.Vuelos;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop
{
    public partial class FrmMain : Form
    {
        private readonly IAuthService _authService;
        private readonly IVueloService _vueloService;
        private readonly ICatalogoService _catalogoService;
        private readonly IUserService _userService;
        private readonly IReporteService _reporteService;
        private readonly INotificacionService _notificacionService;
        private readonly SignalRClient _signalRClient;
        private readonly string _rol;
        private readonly string _nombre;
        private readonly string _token;

        public FrmMain(IAuthService authService, IVueloService vueloService, ICatalogoService catalogoService, IUserService userService, IReporteService reporteService, INotificacionService notificacionService, string token, string rol, string nombre) : this()
        {
            _authService = authService;
            _vueloService = vueloService;
            _catalogoService = catalogoService;
            _userService = userService;
            _reporteService = reporteService;
            _notificacionService = notificacionService;
            _token = token;
            _rol = rol;
            _nombre = nombre;
            _signalRClient = new SignalRClient();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;
            lblUser.Text = $"Usuario: {_nombre} ({_rol})";
            ConfigureSidebarByRole();
            AbrirVistaPorDefecto();
            SetupSignalR();
        }

        public FrmMain()
        {
            InitializeComponent();
        }

        private void ConfigureSidebarByRole()
        {
            if (_vueloService == null || _catalogoService == null || _userService == null || _reporteService == null)
                return;

            int yPos = 50;

            var lblTitle = new Label
            {
                Text = "SIV APP",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };
            pnlSidebar.Controls.Add(lblTitle);

            if (_rol == "Operador" || _rol == "Administrador")
            {
                AddMenuButton("Vuelos", yPos, () => OpenFormInContent(new FrmGestionVuelos(_vueloService, _catalogoService, _signalRClient)));
                yPos += 50;
            }

            if (_rol == "Administrador")
            {
                AddMenuButton("Catalogos", yPos, () => OpenFormInContent(new FrmCatalogos(_catalogoService)));
                yPos += 50;
                AddMenuButton("Usuarios", yPos, () => OpenFormInContent(new FrmUsuariosInternos(_userService)));
                yPos += 50;
            }

            if (_rol == "Administrador" || _rol == "Auditor")
            {
                AddMenuButton("Reportes", yPos, () => OpenFormInContent(new FrmReportes(_reporteService)));
                yPos += 50;
                AddMenuButton("Auditoria Log", yPos, () => OpenFormInContent(new FrmAuditLog(_reporteService)));
                yPos += 50;
                AddMenuButton("Notificaciones", yPos, () => OpenFormInContent(new FrmNotificacionesAdmin(_notificacionService)));
                yPos += 50;
            }

            AddMenuButton("Cerrar Sesion", yPos, () =>
            {
                if (_authService != null)
                    _authService.Logout();
                DialogResult = DialogResult.OK;
                Close();
            }, destacar: true);
        }

        private void AbrirVistaPorDefecto()
        {
            if (_vueloService == null || _catalogoService == null || _reporteService == null)
                return;

            if (_rol == "Operador" || _rol == "Administrador")
            {
                OpenFormInContent(new FrmGestionVuelos(_vueloService, _catalogoService, _signalRClient));
            }
            else if (_rol == "Auditor")
            {
                OpenFormInContent(new FrmAuditLog(_reporteService));
            }
        }

        private void AddMenuButton(string text, int y, Action onClick, bool destacar = false)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(0, y),
                Size = new Size(200, 45),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = destacar ? Color.FromArgb(192, 57, 43) : Color.FromArgb(45, 45, 48),
                Font = new Font("Segoe UI", 10, destacar ? FontStyle.Bold : FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0),
                Margin = destacar ? new Padding(8, 8, 8, 8) : Padding.Empty,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (s, e) => onClick();
            pnlSidebar.Controls.Add(btn);
        }

        private void OpenFormInContent(Form form)
        {
            pnlContent.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(form);
            form.ClientSize = pnlContent.ClientSize;
            form.Show();
        }

        private async void SetupSignalR()
        {
            if (_signalRClient == null)
                return;

            _signalRClient.OnConnectionStatusChanged += status =>
            {
                if (IsHandleCreated)
                {
                    Invoke((MethodInvoker)delegate { lblSignalRStatus.Text = status; });
                }
            };

            await _signalRClient.ConnectAsync(_token);
        }

        protected override async void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (_signalRClient != null)
                await _signalRClient.DisconnectAsync();
        }
    }
}
