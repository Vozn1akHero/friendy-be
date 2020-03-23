using AutoMapper;
using BE.Features.Authentication.Services;
using BE.Features.Chat.Services;
using BE.Features.Comment.Services;
using BE.Features.Event.Dtos;
using BE.Features.Event.Services;
using BE.Features.Friendship.Services;
using BE.Features.FriendshipRecommendation;
using BE.Features.Helpers;
using BE.Features.Notification;
using BE.Features.Photo;
using BE.Features.Post.Services;
using BE.Features.Search.Services;
using BE.Features.User.Dtos;
using BE.Features.User.Services;
using BE.Mapping.Profiles;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BE
{
    public static class ServiceExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services,
            IConfiguration config)
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
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IUserDataService, UserDataService>();
            services.AddTransient<IEventAdminService, EventAdminService>();
            services.AddTransient<IUserPostService, UserPostService>();
            services.AddTransient<IEventParticipantService,
                EventParticipantService>();
            services.AddTransient<IEventDataService, EventDataService>();
            services.AddTransient<IPhotoService, PhotoService>();
            services.AddTransient<IPostCommentService, PostCommentService>();
            services.AddTransient<IUserEventsService, UserEventsService>();
            services.AddTransient<IUserFriendshipService, UserFriendshipService>();
            services.AddTransient<IChatService, ChatService>();
            services.AddTransient<ICommentResponseService, CommentResponseService>();
            services.AddTransient<IFriendshipRecommendationService,
                FriendshipRecommendationService>();
            services
                .AddTransient<IEventParticipationRequestService,
                    EventParticipationRequestService>();
            services
                .AddTransient<IEventParticipationStatusService,
                    EventParticipationStatusService>();
            services.AddTransient<IUserDataUpdateService, UserDataUpdateService>();
            services.AddTransient<ISQLServer_UserSearchService, SQLServer_UserSearchService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<INotificationService, NotificationService>();
        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserPostProfile());
                mc.AddProfile(new EventPostProfile());
                mc.AddProfile(new EventDataProfile());
                mc.AddProfile(new FoundUserDtoProfile());
                mc.CreateMap<NewUserDto, User>();
                mc.AddProfile(new ChatLastMessageProfileProfile());
                mc.CreateMap<EventDataDto, Event>();
            });
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}