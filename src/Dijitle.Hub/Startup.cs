using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dijitle.hub.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Prometheus;

namespace Dijitle.hub
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddHealthChecks();
      services.AddCors(c =>
      {
        c.AddPolicy("AllowOrigin", b =>
        {
          b.WithOrigins(Configuration.GetSection("CORsURLs").Get<string[]>())
           .AllowAnyHeader()
           .AllowAnyMethod().AllowCredentials();
        });
      });

      services.AddSignalR();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();
      app.UseCors("AllowOrigin");
      app.UseHttpMetrics();
      
      app.UseEndpoints(e => {
        e.MapMetrics();
        e.MapHub<hubHub>("/hub");
      });

      app.UseHealthChecks("/health");
    }
  }
}
