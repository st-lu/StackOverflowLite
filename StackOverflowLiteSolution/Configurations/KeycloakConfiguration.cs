using System.Reflection;

namespace Stackoverflow_Lite.Configurations;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

public static class KeycloakConfiguration
{
    private static string _authUrl;
    private static string _tokenUrl;
    private static string _authority;
    private static string _audience;
    
    //Inject values from appsettings.json 
    
    public static void Initialize(IConfiguration configuration)
    {
        _authUrl = configuration["OidcValues:AUTHORIZATION_URL"];
        _tokenUrl = configuration["OidcValues:TOKEN_URL"];
        _authority = configuration["OidcValues:AUTHORITY"];
        _audience = configuration["OidcValues:AUDIENCE"];
    }
    
    public static void AddSwaggerWithKeycloak(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Stackoverflow - Lite", Version = "v1" });

            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(_authUrl),
                        TokenUrl = new Uri(_tokenUrl),
                        Scopes = new Dictionary<string, string>
                        {
                            { "openid", "OpenID Connect scope" }
                        }
                    }
                }
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    new[] { "openid" }
                }
            });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
            c.EnableAnnotations();

        });
    }

    public static void AddKeycloakAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = _authority;
                options.Audience = _audience; 
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RoleClaimType = "realm_access",
                    ValidIssuer = "http://localhost:8081/realms/Stackoverflow-Lite"
                };
            });
    }

    // checking if the token was issues for an administrator (i.e. it contains the "admin" claim configured on the OIDC provider side)
    public static void AddKeycloakAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireAssertion(context =>
                {
                    var rolesClaim = context.User.Claims
                        .FirstOrDefault(c => c.Type == "realm_access")?.Value;
                    
                    // check if the token payload contains the "admin" claim (as part of the roles list) 
                    
                    if (rolesClaim != null)
                    {
                        var roles = JsonDocument.Parse(rolesClaim).RootElement
                            .GetProperty("roles").EnumerateArray()
                            .Select(r => r.GetString())
                            .ToArray();

                        return roles.Contains("admin");
                    }

                    return false;
                })
            );
        });
    }
}
