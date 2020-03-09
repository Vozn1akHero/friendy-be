using System;
using System.Threading.Tasks;
using BE.Features.Event.Commands.Event.EventPost;
using BE.Features.Event.Queries.EventPost;
using BE.Helpers;
using BE.Models;
using BE.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Features.Post
{
    [ApiController]
    [Route("api/event-post")]
    public class EventPostController : ControllerBase
    {
        private readonly IImageSaver _imageSaver;
        private readonly IMediator _mediator;

        public EventPostController(IImageSaver imageSaver, IMediator mediator)
        {
            _imageSaver = imageSaver;
            _mediator = mediator;
        }

        [HttpPost("{eventId}")]
        [AuthorizeEventAdmin]
        [Authorize]
        public async Task<IActionResult> CreateEventPost(
            [FromForm(Name = "image")] IFormFile image,
            [FromForm(Name = "content")] string content,
            [FromRoute(Name = "eventId")] int eventId,
            [FromHeader(Name = "userId")] int userId)
        {
            var imagePath = await _imageSaver
                .SaveAndReturnImagePath(image, "EventPost", userId);
            var newPostDto = await _mediator.Send(new CreateEventPostAndReturnDtoCommand
            {
                EventPost = new EventPost
                {       
                    EventId = eventId,
                    Post = new Models.Post
                    {
                        Content = content,
                        ImagePath = imagePath,
                        Date = DateTime.Now
                    }
                }
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
        public async Task<IActionResult> GetRangeByEventId(
            [FromQuery(Name = "start")] int startIndex,
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
        public async Task<IActionResult> GetRangeWithPaginationByEventId(
            [FromRoute(Name = "page")] int page,
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