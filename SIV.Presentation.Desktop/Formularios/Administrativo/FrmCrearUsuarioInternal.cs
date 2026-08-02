using SIV.Presentation.Desktop.Services;
using SIV.Presentation.Desktop.Services.Dtos;
using SIV.Presentation.Desktop.Services.Interfaces;
using System;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Formularios.Administrativo
{
    public partial class FrmCrearUsuarioInternal : Form
    {
        private readonly IUserService _userService;

        public FrmCrearUsuarioInternal(IUserService userService) : this()
        {
            _userService = userService;
            cmbRol.Items.AddRange(new object[] { "Operador", "Administrador", "Auditor" });
            cmbRol.SelectedIndex = 0;
        }

        public FrmCrearUsuarioInternal()
        {
            InitializeComponent();
        }

        private async void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (_userService == null)
                return;

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) || cmbRol.SelectedIndex == -1)
            {
                MessageBox.Show("Complete todos los campos.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnGuardar.Enabled = false;
            try
            {
                var rol = Rol.Operador;
                switch (cmbRol.SelectedItem.ToString())
                {
                    case "Administrador": rol = Rol.Administrador; break;
                    case "Auditor": rol = Rol.Auditor; break;
                }

                var dto = new RegistroUsuarioInternoDTO
                {
                    Email = txtEmail.Text.Trim(),
                    Password = txtPassword.Text,
                    Rol = rol
                };

                await _userService.RegistrarUsuarioInternoAsync(dto);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(MensajesError.ObtenerMensaje(ex), "Error al crear usuario", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnGuardar.Enabled = true;
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
