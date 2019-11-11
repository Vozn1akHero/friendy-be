using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Helpers;
using BE.Interfaces;
using BE.Middlewares;
using BE.Repositories.RepositoryServices.Interfaces.User;
using BE.RepositoryServices.User;
using BE.Services;
using BE.SignalR.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace BE
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR();
            services.ConfigureRepositoryWrapper();
            
            services.AddSingleton<IFileProvider>(  
                new PhysicalFileProvider(  
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            //services.AddDirectoryBrowser();
            services.AddDirectoryBrowser();
            
            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("AppSettings:JWTSecret"));
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

            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAvatarConverterService, AvatarConverterService>();
            services.AddScoped<ICustomSqlQueryService, CustomSqlQueryService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddScoped<IUserSearchingService, UserSearchingService>();
            
            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            
            services.ConfigureSqlContext(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
/*            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                RequestPath = "/images"
            });*/

            app.UseStaticFiles();
            
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                RequestPath = "/wwwroot",
                EnableDirectoryBrowsing = true
            });
            
            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            //app.UseHttpsRedirection();

            app.UseMiddleware<JWTInHeaderMiddleware>();
           // app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<UserIdInHeaderMiddleware>();
            
            app.UseAuthentication();
            
            app.UseSignalR(routes =>
            {
                routes.MapHub<PostHub>("/entryHub");
                routes.MapHub<ProfileHub>("/profileHub");
            });
            
            app.UseMvc();
        }
    }
}
