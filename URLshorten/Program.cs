using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShortenURL.Abstractions;
using ShortenURL.Services;
using URLshorten.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<URLshortenContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("URLshortenContext") ?? throw new InvalidOperationException("Connection string 'URLshortenContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUrlShorteningService, UrlShorteningService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
