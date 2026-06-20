namespace SIV.Application.Common
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
}