using SIV.Presentation.Desktop.Services;
using SIV.Presentation.Desktop.Services.Dtos;
using SIV.Presentation.Desktop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Formularios.Administrativo
{
    public partial class FrmUsuariosInternos : Form
    {
        private readonly IUserService _userService;

        public FrmUsuariosInternos(IUserService userService) : this()
        {
            _userService = userService;
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;
            LoadUsuarios();
        }

        public FrmUsuariosInternos()
        {
            InitializeComponent();
        }

        private async void LoadUsuarios()
        {
            if (_userService == null)
                return;

            try
            {
                var usuarios = await _userService.ObtenerUsuariosInternosAsync();
                if (usuarios != null)
                {
                    dgvUsuarios.DataSource = usuarios;
                    if (dgvUsuarios.Columns["Rol"] != null)
                        dgvUsuarios.Columns["Rol"].Visible = false;
                    if (dgvUsuarios.Columns["RolNombre"] == null && dgvUsuarios.Columns.Count > 0)
                    {
                        var col = new DataGridViewTextBoxColumn
                        {
                            Name = "RolNombre",
                            HeaderText = "Rol",
                            ReadOnly = true
                        };
                        dgvUsuarios.Columns.Add(col);
                    }
                    foreach (DataGridViewRow row in dgvUsuarios.Rows)
                    {
                        if (row.Cells["Rol"].Value != null)
                            row.Cells["RolNombre"].Value = row.Cells["Rol"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(MensajesError.ObtenerMensaje(ex), "Error al cargar usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            if (_userService == null)
                return;

            using (var frm = new FrmCrearUsuarioInternal(_userService))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    LoadUsuarios();
            }
        }

        private async void BtnDesactivar_Click(object sender, EventArgs e)
        {
            if (_userService == null)
                return;

            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var id = (Guid)dgvUsuarios.SelectedRows[0].Cells["Id"].Value;
            var activo = (bool)dgvUsuarios.SelectedRows[0].Cells["Activo"].Value;
            var accion = activo ? "desactivar" : "desactivar";

            if (MessageBox.Show($"Confirma {accion} este usuario?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                await _userService.DesactivarUsuarioAsync(id);
                LoadUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show(MensajesError.ObtenerMensaje(ex), "Error al actualizar usuario", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
