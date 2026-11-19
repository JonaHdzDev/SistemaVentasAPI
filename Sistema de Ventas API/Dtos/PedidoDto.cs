namespace Sistema_de_Ventas_API.Dtos;

// Lo que DEVUELVE la API
public record PedidoDto(
    int IdPedido,
    int IdCliente,
    string Cliente,
    DateTime FechaPedido,
    string Estado,
    decimal Total);

// Lo que RECIBE la API (los parámetros de sp_Pedido_Crear)
public record PedidoRequest(int IdCliente, int IdProducto, int Cantidad);
