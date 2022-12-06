using Microsoft.Extensions.DependencyInjection;
using Strider.Posterr.Domain.Services;

namespace Strider.Posterr.Service.Extensions;

public static class ServicesRegisterExtensions
{ 
    public static void RegisterServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IPostService, PostService>();
    }
}