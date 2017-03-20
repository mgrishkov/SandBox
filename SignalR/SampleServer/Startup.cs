using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RecurrentTasks;
using SampleServer.Hubs;

namespace SampleServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddSignalR(options =>
            {
                options.Hubs.EnableDetailedErrors = true;
            });

            services.AddMvc()
                .AddJsonOptions(o => o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver()) // Force Camel Case to JSON
                .AddJsonOptions(o => { o.SerializerSettings.NullValueHandling = NullValueHandling.Ignore; });


            services.AddTask<TimeEngine>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole()
                .AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials());

            app.UseMvc();

            app.UseWebSockets();
            
            app.UseSignalR();

            app.StartTask<TimeEngine>(TimeSpan.FromSeconds(5));
        }
    }
}
