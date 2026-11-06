using DSW_3P_PR01.Repositories;
using DSW_3P_PR01.Services;
using DSW_3P_PR01.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddSingleton<IMisionRepository, MisionJsonRepository>();
builder.Services.AddScoped<IMisionService, MisionService>();

//Configuracion de sql
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServerConnection")
    )
);

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Catalogo de misiones");
    });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
