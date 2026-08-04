using SIV.Presentation.Desktop.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Vuelos
{
    public partial class FrmCambiosOperativos : Form
    {
        private readonly IVueloService _vueloService;
        private readonly VueloDTO _vuelo;

        public FrmCambiosOperativos(VueloDTO vuelo, IVueloService vueloService) : this()
        {
            _vueloService = vueloService;
            _vuelo = vuelo;
            cmbEstado.Items.AddRange(new object[]
            {
                "Programado", "Retrasado", "Embarcando", "EnVuelo", "Aterrizado", "Completado", "Cancelado"
            });
            cmbTipo.Items.AddRange(new object[] { "Retraso", "Adelanto" });

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;

            var encabezado = new VueloEncabezado { Location = new Point(10, 10) };
            encabezado.Configurar(_vuelo);
            Controls.Add(encabezado);

            if (string.IsNullOrEmpty(_vuelo.PuertaEmbarque))
            {
                tabControl.TabPages.Remove(tabPuerta);
            }
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
                    await _vueloService.CancelarVueloAsync(_vuelo.Id, txtMotivoEstado.Text.Trim());
                }
                else
                {
                    Enum.TryParse<EstadoVuelo>(estadoNombre, out var nuevoEstado);
                    await _vueloService.ActualizarEstadoAsync(_vuelo.Id, nuevoEstado);
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
                    await _vueloService.RegistrarRetrasoAsync(_vuelo.Id, dtpHora.Value, motivo);
                else
                    await _vueloService.RegistrarAdelantoAsync(_vuelo.Id, dtpHora.Value, motivo);

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
                await _vueloService.RegistrarCambioPuertaAsync(_vuelo.Id, txtPuerta.Text.Trim(), txtMotivoPuerta.Text.Trim());
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
