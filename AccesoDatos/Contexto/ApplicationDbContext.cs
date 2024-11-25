using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Modelos.Models;
using Microsoft.Extensions.Configuration;


namespace AccesoDatos.Contexto;


public partial class ApplicationDbContext : DbContext
{




    private readonly IConfiguration _configuration;

    // Constructor con inyección de IConfiguration
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<GestionEventosEve> GestionEventosEves { get; set; }

    public virtual DbSet<HistorialModificacionEve> HistorialModificacionEves { get; set; }

    public virtual DbSet<InscripcionesEvt> InscripcionesEvts { get; set; }

    public virtual DbSet<UsuariosDatosGenerales> UsuariosUsus { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {            
            string connectionString = _configuration.GetConnectionString("ConexionMensajeriaEscritura")?? string.Empty;
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GestionEventosEve>(entity =>
        {
            entity.HasKey(e => e.IdEvento).HasName("PK__GESTION___BCC70973A9289284");

            entity.ToTable("GESTION_EVENTOS_EVE");

            entity.Property(e => e.IdEvento).HasColumnName("Id_Evento");
            entity.Property(e => e.CapacidadMaxima).HasColumnName("Capacidad_Maxima");
            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaHora)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Hora");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Ubicacion)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.GestionEventosEves)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Evento");
        });

        modelBuilder.Entity<HistorialModificacionEve>(entity =>
        {
            entity.HasKey(e => e.IdModificacion).HasName("PK__HISTORIA__AEE38344BE5B0FEE");

            entity.ToTable("HISTORIAL_MODIFICACION_EVE");

            entity.HasIndex(e => e.IdEvento, "IDX_Evento_Modificacion");

            entity.HasIndex(e => e.IdUsuario, "IDX_Usuario_Modificacion");

            entity.Property(e => e.IdModificacion).HasColumnName("Id_Modificacion");
            entity.Property(e => e.DescripcionModificacion)
                .HasColumnType("text")
                .HasColumnName("Descripcion_Modificacion");
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Modificacion");
            entity.Property(e => e.IdEvento).HasColumnName("Id_Evento");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.HistorialModificacionEves)
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Evento_Modificacion");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.HistorialModificacionEves)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Modificacion");
        });

        modelBuilder.Entity<InscripcionesEvt>(entity =>
        {
            entity.HasKey(e => e.IdInscripcion).HasName("PK__INSCRIPC__C7E9D2F59C9667C4");

            entity.ToTable("INSCRIPCIONES_EVT");

            entity.HasIndex(e => e.IdEvento, "IDX_Evento_Inscripcion");

            entity.HasIndex(e => e.IdUsuario, "IDX_Usuario_Inscripcion");

            entity.HasIndex(e => new { e.IdEvento, e.IdUsuario }, "UQ_Usuario_Evento").IsUnique();

            entity.Property(e => e.IdInscripcion).HasColumnName("Id_Inscripcion");
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaInscripcion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Inscripcion");
            entity.Property(e => e.IdEvento).HasColumnName("Id_Evento");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.InscripcionesEvts)
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Evento_Inscripcion");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.InscripcionesEvts)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Inscripcion");
        });

        modelBuilder.Entity<UsuariosDatosGenerales>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__USUARIOS__63C76BE29A323016");

            entity.ToTable("USUARIOS_USU");

            entity.HasIndex(e => e.CorreoUsuario, "UQ_Correo_Usuario").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
            entity.Property(e => e.CorreoUsuario)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Correo_Usuario");
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Nombre_Usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
