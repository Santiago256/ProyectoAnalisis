using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProyectoAnalisis.Models;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProyectoAnalisis.Datos;
using ProyectoAnalisis.Recursos;


namespace ProyectoAnalisis.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly BaseDeDatosUsuario _context;
        private readonly ILogger<HomeController> _logger;
      

        public HomeController(ILogger<HomeController> logger, BaseDeDatosUsuario context)
        {
            _logger = logger;
            _context = context;

        }
    
        // Acción para mostrar los datos del usuario

        public IActionResult MisDatos()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Buscar el usuario en la base de datos utilizando el ID
            var usuario = _context.Usuario.FirstOrDefault(u => u.Id.ToString() == userId);

            if (usuario == null)
            {
                return NotFound();
            }

            return View("~/Views/Inicio/MisDatos.cshtml", usuario);
        }

     
        public IActionResult ActualizarDatos(Usuario usuarioActualizado)
        {
            if (ModelState.IsValid)
            {
                var usuarioExistente = _context.Usuario.FirstOrDefault(u => u.Id == usuarioActualizado.Id);
                if (usuarioExistente != null)
                {
                    // Verificar y actualizar los campos modificados
                    if (usuarioExistente.Nombre != usuarioActualizado.Nombre)
                    {
                        usuarioExistente.Nombre = usuarioActualizado.Nombre;
                    }
                    if (usuarioExistente.CorreoElectronico != usuarioActualizado.CorreoElectronico)
                    {
                        usuarioExistente.CorreoElectronico = usuarioActualizado.CorreoElectronico;
                    }
                    if (usuarioExistente.Direccion != usuarioActualizado.Direccion)
                    {
                        usuarioExistente.Direccion = usuarioActualizado.Direccion;
                    }
                    if (usuarioExistente.Rol != usuarioActualizado.Rol) // Verifica si el Rol ha cambiado
                    {
                        usuarioExistente.Rol = usuarioActualizado.Rol; // Actualiza el Rol
                    }
                    // Verificar si la contraseña se ha cambiado y encriptarla si es necesario
                    if (usuarioExistente.Contraseña != usuarioActualizado.Contraseña)
                    {
                        usuarioExistente.Contraseña = Utilidades.EncriptarClave(usuarioActualizado.Contraseña);
                    }

                    _context.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return NotFound(); // Usuario no encontrado en la base de datos
                }
            }
            else
            {
                // Captura los errores de validación y agrégalos a una lista
                var erroresValidacion = new List<string>();
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    erroresValidacion.Add(error.ErrorMessage);
                }

                // Retorna la vista con los errores de validación
                ModelState.AddModelError(string.Empty, "Por favor, corrige los errores e intenta de nuevo.");
                ViewBag.ErroresValidacion = erroresValidacion;
                return View("~/Views/Inicio/MisDatos.cshtml", usuarioActualizado);
            }
        }


        public IActionResult Contenido()
        {

            return View("~/Views/Inicio/Contenido.cshtml");
        }

        public IActionResult Repasos()
        {

            return View("~/Views/Inicio/Repasos.cshtml");
        }

        public IActionResult Gramatica()
        {

            return View("~/Views/Inicio/Gramatica.cshtml");
        }

        public IActionResult Juego()
        {

            return View("~/Views/Inicio/Juego.cshtml");
        }

        public IActionResult Volver()
        {
            // Redirige a la acción principal del dispositivo .
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Videos()
        {
          
            return View("~/Views/Inicio/VideoInfo.cshtml");

        }


        public IActionResult Vocabulario()
        {
            var palabras = new List<(string Palabra, string Imagen)>
    {
        ("cook", "cook.jpg"),
        ("eat", "eat.jpg"),
        ("love", "love.jpg"),
        ("pull", "pull.jpg"),
        ("sing", "sing.jpg"),
        ("think", "think.jpg"),
        ("throw", "throw.jpg"),
        ("wash", "wash.jpg"),
        ("write", "write.jpg")
    };

            ViewBag.Palabras = palabras;
            return View("~/Views/Inicio/Vocabulariocshtml.cshtml");
        }



        public IActionResult Index()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string nombreUsuario = "";
            string primerNombre = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                nombreUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
                //Para que tome solamente el primer nombre de usuario en el despliegue del bienvenido
                 primerNombre = nombreUsuario.Split(' ')[0];
             
            }

            ViewData["Nombre"] = primerNombre;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("IniciarSesion", "Inicio");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

      
      

    }
}