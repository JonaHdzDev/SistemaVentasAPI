using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_de_Ventas_API.Data;
using Sistema_de_Ventas_API.Dtos;
using Sistema_de_Ventas_API.Models;

namespace Sistema_de_Ventas_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController(TiendaDBContext db) : ControllerBase
{
    // GET api/productos
    [HttpGet]
    public async Task<ActionResult<List<ProductoDto>>> Listar()
    {
        return await db.Productos
            .AsNoTracking()
            .OrderBy(p => p.IdProducto)
            .Select(p => new ProductoDto(p.IdProducto, p.Nombre, p.Precio, p.Stock, p.Activo))
            .ToListAsync();
    }

    // GET api/productos/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductoDto>> Obtener(int id)
    {
        var producto = await db.Productos
            .AsNoTracking()
            .Where(p => p.IdProducto == id)
            .Select(p => new ProductoDto(p.IdProducto, p.Nombre, p.Precio, p.Stock, p.Activo))
            .FirstOrDefaultAsync();

        if (producto is null) return NotFound(new { error = "Producto no encontrado." });
        return producto;
    }

    // POST api/productos
    [HttpPost]
    public async Task<ActionResult<ProductoDto>> Crear(ProductoRequest r)
    {
        var producto = new Producto
        {
            Nombre = r.Nombre!,
            Precio = r.Precio,
            Stock = r.Stock,
            Activo = true
        };

        db.Productos.Add(producto);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(Obtener), new { id = producto.IdProducto }, ToDto(producto));
    }

    // PUT api/productos/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductoDto>> Actualizar(int id, ProductoRequest r)
    {
        var producto = await db.Productos.FindAsync(id);
        if (producto is null) return NotFound(new { error = "Producto no encontrado." });

        producto.Nombre = r.Nombre!;
        producto.Precio = r.Precio;
        producto.Stock = r.Stock;
        producto.Activo = r.Activo;
        await db.SaveChangesAsync();

        return ToDto(producto);
    }

    // DELETE api/productos/5  (no borra: desactiva)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Desactivar(int id)
    {
        var producto = await db.Productos.FindAsync(id);
        if (producto is null) return NotFound(new { error = "Producto no encontrado." });

        producto.Activo = false;
        await db.SaveChangesAsync();

        return NoContent();
    }

    private static ProductoDto ToDto(Producto p) =>
        new(p.IdProducto, p.Nombre, p.Precio, p.Stock, p.Activo);
}
