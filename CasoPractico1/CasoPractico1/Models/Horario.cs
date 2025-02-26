namespace CasoPractico1.Models
{
    public class Horario
    {
        public int HorarioId { get; set; }
        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }
        public int VehiculoId { get; set; }
        public Vehiculo Vehiculo { get; set; }
        public TimeSpan Hora { get; set; }

        // Relaciones de navegación
        public ICollection<Boleto> Boletos { get; set; }
    }
}
