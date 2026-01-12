using Microsoft.IdentityModel.Tokens;
using Overflow.ServiceDefaults;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.AddServiceDefaults();

builder.Services.AddAuthentication()
    .AddKeycloakJwtBearer(serviceName: "keycloak", realm:"overflow",
        options =>
        {
            options.Authority = "http://localhost:6001/realms/overflow"; // muss exakt dem 'iss' im Token entsprechen
            
            // Performance-Optimierung: Metadaten direkt über IPv4 abrufen, um localhost DNS-Timeouts (~1s) zu vermeiden
            options.MetadataAddress = "http://127.0.0.1:6001/realms/overflow/.well-known/openid-configuration";
            
            options.RequireHttpsMetadata = false; // nur für Entwicklung
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = "overflow", 
                ValidateIssuer = true,
                // Optional: explizit gültigen Issuer setzen
                ValidIssuer = "http://localhost:6001/realms/overflow"
            };
        });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("My Question API");
    });

}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapDefaultEndpoints();
app.Run();