using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sistema_de_Ventas_API.Data;
using Sistema_de_Ventas_API.Dtos;

namespace Sistema_de_Ventas_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController(TiendaDBContext db) : ControllerBase
{
    // GET api/pedidos  (EF genera un INNER JOIN entre Pedidos y Clientes)
    [HttpGet]
    public async Task<ActionResult<List<PedidoDto>>> Listar()
    {
        return await (
            from p in db.Pedidos.AsNoTracking()
            join c in db.Clientes on p.IdCliente equals c.IdCliente
            orderby p.IdPedido descending
            select new PedidoDto(p.IdPedido, p.IdCliente, c.Nombre, p.FechaPedido, p.Estado, p.Total)
        ).ToListAsync();
    }

    // GET api/pedidos/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PedidoDto>> Obtener(int id)
    {
        var pedido = await BuscarPedido(id);
        if (pedido is null) return NotFound(new { error = "Pedido no encontrado." });
        return pedido;
    }

    // POST api/pedidos  (toda la lógica la hace sp_Pedido_Crear)
    [HttpPost]
    public async Task<ActionResult<PedidoDto>> Crear(PedidoRequest r)
    {
        // Parámetro de SALIDA: aquí el SP nos devuelve el Id del pedido creado
        var idGenerado = new SqlParameter("@IdPedidoGenerado", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };

        await db.Database.ExecuteSqlRawAsync(
            "EXEC dbo.sp_Pedido_Crear @IdCliente = @IdCliente, @IdProducto = @IdProducto, " +
            "@Cantidad = @Cantidad, @IdPedidoGenerado = @IdPedidoGenerado OUTPUT",
            new SqlParameter("@IdCliente", r.IdCliente),
            new SqlParameter("@IdProducto", r.IdProducto),
            new SqlParameter("@Cantidad", r.Cantidad),
            idGenerado);

        var id = Convert.ToInt32(idGenerado.Value);
        var pedido = await BuscarPedido(id);
        return CreatedAtAction(nameof(Obtener), new { id }, pedido);
    }

    // PUT api/pedidos/5/cancelar  (sp_Pedido_Cancelar cambia el estado y devuelve el stock)
    [HttpPut("{id:int}/cancelar")]
    public async Task<ActionResult<PedidoDto>> Cancelar(int id)
    {
        await db.Database.ExecuteSqlInterpolatedAsync($"EXEC dbo.sp_Pedido_Cancelar @IdPedido = {id}");

        var pedido = await BuscarPedido(id);
        if (pedido is null) return NotFound(new { error = "Pedido no encontrado." });
        return pedido;
    }

    // Consulta con INNER JOIN reutilizada por Obtener y Crear
    private async Task<PedidoDto?> BuscarPedido(int id)
    {
        return await (
            from p in db.Pedidos.AsNoTracking()
            join c in db.Clientes on p.IdCliente equals c.IdCliente
            where p.IdPedido == id
            select new PedidoDto(p.IdPedido, p.IdCliente, c.Nombre, p.FechaPedido, p.Estado, p.Total)
        ).FirstOrDefaultAsync();
    }
}