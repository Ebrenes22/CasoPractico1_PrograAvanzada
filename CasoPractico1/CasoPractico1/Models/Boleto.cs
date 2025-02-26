namespace CasoPractico1.Models
{
    public class Boleto
    {
        public int BoletoId { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int HorarioId { get; set; }
        public Horario Horario { get; set; }
        public int VehiculoId { get; set; }
        public Vehiculo Vehiculo { get; set; }
        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }
        public DateTime FechaCompra { get; set; } = DateTime.Now;
    }
}
