using SIV.Presentation.Desktop.Services;
using SIV.Presentation.Desktop.Services.Dtos;
using SIV.Presentation.Desktop.Services.Interfaces;
using System;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Formularios.Operativo
{
    public partial class FrmCambiosOperativos : Form
    {
        private readonly IVueloService _vueloService;
        private readonly Guid _vueloId;

        public FrmCambiosOperativos(IVueloService vueloService, Guid vueloId) : this()
        {
            _vueloService = vueloService;
            _vueloId = vueloId;
            cmbEstado.Items.AddRange(new object[]
            {
                "Programado", "Retrasado", "Embarcando", "EnVuelo", "Aterrizado", "Completado", "Cancelado"
            });
            cmbTipo.Items.AddRange(new object[] { "Retraso", "Adelanto" });
        }

        public FrmCambiosOperativos()
        {
            InitializeComponent();
        }

        private async void BtnActualizarEstado_Click(object sender, EventArgs e)
        {
            if (_vueloService == null)
                return;

            if (cmbEstado.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un estado.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var estadoNombre = cmbEstado.SelectedItem.ToString();
            if (estadoNombre == "Cancelado" && string.IsNullOrWhiteSpace(txtMotivoEstado.Text))
            {
                MessageBox.Show("El motivo es obligatorio para cancelar un vuelo.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnActualizarEstado.Enabled = false;
            try
            {
                if (estadoNombre == "Cancelado")
                {
                    await _vueloService.CancelarVueloAsync(_vueloId, txtMotivoEstado.Text.Trim());
                }
                else
                {
                    Enum.TryParse<EstadoVuelo>(estadoNombre, out var nuevoEstado);
                    await _vueloService.ActualizarEstadoAsync(_vueloId, nuevoEstado);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(MensajesError.ObtenerMensaje(ex), "Error al actualizar estado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnActualizarEstado.Enabled = true;
            }
        }

        private async void BtnGuardarTiempo_Click(object sender, EventArgs e)
        {
            if (_vueloService == null)
                return;

            if (cmbTipo.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtMotivoTiempo.Text))
            {
                MessageBox.Show("Complete el tipo y el motivo.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnGuardarTiempo.Enabled = false;
            try
            {
                var motivo = txtMotivoTiempo.Text.Trim();
                if (cmbTipo.SelectedItem.ToString() == "Retraso")
                    await _vueloService.RegistrarRetrasoAsync(_vueloId, dtpHora.Value, motivo);
                else
                    await _vueloService.RegistrarAdelantoAsync(_vueloId, dtpHora.Value, motivo);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(MensajesError.ObtenerMensaje(ex), "Error al registrar cambio de tiempo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnGuardarTiempo.Enabled = true;
            }
        }

        private async void BtnGuardarPuerta_Click(object sender, EventArgs e)
        {
            if (_vueloService == null)
                return;

            if (string.IsNullOrWhiteSpace(txtPuerta.Text) || string.IsNullOrWhiteSpace(txtMotivoPuerta.Text))
            {
                MessageBox.Show("Complete la nueva puerta y el motivo.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnGuardarPuerta.Enabled = false;
            try
            {
                await _vueloService.RegistrarCambioPuertaAsync(_vueloId, txtPuerta.Text.Trim(), txtMotivoPuerta.Text.Trim());
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(MensajesError.ObtenerMensaje(ex), "Error al registrar cambio de puerta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnGuardarPuerta.Enabled = true;
            }
        }
    }
}
