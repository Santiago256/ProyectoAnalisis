// Controlador JuegoController.cs
using Microsoft.AspNetCore.Mvc;
using ProyectoAnalisis.Datos;

using System.Collections.Generic;
using System.Security.Claims;
using ProyectoAnalisis.Models;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoAnalisis.Controllers
{
    [Authorize]
    public class JuegoController : Controller
    {
        private static List<int> puntuaciones = new List<int>(); // Lista para almacenar las puntuaciones


        private readonly BaseDeDatosUsuario _context;

        public JuegoController(BaseDeDatosUsuario context)
        {
            _context = context;
        }

     


        public IActionResult Index()
        {
         
            // Pasa la lista completa de oraciones a la vista
         
            return View("~/Views/Juego/Index.cshtml");
        }

        [HttpPost]
        public IActionResult GuardarPuntuacion(int puntuacion)
        {
            try
            {
                // Obtén el Id del usuario actual
                int usuarioId = ObtenerUsuarioActualId();

                // Busca la nota existente para el usuario
                var notaExistente = _context.Notas.FirstOrDefault(n => n.UsuarioId == usuarioId);

                if (notaExistente != null)
                {
                    // Si ya existe una nota, actualiza la puntuación
                    notaExistente.ContenidoNota = puntuacion;
                }
                else
                {
                    // Si no existe una nota, crea una nueva
                    Notas nuevaNota = new Notas
                    {
                        UsuarioId = usuarioId,
                        ContenidoNota = puntuacion
                    };

                    // Agrega la nueva nota a la base de datos
                    _context.Notas.Add(nuevaNota);
                }

                // Guarda los cambios en la base de datos
                _context.SaveChanges();

                Console.WriteLine($"Puntuación guardada exitosamente para el usuario {usuarioId}, puntuación: {puntuacion}");
                return Ok("Puntuación guardada exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al intentar guardar la puntuación: {ex.Message}");

                // Manejar errores, loguear, etc.
                return BadRequest("Error al intentar guardar la puntuación");
            }
        }

        [HttpPost]
        public IActionResult ReiniciarJuego()
        {
            try
            {
                // Obtén el Id del usuario actual
                int usuarioId = ObtenerUsuarioActualId();

                // Busca y elimina el registro anterior si existe
                var registroAnterior = _context.Notas.FirstOrDefault(n => n.UsuarioId == usuarioId);
                if (registroAnterior != null)
                {
                    _context.Notas.Remove(registroAnterior);
                    _context.SaveChanges();
                }

                // Inicializa cualquier otra lógica necesaria para reiniciar el juego
                // ...

                return Ok(new { mensaje = "Juego reiniciado exitosamente" });
            }
            catch (Exception ex)
            {
                // Manejar errores, loguear, etc.
                return BadRequest("Error al intentar reiniciar el juego");
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
            // Puede lanzar una excepción, devolver un valor predeterminado o tomar alguna otra acción según tus necesidades
            throw new InvalidOperationException("No se pudo obtener el Id del usuario actual");
        }


      
    }
   
}


