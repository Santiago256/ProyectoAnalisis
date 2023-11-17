﻿using System.ComponentModel.DataAnnotations;
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
        public string Contraseña { get; set; }
        public string Direccion { get; set; }
        public string CorreoElectronico { get; set; }

        public List<Notas> Notas { get; set; }

    }
}