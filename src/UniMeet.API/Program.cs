using Microsoft.OpenApi.Models;
using Serilog;
using UniMeet.API.Hubs;
using UniMeet.API.Middlewares;
using UniMeet.MessagingModule.Application.Messages;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add serilog logger
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddTransient<ExceptionMiddleware>();

// CORS — allow all origins in dev (required for SignalR from browser/file://)
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevPolicy", policy =>
    {
        policy
            .SetIsOriginAllowed(_ => true) // dowolny origin (dev only)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // wymagane przez SignalR
    });
});

// SignalR for real-time messaging
builder.Services.AddSignalR();
builder.Services.AddScoped<IMessagingHubNotifier, MessagingHubNotifier>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Load modules
ModularSystem.ModularSystem.RunModules(builder.Configuration, builder.Services);
ModularSystem.ModularSystem.SummarizeModules();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Serve wwwroot (chat.html dev tester)
app.UseStaticFiles();

// Create uploads directory if it doesn't exist
var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

// Serve static files from uploads directory
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        uploadsPath),
    RequestPath = "/uploads"
});

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
app.UseCors("DevPolicy");
app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.MapHub<MessagingHub>("/hubs/messaging");

app.Run();