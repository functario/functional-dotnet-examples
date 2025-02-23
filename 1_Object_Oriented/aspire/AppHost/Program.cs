using AppHost.Local;

var builder = DistributedApplication.CreateBuilder(args);

builder.Configure();

builder.Build().Run();
