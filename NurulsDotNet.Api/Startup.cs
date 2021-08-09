using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.ReDoc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NurulsDotNet.Data.Models.Configs;
using NurulsDotNet.Data.Queries;
using NurulsDotNet.Services.ApiKeyServices;
using NurulsDotNet.Services.OrgServices;
using NurulsDotNet.Services.UserServices;
using NurulsDotNet.Api.Middlewares;

namespace NurulsDotNet.Api
{
  /// <summary>
  /// Startup
  /// </summary>
  public class Startup
  {
    private readonly string _corsPolicyName = "3Status-CorsPolicy";
    /// <summary>
    /// Startup CTOR
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    /// <summary>
    /// Configuration
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {

      #region Add configuration
      services.Configure<DatabaseConfig>(Configuration.GetSection("DatabaseConfig"));
      services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));
      #endregion

      #region Register objects
      //Database
      services.AddSingleton(CreateDatabaseConfig);
      services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();


      //API Key
      services.AddSingleton<IApiKeyQueries, ApiKeyQueries>();
      services.AddSingleton<IApiKeyService, ApiKeyService>();

      //User
      services.AddSingleton<IUserQueries, UserQueries>();
      services.AddSingleton<IUserService, UserService>();

      //Org
      services.AddSingleton<IOrgQueries, OrgQueries>();
      services.AddSingleton<IOrgService, OrgService>();
      #endregion

      services.AddControllers();
      services.AddRouting(CreateRoutingConfig);
      services.AddSwaggerGen(CreateSwaggerDoc);
      services.AddCors(CreateCors);
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
        app.UseDeveloperExceptionPage();

      app.UseSwagger();
      app.UseSwaggerUI(CreateSwaggerUI);
      app.UseReDoc(CreateReDoc);
      app.UseHttpsRedirection();
      app.UseRouting();
      app.UseCors(_corsPolicyName);
      app.UseAuthorization();

      #region Middlewares
      app.UseMiddleware<ErrorHandlerMiddleware>();
      app.UseMiddleware<JwtMiddleware>();
      app.UseMiddleware<SubdomainMiddleware>();
      #endregion

      app.UseEndpoints(x => x.MapControllers());
    }

    #region Helpers

    private DatabaseConfig CreateDatabaseConfig(IServiceProvider provider)
    {
      var config = provider.GetService<IOptions<DatabaseConfig>>().Value;
      return new DatabaseConfig
      {
        MainDatabaseConnectionString = config.MainDatabaseConnectionString,
        ReadOnlyDatabaseConnectionString = config.ReadOnlyDatabaseConnectionString
      };
    }

    private void CreateSwaggerDoc(SwaggerGenOptions c)
    {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "NurulsDotNet.Api", Version = "v1" });
      c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      {
        Description = "JWT Authorization header using the Bearer scheme. \n Enter 'Bearer' [space] and then your token in the text input below. \n Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
      });

      c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
      {
        Description = "API Key authentication. \n Pass API Key through x-api-key header",
        Type = SecuritySchemeType.ApiKey,
        Name = "x-api-key", //TODO: Take from appsettings.json
        In = ParameterLocation.Header,
      });

      c.AddSecurityDefinition("ApiKeyQuery", new OpenApiSecurityScheme()
      {
        Description = "API Key authentication. \n Pass API Key through apiKey query param",
        Type = SecuritySchemeType.ApiKey,
        Name = "apiKey", //TODO: Take from appsettings.json
        In = ParameterLocation.Query,
      });

      c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header,
          },
            new List<string>()
          }
        });

      c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" },
            Scheme = "ApiKey",
            Name = "x-api-key",
            In = ParameterLocation.Header,
          },
          new List<string>()
          }
        });

      c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKeyQuery" },
            Scheme = "ApiKeyQuery",
            Name = "apiKey",
            In = ParameterLocation.Query,
          },
          new List<string>()
          }
        });

      foreach (var filePath in System.IO.Directory.GetFiles(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)), "*.xml"))
      {
        try
        {
          c.IncludeXmlComments(filePath);
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
        }
      }
    }

    private void CreateSwaggerUI(SwaggerUIOptions c)
    {
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "3Status API V1");
    }

    private void CreateReDoc(ReDocOptions c)
    {

      c.SpecUrl("/swagger/v1/swagger.json");
      c.RoutePrefix = "docs";
      c.DocumentTitle = "3Status API";
      c.DisableSearch();
      c.HeadContent =
          "<link rel='icon' type='image/png' sizes='32x32' href='https://porichoy.gov.bd/apps/porichoy-favicon.png'>";
      c.ConfigObject.AdditionalItems["theme"] = new { colors = new { primary = new { main = "#663399" } } };

    }

    private void CreateRoutingConfig(RouteOptions options)
    {
      options.LowercaseQueryStrings = true;
      options.LowercaseUrls = true;
    }
    private void CreateCors(CorsOptions options)
    {
      options.AddPolicy(name: _corsPolicyName, builder =>
      {
        builder.WithOrigins(
          "https://3statusuidev.z21.web.core.windows.net",
          "http://localhost:8080")
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials();
      });

    }

    #endregion
  }
}
