using SIV.Presentation.Desktop.Services;
using SIV.Presentation.Desktop.Services.Dtos;
using SIV.Presentation.Desktop.Services.Interfaces;
using System;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Formularios
{
    public partial class FrmLogin : Form
    {
        private readonly IAuthService _authService;

        public string TokenRespuesta { get; private set; } = string.Empty;
        public string RolUsuario { get; private set; } = string.Empty;
        public string NombreUsuario { get; private set; } = string.Empty;

        public FrmLogin(IAuthService authService) : this()
        {
            _authService = authService;
        }

        public FrmLogin()
        {
            InitializeComponent();
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            if (_authService == null)
                return;

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblError.Text = "Ingrese correo y contraseña.";
                return;
            }

            btnLogin.Enabled = false;
            lblError.Text = "Autenticando...";

            try
            {
                var request = new LoginDTO { Email = txtEmail.Text.Trim(), Password = txtPassword.Text };
                var response = await _authService.LoginAsync(request);

                if (response != null && !string.IsNullOrEmpty(response.Token))
                {
                    TokenRespuesta = response.Token;
                    RolUsuario = JwtHelper.GetRole(TokenRespuesta);
                    NombreUsuario = JwtHelper.GetEmail(TokenRespuesta);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    lblError.Text = "Credenciales incorrectas.";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = MensajesError.ObtenerMensaje(ex);
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }
    }
}
