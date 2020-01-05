using System.IO;
using System.Text;
using BE.ElasticSearch;
using BE.Interfaces;
using BE.Middlewares;
using BE.Repositories.RepositoryServices.Interfaces.User;
using BE.RepositoryServices.User;
using BE.Services.Elasticsearch;
using BE.Services.Global;
using BE.Services.Global.Interfaces;
using BE.Services.Model;
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
using RecommendationAlgorithm;

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
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR();
            services.ConfigureRepositoryWrapper();
            services.ConfigureAutoMapper();

            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

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

            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ICustomSqlQueryService, CustomSqlQueryService>();
            services.AddScoped<IImageSaver, ImageSaver>();
            services.AddScoped<IUserSearchingService, UserSearchingService>();
            services.AddScoped<IRowSqlQueryService, RowSqlQueryService>();
            services.AddScoped<IDialogNotifierService, DialogNotifierService>();
            services.AddScoped<IEventSearchService, EventSearchService>();
            services.AddScoped<IEventDetailedSearch, EventDetailedSearch>();
            services.AddScoped<IUserSearchService, UserSearchService>();
            services.AddScoped<IUserDetailedSearch, UserDetailedSearch>();
            services.AddScoped<IUserDataIndexing, UserDataIndexing>();
            services.AddScoped<IEventDataIndexing, EventDataIndexing>();
            services.AddScoped<ICosSim, CosSim>();

            services.ConfigureRepositoryServices();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling =
                    ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
            });

            services.AddMediatR(typeof(Startup));

            services.AddOptions();
            services.Configure<ElasticConnectionSettings>(Configuration
                .GetSection("ElasticConnectionSettings"));
            services.AddSingleton(typeof(ElasticClientProvider));

            services.ConfigureSqlContext(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            /*if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }*/
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
                EnableDirectoryBrowsing = false
            });

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod()
                .AllowAnyOrigin().AllowCredentials());
            //app.UseHttpsRedirection();

            app.UseMiddleware<JWTInHeaderMiddleware>();
            // app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<UserIdInHeaderMiddleware>();

            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<PostHub>("/postHub");
                routes.MapHub<ProfileHub>("/profileHub");
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