using System;
using System.IO;
using System.Threading.Tasks;
using BE.Commands.EventPost;
using BE.CustomAttributes;
using BE.Interfaces;
using BE.Models;
using BE.Queries.EventPost;
using BE.Services.Global;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/event-post")]
    public class EventPostController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IImageSaver _imageSaver;
        private readonly IMediator _mediator;
        
        public EventPostController(IRepositoryWrapper repository,
            IImageSaver imageSaver, IMediator mediator)
        {
            _repository = repository;
            _imageSaver = imageSaver;
            _mediator = mediator;
        }
        
        [HttpPost("{eventId}")]
        [AuthorizeEventAdmin]
        [Authorize]
        public async Task<IActionResult> CreateEventPost([FromForm(Name = "image")] IFormFile image,
            [FromForm(Name = "content")] string content,
            [FromRoute(Name = "eventId")] int eventId,
            [FromHeader(Name = "userId")] int userId)
        {
            string imagePath = await _imageSaver
                .SaveAndReturnImagePath(image, "EventPost", userId);
            var newPost = new Post
            {
                Content = content, ImagePath = imagePath, Date = DateTime.Now
            };
            await _repository.Post.CreateAsync(newPost);
            var newEventPost = new EventPost
            {
                EventId = eventId, PostId = newPost.Id
            };
            var newPostDto = await _mediator.Send(new CreateEventPostAndReturnDtoCommand
            {
                EventPost = newEventPost
            });
            return Ok(newPostDto);
        }
        
        [HttpGet("{id}/{eventId}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id, 
            [FromHeader(Name = "userId")] int userId)
        {
            var eventPost = await _mediator.Send(new GetEventPostById
            {
                PostId = id,
                UserId = userId
            });
            return Ok(eventPost);
        }

        [HttpGet("{eventId}")]
        [Authorize]
        public async Task<IActionResult> GetRangeByEventId([FromQuery(Name = "start")] int startIndex,
            [FromQuery(Name = "length")] int length, 
            [FromRoute(Name = "eventId")] int eventId, 
            [FromHeader(Name = "userId")] int userId)
        {
            var eventPostRange = await _mediator.Send(new GetEventPostRangeById
            {
                EventId = eventId,
                StartIndex = startIndex,
                Length = length,
                UserId = userId
            });
            return Ok(eventPostRange);
        }
        
        [HttpGet("paginate/{eventId}/{page}")]
        [Authorize]
        public async Task<IActionResult> GetRangeWithPaginationByEventId([FromRoute(Name = "page")] int page,
            [FromRoute(Name = "eventId")] int eventId, 
            [FromHeader(Name = "userId")] int userId)
        {
            var posts = await _mediator.Send(new GetEventPostsRangeWithPaginationQuery
            {
                EventId = eventId,
                Page = page,
                UserId = userId
            });
            return Ok(posts);
        }
    }
}