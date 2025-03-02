using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Alexandria.SociableTests;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Justification = "Requested for DI resolution with xunit."
)]
public sealed class WebAppServicesFactory
{
    private readonly WebApplication _app;

    public WebAppServicesFactory(WebApplication app)
    {
        _app = app;
    }

    public T CreateService<T>()
    {
        var serviceType = typeof(T);

        using var scope = _app.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService(serviceType);
        return (T)service;
    }

    public (T1 service1, T2 service2) CreateServices<T1, T2>()
    {
        var type1 = typeof(T1);
        var type2 = typeof(T2);

        var objects = CreateScopeMany(type1, type2);

        return ((T1)objects[1], (T2)objects[2]);
    }

    public (T1 service1, T2 service2, T3 service3) CreateServices<T1, T2, T3>()
    {
        var type1 = typeof(T1);
        var type2 = typeof(T2);

        var objects = CreateScopeMany(type1, type2);

        return ((T1)objects[1], (T2)objects[2], (T3)objects[3]);
    }

    private object[] CreateScopeMany(params Type[] types)
    {
        var objects = new List<object>();
        using var scope = _app.Services.CreateScope();
        foreach (var type in types)
        {
            var obj = scope.ServiceProvider.GetRequiredService(type);
            objects.Add(obj);
        }

        return [.. objects];
    }
}
