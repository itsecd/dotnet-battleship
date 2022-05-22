using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

namespace Battleship.Server.Services
{
    public class HostedServiceWrapper<THostedService> : IHostedService
    {
        private readonly IHostedService _hostedService;

        public HostedServiceWrapper(THostedService service)
        {
            if (service is not IHostedService hostedService)
                throw new InvalidOperationException();

            _hostedService = hostedService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _hostedService.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _hostedService.StopAsync(cancellationToken);
        }
    }
}
