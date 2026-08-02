using SIV.Presentation.Desktop.Services;
using SIV.Presentation.Desktop.Services.Dtos;
using SIV.Presentation.Desktop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Formularios.Administrativo
{
    public partial class FrmCatalogos : Form
    {
        private readonly ICatalogoService _catalogoService;

        public FrmCatalogos(ICatalogoService catalogoService) : this()
        {
            _catalogoService = catalogoService;
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;
            LoadCatalogos();
        }

        public FrmCatalogos()
        {
            InitializeComponent();
        }

        private async void LoadCatalogos()
        {
            if (_catalogoService == null)
                return;

            try
            {
                var aerolineas = await _catalogoService.ObtenerAerolineasAsync();
                dgvAerolineas.DataSource = aerolineas ?? new List<AerolineaDTO>();

                var aeropuertos = await _catalogoService.ObtenerAeropuertosAsync();
                dgvAeropuertos.DataSource = aeropuertos ?? new List<AeropuertoDTO>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(MensajesError.ObtenerMensaje(ex), "Error al cargar catalogos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNuevaAerolinea_Click(object sender, EventArgs e)
        {
            if (_catalogoService == null)
                return;

            using (var frm = new FrmCrearAerolinea(_catalogoService))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    LoadCatalogos();
            }
        }

        private async void BtnEliminarAerolinea_Click(object sender, EventArgs e)
        {
            if (_catalogoService == null)
                return;

            if (dgvAerolineas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una aerolinea.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var id = (Guid)dgvAerolineas.SelectedRows[0].Cells["Id"].Value;
            if (MessageBox.Show("Confirma desactivar esta aerolinea?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                await _catalogoService.DesactivarAerolineaAsync(id);
                LoadCatalogos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(MensajesError.ObtenerMensaje(ex), "Error al desactivar aerolinea", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNuevoAeropuerto_Click(object sender, EventArgs e)
        {
            if (_catalogoService == null)
                return;

            using (var frm = new FrmCrearAeropuerto(_catalogoService))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    LoadCatalogos();
            }
        }

        private async void BtnDesactivarAeropuerto_Click(object sender, EventArgs e)
        {
            if (_catalogoService == null)
                return;

            if (dgvAeropuertos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un aeropuerto.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var id = (Guid)dgvAeropuertos.SelectedRows[0].Cells["Id"].Value;
            if (MessageBox.Show("Confirma desactivar este aeropuerto?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                await _catalogoService.DesactivarAeropuertoAsync(id);
                LoadCatalogos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(MensajesError.ObtenerMensaje(ex), "Error al desactivar aeropuerto", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
