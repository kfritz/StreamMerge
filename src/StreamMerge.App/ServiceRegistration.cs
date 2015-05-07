using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.DependencyInjection;
using StreamMerge.App.Quiz;

namespace StreamMerge.App
{
    public static class ServiceRegistration
    {
        public static void AddStreamMergeAppServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IStreamRepository), typeof(StreamRepository));
            services.AddTransient(typeof(IStreamClient), typeof(StreamClient));
            services.AddSingleton(typeof(IStreamStore), typeof(StreamStore));
        }
    }
}
