
using SIV.Presentation.Desktop.Auth;
using SIV.Presentation.Desktop.Catalogos;
using SIV.Presentation.Desktop.Common;
using SIV.Presentation.Desktop.Notificaciones;
using SIV.Presentation.Desktop.Reportes;
using SIV.Presentation.Desktop.Usuarios;
using SIV.Presentation.Desktop.Vuelos;
using System;
using System.Windows.Forms;

namespace SIV.Presentation.Desktop
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var apiClient = new ApiClient();
            var authService = new AuthService(apiClient);
            var vueloService = new VueloService(apiClient);
            var catalogoService = new CatalogoService(apiClient);
            var userService = new UserService(apiClient);
            var reporteService = new ReporteService(apiClient);
            var notificacionService = new NotificacionService(apiClient);

            bool mantenerAplicacion = true;

            while (mantenerAplicacion)
            {
                using (var frmLogin = new FrmLogin(authService))
                {
                    if (frmLogin.ShowDialog() == DialogResult.OK)
                    {
                        using (var frmMain = new FrmMain(
                            authService,
                            vueloService,
                            catalogoService,
                            userService,
                            reporteService,
                            notificacionService,
                            frmLogin.TokenRespuesta,
                            frmLogin.RolUsuario,
                            frmLogin.NombreUsuario))
                        {
                            var resultadoMain = frmMain.ShowDialog();
                            if (resultadoMain != DialogResult.OK)
                                mantenerAplicacion = false;
                        }
                    }
                    else
                    {
                        mantenerAplicacion = false;
                    }
                }
            }
        }
    }
}
