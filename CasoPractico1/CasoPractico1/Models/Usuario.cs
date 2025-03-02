using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace CasoPractico1.Models

{
    public class Usuario
    {

        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Contrasena { get; set; }
        public string Rol { get; set; }

    }
}
