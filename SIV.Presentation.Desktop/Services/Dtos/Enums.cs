namespace SIV.Presentation.Desktop.Services.Dtos
{
    public enum EstadoVuelo
    {
        Programado,
        Retrasado,
        Embarcando,
        EnVuelo,
        Aterrizado,
        Completado,
        Cancelado
    }

    public enum TipoCambio
    {
        Retraso,
        Adelanto,
        CambioPuerta,
        Cancelacion,
        CambioEstado
    }

    public enum Rol
    {
        UsuarioRegistrado,
        Operador,
        Administrador,
        Auditor
    }

    public enum Modulo
    {
        Usuarios,
        Vuelos,
        Notificaciones
    }

    public enum TipoAccion
    {
        Crear,
        Actualizar,
        Eliminar,
        DesactivarUsuario,
        Login,
        SeguirVuelo,
        DejarSeguirVuelo,
        ActivarUsuario
    }
}
