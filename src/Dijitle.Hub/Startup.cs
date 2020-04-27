using Dijitle.Hub.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;

namespace Dijitle.Hub
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
        e.MapHub<CentralHub>("/hub");
      });

      app.UseHealthChecks("/health");
    }
  }
}
