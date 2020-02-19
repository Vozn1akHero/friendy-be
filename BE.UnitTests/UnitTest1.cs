using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BE;
using BE.Dtos;
using BE.Features.Event.Queries.EventPost;
using BE.Models;
using BE.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Tests
{
    public class RepositoryTests
    {
        private IMediator _mediator;

        public RepositoryTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<FriendyContext>();
            services.AddTransient<IRepositoryWrapper, RepositoryWrapper>();
            services.AddTransient<IMapper, Mapper>();
            services.AddMediatR(typeof(Startup));
            var serviceProvider = services.BuildServiceProvider();
            _mediator = serviceProvider.GetService<IMediator>();
            /*_userPostService = new UserPostService(serviceProvider
            .GetService<IRepositoryWrapper>(), serviceProvider.GetService<IMapper>());*/
        }
        
        [Test]
        public async Task GetPostsRangeTest()
        {
            var posts = await _mediator.Send(new GetEventPostById {PostId = 1, UserId = 1});
            //var fakePosts = GetPosts();
            Assert.That(posts.Count(), Is.EqualTo(1));
        }
        
    }
}