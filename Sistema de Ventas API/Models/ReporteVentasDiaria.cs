using System;
using System.Collections.Generic;

namespace Sistema_de_Ventas_API.Models;

public partial class ReporteVentasDiaria
{
    public int IdReporte { get; set; }

    public DateOnly FechaReporte { get; set; }

    public int TotalPedidos { get; set; }

    public decimal TotalVentas { get; set; }

    public DateTime FechaGeneracion { get; set; }
}
