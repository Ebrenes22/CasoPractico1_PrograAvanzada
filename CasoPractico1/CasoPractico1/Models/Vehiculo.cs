using System.ComponentModel.DataAnnotations.Schema;

namespace CasoPractico1.Models
{
    public class Vehiculo
    {
        [Column("VehiculoId")]
        public int VehiculoId { get; set; }
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public int Capacidad { get; set; }
        public string Estado { get; set; }
        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public int UsuarioRegistroId { get; set; }
        [ForeignKey("UsuarioRegistroId")]
        public Usuario Usuario { get; set; }
        // Relaciones de navegación
        public ICollection<Boleto> Boletos { get; set; }
    }
}
