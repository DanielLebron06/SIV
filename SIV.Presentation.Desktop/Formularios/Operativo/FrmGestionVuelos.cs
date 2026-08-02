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
    public partial class FrmGestionVuelos : Form
    {
        private readonly IVueloService _vueloService;
        private readonly ICatalogoService _catalogoService;
        private readonly SignalRClient _signalRClient;

        public FrmGestionVuelos(IVueloService vueloService, ICatalogoService catalogoService, SignalRClient signalRClient) : this()
        {
            _vueloService = vueloService;
            _catalogoService = catalogoService;
            _signalRClient = signalRClient;
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;
            cmbEstadoFiltro.Items.Add("Todos");
            cmbEstadoFiltro.Items.AddRange(new object[]
            {
                "Programado", "Retrasado", "Embarcando", "EnVuelo", "Aterrizado", "Completado", "Cancelado"
            });
            cmbEstadoFiltro.SelectedIndex = 0;
            _signalRClient.OnVuelosUpdated += LoadVuelos;
            LoadVuelos();
        }

        public FrmGestionVuelos()
        {
            InitializeComponent();
        }

        private async void LoadVuelos()
        {
            if (_vueloService == null)
                return;

            try
            {
                FiltrosVuelos filtros = null;
                if (cmbEstadoFiltro.SelectedIndex > 0)
                {
                    Enum.TryParse<EstadoVuelo>(cmbEstadoFiltro.SelectedItem.ToString(), out var estado);
                    filtros = new FiltrosVuelos { Estado = estado };
                }

                var vuelos = await _vueloService.ObtenerVuelosAsync(filtros);
                if (vuelos == null)
                    return;

                if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
                {
                    var filtro = txtBuscar.Text.Trim().ToUpperInvariant();
                    vuelos = vuelos.Where(v =>
                        v.NumeroVuelo.ToUpperInvariant().Contains(filtro) ||
                        v.AeropuertoOrigenIATA.ToUpperInvariant().Contains(filtro) ||
                        v.AeropuertoDestinoIATA.ToUpperInvariant().Contains(filtro) ||
                        v.AerolineaNombre.ToUpperInvariant().Contains(filtro)).ToList();
                }

                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate { BindGrid(vuelos); });
                }
                else
                {
                    BindGrid(vuelos);
                }
            }
            catch (Exception ex)
            {
                var mensaje = MensajesError.ObtenerMensaje(ex);
                if (InvokeRequired)
                    Invoke((MethodInvoker)delegate { MessageBox.Show(mensaje, "Error al cargar vuelos", MessageBoxButtons.OK, MessageBoxIcon.Error); });
                else
                    MessageBox.Show(mensaje, "Error al cargar vuelos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindGrid(List<VueloDTO> vuelos)
        {
            dgvVuelos.DataSource = vuelos.Select(v => new
            {
                v.Id,
                v.NumeroVuelo,
                Aerolinea = v.AerolineaNombre,
                Origen = v.AeropuertoOrigenIATA,
                Destino = v.AeropuertoDestinoIATA,
                Estado = v.EstadoActual.ToString(),
                Salida = v.FechaSalidaProgramada.ToString("dd/MM/yyyy HH:mm"),
                Llegada = v.FechaLlegadaProgramada.ToString("dd/MM/yyyy HH:mm")
            }).ToList();
        }

        private void BtnFiltrar_Click(object sender, EventArgs e)
        {
            LoadVuelos();
        }

        private void BtnCrearVuelo_Click(object sender, EventArgs e)
        {
            if (_catalogoService == null || _vueloService == null)
                return;

            using (var frm = new FrmCrearVuelo(_catalogoService, _vueloService))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    LoadVuelos();
            }
        }

        private void BtnCambiosOperativos_Click(object sender, EventArgs e)
        {
            if (_vueloService == null)
                return;

            if (dgvVuelos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un vuelo.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var id = (Guid)dgvVuelos.SelectedRows[0].Cells["Id"].Value;
            using (var frm = new FrmCambiosOperativos(_vueloService, id))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    LoadVuelos();
            }
        }

        private void BtnHistorial_Click(object sender, EventArgs e)
        {
            if (_vueloService == null)
                return;

            if (dgvVuelos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un vuelo.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var id = (Guid)dgvVuelos.SelectedRows[0].Cells["Id"].Value;
            using (var frm = new FrmHistorialVuelo(_vueloService, id))
            {
                frm.ShowDialog();
            }
        }
    }
}
