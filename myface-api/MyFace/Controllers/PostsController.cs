using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using MyFace.Helpers;
using Myface.Services;
using System;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/posts")]
    public class PostsController : ControllerBase
    {    
        private readonly IPostsRepo _posts;
        private readonly IUsersRepo _users;
        private readonly IAuthService _authservice;   // CORRECT SYNTAX?

        public PostsController(IPostsRepo posts, IUsersRepo users, IAuthService authservice)
        {
            _posts = posts;
            _users = users;
            _authservice = authservice;
        }
        
        [HttpGet("")]
        public ActionResult<PostListResponse> Search(
            [FromQuery] PostSearchRequest searchRequest, 
            [FromHeader (Name = "Authorization")] string authorizationHeader)
        {
            
            if (_authservice.isUnAuthorizedResult(authorizationHeader))
            {
                return new UnauthorizedResult();
            }

            var posts = _posts.Search(searchRequest);
            var postCount = _posts.Count(searchRequest);
            return PostListResponse.Create(searchRequest, posts, postCount);
        }

        [HttpGet("{id}")]
        public ActionResult<PostResponse> GetById(
            [FromRoute] int id,
            [FromHeader (Name = "Authorization")] string authorizationHeader)
        {
            if (_authservice.isUnAuthorizedResult(authorizationHeader))
            {
                return new UnauthorizedResult();
            }

            var post = _posts.GetById(id);
            return new PostResponse(post);
        }

        [HttpPost("create")]
        public IActionResult Create(
            [FromBody] CreatePostRequest newPost,
            [FromHeader (Name = "Authorization")] string authorizationHeader)
        {
            if (_authservice.isUnAuthorizedResult(authorizationHeader))
            {
                return new UnauthorizedResult();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var post = _posts.Create(newPost);

            var url = Url.Action("GetById", new { id = post.Id });
            var postResponse = new PostResponse(post);
            return Created(url, postResponse);
        }

        [HttpPatch("{id}/update")]
        public ActionResult<PostResponse> Update(
            [FromRoute] int id, 
            [FromBody] UpdatePostRequest update,
            [FromHeader (Name = "Authorization")] string authorizationHeader)
        {
            if (_authservice.isUnAuthorizedResult(authorizationHeader))
            {
                return new UnauthorizedResult();
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = _posts.Update(id, update);
            return new PostResponse(post);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(
            [FromRoute] int id,
            [FromHeader (Name = "Authorization")] string authorizationHeader)
        {
            if (_authservice.isUnAuthorizedResult(authorizationHeader))
            {
                return new UnauthorizedResult();
            }

            _posts.Delete(id);
            return Ok();
        }
    }
}