using AutoMapper;
using BE.Dtos;
using BE.Interfaces;
using BE.Models;
using BE.Repositories;
using BE.Services.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BE
{
    public static class ServiceExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("connectionString");
            services.AddDbContext<FriendyContext>(o => o.UseSqlServer(connectionString));
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }

        public static void ConfigureRepositoryServices(this
            IServiceCollection services)
        {
            services.AddTransient<IUserDataService, UserDataService>();
            services.AddTransient<IUserPostService, UserPostService>();
            services.AddTransient<IEventParticipantService, 
            EventParticipantService>();
            services.AddTransient<IEventDataService, EventDataService>();
            services.AddTransient<IPhotoService, PhotoService>();
            services.AddTransient<IPostCommentService, PostCommentService>();
            services
                .AddTransient<IFriendshipRecommendationService,
                    FriendshipRecommendationService>();
        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserPostProfile());
                mc.AddProfile(new EventPostProfile());
                mc.AddProfile(new EventDataProfile());
                mc.CreateMap<NewUserDto, User>();
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
