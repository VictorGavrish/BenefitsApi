using Api.Database;
using Api.Logic;
using Api.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure Sqlite DB
var dbConfig = new DatabaseConfig
{
    Name = "Data Source=db.sqlite"
};

var dbInit = new DatabaseInit(dbConfig);
await dbInit.Init();

builder.Services.AddSingleton(dbConfig);
builder.Services.AddSingleton<DatabaseConnection>();

// logic
builder.Services.AddSingleton<PaycheckCalculator>();

// services
builder.Services.AddSingleton<DependentService>();
builder.Services.AddSingleton<EmployeeService>();
builder.Services.AddSingleton<ValidationService>();

// infra
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Employee Benefit Cost Calculation Api",
        Description = "Api to support employee benefit cost calculations"
    });
});

var allowLocalhost = "allow localhost";
builder.Services.AddCors(options =>
{
    options.AddPolicy(allowLocalhost,
        policy => { policy.WithOrigins("http://localhost:3000", "http://localhost"); });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(allowLocalhost);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
