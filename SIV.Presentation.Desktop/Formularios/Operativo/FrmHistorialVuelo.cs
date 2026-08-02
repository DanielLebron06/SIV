using SIV.Presentation.Desktop.Services;
using SIV.Presentation.Desktop.Services.Dtos;
using SIV.Presentation.Desktop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Formularios.Operativo
{
    public partial class FrmHistorialVuelo : Form
    {
        private readonly IVueloService _vueloService;
        private readonly Guid _vueloId;

        public FrmHistorialVuelo(IVueloService vueloService, Guid vueloId) : this()
        {
            _vueloService = vueloService;
            _vueloId = vueloId;
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;
            LoadHistorial();
        }

        public FrmHistorialVuelo()
        {
            InitializeComponent();
        }

        private async void LoadHistorial()
        {
            if (_vueloService == null)
                return;

            try
            {
                var cambios = await _vueloService.ObtenerHistorialCambiosAsync(_vueloId);
                lstHistorial.Items.Clear();
                if (cambios == null || cambios.Count == 0)
                {
                    lstHistorial.Items.Add("No hay cambios registrados para este vuelo.");
                    return;
                }

                foreach (var c in cambios.OrderByDescending(x => x.Timestamp))
                {
                    lstHistorial.Items.Add($"[{c.Timestamp:dd/MM/yyyy HH:mm}] {c.TipoCambio} - {c.Motivo}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(MensajesError.ObtenerMensaje(ex), "Error al cargar el historial", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
