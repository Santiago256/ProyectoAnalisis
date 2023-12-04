using Microsoft.EntityFrameworkCore;
using ProyectoAnalisis.Datos;
using ProyectoAnalisis.Servicios.Contrato;
using ProyectoAnalisis.Servicios.Implementacion;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

using System.Net;
var builder = WebApplication.CreateBuilder(args);

//  inyección de la base de datos aASp.net
builder.Services.AddControllersWithViews();
//colocar la cadena de la base de datos se llama conexionBD para poder usarlo dentro del proyecto 
builder.Services.AddDbContext<BaseDeDatosUsuario>(options
    => options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionBD")));
//Se puede usar ese servicio dentro de cualqueir controlador 

builder.Services.AddScoped<Iusuariocs,UsuarioService>();
//Autenticación 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Inicio/IniciarSesion";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

builder.Services.AddControllersWithViews(options => {
    options.Filters.Add(
            new ResponseCacheAttribute
            {
                NoStore = true,
                Location = ResponseCacheLocation.None,
            }
        );
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Puedes ajustar el tiempo de expiración según tus necesidades
});


builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();

app.UseAuthentication(); 

app.UseAuthorization();



//Define la página con la que comienza el proyecto 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=IniciarSesion}/{id?}");

app.Run();
