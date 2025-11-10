using Serilog;
using UniMeet.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add serilog logger
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddTransient<ExceptionMiddleware>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Load modules
ModularSystem.ModularSystem.RunModules(builder.Configuration, builder.Services);
ModularSystem.ModularSystem.SummarizeModules();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UniMeet API V1");
    });
}

app.UseRouting();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();