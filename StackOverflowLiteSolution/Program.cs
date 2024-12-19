using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Stackoverflow_Lite.Repositories;
using Stackoverflow_Lite.Services;
using Stackoverflow_Lite.Configurations;
using Stackoverflow_Lite.Repositories;
using Stackoverflow_Lite.services;
using Stackoverflow_Lite.Services;
using Stackoverflow_Lite.Services.Interfaces;
using Stackoverflow_Lite.Utils;
using Stackoverflow_Lite.Utils.Interfaces;

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

builder.Services.AddControllers();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenClaimsExtractor, TokenClaimsExtractor>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();

// add in memory caching service 
builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Services.AddMemoryCache();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "StackOveflow-Lite API");
        c.OAuthClientId("LiteClientID"); 
        c.OAuthAppName("My API - Swagger");
        c.OAuthUsePkce(); 
        c.OAuthScopes("openid");
    });}

app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();


app.Run();
