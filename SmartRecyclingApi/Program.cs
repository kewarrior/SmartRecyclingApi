using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using SmartRecyclingApi.Data;
using SmartRecyclingApi.Services.Administrador;
using SmartRecyclingApi.Services.Contentores;
using SmartRecyclingApi.Services.Operador;
using SmartRecyclingApi.Services.Pedido;
using SmartRecyclingApi.Services.Utilizador;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IUtilizadorInterface, UtilizadorService>();
builder.Services.AddScoped<IPedidoInterface, PedidoService>();
builder.Services.AddScoped<IOperadorInterface, OperadorService>();
builder.Services.AddScoped<IContentoresInterface, ContentoresService>();
builder.Services.AddScoped<IAdminInterface, AdministradorService>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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
