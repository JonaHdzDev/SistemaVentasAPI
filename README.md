Sistema de Ventas API
API REST desarrollada con ASP.NET Core 8 y Entity Framework Core 8, conectada a SQL Server 2022. El sistema permite administrar clientes, productos y pedidos, utilizando procedimientos almacenados, restricciones de integridad y mecanismos de auditoría directamente en la base de datos.
Tecnologías utilizadas
- C#
- .NET 8 / ASP.NET Core Web API
- Entity Framework Core 8
- SQL Server 2022
- SQL Server Management Studio 20
- Swagger / OpenAPI
- Git y GitHub
Funcionalidades
Clientes
- Consultar clientes.
- Registrar clientes.
- Actualizar información.
- Eliminar o desactivar clientes.
Productos
- Consultar productos.
- Registrar productos.
- Actualizar información.
- Eliminar o desactivar productos.
- Control de existencias.
Pedidos
- Consultar pedidos con información relacionada.
- Registrar pedidos.
- Validar existencias disponibles.
- Calcular el importe del pedido.
- Cancelar pedidos.
- Devolver existencias al inventario al cancelar.
Base de datos
La base de datos se llama SistemaVentasDB y contiene las siguientes tablas:
Tabla	Descripción
Clientes	Información de los clientes
Productos	Catálogo de productos y existencias
Pedidos	Encabezados de los pedidos
DetallePedido	Productos incluidos en cada pedido
AuditoriaPedidos	Historial de cambios de estado
ReporteVentasDiarias	Resultados de reportes diariosProcedimientos almacenados
- sp_Cliente_Listar: consulta clientes y cantidad de pedidos.
- sp_Producto_Listar: consulta productos.
- sp_Pedido_Listar: consulta pedidos con información relacionada.
- sp_Pedido_Crear: registra pedidos y descuenta existencias.
- sp_Pedido_Cancelar: cancela pedidos y devuelve existencias.
- sp_Reporte_GenerarVentasDiarias: genera o actualiza el reporte de ventas.
Trigger
tr_Pedidos_Auditoria: registra los cambios de estado de los pedidos en la tabla AuditoriaPedidos.
Restricciones y validaciones
La base de datos utiliza claves primarias, claves foráneas, restricciones CHECK, valores predeterminados y restricciones UNIQUE para proteger la integridad de la información.
Arquitectura y acceso a datos
El proyecto utiliza el enfoque Database First de Entity Framework Core.

Los modelos y el contexto de datos se generaron mediante scaffolding a partir de la base de datos existente.
Requisitos previos
- Visual Studio 2022.
- .NET 8 SDK.
- SQL Server 2022.
- SQL Server Management Studio.
- Git.
Instalación y ejecución
1. Clonar el repositorio:
   git clone URL_DEL_REPOSITORIO
2. Abrir la solución SistemaVentasAPI.sln en Visual Studio 2022.
3. Crear la base de datos SistemaVentasDB ejecutando los scripts SQL incluidos en el repositorio.
4. Configurar la cadena de conexión en appsettings.json:
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=SistemaVentasDB;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   Ajustar el servidor y el método de autenticación según el entorno local.
5. Restaurar los paquetes NuGet:
   dotnet restore
6. Compilar el proyecto:
   dotnet build
7. Ejecutar la API:
   dotnet run
8. Abrir Swagger desde la dirección indicada en la consola o en el perfil de ejecución de Visual Studio.
Pruebas

Los endpoints CRUD fueron probados mediante Swagger, verificando operaciones de creación, consulta, actualización y eliminación.
Objetivo del proyecto

Proyecto de práctica orientado al desarrollo backend con C#, ASP.NET Core y SQL Server, aplicando operaciones CRUD, relaciones entre tablas, procedimientos almacenados, validaciones e integridad de datos.

Autor
Jonathan Sebastian Villarreal Hernandez. 
