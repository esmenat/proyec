using Microsoft.EntityFrameworkCore;
using RaymiMusic.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:7029")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("RaymiMusicDb")));

// Registra controladores y configura Newtonsoft.Json
builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options =>
    {
        // Ignorar ciclos de referencia en entidades con navegación bidireccional
        options.SerializerSettings.ReferenceLoopHandling =
            Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        // (Opcional) Formato de fecha, indentado, etc.
        options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss";
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Activa CORS antes de Authorization
app.UseCors("PermitirFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
