using System;
using System.Collections.Generic;

namespace Sistema_de_Ventas_API.Models;

public partial class AuditoriaPedido
{
    public int IdAuditoria { get; set; }

    public int IdPedido { get; set; }

    public string? EstadoAnterior { get; set; }

    public string EstadoNuevo { get; set; } = null!;

    public DateTime FechaCambio { get; set; }
}
