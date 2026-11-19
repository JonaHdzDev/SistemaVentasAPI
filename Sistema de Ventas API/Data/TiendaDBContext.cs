using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Sistema_de_Ventas_API.Models;

namespace Sistema_de_Ventas_API.Data;

public partial class TiendaDBContext : DbContext
{
    public TiendaDBContext(DbContextOptions<TiendaDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditoriaPedido> AuditoriaPedidos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<DetallePedido> DetallePedidos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<ReporteVentasDiaria> ReporteVentasDiarias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditoriaPedido>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria).HasName("PK__Auditori__7FD13FA01816CBF0");

            entity.Property(e => e.EstadoAnterior).HasMaxLength(20);
            entity.Property(e => e.EstadoNuevo).HasMaxLength(20);
            entity.Property(e => e.FechaCambio).HasDefaultValueSql("(sysdatetime())");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Clientes__D59466423565E710");

            entity.HasIndex(e => e.Correo, "UQ_Clientes_Correo").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => e.IdDetalle).HasName("PK__DetalleP__E43646A517E05BA1");

            entity.ToTable("DetallePedido");

            entity.HasIndex(e => new { e.IdPedido, e.IdProducto }, "UQ_Detalle_Pedido_Producto").IsUnique();

            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Subtotal)
                .HasComputedColumnSql("(CONVERT([decimal](12,2),[Cantidad]*[PrecioUnitario]))", true)
                .HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetallePedido_Pedidos");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetallePedido_Productos");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PK__Pedidos__9D335DC310A05D31");

            entity.ToTable(tb => tb.HasTrigger("tr_Pedidos_Auditoria"));

            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaPedido).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Total).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedidos_Clientes");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__09889210DEBC0425");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<ReporteVentasDiaria>(entity =>
        {
            entity.HasKey(e => e.IdReporte).HasName("PK__ReporteV__F9561136EE156261");

            entity.HasIndex(e => e.FechaReporte, "UQ_Reporte_Fecha").IsUnique();

            entity.Property(e => e.FechaGeneracion).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.TotalVentas).HasColumnType("decimal(12, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
