using Dijitle.Hub.Hubs;
using Dijitle.Hub.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Prometheus;
using System;
using System.Net.Http;
using System.Threading.Tasks;

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

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(options =>
      {
        options.Authority = $"https://{Configuration["Auth0:Domain"]}/";
        options.Audience = Configuration["Auth0:Audience"];
        options.Events = new JwtBearerEvents
        {
          
          OnMessageReceived = context =>
          {
            var accessToken = context.Request.Query["access_token"];

            // If the request is for our hub...
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/hub")))
            {              
              // Read the token out of the query string
              context.Token = accessToken;
            }
            return Task.CompletedTask;
          }
        };
      });

      services.AddSingleton(_ => new Messages());
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

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseHttpMetrics();

      app.UseEndpoints(e =>
      {
        e.MapMetrics();
        e.MapHub<CentralHub>("/hub");
      });

      app.UseHealthChecks("/health");
    }
  }
}
