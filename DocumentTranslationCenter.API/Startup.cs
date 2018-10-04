using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentTranslationCenter.API.Core.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DocumentTranslationCenter.API
{
    public class Startup
    {
        public const string DefaultOpen = "DefaultOPEN";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Inject dbcontext
            services
                .AddDbContext<DtcDbContext>(options => options
                .UseSqlServer(Configuration
                .GetConnectionString("DevConnection")));
            // add CORS
            services
                .AddCors(options => options
                .AddPolicy(Startup.DefaultOpen, builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
            ));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                using ( var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    DtcDbContext dbContext = serviceScope.ServiceProvider.GetService<DtcDbContext>();

                    dbContext.SeedData();
                    dbContext.CreateTriggers();
                }
            }

            app.UseMvc();
        }
    }
}
