using CasoPractico1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Servicio
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<TransporteDbContext>(op =>
{ 
    op.UseSqlServer(builder.Configuration.GetConnectionString("TransporteDb"));

});

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TransporteDbContext>();
    if (context.Database.CanConnect())
    {
        Console.WriteLine("Conexión exitosa a la base de datos.");
    }
    else
    {
        Console.WriteLine("No se pudo conectar a la base de datos.");
    }
}


app.Run();
