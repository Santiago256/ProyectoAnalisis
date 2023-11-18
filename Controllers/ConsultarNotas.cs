using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoAnalisis.Datos;
using ProyectoAnalisis.Models;

namespace ProyectoAnalisis.Controllers
{
    public class ConsultarNotas : Controller
    {

        private readonly BaseDeDatosUsuario _context; // Asegúrate de tener una instancia de tu DbContext

        public ConsultarNotas(BaseDeDatosUsuario context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
        
                // Recupera todas las notas de los estudiantes
                var notasEstudiantes = _context.Notas
                    .Include(n => n.Usuario)
                    .Where(n => n.Usuario.Rol == "Estudiante")
                    .ToList();
            return View("~/Views/Profesor/ConsultarNotas.cshtml", notasEstudiantes);

        }

    }
}
