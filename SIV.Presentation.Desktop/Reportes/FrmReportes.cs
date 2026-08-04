using SIV.Presentation.Desktop.Common;
using SIV.Presentation.Desktop.Vuelos;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Reportes
{
    public class FrmReportes : Form
    {
        private readonly IReporteService _reporteService;
        private TabControl tabControl;
        private DateTimePicker dtpDesde;
        private DateTimePicker dtpHasta;
        private DataGridView dgvOperacion;
        private DataGridView dgvCambios;
        private DataGridView dgvSeguimiento;

        public FrmReportes(IReporteService reporteService) : this()
        {
            _reporteService = reporteService;
        }

        public FrmReportes()
        {
            InitializeComponentProgrammatic();
        }

        private void InitializeComponentProgrammatic()
        {
            this.Text = "Dashboard de Reportes Operativos";
            this.BackColor = Color.White;
            this.MinimumSize = new System.Drawing.Size(1024, 600);

            var lblTitle = new Label { Text = "Reportes y Estadísticas", Font = new Font("Segoe UI", 16, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true };
            this.Controls.Add(lblTitle);

            var lblDesde = new Label { Text = "Desde:", Location = new Point(20, 60), Size = new Size(50, 20) };
            dtpDesde = new DateTimePicker { Location = new Point(70, 60), Size = new Size(120, 25), Format = DateTimePickerFormat.Short };

            var lblHasta = new Label { Text = "Hasta:", Location = new Point(210, 60), Size = new Size(50, 20) };
            dtpHasta = new DateTimePicker { Location = new Point(260, 60), Size = new Size(120, 25), Format = DateTimePickerFormat.Short };

            var btnGenerar = new Button { Text = "Generar Reporte", Location = new Point(400, 58), Size = new Size(150, 30), BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            var btnExportar = new Button { Text = "Exportar CSV", Location = new Point(560, 58), Size = new Size(150, 30), BackColor = Color.Green, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };

            this.Controls.Add(lblDesde); this.Controls.Add(dtpDesde);
            this.Controls.Add(lblHasta); this.Controls.Add(dtpHasta);
            this.Controls.Add(btnGenerar); this.Controls.Add(btnExportar);

            tabControl = new TabControl
            {
                Location = new Point(20, 100),
                Size = new Size(740, 460),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            var tabOperacion = new TabPage("Operación"); tabOperacion.BackColor = Color.White;
            var tabCambios = new TabPage("Cambios Operativos"); tabCambios.BackColor = Color.White;
            var tabSeguimiento = new TabPage("Seguimiento"); tabSeguimiento.BackColor = Color.White;

            dgvOperacion = CreateGrid();
            dgvCambios = CreateGrid();
            dgvSeguimiento = CreateGrid();

            tabOperacion.Controls.Add(dgvOperacion);
            tabCambios.Controls.Add(dgvCambios);
            tabSeguimiento.Controls.Add(dgvSeguimiento);

            tabControl.TabPages.Add(tabOperacion);
            tabControl.TabPages.Add(tabCambios);
            tabControl.TabPages.Add(tabSeguimiento);

            this.Controls.Add(tabControl);

            btnGenerar.Click += async (s, e) => await GenerarReportes();
            btnExportar.Click += async (s, e) => await ExportarCsv();
        }

        private DataGridView CreateGrid()
        {
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                RowHeadersVisible = false
            };
            return grid;
        }

        private ReportePeriodoDTO ObtenerPeriodo()
        {
            return new ReportePeriodoDTO
            {
                FechaInicio = dtpDesde.Value.Date,
                FechaFin = dtpHasta.Value.Date.AddDays(1).AddSeconds(-1)
            };
        }

        private async System.Threading.Tasks.Task GenerarReportes()
        {
            if (_reporteService == null)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(async () => await GenerarReportes()));
                return;
            }

            try
            {
                var periodo = ObtenerPeriodo();

                var operacion = await _reporteService.ObtenerReporteOperacionVuelosAsync(periodo);
                if (operacion != null)
                {
                    dgvOperacion.DataSource = new[]
                    {
                        new { Metrica = "Vuelos Registrados", Valor = operacion.TotalVuelosRegistrados },
                        new { Metrica = "Vuelos Completados", Valor = operacion.TotalVuelosCompletados },
                        new { Metrica = "Vuelos Retrasados", Valor = operacion.TotalVuelosRetrasados },
                        new { Metrica = "Vuelos Cancelados", Valor = operacion.TotalVuelosCancelados }
                    };
                }

                var cambios = await _reporteService.ObtenerReporteCambiosOperativosAsync(periodo);
                if (cambios != null)
                {
                    dgvCambios.DataSource = cambios;
                }

                var seguimiento = await _reporteService.ObtenerReporteSeguimientoAsync(periodo);
                if (seguimiento != null)
                {
                    dgvSeguimiento.DataSource = seguimiento.VuelosMasSeguidos;
                }
            }
            catch (Exception ex)
            {
                var mensaje = MensajesError.ObtenerMensaje(ex);
                if (InvokeRequired)
                    Invoke((MethodInvoker)delegate { MessageBox.Show(mensaje, "Error al generar reporte", MessageBoxButtons.OK, MessageBoxIcon.Error); });
                else
                    MessageBox.Show(mensaje, "Error al generar reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async System.Threading.Tasks.Task ExportarCsv()
        {
            if (_reporteService == null)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(async () => await ExportarCsv()));
                return;
            }

            try
            {
                var bytes = await _reporteService.ExportarReporteOperacionVuelosCsvAsync(ObtenerPeriodo());
                if (bytes == null || bytes.Length == 0)
                    return;

                using (var dialog = new SaveFileDialog { Filter = "CSV (*.csv)|*.csv", FileName = "reporte_operacion_vuelos.csv" })
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        System.IO.File.WriteAllBytes(dialog.FileName, bytes);
                        MessageBox.Show("Reporte exportado correctamente.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                var mensaje = MensajesError.ObtenerMensaje(ex);
                if (InvokeRequired)
                    Invoke((MethodInvoker)delegate { MessageBox.Show(mensaje, "Error al exportar reporte", MessageBoxButtons.OK, MessageBoxIcon.Error); });
                else
                    MessageBox.Show(mensaje, "Error al exportar reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
