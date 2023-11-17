using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoAnalisis.Datos;
using ProyectoAnalisis.Models;
using ProyectoAnalisis.Recursos;

namespace ProyectoAnalisis.Controllers
{
    public class CrudUsuariosController : Controller
    {
        private readonly BaseDeDatosUsuario _context;

        public CrudUsuariosController(BaseDeDatosUsuario context)
        {
            _context = context;
        }

        // GET: CrudUsuarios
        public async Task<IActionResult> Index()
        {
              return _context.Usuario != null ? 
                          View(await _context.Usuario.ToListAsync()) :
                          Problem("Entity set 'BaseDeDatosUsuario.Usuario'  is null.");
        }

        // GET: CrudUsuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: CrudUsuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CrudUsuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Rol,Contraseña,Direccion,CorreoElectronico")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                //se usa la clase encriptar clave para guardarla en la Bd
                usuario.Contraseña = Utilidades.EncriptarClave(usuario.Contraseña);

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: CrudUsuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: CrudUsuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            Console.WriteLine($"Entrando al método Edit con ID: {id}, Usuario ID: {usuario.Id}");

            // Elimina el error de validación para el campo Contraseña
            // cuando no se proporciona una nueva contraseña durante la edición del usuario, no hay problema
            ModelState.Remove("Contraseña");

            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Console.WriteLine("ModelState es válido");
                try
                {
                    // Obtener el usuario original de la base de datos
                    var usuarioOriginal = await _context.Usuario.FindAsync(id);

                    if (usuarioOriginal == null)
                    {
                        return NotFound();
                    }

                    // Asignar la contraseña original al usuario que se está editando
                    usuario.Contraseña = usuarioOriginal.Contraseña;

                    // Modificar las propiedades del usuario original con los valores del usuario que se está editando
                    _context.Entry(usuarioOriginal).CurrentValues.SetValues(usuario);

                    // Guardar los cambios en la base de datos
                    await _context.SaveChangesAsync();

                    TempData["MensajeExito"] = "Usuario actualizado correctamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine($"Error al actualizar: {ex.Message}");
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                Console.WriteLine("ModelState no es válido:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"- {error.ErrorMessage}");
                }
            }

            TempData["MensajeError"] = "Hubo un problema al actualizar el usuario. Verifique los datos e intente nuevamente.";

            return View(usuario);
        }


        // GET: CrudUsuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: CrudUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuario == null)
            {
                return Problem("Entity set 'BaseDeDatosUsuario.Usuario'  is null.");
            }
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuario.Remove(usuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
          return (_context.Usuario?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
