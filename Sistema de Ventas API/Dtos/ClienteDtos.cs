namespace Sistema_de_Ventas_API.Dtos;

// Lo que DEVUELVE la API
public record ClienteDto(
    int IdCliente,
    string Nombre,
    string Correo,
    string? Telefono,
    bool Activo,
    DateTime FechaRegistro);

// Lo que RECIBE la API (los campos pueden venir vacíos: la base de datos valida)
public record ClienteRequest(string? Nombre, string? Correo, string? Telefono, bool Activo = true);
