namespace CasoPractico1.Models
{
    public class Vehiculo
    {
        public int VehiculoId { get; set; }
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public int Capacidad { get; set; }
        public string Estado { get; set; }
        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Relaciones de navegación
        public ICollection<Horario> Horarios { get; set; }
        public ICollection<Boleto> Boletos { get; set; }
    }
}
