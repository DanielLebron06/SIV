namespace SIV.Presentation.Desktop.Vuelos
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

}
