using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

namespace Dijitle.Chat
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
      services.AddControllers();
      services.AddMvcCore().AddApiExplorer();

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dijitle Chat API", Version = "v1" });
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);

      });

      services.AddHealthChecks();
      services.AddCors(c =>
      {
        c.AddPolicy("AllowOrigin", b =>
        {
          b.WithOrigins(Configuration.GetSection("CORsURLs").Get<string[]>())
           .AllowAnyHeader()
           .AllowAnyMethod();
        });
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      app.UseCors("AllowOrigin");

      app.UseRouting();
      app.UseStaticFiles();
      app.UseHttpMetrics();
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dijitle Chat API V1");
        c.RoutePrefix = "api";
        c.EnableDeepLinking();
        c.EnableFilter();
        c.InjectJavascript("https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js");
        c.InjectJavascript("/js/swagger.js");
        c.InjectStylesheet("/css/swagger.css");
      });

      app.UseEndpoints(e => {
        e.MapControllers();
        e.MapMetrics();
      });

      app.UseHealthChecks("/health");
    }
  }
}
