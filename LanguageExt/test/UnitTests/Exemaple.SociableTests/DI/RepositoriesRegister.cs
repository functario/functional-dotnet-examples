//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.DependencyInjection.Extensions;

//namespace Example.SociableTests.DI;
//internal static class RepositoriesRegister
//{


//    private static IServiceCollection RegisterTestRepositoriesInWebApplication(
//        this IServiceCollection webApplicationBuilderServices,
//        ServiceProvider serviceProvider,
//        IEnumerable<ServiceDescriptor> serviceDescriptors
//    )
//    {
//        foreach (var serviceDescriptor in serviceDescriptors)
//        {
//            var mockRepository = serviceProvider.GetService(serviceDescriptor.ServiceType);
//            var newDescriptor = ServiceDescriptor.Transient(
//                serviceDescriptor.ServiceType,
//                _ => mockRepository!
//            );

//            webApplicationBuilderServices.Replace(newDescriptor);
//        }

//        return webApplicationBuilderServices;
//    }

//    private static IServiceCollection RegisterTestRepositoriesInTestAssembly(
//        this IServiceCollection services
//    )
//    {
//        var repositoryAssembly = Assembly.Load(LighthousePersistenceProject);
//        var repositories = repositoryAssembly
//            .GetTypes()
//            .Where(t => typeof(IRepository).IsAssignableFrom(t));

//        foreach (var repository in repositories)
//        {
//            var repositoryInterface = repository
//                .GetInterfaces()
//                .First(i => i != typeof(IRepository));

//            var mockRepository = Substitute.For([repositoryInterface], null);
//            services.AddScoped(repositoryInterface, _ => mockRepository);
//        }

//        return services;
//    }
//}
