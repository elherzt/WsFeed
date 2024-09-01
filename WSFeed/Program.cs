using DataLayer;
using DataLayer.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WSFeed.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.




//db reference
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=DBWsFeeed.db"));

//repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

//jwt
//builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("JWTConfig"));
//builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<JWTConfig>>().Value);
builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("JWTConfig"));
builder.Services.AddScoped<IJWTGenerator, JWTGenerator>();


builder.Services.AddControllers(); 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Aplicar migraciones al iniciar la aplicación
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
