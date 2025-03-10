using AppHost.Configurations;
using WellKnowns.Aspires;

var context = Enum.TryParse(args.FirstOrDefault(), true, out AspireContexts ctx)
    ? ctx
    : AspireContexts.Local;

var builder = DistributedApplication.CreateBuilder(args);

builder = context switch
{
    AspireContexts.Local => builder.ConfigureAsLocal(),
    AspireContexts.Test => builder.ConfigureAsTest(),
    _ => throw new NotImplementedException(),
};

builder.Build().Run();
