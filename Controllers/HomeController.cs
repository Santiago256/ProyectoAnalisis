using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProyectoAnalisis.Models;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace ProyectoAnalisis.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
  
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Contenido()
        {

            return View("~/Views/Inicio/Contenido.cshtml");
        }


        public IActionResult Gramatica()
        {

            return View("~/Views/Inicio/Gramatica.cshtml");
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