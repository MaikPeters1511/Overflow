var builder = DistributedApplication.CreateBuilder(args);

// Keycloak hinzufügen (Identity Provider) mit Admin-Credentials
var keyCloak = builder
    .AddKeycloak("keycloak", port: 6001)
    .WithImage("keycloak/keycloak", "26.4.7-0")
    .WithContainerName("Keycloak-Server")
    .WithDataBindMount("./keycloak-data")
    .WithLifetime(ContainerLifetime.Persistent);

// Question Services 
var questionsService = builder.AddProject<Projects.QuestionServices>("Question-Services")
    .WithReference(keyCloak)
    .WaitFor(keyCloak);

builder.Build().Run();