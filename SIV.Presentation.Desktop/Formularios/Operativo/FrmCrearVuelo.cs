using SIV.Presentation.Desktop.Services;
using SIV.Presentation.Desktop.Services.Dtos;
using SIV.Presentation.Desktop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Formularios.Operativo
{
    public partial class FrmCrearVuelo : Form
    {
        private readonly ICatalogoService _catalogoService;
        private readonly IVueloService _vueloService;
        private static readonly Regex IataRegex = new Regex(@"^[A-Z0-9]{2}[0-9]{1,4}$", RegexOptions.Compiled);

        public FrmCrearVuelo(ICatalogoService catalogoService, IVueloService vueloService) : this()
        {
            _catalogoService = catalogoService;
            _vueloService = vueloService;
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;
            LoadCombos();
        }

        public FrmCrearVuelo()
        {
            InitializeComponent();
        }

        private async void LoadCombos()
        {
            if (_catalogoService == null)
                return;

            try
            {
                var aerolineas = await _catalogoService.ObtenerAerolineasAsync();
                if (aerolineas != null)
                {
                    cmbAerolinea.DataSource = aerolineas;
                    cmbAerolinea.DisplayMember = "Nombre";
                    cmbAerolinea.ValueMember = "Id";
                }

                var aeropuertos = await _catalogoService.ObtenerAeropuertosAsync();
                if (aeropuertos != null)
                {
                    cmbOrigen.DataSource = new List<AeropuertoDTO>(aeropuertos);
                    cmbOrigen.DisplayMember = "Nombre";
                    cmbOrigen.ValueMember = "Id";

                    cmbDestino.DataSource = new List<AeropuertoDTO>(aeropuertos);
                    cmbDestino.DisplayMember = "Nombre";
                    cmbDestino.ValueMember = "Id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(MensajesError.ObtenerMensaje(ex), "Error al cargar catalogos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (_catalogoService == null || _vueloService == null)
                return;

            var numeroVuelo = txtNumeroVuelo.Text.Trim().ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(numeroVuelo) || cmbAerolinea.SelectedValue == null || cmbOrigen.SelectedValue == null || cmbDestino.SelectedValue == null)
            {
                MessageBox.Show("Complete todos los campos requeridos.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IataRegex.IsMatch(numeroVuelo))
            {
                MessageBox.Show("El numero de vuelo debe cumplir el formato IATA (ej: AA1234).", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if ((Guid)cmbOrigen.SelectedValue == (Guid)cmbDestino.SelectedValue)
            {
                MessageBox.Show("El origen y el destino no pueden ser iguales.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtpLlegada.Value <= dtpSalida.Value)
            {
                MessageBox.Show("La llegada debe ser posterior a la salida.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnGuardar.Enabled = false;

            try
            {
                var dto = new DatosVueloDTO
                {
                    NumeroVuelo = numeroVuelo,
                    AerolineaId = (Guid)cmbAerolinea.SelectedValue,
                    AeropuertoOrigenId = (Guid)cmbOrigen.SelectedValue,
                    AeropuertoDestinoId = (Guid)cmbDestino.SelectedValue,
                    FechaSalidaProgramada = dtpSalida.Value,
                    FechaLlegadaProgramada = dtpLlegada.Value
                };

                await _vueloService.CrearVueloAsync(dto);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(MensajesError.ObtenerMensaje(ex), "Error al guardar el vuelo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnGuardar.Enabled = true;
            }
        }
    }
}
