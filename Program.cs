using API_Web.Conexion;

var builder = WebApplication.CreateBuilder(args);

HistorialSQL sqlcon = new HistorialSQL();
BDDLocal.HistorialConexion = sqlcon;
if (!sqlcon.TestConnection())
{
    Console.WriteLine("Error al acceder a la tabla: ORDERS_HISTORY, se accedera a la bdd local de ORDERS_HISTORY");
    BDDLocal.HistorialConexion = new HistorialLocal();
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
