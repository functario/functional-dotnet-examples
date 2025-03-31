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

        return ((T1)objects[0], (T2)objects[1]);
    }

    public (T1 service1, T2 service2, T3 service3) CreateServices<T1, T2, T3>()
    {
        var type1 = typeof(T1);
        var type2 = typeof(T2);
        var type3 = typeof(T3);

        var objects = CreateScopeMany(type1, type2, type3);

        return ((T1)objects[0], (T2)objects[1], (T3)objects[2]);
    }

    public (T1 service1, T2 service2, T3 service3, T4 service4) CreateServices<T1, T2, T3, T4>()
    {
        var type1 = typeof(T1);
        var type2 = typeof(T2);
        var type3 = typeof(T3);
        var type4 = typeof(T4);

        var objects = CreateScopeMany(type1, type2, type3, type4);

        return ((T1)objects[0], (T2)objects[1], (T3)objects[2], (T4)objects[3]);
    }

    public (T1 service1, T2 service2, T3 service3, T4 service4, T5 service5) CreateServices<
        T1,
        T2,
        T3,
        T4,
        T5
    >()
    {
        var type1 = typeof(T1);
        var type2 = typeof(T2);
        var type3 = typeof(T3);
        var type4 = typeof(T4);
        var type5 = typeof(T5);

        var objects = CreateScopeMany(type1, type2, type3, type4, type5);

        return ((T1)objects[0], (T2)objects[1], (T3)objects[2], (T4)objects[3], (T5)objects[4]);
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
