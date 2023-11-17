using Microsoft.EntityFrameworkCore;
using ProyectoAnalisis.Models;

namespace ProyectoAnalisis.Datos
{
    public class BaseDeDatosUsuario : DbContext
    {
        public BaseDeDatosUsuario(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; }

    }
}
