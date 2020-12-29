using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebApiSubdivisions.Models
{
    public partial class DivisionsDatabaseContext : DbContext
    {
        public DivisionsDatabaseContext()
        {
        }

        public DivisionsDatabaseContext(DbContextOptions<DivisionsDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActualDatum> ActualData { get; set; }
        public virtual DbSet<Commander> Commanders { get; set; }
        public virtual DbSet<InternalSubdivision> InternalSubdivisions { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Subdivision> Subdivisions { get; set; }
        public virtual DbSet<TypesOfSubdivision> TypesOfSubdivisions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=DivisionsDatabase;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<ActualDatum>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.IdSubdivision).HasColumnName("id_Subdivision");

                entity.Property(e => e.Location)
                    .HasMaxLength(100)
                    .IsFixedLength(true);

                entity.HasOne(d => d.IdSubdivisionNavigation)
                    .WithMany(p => p.ActualData)
                    .HasForeignKey(d => d.IdSubdivision)
                    .HasConstraintName("FK_ActualData_Subdivision");

                entity.HasOne(d => d.LocationNavigation)
                    .WithMany(p => p.ActualData)
                    .HasForeignKey(d => d.Location)
                    .HasConstraintName("FK_ActualData_Locations");
            });

            modelBuilder.Entity<Commander>(entity =>
            {
                entity.HasKey(e => e.IdCommander);

                entity.Property(e => e.IdCommander).HasColumnName("id_Commander");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<InternalSubdivision>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.IdSubdivision).HasColumnName("id_Subdivision");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.Name);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsFixedLength(true);

                entity.Property(e => e.X).HasColumnType("numeric(5, 2)");

                entity.Property(e => e.Y).HasColumnType("numeric(5, 2)");
            });

            modelBuilder.Entity<Subdivision>(entity =>
            {
                entity.HasKey(e => e.IdSubdivision)
                    .HasName("PK_Subdivision");

                entity.Property(e => e.IdSubdivision)
                    .ValueGeneratedNever()
                    .HasColumnName("id_Subdivision");

                entity.Property(e => e.Composition)
                    .HasMaxLength(250)
                    .IsFixedLength(true);

                entity.Property(e => e.Document)
                    .HasMaxLength(100)
                    .IsFixedLength(true);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.TypeOfSubdivision)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.CommanderNavigation)
                    .WithMany(p => p.Subdivisions)
                    .HasForeignKey(d => d.Commander)
                    .HasConstraintName("FK__Subdivisi__Comma__4CA06362");
            });

            modelBuilder.Entity<TypesOfSubdivision>(entity =>
            {
                entity.HasKey(e => e.Name);

                entity.ToTable("TypesOfSubdivision");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
