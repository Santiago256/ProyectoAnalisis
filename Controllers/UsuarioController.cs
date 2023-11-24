using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAnalisis.Datos;
using ProyectoAnalisis.Models;
using System.Data;

namespace ProyectoAnalisis.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly BaseDeDatosUsuario bd;

        public UsuarioController(BaseDeDatosUsuario bd)
        {
            this.bd = bd;
        }
     
        [HttpGet]

        public IActionResult Agregar()
        {

            return View();
        }
        [HttpPost]

        public  async Task<IActionResult> Agregar(AdicionarUsuario modelo)
        {
            Usuario nuevoUsuario = new Usuario
            {
                Nombre = modelo.Nombre,
                Rol = modelo.Rol,
                Contraseña = modelo.Contraseña,
                Direccion = modelo.Direccion,
                CorreoElectronico = modelo.CorreoElectronico
            };
           await  bd.Usuario.AddAsync(nuevoUsuario);
            await bd.SaveChangesAsync();
            return RedirectToAction("ConsultarNota");
        }

    }
}
