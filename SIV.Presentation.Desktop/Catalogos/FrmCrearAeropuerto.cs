using SIV.Presentation.Desktop.Common;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Catalogos
{
    public partial class FrmCrearAeropuerto : Form
    {
        private readonly ICatalogoService _catalogoService;
        private static readonly Regex IataRegex = new Regex(@"^[A-Z]{3}$", RegexOptions.Compiled);

        public FrmCrearAeropuerto(ICatalogoService catalogoService) : this()
        {
            _catalogoService = catalogoService;
        }

        public FrmCrearAeropuerto()
        {
            InitializeComponent();
        }

        private async void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (_catalogoService == null)
                return;

            var codigo = txtCodigoIATA.Text.Trim().ToUpperInvariant();
            var nombre = txtNombre.Text.Trim();
            var ciudad = txtCiudad.Text.Trim();
            var pais = txtPais.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(codigo) ||
                string.IsNullOrWhiteSpace(ciudad) || string.IsNullOrWhiteSpace(pais))
            {
                MessageBox.Show("Complete todos los campos.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (codigo.Length != 3)
            {
                MessageBox.Show("El codigo IATA debe tener 3 caracteres.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnGuardar.Enabled = false;
            try
            {
                var dto = new RegistroAeropuertoDTO
                {
                    Nombre = nombre,
                    CodigoIATA = codigo,
                    Ciudad = ciudad,
                    Pais = pais
                };
                await _catalogoService.RegistrarAeropuertoAsync(dto);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(MensajesError.ObtenerMensaje(ex), "Error al crear aeropuerto", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
