using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_de_Ventas_API.Data;
using Sistema_de_Ventas_API.Dtos;
using Sistema_de_Ventas_API.Models;

namespace Sistema_de_Ventas_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController(TiendaDBContext db) : ControllerBase
{
    // GET api/clientes
    [HttpGet]
    public async Task<ActionResult<List<ClienteDto>>> Listar()
    {
        return await db.Clientes
            .AsNoTracking()
            .OrderBy(c => c.IdCliente)
            .Select(c => new ClienteDto(c.IdCliente, c.Nombre, c.Correo, c.Telefono, c.Activo, c.FechaRegistro))
            .ToListAsync();
    }

    // GET api/clientes/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteDto>> Obtener(int id)
    {
        var cliente = await db.Clientes
            .AsNoTracking()
            .Where(c => c.IdCliente == id)
            .Select(c => new ClienteDto(c.IdCliente, c.Nombre, c.Correo, c.Telefono, c.Activo, c.FechaRegistro))
            .FirstOrDefaultAsync();

        if (cliente is null) return NotFound(new { error = "Cliente no encontrado." });
        return cliente;
    }

    // POST api/clientes
    [HttpPost]
    public async Task<ActionResult<ClienteDto>> Crear(ClienteRequest r)
    {
        var cliente = new Cliente
        {
            Nombre = r.Nombre!,     // si viene null, SQL Server lo rechaza (middleware -> 400)
            Correo = r.Correo!,
            Telefono = r.Telefono,
            Activo = true
        };

        db.Clientes.Add(cliente);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(Obtener), new { id = cliente.IdCliente }, ToDto(cliente));
    }

    // PUT api/clientes/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ClienteDto>> Actualizar(int id, ClienteRequest r)
    {
        var cliente = await db.Clientes.FindAsync(id);
        if (cliente is null) return NotFound(new { error = "Cliente no encontrado." });

        cliente.Nombre = r.Nombre!;
        cliente.Correo = r.Correo!;
        cliente.Telefono = r.Telefono;
        cliente.Activo = r.Activo;
        await db.SaveChangesAsync();

        return ToDto(cliente);
    }

    // DELETE api/clientes/5  (no borra: desactiva)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Desactivar(int id)
    {
        var cliente = await db.Clientes.FindAsync(id);
        if (cliente is null) return NotFound(new { error = "Cliente no encontrado." });

        cliente.Activo = false;
        await db.SaveChangesAsync();

        return NoContent();
    }

    private static ClienteDto ToDto(Cliente c) =>
        new(c.IdCliente, c.Nombre, c.Correo, c.Telefono, c.Activo, c.FechaRegistro);
}