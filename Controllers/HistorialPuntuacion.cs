using Microsoft.AspNetCore.Mvc;
using ProyectoAnalisis.Datos;
using ProyectoAnalisis.Models;
using System.Linq;
using System.Security.Claims;

namespace ProyectoAnalisis.Controllers
{
    public class HistorialPuntuacionController : Controller
    {
        private readonly BaseDeDatosUsuario _context;

        public HistorialPuntuacionController(BaseDeDatosUsuario context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                // Obtén el Id del usuario actual
                int usuarioId = ObtenerUsuarioActualId();

                // Obtén la lista de puntuaciones del usuario
                var ultimaPuntuacion = _context.Notas
                    .Where(n => n.UsuarioId == usuarioId)
                    .OrderByDescending(n => n.Id)
                    .FirstOrDefault();

                if (ultimaPuntuacion != null)
                {
                    return View("~/Views/Juego/Puntuaciones.cshtml", new List<Notas> { ultimaPuntuacion });
                }
                else
                {
                    // Puedes personalizar el mensaje de error si no hay puntuaciones
                    ViewData["ErrorMessage"] = "No se encontraron puntuaciones para este usuario.";
                    return View("~/Views/Juego/Puntuaciones.cshtml", new List<Notas>());
                }
            }
            catch (Exception ex)
            {
                // Loguea el error o realiza alguna acción según tus necesidades
                Console.WriteLine($"Error al intentar obtener el historial de puntuaciones: {ex.Message}");

                // Puedes personalizar el mensaje de error en caso de una excepción
                ViewData["ErrorMessage"] = "Ocurrió un error al obtener el historial de puntuaciones.";
                return View("~/Views/Juego/Puntuaciones.cshtml", new List<Notas>());
            }
        }


        int ObtenerUsuarioActualId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            // Manejar el caso donde no se puede obtener el Id del usuario
         
            throw new InvalidOperationException("No se pudo obtener el Id del usuario actual");
        }

    }
}
