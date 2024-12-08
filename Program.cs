using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Stackoverflow_Lite.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file
Env.Load();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 3, 0))));

// Add services relying on OIDC (using Keycloak as provider) 
KeycloakConfiguration.Initialize(builder.Configuration);
builder.Services.AddSwaggerWithKeycloak();
builder.Services.AddKeycloakAuthentication();
builder.Services.AddKeycloakAuthorization();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "StackOveflow-Lite API");
        c.OAuthClientId("LightClientID"); 
        c.OAuthAppName("My API - Swagger");
        c.OAuthUsePkce(); 
        c.OAuthScopes("openid");
    });}

app.UseHttpsRedirection();



app.Run();
