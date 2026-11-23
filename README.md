# Sistema de Ventas API

API REST desarrollada con ASP.NET Core 8 y Entity Framework Core 8, conectada a SQL Server 2022. Permite administrar clientes, productos y pedidos mediante procedimientos almacenados, restricciones de integridad y mecanismos de auditoría implementados en la base de datos.

## Tecnologías

- C#
- .NET 8 / ASP.NET Core Web API
- Entity Framework Core 8
- SQL Server 2022
- SQL Server Management Studio 20
- Swagger / OpenAPI
- Git y GitHub

## Funcionalidades

### Clientes
- Consultar, registrar y actualizar clientes.
- Eliminar o desactivar clientes.

### Productos
- Consultar, registrar y actualizar productos.
- Eliminar o desactivar productos.
- Controlar existencias.

### Pedidos
- Consultar pedidos con información relacionada.
- Registrar pedidos y validar las existencias disponibles.
- Calcular el importe del pedido.
- Cancelar pedidos y devolver las existencias al inventario.

## Base de datos

La base de datos se llama `SistemaVentasDB` e incluye estas tablas:

| Tabla | Descripción |
|---|---|
| `Clientes` | Información de los clientes |
| `Productos` | Catálogo de productos y existencias |
| `Pedidos` | Encabezados de los pedidos |
| `DetallePedido` | Productos incluidos en cada pedido |
| `AuditoriaPedidos` | Historial de cambios de estado |
| `ReporteVentasDiarias` | Resultados de reportes diarios |

### Procedimientos almacenados y trigger

- `sp_Cliente_Listar`: consulta clientes y cantidad de pedidos.
- `sp_Producto_Listar`: consulta productos.
- `sp_Pedido_Listar`: consulta pedidos con información relacionada.
- `sp_Pedido_Crear`: registra pedidos y descuenta existencias.
- `sp_Pedido_Cancelar`: cancela pedidos y devuelve existencias.
- `sp_Reporte_GenerarVentasDiarias`: genera o actualiza el reporte de ventas.
- `tr_Pedidos_Auditoria`: registra los cambios de estado de los pedidos en `AuditoriaPedidos`.

### Integridad y validaciones

La base de datos utiliza claves primarias y foráneas, restricciones `CHECK` y `UNIQUE`, y valores predeterminados para proteger la integridad de la información.

## Arquitectura y acceso a datos

El proyecto utiliza el enfoque Database First de Entity Framework Core. Los modelos y el contexto de datos se generaron mediante scaffolding a partir de la base de datos existente.

## Requisitos previos

- Visual Studio 2022
- .NET 8 SDK
- SQL Server 2022
- SQL Server Management Studio
- Git

## Instalación y ejecución

1. Clona el repositorio:

   ```bash
   git clone URL_DEL_REPOSITORIO
   ```

2. Abre `SistemaVentasAPI.sln` en Visual Studio 2022.
3. Crea la base de datos `SistemaVentasDB` ejecutando los scripts SQL incluidos en el repositorio.
4. Configura la cadena de conexión en `appsettings.json` y ajústala a tu servidor y método de autenticación:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=SistemaVentasDB;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```

5. Restaura los paquetes NuGet, compila y ejecuta la API:

   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

6. Abre Swagger usando la dirección que aparece en la consola o en el perfil de ejecución de Visual Studio.

## Pruebas

Los endpoints CRUD se probaron mediante Swagger, verificando operaciones de creación, consulta, actualización y eliminación.

## Objetivo del proyecto

Proyecto de práctica orientado al desarrollo backend con C#, ASP.NET Core y SQL Server, aplicando operaciones CRUD, relaciones entre tablas, procedimientos almacenados, validaciones e integridad de datos.

Autor
Jonathan Sebastian Villarreal Hernandez. 
