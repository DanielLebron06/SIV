using SIV.Presentation.Desktop.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Vuelos
{
    /// <summary>
    /// Diálogo para la primera asignación de puerta de embarque de un vuelo.
    /// Solo se habilita cuando el vuelo no tiene puerta asignada.
    /// </summary>
    public class FrmAsignarPuerta : Form
    {
        private readonly IVueloService _vueloService;
        private readonly VueloDTO _vuelo;
        private readonly TextBox _txtPuerta;
        private readonly Button _btnGuardar;

        public FrmAsignarPuerta(VueloDTO vuelo, IVueloService vueloService) : this()
        {
            _vuelo = vuelo;
            _vueloService = vueloService;
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;
            var encabezado = new VueloEncabezado { Location = new Point(10, 10) };
            encabezado.Configurar(_vuelo);
            Controls.Add(encabezado);
        }

        public FrmAsignarPuerta()
        {
            Text = "Asignar Puerta";
            BackColor = Color.White;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ClientSize = new Size(430, 220);
            MaximizeBox = false;
            MinimizeBox = false;

            var lblPuerta = new Label
            {
                Text = "Puerta de Embarque:",
                Location = new Point(20, 100),
                Size = new Size(180, 20)
            };

            _txtPuerta = new TextBox
            {
                Location = new Point(20, 122),
                Size = new Size(390, 25),
                MaxLength = 10
            };

            _btnGuardar = new Button
            {
                Text = "ASIGNAR PUERTA",
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(20, 165),
                Size = new Size(390, 40)
            };
            _btnGuardar.Click += BtnGuardar_Click;

            Controls.AddRange(new Control[] { lblPuerta, _txtPuerta, _btnGuardar });
        }

        private async void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (_vueloService == null)
                return;

            if (string.IsNullOrWhiteSpace(_txtPuerta.Text))
            {
                MessageBox.Show("La puerta de embarque es obligatoria.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _btnGuardar.Enabled = false;
            try
            {
                await _vueloService.AsignarPuertaInicialAsync(_vuelo.Id, _txtPuerta.Text.Trim());
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(MensajesError.ObtenerMensaje(ex), "Error al asignar puerta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _btnGuardar.Enabled = true;
            }
        }
    }
}
