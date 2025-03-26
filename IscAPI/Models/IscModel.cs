//namespace IscAPI.Models
//{
//    public class IscModel
//    {
//    }
//}

using System.ComponentModel.DataAnnotations;

namespace IscAPI.Models
{
    public class Ingenieria
    {
        public int Id { get; set; }

        [Required]
        public string? Nombre { get; set; }

        [Required]
        public string? Apellido { get; set; }

        public string? Caracteristica { get; set; }

        public string? ImagenURL { get; set; }
    }
}

