using ExampleWebApi = WellKnowns.Presentation.ExampleWebApi.ProjectReferences;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Example_WebApi>(ExampleWebApi.ProjectName);

builder.Build().Run();
