using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIV.Domain.Enums
{
    public enum EstadoVuelo
    {
        Programado,
        EnHorario,
        EnEmbarque,
        Abordando,
        EnRuta,
        Aterrizado,
        Desembarcando,
        Finalizado,
        Cancelado,
        NoShow
    }
}
