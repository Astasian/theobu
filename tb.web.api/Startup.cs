using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orleans;
using Orleans.Clustering.Kubernetes;
using Orleans.Configuration;
using Orleans.Hosting;
using tb.datalayer;
using tb.web.api.Services;

namespace tb.web.api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IHostEnvironment _hostEnvironment;

        public Startup(IHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _hostEnvironment = hostEnvironment;
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<DatabaseContext>(options =>
                            options
                            .UseLazyLoadingProxies()
                            .UseNpgsql(
                                Configuration.GetConnectionString("Default"),
                                b => b.MigrationsAssembly("tb.web.api")));

            services.AddSingleton(ConfigureClusterClient);
            services.AddHostedService<ClusterClientService>();
            services.AddScoped<ICardDatalayer, CardDatalayer>();

            if (_hostEnvironment.IsDevelopment())
            {
                services.AddSwaggerDocument();
            }

            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        private IClusterClient ConfigureClusterClient(IServiceProvider sp)
        {
            var client = new ClientBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "clu01";
                    options.ServiceId = "theobu";
                })
                .ConfigureLogging(logging =>
                {
                    if (_hostEnvironment.IsDevelopment())
                    {
                        logging.SetMinimumLevel(LogLevel.Information);
                    }
                    else
                    {
                        logging.SetMinimumLevel(LogLevel.Warning);
                    }

                    logging.AddConsole();
                })
                .AddSimpleMessageStreamProvider("SMSProvider");

            if (_hostEnvironment.IsDevelopment())
            {
                client.UseLocalhostClustering();
            }
            else
            {
                client.UseKubeGatewayListProvider();
            }

            return client.Build();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseOpenApi();
                app.UseSwaggerUi3();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
