using Microsoft.EntityFrameworkCore;
using CsharpWebAt.Models;

namespace CsharpWebAt.Data
{
    public class CsharpWebAtContext : DbContext
    {
        public CsharpWebAtContext(DbContextOptions<CsharpWebAtContext> options)
            : base(options)
        {
        }

        // Definir DbSet para cada entidade do seu domínio
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Destino> Destinos { get; set; } // Representa cidades/países de destino combinados
        public DbSet<PacoteTuristico> PacotesTuristicos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PacoteTuristico>()
                .HasMany(p => p.Destinos)
                .WithMany(); // Sem uma propriedade de navegação em Destino para PacoteTuristico

            modelBuilder.Entity<Reserva>()
                .HasIndex(r => new { r.ClienteId, r.PacoteTuristicoId, r.DataReserva })
                .IsUnique(); // Garante que um cliente não reserve o mesmo pacote mais de uma vez para a mesma data

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Cliente)
                .WithMany(c => c.Reservas)
                .HasForeignKey(r => r.ClienteId);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.PacoteTuristico)
                .WithMany(p => p.Reservas)
                .HasForeignKey(r => r.PacoteTuristicoId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
