using DataLayer;
using Microsoft.EntityFrameworkCore;
using WSFeed.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//jwt
builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("JWTConfig"));
builder.Services.AddScoped<IJWTGenerator, JWTGenerator>();


//db reference
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=DBWsFeeed.db"));

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
