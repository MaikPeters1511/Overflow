var builder = DistributedApplication.CreateBuilder(args);

// Keycloak hinzufügen (Identity Provider) mit Admin-Credentials
var keyCloak = builder
    .AddKeycloak("keycloak", port: 6001)
    .WithImage("keycloak/keycloak", "26.4.7-0")
    .WithContainerName("Keycloak-Server")
    .WithDataBindMount("./keycloak-data")
    .WithLifetime(ContainerLifetime.Persistent);


builder.Build().Run();