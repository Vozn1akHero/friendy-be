using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BE.Features.Post;
using BE.Features.Post.Dtos;
using BE.Features.Post.Repositories;
using BE.Features.Post.Services;
using BE.Helpers;
using BE.Mapping.Profiles;
using BE.Models;
using BE.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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
            var service = GetService(data);
            var dtos = data
                .AsQueryable()
                .Select(UserPostDto.Selector(7))
                .Reverse()
                .ToList();
            var controller = new UserPostController(service);
            var res = controller.GetByPage(_userId, 1) as OkObjectResult;
            Assert.NotNull(res);
            
            var dataRes = res.Value as List<UserPostDto>;
            Assert.AreEqual(dtos.Count, dataRes.Count);
            for (int i = 0; i < dataRes.Count; i++)
            {
                Assert.AreEqual(dataRes.ElementAt(i).Id, dataRes.ElementAt(i).Id);
                Assert.AreEqual(dataRes.ElementAt(i).Content, dataRes.ElementAt(i).Content);
                Assert.AreEqual(dataRes.ElementAt(i).ImagePath, dataRes.ElementAt(i).ImagePath);
                Assert.AreEqual(dataRes.ElementAt(i).IsPostLikedByUser, dataRes.ElementAt(i)
                .IsPostLikedByUser);
                Assert.AreEqual(dataRes.ElementAt(i).LikesCount, dataRes.ElementAt(i).LikesCount);
                Assert.AreEqual(dataRes.ElementAt(i).CommentsCount, dataRes.ElementAt(i)
                .CommentsCount);
            }
        }

        [Test]
        public async Task Create_User_Post_Test()
        {
            var data = GetFakeUserPosts(20);
            var service = GetService(data);
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
            var data = GetFakeUserPosts(20);
            var service = GetService(data);
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
            var data = GetFakeUserPosts(20);
            var service = GetService(data);
            var controller = new UserPostController(service);
            var res = controller.GetRange(_userId, 1, 15);
            var okObjectResult = res as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var model = okObjectResult.Value as List<UserPostDto>;
            Assert.NotNull(model);
            Assert.That(model.Count, Is.GreaterThanOrEqualTo(15));
        }

        private IUserPostService GetService(IEnumerable<UserPost> data)
        {
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