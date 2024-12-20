using Microsoft.EntityFrameworkCore;
using Inzynierka.Core.Entities;

namespace Inzynierka.Infrastructure.Persistance
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Material> Materials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfiguracja Contractor
            modelBuilder.Entity<Contractor>(entity =>
            {
                entity.Property(c => c.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(c => c.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(c => c.CompanyName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.Address)
                    .HasMaxLength(200);

                entity.Property(c => c.PhoneNumber)
                    .HasMaxLength(15);

                entity.Property(c => c.TaxIdNumber)
                    .HasMaxLength(10);

                entity.Property(c => c.NationalBusinessRegistryNumber)
                    .HasMaxLength(9);

                entity.Property(c => c.Email)
                    .HasMaxLength(100);

                entity.Property(c => c.CreatedAt)
                    .IsRequired();
            });

            // Konfiguracja Project
            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(p => p.Description)
                    .HasMaxLength(500);

                entity.HasOne(p => p.Contractor)
                    .WithMany(c => c.Projects)
                    .HasForeignKey(p => p.ContractorId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Konfiguracja Material
            modelBuilder.Entity<Material>(entity =>
            {
                entity.Property(m => m.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(m => m.Quantity)
                    .HasPrecision(18, 2)
                    .IsRequired();

                entity.Property(m => m.Unit)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(m => m.AttachmentPath)
                    .HasMaxLength(255);

                entity.Property(m => m.LastUpdated)
                    .IsRequired();

                entity.HasOne(m => m.Project)
                    .WithMany(p => p.Materials)
                    .HasForeignKey(m => m.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
