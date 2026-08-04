using System.Drawing;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Vuelos
{
    /// <summary>
    /// Encabezado reutilizable con la tarjeta de resumen del vuelo seleccionado.
    /// Se muestra en los paneles y modales de acción y es de solo lectura.
    /// </summary>
    public sealed class VueloEncabezado : UserControl
    {
        private readonly Label _lblNumeroVuelo;
        private readonly Label _lblAerolinea;
        private readonly Label _lblEstado;
        private readonly Label _lblPuerta;

        public VueloEncabezado()
        {
            BackColor = Color.FromArgb(224, 237, 252);
            BorderStyle = BorderStyle.FixedSingle;
            Size = new Size(410, 78);

            _lblNumeroVuelo = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Location = new Point(12, 8)
            };

            _lblAerolinea = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(55, 65, 81),
                Location = new Point(12, 36)
            };

            _lblEstado = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                Location = new Point(200, 36)
            };

            _lblPuerta = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(16, 122, 87),
                Location = new Point(330, 36)
            };

            Controls.AddRange(new Control[] { _lblNumeroVuelo, _lblAerolinea, _lblEstado, _lblPuerta });
        }

        /// <summary>
        /// Carga la información de contexto del vuelo en el encabezado.
        /// </summary>
        public void Configurar(VueloDTO vuelo)
        {
            _lblNumeroVuelo.Text = $"Vuelo {vuelo.NumeroVuelo}";
            _lblAerolinea.Text = $"Aerolínea: {vuelo.AerolineaNombre}";
            _lblEstado.Text = $"Estado: {vuelo.EstadoActual}";
            _lblPuerta.Text = string.IsNullOrWhiteSpace(vuelo.PuertaEmbarque)
                ? "Puerta: Sin asignación"
                : $"Puerta: {vuelo.PuertaEmbarque}";
        }
    }
}
