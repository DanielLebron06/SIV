using SIV.Presentation.Desktop.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Vuelos
{
    public partial class FrmHistorialVuelo : Form
    {
        private readonly IVueloService _vueloService;
        private readonly VueloDTO _vuelo;

        public FrmHistorialVuelo(VueloDTO vuelo, IVueloService vueloService) : this()
        {
            _vueloService = vueloService;
            _vuelo = vuelo;
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;
            var encabezado = new VueloEncabezado { Location = new Point(10, 10) };
            encabezado.Configurar(_vuelo);
            Controls.Add(encabezado);
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

            if (InvokeRequired)
            {
                BeginInvoke(new Action(LoadHistorial));
                return;
            }

            try
            {
                var cambios = await _vueloService.ObtenerHistorialCambiosAsync(_vuelo.Id);
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
                var mensaje = MensajesError.ObtenerMensaje(ex);
                if (InvokeRequired)
                    Invoke((MethodInvoker)delegate { MessageBox.Show(mensaje, "Error al cargar el historial", MessageBoxButtons.OK, MessageBoxIcon.Error); });
                else
                    MessageBox.Show(mensaje, "Error al cargar el historial", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
