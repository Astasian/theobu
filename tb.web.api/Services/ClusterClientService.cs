using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace tb.web.api.Services
{
    class ClusterClientService : IHostedService
    {
        private readonly ILogger logger;
        private readonly IClusterClient clusterClient;

        public ClusterClientService(ILogger<ClusterClientService> logger, IClusterClient clusterClient)
        {
            this.logger = logger;
            this.clusterClient = clusterClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Connect to silo");

            // Retry initial connection for all of eternity
            await clusterClient.Connect(async _ =>
            {
                try
                {
                    await Task.Delay(10000, cancellationToken);
                }
                catch { }

                return !cancellationToken.IsCancellationRequested;
            });

            logger.LogInformation("Client successfully connected to silo host");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (clusterClient.IsInitialized)
            {
                await clusterClient.Close();
            }
        }
    }
}
