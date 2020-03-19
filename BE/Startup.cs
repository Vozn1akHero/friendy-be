using System.IO;
using System.Text;
using BE.ElasticSearch;
using BE.Features.FriendshipRecommendation;
using BE.Features.Search.Services;
using BE.Helpers;
using BE.HostedServices;
using BE.Middlewares;
using BE.Repositories.RepositoryServices.Interfaces.User;
using BE.Repositories.RepositoryServices.User;
using BE.SignalR.Hubs;
using BE.SignalR.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BE
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR();
            services.ConfigureRepositoryWrapper();
            services.ConfigureAutoMapper();

            var path = Path.Combine(Environment.ContentRootPath, "wwwroot");
            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(
                    path));

            //services.AddDirectoryBrowser();
            var key = Encoding.ASCII.GetBytes(
                Configuration.GetValue<string>("AppSettings:JWTSecret"));

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme =
                        JwtBearerDefaults.AuthenticationScheme;
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
            
            services.AddScoped<IJwtConf, JwtConf>();
            services.AddScoped<ICustomSqlQuery, CustomSqlQuery>();
            services.AddScoped<IImageSaver, ImageSaver>();
            services.AddScoped<IUserSearchingService, UserSearchingService>();
            services.AddScoped<IRawSqlQuery, RawSqlQuery>();
            services.AddScoped<IDialogNotifier, DialogNotifier>();
            services.AddScoped<IEventSearchService, EventSearchService>();
            services.AddScoped<IUserSearchService, UserSearchService>();
            services.AddScoped<IUserDetailedSearch, UserDetailedSearch>();
            services.AddScoped<ICosSim, CosSim>();
            services.ConfigureRepositoryServices();
            //services.AddHostedService<FriendshipRecommendationHostedService>();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling =
                    ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            services.AddMediatR(typeof(Startup));

            services.AddOptions();
            services.Configure<ElasticConnectionSettings>(Configuration
                .GetSection("ElasticConnectionSettings"));
            services.AddSingleton(typeof(ElasticClientProvider));

            services.ConfigureSqlContext(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Environment.ContentRootPath, "wwwroot")),
                RequestPath = "/wwwroot",
                EnableDirectoryBrowsing = false
            });

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod()
                .AllowAnyOrigin());
            //app.UseHttpsRedirection();

            app.UseMiddleware<JWTInHeaderMiddleware>();
            // app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<UserIdInHeaderMiddleware>();

            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                //routes.MapHub<PostHub>("/postHub");
                //routes.MapHub<ProfileHub>("/profileHub");
                routes.MapHub<DialogHub>("/dialogHub");
                routes.MapHub<UserStatusHub>("/userStatusHub");
            });

            app.UseEndpointRouting().UseMvc();

            /*app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "Frontend";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });*/
        }
    }
}