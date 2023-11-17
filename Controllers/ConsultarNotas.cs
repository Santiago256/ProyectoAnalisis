using Microsoft.AspNetCore.Mvc;
using ProyectoAnalisis.Datos;

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
            return View();
        }
    }
}
