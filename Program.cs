using API_Web.BDD;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

BDDConexion.EstablecerConexion();

// Add services to the container.
builder.Services.AddControllers();

// CORS de prueba
// Define el acceso a partir de los dominios
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "SuperSecretKey_ChangeMe_123!";
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "ClubesApiIssuer",
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "ClubesApiAudience",
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ClockSkew = TimeSpan.FromSeconds(5)
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();

            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";

            var result = new
            {
                status = 401,
                message = "Acceso no autorizado"
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(result));
        }
    };
});

builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Defino las agrupaciones de APIs para el swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("Clubes", new OpenApiInfo { Title = "API de Clubes", Version = "v1" });
    c.SwaggerDoc("Alumnos", new OpenApiInfo { Title = "API de Alumnos", Version = "v1" });
    c.SwaggerDoc("Historial", new OpenApiInfo { Title = "API de Historial", Version = "v1" });
    c.SwaggerDoc("Ejemplo", new OpenApiInfo { Title = "API de ejemplo", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Ingrese 'Bearer {token}'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement { { securityScheme, Array.Empty<string>() } });
});

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    // Defino las rutas para la interface
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/Clubes/swagger.json", "API de Clubes");
        c.SwaggerEndpoint("/swagger/Alumnos/swagger.json", "API de Alumnos");
        c.SwaggerEndpoint("/swagger/Historial/swagger.json", "API de Historial");
        c.SwaggerEndpoint("/swagger/Ejemplo/swagger.json", "API de Ejemplo");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
