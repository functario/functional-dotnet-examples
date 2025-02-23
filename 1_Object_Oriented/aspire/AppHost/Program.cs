using AlexandriaWebApi = WellKnowns.Presentation.AlexandriaWebApi.ProjectReferences;

var builder = DistributedApplication.CreateBuilder(args);

builder
    .AddProject<Projects.Alexandria_WebApi>(AlexandriaWebApi.ProjectName)
    .WithEndpoint("https", endpoint => endpoint.IsProxied = false)
    .WithEndpoint("http", endpoint => endpoint.IsProxied = false);

builder.Build().Run();
