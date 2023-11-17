
using Microsoft.EntityFrameworkCore;
using ProyectoAnalisis.Datos;
using ProyectoAnalisis.Models;
using ProyectoAnalisis.Servicios.Contrato;

namespace ProyectoAnalisis.Servicios.Implementacion
{
    public class UsuarioService : Iusuariocs
    {
        private readonly BaseDeDatosUsuario _dbContext;
        // se recibe como parámetro el contexto para hacerlo
        public UsuarioService(BaseDeDatosUsuario dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Usuario> GetUsuario(string correo, string clave)
        {
            Usuario usuario_encontrado = await _dbContext.Usuario
               .Where(u => u.CorreoElectronico == correo && u.Contraseña == clave)
        .FirstOrDefaultAsync();
            return usuario_encontrado;
        }

        public async Task<Usuario> saveUsuario(Usuario modelo)
        {
            _dbContext.Usuario.Add(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }
    }
}
