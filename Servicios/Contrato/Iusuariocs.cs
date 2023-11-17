
using Microsoft.EntityFrameworkCore;
using ProyectoAnalisis.Models;

namespace ProyectoAnalisis.Servicios.Contrato
{
    public interface Iusuariocs
    {
        Task<Usuario> GetUsuario(string correo, string clave);
        Task<Usuario> saveUsuario(Usuario modelo);
    }
}
