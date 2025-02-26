using Microsoft.EntityFrameworkCore;

namespace CasoPractico1.Models;
public class TransporteDbContext : DbContext
{
    public TransporteDbContext(DbContextOptions<TransporteDbContext> options)
        : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Ruta> Rutas { get; set; }
    public DbSet<Vehiculo> Vehiculos { get; set; }
    public DbSet<Parada> Paradas { get; set; }
    public DbSet<Horario> Horarios { get; set; }
    public DbSet<Boleto> Boletos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}