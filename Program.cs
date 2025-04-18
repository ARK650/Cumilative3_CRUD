using Microsoft.EntityFrameworkCore;
using Cumilative3_CRUD.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
       .AddRazorRuntimeCompilation();

builder.Services.AddDbContext<SchoolDbContext>(opts =>
  opts.UseMySql(
    builder.Configuration.GetConnectionString("SchoolDb"),
    new MySqlServerVersion(new Version(8, 0, 33))
  )
);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();
