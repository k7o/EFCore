using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Grpc.Net.Client.Web;
using BlogShared;

namespace BlogWebappBlazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

			// Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services
                .AddGrpcClient<BlogProto.BlogProtoClient>(options =>
                {
                    options.Address = new Uri("https://localhost:5001");
                })
                .ConfigurePrimaryHttpMessageHandler(
                    () => new GrpcWebHandler(new HttpClientHandler()));

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
}
