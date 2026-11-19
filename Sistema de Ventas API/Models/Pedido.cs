using System;
using System.Collections.Generic;

namespace Sistema_de_Ventas_API.Models;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public int IdCliente { get; set; }

    public DateTime FechaPedido { get; set; }

    public string Estado { get; set; } = null!;

    public decimal Total { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;
}
