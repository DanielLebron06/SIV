using System.Drawing;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop.Common
{
    /// <summary>
    /// Plantilla de ajuste de layout para los formularios incrustados en el panel central de FrmMain.
    /// Garantiza que ninguna grilla o contenido se desborde horizontalmente fuera del contenedor.
    /// </summary>
    public static class AjusteLayout
    {
        private static readonly Padding MargenUniforme = new Padding(15);

        /// <summary>
        /// Aplica el ajuste global de layout a un formulario hijo:
        /// elimina el tamaño mínimo fijo (para que Dock = Fill adapte el formulario al panel),
        /// ajusta las grillas a AutoSizeColumnsMode.Fill, mantiene visible la columna "Id",
        /// deja márgenes uniformes en las TabPages/paneles que contienen grillas y recálcula el layout.
        /// </summary>
        public static void AjustarFormulario(Form formulario)
        {
            if (formulario == null)
                return;

            formulario.MinimumSize = Size.Empty;

            foreach (Control control in formulario.Controls)
                AjustarControl(control);

            formulario.PerformLayout();
        }

        private static void AjustarControl(Control control)
        {
            if (control is TabControl tabControl)
            {
                foreach (TabPage tabPage in tabControl.TabPages)
                {
                    tabPage.Padding = MargenUniforme;
                    foreach (Control hijo in tabPage.Controls)
                        AjustarControl(hijo);
                }
                return;
            }

            if (control is DataGridView grid)
            {
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                grid.DataBindingComplete += (s, e) =>
                {
                    if (grid.Columns["Id"] != null)
                        grid.Columns["Id"].Visible = true;
                };
                return;
            }

            foreach (Control hijo in control.Controls)
                AjustarControl(hijo);
        }
    }
}
