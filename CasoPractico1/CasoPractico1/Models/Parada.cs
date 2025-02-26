namespace CasoPractico1.Models
{
    public class Parada
    {
        public int ParadaId { get; set; }
        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }
        public string NombreParada { get; set; }
    }
}
