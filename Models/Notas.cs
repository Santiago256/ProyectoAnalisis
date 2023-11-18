using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoAnalisis.Models
{
    public class Notas
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int ContenidoNota { get; set; } // Ajusta el tipo de datos según tus necesidades

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }
    }
}
