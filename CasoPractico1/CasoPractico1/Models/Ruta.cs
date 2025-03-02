using System.ComponentModel.DataAnnotations.Schema;

namespace CasoPractico1.Models
{
    public class Ruta
    {
        public int RutaId { get; set; }
        public string CodigoRuta { get; set; }
        public string NombreRuta { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public int UsuarioRegistroId { get; set; }
        [ForeignKey("UsuarioRegistroId")]
        public Usuario Usuario { get; set; }

        public ICollection<Parada> Paradas { get; set; }
        public ICollection<Horario> Horarios { get; set; }
        public ICollection<Boleto> Boletos { get; set; }
    }
}
