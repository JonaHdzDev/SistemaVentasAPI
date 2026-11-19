using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sistema_de_Ventas_API.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Registro de DBContext
builder.Services.AddDbContext<TiendaDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TiendaDB")
        )
    );


// Uso de Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Convierte los errores de SQL Server en respuestas HTTP claras.
// Va primero para atrapar los errores de todo lo que se ejecute después.
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex) when (ObtenerSqlException(ex) is SqlException sql)
    {
        var (status, mensaje) = sql.Number switch
        {
            2627 or 2601 => (409, "Ya existe un registro con ese valor único (por ejemplo, el correo)."),
            547 => (400, "El dato no cumple una regla de la base de datos (restricción o relación)."),
            515 => (400, "Falta un dato obligatorio."),
            >= 50000 => (400, sql.Message),   // errores lanzados con THROW en SP o triggers
            _ => (500, "Error en la base de datos.")
        };

        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(new
        {
            error = mensaje,
            detalle = status == 500 ? null : sql.Message
        });
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static SqlException? ObtenerSqlException(Exception ex) =>
    ex as SqlException ?? ex.InnerException as SqlException;
