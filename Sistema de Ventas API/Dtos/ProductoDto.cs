namespace Sistema_de_Ventas_API.Dtos;

public record ProductoDto(int IdProducto, string Nombre, decimal Precio, int Stock, bool Activo);

public record ProductoRequest(string? Nombre, decimal Precio, int Stock, bool Activo = true);
