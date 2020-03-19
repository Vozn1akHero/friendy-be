using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using BE.FakeData;
using BE.FakeData.Builders;
using BE.Features.Comment;
using BE.Features.Comment.Dtos;
using BE.Features.Comment.Repositories;
using BE.Features.Comment.Services;
using BE.Features.Post.Repositories;
using BE.Features.Post.Services;
using BE.Models;
using BE.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace BE.UnitTests.Xunit
{
    public class MainCommentTests
    {
        private int _userId = 7;

        public MainCommentTests()
        {
           
        }

        [Fact]
        public async Task Create_Async_Test()
        {
            var options = new DbContextOptionsBuilder<FriendyContext>()
                .UseInMemoryDatabase(databaseName: "Create_Async_Test")
                .Options;
            int postId = 1;
            string content = new Guid().ToString();
            using (var context = new FriendyContext(options))
            {
                var repo = new RepositoryWrapper(context);
                var service = new PostCommentService(repo, null);
                var controller = new PostCommentController(service);
                var res = await controller.CreateAsync(_userId, new NewCommentDto()
                {
                    PostId = postId,
                    Content = content
                }, null);
                var value = res.Value;
                Assert.NotNull(value);
                Assert.IsType<object>(value);
                Assert.Equal(value.Content, content);
            }
        }

        [Theory]
        [InlineData(1,7)]
        //[InlineData(2,7)]
        public async Task Get_All_By_Post_Id_Tests(int postId, int userId)
        {
            var options = new DbContextOptionsBuilder<FriendyContext>()
                .UseInMemoryDatabase(databaseName: "Get_All_By_Post_Id_Tests")
                .Options;

            using (var context = new FriendyContext(options))
            {
                var data = GetData(20).ToList();
                context.MainComment.AddRange(data);
                context.SaveChanges();
            }

            using (var context = new FriendyContext(options))
            {
                var repo = new RepositoryWrapper(context);
                var service = new PostCommentService(repo, null);
                var controller = new PostCommentController(service);
                var res = await controller.GetAllByPostIdAsync(postId, userId);
                Assert.NotNull(res);
            }
        }

        private IEnumerable<MainComment> GetData(int length)
        {
            for (int i = 1; i <= length; i++)
            {
                yield return new MainComment()
                {
                    Id = i,
                    CommentId = i,
                    ResponseToComment = new List<ResponseToComment>(),
                    Comment = new FakeCommentBuilder(i)
                        .WithContent("Test")
                        .WithUser(new FakeUserBuilder(_userId)
                        .WithName("Test")
                        .WithSurname("Test2").Build())
                        .WithCommentLikes(new List<CommentLike>())
                        .WithDate(DateTime.Now)
                        .Build()
                };
            }
        }
    }
}
