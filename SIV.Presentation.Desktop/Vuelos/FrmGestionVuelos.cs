using SIV.Presentation.Desktop.Catalogos;
using SIV.Presentation.Desktop.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Vuelos
{
    public partial class FrmGestionVuelos : Form
    {
        private readonly IVueloService _vueloService;
        private readonly ICatalogoService _catalogoService;
        private readonly SignalRClient _signalRClient;
        private List<VueloDTO> _vuelosCargados = new List<VueloDTO>();

        public FrmGestionVuelos(IVueloService vueloService, ICatalogoService catalogoService, SignalRClient signalRClient) : this()
        {
            _vueloService = vueloService;
            _catalogoService = catalogoService;
            _signalRClient = signalRClient;
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;
            AjusteLayout.AjustarFormulario(this);
            cmbEstadoFiltro.Items.Add("Todos");
            cmbEstadoFiltro.Items.AddRange(new object[]
            {
                "Programado", "Retrasado", "Embarcando", "EnVuelo", "Aterrizado", "Completado", "Cancelado"
            });
            cmbEstadoFiltro.SelectedIndex = 0;
            cmbAerolinea.Items.Add("Todas");
            cmbAerolinea.SelectedIndex = 0;
            cmbOrigen.Items.Add("Todos");
            cmbOrigen.SelectedIndex = 0;
            cmbDestino.Items.Add("Todos");
            cmbDestino.SelectedIndex = 0;
            _signalRClient.OnVuelosUpdated += LoadVuelos;
            dgvVuelos.SelectionChanged += DgvVuelos_SelectionChanged;
            btnAsignarPuerta.Enabled = false;
            CargarAerolineas();
            CargarAeropuertos();
            LoadVuelos();
        }

        public FrmGestionVuelos()
        {
            InitializeComponent();
        }

        private async void CargarAerolineas()
        {
            if (_catalogoService == null)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(CargarAerolineas));
                return;
            }

            try
            {
                var aerolineas = await _catalogoService.ObtenerAerolineasAsync();
                if (aerolineas == null)
                    return;

                var seleccionada = cmbAerolinea.SelectedItem?.ToString();
                cmbAerolinea.Items.Clear();
                cmbAerolinea.Items.Add("Todas");
                foreach (var a in aerolineas.OrderBy(x => x.Nombre))
                {
                    cmbAerolinea.Items.Add(a.Nombre);
                }
                cmbAerolinea.SelectedItem = seleccionada ?? "Todas";
                if (cmbAerolinea.SelectedIndex == -1)
                    cmbAerolinea.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                var mensaje = MensajesError.ObtenerMensaje(ex);
                if (InvokeRequired)
                    Invoke((MethodInvoker)delegate { MessageBox.Show(mensaje, "Error al cargar aerolineas", MessageBoxButtons.OK, MessageBoxIcon.Error); });
                else
                    MessageBox.Show(mensaje, "Error al cargar aerolineas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void CargarAeropuertos()
        {
            if (_catalogoService == null)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(CargarAeropuertos));
                return;
            }

            try
            {
                var aeropuertos = await _catalogoService.ObtenerAeropuertosAsync();
                if (aeropuertos == null)
                    return;

                var seleccionadoOrigen = cmbOrigen.SelectedItem?.ToString();
                var seleccionadoDestino = cmbDestino.SelectedItem?.ToString();

                cmbOrigen.Items.Clear();
                cmbDestino.Items.Clear();
                cmbOrigen.Items.Add("Todos");
                cmbDestino.Items.Add("Todos");
                foreach (var a in aeropuertos.OrderBy(x => x.Nombre))
                {
                    var etiqueta = $"{a.CodigoIATA} - {a.Nombre}";
                    cmbOrigen.Items.Add(etiqueta);
                    cmbDestino.Items.Add(etiqueta);
                }

                cmbOrigen.SelectedItem = seleccionadoOrigen ?? "Todos";
                cmbDestino.SelectedItem = seleccionadoDestino ?? "Todos";
                if (cmbOrigen.SelectedIndex == -1)
                    cmbOrigen.SelectedIndex = 0;
                if (cmbDestino.SelectedIndex == -1)
                    cmbDestino.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                var mensaje = MensajesError.ObtenerMensaje(ex);
                if (InvokeRequired)
                    Invoke((MethodInvoker)delegate { MessageBox.Show(mensaje, "Error al cargar aeropuertos", MessageBoxButtons.OK, MessageBoxIcon.Error); });
                else
                    MessageBox.Show(mensaje, "Error al cargar aeropuertos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void LoadVuelos()
        {
            if (_vueloService == null)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(LoadVuelos));
                return;
            }

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

                if (cmbAerolinea.SelectedIndex > 0 && cmbAerolinea.SelectedItem != null)
                {
                    var aerolineaFiltro = cmbAerolinea.SelectedItem.ToString();
                    vuelos = vuelos.Where(v => string.Equals(v.AerolineaNombre, aerolineaFiltro, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (cmbOrigen.SelectedIndex > 0 && cmbOrigen.SelectedItem != null)
                {
                    var filtroOrigen = ObtenerIATA(cmbOrigen.SelectedItem.ToString());
                    if (!string.IsNullOrEmpty(filtroOrigen))
                        vuelos = vuelos.Where(v => string.Equals(v.AeropuertoOrigenIATA, filtroOrigen, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (cmbDestino.SelectedIndex > 0 && cmbDestino.SelectedItem != null)
                {
                    var filtroDestino = ObtenerIATA(cmbDestino.SelectedItem.ToString());
                    if (!string.IsNullOrEmpty(filtroDestino))
                        vuelos = vuelos.Where(v => string.Equals(v.AeropuertoDestinoIATA, filtroDestino, StringComparison.OrdinalIgnoreCase)).ToList();
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

        private static string ObtenerIATA(string etiqueta)
        {
            if (string.IsNullOrWhiteSpace(etiqueta))
                return null;

            var partes = etiqueta.Split(new[] { " - " }, StringSplitOptions.None);
            return partes.Length > 0 ? partes[0].Trim() : etiqueta.Trim();
        }

        private void BindGrid(List<VueloDTO> vuelos)
        {
            _vuelosCargados = vuelos;
            ActualizarEstadoBotones();
            dgvVuelos.DataSource = vuelos.Select(v => new
            {
                v.Id,
                v.NumeroVuelo,
                Aerolinea = v.AerolineaNombre,
                Origen = v.AeropuertoOrigenIATA,
                Destino = v.AeropuertoDestinoIATA,
                Puerta = string.IsNullOrWhiteSpace(v.PuertaEmbarque) ? "-" : v.PuertaEmbarque,
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

        private void BtnAsignarPuerta_Click(object sender, EventArgs e)
        {
            if (_vueloService == null)
                return;

            var vuelo = ObtenerVueloSeleccionado();
            if (vuelo == null)
            {
                MessageBox.Show("Seleccione un vuelo.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrEmpty(vuelo.PuertaEmbarque))
            {
                MessageBox.Show("El vuelo ya tiene una puerta asignada.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var frm = new FrmAsignarPuerta(vuelo, _vueloService))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    LoadVuelos();
            }
        }

        private void DgvVuelos_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarEstadoBotones();
        }

        private VueloDTO ObtenerVueloSeleccionado()
        {
            if (dgvVuelos.SelectedRows.Count == 0)
                return null;

            var id = (Guid)dgvVuelos.SelectedRows[0].Cells["Id"].Value;
            return _vuelosCargados.FirstOrDefault(v => v.Id == id);
        }

        private void ActualizarEstadoBotones()
        {
            var vuelo = ObtenerVueloSeleccionado();
            btnAsignarPuerta.Enabled = vuelo != null && string.IsNullOrEmpty(vuelo.PuertaEmbarque);
        }

        private void BtnCambiosOperativos_Click(object sender, EventArgs e)
        {
            if (_vueloService == null)
                return;

            var vuelo = ObtenerVueloSeleccionado();
            if (vuelo == null)
            {
                MessageBox.Show("Seleccione un vuelo.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var frm = new FrmCambiosOperativos(vuelo, _vueloService))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    LoadVuelos();
            }
        }

        private void BtnHistorial_Click(object sender, EventArgs e)
        {
            if (_vueloService == null)
                return;

            var vuelo = ObtenerVueloSeleccionado();
            if (vuelo == null)
            {
                MessageBox.Show("Seleccione un vuelo.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var frm = new FrmHistorialVuelo(vuelo, _vueloService))
            {
                frm.ShowDialog();
            }
        }
    }
}
