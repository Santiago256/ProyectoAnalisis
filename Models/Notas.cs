namespace ProyectoAnalisis.Models
{
    public class Notas
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int ContenidoNota { get; set; } // Ajusta el tipo de datos según tus necesidades

        public Usuario Usuario { get; set; }
    }
}
