using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Clustering.Kubernetes;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;
using Orleans.Statistics;
using Orleans.Versions.Compatibility;
using Orleans.Versions.Selector;
using OrleansDashboard;

namespace tb.actor.silo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ThreadPool.SetMinThreads(200, 200);

            var host = new HostBuilder()
                .ConfigureHostConfiguration((config) =>
                {
                    config.AddEnvironmentVariables();
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());

                    config.AddJsonFile("appsettings.json", optional: false)
                        .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);

                    config
                        .AddEnvironmentVariables()
                        .AddCommandLine(args);
                })
                // Configs
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<DashboardOptions>(hostContext.Configuration.GetSection("Dashboard"));
                    services.AddSingleton(s => s.GetService<IOptions<ApplicationInsightsTelemetryConsumerOptions>>().Value);
                })
                .ConfigureServices((hostCtx, services) =>
                {
                    // Wait max 5 minute for a gracefully shutdown.
                    services.Configure<HostOptions>(opts => opts.ShutdownTimeout = TimeSpan.FromMinutes(5));
                })
                .ConfigureLogging((hostCtx, lb) =>
                {
                    lb.AddConfiguration(hostCtx.Configuration.GetSection("Logging"));
                    lb.AddConsole();
                })
                .UseOrleans((hostContext, siloBuilder) =>
                {
                    siloBuilder
                       // .AddApplicationInsightsTelemetryConsumer()
                        .Configure<LoadSheddingOptions>(opts =>
                        {
                            // Only activate load shedding if we'are on linux.
                            // For other platforms, a IHostEnvironmentStatistics provider must first be installed.
                            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                            {
                                opts.LoadSheddingEnabled = true;
                            }
                            // Limit to 90% CPU usage.
                            opts.LoadSheddingLimit = 90;
                        })
                        .Configure<PerformanceTuningOptions>(opts =>
                        {
                            opts.Expect100Continue = false;
                            opts.UseNagleAlgorithm = false;
                        })
                        .Configure<ClusterOptions>(opts =>
                        {
                            opts.ClusterId = "clu01";
                            opts.ServiceId = "theobu";
                        })
                        // Cleanup old membership entries.
                        .Configure<ClusterMembershipOptions>(opts =>
                        {
                            opts.DefunctSiloCleanupPeriod = TimeSpan.FromMinutes(5);
                            opts.DefunctSiloExpiration = TimeSpan.FromMinutes(1);
                            opts.ProbeTimeout = TimeSpan.FromSeconds(5);
                            opts.NumMissedProbesLimit = 2;
                        })
                        // Use rolling release strategy
                        .Configure<GrainVersioningOptions>(options =>
                        {
                            options.DefaultCompatibilityStrategy = nameof(BackwardCompatible);
                            options.DefaultVersionSelectorStrategy = nameof(AllCompatibleVersions);
                        })
                        // To workaround timer reentracy.
                        // See https://github.com/dotnet/orleans/issues/2574
                        .Configure<SchedulingOptions>(opts => opts.AllowCallChainReentrancy = false)
                        .ConfigureServices(services =>
                        {
                            // Register readiness lifecycle participant.
                            services.AddTransient<ILifecycleParticipant<ISiloLifecycle>, ReadinessLifecycleParticipant>();
                        })
                        .ConfigureApplicationParts(parts => parts.ConfigureDefaults())
                        .AddSimpleMessageStreamProvider("SMSProvider")
                        .AddMemoryGrainStorage("PubSubStore");

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        siloBuilder.UseLinuxEnvironmentStatistics();
                    }

                    if (hostContext.HostingEnvironment.IsDevelopment())
                    {
                        siloBuilder
                            .UseDashboard()
                            .UseLocalhostClustering();
                    }
                    else
                    {
                        siloBuilder
                            .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000)
                            .UseDashboard()
                            .UseKubeMembership();
                    }
                })
                .UseConsoleLifetime()
                .Build();

            await host.RunAsync();
        }
    }
}
