using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using System.IO;
using netlush.Models;
using netlush.Helpers;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using System;

namespace netlush
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
      // SQL server connection
      var sqlDbSettings = Configuration.GetSection("ConnectionStrings").Get<DbConnectionString>();
      services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(sqlDbSettings.sqlDb), ServiceLifetime.Transient);

      var o = new DbContextOptionsBuilder<AppDbContext>();
      o.UseSqlServer(sqlDbSettings.sqlDb);
      using (var context = new AppDbContext(o.Options))
      {
        context.Database.EnsureCreated();
        // if (!context.users.Any())
        //     dbInit(context);
      }

      services.AddMvc();

      // set max value for file limit
      services.Configure<FormOptions>(x =>
      {
        x.ValueLengthLimit = int.MaxValue;
        x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });

      // Added for authentication
      // configure strongly typed settings objects
      var appSettingsSection = Configuration.GetSection("AppSettings");
      services.Configure<AppSettings>(appSettingsSection);
      // configure jwt authentication
      var appSettings = appSettingsSection.Get<AppSettings>();
      var key = Encoding.ASCII.GetBytes(appSettings.Secret);
      services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(x =>
      {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false
        };
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      app.UseAuthentication();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      //Redirect non api calls to angular app that will handle routing of the app.    
      app.Use(async (context, next) =>
      {
        await next();
        if (context.Response.StatusCode == 404 &&
                 !Path.HasExtension(context.Request.Path.Value) &&
                 !context.Request.Path.Value.StartsWith("/api/"))
        {
          context.Request.Path = "/index.html";
          await next();
        }
      });

      // configure the app to serve index.html from /wwwroot folder    
      app.UseDefaultFiles();
      app.UseStaticFiles();

      // configure the app for usage as api
      app.UseMvcWithDefaultRoute();

      app.UseMvc();

      app.UseSpa(spa =>
      {
        spa.Options.SourcePath = Environment.CurrentDirectory;

        spa.UseSpaPrerendering(options =>
        {
          options.BootModulePath = $@"{spa.Options.SourcePath}\dist\server\main.js";
          options.BootModuleBuilder = env.IsDevelopment()
              ? new AngularCliBuilder(npmScript: "build:ssr")
              : null;
          options.ExcludeUrls = new[] { "/sockjs-node" };
        });

        if (env.IsDevelopment())
        {
          spa.UseAngularCliServer(npmScript: "start");
        }
      });
    }
  }
}
