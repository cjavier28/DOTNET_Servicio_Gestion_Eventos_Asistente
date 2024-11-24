

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AccesoDatos;
using  Negocio;
using Modelos;


var builder = WebApplication.CreateBuilder(args);
var databaseType = builder.Configuration["DatabaseType"];


if (databaseType == "EntityFramework")
{
    builder.Services.AddScoped<IEventoService, EventoServiceEF>();
   // builder.Services.AddScoped<IEventoRepository, EventoRepositoryEF>();
}
else
{
   // builder.Services.AddScoped<IEventoService, EventoServiceADO>();
   // builder.Services.AddScoped<IEventoRepository, EventoRepositoryADO>();
}
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
