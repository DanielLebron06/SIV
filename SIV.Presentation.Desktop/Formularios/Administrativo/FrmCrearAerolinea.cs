using SIV.Presentation.Desktop.Services;
using SIV.Presentation.Desktop.Services.Dtos;
using SIV.Presentation.Desktop.Services.Interfaces;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Formularios.Administrativo
{
    public partial class FrmCrearAerolinea : Form
    {
        private readonly ICatalogoService _catalogoService;
        private static readonly Regex IataRegex = new Regex(@"^[A-Z]{2}$|^[A-Z0-9]{2}$", RegexOptions.Compiled);

        public FrmCrearAerolinea(ICatalogoService catalogoService) : this()
        {
            _catalogoService = catalogoService;
        }

        public FrmCrearAerolinea()
        {
            InitializeComponent();
        }

        private async void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (_catalogoService == null)
                return;

            var codigo = txtCodigoIATA.Text.Trim().ToUpperInvariant();
            var nombre = txtNombre.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(codigo))
            {
                MessageBox.Show("Complete todos los campos.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (codigo.Length != 2)
            {
                MessageBox.Show("El codigo IATA debe tener 2 caracteres.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnGuardar.Enabled = false;
            try
            {
                var dto = new RegistroAerolineaDTO { Nombre = nombre, CodigoIATA = codigo };
                await _catalogoService.RegistrarAerolineaAsync(dto);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(MensajesError.ObtenerMensaje(ex), "Error al crear aerolinea", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
