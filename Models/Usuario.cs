using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace ProyectoAnalisis.Models
{

    public enum RolUsuario
    {
        Profesor,
        Estudiante
    }
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public string Rol { get; set; }
        [Required(ErrorMessage = "Debes ingresar una nueva contraseña.")]
        public string Contraseña { get; set; }
        public string Direccion { get; set; }
        public string CorreoElectronico { get; set; }

        public Notas? Nota { get; set; }


    }
}
