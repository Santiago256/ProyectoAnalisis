using Microsoft.AspNetCore.Mvc;
using ProyectoAnalisis.Models;
using ProyectoAnalisis.Recursos;

using ProyectoAnalisis.Servicios.Contrato;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;


namespace ProyectoAnalisis.Controllers
{
    public class Inicio : Controller
    {

        private readonly Iusuariocs _usuarioServicio;
        public Inicio(Iusuariocs usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }


        public IActionResult Registrarse()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario modelo)
        {
            modelo.Contraseña = Utilidades.EncriptarClave(modelo.Contraseña);

            Usuario usuario_creado = await  _usuarioServicio.saveUsuario(modelo);

            if (usuario_creado.Id > 0)
                return RedirectToAction("Index", "Home");

            ViewData["Mensaje"] = "No se pudo crear el usuario";
            return View();
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string correo, string clave)
        {

            Usuario usuario_encontrado = await _usuarioServicio.GetUsuario(correo, Utilidades.EncriptarClave(clave));

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, usuario_encontrado.Nombre),
            new Claim(ClaimTypes.Role, usuario_encontrado.Rol.ToString()),
              new Claim(ClaimTypes.NameIdentifier, usuario_encontrado.Id.ToString()) // Agregar el Id del usuario
            };

        


            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            return RedirectToAction("Index", "Home");
        }
    }
}
