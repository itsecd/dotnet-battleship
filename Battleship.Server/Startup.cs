using System;

using Battleship.Server.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Battleship.Server
{
    public sealed class Startup
    {
        public Startup(IConfiguration rootConfiguration)
        {
            _configuration = new Configuration();
            rootConfiguration.GetSection(nameof(Configuration)).Bind(_configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMatchmakingService, MatchmakingService>();
            services.AddSingleton<Configuration>(_configuration);
            services.AddSingleton<GameService>();

            services.AddHostedService<HostedServiceWrapper<IMatchmakingService>>();

            services.AddGrpc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => endpoints.MapGrpcService<EntryPointService>());
        }

        private readonly Configuration _configuration;
    }
}
