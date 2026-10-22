using DesafioProfessores.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração única de CORS para aceitar requisições do Frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IProfessorRepository, JsonProfessorRepository>();
builder.Services.AddTransient<BuscadorProfessoresService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Aplica a política de CORS definida acima
app.UseCors("AllowReactApp");

app.UseAuthorization();
app.MapControllers();

app.Run();