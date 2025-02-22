using ExampleWebApi = WellKnowns.Presentation.ExampleWebApi.ProjectReferences;

var builder = DistributedApplication.CreateBuilder(args);

builder
    .AddProject<Projects.Example_WebApi>(ExampleWebApi.ProjectName)
    .WithEndpoint("https", endpoint => endpoint.IsProxied = false)
    .WithEndpoint("http", endpoint => endpoint.IsProxied = false);

builder.Build().Run();
