using AccesoDatos.Contexto;
using AccesoDatos.Servicios;
using Microsoft.EntityFrameworkCore;
using Modelos;
using Negocio;
using Seguridad;


EncryptionService encryptionService = new EncryptionService();  

var builder = WebApplication.CreateBuilder(args);

// Configura la conexión a la base de datos
IConfiguration configuration = builder.Configuration;
builder.Services.AddDbContext<ApplicationEFDbContext>(options =>
    options.UseSqlServer(encryptionService.Decrypt(configuration!.GetConnectionString("ConexionMensajeriaEscritura")!))
);

// Registrar el servicio de acceso a datos (DataServiceADO)
builder.Services.AddScoped<DataServiceADO>();

// Registrar EventoNegocio como implementación de IEventoService
builder.Services.AddScoped<IEventoService, EventoNegocioAdo>();
builder.Services.AddScoped<IEventoNegocioEf, EventoNegocioEf>();
// Registrar otros servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
