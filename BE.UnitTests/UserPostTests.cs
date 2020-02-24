using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BE.Dtos.PostDtos;
using BE.Features.Comment.Repositories;
using BE.Features.Post;
using BE.Features.Post.Services;
using BE.Features.User;
using BE.Helpers;
using BE.Mapping.Profiles;
using BE.Models;
using BE.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BE.UnitTests
{
    public class UserPostTests
    {
        private readonly IMapper _mapper;
        private readonly int _userId = 7;
        
        public UserPostTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IImageSaver, ImageSaver>();
            var config = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new UserPostProfile());
                });

            _mapper = config.CreateMapper();
        }
        
        [Test]
        public void Get_User_Post_By_Page_Test()
        {
            var data = GetFakeUserPosts(20);
            var dtos = data
                .AsQueryable()
                .Select(UserPostDto.Selector(7)).ToArray();
            var mockSet = Helpers.GetMockDbSet(data.ToList());
            var mockContext = new Mock<FriendyContext>();
            mockContext.Setup(c => c.Set<UserPost>()).Returns(mockSet.Object);
            var mockWrapper = new Mock<RepositoryWrapper>(mockContext.Object);
            var rep = new UserPostRepository(mockContext.Object);
            mockWrapper.Setup(c => c.UserPost).Returns(rep);
            var service = new UserPostService(mockWrapper.Object, null, null);
            var res = service.GetByPage(_userId, 1);
            Assert.NotNull(res);
            foreach (var val in res)
            {
                Assert.Equals(dtos[0].Id, val.Id);
                Assert.Equals(dtos[0].Content, val.Content);
                Assert.Equals(dtos[0].ImagePath, val.ImagePath);
                Assert.Equals(dtos[0].IsPostLikedByUser, val.IsPostLikedByUser);
                Assert.Equals(dtos[0].LikesCount, val.LikesCount);
                Assert.Equals(dtos[0].CommentsCount, val.CommentsCount);
            }
        }

        [Test]
        public async Task Create_User_Post_Test()
        {
            var service = GetService();
            var controller = new UserPostController(service);
            var res = await controller.CreateAsync(null, "test", _userId);
            var okObjectResult = res as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var model = okObjectResult.Value as Models.UserPost;
            Assert.NotNull(model);
        }
        
        [Test]
        public void Get_User_Post_By_Id_Test()
        {
            var service = GetService();
            var controller = new UserPostController(service);
            var res = controller.GetById(1, _userId);
            var okObjectResult = res as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var model = okObjectResult.Value;
            Assert.NotNull(model);
        }
        
        [Test]
        public void Get_User_Post_Range_Test()
        {
            var service = GetService();
            var controller = new UserPostController(service);
            var res = controller.GetRange(_userId, 1, 15);
            var okObjectResult = res as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var model = okObjectResult.Value as List<UserPostDto>;
            Assert.NotNull(model);
            Assert.That(model.Count, Is.GreaterThanOrEqualTo(15));
        }

        private IUserPostService GetService()
        {
            var data = GetFakeUserPosts(20);
            var mockContext = new Mock<FriendyContext>();
            var mockSet = Helpers.GetMockDbSet(data.ToList());
            mockContext.Setup(c => c.Set<UserPost>()).Returns(mockSet.Object);
            var mockWrapper = new Mock<RepositoryWrapper>(mockContext.Object);
            var rep = new UserPostRepository(mockContext.Object);
            mockWrapper.Setup(c => c.UserPost).Returns(rep);
            var service = new UserPostService(mockWrapper.Object, _mapper, null);
            return service;
        }

        private IEnumerable<UserPost> GetFakeUserPosts(int length)
        {
            for (int i = 1; i <= length; i++)
            {
                yield return new UserPost
                {
                    Id = i,
                    UserId = _userId,
                    PostId = i,
                    Post = new Post
                    {
                        Id = i,
                        Content = Convert.ToString(Guid.NewGuid()),
                        ImagePath = null,
                        Comment = new List<Comment>(),
                        PostLike = new List<PostLike>(),
                        Date = DateTime.Now
                    },
                    User = new User {Id = _userId, Avatar = null}
                };
            }
        }
    }
}