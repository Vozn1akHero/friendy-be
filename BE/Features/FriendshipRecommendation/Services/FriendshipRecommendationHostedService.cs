using System;
using System.Threading;
using System.Threading.Tasks;
using BE.Features.FriendshipRecommendation;
using BE.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BE.HostedServices
{
    public class IdModel
    {
        public int Id { get; set; }
    }
    
    public class FriendshipRecommendationHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public FriendshipRecommendationHostedService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        private async Task CalculateAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetService<IRepositoryWrapper>();
                var cosSim = scope.ServiceProvider.GetService<ICosSim>();
                var allUsers = await repository.User.GetAllAsync(e => new IdModel
                {
                    Id = e.Id
                });
                foreach (var user in allUsers)
                {
                    var refreshNeed = cosSim.CheckIfRefreshIsNeeded(user.Id);
                    if (!refreshNeed) continue;
                    await cosSim.DeleteEntitiesForUser(user.Id);
                    var output = await cosSim.CalculateAsync(user.Id);
                    await cosSim.AddRecommendationsByOutputsToDatabase(user.Id, output);
                }
            }
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CalculateAsync();
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}