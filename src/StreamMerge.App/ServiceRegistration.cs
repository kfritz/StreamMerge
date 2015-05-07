using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.DependencyInjection;
using StreamMerge.App.Quiz;

namespace StreamMerge.App
{
    /// <summary>
    /// Extension methods that register the services in the StreamMerge.App library with dependency 
    /// injection.
    /// </summary>
    public static class ServiceRegistration
    {
        /// <summary>
        /// Registers services from StreamMerge.App with ASP.net 5 Dependency Injection.
        /// </summary>
        /// <remarks>
        /// This is implemented as an extension method on <see cref="IServiceCollection"/>, in 
        /// keeping with the pattern illustrated on the 
        /// <a href="https://weblogs.asp.net/scottgu/introducing-asp-net-5">ASP.net blog</a>.
        /// </remarks>
        /// <param name="services">A contract for a collection of service descriptors.</param>
        public static void AddStreamMergeAppServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IStreamRepository), typeof(StreamRepository));
            services.AddTransient(typeof(IStreamClient), typeof(StreamClient));
            services.AddSingleton(typeof(IStreamStore), typeof(StreamStore));
        }
    }
}
